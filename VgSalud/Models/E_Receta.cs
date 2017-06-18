using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Receta
    {


        public int CodFarmaco { get; set; }
        public string Descripcion { get; set; }
        public int FE { get; set; }

        public string Concentra { get; set; }
        public string FormaFarmec { get; set; }
        public string Dosis { get; set; }
        public string Frecuencia { get; set; }
        public string ViaAdmin { get; set; }
        public string Duracion { get; set; }
        public int CodCue { get; set; }
   
        public int Cantidad { get; set; }
        public bool Estado { get; set; }
        public string CodMed { get; set; }
        public string Servicio { get; set; }
        public int Item { get; set; }

    }
}