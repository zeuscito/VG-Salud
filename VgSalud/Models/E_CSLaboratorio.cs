using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CSLaboratorio
    {
        public int Id { get; set; }
        public int CodCue { get; set; }
        public string Paciente { get; set; }
        public string DesTipoCarnet { get; set; }
        public string Manipulador { get; set; }
        
        public int Prioridad { get; set; }
        public int idEstado { get; set; }
        public int Edad { get; set; }
        public string evento { get; set; }
        public string NumeroCarnet { get; set; }

        public string Procedencia { get; set; }

        public bool MuestraS { get; set; }
        public bool MuestraH { get; set; }

        public string HIV { get; set; }
        public string GrupoFactor { get; set; }
        public string RPR { get; set; }
        public string Parasitologico { get; set; }

        public string Observacion { get; set; }

        public string Apto { get; set; }

        public string Reevaluado { get; set; }

        public string FechaAtencion { get; set; }
        
    }
}