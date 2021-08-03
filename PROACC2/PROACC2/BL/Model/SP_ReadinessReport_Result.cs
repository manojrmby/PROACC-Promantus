using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class SP_ReadinessReport_Result
    {
        public Nullable<int> RC { get; set; }
        public Nullable<int> RC_NON { get; set; }
        public Nullable<int> R_NON { get; set; }
        public Nullable<int> R { get; set; }
        public Nullable<int> ECC { get; set; }
        public Nullable<int> Hana { get; set; }
    }
}