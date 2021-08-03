using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class LogedUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Guid ID { get; set; }
        public Int32 Type { get; set; }
        public string Name { get; set; }

        public Guid LogID { get; set; }
    }
}