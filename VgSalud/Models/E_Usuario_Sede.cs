using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Usuario_Sede
    {

        [Required(ErrorMessage ="Seleccione un Usuario")]
        public string CodUsu { get; set; }

        [Required(ErrorMessage = "Seleccione un Sede")]
        public string CodSede { get; set; }

        public string NombreUsuario { get; set; }
        public string NombreSede { get; set; }


    }
}