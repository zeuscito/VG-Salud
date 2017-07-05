using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CSOdontologia
    {
        public int Id { get; set; }
        public int IdProcedencia { get; set; }
        public string Manipulador { get; set; }
        public int CodOdontograma { get; set; }
        public string AptoOdon { get; set; }
        public string Reevaluado  { get; set; }
        public string Observaciones { get; set; }

        public int IdEstado { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
        public string CodCue { get; set; }
    }
}