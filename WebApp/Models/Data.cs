using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace WebApp.Models
{
    public class Data
    {
        public static List<Korisnik> ReadVlasnike(string path)
        {
            List<Korisnik> vlasnici = new List<Korisnik>();
            List<FitnesCentar> vlasnistvoCentri = new List<FitnesCentar>();

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/FitnesCentri.txt");

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {

                

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {
                    using (var stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr2 = new StreamReader(stream2, Encoding.Default))
                    {
                        string line2 = "";

                        string[] tokens = line.Split(';');
                        string[] nizCentara = tokens[8].Split(',');
                        Korisnik vlasnik = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], Convert.ToChar(tokens[4]), tokens[5], Convert.ToDateTime(tokens[6]), (Uloga)Enum.Parse(typeof(Uloga), tokens[7]));

                        while ((line2 = sr2.ReadLine()) != null)
                        {

                            string[] tokens2 = line2.Split(';');

                            if (tokens2[6].Equals(tokens[0]))
                            {
                                Adresa adresa = new Adresa(tokens2[1], tokens2[2], tokens2[3], Int32.Parse(tokens2[4]));

                                vlasnistvoCentri.Add(new FitnesCentar(tokens2[0], adresa, int.Parse(tokens2[5]), vlasnik.KorisnickoIme, int.Parse(tokens2[7]), int.Parse(tokens2[8]), int.Parse(tokens2[9]), int.Parse(tokens2[10]), int.Parse(tokens2[11])));
                            }
                        }
                        vlasnik.FitnesCentriVlasnik = vlasnistvoCentri;
                        vlasnici.Add(vlasnik);
                    }
                }
            }

            return vlasnici;
        }
        
        public static List<FitnesCentar> ReadFitnesCentre(string path)
        {
            List<FitnesCentar> fitnesCentri = new List<FitnesCentar>();

            path = HostingEnvironment.MapPath(path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {

                    string[] tokens = line.Split(';');

                    Adresa adresa = new Adresa(tokens[1], tokens[2], tokens[3], Int32.Parse(tokens[4]));
                    FitnesCentar fc = new FitnesCentar(tokens[0], adresa, int.Parse(tokens[5]), tokens[6], int.Parse(tokens[7]), int.Parse(tokens[8]), int.Parse(tokens[9]), int.Parse(tokens[10]), int.Parse(tokens[11]));
                    fitnesCentri.Add(fc);

                }
            }

            return fitnesCentri;

        }
        
        public static List<GrupniTrening> ReadTreninge(string path)
        {
            List<GrupniTrening> treninzi = new List<GrupniTrening>();

            path = HostingEnvironment.MapPath(path);
           
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            { string line = "";

                while ((line = sr1.ReadLine()) != null)
                {

                    string[] tokens = line.Split(';');

                    string centarId = tokens[2] + "(" + tokens[3] + ", " + tokens[4] + ", " + tokens[5] + ", " + tokens[6] + ")";
                    GrupniTrening trening = new GrupniTrening(tokens[0], (TipTreninga)Enum.Parse(typeof(TipTreninga), tokens[1]), centarId, int.Parse(tokens[7]), Convert.ToDateTime(tokens[8]), int.Parse(tokens[9]), int.Parse(tokens[10]), tokens[11]);
                    treninzi.Add(trening);
                }
            }
           

            return treninzi;
        
        }

        public static List<Korisnik> ReadRegistrovane(string path)
        {
            List<Korisnik> registrovani = new List<Korisnik>();

            path = HostingEnvironment.MapPath(path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {

                    string[] tokens = line.Split(';');
                    string date = tokens[6].Replace('-', '/');
                    Korisnik korisnik = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], Convert.ToChar(tokens[4]), tokens[5], Convert.ToDateTime(tokens[6]));
                    registrovani.Add(korisnik);

                }
            }

            return registrovani;

        }
        public static Dictionary<string,Korisnik> ReadRegistrovaneTrenere(string path)
        {
            Dictionary<string,Korisnik> registrovani = new Dictionary<string, Korisnik>();

            path = HostingEnvironment.MapPath(path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {

                    string[] tokens = line.Split(';');
                    string date = tokens[6].Replace('-', '/');
                    Korisnik korisnik = new Korisnik(tokens[0], tokens[1], tokens[2], tokens[3], Convert.ToChar(tokens[4]), tokens[5], Convert.ToDateTime(tokens[6]), tokens[7]);
                    registrovani.Add(korisnik.FitnesCentarId + korisnik.KorisnickoIme, korisnik);

                }
            }

            return registrovani;

        }
        public static void SacuvajKorisnika(Korisnik korisnik)
        {
        
            string korisnikStr = korisnik.KorisnickoIme + ";" + korisnik.Lozinka + ";" + korisnik.Ime + ";" + korisnik.Prezime +  ";" + korisnik.Pol + ";" + korisnik.Email + ";" + korisnik.DatumRodjenja.ToString("dd/MM/yyyy");
           
            string path2 = HostingEnvironment.MapPath("~/App_Data/RegistrovaniKorisnici.txt");

            using (StreamWriter writer = new StreamWriter(path2, append:true))
            {
                writer.WriteLine(korisnikStr);
            }
        }
        public static void ObrisiKorisnika(Korisnik korisnik)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/RegistrovaniKorisnici.txt");
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp.txt");

            string line = null;
            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {

                        if (line.Contains(korisnik.KorisnickoIme) && line.Contains(korisnik.Lozinka))
                            continue;

                        writer.WriteLine(line);
                }
              
            }
            File.Delete(path);
            File.Move(path2, path);
        }
        public static void IzmeniKorisnika(Korisnik stariKorisnik, Korisnik noviKorisnik)
        {
          
            ObrisiKorisnika(stariKorisnik);
            SacuvajKorisnika(noviKorisnik);
        }

        public static void DodajTreneraCentar(Korisnik trener)
        {
            string korisnikStr = trener.KorisnickoIme + ";" + trener.Lozinka + ";" + trener.Ime + ";" + trener.Prezime +  ";" + trener.Pol + ";" + trener.Email + ";" + trener.DatumRodjenja.ToString("dd/MM/yyyy")+";"+trener.FitnesCentarId;
            string path = HostingEnvironment.MapPath("~/App_Data/Treneri.txt");
            string path2 = HostingEnvironment.MapPath("~/App_Data/Trener"+trener.KorisnickoIme+".txt");
            string path3 = HostingEnvironment.MapPath("~/App_Data/Trener" + trener.KorisnickoIme + "Istorija.txt");
            


            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(korisnikStr);
            }
        }
        public static void ObrisiTrenera(Korisnik korisnik)
        {
            string line = null;
            string path = HostingEnvironment.MapPath("~/App_Data/Treneri.txt");
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp.txt");

            string pathTreninzi = HostingEnvironment.MapPath("~/App_Data/Trener" + korisnik.KorisnickoIme + ".txt");
            string pathTreninziIstorija = HostingEnvironment.MapPath("~/App_Data/Trener" + korisnik.KorisnickoIme + "Istorija.txt");

           
            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {

                    if (line.Contains(korisnik.KorisnickoIme) && line.Contains(korisnik.Lozinka))
                        continue;

                    writer.WriteLine(line);
                }

            }
            File.Delete(path);
            File.Move(path2, path);
        }

        public static void IzmeniTrenera(Korisnik stariTrener, Korisnik noviTrener)
        {
            ObrisiTrenera(stariTrener);
            DodajTreneraCentar(noviTrener);
        }
       
        public static void DodajFitnesCentar(FitnesCentar fCentar, string path)
        {
            //dodaje fitnes centar u txt fajl
            path = HostingEnvironment.MapPath(path);

            string[] adresaNiz = fCentar.AdresaCentra.ToString().Split(new string[] { ", " }, StringSplitOptions.None);
            string adresa = adresaNiz[0] + ";" + adresaNiz[1] + ";" + adresaNiz[2] + ";" + adresaNiz[3];
            string korisnikStr = fCentar.Naziv + ";" + adresa + ";" + fCentar.GodinaOtvaranja + ";" + fCentar.KorisnickoIme + ";" + fCentar.CenaMesecno + ";" + fCentar.CenaGodisnje + ";" + fCentar.CenaJednogTreninga + ";" + fCentar.CenaJednogGrupnog + ";" + fCentar.CenaJednogTrener;

            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(korisnikStr);
            }
         
                  
        }


        public static void ObrisiFitnesCentar(FitnesCentar fCentar, string path)
        {
            string[] adresaNiz = fCentar.AdresaCentra.ToString().Split(new string[] { ", " }, StringSplitOptions.None);
            string adresa = adresaNiz[0] + ";" + adresaNiz[1] + ";" + adresaNiz[2] + ";" + adresaNiz[3];

            string line = null;

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp.txt");
             

            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {

                    if (line.Contains(fCentar.Naziv) && line.Contains(adresa))
                        continue;

                    writer.WriteLine(line);
                }

            }
            File.Delete(path);
            File.Move(path2, path);
        }
        public static void IzmeniFitnesCentar(FitnesCentar stariCentar, FitnesCentar noviCentar, string path)
        {
            //path = HostingEnvironment.MapPath(path);
            string path2 = "~/App_Data/FitnesCentri.txt";

            ObrisiFitnesCentar(stariCentar, path);
            ObrisiFitnesCentar(stariCentar, path2);
            DodajFitnesCentar(noviCentar, path);
            DodajFitnesCentar(noviCentar, path2);
        }
        public static void IzmeniTreninge(GrupniTrening trening, string korisnickoIme)
        {
            string path = "~/App_Data/GrupniTreninzi.txt";
            
            ObrisiTrening(trening, path);
            trening.Trener = korisnickoIme;

            DodajTrening(trening, korisnickoIme, path);
            


        }
        public static void DodajTrening(GrupniTrening trening, string korisnickoIme,string path)
        {
            string[] centarId = trening.CentarId.Split(new string[] { "(" }, StringSplitOptions.None);
            string nazivCentra = centarId[0];
            string[] adresaNiz = centarId[1].Split(new string[] { ", " }, StringSplitOptions.None);
            string postanskiBroj = adresaNiz[3].Substring(0, adresaNiz[3].Length - 1);
            string adresa = adresaNiz[0] + ";" + adresaNiz[1] + ";" + adresaNiz[2] + ";" + postanskiBroj;

            string datum = trening.VremeTreninga.ToString("dd/MM/yyyy HH:mm");
            string treningStr = trening.Naziv + ";" + Convert.ToString(trening.VrstaTreninga) + ";" +nazivCentra+";"+ adresa + ";" + trening.TrajanjeTreninga + ";" + datum + ";" + trening.MaxPosetilaca + ";" + trening.TrenutnoPosetilaca + ";" + korisnickoIme;
            path = HostingEnvironment.MapPath(path);

            using (StreamWriter writer = new StreamWriter(path, append: true))
            {
                writer.WriteLine(treningStr);
            }
        }

        public static void ObrisiTrening(GrupniTrening trening, string path)
        {
            
            string line = null;

            string[] niz = trening.CentarId.Split(new string[] { "(" }, StringSplitOptions.None);
            string nazivCentra = niz[0];
            string[] adresa = niz[1].Split(new string[] { ", " }, StringSplitOptions.None);
            string treningCentarId = nazivCentra + ";" + adresa[0] + ";" + adresa[1] + ";" + adresa[2] + ";" + adresa[3].Substring(0,adresa[3].Length-1) + ";";

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp2.txt");


            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {

                    if (line.Contains(treningCentarId) && line.Contains(trening.Naziv))
                        continue;

                    writer.WriteLine(line);
                }

            }
            File.Delete(path);
            File.Move(path2, path);
        }

        public static void PrijaviNaTrening(string treningId, string korisnickoIme)
        {
            string path = "~/App_Data/SpisakTreninga.txt";
            path = HostingEnvironment.MapPath(path);
            using (StreamWriter writer = new StreamWriter(path, append:true))
            {
                writer.WriteLine(korisnickoIme + ";" + treningId);
            }
        }
        public static List<GrupniTrening> ReadSpisakTreningaPoKorisniku(string path, string korisnickoIme)
        {
            List<GrupniTrening> treninzi = new List<GrupniTrening>();
            List<FitnesCentar> vlasnistvoCentri = new List<FitnesCentar>();

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {
                    using (var stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr2 = new StreamReader(stream2, Encoding.Default))
                    {
                        string line2 = "";

                        string[] tokens = line.Split(';');

                        if (tokens[0].Equals(korisnickoIme))
                        {

                            while ((line2 = sr2.ReadLine()) != null)
                            {

                                string[] tokens2 = line2.Split(';');

                                string centarId = tokens2[2] + "(" + tokens2[3] + ", " + tokens2[4] + ", " + tokens2[5] + ", " + tokens2[6] + ")";
                                GrupniTrening trening = new GrupniTrening(tokens2[0], (TipTreninga)Enum.Parse(typeof(TipTreninga), tokens2[1]), centarId, int.Parse(tokens2[7]), Convert.ToDateTime(tokens2[8]), int.Parse(tokens2[9]), int.Parse(tokens2[10]), tokens2[11]);

                                if (trening.TreningId.Equals(tokens[1]))
                                {
                                    treninzi.Add(trening);
                                }

                            }
                        }
                        
                    }

                }
                
            }
            return treninzi;
            
        }

        public static GrupniTrening ReadSpisakTreningaPoTreningu(string path, string treningId)
        {
            GrupniTrening izabraniTrening = new GrupniTrening();
            List<FitnesCentar> vlasnistvoCentri = new List<FitnesCentar>();

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/GrupniTreninzi.txt");
            using (var stream1 = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream1, Encoding.Default))
            {

                

                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {
                    using (var stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var sr2 = new StreamReader(stream2, Encoding.Default))
                    {
                        string line2 = "";

                        string[] tokens = line.Split(';');

                        if (tokens[1].Equals(treningId))
                        {

                            while ((line2 = sr2.ReadLine()) != null)
                            {

                                string[] tokens2 = line2.Split(';');

                                string centarId = tokens2[2] + "(" + tokens2[3] + ", " + tokens2[4] + ", " + tokens2[5] + ", " + tokens2[6] + ")";
                                GrupniTrening trening = new GrupniTrening(tokens2[0], (TipTreninga)Enum.Parse(typeof(TipTreninga), tokens2[1]), centarId, int.Parse(tokens2[7]), Convert.ToDateTime(tokens2[8]), int.Parse(tokens2[9]), int.Parse(tokens2[10]), tokens2[11]);

                                if (trening.TreningId.Equals(tokens[1]))
                                {
                                    izabraniTrening = trening;
                                    break;
                                }

                            }
                            break;
                        }
                     
                    }
                }
            }
            

            return izabraniTrening;

        }

        public static List<Korisnik>ReadSpisakKorisnikaPoTreningu(string path, string treningId)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            

            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/RegistrovaniKorisnici.txt");

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr1 = new StreamReader(stream, Encoding.Default))
            {
                string line = "";

                while ((line = sr1.ReadLine()) != null)
                {

                        string line2 = "";

                        string[] tokens = line.Split(';');

                        if (tokens[1].Equals(treningId))
                        {

                            using (var stream2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            using (var sr2 = new StreamReader(stream2, Encoding.Default))
                            {
                                while ((line2 = sr2.ReadLine()) != null)
                                {

                                    string[] tokens2 = line2.Split(';');
                                    if (tokens[0].Equals(tokens2[0]))
                                    {

                                        string date = tokens2[6].Replace('-', '/');
                                        Korisnik korisnik = new Korisnik(tokens2[0], tokens2[1], tokens2[2], tokens2[3], Convert.ToChar(tokens2[4]), tokens2[5], Convert.ToDateTime(tokens2[6]));
                                        korisnici.Add(korisnik);
                                    }

                                }
                            }
                        }
                       
                    

                }

            }
            return korisnici;

            
        }
        public static void ObrisiTreningSaSpiskaPoKorisniku(string korisnickoImeStaro)
        {
            string path = "~/App_Data/SpisakTreninga.txt";
            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp2.txt");

            string line = "";

            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(new string[] { ";" }, StringSplitOptions.None);

                    if (tokens[0].Equals(korisnickoImeStaro))
                        continue;

                    writer.WriteLine(line);
                }

            }
            File.Delete(path);
            File.Move(path2, path);

        }
        public static void ObrisiTreningSaSpiskaPoTreningu(string treningId)
        {
            string path = "~/App_Data/SpisakTreninga.txt";
            path = HostingEnvironment.MapPath(path);
            string path2 = HostingEnvironment.MapPath("~/App_Data/Temp2.txt");

            string line = "";

            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(path2))
            {

                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(new string[] { ";" }, StringSplitOptions.None);

                    if (tokens[1].Equals(treningId))
                        continue;

                    writer.WriteLine(line);
                }

            }
            File.Delete(path);
            File.Move(path2, path);

        }

    }
}


