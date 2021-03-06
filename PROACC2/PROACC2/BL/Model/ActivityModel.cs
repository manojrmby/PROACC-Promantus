using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class ActivityModel
    {
        public int Activity_ID { get; set; }
        public string Task { get; set; }
        public int BuildingBlock_id { get; set; }
        public int PhaseID { get; set; }
        public Nullable<int> Sequence_Num { get; set; }
        public int ApplicationAreaID { get; set; }
        public int RoleID { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<double> EST_hours { get; set; }
        public String EST_hours1 { get; set; }
        public String Buildingblock { get; set; }
        public String Phase { get; set; }
        public String Role { get; set; }
        public String ApplicationArea { get; set; }
        public int Task_id { get; set; }
        public String Tasktype { get; set; }
        public String ParallelType { get; set; }
        public Guid? Parallel_Type { get; set; }

        public Guid? Version_Id { get; set; }

        public int PreviousId { get; set; }
        public int? TaskId { get; set; }
        public Nullable<System.Guid> ParallelId { get; set; }
        public String Parallel_Name { get; set; }
    }
}