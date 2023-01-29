using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Zadatak.Models.FileControllers;

namespace Zadatak.Controllers
{
    public class PosjetilacController : Controller
    {
        // GET: Posjetilac
        public ActionResult IstorijaTreninga()
        {
            var ltID = ((Korisnik)Session["user"]).GrupniTreninzi;
            List<GrupniTrening> lt = new List<GrupniTrening>();
            List<string> c = new List<string>();
            foreach (var id in ltID)
            {
                var tr = DataController.TreningFileController.Get(id);
                if (tr.Termin < DateTime.Now)
                {
                    lt.Add(tr);
                    c.Add(DataController.CentarFileController.Get(tr.CentarId).Naziv);
                }
            }
            ViewBag.Treninzi = lt;
            ViewBag.Centri = c;
            return View();
        }



        public ActionResult Pretrazi(string naziv, string tip, string nazivCentra)
        {
            var sviTr = DataController.TreningFileController.GetAll().FindAll(x => x.Termin < DateTime.Now);
            var c = DataController.CentarFileController.GetAll();
            var lt = sviTr.FindAll(x => (string.IsNullOrWhiteSpace(naziv) || x.Naziv == naziv) &&
                (string.IsNullOrWhiteSpace(tip) || Enum.GetName(typeof(TipTreninga), x.Tip) == tip) &&
                (string.IsNullOrWhiteSpace(nazivCentra) || nazivCentra == c.Find(y => y.Id == x.CentarId).Naziv));
            ViewBag.Treninzi = lt;
            var cn = new List<string>();
            foreach (var t in lt)
            {
                cn.Add(DataController.CentarFileController.Get(t.CentarId).Naziv);
            }
            ViewBag.Centri = cn;
            return View("~/Views/Posjetilac/IstorijaTreninga.cshtml");
        }


        public ActionResult Sortiraj(string parametar, string metod)
        {
            var sviTr = DataController.TreningFileController.GetAll().FindAll(x => x.Termin < DateTime.Now);
            List<GrupniTrening> sortirani = new List<GrupniTrening>();
            switch (parametar)
            {
                case "NAZIV":
                    if (metod == "Rastuce")
                        sortirani = sviTr.OrderBy(x => x.Naziv).ToList();
                    else
                        sortirani = sviTr.OrderByDescending(x => x.Naziv).ToList();
                    break;
                case "TIP":
                    if (metod == "Rastuce")
                        sortirani = sviTr.OrderBy(x => Enum.GetName(typeof(TipTreninga), x.Tip)).ToList();
                    else
                        sortirani = sviTr.OrderByDescending(x => Enum.GetName(typeof(TipTreninga), x.Tip)).ToList();
                    break;
                case "DATUM":
                    if (metod == "Rastuce")
                        sortirani = sviTr.OrderBy(x => x.Termin).ToList();
                    else
                        sortirani = sviTr.OrderByDescending(x => x.Termin).ToList();
                    break;
            }
            var cn = new List<string>();
            ViewBag.Treninzi = sortirani;
            foreach (var t in sortirani)
            {
                cn.Add(DataController.CentarFileController.Get(t.CentarId).Naziv);
            }
            ViewBag.Centri = cn;
            return View("~/Views/Posjetilac/IstorijaTreninga.cshtml");
        }



        public ActionResult OstaviKomentar(int id)
        {
            ViewBag.Centar = DataController.CentarFileController.Get(id);
            return View();
        }


        [HttpPost]
        public ActionResult DodajKomentar(Komentar k)
        {
            k.Posjetilac = ((Korisnik)Session["user"]).KorisnickoIme;
            DataController.KomentarFileController.Insert(k);
            return RedirectToAction("IstorijaTreninga");
        }


        [HttpPost]
        public ActionResult Prijava(int posID, int trID, int fID)
        {
            var p = DataController.KorisnikFileController.Get(posID);
            p.GrupniTreninzi.Add(trID);
            DataController.KorisnikFileController.Update(p.Id, p);
            Session["user"] = p;
             
            var t = DataController.TreningFileController.Get(trID);
            t.SpisakPosjetilacaId.Add(posID);
            DataController.TreningFileController.Update(t.Id, t);
            return RedirectToAction("Index", "Home");
        }

    }
}