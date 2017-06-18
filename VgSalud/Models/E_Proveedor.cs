using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Proveedor
    {


        public int IdPro { get; set; }
        public string Razon { get; set; }
        public string Ruc { get; set; }
        public string Direc { get; set; }
        public string Contacto { get; set; }
        public string Telef { get; set; }
        public string Correo { get; set; }
        public string Observ { get; set; }
        public string Rubro { get; set; }
        public string TipoPersona { get; set; }
        public bool Estado { get; set; }

    }
}