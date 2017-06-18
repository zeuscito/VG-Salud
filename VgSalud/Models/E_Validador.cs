using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Validador
    {
        public string Medico { get; set; }
        public string Fecha { get; set; }
        public DateTime FechaAten { get; set; }
        public string turno { get; set; }
        public string servicio { get; set; }
        public int Item { get; set; }
        public string codtar { get; set; }
        public string DescTar { get; set; }
        public int cantidad { get; set; }
        public decimal precioUni { get; set; }
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Nro Cuenta Requerida")]
        public int cuenta { get; set; }

        public string CodUsu { get; set; }
        public string CodServ { get; set; }
        public string CodSede { get; set; }
        public bool General { get; set; }
        public string Evento { get; set; }

        public string Registrado { get; set; }
        public string check { get; set; }
        public string EstadoLiq { get; set; }
        public  string Paciente { get; set; }

    }
}