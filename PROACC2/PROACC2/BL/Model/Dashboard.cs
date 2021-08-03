using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class Dashboard
    {
        public int proj { get; set; }
        public int TotalTask { get; set; }
        public int TotalTaskComp { get; set; }
        public int client { get; set; }
        public int consult { get; set; }
        public string ClientIDs { get; set; }
        public string ClientNames { get; set; }
        public string ConsuIDs { get; set; }
        public string ConsuNames { get; set; }
        public string All_ClientIDs { get; set; }
        public string All_UserIDs { get; set; }
        public string ClientIDs_Image { get; set; }
        public string ConsuIDs_Image { get; set; }

    }
}