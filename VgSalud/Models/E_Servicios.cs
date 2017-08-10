using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
 public   class E_Servicios
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodServ { get; set; }
        [Required(ErrorMessage = "Servicio requerido")]
        public string NomServ { get; set; }
        [Required(ErrorMessage = "Especialidad requerido")]
        public string CodEspec { get; set; }
        [Required(ErrorMessage = "Empresa requerido")]
        public string CodEmp { get; set; }
        [Required(ErrorMessage = "Sede Requerido requerido")]
        public string CodSede { get; set; }
        public bool EstServ { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }
        public string CodTar { get; set; }
        public decimal precio { get; set; }
        public string Empresa { get; set; }
        public string Especialidad { get; set; }
        public string Sede { get; set; }

        public string CodTipTar { get; set; }
        public string DescTipoTar { get; set; }
        public decimal porcentaje { get; set; }
        public string Evento { get; set; }


        public string CodigoServicios { get; set; }
        public string NombreServicios { get; set; }
    }
}
