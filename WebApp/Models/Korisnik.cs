using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Korisnik
    {

        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public char Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public Uloga Uloga { get; set; }
        public List<GrupniTrening> TreninziTrener { get; set; }
        public List<GrupniTrening> TreninziKlijent { get; set; }
        public string FitnesCentarId{ get; set; }
        public List<FitnesCentar> FitnesCentriVlasnik { get; set; }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, char pol, string email, DateTime dr, Uloga ul)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Pol = pol;
            this.Email = email;
            this.DatumRodjenja = dr;
            this.Uloga = ul;
            this.FitnesCentriVlasnik = new List<FitnesCentar>();
        }
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, char pol, string email, DateTime dr)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Pol = pol;
            this.Email = email;
            this.DatumRodjenja = dr;
            TreninziKlijent = new List<GrupniTrening>();
        }
      
        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, char pol, string email, DateTime dr, string fcId)
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
            this.Ime = ime;
            this.Prezime = prezime;
            this.Pol = pol;
            this.Email = email;
            this.DatumRodjenja = dr;
            this.FitnesCentarId = fcId;
            TreninziTrener = new List<GrupniTrening>();
        }
        public Korisnik()
        {
            KorisnickoIme = "";
            Lozinka = "";   
        }
        public override string ToString()
        {
            return Ime + " " + Prezime;
        }
    }
}