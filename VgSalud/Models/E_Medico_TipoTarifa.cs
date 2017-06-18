using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Medico_TipoTarifa
    {
      
        public string CodTipTar { get; set; }
        public string CodMed { get; set; }
        public decimal porcentaje { get; set; }

        public string nomTip { get; set; }
        public string NomMed { get; set; }
    }
}