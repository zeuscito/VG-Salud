using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
   public class E_Sub_Tipo_Tarifa
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodSTipTar { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescSTipTar { get; set; }
        [Required(ErrorMessage = "Tipo tarifa requerido")]
        public string CodTipTar { get; set; }
      
        public bool EstTipTar { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }


        public string TipoTarifa { get; set; }


    }
}
