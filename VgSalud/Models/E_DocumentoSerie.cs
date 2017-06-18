using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
   public class E_DocumentoSerie
    {
        public string CodDocSerie { get; set; }

        [Required(ErrorMessage = "Seleccione Documento Contable")]
        public int CodDocCont { get; set; }
        [Required(ErrorMessage = "Ingrese Serie")]
        public string Serie { get; set; }
        [Required(ErrorMessage = "Ingrese Número")]
        public string NumDoc { get; set; }
        [Required(ErrorMessage = "Seleccione Sede")]
        public string CodSede { get; set; }
        public bool EstDocSerie { get; set; }

        public string Crea { get;set;}
        public string Modifica { get;set;}
        public string Elimina { get;set;}
        //Agregando
        public string DescCodDoc { get; set; }
        public string SerieDocumento { get; set; }

        

    }
}
