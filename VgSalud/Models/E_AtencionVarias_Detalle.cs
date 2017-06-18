using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_AtencionVarias_Detalle
    {
        public int id { get; set; }
        public int CodAtenDet { get; set; }
        public int CodAten { get; set; }
        public int item { get; set; }
        public string CodSede { get; set; }
        public string CodEspec { get; set; }
        public string CodServ { get; set; }
        public string CodMed { get; set; }
        public string CodTar { get; set; }
        public string CodTipTar { get; set; }
        public string CodSTipTar { get; set; }
        public int Cantida { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public string MedicoEnvia { get; set; }
        public string EspeciEnvia { get; set; }
        public string Estado { get; set; }
        public string CodUsu { get; set; }
        public decimal Precio { get; set; }
        public int historia { get; set; }
        public int CodCue { get; set; }
        public int CodCueY { get; set; }
        public string Turno { get; set; }

    }
}