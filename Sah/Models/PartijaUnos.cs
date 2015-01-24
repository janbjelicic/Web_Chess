using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public class PartijaUnos
    {
        public int idPartija { get; set; }
        [Required]
        [Display(Name = "Ime")]
        public string imeBijeli { get; set; }
        [Required]
        [Display(Name = "Prezime")]
        public string prezimeBijeli { get; set; }
        [Required]
        [Display(Name = "Rejting")]
        public int medunarodniRejtingBjeli { get; set; }
        [Required]
        [Display(Name = "Ime")]
        public string imeCrni { get; set; }
        [Required]
        [Display(Name = "Prezime")]
        public string prezimeCrni { get; set; }
        [Required]
        [Display(Name = "Rejting")]
        public int medunarodniRejtingCrni { get; set; }
        [Required]
        [Display(Name = "Datum")]
        public DateTime datumOdigravanja { get; set; }
        [Required]
        [Display(Name = "Potezi")]
        public string poteziSpojeni { get; set; }
        public List<Potez> potezi { get; set; }
    }
}