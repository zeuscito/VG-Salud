using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CajaPago
    {

        public int CodCaja { get; set; }
        public int item { get; set; }
        public string CODMEDIOS { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteSoles { get; set; }
        public string CodTipMon { get; set; }
        public decimal TipoCambio { get; set; }
        public bool Estado { get; set; }

        public string NomMedios { get; set; }
        public string NomMoneda { get; set; }
        public int CodCue { get; set; }
        public string seriePago { get; set; }
        public string rucPago { get; set; }
        public string tipPago { get; set; }
        public string tipMoneda { get; set; }
        public decimal Monto { get; set; }
        public decimal montoCambio { get; set; }

    }
}