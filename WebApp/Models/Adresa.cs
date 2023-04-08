using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Adresa
    {
        public string Ulica { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public int PostanskiBr { get; set; }

        public Adresa(string ulica, string br, string mesto, int postBr)
        {
            this.Ulica = ulica;
            this.Broj = br;
            this.Mesto = mesto;
            this.PostanskiBr = postBr;
        }
        public override string ToString()
        {
            return Ulica + ", " + Broj + ", " + Mesto + ", " + PostanskiBr;
        }
    }
}