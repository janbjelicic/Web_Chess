using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sah.Models
{
    //cvrsta tocka s id-em koja nam omogucuje setanje po stablu
    public class StanjePloca
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int idStanjePloca { get; set; }
        public StanjePloca stanjeRoditelj { get; set; }
        //potez kojim smo dosli u to stanje
        public Potez Potez { get; set; }

        public string DohvatiPut()
        {
            //vracaj se po stablu i tak dobijes put
            return "";
        }
    }

    
}