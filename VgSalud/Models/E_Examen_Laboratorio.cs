using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Examen_Laboratorio : E_Citas

    {
        public int idExamen { get; set; }
        public int idTipoExamen { get; set; }
        public string resultado { get; set; }
        public DateTime fecha { get; set; }
        public string crea { get; set; }
        public string modifica { get; set; }
        public string elimina { get; set; }
        public bool estexa { get; set; }
        public int CodCue { get; set; }



    }
}