using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using Sah.Models;
using Sah.Pozicije;
using Sah.Filters;
//using Newtonsoft.Json;

namespace Sah.Controllers
{
    [InitializeSimpleMembership]
    public class PozicijaController : Controller
    {
        private SahPodaci db = new SahPodaci();
        string[] separators = { "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8.", "9.", "10" };
        //
        // GET: /Pozicija/
        // Ovo bi trebala biti lista problema
        public ActionResult Index()
        {
            var pozicije = db.Pozicije.Include(f => f.korisnik).Include(f=>f.tipIgre);
            return View(pozicije.ToList());
        }

        //lista otvaranja na posebnom View
        public ActionResult Otvaranje()
        {
            var otvaranja = (from u in db.Pozicije.Include("tipIgre")
                             where u.tipIgre.naziv == 1
                             select u);
            return View(otvaranja.ToList());
        }

        //lista sredisnjica na posebnom View
        public ActionResult Sredisnjica()
        {
            var sredisnjica = (from u in db.Pozicije.Include("tipIgre")
                             where u.tipIgre.naziv == 2
                             select u);
            return View(sredisnjica.ToList());

        }

        //lista konacnica na posebnom View
        public ActionResult Konacnica()
        {
            var konacnica = (from u in db.Pozicije.Include("tipIgre")
                               where u.tipIgre.naziv == 3
                               select u);
            return View(konacnica.ToList());
        }

        //lista problema na posebnom View
        public ActionResult Problem()
        {
            var problem = (from u in db.Pozicije.Include("tipIgre")
                               where u.tipIgre.naziv == 4
                               select u);
            return View(problem.ToList());

        }


        //filtriranje problema po tezini
        [HttpPost]
        public ActionResult Problem(int tezina)
        {
            var problem=(from u in db.Pozicije.Include("tipIgre")
                        where u.tipIgre.naziv==4 && u.tezina==tezina 
                        select u);
            return View(problem.ToList());
        }
        //
        // GET: /Pozicija/Details/5
        //Opis pozicije 
        public ActionResult Details(int id = 0)
        {
            //tu se mora mjenjati ovisno o pokusaju salji podatke 
            //tako da korisnik zna drugi put kad dode na cemu je 
            DioIgre dioigre = db.Pozicije.Find(id);
            if (dioigre == null)
            {
                return HttpNotFound();
            }
            List<ViewKomentar> komentari = db.Komentari.Where(k => k.komentiranaPozicija.idDioIgre == id)
                                                .Select(k => new ViewKomentar
                                                {
                                                    osoba = k.korisnik.korisnickoIme,
                                                    vrijeme = k.datUnos,
                                                    tekst = k.sadrzaj
                                                }).ToList();
            komentari.Reverse();
            //prikaz komentara sadrzi trenutnu rjesenost problema
            //ako se radi o problemu, komentare na poziciju te samu poziciju
            PrikazKomentara prik = new PrikazKomentara();
            prik.dioIgre = dioigre;
            prik.komentari = komentari;
            prik.potezi = String.Empty;
            prik.opis = String.Empty;
            PokusajRjesenje pokusaj = db.PokusajiRjesenja.Where(x => x.korisnik.idOsoba == WebSecurity.CurrentUserId).
                                        Where(x => x.dioIgre.idDioIgre == id).SingleOrDefault();
            if (pokusaj != null)
            {
                pokusaj.dioIgre = db.Pozicije.Include(x => x.potezi).SingleOrDefault(x => x.idDioIgre == id);
                if(pokusaj.trenutniPotez!=null)
                    prik.opis = pokusaj.trenutniPotez.opis;
                //trenutni potez je null jer je tocno rijeseno...jos to treba rijesiti 
                string ispisPoteza = String.Empty;
                for (int i = 0; i < pokusaj.dioIgre.potezi.Count; i++)
                {
                    if (pokusaj.trenutniPotez == null || (!pokusaj.trenutniPotez.Equals(pokusaj.dioIgre.potezi[i])))
                    {
                        ispisPoteza += pokusaj.dioIgre.potezi[i].potezZapisanNotacijski + ",";
                    }
                    else
                    {
                        break;
                    }
                }
                if(!ispisPoteza.Equals(String.Empty))
                    ispisPoteza = ispisPoteza.Substring(0, ispisPoteza.Length - 1);
                prik.potezi = ispisPoteza;
            }
            return View(prik);
        }

