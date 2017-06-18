using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Servicio_TipoTarifa
    {


        [Required(ErrorMessage = "Seleccione un Tipo Tarifa")]
        public string CodtipoTar { get; set; } 

        [Required(ErrorMessage = "Descripcion Requerida")]
        public string DescTipTar { get; set; }

        [Required(ErrorMessage = "Seleccione un Servicio")]
        public string codserv { get; set; }
    
        public string nomser { get; set; }
    
        public decimal porcentaje { get; set; }


    }
}