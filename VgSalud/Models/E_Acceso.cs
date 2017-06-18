using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Acceso
    {

      
        public int CodAcc { get; set; }
        public string CodPerf { get; set; }
        public string CodUsu { get; set; }
        [Required(ErrorMessage = "Estado de Acceso requerido")]
        public string EstAcc { get; set; }
        public string NombrePerfil { get; set; } 
        public string NombreUsuario { get; set; } 
        public bool Estado { get; set; }



        public int idModulo { get; set; }
        public string NombreModulo { get; set; }
        public string icono { get; set; }


        public int idCat { get; set; }
        public string NomCat { get; set; }


        public int idForm { get; set; }
        public string nomForm { get; set; }
        public string AliasForm { get; set; }

    }
}