        //Ovdje bi se problemi rjesavali
        [HttpPost]
        public ActionResult RjesiProblem(int id, string tekst)
        {
            PokusajRjesenje novi = db.PokusajiRjesenja.Where(x => x.korisnik.idOsoba == WebSecurity.CurrentUserId).
                                        Where(x => x.dioIgre.idDioIgre == id).SingleOrDefault();
            //stvaramo novi pokusaj rjesenja
            if (novi == null)
            {
                novi = new PokusajRjesenje();
                novi.korisnik = db.Osobe.Find(WebSecurity.CurrentUserId);
                novi.dioIgre = db.Pozicije.Include(x=>x.potezi).SingleOrDefault(x=>x.idDioIgre==id);
                novi.brojac = 0;
                novi.trenutniPotez = novi.dioIgre.potezi[0];
                db.PokusajiRjesenja.Add(novi);
                db.SaveChanges();
            }
            string rezultat=String.Empty;
            string ispisPoteza = String.Empty;
            string komentar = String.Empty;
            //dodajemo kontekst poteza
            novi.dioIgre = db.Pozicije.Include(x => x.potezi).SingleOrDefault(x => x.idDioIgre == id);
            if (novi.trenutniPotez == null)
            {
                rezultat="Problem je uspjeno rjesen i ne moze se rjesavati ponovno";
            }
            else if (tekst.Trim().Equals(novi.trenutniPotez.potezZapisanNotacijski))
            {
                //sljedeci potez je trenutni, provjera da smo dosli do kraja i dodjela bodova
                rezultat="Uspjesno!";
                for (int i = 0; i < novi.dioIgre.potezi.Count; i++)
                {
                    if (novi.trenutniPotez.Equals(novi.dioIgre.potezi[i]))
                    {
                        if (i == (novi.dioIgre.potezi.Count - 1))
                        {
                            //promjena bodova osobi koja je tocno rjesila
                            Osoba a = db.Osobe.SingleOrDefault(x => x.idOsoba == WebSecurity.CurrentUserId);
                            int brojBodova = 5 - novi.brojac;
                            if (brojBodova < 0)
                            {
                                brojBodova = 0;
                            }
                            a.bodovi += brojBodova * novi.dioIgre.tezina;
                            db.Entry(a).State = EntityState.Modified;
                            db.SaveChanges();
                            novi.trenutniPotez = null;
                            break;
                        }
                        else
                        {
                            novi.trenutniPotez = novi.dioIgre.potezi[i + 2];
                            break;
                        }

                    }
                }
                
            }
            else
            {
                novi.brojac++;
                rezultat="Neuspjesno!";
            }
            db.Entry(novi).State = EntityState.Modified;
            db.SaveChanges();
            //komentar uz potez
            if (novi.trenutniPotez != null)
            {
                komentar = novi.trenutniPotez.opis;
            }
            //ispis poteza
            for (int i = 0; i < novi.dioIgre.potezi.Count; i++)
            {
                if (novi.trenutniPotez == null || (!novi.trenutniPotez.Equals(novi.dioIgre.potezi[i])))
                {
                    ispisPoteza += novi.dioIgre.potezi[i].potezZapisanNotacijski + ",";
                }
                else
                {
                    break;
                }
            }
            if(!ispisPoteza.Equals(String.Empty))
                ispisPoteza = ispisPoteza.Substring(0, ispisPoteza.Length-1);
            //vratimo json na stranicu
            if (Request.IsAjaxRequest())
            {
                var test = new {rezultat,ispisPoteza,komentar };
                return Json(test);
            }
            return RedirectToAction("Details", new { id = id });
        }

