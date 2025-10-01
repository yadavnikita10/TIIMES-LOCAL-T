using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;

using System.IO;
using System.Text;
using OfficeOpenXml;
using SelectPdf;
using NonFactors.Mvc.Grid;
using System.Net;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class TechnicalCompetencyEvaluationController : Controller
    {
        // GET: TechnicalCompetencyEvaluation

        DALTechnicalCompetencyEvaluation objTCE = new DALTechnicalCompetencyEvaluation();

        TechnicalCompetencyEvaluation objMTCE = new TechnicalCompetencyEvaluation();


        public ActionResult Tcv(string categoryid)
        {

         
                return RedirectToAction("TechnicalCompetencyEvaluation", new { Id = categoryid });
           
        }

        #region Post
        //[HttpPost]
        //public ActionResult Tcv(TechnicalCompetencyEvaluation S, FormCollection fc, int[] ID_, int? PK_TechnicalCompetencyEvaluation)
        //{

        //    string Result = string.Empty;




        //    try
        //    {

        //        if (PK_TechnicalCompetencyEvaluation > 0)
        //        {
        //            //Update
        //            foreach (var item in S.RangeOfInspectionList)
        //            {
        //                //string ProList = fc["ProductLists"];
        //                //string a = item.LBasicAuthorizationName;
        //                string ProList = string.Join(",", item.LBasicAuthorizationName);
        //                S.BasicAuthorization = ProList;

        //                objMTCE.PK_TechnicalCompetencyEvaluation = Convert.ToInt16(PK_TechnicalCompetencyEvaluation);
        //                objMTCE.FK_RangeInspectionId = item.LFK_RangeInspectionId;
        //                objMTCE.AutharizationLevel = item.LAutharizationLevel;
        //                // objMTCE.BasicAuthorization = item.LBasicAuthorizationName;
        //                objMTCE.BasicAuthorization = ProList;

        //                objMTCE.InspectorName = S.InspectorName;
        //                Result = objTCE.Insert(objMTCE);
        //            }




        //        }
        //        else
        //        {

        //            //foreach (var i in S.FK_RangeInspectionName)
        //            //{

        //            //}
        //            foreach (var item in S.RangeOfInspectionList)
        //            {
        //                string ProList = string.Join(",", item.LBasicAuthorizationName);
        //                S.BasicAuthorization = ProList;

        //                objMTCE.FK_RangeInspectionId = item.LFK_RangeInspectionId;
        //                objMTCE.AutharizationLevel = item.LAutharizationLevel;
        //                // objMTCE.BasicAuthorization = item.LBasicAuthorizationName;
        //                objMTCE.BasicAuthorization = ProList;
        //                objMTCE.InspectorName = S.InspectorName;
        //                Result = objTCE.Insert(objMTCE);
        //            }


        //            if (Convert.ToInt16(Result) > 0)
        //            {
        //                ModelState.Clear();
        //                TempData["message"] = "Record Added Successfully";
        //            }
        //            else
        //            {
        //                TempData["message"] = "Error";
        //            }
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return RedirectToAction("ListTechnicalCompetencyEvaluation", "TechnicalCompetencyEvaluation");
        //}
        #endregion

        public ActionResult TechnicalCompetencyEvaluation(string Id)
        {

            var model = new TechnicalCompetencyEvaluation();

            #region testddl
            //string str = "1001,1003,1005"; //query database and get the selected value

            //List<string> selectedList = str.Split(',').ToList();

            //List<SelectListItem> ddlitemlist = new List<SelectListItem>();// DDLGetInitData().Select(c => new SelectListItem { Text = c.Name, Value = c.ID.ToString(), Selected = selectedList.Contains(c.ID.ToString()) ? true : false }).ToList();

            //ViewBag.ddlitemlist = ddlitemlist;
            #endregion

            #region Bind Basic Auth

            DataSet dsGetAuthName = new DataSet();


            dsGetAuthName = objTCE.GetBasicAuthName();
            List<TechnicalCompetencyEvaluation> searchlist = new List<TechnicalCompetencyEvaluation>();

            foreach (DataRow dr in dsGetAuthName.Tables[0].Rows)
            {

                searchlist.Add(new TechnicalCompetencyEvaluation
                {
                    SkillID = dr["id"].ToString(),
                    BasicAuthText = dr["Text"].ToString(),
                    BasicAuthValue = dr["Value"].ToString()
                });

            }



            ViewBag.lstSkills = searchlist;

            #endregion

            #region Bind Name
            List<EmpName> lstName = new List<EmpName>();
            DataSet dsGetName = new DataSet();
            if(Id!=null)
            {
                dsGetName = objTCE.GetName(Id);
                if (dsGetName.Tables[0].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
                {
                    lstName = (from n in dsGetName.Tables[0].AsEnumerable()
                               select
                               new EmpName()
                               {
                                   Name = n.Field<string>(dsGetName.Tables[0].Columns["Name"].ToString()),
                                   Code = n.Field<string>(dsGetName.Tables[0].Columns["Id"].ToString())

                               }).ToList();
                }
            }
           
            ViewBag.Name = lstName;

            #endregion


            #region Bind RangeInspection
            List<RangeOfInspectionList> lstRangeInspection = new List<RangeOfInspectionList>();
            DataSet dsGetRangeOfInspection = new DataSet();
            DataSet dsGetDataById = new DataSet();
            dsGetRangeOfInspection = objTCE.GetRangeOfInspection();
            if (dsGetRangeOfInspection.Tables[0].Rows.Count > 0)//Dynamic Binding Title DropDwonlist
            {
                lstRangeInspection = (from n in dsGetRangeOfInspection.Tables[0].AsEnumerable()
                                          //select new TechnicalCompetencyEvaluation()
                                      select new RangeOfInspectionList()
                                      {
                                          LIAFScopeNumber =  n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["IAFScopeNumber"].ToString()),
                                          LFK_RangeInspectionName = n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["RangeInspection"].ToString()),
                                          LFK_RangeInspectionId = n.Field<int>(dsGetRangeOfInspection.Tables[0].Columns["PK_RangeInspectionId"].ToString()),
                                           LIAFScopeName =     n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["IAFScopeName"].ToString()),
                                          LFieldOfInspection = n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["FieldOfInspection"].ToString()),

                                          MinimumEducationQualification = n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["MinimumEducationQua"].ToString()),
                                          MinimumRequirementForLevel3 = n.Field<string>(dsGetRangeOfInspection.Tables[0].Columns["MinimumRequirmentForLevel3"].ToString())


                                      }).ToList();
                ViewData["TitleName"] = lstRangeInspection;
            }

            else
            {

            }
            #endregion

            #region bind Authorise level
            List<RangeOfInspectionList> items = new List<RangeOfInspectionList>();
            items.Add(new RangeOfInspectionList
            {
                LAutharizationLevel = "1 – Can work under supervision.",
                LAutharizationLevelId = "1"
            });
            items.Add(new RangeOfInspectionList
            {
                LAutharizationLevel = "2 – Can work individually under guidance of seniors.",
                LAutharizationLevelId = "2"
            });
            items.Add(new RangeOfInspectionList
            {
                LAutharizationLevel = "3 – Can work independently & guide others.",
                LAutharizationLevelId = "3"
            });
            items.Add(new RangeOfInspectionList
            {
                LAutharizationLevel = "4 - Can train, monitor, guide & evaluate others.",
                LAutharizationLevelId = "4"
            });
            items.Add(new RangeOfInspectionList
            {
                LAutharizationLevel = "NA",
                LAutharizationLevelId = "NA"
            });


            ViewBag.AuthoriseLevel = items;

            #endregion

            #region bind Basis of authorization
            //List<RangeOfInspectionList> lstBaisOfAuthorization = new List<RangeOfInspectionList>();
            //lstBaisOfAuthorization.Add(new RangeOfInspectionList
            //{
            //    LBasicAuthorizationName = "A–  Professional Qualification OR Past Experience till date",
            //    LBasicAuthorization = "A"
            //});
            //lstBaisOfAuthorization.Add(new RangeOfInspectionList
            //{
            //    LBasicAuthorizationName = "B -  Interview Assessment",
            //    LBasicAuthorization = "B"
            //});
            //lstBaisOfAuthorization.Add(new RangeOfInspectionList
            //{
            //    LBasicAuthorizationName = "C -  Training Imparted   OR Inspection Under Supervision",
            //    LBasicAuthorization = "C"
            //});
            //lstBaisOfAuthorization.Add(new RangeOfInspectionList
            //{
            //    LBasicAuthorizationName = "D – Supervised Inspection completed",
            //    LBasicAuthorization = "D"
            //});

            //ViewBag.BasisOfAuthorization = lstBaisOfAuthorization;
            #endregion


            if (Id != null && Id != "")
            {
                model.InspectorName = Id;
                List<RangeOfInspectionList> lstRangeOfInspectionList = new List<RangeOfInspectionList>();
                // dsGetDataById = objTCE.GetDataById(Convert.ToInt32(Id));


                dsGetDataById = objTCE.GetDataById(Convert.ToString(Id));
                List<string> Selected = new List<string>();
                string[] splitedProduct_Name;
                //var Existingins=0;
                string Existingins;
                string a = null;



                //  if (dsGetDataById.Tables[0].Rows.Count > 0)
                if (dsGetDataById.Tables[0].Rows.Count > 0)
                {
                    //  a = dsGetDataById.Tables[0].Rows[0]["BasicAuthorization"].ToString();
                    model.InspectorName = Id;//dsGetDataById.Tables[0].Rows[0]["InspectorName"].ToString();
                    model.PCH = dsGetDataById.Tables[0].Rows[0]["PCH"].ToString();

                    if (dsGetDataById.Tables[0].Rows[0]["IsVerified"].ToString() == "1")
                    {
                        model.isVerified = true;
                    }
                    else
                    {
                        model.isVerified = false;
                    }
                    //added by nikita on 17122024
                    foreach (DataRow row in dsGetDataById.Tables[0].Rows)
                    {
                        string AutharizationLevel = row["AutharizationLevel"].ToString();
                        string BasicAuthorization = row["BasicAuthorization"].ToString();

                        //if ((AutharizationLevel == "3" && BasicAuthorization == "C"))
                        //{
                        //    model.isFormFilled = true;
                        //    break; // Exit the loop as we found a match
                        //}
                        //else
                        //{
                        //    model.isFormFilled = false;
                        //}
                        if ((AutharizationLevel != "" && BasicAuthorization !="" ))
                        {
                            model.isFormFilled = true;
                            break; // Exit the loop as we found a match
                        }
                        else
                        {
                            model.isFormFilled = false;
                        }
                    }
                    //commented on 12172024

                    //if (dsGetDataById.Tables[0].Rows[0]["FormFilled"].ToString() == "1")
                    //{
                    //    model.isFormFilled = true;
                    //}
                    //else
                    //{
                    //    model.isFormFilled = false;
                    //}



                    foreach (DataRow dr in dsGetDataById.Tables[0].Rows)
                    {
                        // Existingins = dsGetDataById.Tables[0].Rows[0]["BasicAuthorization"].ToString();
                        //splitedProduct_Name = Existingins.Split(',');
                        //foreach (var single in splitedProduct_Name)
                        //{
                        //    Selected.Add(single);
                        //}
                        //ViewBag.EditproductName = Selected;

                        lstRangeOfInspectionList.Add(new RangeOfInspectionList
                        {
                            //  //model.PK_TechnicalCompetencyEvaluation = Convert.ToInt16(dsGetDataById.Tables[0].Rows[0]["PK_TechnicalCompetencyEvaluation"]),
                            //model.FK_RangeInspectionId = Convert.ToInt32(dsGetDataById.Tables[0].Rows[0]["FK_RangeInspectionId"]),
                            //model.InspectorName = dsGetDataById.Tables[0].Rows[0]["InspectorName"].ToString(),
                            //AutharizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            //model.BasicAuthorization = dsGetDataById.Tables[0].Rows[0]["BasicAuthorization"].ToString(),
                            //model.InspectorBranch = dsGetDataById.Tables[0].Rows[0]["InspectorBranch"].ToString(),
                            //model.EfectiveDate = dsGetDataById.Tables[0].Rows[0]["EfectiveDate"].ToString(),
                            //model.RevisionDate = dsGetDataById.Tables[0].Rows[0]["RevisionDate"].ToString(),
                            //model.ReportNo = dsGetDataById.Tables[0].Rows[0]["ReportNo"].ToString()

                            LIAFScopeNumber = Convert.ToString(dr["IAFScopeNumber"]),
                            LFK_RangeInspectionName = Convert.ToString(dr["RangeInspectionName"]),
                            LFK_RangeInspectionId = Convert.ToInt16(dr["FK_RangeInspectionId"]),
                            LAutharizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            LBasicAuthorizationName = Convert.ToString(dr["BasicAuthorization"]),
                            LIAFScopeName  = Convert.ToString(dr["IAFScopeName"]),
                            LFieldOfInspection = Convert.ToString(dr["FieldOfInspection"]),
                            LisVerified = Convert.ToString(dr["FieldOfInspection"]),
                            PK_TechnicalCompetencyEvaluation = Convert.ToString(dr["PK_TechnicalCompetencyEvaluation"]),

                            
                             

                            MinimumEducationQualification = Convert.ToString(dr["MinimumEducationQua"]),
                            MinimumRequirementForLevel3 =  Convert.ToString(dr["MinimumRequirmentForLevel3"]),
                            skillID = Convert.ToString(dr["BasicAuthorization"]),
                            Remarks = Convert.ToString(dr["Remarks"]),
                            //LIAFScopeNumber 
                            //LFK_RangeInspectionName
                            //LFK_RangeInspectionId
                            // LIAFScopeName =
                            //LFieldOfInspection = 


                        }

                        );
                    }

                    ViewData["TitleName"] = lstRangeOfInspectionList;
                    
                    /*********************************************************/

                    DataTable DTGetUploadedFile = new DataTable();
                    List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                    DTGetUploadedFile = objTCE.EditUploadedFile(Id);
                    if (DTGetUploadedFile.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTGetUploadedFile.Rows)
                        {
                            lstEditFileDetails.Add(
                               new FileDetails
                               {

                                   PK_ID = Convert.ToInt32(dr["PK_ID"]),
                                   FileName = Convert.ToString(dr["FileName"]),
                                   Extension = Convert.ToString(dr["Extenstion"]),
                                   IDS = Convert.ToString(dr["FileID"]),
                               }
                             );
                        }
                        ViewData["lstEditFileDetails"] = lstEditFileDetails;
                        model.FileDetails = lstEditFileDetails;
                    }

                    return View(model);

                }
                    //latest change
                //ViewData["TitleName"] = lstRangeOfInspectionList;
                //return View(model);
                else
                {
                    return View(model);
                }
            }



            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult TechnicalCompetencyEvaluation(TechnicalCompetencyEvaluation S, FormCollection fc, int[] ID_, int? PK_TechnicalCompetencyEvaluation)
        {

            string Result = string.Empty;

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            bool formfilled = false;

            try
            {

                if (PK_TechnicalCompetencyEvaluation > 0)
                {
                    //Update
                    foreach (var item in S.RangeOfInspectionList)
                    {
                        //string ProList = fc["ProductLists"];
                        //string a = item.LBasicAuthorizationName;
                        //string ProList = string.Join(",", item.LBasicAuthorizationName);
                        //S.BasicAuthorization = ProList;
                        if(item.ListBasicAuthorizationName!=null)
                        {
                            objMTCE.BasicAuthorization = String.Join(",", item.ListBasicAuthorizationName.ToArray());
                        }
                        else
                        {
                            objMTCE.BasicAuthorization = null;
                        }


                        //if ((item.LAutharizationLevel == "3" && item.skillID == "C"))
                        //{
                        //    formfilled = true;

                        //}
                        if ((item.LAutharizationLevel != null && item.skillID != null))
                        {
                            formfilled = true;

                        }
                        objMTCE.PK_TechnicalCompetencyEvaluation = Convert.ToInt16(item.PK_TechnicalCompetencyEvaluation);
                        objMTCE.FK_RangeInspectionId = item.LFK_RangeInspectionId;
                        objMTCE.AutharizationLevel = item.LAutharizationLevel;
                        // objMTCE.BasicAuthorization = item.LBasicAuthorizationName;
                        // objMTCE.BasicAuthorization = ProList;
                        objMTCE.BasicAuthorization = item.skillID;
                        objMTCE.InspectorName = S.InspectorName;
                        Result = objTCE.Insert(objMTCE);
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objTCE.InsertFileAttachment(lstFileDtls, objMTCE.InspectorName);
                            Session["listJobMasterUploadedFile"] = null;
                        }
                    }




                }
                else
                {

                    //foreach (var i in S.FK_RangeInspectionName)
                    //{

                    //}
                    foreach (var item in S.RangeOfInspectionList)
                    {
                        if (item.ListBasicAuthorizationName != null)
                        {
                            objMTCE.BasicAuthorization = String.Join(",", item.ListBasicAuthorizationName.ToArray());
                        }
                        else
                        {
                            objMTCE.BasicAuthorization = null;
                        }
                        //if ((item.LAutharizationLevel == "3" && item.skillID == "C"))
                        //{
                        //    formfilled = true;

                        //}
                        if ((item.LAutharizationLevel != null && item.skillID != null))
                        {
                            formfilled = true;

                        }
                        //string ProList = string.Join(",", item.LBasicAuthorizationName);
                        //S.BasicAuthorization = ProList;

                        objMTCE.FK_RangeInspectionId = item.LFK_RangeInspectionId;
                        objMTCE.AutharizationLevel = item.LAutharizationLevel;
                        objMTCE.BasicAuthorization = item.skillID;
                        // objMTCE.BasicAuthorization = item.LBasicAuthorizationName;
                        // objMTCE.BasicAuthorization = ProList;
                        objMTCE.BasicAuthorization = item.skillID;
                        objMTCE.InspectorName = S.InspectorName;
                        objMTCE.isVerified = S.isVerified;
                        objMTCE.isFormFilled = formfilled ? true : S.isFormFilled;
                        //objMTCE.isFormFilled = S.isFormFilled;
                        objMTCE.Remarks = item.Remarks;
                        Result = objTCE.Insert(objMTCE);

                    }

                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        Result = objTCE.InsertFileAttachment(lstFileDtls, objMTCE.InspectorName);
                        Session["listJobMasterUploadedFile"] = null;
                    }


                    if (Convert.ToInt16(Result) > 0)
                    {
                        //ModelState.Clear(); 05/09/2022
                        TempData["message"] = "Record Added Successfully";
                    }
                    else
                    {
                        TempData["message"] = "Error";
                    }
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("TechnicalCompetencyEvaluation", new { Id = S.InspectorName});

            // S.InspectorName
            return RedirectToAction("ListTechnicalCompetencyEvaluation", "TechnicalCompetencyEvaluation");
            //return RedirectToAction("ListTechnicalCompetencyEvaluation", "TechnicalCompetencyEvaluation");
        }


        [HttpGet]
        public ActionResult ListTechnicalCompetencyEvaluation()
        {
            List<TechnicalCompetencyEvaluation> lmd = new List<TechnicalCompetencyEvaluation>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objTCE.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TechnicalCompetencyEvaluation
                {
                    //PK_TechnicalCompetencyEvaluation = Convert.ToInt32(dr["PK_TechnicalCompetencyEvaluation"]),
                    //FK_RangeInspectionName = Convert.ToString(dr["FK_RangeInspectionName"]),
                    //AutharizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                    //BasicAuthorization = Convert.ToString(dr["BasicAuthorization"]),
                    InspectorId = Convert.ToString(dr["PK_UserId"]),
                    InspectorName = Convert.ToString(dr["InspectorName"]),
                    Branch = Convert.ToString(dr["Branch_Name"]),
                    TiimesRole = Convert.ToString(dr["RoleName"]),
                    Mobile = Convert.ToString(dr["MobileNo"]),
                    Email = Convert.ToString(dr["Tuv_Email_Id"]),
                    EmployeeCategory = Convert.ToString(dr["EmployementCategory"]),
                    ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                    ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                    InspectorBranch = Convert.ToString(dr["Branch_Name"]),
                    FormFilled = Convert.ToString(dr["FormFilled"]),
                    Verified = Convert.ToString(dr["IsVerified"]),
                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),  //added by nikita on 11092023

                    //ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                    //InspectorBranch = Convert.ToString(dr["InspectorBranch"]),
                    //EfectiveDate = Convert.ToString(dr["EfectiveDate"]),
                    //RevisionDate = Convert.ToString(dr["RevisionDate"]),
                    //ReportNo = Convert.ToString(dr["ReportNo"]),

                });
            }

            objMTCE.lmd1 = lmd;
            // return View(lmd.ToList());
            return View(objMTCE);
        }

        public JsonResult GetSurveyorName(string prefix)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objTCE.GetSurveyorName(prefix);
            List<StampRegister> searchlist = new List<StampRegister>();

            foreach (DataRow dr in dsTopic.Tables[0].Rows)

            {

                searchlist.Add(new StampRegister
                {
                    SurveyorName = dr["SurveyorName"].ToString(),
                    SurveyorId = dr["SurveyorId"].ToString()
                });

            }
            return Json(searchlist, JsonRequestBehavior.AllowGet);


        }

        string InspectorBranch;
        public JsonResult GetLocation(string Category)

        {
            DataSet dsTopic = new DataSet();
            // DataSet ds = dblayer.GetName(prefix);
            dsTopic = objTCE.GetDateOfJoining(Category);
            List<TechnicalCompetencyEvaluation> searchlist = new List<TechnicalCompetencyEvaluation>();

            if (dsTopic.Tables[0].Rows.Count > 0)
            {

                DateTime? myDate = Convert.ToDateTime(dsTopic.Tables[0].Rows[0]["DateOfJoining"]);
                //DateTime? myDate = Convert.ToDateTime(dr["DateOfJoining"]);
                string sqlFormattedDate = myDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

                //searchlist.Add(new StampRegister
                //{
                //JoiningDate1 = sqlFormattedDate;
                // JoiningDate1 = sqlFormattedDate;

                InspectorBranch = dsTopic.Tables[0].Rows[0]["BranchName"].ToString();
                //});


            }

            return Json(InspectorBranch, JsonRequestBehavior.AllowGet);


        }

       

    #region  Print Pdf COde 


    [HttpGet]
        public ActionResult TechnicalCompetencyEvalReportsOrig(int? pktce, string InspectorId,string InspectorFullName)
        {


            string IFName = InspectorFullName;
            objMTCE.InspectorFullName = IFName;
            #region GetIAFScope
            List<Report> lstTCE = new List<Report>();
            List<Report> lstFOI = new List<Report>();
            List<Report> lstROI = new List<Report>();
            DataSet dsGetReportIAFScope = new DataSet();
            DataSet dsGetReportFieldOfInspection = new DataSet();
            DataSet dsGetRangeOfInspection = new DataSet();


            dsGetReportIAFScope = objTCE.GetReport(InspectorId);
            if (dsGetReportIAFScope.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[0].Rows)
                {
                    lstTCE.Add(
                        new Report
                        {
                            PK_IAFScopeId = Convert.ToInt16(dr["PK_IAFScopeId"]),
                            IAFScopeName = Convert.ToString(dr["IAFScopeName"])


                        }
                        );
                }
                ViewData["VDIAFScope"] = lstTCE;
            }
            #endregion

            #region test


            dsGetReportIAFScope = objTCE.test(InspectorId, Convert.ToInt32(pktce));
            if (dsGetReportIAFScope.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[0].Rows)
                {
                    lstTCE.Add(
                        new Report
                        {
                            IAFScope = Convert.ToString(dr["IAFScope"]),
                            InspectionName = Convert.ToString(dr["FieldOfInspection"]),
                            RangeInspection = Convert.ToString(dr["Type_RangeOfInspection"]),
                            AuthorizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            BasisOfAuthorization = Convert.ToString(dr["BasisOfAuthorization"]),
                           
                        }
                        );
                }
                ViewData["Test"] = lstTCE;
            }

            string d1 = "";
            string d2 = "";
            string d3 = "";
            string d4 = "";
            string d5 = "";
            foreach (Report v in lstTCE)
            {
                d1 += "<tr><td>" + v.IAFScope + "<td></tr>";
                d2 += "<tr><td>" + v.InspectionName + "</td></tr>";
                d3 += "<tr><td>" + v.RangeInspection + "</td></tr>";
                d4 += "<tr><td>" + v.AuthorizationLevel + "</td></tr>";
                d5 += "<tr><td>" + v.BasisOfAuthorization + "</td></tr>";
            }

            #endregion

            #region Abal Code
            //string emailBody = DALTechnicalCompetencyEvaluation.RenderViewToString(this.ControllerContext.Controller.ToString(), "~/Views/TechnicalCompetencyEvaluation/_Test.cshtml", lstTCE);
            #endregion




            var modelR = new Report();
            DataSet dsGetReport = new DataSet();

            int count = 0;
            DataTable ImageReportDashBoard = new DataTable();
            List<ReportImageModel> ImageDashBoard = new List<ReportImageModel>();
            dsGetReport = objTCE.GetReport(InspectorId);
            //if (dsGetReport.Tables[0].Rows.Count > 0)
            //    {
            //        modelR.IAFScope = Convert.ToString(dsGetReport.Tables[0].Rows[0]["IAFScope"]);
            //    }
            #region Save to Pdf Code 



            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;
            int J = 0;
            int K = 0;
            int L = 0;
            int M = 0;
            string Datarow = "";
            string DatarowFOI = "";
            string data1 = "";
            int data0 = 0;
            string data2 = "";
            int data02 = 0;
            string data3 = "";
            int data03 = 0;
            string data4 = "";
            int data04 = 0;


            //using (StreamReader reader = new StreamReader(Server.MapPath("~/inspection-visit-report.html")))
            using (StreamReader reader = new StreamReader(Server.MapPath("~/technicalcompetencyevaluation.html")))
            {
                body = reader.ReadToEnd();
            }










            // body = body.Replace("[IAFScope]", modelR.IAFScope); vaibhav
            foreach (Report v in lstTCE)
            {
                J = J + 1;
                Datarow += "<tr></tr>";
                data1 += Datarow + "<td> " + v.IAFScopeName + "</td>";
                data0 += v.PK_IAFScopeId;
                //body = body.Replace("[data1]", data1);
                #region getFieldOfInspection
                List<Report> lstTCE1 = new List<Report>();
                DataSet dsGetFieldOfInspection = new DataSet();

                dsGetReportFieldOfInspection = objTCE.GetFieldOfInspection(data0);
                if (dsGetReportFieldOfInspection.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsGetReportFieldOfInspection.Tables[0].Rows)
                    {
                        lstFOI.Add(
                            new Report
                            {
                                PK_FieldInspection = Convert.ToInt16(dr["PK_FieldInspection"]),
                                InspectionName = Convert.ToString(dr["InspectionName"])


                            }
                            );
                    }
                    ViewData["VDFieldOfInspection"] = lstFOI;
                }
                //print FOI
                #region print FOI
                //        foreach (Report objFOI in lstFOI)
                //{
                //    DatarowFOI += "<tr></tr>";       
                //    data2 += DatarowFOI+"<td>  " + objFOI.InspectionName + "</td>";
                //    data02 = objFOI.PK_FieldInspection;


                //    #region GetType/Range Of Inspection
                //    dsGetRangeOfInspection = objTCE.GetRangeOfInspection(data02);
                //    if (dsGetRangeOfInspection.Tables[0].Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in dsGetRangeOfInspection.Tables[0].Rows)
                //        {
                //            lstROI.Add(
                //                new Report
                //                {
                //                    PK_RangeInspectionId = Convert.ToInt16(dr["PK_RangeInspectionId"]),
                //                    RangeInspection = Convert.ToString(dr["RangeInspection"])


                //                }
                //                );
                //        }

                //    }
                #endregion

                //}


                //print ROI
                //        foreach (Report objROI in lstROI)
                //{

                //    data3 += "<td> " + objROI.RangeInspection + " </td>";
                //    data03 += objROI.PK_RangeInspectionId;

                //}

                #endregion


                // foreach ()
                //foreach ()
                //{
                //    K = K + 1;
                //    data2 += "<td> " + J + " </td>";
                //    foreach ()
                //    {

                //        L = L + 1;
                //        data3 += "<td> " + J + " </td>";
                //        data4 += "<td> " + J + " </td>";

                //    }

                //}

            }
            // wait


            //Rahul Print
            body = body.Replace("[data1]", data1);
            body = body.Replace("[d1]", d1);
            body = body.Replace("[d2]", d2);
            body = body.Replace("[d3]", d3);
            body = body.Replace("[d4]", d4);
            body = body.Replace("[d5]", d5);
            // body = body.Replace("[data2]", data2);
            //body = body.Replace("[data3]", data3);
            body = body.Replace("[Logo]", "http://localhost:54895/AllJsAndCss/images/logo.png");
            body = body.Replace("[Stamp]", "http://localhost:54895/Stamp.png");
            body = body.Replace("[Signature]", "http://localhost:54895/signature.jpg");



            strs.Append(body);
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            string Vaibhav = null;

            //When javascript needs to be enabled
            converter.Options.JavaScriptEnabled = true;//(HtmlToPdfStartupMode)Enum.Parse(typeof(HtmlToPdfStartupMode),, true);

            converter.Options.MaxPageLoadTime = 180;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            PdfDocument doc = converter.ConvertHtmlString(body);
            string ReportName = Vaibhav + "/" + count + ".pdf";
            string path = Server.MapPath("~/TechnicalCompetencyEvaluationReport");
            doc.Save(path + '\\' + ReportName);
            doc.Close();
            #endregion

            return View(objMTCE);
            //return RedirectToAction("ListTechnicalCompetencyEvaluation", "TechnicalCompetencyEvaluation");
        }

        #endregion

        //vaibhav
        public ActionResult TechnicalCompetencyEvalReports( int? pktce, string InspectorId, string InspectorFullName, string GridHtml,DateTime ? ModifiedDate)
        {

            #region print return to next page
            string IFName = InspectorFullName;
            objMTCE.InspectorFullName = IFName;
            if(objMTCE.ModifiedDate!=null)
            { 
            objMTCE.ModifiedDate = Convert.ToString(ModifiedDate);
            }
            DataSet dsGetReportIAFScope = new DataSet();
            List<Report> lstTCE = new List<Report>();
            dsGetReportIAFScope = objTCE.test(InspectorId, Convert.ToInt32(pktce));
            if (dsGetReportIAFScope.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsGetReportIAFScope.Tables[0].Rows)
                {
                    objMTCE.Signature = Convert.ToString(dr["Signature"]); 
                    lstTCE.Add(
                        new Report
                        {
                            IAFScope = Convert.ToString(dr["IAFScope"]),
                            InspectionName = Convert.ToString(dr["FieldOfInspection"]),
                            RangeInspection = Convert.ToString(dr["Type_RangeOfInspection"]),
                            AuthorizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            BasisOfAuthorization = Convert.ToString(dr["BasisOfAuthorization"]),
                            MinimumEducationQua = Convert.ToString(dr["MinimumEducationQua"]),
                            MinimumRequirmentForLevel3 = Convert.ToString(dr["MinimumRequirmentForLevel3"])

                        }
                        );
                }
                ViewData["Test"] = lstTCE;
            }
            if (dsGetReportIAFScope.Tables[1].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[1].Rows)
                {
                    //objMTCE.FullName = Convert.ToString(dr["BranchAdminName"]);
                    //objMTCE.Signature1 = Convert.ToString(dr["Signature"]);
                    //objMTCE.AdminDesignation = Convert.ToString(dr["AdminDesignation"]);

                    objMTCE.CreatedBy = Convert.ToString(dr["CreatedBy"]);
                    objMTCE.CreatedDesignation = Convert.ToString(dr["CreatedDesignation"]);
                    objMTCE.CreatedSignature1 = Convert.ToString(dr["CreatedSignature"]);//(byte[])(dr["CreatedSignature"]);
                    objMTCE.Verified=Convert.ToString(dr["IsVerified"]);
                    objMTCE.VerifiedDateTime = Convert.ToString(dr["VerifiedDateTime"]);
                    objMTCE.ModifiedDate = Convert.ToString(dr["modifieddate"]);
                    //objMTCE.CreatedBy

                }
            }

            if (dsGetReportIAFScope.Tables[3].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[3].Rows)
                {
                    //objMTCE.FullName = Convert.ToString(dr["BranchAdminName"]);
                    //objMTCE.Signature1 = Convert.ToString(dr["Signature"]);
                    //objMTCE.AdminDesignation = Convert.ToString(dr["AdminDesignation"]);
                    objMTCE.InspectorBranch = Convert.ToString(dr["Branch_Name"]);
                  
                }
            }

            if (dsGetReportIAFScope.Tables[4].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[4].Rows)
                {
                   
                    objMTCE.ReportingPerson1Name = Convert.ToString(dr["ReportingPerson1Name"]);
                    objMTCE.ReportingPerson1Designation = Convert.ToString(dr["ReportingPerson1Designation"]);

                    objMTCE.ReportingPerson1Signature = Convert.ToString(dr["ReportingPerson1Signature"]);
                    
                }
            }
            else
            {
                
                objMTCE.ReportingPerson1Name = "";
               
                objMTCE.ReportingPerson1Designation = "";
                objMTCE.ReportingPerson1Signature = null;
                //objMTCE.ReportingPerson1Signature = Convert.ToByte("");
                //;
                //objMTCE.ReportingPerson1Signature = (byte[])(dsGetReportIAFScope.Tables[4].Columns["ReportingPerson1Signature"]);
            }

            //if (dsGetReportIAFScope.Tables[2].Rows.Count > 0)
            //{
            //    foreach (DataRow dr in dsGetReportIAFScope.Tables[2].Rows)
            //    {
            //        objMTCE.CreatedBy = Convert.ToString(dr["FullName"]);
            //        objMTCE.CreatedDate = Convert.ToString(dr["CreatedDate"]);
            //        objMTCE.Signature = Convert.ToString(dr["Signature"]);
            //        objMTCE.InspectorDesignation = Convert.ToString(dr["InspectorDesignation"]);
            //    }
            //}
            #endregion

            return View(objMTCE);

            
        }

        public ActionResult TechnicalCompetencyEvalReportsTestDownloadRahul(int? pktce, string InspectorId, string InspectorFullName, string GridHtml)
        {
            string d1 = ""; 
            string d2= ""; 
            string d3= ""; 
            string d4= ""; 
            string d5 = "";
            #region print return to next page
            string IFName = InspectorFullName;
            objMTCE.InspectorFullName = IFName;
            DataSet dsGetReportIAFScope = new DataSet();
            List<Report> lstTCE = new List<Report>();
            dsGetReportIAFScope = objTCE.test(InspectorId, Convert.ToInt32(pktce));
            if (dsGetReportIAFScope.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[0].Rows)
                {
                    lstTCE.Add(
                        new Report
                        {
                            IAFScope = Convert.ToString(dr["IAFScope"]),
                            InspectionName = Convert.ToString(dr["FieldOfInspection"]),
                            RangeInspection = Convert.ToString(dr["Type_RangeOfInspection"]),
                            AuthorizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            BasisOfAuthorization = Convert.ToString(dr["BasisOfAuthorization"])

                        }
                        );
                }
                ViewData["Test"] = lstTCE;
            }
            #endregion

            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;
            int count = 0;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/rahul.html")))
            {
                body = reader.ReadToEnd();
            }


            foreach (var v in lstTCE)
            {
                d1 += "<td>"+ v.IAFScope + "</td>" ;
                d2 += "<td>"+  v.InspectionName + "</td>";
                d3 += "<td>"+  v.RangeInspection + "</td>";
                d4 += "<td>"+  v.AuthorizationLevel + "</td>";
                d5 += "<td>" + v.BasisOfAuthorization + "</td>";
            }


            body = body.Replace("[d1]", d1);
            body = body.Replace("[d2]", d2);
            body = body.Replace("[d3]", d3);
            body = body.Replace("[d4]", d4);
            body = body.Replace("[d5]", d5);
            // body = body.Replace("[data2]", data2);
            //body = body.Replace("[data3]", data3);
            body = body.Replace("[Logo]", "http://localhost:54895/AllJsAndCss/images/logo.png");
            body = body.Replace("[Stamp]", "http://localhost:54895/Stamp.png");
            body = body.Replace("[Signature]", "http://localhost:54895/signature.jpg");



            strs.Append(body);
            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            string Vaibhav = null;

            //When javascript needs to be enabled
            converter.Options.JavaScriptEnabled = true;//(HtmlToPdfStartupMode)Enum.Parse(typeof(HtmlToPdfStartupMode),, true);

            converter.Options.MaxPageLoadTime = 180;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            PdfDocument doc = converter.ConvertHtmlString(body);
            string ReportName = Vaibhav + "/" + count + ".pdf";
            string path = Server.MapPath("~/TechnicalCompetencyEvaluationReport");
            doc.Save(path + '\\' + ReportName);
            doc.Close();

            return View(objMTCE);


        }


        #region Download pdf
        public ActionResult TechnicalCompetencyEvalReportsDownloadPdf(int? pktce, string InspectorId)
        {
            #region test

            DataSet dsGetReportIAFScope = new DataSet();
            List<Report> lstTCE = new List<Report>();
            dsGetReportIAFScope = objTCE.test(InspectorId, Convert.ToInt32(pktce));
            if (dsGetReportIAFScope.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetReportIAFScope.Tables[0].Rows)
                {
                    lstTCE.Add(
                        new Report
                        {
                            IAFScope = Convert.ToString(dr["IAFScope"]),
                            InspectionName = Convert.ToString(dr["FieldOfInspection"]),
                            RangeInspection = Convert.ToString(dr["Type_RangeOfInspection"]),
                            AuthorizationLevel = Convert.ToString(dr["AutharizationLevel"]),
                            BasisOfAuthorization = Convert.ToString(dr["BasisOfAuthorization"])

                        }
                        );
                }
                ViewData["Test"] = lstTCE;
            }
           

            
            return View();


            #endregion
        }
        #endregion

        public ActionResult Delete(string InspectorId)
        {
            string Result = string.Empty;
            try
            {
                Result = objTCE.Delete(InspectorId);
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
            return RedirectToAction("ListTechnicalCompetencyEvaluation");


        }


        public JsonResult Productsview(string categoryid)
        {

            //write your logic

            var Data = new { ok = true, catid = categoryid };

            return Json(Data, JsonRequestBehavior.AllowGet);
        }



        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();

            //Adding New Code 13 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<FileDetails> fileJobDetails = new List<FileDetails>();


            if (Session["listJobMasterUploadedFile"] != null)
            {
                fileJobDetails = Session["listJobMasterUploadedFile"] as List<FileDetails>;
            }


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
                        if (files.FileName.ToUpper().EndsWith(".XLSX") || files.FileName.ToUpper().EndsWith(".XLS") || files.FileName.ToUpper().EndsWith(".PDF") || files.FileName.ToUpper().EndsWith(".DOC") || files.FileName.ToUpper().EndsWith(".DOCX"))
                        ///|| files.FileName.ToUpper().EndsWith(".JPEG") || files.FileName.ToUpper().EndsWith(".JPG") || files.FileName.ToUpper().EndsWith(".PNG") || files.FileName.ToUpper().EndsWith(".gif") ||

                        {
                            string fileName = files.FileName;
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


                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;
                            // files.SaveAs(filePath);

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

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        public void Download(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objTCE.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }

        [HttpPost]
        public JsonResult DeleteFile(string id)
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
                DTGetDeleteFile = objTCE.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objTCE.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Content/JobDocument/"), id + fileDetails.Extension);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "ERROR" });
        }

        #region export to excel

        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TechnicalCompetencyEvaluation> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<TechnicalCompetencyEvaluation> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "TechnicalCompetencyEvaluation-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<TechnicalCompetencyEvaluation> CreateExportableGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<TechnicalCompetencyEvaluation> grid = new Grid<TechnicalCompetencyEvaluation>(GetData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            //added by nikita on 11092023
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(model => model.InspectorBranch).Titled("Branch");
            grid.Columns.Add(model => model.Email).Titled("Email");
            grid.Columns.Add(model => model.Mobile).Titled("Mobile");
            grid.Columns.Add(model => model.FormFilled).Titled("Form Filled");
            grid.Columns.Add(model => model.Verified).Titled("Verified");
            grid.Columns.Add(model => model.TiimesRole).Titled("Tiimes Role");
            grid.Columns.Add(model => model.EmployeeCategory).Titled("Employee Category");
            grid.Columns.Add(model => model.ModifiedBy).Titled("Modified By");
            grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");


            grid.Pager = new GridPager<TechnicalCompetencyEvaluation>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMTCE.lmd1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<TechnicalCompetencyEvaluation> GetData()
        {

            List<TechnicalCompetencyEvaluation> lmd = new List<TechnicalCompetencyEvaluation>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = objTCE.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TechnicalCompetencyEvaluation
                {
                    //added by nikita on 11092023
                    InspectorId = Convert.ToString(dr["PK_UserId"]),
                    InspectorName = Convert.ToString(dr["InspectorName"]),
                    Branch = Convert.ToString(dr["Branch_Name"]),
                    TiimesRole = Convert.ToString(dr["RoleName"]),
                    Mobile = Convert.ToString(dr["MobileNo"]),
                    Email = Convert.ToString(dr["Tuv_Email_Id"]),
                    EmployeeCategory = Convert.ToString(dr["EmployementCategory"]),
                    ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                    ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                    InspectorBranch = Convert.ToString(dr["Branch_Name"]),
                    FormFilled = Convert.ToString(dr["FormFilled"]),
                    Verified = Convert.ToString(dr["IsVerified"]),
                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),  //added by nikita on 11092023

                });
            }

            objMTCE.lmd1 = lmd;
            return objMTCE.lmd1;
        }

        #endregion

        [HttpGet]
        public ActionResult GetTrainingData(string InspectorName)
        {
            DataSet ds = new DataSet();
            ds = objTCE.GetDataTraining(InspectorName);
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}