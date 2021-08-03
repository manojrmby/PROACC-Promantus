using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class CustomerModel
    {
        public System.Guid Customer_ID { get; set; }
        public string Company_Name { get; set; }
        public int IndustrySector_ID { get; set; }
        public string Contact { get; set; }
        public string Countrycode { get; set; }
        public string DialCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] TS { get; set; }
       
        public String IndustrySector { get; set; }
        public String Customer_Count { get; set; }
    }
}