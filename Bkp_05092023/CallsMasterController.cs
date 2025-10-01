using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.IO;
using System.Text;

using System.Net.Mail;
using System.Net;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;

//using System.Configuration;
//using System.IO;
namespace TuvVision.Controllers
{
    public class CallsMasterController : Controller
    {

        DateTime TodayDate = DateTime.Now;
        DateTime CheckPoDate = new DateTime();
        int ManDaysCount = 0;
        //int ManDay = 0;
        Double ManDay = 0;
        string PODate = string.Empty;
        string ManDays = string.Empty;
        string JobNumber = string.Empty;
        DataSet DSCheckValidCall = new DataSet();

        DALCalls objDalCalls = new DALCalls();
        CallsModel ObjModelsubJob = new CallsModel();
        abc ObjModel = new abc();
        JobMasters ObjModelJob = new JobMasters();
        Users ObjModelUsers = new Users();

        DALCallMaster objDAM = new DALCallMaster();
        DataSet dsEmailDetails = new DataSet();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        // GET: CallsMaster



        [HttpGet]
        public ActionResult InsertCalls(int? PK_SubJob_Id, int? PK_Call_ID)
        {





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



            var Data = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

            var ReasonData = objDalCalls.GetReasonList("CAN");
            ViewBag.Reasonlist = new SelectList(ReasonData, "CReason_Id", "Reason");


            var UserData = objDalCalls.GetInspectorList();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            DataSet DSGetEmployeeType = new DataSet();
            DSGetEmployeeType = objDalEnquiryMaster.GetEmployeeTypeLst();
            List<NameCode> lstEmployeetType = new List<NameCode>();

            if (DSGetEmployeeType.Tables[0].Rows.Count > 0)
            {
                lstEmployeetType = (from n in DSGetEmployeeType.Tables[0].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEmployeeType.Tables[0].Columns["ProjectName"].ToString()),
                                        Code = n.Field<Int32>(DSGetEmployeeType.Tables[0].Columns["PK_ID"].ToString())
                                    }).ToList();
            }

            IEnumerable<SelectListItem> EmployeeType;
            EmployeeType = new SelectList(lstEmployeetType, "Code", "Name");
            ViewBag.ExeService = EmployeeType;
            ViewData["ExeService"] = EmployeeType;





