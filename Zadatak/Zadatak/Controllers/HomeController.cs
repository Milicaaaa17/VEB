using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Zadatak.Models.FileControllers;

namespace Zadatak.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.FitnesCentri = DataController.CentarFileController.GetAll();
            return View();
        }


        public ActionResult Detaljnije(int id)
        {
            var f = DataController.CentarFileController.Get(id);
            if (f == null)
                return RedirectToAction("Index");
            ViewBag.Centar = f;
            ViewBag.Vlasnik = DataController.KorisnikFileController.Get(f.VlasnikId);
            ViewBag.Treninzi = DataController.TreningFileController.GetAll().FindAll(x => x.CentarId == id && x.Termin > DateTime.Now);
            ViewBag.Komentari = DataController.KomentarFileController.GetAll().FindAll(x => x.FitCentarId == id && x.Stanje == Stanje.POTVRDJEN);
            if (Session["user"] != null && ((Korisnik)Session["user"]).Uloga == Uloga.POSJETILAC)
                ViewBag.PrijavljenID = ((Korisnik)Session["user"]).Id;
            else
                ViewBag.PrijavljenID = -1;
            return View();
        }


        public ActionResult Pretrazi(string naziv, string adresa, string min, string max)
        {
            var l = DataController.CentarFileController.GetAll();
            int minGodina, maxGodina;
            int.TryParse(min, out minGodina);
            int.TryParse(max, out maxGodina);
            if (minGodina > maxGodina && maxGodina != 0)
                return RedirectToAction("Index");

            ViewBag.FitnesCentri = l.FindAll(x => (string.IsNullOrEmpty(naziv) || x.Naziv == naziv) && (string.IsNullOrEmpty(adresa) || x.Adresa.ToString().Contains(adresa))
               && (string.IsNullOrEmpty(min) || x.GodinaOsnivanja > minGodina) && (string.IsNullOrEmpty(max) || x.GodinaOsnivanja < maxGodina));

            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Sortiraj(string parametar, string metod)
        {
            var l = DataController.CentarFileController.GetAll();
            switch (parametar)
            {
                case "NAZIV":
                    if (metod == "RASTUCE")
                        ViewBag.FitnesCentri = l.OrderBy(x => x.Naziv);
                    else
                        ViewBag.FitnesCentri = l.OrderByDescending(x => x.Naziv);
                    break;
                case "ADRESA":
                    if (metod == "RASTUCE")
                        ViewBag.FitnesCentri = l.OrderBy(x => x.Adresa.ToString());
                    else
                        ViewBag.FitnesCentri = l.OrderByDescending(x => x.Adresa.ToString());
                    break;
                case "GODINA":
                    if (metod == "RASTUCE")
                        ViewBag.FitnesCentri = l.OrderBy(x => x.GodinaOsnivanja);
                    else
                        ViewBag.FitnesCentri = l.OrderByDescending(x => x.GodinaOsnivanja);
                    break;
            }

            return View("~/Views/Home/Index.cshtml");
        }
        public ActionResult Registracija()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
    }
}