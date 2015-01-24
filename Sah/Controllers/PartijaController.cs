using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sah.Models;
using Sah.Filters;
using Sah.Pozicije;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;

namespace Sah.Controllers
{
    public class PartijaController : Controller
    {
        private SahPodaci db = new SahPodaci();

        //
        // GET: /Partija/

        public ActionResult Index()
        {
            return View(db.Partije.Include(x=>x.bijeli).Include(x=>x.crni).ToList());
        }

        //
        // GET: /Partija/Details/5

        public ActionResult Details(int id = 0)
        {
            Partija partija = db.Partije.Include(x=>x.potezi).Include(x=>x.bijeli).Include(x=>x.crni).
                                    SingleOrDefault(x=>x.idPartija==id);
            if (partija == null)
            {
                return HttpNotFound();
            }
            PartijaUnos partijaUnos = new PartijaUnos();
            partijaUnos.idPartija = partija.idPartija;
            partijaUnos.imeBijeli = partija.bijeli.ime;
            partijaUnos.prezimeBijeli = partija.bijeli.prezime;
            partijaUnos.medunarodniRejtingBjeli = partija.bijeli.medunarodniRejting;
            partijaUnos.imeCrni = partija.crni.ime;
            partijaUnos.prezimeCrni = partija.crni.prezime;
            partijaUnos.medunarodniRejtingCrni = partija.crni.medunarodniRejting;
            partijaUnos.potezi = partija.potezi;
            partijaUnos.poteziSpojeni = String.Empty;
            foreach (Potez pot in partija.potezi)
            {
                string notacijskiZapis = pot.potezZapisanNotacijski;
                string opis = pot.opis;
                partijaUnos.poteziSpojeni += notacijskiZapis + "/" + opis + "/";
                if (!pot.Equals(partija.potezi.Last()))
                {
                    partijaUnos.poteziSpojeni += ",";
                }
            }
            return View(partijaUnos);
        }

