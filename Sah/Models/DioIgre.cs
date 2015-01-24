using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public enum tezinaProblema
    {
        nijeProblem,
        pocetnik,
        amater,
        kandidat,
        majstor,
        velemajstor
    }

    public class DioIgre
    {
    
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        //id pozicije
        public int idDioIgre { get; set; }
        //naziv je u problemima obicno gdje je odigrano
        //a u ostalima bas naziv pojedine karakteristicne pozicije
        public string naziv { get; set; }
        //objasnjava tezinu problema ili opisuje ostale pozicije
        public string opis { get; set; }
        //korisnik koji je unio
        public Osoba korisnik { get; set; }
        public DateTime datumUnos { get; set; }
        //postoji samo za probleme
        public int tezina { get; set; }
        //postoji samo za probleme, oni koji vode do rjesenja
        public List<Potez> potezi { get; set; }
        //definira se tip
        public TipIgre tipIgre { get; set; }
        //public StanjePloca pocetnoStanje { get; set; }
        public List<Komentar> komentari { get; set; }
        //nek za poteze bude stablo
        //to bi bila druga i bolja varijanta za poboljsanje
    }
}