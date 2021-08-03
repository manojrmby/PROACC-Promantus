using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class BellModel
    {
        public Guid Id { get; set; }
        public Guid Table_Id { get; set; }
        public string Table_Name { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string IssueName { get; set; }
        public string ConsUserName { get; set; }
        public string PmName { get; set; }

        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public String Createdon { get; set; }
        public String Day { get; set; }
        public String Hour { get; set; }
        public String Minutes { get; set; }
        public String Second { get; set; }
        public String Month { get; set; }
        public String Year { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public int CountMessgae { get; set; }
    }
}