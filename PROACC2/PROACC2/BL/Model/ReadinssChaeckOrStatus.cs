using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL
{
    public class ReadinssChaeckOrStatus
    {
        public int UploadStatus { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int TotalTask { get; set; }
        public int WIP { get; set; }
        public int ONHOLD { get; set; }
        public int YetToStart { get; set; }
    }
}