        //
        // GET: /Partija/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Partija/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PartijaUnos partijaUnos)
        {
            if (ModelState.IsValid)
            {
                Partija partija = new Partija();
                #region Unos bijelog
                
                
                    Osoba bijeli = db.Osobe.Where(x => x.ime == partijaUnos.imeBijeli).SingleOrDefault(x => x.prezime == partijaUnos.prezimeBijeli);
                
                if (bijeli != null)
                {
                    bijeli.brUnesenihPozicija++;
                    db.Entry(bijeli).State = EntityState.Modified;
                    db.SaveChanges();
                    partija.bijeli = bijeli;
                }
                else
                {
                    bijeli = new Osoba();
                    //registracija svih korisnika radi problema s indeksima u bazi
                    WebSecurity.CreateUserAndAccount(partijaUnos.imeBijeli+"_"+partijaUnos.prezimeBijeli, "123456");
                    bijeli.ime = partijaUnos.imeBijeli;
                    bijeli.prezime = partijaUnos.prezimeBijeli;
                    bijeli.medunarodniRejting = partijaUnos.medunarodniRejtingBjeli;
                    bijeli.datumRegistracija = DateTime.Now;
                    bijeli.brUnesenihPozicija = 1;
                    bijeli.pozicije = null;
                    bijeli.komentari=null;
                    bijeli.partije = new List<Partija>();
                    bijeli.korisnickoIme = String.Empty;
                    bijeli.tipOsoba = Convert.ToInt32(TipOsoba.sahist);
                    bijeli.autoritetKorisnik = 3;
                    bijeli.bodovi = 0;
                    db.Osobe.Add(bijeli);
                    db.SaveChanges();
                    partija.bijeli = bijeli;
                }
                #endregion
                #region Unos crnog
                Osoba crni =db.Osobe.Where(x => x.ime == partijaUnos.imeCrni).SingleOrDefault(x => x.prezime == partijaUnos.prezimeCrni);
                if (crni != null)
                {
                    crni.brUnesenihPozicija++;
                    db.Entry(crni).State = EntityState.Modified;
                    db.SaveChanges();
                    partija.crni = crni;
                }
                else
                {
                    crni = new Osoba();
                    //registracija svih korisnika radi problema s indeksima u bazi
                    WebSecurity.CreateUserAndAccount(partijaUnos.imeCrni+"_"+partijaUnos.prezimeCrni, "123456");
                    crni.ime = partijaUnos.imeCrni;
                    crni.prezime = partijaUnos.prezimeCrni;
                    crni.medunarodniRejting = partijaUnos.medunarodniRejtingCrni;
                    crni.datumRegistracija = DateTime.Now;
                    crni.brUnesenihPozicija = 1;
                    crni.pozicije = null;
                    crni.komentari = null;
                    crni.partije = new List<Partija> ();
                    crni.korisnickoIme = String.Empty;
                    crni.tipOsoba = Convert.ToInt32(TipOsoba.sahist);
                    crni.autoritetKorisnik = 3;
                    crni.bodovi = 0;
                    db.Osobe.Add(crni);
                    db.SaveChanges();
                    partija.crni = (crni);
                }
                #endregion
                #region Unos poteza
                //izbaciti zareze iz komentara
                List<string> potezi = partijaUnos.poteziSpojeni.Split(',').ToList();
                partija.potezi = new List<Potez>();
                try
                {
                    foreach (string a in potezi)
                    {
                        Potez novi = new Potez();
                        List<string> potezKomentar = a.Split('/').ToList();
                        novi.potezZapisanNotacijski = potezKomentar[0].Trim();
                        novi.opis = potezKomentar[1].Trim();
                        db.Potezi.Add(novi);
                        db.SaveChanges();
                        partija.potezi.Add(novi);
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }
                #endregion
                partija.datumOdigravanja=partijaUnos.datumOdigravanja;
                partija.dioIgre = null;
                partija.komentari = null;
                db.Partije.Add(partija);
                db.SaveChanges();
                //bijeli.partije.Add(partija);
                //db.Entry(bijeli).State = EntityState.Modified;
                //db.SaveChanges();
                //crni.partije.Add(partija);
                //db.Entry(crni).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partijaUnos);
        }

        //
        // GET: /Partija/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Partija partija = db.Partije.Include(x=>x.crni).Include(x=>x.bijeli).Include(x=>x.potezi).SingleOrDefault(x=>x.idPartija==id);
            if (partija == null)
            {
                return HttpNotFound();
            }
            //provjeriti da li je na ovu stranicu dosao admin
            PartijaUnos partijaUnos = new PartijaUnos();
            partijaUnos.imeBijeli = partija.bijeli.ime;
            partijaUnos.imeCrni = partija.crni.ime;
            partijaUnos.prezimeBijeli = partija.bijeli.prezime;
            partijaUnos.prezimeCrni = partija.crni.prezime;
            partijaUnos.medunarodniRejtingCrni = partija.crni.medunarodniRejting;
            partijaUnos.medunarodniRejtingBjeli = partija.bijeli.medunarodniRejting;
            partijaUnos.idPartija = id;
            partijaUnos.datumOdigravanja = partija.datumOdigravanja;
            partijaUnos.poteziSpojeni = String.Empty;
            
            foreach (Potez pot in partija.potezi)
            {
                string notacijskiZapis = pot.potezZapisanNotacijski;
                string opis = pot.opis;
                partijaUnos.poteziSpojeni += notacijskiZapis + "/" + opis + "/";
                if(!pot.Equals(partija.potezi.Last()))
                {
                    partijaUnos.poteziSpojeni+=",";
                }
            }
            return View(partijaUnos);
        }

        //
        // POST: /Partija/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PartijaUnos partijaUnos)
        {
            if (ModelState.IsValid)
            {
                Partija partija = db.Partije.Include(x=>x.potezi).Include(x=>x.crni).Include(x=>x.bijeli).
                                    Single(x=>x.idPartija==partijaUnos.idPartija);
                partija.bijeli.ime = partijaUnos.imeBijeli;
                partija.bijeli.prezime = partijaUnos.prezimeBijeli;
                partija.bijeli.medunarodniRejting = partijaUnos.medunarodniRejtingBjeli;
                partija.crni.ime = partijaUnos.imeCrni;
                partija.crni.prezime = partijaUnos.prezimeCrni;
                partija.crni.medunarodniRejting = partijaUnos.medunarodniRejtingCrni;
                partija.datumOdigravanja = partijaUnos.datumOdigravanja;
                List<string> potezi = partijaUnos.poteziSpojeni.Split(',').ToList();
                partija.potezi = new List<Potez>();
                try
                {
                    foreach (string a in potezi)
                    {
                        Potez novi = new Potez();
                        List<string> potezKomentar = a.Split('/').ToList();
                        novi.potezZapisanNotacijski = potezKomentar[0].Trim();
                        novi.opis = potezKomentar[1].Trim();
                        db.Potezi.Add(novi);
                        db.SaveChanges();
                        partija.potezi.Add(novi);
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }
                db.Entry(partija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partijaUnos);
        }

        //
        // GET: /Partija/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Partija partija = db.Partije.Find(id);
            if (partija == null)
            {
                return HttpNotFound();
            }
            return View(partija);
        }

        //
        // POST: /Partija/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partija partija = db.Partije.Include(x=>x.potezi).Single(x=>x.idPartija==id);
            db.Partije.Remove(partija);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}