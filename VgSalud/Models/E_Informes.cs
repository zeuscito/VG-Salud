using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Informes
    {
        public string Carnet { get; set; }
        public string NumDoc { get; set; }
        public string ApePat { get; set; }
        public string ApeMat { get; set; }
        public string NomPac { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaAtenLab { get; set; }
        public string ObservacionLab { get; set; }
        public string ReevaluadoLab { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FechaAtenOdo { get; set; }
        public string ObservacionOdon { get; set; }
        public string ReevaluadoOndon { get; set; }
        public string AptoLab { get; set; }
        public string AptoOdon { get; set; }
        public string AptoMed { get; set; }

        public int Historia { get; set; }

        public int IdMedicina { get; set; }
        public int nroCarnet { get; set; }
        public string Observaciones { get; set; }
        public string ReevaluadoMed { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaAtenMed { get; set; }
        public int evento { get; set; }


        public int IdNroCarnet { get; set; }

    }
}