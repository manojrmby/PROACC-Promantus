﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class IssueTrackModel
    {
        public System.Guid Issuetrack_Id { get; set; }
        public int RunningID { get; set; }
        public string IssueName { get; set; }
        public int PhaseID { get; set; }
        public Nullable<int> TaskId { get; set; }
        public System.Guid ProjectInstance_Id { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
        public System.Guid AssignedTo { get; set; }
        public bool IsApproved { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public String Status { get; set; }
        public String Comments { get; set; }
        public String Task { get; set; }
        public String IssueID { get; set; }
        public String Phase { get; set; }
        public String Assigned { get; set; }
        public String Description { get; set; }
        public String Instance { get; set; }
        public String Project { get; set; }
        public String Customer { get; set; }
        public String Created_By { get; set; }
        public System.Guid Customer_Id { get; set; }
        public System.Guid Project_Id { get; set; }
        public System.Guid Instance_Id { get; set; }
        public String Name { get; set; }
        public int HistoryLogId { get; set; }
        public string Cre_on_str { get; set; }
        public bool AssigneeCount { get; set; }

        public string UserID { get; set; }
        public string Image { get; set; }
    }
}