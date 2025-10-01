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
    public class QAMetrixController : Controller
    {

        DalQAMetrix objCMV = new DalQAMetrix();
        DALTrainingSchedule objDTS = new DALTrainingSchedule();

        // GET: QAMetrix
        public ActionResult Index()
        {
            return View();
        }
        //[HttpGet]
        //public ActionResult QAMetrix()
        //{
        //    DataTable dtGetNAME = new DataTable();

        //    dtGetNAME = objCMV.dtGetNAME();


        //    ViewBag.BodyData = dtGetNAME;


        //    return View();
        //}

        [HttpGet]
        public ActionResult QAMetrix()
        {

            List<NameCode> lstProjectType = new List<NameCode>();
            List<NameCode> lstScope = new List<NameCode>();
            DataSet DSGetAllddllst = new DataSet();
            DataSet dtIAFScope = new DataSet();
            DataSet DSEditGetList = new DataSet();

            List<NameCode> lstEditBranchList = new List<NameCode>();
            List<NameCode> lstEditUserList = new List<NameCode>();
            List<NameCode> lstEmploymentCategory = new List<NameCode>();
            List<NameCode> listTrainCate = new List<NameCode>();

            DSEditGetList = objDTS.GetDdlLst();

            //// OBS Types
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ProjectName"].ToString()),
                                      Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            IEnumerable<SelectListItem> OBSTypeItems;
            OBSTypeItems = new SelectList(lstProjectType, "Code", "Name");

            ViewBag.OBSType = lstProjectType;
            ViewData["OBSType"] = OBSTypeItems;
            DataTable dtGetNAME = new DataTable();

            dtGetNAME = objCMV.dtGetNAME();
            //var pivotedData = PivotData(dtGetNAME);
            //ViewBag.BodyData = pivotedData;

            ViewBag.BodyData = dtGetNAME;



            return View();
        }


        //[HttpPost]
        //public ActionResult QAMetrix(string FromDate, string ToDate)
        //{
        //    DataTable dtGetNAME = new DataTable();   
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);      
        //    dtGetNAME = objCMV.GetDataByDate(FromDate, ToDate);
        //    ViewBag.BodyData = dtGetNAME;
        //    return View();
        //}
        [HttpPost]
        public ActionResult QAMetrix(string FromDate, string ToDate, FormCollection fc, QAMetrix S)
        {
            //added by ruchita on 02042024
            #region
            List<NameCode> lstProjectType = new List<NameCode>();
            List<NameCode> lstScope = new List<NameCode>();
            DataSet DSGetAllddllst = new DataSet();
            DataSet dtIAFScope = new DataSet();
            DataSet DSEditGetList = new DataSet();


            List<NameCode> lstEditBranchList = new List<NameCode>();
            List<NameCode> lstEditUserList = new List<NameCode>();
            List<NameCode> lstEmploymentCategory = new List<NameCode>();
            List<NameCode> listTrainCate = new List<NameCode>();

            DSEditGetList = objDTS.GetDdlLst();
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstProjectType = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ProjectName"].ToString()),
                                      Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                                  }).ToList();
            }
            IEnumerable<SelectListItem> OBSTypeItems;
            OBSTypeItems = new SelectList(lstProjectType, "Code", "Name");

            ViewBag.OBSType = lstProjectType;
            ViewData["OBSType"] = OBSTypeItems;
            #endregion
            string Costcenter = string.Join(",", fc["OBS"]);
            S.CostCenter = Costcenter;

            DataTable dtGetNAME = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            dtGetNAME = objCMV.GetDataByDate(FromDate, ToDate, S);
            ViewBag.BodyData = dtGetNAME;
            string[] costCenters = S.CostCenter.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            ViewBag.CostCenters = costCenters;

            ViewData["OBSTypechecked"] = S.CostCenter;  // added by nikita on 10012024

            ViewData["selected"] = S.CostCenter;// added by ruchita on 02042024
            return View();
        }


    }
}