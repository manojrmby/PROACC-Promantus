using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class PlannedVsActual
    {
        public Guid ID { get; set; }
        public int ActivityID { get; set; }
        public string Task { get; set; }
        public string Planed__St_Date { get; set; }
        public string Planed__En_Date { get; set; }
        public string Actual_St_Date { get; set; }
        public string Actual_En_Date { get; set; }
        public decimal Actual_St_hours { get; set; }
        //public class Table_List
        //{
           
        //}
        //public class DataList
        //{
        //    public string minDate { get; set; }
        //    public string maxDate { get; set; }

        //    public List<Table_List> Table_s { get; set; }
        //}

    }
}