using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Master
    {
        public string nombre { get; set; }
        public string cantidad { get; set; }

        public DateTime HoraServidor { get; set; }

        public int IdForm { get; set; }
        public string NombreForm { get; set; }
        public string AliasForm { get; set; } 

    }
}