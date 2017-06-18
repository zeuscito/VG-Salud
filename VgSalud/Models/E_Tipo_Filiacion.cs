using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
   public class E_Tipo_Filiacion
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodTipFil { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescTipFil { get; set; }
        public bool Estado { get; set; }



    }
}
