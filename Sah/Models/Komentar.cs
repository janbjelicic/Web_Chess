using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    public class Komentar
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idKomentar { get; set; }
        public Osoba korisnik{ get; set; }
        public DioIgre komentiranaPozicija { get; set; }
        public string sadrzaj { get; set; }
        public DateTime datUnos { get; set; }
    }
}