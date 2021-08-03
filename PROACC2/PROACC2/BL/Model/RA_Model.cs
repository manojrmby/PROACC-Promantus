using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class RA_Model
    {
        public int Id { get; set; }
        public string Probability { get; set; }
        public string RiskClass { get; set; }
        public string RiskOwner { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public Nullable<System.Guid> Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool isDeleted { get; set; }
       
    }
}