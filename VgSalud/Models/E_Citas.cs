using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Citas
    {
        public int numero { get; set; }
        public int CodCita { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public string NombrePaciente { get; set; }
        public string NombreCatePaciente { get; set; }

        public string CodEspec { get; set; }
        public string NomEspec { get; set; }
        public string CodServ { get; set; }
        public string NomServ { get; set; }
        public string CodTar { get; set; }
        public string DescTar { get; set; }
        public string CodMed { get; set; }
        public string NomMed { get; set; }
        public string fechaCita { get; set; }
        public string Turno { get; set; }

        public TimeSpan horaI { get; set; }
        public TimeSpan horaF { get; set; }
        public int intMin { get; set; }

        public int Historia { get; set; }
        public string TipoPac { get; set; }
        public string Obser { get; set; }
        public string CodCatPac { get; set; }
        public decimal precio { get; set; }
        public decimal igv { get; set; }
        public decimal total { get; set; }
        public string CodTipTar { get; set; }
        public DateTime FechaRegistro { get; set; }
        [Required(ErrorMessage = "Fecha Nacimiento requerido")]
        [DataType(DataType.Date)]
        public DateTime fechaCitas { get; set; }
        public int CodCue { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string TipoRegistro { get; set; }

        public string CodSede { get; set; }
        public string MedInter { get; set; }
        
        public string TipoConsu { get; set; }
        public string DescTipoTar { get; set; }
        public string NomTar { get; set; }

        public string Consultorio { get; set; }
        public string cadena { get; set; }

        public int dimension { get; set; }

        public int intervalo { get; set; }

        public int evento { get; set; }

    }
}