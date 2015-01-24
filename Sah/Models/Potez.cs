using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public class Potez
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idPotez { get; set; }
        public string potezZapisanNotacijski { get; set; }
        public string opis { get; set; }
        //public StanjePloca trenutnoStanje { get; set; }
        //za odredeno stanje i odreden potez idemo u listu sljedecih stanja
        //public List<StanjePloca> listaSljedecihStanje { get; set; }
    }
}