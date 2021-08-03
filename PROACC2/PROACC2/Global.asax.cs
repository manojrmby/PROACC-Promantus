using PROACC2.BL;
using PROACC2.BL.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PROACC2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Boolean Serverstatus = false;
            Base @base = new Base();
            Serverstatus =@base.IsServerConnected();
            if (Serverstatus)
            {
                
            }
            else
            {
                //throw new InvalidOperationException("Sql Connection Failed Expection");

            }
            Mail mail = new Mail();
            mail.StartMailSend();
        }
        
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            //    //log the error!
                LogHelper log = new LogHelper();
                log.createLog(ex);

        }
    }
}
