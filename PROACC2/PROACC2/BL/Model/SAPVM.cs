using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class SAPCls
    {
        public string FIELD1 { get; set; }
        public string FIELD2 { get; set; }
        public string FIELD3 { get; set; }
        public string FIELD4 { get; set; }
        public string FIELD5 { get; set; }
        public string FIELD6 { get; set; }
        public string FIELD7 { get; set; }
    }
    public class SAPVM
    {
        public List<SAPCls> ZPROACC { get; set; }
    }
}