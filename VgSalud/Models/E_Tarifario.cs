using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Tarifario
    {
        [Required(ErrorMessage = "Código requerido")]
        public string CodTar { get; set; }
        [Required(ErrorMessage = "Descripción requerido")]
        public string DescTar { get; set; }
        [Required(ErrorMessage = "Precio requerido")]
        public decimal Precio { get; set; }
        public bool AfecIgcv { get; set; }
        [Required(ErrorMessage = "Código requerido")]
        public bool ModPrecTar { get; set; }
        [Required(ErrorMessage = "Especialidad requerido")]
        public string CodEspec { get; set; }

        [Required(ErrorMessage = "Tipo Tarifa requerido")]
        public string CodTipTar { get; set; }
        //[Required(ErrorMessage = "STipo Tarifa requerido")]
        public string CodSTipTar { get; set; }
        [Required(ErrorMessage = "Moneda requerido")]
        public string CodTipMon { get; set; }
        public string CodSede { get; set; }
        public bool EstTar { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        public string CodTarE { get; set; }
        public bool ModPrecio { get; set; }

        [Required(ErrorMessage = "Cuenta Contable requerido")]
        public int IdCtaCont { get; set; }

        [Required(ErrorMessage = "Perfil Ficha Electrónica requerido")]
        public int idPFA { get; set; }


        //Cuenta Contable
        public string Evento { get; set; }
        public string codigo { get; set; }
        public string Descrip { get; set; }
        public bool Estado { get; set; }
        public string ConcatenadoCuenta { get; set; }

        //Perfil de FE
        public string Nombre { get; set; }
        public string contenido { get; set; }

        public string CodCatPac { get; set; }

        public decimal PrecioD { get; set; }
        public int TiempoApox { get; set; }
        public decimal igv { get; set; }
        public decimal total { get; set; }
        public int procedencia { get; set; }
    }
}
