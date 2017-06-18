using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Distrito
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodDist { get; set; }
        [Required(ErrorMessage = "Distrito requerido")]
        public string NomDist { get; set; }
        public bool EstDist { get; set; }
        [Required(ErrorMessage = "Provincia requerido")]
        public string CodProv { get; set; }
        [Required(ErrorMessage = "Departamento requerido")]
        public string CodDep { get; set; }
        public string CodPais { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        



    }
}
