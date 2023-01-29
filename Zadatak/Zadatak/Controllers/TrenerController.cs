using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Zadatak.Models.FileControllers;

namespace Zadatak.Controllers
{
    public class TrenerController : Controller
    {
        public ActionResult Treninzi()
        {
            var k = (Korisnik)Session["user"];
            ViewBag.Treninzi = DataController.TreningFileController.GetAll().FindAll(x => k.GrupniTreninzi.Contains(x.Id) && x.Termin > DateTime.Now);
            return View();
        }


        [HttpPost]
        public ActionResult KreirajTrening(GrupniTrening t)
        {
            var k = (Korisnik)Session["user"];
            if (t.Termin < DateTime.Now.AddDays(3))
                return RedirectToAction("Treninzi", "Trener");
            t.CentarId = k.CentarId;
            DataController.TreningFileController.Insert(t);
            var l = DataController.TreningFileController.GetAll();
            k.GrupniTreninzi.Add(l[l.Count - 1].Id);
            DataController.KorisnikFileController.Update(k.Id, k);
            return RedirectToAction("Treninzi", "Trener");
        }


        [HttpPost]
        public ActionResult ObrisiTrening(int id)
        {
            var k = (Korisnik)Session["user"];
            var tr = DataController.TreningFileController.Get(id);
            if (tr.SpisakPosjetilacaId.Count > 0)
                return RedirectToAction("Treninzi", "Trener");
            k.GrupniTreninzi.Remove(tr.Id);
            DataController.KorisnikFileController.Update(k.Id, k);
            DataController.TreningFileController.Delete(id);
            return RedirectToAction("Treninzi", "Trener");
        }


        public ActionResult Izmijeni(int id)
        {
            ViewBag.Trening = DataController.TreningFileController.Get(id);
            return View();
        }



        [HttpPost]
        public ActionResult IzmijeniTrening(GrupniTrening tr)
        {
            var t = DataController.TreningFileController.Get(tr.Id);
            t.MaxBrojPosjetilaca = tr.MaxBrojPosjetilaca;
            t.Naziv = tr.Naziv;
            t.Termin = tr.Termin;
            t.Tip = tr.Tip;
            t.Vrijeme = tr.Vrijeme;
            DataController.TreningFileController.Update(t.Id, t);
            return RedirectToAction("Treninzi", "Trener");
        }



        public ActionResult IstorijaTreninga()
        {
            var k = (Korisnik)Session["user"];
            ViewBag.Treninzi = DataController.TreningFileController.GetAll().FindAll(x => k.GrupniTreninzi.Contains(x.Id) && x.Termin < DateTime.Now);
            ViewBag.Posjetioci = new List<Korisnik>();
            return View();
        }

        public ActionResult PretraziIstoriju(string naziv, string tip, string minDatum, string maxDatum)
        {
            var k = (Korisnik)Session["user"];
            var sviTr = DataController.TreningFileController.GetAll().FindAll(x => k.GrupniTreninzi.Contains(x.Id) && x.Termin < DateTime.Now);
            ViewBag.Treninzi = sviTr.FindAll(x => (string.IsNullOrWhiteSpace(naziv) || naziv == x.Naziv) &&
            (string.IsNullOrWhiteSpace(tip) || Enum.GetName(typeof(TipTreninga), x.Tip) == tip) &&
            (string.IsNullOrWhiteSpace(minDatum) || DateTime.Parse(minDatum) < x.Termin) &&
            (string.IsNullOrWhiteSpace(maxDatum) || DateTime.Parse(maxDatum) > x.Termin));
            if (ViewBag.Treninzi == null)
                ViewBag.Treninzi = new List<GrupniTrening>();

            ViewBag.Posjetioci = new List<Korisnik>();
            return View("~/Views/Trener/IstorijaTreninga.cshtml");
        }

        public ActionResult SortirajIstoriju(string parametar, string metod)
        {
            var k = (Korisnik)Session["user"];
            var sviTr = DataController.TreningFileController.GetAll().FindAll(x => k.GrupniTreninzi.Contains(x.Id) && x.Termin < DateTime.Now);
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
            ViewBag.Treninzi = sortirani;
            ViewBag.Posjetioci = new List<Korisnik>();
            return View("~/Views/Trener/IstorijaTreninga.cshtml");
        }


        public ActionResult VidiPosjetioce(int id)
        {
            var k = (Korisnik)Session["user"];
            ViewBag.Treninzi = DataController.TreningFileController.GetAll().FindAll(x => k.GrupniTreninzi.Contains(x.Id) && x.Termin < DateTime.Now);
            var l = DataController.KorisnikFileController.GetAll().FindAll(x => x.GrupniTreninzi.Contains(id) && x.Uloga == Uloga.POSJETILAC);
            if (l == null)
                ViewBag.Posjetioci = new List<Korisnik>();
            else
                ViewBag.Posjetioci = l;

            return View("~/Views/Trener/IstorijaTreninga.cshtml");
        }
    }
}