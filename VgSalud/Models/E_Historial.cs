using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Historial
    {
        public int historia { get; set; }
        public string CodEspec { get; set; }
        public string NomServ { get; set; }

        public string NomMed { get; set; }

        public string FecRegMed { get; set; }
        public string Turno { get; set; }

        public string DescripcionProc { get; set; }

        public int Item { get; set; }
        public int FE { get; set; }
        public DateTime? FechaAtencion { get; set; }
        public DateTime FechaPago { get; set; }
        public string fechaI { get; set; }
        public string fechaF { get; set; }
        public string Evento { get; set; }
        public string[] arreglo { get; set; }

    }
}