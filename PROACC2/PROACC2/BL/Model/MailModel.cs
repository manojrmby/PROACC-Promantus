using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PROACC2.BL.Model
{
    public class MailModel
    {
        public int Running_ID { get; set; }
        public string Name { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string TemplateName { get; set; }

        public int Q_UserID { get; set; }
        public bool MailStatus { get; set; }
        public Guid? LineID { get; set; }
        // public string Task { get; set; }
    }
}