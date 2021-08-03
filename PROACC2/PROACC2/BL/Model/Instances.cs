using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Instances
    {

        public System.Guid Instance_id { get; set; }
        public string InstanceName { get; set; }
        public System.Guid Project_ID { get; set; }
        public System.Guid Version_ID { get; set; }
        public string Version_Name { get; set; }
        public System.DateTime LastUpdated_Dt { get; set; }
        public bool AssessmentUploadStatus { get; set; }
        public bool PreConvertionIsActive { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public String Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public String ProjectName { get; set; }

        public string Description { get; set; }
        public string Sys_Landscape { get; set; }
        public bool IsDeleted { get; set; }


        public List<Instances> Insta { get; set; }

        // SAP Connection

        public String destinationName { get; set; }
        public String AppServerHost { get; set; }
        public String SystemNumber { get; set; }
        public String SAPRouter { get; set; }
        public String User { get; set; }
        public String Password { get; set; }
        public String Client { get; set; }
        public String Language { get; set; }
        public String PoolSize { get; set; }
    }
}