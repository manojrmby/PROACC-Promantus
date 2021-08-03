using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class ParallelType
    {
        public Guid ParallelId { get; set; }
        public int ParallelName { get; set; }
        public string Parallel_Name { get; set; }
        public Guid Cre_By { get; set; }

    }
}