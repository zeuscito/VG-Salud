using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class Alertas
    {
        public int id { get; set; }
        public string mensaje { get; set; }
        public string asunto { get; set; }
        public bool estado { get; set; }
        public string usuarioRecibe { get; set; }
        public string usuarioManda { get; set; }
        public DateTime fechaRegistro { get; set; }
        public TimeSpan horaRegistro { get; set; }
        public string duracion { get; set; }
        public int cantidad { get; set; }
        public string nombreEnvia { get; set; }
    }
}