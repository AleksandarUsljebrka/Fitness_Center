using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexUlogovan()
        {
            return View();
        }
        public ActionResult Registruj()
        {
           
            
            return View();
        }
        [HttpPost]
        public ActionResult Registruj(Korisnik korisnik)
        {
          //  Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["posetioci"];
            List<Korisnik> registrovani = (List<Korisnik>)HttpContext.Application["registrovani"];
            Korisnik noviKorisnik = registrovani.Find(k => k.KorisnickoIme.Equals(korisnik.KorisnickoIme));
           
            if (noviKorisnik !=null)
            {
                ViewBag.Message = $"Korisnik sa {korisnik.KorisnickoIme} vec postoji";
                return View();
            }


            //korisnici.Add(korisnik.KorisnickoIme,korisnik);
            korisnik.Uloga = (Uloga)Enum.Parse(typeof(Uloga), "POSETILAC");

            Session["korisnik"] = korisnik;
            Data.SacuvajKorisnika(korisnik);
            return RedirectToAction("Index", "Home");


        }

        [HttpPost]
        public ActionResult Uloguj(string korisnickoIme, string lozinka)
        {
            HttpContext.Application["registrovani"] = Data.ReadRegistrovane("~/App_Data/RegistrovaniKorisnici.txt");
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");

            List<Korisnik> vlasnici = (List<Korisnik>)HttpContext.Application["vlasnici"];
           // Dictionary<string, Korisnik> posetioci = (Dictionary<string, Korisnik>)HttpContext.Application["posetioci"];
            List<Korisnik> registrovani = (List<Korisnik>)HttpContext.Application["registrovani"];
            Dictionary<string, Korisnik> treneri = (Dictionary<string, Korisnik>)HttpContext.Application["treneri"];
           
            Korisnik korisnik = registrovani.Find(p => p.KorisnickoIme.Equals(korisnickoIme) && p.Lozinka.Equals(lozinka));
            Korisnik vlasnik = vlasnici.Find(v => v.KorisnickoIme.Equals(korisnickoIme) && v.Lozinka.Equals(lozinka));
            Korisnik trener = null;

            foreach(var item in treneri.Values)
            {
                if (item.KorisnickoIme.Equals(korisnickoIme) && item.Lozinka.Equals(lozinka))
                    trener = item;
            }
            if (korisnik == null && vlasnik == null && trener == null)
            {
                ViewBag.Message = $"Korisnik ne postoji!";
                return View("Index");
            }
            else if (korisnik != null)
            {
                korisnik.Uloga = (Uloga)Enum.Parse(typeof(Uloga), "POSETILAC");
                Session["korisnik"] = korisnik;
               
                return RedirectToAction("Index", "Home");
            }
            else if (trener != null)
            {
                trener.Uloga = (Uloga)Enum.Parse(typeof(Uloga), "TRENER");
                Session["korisnik"] = trener;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["korisnik"] = vlasnik;
               
                return RedirectToAction("Index","Home");
            }
        }
        
        public ActionResult Izloguj()
        {
           

            HttpContext.Application["registrovani"] = Data.ReadRegistrovane("~/App_Data/RegistrovaniKorisnici.txt");
            HttpContext.Application["trener"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");


            Session["korisnik"] = null;
            
            return RedirectToAction("Index", "Home");


        }
    }
}