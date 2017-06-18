using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Pais
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodPais { get; set; }
        [Required(ErrorMessage = "Pais requerido")]
        public string NomPais { get; set; }
        [Required(ErrorMessage = "Abreviado de Pais requerido")]
        public string AbrvPais { get; set; }
       
        public bool EstPais { get; set; }
    }
}
