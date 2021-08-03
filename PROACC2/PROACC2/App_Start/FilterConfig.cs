using PROACC2.BL.General;
using System.Web;
using System.Web.Mvc;

namespace PROACC2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleExceptionAttribute());
        }
    }
}
