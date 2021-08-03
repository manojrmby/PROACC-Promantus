using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class FileUploadMaster
    {
        public System.Guid Id { get; set; }
        public int id_Int { get; set; }
        public System.Guid InstanceID { get; set; }
        public string File_Type { get; set; }
        public string C_FileName { get; set; }
        public int FileType { get; set; }
        public bool isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_by { get; set; }
        public bool IsDeleted { get; set; }
    }
}