using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class AMVersion
    {
        public Guid ID { get; set; }
        public string Version_Name { get; set; }
        public Guid FileName { get; set; }
        public Boolean isActive { get; set; }
        public DateTime Modified_On { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid Modified_By { get; set; }
    }
}