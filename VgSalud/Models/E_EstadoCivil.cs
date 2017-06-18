using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_EstadoCivil
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodEstCivil { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string NomEstCivil { get; set; }
        public bool Estado { get; set; }
    }
}
