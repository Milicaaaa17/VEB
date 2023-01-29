using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{

    public class Adresa
    {
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public int PostanskiBroj { get; set; }

        public Adresa()
        {

        }

        public override string ToString()
        {
            return $"{Ulica} {Broj}, {PostanskiBroj}, {Mesto}";
        }
    }

    public class FitnesCentar
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public Adresa Adresa { get; set; }
        public int GodinaOsnivanja { get; set; }
        public int VlasnikId { get; set; }
        public double MjesecnaClanarina { get; set; }
        public double GodisnjaClanarina { get; set; }
        public double CijenaJednogTreninga { get; set; }
        public double CijenaGrupnogTreninga { get; set; }
        public double CijenaTreningaSaPersTrenerom { get; set; }
        public bool Obrisan { get; set; } = false;

        public FitnesCentar()
        {

        }

        public FitnesCentar(string naziv, Adresa adresa, int godinaOsnivanja, int vlasnikId, double mjesecnaClanarina, double godisnjaClanarina, double cijenaJednogTreninga, double cijenaGrupnogTreninga, double cijenaTreningaSaPersTrenerom)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOsnivanja = godinaOsnivanja;
            VlasnikId = vlasnikId;
            MjesecnaClanarina = mjesecnaClanarina;
            GodisnjaClanarina = godisnjaClanarina;
            CijenaJednogTreninga = cijenaJednogTreninga;
            CijenaGrupnogTreninga = cijenaGrupnogTreninga;
            CijenaTreningaSaPersTrenerom = cijenaTreningaSaPersTrenerom;
        }
    }
}