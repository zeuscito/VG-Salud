using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_TipoMoneda
    {

        [Required(ErrorMessage = "Código requerido")]
        public string CodTipMon { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescTipMon { get; set; }
        public bool EstTipMon { get; set; }
        [Required(ErrorMessage = "Tipo de cambio requerido")]
        public decimal TipoCambio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? fecha { get; set; }
        public string fechaParse { get; set; }
        public int IdTipoCambio { get; set; }
        public  string Evento { get; set; }

    }
}