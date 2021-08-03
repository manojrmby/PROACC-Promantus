using PROACC2.BL;
using PROACC2.BL.General;
using PROACC2.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROACC2.Controllers
{

    [CheckSessionTimeOut]
    [Authorize(Roles = "Admin")]
    public class OtherController : Controller
    {
        // GET: Other
        Base _base = new Base();
        public ActionResult Index(string name)
        {
            List<AMVersion> Version = _base.Sp_GetVersion();
            ViewBag.Version = Version;
            ViewBag.VersionCount = Version.Count();
            return PartialView(name);
        }
        public ActionResult _VersionIndex()
        {
            return PartialView("_VersionIndex");
        }
        public ActionResult __UpdateVersion(Guid id)
        {
            AMVersion V = new AMVersion();
            V = _base.SP_GetVersionbyid(id);
            //ViewBag.Instance = Insta;
            return Json(V, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateVersion(Guid Id, string VersionName)
        {
            AMVersion AM = new AMVersion();
            AM.ID = Id; 
            AM.Modified_By = Guid.Parse(Session["loginid"].ToString());
            AM.Version_Name = VersionName;

            bool result = _base.SP_UpdateVersion(AM);
            if (result == true)
            {
                return Json("success");
            }
            else
            {
                return Json("error");
            }
        }

    }
}