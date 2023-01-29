using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zadatak.Models.FileControllers
{
    public class DataController
    {
        public static CentarFileController CentarFileController { get; set; }
        public static KorisnikFileController KorisnikFileController { get; set; }
        public static TreningFileController TreningFileController { get; set; }
        public static KomentarFileController KomentarFileController { get; set; }

        public static void Start()
        {
            CentarFileController = new CentarFileController();
            KorisnikFileController = new KorisnikFileController();
            TreningFileController = new TreningFileController();
            KomentarFileController = new KomentarFileController();
        }
    }
}