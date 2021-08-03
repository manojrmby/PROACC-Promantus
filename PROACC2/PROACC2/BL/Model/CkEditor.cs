using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROACC2.BL.Model
{
    public class CkEditor
    {
        public Guid Id { get; set; }

        [AllowHtml]
        public string Comments { get; set; }
        public Guid Cre_By { get; set; }

    }
}