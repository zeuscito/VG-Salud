using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Departamento
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodDep { get; set; }
        [Required(ErrorMessage = "Departamento requerido")]
        public string NomDep { get; set; }
       
        public bool EstDep { get; set; }
        [Required(ErrorMessage = "Pais requerido")]
        public string CodPais { get; set; }
        public string crea { get; set; }
        public string modifica { get; set; }
        public string elimina { get; set; }
      //  public int evento { get; set; }
    }
}
