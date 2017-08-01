using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CSOdontologia
    {

        public int Id { get; set; }
        public int CodCue { get; set; }
        public string Paciente { get; set; }
        public string DesTipoCarnet { get; set; }
        public string Manipulador { get; set; }
        
        public int Prioridad { get; set; }
        public int idEstado { get; set; }
        public int Edad { get; set; }
        public string evento { get; set; }

        public string Observacion { get; set; }
        public string Apto { get; set; }


        //public int edad { get; set; }
        public string NumeroCarnet { get; set; }
        public string Procedencia { get; set; }
        

        //public int IdOdontologia { get; set; }

        public string ArregloOdontograma { get; set; }
        public string NuevoArregloOdontograma { get; set; }
        


        public string DetalleArreglo { get; set; }

        public int CodOdontograma { get; set; }

        


    }
}