using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Sexo
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodSexo { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string NomSexo { get; set; }
        public bool Estado { get; set; }
    }
}
