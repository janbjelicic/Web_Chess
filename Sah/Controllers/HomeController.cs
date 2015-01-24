using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sah.Models;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using System.IO;
using System.Text;
using Sah.Filters;
using System.Data.Entity;

namespace Sah.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        
        private SahPodaci db = new SahPodaci();

        public ActionResult Index()
        {
                 var   zadnjiProblemi = (from u in db.Pozicije
                                      where u.tezina > 0
                                      select u).OrderByDescending(f => f.datumUnos).Take(6);

                 var tablica = (from u in db.Osobe where u.autoritetKorisnik==2
                           select u).OrderByDescending(f => f.bodovi).Take(10);
           
            ViewBag.tablica = tablica;
            return View(zadnjiProblemi);
        }

        public ActionResult Profil(){
            Osoba trenutni = db.Osobe.Include(x=>x.pozicije).Single(x=>x.idOsoba==WebSecurity.CurrentUserId);
            if (trenutni == null)
            {
                return HttpNotFound();
            }
            return View(trenutni);
        }

        public ActionResult Citati()
        {
            return View();
        }

        public ActionResult Povijest()
        {
            return View();
        }
    }
}
