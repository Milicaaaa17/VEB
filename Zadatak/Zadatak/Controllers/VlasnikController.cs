using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zadatak.Models;
using Zadatak.Models.FileControllers;

namespace Zadatak.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult FitnesCentri()
        {
            var k = (Korisnik)Session["user"];
            ViewBag.Centri = DataController.CentarFileController.GetAll().FindAll(x => k.CentriId.Contains(x.Id));
            if (ViewBag.Centri == null)
                ViewBag.Centri = new List<FitnesCentar>();
            return View();
        }

        [HttpPost]
        public ActionResult KreirajCentar(FitnesCentar f, string mesto, string ulica, string broj, int postanskiBroj)
        {
            var k = (Korisnik)Session["user"];
            f.VlasnikId = k.Id;
            f.Adresa = new Adresa() { Broj = broj, Ulica = ulica, Mesto = mesto, PostanskiBroj = postanskiBroj };
            DataController.CentarFileController.Insert(f);
            return RedirectToAction("FitnesCentri");
        }


        [HttpPost]
        public ActionResult ObrisiCentar(int id)
        {
            if (DataController.TreningFileController.GetAll().Find(x => x.CentarId == id && x.Termin > DateTime.Now) != null)
                return RedirectToAction("FitnesCentri");
            var l = DataController.KorisnikFileController.GetAll().FindAll(x => x.CentarId == id);
            if (l != null)
            {
                foreach (var t in l)
                {
                    t.Obrisan = true;
                    DataController.KorisnikFileController.Update(t.Id, t);
                }
            }
            DataController.CentarFileController.Delete(id);
            return RedirectToAction("FitnesCentri");
        }

        public ActionResult Izmijeni(int id)
        {
            ViewBag.Centar = DataController.CentarFileController.Get(id);
            return View();
        }

        [HttpPost]
        public ActionResult IzmijeniCentar(FitnesCentar f, string mesto, string ulica, string broj, int postanskiBroj)
        {
            var fc = DataController.CentarFileController.Get(f.Id);
            fc.Adresa = new Adresa() { Broj = broj, Ulica = ulica, Mesto = mesto, PostanskiBroj = postanskiBroj };
            fc.CijenaGrupnogTreninga = f.CijenaGrupnogTreninga;
            fc.CijenaJednogTreninga = f.CijenaJednogTreninga;
            fc.CijenaTreningaSaPersTrenerom = f.CijenaTreningaSaPersTrenerom;
            fc.GodinaOsnivanja = f.GodinaOsnivanja;
            fc.GodisnjaClanarina = f.GodisnjaClanarina;
            fc.Naziv = f.Naziv;
            fc.MjesecnaClanarina = f.MjesecnaClanarina;
            DataController.CentarFileController.Update(fc.Id, fc);
            return RedirectToAction("Treninzi", "Trener");
        }




        [HttpPost]
        public ActionResult Registrovanje(Korisnik k)
        {
            if (DataController.KorisnikFileController.GetAll().Find(x => x.KorisnickoIme == k.KorisnickoIme) != null)
                return RedirectToAction("RegistracijaTrenera");
            k.Uloga = Uloga.TRENER;
            DataController.KorisnikFileController.Insert(k);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult RegistracijaTrenera()
        {
            var k = (Korisnik)Session["user"];
            var c = DataController.CentarFileController.GetAll().FindAll(x => k.CentriId.Contains(x.Id));
            if (c == null)
                c = new List<FitnesCentar>();
            var treneri = new List<Korisnik>();
            foreach (var cc in c)
            {
                var tr = DataController.KorisnikFileController.GetAll().FindAll(x => x.CentarId == cc.Id);
                if (tr != null)
                {
                    foreach (var i in tr)
                    {
                        treneri.Add(i);
                    }
                }
            }
            ViewBag.Treneri = treneri;
            ViewBag.Centri = c;
            return View();

        }

        [HttpPost]
        public ActionResult Blokiraj(int id)
        {
            DataController.KorisnikFileController.Delete(id);
            return RedirectToAction("Index", "Home");
        }




        public ActionResult StanjeKomentara(int id, int s)
        {
            var k = DataController.KomentarFileController.Get(id);
            k.Stanje = s == 0 ? Stanje.POTVRDJEN : Stanje.ODBIJEN;
            DataController.KomentarFileController.Update(k.Id, k);
            return RedirectToAction("Komentari");
        }

        public ActionResult Komentari()
        {
            var k = (Korisnik)Session["user"];
            ViewBag.KomCeka = DataController.KomentarFileController.GetAll().FindAll(x => k.CentriId.Contains(x.FitCentarId) && x.Stanje == Stanje.CEKA);
            if (ViewBag.KomCeka == null)
                ViewBag.KomCeka = new List<Komentar>();
            return View();
        }
    }
}