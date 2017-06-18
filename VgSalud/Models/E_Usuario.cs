using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 
namespace VgSalud.Models
{
    public class E_Usuario
    {

        
        public string codUsu { get; set; }
        [Required(ErrorMessage = "Nombre del alias requerido")]
        public string aliasUsu { get; set; }

        [Required(ErrorMessage = "Password requerido")]
        public string pwdUsu { get; set; }

        [Required(ErrorMessage = "Dni requerido")]
        [StringLength(8)] 
        
        public string  DniUsu { get; set; }
        [Required(ErrorMessage = "Apellido Paterno requerido")]
        public string ApePaterUsu { get; set; }
        [Required(ErrorMessage = "Apellido Materno requerido")]
        public string ApeMaterUsu { get; set; }
        [Required(ErrorMessage = "Nombre Usuario requerido")]
        public string NomUsu { get; set; }
        [Required(ErrorMessage = "Fecha de Nacimiento requerido")]
        public DateTime FecNacUsu { get; set; }
        [Required(ErrorMessage = "Estado Usuario requerido")]
        public bool EstUsu { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
        public string Concatena { get; set; }

        [Required(ErrorMessage = "Contraseña antigua Requerida")]
        public string passwordLast { get; set; }
        [Required(ErrorMessage = "Contraseña Nueva Requerida")]
        public string passwordNew { get; set; }
        [Required(ErrorMessage = "Repetir contraseña Nueva Requerida")]
        public string passwordRepit { get; set; }

    }
}