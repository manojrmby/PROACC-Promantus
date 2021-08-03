using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class ParallelTypeModel
    {
        public System.Guid ParallelId { get; set; }
        public Nullable<int> ParallelName { get; set; }
        public string Parallel_Name { get; set; }
        public Nullable<bool> isActive { get; set; }
        public System.DateTime Cre_on { get; set; }
        public System.Guid Cre_By { get; set; }
        public Nullable<System.DateTime> Modified_On { get; set; }
        public Nullable<System.Guid> Modified_By { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}