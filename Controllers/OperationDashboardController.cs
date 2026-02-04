
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

namespace TuvVision.Controllers
{
    public class OperationDashboardController : Controller
    {
        // GET: OperationDashboard
        DALMISOPEReport objDMOR = new DALMISOPEReport();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult OperationDashboard()
        {
            return View();
        }
        public ActionResult GetFolder()
        {

            DataSet ds = objDMOR.GetFolder();

            List<string> folderList = new List<string>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    folderList.Add(row["FolderName"].ToString());
                }
            }


            return Json(folderList, JsonRequestBehavior.AllowGet);
        
    }

    }
}