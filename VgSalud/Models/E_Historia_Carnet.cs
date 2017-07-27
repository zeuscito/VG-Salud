using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Historia_Carnet
    {
        public string Carnet { get; set; }
        public string ApePat { get; set; }
        public string ApeMat { get; set; }
        public string NomPac { get; set; }
        public string EstadoCarnet { get; set; }
        [DataType(DataType.Date)]
        public DateTime FecRecojo { get; set; }
        [DataType(DataType.Date)]
        public DateTime FecVencimiento { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }
        public string NumDoc { get; set; }
    }
}