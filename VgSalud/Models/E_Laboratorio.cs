using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Laboratorio
    {
        public int Id { get; set; }
        public int IdProcedencia { get; set; }
        public string Manipulador { get; set; }
        public string Procedencia { get; set; }
        public string HIV { get; set; }
        public string GrupoFactor { get; set; }
        public string RPR { get; set; }
        public string Apto { get; set; }
        public string Reevaluado { get; set; }
        public string Observacion { get; set; }
        public int Prioridad{ get; set; }
        public bool MuestraSangre { get; set; }
        public bool MuestraHeces { get; set; }
        public DateTime FechaReg { get; set; }
        public DateTime FechaAten { get; set; }
        public int IdEstado { get; set; }
        public string CodSede { get; set; }
    }
}