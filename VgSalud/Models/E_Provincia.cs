using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Provincia
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodProv { get; set; }
        [Required(ErrorMessage = "Nombre requerido")]
        public string NomProv { get; set; }
       
        public bool EstProv { get; set; }
        [Required(ErrorMessage = "Departamento requerido")]
        public string CodDep { get; set; }
        [Required(ErrorMessage = "Pais requerido")]
       // public string codPAis { get; set; }
        public string crea { get; set; }
        public string modifica { get; set; }
        public string elimina { get; set; }
       // public int evento { get; set; }
    }
}
