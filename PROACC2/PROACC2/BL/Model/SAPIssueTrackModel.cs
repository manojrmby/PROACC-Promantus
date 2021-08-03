using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class SAPIssueTrackModel
    {
        public System.Guid SAPIssuetrack_Id { get; set; }
        public int RunningID { get; set; }
        public int IssueNo { get; set; }
        public string IssueName { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }
        public string Assignee { get; set; }
        public string RaisedBy { get; set; }
        public string ApplicationArea { get; set; }
        public string OpenDt { get; set; }
        public string CloseDt { get; set; }
        public string status { get; set; }
        public int Status_Id { get; set; }
        public string Resolution { get; set; }
        public string Comments { get; set; }
    }
}