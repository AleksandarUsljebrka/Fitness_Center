using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class FitnesCentar
    {
        public FitnesCentar(string naziv, Adresa adresaCentra, int godinaOtvaranja, string korisnickoIme, int cenaMesecno, int cenaGodisnje, int cenaJednogTreninga, int cenaJednogGrupnog, int cenaJednogTrener)
        {
            Naziv = naziv;
            AdresaCentra = adresaCentra;
            GodinaOtvaranja = godinaOtvaranja;
            KorisnickoIme = korisnickoIme;
            CenaMesecno = cenaMesecno;
            CenaGodisnje = cenaGodisnje;
            CenaJednogTreninga = cenaJednogTreninga;
            CenaJednogGrupnog = cenaJednogGrupnog;
            CenaJednogTrener = cenaJednogTrener;
            Id = naziv+"(" + adresaCentra.ToString()+")";
        }
        public FitnesCentar()
        {

        }
        public string Naziv { get; set; }
        public Adresa AdresaCentra{get;set;}
        public int GodinaOtvaranja {get;set;}
        public string KorisnickoIme    {get;set;}
        public int CenaMesecno  {get;set;}
        public int CenaGodisnje {get;set;}
        public int CenaJednogTreninga {get;set;}
        public int CenaJednogGrupnog {get;set;}
        public int CenaJednogTrener  {get;set;}
        public string Id { get; set; }
 
    }
}