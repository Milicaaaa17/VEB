using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public enum Stanje { CEKA, ODBIJEN, POTVRDJEN }
    public class Komentar
    {
        public int Id { get; set; }
        public string Posjetilac { get; set; }
        public int FitCentarId { get; set; }
        public string Tekst { get; set; }
        public int Ocijena { get; set; }
        public Stanje Stanje { get; set; } = Stanje.CEKA;
        public bool Obrisan { get; set; } = false;

        public Komentar()
        {

        }

        public Komentar(string posjetilac, int fitCentarId, string tekst, int ocijena)
        {
            Posjetilac = posjetilac;
            FitCentarId = fitCentarId;
            Tekst = tekst;
            Ocijena = ocijena;
        }
    }
}