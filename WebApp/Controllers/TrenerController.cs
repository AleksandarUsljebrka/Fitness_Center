using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
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

        public ActionResult Profil()
        {
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            ViewBag.korisnik = korisnik;
            string datum = korisnik.DatumRodjenja.ToString("dd/MM/yyyy");
            string[] datumNiz = datum.Split('-');
            ViewBag.dan = datumNiz[0];
            ViewBag.mesec = datumNiz[1];
            ViewBag.godina = datumNiz[2];
            return View();
        }

        [HttpPost]
        public ActionResult Izmeni(Korisnik noviTrener)
        {
            Korisnik stariTrener= (Korisnik)Session["korisnik"];

            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> centri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> treninziTrenera = new List<GrupniTrening>();
            FitnesCentar fc = centri.Find(f => f.Id.Equals(stariTrener.FitnesCentarId));
            foreach(var item in treninzi)
            {
                if (item.Trener.Equals(stariTrener.KorisnickoIme))
                    treninziTrenera.Add(item);
            }

            //  Data.IzmeniKorisnika(stariKorisnik, korisnik);
            Session["korisnik"] = noviTrener;
            Data.IzmeniTrenera(stariTrener, noviTrener);
            foreach(var item in treninziTrenera)
            {
                Data.IzmeniTreninge(item, noviTrener.KorisnickoIme);
            }

            return RedirectToAction("Profil");
        }

        public ActionResult RadSaTreninzima()
        {
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            Korisnik trener = (Korisnik)Session["korisnik"];
            trener.TreninziTrener = new List<GrupniTrening>();

            DateTime trenutno = DateTime.Now;

            foreach(var item in treninzi)
            {
                if(item.Trener.Equals(trener.KorisnickoIme) && DateTime.Compare(item.VremeTreninga, trenutno)>0)
                {
                
                    trener.TreninziTrener.Add(item);
                    
                }

            }
            ViewBag.treninzi = trener.TreninziTrener;
       
            return View();
        }
        
        
        public ActionResult DodajTrening()
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            Korisnik trener = (Korisnik)Session["korisnik"];
           
            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.treninzi = tipovi;
            return View();
        }

        [HttpPost]
        public ActionResult DodajTrening(GrupniTrening noviTrening)
        {
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");

            List<FitnesCentar> centri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            string path = "~/App_Data/GrupniTreninzi.txt";


            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            GrupniTrening trening = treninzi.Find(t => t.TreningId.Equals(noviTrening.TreningId));

            if(trening != null)
            {
                ViewBag.poruka = $"Trening vec postoji";
                return View();
            }

            Korisnik trener = (Korisnik)Session["korisnik"];

            DateTime trenutno = DateTime.Now;
            DateTime najkasnijeVreme = DateTime.Now.AddHours(3);

            if(DateTime.Compare(noviTrening.VremeTreninga,najkasnijeVreme)>0)
            {

                FitnesCentar fc = centri.Find(c => c.Id.Equals(trener.FitnesCentarId));
                noviTrening.CentarId = fc.Id;

           
                trener.TreninziTrener.Add(noviTrening);
                Data.DodajTrening(noviTrening,trener.KorisnickoIme,path);
             //   Data.DodajTrening(noviTrening, trener.KorisnickoIme, "~/App_Data/" + trener.KorisnickoIme + "Istorija.txt");
                return RedirectToAction("RadSaTreninzima");
            }
            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.treninzi = tipovi;
            ViewBag.poruka = "Trening morate zakazati minimum 3h unapred!";
            return View();
        }

        [HttpPost]
        public ActionResult ObrisiTrening(string treningId)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            Korisnik trener = (Korisnik)Session["korisnik"];

            string path = "~/App_Data/GrupniTreninzi.txt";

            GrupniTrening treningBrisanje = trener.TreninziTrener.Find(t=>t.TreningId.Equals(treningId));
            
            DateTime trenutno = DateTime.Now;


            if(DateTime.Compare(treningBrisanje.VremeTreninga,trenutno)>0)
            {

                if (treningBrisanje.TrenutnoPosetilaca != 0)
                {
                    ViewBag.poruka = "Ne moze biti obrisan zbog postojecih prijava!";
                }
            
                else
                {
                    Data.ObrisiTrening(treningBrisanje, path);
                    ViewBag.poruka = "Uspesno obrisan trening!";
                }
            }
            else
            {
                ViewBag.poruka = "Ne mozete brisati treninge u proslosti";
            }


            return View();
        }
        [HttpPost]
        public ActionResult ModifikujTrening(string treningId)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            GrupniTrening trening = treninzi.Find(tr => tr.TreningId.Equals(treningId));

            ViewBag.trening = trening;
            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.treninzi = tipovi;
            return View();
        }

        [HttpPost]
        public ActionResult SacuvajIzmeneTreninga(GrupniTrening noviTrening,string treningId)
        {
            Korisnik trener = (Korisnik)Session["korisnik"];

            string pathGrupni = "~/App_Data/GrupniTreninzi.txt";
            string noviTreningId = noviTrening.Naziv + noviTrening.VrstaTreninga + Convert.ToString(noviTrening.VremeTreninga);

            HttpContext.Application["treningSaSpiska"] = Data.ReadSpisakTreningaPoTreningu("~/App_Data/SpisakTreninga.txt", treningId);
            GrupniTrening treningIzmenaSpisak = (GrupniTrening)HttpContext.Application["treningSaSpiska"];
            
            //dobavljamo sve korisnike koji su prijavljeni na trening koji se menja
            HttpContext.Application["spisakKorisnika"] = Data.ReadSpisakKorisnikaPoTreningu("~/App_Data/SpisakTreninga.txt", treningId);
            List<Korisnik> korisniciSaTreninga = (List<Korisnik>)HttpContext.Application["spisakKorisnika"];

            HttpContext.Application["treninzi"] = Data.ReadTreninge(pathGrupni);
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            GrupniTrening stariTrening = treninzi.Find(tr => tr.TreningId.Equals(treningId));

            
            DateTime trenutno = DateTime.Now;
            if(DateTime.Compare(stariTrening.VremeTreninga,trenutno)>0)
            {
                noviTrening.TreningId = noviTreningId;
                Data.ObrisiTrening(stariTrening, pathGrupni);
                Data.DodajTrening(noviTrening, trener.KorisnickoIme, pathGrupni);

                Data.ObrisiTreningSaSpiskaPoTreningu(treningId);
                foreach(var korisnik in korisniciSaTreninga)
                {
                    Data.PrijaviNaTrening(noviTrening.TreningId, korisnik.KorisnickoIme);
                }
                return RedirectToAction("RadSaTreninzima");
            }

            ViewBag.poruka = "Ne mogu se menjati treninzi u proslosti!";
            return View();
        }
        public ActionResult Istorija()
        {
           
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            Korisnik trener = (Korisnik)Session["korisnik"];
            trener.TreninziTrener = new List<GrupniTrening>();

            DateTime trenutno = DateTime.Now;

            foreach (var item in treninzi)
            {
                if (item.Trener.Equals(trener.KorisnickoIme) && DateTime.Compare(item.VremeTreninga, trenutno) < 0)
                {

                    trener.TreninziTrener.Add(item);

                }

            }
            ViewBag.treninzi = trener.TreninziTrener;

            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.tipovi = tipovi;
            ViewBag.nonetip = TipTreninga.NONE;

            return View();
        }

        [HttpPost]
        public ActionResult SpisakPosetilaca(string treningId)
        {
            HttpContext.Application["spisakKorisnika"] = Data.ReadSpisakKorisnikaPoTreningu("~/App_Data/SpisakTreninga.txt", treningId);
            List<Korisnik> korisniciSaTreninga = (List<Korisnik>)HttpContext.Application["spisakKorisnika"];

            ViewBag.posetioci = korisniciSaTreninga;
            return View();
        }

        private List<GrupniTrening> GetPoNazivu(string naziv)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<GrupniTrening> filtrirani = new List<GrupniTrening>();
            if (naziv.Equals(""))
                return treninzi;
            else
            {
                foreach (var item in treninzi)
                {
                    if (item.Naziv.StartsWith(naziv))
                    {
                        filtrirani.Add(item);
                        
                    }
                }
                return filtrirani;
            }
        }

        private List<GrupniTrening> GetFilter(string naziv, TipTreninga tipTreninga)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            if (!naziv.Equals(""))
            {
                treninzi = GetPoNazivu(naziv);
            }
            if (!(tipTreninga == TipTreninga.NONE))
            {
                List<GrupniTrening> filtriraniTip= new List<GrupniTrening>();

                foreach (var item in treninzi)
                {
                    
                    if (item.VrstaTreninga == tipTreninga)
                        filtriraniTip.Add(item);
                }
                treninzi = filtriraniTip;
            }
            /* DateTime dateMin = Convert.ToDateTime("01-01-0001");
             DateTime dateMax = Convert.ToDateTime("31-12-3000");

             if (DateTime.Compare(datumDonji,dateMin)>0 || DateTime.Compare(dateMax, datumGornji)>0)
             {
                 List<GrupniTrening> filtrirani = new List<GrupniTrening>();

                 foreach (var item in treninzi)
                 {
                     if (DateTime.Compare(datumDonji, dateMin) > 0 && DateTime.Compare(dateMax, datumGornji) == 0)
                     {
                         if (DateTime.Compare(item.VremeTreninga, datumDonji) > 0)
                             filtrirani.Add(item);
                     }
                     else if (DateTime.Compare(dateMax, datumGornji) > 0 && DateTime.Compare(datumDonji, dateMin) == 0)
                     {
                         if (DateTime.Compare(datumGornji, item.VremeTreninga)>0)
                             filtrirani.Add(item);
                     }
                     else
                     {

                         if (DateTime.Compare(item.VremeTreninga, datumDonji) > 0 && DateTime.Compare(datumGornji, item.VremeTreninga) > 0)
                             filtrirani.Add(item);

                     }
                 }
                 treninzi = filtrirani;
             }*/
            DateTime trenutno = DateTime.Now;
            List<GrupniTrening> treninziIstorija = new List<GrupniTrening>();
            foreach(var item in treninzi)
            {
                if (DateTime.Compare(item.VremeTreninga, trenutno) < 0)
                    treninziIstorija.Add(item);

            }
            return treninziIstorija;

        }
        [HttpPost]
        public ActionResult Filter(string naziv, TipTreninga tipTreninga)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

           

            //if (tipTreninga.Equals("0")) tipTreninga= TipTreninga.NONE;
            ViewBag.treninzi = GetFilter(naziv, tipTreninga);

            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.tipovi = tipovi;
            ViewBag.nonetip = TipTreninga.NONE;
            return View("Istorija");
        }

    }
}
