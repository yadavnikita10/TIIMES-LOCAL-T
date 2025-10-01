using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TuvVision
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 72 * 60;
          
        }
        protected void Session_End(object sender, EventArgs e)
        {
            Session["UserLoginID"] = null;
            Session.Abandon();
            Session.Clear();

        }
        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
                
            }
        }


        ///added by Savio S for error control on 2nd April 2024
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();

        //    if (exception.InnerException != null)
        //    {
        //        //HttpContext.Current.Session["ExceptionMessage"] = exception.Message;
        //        HttpContext.Current.Session["ExceptionMessage"] = exception.InnerException.Message;
        //    }
        //    else
        //    {
        //        //HttpContext.Current.Session["ExceptionMessage"] = exception.InnerException.Message;
        //        HttpContext.Current.Session["ExceptionMessage"] = exception.Message;
        //    }

            
        //    HttpContext.Current.Session["StackTrace"] = exception.StackTrace;

        //    Response.Redirect("~/Error/ErrorPage");
        //}
        ///added by Savio S for error control on 2nd April 2024


    }
}
