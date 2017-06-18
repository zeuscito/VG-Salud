using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Reporte
    {
        [DataType(DataType.Date)]
        public DateTime fechaEmision { get; set; }
        public int cantidad { get; set; }
        public string NomServ { get; set; }
        public string DescTar { get; set; }
        public decimal total { get; set; }
        public string NomEspec { get; set; }

        public string CodServ { get; set; }
        public string CodEspec { get; set; }
        public string CodMed { get; set; }



        public int PacAtendidos { get; set; }
        public int TotAtendido { get; set; }
        public int totalpac { get; set; }
        public string fecha { get; set; }





        public string NomMed { get; set; }
        public string tipoTar { get; set; }
        public string turno { get; set; }
        public string CodTar { get; set; }
        public string CodSede { get; set; }
        public string DescTipTar { get; set; }


        public string NombrePac { get; set; }
        public string Especialidad { get; set; }
        public string Servicio { get; set; }
        public string Medico { get; set; }
        public string Tarifa { get; set; }
        public string FechaAtenReg { get; set; }
        public int CodCue { get; set; }
        public int Item { get; set; }
        public string ItemCue { get; set; }
        public string FechaPago { get; set; }

    }
}