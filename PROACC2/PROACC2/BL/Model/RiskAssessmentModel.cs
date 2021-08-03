using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class RiskAssessmentModel
    {
        public System.Guid Id { get; set; }
        public int RiskId { get; set; }
        public string RiskId_ { get; set; }
        public System.Guid Project_Id { get; set; }
        public string RiskDescription { get; set; }
        public string RiskManagement { get; set; }
        public int Probability_Id { get; set; }
        public int Consequence { get; set; }
        public int RiskClass_Id { get; set; }
        public int RiskOwner_Id { get; set; }
        public string Area { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public Nullable<System.Guid> Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool isDeleted { get; set; }
        public string Probability { get; set; }
        public string RiskClass { get; set; }
        public string RiskOwner { get; set; }
        
    }
}