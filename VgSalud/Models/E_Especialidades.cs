using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
   public class E_Especialidades
    {

        [Required(ErrorMessage = "Código requerido")]
        public string CodEspec { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        public string NomEspec { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescEspec { get; set; }
        
        public bool EstEspec { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        public string CodTar { get; set; }
        public string CodSed { get; set; }
        public bool General { get; set; }


    }
}
