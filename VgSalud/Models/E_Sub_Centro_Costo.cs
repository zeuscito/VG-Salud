using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Sub_Centro_Costo
    {

        public string IdScc { get; set; }
        [Required(ErrorMessage = "Descripcion Requerida")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Seleccione un Centro de Costo")]
        public string Idcc { get; set; }
        public bool Estado { get; set; } 
        public string Evento { get; set; }
    }
}