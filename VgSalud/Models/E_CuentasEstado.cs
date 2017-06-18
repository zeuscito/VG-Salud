using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CuentasEstado
    {


        public int CodCue { get; set; }
        public int Item { get; set; }
        public string fecha { get; set; }
        public string Servicio { get; set; }
        public string Medico { get; set; }
        public string Tarifa { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

    }
}