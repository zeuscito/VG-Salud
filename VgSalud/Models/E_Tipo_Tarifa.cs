using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Tipo_Tarifa
    {
        [Required(ErrorMessage = "Código Requerido")]
        public string CodTipTar { get; set; }
        [Required(ErrorMessage = "Descripción Requerido")]
        public string DescTipTar { get; set; }
        [Required(ErrorMessage = "Especialidad Requerido")]
        public string CodEspec { get; set; }
        public bool EstTipTar { get; set; }
        public string Especialidad { get; set; }
        public string CodSede { get; set; }

        public bool AtencionRapida { get; set; }

        public int Modulo { get; set; }
        public string DescMod { get; set; }
        public string url { get; set; }
        public string urlImprime { get; set; }
        public string urlImprimeReceta { get; set; }

    }
}
