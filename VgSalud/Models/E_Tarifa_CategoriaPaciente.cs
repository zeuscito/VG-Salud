using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
  public  class E_Tarifa_CategoriaPaciente
    {

        [Required(ErrorMessage = "Código requerido")]
        public int CodTarCate { get; set; }
       
        public string CodCatPac { get; set; }
        [Required(ErrorMessage = "Tarifa requerido")]
        public string CodTar { get; set; }
        [Required(ErrorMessage = "Precio requerido")]
        public decimal Precio { get; set; }
        public string TipMoneda { get; set; }
        public string CodSede { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        public string DescCatPac { get; set; }
        public string demo { get; set; }
        public string costo { get; set; }

        public string espe { get; set; }
        public string tipoTar { get; set; }
        public string tipoSubTar { get; set; }
        public int IdCtaCont { get; set; }
        public int idPFA { get; set; }

    }
}
