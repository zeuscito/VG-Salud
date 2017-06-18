using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CuentaDetalle
    {
        public int Procedencia { get; set; }
        public int CodCue { get; set; }
        public int Item { get; set; }
        public string Tarifa { get; set; }
        public int CodProce { get; set; }
        public int CodDetalleP { get; set; }
        public string CodSede { get; set; }
        public decimal precio { get; set; }
        public decimal igv { get; set; }
        public decimal total { get; set; }
        public string EstDet { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
        public decimal precioUni { get; set; }
        public string FechaAten { get; set; } 
        public string TurnoAten { get; set; }
        public string RegMedico { get; set; }
        public int Cantidad { get; set; }
        public string MedicoEnvia { get; set; }
        public string CodCuentaGeneral { get; set; }
        public string NombreTarifario { get; set; }
    }
}