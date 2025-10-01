using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult MdSpeak()
        {
            return View();
        }

        public ActionResult Clusterheadspeak()
        {
            return View();
        }

        public ActionResult QualityPolicy()
        {
            return View();
        }

        public FileStreamResult GetQualityPolicyPDF()
        {
            string path = HttpContext.Server.MapPath("../PDF/QualityPolicy.pdf");
            FileStream fs = new FileStream( path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }      

        public ActionResult QualityObjective()
        {
            return View();
        }

        public FileStreamResult GetQObjPDF()
        {
            string path = HttpContext.Server.MapPath("../PDF/QualityObjective.pdf");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult HSCPolicy()
        {
            return View();
        }

        public FileStreamResult GetHSCPolicyPDF()
        {
            string path = HttpContext.Server.MapPath("../PDF/HSEPolicy.pdf");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult ISMSPolicy()
        {
            return View();
        }

        public FileStreamResult GetISMSPolicyPDF()
        {
            string path = HttpContext.Server.MapPath("../PDF/ISMSPolicy.pdf");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        public ActionResult EnvPolicy()
        {
            return View();
        }

        public FileStreamResult GetEnvPolicyPDF()
        {
            string path = HttpContext.Server.MapPath("../PDF/QualityPolicy.pdf");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }
    }
}