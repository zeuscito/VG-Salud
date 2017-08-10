using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_AtencionVarias
    {
        public int CodAten { get; set; }
        public string CodSede { get; set; }
        public int CodCue { get; set; }
        public int Historia { get; set; }
        public string NomPac { get; set; }
        public string CodCatPac { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public string CodUsu { get; set; }
        public string EstConsul { get; set; }

        public string CodEspec { get; set; }
        public string CodServ { get; set; }
        public string CodMed { get; set; }
        public string CodTar { get; set; }


        public string CodigoEspecialidad { get; set; }

    }
}