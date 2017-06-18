using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Perfil
    {


       

        public string codperf { get; set; }
        [Required(ErrorMessage = "Nombre de la descripcion del perfil")]
        public string descPerf { get; set; }
        public bool EstperF { get; set; }

       


    }
}