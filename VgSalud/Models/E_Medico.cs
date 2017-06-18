using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Medico
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodMed { get; set; }
        [Required(ErrorMessage = "Nombres requerido")]
        public string NomMed { get; set; }
        [Required(ErrorMessage = "Dni requerido")]
        public string DniMed { get; set; }
        [Required(ErrorMessage = "Tipo Medico requerido")]
        public string TipPrfMed { get; set; }
        public string ColgMed { get; set; }
        [Required(ErrorMessage = "Rne requerido")]
        public string RneMed { get; set; }
        [Required(ErrorMessage = "Direccion requerido")]
        public string DireccMed { get; set; }
        [Required(ErrorMessage = "Fecha requerido")]
        public DateTime FecIngMed { get; set; }
        [Required(ErrorMessage = "Teléfono requerido")]
        public string TelfMed { get; set; }
        public string ObservMed { get; set; }
        [Required(ErrorMessage = "Servicio requerido")]
        public string CodServ { get; set; }
        [Required(ErrorMessage = "Especialidad requerido")]
        public string CodEspec { get; set; }
        [Required(ErrorMessage = "Empresa requerido")]
        public string CodEmp { get; set; }
        public string CodUsu { get; set; }
        public string CodSede { get; set; }
        public decimal PagoTurno { get; set; }
        public bool EstMed { get; set; }

        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        public string abreviatura { get; set; }

        public string Servicio { get; set; }
        public string Especialidad { get; set; }
        public string empresa { get; set; }
        public string Sede { get; set; }


        public int evento { get; set; }
        public string codTipTar { get; set; }
        public string codTipTarElimina { get; set; }
        public string TipoTarifa { get; set; }
        public decimal porcentaje { get; set; }
        public bool EnLista { get; set; }
        public bool EjecFichaElec { get; set; }
        public string CodTar { get; set; }
        public decimal porcentaje1 { get; set; }
        public string DescTar { get; set; }

        public string CodTar1 { get; set; }
        public string CodTipTar1 { get; set; }

        public string Alias { get; set; }
        public bool CrearUsu { get; set; }


    }
}
