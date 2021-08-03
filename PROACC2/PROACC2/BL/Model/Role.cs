using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public String RoleName { get; set; }
        public String Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Count { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid ModifiedBy { get; set; }
        public byte[] TS { get; set; }
    }

}