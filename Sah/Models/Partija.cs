using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public class Partija
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idPartija { get; set; }
        public Osoba bijeli { get; set; }
        public Osoba crni { get; set; }
        public DateTime datumOdigravanja { get; set; }
        public List<Potez> potezi { get; set; }
        public DioIgre dioIgre { get; set; }
        public List<Komentar> komentari { get; set; }
    }
}