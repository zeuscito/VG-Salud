using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CajaDetalle
    {

        public string CodSede { get; set; }
        public int CodCaja { get; set; }
        public int CodCue { get; set; }
        public int item { get; set; }
        public string CodTar { get; set; }
        public string NomTar { get; set; }
        public int Cantidad { get; set; }
        public decimal PUnit { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public decimal TazaIgv { get; set; }

    }
}