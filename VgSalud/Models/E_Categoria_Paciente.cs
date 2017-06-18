
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
  public  class E_Categoria_Paciente
    {

        [Required(ErrorMessage ="Código requerido")]
        public string CodCatPac { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescCatPac { get; set; }
        [Required(ErrorMessage = "Sede requerido")]
        public string CodSede { get; set; }
        [Required(ErrorMessage = "Estado requerido")]
        public bool EstCatPac { get; set; }

        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }




    }
}
