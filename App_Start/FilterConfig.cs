using System.Web;
using System.Web.Mvc;

namespace TuvVision
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CheckSessionOutAttribute());
            filters.Add(new SessionTimeoutAttribute());
        }
    }
}
