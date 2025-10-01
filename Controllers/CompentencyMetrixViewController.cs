using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using OfficeOpenXml.Style;
using static TuvVision.Models.monitors;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Globalization;

namespace TuvVision.Controllers
{
    public class CompentencyMetrixViewController : Controller
    {
        DALUsers objDalCreateUser = new DALUsers();
        CommonControl objCommonControl = new CommonControl();
        DALOffSiteMonitoring objoff = new DALOffSiteMonitoring();
        DataTable DTEditUser = new DataTable();
        OnSiteMonitoring objDAM = new OnSiteMonitoring();
        Users ObjModelUsers = new Users();
        monitors obj = new monitors();
        Mentoring obj2 = new Mentoring();
        MonitoringOfMonitors Obj3 = new MonitoringOfMonitors();
        DALMentoring DalObj2 = new DALMentoring();
        DALMonitoringOfMonitors objMOM = new DALMonitoringOfMonitors();
        ModelOffSiteMonitoring OFS = new ModelOffSiteMonitoring();
        DALBranchMaster objDalCompany = new DALBranchMaster();
        BranchMasters ObjModelCompany = new BranchMasters();
        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        DALLibraryMaster OBJAppl = new DALLibraryMaster();
        List<BranchMasters> lstCompanyDashBoard = new List<BranchMasters>();
        // GET: CompentencyMetrix
        DALCompentencyMatrixView objCMV = new DALCompentencyMatrixView();
        List<NameCode> lstEditUserList = new List<NameCode>();
        MonitoringRecord MonitoringRecord = new MonitoringRecord();
        MonitorRecordData MonitorRecordData = new MonitorRecordData();
        string[] ids;
        // DALCalls objDalCalls = new DALCalls();
        public ActionResult CompentencyMetrixView(String Id)
        {

            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Stamp (Image)
            dtGetNAME = objCMV.dtGetNAME();
            List<CompentencyMetrixView> lstGetName = new List<CompentencyMetrixView>();

            if (dtGetNAME.Rows.Count > 0)
            {

                foreach (DataRow dr in dtGetNAME.Rows)
                {

                    lstGetName.Add(new CompentencyMetrixView
                    {
                        CompentencyMetrixMasterId = Convert.ToInt32(dr["Id"]),
                        CompentencyMetrixMasterName = Convert.ToString(dr["Name1"]),

                    }
                    );
                }
            }
            ViewData["Name"] = lstGetName;
            #endregion

            var model = new CompentencyMetrixView();
            DataSet dss = new DataSet();
            DataTable dsGetStamp = new DataTable();



            // if (Id != null)
            if (Id == "0")
            {
                return View();
            }
            else
            {
                // dss = objCMV.GetDataById(Convert.ToInt32(Id));
                dss = objCMV.GetDataById(Id);
                if (dss.Tables[0].Rows.Count > 0)
                {


                    model.Id = Convert.ToInt32(dss.Tables[0].Rows[0]["Id"]);
                    model.CandidateName = dss.Tables[0].Rows[0]["CandidateName"].ToString();
                    model.Location = dss.Tables[0].Rows[0]["Location"].ToString();
                    model.EducationalQualification = dss.Tables[0].Rows[0]["EducationalQualification"].ToString();
                    model.AdditionalQualification = "";//dss.Tables[0].Rows[0]["AdditionalQualification"].ToString();
                    model.Designation = dss.Tables[0].Rows[0]["Designation"].ToString();
                    model.EmailId = dss.Tables[0].Rows[0]["EmailId"].ToString();
                    model.CellPhoneNumber = dss.Tables[0].Rows[0]["CellPhoneNumber"].ToString();
                    model.JoiningDate1 = dss.Tables[0].Rows[0]["JoiningDate"].ToString();
                    model.TotalExperienceInYears = dss.Tables[0].Rows[0]["TotalExperienceInYears"].ToString();
                    model.NumberOfYearsWithTUVIndia = dss.Tables[0].Rows[0]["NumberOfYearsWithTUVIndia"].ToString();
                    model.CheckBoxValue = dss.Tables[0].Rows[0]["CheckBoxValue"].ToString();
                    model.CandidateId = dss.Tables[0].Rows[0]["CandidateId"].ToString();
                }
                return View(model);

            }
        }


        [HttpPost]
        public ActionResult CompentencyMetrixView(CompentencyMetrixView C, FormCollection fc, int[] ID_)
        {
            string Result = string.Empty;
            try
            {
                #region get Scope Id (CheckBoxValue)
                if (ID_ != null)
                {
                    ids = fc["ID_"].Split(new char[] { ',' }); //fc["ID"].Split(new char[] { ',' });

                    //Remove null entries
                    ids = ids.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    string ScopeId = String.Join(",", ids.Where(s => !string.IsNullOrEmpty(s)));
                    string a = Convert.ToString(ScopeId);
                    C.CheckBoxValue = a;
                }
                #endregion

                if (C.CandidateId != null)
                {
                    //Update
                    Result = objCMV.Insert(C);
                }
                else
                {

                    Result = objCMV.Insert(C);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        ModelState.Clear();
                        TempData["message"] = "Record Added Successfully...";
                    }
                    else
                    {
                        TempData["message"] = "Something went Wrong! Please try Again";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("ListCompentencyMetrixView", "CompentencyMetrixView");
        }

        [HttpGet]
        public ActionResult ListCompentencyMetrixView()
        {
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.  
            DataSet ds = new DataSet();


            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Scope (Name)
            dtGetNAME = objCMV.dtGetNAME();
            List<CompentencyMetrixView> lstGetName = new List<CompentencyMetrixView>();

            if (dtGetNAME.Rows.Count > 0)
            {

                foreach (DataRow dr in dtGetNAME.Rows)
                {

                    lstGetName.Add(new CompentencyMetrixView
                    {
                        CompentencyMetrixMasterId = Convert.ToInt32(dr["Id"]),
                        CompentencyMetrixMasterName = Convert.ToString(dr["Name1"]),

                    }
                    );
                }
            }
            ViewData["Name"] = lstGetName;
            #endregion


            ds = objCMV.GetData(); // fill dataset  



            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                DateTime JoiningD = Convert.ToDateTime(ds.Tables[0].Rows[0]["JoiningDate"]);
                DateTime cuD = DateTime.Now;
                // DateTime calD =
                // TimeSpan ts = cuD.Year - JoiningD.Year;

                lmd.Add(new CompentencyMetrixView
                {
                    Id = 0,// Convert.ToInt32(dr["Id"]),
                    CandidateName = Convert.ToString(dr["InspectorName"]),
                    Location = Convert.ToString(dr["Branch_Name"]),
                    EducationalQualification = Convert.ToString(dr["EducationalQualification"]),
                    //AdditionalQualification     = Convert.ToString(dr["AdditionalQualification"]),
                    Designation = Convert.ToString(dr["Designation"]),
                    EmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                    CellPhoneNumber = Convert.ToString(dr["CellPhoneNumber"]),
                    JoiningDate1 = Convert.ToString(dr["DateOfJoining"]),
                    TotalExperienceInYears = Convert.ToString(dr["BeforeTUVExperience"]),
                    NumberOfYearsWithTUVIndia = Convert.ToString(dr["NumberOfYearsWithTUVIndia"]),
                    CheckBoxValue = Convert.ToString(dr["CheckBoxValue"]),
                    TotalTUVExperience = Convert.ToString(dr["TotalTUVExp"]),
                    PK_userId = Convert.ToString(dr["PK_userId"]),
                    OverAllExp = Convert.ToString(dr["OverAllExp"]),
                    StampNumber = Convert.ToString(dr["TUVIStampNo"])

                });
            }
            return View(lmd.ToList());

        }

        public JsonResult GetCandidateName(string prefix)

        {
            DataSet dsCandidateName = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsCandidateName = objCMV.GetCandidateName(prefix);
            List<CompentencyMetrixView> searchlist = new List<CompentencyMetrixView>();

            foreach (DataRow dr in dsCandidateName.Tables[0].Rows)

            {

                searchlist.Add(new CompentencyMetrixView
                {
                    CandidateName = dr["CandidateName"].ToString(),
                    CandidateId = dr["CandidateId"].ToString()
                });

            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetCandidateDetail(string IRNReport)

        {
            var m = new CompentencyMetrixView();
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objCMV.GetCandidateDetail(IRNReport);
            List<CompentencyMetrixView> searchlist = new List<CompentencyMetrixView>();


            if (dsTopic.Tables[0].Rows.Count > 0)
            {
                ViewBag.JoiningDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);
                //searchlist.Add(new CompentencyMetrixView
                //{

                m.Location = Convert.ToString(dsTopic.Tables[0].Rows[0]["BranchName"]);

                m.EducationalQualification = Convert.ToString(dsTopic.Tables[0].Rows[0]["Qualification"]);

                m.Designation = Convert.ToString(dsTopic.Tables[0].Rows[0]["Designation"]);

                m.EmailId = Convert.ToString(dsTopic.Tables[0].Rows[0]["EmailId"]);

                m.CellPhoneNumber = Convert.ToString(dsTopic.Tables[0].Rows[0]["MobileNo"]);
                m.JoiningDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);

                m.JoiningDate1 = dsTopic.Tables[0].Rows[0]["DateOfJoining"].ToString();
                m.CandidateId = dsTopic.Tables[0].Rows[0]["PK_UserID"].ToString();
                m.Success = dsTopic.Tables[0].Rows[0]["Success"].ToString();
                m.AdditionalQualification = dsTopic.Tables[0].Rows[0]["Qualification"].ToString();
                m.TotalExperienceInYears = dsTopic.Tables[0].Rows[0]["TotalyearofExprience"].ToString();


            }
            return Json(m, JsonRequestBehavior.AllowGet);






        }

        public ActionResult Delete(int? Id)
        {
            string Result = string.Empty;
            try
            {
                Result = objCMV.Delete(Convert.ToInt32(Id));
                if (Convert.ToInt16(Result) > 0)
                {


                    ModelState.Clear();
                }
                else
                {

                    TempData["message"] = "Error";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ModelState.Clear();
            return RedirectToAction("ListCompentencyMetrixView");


        }


        [HttpGet]
        public ActionResult ListCompentencyMetrixViewN()
        {
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.  
            DataSet ds = new DataSet();


            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Scope (Name)
            dtGetNAME = objCMV.dtGetNAME1();
            List<CompentencyMetrixView> lstGetName = new List<CompentencyMetrixView>();

            if (dtGetNAME.Rows.Count > 0)
            {

                foreach (DataRow dr in dtGetNAME.Rows)
                {

                    lstGetName.Add(new CompentencyMetrixView
                    {
                        CompentencyMetrixMasterId = Convert.ToInt32(dr["Id"]),
                        CompentencyMetrixMasterName = Convert.ToString(dr["Name"]),

                    }
                    );
                }
            }
            ViewData["Name"] = lstGetName;
            #endregion


            ds = objCMV.GetDataN(); // fill dataset  



            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                DateTime JoiningD = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateOfJoining"]);
                DateTime cuD = DateTime.Now;
                // DateTime calD =
                // TimeSpan ts = cuD.Year - JoiningD.Year;

                lmd.Add(new CompentencyMetrixView
                {
                    //PK_userId = Convert.ToString(dr["PK_userId"]),
                    //TiimesRole = Convert.ToString(dr["RoleName"]),
                    //EmployeeCategory = Convert.ToString(dr["EmployementCategory"]),
                    //StampNumber = Convert.ToString(dr["TUVIStampNo"]),

                    //Id = 0,//Convert.ToInt32(dr["Id"]),
                    //CandidateName = Convert.ToString(dr["InspectorName"]),
                    //Location = Convert.ToString(dr["Branch_Name"]),
                    //EducationalQualification = "",//Convert.ToString(dr["EducationalQualification"]),
                    ////AdditionalQualification = "",//Convert.ToString(dr["AdditionalQualification"]),
                    //Designation = Convert.ToString(dr["Designation"]),
                    //EmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                    //CellPhoneNumber = Convert.ToString(dr["MobileNo"]),
                    //JoiningDate1 = Convert.ToString(dr["DateOfJoining"]),
                    //TotalExperienceInYears = Convert.ToString(dr["BeforeTUVExperience"]),
                    //NumberOfYearsWithTUVIndia = "",//Convert.ToString(dr["NumberOfYearsWithTUVIndia"]),
                    //CheckBoxValue = "",//Convert.ToString(dr["CheckBoxValue"]),
                    //TotalTUVExperience = "",//Convert.ToString(dr["TotalTUVExp"]),
                    //OverAllExp = "",//Convert.ToString(dr["OverAllExp"]),
                    //AutharizeLevel = Convert.ToString(dr["AutharizeLevel"])

                    PK_userId = Convert.ToString(dr["PK_userId"]),
                    TiimesRole = Convert.ToString(dr["RoleName"]),
                    EmployeeCategory = Convert.ToString(dr["EmployementCategory"]),
                    StampNumber = Convert.ToString(dr["TUVIStampNo"]),

                    Id = 0,//Convert.ToInt32(dr["Id"]),
                    CandidateName = Convert.ToString(dr["InspectorName"]),
                    Location = Convert.ToString(dr["Branch_Name"]),
                    EducationalQualification = Convert.ToString(dr["Qualification"]),
                    ///  Course = Convert.ToString(dr["Course"]),

                    Designation = Convert.ToString(dr["Designation"]),
                    EmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                    CellPhoneNumber = Convert.ToString(dr["MobileNo"]),
                    JoiningDate1 = Convert.ToString(dr["DateOfJoining"]),
                    TotalExperienceInYears = Convert.ToString(dr["BeforeTUVExperience"]),
                    NumberOfYearsWithTUVIndia = "",//Convert.ToString(dr["NumberOfYearsWithTUVIndia"]),
                    CheckBoxValue = "",//Convert.ToString(dr["CheckBoxValue"]),
                    TotalTUVExperience = Convert.ToString(dr["TotalTUVExp"]),
                    OverAllExp = Convert.ToString(dr["OverAllExp"]),
                    AutharizeLevel = Convert.ToString(dr["AutharizeLevel"]),
                    strFormFilled = Convert.ToString(dr["FormFilled"]),
                    AdditionalQualification = Convert.ToString(dr["ProfCertificates"]),



                    ProfileDataCompleted = Convert.ToString(dr["ProfileDataCompleted"]),
                    OBSName = Convert.ToString(dr["OBSName"]),
                    TechniqalCompetencyVerify = Convert.ToString(dr["TechniqalCompetencyVerify"]),
                    CoreFieldOfStudy = Convert.ToString(dr["CoreFieldOfStudy"]),
                    FormFilled = Convert.ToString(dr["FormFilled"]),
                    CompetentInScope = Convert.ToString(dr["CompetentInScope"]),
                    
                });
            }
            return View(lmd.ToList());

        }


        #region Export to excel
        //[HttpGet]
        //public ActionResult ExportIndex(String Type)
        //{
        //    string strFromFilled = string.Empty;

        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        try
        //        {
        //            int colcount = 1;
        //            int rowCount = 1;
        //            package.Workbook.Worksheets.Add("Data");
        //            ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];
        //            DataTable dtgridN = GetData();
        //            DataSet MasterData = GetMasterData();
        //            sheet.Cells[rowCount, colcount].Value = "F-MR-16 R06 : Competency Matrix " + DateTime.Now.ToString("dd/MM/yyyy");


        //            rowCount = rowCount + 2;

        //            /// Header Print
        //            for (int column = 0; column < dtgridN.Columns.Count; column++)
        //            {

        //                if (dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "AUTHARIZELEVEL")
        //                {
        //                    sheet.Cells[rowCount, colcount].Value = dtgridN.Columns[column].ColumnName.ToString();
        //                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
        //                    colcount++;
        //                }
        //            }
        //            // colcount++;

        //            /// Header Print for Basic Authorisation
        //            if (MasterData.Tables.Count > 0)
        //            {

        //                for (int newMCol = 0; newMCol < MasterData.Tables[0].Rows.Count; newMCol++)
        //                {

        //                    sheet.Cells[rowCount, colcount].Value = MasterData.Tables[0].Rows[newMCol][1].ToString();
        //                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
        //                    colcount++;
        //                }
        //            }

        //            int newcol = 0;


        //            rowCount++;

        //            ///// Data Print 

        //            for (int rowcnt = 0; rowcnt < dtgridN.Rows.Count; rowcnt++)
        //            {
        //                colcount = 1;

        //                for (int colcnt = 0; colcnt < dtgridN.Columns.Count; colcnt++)
        //                {
        //                    if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() == "FORMFILLED")
        //                    {
        //                        if (dtgridN.Rows[rowcnt][colcnt].ToString() == "0")
        //                        {
        //                            strFromFilled = "NO";
        //                        }
        //                        else
        //                        {
        //                            strFromFilled = "YES";
        //                        }

        //                        sheet.Cells[rowCount, colcount].Value = strFromFilled;
        //                        colcount++;
        //                    }
        //                    else if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "AUTHARIZELEVEL")
        //                    {
        //                        sheet.Cells[rowCount, colcount].Value = dtgridN.Rows[rowcnt][colcnt].ToString();
        //                        colcount++;
        //                    }
        //                    else if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() == "AUTHARIZELEVEL")
        //                    {
        //                        DataRow[] dr = MasterData.Tables[1].Select("PK_UserID = '" + dtgridN.Rows[rowcnt]["Pk_userID"].ToString().ToUpper().Trim() + "'");
        //                        newcol = colcount;
        //                        string[] strArray;
        //                        string strAuth = string.Empty;
        //                        // newcol = colcount;

        //                        if (dr != null && dr.Count() > 0)
        //                        {
        //                            strArray = dr[0]["AUTHARIZELEVEL"].ToString().Split('#');

        //                            for (int AuthCol = 0; AuthCol < strArray.Length; AuthCol++)
        //                            {
        //                                sheet.Cells[rowCount, newcol].Value = strArray[AuthCol].ToString();
        //                                newcol++;
        //                            }
        //                        }

        //                    }
        //                }

        //                rowCount++;
        //            }






        //        }
        //        catch (Exception ex)
        //        {
        //            string errMsg = ex.Message;
        //        }

        //        return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //    }




        //}


        [HttpGet]
        public ActionResult ExportIndex(String Type)
        {
            string strFromFilled = string.Empty;

            using (ExcelPackage package = new ExcelPackage())
            {
                try
                {
                    int colcount = 1;
                    int rowCount = 1;
                    package.Workbook.Worksheets.Add("Data");

                    ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                    sheet.Protection.IsProtected = true; //--------Protect whole sheet

                    DataTable dtgridN = GetData();
                    DataSet MasterData = GetMasterData();
                    sheet.Cells[rowCount, colcount].Value = "TUV INDIA PVT LTD";
                    rowCount++;
                    sheet.Cells[rowCount, colcount].Value = "F-MR-16 R06 : Competency Matrix " + DateTime.Now.ToString();
                    rowCount++;

                    sheet.Cells[rowCount, colcount].Value = "Downloaded By " + Session["LoginName"].ToString();

                    sheet.DefaultColWidth = 50;
                    sheet.Protection.AllowAutoFilter = true;
                    rowCount = rowCount + 2;

                    /// Header Print
                    for (int column = 0; column < dtgridN.Columns.Count; column++)
                    {

                        if (
                            //dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "ROLENAME" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDBY" &&
                            //dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "FORMFILLED" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDDATE" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "PK_USERID" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MOBILENO" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "TUV_EMAIL_ID" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "EMPLOYEMENTCATEGORY" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "TUVISTAMPNO" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "AUTHARIZELEVEL"
                            )




                        {


                            sheet.Cells[rowCount, colcount].Value = dtgridN.Columns[column].ColumnName.ToString();
                            sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            colcount++;
                        }
                    }
                    // colcount++;

                    /// Header Print for Basic Authorisation
                    if (MasterData.Tables.Count > 0)
                    {

                        for (int newMCol = 0; newMCol < MasterData.Tables[0].Rows.Count; newMCol++)
                        {

                            sheet.Cells[rowCount, colcount].Value = MasterData.Tables[0].Rows[newMCol][1].ToString();
                            sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[rowCount, 1, rowCount, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            colcount++;
                        }
                    }

                    int newcol = 0;


                    rowCount++;

                    ///// Data Print 

                    for (int rowcnt = 0; rowcnt < dtgridN.Rows.Count; rowcnt++)
                    {
                        colcount = 1;

                        for (int colcnt = 0; colcnt < dtgridN.Columns.Count; colcnt++)
                        {
                            /* if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() == "FORMFILLED")
                             {
                                 if (dtgridN.Rows[rowcnt][colcnt].ToString() == "0")
                                 {
                                     strFromFilled = "NO";
                                 }
                                 else
                                 {
                                     strFromFilled = "YES";
                                 }
                                 sheet.Cells[rowCount, colcount].Value = strFromFilled;
                                 colcount++;
                             }
                             */

                            if (
                                //dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "ROLENAME" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDBY" &&
                                //dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "FORMFILLED" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDDATE" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "PK_USERID" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MOBILENO" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "TUV_EMAIL_ID" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "EMPLOYEMENTCATEGORY" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "TUVISTAMPNO" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "AUTHARIZELEVEL"
                            )

                            {
                                sheet.Cells[rowCount, colcount].Value = dtgridN.Rows[rowcnt][colcnt].ToString();

                            }
                            else if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() == "AUTHARIZELEVEL")
                            {
                                DataRow[] dr = MasterData.Tables[1].Select("PK_UserID = '" + dtgridN.Rows[rowcnt]["Pk_userID"].ToString().ToUpper().Trim() + "'");
                                newcol = colcount;
                                string[] strArray;
                                string strAuth = string.Empty;
                                // newcol = colcount;

                                if (dr != null && dr.Count() > 0)
                                {
                                    strArray = dr[0]["AUTHARIZELEVEL"].ToString().Split('#');

                                    for (int AuthCol = 0; AuthCol < strArray.Length; AuthCol++)
                                    {
                                        sheet.Cells[rowCount, newcol].Value = strArray[AuthCol].ToString();
                                        newcol++;
                                    }
                                }

                            }
                            colcount++;
                        }

                        rowCount++;
                    }






                }
                catch (Exception ex)
                {
                    string errMsg = ex.Message;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Competency Matrix"+ DateTime.Now.ToShortDateString()+ ".xlsx");
            }




        }

        public DataTable GetData()
        {

            DataSet dsNew = new DataSet();
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.

            dsNew = objCMV.GetDataN(); // fill dataset

            return dsNew.Tables[0];
        }


        public DataSet GetMasterData()
        {

            DataSet dsMasterNew = new DataSet();


            dsMasterNew = objCMV.GetMasterDataN(); // fill dataset  



            return dsMasterNew;
        }

        #endregion






        //create 23-03-2023  shrutika salve//
        //Drop-Down//

        public ActionResult MentorList(int? Br_Id, string UserID)
        {
            try
            {



                DataSet DTUserDashBoard1 = new DataSet();
                List<UNameCode> lstUserList = new List<UNameCode>();
                DTUserDashBoard1 = objDalCreateUser.GetMentorList();

                if (DTUserDashBoard1.Tables[0].Rows.Count > 0)//All Items to be Inspected
                {
                    lstUserList = (from n in DTUserDashBoard1.Tables[0].AsEnumerable()
                                   select new UNameCode()
                                   {
                                       Name = n.Field<string>(DTUserDashBoard1.Tables[0].Columns["FullName"].ToString()),
                                       Code = n.Field<string>(DTUserDashBoard1.Tables[0].Columns["PK_UserID"].ToString())
                                   }).ToList();
                }

                IEnumerable<SelectListItem> Items;
                Items = new SelectList(lstUserList, "Code", "Name");
                ViewBag.Employees = Items;
                ViewData["Employees"] = Items;
                //new code start
                List<Users> lmd = new List<Users>();
                DataSet ds = new DataSet();
                string UserId1 = Session["UserID"].ToString();
                ds = objDalCreateUser.GetDataMentor(UserId1);
                //ds=ds.Tables.MenteesList

                //check dowpdown check value 


                DataSet dc = new DataSet();

               
                //ViewData["EmployeesDetails"] = ds;

                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {

                        lmd.Add(new Users
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ManteeName = Convert.ToString(dr["ManteeName"]),
                            IsMentor = Convert.ToString(dr["IsMentor"]),
                            MentorName = Convert.ToString(dr["MentorName"]),
                            MentorListName = Convert.ToString(dr["MantorList"]),
                            BranchName = Convert.ToString(dr["Branch_Name"])

                        });

                        ViewData["EmployeesDetails"] = lmd;
                        
                    }

                }
                //later on decide to keep it
                // TempData.Keep("Employees");
                //new code end
                if (Br_Id != 0 && Br_Id != null)
                {
                    DataSet DSEditCompany = new DataSet();
                    DSEditCompany = objDalCompany.EditBranch(Br_Id);
                    if (DSEditCompany.Tables[0].Rows.Count > 0)
                    {
                        ObjModelCompany.Br_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["Br_Id"]);
                        ObjModelCompany.Branch_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch_Name"]);
                        ObjModelCompany.Branch_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Branch_Code"]);
                        ObjModelCompany.Manager = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Manager"]);
                        ObjModelCompany.Service_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_Code"]);
                        ObjModelCompany.Sequence_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sequence_Number"]);
                        ObjModelCompany.Address1 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address1"]);
                        ObjModelCompany.Address2 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address2"]);
                        ObjModelCompany.Address3 = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Address3"]);
                        ObjModelCompany.Country = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Country"]);
                        ObjModelCompany.State = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["State"]);
                        ObjModelCompany.Postal_Code = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Postal_Code"]);
                        ObjModelCompany.Email_Id = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Email_Id"]);
                        ObjModelCompany.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                        ObjModelCompany.CityName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CityName"]);

                        ObjModelCompany.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                        ObjModelCompany.Coordinator_Email_Id = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Coordinator_Email_Id"]);

                        ObjModelCompany.BranchAdmin = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["BranchAdmin"]);
                        // ViewData["ListBranchchecked"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Mentor"]);
                    }

                    return View(ObjModelCompany);
                }
                else
                {

                }



