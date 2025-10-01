using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult ErrorPage()
        {
            ///added by Savio S for error control on 2nd May 2024

            ViewBag.ExceptionMessage = HttpContext.Session["ExceptionMessage"];
            ViewBag.StackTrace = HttpContext.Session["StackTrace"];

            HttpContext.Session.Remove("ExceptionMessage");
            HttpContext.Session.Remove("StackTrace");

            return View();
            ///added by Savio S for error control on 2nd May 2024
        }
    }
}