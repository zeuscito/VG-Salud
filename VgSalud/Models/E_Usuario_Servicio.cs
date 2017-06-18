using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Usuario_Servicio
    {

        [Required(ErrorMessage = "Seleccione un Usuario")]
        public string CodUsu { get; set; }
        [Required(ErrorMessage = "Seleccione un Servicio")]
        public string CodServ { get; set; }  

        public string NombreUSuario { get; set; }
        public string NomServ { get; set; }

        public string CodEsp { get; set; }
        public bool General { get; set; }

    }
}