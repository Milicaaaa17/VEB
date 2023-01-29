using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models
{
    public enum TipTreninga { JOGA, LESSMILLSTONE, BODYPUMP }
    public class GrupniTrening
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public TipTreninga Tip { get; set; }
        public int CentarId { get; set; }
        public int Vrijeme { get; set; }
        public DateTime Termin { get; set; }
        public int MaxBrojPosjetilaca { get; set; }
        public List<int> SpisakPosjetilacaId = new List<int>();
        public bool Obrisan { get; set; } = false;

        public GrupniTrening()
        {

        }

        public GrupniTrening(string naziv, TipTreninga tip, int centarId, int vrijeme, DateTime termin, int maxBrojPosjetilaca)
        {
            Naziv = naziv;
            Tip = tip;
            CentarId = centarId;
            Vrijeme = vrijeme;
            Termin = termin;
            MaxBrojPosjetilaca = maxBrojPosjetilaca;
        }
    }
}