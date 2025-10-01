using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TuvVision
{
    public class SessionTimeoutAttribute:ActionFilterAttribute
    {
        private readonly Type[] excludedControllers;

        public SessionTimeoutAttribute(params Type[] excludedControllers)
        {
            this.excludedControllers = excludedControllers;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerType = filterContext.Controller.GetType();

            // Check if the current controller is in the excluded list
            if (excludedControllers.Any(c => c == controllerType))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            HttpSessionStateBase session = filterContext.HttpContext.Session;


            //checks if session is null or user is not authenticated.
            if (session == null || session["UserId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/UserLogin");
            }


            //checks if user is inactive for more than 20 mins.
            DateTime lastActivity = Convert.ToDateTime(session["LastActivity"]);
            TimeSpan timeSinceLastActivity = DateTime.Now - lastActivity;

            if (timeSinceLastActivity.TotalMinutes > 20)
            {
                if (FormsAuthentication.IsEnabled)
                {
                    FormsAuthentication.SignOut();
                }

                global.SessionModule(Convert.ToString(session["UserLoginID"]), Convert.ToString(session["UserIDs"]), "3", Convert.ToString(session["SessionID"]), null);
                global.ActivityPing(Convert.ToString(session["UserIDs"]), Convert.ToString(session["SessionID"]), "session Terminated");

                filterContext.Result = new RedirectResult("~/Login/UserLogin");
                return;
            }

            session["LastActivity"] = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }
    }
}