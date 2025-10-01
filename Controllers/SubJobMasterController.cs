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
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Helpers;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;

namespace TuvVision.Controllers
{
    public class SubJobMasterController : Controller
    {
        
        DALInspectionVisitReport objDalVisitReport = new DALInspectionVisitReport();
        CommonControl objCommonControl = new CommonControl();
        DALSubJob objDalSubjob = new DALSubJob();
        SubJobs ObjModelsubJob = new SubJobs();
        CallsModel objModelCallModel = new CallsModel();
        DALCalls objDalCalls = new DALCalls();
        InspectionvisitReportModel ObjModelVisitReport = new InspectionvisitReportModel();
        int PK_Call_ID = 0;
        // GET: SubJobMaster
        public ActionResult CreatSubJob(int? PK_JOB_ID, int? PK_SubJob_Id)
        {
            string SubJobStatus = string.Empty;
            string SubSubJobStatus = string.Empty;

            //PK_JOB_ID = 1;

          //Session["CallLimit"] = null;



            if (PK_JOB_ID > 0)
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DataTable DSEditQutationTabledata = new DataTable();
                // DataTable dtedit = objDalSubjob.getDatajob(PK_JOB_ID);
                DSEditQutationTabledata = objDalSubjob.GetJobDataById(PK_JOB_ID);

                // DSJobMasterByQtId = objDalSubjob.CheckQutationdata(PK_JOB_ID);

                if (DSEditQutationTabledata.Rows.Count > 0)
                {
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditQutationTabledata.Rows[0]["PK_QT_ID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditQutationTabledata.Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditQutationTabledata.Rows[0]["Description"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditQutationTabledata.Rows[0]["Client_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditQutationTabledata.Rows[0]["Job_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditQutationTabledata.Rows[0]["Job_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditQutationTabledata.Rows[0]["Service_type"]);
                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditQutationTabledata.Rows[0]["SAP_No"]);               
					ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditQutationTabledata.Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditQutationTabledata.Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditQutationTabledata.Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.CostsheetApproval = Convert.ToString(DSEditQutationTabledata.Rows[0]["CostsheetApproval"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditQutationTabledata.Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditQutationTabledata.Rows[0]["DECNumber"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditQutationTabledata.Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.Consume = Convert.ToString(DSEditQutationTabledata.Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditQutationTabledata.Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditQutationTabledata.Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditQutationTabledata.Rows[0]["POMandays"]);
                    //added by shrutika salve 
                    ObjModelsubJob.checkIFExpeditingReport = Convert.ToBoolean(DSEditQutationTabledata.Rows[0]["ReportType"]);
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditQutationTabledata.Rows[0]["ChkIfShareReportVendor"]);
                    //added by shrutika salve 07022024
                    ObjModelsubJob.checkIFConcernDisplay = Convert.ToBoolean(DSEditQutationTabledata.Rows[0]["IfConcernsDisplayOfPDF"]);
                    if (Convert.ToString(DSEditQutationTabledata.Rows[0]["checkIFCustomerSpecificReportNo"]) == "1")
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = false;
                    }
                    if (Convert.ToString(DSEditQutationTabledata.Rows[0]["ItemDescriptionDynamic"]) == "1")
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = true;
                    }
                    else
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = false;
                    }
                    if (Convert.ToString(DSEditQutationTabledata.Rows[0]["checkIFExpeditingReport"]) == "1")
                    {
                        ObjModelsubJob.checkIFExpeditingReport = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFExpeditingReport = false;
                    }
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditQutationTabledata.Rows[0]["Client_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditQutationTabledata.Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditQutationTabledata.Rows[0]["Client_Contact"]);
                }


                // return View(ObjModelsubJob);
            }
            else if (PK_SubJob_Id > 0)
            {
                Session["PK_SubJobID"] = PK_SubJob_Id;
               
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.EditSubJob(PK_SubJob_Id);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);

