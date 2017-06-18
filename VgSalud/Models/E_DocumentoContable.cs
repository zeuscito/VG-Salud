using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
   public class E_DocumentoContable
    {

        public int CodDocCont { get; set; }

        [Required(ErrorMessage = "Ingrese Descripcion de Documento")]
        public string DescCodDoc { get; set; }

        [Required(ErrorMessage = "Ingrese Alias del Documento")]
        public string AliasCodDoc { get; set; }
        public bool EstCodDoc { get; set; }
        public bool IncluRegVen { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
        


    }
}
