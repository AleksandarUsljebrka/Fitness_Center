using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Models;

namespace WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            List<Korisnik> vlasnici = Data.ReadVlasnike("~/App_Data/Vlasnici.txt");
            HttpContext.Current.Application["vlasnici"] = vlasnici;

            List<FitnesCentar> fitnesCentri = Data.ReadFitnesCentre("~/App_Data/FitnesCentri.txt");
            HttpContext.Current.Application["fitnesCentri"] = fitnesCentri;

            List<GrupniTrening> treninzi = Data.ReadTreninge("~/App_Data/GrupniTreninzi.txt");
            HttpContext.Current.Application["treninzi"] = treninzi;

            List<Korisnik> registrovani = Data.ReadRegistrovane("~/App_Data/RegistrovaniKorisnici.txt");
            HttpContext.Current.Application["registrovani"] = registrovani;

            Dictionary<string,Korisnik> treneri = Data.ReadRegistrovaneTrenere("~/App_Data/Treneri.txt");
            HttpContext.Current.Application["treneri"] = treneri;

           
           
        }
    }
}
