using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Dasboard_Donut
    {
        public Guid customerId { get; set; }
        public string customerName { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid InstanceId { get; set; }
        public string InstanceName { get; set; }
        public int ReadinessCheck  { get; set; }
        public int PhaseId { get; set; }
        public int Total_Task { get; set; }
        public int Comp_Task { get; set; }
        public string Top5Con_ID { get; set; }
        public string Top5Con_Image { get; set; }
        public string Top5Con_Name { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Colour { get; set; }

        public string YetToStart { get; set; }
        public string TotalTask { get; set; }
        public string Completed { get; set; }
    }
}