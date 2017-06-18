using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_RolFormulario
    {

        [Required(ErrorMessage = "Seleccione un Perfil")]
        public string CodPerf { get; set; }
        public int? IdForm { get; set; }
        public string AliasForm { get; set; }
        public string DescPerf { get; set; }
        public int idCat { get; set; }
        public int idModulo { get; set; }
        public int Estado { get; set; }
        public string[] array { get; set; }

    }
}