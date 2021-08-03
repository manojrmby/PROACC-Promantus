using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class FileUpload_Model
    {
        public Guid CreatedBy { get; set; }
        public Guid FileName { get; set; }
        public DateTime Createdon { get; set; }
    }
}