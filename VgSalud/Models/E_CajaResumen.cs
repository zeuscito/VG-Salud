using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CajaResumen
    {

        public int CodCajRes { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaDeposito { get; set; }
        public DateTime FechaCaja { get; set; }
        public string TipoPago { get; set; }
        public decimal TipoCambio { get; set; }
        public decimal TotalDolares { get; set; }
        public decimal TotalUsuario { get; set; }
        public decimal TotalSistema { get; set; }
        public decimal Diferencia { get; set; }
        public bool Estado { get; set; }
        public string CodMedios { get; set; }
        public decimal Total { get; set; }

    }
}