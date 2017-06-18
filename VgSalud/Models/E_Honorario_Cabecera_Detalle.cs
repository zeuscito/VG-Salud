using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Honorario_Cabecera_Detalle
    {
        
        //PARAMETROS DE CABECERA
        public string IdC { get; set; }
        public string CodServ { get; set; }

        public string CodMed { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public bool Estado { get; set; }
        public string TipoDoc { get; set; }
        public string TipoPago { get; set; }
        public Decimal Total { get; set; }
        public DateTime FechaAnulado { get; set; }
        public string UsuarioAnulado { get; set; }
        public string CodSede { get; set; }
        public int TipoLiquidacion { get; set; }
        public string TipoRango { get; set; }
        public decimal PagoTurno { get; set; }
        public int CantTurno { get; set; }
        public decimal PagoTotalTurno { get; set; }
        public int FormaLiq { get; set; }
        public DateTime fechaCrea { get; set; }

        
        public string idD { get; set; }
        public string CodSedeD { get; set; }
        public int Historia { get; set; }
        public int  CodCuenta { get; set; }
        public int ItemCita { get; set; }
        public string TipoDocD { get; set; }
        public string SerieDocumento { get; set; }

        public decimal Tot_Tarifa { get; set; }
        public decimal PorcentajeTarifa { get; set; }
        public decimal APagar { get; set; }
        public int SecFactCaja { get; set; }
        public string EstadoD { get; set; }
        public string FechaEmision { get; set; }
        public string FechaAtencion { get; set; }

        public string NumDoc { get; set; }
        public string Tarifa { get; set; }
        public int Cantidad { get; set; }
        public string Turno { get; set; }

        public string[] arreglo { get; set; }

       public string CodDocSerie { get; set; }

        public decimal Precio { get; set; } 

        public decimal CTprecio { get; set; }
        public string nombre { get; set; }
        public int NroDocumentos { get; set; }
        public int NroAtenciones { get; set; }
        public int comoPagar { get; set; }


        public DateTime FechaGroup { get; set; }
        public decimal PagaTurno { get; set; }

        public int idTemporal { get; set; }

        public decimal TotalFac { get; set; }
        public int primario { get; set; }

        public int TotalTurnos { get; set; }
        public string fechaTur { get; set; }

    }
}