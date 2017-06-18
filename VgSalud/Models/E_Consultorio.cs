using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace VgSalud.Models
{
    public class E_Consultorio
    {

        [Required(ErrorMessage = "Código requerido")]
        public string IdConsul { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
         public string DescConsul { get; set; }
        [Required(ErrorMessage = "Sede Requerido requerido")]
        public string CodSede { get; set; }
        public string CodServ { get; set; }
        public bool Mixto { get; set; }
        public bool EstConsul { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
      

        public string NomServ { get; set; }
    }
}