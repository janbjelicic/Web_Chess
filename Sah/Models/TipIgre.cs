using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Sah.Models
{
    public enum tipIgre
    {
        otvaranje=1,
        sredisnjica,
        konacnica,
        problem
    }
    public class TipIgre
    {
        [Key]
        public int idTip { get; set; }
        public int naziv { get; set; }
    }
}