using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
namespace TuvVision
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CheckSessionOutAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var routeDataSet = filterContext.RouteData;
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (!controllerName.Contains("Login"))
            {
                HttpSessionStateBase session = filterContext.HttpContext.Session;
                var user = session["SessionLoggedUserID"]; //Key 2 should be User or UserName
                //&& (!session.IsNewSession)
                if (((user == null)))
                {
                    HttpContext context = HttpContext.Current;
                    //send them off to the login page
                    var url = new UrlHelper(filterContext.RequestContext);


                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult
                        {
                            Data = new
                            {
                                ErrorMessage = "SystemSessionTimeOut"
                            },
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                    else
                    {

                        string redirectTo = "~/Login/UserLogin";
                        redirectTo = string.Format("~/Login/UserLogin");
                        filterContext.Result = new RedirectResult(redirectTo);
                    }
                    return;

                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}