                return View(ObjModelUsers);

            }
            catch (Exception e)
            {

            }

            return View();

        }


        // ---------Insert Data----------------//

        [HttpPost]
        public ActionResult MentorList1(Users CM, FormCollection fc)
        {
            DataTable DTExistUsertName = new DataTable();
            string ProList = string.Join(",", fc["BrAuditee"]);
            CM.BrAuditee = ProList;
            var obj = CM.FullName;
            var obj2 = CM.CreatedBy;

            List<Users> lstFileDtls = new List<Users>();
            //Mantor OBJMantor = new Mantor();
            var UserData = objDalCreateUser.InsertMantorList(CM);


            return Redirect("MentorList");
        }




        public ActionResult DeleteDataRecord(int ID)
        {
            int Result = 0;
            try
            {
                if (objDalCreateUser.DeleteDataRecord(ID))
                {
                    //TempData["Result"] = Result;
                    TempData["Deleted"] = " Details Deleted Successfully ...";

                }
                return RedirectToAction("MentorList");
            }
            catch (Exception)
            {
                return View("MentorList");
            }
        }
        //Excel Upload Data//


        public ActionResult ExportIndex1(Users U)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Users> grid = CreateExportableGrid(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                using (ExcelRange range = sheet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Users> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Mentee&Mentorlist-" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
        }
        private IGrid<Users> CreateExportableGrid(Users U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Users> grid = new Grid<Users>(GetData(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.ManteeName).Titled("Employee Name");
            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.IsMentor).Titled("Is Mentor");

            grid.Columns.Add(model => model.MentorName).Titled("Mentor Name");

            grid.Columns.Add(model => model.MentorListName).Titled("Mantee's List");


            grid.Pager = new GridPager<Users>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelUsers.lstCallDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<Users> GetData(Users U)
        {

            DataTable DTCallDashBoard = new DataTable();
            List<Users> lstCallDashBoard = new List<Users>();
            DTCallDashBoard = objDalCreateUser.GetDetails();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Users
                            {

                                Id = Convert.ToInt32(dr["Id"]),
                                ManteeName = Convert.ToString(dr["ManteeName"]),
                                IsMentor = Convert.ToString(dr["IsMentor"]),
                                MentorName = Convert.ToString(dr["MentorName"]),
                                MentorListName = Convert.ToString(dr["MantorList"]),
                                BranchName = Convert.ToString(dr["Branch_Name"])

                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["MentorList"] = lstCallDashBoard;

            ObjModelUsers.lstCallDashBoard1 = lstCallDashBoard;


            return ObjModelUsers.lstCallDashBoard1;
        }


        [HttpGet]
        public ActionResult MonitoringList()
        {

            DataTable DTComplaintDashBoard = new DataTable();
            List<monitors> lstComplaintDashBoard = new List<monitors>();
            //DTComplaintDashBoard = objDAM.GetReportsDashBoard();

            DataTable DTIsMentor = new DataTable();
            string MentorId = Session["UserID"].ToString();
            DTIsMentor = objDAM.GetMentorList(MentorId);
           

            Session["IsMentor"] = DTIsMentor.Rows[0][0].ToString();
            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {
                 obj.UserId = Session["UserID"].ToString();
                obj.FromDate = Convert.ToString(TempData["FromDate"]);
                obj.ToDate = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                DTComplaintDashBoard = objDAM.GetDataByDate(obj);

            }
            else
            {
                string UserId1 = Session["UserID"].ToString();
                DTComplaintDashBoard = objDAM.GetReportsDashBoard(UserId1);
            }
            Session["DTComplaint"] = DTComplaintDashBoard;
            string showData = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    string _TypeOfMonitoring = "";
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        _TypeOfMonitoring = _TypeOfMonitoring + ";" + Convert.ToString(dr["Type_Of_Monitoring"]);

                        lstComplaintDashBoard.Add(
                            new monitors
                            {
                                
                                UINId = Convert.ToString(dr["UIN"]),
                                Inspector_Name = Convert.ToString(dr["InspectorName"]),
                                Monitor_Name = Convert.ToString(dr["Monitor_Name"]),
                                TypeOfmonitoring = Convert.ToString(dr["Type_Of_Monitoring"]),
                                Date = Convert.ToString(dr["date"]),
                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                InspectorComment = Convert.ToString(dr["Inspector_comment"]),
                                Reporting_manager_comments = Convert.ToString(dr["Reporting_manager_comment"]),
                                Q1 = Convert.ToString(dr["Is there any need for additional training?"]),
                                Q2 = Convert.ToString(dr["If yes, mention training topic name"]),
                                Q3 = Convert.ToString(dr["Is inspection activity performed by considering observations made during earlier monitoring?"]),
                                Q4 = Convert.ToString(dr["Observations"]),
                                chkManData = Convert.ToString(dr["ManMonthsAssignment"]),
                                CurrentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                                specialMonitoringdata = Convert.ToString(dr["SpecialMonitoring"]),
                                SpecialServicesData = Convert.ToString(dr["SpecialServices"]),


                            }
                            );
                    }

                    Session["TypeOfMonitoring"] = _TypeOfMonitoring.Substring(1);
                  
                    
                    

                    ViewData["List"] = lstComplaintDashBoard;
                    obj.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    return View(obj);
                }
                else
                {
                    ViewData["List"] = Session["lstComplaintDashBoard"];
                    obj.lstComplaintDashBoard1 = lstComplaintDashBoard;
                   
                    return View(obj);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            //ViewData["ComplaintList"] = lstComplaintDashBoard;
            //CMREegister.lstComplaintDashBoard1 = lstComplaintDashBoard;
            return View();
        }




        [HttpGet]
        public ActionResult Checkbox(string userid)
        {
            DataSet Checkbox = new DataSet();
            string MentorName = Session["UserID"].ToString();
            Checkbox = objDAM.Checkbox(userid);
            string jsonString = "";
            if (Checkbox != null)
            {
                jsonString = JsonConvert.SerializeObject(Checkbox);
            }


            return Json(jsonString, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public ActionResult MonitoringList(monitors CM)
        {
            List<monitors> lmd = new List<monitors>();  // creating list of model.  
            DataSet ds = new DataSet();

            //ds = DALobj.GetDataByDate(LD); // fill dataset  

           
            //Session["FromDate"] = LD.FromD;
            //Session["ToDate"] = LD.ToD;

            TempData["FromDate"] = CM.FromDate;
            TempData["ToDate"] = CM.ToDate;
            TempData.Keep();

          
            return RedirectToAction("MonitoringList");
            return View(obj);




        }


        public ActionResult ViewDash(string UID, String Type)
        {
            if (Type == "Onsite Monitoring")
            {
                //RedirectToAction("MonitoringDetails");
                return RedirectToAction("MonitoringDetails", new { UID = UID });

            }
            if (Type == "Mentoring")
            {
                //Redirect("Mentoring");
                return RedirectToAction("Mentoring", new { UID = UID });
            }
            if (Type == "Offsite Monitoring")
            {
                //Redirect("Mentoring");
                return RedirectToAction("OffSiteMonitoringDetails", new { UID = UID });
            }
            if (Type == "Monitoring of monitors")
            {
                //Redirect("Mentoring");
                return RedirectToAction("MonitoringOfMonitors", new { UID = UID });
            }


            return View();
        }





        [HttpPost]
        public ActionResult ReportData(string ReportNo, String Type)
        {
            var msg = "";
            if (Type == "Onsite Monitoring")
            {
                //RedirectToAction("MonitoringDetails");
                //return RedirectToAction("MonitoringDetails", new { UID = ReportNo });
                 msg = Url.Action("MonitoringDetails", new { UID = ReportNo });

            }
            if (Type == "Mentoring")
            {
                //Redirect("Mentoring");
                //return RedirectToAction("Mentoring", new { UID = ReportNo });
                 msg = Url.Action("Mentoring", new { UID = ReportNo });
            }
            if (Type == "Offsite Monitoring")
            {
                //Redirect("Mentoring");
                //return RedirectToAction("OffSiteMonitoringDetails", new { UID = ReportNo });
             msg = Url.Action("OffSiteMonitoringDetails", new { UID = ReportNo });
            }

            return Json(new { msg });


        }


        //public ActionResult CreateReport(string UIN)
        //{

        //    monitors abc = new monitors();
        //    DataTable DTGetUploadedFile = new DataTable();
        //    List<monitors> lstEditFileDetails = new List<monitors>();
        //    DTGetUploadedFile = objDAM.EditReport(UIN);

        //    if (DTGetUploadedFile.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in DTGetUploadedFile.Rows)
        //        {
        //            lstEditFileDetails.Add(
        //               new monitors
        //               {

        //                   UINId = Convert.ToString(dr["UIN"]),
        //                   Date = Convert.ToString(dr["date"]),
        //                   Inspector_Name = Convert.ToString(dr["InspectorName"]),
        //                   Monitor_Name = Convert.ToString(dr["Monitor_Name"]),
        //                   Scope = Convert.ToString(dr["Scope"]),
        //                   Monitor_level_of_authorisation = Convert.ToString(dr["Monitor_level"]),
        //                   TUVI_control_number = Convert.ToString(dr["TUVI_Control_number"]),
        //                   Customer_Name = Convert.ToString(dr["Customer_Name"]),
        //                   EndCustomerName = Convert.ToString(dr["End_Customer_Name"]),
        //                   ProjectName = Convert.ToString(dr["Project_Name"]),
        //                   Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
        //                   Vendor_Location = Convert.ToString(dr["Vendor_Location"]),
        //                   Item_Inspected = Convert.ToString(dr["Item_Inspected"]),

        //            }


        //               );
        //        }
        //        //ViewData["lstEditFileDetails"] = lstEditFileDetails;
        //        ViewBag.EditCompCategory = lstEditFileDetails;
        //        //obj.ATT1 = lstEditFileDetails;





        //    }


        //      return Redirect("MonitoringDetails");
        //        }


        public ActionResult DeleteData(string UID)
        {
            int Result = 0;
            try
            {
                if (objDAM.DeleteData(UID))
                {
                    TempData["Result"] = Result;
                    return RedirectToAction("MonitoringList");
                    //ViewBag.Delete = "Question Details Deleted Successfully ...";
                }
               
            }
            catch (Exception)
            {
                return View("MonitoringList");
            }
            return View();
        }


        //Onsite Monitor Report//
        [HttpGet]
        public ActionResult MonitoringDetails(string UID)
        {
            Session["Uid"] = UID;
            //string UIN = Session["UIN"].ToString();
            //if(UIN !="")
            //{
            //    UID = UIN;
            //    Session["Uid"] = UID;
            //    Session["UIN"] = null;
            //}
            System.Data.DataSet id = new System.Data.DataSet();
            DataTable DtData = new DataTable();

            id = objDAM.Monitoringid();
            DtData = id.Tables[0];
            obj.monitoringid = id.Tables[0].Rows[0][0].ToString();
            string monitoringid = obj.monitoringid;



            string Result = string.Empty;
            string[] splitedProduct_Name;
            string[] splitedScope;
            int _mins = 1000;
            int _maxs = 9999;
            Random rnd = new Random();
            int myRandomNo = rnd.Next(_mins, _maxs);
            string strmyRandomNo = "ONS" + Convert.ToString(myRandomNo) + monitoringid;
            //string T = "ONS" + Convert.ToString(a);
            obj.UIN = strmyRandomNo;

            string var = Convert.ToString(System.Web.HttpContext.Current.Session["LoginName"]);
            string var1 = Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]);
            
            obj.Monitor_Name = var;


            

            //ViewBag.BranchManger = objDAM.GetBranchManger();
            // var Data = objDAM.GetBranchManger();
            //ViewBag.BranchManger = new SelectList(Data, "BranchManager");

            List<MNameCode> lstScope = new List<MNameCode>();
            DataSet DSEditGetList = new DataSet();
            DSEditGetList = objDAM.GetDdlLst();
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstScope = (from n in DSEditGetList.Tables[0].AsEnumerable()
                            select new MNameCode()
                            {
                                Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ScopeName"].ToString()),
                                Code = n.Field<int>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                            }).ToList();
            }
            IEnumerable<SelectListItem> ScopeItems;
            ScopeItems = new SelectList(lstScope, "Code", "Name");
            ViewBag.Scope = ScopeItems;
            ViewData["Scopes"] = ScopeItems;
            ViewBag.Scope = lstScope;

            
            //Get Branch Details

            DataTable Branchmanger = new DataTable();
            string UserId = Session["UserID"].ToString();
            Branchmanger = objDAM.GetBranchManger(UserId); 

            Session["Branch"] = Branchmanger.Rows[0][0].ToString();





            DataTable UIDNO = new DataTable();
            UIDNO = objDAM.UIDNoCheck(UID);

            Session["UIDNO"] = UIDNO.Rows[0][0].ToString();

            //DataTable Ds = new DataTable();
            //List<monitors> lst = new List<monitors>();

            //Ds = objDAM.GetBranchManger();

            //if (Ds.Rows.Count > 0)
            //{
            //    foreach (DataRow dr in Ds.Rows)
            //    {
            //        lst.Add(new monitors
            //        {

            //            BranchManager = Convert.ToString(dr["BranchManager"]),

            //        }
            //       );

            //    }

            //    ViewBag.BranchManger = lst;
            //    ViewData["BranchManger"] = lst;


            //}

            //end code branch

            DataTable DTGetProductLst = new DataTable();
            List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
            DTGetProductLst = objDAM.getlistforEdit();

            if (DTGetProductLst.Rows.Count > 0)
            {
                lstEditInspector = (from n in DTGetProductLst.AsEnumerable()
                                    select new NameCodeProduct()
                                    {
                                        Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())



                                    }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstEditInspector, "Name", "Name");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = ProductcheckItems;
            ViewData["Drpproduct"] = objDAM.GetDrpList();

            DataTable ds = new DataTable();
            List<monitors> lstCallDashBoard = new List<monitors>();  // creating list of model.  

            ds = objDAM.GetDetails();

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    lstCallDashBoard.Add(new monitors
                    {


                        QuestionNo = Convert.ToString(dr["QuestionNo"]),
                        Question = Convert.ToString(dr["Question"]),
                        OptButton = Convert.ToString(dr["OptButton"]),
                        checkbox = Convert.ToString(dr["Checkbox"]),
                        FreeText = Convert.ToString(dr["FreeTextBox"]),
                        otp= Convert.ToString(dr["otp"]),


                    }
                   );

                }

                ViewBag.QuestionNo = lstCallDashBoard;
                ViewData["QuestionNo"] = lstCallDashBoard;
                obj.lstCallDashBoard1 = lstCallDashBoard;

            }

            //added by shrutika salve 28092023

            DataTable data = new DataTable();

            List<monitors> lstQuestionRate = new List<monitors>();
            data = objDAM.GetDetailsQuestionRate();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    lstQuestionRate.Add(new monitors
                    {


                        Qidrate = Convert.ToString(dr["Qrateid"]),
                        RateQuestion = Convert.ToString(dr["Question"]),



                    }
                   );

                }

                ViewBag.Qidrate = lstQuestionRate;
                ViewData["RateQuestion"] = lstQuestionRate;
                obj.lstQuestionrate = lstQuestionRate;

            }

            //DataEdit//
            Session["Operation"] = "I";
            DataSet DTGetUploaded = new DataSet();
            if (UID != null)
            {
                Session["Operation"] = "U";

                ViewBag.check = "productcheck";
                ViewBag.Jobcheck = "JobCheck";

                

                //string[] splitedProduct_Name;

                DTGetUploaded = DalObj2.EditReport(UID);
                if (DTGetUploaded.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable table in DTGetUploaded.Tables)
                    {
                        obj.Id = Convert.ToInt32(DTGetUploaded.Tables[0].Rows[0]["Id"]);
                        obj.UIN = DTGetUploaded.Tables[0].Rows[0]["UIN"].ToString();
                        obj.pk_inspectionId = DTGetUploaded.Tables[0].Rows[0]["pk_inspectionid"].ToString();//added by satish
                        obj.pk_call_id = DTGetUploaded.Tables[0].Rows[0]["pk_call_id"].ToString();
                        //string var = Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginId"]);
                        //obj.CreatedDate = Convert.ToDateTime(System.Web.HttpContext.Current.Session["Call no"].ToString();
                        obj.Call_No = DTGetUploaded.Tables[0].Rows[0]["Call no"].ToString();
                        obj.Date = DTGetUploaded.Tables[0].Rows[0]["date"].ToString();
                        obj.Inspector_Name = DTGetUploaded.Tables[0].Rows[0]["InspectorName"].ToString();
                        obj.inspectorId = DTGetUploaded.Tables[0].Rows[0]["inspectorId"].ToString();
                        obj.Inspector_Level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Inspector Level of authorisation"].ToString();
                        obj.Scope = DTGetUploaded.Tables[0].Rows[0]["Scope"].ToString();
                        obj.Monitor_Name = DTGetUploaded.Tables[0].Rows[0]["Monitor_Name"].ToString();
                        obj.Monitor_level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Monitor level of authorisation"].ToString();
                        obj.TUVI_control_number = DTGetUploaded.Tables[0].Rows[0]["TUVI Control Number"].ToString();
                        obj.Customer_Name = DTGetUploaded.Tables[0].Rows[0]["Customer Name"].ToString();
                        obj.EndCustomerName = DTGetUploaded.Tables[0].Rows[0]["End Customer Name"].ToString();
                        obj.ProjectName = DTGetUploaded.Tables[0].Rows[0]["Project Name"].ToString();
                        obj.Vendor_Name = DTGetUploaded.Tables[0].Rows[0]["VendorName"].ToString();
                        obj.Vendor_Location = DTGetUploaded.Tables[0].Rows[0]["Vendor Location"].ToString();
                        obj.Item_Inspected = DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"].ToString();
                        obj.on_site_time = DTGetUploaded.Tables[0].Rows[0]["On site Time"].ToString();
                        obj.off_site_time = DTGetUploaded.Tables[0].Rows[0]["Off Site Time"].ToString();
                        obj.travel_time = DTGetUploaded.Tables[0].Rows[0]["TravelTime"].ToString();
                        obj.Reference_Document = DTGetUploaded.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                        obj.Details_of_inspection_activity = DTGetUploaded.Tables[0].Rows[0]["DetailsOfInspectionActivity"].ToString();
                        obj.InspectorComment = DTGetUploaded.Tables[0].Rows[0]["InspectorComment"].ToString();
                        obj.Reporting_manager_comments = DTGetUploaded.Tables[0].Rows[0]["ManagerComments"].ToString();
                        obj.Date_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["TopvendorPODate"]);
                        obj.Sub_Vendor_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorName"]);
                        obj.Po_No_SubVendor = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPONo"]);
                        obj.SubSubVendorDate_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPoDate"]);
                        obj.DEC_PMC_EPC_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECName"]);
                        obj.DEC_PMC_EPC_Assignment_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECNumber"]);
                        obj.Po_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Vendor_Po_No"]); 
                        obj.itemDescription = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["itemDescription"]);
                        //added by shrutika salve 08-06-2023
                        obj.CreatedDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CreatedDate"]);
                        obj.inspectorCommetName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["inspectorCommetName"]);
                        obj.InspectorCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["InspectorCommentDate"]);
                        obj.ManagerCommentName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentName"]);
                        obj.ManagerCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentDate"]);
                        //added by shrutika salve 23042024
                        obj.chkMan = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["ManMonthsAssignment"]);
                        obj.CurrentAssignment = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CurrentAssignment"]);
                        //added by shrutika salve 09052024
                        obj.specialMonitoring = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["specialMonitoring"]);




                        List<string> Selected1 = new List<string>();
                        var Existingins1 = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Scope"]);
                        splitedScope = Existingins1.Split(',');
                        foreach (var single1 in splitedScope)
                        {
                            Selected1.Add(single1);
                        }
                        ViewBag.EditproductName = Selected1;



                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName1 = Selected;



                    }



                    //added by attachement 07092023
                    DataTable DTGetUploadedFilemonitoing = new DataTable();
                    List<FileDetails> lstEditFileDetailsmonitoring = new List<FileDetails>();
                    DTGetUploadedFilemonitoing = DalObj2.EditUploadedFile(UID);
                    if (DTGetUploadedFilemonitoing.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFilemonitoing.Rows)
                        {
                            lstEditFileDetailsmonitoring.Add(
                               new FileDetails
                               {
                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetailsmonitoring;
                        obj.FileDetails = lstEditFileDetailsmonitoring;
                        //IVR.FileDetails = lstEditFileDetails;
                    }

                    DataTable DTGetUploadedFile = new DataTable();
                    List<monitors> lstEditFileDetails = new List<monitors>();
                    DTGetUploadedFile = objDAM.EditReportQUestion(UID);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new monitors
                               {
                                   QuestionNo = Convert.ToString(dr["Qid"]),
                                   Question = Convert.ToString(dr["Question"]),
                                   OptButton = Convert.ToString(dr["OptButton"]),
                                   Ans = Convert.ToString(dr["Ans"]),
                                   checkbox = Convert.ToString(dr["checkbox1"]),
                                   FreeText = Convert.ToString(dr["FreeTextBox"]),
                                   cheboxans = Convert.ToString(dr["checkbox"]),
                                   FreeText1 = Convert.ToString(dr["FreeTextBoxans"]),
                                   otp = Convert.ToString(dr["otp1"]),
                                   Check1 = Convert.ToString(dr["otp2"])

                               }
                             );
                        }

                        ViewData["QuestionNo"] = lstEditFileDetails;
                        obj.lstCallDashBoard1 = lstEditFileDetails;
                    }
                    DataTable DataRate = new DataTable();
                    List<monitors> DataRateing = new List<monitors>();
                    DataRate = objDAM.EditRatingQuetion(UID);
                    if (DataRate.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataRate.Rows)
                        {
                            DataRateing.Add(
                               new monitors
                               {
                                   Qidrate = Convert.ToString(dr["ID"]),
                                   RateQuestion = Convert.ToString(dr["Question"]),
                                   Status = Convert.ToInt32(dr["Ans"]),


                               }
                             );
                        }

                        //ViewData["QuestionNo"] = DataRateing;
                        //obj.lstCallDashBoard1 = DataRateing;
                        ViewBag.Qidrate = DataRateing;
                        ViewData["RateQuestion"] = DataRateing;
                        obj.lstQuestionrate = DataRateing;
                    }


                    //added AVG Value
                    DataSet avgvalue = new DataSet();
                    avgvalue = DalObj2.EditReportavgvalue(UID);
                    if (DTGetUploaded.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable table in avgvalue.Tables)
                        {
                            obj.RatingAvg = Convert.ToInt32(avgvalue.Tables[0].Rows[0]["AvgAnd"]);
                        }
                    }

                    Session["UIN"] = obj;
                    //string Call_No= Convert.ToString(System.Web.HttpContext.Current.Session["Call no"]);
                    return View(obj);
                    //return View(obj);


                }

            }

           

            Session["UIN"] = obj;
            return View(obj);
            //return View(obj);
        }

        
        [HttpGet]
        public JsonResult GetDataByControllNo(string Call_No)
        {
            
            var address = objDAM.GetDataByControllNo(Call_No);
            ViewData["ListBranchchecked"] = address.Item_Inspected;
            ViewBag.ListBranchchecked = address.Item_Inspected;
            return Json(address, JsonRequestBehavior.AllowGet);
        }




        public JsonResult getAutoControlno(string input)
        {


            DataTable DTcontrol = new DataTable();
            List<monitors> clst = new List<monitors>();
            DTcontrol = objDAM.getcontrolNodata();


            try
            {
                if (DTcontrol.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTcontrol.Rows)
                    {
                        clst.Add(
                            new monitors
                            {
                                Call_No = Convert.ToString(dr["Call_No"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            var getcontrolnolst = from n in clst
                                  where n.Call_No.StartsWith(input)
                                  select new
                                  {
                                      n.Call_No
                                  };


            return Json(getcontrolnolst);
        }






        //On site Monitoring Details Post//
        [HttpPost]
        public ActionResult OnMonitoringDetails(monitors CM, FormCollection fc, string UID, NonInspectionActivity R)
        {

            string Result = "";


            string Result1 = string.Empty;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["listJobMasterUploadedFile"] as List<FileDetails>;

            if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
            {
                objCommonControl.SaveFileToPhysicalLocation1(lstFileDtls1, CM.UIN);

                Result1 = DalObj2.InsertFileAttachment(lstFileDtls1, CM.UIN, R.PK_Call_ID);
                Session["listJobMasterUploadedFile"] = null;
            }

            //Session["UIN"] = CM.UIN;
            

            string ProList = string.Join(",", fc["BrAuditee1"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;


          



            // Is Used to Update Existing Record
            if (Session["Uid"] != null)
                {
                    DataTable DTExistUsertName1 = new DataTable();
                    List<monitors> lstCallDashBoard = new List<monitors>();

                    List<monitors> lstFileDtls = new List<monitors>();
                    var User = objDAM.UpdateReport(CM);
                    string Answer = string.Empty;

                    string Qid = string.Empty;

                    Qid = CM.QuestionNo;
                    if (CM.Checkbox1 == true)
                    {
                        string Ab = Convert.ToString(CM.Checkbox1);
                        CM.insertcheckbox = "Ab";
                    }
                if (CM.Check == true)
                {
                    string Ab = Convert.ToString(CM.Check);
                    CM.Check1 = "A";
                }
                if (CM.Check2 == true)
                {
                    string Ab = Convert.ToString(CM.Check2);
                    CM.che2 = "c";
                }
                if (CM.check3 == true)
                {
                    string Ab = Convert.ToString(CM.check3);
                    CM.che3 = "D";
                }
                else
                    {
                        CM.insertcheckbox = null;
                    }

                    foreach (var item in CM.lstCallDashBoard1)
                    {

                        CM.Qid = Convert.ToString(item.QuestionNo);
                        CM.Ans = Convert.ToString(item.Ans);
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    CM.Check1 = null;

                    CM.insertcheckbox = null;


                        if (item.Yes != "" && item.Yes != null)
                        {
                            CM.Ans = Convert.ToString(item.Yes);
                        }
                        else if (item.No != "" && item.No != null)
                        {
                            CM.Ans = Convert.ToString(item.No);
                        }
                        else if (item.NA != "" && item.NA != null)
                        {
                            CM.Ans = Convert.ToString(item.NA);
                        }
                         if (item.FreeText1 != "" && item.FreeText1 != null)
                        {
                            CM.FreeText = Convert.ToString(item.FreeText1);
                        }
                     if (CM.Check == true)
                    {
                        if (CM.Qid == "16a")
                        {
                            CM.Check1 = "1";
                        }
                    }
                    if (CM.Check2 == true)
                    {
                        if (CM.Qid == "16b")
                        {
                            CM.Check1 = "1";
                        }
                    }
                    if (CM.check3 == true)
                    {
                        if (CM.Qid == "16c")
                        {
                            CM.Check1 = "1";
                        }
                    }
                    //else if (item.Check1 != "" && item.Check1 != null)
                    // {
                    //     CM.Check = Convert.ToString(item.Check1);
                    // }
                    // else if (item.Check2 != "" && item.Check2 != null)
                    // {
                    //     CM.Check = Convert.ToString(item.Check2);
                    // }
                     if (CM.Checkbox1 == true)
                        {
                        //CM.cheboxans = Convert.ToString(item.Checkbox1);
                        //if (CM.Qid == "16a")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        //else if (CM.Qid == "16b")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        //else if (CM.Qid == "16c")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        if (CM.Qid == "34")
                        {
                            CM.insertcheckbox = "1";
                        }

                    }

                        var Type = CM.TypeOfmonitoring;
                        Answer = objDAM.UpdateReportQuestion(CM);
                    }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objDAM.updateRatingAnswers(CM);
                }


                TempData["UpdateData"] = " Report Updated Successfully...";
                    return RedirectToAction("MonitoringDetails", new { UID = (Session["Uid"]) });

                }
                // Is Used to Insert New Record
              else
                {
                    Session["UIN"] = CM.UIN;

                    //string ProList = string.Join(",", fc["BrAuditee1"]);
                    //CM.Item_Inspected = ProList;

                    //string itemList = string.Join(",", fc["BrScope"]);
                    //CM.Scope = itemList;

                    string Ans = string.Empty;

                    var UserData = objDAM.InsertDetails(CM);
                    if (CM.Checkbox1 == true)
                    {
                        string Ab = Convert.ToString(CM.Checkbox1);
                        CM.insertcheckbox = "Ab";
                    }
                if (CM.Check == true)
                {
                    string Ab = Convert.ToString(CM.Check);
                    CM.Check1 = "A";
                }
                if (CM.Check2 == true)
                {
                    string Ab = Convert.ToString(CM.Check2);
                    CM.che2 = "c";
                }
                if (CM.check3 == true)
                {
                    string Ab = Convert.ToString(CM.check3);
                    CM.che3 = "D";
                }
                else
                    {
                        CM.insertcheckbox = null;
                    }
                    foreach (var item in CM.lstCallDashBoard1)

                    {

                        CM.Qid = Convert.ToString(item.QuestionNo);
                        CM.Ans = Convert.ToString(item.Ans);
                        CM.FreeText = Convert.ToString(item.FreeText1);
                        CM.Check1  = null;
                        CM.insertcheckbox = null;

                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                  if (CM.Check == true)
                    {
                        if (CM.Qid == "16a")
                        {
                            CM.Check1 = "1";
                        }
                    }
                 if (CM.Check2 == true)
                    {
                        if (CM.Qid == "16b")
                        {
                            CM.Check1 = "1";
                        }
                    }
                  if (CM.check3 == true)
                    {
                        if (CM.Qid == "16c")
                        {
                            CM.Check1 = "1";
                        }
                    }

                  if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    }
                    if (CM.Checkbox1 == true)
                    {

                        //CM.cheboxans = Convert.ToString(item.Checkbox1);

                        //if (CM.Qid == "16a")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        //else if (CM.Qid == "16b")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        //else if (CM.Qid == "16c")
                        //{
                        //    CM.insertcheckbox = "1";
                        //}
                        if (CM.Qid == "34")
                        {
                            CM.insertcheckbox = "1";
                        }
                    }
                    
                    //else if (item.Check1 != "" && item.Check1 != null)
                    //{
                    //    CM.Check = Convert.ToString(item.Check1);
                    //}
                    //else if (item.Check2 != "" && item.Check2 != null)
                    //{
                    //    CM.Check = Convert.ToString(item.Check2);
                    //}
                   

                   

                    Ans = objDAM.AddQuestionsAnswers(CM);
                    }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objDAM.AddRatingAnswers(CM);
                }
                // return Redirect(Session["Call_No"]);
                //string Call_No = Convert.ToString(System.Web.HttpContext.Current.Session["Call_No"]);
                //return View(Session["Call_No"]);
                //Commeneted To Test By Satish Pawar
                //   return RedirectToAction("MonitoringDetails", new{ UID = (Session["UIN"]) });
                TempData["UpdateData"] = " Data Added Successfully!!!!";
                    return RedirectToAction("MonitoringDetails", new { UID = (Session["UIN"].ToString()) });
                }

                //return Redirect(Session["Call_No"]);
                //string Call_No1 = Convert.ToString(System.Web.HttpContext.Current.Session["Call_No"]);
                //return Redirect(Call_No1); new { id = 99 }
                //return RedirectToAction("MonitoringDetails", new{ id = (Session["UIN"]) });


                //Coomeneted BySatish Pawar
                //return RedirectToAction("MonitoringDetails", new { UID = (Session["UIN"]) });
 


        }


        [HttpPost]
           public ActionResult validation(string Onsite,string Offsite,string TravelTime,string monitor,string Date,NonInspectionActivity R)
           {
            var sum = Onsite + Offsite + TravelTime;
           // Date = StDt;
            //R.DateSE = CM.Date;
            DateTime StDt = Convert.ToDateTime(Date);
            R.StartTime = Convert.ToDouble(Onsite);
            R.EndTime = Convert.ToDouble(Offsite);
            R.TravelTime = Convert.ToDouble(TravelTime);
            R.CreatedBy = monitor;


            R.DateSE = StDt.ToString("dd/MM/yyyy");
            //10 Aug
            Double CurrentTotal = Convert.ToDouble(R.StartTime) + Convert.ToDouble(R.EndTime) + Convert.ToDouble(R.TravelTime);

            DataTable DTValidateTT = new DataTable();
            {
                DTValidateTT = objDAM.CheckPreviousActivityWithCallId(R.DateSE, R.CreatedBy);
            }
            if (R.CreatedBy != null)
            {
                DTValidateTT = objDAM.CheckPreviousActivityWithCallId(R.DateSE, R.CreatedBy);
            }
            else
            {
                DTValidateTT = objDAM.CheckPreviousActivity(R.DateSE);
            }
            DTValidateTT = objDAM.CheckPreviousActivity(R.DateSE);

            DataTable DTChkLeave = new DataTable();
            DTChkLeave = objDAM.CheckIfLeavePresent(R.DateSE);
            string msg = "";
            if (DTChkLeave.Rows.Count > 0)
            {
                msg = "Failed_Leave has been added for " + StDt.ToString("dd/MM/yyyy");
                //return RedirectToAction("MonitoringDetails", "CompentencyMetrixView", new { UID = (Session["Uid"]) });
                //return Json(1);
            }
            else
            {
                if (DTValidateTT.Rows.Count > 0)
                {
                    //10 Aug
                    //int PriviousTotal = Convert.ToInt32(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToInt32(DTValidateTT.Rows[0]["TravelTime"]);
                    //int GrandTotal = PriviousTotal + CurrentTotal;
                    Double PriviousTotal = Convert.ToDouble(DTValidateTT.Rows[0]["StartTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["EndTime"]) + Convert.ToDouble(DTValidateTT.Rows[0]["TravelTime"]);

                    Double GrandTotal = PriviousTotal + CurrentTotal;


                    if (GrandTotal > 24)
                    {
                        msg = "Failed_Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                        
                    }
                    else
                    {
                        msg = "Sucess";
                    }
                }
                else if (CurrentTotal > 24)
                {
                   
                    msg = "Failed_Exceeded limit of 24 hours for the day " + StDt.ToString("dd/MM/yyyy");
                    //return RedirectToAction("MonitoringDetails", "CompentencyMetrixView", new { UID = (Session["Uid"]) });
                }
                else
                {
                    msg = "Sucess";
                }

            }

            return Json(msg, JsonRequestBehavior.AllowGet);

        }





        //Offsite monitoring details view//


        public ActionResult OffSiteMonitoringDetails(string UID)
        {
            Session["Uid"] = UID;

            System.Data.DataSet id = new System.Data.DataSet();
            DataTable DtData = new DataTable();

            id = objDAM.Monitoringid();
            DtData = id.Tables[0];
            obj.monitoringid = id.Tables[0].Rows[0][0].ToString();
            string monitoringid = obj.monitoringid;

            string[] splitedProduct_Name;
            string[] splitedScope;
            int _mins = 1000;
            int _maxs = 9999;
            Random rnd = new Random();
            int myRandomNo = rnd.Next(_mins, _maxs);
            string strmyRandomNo = "OFS" + Convert.ToString(myRandomNo) + monitoringid;
            //string T = "ONS" + Convert.ToString(a);
            OFS.UIN = strmyRandomNo;



            string var = Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]);
            OFS.MonitorName = var;


            DataTable UIDNO = new DataTable();
            UIDNO = objDAM.UIDNoCheck(UID);

            Session["UIDNO"] = UIDNO.Rows[0][0].ToString();



            var Data1 = objoff.GetScopeList();

            List<MNameCode> lstScope = new List<MNameCode>();
            DataSet DSEditGetList = new DataSet();
            DSEditGetList = objDAM.GetDdlLst();
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstScope = (from n in DSEditGetList.Tables[0].AsEnumerable()
                            select new MNameCode()
                            {
                                Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ScopeName"].ToString()),
                                Code = n.Field<int>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                            }).ToList();
            }
            IEnumerable<SelectListItem> ScopeItems;
            ScopeItems = new SelectList(lstScope, "Code", "Name");
            ViewBag.Scope = ScopeItems;
            ViewData["Scopes"] = ScopeItems;
            ViewBag.Scope = lstScope;





            DataTable DTGetProductLst = new DataTable();
            List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
            DTGetProductLst = objDAM.getlistforEdit();

            if (DTGetProductLst.Rows.Count > 0)
            {
                lstEditInspector = (from n in DTGetProductLst.AsEnumerable()
                                    select new NameCodeProduct()
                                    {
                                        Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())



                                    }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstEditInspector, "Name", "Name");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = ProductcheckItems;
            ViewData["Drpproduct"] = objDAM.GetDrpList();


            



            DataTable ds = new DataTable();
            List<ModelOffSiteMonitoring> lstCallDashBoard = new List<ModelOffSiteMonitoring>();  // creating list of model.  

            ds = objoff.GetDetails();

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    lstCallDashBoard.Add(new ModelOffSiteMonitoring
                    {


                        QuestionNo = Convert.ToString(dr["QuestionNo"]),
                        Question = Convert.ToString(dr["Question"]),
                        OptButton = Convert.ToString(dr["OptButton"]),
                        checkbox = Convert.ToString(dr["Checkbox"]),
                        FreeText = Convert.ToString(dr["FreeTextBox"]),


                    }
                   );

                }

                ViewBag.QuestionNo = lstCallDashBoard;
                ViewData["QuestionNo2"] = lstCallDashBoard;
                OFS.lstCallDashBoard1 = lstCallDashBoard;

            }
            //added by shrutika salve 28092023

            DataTable data = new DataTable();

            List<ModelOffSiteMonitoring> lstQuestionRate = new List<ModelOffSiteMonitoring>();
            data = objoff.GetDetailsQuestionRate();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    lstQuestionRate.Add(new ModelOffSiteMonitoring
                    {


                        Qidrate = Convert.ToString(dr["Qrateid"]),
                        RateQuestion = Convert.ToString(dr["Question"]),



                    }
                    );

                }

                ViewBag.Qidrate = lstQuestionRate;
                ViewData["RateQuestionoff"] = lstQuestionRate;
                OFS.lstQuestionrate = lstQuestionRate;

            }

            DataTable Branchmanger = new DataTable();
            string UserId = Session["UserID"].ToString();
            Branchmanger = objDAM.GetBranchManger(UserId);

            Session["Branch"] = Branchmanger.Rows[0][0].ToString();




            Session["Operation"] = "I";
            DataSet DTGetUploaded = new DataSet();
            if (UID != null)
            {
                Session["Operation"] = "U";

                ViewBag.check = "productcheck";
                ViewBag.Jobcheck = "JobCheck";

                DTGetUploaded = objoff.EditReport(UID);
                if (DTGetUploaded.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable table in DTGetUploaded.Tables)
                    {
                        OFS.UIN = DTGetUploaded.Tables[0].Rows[0]["UIN"].ToString();
                        OFS.id = Convert.ToInt32(DTGetUploaded.Tables[0].Rows[0]["Id"]);
                        OFS.TUVIcontrolnumber = DTGetUploaded.Tables[0].Rows[0]["TUVI_Controll_Number"].ToString();
                        OFS.Date = DTGetUploaded.Tables[0].Rows[0]["Date"].ToString();
                        OFS.InspectorName = DTGetUploaded.Tables[0].Rows[0]["InspectorName"].ToString();
                        OFS.inspectorId = DTGetUploaded.Tables[0].Rows[0]["inspectorId"].ToString();
                        OFS.InspectrorLevelofauthorisation = DTGetUploaded.Tables[0].Rows[0]["Inspector Level of authorisation"].ToString();
                        OFS.Scope = DTGetUploaded.Tables[0].Rows[0]["Scope"].ToString();
                        OFS.MonitorName = DTGetUploaded.Tables[0].Rows[0]["Monitor_Name"].ToString();
                        OFS.Monitorlevelofauthorisation = DTGetUploaded.Tables[0].Rows[0]["Monitor level of authorisation"].ToString();
                        OFS.TUVIConrolNumber = DTGetUploaded.Tables[0].Rows[0]["TUVI Control Number"].ToString();
                        OFS.CustomerName = DTGetUploaded.Tables[0].Rows[0]["Customer Name"].ToString();
                        OFS.EndCustomerName = DTGetUploaded.Tables[0].Rows[0]["End Customer Name"].ToString();
                        OFS.ProjectName = DTGetUploaded.Tables[0].Rows[0]["Project_Name"].ToString();
                        OFS.VendorName = DTGetUploaded.Tables[0].Rows[0]["VendorName"].ToString();
                        OFS.VendorLocation = DTGetUploaded.Tables[0].Rows[0]["Vendor_Location"].ToString();
                        OFS.Item_Inspected = DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"].ToString();
                        OFS.on_site_time = DTGetUploaded.Tables[0].Rows[0]["On site Time"].ToString();
                        OFS.off_site_time = DTGetUploaded.Tables[0].Rows[0]["Off Site Time"].ToString();
                        OFS.travel_time = DTGetUploaded.Tables[0].Rows[0]["TravelTime"].ToString();
                        OFS.InspectorComment = DTGetUploaded.Tables[0].Rows[0]["InspectorComment"].ToString();
                        OFS.Reporting_manager_comments = DTGetUploaded.Tables[0].Rows[0]["ManagerComments"].ToString();
                        OFS.Date_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["TopvendorPODate"]);
                        OFS.Sub_Vendor_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorName"]);
                        OFS.Po_No_SubVendor = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPONo"]);
                        OFS.SubSubVendorDate_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPoDate"]);
                        OFS.DEC_PMC_EPC_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECName"]);
                        OFS.DEC_PMC_EPC_Assignment_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECNumber"]);
                        OFS.Po_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Vendor_Po_No"]); 
                        OFS.itemDescription = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["itemDescription"]); 
                        OFS.Reference_Document = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ReferenceDocument"]); 
                        OFS.Report = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ReportName"]);
                        //added by shrutika salve 08-06-2023
                        OFS.CreatedDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CreatedDate"]);
                        OFS.inspectorCommetName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["inspectorCommetName"]);
                        OFS.InspectorCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["InspectorCommentDate"]);
                        OFS.ManagerCommentName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentName"]);
                        OFS.ManagerCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentDate"]);
                        //added by shrutika salve 23042024 
                        OFS.chkMan = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["ManMonthsAssignment"]);
                        OFS.CurrentAssignment = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CurrentAssignment"]);

                        List<string> Selected1 = new List<string>();
                        var Existingins1 = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Scope"]);
                        splitedScope = Existingins1.Split(',');
                        foreach (var single1 in splitedScope)
                        {
                            Selected1.Add(single1);
                        }
                        ViewBag.EditproductName = Selected1;



                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName1 = Selected;



                    }

                    //added by attachement 07092023
                    DataTable DTGetUploadedFilemonitoing = new DataTable();
                    List<FileDetails> lstEditFileDetailsmonitoring = new List<FileDetails>();
                    DTGetUploadedFilemonitoing = DalObj2.EditUploadedFile(UID);
                    if (DTGetUploadedFilemonitoing.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFilemonitoing.Rows)
                        {
                            lstEditFileDetailsmonitoring.Add(
                               new FileDetails
                               {
                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetailsmonitoring;
                        OFS.FileDetails = lstEditFileDetailsmonitoring;
                        //IVR.FileDetails = lstEditFileDetails;
                    }


                    DataTable DTGetUploadedFile = new DataTable();
                    List<ModelOffSiteMonitoring> lstEditFileDetails = new List<ModelOffSiteMonitoring>();
                    DTGetUploadedFile = objoff.EditReportQUestion(UID);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new ModelOffSiteMonitoring
                               {
                                   QuestionNo = Convert.ToString(dr["Qid"]),
                                   Question = Convert.ToString(dr["Question"]),
                                   OptButton = Convert.ToString(dr["OptButton"]),
                                   Ans = Convert.ToString(dr["Ans"]),
                                   checkbox = Convert.ToString(dr["checkbox1"]),
                                   FreeText = Convert.ToString(dr["FreeTextBox"]),
                                   cheboxans = Convert.ToString(dr["checkbox"]),
                                   FreeText1 = Convert.ToString(dr["FreeTextBoxans"]),
                                   //Checkbox1 = Convert.ToBoolean(dr["checkbox"])
                               }
                             );
                        }

                        ViewData["QuestionNo2"] = lstEditFileDetails;
                        OFS.lstCallDashBoard1 = lstEditFileDetails;
                    }

                    DataTable DataRate = new DataTable();
                    List<ModelOffSiteMonitoring> DataRateingoff = new List<ModelOffSiteMonitoring>();
                    DataRate = objoff.EditRatingQuetion(UID);
                    if (DataRate.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataRate.Rows)
                        {
                            DataRateingoff.Add(
                               new ModelOffSiteMonitoring
                               {
                                   Qidrate = Convert.ToString(dr["ID"]),
                                   RateQuestion = Convert.ToString(dr["Question"]),
                                   Status = Convert.ToInt32(dr["Ans"]),


                               }
                             );
                        }

                        //ViewData["QuestionNo"] = DataRateing;
                        //obj.lstCallDashBoard1 = DataRateing;
                        ViewBag.Qidrate = DataRateingoff;
                        ViewData["RateQuestionoff"] = DataRateingoff;
                        OFS.lstQuestionrate = DataRateingoff;
                    }

                    DataSet avgvalue = new DataSet();
                    avgvalue = objoff.EditReportavgvalue(UID);
                    if (DTGetUploaded.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable table in avgvalue.Tables)
                        {
                            OFS.RatingAvg = Convert.ToInt32(avgvalue.Tables[0].Rows[0]["AvgAnd"]);
                        }
                    }



                    return View(OFS);


                }


            }



            var ReportData = objoff.GetAllReportList();
            ViewBag.ReportList = new SelectList(ReportData, "ReportName", "ReportName");

            return View(OFS);
        }





      //off site data search 

        [HttpPost]
        public JsonResult GetdataCopy(string Type,String PK_CALL_ID)
        {
            try
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DSJobMasterByQtId = objoff.GetReportListBySubjobno(Type, PK_CALL_ID);
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    OFS.Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date"]);
                    OFS.ProjectName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name"]);
                    OFS.EndCustomerName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["EndUser"]);
                    OFS.TUVIcontrolnumber = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["TUVI_Control_Number"]);
                    OFS.VendorName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["vendor_name"]);
                    OFS.TUVIConrolNumber = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["TUVIControlNumber"]);
                    OFS.Item_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Item_Inspected"]);
                    OFS.Date_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["TopvendorPODate"]);
                    OFS.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorName"]);
                    OFS.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorPONo"]);
                    OFS.SubSubVendorDate_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorPoDate"]);
                    OFS.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DECName"]);
                    OFS.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DECNumber"]);
                    OFS.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Po_No"]);
                    // ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);

                    OFS.VendorLocation = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Location"]);
                    OFS.InspectorName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Inspector_Name"]);
                    OFS.CustomerName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_name"]);
                    OFS.PK_CALL_ID =Convert.ToInt32(PK_CALL_ID);//Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_CALL_ID"]);
                    OFS.inspectorId = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["inspectorId"]);
                    OFS.ReportName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);
                    OFS.chkMan = Convert.ToBoolean(DSJobMasterByQtId.Tables[0].Rows[0]["ManMonthsAssignment"]);


                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json(OFS, JsonRequestBehavior.AllowGet);
        }








        //[HttpGet]
        //public ActionResult Searchdata(string SubJobNo)
        //{

        //    DataTable Reportdashboard = new DataTable();
        //    List<ModelOffSiteMonitoring> lstCompanyDashBoard = new List<ModelOffSiteMonitoring>();
        //    try
        //    {
        //        Reportdashboard = objoff.GetReportListBySubjobno(SubJobNo);

        //        if (Reportdashboard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in Reportdashboard.Rows)
        //            {
        //                lstCompanyDashBoard.Add(
        //                    new ModelOffSiteMonitoring
        //                    {
        //                        ReportName = Convert.ToString(dr["ReportNo"]),
        //                        Date = Convert.ToString(dr["Date"]),
        //                        InspectorName = Convert.ToString(dr["Inspector_Name"]),
        //                        PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
        //                        PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
        //                        ProjectName = Convert.ToString(dr["ProjectName"]),
        //                        VendorName = Convert.ToString(dr["VendorName"]),
        //                        EndCustomerName = Convert.ToString(dr["End_Customer"]),
        //                        TUVIcontrolnumber = Convert.ToString(dr["TUV_Control_Number"]),
        //                        CustomerName = Convert.ToString(dr["Customer_name"])


        //                    }
        //                  );
        //            }
        //        }
        //        return Json(lstCompanyDashBoard, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}





        //Edit data//

        //[HttpPost]
        //public JsonResult GetdataCopyByreportId(string IRNReport)
        //{

        //    DataSet DSJobMasterByQtId = new DataSet();
        //    DataTable DSJobMasterByReportNo = new DataTable();

        //    DSJobMasterByQtId = OBJDALIRN.GetReportByCallid(IRNReport);
        //    if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
        //    {
        //        ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_CALL_ID"]);


        //    }

        //    DSJobMasterByQtId = OBJDALIRN.GetCallId(Convert.ToInt32(ObjModelVisitReport.PK_Call_ID));
        //    if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
        //    {

        //        ObjModelVisitReport.Sap_And_Controle_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sap_And_Controle_No"]);
        //        ObjModelVisitReport.PK_IVR_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_IVR_ID"]);
        //        ObjModelVisitReport.Project_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name_Location"]);
        //        ObjModelVisitReport.Address_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Address_Of_Inspection"]);
        //        ObjModelVisitReport.End_user_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["End_user_Name"]);
        //        ObjModelVisitReport.Vendor_Name_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Name_Location"]);
        //        // ObjModelVisitReport.PK_Call_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
        //        ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);
        //        ObjModelVisitReport.Date_Of_Inspection = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date_Of_Inspection"]);
        //        ObjModelVisitReport.Client_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Client_Name"]);
        //        ObjModelVisitReport.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Name"]);
        //        ObjModelVisitReport.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DEC_PMC_EPC_Assignment_No"]);
        //        ObjModelVisitReport.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No"]);
        //        ObjModelVisitReport.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Sub_Vendor_Name"]);
        //        ObjModelVisitReport.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Po_No_SubVendor"]);
        //        ObjModelVisitReport.Emails_Distribution = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Emails_Distribution"]);
        //        ObjModelVisitReport.client_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["client_Email"]);
        //        ObjModelVisitReport.Vendor_Email = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Email"]);
        //        ObjModelVisitReport.Tuv_Branch = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Tuv_Branch"]);

        //        ObjModelVisitReport.Pending_Activites = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Pending_Activites"]);
        //        ObjModelVisitReport.Areas_Of_Concerns = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Areas_Of_Concerns"]);
        //        ObjModelVisitReport.Non_Conformities_raised = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Non_Conformities_raised"]);
        //        ObjModelVisitReport.Type = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Type"]);
        //        ObjModelVisitReport.Report_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Report_No"]);
        //        ObjModelVisitReport.SubJob_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubJob_No"]);
        //        Session["PK_IVR_ID"] = ObjModelVisitReport.PK_IVR_ID;

        //    }

        //    return Json(ObjModelVisitReport, JsonRequestBehavior.AllowGet);
        //}



        // off side monitoring details insert and update//


        [HttpPost]
        public ActionResult OffMonitoringDetails(ModelOffSiteMonitoring CM, FormCollection fc, string UID)
        {
            string Result1 = string.Empty;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["listJobMasterUploadedFile"] as List<FileDetails>;

            if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
            {
                objCommonControl.SaveFileToPhysicalLocation1(lstFileDtls1, CM.UIN);

                Result1 = DalObj2.InsertFileAttachment(lstFileDtls1, CM.UIN, CM.PK_CALL_ID);
                Session["listJobMasterUploadedFile"] = null;
            }

            if (Session["Uid"] != null)
            {
                DataTable DTExistUsertName1 = new DataTable();
                List<ModelOffSiteMonitoring> lstCallDashBoard = new List<ModelOffSiteMonitoring>();


                string ProList = string.Join(",", fc["BrAuditee1"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;


                List<ModelOffSiteMonitoring> lstFileDtls = new List<ModelOffSiteMonitoring>();
                var User = objoff.UpdateReport(CM);
                string Answer = string.Empty;
                string Result = string.Empty;
                string Qid = string.Empty;

                Qid = CM.QuestionNo;
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }

                foreach (var item in CM.lstCallDashBoard1)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText1 = Convert.ToString(item.FreeText1);
                    CM.insertcheckbox = null;


                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText1 = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "20")
                        {
                            CM.insertcheckbox = "1";
                        }

                    }


                    Answer = objoff.UpdateReportQuestion(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objoff.updateRatingAnswers(CM);
                }
                TempData["UpdateData"] = " Report Updated Successfully...";
                return RedirectToAction("OffSiteMonitoringDetails", new { UID = (Session["Uid"]) });
            }
            else
            {

                Session["UIN"] = CM.UIN;
                string ProList = string.Join(",", fc["BrAuditee1"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;

                string Ans = string.Empty;

                var UserData = objoff.InsertDetails(CM);
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }
                foreach (var item in CM.lstCallDashBoard1)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText1 = Convert.ToString(item.FreeText1);
                    CM.insertcheckbox = null;

                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText1 = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "20")
                        {
                            CM.insertcheckbox = "1";
                        }

                    }

                    Ans = objoff.AddQuestionsAnswers(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objoff.AddRatingAnswers(CM);
                }
                //return RedirectToAction("OffSiteMonitoringDetails");
                TempData["UpdateData"] = " Data Added Successfully!!!!";
                return RedirectToAction("OffSiteMonitoringDetails", new { UID = (Session["UIN"].ToString()) });
            }

            //return RedirectToAction("OffSiteMonitoringDetails");
        }




        [HttpGet]
        public JsonResult GetMentoringTUVControll(string TUVIcontrolnumber)
        {

            var address = objoff.GetDataByControllNo(TUVIcontrolnumber);
            return Json(address, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getAutoTuvControll(string input)
        {


            DataTable DTcontrol = new DataTable();
            List<ModelOffSiteMonitoring> clst = new List<ModelOffSiteMonitoring>();
            DTcontrol = objoff.getcontrolNodata();


            try
            {
                if (DTcontrol.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTcontrol.Rows)
                    {
                        clst.Add(
                            new ModelOffSiteMonitoring
                            {
                                TUVIcontrolnumber = Convert.ToString(dr["sub_job"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            var getcontrolnolst = from n in clst
                                  where n.TUVIcontrolnumber.StartsWith(input)
                                  select new
                                  {
                                      n.TUVIcontrolnumber
                                  };


            return Json(getcontrolnolst);
        }






        [HttpPost]
        public JsonResult GetReportNoForCopy(string SubJobNo, string Type)
        {
            int id = 0;
            string i = "";
            // var Data="";


            DataTable DSJobMasterByQtId = new DataTable();


            //DSJobMasterByQtId = objDalVisitReport.GetReportNoIRN(SubJobNo, VisitReport, Type);

            //if (DSJobMasterByQtId.Rows.Count > 0)
            //{

            //    i = String.Join(",", DSJobMasterByQtId.AsEnumerable().Select(x => x.Field<string>("PK_Call_ID").ToString()).ToArray());

            //}


            var Data = objoff.StringGetReportListForIRNNew(SubJobNo, Type);
            TempData["SearchString"] = Data;

            return Json(Data, JsonRequestBehavior.AllowGet);
        }




        //Mentoring Report Form Start //
        [HttpGet]
        public ActionResult Mentoring(string UID)
        {

            Session["Uid"] = UID;
            System.Data.DataSet id = new System.Data.DataSet();
            DataTable DtData = new DataTable();

            id = objDAM.Monitoringid();
            DtData = id.Tables[0];
            obj.monitoringid = id.Tables[0].Rows[0][0].ToString();
            string monitoringid = obj.monitoringid;

            string[] splitedProduct_Name;
            string[] splitedScope;
            int _mins = 1000;
            int _maxs = 9999;
            Random rnd = new Random();
            int myRandomNo = rnd.Next(_mins, _maxs);
            string strmyRandomNo = "MEN" + Convert.ToString(myRandomNo) + monitoringid;
            //string T = "ONS" + Convert.ToString(a);
            obj2.UIN = strmyRandomNo;

            string var = Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]);
            obj2.Monitor_Name = var;

            DataTable Branchmanger = new DataTable();
            string UserId = Session["UserID"].ToString();
            Branchmanger = objDAM.GetBranchManger(UserId);

            Session["Branch"] = Branchmanger.Rows[0][0].ToString();





            DataTable UIDNO = new DataTable();
            UIDNO = objDAM.UIDNoCheck(UID);

            Session["UIDNO"] = UIDNO.Rows[0][0].ToString();

            List<MNameCode> lstScope = new List<MNameCode>();
            DataSet DSEditGetList = new DataSet();
            DSEditGetList = objDAM.GetDdlLst();
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstScope = (from n in DSEditGetList.Tables[0].AsEnumerable()
                            select new MNameCode()
                            {
                                Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ScopeName"].ToString()),
                                Code = n.Field<int>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                            }).ToList();
            }
            IEnumerable<SelectListItem> ScopeItems;
            ScopeItems = new SelectList(lstScope, "Code", "Name");
            ViewBag.Scope = ScopeItems;
            ViewData["Scopes"] = ScopeItems;
            ViewBag.Scope = lstScope;





            DataTable DTGetProductLst = new DataTable();
            List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
            DTGetProductLst = objDAM.getlistforEdit();

            if (DTGetProductLst.Rows.Count > 0)
            {
                lstEditInspector = (from n in DTGetProductLst.AsEnumerable()
                                    select new NameCodeProduct()
                                    {
                                        Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())



                                    }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstEditInspector, "Name", "Name");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = ProductcheckItems;
            ViewData["Drpproduct"] = objDAM.GetDrpList();

            //Display Question //

            DataTable ds = new DataTable();
            List<Mentoring> lstCallDashBoard = new List<Mentoring>();  // creating list of model.  


            ds = DalObj2.GetMentoringDetailsQue();


            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    lstCallDashBoard.Add(new Mentoring
                    {


                        QuestionNo = Convert.ToString(dr["QuestionNo"]),
                        Question = Convert.ToString(dr["Question"]),
                        OptButton = Convert.ToString(dr["OptButton"]),
                        checkbox = Convert.ToString(dr["Checkbox"]),
                        FreeText = Convert.ToString(dr["FreeTextBox"]),
                        TypeOfMonitoring = Convert.ToString(dr["Type"]),
                    }
                   );

                }

                ViewBag.QuestionNo = lstCallDashBoard;
                ViewData["QuestionNo"] = lstCallDashBoard;
                obj2.lstCallDashBoard = lstCallDashBoard;

            }
            //added by shrutika salve 28092023

            DataTable data = new DataTable();

            List<Mentoring> lstQuestionRate = new List<Mentoring>();
            data = DalObj2.GetDetailsQuestionRate();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    lstQuestionRate.Add(new Mentoring
                    {


                        Qidrate = Convert.ToString(dr["Qrateid"]),
                        RateQuestion = Convert.ToString(dr["Question"]),



                    }
                   );

                }

                ViewBag.Qidrate = lstQuestionRate;
                ViewData["RateQuestionMen"] = lstQuestionRate;
                obj2.lstQuestionrate = lstQuestionRate;

            }
            //Data Edit
            Session["Operation"] = "I";
            DataSet DTGetUploaded = new DataSet();
            if (UID != null)
            {
                Session["Operation"] = "U";
                ViewBag.check = "productcheck";
                ViewBag.Jobcheck = "JobCheck";

                DTGetUploaded = DalObj2.EditReport(UID);
                if (DTGetUploaded.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable table in DTGetUploaded.Tables)
                    {
                        obj2.UIN = DTGetUploaded.Tables[0].Rows[0]["UIN"].ToString();
                        obj2.Id = Convert.ToInt32(DTGetUploaded.Tables[0].Rows[0]["Id"]);
                        obj2.Call_No = DTGetUploaded.Tables[0].Rows[0]["Call no"].ToString();
                        obj2.pk_inspectionId = DTGetUploaded.Tables[0].Rows[0]["pk_inspectionid"].ToString();//added by satish yadav
                        obj2.Date = DTGetUploaded.Tables[0].Rows[0]["date"].ToString();
                        obj2.Inspector_Name = DTGetUploaded.Tables[0].Rows[0]["InspectorName"].ToString();
                        obj2.inspectorId = DTGetUploaded.Tables[0].Rows[0]["inspectorId"].ToString();
                        obj2.Inspector_Level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Inspector Level of authorisation"].ToString();
                        obj2.Scope = DTGetUploaded.Tables[0].Rows[0]["Scope"].ToString();
                        obj2.Monitor_Name = DTGetUploaded.Tables[0].Rows[0]["Monitor_Name"].ToString();
                        obj2.Monitor_level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Monitor level of authorisation"].ToString();
                        obj2.TUVI_control_number = DTGetUploaded.Tables[0].Rows[0]["TUVI Control Number"].ToString();
                        obj2.Customer_Name = DTGetUploaded.Tables[0].Rows[0]["Customer Name"].ToString();
                        obj2.EndCustomerName = DTGetUploaded.Tables[0].Rows[0]["End Customer Name"].ToString();
                        obj2.ProjectName = DTGetUploaded.Tables[0].Rows[0]["Project Name"].ToString();
                        obj2.Vendor_Name = DTGetUploaded.Tables[0].Rows[0]["VendorName"].ToString();
                        obj2.Vendor_Location = DTGetUploaded.Tables[0].Rows[0]["Vendor Location"].ToString();
                        obj2.Item_Inspected = DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"].ToString();
                        obj2.on_site_time = DTGetUploaded.Tables[0].Rows[0]["On site Time"].ToString();
                        obj2.off_site_time = DTGetUploaded.Tables[0].Rows[0]["Off Site Time"].ToString();
                        obj2.travel_time = DTGetUploaded.Tables[0].Rows[0]["TravelTime"].ToString();
                        obj2.Reference_Document = DTGetUploaded.Tables[0].Rows[0]["ReferenceDocument"].ToString();
                        obj2.Details_of_inspection_activity = DTGetUploaded.Tables[0].Rows[0]["DetailsOfInspectionActivity"].ToString();
                        obj2.InspectorComment = DTGetUploaded.Tables[0].Rows[0]["InspectorComment"].ToString();
                        obj2.Reporting_manager_comments = DTGetUploaded.Tables[0].Rows[0]["ManagerComments"].ToString();
                        obj2.Date_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["TopvendorPODate"]);
                        obj2.Sub_Vendor_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorName"]);
                        obj2.Po_No_SubVendor = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPONo"]);
                        obj2.SubSubVendorDate_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPoDate"]);
                        obj2.DEC_PMC_EPC_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECName"]);
                        obj2.DEC_PMC_EPC_Assignment_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECNumber"]);
                        obj2.Po_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Vendor_Po_No"]);
                        obj2.itemDescription = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["itemDescription"]);
                        //added by shrutika salve 08-06-2023
                        obj2.CreatedDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CreatedDate"]);
                        obj2.inspectorCommetName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["inspectorCommetName"]);
                        obj2.InspectorCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["InspectorCommentDate"]);
                        obj2.ManagerCommentName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentName"]);
                        obj2.ManagerCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentDate"]);


                        //added by shrutika salve 23042024
                        obj2.chkMan = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["ManMonthsAssignment"]);

                        obj2.CurrentAssignment = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CurrentAssignment"]);



                        List<string> Selected1 = new List<string>();
                        var Existingins1 = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Scope"]);
                        splitedScope = Existingins1.Split(',');
                        foreach (var single1 in splitedScope)
                        {
                            Selected1.Add(single1);
                        }
                        ViewBag.EditproductName = Selected1;



                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Item_Inspected"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName1 = Selected;




                    }

                    //added by attachement 07092023
                    DataTable DTGetUploadedFilemonitoing = new DataTable();
                    List<FileDetails> lstEditFileDetailsmonitoring = new List<FileDetails>();
                    DTGetUploadedFilemonitoing = DalObj2.EditUploadedFile(UID);
                    if (DTGetUploadedFilemonitoing.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFilemonitoing.Rows)
                        {
                            lstEditFileDetailsmonitoring.Add(
                               new FileDetails
                               {
                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetailsmonitoring;
                        obj2.FileDetails = lstEditFileDetailsmonitoring;
                        //IVR.FileDetails = lstEditFileDetails;
                    }



                    DataTable DTGetUploadedFile = new DataTable();
                    List<Mentoring> lstEditFileDetails = new List<Mentoring>();
                    DTGetUploadedFile = DalObj2.EditReportQUestion(UID);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new Mentoring
                               {
                                   QuestionNo = Convert.ToString(dr["Qid"]),
                                   Question = Convert.ToString(dr["Question"]),
                                   OptButton = Convert.ToString(dr["OptButton"]),
                                   Ans = Convert.ToString(dr["Ans"]),
                                   checkbox = Convert.ToString(dr["checkbox1"]),
                                   FreeText = Convert.ToString(dr["FreeTextBox"]),
                                   cheboxans = Convert.ToString(dr["checkbox"]),
                                   FreeText1 = Convert.ToString(dr["FreeTextBoxans"]),
                                   //Checkbox1 = Convert.ToBoolean(dr["checkbox"])
                               }
                             );
                        }

                        ViewData["QuestionNo"] = lstEditFileDetails;
                        obj2.lstCallDashBoard = lstEditFileDetails;
                    }

                    DataTable DataRate = new DataTable();
                    List<Mentoring> DataRateingoff = new List<Mentoring>();
                    DataRate = DalObj2.EditRatingQuetion(UID);
                    if (DataRate.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataRate.Rows)
                        {
                            DataRateingoff.Add(
                               new Mentoring
                               {
                                   Qidrate = Convert.ToString(dr["ID"]),
                                   RateQuestion = Convert.ToString(dr["Question"]),
                                   Status = Convert.ToInt32(dr["Ans"]),


                               }
                             );
                        }

                        //ViewData["QuestionNo"] = DataRateing;
                        //obj.lstCallDashBoard1 = DataRateing;
                        ViewBag.Qidrate = DataRateingoff;
                        ViewData["RateQuestionMen"] = DataRateingoff;
                        obj2.lstQuestionrate = DataRateingoff;
                    }

                    DataSet avgvalue = new DataSet();
                    avgvalue = DalObj2.EditReportavgvalue(UID);
                    if (DTGetUploaded.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable table in avgvalue.Tables)
                        {
                            obj2.RatingAvg = Convert.ToInt32(avgvalue.Tables[0].Rows[0]["AvgAnd"]);
                        }
                    }
                    return View(obj2);
                }
            }
            return View(obj2);
        }



        //mentoring data search

        [HttpGet]
        public JsonResult GetMentoringCallNo(string Call_No)
        {

            var address = objDAM.GetDataByControllNo(Call_No);
            return Json(address, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetMentoring(string input)
        {
            DataTable DTcontrol = new DataTable();
            List<Mentoring> clst = new List<Mentoring>();
            DTcontrol = DalObj2.getcallNo();


            try
            {
                if (DTcontrol.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTcontrol.Rows)
                    {
                        clst.Add(
                            new Mentoring
                            {
                                Call_No = Convert.ToString(dr["Call_No"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            var getcontrolnolst = from n in clst
                                  where n.Call_No.StartsWith(input)
                                  select new
                                  {
                                      n.Call_No
                                  };


            return Json(getcontrolnolst);
        }

        [HttpPost]
        public ActionResult Mentoring(Mentoring CM, FormCollection fc, string UID)
        {
            string Result1 = string.Empty;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["listJobMasterUploadedFile"] as List<FileDetails>;

            if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
            {
                objCommonControl.SaveFileToPhysicalLocation1(lstFileDtls1, CM.UIN);

                Result1 = DalObj2.InsertFileAttachment(lstFileDtls1, CM.UIN, CM.pk_call_id);
                Session["listJobMasterUploadedFile"] = null;
            }
            if (Session["Uid"] != null)
            {
                DataTable DTExistUsertName1 = new DataTable();
                List<Mentoring> lstCallDashBoard = new List<Mentoring>();


                string ProList = string.Join(",", fc["BrAuditee"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;


                List<Mentoring> lstFileDtls = new List<Mentoring>();
                var User = DalObj2.UpdateReport(CM);
                string Answer = string.Empty;
                string Result = string.Empty;
                string Qid = string.Empty;

                Qid = CM.QuestionNo;
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }

                foreach (var item in CM.lstCallDashBoard)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText = Convert.ToString(item.FreeText1);
                    CM.insertcheckbox = CM.insertcheckbox;


                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "12")
                        {
                            CM.insertcheckbox = "1";
                        }
                        else
                        {
                            CM.insertcheckbox = null;
                        }
                    }

                    var Type = CM.TypeOfMonitoring;
                    Answer = DalObj2.UpdateReportQuestion(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = DalObj2.updateRatingAnswers(CM);
                }
                TempData["UpdateData"] = " Report Updated Successfully...";
                return RedirectToAction("Mentoring", new { UID = (Session["Uid"]) });
            }
            else
            {
                Session["UIN"] = CM.UIN;
                string ProList = string.Join(",", fc["BrAuditee"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;

                string Ans = string.Empty;



                var UserData = DalObj2.InsertDetailsMentoring(CM);
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }
                foreach (var item in CM.lstCallDashBoard)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText = Convert.ToString(item.FreeText1);
                    //    CM.insertcheckbox = Convert.ToString(item.Checkbox1); 

                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "12")
                        {
                            CM.insertcheckbox = "1";
                        }
                        else
                        {
                            CM.insertcheckbox = null;
                        }
                        //string A = Convert.ToString(item.Checkbox1);
                        //CM.insertcheckbox = A;

                    }

                    Ans = DalObj2.AddQuestionsAnswers(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = DalObj2.AddRatingAnswers(CM);
                }
                TempData["UpdateData"] = "Data Added Successfully!!!!";
                return RedirectToAction("Mentoring", new { UID = (Session["UIN"].ToString()) });
            }


            //return RedirectToAction("Mentoring");
        }











        //DataTable DTExistUsertName1 = new DataTable();
        //List<Mentoring> lstQuestion = new List<Mentoring>();

        //var obj = QM.UIN;
        //var obj1 = QM.Call_No;
        //var obj2 = QM.InspectorComment;
        //var obj3 = QM.Monitor_level_of_authorisation;

        //var Manager = QM.Reporting_manager_comments;
        //var Inspector_Level_of_authorisation = QM.Inspector_Level_of_authorisation;
        //var scope = QM.Scope;
        //var Reference_Document = QM.Reference_Document;
        //var details = QM.Details_of_inspection_activity;
        //var on_site_time = QM.on_site_time;
        //var off_site_time = QM.off_site_time;
        //var travel_time = QM.travel_time;


        //string ProList = string.Join(",", fc["BrAuditee"]);
        //QM.Item_Inspected = ProList;

        //string itemList = string.Join(",", fc["BrScope"]);
        //QM.Scope = itemList;



        //List<Mentoring> lstFileDtls = new List<Mentoring>();
        //var UserData = DalObj2.InsertDetailsMentoring(QM);
        //string Answer = string.Empty;
        //string Result = string.Empty;
        //string Qid = string.Empty;

        //Qid = QM.QuestionNo;


        //foreach (var item in QM.Questionlist)
        //{

        //    QM.TypeOfMonitoring = Convert.ToString(item.TypeOfMonitoring);
        //    QM.Qid = Convert.ToString(item.QuestionNo);
        //    QM.Ans = Convert.ToString(item.Ans);
        //    QM.FreeText = Convert.ToString(item.FreeText1);
        //    QM.insertcheckbox = null;



        //    if (item.Yes != "" && item.Yes != null)
        //    {
        //        QM.Ans = Convert.ToString(item.Yes);
        //    }
        //    else if (item.No != "" && item.No != null)
        //    {
        //        QM.Ans = Convert.ToString(item.No);
        //    }
        //    else if (item.NA != "" && item.NA != null)
        //    {
        //        QM.Ans = Convert.ToString(item.NA);
        //    }
        //    else if (item.FreeText1 != "" && item.FreeText1 != null)
        //    {
        //        QM.FreeText = Convert.ToString(item.FreeText1);
        //    }
        //    else if (item.Checkbox1 == true && item.Checkbox1 != null)
        //    {
        //        string A = Convert.ToString(item.Checkbox1);
        //        QM.insertcheckbox = A;

        //    }
        //    var Type = QM.TypeOfMonitoring;
        //    Answer = DalObj2.AddQuestionsAnswers(QM);
        //}







        // MonitoringOfMonitors Report 19042023

        [HttpGet]
        public ActionResult MonitoringOfMonitors(string UID)
        {
            Session["Uid"] = UID;

            System.Data.DataSet id = new System.Data.DataSet();
            DataTable DtData = new DataTable();

            id = objDAM.Monitoringid();
            DtData = id.Tables[0];
            obj.monitoringid = id.Tables[0].Rows[0][0].ToString();
            string monitoringid = obj.monitoringid;

            string[] splitedProduct_Name;
            string[] splitedScope;
            int _mins = 1000;
            int _maxs = 9999;
            Random rnd = new Random();
            int myRandomNo = rnd.Next(_mins, _maxs);
            string strmyRandomNo = "MOM" + Convert.ToString(myRandomNo)+ monitoringid;
            //string T = "ONS" + Convert.ToString(a);
            Obj3.UIN = strmyRandomNo;

            string var = Convert.ToString(System.Web.HttpContext.Current.Session["fullName"]);
            Obj3.Monitor_Name = var;
            //var Data1 = objDalCreateUser.GetScopeList();
            //ViewBag.SubCatlist = new SelectList(Data1, "PK_IAFScopeId", "IAFScopeName");



            DataTable Branchmanger = new DataTable();
            string UserId = Session["UserID"].ToString();
            Branchmanger = objDAM.GetBranchManger(UserId);

            Session["Branch"] = Branchmanger.Rows[0][0].ToString();


            var lst = "";
            ActionResult result = MonitoringList();
            Session["lstComplaintDashBoard"] = lst;




            List<MNameCode> lstScope = new List<MNameCode>();
            DataSet DSEditGetList = new DataSet();
            DSEditGetList = objDAM.GetDdlLst();
            if (DSEditGetList.Tables[0].Rows.Count > 0)
            {
                lstScope = (from n in DSEditGetList.Tables[0].AsEnumerable()
                            select new MNameCode()
                            {
                                Name = n.Field<string>(DSEditGetList.Tables[0].Columns["ScopeName"].ToString()),
                                Code = n.Field<int>(DSEditGetList.Tables[0].Columns["PK_ID"].ToString())

                            }).ToList();
            }
            IEnumerable<SelectListItem> ScopeItems;
            ScopeItems = new SelectList(lstScope, "Code", "Name");
            ViewBag.Scope = ScopeItems;
            ViewData["Scopes"] = ScopeItems;
            ViewBag.Scope = lstScope;





            DataTable DTGetProductLst = new DataTable();
            List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
            DTGetProductLst = objDAM.getlistforEdit();

            if (DTGetProductLst.Rows.Count > 0)
            {
                lstEditInspector = (from n in DTGetProductLst.AsEnumerable()
                                    select new NameCodeProduct()
                                    {
                                        Name = n.Field<string>(DTGetProductLst.Columns["Name"].ToString())



                                    }).ToList();
            }

            IEnumerable<SelectListItem> ProductcheckItems;
            ProductcheckItems = new SelectList(lstEditInspector, "Name", "Name");
            ViewBag.ProjectTypeItems = ProductcheckItems;
            ViewData["ProjectTypeItems"] = ProductcheckItems;
            ViewData["Drpproduct"] = objDAM.GetDrpList();

            DataTable UIDNO = new DataTable();
            UIDNO = objDAM.UIDNoCheck(UID);

            Session["UIDNO"] = UIDNO.Rows[0][0].ToString();


            //Display Question //

            DataTable ds = new DataTable();
            List<MonitoringOfMonitors> lstCallDashBoard = new List<MonitoringOfMonitors>();  // creating list of model.  


            ds = objMOM.GetMentoringDetailsQue();


            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows)
                {
                    lstCallDashBoard.Add(new MonitoringOfMonitors
                    {


                        QuestionNo = Convert.ToString(dr["QuestionNo"]),
                        Question = Convert.ToString(dr["Question"]),
                        OptButton = Convert.ToString(dr["OptButton"]),
                        checkbox = Convert.ToString(dr["Checkbox"]),
                        FreeText = Convert.ToString(dr["FreeTextBox"]),

                    }
                   );

                }

                ViewBag.QuestionNo3 = lstCallDashBoard;
                ViewData["QuestionNo4"] = lstCallDashBoard;
                Obj3.lstCallDashBoard4 = lstCallDashBoard;
            }
            //added by shrutika salve 28092023

            DataTable data = new DataTable();

            List<MonitoringOfMonitors> lstQuestionRate = new List<MonitoringOfMonitors>();
            data = objMOM.GetDetailsQuestionRate();

            if (data.Rows.Count > 0)
            {
                foreach (DataRow dr in data.Rows)
                {
                    lstQuestionRate.Add(new MonitoringOfMonitors
                    {


                        Qidrate = Convert.ToString(dr["Qrateid"]),
                        RateQuestion = Convert.ToString(dr["Question"]),



                    }
                   );

                }

                ViewBag.Qidrate = lstQuestionRate;
                ViewData["RateQuestionMom"] = lstQuestionRate;
                Obj3.lstQuestionrate = lstQuestionRate;

            }


            Session["Operation"] = "I";
            DataSet DTGetUploaded = new DataSet();
            if (UID != null)
            {
                Session["Operation"] = "U";
                ViewBag.check = "productcheck";
                ViewBag.check1 = "productcheck1";

                DTGetUploaded = objMOM.EditReport(UID);
                if (DTGetUploaded.Tables[0].Rows.Count > 0)
                {
                    foreach (DataTable table in DTGetUploaded.Tables)
                    {
                        Obj3.Id = Convert.ToInt32(DTGetUploaded.Tables[0].Rows[0]["Id"]);
                        Obj3.UIN = DTGetUploaded.Tables[0].Rows[0]["UIN"].ToString();
                        Obj3.Report_No = DTGetUploaded.Tables[0].Rows[0]["Call_no"].ToString();
                        Obj3.Date = DTGetUploaded.Tables[0].Rows[0]["date"].ToString();
                        Obj3.Monitor_Name = DTGetUploaded.Tables[0].Rows[0]["Monitor_Name"].ToString();
                        Obj3.Inspector_Name = DTGetUploaded.Tables[0].Rows[0]["MonitorNameMOMReport"].ToString();
                        Obj3.MonitorinspectorName = DTGetUploaded.Tables[0].Rows[0]["inspector"].ToString();
                        
                        Obj3.inspectorId = DTGetUploaded.Tables[0].Rows[0]["inspectorId"].ToString();
                        Obj3.Inspector_Level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Inspecto_level"].ToString();
                        Obj3.Scope = DTGetUploaded.Tables[0].Rows[0]["Scope"].ToString();
                        
                        Obj3.Monitor_level_of_authorisation = DTGetUploaded.Tables[0].Rows[0]["Monitor_level"].ToString();
                        Obj3.TUVI_control_number = DTGetUploaded.Tables[0].Rows[0]["TUVI Control Number"].ToString();
                        Obj3.Customer_Name = DTGetUploaded.Tables[0].Rows[0]["Customer_name"].ToString();
                        Obj3.EndCustomerName = DTGetUploaded.Tables[0].Rows[0]["End Customer Name"].ToString();
                        Obj3.ProjectName = DTGetUploaded.Tables[0].Rows[0]["Project_Name"].ToString();
                        Obj3.Vendor_Name = DTGetUploaded.Tables[0].Rows[0]["VendorName"].ToString();
                        Obj3.Vendor_Location = DTGetUploaded.Tables[0].Rows[0]["Vendor_Location"].ToString();
                        Obj3.Item_Inspected = DTGetUploaded.Tables[0].Rows[0]["ItemInspected"].ToString();
                        Obj3.on_site_time = DTGetUploaded.Tables[0].Rows[0]["OnSiteTime"].ToString();
                        Obj3.off_site_time = DTGetUploaded.Tables[0].Rows[0]["OffSiteTime"].ToString();
                        Obj3.travel_time = DTGetUploaded.Tables[0].Rows[0]["TravelTime"].ToString();
                        Obj3.itemDescription = DTGetUploaded.Tables[0].Rows[0]["itemDescription"].ToString();
                        Obj3.InspectorComment = DTGetUploaded.Tables[0].Rows[0]["InspectorComment"].ToString();
                        Obj3.Reporting_manager_comments = DTGetUploaded.Tables[0].Rows[0]["ManagerComments"].ToString();

                        Obj3.Date_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["TopvendorPODate"]);
                        Obj3.Sub_Vendor_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorName"]);
                        Obj3.Po_No_SubVendor = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPONo"]);
                        Obj3.SubSubVendorDate_of_PO = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["SubVendorPoDate"]);
                        Obj3.DEC_PMC_EPC_Name = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECName"]);
                        Obj3.DEC_PMC_EPC_Assignment_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["DECNumber"]);
                        Obj3.Po_No = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Vendor_Po_No"]);
                        Obj3.ReportNo = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ReportNo"]);
                        Obj3.InspectorLevelauthorisation = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Inspecto_level"]);

                        //added by shrutika salve 08-06-2023
                        Obj3.CreatedDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CreatedDate"]);
                        Obj3.inspectorCommetName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["inspectorCommetName"]);
                        Obj3.InspectorCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["InspectorCommentDate"]);
                        Obj3.ManagerCommentName = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentName"]);
                        Obj3.ManagerCommentDate = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ManagerCommentDate"]);
                        //added by shrutika salve 23042024
                        Obj3.chkMan = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["ManMonthsAssignment"]);
                        Obj3.CurrentAssignment = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["CurrentAssignment"]);
                        Obj3.specialMonitoring = Convert.ToBoolean(DTGetUploaded.Tables[0].Rows[0]["specialMonitoring"]);



                        List<string> Selected1 = new List<string>();
                        var Existingins1 = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["Scope"]);
                        splitedProduct_Name = Existingins1.Split(',');
                        foreach (var single1 in splitedProduct_Name)
                        {
                            Selected1.Add(single1);
                        }
                        ViewBag.EditproductName = Selected1;



                        List<string> Selected = new List<string>();
                        var Existingins = Convert.ToString(DTGetUploaded.Tables[0].Rows[0]["ItemInspected"]);
                        splitedProduct_Name = Existingins.Split(',');
                        foreach (var single in splitedProduct_Name)
                        {
                            Selected.Add(single);
                        }
                        ViewBag.EditproductName1 = Selected;



                    }

                    //added by attachement 07092023
                    DataTable DTGetUploadedFilemonitoing = new DataTable();
                    List<FileDetails> lstEditFileDetailsmonitoring = new List<FileDetails>();
                    DTGetUploadedFilemonitoing = DalObj2.EditUploadedFile(UID);
                    if (DTGetUploadedFilemonitoing.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFilemonitoing.Rows)
                        {
                            lstEditFileDetailsmonitoring.Add(
                               new FileDetails
                               {
                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetailsmonitoring;
                        Obj3.FileDetails = lstEditFileDetailsmonitoring;
                        //IVR.FileDetails = lstEditFileDetails;
                    }

                    DataTable DTGetUploadedFile = new DataTable();
                    List<MonitoringOfMonitors> lstEditFileDetails = new List<MonitoringOfMonitors>();
                    DTGetUploadedFile = objMOM.EditReportQUestion(UID);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new MonitoringOfMonitors
                               {
                                   QuestionNo = Convert.ToString(dr["Qid"]),
                                   Question = Convert.ToString(dr["Question"]),
                                   OptButton = Convert.ToString(dr["OptButton"]),
                                   Ans = Convert.ToString(dr["Ans"]),
                                   checkbox = Convert.ToString(dr["checkbox1"]),
                                   FreeText = Convert.ToString(dr["FreeTextBox"]),
                                   cheboxans = Convert.ToString(dr["checkbox"]),
                                   FreeText1 = Convert.ToString(dr["FreeTextBoxans"]),
                                   //Checkbox1 = Convert.ToBoolean(dr["checkbox"])
                               }
                             );
                        }

                        ViewData["QuestionNo4"] = lstEditFileDetails;
                        Obj3.lstCallDashBoard4 = lstEditFileDetails;
                    }

                    DataTable DataRate = new DataTable();
                    List<MonitoringOfMonitors> DataRateingoff = new List<MonitoringOfMonitors>();
                    DataRate = objMOM.EditRatingQuetion(UID);
                    if (DataRate.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataRate.Rows)
                        {
                            DataRateingoff.Add(
                               new MonitoringOfMonitors
                               {
                                   Qidrate = Convert.ToString(dr["ID"]),
                                   RateQuestion = Convert.ToString(dr["Question"]),
                                   Status = Convert.ToInt32(dr["Ans"]),


                               }
                             );
                        }

                        //ViewData["QuestionNo"] = DataRateing;
                        //obj.lstCallDashBoard1 = DataRateing;
                        ViewBag.Qidrate = DataRateingoff;
                        ViewData["RateQuestionMen"] = DataRateingoff;
                        Obj3.lstQuestionrate = DataRateingoff;
                    }

                    DataSet avgvalue = new DataSet();
                    avgvalue = objMOM.EditReportavgvalue(UID);
                    if (DTGetUploaded.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataTable table in avgvalue.Tables)
                        {
                            Obj3.RatingAvg = Convert.ToInt32(avgvalue.Tables[0].Rows[0]["AvgAnd"]);
                        }
                    }
                    return View(Obj3);
                }

            }

            return View(Obj3);
        }


        [HttpPost]
        public JsonResult GetdataMonitoringofmonitor(string Report_No)
        {
            try
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DSJobMasterByQtId = objMOM.GetReportList(Report_No);
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    Obj3.PK_CALL_ID = Convert.ToInt32(DSJobMasterByQtId.Tables[0].Rows[0]["PK_Call_ID"]);
                   
                    Obj3.Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Date"]);

                    Obj3.ProjectName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Project_Name"]);
                    Obj3.EndCustomerName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["EndUser"]);
                    Obj3.TUVI_control_number = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["TUVI Control Number"]);
                    Obj3.Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["VendorName"]);
                   
                    Obj3.Item_Inspected = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ItemInspected"]);
                    Obj3.inspectorId = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["inspectorId"]);
                    Obj3.Inspector_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Monitor_Name"]);
                    Obj3.Scope = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Scope"]);
                    
                    //Obj3ObjModelVisitReport.Notification_Name_No_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Notification_Name_No_Date"]);

                    Obj3.Vendor_Location = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Location"]);
                    Obj3.MonitorinspectorName = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["InspectorName"]);
                    Obj3.Customer_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_name"]);

                    Obj3.Date_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["TopvendorPODate"]);
                    Obj3.Sub_Vendor_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorName"]);
                    Obj3.Po_No_SubVendor = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorPONo"]);
                    Obj3.SubSubVendorDate_of_PO = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["SubVendorPoDate"]);
                    Obj3.DEC_PMC_EPC_Name = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DECName"]);
                    Obj3.DEC_PMC_EPC_Assignment_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["DECNumber"]);
                    Obj3.Po_No = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Vendor_Po_No"]);
                    Obj3.ReportNo = Convert.ToString(Report_No);
                    Obj3.InspectorLevelauthorisation = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Inspecto_level"]);

                    //added by shrutika salve 23042024
                    Obj3.chkMan = Convert.ToBoolean(DSJobMasterByQtId.Tables[0].Rows[0]["ManMonthsAssignment"]);



                }
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json(Obj3, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult MonitoringOfMonitoring(MonitoringOfMonitors CM, FormCollection fc, string UID)
        {
            string Result1 = string.Empty;
            List<FileDetails> lstFileDtls1 = new List<FileDetails>();
            lstFileDtls1 = Session["listJobMasterUploadedFile"] as List<FileDetails>;

            if (lstFileDtls1 != null && lstFileDtls1.Count > 0)
            {
                objCommonControl.SaveFileToPhysicalLocation1(lstFileDtls1, CM.UIN);

                Result1 = DalObj2.InsertFileAttachment(lstFileDtls1, CM.UIN, CM.PK_CALL_ID);
                Session["listJobMasterUploadedFile"] = null;
            }


            if (Session["Uid"] != null)
            {
                DataTable DTExistUsertName1 = new DataTable();
                List<MonitoringOfMonitors> lstCallDashBoard = new List<MonitoringOfMonitors>();


                string ProList = string.Join(",", fc["BrAuditee1"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;


                List<MonitoringOfMonitors> lstFileDtls = new List<MonitoringOfMonitors>();
                var User = objMOM.UpdateReport(CM);
                string Answer = string.Empty;
                string Result = string.Empty;
                string Qid = string.Empty;

                Qid = CM.QuestionNo;
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }

                foreach (var item in CM.lstCallDashBoard4)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText = Convert.ToString(item.FreeText1);
                    CM.insertcheckbox = null;


                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "12")
                        {
                            CM.insertcheckbox = "1";
                        }
                        

                    }

                    var Type = CM.TypeOfmonitoring;
                    Answer = objMOM.UpdateReportQuestion(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objMOM.updateRatingAnswers(CM);
                }
                TempData["UpdateData"] = "Report Updated Successfully...";
                return RedirectToAction("MonitoringOfMonitors", new { UID = (Session["Uid"]) });
            }
            else
            {
                Session["UIN"] = CM.UIN;
                string ProList = string.Join(",", fc["BrAuditee1"]);
                CM.Item_Inspected = ProList;

                string itemList = string.Join(",", fc["BrScope"]);
                CM.Scope = itemList;

                string Ans = string.Empty;

                var UserData = objMOM.InsertDetails(CM);
                if (CM.Checkbox1 == true)
                {
                    string Ab = Convert.ToString(CM.Checkbox1);
                    CM.insertcheckbox = "Ab";
                }
                else
                {
                    CM.insertcheckbox = null;
                }
                foreach (var item in CM.lstCallDashBoard4)
                {

                    CM.Qid = Convert.ToString(item.QuestionNo);
                    CM.Ans = Convert.ToString(item.Ans);
                    CM.FreeText = Convert.ToString(item.FreeText1);
                    CM.insertcheckbox = null;

                    if (item.Yes != "" && item.Yes != null)
                    {
                        CM.Ans = Convert.ToString(item.Yes);
                    }
                    else if (item.No != "" && item.No != null)
                    {
                        CM.Ans = Convert.ToString(item.No);
                    }
                    else if (item.NA != "" && item.NA != null)
                    {
                        CM.Ans = Convert.ToString(item.NA);
                    }
                    else if (item.FreeText1 != "" && item.FreeText1 != null)
                    {
                        CM.FreeText = Convert.ToString(item.FreeText1);
                    }
                    else if (CM.Checkbox1 == true)
                    {
                        if (CM.Qid == "12")
                        {
                            CM.insertcheckbox = "1";
                        }
                       
                    }

                    Ans = objMOM.AddQuestionsAnswers(CM);
                }
                foreach (var Data in CM.lstQuestionrate)
                {

                    CM.Qidrate = Convert.ToString(Data.Qidrate);
                    CM.Status = Convert.ToInt32(Data.Status);
                    if (CM.Status != null)
                    {
                        CM.rating = CM.Status;
                    }
                    var Rating = objMOM.AddRatingAnswers(CM);
                }
                TempData["UpdateData"] = " Data Added Successfully!!!!";
                return RedirectToAction("MonitoringOfMonitors", new { UID = (Session["UIN"].ToString()) });
            }

            
        }


        //[HttpGet]
        //public ActionResult MonitoringRecords()


        //{
        //    try
        //    {
        //        DataTable DTComplaintDashBoard = new DataTable();
        //        List<MonitorRecordData> lstMonitorrecord = new List<MonitorRecordData>();
        //        //     DTComplaintDashBoard = monitoringrecord.GetMonitorrecord();

        //        if (TempData["FromDate"] != null && TempData["ToDate"] != null)
        //        {
        //            MonitorRecordData.FromDate = Convert.ToString(TempData["FromDate"]);
        //            MonitorRecordData.ToDate = Convert.ToString(TempData["ToDate"]);
        //            TempData.Keep();
        //            DTComplaintDashBoard = MonitoringRecord.GetDataByDate(MonitorRecordData);

        //        }
        //        else
        //        {
        //            string UserId1 = Session["UserID"].ToString();
        //            //    DTComplaintDashBoard = monitoringrecord.GetMonitorrecord();
        //        }

        //        if (DTComplaintDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTComplaintDashBoard.Rows)
        //            {
        //                lstMonitorrecord.Add(new MonitorRecordData
        //                {

        //                    Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
        //                    EmployeeName = Convert.ToString(dr["EmployeeName"]),
        //                    Brach_Name = Convert.ToString(dr["Branch_Name"]),
        //                    MobileNo = Convert.ToString(dr["MobileNo"]),
        //                    Designation = Convert.ToString(dr["Designation"]),
        //                    IsMentor = Convert.ToString(dr["IsMentor"]),
        //                    Mentoring = Convert.ToString(dr["Mentoring"]),
        //                    MonitoringOfmonitors = Convert.ToString(dr["Monitoring of monitors"]),
        //                    OffsiteMonitoring = Convert.ToString(dr["Offsite Monitoring"]),
        //                    OnsiteMonitoring = Convert.ToString(dr["Onsite Monitoring"]),

        //                }
        //               );
        //                ViewData["lstmonitorecord"] = lstMonitorrecord;
        //                MonitorRecordData.listingmonitoringrecord = lstMonitorrecord;
        //                //string lstMonitor = lstMonitorrecord.ToString();
        //                //string[] separatedList = lstMonitor.Split(",");
        //                //foreach (string item in separatedList)
        //                //{
        //                //    Console.WriteLine(item);
        //                //}

        //            }

        //        }


        //        MonitorRecordData.listingmonitoringrecord = lstMonitorrecord;

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return View(MonitorRecordData);
        //}

        //[HttpPost]
        //public ActionResult MonitoringRecords(MonitorRecordData CM)
        //{
        //    List<monitors> lmd = new List<monitors>();  // creating list of model.  
        //    DataSet ds = new DataSet();

        //    TempData["FromDate"] = CM.FromDate;
        //    TempData["ToDate"] = CM.ToDate;
        //    TempData.Keep();
        //    return RedirectToAction("MonitoringRecords");
        //    return View(MonitoringRecord);




        //}


        //added by nikita on 13092023
        [HttpGet]
        public ActionResult MonitoringRecords()


        {
            try
            {

                Session["GetExcelData"] = "Yes";
                List<MonitorRecordData> lstMonitoringRecord = new List<MonitorRecordData>();  // creating list of model.  
                lstMonitoringRecord = MonitoringRecord.GetMonitorrecord();
                ViewData["EnquiryMaster"] = lstMonitoringRecord;
                MonitorRecordData.listingmonitoringrecord = lstMonitoringRecord;
                return View(MonitorRecordData);


            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(MonitorRecordData.listingmonitoringrecord);
        }

        [HttpPost]
        public ActionResult MonitoringRecords(string FromDate, string ToDate)
        {

            try
            {
                DataTable DTComplaintDashBoard = new DataTable();
                List<MonitorRecordData> lstMonitorrecord = new List<MonitorRecordData>();
                //     DTComplaintDashBoard = monitoringrecord.GetMonitorrecord();
                Session["FromDate"] = FromDate;
                Session["ToDate"] = ToDate;

                if (Session["FromDate"] != null && Session["ToDate"] != null)
                {
                    MonitorRecordData.FromDate = Convert.ToString(TempData["FromDate"]);
                    MonitorRecordData.ToDate = Convert.ToString(TempData["ToDate"]);
                    TempData.Keep();
                    DTComplaintDashBoard = MonitoringRecord.GetDataByDate(FromDate, ToDate);

                }
                else
                {
                    string UserId1 = Session["UserID"].ToString();
                    //  DTComplaintDashBoard = MonitoringRecord.GetMonitorrecord();
                }

                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstMonitorrecord.Add(new MonitorRecordData
                        {

                            Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                            EmployeeName = Convert.ToString(dr["EmployeeName"]),
                            Brach_Name = Convert.ToString(dr["Branch_Name"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            IsMentor = Convert.ToString(dr["IsMentor"]),
                            Mentoring = Convert.ToString(dr["Mentoring"]),
                            MonitoringOfmonitors = Convert.ToString(dr["Monitoring of monitors"]),
                            OffsiteMonitoring = Convert.ToString(dr["Offsite Monitoring"]),
                            OnsiteMonitoring = Convert.ToString(dr["Onsite Monitoring"]),
                            Mentoring_Count = Convert.ToInt32(dr["Mentoring_Count"]),
                            Monitoring_of_monitors_Count = Convert.ToInt32(dr["Monitoring_of_monitors_Count"]),
                            Offsite_Monitoring_Count = Convert.ToInt32(dr["Offsite_Monitoring_Count"]),
                            Onsite_Monitoring_Count = Convert.ToInt32(dr["Onsite_Monitoring_Count"]),
                        }
                       );
                        ViewData["lstmonitorecord"] = lstMonitorrecord;
                        MonitorRecordData.listingmonitoringrecord = lstMonitorrecord;


                    }

                }


                MonitorRecordData.listingmonitoringrecord = lstMonitorrecord;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(MonitorRecordData);



        }

        public ActionResult ExportIndex2(monitors U)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<monitors> grid = CreateExportableGrid1(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                using (ExcelRange range = sheet.Cells["A1:K1"])
                {
                    range.Style.Font.Bold = true;

                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<monitors> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Monitoringlist_" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
        }
        private IGrid<monitors> CreateExportableGrid1(monitors U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<monitors> grid = new Grid<monitors>(GetData1(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.UINId).Titled("UIN");
            grid.Columns.Add(model => model.Inspector_Name).Titled("Inspector Name");
            grid.Columns.Add(model => model.SpecialServicesData).Titled("Product Quality Services");
            //added by shrutika salve 18042023
            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.Monitor_Name).Titled("Monitor Name");

            grid.Columns.Add(model => model.TypeOfmonitoring).Titled("Type Of monitoring");
            grid.Columns.Add(model => model.Date).Titled("Date");

            grid.Columns.Add(model => model.Q3).Titled("Is this for Verification of earlier monitoring");
            grid.Columns.Add(model => model.Q4).Titled("Observations");
            grid.Columns.Add(model => model.InspectorComment).Titled("Inspector Comment");
            grid.Columns.Add(model => model.Q1).Titled("Training need identified");
            grid.Columns.Add(model => model.Q2).Titled("Training topic");
            grid.Columns.Add(model => model.Reporting_manager_comments).Titled("Reporting manager comment");
            grid.Columns.Add(model => model.chkManData).Titled("Man-Month asgmt.");
            grid.Columns.Add(model => model.CurrentAssignment).Titled("Current Assignment");
            grid.Columns.Add(model => model.specialMonitoringdata).Titled("surprise Monitoring");





            grid.Pager = new GridPager<monitors>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj.lstComplaintDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<monitors> GetData1(monitors U)
        {
            var Value = Session["datatable"];
            DataTable dataTable = (DataTable)Session["DTComplaint"];
            List<monitors> monitorsList = new List<monitors>();

            DataTable List = new DataTable();
            // Session["DTComplaint"] = List;
            if (dataTable != null)
            {
                foreach (DataRow dr in dataTable.Rows)
                {

                    monitors Data = new monitors();

                    Data.UINId = Convert.ToString(dr["UIN"]);
                    Data.Inspector_Name = Convert.ToString(dr["InspectorName"]);
                    Data.BranchName = Convert.ToString(dr["Branch_Name"]);
                    Data.Monitor_Name = Convert.ToString(dr["Monitor_Name"]);
                    Data.TypeOfmonitoring = Convert.ToString(dr["Type_Of_Monitoring"]);
                    Data.Date = Convert.ToString(dr["date"]);
                    Data.InspectorComment = Convert.ToString(dr["Inspector_comment"]);
                    Data.Reporting_manager_comments = Convert.ToString(dr["Reporting_manager_comment"]);
                    Data.Q1 = Convert.ToString(dr["Is there any need for additional training?"]);
                    Data.Q2 = Convert.ToString(dr["If yes, mention training topic name"]);
                    Data.Q3 = Convert.ToString(dr["Is inspection activity performed by considering observations made during earlier monitoring?"]);
                    Data.Q4 = Convert.ToString(dr["Observations"]);
                    Data.chkManData = Convert.ToString(dr["ManMonthsAssignment"]);
                    Data.CurrentAssignment = Convert.ToString(dr["CurrentAssignment"]);
                    Data.specialMonitoringdata = Convert.ToString(dr["SpecialMonitoring"]);
                    Data.SpecialServicesData = Convert.ToString(dr["SpecialServices"]);

                    monitorsList.Add(Data);
                }
            }

            obj.lstComplaintDashBoard1 = monitorsList;
            return obj.lstComplaintDashBoard1;

        }



        //added by shrutika salve 08092023


        public void SaveFileToPhysicalLocation(List<FileDetails> lstFileDtls, int ID)
        {
            foreach (var item in lstFileDtls)
            {
                string CurrentYear = DateTime.Now.Year.ToString();
                string CurrentMonth = DateTime.Now.Month.ToString();

                string pathYear = "~/Content/" + CurrentYear;
                string pathMonth = "~/Content/" + CurrentMonth;

                string FinalPath = "~/Content/" + CurrentYear + '/' + CurrentMonth;
                string FinalPath1 = "~/Content/" + CurrentYear + '/' + CurrentMonth + '/' + "~/Content/";

                if (!Directory.Exists(pathYear))
                {
                    //Directory.CreateDirectory(CurrentYear);
                    Directory.CreateDirectory(Server.MapPath("~/Content/" + CurrentYear));
                    //Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/Content/" + CurrentYear));


                    if (!Directory.Exists(FinalPath))
                    {
                        //Create Final Path
                        Directory.CreateDirectory(Server.MapPath(FinalPath));
                        //Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(FinalPath));


                        //Save File
                        string savePath = (FinalPath1 + ID + '_' + item.FileName);
                        System.IO.File.WriteAllBytes(savePath, item.FileContent);
                    }
                    else
                    {
                        string savePath = (FinalPath1 + ID + '_' + item.FileName);
                        System.IO.File.WriteAllBytes(savePath, item.FileContent);
                    }
                }
                else
                {
                    if (!Directory.Exists(FinalPath))
                    {
                        Directory.CreateDirectory(pathYear);
                    }
                    else
                    {
                        string savePath = (FinalPath + ID + '_' + item.FileName);
                        System.IO.File.WriteAllBytes(savePath, item.FileContent);
                    }

                }


            }
        }



        public FileResult Download(string d)
        {

            string FileName = "";
            string Date = "";
            string ID = string.Empty;
            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = DalObj2.GetFileContent(Convert.ToInt32(d));

            if (DTDownloadFile.Rows.Count > 0)
            {
                ID = DTDownloadFile.Rows[0]["ID"].ToString();
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;
            string path = string.Empty;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;
            if (!System.IO.File.Exists(path))
            {
                path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + ID + "_" + FileName;
            }
            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }



        [HttpPost]
        public JsonResult DeleteConFile(string id)
        {
            string Results = string.Empty;
            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                //  Guid guid = new Guid(id);
                DTGetDeleteFile = DalObj2.GetConFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                    fileDetails.FileName = Convert.ToString(DTGetDeleteFile.Rows[0]["FileName"]);
                }
                if (id != null && id != "")
                {
                    Results = DalObj2.DeleteConUploadedFile(id);
                    //var path = Path.Combine(Server.MapPath("~/Content/"), fileDetails.FileName);
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "ERROR" });
        }



        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            List<FileDetails> fileDetails = new List<FileDetails>();
            List<FileDetails> fileJobDetails = new List<FileDetails>();
            List<FileDetails> fileSubjobDetails = new List<FileDetails>();
            //List<FileDetails> fileEquiDetails = new List<FileDetails>();

            if (Session["listJobMasterUploadedFile"] != null)
            {
                fileJobDetails = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            }

            //if (Session["listSubUploadedFile"] != null)
            //{
            //    fileSubjobDetails = Session["listSubUploadedFile"] as List<FileDetails>;
            //}

            //if (Session["listEquiDetails"] != null)
            //{
            //    fileEquiDetails = Session["listEquiDetails"] as List<FileDetails>;
            //}

            //---Adding end Code
            try
            {

                FormCollection fc = new FormCollection();
                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    if (files != null && files.ContentLength > 0)
                    {
                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))

                        {
                            #region Generate random no
                            ////int _min = 100000000;
                            ////int _max = 999999999;
                            ////Random _rdm = new Random();
                            ////int Rjno = _rdm.Next(_min, _max);
                            ////string ConfirmCode = Convert.ToString(Rjno);
                            #endregion

                            string fileName = files.FileName;//ConfirmCode + files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;

                            if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
                            {
                                fileJobDetails.Add(fileDetail);
                            }
                            //else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                            //{
                            //    fileSubjobDetails.Add(fileDetail);
                            //}
                            //else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                            //{
                            //    fileEquiDetails.Add(fileDetail);
                            //}

                            //-----------------------------------------------------


                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                }

                if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD")
                {
                    Session["listJobMasterUploadedFile"] = fileJobDetails;
                }
                //else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD1")
                //{
                //    Session["listSubUploadedFile"] = fileSubjobDetails;
                //}
                //else if (Request.Files.Keys[0].ToString().ToUpper() == "FILEUPLOAD2")
                //{
                //    Session["listEquiDetails"] = fileEquiDetails;
                //}

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();

            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        //added by nikita on 12092023
        //export to excel for monitoring of records

        [HttpGet]
        public ActionResult ExportIndexMonitoringRecords()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MonitorRecordData> grid = CreateExportableGridEnquiryReport();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<MonitorRecordData> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "MonitoringRecords-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<MonitorRecordData> CreateExportableGridEnquiryReport()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<MonitorRecordData> grid = new Grid<MonitorRecordData>(GetDataMonitoringRecord());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            //added by nikita 31-08-2023
            grid.Columns.Add(model => model.EmployeeName).Titled("Employee Name");
            grid.Columns.Add(c => c.Brach_Name).Titled("Branch Name");
            grid.Columns.Add(c => c.Tuv_Email_Id).Titled("EmailID");
            grid.Columns.Add(c => c.MobileNo).Titled("Mobile No");
            grid.Columns.Add(c => c.Designation).Titled("Designation");
            grid.Columns.Add(c => c.IsMentor).Titled("IsMentor Yes/No");
            grid.Columns.Add(c => c.Mentoring).Titled("Mentoring");
            grid.Columns.Add(c => c.Mentoring_Count).Titled("Mentoring Count");

            grid.Columns.Add(c => c.OnsiteMonitoring).Titled("OnSite Monitoring");
            grid.Columns.Add(c => c.Onsite_Monitoring_Count).Titled("OnSite Monitoring Count");

            grid.Columns.Add(c => c.OffsiteMonitoring).Titled("Offsite Monitoring");
            grid.Columns.Add(c => c.Offsite_Monitoring_Count).Titled("Offsite Monitoring Count");

            grid.Columns.Add(c => c.MonitoringOfmonitors).Titled("Monitoring of Monitoring");
            grid.Columns.Add(c => c.Monitoring_of_monitors_Count).Titled("Monitoring of Monitoring Count");




            grid.Pager = new GridPager<MonitorRecordData>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = MonitorRecordData.listingmonitoringrecord.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            return grid;
        }

        public List<MonitorRecordData> GetDataMonitoringRecord()
        {



            List<MonitorRecordData> lstMonitoringrecord = new List<MonitorRecordData>();
            DataTable DTSearchByDateWiseData = new DataTable();

            string FromDate = string.Empty;
            string ToDate = string.Empty;

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);

                DTSearchByDateWiseData = MonitoringRecord.GetDataByDate(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstMonitoringrecord.Add
                        (
                           new MonitorRecordData
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,

                               Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                               EmployeeName = Convert.ToString(dr["EmployeeName"]),
                               Brach_Name = Convert.ToString(dr["Branch_Name"]),
                               MobileNo = Convert.ToString(dr["MobileNo"]),
                               Designation = Convert.ToString(dr["Designation"]),
                               IsMentor = Convert.ToString(dr["IsMentor"]),
                               Mentoring = Convert.ToString(dr["Mentoring"]),
                               MonitoringOfmonitors = Convert.ToString(dr["Monitoring of monitors"]),
                               OffsiteMonitoring = Convert.ToString(dr["Offsite Monitoring"]),
                               OnsiteMonitoring = Convert.ToString(dr["Onsite Monitoring"]),
                               Mentoring_Count = Convert.ToInt32(dr["Mentoring_Count"]),
                               Monitoring_of_monitors_Count = Convert.ToInt32(dr["Monitoring_of_monitors_Count"]),
                               Offsite_Monitoring_Count = Convert.ToInt32(dr["Offsite_Monitoring_Count"]),
                               Onsite_Monitoring_Count = Convert.ToInt32(dr["Onsite_Monitoring_Count"]),

                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    MonitorRecordData.listingmonitoringrecord = lstMonitoringrecord;
                    //return objEM.lst1;
                }
            }
            else
            {
                lstMonitoringrecord = MonitoringRecord.GetMonitorrecord();
            }

            ViewData["EnquiryMaster"] = lstMonitoringrecord;
            TempData["Result"] = null;
            TempData.Keep();
            MonitorRecordData.listingmonitoringrecord = lstMonitoringrecord;

            return MonitorRecordData.listingmonitoringrecord;
        }


        [HttpGet]
        public ActionResult CompentencyApproval()
        {
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.  
            DataSet ds = new DataSet();


            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Scope (Name)
            dtGetNAME = objCMV.dtGetNAME1();
            List<CompentencyMetrixView> lstGetName = new List<CompentencyMetrixView>();

            if (dtGetNAME.Rows.Count > 0)
            {

                foreach (DataRow dr in dtGetNAME.Rows)
                {

                    lstGetName.Add(new CompentencyMetrixView
                    {
                        CompentencyMetrixMasterId = Convert.ToInt32(dr["Id"]),
                        CompentencyMetrixMasterName = Convert.ToString(dr["Name"]),

                    }
                    );
                }
            }
            ViewData["Name"] = lstGetName;
            #endregion


            ds = objCMV.GetApprovalDataN(); // fill dataset  



            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                DateTime JoiningD = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateOfJoining"]);
                DateTime cuD = DateTime.Now;
                // DateTime calD =
                // TimeSpan ts = cuD.Year - JoiningD.Year;

                lmd.Add(new CompentencyMetrixView
                {
                    PK_userId = Convert.ToString(dr["PK_userId"]),
                    TiimesRole = Convert.ToString(dr["RoleName"]),
                    EmployeeCategory = Convert.ToString(dr["EmployementCategory"]),
                    StampNumber = Convert.ToString(dr["TUVIStampNo"]),

                    Id = 0,//Convert.ToInt32(dr["Id"]),
                    CandidateName = Convert.ToString(dr["InspectorName"]),
                    Location = Convert.ToString(dr["Branch_Name"]),
                    EducationalQualification = Convert.ToString(dr["Qualification"]),
                    

                    Designation = Convert.ToString(dr["Designation"]),
                    EmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                    CellPhoneNumber = Convert.ToString(dr["MobileNo"]),
                    JoiningDate1 = Convert.ToString(dr["DateOfJoining"]),
                    TotalExperienceInYears = Convert.ToString(dr["BeforeTUVExperience"]),
                    NumberOfYearsWithTUVIndia = "",//Convert.ToString(dr["NumberOfYearsWithTUVIndia"]),
                    CheckBoxValue = "",//Convert.ToString(dr["CheckBoxValue"]),
                    TotalTUVExperience = Convert.ToString(dr["TotalTUVExp"]),
                    OverAllExp = Convert.ToString(dr["OverAllExp"]),
                    AutharizeLevel = Convert.ToString(dr["AutharizeLevel"]),
                    strFormFilled = Convert.ToString(dr["FormFilled"]),
                    CreatedBy = Convert.ToString(dr["CreatedBy"]),
                    ApprovedBy = Convert.ToString(dr["ApprovedBy"]),
                    ApprovalStatus = Convert.ToString(dr["Approved"]),
                    BranchID = Convert.ToString(dr["BranchID"])


                });
            }
            return View(lmd.ToList());

        }

        //added by nikita on 1602024
        public ActionResult CheckSendForApproval(string pkcallid)
        {
            DataTable ds = new DataTable();
            string result;
            try
            {
                ds = objMOM.CheckData(pkcallid);
                result = JsonConvert.SerializeObject(ds);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        //added by nikita on 06052024
        public ActionResult CheckSendForApproval_monitoring(string pkcallid)
        {
            DataTable ds = new DataTable();
            string result;
            try
            {
                ds = objMOM.CheckData_Mom(pkcallid);
                result = JsonConvert.SerializeObject(ds);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult ExcelUpload(HttpPostedFileBase FileUpload1, monitors CM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/Excel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["Reference_Document"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 5; i <= 21; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifyes"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[22, 3]).Value);


                    for (int i = 23; i <= 40; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[41, 3]).Value);

                    for (int i = 42; i <= 56; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }


                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[57, 3]).Value);

                    for (int i = 58; i <= 58; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[59, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[60, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[60, 3]).Value);


                    for (int i = 61; i <= 69; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;

                        }
                    }

                    int startrow = 60;
                    int startcol = 5;
                    int endrow = 64;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);
                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }


        [HttpPost]
        public JsonResult OffsiteExcelUpload(HttpPostedFileBase FileUpload1, ModelOffSiteMonitoring OffM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["Reference_Document"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 4; i <= 7; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifyes"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[8, 3]).Value);


                    for (int i = 9; i <= 11; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["ifIRN"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[12, 3]).Value);

                    for (int i = 13; i <= 38; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }


                    values["rptprcs"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[39, 3]).Value);

                    for (int i = 40; i <= 52; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trngtpc"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[53, 3]).Value);

                    for (int i = 54; i <= 54; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[55, 3]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[56, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[56, 3]).Value);


                    for (int i = 57; i <= 65; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;

                        }
                    }

                    int startrow = 56;
                    int startcol = 5;
                    int endrow = 60;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

        [HttpPost]
        public JsonResult MentoringExcelUpload(HttpPostedFileBase FileUpload1, Mentoring MM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    values["DesInsAct"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 4; i <= 12; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[13, 3]).Value);


                    for (int i = 14; i <= 14; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[15, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[16, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[16, 3]).Value);


                    for (int i = 17; i <= 25; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }



                    int startrow = 16;
                    int startcol = 5;
                    int endrow = 20;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }

        [HttpPost]
        public JsonResult MOMsiteExcelUpload(HttpPostedFileBase FileUpload1, MonitoringOfMonitors MOM)
        {
            if (FileUpload1 != null && FileUpload1.ContentLength > 0)
            {
                try
                {
                    Random rnd = new Random();
                    int myRandomNo = rnd.Next(10000000, 99999999);
                    string strmyRandomNo = Convert.ToString(myRandomNo);
                    string path = Server.MapPath("~/IVRIRNExcel/");

                    // Ensure the directory exists
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Save the uploaded file
                    string filePath = path + Path.GetFileName(strmyRandomNo + FileUpload1.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    FileUpload1.SaveAs(filePath);

                    // Process the Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open(filePath);
                    Microsoft.Office.Interop.Excel.Worksheet wSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelBook.Worksheets[1];

                    // Dictionary to store extracted values
                    Dictionary<string, string> values = new Dictionary<string, string>();
                    List<object> tableData = new List<object>();

                    // Extract specific values from cells
                    values["itemDescription"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[2, 3]).Value); // Item Description
                    //values["DesInsAct"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[3, 3]).Value); // Reference Document
                    //values["Details_of_inspection_activity"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[4, 3]).Value);



                    // Extract questions and answers
                    Dictionary<string, string> answers = new Dictionary<string, string>();
                    for (int i = 3; i <= 24; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }

                    values["trainingtopic"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[25, 3]).Value);




                    values["observation"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[26, 3]).Value);
                    values["obscatno"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[27, 1]).Value);
                    values["obscat"] = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[27, 3]).Value);


                    for (int i = 28; i <= 36; i++) // Assuming questions start from row 4
                    {
                        string questionNumber = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 1]).Value); // Sr.No
                        string questionText = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 2]).Value);   // Parameters
                        string answer = Convert.ToString(((Microsoft.Office.Interop.Excel.Range)wSheet.Cells[i, 3]).Value);        // Answer (YES/NO/NA)

                        if (!string.IsNullOrEmpty(questionNumber) && !string.IsNullOrEmpty(questionText))
                        {
                            answers[questionNumber] = answer;
                        }
                    }



                    int startrow = 28;
                    int startcol = 5;
                    int endrow = 32;
                    int endcol = 6;


                    for (int row = startrow; row <= endrow; row++)
                    {
                        var rowData = new List<string>(); // A list to hold cell data for a single row
                        for (int col = startcol; col <= endcol; col++)
                        {
                            // Add the text of each cell to the row data
                            rowData.Add(wSheet.Cells[row, col].Text ?? string.Empty); // Handle nulls
                        }
                        tableData.Add(rowData); // Add the row to the table
                    }

                    // Serialize the plain table data to JSON
                    var tablehtml = Newtonsoft.Json.JsonConvert.SerializeObject(tableData);


                    // Clean up Excel resources
                    excelBook.Close(false);
                    xlApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);

                    // Return values and answers as JSON
                    return Json(new { success = true, values = values, answers = answers, tablehtml = tablehtml }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No file uploaded or file is empty." });
            }
        }
    }
}