            if (PK_SubJob_Id != 0 && PK_SubJob_Id != null)
            {
                JobNumber = Convert.ToString(Session["JobNumber"]);
                /*******************Multiple Job List***************************/


                DataTable DTGetJobMasterLst = new DataTable();
                List<CallsModel> lstJobByJobNo = new List<CallsModel>();
                DTGetJobMasterLst = objDAM.getJoblistforEdit(PK_SubJob_Id);

                if (DTGetJobMasterLst.Rows.Count > 0)
                {
                    lstJobByJobNo = (from n in DTGetJobMasterLst.AsEnumerable()
                                     select new CallsModel()
                                     {
                                         DJob_No = n.Field<string>(DTGetJobMasterLst.Columns["Name"].ToString()),
                                         DPK_Job_Id = n.Field<int>(DTGetJobMasterLst.Columns["code"].ToString())

                                     }).ToList();
                }

                IEnumerable<SelectListItem> JAuditorName;
                JAuditorName = new SelectList(lstJobByJobNo, "DAuditorCode", "DAuditorName");

                ViewBag.MultiJobNos = lstJobByJobNo;
                ViewData["JobNos"] = lstJobByJobNo;


                /*******************Multiple Job List Over ***************************/


                #region Bind Drop for subjobno list by job no
                DataSet dsSubJobList = new DataSet();
                //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
                List<CallsModel> lstSubJobByJobNo = new List<CallsModel>();
                dsSubJobList = objDalCalls.BindSubJobByControlNo(JobNumber);

                if (dsSubJobList.Tables[0].Rows.Count > 0)
                {
                    lstSubJobByJobNo = (from n in dsSubJobList.Tables[0].AsEnumerable()
                                        select new CallsModel()
                                        {
                                            DSubJob_No = n.Field<string>(dsSubJobList.Tables[0].Columns["SubJob_No"].ToString()),
                                            DPK_SubJob_Id = n.Field<int>(dsSubJobList.Tables[0].Columns["PK_SubJob_Id"].ToString())

                                        }).ToList();
                }

                IEnumerable<SelectListItem> AuditorName;
                AuditorName = new SelectList(lstSubJobByJobNo, "DAuditorCode", "DAuditorName");
                //ViewBag.AuditorName = AuditorName;
                //ViewData["AuditorName"] = AuditorName;
                ViewData["SubJob_NoByJobNo"] = lstSubJobByJobNo;
                #endregion




                DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                {
                    CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                    ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                    //ManDay = Convert.ToInt32(ManDays);
                    ManDay = Convert.ToDouble(ManDays);

                }
                if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                {
                    ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                }
                if (CheckPoDate >= TodayDate)
                {
                    if (ManDay > ManDaysCount)
                    {



                        if (PK_SubJob_Id != 0 && PK_SubJob_Id != null)
                        {
                            DataSet DSJobMasterByQtId = new DataSet();
                            DataSet DSEditQutationTabledata = new DataSet();
                            DataSet DSEditVendorpo = new DataSet();
                            DSEditQutationTabledata = objDalCalls.GetQutationDetails(PK_SubJob_Id);
                            // DSJobMasterByQtId = objDalSubjob.CheckQutationdata(PK_JOB_ID);
                            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                            {
                                ObjModelsubJob.POAmountGreaterThan = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["P"]);
                                if (ObjModelsubJob.POAmountGreaterThan == "Yes")
                                {
                                    ObjModelsubJob.POAmountGreaterThan = "80% of PO amount has been consumed, inform to all concern persons.";
                                }
                                else
                                {
                                    ObjModelsubJob.POAmountGreaterThan = "";
                                }
                                ObjModelsubJob.MandaysConsumedValidity = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CalculateMandays"]);

                                ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_SubJob_Id"]);
                                ObjModelsubJob.Company_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Company_Name"]);
                                ObjModelsubJob.Status = "Open";
                                ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_JOB_ID"]);
                                ObjModelsubJob.Type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Service_type"]);
                                //ObjModelsubJob.Executing_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);

                                int v = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Br_Id"]);
                                #region bind inspector list
                                if (v >= 0)
                                {
                                    var UserDataByExecuting_Branch = objDalCalls.GetInsByExBr(v);
                                    ViewBag.Userlist = new SelectList(UserDataByExecuting_Branch, "PK_UserID", "FirstName");
                                }

                                #endregion

                                ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["vendor_name"]); ///Second Level Vendor Name
                                ObjModelsubJob.PO_Number = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Po_No"]); ///Second Level Vendor PO
                                ObjModelsubJob.SubVendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPODate"]); ///Second Level Vendor PO Date

                                ObjModelsubJob.TopSubVendorName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopVendor"]); /// First level Vendor Name
                                ObjModelsubJob.TopSubVendorPONo = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopVendorPO"]); /// First level Vendor PO
                                ObjModelsubJob.TopvendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopvendorPODate"]); /// First level Vendor PO date


                                ObjModelsubJob.Originating_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch"]);
                                ObjModelsubJob.Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Control_Number"]);
                                ObjModelsubJob.Sub_Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubJob_No"]);
                                ObjModelsubJob.SubSubJobNo = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubSubJob_No"]);
                                ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                                ObjModelsubJob.End_Customer = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["End_User"]);


                                ObjModelsubJob.Job_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Address"]);
                              //  ObjModelsubJob.Br_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Br_Id"]);

                                ObjModelsubJob.Client_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Email"]);
                                ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Email"]);
                                ObjModelsubJob.Tuv_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Tuv_Email"]);

                                ObjModelsubJob.Client_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Contact"]);
                                ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                                ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Contact"]);

                                ObjModelsubJob.CompetencyCheck = true;
                                ObjModelsubJob.ImpartialityCheck = true;
                                if (DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch_EmailID"] != null)
                                {
                                    // ObjModelsubJob.Tuv_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch_EmailID"]);
                                    Session["Originating_Branch_EmailID"] = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch_EmailID"]);
                                }
                                string SSJobType = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Type"]);
                                if (SSJobType == "SubSub Job")
                                {
                                    DSEditVendorpo = objDalCalls.GetVendorPo(ObjModelsubJob.Sub_Job);
                                    if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                                    {
                                        ObjModelsubJob.Po_No_SSJob = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Po_No"]);
                                    }
                                }
                                ObjModelsubJob.ExecutingService = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PortID"]);
                                ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                                ObjModelsubJob.DECName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECname"]);
                                ObjModelsubJob.DECNumber = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECNumber"]);
                                ObjModelsubJob.chkARC = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["chkARC"]);
                                ObjModelsubJob.SAP_no = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SAP_No"]);
                                ObjModelsubJob.MandayRate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["OrderRate"]);


                            }
                            Session["CallLimit"] = null;
                            //**********************************************Code Added by Manoj Sharma for Delete file and update file
                            DataTable DTGetUploadedFile = new DataTable();
                            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                            PK_Call_ID = ObjModelsubJob.PK_Call_ID;
                            if (PK_Call_ID != null && PK_Call_ID != 0)
                            {
                                DTGetUploadedFile = objDalCalls.EditUploadedFile(PK_Call_ID);
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
                                    ObjModelsubJob.FileDetails = lstEditFileDetails;
                                }
                            }
                            //**********************************************Code Added by Manoj Sharma for Delete file and update file


                            return View(ObjModelsubJob);
                        }
                    }
                    else
                    {
                        //Session["CallLimit"] = "You cant not create call, because call limit has been end";
                        Session["CallLimit"] = "Call Creation is restricted. Check available Mandays or PO validity.";
                        return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                    }
                }
                else
                {
                    //Session["CallLimit"] = "You cant not create call, because call limit has been end";
                    Session["CallLimit"] = "Call Creation is restricted. Check available Mandays or PO validity.";
                    return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                }
            }

            if (PK_Call_ID != 0)
            {
                ViewBag.check = "productcheck";
                ViewBag.Jobcheck = "JobCheck";
                string[] splitedProduct_Name;
                string[] JobsplitedProduct_Name;

                DataSet DSJobMasterByQtId = new DataSet();
                DataSet DSEditQutationTabledata = new DataSet();
                DSEditQutationTabledata = objDalCalls.EditCall(PK_Call_ID);

                // DSJobMasterByQtId = objDalSubjob.CheckQutationdata(PK_JOB_ID);

                if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Status"]);
                    // ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Br_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Executing_Branch"]);
                    ObjModelsubJob.Originating_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch"]);
                    ObjModelsubJob.Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job"]);
                    ObjModelsubJob.Sub_Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Job"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.End_Customer = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["End_Customer"]);

                    ObjModelsubJob.PK_Call_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelsubJob.Contact_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Contact_Name"]);


                    ObjModelsubJob.Call_Recived_date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Recived_date"]);
                    ObjModelsubJob.Call_Request_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Request_Date"]);
                    ObjModelsubJob.Planned_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Planned_Date"]);
                    // ObjModelsubJob.From_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["From_Date"]);
                    //ObjModelsubJob.To_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["To_Date"]);

                    ObjModelsubJob.FromDateNew = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["From_Date"]);
                    ObjModelsubJob.ToDateNew = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["To_Date"]);

                    /* ObjModelsubJob.Call_Recived_date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Recived_date"]);
                     ObjModelsubJob.Call_Request_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Request_Date"]);
                     ObjModelsubJob.Planned_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Planned_Date"]);
                     ObjModelsubJob.From_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["From_Date"]);
                     ObjModelsubJob.To_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["To_Date"]);*/



                    ObjModelsubJob.Source = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Source"]);

                    ObjModelsubJob.Urgency = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Urgency"]);
                    ObjModelsubJob.Competency_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Competency_Check"]);
                    ObjModelsubJob.Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Category"]);
                    ObjModelsubJob.Sub_Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Category"]);
                    ObjModelsubJob.Empartiality_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Empartiality_Check"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Job_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job_Location"]);
                    ObjModelsubJob.Final_Inspection = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Final_Inspection"]);

                    ObjModelsubJob.FirstName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Inspector"]);
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Tuv_Branch"]);

                    ObjModelsubJob.Homecheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["Homecheckbox"]);
                    ObjModelsubJob.Vendorcheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["Vendorcheckbox"]);
                    ObjModelsubJob.ClientEmailcheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["ClientEmailcheckbox"]);

                    ObjModelsubJob.Call_No = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_No"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Attachment"]);

                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Contact"]);


                    ObjModelsubJob.Description = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Description"]);
                    ObjModelsubJob.Quantity = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Quantity"]);


                    ///// SubSub Vendor For /1/1 Sub SubJob
                    ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorName"]);
                    ObjModelsubJob.Po_No_SSJob = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Po_No_SSJob"]);
                    ObjModelsubJob.SubVendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPODate"]);

                    ///// Main Vendor For /1 SubJob
                    ObjModelsubJob.TopSubVendorName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_name"]);
                    ObjModelsubJob.TopSubVendorPONo = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPONo"]);
                    ObjModelsubJob.TopvendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopvendorPODate"]);


                    ObjModelsubJob.EstimatedHours = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["EstimatedHours"]);
                    ObjModelsubJob.ExecutingService = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["ExecutingService"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["checkIFCustomerSpecific"]);

                    int CompetencyCheck = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["CompetencyCheck"]);
                    ObjModelsubJob.CompetencyCheck = Convert.ToBoolean(CompetencyCheck);
                    int ImpartialityCheck = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["ImpartialityCheck"]);
                    ObjModelsubJob.ImpartialityCheck = Convert.ToBoolean(ImpartialityCheck);
                    int FinalInspection = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FinalInspection"]);
                    ObjModelsubJob.FinalInspection = Convert.ToBoolean(FinalInspection);

                    int chkContinuousCall = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["chkContinuousCall"]);
                    ObjModelsubJob.ChkContinuousCall = Convert.ToBoolean(chkContinuousCall);

                    int ChkMultipleSubJobNo = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["ChkMultipleSubJobNo"]);
                    ObjModelsubJob.ChkMultipleSubJobNo = Convert.ToBoolean(ChkMultipleSubJobNo);

                    if (ObjModelsubJob.Status == "Cancelled")
                    {
                        int ChkcancelledCall = 1;
                        ObjModelsubJob.chkCallCancelled = Convert.ToBoolean(ChkcancelledCall);
                    }
                    else
                    {
                        int ChkcancelledCall = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["chkCallCancelled"]);
                        ObjModelsubJob.chkCallCancelled = Convert.ToBoolean(ChkcancelledCall);
                    }
                    ObjModelsubJob.Reasion = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Reasion"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECname"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECNumber"]);

                    ObjModelsubJob.chkARC = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.SAP_no = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SAP_No"]);

                    ObjModelsubJob.MandayRate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["MandayRate"]);
                    ObjModelsubJob.inspectorCompetant = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["iscompetant"]);

                    /*******************Multiple Job List***************************/


                    DataTable DTGetJobMasterLst = new DataTable();
                    List<CallsModel> lstJobByJobNo = new List<CallsModel>();
                    DTGetJobMasterLst = objDAM.getJoblistforEdit(ObjModelsubJob.PK_SubJob_Id);

                    if (DTGetJobMasterLst.Rows.Count > 0)
                    {
                        lstJobByJobNo = (from n in DTGetJobMasterLst.AsEnumerable()
                                         select new CallsModel()
                                         {
                                             DJob_No = n.Field<string>(DTGetJobMasterLst.Columns["Name"].ToString()),
                                             DPK_Job_Id = n.Field<int>(DTGetJobMasterLst.Columns["code"].ToString())

                                         }).ToList();
                    }

                    IEnumerable<SelectListItem> JAuditorName;
                    JAuditorName = new SelectList(lstJobByJobNo, "DAuditorCode", "DAuditorName");

                    ViewBag.MultiJobNos = lstJobByJobNo;
                    ViewData["JobNos"] = lstJobByJobNo;


                    /*******************Multiple Job List Over ***************************/


                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Product_item"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;



                    List<string> JobSelected = new List<string>();
                    var JobExistingins = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["JobList"]);
                    JobsplitedProduct_Name = JobExistingins.Split(',');

                    foreach (var Jobsingle in JobsplitedProduct_Name)
                    {
                        JobSelected.Add(Jobsingle);
                    }
                    ViewBag.EditstrJobproductName = JobExistingins;
                    ViewBag.EditJobproductName = JobSelected;

                    int ChkMultipleJobNo = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["ChkMultipleJobNo"]);
                    ObjModelsubJob.ChkMultipleJobNo = Convert.ToBoolean(ChkMultipleJobNo);
                    ObjModelsubJob.CallReceiveTime = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CallReceiveTime"]);
                }

                //********************************************** Code Added by Manoj Sharma for Delete file and update file
                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalCalls.EditUploadedFile(PK_Call_ID);
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
                    ObjModelsubJob.FileDetails = lstEditFileDetails;
                }
                //**********************************************Code Added by Manoj Sharma for Delete file and update file

                DataTable dtrevise = new DataTable();
                List<CallsModel> lstCallDetails = new List<CallsModel>();
                dtrevise = objDalCalls.CallReviseData(PK_Call_ID);
                if (dtrevise.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtrevise.Rows)
                    {
                        lstCallDetails.Add(
                           new CallsModel
                           {
                               PK_AddCallID = Convert.ToInt32(dr["PK_AddCallID"]),
                               NewPlannedDate = Convert.ToString(dr["NewPlannedDate"]),
                               Remark = Convert.ToString(dr["Remark"]),
                               Reason = Convert.ToString(dr["Reason"]),
                               ActionSelected = Convert.ToString(dr["ActionSelected"]),
                           }
                         );
                    }

                }
                ViewData["Callrevise"] = lstCallDetails;
                ObjModelsubJob.lstAddCallDetails = lstCallDetails;



                return View(ObjModelsubJob);
            }
            else
            {
                return RedirectToAction("CallsList");
            }

        }

        [HttpPost]
        public ActionResult InsertCalls(CallsModel CM, HttpPostedFileBase[] Image, List<HttpPostedFileBase> img_Banner, FormCollection fc)
        {
            string ProList = string.Join(",", fc["ProductList"]);
            CM.ProductList = ProList;

            string MultiJobList = string.Join(",", fc["MultiJob"]);
            CM.JobList = MultiJobList;


            #region
            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            try
            {
                List<string> lstAttachment = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                    {
                        if (single != null && single.FileName != "")
                        {
                            var filename = CommonControl.FileUpload("~/Content/Uploads/Images/", single);
                            lstAttachment.Add(filename);
                        }
                    }
                    CM.Attachment = string.Join(",", lstAttachment);
                    if (string.IsNullOrEmpty(CM.Attachment))
                    {
                        CM.Attachment = "NoImage.gif";
                    }
                }
                else
                {
                    CM.Attachment = "NoImage.gif";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #endregion

            CM.Attachment = IPath;

            int CallID = 0;

            string Result = string.Empty;
            string newplannedDate = string.Empty;
            string ListofCall = string.Empty;
            //Code Added by Manoj Sharma For Multiple File uploading 13 March 2020
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listCallMastUploadedFile"] as List<FileDetails>;

            var UserData = objDalCalls.GetInspectorList();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");
            try
            {
                JobNumber = Convert.ToString(CM.Job);

                #region


                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                int days = 0;
                if (CM.ChkContinuousCall == true)
                {
                    if (CM.FromDateNew != null && CM.ToDateNew != null)
                    {
                        DateTime dt1 = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo);
                        DateTime dt2 = DateTime.ParseExact(CM.ToDateNew, "dd/MM/yyyy", theCultureInfo);

                        TimeSpan ts = dt2.Subtract(dt1);


                        if (ts.Days != 0)
                        {
                            days = ts.Days + 1;
                        }
                        else
                        {

                        }
                    }

                }


                #endregion
                newplannedDate = CM.Planned_Date;

                if (CM.PK_Call_ID == 0)
                {
                    if (CM.ChkMultipleSubJobNo == true) //For Multiple Sub Job No
                    {
                        string ListPKSubJob = string.Join(",", fc["AuditeeName"]);
                        string[] split = ListPKSubJob.Split(',');

                        foreach (string item in split)
                        {
                            ListofCall = string.Empty; //bind call no for each sub job 							  
                            CM.PK_SubJob_Id = Convert.ToInt32(item);



                            if (days > 0 && CM.PK_Call_ID == 0) //Countinuous call
                            {
                                for (int i = 1; i <= days; i++)
                                {
                                    string dates = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).ToString();

                                    CM.ApplyDate = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).ToString();

                                    #region InsertCode
                                    int _min = 10000;
                                    int _max = 99999;
                                    Random _rdm = new Random();
                                    int Rjno = _rdm.Next(_min, _max);
                                    string ConfirmCode = Convert.ToString(Rjno);

                                    int _mins = 100000;
                                    int _maxs = 999999;
                                    Random _rdms = new Random();
                                    int Rjnos = _rdm.Next(_mins, _maxs);
                                    string ConfirmSecondCode = Convert.ToString(Rjnos);

                                    DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                                    if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                                    {
                                        CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                                        ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                                        ManDay = Convert.ToDouble(ManDays);//3/1/2022

                                    }
                                    if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                                    {
                                        ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                                    }
                                    if (CheckPoDate >= TodayDate)
                                    {
                                        if (ManDay > ManDaysCount)
                                        {
                                            //CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                            CM.Executing_Branch = Convert.ToString(CM.Br_Id);

                                            if (CM.FirstName != null)
                                            {
                                                if (i == 1)
                                                {
                                                    CM.Call_Request_Date = CM.FromDateNew;
                                                    // CM.Actual_Visit_Date = CM.FromDateNew;
                                                    CM.Actual_Visit_Date = newplannedDate.ToString();
                                                    CM.Planned_Date = newplannedDate.ToString();

                                                    CM.From_Date = CM.FromDateNew;
                                                    CM.To_Date = CM.ToDateNew;
                                                }
                                                else
                                                {
                                                    CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    //CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                    CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                                }

                                                CM.Inspector = CM.FirstName;
                                                CM.AssignStatus = "1";
                                                //CM.Status = "Open";
                                                CM.Status = "Assigned";


                                            }
                                            else
                                            {
                                                CM.AssignStatus = "0";
                                                CM.Status = "Open";
                                                if (CM.FromDateNew != null)
                                                {
                                                    if (i == 1)
                                                    {
                                                        CM.Call_Request_Date = CM.FromDateNew;
                                                        // CM.Actual_Visit_Date = CM.FromDateNew;
                                                        CM.Actual_Visit_Date = newplannedDate;
                                                        CM.Planned_Date = newplannedDate;
                                                        CM.From_Date = CM.FromDateNew;
                                                        CM.To_Date = CM.ToDateNew;
                                                    }
                                                    else
                                                    {
                                                        CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        //CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                        //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                        CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                        CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                                    }
                                                }

                                            }

                                            Result = objDalCalls.InsertUpdateCalls(CM);


                                            //Code Added by Manoj Sharma 13 March 2020 for Multiple File Uploaded
                                            CallID = Convert.ToInt32(Result);


                                            if (CallID != null && CallID != 0)
                                            {
                                                if (lstFileDtls != null && lstFileDtls.Count > 0)
                                                {
                                                    Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                                    Session["listCallMastUploadedFile"] = null;
                                                }
                                            }



                                            #region

                                            CallsModel IVRNew = new CallsModel();

                                            IVRNew.PK_Call_ID = Convert.ToInt32(Result);

                                            if (ListofCall == string.Empty)
                                            {
                                                ListofCall = IVRNew.PK_Call_ID.ToString();
                                            }
                                            else
                                            {
                                                ListofCall = ListofCall + "," + IVRNew.PK_Call_ID.ToString();
                                            }

                                            //  string[] split = ListPKSubJob.Split(',');

                                            #endregion
                                            /*if (CM.FirstName != null)
                                            {
                                                dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(IVRNew.PK_Call_ID));
                                                if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                                {
                                                    CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                                }
                                                if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                                {
                                                    CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                                }
                                                if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                                {
                                                    CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                                }

                                                if (CM.FirstName != null)
                                                {
                                                    CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                                }
                                            }*/

                                            var Data = objDalCalls.GetBranchList();
                                            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
                                            TempData["InsertCall"] = Result;
                                            if (Result != "" && Result != null)
                                            {
                                                TempData["InsertCall"] = Result;


                                            }
                                        }
                                        else
                                        {
                                            Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                            return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                        }
                                    }
                                    else
                                    {
                                        Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                        return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                    }

                                    #endregion
                                }
                                #region Test Mail 2 march 2021
                                if (days > 0)
                                {
                                    string[] splitcalls = ListofCall.Split(',');
                                    if (CM.FirstName != null)
                                    {

                                        dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(splitcalls[0]));



                                        if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                        {
                                            ContinuousCallMail(ListofCall, CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                        }
                                        if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                        {
                                            ContinuousCallMail(ListofCall, CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                        }
                                        if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                        {
                                            ContinuousCallMail(ListofCall, CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                        }

                                        if (CM.FirstName != null)
                                        {
                                            ContinuousCallMail(ListofCall, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                        }
                                    }
                                }
                            }
                            else  //single call
                            {
                                DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                                if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                                {
                                    CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                                    ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                                    ManDay = Convert.ToDouble(ManDays);//3/01/2022

                                }
                                if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                                {
                                    ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                                }
                                if (CheckPoDate >= TodayDate)
                                {
                                    if (ManDay > ManDaysCount)
                                    {
                                        #region InsertCode



                                        int _min = 10000;
                                        int _max = 99999;
                                        Random _rdm = new Random();
                                        int Rjno = _rdm.Next(_min, _max);
                                        string ConfirmCode = Convert.ToString(Rjno);

                                        int _mins = 100000;
                                        int _maxs = 999999;
                                        Random _rdms = new Random();
                                        int Rjnos = _rdm.Next(_mins, _maxs);
                                        string ConfirmSecondCode = Convert.ToString(Rjnos);

                                        // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                        CM.Executing_Branch = Convert.ToString(CM.Br_Id);


                                        if (CM.FirstName != null)
                                        {
                                            CM.Inspector = CM.FirstName;
                                            CM.Actual_Visit_Date = CM.Planned_Date;
                                            CM.AssignStatus = "1";
                                            //CM.Status = "Open";
                                            CM.Status = "Assigned";
                                        }
                                        else
                                        {
                                            CM.AssignStatus = "0";
                                            CM.Status = "Open";
                                            CM.Actual_Visit_Date = CM.Planned_Date;
                                        }

                                        Result = objDalCalls.InsertUpdateCalls(CM);
                                        CallID = Convert.ToInt32(Result);

                                        if (CallID != null && CallID != 0)
                                        {
                                            if (lstFileDtls != null && lstFileDtls.Count > 0)
                                            {
                                                Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                                Session["listCallMastUploadedFile"] = null;
                                            }
                                        }

                                        if (CM.FirstName != null)
                                        {
                                            dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(Result));
                                            if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                            {
                                                //ServicebookingMail(CM,CM.Tuv_Branch, CM.FirstName);  //------------------Emails
                                                CallManagementMail(Convert.ToInt32(Result), CM.Tuv_Branch.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                            }
                                            if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(Result), CM.Vendor_Email.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo); //------------------Emails
                                            }
                                            if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(Result), CM.Client_Email.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                            }


                                            if (CM.FirstName != null)
                                            {
                                                // ServicebookingMail(CM,dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), CM.FirstName);  //------------------Emails
                                                CallManagementMail(Convert.ToInt32(Result), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                            }

                                        }

                                        var Data = objDalCalls.GetBranchList();
                                        ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

                                        if (Result != "" && Result != null)
                                        {
                                            #region vaibhav code to resolve null reference error in Product Item //on return need to chk callid and bind selected item Product Item
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
                                            #endregion

                                            TempData["InsertCall"] = Result;

                                            // return View(CM);//13-2-2020
                                            // return RedirectToAction("CallsList");
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                        return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                    }
                                }
                                else
                                {
                                    Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                    return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                }
                            }

                            #endregion
                        }

                    }
                    else if (days > 0 && CM.PK_Call_ID == 0) //Countinuous call
                    {
                        for (int i = 1; i <= days; i++)
                        {
                            //   string dates = Convert.ToDateTime(CM.FromDateNew).ToString("dd/mm/yyyy");
                            CM.ApplyDate = Convert.ToString(CM.FromDateNew);
                            #region InsertCode
                            int _min = 10000;
                            int _max = 99999;
                            Random _rdm = new Random();
                            int Rjno = _rdm.Next(_min, _max);
                            string ConfirmCode = Convert.ToString(Rjno);

                            int _mins = 100000;
                            int _maxs = 999999;
                            Random _rdms = new Random();
                            int Rjnos = _rdm.Next(_mins, _maxs);
                            string ConfirmSecondCode = Convert.ToString(Rjnos);
                            DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                            if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                            {
                                CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                                ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                                ManDay = Convert.ToDouble(ManDays);

                            }
                            if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                            {
                                ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                            }
                            if (CheckPoDate > TodayDate)
                            {
                                if (ManDay > ManDaysCount)
                                {
                                    // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                    CM.Executing_Branch = Convert.ToString(CM.Br_Id);

                                    if (CM.FirstName != null)
                                    {
                                        if (i == 1)
                                        {
                                            CM.Call_Request_Date = CM.FromDateNew;
                                            CM.Actual_Visit_Date = newplannedDate;
                                            CM.Planned_Date = newplannedDate;
                                            CM.From_Date = CM.FromDateNew;
                                            CM.To_Date = CM.ToDateNew;
                                        }
                                        else
                                        {
                                            CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");                                            
                                            //CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                            CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                            CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                        }

                                        CM.Inspector = CM.FirstName;
                                        CM.AssignStatus = "1";
                                        //CM.Status = "Open";
                                        CM.Status = "Assigned";

                                    }
                                    else
                                    {
                                        CM.AssignStatus = "0";
                                        CM.Status = "Open";
                                        if (CM.FromDateNew != null)
                                        {
                                            if (i == 1)
                                            {
                                                CM.Call_Request_Date = CM.FromDateNew;
                                                //CM.Actual_Visit_Date = CM.FromDateNew; // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                //CM.Planned_Date = CM.Planned_Date; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.Actual_Visit_Date = newplannedDate;
                                                CM.Planned_Date = newplannedDate;
                                                CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                            }
                                            else
                                            {
                                                CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020

                                            }
                                        }

                                    }

                                    Result = objDalCalls.InsertUpdateCalls(CM);

                                    //Code Added by Manoj Sharma 13 March 2020 for Multiple File Uploaded
                                    CallID = Convert.ToInt32(Result);
                                    if (CallID != null && CallID != 0)
                                    {
                                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                                        {
                                            Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                            Session["listCallMastUploadedFile"] = null;
                                        }
                                    }

                                    //DateTime actualdata = Convert.ToDateTime(dates).AddDays(1);
                                    // CM.From_Date = Convert.ToString(actualdata);
                                    #region
                                    //int id = objDalCalls.InsertExtendCalls(CM);
                                    CallsModel IVRNew = new CallsModel();
                                    //  IVRNew.Call_No = DateTime.Now.Year + "/00" + Result; //Call No Generated
                                    IVRNew.PK_Call_ID = Convert.ToInt32(CallID);
                                    if (ListofCall == string.Empty)
                                    {
                                        ListofCall = IVRNew.PK_Call_ID.ToString();
                                    }
                                    else
                                    {
                                        ListofCall = ListofCall + "," + IVRNew.PK_Call_ID.ToString();
                                    }
                                    // objDalCalls.UpdateExtendCalls(IVRNew);  //Update call Id Comment by Vaibhav Generated from store procedure
                                    #endregion
                                    /*if (CM.FirstName != null)
                                    {
                                        if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                        {
                                            ServicebookingMail(IVRNew, CM.Tuv_Branch, CM.FirstName);  //------------------Emails
                                        }
                                        if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                        {
                                            ServicebookingMail(IVRNew, CM.Vendor_Email, CM.FirstName);  //------------------Emails
                                        }
                                        if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                        {
                                            ServicebookingMail(IVRNew, CM.Client_Email, CM.FirstName);  //------------------Emails
                                        }
                                        dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(IVRNew.PK_Call_ID));
                                        if (CM.FirstName != null)
                                        {
                                            ServicebookingMail(IVRNew, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), CM.FirstName);  //------------------Emails
                                        }
                                    }*/

                                    var Data = objDalCalls.GetBranchList();
                                    ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
                                    TempData["InsertCall"] = Result;
                                    if (Result != "" && Result != null)
                                    {
                                        TempData["InsertCall"] = Result;

                                        // return RedirectToAction("CallsList");
                                    }
                                }
                                else
                                {
                                    Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                    return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                }
                            }
                            else
                            {
                                Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                            }
                            #endregion
                        }
                        #region Countinuous call mail vaibhav
                        if (days > 0)
                        {
                            string[] splitcalls = ListofCall.Split(',');
                            if (CM.FirstName != null)
                            {
                                dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(splitcalls[0]));
                                if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                {
                                    ContinuousCallMail(ListofCall, CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                }
                                if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                {
                                    ContinuousCallMail(ListofCall, CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                }
                                if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                {
                                    ContinuousCallMail(ListofCall, CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                }

                                if (CM.FirstName != null)
                                {
                                    ContinuousCallMail(ListofCall, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                }
                            }
                        }


                        #endregion												
                    }
                    else  //single call
                    {
                        #region InsertCode



                        int _min = 10000;
                        int _max = 99999;
                        Random _rdm = new Random();
                        int Rjno = _rdm.Next(_min, _max);
                        string ConfirmCode = Convert.ToString(Rjno);

                        int _mins = 100000;
                        int _maxs = 999999;
                        Random _rdms = new Random();
                        int Rjnos = _rdm.Next(_mins, _maxs);
                        string ConfirmSecondCode = Convert.ToString(Rjnos);
                        DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                        if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                        {
                            CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                            ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                            ManDay = Convert.ToDouble(ManDays);

                        }
                        if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                        {
                            ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                        }
                        if (CheckPoDate > TodayDate)
                        {
                            if (ManDay > ManDaysCount)
                            {
                                // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                CM.Executing_Branch = Convert.ToString(CM.Br_Id);


                                if (CM.FirstName != null)
                                {
                                    CM.Inspector = CM.FirstName;
                                    CM.Actual_Visit_Date = CM.Planned_Date;
                                    CM.AssignStatus = "1";
                                    //CM.Status = "Open";
                                    CM.Status = "Assigned";

                                }
                                else
                                {
                                    CM.AssignStatus = "0";
                                    CM.Status = "Open";
                                    CM.Actual_Visit_Date = CM.Planned_Date;
                                }

                                Result = objDalCalls.InsertUpdateCalls(CM);

                                CallID = Convert.ToInt32(Result);

                                if (CallID != null && CallID != 0)
                                {
                                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                                    {
                                        Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                        Session["listCallMastUploadedFile"] = null;
                                    }
                                }

                                if (CM.FirstName != null)
                                {
                                    dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(Result));
                                    if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                    {
                                        CallManagementMail(Convert.ToInt32(Result), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                    }
                                    if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                    {
                                        CallManagementMail(Convert.ToInt32(Result), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                    }
                                    if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                    {
                                        CallManagementMail(Convert.ToInt32(Result), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo); //------------------Emails
                                    }

                                    if (CM.FirstName != null)
                                    {
                                        CallManagementMail(Convert.ToInt32(Result), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                    }
                                }

                                var Data = objDalCalls.GetBranchList();
                                ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

                                if (Result != "" && Result != null)
                                {
                                    #region vaibhav code to resolve null reference error in Product Item //on return need to chk callid and bind selected item Product Item
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
                                    #endregion

                                    TempData["InsertCall"] = Result;

                                    // return View(CM);//13-2-2020
                                    return RedirectToAction("CallsList");
                                }
                            }
                            else
                            {
                                Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                            }
                        }
                        else
                        {
                            Session["CallLimit"] = "You cant not create call, because call limit has been end";
                            return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                        }
                        #endregion
                    }

                }
                else
                {
                    //////Update Calls 
                    string prevInspector = string.Empty;

                    DSCheckValidCall = objDalCalls.GetInspectorName(CM.PK_Call_ID);

                    prevInspector = DSCheckValidCall.Tables[0].Rows[0]["Inspector"].ToString();

                    DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                    if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                    {
                        CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                        ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                        ManDay = Convert.ToInt32(ManDays);

                    }
                    if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                    {
                        ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                    }
                    //if (CheckPoDate > TodayDate)
                    //  {
                    //if (ManDay > ManDaysCount)
                    //{
                    CM.Actual_Visit_Date = CM.Planned_Date;

                    if (CM.FirstName != null && CM.Status != "Closed")
                    {
                        CM.AssignStatus = "1";
                        CM.Status = "Assigned";
                        if (CM.chkCallCancelled == true)
                        {
                            CM.Status = "Cancelled";
                        }
                    }
                    else if (CM.FirstName != null && CM.Status == "Closed")
                    {

                        if (CM.chkCallCancelled == true)
                        {

                            CM.Status = "Cancelled";
                        }
                    }
                    else
                    {
                        CM.AssignStatus = "0";
                        CM.Status = "Open";
                        if (CM.chkCallCancelled == true)
                        {
                            CM.Status = "Cancelled";
                        }


                    }

                    Result = objDalCalls.InsertUpdateCalls(CM);

                    //Code Added by Manoj Sharma 13 March 2020 for Multiple File Uploaded
                    CallID = Convert.ToInt32(Result);
                    if (CallID != null && CallID != 0)
                    {
                        if (CallID == -1)
                        {
                            CallID = CM.PK_Call_ID;

                        }
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                            Session["listCallMastUploadedFile"] = null;
                        }

                        /////Send Mail after Updating Inspector
                        if (prevInspector != string.Empty)
                        {
                            if (CM.FirstName != null && prevInspector != CM.FirstName)
                            {
                                dsEmailDetails = objDalCalls.GetEmailDetails(CallID);
                                if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                {
                                    CallUpdateMail(Convert.ToInt32(CallID), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), prevInspector);  //------------------Emails
                                }
                                if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                {
                                    CallUpdateMail(Convert.ToInt32(CallID), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), prevInspector);  //------------------Emails
                                }
                                if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                {
                                    CallUpdateMail(Convert.ToInt32(CallID), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), prevInspector);  //------------------Emails
                                }

                                if (CM.FirstName != null)
                                {
                                    CallUpdateMail(Convert.ToInt32(CallID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), prevInspector);  //------------------Emails
                                }
                            }
                        }
                        else
                        {
                            if (CM.FirstName != null)
                            {
                                dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(CallID));
                                if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                {
                                    CallManagementMail(Convert.ToInt32(CallID), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }
                                if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                {
                                    CallManagementMail(Convert.ToInt32(CallID), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }
                                if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                {
                                    CallManagementMail(Convert.ToInt32(CallID), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }

                                if (CM.FirstName != null)
                                {
                                    CallManagementMail(Convert.ToInt32(CallID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);//------------------Emails
                                }
                            }

                        }
                    }

                    var Data = objDalCalls.GetBranchList();
                    ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
                    if (Result != null && Result != "")
                    {
                        #region
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
                        #endregion
                        // TempData["InsertCall"] = Result;
                        //  return View(CM);
                        //return RedirectToAction("CallsList");
                        return RedirectToAction("InsertCalls", "CallsMaster", new { PK_Call_ID = CM.PK_Call_ID });
                    }


                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return RedirectToAction("CallsList");
        }




        [HttpGet]
        public ActionResult ExtendCalls(int PK_Call_ID)
        {
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



            var Data = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");


            var UserData = objDalCalls.GetInspectorList();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            DataSet DSGetEmployeeType = new DataSet();
            DSGetEmployeeType = objDalEnquiryMaster.GetEmployeeTypeLst();
            List<NameCode> lstEmployeetType = new List<NameCode>();

            if (DSGetEmployeeType.Tables[0].Rows.Count > 0)
            {
                lstEmployeetType = (from n in DSGetEmployeeType.Tables[0].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEmployeeType.Tables[0].Columns["ProjectName"].ToString()),
                                        Code = n.Field<Int32>(DSGetEmployeeType.Tables[0].Columns["PK_ID"].ToString())
                                    }).ToList();
            }

            IEnumerable<SelectListItem> EmployeeType;
            EmployeeType = new SelectList(lstEmployeetType, "Code", "Name");
            ViewBag.ExeService = EmployeeType;
            ViewData["ExeService"] = EmployeeType;




            if (PK_Call_ID != 0)
            {
                ViewBag.check = "productcheck";
                ViewBag.Jobcheck = "JobCheck";
                string[] splitedProduct_Name;

                DataSet DSEditQutationTabledata = new DataSet();
                DSEditQutationTabledata = objDalCalls.EditCall(PK_Call_ID);

                if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Company_Name"]);
                    //ObjModelsubJob.Status = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Status"]);
                    ObjModelsubJob.Status = Convert.ToString("Assigned");
                    // ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Br_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Executing_Branch"]);
                    ObjModelsubJob.Originating_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch"]);
                    ObjModelsubJob.Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job"]);
                    ObjModelsubJob.Sub_Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Job"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.End_Customer = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["End_Customer"]);
                    ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Name"]);
                    ObjModelsubJob.PK_Call_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelsubJob.Contact_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Contact_Name"]);
                    //ObjModelsubJob.Call_Recived_date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Recived_date"]);
                    //ObjModelsubJob.Call_Request_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Request_Date"]);
                    ObjModelsubJob.Source = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Source"]);
                    //ObjModelsubJob.Planned_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Planned_Date"]);
                    ObjModelsubJob.Urgency = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Urgency"]);
                    ObjModelsubJob.Competency_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Competency_Check"]);
                    ObjModelsubJob.Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Category"]);
                    ObjModelsubJob.Sub_Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Category"]);
                    ObjModelsubJob.Empartiality_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Empartiality_Check"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Job_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job_Location"]);
                    ObjModelsubJob.Final_Inspection = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Final_Inspection"]);
                    ObjModelsubJob.From_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["From_Date"]);
                    ObjModelsubJob.To_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["To_Date"]);
                    ObjModelsubJob.FirstName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Inspector"]);
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Tuv_Branch"]);
                    ObjModelsubJob.Call_No = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_No"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Attachment"]);

                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.PO_Number = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PO_Number"]);
                    ObjModelsubJob.Po_No_SSJob = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Po_No_SSJob"]);

                    int CompetencyCheck = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["CompetencyCheck"]);
                    ObjModelsubJob.CompetencyCheck = Convert.ToBoolean(CompetencyCheck);
                    int ImpartialityCheck = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["ImpartialityCheck"]);
                    ObjModelsubJob.ImpartialityCheck = Convert.ToBoolean(ImpartialityCheck);
                    int FinalInspection = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FinalInspection"]);
                    ObjModelsubJob.FinalInspection = Convert.ToBoolean(FinalInspection);

                    //int chkContinuousCall = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["chkContinuousCall"]);
                    //ObjModelsubJob.ChkContinuousCall = Convert.ToBoolean(chkContinuousCall);

                    int ChkMultipleSubJobNo = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["ChkMultipleSubJobNo"]);
                    ObjModelsubJob.ChkMultipleSubJobNo = Convert.ToBoolean(ChkMultipleSubJobNo);



                    ObjModelsubJob.ProductList = DSEditQutationTabledata.Tables[0].Rows[0]["Product_Item"].ToString();
                    ObjModelsubJob.Description = DSEditQutationTabledata.Tables[0].Rows[0]["Description"].ToString();
                    ObjModelsubJob.Quantity = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Quantity"].ToString());
                    ObjModelsubJob.ExecutingService = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["ExecutingService"].ToString());

                    ///// SubSub Vendor For /1/1 Sub SubJob
                    ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorName"]);
                    ObjModelsubJob.Po_No_SSJob = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Po_No_SSJob"]);
                    ObjModelsubJob.SubVendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPODate"]);

                    ///// Main Vendor For /1 SubJob
                    ObjModelsubJob.TopSubVendorName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_name"]);
                    ObjModelsubJob.TopSubVendorPONo = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPONo"]);
                    ObjModelsubJob.TopvendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopvendorPODate"]);



                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECname"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.chkARC = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.SAP_no = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.MandayRate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["MandayRate"]);


                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Product_item"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;



                }
                return View(ObjModelsubJob);
            }
            else
            {
                return RedirectToAction("CallsList");
            }

        }


        [HttpPost]
        public ActionResult ExtendCalls(CallsModel CM, HttpPostedFileBase[] Image, List<HttpPostedFileBase> img_Banner, FormCollection fc)
        {
            string Result = string.Empty;
            #region
            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }
            try
            {

                List<string> lstAttachment = new List<string>();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                {
                    foreach (HttpPostedFileBase single in img_Banner) // Added by Sagar Panigrahi
                    {
                        if (single != null && single.FileName != "")
                        {
                            var filename = CommonControl.FileUpload("~/Content/Uploads/Images/", single);
                            lstAttachment.Add(filename);
                        }
                    }
                    CM.Attachment = string.Join(",", lstAttachment);
                    if (string.IsNullOrEmpty(CM.Attachment))
                    {
                        CM.Attachment = "NoImage.gif";
                    }
                }
                else
                {
                    CM.Attachment = "NoImage.gif";
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #endregion




            #region new code

            JobNumber = Convert.ToString(CM.Job);
            string ProList = string.Join(",", fc["ProductList"]);
            CM.ProductList = ProList;
     
            CM.Attachment = IPath;
            int CallID = 0;


            #region


            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            int days = 0;
            if (CM.ChkContinuousCall == true)
            {
                if (CM.FromDateNew != null && CM.ToDateNew != null)
                {
                    DateTime dt1 = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo);
                    DateTime dt2 = DateTime.ParseExact(CM.ToDateNew, "dd/MM/yyyy", theCultureInfo);

                    TimeSpan ts = dt2.Subtract(dt1);


                    if (ts.Days != 0)
                    {
                        days = ts.Days + 1;
                    }
                    else
                    {

                    }
                }

            }


            #endregion


            string newplannedDate = string.Empty;
            string ListofCall = string.Empty;
            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listCallMastUploadedFile"] as List<FileDetails>;




            newplannedDate = CM.Planned_Date;

            //if (CM.PK_Call_ID == 0)
            //{
                if (CM.ChkMultipleSubJobNo == true) //For Multiple Sub Job No
                {
                    string ListPKSubJob = string.Join(",", fc["AuditeeName"]);
                    string[] split = ListPKSubJob.Split(',');

                    foreach (string item in split)
                    {
                        ListofCall = string.Empty; //bind call no for each sub job 							  
                        CM.PK_SubJob_Id = Convert.ToInt32(item);

                        if (days > 0 /*&& CM.PK_Call_ID == 0*/) //Countinuous call
                        {
                            for (int i = 1; i <= days; i++)
                            {
                                string dates = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).ToString();

                                CM.ApplyDate = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).ToString();

                                #region InsertCode

                                int _min = 10000;
                                int _max = 99999;
                                Random _rdm = new Random();
                                int Rjno = _rdm.Next(_min, _max);
                                string ConfirmCode = Convert.ToString(Rjno);

                                int _mins = 100000;
                                int _maxs = 999999;
                                Random _rdms = new Random();
                                int Rjnos = _rdm.Next(_mins, _maxs);
                                string ConfirmSecondCode = Convert.ToString(Rjnos);

                                DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                                if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                                {
                                    CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                                    ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                                    ManDay = Convert.ToInt32(ManDays);

                                }
                                if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                                {
                                    ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                                }
                                if (CheckPoDate >= TodayDate)
                                {
                                    if (ManDay > ManDaysCount)
                                    {
                                        //CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                        CM.Executing_Branch = Convert.ToString(CM.Br_Id);

                                        if (CM.FirstName != null)
                                        {
                                            if (i == 1)
                                            {
                                                CM.Call_Request_Date = CM.FromDateNew;
                                                // CM.Actual_Visit_Date = CM.FromDateNew;
                                                CM.Actual_Visit_Date = newplannedDate.ToString();
                                                CM.Planned_Date = newplannedDate.ToString();

                                                CM.From_Date = CM.FromDateNew;
                                                CM.To_Date = CM.ToDateNew;
                                            }
                                            else
                                            {
                                                CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); 
                                                CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                            }

                                            CM.Inspector = CM.FirstName;
                                            CM.AssignStatus = "1";
                                            //CM.Status = "Open";
                                            CM.Status = "Assigned";


                                        }
                                        else
                                        {
                                            CM.AssignStatus = "0";
                                            CM.Status = "Open";
                                            if (CM.FromDateNew != null)
                                            {
                                                if (i == 1)
                                                {
                                                    CM.Call_Request_Date = CM.FromDateNew;
                                                    // CM.Actual_Visit_Date = CM.FromDateNew;
                                                    CM.Actual_Visit_Date = newplannedDate;
                                                    CM.Planned_Date = newplannedDate;
                                                    CM.From_Date = CM.FromDateNew;
                                                    CM.To_Date = CM.ToDateNew;
                                                }
                                                else
                                                {
                                                    CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                                    //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                                    //CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                    //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                                    CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                    CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                                }
                                            }

                                        }

                                        
                                        Result = objDalCalls.InsertUpdateCalls(CM);


                                        //Code Added by Manoj Sharma 13 March 2020 for Multiple File Uploaded
                                        CallID = Convert.ToInt32(Result);


                                        if (CallID != null && CallID != 0)
                                        {
                                            if (lstFileDtls != null && lstFileDtls.Count > 0)
                                            {
                                                Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                                Session["listCallMastUploadedFile"] = null;
                                            }
                                        }



                                        #region

                                        CallsModel IVRNew = new CallsModel();

                                        IVRNew.PK_Call_ID = Convert.ToInt32(Result);

                                        if (ListofCall == string.Empty)
                                        {
                                            ListofCall = IVRNew.PK_Call_ID.ToString();
                                        }
                                        else
                                        {
                                            ListofCall = ListofCall + "," + IVRNew.PK_Call_ID.ToString();
                                        }

                                        //  string[] split = ListPKSubJob.Split(',');

                                        #endregion
                                        /*if (CM.FirstName != null)
                                        {
                                            dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(IVRNew.PK_Call_ID));
                                            if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                            }
                                            if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                            }
                                            if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                            }

                                            if (CM.FirstName != null)
                                            {
                                                CallManagementMail(Convert.ToInt32(IVRNew.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                            }
                                        }*/

                                        var Data = objDalCalls.GetBranchList();
                                        ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
                                        TempData["InsertCall"] = Result;
                                        if (Result != "" && Result != null)
                                        {
                                            TempData["InsertCall"] = Result;


                                        }
                                    }
                                    else
                                    {
                                        Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                        return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                    }
                                }
                                else
                                {
                                    Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                    return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                }

                                #endregion

                            }
                            #region Test Mail 2 march 2021
                            if (days > 0)
                            {
                                string[] splitcalls = ListofCall.Split(',');
                                if (CM.FirstName != null)
                                {

                                    dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(splitcalls[0]));



                                    if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                    {
                                        ContinuousCallMail(ListofCall, CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                    }
                                    if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                    {
                                        ContinuousCallMail(ListofCall, CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                    }
                                    if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                    {
                                        ContinuousCallMail(ListofCall, CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                    }

                                    if (CM.FirstName != null)
                                    {
                                        ContinuousCallMail(ListofCall, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                                    }
                                }
                            }
                        }
                        else  //single call
                        {
                            DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                            if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                            {
                                CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                                ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                                ManDay = Convert.ToInt32(ManDays);

                            }
                            if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                            {
                                ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                            }
                            if (CheckPoDate >= TodayDate)
                            {
                                if (ManDay > ManDaysCount)
                                {
                                    #region InsertCode



                                    int _min = 10000;
                                    int _max = 99999;
                                    Random _rdm = new Random();
                                    int Rjno = _rdm.Next(_min, _max);
                                    string ConfirmCode = Convert.ToString(Rjno);

                                    int _mins = 100000;
                                    int _maxs = 999999;
                                    Random _rdms = new Random();
                                    int Rjnos = _rdm.Next(_mins, _maxs);
                                    string ConfirmSecondCode = Convert.ToString(Rjnos);

                                    // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                    CM.Executing_Branch = Convert.ToString(CM.Br_Id);


                                    if (CM.FirstName != null)
                                    {
                                        CM.Inspector = CM.FirstName;
                                        CM.Actual_Visit_Date = CM.Planned_Date;
                                        CM.AssignStatus = "1";
                                        //CM.Status = "Open";
                                        CM.Status = "Assigned";
                                    }
                                    else
                                    {
                                        CM.AssignStatus = "0";
                                        CM.Status = "Open";
                                        CM.Actual_Visit_Date = CM.Planned_Date;
                                    }

                                    Result = objDalCalls.InsertUpdateCalls(CM);
                                    CallID = Convert.ToInt32(Result);

                                    if (CallID != null && CallID != 0)
                                    {
                                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                                        {
                                            Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                            Session["listCallMastUploadedFile"] = null;
                                        }
                                    }

                                    if (CM.FirstName != null)
                                    {
                                        dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(Result));
                                        if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                        {
                                            //ServicebookingMail(CM,CM.Tuv_Branch, CM.FirstName);  //------------------Emails
                                            CallManagementMail(Convert.ToInt32(Result), CM.Tuv_Branch.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                        }
                                        if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                        {
                                            CallManagementMail(Convert.ToInt32(Result), CM.Vendor_Email.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo); //------------------Emails
                                        }
                                        if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                        {
                                            CallManagementMail(Convert.ToInt32(Result), CM.Client_Email.ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                        }


                                        if (CM.FirstName != null)
                                        {
                                            // ServicebookingMail(CM,dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), CM.FirstName);  //------------------Emails
                                            CallManagementMail(Convert.ToInt32(Result), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                        }

                                    }

                                    var Data = objDalCalls.GetBranchList();
                                    ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

                                    if (Result != "" && Result != null)
                                    {
                                        #region vaibhav code to resolve null reference error in Product Item //on return need to chk callid and bind selected item Product Item
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
                                        #endregion

                                        TempData["InsertCall"] = Result;

                                        // return View(CM);//13-2-2020
                                        // return RedirectToAction("CallsList");
                                    }

                                    #endregion
                                }
                                else
                                {
                                    Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                    return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                                }
                            }
                            else
                            {
                                Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                            }
                        }

                        #endregion
                    }

                }
                else if (days > 0 /*&& CM.PK_Call_ID == 0*/) //Countinuous call
                {
                    for (int i = 1; i <= days; i++)
                    {
                        //   string dates = Convert.ToDateTime(CM.FromDateNew).ToString("dd/mm/yyyy");
                        CM.ApplyDate = Convert.ToString(CM.FromDateNew);
                        #region InsertCode
                        int _min = 10000;
                        int _max = 99999;
                        Random _rdm = new Random();
                        int Rjno = _rdm.Next(_min, _max);
                        string ConfirmCode = Convert.ToString(Rjno);

                        int _mins = 100000;
                        int _maxs = 999999;
                        Random _rdms = new Random();
                        int Rjnos = _rdm.Next(_mins, _maxs);
                        string ConfirmSecondCode = Convert.ToString(Rjnos);
                        DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                        if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                        {
                            CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                            ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                            ManDay = Convert.ToInt32(ManDays);

                        }
                        if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                        {
                            ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                        }
                        if (CheckPoDate > TodayDate)
                        {
                            if (ManDay > ManDaysCount)
                            {
                                // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                                CM.Executing_Branch = Convert.ToString(CM.Br_Id);

                                if (CM.FirstName != null)
                                {
                                    if (i == 1)
                                    {
                                        CM.Call_Request_Date = CM.FromDateNew;
                                        CM.Actual_Visit_Date = newplannedDate;
                                        CM.Planned_Date = newplannedDate;
                                        CM.From_Date = CM.FromDateNew;
                                        CM.To_Date = CM.ToDateNew;
                                    }
                                    else
                                    {
                                        CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                        //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");                                            
                                                                                                                                                                        //CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                        //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                        //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                        CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                        CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                        CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                        CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                    }

                                    CM.Inspector = CM.FirstName;
                                    CM.AssignStatus = "1";
                                    //CM.Status = "Open";
                                    CM.Status = "Assigned";

                                }
                                else
                                {
                                    CM.AssignStatus = "0";
                                    CM.Status = "Open";
                                    if (CM.FromDateNew != null)
                                    {
                                        if (i == 1)
                                        {
                                            CM.Call_Request_Date = CM.FromDateNew;
                                            //CM.Actual_Visit_Date = CM.FromDateNew; // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            //CM.Planned_Date = CM.Planned_Date; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.Actual_Visit_Date = newplannedDate;
                                            CM.Planned_Date = newplannedDate;
                                            CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020
                                        }
                                        else
                                        {
                                            CM.Call_Request_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                            //CM.Actual_Visit_Date = DateTime.ParseExact(CM.FromDateNew, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                                                                                                                                                            //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                            //CM.Planned_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).AddDays(1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            //CM.Actual_Visit_Date = DateTime.ParseExact(CM.Planned_Date, "dd/MM/yyyy", theCultureInfo).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                            CM.Actual_Visit_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy"); //.AddDays(i - 1).ToString(); // DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.Planned_Date = DateTime.ParseExact(newplannedDate, "dd/MM/yyyy", theCultureInfo).AddDays(i - 1).ToString("dd/MM/yyyy");  //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");

                                            CM.From_Date = CM.FromDateNew; //DateTime.Parse(Convert.ToString(CM.FromDateNew)).ToString("dd/MM/yyyy");
                                            CM.To_Date = CM.ToDateNew; // DateTime.Parse(Convert.ToString(CM.ToDateNew)).ToString("dd/MM/yyyy");//7 Feb 2020

                                        }
                                    }

                                }

                            //  Result = objDalCalls.InsertUpdateCalls(CM);
                            int id = objDalCalls.InsertExtendCalls(CM);
                            //Code Added by Manoj Sharma 13 March 2020 for Multiple File Uploaded

                            //CallID = Convert.ToInt32(Result);
                            CallID = Convert.ToInt32(id);
                            if (CallID != null && CallID != 0)
                                {
                                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                                    {
                                        Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                        Session["listCallMastUploadedFile"] = null;
                                    }
                                }

                                //DateTime actualdata = Convert.ToDateTime(dates).AddDays(1);
                                // CM.From_Date = Convert.ToString(actualdata);
                                #region
                                //int id = objDalCalls.InsertExtendCalls(CM);
                                CallsModel IVRNew = new CallsModel();
                                //  IVRNew.Call_No = DateTime.Now.Year + "/00" + Result; //Call No Generated
                                IVRNew.PK_Call_ID = Convert.ToInt32(CallID);
                                if (ListofCall == string.Empty)
                                {
                                    ListofCall = IVRNew.PK_Call_ID.ToString();
                                }
                                else
                                {
                                    ListofCall = ListofCall + "," + IVRNew.PK_Call_ID.ToString();
                                }
                                // objDalCalls.UpdateExtendCalls(IVRNew);  //Update call Id Comment by Vaibhav Generated from store procedure
                                #endregion
                                /*if (CM.FirstName != null)
                                {
                                    if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                    {
                                        ServicebookingMail(IVRNew, CM.Tuv_Branch, CM.FirstName);  //------------------Emails
                                    }
                                    if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                    {
                                        ServicebookingMail(IVRNew, CM.Vendor_Email, CM.FirstName);  //------------------Emails
                                    }
                                    if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                    {
                                        ServicebookingMail(IVRNew, CM.Client_Email, CM.FirstName);  //------------------Emails
                                    }
                                    dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(IVRNew.PK_Call_ID));
                                    if (CM.FirstName != null)
                                    {
                                        ServicebookingMail(IVRNew, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), CM.FirstName);  //------------------Emails
                                    }
                                }*/

                                var Data = objDalCalls.GetBranchList();
                                ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");
                                TempData["InsertCall"] = Result;
                                if (Result != "" && Result != null)
                                {
                                    TempData["InsertCall"] = Result;

                                    // return RedirectToAction("CallsList");
                                }
                            }
                            else
                            {
                                Session["CallLimit"] = "You cant not create call, because call limit has been end";
                                return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                            }
                        }
                        else
                        {
                            Session["CallLimit"] = "You cant not create call, because call limit has been end";
                            return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                        }
                        #endregion
                    }
                    #region Countinuous call mail vaibhav
                    if (days > 0)
                    {
                        string[] splitcalls = ListofCall.Split(',');
                        if (CM.FirstName != null)
                        {
                            dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(splitcalls[0]));
                            if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                            {
                                ContinuousCallMail(ListofCall, CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                            }
                            if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                            {
                                ContinuousCallMail(ListofCall, CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                            }
                            if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                            {
                                ContinuousCallMail(ListofCall, CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                            }

                            if (CM.FirstName != null)
                            {
                                ContinuousCallMail(ListofCall, dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString());  //------------------Emails
                            }
                        }
                    }


                    #endregion
                }
                else  //single call
                {
                    #region InsertCode



                    int _min = 10000;
                    int _max = 99999;
                    Random _rdm = new Random();
                    int Rjno = _rdm.Next(_min, _max);
                    string ConfirmCode = Convert.ToString(Rjno);

                    int _mins = 100000;
                    int _maxs = 999999;
                    Random _rdms = new Random();
                    int Rjnos = _rdm.Next(_mins, _maxs);
                    string ConfirmSecondCode = Convert.ToString(Rjnos);
                    DSCheckValidCall = objDalCalls.CheckValidCall(JobNumber);
                    if (DSCheckValidCall.Tables[0].Rows.Count > 0)
                    {
                        CheckPoDate = Convert.ToDateTime(DSCheckValidCall.Tables[0].Rows[0]["POValidity"]);
                        ManDays = Convert.ToString(DSCheckValidCall.Tables[0].Rows[0]["ManDays"]);
                        ManDay = Convert.ToInt32(ManDays);

                    }
                    if (DSCheckValidCall.Tables[1].Rows.Count > 0)
                    {
                        ManDaysCount = Convert.ToInt32(DSCheckValidCall.Tables[1].Rows[0]["MandaysCount"]);
                    }
                    if (CheckPoDate > TodayDate)
                    {
                        if (ManDay > ManDaysCount)
                        {
                            // CM.Call_No = Convert.ToString(ConfirmCode) + ConfirmSecondCode;
                            CM.Executing_Branch = Convert.ToString(CM.Br_Id);


                            if (CM.FirstName != null)
                            {
                                CM.Inspector = CM.FirstName;
                                CM.Actual_Visit_Date = CM.Planned_Date;
                                CM.AssignStatus = "1";
                                //CM.Status = "Open";
                                CM.Status = "Assigned";
                            
                            }
                            else
                            {
                                CM.AssignStatus = "0";
                                CM.Status = "Open";
                                CM.Actual_Visit_Date = CM.Planned_Date;
                            }

                           // Result = objDalCalls.InsertUpdateCalls(CM);
                        int id = objDalCalls.InsertExtendCalls(CM);

                        // CallID = Convert.ToInt32(Result);
                        CallID = Convert.ToInt32(id);

                        if (CallID != null && CallID != 0)
                            {
                                if (lstFileDtls != null && lstFileDtls.Count > 0)
                                {
                                    Result = objDalCalls.InsertFileAttachment(lstFileDtls, CallID);
                                    Session["listCallMastUploadedFile"] = null;
                                }
                            }

                            if (CM.FirstName != null)
                            {
                            //dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(Result));
                            dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(id));
                            if (CM.Homecheckbox == true && CM.Tuv_Branch != null)
                                {
                                    CallManagementMail(Convert.ToInt32(id), CM.Tuv_Branch, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }
                                if (CM.Vendorcheckbox && CM.Vendor_Email != null)
                                {
                                    CallManagementMail(Convert.ToInt32(id), CM.Vendor_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }
                                if (CM.ClientEmailcheckbox && CM.Client_Email != null)
                                {
                                    CallManagementMail(Convert.ToInt32(id), CM.Client_Email, dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo); //------------------Emails
                                }

                                if (CM.FirstName != null)
                                {
                                    CallManagementMail(Convert.ToInt32(id), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), CM.TopSubVendorName, CM.TopSubVendorPONo);  //------------------Emails
                                }
                            }

                            var Data = objDalCalls.GetBranchList();
                            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

                            if (Result != "" && Result != null)
                            {
                                #region vaibhav code to resolve null reference error in Product Item //on return need to chk callid and bind selected item Product Item
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
                                #endregion

                                TempData["InsertCall"] = Result;

                                // return View(CM);//13-2-2020
                                return RedirectToAction("CallsList");
                            }
                        }
                        else
                        {
                            Session["CallLimit"] = "You cant not create call, because call limit has been end";
                            return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                        }
                    }
                    else
                    {
                        Session["CallLimit"] = "You cant not create call, because call limit has been end";
                        return RedirectToAction("CreatSubJob", "SubJobMaster", new { PK_SubJob_Id = Convert.ToInt32(Session["PK_SubJobID"]) });
                    }
                    #endregion
                }
            //}
                #endregion



                if (Result != "" && Result != null)
            {
                TempData["ExtendCalls"] = Result;

                
            }
            
            return RedirectToAction("CallsList");

        }




        #region image

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 12 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();
            //---Adding end Code

            if (Session["listCallMastUploadedFile"] != null)
            {

                fileDetails = Session["listCallMastUploadedFile"] as List<FileDetails>;
            }

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
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = fileName;
                            fileDetail.Extension = Path.GetExtension(fileName);
                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;

                            fileDetails.Add(fileDetail);


                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;
                            //files.SaveAs(filePath);

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
                Session["listCallMastUploadedFile"] = fileDetails;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        #endregion



        public void ServicebookingMail(CallsModel CM, string ClientEmail, string inspactorName)
        {
            try
            {

                DataTable Details = new DataTable();

                Details = objDalCalls.GetCallDetails(CM.PK_Call_ID);
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string displayName = string.Empty;

                //bodyTxt += " Dear Sir/Madam, " + "<br><br>";
                //bodyTxt += "Please be informed that our inspector " + inspactorName + " will be attending the inspection call as per details below. <br><br>";
                //bodyTxt += " Inspectors contact details : 8870845711 <br><br>";               
                //bodyTxt += "Best Regards,<br>";
                //     += "TUV India Pvt Ltd.<br>";

                bodyTxt = @"<html>
                        <head>
                            <title></title>
                        </head>
                        <body>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir/Madam,</span></span></div>
                            <div>
                                &nbsp;</div>
                            <div>
                                <span style='font-size:10px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that our inspector  <b>" + Details.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection call as per details below. </br></br>";
                bodyTxt = bodyTxt + "Inspectors contact details : " + Details.Rows[0]["MOBILENO"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Email Id : " + Details.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";

                bodyTxt = bodyTxt + "<div><span style='font-size:10px;'><span style='font-family:verdana,geneva,sans-serif;'></br></br>";
                bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><tr>";

                bodyTxt = bodyTxt + "<td style='width:10%;'>Call No</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%;'>Sub Job Number/Sub-sub job number</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>SAP Number</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Planned Inspection Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO NO</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Client</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Vendor/Sub Vendor</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Location of inspection</td> ";

                bodyTxt = bodyTxt + " </tr>";
                bodyTxt = bodyTxt + "<tr>";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Po_Number"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Coordinator Name : " + Details.Rows[0]["CoordinateName"].ToString() + " ,Mobile No : " + Details.Rows[0]["CoordinateMobile"].ToString() + ",Email : " + Details.Rows[0]["CoordinateEmail"].ToString() + " </br>";

                bodyTxt = bodyTxt + "</tr></Table></span></span></div></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Best regards,</br>";

                bodyTxt = bodyTxt + " TUV India Pvt Ltd. </br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Note :This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";
                displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

                msg.From = new MailAddress(MailFrom, displayName);
                ///msg.From = new MailAddress(MailFrom);

                string To = ClientEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                // msg.To.Add(MailTo);
                //msg.CC.Add(MailCC);
                //msg.Bcc.Add(MailBCC);
                msg.Subject = "Confirmation of inspection visit";
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        public void CallManagementMail(int CallId, string ClientEmail, string inspactorName, string VendorName, string PoNo)
        {
            try
            {

                DataTable Details = new DataTable();

                Details = objDalCalls.GetCallDetails(CallId);
                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string displayName = string.Empty;

                //bodyTxt += " Dear Sir/Madam, " + "<br><br>";
                //bodyTxt += "Please be informed that our inspector " + inspactorName + " will be attending the inspection call as per details below. <br><br>";
                //bodyTxt += " Inspectors contact details : 8870845711 <br><br>";               
                //bodyTxt += "Best Regards,<br>";
                //     += "TUV India Pvt Ltd.<br>";

                bodyTxt = @"<html>
                        <head>
                            <title></title>
                        </head>
                        <body>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                            <div>
                                &nbsp;</div>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that our inspector  <b>" + Details.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection call as per details given below. </br></br>";
                bodyTxt = bodyTxt + "Inspector contact details : " + "</br>";
                bodyTxt = bodyTxt + "Mobile Number : " + Details.Rows[0]["MOBILENO"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Email Id : " + Details.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";

                bodyTxt = bodyTxt + "<div></br>";
                bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'><tr>";

                bodyTxt = bodyTxt + "<td style='width:8%'>Call No</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>Sub Job Number/Sub-sub job number</td> ";
                bodyTxt = bodyTxt + "<td style='width:8%'>SAP Number</td> ";
                bodyTxt = bodyTxt + "<td style='width:6%'>Planned Inspection Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:12%;'>PO NO - PO Date</td> ";
                //bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Client Name</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Vendor Name</td>";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Sub Vendor Name</td>";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Location of inspection</td> ";

                bodyTxt = bodyTxt + " </tr>";
                bodyTxt = bodyTxt + "<tr>";
                bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:6%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:12%'>" + Details.Rows[0]["Po_Number"].ToString() + " - " + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
                //bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["SubVendorName"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";

                bodyTxt = bodyTxt + "</tr></span></span></table></span></span></div></br>";
																																				 

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Coordinator Name: " + Details.Rows[0]["CoordinateName"].ToString() + "  </br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Mobile Number : " + Details.Rows[0]["CoordinateMobile"].ToString() + " </br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Email Id: " + Details.Rows[0]["CoordinateEmail"].ToString() + " " + "</br></br></br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Kindly Note,";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>In these difficult times, TUV India is striving hard to meet customers expectations and attending all inspections where permitted.  In order to further improve the process, we request as follows :";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• We request minimum two working days notice to plan and execute your call. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• In case you are giving the call 1 day before, please share your inspection call details on or before 1:00 PM in order to plan and communicate our inspector deputation for next day. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• If inspection is required on Monday; please do intimate the same on Friday or latest before 12.00 p.m. noon on Saturday.  ";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• Calls received earlier will be given higher priority although we try to accommodate all the calls for the given day. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> <span> &nbsp;&nbsp;</span> This also depends on approved surveyors / joint inspection calls / unexpected continuation of previous days calls. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• Please ensure full readiness of the documents, items (with internal inspection) and internal inspection reports. " + "</br></br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br></br>";

                bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";

                displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

                msg.From = new MailAddress(MailFrom, displayName);

                string To = ClientEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }

                string MentorEmail = string.Empty;
                DataTable MDetails = new DataTable();

                MDetails = objDalCalls.GetMentorName(CallId.ToString());

                if (MDetails.Rows.Count > 0)
                {
                    MentorEmail = MDetails.Rows[0]["MentorEmail"].ToString();
                    msg.CC.Add(MentorEmail);
                }
                
                msg.Subject = "Confirmation of inspection visit at  " + VendorName + " for " + Details.Rows[0]["Company_Name"].ToString() + " PO No " + PoNo;
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        //public void ContinuousCallMail(string callList, string ClientEmail, string inspactorName)
        //{
        //    try
        //    {

        //        DataTable Details = new DataTable();
        //        DataTable InspectorDetails = new DataTable();

        //        string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
        //        string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
        //        string bodyTxt = "";
        //        string displayName = string.Empty;
        //        MailMessage msg = new MailMessage();

        //        string[] splitcallID = callList.Split(',');



        //        string rawCSV = callList;
        //        string ids = string.Join(",", rawCSV.Split(',').Select(s => "'" + s + "'"));
        //        // string ids = string.Join(",", rawCSV.Split(','));
        //        // int Iids = Convert.ToInt32(ids);

        //        //InspectorDetails = objDalCalls.GetCallDetailsNew(Convert.ToInt32(splitcallID[0]), rawCSV);
        //        Details = objDalCalls.GetCallDetailsNew(Convert.ToInt32(splitcallID[0]), rawCSV);

        //        bodyTxt = @"<html>
        //                <head>
        //                    <title></title>
        //                </head>
        //                <body>
        //                    <div>
        //                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
        //                    <div>
        //                        &nbsp;</div>
        //                    <div>
        //                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that our inspector  <b>" + Details.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection calls as per details given below. </br></br>";
        //        bodyTxt = bodyTxt + "Inspectors contact details : " + Details.Rows[0]["MOBILENO"].ToString() + "</br>";
        //        bodyTxt = bodyTxt + "Email Id : " + Details.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";
        //        bodyTxt = bodyTxt + "<div></br>";
        //        bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'><tr>";

        //        bodyTxt = bodyTxt + "<td style='width:8%'>Call Nos.</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:10%'>Sub Job Number/Sub-sub job number</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:8%'>SAP Number</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:10%'>Planned Inspection Dates</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:12%;'>PO NO - PO Date</td> ";
        //        //bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:15%;'>Client Name</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:15%;'>Vendor Name</td>";
        //        bodyTxt = bodyTxt + "<td style='width:15%;'>Sub Vendor Name</td>";
        //        bodyTxt = bodyTxt + "<td style='width:15%;'>Location of inspection</td> ";

        //        bodyTxt = bodyTxt + " </tr>";

        //        //foreach (string CallId in splitcallID)
        //        //{

        //        //  Details = objDalCalls.GetCallDetails(Convert.ToInt32(CallId));




        //        bodyTxt = bodyTxt + "<tr>";
        //        //bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> "; vai 2 march

        //        bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["LCallNo"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
        //        //bodyTxt = bodyTxt + "<td style='width:6%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["LDates"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:12%'>" + Details.Rows[0]["Po_Number"].ToString() + " - " + Details.Rows[0]["PO_Date"].ToString() + "</td> ";



        //        bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["SubVendorName"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";
        //        bodyTxt = bodyTxt + "</tr>";



        //        //}

        //        bodyTxt = bodyTxt + "</span></span></table></span></span></div></br>";


        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Coordinator Name : " + Details.Rows[0]["CoordinateName"].ToString() + " </br>";

        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> Mobile No : " + Details.Rows[0]["CoordinateMobile"].ToString() + " </br>";
        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> Email : " + Details.Rows[0]["CoordinateEmail"].ToString() + " " + "</br></br></br>";


        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Best regards," + "</br></br>";

        //        bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Note :This is auto generated mail. Please do not reply.</span></span></div></br>";
        //        bodyTxt = bodyTxt + "</body></html> ";

        //        displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

        //        msg.From = new MailAddress(MailFrom, displayName);

        //        string To = ClientEmail.ToString();
        //        char[] delimiters = new[] { ',', ';', ' ' };
        //        string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string MultiEmailTemp in EmailIDs)
        //        {
        //            msg.To.Add(new MailAddress(MultiEmailTemp));
        //        }

        //        // msg.Subject = "Confirmation of inspection visit";
        //        msg.Subject = "Confirmation of inspection visit at  " + Details.Rows[0]["Vendor_Name"].ToString() + " for " + Details.Rows[0]["Company_Name"].ToString() + " PO No " + Details.Rows[0]["Po_Number"].ToString();

        //        msg.Body = bodyTxt;
        //        msg.IsBodyHtml = true;
        //        msg.Priority = MailPriority.Normal;
        //        SmtpClient client = new SmtpClient();

        //        client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
        //        client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
        //        client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
        //        client.EnableSsl = true;
        //        client.Send(msg);

        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;

        //    }
        //}



        //public void ContinuousCallMail(string callList, string ClientEmail, string inspactorName)
        //{
        //    try
        //    {

        //        DataTable Details = new DataTable();
        //        DataTable InspectorDetails = new DataTable();

        //        string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
        //        string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
        //        string bodyTxt = "";
        //        string displayName = string.Empty;
        //        MailMessage msg = new MailMessage();

        //        string[] splitcallID = callList.Split(',');

        //        InspectorDetails = objDalCalls.GetCallDetails(Convert.ToInt32(splitcallID[0]));

        //        bodyTxt = @"<html>
        //                <head>
        //                    <title></title>
        //                </head>
        //                <body>
        //                    <div>
        //                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir/Madam,</span></span></div>
        //                    <div>
        //                        &nbsp;</div>
        //                    <div>
        //                        <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that our inspector  <b>" + InspectorDetails.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection call as per details below. </br></br>";
        //        bodyTxt = bodyTxt + "Inspectors contact details : " + InspectorDetails.Rows[0]["MOBILENO"].ToString() + "</br>";
        //        bodyTxt = bodyTxt + "Email Id : " + InspectorDetails.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";
        //        bodyTxt = bodyTxt + "<div></br></br>";
        //        bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'><tr>";

        //        bodyTxt = bodyTxt + "<td style='width:10%'>Call No</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:10%'>Sub Job Number/Sub-sub job number</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%'>SAP Number</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%'>Planned Inspection Date</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%;'>PO NO</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%;'>Client</td> ";
        //        bodyTxt = bodyTxt + "<td style='width:20%;'>Vendor/Sub Vendor</td>";
        //        bodyTxt = bodyTxt + "<td style='width:20%;'>Location of inspection</td> ";

        //        bodyTxt = bodyTxt + " </tr>";

        //        foreach (string CallId in splitcallID)
        //        {

        //            Details = objDalCalls.GetCallDetails(Convert.ToInt32(CallId));




        //            bodyTxt = bodyTxt + "<tr>";
        //            bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Po_Number"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";
        //            bodyTxt = bodyTxt + "</tr>";


        //        }

        //        bodyTxt = bodyTxt + "</span></span></table></span></span></div></br>";
        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Best regards,</br>";

        //        bodyTxt = bodyTxt + " TUV India Pvt Ltd. </br></br>";
        //        bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Note :This is auto generated mail. Please do not reply.</span></span></div></br>";
        //        bodyTxt = bodyTxt + "</body></html> ";

        //        displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

        //        msg.From = new MailAddress(MailFrom, displayName);

        //        string To = ClientEmail.ToString();
        //        char[] delimiters = new[] { ',', ';', ' ' };
        //        string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string MultiEmailTemp in EmailIDs)
        //        {
        //            msg.To.Add(new MailAddress(MultiEmailTemp));
        //        }

        //        msg.Subject = "Confirmation of inspection visit";

        //        msg.Body = bodyTxt;
        //        msg.IsBodyHtml = true;
        //        msg.Priority = MailPriority.Normal;
        //        SmtpClient client = new SmtpClient();

        //        client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
        //        client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
        //        client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
        //        client.EnableSsl = true;
        //        client.Send(msg);

        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;

        //    }
        //}

        public void ContinuousCallMail(string callList, string ClientEmail, string inspactorName)
        {
            try
            {

                DataTable Details = new DataTable();
                
                DataTable InspectorDetails = new DataTable();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string displayName = string.Empty;
                
                MailMessage msg = new MailMessage();

                string[] splitcallID = callList.Split(',');


                string rawCSV = callList;
                string ids = string.Join(",", rawCSV.Split(',').Select(s => "'" + s + "'"));
                // string ids = string.Join(",", rawCSV.Split(','));
                // int Iids = Convert.ToInt32(ids);

                //InspectorDetails = objDalCalls.GetCallDetailsNew(Convert.ToInt32(splitcallID[0]), rawCSV);
                Details = objDalCalls.GetCallDetailsNew(Convert.ToInt32(splitcallID[0]), rawCSV);

                bodyTxt = @"<html>
                        <head>
                            <title></title>
                        </head>
                        <body>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir / Madam,</span></span></div>
                            <div>
                                &nbsp;</div>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that our inspector  <b>" + Details.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection calls as per details given below. </br></br>";
                bodyTxt = bodyTxt + "Inspector contact details : " + "</br>";
                bodyTxt = bodyTxt + "Mobile Number : " + Details.Rows[0]["MOBILENO"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Email Id : " + Details.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";
                bodyTxt = bodyTxt + "<div></br>";
                bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'><tr>";

                bodyTxt = bodyTxt + "<td style='width:8%'>Call Nos.</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>Sub Job Number/Sub-sub job number</td> ";
                bodyTxt = bodyTxt + "<td style='width:8%'>SAP Number</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>Planned Inspection Dates</td> ";
                bodyTxt = bodyTxt + "<td style='width:12%;'>PO NO - PO Date</td> ";
                //bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Client Name</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Vendor Name</td>";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Sub Vendor Name</td>";
                bodyTxt = bodyTxt + "<td style='width:15%;'>Location of inspection</td> ";

                bodyTxt = bodyTxt + " </tr>";

                //foreach (string CallId in splitcallID)
                //{

                //  Details = objDalCalls.GetCallDetails(Convert.ToInt32(CallId));




                bodyTxt = bodyTxt + "<tr>";
                //bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> "; vai 2 march
                bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["LCallNo"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:8%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
                //bodyTxt = bodyTxt + "<td style='width:6%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["LDates"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:12%'>" + Details.Rows[0]["Po_Number"].ToString() + " - " + Details.Rows[0]["PO_Date"].ToString() + "</td> ";

                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["SubVendorName"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:15%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "</tr>";


                //}

                bodyTxt = bodyTxt + "</span></span></table></span></span></div></br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Coordinator Name : " + Details.Rows[0]["CoordinateName"].ToString() + " </br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> Mobile Number : " + Details.Rows[0]["CoordinateMobile"].ToString() + " </br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> Email Id : " + Details.Rows[0]["CoordinateEmail"].ToString() + " " + "</br></br></br>";


                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Kindly Note,";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>In these difficult times, TUV India is striving hard to meet customers expectations and attending all inspections where permitted.  In order to further improve the process, we request as follows :";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• We request minimum two working days notice to plan and execute your call. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• In case you are giving the call 1 day before, please share your inspection call details on or before 1:00 PM in order to plan and communicate our inspector deputation for next day. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• If inspection is required on Monday; please do intimate the same on Friday or latest before 12.00 p.m. noon on Saturday.  ";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• Calls received earlier will be given higher priority although we try to accommodate all the calls for the given day. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'> <span> &nbsp;&nbsp;</span> This also depends on approved surveyors / joint inspection calls / unexpected continuation of previous days calls. ";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>• Please ensure full readiness of the documents, items (with internal inspection) and internal inspection reports. " + "</br></br>";

                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Thank You & Best regards," + "</br></br>";

                bodyTxt = bodyTxt + " TUV India Private Limited. " + "</br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";

                displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

                msg.From = new MailAddress(MailFrom, displayName);

                string To = ClientEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }

                string MentorEmail = string.Empty;
                DataTable MDetails = new DataTable();
                MDetails = objDalCalls.GetMentorName(splitcallID[0]);

                if(MDetails.Rows.Count > 0)
                {
                    MentorEmail = MDetails.Rows[0]["MentorEmail"].ToString();
                    msg.CC.Add(MentorEmail);
                }

                // msg.Subject = "Confirmation of inspection visit";
                msg.Subject = "Confirmation of inspection visit at  " + Details.Rows[0]["Vendor_Name"].ToString() + " for " + Details.Rows[0]["Company_Name"].ToString() + " PO No " + Details.Rows[0]["Po_Number"].ToString();

                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        public void CallUpdateMail(int CallId, string ClientEmail, string inspactorName, string prevInspector)
        {
            try
            {

                DataTable Details = new DataTable();
                DataSet PreDetails = new DataSet();

                Details = objDalCalls.GetCallDetails(CallId);
                PreDetails = objDalCalls.GetInspectorDetails(prevInspector);

                MailMessage msg = new MailMessage();

                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                string displayName = string.Empty;



                bodyTxt = @"<html>
                        <head>
                            <title></title>
                        </head>
                        <body>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Dear Sir/Madam,</span></span></div>
                            <div>
                                &nbsp;</div>
                            <div>
                                <span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Please be informed that in place of <b>" + PreDetails.Tables[0].Rows[0]["inspector"].ToString() + "</b> our inspector  <b>" + Details.Rows[0]["inspector"].ToString() + "</b> will be attending the inspection call as per details below. </br></br>";
                bodyTxt = bodyTxt + "Inspectors contact details : " + Details.Rows[0]["MOBILENO"].ToString() + "</br>";
                bodyTxt = bodyTxt + "Email Id : " + Details.Rows[0]["Tuv_Email_Id"].ToString() + "</br></span></span></div>";

                bodyTxt = bodyTxt + "<div></br></br>";
                bodyTxt = bodyTxt + "<table border='1' bordercolor='black'><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'><tr>";

                bodyTxt = bodyTxt + "<td style='width:10%'>Call No</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>Sub Job Number/Sub-sub job number</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>SAP Number</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>Planned Inspection Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO NO</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>PO Date</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Client</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Vendor/Sub Vendor</td>";
                bodyTxt = bodyTxt + "<td style='width:20%;'>Location of inspection</td> ";

                bodyTxt = bodyTxt + " </tr>";
                bodyTxt = bodyTxt + "<tr>";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Call_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:10%'>" + Details.Rows[0]["Sub_Job"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["SAP_No"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Actual_Visit_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Po_Number"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["PO_Date"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Company_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Vendor_Name"].ToString() + "</td> ";
                bodyTxt = bodyTxt + "<td style='width:20%'>" + Details.Rows[0]["Job_Location"].ToString() + "</td> ";

                bodyTxt = bodyTxt + "</tr></span></span></table></span></span></div></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Best regards,</br>";

                bodyTxt = bodyTxt + " TUV India Pvt Ltd. </br></br>";
                bodyTxt = bodyTxt + "<div><span style='font-size:12px;'><span style='font-family:verdana,geneva,sans-serif;'>Note :This is auto generated mail. Please do not reply.</span></span></div></br>";
                bodyTxt = bodyTxt + "</body></html> ";

                displayName = Details.Rows[0]["Branch"].ToString() + "-Inspection";

                msg.From = new MailAddress(MailFrom, displayName);

                string To = ClientEmail.ToString();
                char[] delimiters = new[] { ',', ';', ' ' };
                string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MultiEmailTemp in EmailIDs)
                {
                    msg.To.Add(new MailAddress(MultiEmailTemp));
                }
                string MentorEmail = string.Empty;
                DataTable MDetails = new DataTable();
                MDetails = objDalCalls.GetMentorName(CallId.ToString());

                if (MDetails.Rows.Count > 0)
                {
                    MentorEmail = MDetails.Rows[0]["MentorEmail"].ToString();
                    msg.CC.Add(MentorEmail);
                }
                // msg.To.Add(MailTo);
                //msg.CC.Add(MailCC);
                //msg.Bcc.Add(MailBCC);
                msg.Subject = "Confirmation of inspection visit";
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient();
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
        }

        [HttpGet]
        public ActionResult CallsList()
        {
            Session["GetExcelData"] = "Yes";
            Session["FromDate"] = null;
            Session["ToDate"] = null;

            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();
            CompanyDashBoard = objDalCalls.GetCallsList();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                //  ModifyDate = Convert.ToDateTime(dr["ModifyDate"]).Date,
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).Date,
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Excuting_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                                OrgID = Convert.ToInt32(dr["OrgID"]),
                                DeleteVisible = Convert.ToString(dr["DeleteVisible"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lstCallsModel1 = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }

        [HttpPost]
        public ActionResult CallsList(CallsModel CM)
        {
            Session["FromDate"] = CM.FromDate;
            Session["ToDate"] = CM.ToDate;

            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();
            if (CM.FromDate == null && CM.ToDate == null) 
            {
                CompanyDashBoard = objDalCalls.GetCallsList();
            }
            else
            { 
                CompanyDashBoard = objDalCalls.GetCallsListWithDate(CM);
            }
            lstCompanyDashBoard.Clear();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),

                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = Convert.ToDateTime(dr["CreatedDate"]).Date,
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Excuting_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                Call_Type = Convert.ToString(dr["call_Type"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                                OrgID = Convert.ToInt32(dr["OrgID"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lstCallsModel1 = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }

        #region   Heeeeeena
        public JsonResult GetAutoCompleteData(string InspectorName, string Br_Id)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<Users> lstCallDashBoard = new List<Users>();
            int id = Convert.ToInt32(Br_Id);
            DTCallDashBoard = objDalCalls.GetInspectorById(id);

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new Users
                            {
                                FirstName = Convert.ToString(dr["FirstName"]) + "  " + Convert.ToString(dr["LastName"]),
                                PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                FK_BranchID = Convert.ToString(dr["FK_BranchID"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                //Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                //Sub_Job = Convert.ToString(dr["Sub_job"]),
                                //Job_Location = Convert.ToString(dr["job_Location"]),
                                //Inspector = Convert.ToString(dr["Inspector"]),
                                //Product_item = Convert.ToString(dr["Product_item"])
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                          where n.FirstName.StartsWith(InspectorName)
                          select new { n.FirstName };
            return Json(getList);
        }



        public JsonResult GetSubNoAutoComplete(string InspectorName)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<SubJobs> lstCallDashBoard = new List<SubJobs>();

            DTCallDashBoard = objDalCalls.GetSubJobNo();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new SubJobs
                            {
                                Sub_Job = Convert.ToString(dr["SubJob_No"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                          where n.Sub_Job.StartsWith(InspectorName)
                          select new { n.Sub_Job };
            return Json(getList);
        }
        #endregion


        #region Calls management
        [HttpGet]
        public ActionResult CallsManagment(int? ConditionId, CallsModel vcm)
        {

            Session["GetExcelData"] = "Yes";
            var Dataa = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Dataa, "Br_Id", "Branch_Name");
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();

            #region
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = objDalCalls.GetBranchName();
            ObjModelsubJob.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            ObjModelsubJob.Br_Id = 0;
            ViewBag.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            #endregion


            var Data = objDalCalls.GetInspectorList();
            ViewBag.SubCatlist = new SelectList(Data, "PK_UserID", "FirstName");


            if (vcm.FromDate != null && vcm.ToDate != null || vcm.Call_No != null && vcm.Call_No != "" || vcm.Excuting_Branch != null && vcm.Excuting_Branch != "" || vcm.Originating_Branch != null && vcm.Originating_Branch != "")
            {
                CompanyDashBoard = objDalCalls.GetCallmanagmentsListByCondition(vcm);


                Session["GetExcelData"] = null;//6-Feb-2019
            }

            else
            {
                CompanyDashBoard = objDalCalls.GetCallmanagmentsList();
            }

            try
            {
                lstCompanyDashBoard.Clear();
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    Session["count"] = CompanyDashBoard.Rows.Count;
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                Count = CompanyDashBoard.Rows.Count,
                                OrgID = Convert.ToInt32(dr["OrgID"]),
                                EXEBRId = Convert.ToInt32(dr["EXEBRId"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                JobType = Convert.ToString(dr["Job_type"]),
                                ServiceType = Convert.ToString(dr["subserviceType"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                RequstedInspectedDate = Convert.ToString(dr["RequstedInspectedDate"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),

                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                //ToDate = Convert.ToString(dr["To_Date"]),

                                // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = Convert.ToString(dr["PO_No_SSJob"]),// Convert.ToString(dr["PONumberOnSubVender"]),

                                StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"]),//6-Feb-2019
                                VendorName = Convert.ToString(dr["VendorName"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                VendorPONo = Convert.ToString(dr["VendorPoNo"]),
                                SubVendorPONo = Convert.ToString(dr["SubVendorPoNo"]),
                                OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                ExecutingBranch = Convert.ToString(dr["ExecutingBranch"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelsubJob.ListDashboard = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }

        [HttpPost]
        public ActionResult CallsManagment(CallsModel vcm, DateTime? FromDate, DateTime? ToDate, string CallNo, FormCollection FC, string Command)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = vcm.FromDate;
            Session["ToDate"] = vcm.ToDate;
            Session["CallNo"] = vcm.Call_No;
            Session["BR_ID"] = vcm.Br_Id;
            vcm.Actual_Visit_Date = FC["VisitDate"];

            //foreach (var key in FC.)
            //{
            //    var value = FC[key];
            //}

            List<string> list = new List<string>();



            string FirstName = "";
            string Date = "";
            string PK_Call_ID = "";

            List<Item> itemList = new List<Item>();
            List<ItemTest> itemListTest = new List<ItemTest>();
            List<abc> C = new List<abc>();
            int v = Convert.ToInt16(Session["count"]);
            if (v > 9)
            {
                v = FC.AllKeys.Count();
            }
            else
            {
                v = v;
            }

            if (FC["CallListData[0].VisitDate"] != null)
            {
                foreach (string key in FC.Keys)
                {
                    Date = FC["CallListData[0].VisitDate"].ToString();
                    FirstName = FC["CallListData[0].FirstName"].ToString();
                    PK_Call_ID = FC["CallListData[0].PK_Call_ID"].ToString();

                    break;

                }

                string[] RDate = Date.Split(',');
                string[] RFirstName = FirstName.Split(',');
                string[] RPK_Call_ID = PK_Call_ID.Split(',');

                for (int i = 0; i < RFirstName.Length; i++)
                {
                    itemListTest.Add(new ItemTest()
                    {
                        Name = RFirstName[i].ToString(),
                        Date = RDate[i].ToString(),
                        PK_Call_ID = RPK_Call_ID[i].ToString()

                    });

                }


            }


            DateTime ActualVDate;

            if (Command == "Search")
            {

                if (vcm.FromDate != null && vcm.ToDate != null || vcm.Call_No != null && vcm.Call_No != "" || vcm.Excuting_Branch != null && vcm.Excuting_Branch != "" || vcm.Originating_Branch != null && vcm.Originating_Branch != "")
                {
                    return RedirectToAction("CallsManagment", vcm);
                }
            }


            string Result = string.Empty;
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();

            string InspectorName = string.Empty;
            string joined = string.Empty;
            DataSet DTChkLeave = new DataSet();


            #region Post Gridview Data
            if (itemListTest != null)
            {
                foreach (var item in itemListTest)
                {
                    if (item.Name != "" && item.Date != "")
                    {
                        DTChkLeave = objDalCalls.ChkLeave(DateTime.Parse(item.Date).ToString("dd/MM/yyyy"), item.Name);
                        if (DTChkLeave.Tables.Count > 1)
                        {
                            if (DTChkLeave.Tables[0].Rows.Count > 0)
                            {
                                // return Json(1);
                                InspectorName = DTChkLeave.Tables[1].Rows[0]["Inspector"].ToString();
                                //// TempData["Error"] = TempData["Error"] + "<br>" + InspectorName + " - " + item.Date.ToString();
                                if (TempData["Error"] != null)
                                    TempData["Error"] = TempData["Error"] + " , " + InspectorName + " - " + item.Date.ToString();
                                else
                                    TempData["Error"] = InspectorName + " - " + item.Date.ToString();
                            }
                            else
                            {

                                ObjModelsubJob.Inspector = item.Name;
                                // ActualVDate = Convert.ToDateTime(item.Date);


                                ObjModelsubJob.Actual_Visit_Date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                                ObjModelsubJob.PK_Call_ID = Convert.ToInt32(item.PK_Call_ID);
                                ObjModelsubJob.AssignStatus = "1";
                                ObjModelsubJob.Status = "Assigned";
                                Result = objDalCalls.UpdateCallAssignBydate(ObjModelsubJob);
                                DataSet CM = new DataSet();
                                CM = objDalCalls.EditCall(Convert.ToInt32(item.PK_Call_ID));

                                dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(item.PK_Call_ID));

                               
                                if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["Homecheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Tuv_Branch"] != null)
                                {
                                    CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Tuv_Branch"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString() , dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString()); //------------------Emails
                                }
                                if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["Vendorcheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Vendor_Email"] != null)
                                {
                                    CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Vendor_Email"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                                }
                                if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["ClientEmailcheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Client_Email"] != null)
                                {
                                    CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Client_Email"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString()); //------------------Emails
                                }
                                /// Inspector Mail
                                if (dsEmailDetails.Tables[0].Rows[0]["EmailID"] != null)
                                {
                                    CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                                }
                               

                            }
                        }
                        else
                        {

                            ObjModelsubJob.Inspector = item.Name;
                            // ActualVDate = Convert.ToDateTime(item.Date);


                            ObjModelsubJob.Actual_Visit_Date = Convert.ToDateTime(item.Date).ToString("dd/MM/yyyy");
                            ObjModelsubJob.PK_Call_ID = Convert.ToInt32(item.PK_Call_ID);
                            ObjModelsubJob.AssignStatus = "1";
                            ObjModelsubJob.Status = "Assigned";
                            Result = objDalCalls.UpdateCallAssignBydate(ObjModelsubJob);
                            DataSet CM = new DataSet();
                            CM = objDalCalls.EditCall(Convert.ToInt32(item.PK_Call_ID));

                            dsEmailDetails = objDalCalls.GetEmailDetails(Convert.ToInt32(item.PK_Call_ID));


                            if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["Homecheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Tuv_Branch"] != null)
                            {
                                CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Tuv_Branch"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                            }
                            if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["Vendorcheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Vendor_Email"] != null)
                            {
                                CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Vendor_Email"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                            }
                            if (Convert.ToBoolean(dsEmailDetails.Tables[0].Rows[0]["ClientEmailcheckbox"]) == true && dsEmailDetails.Tables[0].Rows[0]["Client_Email"] != null)
                            {
                                CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["Client_Email"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                            }
                            /// Inspector Mail
                            if (dsEmailDetails.Tables[0].Rows[0]["EmailID"] != null)
                            {
                                CallManagementMail(Convert.ToInt32(item.PK_Call_ID), dsEmailDetails.Tables[0].Rows[0]["EmailID"].ToString(), dsEmailDetails.Tables[0].Rows[0]["InspectorName"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Vendor_Name"].ToString(), dsEmailDetails.Tables[0].Rows[0]["Po_Number"].ToString());  //------------------Emails
                            }

                        }
                    }

                }
                return RedirectToAction("CallsManagment");
            }
            #endregion

            else
            {

                CompanyDashBoard = objDalCalls.GetCallmanagmentsList();
                try
                {


                    if (CompanyDashBoard.Rows.Count > 0)
                    {
                        foreach (DataRow dr in CompanyDashBoard.Rows)
                        {
                            lstCompanyDashBoard.Add(
                                new CallsModel
                                {

                                    PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                    OrgID = Convert.ToInt32(dr["OrgId"]),
                                    Call_No = Convert.ToString(dr["Call_No"]),
                                    Company_Name = Convert.ToString(dr["Company_Name"]),
                                    Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                    Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                    Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                    ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                                              //Inspector =  Convert.ToString(dr["Inspector"]),
                                    Project_Name = Convert.ToString(dr["Project_Name"]),
                                    CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                    Reasion = Convert.ToString(dr["Reasion"]),
                                    ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                    Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                    SAP_Number = Convert.ToString(dr["SAP_No"]),
                                    Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                    Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                    RequstedInspectedDate = Convert.ToString(dr["RequstedInspectedDate"]),
                                    Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),

                                    Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                    // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                    //ToDate = Convert.ToString(dr["To_Date"]),

                                    // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),
                                    PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                    PONumberOnSubVender = Convert.ToString(dr["PO_No_SSJob"]),// Convert.ToString(dr["PONumberOnSubVender"]),

                                    StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                    InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                    ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                    LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"]),//6-Feb-2019



                                    VendorName = Convert.ToString(dr["VendorName"]),
                                    SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                    VendorPONo = Convert.ToString(dr["VendorPoNo"]),
                                    SubVendorPONo = Convert.ToString(dr["SubVendorPoNo"]),
                                    OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                    ExecutingBranch = Convert.ToString(dr["ExecutingBranch"]),
                                    EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {

                    string Error = ex.Message.ToString();
                }

                ViewData["BranchList"] = lstCompanyDashBoard;
                // return RedirectToAction("CallsManagment");

                #region
                DataSet DSEditQutationTabledatas = new DataSet();
                DSEditQutationTabledatas = objDalCalls.GetBranchName();
                ObjModelsubJob.UserBranchName = Convert.ToString(DSEditQutationTabledatas.Tables[0].Rows[0]["Branch_Name"]);
                ViewBag.UserBranchName = Convert.ToString(DSEditQutationTabledatas.Tables[0].Rows[0]["Branch_Name"]);
                ObjModelsubJob.Br_Id = 0;

                var Data2 = objDalCalls.GetBranchList();
                ViewBag.SubCatlistss = new SelectList(Data2, "Br_Id", "Branch_Name");

                var Data3 = objDalCalls.GetInspectorList();
                ViewBag.SubCatlist = new SelectList(Data3, "PK_UserID", "FirstName");
                #endregion





                return View(ObjModelsubJob);




            }



            CompanyDashBoard = objDalCalls.GetCallmanagmentsList();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                //PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                //Call_No = Convert.ToString(dr["Call_No"]),
                                //Company_Name = Convert.ToString(dr["Company_Name"]),
                                //Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                //Status = Convert.ToString(dr["Status"]),
                                //Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                //Urgency = Convert.ToString(dr["Urgency"]),
                                //Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                //ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                //Inspector = Convert.ToString(dr["Inspector"]),
                                //Project_Name = Convert.ToString(dr["Project_Name"]),
                                //CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),

                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                //ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                SAP_Number = "NA",// Convert.ToString(dr["SAP_Number"]),
                                Sub_Job_Number = Convert.ToString(dr["Sub_Job"]),
                                Inspection_CallIntimationReceivedOn = "NA",// Convert.ToString(dr["Inspection_CallIntimationReceivedOn"]),
                                RequstedInspectedDate = "NA",// Convert.ToString(dr["RequstedInspectedDate "]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),


                                ExtendCall_Status = "NA",// Convert.ToString(dr["CallStatus "]),
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = "NA",// Convert.ToString(dr["PONumberOnSubVender"]),

                                StageOfInspection = "NA",// Convert.ToString(dr["StageOfInspection"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = "NA",// Convert.ToString(dr["ItemsToBeInpsected"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // ObjModelsubJob = lstCompanyDashBoard;
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelsubJob.ListDashboard = lstCompanyDashBoard;
            // return RedirectToAction("CallsManagment");

            #region
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = objDalCalls.GetBranchName();
            ObjModelsubJob.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            ViewBag.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            ObjModelsubJob.Br_Id = 0;

            var Data1 = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Data1, "Br_Id", "Branch_Name");

            var Data = objDalCalls.GetInspectorList();
            ViewBag.SubCatlist = new SelectList(Data, "PK_UserID", "FirstName");
            #endregion

            return View(ObjModelsubJob);
        }


        [HttpPost]
        public ActionResult CallsManagmentSearch(CallsModel vcm)
        {
            return RedirectToAction("CallsManagment", vcm);
        }


        [HttpPost]
        public ActionResult SendMailToInspector(string[] arr)
        {
            for (int i = 0; i < arr.Count(); i++)
            {
                // admin.ApprovedRegistrationTable(Convert.ToInt32(arr[i]));
            }
            TempData["regs"] = "Approved";
            return RedirectToAction("CallsManagment", "Admin");
        }
        #endregion

        #region CallAssignment


        public ActionResult CallAssignment()
        {
            Session["GetExcelData"] = "Yes";

            var Dataa = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Dataa, "Br_Id", "Branch_Name");
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();


            CompanyDashBoard = objDalCalls.GetCallAssignList();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = Convert.ToString(dr["PO_No_SSjob"]),



                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                //Inspector =  Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                AI = Convert.ToString(dr["LastAssName"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                //Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Sub_Job = Convert.ToString(dr["A1"]),
                                Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                RequstedInspectedDate = Convert.ToString(dr["RequstedInspectedDate"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                //ToDate = Convert.ToString(dr["To_Date"]),

                                // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),

                                StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelsubJob.CADashboard = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }

        [HttpPost]
        public ActionResult CallAssignment(CallsModel vcm)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = vcm.FromDate;
            Session["ToDate"] = vcm.ToDate;
            Session["CallNo"] = vcm.Call_No;
            //Session["Branch"] = vcm.Br_Id;
            Session["OrgBranch"] = vcm.Originating_Branch;

            var Dataa = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Dataa, "Br_Id", "Branch_Name");
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();


            CompanyDashBoard = objDalCalls.GetAssignmentDashBoardWithDate(vcm);
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = Convert.ToString(dr["PO_No_SSjob"]),



                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                //Inspector =  Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                AI = Convert.ToString(dr["LastAssName"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                RequstedInspectedDate = Convert.ToString(dr["RequstedInspectedDate"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),

                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                //ToDate = Convert.ToString(dr["To_Date"]),

                                // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),

                                StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"])//6-Feb-2019
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelsubJob.CADashboard = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }


        //  [HttpPost]
        public ActionResult UpdateCallAssignment(int? PK_Call_ID)
        {


            string Result = string.Empty;
            try
            {
                if (PK_Call_ID != 0)
                {
                    ObjModelsubJob.PK_Call_ID = Convert.ToInt32(PK_Call_ID);
                    ObjModelsubJob.AssignStatus = "0";
                    Result = objDalCalls.AssigntUpdate(ObjModelsubJob);

                    if (Result != "" && Result != null)
                    {
                        TempData["InsertCompany"] = Result;

                        return RedirectToAction("CallsManagment");
                    }

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return View();
        }
        #endregion

        #region Emails
        public void EmailSendtouser(string EmailID, string MobileNo, string FirstName)
        {
            try
            {
                MailMessage msg = new MailMessage();
                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string bodyTxt = "";
                bodyTxt += " Dear Sir/Madam, " + "<br><br>";
                bodyTxt += "Please be informed that our inspector " + FirstName + " will be attending the inspection call as per details below. <br><br>";
                bodyTxt += " Inspectors contact details : " + MobileNo + " <br><br>";
                bodyTxt += "  Email Id : " + EmailID + " <br><br>";

                //bodyTxt += "< p style = 'font-family:'Open Sans', arial, sans-serif;font-size:14px;color:#000000;text-align:justify; font-weight:normal;' > If you have any queries or need any assistance, please get in touch with our support team at - < a href = 'mailto:support@ocb.com?Subject=Online Enquiry' style = 'font-family:'Open Sans', arial, sans-serif;color:#630811;text-align:justify;font-weight:bold;text-decoration:none;' > support@ocb.com </ a ></ p >";
                bodyTxt += "Best Regards,<br>";
                bodyTxt += "TUV India Pvt Ltd.<br>";
                msg.From = new MailAddress(MailFrom);
                //string To = EmailID.ToString();
                //char[] delimiters = new[] { ',', ';', ' ' };
                //string[] EmailIDs = To.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                //foreach (string MultiEmailTemp in EmailIDs)
                //{
                //    msg.To.Add(new MailAddress(MultiEmailTemp));
                //}
                msg.To.Add(EmailID);
                //msg.CC.Add(MailCC);
                //msg.Bcc.Add(MailBCC);
                msg.Subject = "Confirmation of Inspection Visit";
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient(smtpHost);
                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(msg);
            }
            catch (Exception)
            {


            }
        }
        #endregion



        public ActionResult CallsListByInspector()
        {

            Session["PK_Call_ID"] = null;
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();
            CompanyDashBoard = objDalCalls.GetCallsListByInspector();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                // ModifyDate = DateTime.ParseExact(dr["ModifyDate"].ToString(), "dd/mm/yyyy", null),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = DateTime.ParseExact(dr["CreatedDate"].ToString(),"dd/mm/yyyy",null),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                SubSubJobNo = Convert.ToString(dr["Sub_job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                SubVendorPONo = Convert.ToString(dr["SubVendorPoNo"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lst1 = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }


        #region  Calls Details Display by Inspector

        [HttpGet]
        public ActionResult CallsDetails(int? PK_Call_ID)
        {

            ViewBag.check = "productcheck";
            string[] splitedProduct_Name;
            Session["PK_Call_ID"] = null;
            var Data = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data, "Br_Id", "Branch_Name");

            var UserData = objDalCalls.GetInspectorList();
            ViewBag.Userlist = new SelectList(UserData, "PK_UserID", "FirstName");

            var ReasonData = objDalCalls.GetReasonList("");
            ViewBag.Reasonlist = new SelectList(ReasonData, "CReason_Id", "Reason");

            #region bind originating branch
            var Data1 = objDalCalls.GetBranchList();
            ViewBag.SubCatlist = new SelectList(Data1, "Br_Id", "Branch_Name");
            #endregion

            #region Job Bind Format of report 
            DataTable DTGetUploadedFileFormat = new DataTable();
            List<FileDetails> lstEditFileDetailsFormat = new List<FileDetails>();
            DTGetUploadedFileFormat = objDalCalls.EditUploadedFileFormat1(PK_Call_ID);
            if (DTGetUploadedFileFormat.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedFileFormat.Rows)
                {
                    lstEditFileDetailsFormat.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstEditFileDetailsFormat"] = lstEditFileDetailsFormat;
                ObjModelsubJob.FileDetailsFormat1 = lstEditFileDetailsFormat;
            }
            #endregion

            #region Bind Sub Job Attachment
            DataTable DTGetUploadedSubJobFiles = new DataTable();
            List<FileDetails> lstSubJobFiles = new List<FileDetails>();
            DTGetUploadedSubJobFiles = objDalCalls.GetDownloadSubJobFileFromCalls(PK_Call_ID);
            if (DTGetUploadedSubJobFiles.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedSubJobFiles.Rows)
                {
                    lstSubJobFiles.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstSubJobFiles"] = lstSubJobFiles;
                ObjModelsubJob.SubJobFileDetails = lstSubJobFiles;
            }
            #endregion

            #region Bind Sub Sub Job Attachment
            DataTable DTGetUploadedSubSubJobFiles = new DataTable();
            List<FileDetails> lstSubSubJobFiles = new List<FileDetails>();
            DTGetUploadedSubSubJobFiles = objDalCalls.GetDownloadSubSubJobFileFromCalls(PK_Call_ID);
            if (DTGetUploadedSubSubJobFiles.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedSubSubJobFiles.Rows)
                {
                    lstSubSubJobFiles.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstSubJobFiles"] = lstSubSubJobFiles;
                ObjModelsubJob.SubSubJobFileDetails = lstSubSubJobFiles;
            }
            #endregion

            #region Bind Call Attachment

            DataTable DTGetUploadedCallFiles = new DataTable();
            List<FileDetails> lstCallFileDetails = new List<FileDetails>();
            DTGetUploadedCallFiles = objDalCalls.EditCallFiles(PK_Call_ID);
            if (DTGetUploadedCallFiles.Rows.Count > 0)
            {
                foreach (DataRow dr in DTGetUploadedCallFiles.Rows)
                {
                    lstCallFileDetails.Add(
                       new FileDetails
                       {

                           PK_ID = Convert.ToInt32(dr["PK_ID"]),
                           FileName = Convert.ToString(dr["FileName"]),
                           Extension = Convert.ToString(dr["Extenstion"]),
                           IDS = Convert.ToString(dr["FileID"]),
                       }
                     );
                }
                ViewData["lstCallFileDetails"] = lstCallFileDetails;
                ObjModelsubJob.CallFileDetails = lstCallFileDetails;
            }
            #endregion

            if (PK_Call_ID != 0)
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DataSet DSEditQutationTabledata = new DataSet();
                DSEditQutationTabledata = objDalCalls.EditCallByInspector(PK_Call_ID);
                if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Status"]);

                    ObjModelsubJob.Type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Br_Id = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Executing_Branch"]);
                    ObjModelsubJob.Originating_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Originating_Branch"]);
                    ObjModelsubJob.Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job"]);
                    ObjModelsubJob.Sub_Job = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Job"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.End_Customer = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["End_Customer"]);


                    //ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Name"]);
                    //ObjModelsubJob.PO_Number = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["PO_Number"]);

                    ObjModelsubJob.Vendor_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorName"]); ///Second Level Vendor Name
                    ObjModelsubJob.PO_Number = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPONo"]); ///Second Level Vendor PO
                    ObjModelsubJob.SubVendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPODate"]); ///Second Level Vendor PO Date

                    ObjModelsubJob.TopSubVendorName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopVendor"]); /// First level Vendor Name
                    ObjModelsubJob.TopSubVendorPONo = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopVendorPO"]); /// First level Vendor PO
                    ObjModelsubJob.TopvendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["TopvendorPODate"]); /// First level Vendor PO date


                    ObjModelsubJob.PK_Call_ID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["PK_Call_ID"]);
                    ObjModelsubJob.Contact_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Contact_Name"]);
                    ObjModelsubJob.Call_Recived_date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Recived_date"]);
                    ObjModelsubJob.Call_Request_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_Request_Date"]);
                    ObjModelsubJob.Source = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Source"]);
                    ObjModelsubJob.Planned_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Planned_Date"]);
                    ObjModelsubJob.Urgency = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Urgency"]);
                    ObjModelsubJob.Competency_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Competency_Check"]);
                    ObjModelsubJob.Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Category"]);
                    ObjModelsubJob.Sub_Category = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Category"]);
                    ObjModelsubJob.Empartiality_Check = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Empartiality_Check"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Job_Location = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Job_Location"]);
                    ObjModelsubJob.Final_Inspection = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Final_Inspection"]);
                    ObjModelsubJob.From_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["From_Date"]);
                    ObjModelsubJob.To_Date = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["To_Date"]);
                    ObjModelsubJob.FirstName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Inspector"]);

                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Branch = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Tuv_Branch"]);
                    ObjModelsubJob.Homecheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["Homecheckbox"]);
                    ObjModelsubJob.Vendorcheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["Vendorcheckbox"]);
                    ObjModelsubJob.ClientEmailcheckbox = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["ClientEmailcheckbox"]);

                    ObjModelsubJob.Call_No = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Call_No"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Vendor_Contact"]);


                    ObjModelsubJob.SubVendorPODate = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SubVendorPODate"]); ///Second Level Vendor PO Date





                    ObjModelsubJob.formats_Of_Report = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["formats_Of_Report"]);
                    ViewBag.File = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["formats_Of_Report"]);
                    ObjModelsubJob.Description = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Description"]);
                    ObjModelsubJob.Quantity = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["Quantity"]);
                    ObjModelsubJob.Product_Name = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Product_item"]);
                    ObjModelsubJob.Special_Notes = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Special_Notes"]);
                    ObjModelsubJob.EstimatedHours = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["EstimatedHours"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.Sub_Type = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Sub_Type"]);
                    ObjModelsubJob.FirstSubJob = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["FirstSubJob"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECname"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["DECNumber"]);

                    ObjModelsubJob.CoordinatorName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CoordinatorName"]);
                    ObjModelsubJob.CoordinatorContactDetail = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["CoordinatorContactDetail"]);
                    ObjModelsubJob.SAP_no = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["SAP_no"]);
                    ObjModelsubJob.Reasion = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["resone"]);
                    ObjModelsubJob.chkARC = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.ChkContinuousCall = Convert.ToBoolean(DSEditQutationTabledata.Tables[0].Rows[0]["ChkContinuousCall"]);

                    List<string> Selected = new List<string>();
                    var Existingins = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Product_item"]);
                    splitedProduct_Name = Existingins.Split(',');
                    foreach (var single in splitedProduct_Name)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.EditproductName = Selected;
                }

                DataTable DTGetProductLst = new DataTable();
                List<NameCodeProduct> lstEditInspector = new List<NameCodeProduct>();
                DTGetProductLst = objDAM.getlistforEdit();
                DTGetProductLst = objDAM.getlistforEdit1();
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
                ViewBag.ProjectTypeItems1 = ProductcheckItems;
                ViewData["ProjectTypeItems"] = ProductcheckItems;

                ViewData["Drpproduct"] = objDAM.GetDrpList();
                return View(ObjModelsubJob);
            }

            else
            {
                return RedirectToAction("CallsListByInspector");
            }
        }

        [HttpPost]
        public ActionResult CallsDetails(CallsModel CD)
        {
            string Result = string.Empty;

            if (CD.Status == null)//8/4/2023
            {

            }
            else
            {
                if (CD.Status == "Closed")
                {
                    CD.Inspector = CD.FirstName;
                    CD.AssignStatus = "1";
                    CD.IsVisitReportGenerated = "No";
                    Result = objDalCalls.UpdateReason(CD);
                }
                else if (CD.Status == "Cancelled")
                {
                    CD.Inspector = CD.FirstName;
                    if (CD.FirstName != null || CD.FirstName != "")
                    {
                        CD.AssignStatus = "1";
                    }
                    else
                    {
                        CD.AssignStatus = "0";
                    }

                    Result = objDalCalls.UpdateReason(CD);
                }
                else
                {
                    //CD.Inspector = null;
                    //CD.AssignStatus = "0";
                    Result = objDalCalls.UpdateReason(CD);
                }
            }

            

            if (Result != null && Result != "")
            {
                TempData["UpdateCompany"] = Result;
            }
            return RedirectToAction("CallsListByInspector");

        }
        #endregion

        public ActionResult DeleteCallsData(int? PK_Call_ID)
        {
            int Result = 0;
            try
            {
                Result = objDalCalls.DeleteCalls(PK_Call_ID);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("CallsList");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public JsonResult GetEBEmailByBrName(int? id)
        {
            //DataSet DSGetEmail = new DataSet();
            //DSGetEmail = objDalCalls.GetEmailByBranch(Convert.ToInt32(id));

            //if (DSGetEmail.Tables[0].Rows.Count > 0)
            //{
            //    ObjModelsubJob.EB_Email = DSGetEmail.Tables[0].Rows[0]["Coordinator_Email_Id"].ToString();
            //}
            //return Json(ObjModelsubJob, JsonRequestBehavior.AllowGet);//manoj
            DataSet DSGetEmail = new DataSet();
            DSGetEmail = objDalCalls.GetEmailByBranch(Convert.ToInt32(id));
            string OBEmailID = Convert.ToString(Session["Originating_Branch_EmailID"]);
            if (DSGetEmail.Tables[0].Rows.Count > 0)
            {
                ObjModelsubJob.EB_Email = OBEmailID + " , " + DSGetEmail.Tables[0].Rows[0]["Coordinator_Email_Id"].ToString();
            }
            return Json(ObjModelsubJob, JsonRequestBehavior.AllowGet);
        }

        #region
        [HttpGet]
        public ActionResult ExportIndex(CallsModel c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<CallsModel> CreateExportableGrid(CallsModel c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.SAP_Number).Titled("SAP Number");
            grid.Columns.Add(model => model.Sub_Job_Number).Titled("Sub-Job Number");
            grid.Columns.Add(model => model.Inspection_CallIntimationReceivedOn).Titled("Inspection Call Intimation Received On");
            grid.Columns.Add(model => model.RequstedInspectedDate).Titled("Requested inspection date (from to date if calls are continuous)");
            
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.LastInspectorName_DateOfInspection).Titled("Last Inspector's Name and Date of Inspection");            

            grid.Columns.Add(model => model.Status).Titled("Call Status");
            grid.Columns.Add(model => model.ExtendCall_Status).Titled("Extend Call Status");
            grid.Columns.Add(model => model.PONumberOnVender).Titled("PO Number on Vendor");
            grid.Columns.Add(model => model.PONumberOnSubVender).Titled("PO Number on Sub Vendor");
            grid.Columns.Add(model => model.OriginatingBranch).Titled("Originating Branch");
            grid.Columns.Add(model => model.StageOfInspection).Titled("Stage of Inspection");
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.ItemsToBeInpsected).Titled("Items to be Inspected");
            grid.Columns.Add(model => model.EstimatedHours).Titled("Estimated Hours");
            grid.Columns.Add(model => model.FirstName).Titled("Inspector Name");

            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelsubJob.ListDashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetData(CallsModel c)//6-Feb-2019
        {
            DataTable DTFeedback = new DataTable();
            if ((Session["FromDate"] != null && Session["ToDate"] != null) || Session["CallNo"] != null || Session["BR_ID"] != null)
            {
                if (Session["FromDate"] != null && Session["ToDate"] != null)
                {
                    c.FromDate = (Session["FromDate"]).ToString();
                    c.ToDate = (Session["ToDate"]).ToString();
                }

                if (Session["CallNo"] != null)
                {
                    c.Call_No = Convert.ToString(Session["CallNo"]);

                }
                if (Session["BR_ID"] != null)
                {
                    c.Br_Id = Convert.ToInt32(Session["BR_ID"]);
                }

                DTFeedback = objDalCalls.GetCallmanagmentsList();
            }
            else
            {
                DTFeedback = objDalCalls.GetCallmanagmentsList();

            }
            /*
            if (Session["GetExcelData"] == "Yes")
            {
                DTFeedback = objDalCalls.GetCallmanagmentsList();
            }
            else
            {

                c.FromDate = (Session["FromDate"]).ToString();
                c.ToDate = (Session["ToDate"]).ToString();
                c.Call_No = Convert.ToString(Session["CallNo"]);
                c.Br_Id = Convert.ToInt32(Session["BR_ID"]);

                // DTFeedback = objDalCalls.GetCallmanagmentsListAfterSearch(c);
                DTFeedback = objDalCalls.GetCallmanagmentsListByCondition(c);

            }
            */

            var Dataa = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Dataa, "Br_Id", "Branch_Name");
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();

            #region
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = objDalCalls.GetBranchName();
            ObjModelsubJob.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            ObjModelsubJob.Br_Id = 0;
            ViewBag.UserBranchName = Convert.ToString(DSEditQutationTabledata.Tables[0].Rows[0]["Branch_Name"]);
            #endregion


            var Data = objDalCalls.GetInspectorList();
            ViewBag.SubCatlist = new SelectList(Data, "PK_UserID", "FirstName");

            try
            {
                if (DTFeedback.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),

                                OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                //Inspector =  Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),

                                SAP_Number = "NA",// Convert.ToString(dr["SAP_Number"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                //RequstedInspectedDate  = Convert.ToString(dr["Call_Request_Date"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),

                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                //ToDate = Convert.ToString(dr["To_Date"]),

                                // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = Convert.ToString(dr["PO_No_SSJob"]),// Convert.ToString(dr["PONumberOnSubVender"]),

                                StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"])


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;
            ObjModelsubJob.ListDashboard = lstCompanyDashBoard;

            return ObjModelsubJob.ListDashboard;
        }

        #endregion


        #region Export to excel Inspector call

        [HttpGet]
        public ActionResult ExportIndexInspector()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGridInspector();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<CallsModel> CreateExportableGridInspector()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetDataInspector());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");
            grid.Columns.Add(model => model.SubSubJobNo).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor Name");
            grid.Columns.Add(model => model.PO_Number).Titled("Vendor PO No");
            grid.Columns.Add(model => model.SubVendorName).Titled("Sub Vendor Name");
            grid.Columns.Add(model => model.SubVendorPONo).Titled("Sub Vendor PO No");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.StageOfInspection).Titled("Stage of Inspection");
            grid.Columns.Add(model => model.EstimatedHours).Titled("Estimated Hours");
            grid.Columns.Add(model => model.ItemsToBeInpsected).Titled("Items to be Inspected");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");
            grid.Columns.Add(model => model.Call_Type).Titled("Call Type");
            grid.Columns.Add(model => model.ExtendCall_Status).Titled("Extend Call Status");
            grid.Columns.Add(model => model.Urgency).Titled("Urgency");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Received date");

            

            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelsubJob.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetDataInspector()
        {
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();
            CompanyDashBoard = objDalCalls.GetCallsListByInspector();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                Count = CompanyDashBoard.Rows.Count,
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),

                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                SubSubJobNo = Convert.ToString(dr["Sub_job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                SubVendorPONo = Convert.ToString(dr["SubVendorPoNo"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lst1 = lstCompanyDashBoard;

            return ObjModelsubJob.lst1;
        }


        #endregion

        #region export to excel CallList Operation-Calls
        public ActionResult ExportCallList(CallsModel a)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableCallList(a);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "CallList-" + filename + ".xlsx");
            }
        }

        private IGrid<CallsModel> CreateExportableCallList(CallsModel a)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetDataCallList(a));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.Call_No).Titled("Call No");

            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Call_Request_Date).Titled("Call Requested Date");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Recived Date");
            grid.Columns.Add(model => model.CreatedDate).Titled("Call Created Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Call Created By");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");            
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.V1).Titled("Sub Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Vendor Name");
            grid.Columns.Add(model => model.P1).Titled("Sub Vendor PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Vendor PO No");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Excuting_Branch).Titled("Executing Branch");
            grid.Columns.Add(model => model.Call_Type).Titled("Call Type");
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.StageOfInspection).Titled("Stage of Inspection");
            grid.Columns.Add(model => model.EstimatedHours).Titled("Estimated Hours");
            grid.Columns.Add(model => model.ItemsToBeInpsected).Titled("Items to be Inspected");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.ExtendCall_Status).Titled("Status Call Extend");
            grid.Columns.Add(model => model.Urgency).Titled("Urgency");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Recived date");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");

            // grid.Columns.Add(model => model.CreatedDate.Value.ToString("dd/MM/yyyy")).Titled("Created Date").Css("aa");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date").Css("aa");




            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelsubJob.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetDataCallList(CallsModel a)
        {
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                a.FromDate = Session["FromDate"].ToString();
                a.ToDate = Session["ToDate"].ToString();
                CompanyDashBoard = objDalCalls.GetCallsListWithDate(a);
            }
            else
            {

                CompanyDashBoard = objDalCalls.GetCallsList();
            }
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Excuting_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),



                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lst1 = lstCompanyDashBoard;

            return ObjModelsubJob.lst1;
        }
        #endregion

        public JsonResult GetInsByExBr(int? sid)
        {
            var i = objDalCalls.GetInsByExBr(Convert.ToInt32(sid)).ToList();
            return Json(i, JsonRequestBehavior.AllowGet);
        }

        #region Export to Excel - added by Ankush
        [HttpGet]
        public ActionResult ExportIndexAssignment(CallsModel a)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateAssignmentExportableGrid(a);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<CallsModel> CreateAssignmentExportableGrid(CallsModel a)  //CreateExportableGrid
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetAssignmentData(a));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.SAP_Number).Titled("SAP Number");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job Number");
            grid.Columns.Add(model => model.Inspection_CallIntimationReceivedOn).Titled("Inspection Call Intimation Received On");
            grid.Columns.Add(model => model.Call_Request_Date).Titled("Requested Inspection Date");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.LastInspectorName_DateOfInspection).Titled("Last Inspector's Name and Date of Inspection");
            grid.Columns.Add(model => model.Status).Titled("Call Status");
            grid.Columns.Add(model => model.PONumberOnVender).Titled("PO Number on Vendor");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.StageOfInspection).Titled("Stage of Inspection");
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.ItemsToBeInpsected).Titled("Items to be Inspected");

            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelsubJob.CADashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            return grid;
        }

        public List<CallsModel> GetAssignmentData(CallsModel a) //GetData
        {
            var Dataa = objDalCalls.GetBranchList();
            ViewBag.SubCatlistss = new SelectList(Dataa, "Br_Id", "Branch_Name");

            DataTable DTAssignment = new DataTable();
            List<CallsModel> lstAssignment = new List<CallsModel>();

            if (Session["GetExcelData"] == "Yes")
            {
                DTAssignment = objDalCalls.GetCallAssignList();
            }
            else
            {
                a.From_Date = Session["FromDate"].ToString();
                a.To_Date = Session["ToDate"].ToString();
                a.Call_No = Session["CallNo"].ToString();
                //a.Br_Id = Convert.ToInt32(Session["Branch"]);
                a.ExBr_Id = Convert.ToInt32(Session["OrgBranch"]);

                DTAssignment = objDalCalls.GetAssignmentDashBoardWithDate(a);
            }
            try
            {
                if (DTAssignment.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAssignment.Rows)
                    {
                        lstAssignment.Add(
                            new CallsModel
                            {
                                CACount = DTAssignment.Rows.Count,
                                //Inspector = Convert.ToString(dr["Inspector"]),
                                PONumberOnVender = Convert.ToString(dr["Po_Number"]),
                                PONumberOnSubVender = Convert.ToString(dr["PO_No_SSjob"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = "NA",// Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Urgency = "NA",// Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = "NA",// Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = DateTime.Now,//Convert.ToDateTime(dr["ModifyDate"]),
                                //Inspector =  Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Inspection_CallIntimationReceivedOn = Convert.ToString(dr["Call_Recived_Date"]),
                                RequstedInspectedDate = Convert.ToString(dr["RequstedInspectedDate"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                // FromDate = Convert.ToString(dr["ContiniousCalls"]),
                                //ToDate = Convert.ToString(dr["To_Date"]),
                                // CallStatus ="NA",// Convert.ToString(dr["CallStatus "]),
                                StageOfInspection = Convert.ToString(dr["Sub_Category"]),
                                InspectionLocation = Convert.ToString(dr["Inspection_Location"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastInspectorName"])//6-Feb-2019
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstAssignment;
            ObjModelsubJob.CADashboard = lstAssignment;

            return ObjModelsubJob.CADashboard;
        }
        #endregion
        //Delete Uploaded Multiple file in Quotation Code added by manoj Sharma on 12 March 2020
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
                Guid guid = new Guid(id);
                DTGetDeleteFile = objDalCalls.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                }
                if (id != null && id != "")
                {
                    Results = objDalCalls.DeleteUploadedFile(id);
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
        //public FileResult Download(String p, String d)
        //{
        //    return File(Path.Combine(Server.MapPath("~/Content/JobDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        //}

        public void Download(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCalls.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
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


        public JsonResult CheckLeave(string FromDate, string ToDate, string ActualVisitDate, string Inspector, string chkContinuousCall)//Checking Existing User Name
        {
            List<CallsModel> lmd = new List<CallsModel>();
            string Result = string.Empty;
            string InspectorName = string.Empty;
            string joined = string.Empty;

            DataSet DTChkLeave = new DataSet();
            try
            {
                //if (chkContinuousCall=="true")
                if (FromDate != "" && ToDate != null)
                {
                    DTChkLeave = objDalCalls.ChkLeaveForCountinuous(FromDate, ToDate, ActualVisitDate, Inspector);

                    if (DTChkLeave.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTChkLeave.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                        {
                            lmd.Add(new CallsModel
                            {
                                Actual_Visit_Date = Convert.ToString(dr["DateSE"]),

                            });

                        }
                        joined = string.Join(",", lmd.Select(x => x.Actual_Visit_Date));
                        InspectorName = DTChkLeave.Tables[1].Rows[0]["Inspector"].ToString();

                        return Json(InspectorName + " is on leave for the Dates " + joined.ToString());
                    }
                }
                else
                {
                    DTChkLeave = objDalCalls.ChkLeave(ActualVisitDate, Inspector);

                    if (DTChkLeave.Tables[0].Rows.Count > 0)
                    {
                        // return Json(1);
                        InspectorName = DTChkLeave.Tables[1].Rows[0]["Inspector"].ToString();
                        return Json(InspectorName + " is on leave for date : " + ActualVisitDate.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(0);
        }

        List<CallsModel> lstInsSchedule = new List<CallsModel>();
        public ActionResult InspectorsSchedule(string Type)
        {
            //Session["GetExcelData"] = "Yes";

            lstInsSchedule = objDAM.GetInspectorSchedule();
            //ViewData["EnquiryMaster"] = lstInsSchedule;
            
            ObjModelsubJob.lstInspectorSchedule = lstInsSchedule;
            ViewBag.CostSheet = lstInsSchedule;
            return View(ObjModelsubJob);
        }

        //Record Search By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        [HttpPost]
        public ActionResult InspectorsSchedule(string FromDate, string ToDate)
        {
            //Session["GetExcelData"] = null;
            //Session["FromDate"] = FromDate;
            //Session["ToDate"] = ToDate;
            //IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            //DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            //DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            //List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();

            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDAM.GetInspectorScheduleFromDate(FromDate); //objDalEnquiryMaster.GetDataByDateWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstInsSchedule.Add(
                       new CallsModel
                       {
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                           VendorName = Convert.ToString(dr["VendorName"]),
                           SubVendorName = Convert.ToString(dr["SubVendorName"]),
                           InspectorAddress = Convert.ToString(dr["Address"]),
                           CurrentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                           EstimatedHours = Convert.ToString(dr["EstimatedHours"])
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                ObjModelsubJob.lstInspectorSchedule = lstInsSchedule;
                ViewBag.CostSheet = lstInsSchedule;
                return View(ObjModelsubJob);
            }
            ViewData["EnquiryMaster"] = lstInsSchedule;
            TempData["Result"] = null;
            TempData.Keep();
            ObjModelsubJob.lstInspectorSchedule = lstInsSchedule;
            ViewBag.CostSheet = lstInsSchedule;
            return View(ObjModelsubJob);
        }


        public JsonResult GetPlannedDate(string PK_Call_ID)
        {
            
            string strPlannedDate = string.Empty;

            strPlannedDate = objDalCalls.GetPlannedDate(PK_Call_ID);          
            return Json(strPlannedDate);
        }

        public JsonResult GetTSDate(string PlannedDate ,string Inspector )
        {
            DataTable dtTS = new DataTable();
            int intTotalTime = 0;

            dtTS = objDalCalls.GetInspectorTS(Inspector, PlannedDate);
            intTotalTime = Convert.ToInt32( dtTS.Rows[0]["TotalTime"].ToString());

            return Json(intTotalTime);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(CallsModel AddPO)
        {
            string Result = string.Empty;
            string InspectorName = string.Empty;
            string joined = string.Empty;


            try
            {

                Result = objDalCalls.CheckSingleCall(AddPO.PK_Call_ID.ToString());
                if (Result == "NOSINGLECALL")
                {
                    ViewBag.checkCall = "Mentioned call is single call , thus we have changed the call to Cancelled and deleted all relevent Data";

                }
                Result = objDalCalls.InsertUpdateAddMandays(AddPO);
            }


            catch (Exception ex)
            {
                string errMsg = ex.Message.ToString();
                return Json(new { Result = "ERROR" });
            }

            return Json(new { Result = "SUCCESS" });
        }


        public JsonResult GetCallReason(string Status)
        {
            DataTable DsCompanyAddr = new DataTable();
            List<CallReasonMaster> lstCompanyAddr = new List<CallReasonMaster>();

            string CompAddress = string.Empty;
            if (Status != null && Status != "")
            {
                lstCompanyAddr = objDalCalls.GetReasonList(Status);                
                return Json(lstCompanyAddr, JsonRequestBehavior.AllowGet);                
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }

        public JsonResult Updatedeclare(string pkCallID)
        {
            

            string Result = string.Empty;
            try
            {
                Result = objDalCalls.UpdateDeclareStatus(pkCallID);

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }

        /*
         * public JsonResult GetScope(string Status)
        {

            string Result = string.Empty;

            try
            {
                Result = objDalCalls.GetScopeDetails(Status);

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }
        */

        public JsonResult GetCompetancy(string Name, string Status)
        {


            string Result = string.Empty;
            string strResult = string.Empty;
            DataTable dtScope = new DataTable();
            bool blnCompetant = false;

            try
            {
                dtScope = objDalCalls.GetScopeDetails(Status);

                if(dtScope.Rows.Count > 0)
                {
                    
                    DataView view = new DataView(dtScope);

                    DataTable distinctValues = view.ToTable(true, "Scope");

                    for (int i=0; i < distinctValues.Rows.Count;i++)
                    {
                        Result = objDalCalls.GetCompetancy(Name, distinctValues.Rows[i]["Scope"].ToString());
                        if(Result=="0")
                        {
                            blnCompetant = false;
                            break;
                        }
                        else if (Result == "1")
                        {
                            blnCompetant = true;
                        }
                    }                    
                }

                if(blnCompetant)
                {
                    strResult = "YES";
                }
                else
                {
                    strResult = "NO";
                }

                

                return Json(strResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public ActionResult ExpeditingCallListByInspector(CallsModel CM)
        {
            Session["PK_Call_ID"] = null;
            DataTable CompanyDashBoard = new DataTable();
            List<CallsModel> lstCompanyDashBoard = new List<CallsModel>();
            CompanyDashBoard = objDalCalls.GetExpeditingCallsListByInspector();
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                // ModifyDate = DateTime.ParseExact(dr["ModifyDate"].ToString(), "dd/mm/yyyy", null),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                // CreatedDate = DateTime.ParseExact(dr["CreatedDate"].ToString(),"dd/mm/yyyy",null),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Reasion = Convert.ToString(dr["Reason"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                SubSubJobNo = Convert.ToString(dr["Sub_job"]),
                                Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                PO_Number = Convert.ToString(dr["Po_Number"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                SubVendorPONo = Convert.ToString(dr["SubVendorPoNo"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCompanyDashBoard;

            ObjModelsubJob.lst1 = lstCompanyDashBoard;

            return View(ObjModelsubJob);
        }





    }
}
 