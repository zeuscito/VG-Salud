using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Diagnostico
    {
        public int CIe10 { get; set; }
        public int CodCue { get; set; }
        public int FE { get; set; }
        public int Historia { get; set; }
        public string Descripcion { get; set; }
        public bool Bit { get; set; }
        public string Medico { get; set; }
        public string Servicio { get; set; }
        public int Item { get; set; }
    }
}