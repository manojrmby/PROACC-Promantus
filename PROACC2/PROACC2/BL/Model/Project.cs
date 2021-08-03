using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Project
    {
        public List<Project> _Proj { get; set; }
        public Guid PID { get; set; }

        public string ProjectID { get; set; }
        public Guid ProjectId { get; set; }
        public String ProjectName { get; set; }
        public String Description { get; set; }
        public String CustomerName { get; set; }

        public String ProjectManagerName { get; set; }
        public String ScenarioName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }

        public List<Instances> Instance { get; set; }
        public int Count { get; set; }
        public int Ins_Count { get; set; }
        public int PM_Count { get; set; }
        public Guid ProjectManagerID { get; set; }
        public Guid CustomerID { get; set; }
        public int  ScenerioID { get; set; }
        public string BuildingBlockName { get; set; }

        public Boolean Scenario_Status { get; set; }
    }
}