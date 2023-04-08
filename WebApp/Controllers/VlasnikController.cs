using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
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
            ViewBag.korisnik = korisnik;
            return View();
        }

        //************RAD SA TRENEROM************//
        public ActionResult RadSaTrenerima()
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> vlasnistvoCentri = new List<FitnesCentar>();

            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            vlasnistvoCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];

            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            Dictionary<string,Korisnik> treneri = (Dictionary<string,Korisnik>)HttpContext.Application["treneri"];
            List<Korisnik> treneriCentra = new List<Korisnik>();
            FitnesCentar fc = new FitnesCentar();
            foreach (var item in treneri.Keys)
            {
                fc = vlasnistvoCentri.Find(f => item.Contains(f.Id));
                if(fc!=null)
                {
                    treneriCentra.Add(treneri[item]);
                }
            }
            ViewBag.treneri = treneriCentra;
            return View();
        }

        public ActionResult DodajTrenera()
        { 
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> vlasnistvoCentri = new List<FitnesCentar>();
            
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            vlasnistvoCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            /* foreach (var item in fitnesCentri)
             {
                 if(item.KorisnickoIme.Equals(vlasnik.KorisnickoIme))
                 {
                     vlasnistvoCentri.Add(item);
                 }
             }*/
            ViewBag.fitnesCentri = vlasnistvoCentri;
            return View();
        }

        [HttpPost]
        public ActionResult DodajTrenera(Korisnik trener)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");

            List<FitnesCentar> vlasnistvoCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            ViewBag.fitnesCentri = vlasnistvoCentri;


            Dictionary<string,Korisnik> treneri = (Dictionary<string,Korisnik>)HttpContext.Application["treneri"];
            foreach(var item in treneri.Values)
            {
                if (trener.KorisnickoIme.Equals(item.KorisnickoIme))
                {
                    ViewBag.Message = $"Trener {trener.KorisnickoIme} je vec dodat!";
                    return View();
                }
            }

            trener.Uloga = (Uloga)Enum.Parse(typeof(Uloga), "TRENER");
            Data.DodajTreneraCentar(trener);
            return RedirectToAction("RadSaTrenerima");


        }
        [HttpPost]
        public ActionResult DetaljnoTrener(string korisnickoIme)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            Dictionary<string, Korisnik> treneri = (Dictionary<string, Korisnik>)HttpContext.Application["treneri"];
            Korisnik trener = new Korisnik();

            foreach(var item in treneri.Values)
            {
                if (item.KorisnickoIme.Equals(korisnickoIme))
                    trener = item;

            }
            ViewBag.trener = trener;
            return View();
        }
        [HttpPost]
        public ActionResult Blokiraj(string korisnickoIme)
        {
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");

            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> treninziTrenera = new List<GrupniTrening>();

            Dictionary<string,Korisnik> treneri = (Dictionary<string,Korisnik>)HttpContext.Application["treneri"];
            Korisnik blokirani = new Korisnik();

            foreach(var item in treneri.Values)
            {
                if (item.KorisnickoIme.Equals(korisnickoIme))
                    blokirani = item;
            }
            
          
            if (blokirani == null)
            {
                ViewBag.Message = $"Trener {blokirani.KorisnickoIme} ne postoji";
                return RedirectToAction("RadSaTrenerima");
            }

            string path = "~/App_Data/GrupniTreninzi.txt";


            foreach (var item in treninzi)
            {
                if (item.Trener.Equals(blokirani.KorisnickoIme))
                {
                 //   treninziTrenera.Add(item);
                    Data.ObrisiTrening(item,path);
                }
            }

            // Session["korisnik"] = korisnik;
            foreach(var item in blokirani.TreninziTrener)
            {
                blokirani.TreninziTrener.Remove(item);
            }
            Data.ObrisiTrenera(blokirani);

            ViewBag.Message = $"Trener {blokirani.KorisnickoIme} je uspesno blokiran";

            return RedirectToAction("RadSaTrenerima");
        }
        

        //************RAD SA FITNES CENTRIMA******************//
        //vraca view sa centrima vlasnika
        public ActionResult RadSaFitnesCentrima()
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            List<string> adrese = new List<string>();
            foreach(var item in fitnesCentri)
            {
                adrese.Add(item.AdresaCentra.ToString());
            }
            ViewBag.Adrese = adrese;
            ViewBag.fitnesCentri = fitnesCentri;
            return View();
        }

        public ActionResult DodajFitnesCentar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DodajFitnesCentar(string naziv, string ulica, string mesto, int broj,int postanskiBroj, int godinaOtvaranja, string korisnickoIme, int cenaMesecno, int cenaGodisnje, int cenaJednogTreninga, int cenaJednogGrupnog, int cenaJednogTrener)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            string path = "~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt";
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre(path);

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            List<string> adrese = new List<string>();

            Adresa adresa = new Adresa(ulica, Convert.ToString(broj), mesto, postanskiBroj);
            FitnesCentar fCentar = new FitnesCentar(naziv, adresa, godinaOtvaranja, korisnickoIme, cenaMesecno, cenaGodisnje, cenaJednogTreninga, cenaJednogGrupnog, cenaJednogTrener);

            FitnesCentar fc = fitnesCentri.Find(f => f.Id.Equals(fCentar.Id));
            if(fc!=null)
            {
                

                ViewBag.Message = $"Fitnes centar {fc.Naziv} vec postoji";
                return View();
            }
           
           
            fitnesCentri.Add(fCentar);
            Data.DodajFitnesCentar(fCentar, path);
            Data.DodajFitnesCentar(fCentar, "~/App_Data/FitnesCentri.txt");
            ViewBag.fitnesCentri = fitnesCentri;
            return RedirectToAction("RadSaFitnesCentrima");

        }
        [HttpPost]
        public ActionResult Filter(string naziv, string adresa, int godinaOtvaranja, int godinaOtvaranjaGornja)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");

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
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            if (!naziv.Equals(""))
            {
                fitnesCentri = GetPoNazivu(naziv);
            }
            if (!adresa.Equals(""))
            {
                List<FitnesCentar> filtriraniAdr = new List<FitnesCentar>();

                foreach (var item in fitnesCentri)
                {
                    if (item.AdresaCentra.ToString().Equals(adresa))
                        filtriraniAdr.Add(item);
                }
                fitnesCentri = filtriraniAdr;
            }
            if (godina != 0 || godinaGornja != 0)
            {
                List<FitnesCentar> filtrirani = new List<FitnesCentar>();

                foreach (var item in fitnesCentri)
                {
                    if (godina != 0 && godinaGornja == 0)
                    {
                        if (item.GodinaOtvaranja >= godina)
                            filtrirani.Add(item);
                    }
                    else if (godinaGornja != 0 && godina == 0)
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
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> filtrirani = new List<FitnesCentar>();
            if (naziv.Equals(""))
                return fitnesCentri;
            else
            {
                foreach (var item in fitnesCentri)
                {
                    if (item.Naziv.StartsWith(naziv))
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
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> treninziCentra = new List<GrupniTrening>();
            FitnesCentar centar = fitnesCentri.Find(f => f.Id.Equals(id));

            foreach (var item in treninzi)
            {
                if (item.CentarId.Equals(centar.Id))
                    treninziCentra.Add(item);

            }
            ViewBag.fitnesCentar = centar;
            ViewBag.treninzi = treninziCentra;
            return View();
        }

        [HttpPost]
        public ActionResult Modifikuj(string centarId)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            FitnesCentar fc = new FitnesCentar();

            foreach(var item in fitnesCentri)
            {
                if(item.Id.Equals(centarId))
                {
                    fc = item;
                }
            }
            ViewBag.fitnesCentar = fc;
            string[] adresaNiz = fc.AdresaCentra.ToString().Split(new string[] { ", " }, StringSplitOptions.None);
           

            ViewBag.ulica = adresaNiz[0];
            ViewBag.broj = adresaNiz[1];
            ViewBag.mesto = adresaNiz[2];
            ViewBag.postanskiBroj = adresaNiz[3];
            return View();
        }

       
        [HttpPost]
        public ActionResult SacuvajIzmene(string centarId, string naziv, string ulica, string mesto, int broj, int postanskiBroj, int godinaOtvaranja, string korisnickoIme, int cenaMesecno, int cenaGodisnje, int cenaJednogTreninga, int cenaJednogGrupnog, int cenaJednogTrener)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            string path = "~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt";
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre(path);
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");

            Dictionary<string, Korisnik> treneri = (Dictionary<string,Korisnik>)HttpContext.Application["treneri"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            FitnesCentar fcStari = new FitnesCentar();


            foreach(var item in fitnesCentri)
            {
                if(item.Id.Equals(centarId))
                {
                    fcStari = item;
                }
            }

            Adresa adresa = new Adresa(ulica, Convert.ToString(broj), mesto, postanskiBroj);
            FitnesCentar fCentar = new FitnesCentar(naziv, adresa, godinaOtvaranja, korisnickoIme, cenaMesecno, cenaGodisnje, cenaJednogTreninga, cenaJednogGrupnog, cenaJednogTrener);
            Data.IzmeniFitnesCentar(fcStari, fCentar,path);

            foreach (var trening in treninzi)
            {
                if(trening.CentarId.Equals(centarId))
                {
                    Data.ObrisiTrening(trening, "~/App_Data/GrupniTreninzi.txt");
                    trening.CentarId = fCentar.Id;
                    Data.DodajTrening(trening,trening.Trener, "~/App_Data/GrupniTreninzi.txt");
                }
            }
            foreach(var item in treneri.Keys)
            {
                if(item.Contains(centarId))
                {
                    Data.ObrisiTrenera(treneri[item]);
                    treneri[item].FitnesCentarId = fCentar.Id;
                    Data.DodajTreneraCentar(treneri[item]);
                }
            }
            return RedirectToAction("RadSaFitnesCentrima");
        }
        
        [HttpPost]
        public ActionResult ObrisiFitnesCentar(string id)
        {
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            HttpContext.Application["fitnesCentriVlasnik"] = Data.ReadFitnesCentre("~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");

            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            Dictionary<string, Korisnik> treneri = (Dictionary<string, Korisnik>)HttpContext.Application["treneri"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentriVlasnik"];
            FitnesCentar fc = fitnesCentri.Find(f => f.Id.Equals(id));

            foreach(var item in treninzi)
            {
                if(item.CentarId.Equals(fc.Id))
                {
                    ViewBag.poruka = "Ne moze se obrisati centar zbog zakazanih treninga!";
                    return View();
                }
            }
            foreach(var item in treneri.Values)
            {
                if (item.FitnesCentarId.Equals(fc.Id))
                    Data.ObrisiTrenera(item);
            }
            
            Data.ObrisiFitnesCentar(fc, "~/App_Data/" + vlasnik.KorisnickoIme + "Centri.txt");
            Data.ObrisiFitnesCentar(fc, "~/App_Data/FitnesCentri.txt");
            return RedirectToAction("RadSaFitnesCentrima");
        }
    }
}