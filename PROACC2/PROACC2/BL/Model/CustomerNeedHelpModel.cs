using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class CustomerNeedHelpModel
    {
        public int User_ID { get; set; }
        public string First_Name { get; set; }
        public string Second_Name { get; set; }
        public string Email { get; set; }
        public string Phone_No { get; set; }
        public Boolean Need_Help { get; set; }
    }
}