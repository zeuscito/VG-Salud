using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace VgSalud.Models
{
    public class E_Caja
    {

        public int CodCaja { get; set; }
        public string CodSede { get; set; }
        public int CodCue { get; set; }
        public string CodDocSerie { get; set; }
        public string Serie { get; set; }
        public string CodSerie { get; set; }
        public string NumDoc { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaEmision { get; set; }
        public TimeSpan HoraEmision { get; set; }
        public int Historia { get; set; }
        public string NomPac { get; set; }
        public string DirecPac { get; set; }
        public string Ruc { get; set; }
        public string NrRUC { get; set; }
        public string RazonSocial { get; set; }
        public string DirRazSoc { get; set; }
        public string RucA { get; set; }
        public string RazonSocialA { get; set; }
        public string DirRazSocA { get; set; }
        public string CodCatPac { get; set; }
        public bool Estado { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public string UsuCrea { get; set; }
        public string UsuAnula { get; set; }
        public string FechaAnula { get; set; }
        public string TipoPago { get; set; }
        public string CodRazSoc { get; set; }
        public string CodTipMon { get; set; }
        public decimal TipoCambio { get; set; }
        public string Obser { get; set; }
        public decimal TazaIgv { get; set; }
        public string AliasCodDoc { get; set; }
        public string est { get; set; }
        public string NomSede { get; set; }
        public string DescCodDoc { get; set; }
        public string DescCatPac { get; set; }
        public string DescTipMon { get; set; }
        public string AutorizaAnu { get; set; }
        public bool PorAnular { get; set; }
        public string PrecioLetra { get; set; }
        public DateTime FecNac { get; set; }
        public int Edad { get; set; }
        public string CodUsu { get; set; }
        public int evento { get; set; }


        public string fechaI { get; set; }
        public string fechaF { get; set; }

        public decimal[] array { get; set; }




        public int item { get; set; }
        public string CODMEDIOS { get; set; }
        public decimal Importe { get; set; }
        public decimal ImporteSoles { get; set; }

        public string NomMedios { get; set; }
        public string NomMoneda { get; set; }
        public string seriePago { get; set; }
        public string rucPago { get; set; }
        public string tipPago { get; set; }
        public string tipMoneda { get; set; }
        public decimal Monto { get; set; }
        public decimal montoCambio { get; set; }

        public int CodCajRes { get; set; }
        public DateTime FechaDeposito { get; set; }
        public DateTime FechaCaja { get; set; }
        public decimal TotalDolares { get; set; }
        public decimal TotalUsuario { get; set; }
        public decimal TotalSistema { get; set; }
        public decimal Diferencia { get; set; }
        public int corte { get; set; }
        public int anuladas { get; set; }
        public int nroTickets { get; set; }
        public TimeSpan horaFin { get; set; }
        public TimeSpan horaInicio { get; set; }
        public int id { get; set; }

        public string DirecSede { get; set; }


        public string Hora { get; set; }

    }
}