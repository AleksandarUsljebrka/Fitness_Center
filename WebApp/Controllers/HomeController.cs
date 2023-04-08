
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<string> adrese = new List<string>();
            foreach (var item in fitnesCentri)
            {
                adrese.Add(item.AdresaCentra.ToString());
            }
            ViewBag.Adrese = adrese;
            ViewBag.fitnesCentri = fitnesCentri;

            Korisnik korisnik = (Korisnik)Session["korisnik"];

            if (korisnik!=null)
            {
              
                if (korisnik.Uloga.ToString().Equals("POSETILAC")) 
                {
                    return RedirectToAction("Index", "Posetilac");
                }
                else if (korisnik.Uloga.ToString().Equals("VLASNIK"))
                {
                    return RedirectToAction("Index", "Vlasnik");
                }
                else if(korisnik.Uloga.ToString().Equals("TRENER"))
                {
                    return RedirectToAction("Index", "Trener");
                }
            }
            

            return View();
        }

        
        [HttpPost]
        public ActionResult Filter(string naziv, string adresa, int godinaOtvaranja, int godinaOtvaranjaGornja)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<string> adrese = new List<string>();
            foreach (var item in fitnesCentri)
            {
                adrese.Add(item.AdresaCentra.ToString());
            }
            ViewBag.Adrese = adrese;
            if (adresa.Equals("0")) adresa = "";
            ViewBag.fitnesCentri = GetFilter(naziv, adresa, godinaOtvaranja, godinaOtvaranjaGornja);
            return View("Index");
        }

        private List<FitnesCentar> GetFilter(string naziv, string adresa, int godina, int godinaGornja)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            if (!naziv.Equals(""))
            {
                fitnesCentri = GetPoNazivu(naziv);
            }
            if(!adresa.Equals(""))
            {
                List<FitnesCentar> filtriraniAdr = new List<FitnesCentar>();

                foreach (var item in fitnesCentri)
                {
                    if (item.AdresaCentra.ToString().Equals(adresa))
                        filtriraniAdr.Add(item);
                }
                fitnesCentri = filtriraniAdr;
            }
            if(godina != 0 || godinaGornja != 0)
            {
                List<FitnesCentar> filtrirani = new List<FitnesCentar>();

                foreach (var item in fitnesCentri)
                {
                    if(godina != 0 && godinaGornja == 0)
                    {
                        if (item.GodinaOtvaranja >= godina)
                            filtrirani.Add(item);
                    }
                    else if(godinaGornja !=0 && godina == 0)
                    {
                        if (item.GodinaOtvaranja <= godinaGornja)
                            filtrirani.Add(item);
                    }
                    else
                    {
                        
                        if (item.GodinaOtvaranja >= godina && item.GodinaOtvaranja <= godinaGornja)
                            filtrirani.Add(item);
                        
                    }
                }
                fitnesCentri = filtrirani;
            }
            return fitnesCentri;

        }

        private List<FitnesCentar> GetPoNazivu(string naziv)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> filtrirani = new List<FitnesCentar>();
            if (naziv.Equals(""))
                return fitnesCentri;
            else
            {
                foreach(var item in fitnesCentri)
                {
                    if(item.Naziv.StartsWith(naziv))
                    {
                        filtrirani.Add(item);
                    }
                }
                return filtrirani;
            }
        }
        [HttpPost]
        public ActionResult Detalji(string id)
         {
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
         //   bool user = User.IsInRole("POSETILAC");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> treninziCentra = new List<GrupniTrening>();
            FitnesCentar centar = fitnesCentri.Find(f=>f.Id.Equals(id));
            //FitnesCentar centarDetaljno;

            DateTime trenutno = DateTime.Now;

            foreach (var item in treninzi)
            {
                if (item.CentarId.Equals(centar.Id)&& DateTime.Compare(item.VremeTreninga, trenutno) >0)
                    treninziCentra.Add(item);

            }
            ViewBag.fitnesCentar = centar;
            ViewBag.treninzi = treninziCentra;
            return View();
         }
    }
}