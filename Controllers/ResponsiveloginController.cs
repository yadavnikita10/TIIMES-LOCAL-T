using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Controllers
{
    public class ResponsiveloginController : Controller
    {
        // GET: Responsivelogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginForm()
        {
            return View();
        }
    }
}