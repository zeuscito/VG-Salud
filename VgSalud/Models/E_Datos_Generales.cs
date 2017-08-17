using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Datos_Generales
    {
        //actualizado 100
        public decimal igv { get; set; }
        public decimal Tipo_Cambio { get; set; }
        public bool MUESTRA_ANTECEDENTE { get; set; }
        public bool ATENCIONESPAGADAS { get; set; }
        public bool PREGXATENCIONPROGRAMADAS { get; set; }
        public bool PREGXATENCIONNOPROGRAMADAS { get; set; }
        public bool GENERARCUENTAAUTO  { get; set; } 
        public bool MOSTRARPACIENTETICKET { get; set; }
        public string CodDist { get; set; }
        public string NomDist { get; set; }
        public string CODSEDE { get; set; }
        public string MODULOS { get; set; } 
        public string servicio { get; set; }
        public string sedes { get; set; }
        public string fecha { get; set; }
        public decimal montoCierre { get; set; }
    }
}