using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class EccHanaCountModel
    {
        //ECC S4 Hana Count
        public double S_No { get; set; }
        public string User { get; set; }
        public string UserType { get; set; }
        public string UserCategory { get; set; }
        public string User_Status { get; set; }
        public string System { get; set; }
        public string Last_Logon { get; set; }
    }
}