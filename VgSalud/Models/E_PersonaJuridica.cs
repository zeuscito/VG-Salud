using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_PersonaJuridica
    {

        public int CodPerJur { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public bool Estado { get; set; }
        public string CodCue { get; set; } 
        public string Concatenado { get; set; }

    }
}