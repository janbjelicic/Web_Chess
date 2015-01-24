using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sah.Models
{
    public class PrikazKomentara
    {
        public DioIgre dioIgre { get; set; }
        public List<ViewKomentar> komentari { get; set; }
        public string potezi { get; set; }
        public string opis { get; set; }
    }
}