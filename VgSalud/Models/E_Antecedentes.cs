using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VgSalud.Models
{
    public class E_Antecedentes
    {
        public int CodAnt { get; set; }
        public int Historia { get; set; }
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
        public bool ObservacionP { get; set; }
        public bool ObservacionF { get; set; }
        public bool Usuario { get; set; }
    }
}