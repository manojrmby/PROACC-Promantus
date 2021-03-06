using System;

namespace PROACC2.BL.Model
{
	public class ProjectMonitorModel
	{
		public int SNO { get; set; }
		public Guid Id { get; set; }
		public int ActivityID { get; set; }
		public Guid Instance { get; set; }

		public string Task { get; set; }

		public int BuildingBlockID { get; set; }
		public int PhaseId { get; set; }
		public int SequenceNum { get; set; }

		public int ApplicationAreaID { get; set; }

		public bool Task_Other_Environment { get; set; }
		public bool Dependency { get; set; }
		public string Pending { get; set; }
		public bool Delay_occurred { get; set; }

		public string Delay_occurred_Report { get; set; }

		public string Delayed_Reas { get; set; }
		public int RoleID { get; set; }
		public Guid UserID { get; set; }
		public int StatusId { get; set; }
		//public double EST_hours { get; set; }
		public decimal EST_hours { get; set; }
		public String EST_hrs { get; set; }
		//public double Actual_St_hours { get; set; }
		public decimal Actual_St_hours { get; set; }
		public String Actual_St_hrs { get; set; }

		public DateTime Planed__St_Date { get; set; }

		public DateTime Actual_St_Date { get; set; }

		public DateTime Planed__En_Date { get; set; }
		public DateTime Actual_En_Date { get; set; }
		public string Notes { get; set; }
		//public bool isActive { get; set; }
		public DateTime Cre_on { get; set; }
		public string Cre_on_str { get; set; }
		public Guid Cre_By { get; set; }
		public DateTime Modified_On { get; set; }
		public String Modified_date { get; set; }
		public Guid Modified_by { get; set; }
		//public bool IsDeleted { get; set; }

		public String Phase { get; set; }
		public String ApplicationArea { get; set; }
		public String BuildingBlock { get; set; }
		public String Owner { get; set; }
		public String Status { get; set; }
		public String CreatedDate { get; set; }
		public String PlanedDate { get; set; }
		public String ActualDate { get; set; }
		public String PlanedEn_Date { get; set; }
		public String ActualEn_Date { get; set; }
		public int PreviousID { get; set; }
		public int Task_id { get; set; }
		public Guid? parallel_Id { get; set; }
		public int ParallelName { get; set; }
		public String Parallel_Name { get; set; }
		public String Role { get; set; }
		public Guid Comments_Id { get; set; }
		public String Comments { get; set; }
		public String Name { get; set; }
		public String ShortName { get; set; }
		public String Comments_Count { get; set; }
		public String TotalTask { get; set; }
		public String Completed { get; set; }
		public String WIP { get; set; }
		public String ONHOLD { get; set; }
		public String YetToStart { get; set; }
		public String TaskType { get; set; }
		public string UserID1 { get; set; }
		public string Image { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string Colour { get; set; }
		public string Unassigned_Count { get; set; }
		public byte[] TS { get; set; }

	}
}