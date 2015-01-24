using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;


namespace Sah.Models
{
    
    public class DioIgreUnos
    {
        public int idDioIgre { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string Opis { get; set; }
        [Required]
        [Display(Name = "Naziv")]
        public string Naziv { get; set; }
        public Osoba korisnik { get; set; }
        [Required]
        [Display(Name = "Tezina")]
        public int Tezina { get; set; }
        public string poteziSpojeno { get; set; }
        [Required]
        [Display(Name = "Tip igre")]
        public int tipIgre { get; set; }
        
        [Display(Name = "Pozicija")]
        public HttpPostedFileBase Pozicija { get; set; }
    }
}