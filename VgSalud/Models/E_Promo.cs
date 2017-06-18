using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Promo
    {
        //        @IdPC int  , 
        //@Descripcion varchar(100) , 
        //@CodEspec char(5) , 
        //@CodServ char(5) , 
        //@CodSede char(5)  , 
        //@Subtotal decimal (10,2) , 
        //@Igv decimal(7,2) , 
        //@Total decimal(10,2)  

        //        @IdPC int  , 
        //@item int , 
        //@CodTar char(6) , 
        //@CodTipTar char(5)  , 
        //@SubTotal decimal(8,2) , 
        //@Igv decimal(7,2) , 
        //@Total decimal(10,2) 

        public int IdPC { get; set; }
        public string Descripcion { get; set; }
        public string CodEspec { get; set; }
        public string CodServ { get; set; }
        public string CodSede { get; set; }
        public decimal SubtotalC { get; set; }
        public decimal IgvC { get; set; }
        public decimal TotalC { get; set; }
        public int item { get; set; }
        public string CodTar { get; set; }
        public string DescTar { get; set; }
        public string CodTipTar { get; set; }
        public decimal SubtotalD { get; set; }
        public decimal IgvD { get; set; }
        public decimal TotalD { get; set; }
        public int cantidad { get; set; }
        public string Evento { get; set; }
        public decimal importe { get; set; }
        public string[] igv { get; set; }
        public bool afectaigv { get; set; }
        public string CodTar1 { get; set; }
        public bool estado { get; set; }
        public decimal Precio { get; set; }
        public string DescEsp { get; set; }

    }
}