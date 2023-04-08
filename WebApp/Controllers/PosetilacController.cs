using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac

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
        public ActionResult Izmeni(Korisnik korisnik)
        {
            Korisnik stariKorisnik = (Korisnik)Session["korisnik"];

            HttpContext.Application["spisakTreninga"] = Data.ReadSpisakTreningaPoKorisniku("~/App_Data/SpisakTreninga.txt", stariKorisnik.KorisnickoIme);
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["spisakTreninga"];

            Data.ObrisiTreningSaSpiskaPoKorisniku(stariKorisnik.KorisnickoIme);
            foreach(var item in treninzi)
            {
                Data.PrijaviNaTrening(item.TreningId, korisnik.KorisnickoIme);
            }

            Data.IzmeniKorisnika(stariKorisnik, korisnik);
            Session["korisnik"] = korisnik;



            return RedirectToAction("Profil");
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
            FitnesCentar centar = fitnesCentri.Find(f => f.Id.Equals(id));
            //FitnesCentar centarDetaljno;

            DateTime trenutno = DateTime.Now;


            foreach (var item in treninzi)
            {
                if (item.CentarId.Equals(centar.Id) && DateTime.Compare(item.VremeTreninga, trenutno) > 0)
                    treninziCentra.Add(item);

            }
            ViewBag.fitnesCentar = centar;
            ViewBag.treninzi = treninziCentra;
            return View();
        }
        [HttpPost]
        public ActionResult PrijavaNaTrening(string treningId)
        {
            HttpContext.Application["treninzi"] = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            HttpContext.Application["treneri"] = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Application["fitnesCentri"] = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            HttpContext.Application["registrovani"] = Data.ReadRegistrovane("~/App_Data/RegistrovaniKorisnici.txt");
            //HttpContext.Application["spisakPosetilaca"] = Data.ReadSpisakPosetilaca("~/App_Data/SpisakPosetilaca.txt", treningId);

            Korisnik trenutniPosetilac = (Korisnik)Session["korisnik"];

            Dictionary<string, Korisnik> treneri = (Dictionary<string, Korisnik>)HttpContext.Application["treneri"];
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];
            List<FitnesCentar> fCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<Korisnik> registrovaniKorisnici = (List<Korisnik>)HttpContext.Application["registrovani"];
           // List<string> spisakPosetilaca = (List<string>)HttpContext.Application["spisakPosetilaca"];

            GrupniTrening trening = new GrupniTrening();
            Korisnik trener = new Korisnik();
            //   FitnesCentar fCentar = new FitnesCentar();

            trenutniPosetilac.TreninziKlijent = Data.ReadSpisakTreningaPoKorisniku("~/App_Data/SpisakTreninga.txt", trenutniPosetilac.KorisnickoIme);
            
            foreach (var item in treninzi)
            {
                if(item.TreningId.Equals(treningId))
                {
                    trening = item;
                   // trenutniPosetilac.TreninziKlijent.Add(item);
                    break;
                }
            }
            foreach(var item in treneri.Values)
            {
                if (trening.Trener.Equals(item.KorisnickoIme))
                {
                    trener = item;
                    break;
                }
            }/*
            foreach(var item in fCentri)
            {
                if (item.Id.Equals(trener.FitnesCentarId))
                    fCentar = item;
            }*/

            bool postoji = false;

            if(trening.TrenutnoPosetilaca<trening.MaxPosetilaca)
            {
                foreach(var item in trenutniPosetilac.TreninziKlijent)
                {
                    if(item.TreningId.Equals(trening.TreningId))
                    {
                        postoji = true;
                    }
                }
                if(!postoji)
                {
                    //trening.SpisakPosetilaca.Add(trenutniPosetilac);
                    trenutniPosetilac.TreninziKlijent.Add(trening);
                    trening.TrenutnoPosetilaca++;
                    Data.IzmeniTreninge(trening, trening.Trener);
                    Data.PrijaviNaTrening(trening.TreningId, trenutniPosetilac.KorisnickoIme);
                    ViewBag.flag = 1;

                }
                else
                {
                    ViewBag.poruka = "Vec ste prijavljeni na izabrani trening";
                   // ViewBag.flag = 2;
                }
            }
            else
            {
                ViewBag.poruka = "Na izabranom treningu nema slobodnog mesta!";
                //ViewBag.flag = 3;
            }
            ViewBag.trening = trening;
            ViewBag.trener = trener;

            return View();
        }

        public ActionResult SpisakTreninga()
        {
            Korisnik posetilac = (Korisnik)Session["korisnik"];

            HttpContext.Application["treninzi"] = Data.ReadSpisakTreningaPoKorisniku("~/App_Data/SpisakTreninga.txt", posetilac.KorisnickoIme);
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            DateTime trenutno = DateTime.Now;
            List<GrupniTrening> treninziIstorija = new List<GrupniTrening>();

            foreach (var item in treninzi)
            {
                if (DateTime.Compare(item.VremeTreninga, trenutno) < 0)
                {

                   treninziIstorija.Add(item);

                }

            }

            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.tipovi = tipovi;
            ViewBag.nonetip = TipTreninga.NONE;

            ViewBag.treninzi = treninziIstorija;
            return View();
        }



        [HttpPost]
        public ActionResult Filter(string naziv,TipTreninga tipTreninga, string nazivFitnesCentra)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];


            ViewBag.treninzi = GetFilter(naziv, tipTreninga, nazivFitnesCentra);

            List<TipTreninga> tipovi = new List<TipTreninga>();
            tipovi.Add(TipTreninga.BODYBUILDING);
            tipovi.Add(TipTreninga.TONE);
            tipovi.Add(TipTreninga.POWERLIFTING);
            tipovi.Add(TipTreninga.CARDIO);
            tipovi.Add(TipTreninga.BODY_PUMP);
            tipovi.Add(TipTreninga.YOGA);
            ViewBag.tipovi = tipovi;
            ViewBag.nonetip = TipTreninga.NONE;
            return View("SpisakTreninga");
        }

        private List<GrupniTrening> GetFilter(string naziv, TipTreninga tipTreninga, string nazivFitnesCentra)
        {
            List<GrupniTrening> treninzi = (List<GrupniTrening>)HttpContext.Application["treninzi"];

            if (!naziv.Equals(""))
            {
                treninzi = GetPoNazivu(naziv);
            }
            if (!(tipTreninga == TipTreninga.NONE))
            {
                List<GrupniTrening> filtriraniTip = new List<GrupniTrening>();

                foreach (var item in treninzi)
                {

                    if (item.VrstaTreninga == tipTreninga)
                        filtriraniTip.Add(item);
                }
                treninzi = filtriraniTip;
            }
            if (!nazivFitnesCentra.Equals(""))
            {
                List<GrupniTrening> filtriraniPoCentru = new List<GrupniTrening>();
                foreach (var item in treninzi)
                {
                    if (item.CentarId.StartsWith(nazivFitnesCentra))
                    {
                        filtriraniPoCentru.Add(item);

                    }
                }
                treninzi = filtriraniPoCentru;
            }

            DateTime trenutno = DateTime.Now;
            List<GrupniTrening> treninziIstorija = new List<GrupniTrening>();
            foreach (var item in treninzi)
            {
                if (DateTime.Compare(item.VremeTreninga, trenutno) < 0)
                    treninziIstorija.Add(item);

            }
            return treninziIstorija;

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
    }
}