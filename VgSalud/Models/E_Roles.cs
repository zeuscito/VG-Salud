using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 
namespace VgSalud.Models
{
    public class E_Roles
    {
      
        public string CodRoles { get; set; } 
        [Required(ErrorMessage = "Descripcion Requerida")]
        public string  DescRoles { get; set; } 
        [Required(ErrorMessage ="Observacion Requerida")]
        public string ObsRoles { get; set; }

        public bool EstRoles { get; set; }

     

    }
}