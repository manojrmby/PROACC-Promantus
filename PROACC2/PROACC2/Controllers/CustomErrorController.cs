using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PROACC2.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: Error
       
        [HandleError]
        public ActionResult Index()
        {
            return View();
        }
        [HandleError]
        public ActionResult NotFound(object A)
        {
            return View();
        }
        [HandleError]
        public ActionResult SqlConnectionError()
        {
            return View();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            //_Logger.Error(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/ErrorHandler/Index.cshtml"
            };
        }

    }
}