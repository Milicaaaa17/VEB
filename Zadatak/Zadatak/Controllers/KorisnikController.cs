using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Zadatak.Models.FileControllers;

namespace Zadatak.Controllers
{
    public class KorisnikController : Controller
    {
        // GET: Korisnik
        public ActionResult Registrovanje(Korisnik k)
        {
            if (DataController.KorisnikFileController.GetAll().Find(x => x.KorisnickoIme == k.KorisnickoIme) != null)
            {
                return RedirectToAction("Registracija", "Home");
            }
            k.Uloga = Uloga.POSJETILAC;
            DataController.KorisnikFileController.Insert(k);
            Session["user"] = k;
            return RedirectToAction("Index", "Home"); ;
        }

        public ActionResult Login(Korisnik k)
        {
            var kor = DataController.KorisnikFileController.GetAll().Find(x => x.KorisnickoIme == k.KorisnickoIme && x.Lozinka == k.Lozinka);
            if (kor != null)
            {
                Session["user"] = kor;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Poruka = "Pogresno korisnicko ime ili lozinka";
                return RedirectToAction("Login", "Home");
            }
        }


        public ActionResult PrikaziIzmjene()
        {
            ViewBag.Korisnik = Session["user"];
            return View();
        }

        [HttpPost]
        public ActionResult IzmijeniProfil(Korisnik k)
        {
            var kor = (Korisnik)Session["user"];
            if (kor.KorisnickoIme != k.KorisnickoIme && DataController.KorisnikFileController.GetAll().Find(x => x.KorisnickoIme == k.KorisnickoIme) != null)
            {
                ViewBag.Korisnik = Session["user"];
                ViewBag.Poruka = "Postoji takvo korisnicko ime!";
                return View("~/Views/Korisnik/PrikaziIzmjene.cshtml");
            }


            kor.Ime = k.Ime;
            kor.Prezime = k.Prezime;
            kor.KorisnickoIme = k.KorisnickoIme;
            kor.Lozinka = k.Lozinka;
            kor.Pol = k.Pol;
            kor.DatumRodjenja = k.DatumRodjenja; 
            kor.Email = k.Email;
            DataController.KorisnikFileController.Update(kor.Id, kor);
            return RedirectToAction("PrikaziIzmjene");
        }


        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}