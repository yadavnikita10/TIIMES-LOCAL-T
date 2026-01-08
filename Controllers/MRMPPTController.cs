using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;

namespace TuvVision.Controllers
{
    public class MRMPPTController : Controller
    {
        // GET: MRMPPT

        DALInspectionVisitReport objdal = new DALInspectionVisitReport();

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MRMReport()
        {
            return View();
        }


        public ActionResult GenerateMRMReport()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetDataMRM();
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string fileName = "MRM_Report.pptx";
                    string filePath = Server.MapPath("~/Reports/" + fileName);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}