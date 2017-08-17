using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace VgSalud.Models
{
  public  class E_Pacientes
    {
        [Required(ErrorMessage = "Número de Historia requerido")]
        public int Historia { get;set;}
        [Required(ErrorMessage = "Apellido Paterno requerido")]
        public string ApePat  { get; set; }

        public string ApeMat  { get; set; }
        [Required(ErrorMessage = "Nombres requerido")]
        public string NomPac   { get; set; }
        [Required(ErrorMessage = "Fecha Nacimiento requerido")]
        [DataType (DataType.Date)]
        public DateTime  FecNac     { get; set; }

        public string    Sector     { get; set; }
        [Required(ErrorMessage = "Número de Documento requerido")]
        public string    NumDoc     { get; set; }
       
        public string    Ruc        { get; set; }
       
        public string    Essalud    { get; set; }
       
        public string    CodAseg    { get; set; }
       
        public DateTime  FecAfil    { get; set; }

        public string LugarNac { get; set; }
        //[Required(ErrorMessage = "Dirección requerido")]
        public string    Direcc     { get; set; }
       
        public string    Email      { get; set; }
        //[Required(ErrorMessage = "Teléfono requerido")]
        public string    TelfFijo   { get; set; }
        
        public string    TelfCel    { get; set; }
       
        public int?       CodTipPac  { get; set; }
     
        public string    Observ         { get; set; }
       
        public bool       Discap         { get; set; }

        public string DiscaObs { get; set; }

        public string    TipSang    { get; set; }
        
        public string    Alerg      { get; set; }
     
        public string    TitParent  { get; set; }
      
        public string    TitNom     { get; set; }
        
        public string    TitDni     { get; set; }
       
        public string    TitObs     { get; set; }
        public string    Ocup       { get; set; }
        public string    DirTrab    { get; set; }
        public string    TelTrab    { get; set; } 
        [Required(ErrorMessage ="Categoria Paciente Requerido")]
        public string    CodCatPac  { get; set; }
        public string    CodEstCivil { get; set; }
        [Required(ErrorMessage = "Sexo requerido")]
        public string    CodSexo    { get; set; }
        [Required(ErrorMessage = "Tipo Filiación requerido")]
        public string    CodTipFil  { get; set; }
        [Required(ErrorMessage = "Documento requerido")]
        public string    CodDocIdent { get; set; }
        [Required(ErrorMessage = "Distrito requerido")]
        public string    CodDist    { get; set; }
        [Required(ErrorMessage = "Provincia requerido")]
        public string    CodProv    { get; set; }
        [Required(ErrorMessage = "Departamento requerido")]
        public string    CodDep     { get; set; }
        public string    CodPais    { get; set; }
        public string CodUsu { get; set; }
        public bool    EstPac     { get; set; }
        public string    Crea       { get; set; }
        public string    Modifica   { get; set; }
        public string    Elimina    { get; set; }
        public string nombCompleto { get; set; }

  
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

        public string Edad { get; set; }
        public string NonSex { get; set; }
        public string NomEstCivil { get; set; }
        public string NomDist { get; set; }
        public string NumerosTelefonicos { get; set; }

        public string NomEspec { get; set; }
    }
}