        //ovdje dodajemo komentare
        [HttpPost]
        public ActionResult DodajKomentar(int id, string tekst)
        {
            Komentar novi = new Komentar();            
            novi.datUnos = DateTime.Now;
            novi.sadrzaj = tekst;
            //osoba mora biti registrirana
            try
            {
                novi.korisnik = db.Osobe.Find(WebSecurity.CurrentUserId);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
            novi.komentiranaPozicija = db.Pozicije.Find(id);
            db.Komentari.Add(novi);
            db.SaveChanges();
            string datum = novi.datUnos.ToString();
            string korisnickoIme = novi.korisnik.korisnickoIme;
            if (Request.IsAjaxRequest())
            {
                var test = new { korisnickoIme, novi.sadrzaj, datum};
                return Json(test);
            }
            return RedirectToAction("Details", new { id = id });
        }

        //
        // GET: /Pozicija/Create
        //Ovdje se dodaju pozicije

        public ActionResult Create()
        {
            Osoba trenutni = db.Osobe.Find(WebSecurity.CurrentUserId);
            if (trenutni.autoritetKorisnik == 2)
            {
                ViewBag.idOsoba = new SelectList(db.Osobe, "idOsoba", "korisnickoIme");
                return View();
            }
            else
                return RedirectToAction("Index", new { controle = 0 });
        }

        //
        // POST: /Pozicija/Create

        [HttpPost]
        public ActionResult Create(DioIgreUnos dioigre)
        {
            if (ModelState.IsValid)
            {
                DioIgre pozicija = new DioIgre();
                #region UnosPozicije
                pozicija.opis = dioigre.Opis;
                pozicija.datumUnos = DateTime.Now;
                //osoba mora biti registrirana
                try
                {
                    pozicija.korisnik = db.Osobe.Find(WebSecurity.CurrentUserId);
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index", "Home");
                }
                pozicija.tezina = dioigre.Tezina;
                pozicija.naziv = dioigre.Naziv;
                TipIgre tipPozicije = new TipIgre();
                tipPozicije.naziv = dioigre.tipIgre;
                pozicija.tipIgre = tipPozicije;
                #endregion
                Osoba a = db.Osobe.Find(WebSecurity.CurrentUserId);
                a.brUnesenihPozicija += 1;
                //Ako je pozicija problem onda unesi bodove za unos korisniku
                if (dioigre.tipIgre == 4)
                {
                    a.bodovi += 1;
                    //ovdje su varijante zajedno s brojkama i pretpostavljamo da se u komentarima 
                    //ne nalaze tocke
                    try
                    {
                        List<string> varijante= dioigre.poteziSpojeno.Split(separators,StringSplitOptions.RemoveEmptyEntries).ToList();
                        for (int i = 0; i < varijante.Count; i++)
                        {
                            varijante[i] = varijante[i].Replace("\n", "");
                            varijante[i] = varijante[i].Replace("\r", "");
                            varijante[i] = varijante[i].Trim();
                        }
                        string osnovnaVarijanta = varijante[0];
                        List<string> potezKomentar = osnovnaVarijanta.Split(',').ToList();
                        pozicija.potezi = new List<Potez>();
                        //unos poteza
                        for (int i = 0; i < potezKomentar.Count; i++)
                        {
                            Potez pot = new Potez();
                            List<string> pojediniPotez = potezKomentar[i].Split('/').ToList();
                            pot.potezZapisanNotacijski = pojediniPotez[0].Trim();
                            pot.opis = pojediniPotez[1].Trim();
                            db.Potezi.Add(pot);
                            db.SaveChanges();
                            pozicija.potezi.Add(pot);
                        }
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Profil", "Home");
                    }
                }
                db.Entry(a).State = EntityState.Modified;
                pozicija.komentari = new List<Komentar>();
                db.Pozicije.Add(pozicija);
                db.SaveChanges();
                var skladiste = new SpremistePozicija();
                skladiste.SpremiPoziciju(pozicija.idDioIgre, dioigre.Pozicija);
                return RedirectToAction("Profil","Home");
            }

            return View(dioigre);
        }

        //
        // GET: /Pozicija/Edit/5
        //uredi problem s tim da se potezi resetiraju
        public ActionResult Edit(int id = 0)
        {
            DioIgre dioigre = db.Pozicije.Include(x=>x.korisnik).Include(x=>x.tipIgre).Single(x=>x.idDioIgre==id);
            if (dioigre == null)
            {
                return HttpNotFound();
            }
            if (dioigre.korisnik.idOsoba != WebSecurity.CurrentUserId)
            {
                return RedirectToAction("Index", "Home");
            }
            DioIgreUnos pozicija = new DioIgreUnos();
            pozicija.idDioIgre = id;
            pozicija.korisnik = db.Osobe.Find(dioigre.korisnik.idOsoba);
            pozicija.Naziv = dioigre.naziv;
            pozicija.Opis = dioigre.opis;
            pozicija.Tezina = dioigre.tezina;
            pozicija.tipIgre = dioigre.tipIgre.naziv;
            pozicija.poteziSpojeno = String.Empty;
            if (pozicija.tipIgre == 4)
            {
                foreach (Potez pot in dioigre.potezi)
                {
                    string notacijskiZapis = pot.potezZapisanNotacijski;
                    string opis = pot.opis;
                    pozicija.poteziSpojeno += notacijskiZapis + "/" + opis + "/";
                    if (!pot.Equals(dioigre.potezi.Last()))
                    {
                        pozicija.poteziSpojeno += ",";
                    }
                }
            }
            return View(pozicija);
        }

        //
        // POST: /Pozicija/Edit/5

        [HttpPost]
        public ActionResult Edit(DioIgreUnos dioigre)
        {
            
            if (ModelState.IsValid)
            {
                DioIgre pozicija = db.Pozicije.Include(x=>x.komentari).Include(x=>x.korisnik).Include(x=>x.tipIgre).
                                        Include(x=>x.potezi).Single(x=>x.idDioIgre==dioigre.idDioIgre);
                pozicija.opis = dioigre.Opis;
                pozicija.datumUnos = DateTime.Now;
                pozicija.tezina = dioigre.Tezina;
                pozicija.naziv = dioigre.Naziv;
                TipIgre tipPozicije = new TipIgre();
                tipPozicije.naziv = dioigre.tipIgre;
                pozicija.tipIgre = tipPozicije;
                if (dioigre.tipIgre == 4)
                {
                    try
                    {
                        List<string> varijante = dioigre.poteziSpojeno.Split(separators, StringSplitOptions.RemoveEmptyEntries).ToList();
                        for (int i = 0; i < varijante.Count; i++)
                        {
                            varijante[i] = varijante[i].Replace("\n", "");
                            varijante[i] = varijante[i].Replace("\r", "");
                            varijante[i] = varijante[i].Trim();
                        }
                        string osnovnaVarijanta = varijante[0];
                        List<string> potezKomentar = osnovnaVarijanta.Split(',').ToList();
                        pozicija.potezi = new List<Potez>();
                        //unos poteza
                        for (int i = 0; i < potezKomentar.Count; i++)
                        {
                            Potez pot = new Potez();
                            List<string> pojediniPotez = potezKomentar[i].Split('/').ToList();
                            pot.potezZapisanNotacijski = pojediniPotez[0].Trim();
                            pot.opis = pojediniPotez[1].Trim();
                            db.Potezi.Add(pot);
                            db.SaveChanges();
                            pozicija.potezi.Add(pot);
                        }
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Profil", "Home");
                    }
                }
                pozicija.komentari = new List<Komentar>();
                db.Entry(pozicija).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profil","Home");
            }
            return View(dioigre);
        }

        //
        // GET: /Pozicija/Delete/5

        public ActionResult Delete(int id = 0)
        {
            DioIgre dioigre = db.Pozicije.Include(f=>f.korisnik).Include(f=>f.komentari).Single(f => f.idDioIgre == id);
            if (dioigre == null)
            {
                return HttpNotFound();
            }
            if (dioigre.korisnik.idOsoba != WebSecurity.CurrentUserId)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(dioigre);
        }

        //
        // POST: /Pozicija/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            DioIgre dioigre = db.Pozicije.Include(f=>f.korisnik).Include(f=>f.potezi).
                                Include(f=>f.komentari).Single(f=>f.idDioIgre==id);
            if (dioigre.tezina > 0)
            {
                Osoba k = db.Osobe.Find(dioigre.korisnik.idOsoba);
                k.bodovi -= 1;
                k.brUnesenihPozicija -= 1;
                db.Entry(k).State = EntityState.Modified;
            }
            db.Pozicije.Remove(dioigre);
            db.SaveChanges();
            return RedirectToAction("Profil","Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}