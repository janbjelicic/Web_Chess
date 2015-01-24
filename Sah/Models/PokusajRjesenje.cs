using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    
    public class PokusajRjesenje
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idPokusaj { get; set; }
        public DioIgre dioIgre { get; set; }
        public Osoba korisnik{ get; set; }
        //public List<StanjePloca> listaOtvorenihStanja { get; set; }
        public Potez trenutniPotez { get; set; }
        public int brojac { get; set; }
    }
}