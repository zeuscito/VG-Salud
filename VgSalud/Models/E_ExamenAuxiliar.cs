using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_ExamenAuxiliar
    {

        public int EA { get; set; }
        public int CodCue { get; set; }
        public int FE { get; set; }
        public string Especialidad { get; set; }
        public string Tarifa { get; set; }
        public int cant { get; set; }
        public string Otros { get; set; }
        public bool Estado { get; set; }
        public string Medico { get; set; }
        public string Servicio { get; set; }
        public string Examen_Proced { get; set; }
        public int Item { get; set; }

    }
}