using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GrupniTrening
    {
        public GrupniTrening(string naziv, TipTreninga vrstaTreninga, string centarId, int trajanjeTreninga, DateTime vremeTreninga, int maxPosetilaca, int trenutnoPosetilaca, string trener)
        {
            Naziv = naziv;
            VrstaTreninga = vrstaTreninga;
            CentarId = centarId;
            TrajanjeTreninga = trajanjeTreninga;
            VremeTreninga = vremeTreninga;
            MaxPosetilaca = maxPosetilaca;
            TrenutnoPosetilaca = trenutnoPosetilaca;
            SpisakPosetilaca = new List<Korisnik>();
            Trener = trener;
            TreningId = naziv + vrstaTreninga + Convert.ToString(vremeTreninga);
        }
        public GrupniTrening()
        {
            SpisakPosetilaca = new List<Korisnik>();

        }

        public string Naziv { get; set; }
        public TipTreninga VrstaTreninga { get; set; }
        public string CentarId{ get; set; }
        public int TrajanjeTreninga { get; set; }
        public DateTime VremeTreninga { get; set; }
        public int MaxPosetilaca { get; set; }
        public int TrenutnoPosetilaca { get; set; }
        public List<Korisnik> SpisakPosetilaca { get; set; }
        public string Trener { get; set; }
        public string TreningId { get; set; }
    }
}