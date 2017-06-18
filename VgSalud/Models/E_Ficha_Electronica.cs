using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VgSalud.Models
{
    public class E_Ficha_Electronica
    {


        public int FE { get; set; }
        public int Historia { get; set; }
        public string FC { get; set; }
        public string PA { get; set; }
        public string FR { get; set; }
        public decimal Tax { get; set; }
        public decimal Tanal { get; set; }
        public decimal Peso { get; set; }
        public decimal talla  { get; set; }
        public decimal IMC { get; set; }
        public string FUR { get; set; }
        public string MotivoConsulta { get; set; }
        public string Relato { get; set; }
        public string TiempoEnfermedad { get; set; }
        public string Inicio { get; set; }
        public string Curso { get; set; }
        public string sed { get; set; }
        public string sueño { get; set; }
        public string Orina { get; set; }
        public string apetito { get; set; }
        public int paridad { get; set; }
        public string FPP { get; set; }
        public int EdadGestacional { get; set; }
        public string PIG { get; set; }
        public string UltimoPap { get; set; } 
        public string ResultPap { get; set; }
        public string MACtipTime { get; set; }
        public string  Otrosginec { get; set; } 
        public string ProxCita { get; set; }
        public string ExaGeneral { get; set; }

        public string UbicEspTampa { get; set; }
        public string EstNutricion { get; set; }
        public string EstHidratacion { get; set; }
        public string PielFanerasTejido { get; set; }
        public string Mamas { get; set; }
        public string SisRespiratorio { get; set; }
        public string SisCardiovascular { get; set; } 
        public string CabezaCuello { get; set; }
        public string SisOsteoMuscular { get; set; }
        public string AbdomenPelvis { get; set; }
        public string ExaObstetrico { get; set; }
        public string SisGenitourinario { get; set; }
        public string SisNervioso { get; set; }
        public bool Estado { get; set; }
        public string Asistente { get; set; }
        public string FecRegAsist { get; set; }
        public string Medico { get; set; }
        public string Servicio { get; set; }
        public string FecRegMed { get; set; }

        public string InterConsulta { get; set; }
        public string Tarifa { get; set; }
        public string CodTipTar { get; set; }
        public string DescTipTar { get; set; }
        public int Item { get; set; }


        // Listado de atenciones
        public int CodCue { get; set; }
        public string RegMedico { get; set; }
        public string EstDet { get; set; }
        public string CodTar { get; set; }
        public string CodEspec { get; set; }
        public string DescTar { get; set; }
        // public int Historia { get; set; }
        public string Paciente { get; set; }
        // public int Item { get; set; }
        public decimal total { get; set; }
        public int Cantidad { get; set; }
        public int Procedencia { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaAtencion { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public string CodServ { get; set; }
        public string NomMed { get; set; }
        public string FechaAtenReg { get; set; } 
        public string EstadoCuenta { get; set; }
        public int? FEL { get; set; }



        //Antecedentes
        public int CodAnt { get; set; }
        public bool CancerP { get; set; }
        public bool DiabetesP { get; set; }
        public bool ACVP { get; set; }
        public bool AlergiaP { get; set; }
        public bool HipertArtP { get; set; }
        public bool OtrosP { get; set; }
        public bool CancerF { get; set; }
        public bool DiabetesF { get; set; }
        public bool ACVF { get; set; }
        public bool AlergiaF { get; set; }
        public bool HipertArtF { get; set; }
        public bool OtrosF { get; set; }
        public string ObservacionP { get; set; }
        public string ObservacionF { get; set; }
        public bool Usuario { get; set; }


        // Diagnostico
        public string CIe10 { get; set; }
        public string Descripcion { get; set; }
        public int ProcedenciaDiagnostico { get; set; }
        public bool EstadoCie10 { get; set; }
        public string ConcatCie10 { get; set; }
        public string EliminaDiagnostico { get; set; }


        // Examen auxiliar

        public int idSesionExamen { get; set; }
        public int EA { get; set; }
        public string Especialidad { get; set; }
        public string DescrEspecialidad { get; set; }
        public string TarifaExamen { get; set; }
        public string DescrTarifaExamen { get; set; }
        public int cant { get; set; }
        public string Otros { get; set; }
        public string Examen_Proced { get; set; }
        public int EliminaExamen { get; set; }


        //Receta
        public int idRecetaSesion { get; set; }
        public int CodFarmaco { get; set; }
        public string Concentra { get; set; }
        public int FormaFarmec { get; set; }
        public string Dosis { get; set; }
        public int Frecuencia { get; set; }
        public string ViaAdmin { get; set; }
        public string Duracion { get; set; }
        public string CodMed { get; set; }
        public string Obsv { get; set; }



        public int evento { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNac { get; set; }
        public string NomServ { get; set; }


        public int idPFA { get; set; }
        public string Nombre { get; set; }
        public string contenido { get; set; }
        public int EliminaPerfil { get; set; }
        public int EditaPerfil { get; set; }


        public int tipoFichaElectronica { get; set; }
        public int idProc { get; set; }
        public string Observ { get; set; }
        public string ProximaProc { get; set; }
        public string DescripcionProc { get; set; }

        public string FecRegMedProc { get; set; }
        public string Turno { get; set; }
        public string NombrePac { get; set; }
        public int edad { get; set; }
        public string numDoc { get; set; }
        public string sexo { get; set; }
        //public string descTar { get; set; }
        public string DesFromFar { get; set; }
        public string DesFre { get; set; }
        public string Abreviatura { get; set; }

        public string tipoDiagnostico { get; set; }

        public string ItemCue { get; set; }

        public int idExAux { get; set; }

        public int Modulo { get; set; }
        public string url { get; set; }
        public string urlImprime { get; set; }
        public string urlImprimeReceta { get; set; }

    }
}