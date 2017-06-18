using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
 public   class E_Documento_Identidad
    {

        [Required(ErrorMessage = "Código requerido")]
        public string CodDocIdent { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        public string NomDocIdent { get; set; }
        public bool Estado { get; set; }
     

   
    }
}
