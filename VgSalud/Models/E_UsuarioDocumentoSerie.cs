using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_UsuarioDocumentoSerie
    {
        [Required(ErrorMessage = "Seleccione el usuario Dato Requerido")]
        public string CodUsu { get; set; }
        [Required(ErrorMessage = "Seleccione El documento Dato Requerido")]
        public string CodDocSerie { get; set; }
        public bool Prioridad { get; set; }
        public bool EstUDs { get; set; }
        public string AliasUsu { get; set; }
        public string Serie { get; set; }

    }
}