using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Informes
    {
        public int NroCarnet { get; set; }
        public string NumDoc { get; set; }
        public string ApePat { get; set; }
        public string ApeMat { get; set; }
        public string NomPac { get; set; }
        public DateTime FechaAtenLab { get; set; }
        public string ObservacionLab { get; set; }
        public string ReevaluadoLab { get; set; }
        public DateTime FechaAtenOdon { get; set; }
        public string ObservacionOdon { get; set; }
        public string ReevaluadoOndon { get; set; }
        public string AptoLab { get; set; }
        public string AptoOdon { get; set; }
        public string AptoMed { get; set; }
    }
}