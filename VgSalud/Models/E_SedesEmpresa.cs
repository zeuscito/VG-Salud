using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_SedesEmpresa
    {

        [Required(ErrorMessage ="Sede Requerido")]
        public string CodSede { get;set;}

        [Required(ErrorMessage = "Empresa Requerido")]
        public string CodEmp { get;set;}

        public string Sede { get; set; }
        public string Empresa { get; set; }

    }
}