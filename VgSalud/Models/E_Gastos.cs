using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VgSalud.Controllers;
using VgSalud.Models; 
namespace VgSalud.Models
{
    public class E_Gastos : E_Proveedor
    {

        public int IdgastosC { get; set; }
        public string RazonS { get; set; }
     
        public string Direccion { get; set; }
        public decimal Total { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal igvCS { get; set; }
        public decimal IgvC { get; set; }
       
        public decimal Renta { get; set; }
        public decimal Detraccion { get; set; }
        public DateTime FechaEmision { get; set; }
        public string TipoVentaC { get; set; }
        public string Documento { get; set; }
        public string Serie { get; set; }
        public string TipoDoc { get; set; }
        public DateTime FechaProxPago { get; set; }
        public decimal TotalBruto { get; set; }
        public int ItemACta { get; set; }
        public  string DescripTip { get; set; }
        public decimal Monto { get; set; }
        public string glosa { get; set; }
        public int TipoCompra { get; set; }
        public string CodTipMon { get; set; }
        public decimal TipoCambio { get; set; }
        public string DescTipMon { get; set; }
        public decimal Retencion { get; set; }
        public string ObservAG { get; set; }
        public bool EstadoC { get; set; }
        public int ItemG { get; set; }
        public string FechaPago { get; set; }
        public string NroOperacion { get; set; }
        public string NroDetraccion { get; set; }
        public string FechaDetraccion { get; set; }
        public int IdCtaCont { get; set; }
        public int Item { get; set; }
        public decimal PrecioU { get; set; }
        public int Cant { get; set; }
        public decimal TotalProd { get; set; }
        public decimal igv { get; set; }
        public string TipoVentaD { get; set; }
        public bool EstadoD { get; set; }

        public string TipoPersonaC { get; set; }
        public DateTime FechaReg { get; set; }
        public string ObservacionC { get; set; }
        public string TipoPago { get; set; }
        public string TipoPagoC { get; set; }
        public bool EstadoG { get; set; }

        public string Evento { get; set; }

        public decimal TotalAcuenta { get; set; }
        //bienes Servicio
        public int IDbs { get; set; }
        public int Porcentaje { get; set; }
        public string Titulo { get; set; }

        //Cuenta Contable
        
     
        public int IDctaContable { get; set; }
        public string CodigoContable { get; set; }
        public string DescripcionCtaContable { get; set; }
       public bool EstadoCtaContable { get; set; }


     //centro de costo
        public string IdCentroCosto { get; set; }
        public string DescripcionCentroCosto { get; set; }
        public bool EstadoCentroCosto { get; set; }

        //        create table Sub_Centro_Costos(
     

        public string IdSubCentroCosto { get; set; }
        public string DescripcionSubCentroCosto { get; set; }
        public bool EstadoSubCentroCosto { get; set; }

        // Tipo Documento 
   
        public int CodigoTipoDocumento { get; set; }
        public  string DescripcionTipoDocumento { get; set; }
        public bool EstadoTipoDocumento { get; set; }


        public HttpPostedFileBase foto { get; set; }
        public string urlimagen { get; set; }
       
    }
}