                    ObjModelsubJob.vendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v1"]);
                    ObjModelsubJob.Vendor_Po_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p1"]);
                    ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d1"]);
                    // ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["ChkIfShareReportVendor"]);
                    //ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);
                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.SubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);
                    //ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["status"]);
                    ObjModelsubJob.VendorAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["VendorAddress"]);
                    ObjModelsubJob.VendorPO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["VendorPO_Amount"]);
					ObjModelsubJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);                    
                    ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.CostsheetApproval = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CostsheetApproval"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.Vendor_ContactAll = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_ContactAll"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.JobBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobBlock"]);
                    //added by shrutika salve 08052024
                    ObjModelsubJob.checkIFConcernDisplay = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["IfConcernsDisplayOfPDF"]);
                    //ObjModelsubJob.CustomerBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CustomerActive"]);
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecificReportNo"]) == "1")
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ItemDescriptionDynamic"]) == "1")
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = true;
                    }
                    else
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFExpeditingReport"]) == "1")
                    {
                        ObjModelsubJob.checkIFExpeditingReport = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFExpeditingReport = false;
                    }


                    var contacts = ObjModelsubJob.Vendor_Contact?.Split(',').Select(c => new SelectListItem
                    {
                        Value = c,
                        Text = c
                    }).ToList() ?? new List<SelectListItem>();

                    ViewBag.Contacts = contacts;



                }
                if (DSEditCompany.Tables[2].Rows.Count > 0)
                {
                    string SubjobNo = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["SubJob_No"]);
                    string OrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["OrderStatus"]);
                    string SubOrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["Sub_Order_Status"]);
                    int No = Regex.Matches(SubjobNo, "/").Count;
                    if (No == 2 && SubOrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else if (No == 1 && OrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else
                    {
                        ObjModelsubJob.Status = Convert.ToString("InComplete");
                    }                    
                }               
                else
                {
                    ObjModelsubJob.Status = Convert.ToString("InComplete");
                }

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalSubjob.EditUploadedFile(PK_SubJob_Id);
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
                    //Session["listUploadedFile"] = lstEditFileDetails;
                    ObjModelsubJob.FileDetails = lstEditFileDetails;
                }

                DataTable DTCallSummary = new DataTable();
                DTCallSummary = objDalSubjob.GetCallSummary(PK_SubJob_Id);
                List<CallsModel> lstCallSummary = new List<CallsModel>();

                if (DTCallSummary.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallSummary.Rows)
                    {
                        lstCallSummary.Add(
                           new CallsModel
                           {

                               OpenCalls = Convert.ToString(dr["OpenCalls"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               AssignedCalls = Convert.ToString(dr["AssignedCalls"]),
                               NotDoneCalls = Convert.ToString(dr["NotDoneCalls"]),
                               CancelledCalls = Convert.ToString(dr["CancelledCalls"]),
                               TotalCalls = Convert.ToString(dr["TotalCalls"]),
                           }
                         );
                    }
                    ViewData["lstCallSummary"] = lstCallSummary;
                    ObjModelsubJob.lstCallSummary = lstCallSummary;
                }

                // return View(ObjModelsubJob);                
            }
            else
            {
                return RedirectToAction("SubJobList");
            }

            #region Job No Binding on Dropdown

            List<SubJobs> lstEnquiryMast = new List<SubJobs>();
            var Data = objDalSubjob.GetSubJobNo();
            // var Data1 = admin.getAllMainImage();
            ViewBag.SubCatlist = new SelectList(Data, "PK_SubJob_Id", "SubJob_No");
            #endregion
			
			#region Calls data Binding
            DataTable CallDashboard = new DataTable();
            List<SubJobs> lstCallDashBoard = new List<SubJobs>();
            CallDashboard = objDalSubjob.GetCallsList(PK_SubJob_Id);
            try
            {
                if (CallDashboard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CallDashboard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new SubJobs
                            {
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
																					  
                                Status = Convert.ToString(dr["Status"]),
																			
																								  
																								
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
                            }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["BranchList"] = lstCallDashBoard;
            ObjModelsubJob.callListDashboard = lstCallDashBoard;
            #endregion
			
            DataTable CompanyDashBoard = new DataTable();
            List<SubJobs> lstCompanyDashBoard = new List<SubJobs>();
            CompanyDashBoard = objDalSubjob.GetSubSubJobList(PK_SubJob_Id);

            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new SubJobs
                            {

                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]), 
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                V3= Convert.ToString(dr["V3"]),
                                P3 = Convert.ToString(dr["P3"]),
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
			/*
            #region Calls data Binding
           
            CompanyDashBoard = objDalSubjob.GetCallsList(PK_SubJob_Id);
            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new SubJobs
                            {
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                //Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                //Urgency = Convert.ToString(dr["Urgency"]),
                                //Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                //Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),  
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),																			  
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),																				
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
			
			
            #endregion
            */

            return View(ObjModelsubJob);
        }

        [HttpPost]
        public ActionResult CreatSubJob(SubJobs JM, HttpPostedFileBase File, HttpPostedFileBase[] Image, FormCollection fc)
        {
            string Result = string.Empty;
            
            string fileName = string.Empty;

            string ProList = string.Join(",", fc["Vendor_Contact"]);
            JM.Vendor_Contact = ProList;



            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listUploadedFile"] as List<FileDetails>;


            int SJID = 0;
            
            
            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();
            
            
            
            

            if (JM.PK_SubJob_Id == 0)
            {

            
                DataTable JobDashBoard = new DataTable();
                 //JobDashBoard = objDalSubjob.GetJobDetailsCountBydata(JM.PK_JOB_ID);
                JobDashBoard = objDalSubjob.GetJobCountBydata(JM.PK_JOB_ID);
                if (JobDashBoard.Rows.Count >= 0)
                {
                    int abc = JobDashBoard.Rows.Count;
                    int data = 1 + abc;
                    JM.SubJob_No = JM.Control_Number + '/' + data;
                }
                else
                {
                    JM.SubJob_No = JM.Control_Number + '/' + 1;
                }
                JM.Type = Convert.ToString("Sub Job");  //Ankussh change
                Result = objDalSubjob.InsertUpdateSubJOb(JM);


                //SJID = Convert.ToInt32(Session["SJIDs"]);
                SJID = Convert.ToInt32(Result);

                if (SJID != null && SJID != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, SJID);
                        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                        
                        Session["listUploadedFile"] = null;
                    }
                }

                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;
                    int PK_SubJob_Id = Convert.ToInt32(Session["GetPkJobId"]);
                    
                    
                    return RedirectToAction("CreatSubJob", new { PK_SubJob_Id = PK_SubJob_Id });                 
                    
                }
            
            }
            else
            {

                SJID = JM.PK_SubJob_Id;
                //if (SJID != null && SJID != 0)
                //{
                //    if (lstFileDtls != null && lstFileDtls.Count > 0)
                //    {
                //        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID);
                //        Session["listSJUploadedFile"] = null;
                //    }
                //}

                if (JM.PK_SubJob_Id != 0 && JM.Type == "SubSub Job" && JM.SubSubJob_No == null)
                {
                    
                    DataTable JobDashBoard = new DataTable();
                    JobDashBoard = objDalSubjob.GetdataBySubJobNo(JM.SubJob_No);
                    if (JobDashBoard.Rows.Count >= 0)
                    {
                        int abc = JobDashBoard.Rows.Count;
                        int data = 1 + abc;
                        JM.SubSubJob_No = JM.SubJob_No + '/' + data;
                    }
                    else
                    {
                        JM.SubSubJob_No = JM.SubJob_No + '/' + 1;
                    }
                    JM.PK_SubJob_Id = 0;
                    Result = objDalSubjob.InsertUpdateSubJOb(JM);
                    SJID = Convert.ToInt32(Result);
                    if (SJID != null && SJID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                            Session["listUploadedFile"] = null;
                        }
                    }

                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateUsers"] = Result;
                        return RedirectToAction("SubJobList");
                    }
                    
                }
                else
                {
                    SJID = JM.PK_SubJob_Id;
                    if (SJID != null && SJID != 0)
                    {
                        if (lstFileDtls != null && lstFileDtls.Count > 0)
                        {
                            objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, SJID);
                            Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                            //Session["listSJUploadedFile"] = null;
                            Session["listUploadedFile"] = null;


                        }
                    }

                    Result = objDalSubjob.InsertUpdateSubJOb(JM);
                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateUsers"] = Result;
                        
                        return RedirectToAction("CreatSubJob", new { PK_SubJob_Id = JM.PK_SubJob_Id });
                    }                    
                }
            }
            return RedirectToAction("SubJobList");

        }

		public ActionResult CreatSubSubJob(string SubType, int? PK_SubJob_Id)
        {
            string SubJobStatus = string.Empty;
            string SubSubJobStatus = string.Empty;




            ////Edit Existing Sub Sub Job

            if (PK_SubJob_Id > 0 && SubType.ToUpper() == "SUBSUB JOB")
            {
                Session["PK_SubJobID"] = PK_SubJob_Id;

                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.EditSubJob(PK_SubJob_Id);

                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["ChkIfShareReportVendor"]);
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);

                    ObjModelsubJob.vendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v2"]);    //SubSub Job
                    ObjModelsubJob.Vendor_Po_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p2"]);  //SubSub Job
                    ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d2"]);// SubSub Job


                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v1"]); //Sub Job
                    ObjModelsubJob.TopPoNo = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p1"]); //Sub Job
                    ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d1"]); //Sub Job

                    // ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);


                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.SubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);
                    //ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["status"]);
                    ObjModelsubJob.VendorAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["VendorAddress"]);
                    ObjModelsubJob.VendorPO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["VendorPO_Amount"]);
                    ObjModelsubJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);
                    ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.Vendor_ContactAll = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Subvendor_contactAllDetails"]);
                    //added by shrutika salve 07022024
                    ObjModelsubJob.checkIFExpeditingReport = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["RepotType"]);

                    ObjModelsubJob.subvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Subvendor_Email"]);
                    ObjModelsubJob.JobBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobBlock"]);
                    ObjModelsubJob.CustomerBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CustomerActive"]);
                    //added by shrutika salve 08052023
                    ObjModelsubJob.checkIFConcernDisplay = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["IfConcernsDisplayOfPDF"]);
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecificReportNo"]) == "1")
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ItemDescriptionDynamic"]) == "1")
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = true;
                    }
                    else
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFExpeditingReport"]) == "1")
                    {
                        ObjModelsubJob.checkIFExpeditingReport = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFExpeditingReport = false;
                    }


                    var contacts = ObjModelsubJob.Vendor_Contact?.Split(',').Select(c => new SelectListItem
                    {
                        Value = c,
                        Text = c
                    }).ToList() ?? new List<SelectListItem>();

                    ViewBag.Contacts = contacts;


                    var subcontacts = ObjModelsubJob.Sub_Vendor_Contact?.Split(',').Select(c => new SelectListItem
                    {
                        Value = c,
                        Text = c
                    }).ToList() ?? new List<SelectListItem>();

                    ViewBag.SubContacts = contacts;



                }

                if (DSEditCompany.Tables[2].Rows.Count > 0)
                {
                    string SubjobNo = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["SubJob_No"]);
                    string OrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["OrderStatus"]);
                    string SubOrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["Sub_Order_Status"]);

                    int No = Regex.Matches(SubjobNo, "/").Count;
                    if (No == 2 && SubOrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else if (No == 1 && OrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else
                    {
                        ObjModelsubJob.Status = Convert.ToString("InComplete");
                    }
                }
                else
                {
                    ObjModelsubJob.Status = Convert.ToString("InComplete");
                }

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalSubjob.EditUploadedFile(PK_SubJob_Id);

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
                    //Session["listUploadedFile"] = lstEditFileDetails;
                    ObjModelsubJob.FileDetails = lstEditFileDetails;
                }

                DataTable DTCallSummary = new DataTable();
                DTCallSummary = objDalSubjob.GetCallSummary(PK_SubJob_Id);
                List<CallsModel> lstCallSummary = new List<CallsModel>();

                if (DTCallSummary.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallSummary.Rows)
                    {
                        lstCallSummary.Add(
                           new CallsModel
                           {

                               OpenCalls = Convert.ToString(dr["OpenCalls"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               AssignedCalls = Convert.ToString(dr["AssignedCalls"]),
                               NotDoneCalls = Convert.ToString(dr["NotDoneCalls"]),
                               CancelledCalls = Convert.ToString(dr["CancelledCalls"]),
                               TotalCalls = Convert.ToString(dr["TotalCalls"]),
                           }
                         );
                    }
                    ViewData["lstCallSummary"] = lstCallSummary;
                    ObjModelsubJob.lstCallSummary = lstCallSummary;
                }

                #region Bind SubSubSub List
                DataTable dtSubSubSubJob = new DataTable();
                List<SubJobs> lstSubSubSubJob = new List<SubJobs>();
                dtSubSubSubJob = objDalSubjob.GetSubSubSubJobListToDisplayOnCreateSubSub(PK_SubJob_Id);

                try
                {
                    if (dtSubSubSubJob.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtSubSubSubJob.Rows)
                        {
                            lstSubSubSubJob.Add(
                                new SubJobs
                                {

                                    PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                    PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                    PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                    Project_Name = Convert.ToString(dr["Project_Name"]),
                                    Company_Name = Convert.ToString(dr["Company_Name"]),
                                    Control_Number = Convert.ToString(dr["Control_Number"]),
                                    Service_type = Convert.ToString(dr["Service_type"]),
                                    vendor_name = Convert.ToString(dr["vendor_name"]),
                                    Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                    Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                    SAP_No = Convert.ToString(dr["SAP_No"]),
                                    SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                    SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                    Status = Convert.ToString(dr["Status"]),
                                    Client_Email = Convert.ToString(dr["Client_Email"]),
                                    Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                    Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                    Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                    Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                    Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                    Attachment = Convert.ToString(dr["Attachment"]),
                                    Type = Convert.ToString(dr["Type"]),
                                    V1 = Convert.ToString(dr["V1"]),
                                    V2 = Convert.ToString(dr["V2"]),
                                    P1 = Convert.ToString(dr["P1"]),
                                    P2 = Convert.ToString(dr["P2"]),
                                    SubSubSubJob_No = Convert.ToString(dr["SubSubSubJob_No"]),
                                    V3 = Convert.ToString(dr["V3"]),
                                    P3 = Convert.ToString(dr["P3"]),
                                }
                                );
                        }
                    }
                }
                catch (Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                ViewData["lstSubSubSubJob"] = lstSubSubSubJob;
                #endregion

            }
            else if (PK_SubJob_Id > 0)
            {
                Session["PK_SubJobID"] = PK_SubJob_Id;

                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.EditSubJob(PK_SubJob_Id);

                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["ChkIfShareReportVendor"]);
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);
                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["vendor_name"]);

                    //ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);
                    //ObjModelsubJob.Vendor_Po_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Po_No"]);
                    //ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);
                    //ObjModelsubJob.VendorPO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["VendorPO_Amount"]);

                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopVendor"]); //Sub Job
                    ObjModelsubJob.TopPoNo = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopPONo"]); //Sub Job
                    ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopPODate"]); //Sub Job



                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.SubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJob_No"]);
                    

                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["status"]);
                    ObjModelsubJob.VendorAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["VendorAddress"]);
                    
                    ObjModelsubJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);
                    ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.subvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Subvendor_Email"]);
                    ObjModelsubJob.subsubvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubvendor_Email"]);

                    //added by shrutika salve 07022024
                    ObjModelsubJob.checkIFExpeditingReport = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["RepotType"]);
                    ObjModelsubJob.JobBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobBlock"]);
                    ObjModelsubJob.CustomerBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["CustomerActive"]);
                    var contacts = ObjModelsubJob.Vendor_Contact?.Split(',').Select(c => new SelectListItem
                    {
                        Value = c,
                        Text = c
                    }).ToList() ?? new List<SelectListItem>();

                    ViewBag.Contacts = contacts;

                }

                if (DSEditCompany.Tables[2].Rows.Count > 0)
                {
                    string SubjobNo = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["SubJob_No"]);
                    string OrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["OrderStatus"]);
                    string SubOrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["Sub_Order_Status"]);

                    int No = Regex.Matches(SubjobNo, "/").Count;
                    if (No == 2 && SubOrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else if (No == 1 && OrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else
                    {
                        ObjModelsubJob.Status = Convert.ToString("InComplete");
                    }
                }
                else
                {
                    ObjModelsubJob.Status = Convert.ToString("InComplete");
                }

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                //DTGetUploadedFile = objDalSubjob.EditUploadedFile(PK_SubJob_Id);

                //if (DTGetUploadedFile.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in DTGetUploadedFile.Rows)
                //    {
                //        lstEditFileDetails.Add(
                //           new FileDetails
                //           {

                //               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                //               FileName = Convert.ToString(dr["FileName"]),
                //               Extension = Convert.ToString(dr["Extenstion"]),
                //               IDS = Convert.ToString(dr["FileID"]),
                //           }
                //         );
                //    }
                //    ViewData["lstEditFileDetails"] = lstEditFileDetails;

                //    ObjModelsubJob.FileDetails = lstEditFileDetails;
                //}
            }
            else
            {
                return RedirectToAction("SubJobList");
            }

            #region Calls data Binding
            DataTable CompanyDashBoard = new DataTable();
            List<SubJobs> lstCompanyDashBoard = new List<SubJobs>();
            CompanyDashBoard = objDalSubjob.GetSubSubCallsList(PK_SubJob_Id);

            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new SubJobs
                            {
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
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

            #endregion

            #region Job No Binding on Dropdown

            List<SubJobs> lstEnquiryMast = new List<SubJobs>();
            var Data = objDalSubjob.GetSubJobNo();
            // var Data1 = admin.getAllMainImage();
            ViewBag.SubCatlist = new SelectList(Data, "PK_SubJob_Id", "SubJob_No");
            #endregion




            return View(ObjModelsubJob);
        }

        [HttpPost]
        public ActionResult CreatSubSubJob(SubJobs JM, HttpPostedFileBase File, HttpPostedFileBase[] Image, FormCollection fc)
        {
            string Result = string.Empty;

            string fileName = string.Empty;

              string ProList = string.Join(",", fc["Sub_Vendor_Contact"]);
            JM.Sub_Vendor_Contact = ProList;


            string ProList1 = string.Join(",", fc["Vendor_Contact"]);
            JM.Vendor_Contact = ProList1;


            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listUploadedFile"] as List<FileDetails>;


            int SJID = 0;


            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();




            //// New sub sub Job No
            if (JM.PK_SubJob_Id > 0 && JM.SubSubJob_No == null)
            {


                DataTable JobDashBoard = new DataTable();

                JobDashBoard = objDalSubjob.GetSubJobCountBydata(JM.PK_SubJob_Id);


                DataTable SubJobNoData = new DataTable();

                SubJobNoData = objDalSubjob.GetSubJobNoDetail(JM.PK_SubJob_Id);

                if (JobDashBoard.Rows.Count > 0)
                {
                    int abc = JobDashBoard.Rows.Count;
                    int data = 1 + abc;
                    JM.SubSubJob_No = SubJobNoData.Rows[0]["SubJob_No"].ToString() + '/' + data;
                }
                else
                {
                    JM.SubSubJob_No = SubJobNoData.Rows[0]["SubJob_No"].ToString() + '/' + 1;
                }
                JM.Type = Convert.ToString("SubSub Job");  //Ankussh change
                Result = objDalSubjob.InsertUpdateSubSubJOb(JM, 1);

                //SJID = Convert.ToInt32(Session["SJIDs"]);
                SJID = Convert.ToInt32(Result);

                if (SJID != null && SJID != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(SJID));
                        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                        Session["listUploadedFile"] = null;
                    }
                }

                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;
                    int PK_SubJob_Id = Convert.ToInt32(Session["GetPkJobId"]);


                    return RedirectToAction("CreatSubSubJob", new { PK_SubJob_Id = PK_SubJob_Id, SubType = JM.Type });

                }

            }
            else
            {

                SJID = JM.PK_SubJob_Id;


                JM.Type = Convert.ToString("SubSub Job");
                SJID = JM.PK_SubJob_Id;
                if (SJID != null && SJID != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(SJID));
                        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                        //Session["listSJUploadedFile"] = null;
                        Session["listUploadedFile"] = null;

                    }
                }

                Result = objDalSubjob.InsertUpdateSubSubJOb(JM, 2);
                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;

                    return RedirectToAction("CreatSubSubJob", new { PK_SubJob_Id = JM.PK_SubJob_Id, SubType = JM.Type });
                }
            }

            return RedirectToAction("SubJobList");

        }			 
        List<SubJobs> lstSubJobDashBoard = new List<SubJobs>();
        WebGrid grid;

        public ActionResult SubJobList()
        {
            Session["CallLimit"] = null;
            DataTable subJobDashBoard = new DataTable();

            subJobDashBoard = objDalSubjob.GetSubJOBList();
            try
            {
                if (subJobDashBoard.Rows.Count > 0)
                {
                    // int abc = subJobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in subJobDashBoard.Rows)
                    {
                        lstSubJobDashBoard.Add(
                            new SubJobs
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["v1"]),
                                Vendor_Po_No = Convert.ToString(dr["p1"]),
                                CreatedBy = Convert.ToString(dr["V2"]),
                                ModifyBy = Convert.ToString(dr["P2"]),
                                V3 = Convert.ToString(dr["V3"]),
                                P3 = Convert.ToString(dr["P3"]),
                                Date_of_Po = Convert.ToString(dr["Date_of_Po"]),  /*.ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),*/
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                StringDate_of_Po = Convert.ToString(dr["Date_of_Po"]),
                                SubSubSubJob_No = Convert.ToString(dr["SubSubSubJob_No"]),
                                #endregion


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["subJobList"] = lstSubJobDashBoard;


            //  lstSubJobDashBoard = ObjModelsubJob.lstSubJobDashBoard;
            ObjModelsubJob.lstSubJobDashBoard1 = lstSubJobDashBoard;

            return View(ObjModelsubJob);
        }

        public ActionResult DeleteSubJobData(int? PK_SubJob_Id)
        {
            int Result = 0;
            try
            {
                Result = objDalSubjob.DeleteSubJob(PK_SubJob_Id);
                if (Result != 0)
                {
                    TempData["DeleteBranch"] = Result;
                    return RedirectToAction("SubJobList");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        public ActionResult DeleteCallsData(int? PK_Call_ID)
        {
            int Result = 0;
            try
            {
                Result = objDalCalls.DeleteCalls(PK_Call_ID);
                if (Result != 0)
                {
                    TempData["DeleteCall"] = Result;
                    return RedirectToAction("SubJobList");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }


        public JsonResult GetCompanyName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<SubJobs> lstAutoComplete = new List<SubJobs>();
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalSubjob.GetCompanyName(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new SubJobs
                           {
                               vendor_name = Convert.ToString(dr["CompanyName"]),
                               Company_Name = Convert.ToString(dr["CompanyNames"]),
                               //PreviousClosing = Convert.ToDecimal(dr["BseCurrprice"]),
                           }
                         );
                    }
                    //Session["CompanyNames"] = Convert.ToString(DTResult.Rows[0]["CompanyNames"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetCompanyAddress(string Prefix)
        //{
        //    DataTable DTResult = new DataTable();
        //    string CompAddress = string.Empty;
        //    if (Prefix != null && Prefix != "")
        //    {
        //        DTResult = objDalSubjob.GetCompanyAddress(Prefix);
        //        if (DTResult.Rows.Count > 0)
        //        {

        //            CompAddress = DTResult.Rows[0]["Address"].ToString();
        //            return Json(CompAddress, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    return Json("", JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetCompanyAddress(string Prefix)
            {
            DataTable DTResult = new DataTable();
            List<SubJobs> lstSiteCompany = new List<SubJobs>();
            string CompAddress = string.Empty;
            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalSubjob.GetSiteCompanyAddress(Prefix);
                if (DTResult.Rows.Count > 0)
                {
                    /// CompAddress = DTResult.Rows[0]["Address"].ToString();
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstSiteCompany.Add(
                           new SubJobs
                           {
                               pkID = Convert.ToInt32(dr["PKId"]),
                               Company_Name = Convert.ToString(dr["Address"]),

                           }
                         );
                    }
                    return Json(lstSiteCompany, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }

        #region  Ivr & IRN
        [HttpGet]
        public ActionResult IVRReportsBySubjob(ReportModel IVR, String Type, string JobId)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            if (Type == "JobMaster")
            {
                //String strJobId = Convert.ToString(JobId);
                CostSheetDashBoard = objDalSubjob.GetReportByJob(JobId);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Conclusion = Convert.ToString(dr["Conclusion"]),
                                Areas_Of_Concerns = Convert.ToString(dr["Areas_Of_Concerns"]),
                                PendingActivities = Convert.ToString(dr["Pending_Activites"]),
                                NCR = Convert.ToString(dr["Pdf"]),
                                ImageReport = Convert.ToString(dr["Image"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                                Inspector = Convert.ToString(dr["CreatedBy"]),
                                AttFileName = Convert.ToString(dr["attFilename"]),
                                refDocument = Convert.ToString(dr["RefDocument"]),
                               detailsDocument = Convert.ToString(dr["DetailsDocument"]),
                            }
                            );
                    }
                }

                #region Conclusion attachament
                //DataTable DTGetUploadedFile = new DataTable();
                //List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                //DTGetUploadedFile = objDalVisitReport.EditConUploadedFile(Convert.ToInt32(ObjModelVisitReport.PK_IVR_ID));
                //if (DTGetUploadedFile.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in DTGetUploadedFile.Rows)
                //    {
                //        lstEditFileDetails.Add(
                //           new FileDetails
                //           {
                //               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                //               FileName = Convert.ToString(dr["FileName"]),
                //               Extension = Convert.ToString(dr["Extenstion"]),
                //               IDS = Convert.ToString(dr["FileID"]),
                //           }
                //         );
                //    }
                //    ViewData["lstEditFileDetails"] = lstEditFileDetails;
                //    ObjModelVisitReport.FileDetails = lstEditFileDetails;
                //}
                #endregion




            }
            #region Sub job & call
            if (IVR.SubJob_No != null)
            {
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.GetSubJobNoByID(IVR.SubJob_No);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ViewBag.ID = null;
                    ViewBag.ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                }
                if (IVR.PK_SubJob_Id != 0 && IVR.PK_SubJob_Id != null)
                {
                    CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(Convert.ToString(IVR.PK_SubJob_Id));
                }
                else
                {
                    CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(Convert.ToString(ViewBag.ID));
                }
                //CostSheetDashBoard = objDalSubjob.GetReportByCall_Id(IVR.SubJob_No);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                Conclusion = Convert.ToString(dr["Conclusion"]),
                                Areas_Of_Concerns = Convert.ToString(dr["Areas_Of_Concerns"]),
                                PendingActivities = Convert.ToString(dr["Pending_Activites"]),
                                NCR = Convert.ToString(dr["Pdf"]),
                                ImageReport = Convert.ToString(dr["Image"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                                Inspector = Convert.ToString(dr["CreatedBy"]),
                                AttFileName = Convert.ToString(dr["attFilename"]),
                            }
                            );
                    }
                }
            }
            #endregion

            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(IVR);
        }

        [HttpGet]
        public ActionResult IRNReportsBySubjob(ReportModel IVR, String Type, string JobId, int? PK_CALL_ID)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();

            #region IRNByJobId 26-03-2021
            if (Type == "JobMaster")
            {
                CostSheetDashBoard = objDalSubjob.GetIRNReportByJobNo(Convert.ToString(JobId));
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportDate = Convert.ToString(dr["DateOfIssue"]),
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                inspectionDate = Convert.ToString(dr["DateOfInspection"]),
                                IVRNo = Convert.ToString(dr["VisitReportName"]),
                                
                                CreatedBy = Convert.ToString(dr["Inspector"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                            }
                            );
                    }
                }
                ViewData["CostSheet"] = lstCompanyDashBoard;
            }




            #endregion

            #region Type= Call or IRN
            if (IVR.SubJob_No != null)
            {
                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.GetSubJobNoByID(IVR.SubJob_No);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ViewBag.IRNID = null;
                    ViewBag.IRNID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                }
                //if (IVR.PK_SubJob_Id != 0)
                if (IVR.PK_SubJob_Id != 0 && IVR.PK_SubJob_Id != null)
                {
                    CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(Convert.ToString(IVR.PK_SubJob_Id));
                }
                else
                {
                    //  CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(IVR.SubJob_No);
                    CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(Convert.ToString(ViewBag.IRNID));
                }
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportDate = Convert.ToString(dr["DateOfIssue"]),
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                inspectionDate = Convert.ToString(dr["DateOfInspection"]),
                                IVRNo = Convert.ToString(dr["VisitReportName"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                                //Report = Convert.ToString(dr["Report"]),
                                //CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                //PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["Inspector"]),
                            }
                            );
                    }
                }
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;

            if (PK_CALL_ID > 0)//Bind from call page
            {
                CostSheetDashBoard = objDalSubjob.GetIRNReportByCallPage(PK_CALL_ID);
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportDate = Convert.ToString(dr["DateOfIssue"]),
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                inspectionDate = Convert.ToString(dr["DateOfInspection"]),
                                IVRNo = Convert.ToString(dr["VisitReportName"]),

                                //Report = Convert.ToString(dr["Report"]),
                                //CraetedDate = Convert.ToString(dr["CraetedDate"]),
                                //PK_RM_ID = Convert.ToInt32(dr["PK_RM_ID"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["Inspector"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                            }
                            );
                    }
                }
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            #endregion


            return View(IVR);
        }
        #endregion




        public ActionResult NCRReportsBySubjob(ReportModel IVR)
        {
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<ReportModel> lstCompanyDashBoard = new List<ReportModel>();
            if (IVR.SubJob_No != null)
            {
                DataSet DSEditCompany = new DataSet();
                //DSEditCompany = objDalSubjob.GetSubJobNoByID(IVR.SubJob_No);
                //if (DSEditCompany.Tables[0].Rows.Count > 0)
                //{
                //    ViewBag.IRNID = null;
                //    ViewBag.IRNID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                //}
                //// CostSheetDashBoard = objDalSubjob.GetNCRreports(IVR.SubJob_No);
                //if (IVR.PK_SubJob_Id != 0 && IVR.PK_SubJob_Id != null)
                //{
                //    CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(Convert.ToString(IVR.PK_SubJob_Id));
                //}
                //else
                //{
                //    //  CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(IVR.SubJob_No);
                //    // CostSheetDashBoard = objDalSubjob.GetIRNReportByCall_Id(ViewBag.IRNID);
                //    CostSheetDashBoard = objDalSubjob.GetNCRreportsByCallPage(Convert.ToString(ViewBag.IRNID));
                //}

                CostSheetDashBoard = objDalSubjob.GetNCRreportsByCallPage(Convert.ToString(IVR.PK_CALL_ID));
                if (CostSheetDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CostSheetDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ReportModel
                            {
                                ReportName = Convert.ToString(dr["ReportName"]),
                                Report = Convert.ToString(dr["Report"]),
                                CraetedDate = Convert.ToString(dr["CreatedDate"]),
                                PK_RM_ID = Convert.ToInt32(dr["Id"]),
                                //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                ImageName = Convert.ToString(dr["ImageName"]),
                            }
                            );
                    }
                }
            }
            ViewData["CostSheet"] = lstCompanyDashBoard;
            return View(IVR);
        }







        public ActionResult ExportToExcel()
        {
            //DataSet DSExportToExcel = new DataSet();
            //DSExportToExcel = objDalExchangeTradeBonds.GetDashBoard();

            DataSet subJobDashBoard = new DataSet();
            subJobDashBoard = objDalSubjob.GetSubJOBListExport();
            DataTable DTExportToExcel = new DataTable();
            #region test
            DataTable subJobDashBoard1 = new DataTable();

            subJobDashBoard1 = objDalSubjob.GetSubJOBList();
            try
            {
                if (subJobDashBoard1.Rows.Count > 0)
                {
                    // int abc = subJobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in subJobDashBoard1.Rows)
                    {
                        lstSubJobDashBoard.Add(
                            new SubJobs
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                #endregion


                            }

                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #endregion

            WebGrid grid = new WebGrid(source: lstSubJobDashBoard, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
            columns: grid.Columns(
                            grid.Column("Service_type", "Service type"),
                            grid.Column("Type", "type")

                            )).ToString();



            //DataTable DTExportToExcel = new DataTable();
            if (subJobDashBoard.Tables[0].Rows.Count > 0)
            {
                //subJobDashBoard.Rows[0]["PK_SubJob_Id"].ToString();// = "Scrip Code";       
                //subJobDashBoard.Rows[0]["PK_QTID"] = "Scrip Code";     
                //subJobDashBoard.Rows[0]["PK_JOB_ID"] = "Scrip Code";      
                //subJobDashBoard.Rows[0]["Project_Name"] = "Scrip Code";

                subJobDashBoard.Tables[0].Columns["PK_SubJob_Id"].ColumnName = "Maturity Date";
                subJobDashBoard.Tables[0].Columns["PK_QTID"].ColumnName = "Coupon Rate";
                //DSExportToExcel.Tables[0].Columns["LTP_Decimal"].ColumnName = "Last Traded Price(Rs.)";
                //DSExportToExcel.Tables[0].Columns["Yield_Decimal"].ColumnName = "Yield(%)";
                //DSExportToExcel.Tables[0].Columns["PostTaxYield_Decimal"].ColumnName = "Post Tax Yield(%)";
                //DSExportToExcel.Tables[0].Columns["NextDate"].ColumnName = "Next Interest Payment Date";
                //DSExportToExcel.Tables[0].Columns["LastDate"].ColumnName = "Last Traded Date";
                //DSExportToExcel.Tables[0].Columns["Sc_Name"].ColumnName = "Bond Name";
                //DSExportToExcel.Tables[0].Columns["CompanyShortName"].ColumnName = "Company Issuing";
                //DSExportToExcel.Tables[0].Columns["FaceValueInRs"].ColumnName = "Face Value (Rs.)";
                DTExportToExcel = subJobDashBoard.Tables[0];
                //ExportToExcelFile(DTExportToExcel);
                ExportToExcelFile(DTExportToExcel);

            }
            return View("ETBDebenturesDashBoard");
        }
        public void ExportToExcelFile(DataTable dt)
        {
            if (dt.Rows.Count >= 0)
            {
                string filename = "ExchangeTradedBonds&Debentures" + "_" + DateTime.Now.ToString("dd MMM yyyy");
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
                Response.ContentType = "application/excel";
                dgGrid.RenderControl(hw);
                Response.Write(tw.ToString());

                Response.End();


                //Response.ClearContent();
                ////give name to excel sheet.  
                //Response.AddHeader("content-disposition", "attachment; filename=UserData.xls");
                ////specify content type  
                //Response.ContentType = "application/excel";
                ////write excel data using this method   
                //Response.Write(gridData);
                //Response.End();
            }
        }

        public void PrintExcel()
        {
            //DataSet subJobDashBoard = new DataSet();
            //subJobDashBoard = objDalSubjob.GetSubJOBListExport();


            //create object to webgrid  
            DataTable subJobDashBoard = new DataTable();

            subJobDashBoard = objDalSubjob.GetSubJOBList();
            try
            {
                if (subJobDashBoard.Rows.Count > 0)
                {
                    // int abc = subJobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in subJobDashBoard.Rows)
                    {
                        lstSubJobDashBoard.Add(
                            new SubJobs
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                Date_of_Po = dr["Date_of_Po"].ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                #endregion


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }






            WebGrid grid = new WebGrid(source: lstSubJobDashBoard, canPage: false, canSort: false);

            string gridData = grid.GetHtml(
                tableStyle: "table table-bordered",

            columns: grid.Columns(

                            grid.Column("Service_type", "Service type"),
                            grid.Column("PK_QTID", "PK_QTID")

                            )).ToString();





            Response.ClearContent();
            //give name to excel sheet.  
            Response.AddHeader("content-disposition", "attachment; filename=UserData.xls");
            //specify content type  
            Response.ContentType = "application/excel";
            //write excel data using this method   
            Response.Write(gridData);
            Response.End();




        }


        #region MVC grid link S

        [HttpGet]
        public ActionResult ExportIndex()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<SubJobs> grid = CreateExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<SubJobs> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 06-09-2023
                var filename = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                return File(package.GetAsByteArray(), "application/unknown", "SubJobList-" + filename + ".xlsx");
            }
        }
        private IGrid<SubJobs> CreateExportableGrid()
        {

            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetPeople());
            IGrid<SubJobs> grid = new Grid<SubJobs>(GetPeople());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            // grid.Query = Request.Query;

            //grid.Columns.Add(model => model.Service_type).Titled("Service type");
            //grid.Columns.Add(model => model.Type).Titled("Job Type");
            //grid.Columns.Add(model => model.Company_Name).Titled("Company Name");
            //grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            //grid.Columns.Add(model => model.vendor_name).Titled("Sub Job Vendor Name");
            //grid.Columns.Add(model => model.CreatedBy).Titled("Sub-Sub Job Vendor Name");
            //if (ObjModelsubJob.Type == "SubSub Job")
            //{
            //    grid.Columns.Add(model => model.SubSubJob_No).Titled("Sub Job Number");
            //}
            //else
            //{
            //    grid.Columns.Add(model => model.SubJob_No).Titled("Sub Job Number");
            //}
            //grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            //grid.Columns.Add(model => model.Vendor_Po_No).Titled("Sub Job PO Number");
            //grid.Columns.Add(model => model.Vendor_Po_No).Titled("Sub-Sub Job PO Number");
            //grid.Columns.Add(model => model.Date_of_Po).Titled("PO Date");

            //added by nikita on 06-09-2023

            if (ObjModelsubJob.Type == "SubSub Job")
            {
                grid.Columns.Add(model => model.SubSubJob_No).Titled("Sub-Job Number");
            }
            else
            {
                grid.Columns.Add(model => model.SubJob_No).Titled("Sub-Job Number");
            }
            grid.Columns.Add(model => model.Service_type).Titled("Service type");
            grid.Columns.Add(model => model.Type).Titled("Job Type");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");

            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.vendor_name).Titled("Vendor Name");
            grid.Columns.Add(model => model.CreatedBy).Titled("Sub Vendor Name");

            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.Vendor_Po_No).Titled("Customer PO No on Vendor");
            grid.Columns.Add(model => model.ModifyBy).Titled("Vendor PO No on Sub Vendor");
            grid.Columns.Add(model => model.Status).Titled("Order Status");


            grid.Pager = new GridPager<SubJobs>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelsubJob.lstSubJobDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<SubJobs> GetPeople()
        {
            DataSet subJobDashBoard = new DataSet();
          //  subJobDashBoard = objDalSubjob.GetSubJOBListExport();
         //   DataTable DTExportToExcel = new DataTable();
            #region test
            DataTable subJobDashBoard1 = new DataTable();

            subJobDashBoard1 = objDalSubjob.GetSubJOBList();
            lstSubJobDashBoard.Clear();
            try
            {
                if (subJobDashBoard1.Rows.Count > 0)
                {
                    // int abc = subJobDashBoard.Rows.Count;
                    // int data = 1 + abc;
                    foreach (DataRow dr in subJobDashBoard1.Rows)
                    {
                        lstSubJobDashBoard.Add(
                            new SubJobs
                            {
                                #region
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                                PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Control_Number = Convert.ToString(dr["Control_Number"]),
                                Service_type = Convert.ToString(dr["Service_type"]),
                                vendor_name = Convert.ToString(dr["vendor_name"]),
                                Vendor_Po_No = Convert.ToString(dr["Vendor_Po_No"]),
                                CreatedBy = Convert.ToString(dr["V2"]),
                                ModifyBy = Convert.ToString(dr["P2"]),
                                Date_of_Po = Convert.ToString(dr["Date_of_Po"]),  /*.ToString() == string.Empty ? string.Empty : dr["Date_of_Po"].ToString(),*/
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                SubJob_No = Convert.ToString(dr["SubJob_No"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Status = Convert.ToString(dr["Status"]),
                                Client_Email = Convert.ToString(dr["Client_Email"]),
                                Vendor_Email = Convert.ToString(dr["Vendor_Email"]),
                                Tuv_Email = Convert.ToString(dr["Tuv_Email"]),
                                Client_Contact = Convert.ToString(dr["Client_Contact"]),
                                Vendor_Contact = Convert.ToString(dr["Vendor_Contact"]),
                                Sub_Vendor_Contact = Convert.ToString(dr["Sub_Vendor_Contact"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                Type = Convert.ToString(dr["Type"]),
                                StringDate_of_Po = Convert.ToString(dr["Date_of_Po"]),
                                #endregion


                            }

                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            #endregion
            ObjModelsubJob.lstSubJobDashBoard1 = lstSubJobDashBoard;
            return ObjModelsubJob.lstSubJobDashBoard1;
        }

        #endregion

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

        public ActionResult GetCallsInfo(int? PK_Call_ID)
        {
            Session["CallLimit"] = null;
            return RedirectToAction("InsertCalls", "CallsMaster", new { PK_Call_ID = PK_Call_ID });
            //return RedirectToAction("CallsMaster", "InsertCalls", new { PK_Call_ID = ObjModelsubJob.PK_Call_ID });
        }

        #region Added by Ankush
        [HttpPost]
        public ActionResult SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Server.MapPath($"/Content/JobDocument/{file.FileName}"));
                }
            }
            return Json(true);
        }
        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            //Adding New Code 7 March 2020
            List<FileDetails> fileDetails = new List<FileDetails>();

            //---Adding end Code
            if (Session["listUploadedFile"] != null)
            {
                fileDetails = Session["listUploadedFile"] as List<FileDetails>;
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
                            //Adding New Code as per new requirement 7 March 2020, Manoj Sharma
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
                            //filePath = Path.Combine(Server.MapPath("~/Files/Documents/"), filePath);
                            var K = "~/Content/JobDocument/" + fileName;

                            IPath = K;//K.TrimStart('~');
                            //files.SaveAs(Server.MapPath(IPath));
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
                Session["listUploadedFile"] = fileDetails;

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
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
                DTGetDeleteFile = objDalSubjob.GetFileExt(id);
                if (DTGetDeleteFile.Rows.Count > 0)
                {
                    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                    fileDetails.FileName = Convert.ToString(DTGetDeleteFile.Rows[0]["FileName"]);
                }
                if (id != null && id != "")
                {
                    Results = objDalSubjob.DeleteUploadedFile(id);
                    var path = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetails.FileName + fileDetails.Extension);
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
        //public void Download(String p, String d)
        //{
        //   // return File(Path.Combine(Server.MapPath("~/Content/JobDocument/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);

        //    DataTable DTDownloadFile = new DataTable();
        //    List<FileDetails> lstEditFileDetails = new List<FileDetails>();
        //    DTDownloadFile = objDalSubjob.GetFileContent(Convert.ToInt32(d));

        //    string fileName = string.Empty;
        //    string contentType = string.Empty;
        //    byte[] bytes = null;

        //    if (DTDownloadFile.Rows.Count > 0)
        //    {
        //        bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
        //    }

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.Charset = "";
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = contentType;
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    Response.BinaryWrite(bytes);
        //    Response.Flush();
        //    Response.End();
        //}


        public FileResult Download(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalVisitReport.GetFileContentRefDoc(Convert.ToInt32(d));

            if (DTDownloadFile.Rows.Count > 0)
            {
                //fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            string path = Server.MapPath("~/Content/" + year + "/" + CurrentMonth + "/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }
        #endregion

        #region Sub SubSubJob
        public ActionResult CreatSubSubSubJob(string SubType, int? PK_SubJob_Id)
        {
            string SubJobStatus = string.Empty;
            string SubSubJobStatus = string.Empty;




            ////Edit Existing Sub Sub Job

            if (PK_SubJob_Id > 0 && SubType.ToUpper() == "SUBSUBSUB JOB")
            {
                Session["PK_SubJobID"] = PK_SubJob_Id;

                DataSet DSEditCompany = new DataSet();
                DSEditCompany = objDalSubjob.EditSubJob(PK_SubJob_Id);

                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["ChkIfShareReportVendor"]);

                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);

                    //1 st
                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v1"]); //Sub Job
                    ObjModelsubJob.TopPoNo = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p1"]); //Sub Job
                    ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d1"]); //Sub Job
                    //2 st
                    ObjModelsubJob.SubSubVendorName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v2"]);
                    ObjModelsubJob.SubSubVendorPO = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p2"]);
                    ObjModelsubJob.SubSubVendorDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d2"]);

                    //3 st

                    ObjModelsubJob.vendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["v3"]);    //SubSub Job
                    ObjModelsubJob.Vendor_Po_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["p3"]);  //SubSub Job
                    ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["d3"]);// SubSub Job

                    // ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);


                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.SubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);
                    //ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Status"]);
                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["status"]);
                    ObjModelsubJob.VendorAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["VendorAddress"]);
                    ObjModelsubJob.VendorPO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["VendorPO_Amount"]);
                    ObjModelsubJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);
                    ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);
                    ObjModelsubJob.SubSubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubSubJob_No"]);


                    ObjModelsubJob.SubSub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSub_Vendor_Contact"]);
                    ObjModelsubJob.subVendor_ContactAll = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubvendor_contactAllDetails"]);
                    ObjModelsubJob.subvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Subvendor_Email"]);
                    ObjModelsubJob.subsubvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubvendor_Email"]);
                    ObjModelsubJob.JobBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobBlock"]);
                    //added by shrutika salve 20052024

                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecificReportNo"]) == "1")
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ItemDescriptionDynamic"]) == "1")
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = true;
                    }
                    else
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFExpeditingReport"]) == "1")
                    {
                        ObjModelsubJob.checkIFExpeditingReport = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFExpeditingReport = false;
                    }

                    var contacts = ObjModelsubJob.SubSub_Vendor_Contact?.Split(',').Select(c => new SelectListItem
                    {
                        Value = c,
                        Text = c
                    }).ToList() ?? new List<SelectListItem>();


                    ViewBag.contacts = contacts;



                }

                if (DSEditCompany.Tables[2].Rows.Count > 0)
                {
                    string SubjobNo = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["SubJob_No"]);
                    string OrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["OrderStatus"]);
                    string SubOrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["Sub_Order_Status"]);

                    int No = Regex.Matches(SubjobNo, "/").Count;
                    if (No == 2 && SubOrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else if (No == 1 && OrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else
                    {
                        ObjModelsubJob.Status = Convert.ToString("InComplete");
                    }
                }
                else
                {
                    ObjModelsubJob.Status = Convert.ToString("InComplete");
                }

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                DTGetUploadedFile = objDalSubjob.EditUploadedFile(PK_SubJob_Id);

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
                    //Session["listUploadedFile"] = lstEditFileDetails;
                    ObjModelsubJob.FileDetails = lstEditFileDetails;
                }

                DataTable DTCallSummary = new DataTable();
                DTCallSummary = objDalSubjob.GetCallSummary(PK_SubJob_Id);
                List<CallsModel> lstCallSummary = new List<CallsModel>();

                if (DTCallSummary.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallSummary.Rows)
                    {
                        lstCallSummary.Add(
                           new CallsModel
                           {

                               OpenCalls = Convert.ToString(dr["OpenCalls"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               AssignedCalls = Convert.ToString(dr["AssignedCalls"]),
                               NotDoneCalls = Convert.ToString(dr["NotDoneCalls"]),
                               CancelledCalls = Convert.ToString(dr["CancelledCalls"]),
                               TotalCalls = Convert.ToString(dr["TotalCalls"]),
                           }
                         );
                    }
                    ViewData["lstCallSummary"] = lstCallSummary;
                    ObjModelsubJob.lstCallSummary = lstCallSummary;
                }



            }
            else if (PK_SubJob_Id > 0)
            {
                Session["PK_SubJobID"] = PK_SubJob_Id;

                DataSet DSEditCompany = new DataSet();
                //DSEditCompany = objDalSubjob.EditSubJob(PK_SubJob_Id);
                DSEditCompany = objDalSubjob.EditSubSubJob(PK_SubJob_Id);
                if (DSEditCompany.Tables[0].Rows.Count > 0)
                {
                    ObjModelsubJob.chkDoNotshareVendor = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["ChkIfShareReportVendor"]);
                    ObjModelsubJob.PK_SubJob_Id = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_SubJob_Id"]);
                    ObjModelsubJob.PK_QTID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_QTID"]);
                    ObjModelsubJob.PK_JOB_ID = Convert.ToInt32(DSEditCompany.Tables[0].Rows[0]["PK_JOB_ID"]);
                    ObjModelsubJob.Project_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Project_Name"]);
                    ObjModelsubJob.Company_Name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Company_Name"]);
                    ObjModelsubJob.Control_Number = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    Session["JobNumber"] = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Control_Number"]);
                    ObjModelsubJob.Service_type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Service_type"]);
                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["vendor_name"]);

                    //ObjModelsubJob.Date_Of_PoDateTime = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_of_Po"]);
                    //ObjModelsubJob.Vendor_Po_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Po_No"]);
                    //ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);
                    //ObjModelsubJob.VendorPO_Amount = Convert.ToDecimal(DSEditCompany.Tables[0].Rows[0]["VendorPO_Amount"]);


                    ////First Level
                    ObjModelsubJob.Pvendor_name = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopVendor"]); //Sub Job
                    ObjModelsubJob.TopPoNo = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopPONo"]); //Sub Job
                    ObjModelsubJob.Date_of_Po = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["TopPODate"]); //Sub Job

                    ////Second Level
                    //ObjModelsubJob.SubSubVendorName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubVendorName"]); 
                    //ObjModelsubJob.SubSubVendorPO = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubVendorPO"]); 
                    //ObjModelsubJob.SubSubVendorDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubVendorDate"]); 
                    ObjModelsubJob.SubSubVendorName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_name"]);
                    ObjModelsubJob.SubSubVendorPO = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Po_No"]);
                    ObjModelsubJob.SubSubVendorDate = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Date_Of_Po"]);


                    ObjModelsubJob.SAP_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SAP_No"]);
                    ObjModelsubJob.SubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJob_No"]);
                    ObjModelsubJob.SubSubJob_No = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubJob_No"]);


                    ObjModelsubJob.Client_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Email"]);
                    ObjModelsubJob.Vendor_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Email"]);
                    ObjModelsubJob.Tuv_Email = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Tuv_Email"]);
                    ObjModelsubJob.Client_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Client_Contact"]);
                    ObjModelsubJob.Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Vendor_Contact"]);
                    ObjModelsubJob.Sub_Vendor_Contact = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Sub_Vendor_Contact"]);
                    ObjModelsubJob.Attachment = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Attachment"]);
                    ObjModelsubJob.Type = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Type"]);
                    ObjModelsubJob.Status = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["status"]);
                    ObjModelsubJob.VendorAddress = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["VendorAddress"]);

                    ObjModelsubJob.Consume = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ConsumeCall"]);
                    ObjModelsubJob.Remaining = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["RemainingCall"]);
                    ObjModelsubJob.ProposedCall = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ProposedCall"]);
                    ObjModelsubJob.POMandays = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["POMandays"]);
                    ObjModelsubJob.chkArc = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["chkARC"]);
                    ObjModelsubJob.End_User = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["EndUser"]);
                    ObjModelsubJob.SubJobProjectName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubJobProjectName"]);
                    ObjModelsubJob.DECName = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECName"]);
                    ObjModelsubJob.DECNumber = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["DECNumber"]);
                    ObjModelsubJob.checkIFCustomerSpecific = Convert.ToBoolean(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecific"]);

                    ObjModelsubJob.subvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["Subvendor_Email"]);
                    ObjModelsubJob.subsubvendorEmailid = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["SubSubvendor_Email"]);
                    ObjModelsubJob.JobBlock = Convert.ToString(DSEditCompany.Tables[0].Rows[0]["JobBlock"]);

                    //added by shrutika salve 20052024

                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFCustomerSpecificReportNo"]) == "1")
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFCustomerSpecificReportNo = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["ItemDescriptionDynamic"]) == "1")
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = true;
                    }
                    else
                    {
                        ObjModelsubJob.ItemDescriptionDynamic = false;
                    }
                    if (Convert.ToString(DSEditCompany.Tables[0].Rows[0]["checkIFExpeditingReport"]) == "1")
                    {
                        ObjModelsubJob.checkIFExpeditingReport = true;
                    }
                    else
                    {
                        ObjModelsubJob.checkIFExpeditingReport = false;
                    }




                }

                if (DSEditCompany.Tables[2].Rows.Count > 0)
                {
                    string SubjobNo = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["SubJob_No"]);
                    string OrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["OrderStatus"]);
                    string SubOrderStatus = Convert.ToString(DSEditCompany.Tables[2].Rows[0]["Sub_Order_Status"]);

                    int No = Regex.Matches(SubjobNo, "/").Count;
                    if (No == 2 && SubOrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else if (No == 1 && OrderStatus == "Complete")
                    {
                        ObjModelsubJob.Status = Convert.ToString("Complete");
                    }
                    else
                    {
                        ObjModelsubJob.Status = Convert.ToString("InComplete");
                    }
                }
                else
                {
                    ObjModelsubJob.Status = Convert.ToString("InComplete");
                }

                DataTable DTGetUploadedFile = new DataTable();
                List<FileDetails> lstEditFileDetails = new List<FileDetails>();
                //DTGetUploadedFile = objDalSubjob.EditUploadedFile(PK_SubJob_Id);

                //if (DTGetUploadedFile.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in DTGetUploadedFile.Rows)
                //    {
                //        lstEditFileDetails.Add(
                //           new FileDetails
                //           {

                //               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                //               FileName = Convert.ToString(dr["FileName"]),
                //               Extension = Convert.ToString(dr["Extenstion"]),
                //               IDS = Convert.ToString(dr["FileID"]),
                //           }
                //         );
                //    }
                //    ViewData["lstEditFileDetails"] = lstEditFileDetails;

                //    ObjModelsubJob.FileDetails = lstEditFileDetails;
                //}





            }
            else
            {
                return RedirectToAction("SubJobList");
            }

            #region Calls data Binding
            DataTable CompanyDashBoard = new DataTable();
            List<SubJobs> lstCompanyDashBoard = new List<SubJobs>();
            CompanyDashBoard = objDalSubjob.GetSubSubSubCallsList(PK_SubJob_Id);

            try
            {
                if (CompanyDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in CompanyDashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new SubJobs
                            {
                                PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Status = Convert.ToString(dr["Status"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                SubSubJob_No = Convert.ToString(dr["SubSubJob_No"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Product_item"]),
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

            #endregion

            #region Job No Binding on Dropdown

            List<SubJobs> lstEnquiryMast = new List<SubJobs>();
            var Data = objDalSubjob.GetSubJobNo();
            // var Data1 = admin.getAllMainImage();
            ViewBag.SubCatlist = new SelectList(Data, "PK_SubJob_Id", "SubJob_No");
            #endregion




            return View(ObjModelsubJob);
        }



        [HttpPost]
        public ActionResult CreatSubSubSubJob(SubJobs JM, HttpPostedFileBase File, HttpPostedFileBase[] Image, FormCollection fc)
        {
            string Result = string.Empty;

            string fileName = string.Empty;

            string ProList2 = string.Join(",", fc["SubSub_Vendor_Contact"]);
            JM.SubSub_Vendor_Contact = ProList2;


            string IPath = string.Empty;
            var list = Session["list"] as List<string>;
            if (list != null && list.Count != 0)
            {
                IPath = string.Join(",", list.ToList());
                IPath = IPath.TrimEnd(',');
            }

            List<FileDetails> lstFileDtls = new List<FileDetails>();
            lstFileDtls = Session["listUploadedFile"] as List<FileDetails>;


            int SJID = 0;


            List<FileDetails> fileDetails = new List<FileDetails>();
            List<string> Selected = new List<string>();




            //// New sub sub sub Job No
            if (JM.PK_SubJob_Id > 0 && JM.SubSubSubJob_No == null)
            {


                DataTable JobDashBoard = new DataTable();

                //JobDashBoard = objDalSubjob.GetSubJobCountBydata(JM.PK_SubJob_Id);
                JobDashBoard = objDalSubjob.GetSubSubJobCountBydata(JM.PK_SubJob_Id);


                DataTable SubJobNoData = new DataTable();

                SubJobNoData = objDalSubjob.GetSubJobNoDetail(JM.PK_SubJob_Id);

                if (JobDashBoard.Rows.Count > 0)
                {
                    int abc = JobDashBoard.Rows.Count;
                    int data = 1 + abc;
                    JM.SubJob_No = SubJobNoData.Rows[0]["SubJob_No"].ToString();
                    JM.SubSubJob_No = SubJobNoData.Rows[0]["SubSubJob_No"].ToString();
                    JM.SubSubSubJob_No = SubJobNoData.Rows[0]["SubSubJob_No"].ToString() + '/' + data;
                }
                else
                {
                    JM.SubJob_No = SubJobNoData.Rows[0]["SubJob_No"].ToString();
                    JM.SubSubJob_No = SubJobNoData.Rows[0]["SubSubJob_No"].ToString();
                    JM.SubSubSubJob_No = SubJobNoData.Rows[0]["SubSubJob_No"].ToString() + '/' + 1;
                }
                JM.Type = Convert.ToString("SubSubSub Job");  //Ankussh change
                //Result = objDalSubjob.InsertUpdateSubSubJOb(JM, 1);
                Result = objDalSubjob.InsertUpdateSubSubSubJOb(JM, 1);

                //SJID = Convert.ToInt32(Session["SJIDs"]);
                SJID = Convert.ToInt32(Result);

                if (SJID != null && SJID != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(SJID));
                        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                        Session["listUploadedFile"] = null;
                    }
                }

                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;
                    int PK_SubJob_Id = Convert.ToInt32(Session["GetPkJobId"]);


                    return RedirectToAction("CreatSubSubSubJob", new { PK_SubJob_Id = PK_SubJob_Id, SubType = JM.Type });

                }

            }
            else
            {

                SJID = JM.PK_SubJob_Id;


                JM.Type = Convert.ToString("SubSubSub Job");
                SJID = JM.PK_SubJob_Id;
                if (SJID != null && SJID != 0)
                {
                    if (lstFileDtls != null && lstFileDtls.Count > 0)
                    {
                        objCommonControl.SaveFileToPhysicalLocation(lstFileDtls, Convert.ToInt32(SJID));
                        Result = objDalSubjob.InsertFileAttachment(lstFileDtls, SJID, PK_Call_ID);
                        //Session["listSJUploadedFile"] = null;
                        Session["listUploadedFile"] = null;

                    }
                }

                Result = objDalSubjob.InsertUpdateSubSubJOb(JM, 2);
                if (Result != "" && Result != null)
                {
                    TempData["UpdateUsers"] = Result;

                    return RedirectToAction("CreatSubSubSubJob", new { PK_SubJob_Id = SJID, SubType = JM.Type });
                }
            }

            return RedirectToAction("SubJobList");

        }



        #endregion


        public JsonResult Getvendordetails(string Pre)
        {
            DataTable DT = new DataTable();
            List<SubJobs> lstSite = new List<SubJobs>();
            string CompAddress = string.Empty;
            if (Pre != null && Pre != "")
            {
                DT = objDalSubjob.GetVendorDetails(Pre);
                if (DT.Rows.Count > 0)
                {
                    /// CompAddress = DTResult.Rows[0]["Address"].ToString();
                    foreach (DataRow dr in DT.Rows)
                    {
                        lstSite.Add(
                           new SubJobs
                           {
                               Vendor_Contact = Convert.ToString(dr["PK_ContID"]),
                               vendor_name = Convert.ToString(dr["ContactName"]),

                           }
                         );
                    }
                    return Json(lstSite, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }

       
        public JsonResult GetClientdetails(string Pre)   //added by nikita on 10112024
        {
            DataTable DT = new DataTable();
            List<SubJobs> lstSite = new List<SubJobs>();
            string CompAddress = string.Empty;
            if (Pre != null && Pre != "")
            {
                DT = objDalSubjob.GetInspectorList(Pre);
                if (DT.Rows.Count > 0)
                {
                    /// CompAddress = DTResult.Rows[0]["Address"].ToString();
                    foreach (DataRow dr in DT.Rows)
                    {
                        lstSite.Add(
                           new SubJobs
                           {
                               Vendor_Contact = Convert.ToString(dr["PK_ContID"]),
                               vendor_name = Convert.ToString(dr["ContactName"]),

                           }
                         );
                    }
                    return Json(lstSite, JsonRequestBehavior.AllowGet);
                }
            }

            return Json("failure", JsonRequestBehavior.AllowGet);

        }



        public JsonResult GetvendorEmailId(string myArray, string vendorComapny)
        {
            var jsonString = myArray;

            // Parse the JSON string to extract the array of IDs
            var parsedData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);

            // Extract the array of vendor IDs
            var vendorIds = parsedData["vendorid"];

            DataTable DTEmail = new DataTable();
            List<SubJobs> lstEmail = new List<SubJobs>();

            // Process each ID in the array
            foreach (var vendorId in vendorIds)
            {
                if (!string.IsNullOrEmpty(vendorId))
                {
                    DTEmail = objDalSubjob.GetVendorEmailId(vendorId, vendorComapny);
                    if (DTEmail.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTEmail.Rows)
                        {
                            lstEmail.Add(new SubJobs
                            {
                                pkID = Convert.ToInt32(dr["PK_ContID"]),
                                Vendor_Email = Convert.ToString(dr["Email"]),
                            });
                        }
                    }
                }
            }

            // Return the result as JSON
            if (lstEmail.Count > 0)
            {
                return Json(lstEmail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult GetvendorDetailsall(string myArray, string vendorComapny)
        {
            var jsonString = myArray;

            // Parse the JSON string to extract the array of IDs
            var parsedData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);

            // Extract the array of vendor IDs
            var vendorIds = parsedData["vendorid"];

            DataTable DTvendorDetails = new DataTable();
            List<SubJobs> lstVendorDetails = new List<SubJobs>();

            // Process each ID in the array
            foreach (var vendorId in vendorIds)
            {
                if (!string.IsNullOrEmpty(vendorId))
                {
                    DTvendorDetails = objDalSubjob.GetVendorDetails(vendorId, vendorComapny);
                    if (DTvendorDetails.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTvendorDetails.Rows)
                        {
                            lstVendorDetails.Add(new SubJobs
                            {
                                //pkID = Convert.ToInt32(dr["PK_ContID"]),
                                ContactNames = Convert.ToString(dr["VendorAllDetails"]),
                            });
                        }
                    }
                }
            }

            // Return the result as JSON
            if (lstVendorDetails.Count > 0)
            {
                return Json(lstVendorDetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("failure", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult CreatSubJobExceldata()
        {
            try
            {
                // Read DArray from the request
                var DArrayString = Request.Form["DArray"];
                var DArray = JsonConvert.DeserializeObject<List<SubJobs>>(DArrayString);


                var duplicatePoNumbers = DArray.GroupBy(x => x.Vendor_Po_No)
                               .Where(group => group.Count() > 1)
                               .Select(group => group.Key)
                               .ToList();



                if (duplicatePoNumbers.Any())
                {

                    string errorMessage = "Duplicate Vendor_Po_No values found: " + string.Join(", ", duplicatePoNumbers);

                    return Json(new { success = false, message = errorMessage });
                }
                else
                {
                    foreach (var item in DArray)
                    {
                        string Result1 = "";
                        DALJob objDalCompany = new DALJob();
                        Result1 = objDalCompany.CheckData(item.PK_JOB_ID, item.Vendor_Po_No);
                        // Insert or update SubJob and capture the result
                        if (Result1 == "1")
                        {
                            string errorMessage = "Duplicate Vendor_Po_No values found in system:";
                            // Handle the error, maybe return an error response or throw an exception
                            return Json(new { success = false, message = errorMessage });
                        }

                        DataTable DtData = new DataTable();


                        item.Vendor_Contact = string.Join(",", item.Vendor_ContactDetails);
                        item.Vendor_ContactAll = string.Join(",", item.Vendor_ContactAll);

                        System.Data.DataSet ds = new System.Data.DataSet();
                        int PK_QIId = 0;
                        ds = objDalCompany.VendorDetais(Convert.ToString(item.PK_JOB_ID));
                        DtData = ds.Tables[0];
                        int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out PK_QIId);

                        item.PK_QTID = PK_QIId;


                        DataTable JobDashBoard = objDalSubjob.GetJobCountBydata(item.PK_JOB_ID);
                        if (JobDashBoard.Rows.Count >= 0)
                        {
                            int abc = JobDashBoard.Rows.Count;
                            int data = 1 + abc;
                            item.SubJob_No = item.Control_Number + '/' + data;
                        }
                        else
                        {
                            item.SubJob_No = item.Control_Number + '/' + 1;
                        }
                        item.Type = "Sub Job";
                        item.Status = "InComplete";

                        string Result = objDalSubjob.InsertUpdateSubJOb(item);
                        int SJID = Convert.ToInt32(Result); // Save SJID obtained from the SubJob insertion/update

                        if (Request.Files.Count > 0)
                        {

                            foreach (string fileName in Request.Files)
                            {
                                List<FileDetails> lstFile = new List<FileDetails>();
                                string rowValue = string.Empty;
                                string[] parts = fileName.Split('_');
                                string rowName = parts[0];
                                Match match = Regex.Match(fileName, @"^row_(\d+)");
                                if (match.Success)
                                {
                                    rowValue = match.Value;
                                }


                                if (rowValue == item.Rowid)
                                {

                                    var file = Request.Files[fileName];
                                    if (file != null && file.ContentLength > 0)
                                    {
                                        // Saving the file
                                        var fileDetail = new FileDetails
                                        {
                                            FileName = file.FileName,
                                            FileSize = file.ContentType,

                                            //FileContent = file.ContentLength,

                                        };

                                        // Save file detail to list
                                        lstFile.Add(fileDetail);

                                        // Save the file to physical location
                                        string CurrentMonth = DateTime.Now.ToString("MMM");
                                        string CurrentYear = DateTime.Now.Year.ToString();
                                        string pathYear = "~/Content/" + CurrentYear;
                                        string pathMonth = "~/Content/" + CurrentMonth;
                                        string FinalPath = "~/Content/" + CurrentYear + '/' + CurrentMonth;

                                        // Ensure both year and month directories are created if they do not exist
                                        string serverPathYear = Server.MapPath(pathYear);
                                        string serverPathFinal = Server.MapPath(FinalPath);

                                        if (!Directory.Exists(serverPathYear))
                                        {
                                            Directory.CreateDirectory(serverPathYear);
                                        }

                                        if (!Directory.Exists(serverPathFinal))
                                        {
                                            Directory.CreateDirectory(serverPathFinal);
                                        }

                                        // Combine the final path and save the file
                                        var savePath = Path.Combine(serverPathFinal, SJID + "_" + file.FileName);
                                        file.SaveAs(savePath);

                                        Result = objDalSubjob.InsertFileAttachment(lstFile, SJID, PK_Call_ID);
                                    }

                                }
                                else
                                {

                                }
                            }
                        }


                    }

                    // Handle success response
                    return Json(new { success = true, message = "Data saved successfully" });
                }

                // return Json(new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return Json(new { success = false, message = ex.Message });
            }
        }


        //added by nikita on 27062024
        public ActionResult Validatecall(int PK_SubJob_Id)
        {
            string result;

            try
            {
                DataTable dt = new DataTable();
                dt = objDalSubjob.Validatecall_(PK_SubJob_Id);
                result = JsonConvert.SerializeObject(dt);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetListOfIssue(int PK_SubJob_Id, string Type, InspectionvisitReportModel objModel)
        {
            #region  All Previous Concerns
            objModel.Type = Type;
            DataSet AllPreviousConcern = new DataSet();
            List<InspectionvisitReportModel> lstAllConvern = new List<InspectionvisitReportModel>();
            AllPreviousConcern = objDalVisitReport.AllPreviousConcern_Subjob(PK_SubJob_Id);



            if (AllPreviousConcern.Tables.Count > 0)
            {
                if (AllPreviousConcern.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AllPreviousConcern.Tables[0].Rows)
                    {
                        lstAllConvern.Add(
                            new InspectionvisitReportModel
                            {
                                #region
                                Areas_Of_Concerns = Convert.ToString(dr["AreasOfConcern"]),
                                //Pending_Activites = Convert.ToString(dr["Pending_Activites"]),
                                //Non_Conformities_raised = Convert.ToString(dr["Non_Conformities_raised"]),
                                //Date_Of_Inspection = Convert.ToString(dr["Date_Of_Inspection"]),
                                Report_No = Convert.ToString(dr["ReportNo"]),
                                Name = Convert.ToString(dr["Name"]),
                                PkId = Convert.ToString(dr["PkId"]),
                                Type = Convert.ToString(dr["Type"]),
                                ReopenBy = Convert.ToString(dr["ReopenBy"]),
                                Reason = Convert.ToString(dr["Reason"]),
                                PK_IVR_ID = Convert.ToInt32(dr["PK_IVR_ID"]),
                                #endregion


                            }
                            );
                    }

                }
            }

            ViewData["AllConcerns"] = lstAllConvern;
            ViewBag.AllConcerns = lstAllConvern;

            #endregion

            DataTable dt = new DataTable();
            //dt = objDalVisitReport.GetPOItemNo(PK_Call_ID);
            //objModel.PODescription = dt.Rows[0]["POItem_Des"].ToString();
            //objModel.strIsIssuePending = dt.Rows[0]["IsIssuePending"].ToString();
            //if (objModel.strIsIssuePending == "1")
            //{ objModel.AreasOfConcern = true; }
            //else
            //{ objModel.AreasOfConcern = false; }
            #region All Closed Concerns
            DataSet AllPreviousCloseConcern = new DataSet();
            List<InspectionvisitReportModel> lstAllCloseConvern = new List<InspectionvisitReportModel>();
            AllPreviousCloseConcern = objDalVisitReport.AllPreviousCloseConcern_Subjob(PK_SubJob_Id);
            if (AllPreviousCloseConcern.Tables.Count > 0)
            {
                if (AllPreviousCloseConcern.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AllPreviousCloseConcern.Tables[0].Rows)
                    {
                        lstAllCloseConvern.Add(
                            new InspectionvisitReportModel
                            {
                                #region
                                Areas_Of_Concerns = Convert.ToString(dr["AreasOfConcern"]),
                                Report_No = Convert.ToString(dr["ReportNo"]),
                                CreatedBy = Convert.ToString(dr["Name"]),
                                Name = Convert.ToString(dr["CloseBy"]),
                                PkId = Convert.ToString(dr["PkId"]),
                                Type = Convert.ToString(dr["Type"]),
                                Reason = Convert.ToString(dr["Reason"]),
                                Mitigateddate = Convert.ToString(dr["mitigateddate"])
                                #endregion


                            }
                            );
                    }

                }
            }

            ViewData["AllCloseConcerns"] = lstAllCloseConvern;

            #endregion
            DataSet AllPreviousHistory = new DataSet();
            List<InspectionvisitReportModel> lstPreviousHistory_ = new List<InspectionvisitReportModel>();
            AllPreviousHistory = objDalVisitReport.GetIVRdata(PK_Call_ID);
            if (AllPreviousHistory.Tables.Count > 0)
            {
                if (AllPreviousHistory.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in AllPreviousHistory.Tables[0].Rows)
                    {

                        lstPreviousHistory_.Add(
                        new InspectionvisitReportModel
                        {
                            #region
                            Date_Of_Inspection = Convert.ToString(dr["inspectionDate"]),
                            ReportName = Convert.ToString(dr["Reportname"]),
                            PODescription = Convert.ToString(dr["POItem_Des"]),
                            Name = Convert.ToString(dr["Inspectorname"]),
                            POItems = Convert.ToString(dr["Po_Item_No"]),
                            #endregion


                        }
                        );
                    }
                }
            }
            ViewData["AllHistoryIVR"] = lstPreviousHistory_;

            #region NCR



            #endregion


            return View(objModel);
        }

        [HttpGet]
        public ActionResult ExportIndex1(int? PK_SubJob_Id)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<InspectionvisitReportModel> grid = CreateExportableGrid1(PK_SubJob_Id);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<InspectionvisitReportModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<InspectionvisitReportModel> CreateExportableGrid1(int? PK_SubJob_Id)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<InspectionvisitReportModel> grid = new Grid<InspectionvisitReportModel>(GetData1(PK_SubJob_Id));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.Type).Titled("Type");
            grid.Columns.Add(model => model.Areas_Of_Concerns).Titled("Areas_Of_Concerns");
            grid.Columns.Add(model => model.Name).Titled("Name");
            grid.Columns.Add(model => model.ReopenBy).Titled("ReopenBy");
            grid.Columns.Add(model => model.Report_No).Titled("ReportNo");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.Reason).Titled("Mitigrated By");
            grid.Columns.Add(model => model.ClosedBy).Titled("Closed By");


            grid.Pager = new GridPager<InspectionvisitReportModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelVisitReport.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<InspectionvisitReportModel> GetData1(int? PK_SubJob_Id)
        {
            Session["PK_Call_ID"] = null;
            DataTable CostSheetDashBoard = new DataTable();
            DataSet ItemDescriptionData = new DataSet();
            List<InspectionvisitReportModel> lstCompanyDashBoard = new List<InspectionvisitReportModel>();







            CostSheetDashBoard = objDalVisitReport.GetDataForExportToExcelAllConcern_Subjob(PK_SubJob_Id);
            if (CostSheetDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in CostSheetDashBoard.Rows)
                {
                    lstCompanyDashBoard.Add(
                        new InspectionvisitReportModel
                        {

                            Type = Convert.ToString(dr["Type"]),
                            Areas_Of_Concerns = Convert.ToString(dr["AreasOfConcern"]),
                            Name = Convert.ToString(dr["Name"]),
                            ReopenBy = Convert.ToString(dr["ReopenBy"]),
                            Report_No = Convert.ToString(dr["ReportNo"]),
                            Status = Convert.ToString(dr["Status"]),
                            Reason = Convert.ToString(dr["Reason"]),
                            ClosedBy = Convert.ToString(dr["ClosedbyName"]),

                        }
                        );
                }
            }







            ViewData["CostSheet"] = lstCompanyDashBoard;
            ViewBag.ExportToExcel = lstCompanyDashBoard;
            // IVR.lst1 = lstCompanyDashBoard;

            ObjModelVisitReport.lst1 = lstCompanyDashBoard;
            return ObjModelVisitReport.lst1;
        }




    }
}