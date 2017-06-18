using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Cuentas
    {

        public int CodCue { get; set; }
        public string CodSede { get; set; }
        public int Historia { get; set; }
        public string CodcatPac { get; set; }
        public decimal STotCue { get; set; }
        public decimal IgvCue { get; set; }
        public decimal TotCue { get; set; }
        [DataType(DataType.Date)]
        public DateTime FecCrea { get; set; }
        public string FecAnul { get; set; }
        public bool EstCue { get; set; }
        public string EstGene { get; set; }

        public string SecFact { get; set; }
        public string Usuario { get; set; }
        public string UsuarioAnula { get; set; }


        public string NomPac { get; set; }
        public string ApePat { get; set; }
        public string ApeMat { get; set; }
        public string nomCompleto { get; set; }


        //Consentimiento informado
        public int TipoCons { get; set; }
        public string desConsentimiento { get; set; }
        public bool estadoConsentimiento { get; set; }
        public string[] consInf { get; set; }
        public string textoConsentimiento { get; set; }



    }
}