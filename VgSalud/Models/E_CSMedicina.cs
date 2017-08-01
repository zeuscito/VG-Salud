using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_CSMedicina
    {
        public int Id { get; set; }
        public int CodCue { get; set; }
        public string Paciente { get; set; }
        public string DesTipoCarnet { get; set; }
        public int idEstado { get; set; }
        public int Edad { get; set; }
        public int IdProcedencia { get; set; }
        public string NumeroCarnet { get; set; }
        public string Manipulador { get; set; }
        public string Procedencia { get; set; }
        public string Conclusion { get; set; }
        public string AptoMed { get; set; }
        public string Reevaluado { get; set; }
        public string Observaciones { get; set; }
        public int Prioridad { get; set; }
        public DateTime FechaRegMed { get; set; }
        public DateTime FechaAtenMed { get; set; }
        public int Estado { get; set; }
        public string CodSede { get; set; }
        public string Crea { get; set; }
        public string Modifica { get; set; }
        public string Elimina { get; set; }

        public string AptoLab { get; set; }
        public string AptoOdon { get; set; }
        public int NroCarnet { get; set; }



        public bool MuestraSangre { get; set; }
        public bool MuestraHeces { get; set; }

        public string TituloMuestraSangre { get; set; }
        public string TituloMuestraHeces { get; set; }


        public string Lab_MuestraSangre { get; set; }
        public string Lab_MuestraHeces { get; set; }


        public string TituloRPR { get; set; }
        public string TituloParasitologico { get; set; }
        public string TituloObservacion { get; set; }

        public string Lab_RPR { get; set; }
        public string Lab_Parasitologico { get; set; }
        public string Lab_Observacion { get; set; }



        public string OdoObservacion { get; set; }


        public int IdOdontologia { get; set; }


        public DateTime FechaRegistroOdontograma { get; set; }
        public int CodOdontograma { get; set; }


        public string ArregloOdontograma { get; set; }


    }
}