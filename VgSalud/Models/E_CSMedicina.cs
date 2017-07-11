using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CSMedicina
    {
        public int Id { get; set; }
        public int CodCue { get; set; }
        public string Paciente { get; set; }
        public string DesTipoCarnet { get; set; }
        public int idEstado { get; set; }
        public int Edad { get; set; }
        public int IdProcedencia { get; set; }
        public string Manipulador { get; set; }
        public string Procedencia { get; set; }
        public string Conclusion{ get; set; }
        public string AptoMed { get; set; }
        public string Reevaluado { get; set; }
        public string Observaciones { get; set; }
        public int Prioridad { get; set; }
        public DateTime FechaRegMed { get; set; }
        public DateTime FechaAtenMed { get; set; }
        public int Estado { get; set; }
        public string CodSede { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }



    }
}