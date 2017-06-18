using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Medios_Pago
    {
        public string CODMEDIOS { get; set; }
        [Required( ErrorMessage = "Descripcion Requerido" )]
        public string DESCRIPCION { get; set; }
        public bool ESTADO { get; set; }

    }
}