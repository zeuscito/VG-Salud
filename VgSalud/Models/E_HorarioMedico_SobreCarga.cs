using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_HorarioMedico_SobreCarga
    {
        public string CodHor { get; set; }

        [DataType(DataType.Date)]
        public DateTime dia { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public string horaInicioB { get; set; }
        public string horaFinB { get; set; }
        public int IntMin { get; set; }
        public string Turno { get; set; }
        public string CodMed { get; set; }
        public string fecha { get; set; }

        public string MesConsulta { get; set; }
        public string AnioConsulta { get; set; }
        public int cantDias { get; set; }
        public string Consultorio { get; set; }
        public bool Estado { get; set; }
        public string Asistencia { get; set; }

        public int mes { get; set; }
        public int anio { get; set; }
        public string cadena { get; set; }


        
        public string NomMed { get; set; }
        public string NomServ { get; set; }
        public string CodServ { get; set; }
        

    }
}