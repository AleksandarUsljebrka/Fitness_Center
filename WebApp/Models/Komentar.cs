using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Komentar
    {
        public Korisnik Posetilac { get; set; }
        public FitnesCentar fitnesCentarKomentar { get; set; }
        public string TextKom { get; set; }
        int Ocena { get; set; }
    }
}