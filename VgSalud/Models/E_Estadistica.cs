using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Estadistica
    {

        //Facturacion Dia  

        public decimal Total { get; set; }
        public int dia { get; set; }

        //Facturacion Por pacientes Atendido 
        public int TotPaciente { get; set; }

        public string NomTipTar { get; set; }

        public string CodTipTar { get; set; }
        public string color1 { get; set; }
        public int color2 { get; set; }
        public int color3 { get; set; }


        //5 
        public string nomSexo { get; set; }
        public string Turno { get; set; }

        public string NameDia { get; set; }

    }
}