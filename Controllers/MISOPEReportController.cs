
using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.DataAccessLayer;
using TuvVision.Models;


namespace TuvVision.Controllers
{
    public class MISOPEReportController : Controller
    {
        DALMISOPEReport objDMOR = new DALMISOPEReport();
        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();
        DALCustomerFeedback OBJCust = new DALCustomerFeedback();
        CustomerFeedback obj = new CustomerFeedback();
        MISOPEReport objMOR = new MISOPEReport();
        MISJobWiseTimeSheet objMISJWTS = new MISJobWiseTimeSheet();
        NonInspectionActivity objModel = new NonInspectionActivity();
        ProfileUser objProfile = new ProfileUser();
        //added by shrutika salve 23/06/2023
        CallsModel obj1 = new CallsModel();
        DalCallHistory Dalobj = new DalCallHistory();
        TravelExpense Expense = new TravelExpense();  //added by nikita on 09102023
       //added by shrutika salve 12102023
        ObswiseUtilisationSummary OBS = new ObswiseUtilisationSummary();
        DalObswiseutilisationSummary Dalobs = new DalObswiseutilisationSummary();

        DALQuotationMaster objDALQuotationMast = new DALQuotationMaster();
        QuotationMaster ObjModelQuotationMast = new QuotationMaster();

        DALJob objJob = new DALJob();
        // GET: MISOPEReport
        public ActionResult MISOPEReport()
        {
            Session["GetExcelData"] = "Yes";
            if (Session["FromDate"] != null)
            {
                objMOR.FromDate = Convert.ToString(Session["FromDate"]);
            }
            if (Session["ToDate"] != null)
            {
                objMOR.ToDate = Convert.ToString(Session["ToDate"]);
            }
            string Month = "", Year = "";
            if (Session["Month"] != null && Session["Year"] != null)
            {
                Month = Convert.ToString(Session["Month"]);
                Year = Convert.ToString(Session["Year"]);
            }

            int intTotalTime = 0;
            int intOPERate = 0;
            int objTotalTime = 0;
            int objOPERate = 0;
            int objManday = 0;
            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            //ds = objDMOR.GetData(objMOR); // fill dataset  

            ds = objDMOR.GetData(Month, Year); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    //intTotalTime = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTime"]);
                    //intOPERate = Convert.ToInt32(ds.Tables[0].Rows[0]["OPERate"]);
                    //if (intTotalTime >= 4)
                    //{
                    //    objTotalTime = 4;
                    //    objOPERate = Convert.ToInt32(intOPERate);
                    //    objManday = 1;
                    //}
                    //else
                    //{
                    //    objTotalTime = Convert.ToInt32(intTotalTime);
                    //    objOPERate = intOPERate / 2;
                    //    objManday = Convert.ToInt32(0.5);
                    //}

                    lmd.Add(new MISOPEReport
                    {

                        // Date = Convert.ToString(dr["Date"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Job = Convert.ToString(dr["Job"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        Job_Location = Convert.ToString(dr["Job_Location"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        OPERate = Convert.ToDouble(0),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        trans = Convert.ToString(dr["trans"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"]),
                        //Added By Satish Pawar 06 June 2023
                        OP_Number = Convert.ToString(dr["OP_Number"])

                        /* Date = Convert.ToString(dr["Date"]),

                         InspectorName = Convert.ToString(dr["InspectorName"]),
                         Branch_Name = Convert.ToString(dr["Branch_Name"]),
                         EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                         ActivityType = Convert.ToString(dr["ActivityType"]),
                         Job = Convert.ToString(dr["Job"]),
                         Sub_Job = Convert.ToString(dr["Sub_Job"]),
                         SAP_No = Convert.ToString(dr["SAP_No"]),
                         Project_Name = Convert.ToString(dr["Project_Name"]),
                         Job_Location = Convert.ToString(dr["Job_Location"]),
                         Company_Name = Convert.ToString(dr["Company_Name"]),
                        // StartTime = Convert.ToString(dr["StartTime"]),
                         //EndTime = Convert.ToString(dr["EndTime"]),
                        // TravelTime = Convert.ToString(dr["TravelTime"]),
                         TotalTime = objTotalTime,
                         OPERate = objOPERate,
                         Manday = objManday*/


                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;

            objMOR.FromDate = null;
            objMOR.ToDate = null;

            return View(objMOR);
            //return View();
        }

        //[HttpPost]
        //public ActionResult MISOPEReport(MISOPEReport objMOR)
        [HttpGet]
        public ActionResult MISOPEReport(string Month, string Year)
        {
            //MISOPEReport objMOR =new MISOPEReport();
            Session["GetExcelData"] = null;

            Session["FromDate"] = objMOR.FromDate;
            Session["ToDate"] = objMOR.ToDate;

            int intTotalTime = 0;
            int intOPERate = 0;
            int objTotalTime = 0;
            int objOPERate = 0;
            int objManday = 0;
            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            //Session["Month"] = Month;
            //Session["Year"] = Year;


            //ds = objDMOR.GetData(objMOR); // fill dataset  
            ds = objDMOR.GetData(Month, Year); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                lmd.Clear();

                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    ///   intTotalTime = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTime"]);
                    //  intOPERate = Convert.ToInt32(ds.Tables[0].Rows[0]["OPERate"]);

                    //if (intTotalTime >= 5)
                    //{
                    //    objTotalTime = 4;
                    //    objOPERate = Convert.ToInt32(intOPERate);

                    //    if (objTotalTime > 0)
                    //        objOPERate = intOPERate / 2;
                    //    else
                    //        objOPERate = 0;
                    //}
                    //else
                    //{
                    //    objTotalTime = Convert.ToInt32(intTotalTime);
                    //    if (objTotalTime > 0)
                    //        objOPERate = intOPERate / 2;
                    //    else
                    //        objOPERate = 0;

                    //}

                    lmd.Add(new MISOPEReport
                    {
                        //Date = Convert.ToString(dr["Date"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Job = Convert.ToString(dr["Job"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        Job_Location = Convert.ToString(dr["Job_Location"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        OPERate = 0,//Convert.ToDouble(dr["OPEClaim"]),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        trans = Convert.ToString(dr["trans"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"])

                    });
                }
            }

            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;
            return View(objMOR);

        }


        #region MIS Admin
        public ActionResult MISOPEAdmin()
        {
            Session["GetExcelData"] = "Yes";
            if (Session["Fromdate"] != null && Session["ToDate"] != null)
            {

                objMOR.FromDate = Convert.ToString(Session["Fromdate"]);
                objMOR.ToDate = Convert.ToString(Session["ToDdate"]);
            }

            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            ds = objDMOR.GetOPEAPPData(objMOR); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {


                    lmd.Add(new MISOPEReport
                    {

                        Date = Convert.ToString(dr["Date"]),

                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Job = Convert.ToString(dr["Job"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        Job_Location = Convert.ToString(dr["Job_Location"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        OPERate = Convert.ToDouble(dr["OPEClaim"]),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        //trans = Convert.ToString(dr["trans"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"]),
                        Id = Convert.ToString(dr["Id"]),




                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;

            objMOR.FromDate = null;
            objMOR.ToDate = null;

            return View(objMOR);
            //return View();
        }


        [HttpPost]
        public ActionResult MISOPEAdmin(MISOPEReport objMOR)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = objMOR.FromDate;
            Session["ToDate"] = objMOR.ToDate;


            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            ds = objDMOR.GetOPEAPPData(objMOR); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {


                    lmd.Add(new MISOPEReport
                    {
                        EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        OPERate = Convert.ToDouble(dr["OPERate"]),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"]),
                        Id = Convert.ToString(dr["Id"])



                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;
            return View(objMOR);

        }
        #endregion

        public ActionResult MISOPEInspector(MISOPEReport objMOR, string FKVoucherId)
        {
            Session["GetExcelData"] = null;
            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                objMOR.FromDate = Convert.ToString(Session["FromDate"]);
                objMOR.ToDate = Convert.ToString(Session["ToDate"]);

            }



            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            ds = objDMOR.GetOPEUserData(objMOR, FKVoucherId); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {


                    lmd.Add(new MISOPEReport
                    {
                        Date = Convert.ToString(dr["Date"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Job = Convert.ToString(dr["Job"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        Job_Location = Convert.ToString(dr["Job_Location"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        OPERate = Convert.ToDouble(dr["OPEClaim"]),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        //trans = Convert.ToString(dr["trans"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"])



                    }); 
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;
            Session["FromDate"] = objMOR.FromDate;
            Session["ToDate"] = objMOR.ToDate;
            return View(objMOR);

        }

        public ActionResult JobWiseTimeSheet()
        {
            Session["GetExcelData"] = "Yes";
            int intTotalTime = 0;
            int intOPERate = 0;
            int objTotalTime = 0;
            int objOPEClaim = 0;
            int objManday = 0;

            DataSet dsJobWiseTimeSheet = new DataSet();
            //dsJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet();




            List<MISJobWiseTimeSheet> lJWTS = new List<MISJobWiseTimeSheet>();


            dsJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet();// fill dataset  
            if (dsJobWiseTimeSheet.Tables.Count > 0)
            {
                foreach (DataRow dr in dsJobWiseTimeSheet.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    intTotalTime = Convert.ToInt32(dsJobWiseTimeSheet.Tables[0].Rows[0]["TotalTime"]);

                    if (intTotalTime >= 4)
                    {
                        objTotalTime = 4;
                        objOPEClaim = 275;

                    }
                    else
                    {
                        objTotalTime = Convert.ToInt32(intTotalTime);
                        objOPEClaim = Convert.ToInt32(137.5);

                    }

                    lJWTS.Add(new MISJobWiseTimeSheet
                    {

                        SurveyorName = Convert.ToString(dr["SurveyorName"]),
                        CreatedDateTime = Convert.ToString(dr["CreatedDateTime"]),
                        Call_No = Convert.ToString(dr["Call_No"]),
                        JOBNO = Convert.ToString(dr["JOBNO"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        StartTime = Convert.ToString(dr["StartTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        TotalTime = Convert.ToString(dr["TotalTime"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        Description = Convert.ToString(dr["Description"]),
                        TravelExpense = Convert.ToString(dr["TravelExpense"]),
                        VRNumber = Convert.ToString(dr["VRNumber"]),
                        VisitReport = Convert.ToString(dr["VisitReport"]),
                        VRNo = Convert.ToString(dr["VRNo"]),
                        Attachment = Convert.ToString(dr["Attachment"]),
                        OPEClaim = objOPEClaim,
                        V1 = Convert.ToString(dr["V1"]),
                        V2 = Convert.ToString(dr["V2"]),
                        P1 = Convert.ToString(dr["P1"]),
                        P2 = Convert.ToString(dr["P2"]),
                        ActType = Convert.ToString(dr["ActType"]),
                        VAttachment = Convert.ToString(dr["VAttachment"]),
                        CustomerPO = Convert.ToString(dr["CustomerPO"]),
                        ClientName = Convert.ToString(dr["ClientName"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                        EBranch_Name = Convert.ToString(dr["ExecBranch"]),
                        CallType = Convert.ToString(dr["CallType"]),
                        PK_IVR_ID = Convert.ToString(dr["PK_IVR_ID"]),
                        TUV_Email_ID = Convert.ToString(dr["TUV_Email_ID"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        callstatus = Convert.ToString(dr["Status"]),
                        CoordinatorEmail = Convert.ToString(dr["CoordinatorEmail"]),
                        CoordinatorNo = Convert.ToString(dr["CoordinatorNo"]),
                    });
                }
            }
            ViewData["JobWiseTimeSheet"] = lJWTS;


            objMISJWTS.lstJWTS1 = lJWTS;
            return View(objMISJWTS);



        }

        //Code By Manoj Sharma 17 Dec 2019, Data Search by From Date and To Date
        [HttpPost]
        public ActionResult JobWiseTimeSheet(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;

            int intTotalTime = 0;
            int objTotalTime = 0;
            int objOPEClaim = 0;

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);

            DataSet DSJobWiseTimeSheet = new DataSet();
            List<MISJobWiseTimeSheet> lJWTS = new List<MISJobWiseTimeSheet>();
            DSJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet(FromDate, ToDate);// fill dataset  
            if (DSJobWiseTimeSheet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DSJobWiseTimeSheet.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    intTotalTime = Convert.ToInt32(DSJobWiseTimeSheet.Tables[0].Rows[0]["TotalTime"]);
                    if (intTotalTime >= 4)
                    {
                        objTotalTime = 4;
                        objOPEClaim = 275;
                    }
                    else
                    {
                        objTotalTime = Convert.ToInt32(intTotalTime);
                        objOPEClaim = Convert.ToInt32(137.5);
                    }
                    lJWTS.Add(new MISJobWiseTimeSheet
                    {
                        SurveyorName = Convert.ToString(dr["SurveyorName"]),
                        CreatedDateTime = Convert.ToString(dr["CreatedDateTime"]),
                        Call_No = Convert.ToString(dr["Call_No"]),
                        JOBNO = Convert.ToString(dr["JOBNO"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        StartTime = Convert.ToString(dr["StartTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        TotalTime = Convert.ToString(dr["TotalTime"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        Description = Convert.ToString(dr["Description"]),
                        TravelExpense = Convert.ToString(dr["TravelExpense"]),
                        VRNumber = Convert.ToString(dr["VRNumber"]),
                        VisitReport = Convert.ToString(dr["VisitReport"]),
                        VRNo = Convert.ToString(dr["VRNo"]),
                        Attachment = Convert.ToString(dr["Attachment"]),
                        OPEClaim = objOPEClaim,
                        V1 = Convert.ToString(dr["V1"]),
                        V2 = Convert.ToString(dr["V2"]),
                        P1 = Convert.ToString(dr["P1"]),
                        P2 = Convert.ToString(dr["P2"]),
                        ActType = Convert.ToString(dr["ActType"]),
                        VAttachment = Convert.ToString(dr["VAttachment"]),
                        CustomerPO = Convert.ToString(dr["CustomerPO"]),
                        ClientName = Convert.ToString(dr["ClientName"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                        EBranch_Name = Convert.ToString(dr["ExecBranch"]),
                        CallType = Convert.ToString(dr["CallType"]),
                        PK_IVR_ID = Convert.ToString(dr["PK_IVR_ID"]),
                        TUV_Email_ID = Convert.ToString(dr["TUV_Email_ID"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        callstatus = Convert.ToString(dr["Status"]),
                        CoordinatorEmail = Convert.ToString(dr["CoordinatorEmail"]),
                        CoordinatorNo = Convert.ToString(dr["CoordinatorNo"]),
                    });
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                //return View();
                objMISJWTS.lstJWTS1 = lJWTS;
                return View(objMISJWTS);
            }
            ViewData["JobWiseTimeSheet"] = lJWTS;
            TempData["Result"] = null;
            TempData.Keep();
            objMISJWTS.lstJWTS1 = lJWTS;
            return View(objMISJWTS);
        }


        [HttpGet]
        public ActionResult MISFeedBack()
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTFeedback = new DataTable();
            DTFeedback = OBJCust.GetFeedbackDashBoard();
            List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
            try
            {
                if (DTFeedback.Rows.Count > 0)
                {



                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstFeedback.Add(
                            new CustomerFeedback
                            {
                                FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
                                Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
                                Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
                                Order_No = Convert.ToString(dr["Order_No"]),
                                Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
                                Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
                                Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
                                Quotation_information = Convert.ToString(dr["Quotation_information"]),
                                Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
                                Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
                                Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
                                Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
                                Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
                                Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
                                implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
                                quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
                                efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
                                Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
                                inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
                                Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
                                report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
                                association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
                                Suggestions = Convert.ToString(dr["Suggestions"]),

                                Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
                                Score_percentage = Convert.ToString(dr["Score_percentage"]),
                                Client_Location = Convert.ToString(dr["Client_Location"]),




                                ScoreAchieved = Convert.ToInt32(dr["ScoreAchieved"])
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["FeedBackList"] = lstFeedback;
            obj.lstFeedback1 = lstFeedback;
            return View(obj);
        }

        [HttpPost]
        public ActionResult MISFeedBack(string FromDate, string ToDate, CustomerFeedback c)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = c.FromDate;
            Session["ToDate"] = c.ToDate;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            //IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            //DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            //DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);

            DataTable DTFeedback = new DataTable();
            DTFeedback = OBJCust.GetFeedbackDashBoardByDate(c);
            List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
            try
            {
                if (DTFeedback.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTFeedback.Rows)
                    {
                        lstFeedback.Add(
                            new CustomerFeedback
                            {
                                FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
                                Created_by = Convert.ToString(dr["Created_by"]),
                                Created_Date = Convert.ToString(dr["Created_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
                                Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
                                Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
                                Order_No = Convert.ToString(dr["Order_No"]),
                                Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
                                Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
                                Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
                                Quotation_information = Convert.ToString(dr["Quotation_information"]),
                                Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
                                Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
                                Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
                                Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
                                Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
                                Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
                                implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
                                quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
                                efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
                                Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
                                inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
                                Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
                                report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
                                association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
                                Suggestions = Convert.ToString(dr["Suggestions"]),
                                Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
                                Score_percentage = Convert.ToString(dr["Score_percentage"]),
                                Client_Location = Convert.ToString(dr["Client_Location"]),
                                ScoreAchieved = Convert.ToInt32(dr["ScoreAchieved"])
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["FeedBackList"] = lstFeedback;
            obj.lstFeedback1 = lstFeedback;
            return View(obj);
        }


        [HttpGet]
        public ActionResult MISCallRegister(MISCallRegister objMCR)
        {
            Session["GetExcelData"] = "Yes";
            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.CallRegister();
            List<MISCallRegister> lstFeedback = new List<MISCallRegister>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new MISCallRegister
                            {
                                /*CreatedDate          = Convert.ToString(dr["CreatedDate"]),
                                Call_No              = Convert.ToString(dr["Call_No"]),
                                ClientName           = Convert.ToString(dr["ClientName"]),
                                Project_Name         = Convert.ToString(dr["Project_Name"]),
                                Vendor_Name          = Convert.ToString(dr["Vendor_Name"]),
                                Sub_Job              = Convert.ToString(dr["Sub_Job"]),
                                SAP_No               = Convert.ToString(dr["SAP_No"]),
                                Po_Number            = Convert.ToString(dr["Po_Number"]),
                                Po_No_SSJob          = Convert.ToString(dr["Po_No_SSJob"]),
                                Originating_Branch   = Convert.ToString(dr["OriginatingBranch"]),
                                Executing_Branch     = Convert.ToString(dr["Executing_Branch"]),
                                InspectedItems       = Convert.ToString(dr["InspectedItems"]),
                                Status               = Convert.ToString(dr["Status"]),
                                Call_Recived_date    = Convert.ToString(dr["Call_Recived_date"]),
                                Call_Request_date    = Convert.ToString(dr["Call_Request_date"]),
                                ActualVisitDate      = Convert.ToString(dr["ActualVisitDate"]),
                                Inspector            = Convert.ToString(dr["Inspector"]),
                                Job_Location         = Convert.ToString(dr["Job_Location"]),
                                Product              = Convert.ToString(dr["Product"]),
                                Reason               = Convert.ToString(dr["Reason"]),
                                V1                   = Convert.ToString(dr["V1"]),
                                V2                   = Convert.ToString(dr["V2"]),
                                P1                   = Convert.ToString(dr["P1"]),
                                P2                   = Convert.ToString(dr["P2"]),
                                Type                 = Convert.ToString(dr["CallType"]),
                                delay = Convert.ToInt32(dr["delay"]),*/

                                PK_Call_ID = Convert.ToString(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                Call_Request_date = Convert.ToString(dr["Call_Request_Date"]),

                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lst1 = lstFeedback;
            return View(objMCR);
        }

        [HttpPost]
        public ActionResult MISCallRegister(string FromDate, string ToDate, MISCallRegister objMCR)
        {
            Session["GetExcelData"] = null;
            //objMCR.FromDate = Convert.ToDateTime(Session["FromDate"]);
            //objMCR.ToDate = Convert.ToDateTime(Session["ToDate"]);

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);

            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.CallRegister(FromDate, ToDate);
            List<MISCallRegister> lstFeedback = new List<MISCallRegister>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new MISCallRegister
                            {
                                /* CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                 Call_No = Convert.ToString(dr["Call_No"]),
                                 ClientName = Convert.ToString(dr["ClientName"]),
                                 Project_Name = Convert.ToString(dr["Project_Name"]),
                                 Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                 Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                 SAP_No = Convert.ToString(dr["SAP_No"]),
                                 Po_Number = Convert.ToString(dr["Po_Number"]),
                                 Po_No_SSJob = Convert.ToString(dr["Po_No_SSJob"]),
                                 Originating_Branch = Convert.ToString(dr["OriginatingBranch"]),
                                 Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                 InspectedItems = Convert.ToString(dr["InspectedItems"]),
                                 Status = Convert.ToString(dr["Status"]),
                                 Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                 Call_Request_date = Convert.ToString(dr["Call_Request_date"]),
                                 ActualVisitDate = Convert.ToString(dr["ActualVisitDate"]),
                                 Inspector = Convert.ToString(dr["Inspector"]),
                                 Job_Location = Convert.ToString(dr["Job_Location"]),
                                 Product = Convert.ToString(dr["Product"]),
                                 Reason = Convert.ToString(dr["Reason"]),
                                 V1 = Convert.ToString(dr["V1"]),
                                 V2 = Convert.ToString(dr["V2"]),
                                 P1 = Convert.ToString(dr["P1"]),
                                 P2 = Convert.ToString(dr["P2"]),
                                 Type = Convert.ToString(dr["CallType"]),
                                 delay = Convert.ToInt32(dr["delay"]),
                                 */
                                PK_Call_ID = Convert.ToString(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                Call_Request_date = Convert.ToString(dr["Call_Request_Date"]),

                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            });
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lst1 = lstFeedback;
            TempData["Result"] = null;
            TempData.Keep();
            return View(objMCR);
        }


        [HttpGet]
        public ActionResult ProjectStatus(Projectstatus objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.ProjectStatus(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.ProjectStatus();
            }




            List<Projectstatus> lstFeedback = new List<Projectstatus>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new Projectstatus
                            {
                                Job_Location = Convert.ToString(dr["Inspection Location"]),
                                Job = Convert.ToString(dr["Job"]),
                                Description = Convert.ToString(dr["Project Name"]),
                                call_no = Convert.ToString(dr["call_no"]),
                                Sub_Job = Convert.ToString(dr["sub_job"]),
                                Company_Name = Convert.ToString(dr["Client Name"]),
                                VendorName = Convert.ToString(dr["VendorName"]),
                                VendorPO = Convert.ToString(dr["VendorPoNo"]),
                                Sub_Vendor_Name = Convert.ToString(dr["Sub Vendor Name"]),
                                SubVendorPO = Convert.ToString(dr["Sub Vendor PO"]),
                                ReqNo = Convert.ToString(dr["Requisition No"]),
                                PropDateofInspFrom = Convert.ToString(dr["Proposed Date of Inspection From"]),
                                PropDateofInspTo = Convert.ToString(dr["Proposed Date of Inspection To"]),
                                ActualDateofInpFrom = Convert.ToString(dr["Actual Visit Date of Inspection From"]),
                                ActualDateofInpTo = Convert.ToString(dr["Actual Visit Date of Inspection To"]),
                                NoOfDays = Convert.ToString(dr["No of Days"]),
                                Delay = Convert.ToString(dr["Delay of Inspection"]),
                                Inspector = Convert.ToString(dr["Surveyor Name"]),
                                VisitReport = Convert.ToString(dr["Inspection Report"]),
                                ReleaseNote = Convert.ToString(dr["Release Note"]),
                                TagNo = Convert.ToString(dr["Tag No"]),
                                AreaofConcern = Convert.ToString(dr["Details of Area of Concern/Punch Point"]),
                                NC = Convert.ToString(dr["Non Confirmaties Raised"]),
                                PendingAct = Convert.ToString(dr["Pending Activities"]),
                                IssuesPONumber = Convert.ToString(dr["Issued PO Item Numbers"]),
                                CanIRNIssued = Convert.ToString(dr["Can IRN be Issued"]),
                                NotificationNo = Convert.ToString(dr["Notification No"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                callreceivedDate = Convert.ToString(dr["Notification Received Date"]),
                                AllPONumbers = Convert.ToString(dr["AllPONumbers"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                ProdDescription = Convert.ToString(dr["ProdDescription"]),
                                callType = Convert.ToString(dr["callType"]),
                                status = Convert.ToString(dr["status"]),
                                branch = Convert.ToString(dr["branch"]),
                                OrderType = Convert.ToString(dr["OrderType"]),
                                OrgBranch = Convert.ToString(dr["OrgBranch"]),

                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            return View(objMCR);
        }

        [HttpPost]
        public ActionResult ProjectStatus(string FromDate, string ToDate, Projectstatus objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.ProjectStatus(FromDate, ToDate);
            List<Projectstatus> lstFeedback = new List<Projectstatus>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new Projectstatus
                            {
                                Job_Location = Convert.ToString(dr["Inspection Location"]),
                                Job = Convert.ToString(dr["Job"]),
                                Description = Convert.ToString(dr["Project Name"]),
                                call_no = Convert.ToString(dr["call_no"]),
                                Sub_Job = Convert.ToString(dr["sub_job"]),
                                Company_Name = Convert.ToString(dr["Client Name"]),
                                VendorName = Convert.ToString(dr["VendorName"]),
                                VendorPO = Convert.ToString(dr["VendorPoNo"]),
                                Sub_Vendor_Name = Convert.ToString(dr["Sub Vendor Name"]),
                                SubVendorPO = Convert.ToString(dr["Sub Vendor PO"]),
                                ReqNo = Convert.ToString(dr["Requisition No"]),
                                PropDateofInspFrom = Convert.ToString(dr["Proposed Date of Inspection From"]),
                                PropDateofInspTo = Convert.ToString(dr["Proposed Date of Inspection To"]),
                                ActualDateofInpFrom = Convert.ToString(dr["Actual Visit Date of Inspection From"]),
                                ActualDateofInpTo = Convert.ToString(dr["Actual Visit Date of Inspection To"]),
                                NoOfDays = Convert.ToString(dr["No of Days"]),
                                Delay = Convert.ToString(dr["Delay of Inspection"]),
                                Inspector = Convert.ToString(dr["Surveyor Name"]),
                                VisitReport = Convert.ToString(dr["Inspection Report"]),
                                ReleaseNote = Convert.ToString(dr["Release Note"]),
                                TagNo = Convert.ToString(dr["Tag No"]),
                                AreaofConcern = Convert.ToString(dr["Details of Area of Concern/Punch Point"]),
                                NC = Convert.ToString(dr["Non Confirmaties Raised"]),
                                PendingAct = Convert.ToString(dr["Pending Activities"]),
                                IssuesPONumber = Convert.ToString(dr["Issued PO Item Numbers"]),
                                CanIRNIssued = Convert.ToString(dr["Can IRN be Issued"]),
                                NotificationNo = Convert.ToString(dr["Notification No"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                AllPONumbers = Convert.ToString(dr["AllPONumbers"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                ProdDescription = Convert.ToString(dr["ProdDescription"]),
                                callType = Convert.ToString(dr["callType"]),
                                status = Convert.ToString(dr["status"]),
                                branch = Convert.ToString(dr["branch"]),
                                OrderType = Convert.ToString(dr["OrderType"]),
                                OrgBranch = Convert.ToString(dr["OrgBranch"]),
                            });
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }


        [HttpGet]
        public ActionResult DebitCredit(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.DebitCredit(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.DebitCredit();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                UserName = Convert.ToString(dr["UserName"]),
                                ExecutingBranch = Convert.ToString(dr["ExecutingBranch"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                OnSiteHours = Convert.ToString(dr["OnSiteHours"]),
                                OffSiteHours = Convert.ToString(dr["OffSiteHours"]),
                                TotalChargeble = Convert.ToString(dr["TotalChargeble"]),
                                EmployeeCostcentre = Convert.ToString(dr["EmployeeCostcentre"]),
                                SOCostCenter = Convert.ToString(dr["SOCostCenter"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                OnSiteHourRate = Convert.ToString(dr["OnSiteHourRate"]),
                                AmountDRCR = Convert.ToString(dr["AmountDRCR"]),
                                Description = Convert.ToString(dr["Description"]),
                                CreditBranch = Convert.ToString(dr["CreditBranch"]),
                                DebitBranch = Convert.ToString(dr["DebitBranch"]),

                               // EMaterial = Convert.ToString(dr["EMaterial"]),
                                ESalesOrderNo = Convert.ToString(dr["ESalesOrderNo"]),
                                ESOCode = Convert.ToString(dr["ESOCode"]),
                              //  TMaterial = Convert.ToString(dr["TMaterial"]),
                                TSalesOrderNo = Convert.ToString(dr["TSalesOrderNo"]),
                                TSOCode = Convert.ToString(dr["TSOCode"]),


                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            return View(objMCR);
        }

        [HttpPost]
        public ActionResult DebitCredit(string FromDate, string ToDate, DebitCredit objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.DebitCredit(FromDate, ToDate);
            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                UserName = Convert.ToString(dr["UserName"]),
                                ExecutingBranch = Convert.ToString(dr["ExecutingBranch"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                OnSiteHours = Convert.ToString(dr["OnSiteHours"]),
                                OffSiteHours = Convert.ToString(dr["OffSiteHours"]),
                                TotalChargeble = Convert.ToString(dr["TotalChargeble"]),
                                EmployeeCostcentre = Convert.ToString(dr["EmployeeCostcentre"]),
                                SOCostCenter = Convert.ToString(dr["SOCostCenter"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                OnSiteHourRate = Convert.ToString(dr["OnSiteHourRate"]),
                                AmountDRCR = Convert.ToString(dr["AmountDRCR"]),
                                Description = Convert.ToString(dr["Description"]),
                                CreditBranch = Convert.ToString(dr["CreditBranch"]),
                                DebitBranch = Convert.ToString(dr["DebitBranch"]),
                               // EMaterial = Convert.ToString(dr["EMaterial"]),
                                ESalesOrderNo = Convert.ToString(dr["ESalesOrderNo"]),
                                ESOCode = Convert.ToString(dr["ESOCode"]),
                               // TMaterial = Convert.ToString(dr["TMaterial"]),
                                TSalesOrderNo = Convert.ToString(dr["TSalesOrderNo"]),
                                TSOCode = Convert.ToString(dr["TSOCode"]),
                            });
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }



        [HttpGet]
        public ActionResult DebitCreditSummary(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.DebitCreditSummaryDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.DebitCreditSummary();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Costcentre = Convert.ToString(dr["Costcentre"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                CrAmount = Convert.ToString(dr["CrAmount"]),
                                DrAmount = Convert.ToString(dr["DrAmount"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;
            return View(objMCR);
        }


        [HttpPost]
        public ActionResult DebitCreditSummary(string FromDate, string ToDate, DebitCredit objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.DebitCreditSummaryDate(FromDate, ToDate);
            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Costcentre = Convert.ToString(dr["Costcentre"]),
                                ExecutingBranch = Convert.ToString(dr["Branch"]),
                                UserName = Convert.ToString(dr["CrAmount"]),
                                DrAmount = Convert.ToString(dr["DrAmount"]),

                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }


        [HttpGet]
        public ActionResult VendorMIS(TOYOMIS objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.TOYOMIS(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.TOYOMIS();
            }




            List<TOYOMIS> lstFeedback = new List<TOYOMIS>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new TOYOMIS
                            {
                                VendorName = Convert.ToString(dr["VendorName"]),
                                client = Convert.ToString(dr["client"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                Requisition_No = Convert.ToString(dr["Requisition_No"]),
                                Inspection_Location = Convert.ToString(dr["Inspection_Location"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                PO_Quantity = Convert.ToString(dr["PO_Quantity"]),
                                Ammend_Quantity = Convert.ToString(dr["Ammend_Quantity"]),
                                Offered_Quanity = Convert.ToString(dr["Offered_Quanity"]),
                                Notification_Received_Date = Convert.ToString(dr["Notification_Received_Date"]),
                                Proposed_Date_of_Inspection_From = Convert.ToString(dr["Proposed_Date_of_Inspection_From"]),
                                Proposed_Date_of_Inspection_To = Convert.ToString(dr["Proposed_Date_of_Inspection_To"]),
                                Actual_Visit_Date_of_Inspection_From = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_From"]),
                                Actual_Visit_Date_of_Inspection_To = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_To"]),
                                No_of_Days = Convert.ToString(dr["No_of_Days"]),
                                Delay_of_Inspection = Convert.ToString(dr["Delay_of_Inspection"]),
                                Surveyor_Name = Convert.ToString(dr["Surveyor_Name"]),
                                Inspection_Report = Convert.ToString(dr["Inspection_Report"]),
                                Release_Note = Convert.ToString(dr["Release_Note"]),
                                Final_IRN_Issued_Date = Convert.ToString(dr["Final_IRN_Issued_Date"]),
                                Details_of_Area_of_Concern = Convert.ToString(dr["Details_of_Area_of_Concern"]),
                                IRN_Quantity = Convert.ToString(dr["IRN_Quantity"]),
                                Pending_Activities = Convert.ToString(dr["Pending_Activities"]),
                                // Areas_of_Concers_if_Any = Convert.ToString(dr["Areas_of_Concers_if_Any"]),
                                Updated_Remark = Convert.ToString(dr["Updated_Remark"]),

                                Job = Convert.ToString(dr["job"]),
                                Project = Convert.ToString(dr["project"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                CanIRNbeIssued = Convert.ToString(dr["CanIRNbeIssued"]),
                                InspectionActivity = Convert.ToString(dr["InspectionActivity"]),
                                SubJob = Convert.ToString(dr["Sub_job"]),
                                Report = Convert.ToString(dr["Inspection_Report"]),
                                CreationDate = Convert.ToString(dr["IVR_CreateDate"]),

                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            return View(objMCR);
        }

        [HttpPost]
        public ActionResult VendorMIS(string FromDate, string ToDate, TOYOMIS objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.TOYOMIS(FromDate, ToDate);
            List<TOYOMIS> lstFeedback = new List<TOYOMIS>();

            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new TOYOMIS
                            {
                                VendorName = Convert.ToString(dr["VendorName"]),
                                client = Convert.ToString(dr["client"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                Requisition_No = Convert.ToString(dr["Requisition_No"]),
                                Inspection_Location = Convert.ToString(dr["Inspection_Location"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                PO_Quantity = Convert.ToString(dr["PO_Quantity"]),
                                Ammend_Quantity = Convert.ToString(dr["Ammend_Quantity"]),
                                Offered_Quanity = Convert.ToString(dr["Offered_Quanity"]),
                                Notification_Received_Date = Convert.ToString(dr["Notification_Received_Date"]),
                                Proposed_Date_of_Inspection_From = Convert.ToString(dr["Proposed_Date_of_Inspection_From"]),
                                Proposed_Date_of_Inspection_To = Convert.ToString(dr["Proposed_Date_of_Inspection_To"]),
                                Actual_Visit_Date_of_Inspection_From = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_From"]),
                                Actual_Visit_Date_of_Inspection_To = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_To"]),
                                No_of_Days = Convert.ToString(dr["No_of_Days"]),
                                Delay_of_Inspection = Convert.ToString(dr["Delay_of_Inspection"]),
                                Surveyor_Name = Convert.ToString(dr["Surveyor_Name"]),
                                Inspection_Report = Convert.ToString(dr["Inspection_Report"]),
                                Release_Note = Convert.ToString(dr["Release_Note"]),
                                Final_IRN_Issued_Date = Convert.ToString(dr["Final_IRN_Issued_Date"]),
                                Details_of_Area_of_Concern = Convert.ToString(dr["Details_of_Area_of_Concern"]),
                                IRN_Quantity = Convert.ToString(dr["IRN_Quantity"]),
                                Pending_Activities = Convert.ToString(dr["Pending_Activities"]),
                                //  Areas_of_Concers_if_Any = Convert.ToString(dr["Areas_of_Concers_if_Any"]),
                                Updated_Remark = Convert.ToString(dr["Updated_Remark"]),

                                Job = Convert.ToString(dr["job"]),
                                Project = Convert.ToString(dr["project"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                CanIRNbeIssued = Convert.ToString(dr["CanIRNbeIssued"]),
                                InspectionActivity = Convert.ToString(dr["InspectionActivity"]),
                                SubJob = Convert.ToString(dr["Sub_job"]),
                                Report = Convert.ToString(dr["Inspection_Report"]),
                                CreationDate = Convert.ToString(dr["IVR_CreateDate"]),
                            });
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }



        #region MISFeedBack Export to excel
//12/3/2023 comment on shrutika salve
        //[HttpGet]
        //public ActionResult ExportIndex(CustomerFeedback c)
        //{
        //    // Using EPPlus from nuget
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        Int32 row = 2;
        //        Int32 col = 1;

        //        package.Workbook.Worksheets.Add("Data");
        //        IGrid<CustomerFeedback> grid = CreateExportableGrid(c);
        //        ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //        foreach (IGridColumn column in grid.Columns)
        //        {
        //            sheet.Cells[1, col].Value = column.Title;
        //            sheet.Column(col++).Width = 18;

        //            column.IsEncoded = false;
        //        }

        //        foreach (IGridRow<CustomerFeedback> gridRow in grid.Rows)
        //        {
        //            col = 1;
        //            foreach (IGridColumn column in grid.Columns)
        //                sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

        //            row++;
        //        }

        //        return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //    }
        //}
        //private IGrid<CustomerFeedback> CreateExportableGrid(CustomerFeedback c)
        //{
        //    //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
        //    IGrid<CustomerFeedback> grid = new Grid<CustomerFeedback>(GetData(c));
        //    grid.ViewContext = new ViewContext { HttpContext = HttpContext };


        //    grid.Columns.Add(model => model.Name_Of_organisation).Titled("Inspection Parameters");
        //    grid.Columns.Add(model => model.Order_No).Titled("Feedback against which Order / Project / Item / Vendor / Equipment / P.O./ TUVI control No.?");
        //    grid.Columns.Add(model => model.Client_Location).Titled("Client Location");
        //    grid.Columns.Add(model => model.Enquiry_response).Titled("How well did we respond to your Enquiry? *");
        //    grid.Columns.Add(model => model.Quotation_Time_Frame_feedback).Titled("Did You receive the Quotation within Expected Time Frame? *ENQUIRY HANDLING");
        //    grid.Columns.Add(model => model.Requirement_Understanding).Titled("How well did we understand your requirements? *");
        //    grid.Columns.Add(model => model.Quotation_information).Titled("Did the quotation contain all the required information?");
        //    grid.Columns.Add(model => model.Quotation_Submit_Response).Titled("How quickly were your queries resolved & revised quotation submitted? ");
        //    grid.Columns.Add(model => model.Email_call_ResponseTime).Titled("Was your email / call responded on time?");
        //    grid.Columns.Add(model => model.Requested_Call_schedule).Titled("Was the call scheduled as per your request? *");
        //    grid.Columns.Add(model => model.Confirmation_Reception).Titled("Did you receive information regarding conformation of calls, inspector details in advance? *");
        //    grid.Columns.Add(model => model.Change_in_schedule_Response).Titled("In case of change in schedule were you informed within expected time frame? *");
        //    grid.Columns.Add(model => model.Communication_satisfaction).Titled("How satisfied are you with the communication process with the inspection coordination team? *");
        //    grid.Columns.Add(model => model.Behaiviour_of_Inspector).Titled("How would you rate the behavior of TUV inspector with your staff (w.r.t. language, attitude)? *");
        //    grid.Columns.Add(model => model.implementation_of_safety_requirements_Of_Inspector).Titled("How would you rate the inspector’s safety awareness & leadership in implementation of safety requirements? *");
        //    grid.Columns.Add(model => model.quality_of_inspection).Titled("How would you rate the quality of inspection w.r.t. observations raised? *");
        //    grid.Columns.Add(model => model.efficiency_with_time).Titled("How would you rate the efficiency of our inspector w.r.t time taken for inspection? *");
        //    grid.Columns.Add(model => model.Maintanance_Of_confidentiality_and_Integrity).Titled("How would you rate our inspectors for maintaining confidentiality & Integrity? *");
        //    grid.Columns.Add(model => model.inspection_report_or_Releasenote_Time).Titled("Did you receive the inspection report/Release note in time? *");
        //    grid.Columns.Add(model => model.Expectation_Meet).Titled("Did the report meet your expectations w.r.t. completeness, contents and conclusion *");
        //    grid.Columns.Add(model => model.report_for_number_of_errors).Titled("How would you rate the report for number of errors, revisions? *");
        //    grid.Columns.Add(model => model.association_with_TUV_India).Titled("How likely would you continue your association with TUV India? *");
        //    grid.Columns.Add(model => model.Suggestions).Titled("Would you like to give us any suggestion to improve our service ");
        //    grid.Columns.Add(model => model.ScoreAchieved).Titled("Score Achieved");
        //    grid.Columns.Add(model => model.ScoreAchieved + ".00%").Titled("Score Percentage");


        //    grid.Pager = new GridPager<CustomerFeedback>(grid);
        //    grid.Processors.Add(grid.Pager);
        //    grid.Pager.RowsPerPage = obj.lstFeedback1.Count;

        //    foreach (IGridColumn column in grid.Columns)
        //    {
        //        column.Filter.IsEnabled = true;
        //        column.Sort.IsEnabled = true;
        //    }

        //    return grid;
        //}

        //public List<CustomerFeedback> GetData(CustomerFeedback c)
        //{

        //    DataTable DTFeedback = new DataTable();


        //    if (Session["GetExcelData"] == "Yes")
        //    {
        //        DTFeedback = OBJCust.GetFeedbackDashBoard();
        //    }
        //    else
        //    {

        //        c.FromDate = Session["FromDate"].ToString();
        //        c.ToDate = Session["ToDate"].ToString();
        //        DTFeedback = OBJCust.GetFeedbackDashBoardByDate(c);
        //    }


        //    List<CustomerFeedback> lstFeedback = new List<CustomerFeedback>();
        //    try
        //    {
        //        if (DTFeedback.Rows.Count > 0)
        //        {



        //            foreach (DataRow dr in DTFeedback.Rows)
        //            {
        //                lstFeedback.Add(
        //                    new CustomerFeedback
        //                    {
        //                        Count = DTFeedback.Rows.Count,
        //                        FeedBack_ID = Convert.ToInt32(dr["FeedBack_ID"]),
        //                        Created_by = Convert.ToString(dr["Created_by"]),
        //                        Created_Date = Convert.ToString(dr["Created_Date"]),
        //                        Status = Convert.ToString(dr["Status"]),
        //                        Name_Of_organisation = Convert.ToString(dr["Name_Of_organisation"]),
        //                        Name_Of_Respondent = Convert.ToString(dr["Name_Of_Respondent"]),
        //                        Desighnation_Of_Respondent = Convert.ToString(dr["Desighnation_Of_Respondent"]),
        //                        Order_No = Convert.ToString(dr["Order_No"]),
        //                        Enquiry_response = Convert.ToString(dr["Enquiry_response"]),
        //                        Quotation_Time_Frame_feedback = Convert.ToString(dr["Quotation_Time_Frame_feedback"]),
        //                        Requirement_Understanding = Convert.ToString(dr["Requirement_Understanding"]),
        //                        Quotation_information = Convert.ToString(dr["Quotation_information"]),
        //                        Quotation_Submit_Response = Convert.ToString(dr["Quotation_Submit_Response"]),
        //                        Email_call_ResponseTime = Convert.ToString(dr["Email_call_ResponseTime"]),
        //                        Requested_Call_schedule = Convert.ToString(dr["Requested_Call_schedule"]),
        //                        Confirmation_Reception = Convert.ToString(dr["Confirmation_Reception"]),
        //                        Change_in_schedule_Response = Convert.ToString(dr["Change_in_schedule_Response"]),
        //                        Behaiviour_of_Inspector = Convert.ToString(dr["Behaiviour_of_Inspector"]),
        //                        implementation_of_safety_requirements_Of_Inspector = Convert.ToString(dr["implementation_of_safety_requirements_Of_Inspector"]),
        //                        quality_of_inspection = Convert.ToString(dr["quality_of_inspection"]),
        //                        efficiency_with_time = Convert.ToString(dr["efficiency_with_time"]),
        //                        Maintanance_Of_confidentiality_and_Integrity = Convert.ToString(dr["Maintanance_Of_confidentiality_and_Integrity"]),
        //                        inspection_report_or_Releasenote_Time = Convert.ToString(dr["inspection_report_or_Releasenote_Time"]),
        //                        Expectation_Meet = Convert.ToString(dr["Expectation_Meet"]),
        //                        report_for_number_of_errors = Convert.ToString(dr["report_for_number_of_errors"]),
        //                        association_with_TUV_India = Convert.ToString(dr["association_with_TUV_India"]),
        //                        Suggestions = Convert.ToString(dr["Suggestions"]),

        //                        Score_Achieved = Convert.ToString(dr["Score_Achieved"]),
        //                        Score_percentage = Convert.ToString(dr["Score_percentage"]),
        //                        Client_Location = Convert.ToString(dr["Client_Location"]),




        //                        ScoreAchieved = Convert.ToInt32(dr["ScoreAchieved"])
        //                    });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    ViewData["FeedBackList"] = lstFeedback;
        //    obj.lstFeedback1 = lstFeedback;

        //    return obj.lstFeedback1;
        //}

        #endregion

        #region MIScallRegister export To excel

        [HttpGet]
        public ActionResult ExportIndexMISCallRegister(MISCallRegister objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MISCallRegister> grid = CreateExportableGrid(objMCR);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<MISCallRegister> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<MISCallRegister> CreateExportableGrid(MISCallRegister objMCR)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<MISCallRegister> grid = new Grid<MISCallRegister>(GetData(objMCR));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.CreatedDate).Titled("Call Creation Date");
            //grid.Columns.Add(model => model.Call_No).Titled("Call Number");
            //grid.Columns.Add(model => model.Type).Titled("Call Type");
            //grid.Columns.Add(model => model.ClientName).Titled("Client Name");
            //grid.Columns.Add(model => model.Project_Name).Titled("Project Name");

            //grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job Number");
            //grid.Columns.Add(model => model.V1).Titled("Sub Job Vendor Name");
            //grid.Columns.Add(model => model.V2).Titled("Sub-Sub Job Vendor Name");
            //grid.Columns.Add(model => model.SAP_No).Titled("SAP Number");
            //grid.Columns.Add(model => model.P1).Titled("Sub Job PO No");
            //grid.Columns.Add(model => model.P2).Titled("Sub-Sub Job PO No");
            //grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            //grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            //grid.Columns.Add(model => model.InspectedItems).Titled("Inspected Items");
            //grid.Columns.Add(model => model.Status).Titled("Call Status");
            //grid.Columns.Add(model => "Single Day Call").Titled("Single Day Call/Continuous Call");
            //grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Received Date");
            //grid.Columns.Add(model => model.Call_Request_date).Titled("Inspection Requested Date");
            //grid.Columns.Add(model => model.Product).Titled("Product");
            //grid.Columns.Add(model => model.Reason).Titled("Delayed Reason");
            //grid.Columns.Add(model => model.Job_Location).Titled("Inspection Location");
            //grid.Columns.Add(model => model.Inspector).Titled("Visiting Inspector Name");
            //grid.Columns.Add(model => model.ActualVisitDate).Titled("Actual Visit Date");

            grid.Columns.Add(model => model.Call_No).Titled("Call Number");
            grid.Columns.Add(model => model.Call_Request_date).Titled("Call Requested Date");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Recived Date");
            grid.Columns.Add(model => model.CreatedDate).Titled("Call Created Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Call Created By");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Call_Type).Titled("Call Type");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.V1).Titled("Sub Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Vendor Name");
            grid.Columns.Add(model => model.P1).Titled("Sub Vendor PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Vendor PO No");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.ExtendCall_Status).Titled("Status Call Extend");
            grid.Columns.Add(model => model.Urgency).Titled("Urgency");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");


            grid.Pager = new GridPager<MISCallRegister>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<MISCallRegister> GetData(MISCallRegister objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["GetExcelData"] == "Yes")
            {
                dsCallRegister = objDMOR.CallRegister();
            }
            else
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime FromDt = DateTime.ParseExact(Convert.ToString(Session["FromDate"]), "dd/MM/yyyy", theCultureInfo);
                DateTime ToDt = DateTime.ParseExact(Convert.ToString(Session["ToDate"]), "dd/MM/yyyy", theCultureInfo);



                dsCallRegister = objDMOR.CallRegister(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }


            List<MISCallRegister> lstFeedback = new List<MISCallRegister>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new MISCallRegister
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                //CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                //Call_No = Convert.ToString(dr["Call_No"]),
                                //ClientName = Convert.ToString(dr["ClientName"]),
                                //Project_Name = Convert.ToString(dr["Project_Name"]),
                                //Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                                //Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                //SAP_No = Convert.ToString(dr["SAP_No"]),
                                //Po_Number = Convert.ToString(dr["Po_Number"]),
                                //Po_No_SSJob = Convert.ToString(dr["Po_No_SSJob"]),
                                //Originating_Branch = Convert.ToString(dr["OriginatingBranch"]),
                                //Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                //InspectedItems = Convert.ToString(dr["InspectedItems"]),
                                //Status = Convert.ToString(dr["Status"]),
                                //Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                //Call_Request_date = Convert.ToString(dr["Call_Request_date"]),
                                //ActualVisitDate = Convert.ToString(dr["ActualVisitDate"]),
                                //Inspector = Convert.ToString(dr["Inspector"]),
                                //Job_Location = Convert.ToString(dr["Job_Location"]),
                                //Product = Convert.ToString(dr["Product"]),
                                //Reason = Convert.ToString(dr["Reason"]),
                                //V1 = Convert.ToString(dr["V1"]),
                                //V2 = Convert.ToString(dr["V2"]),
                                //P1 = Convert.ToString(dr["P1"]),
                                //P2 = Convert.ToString(dr["P2"]),
                                //Type = Convert.ToString(dr["CallType"])

                                PK_Call_ID = Convert.ToString(dr["PK_Call_ID"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Contact_Name = Convert.ToString(dr["Contact_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Status = Convert.ToString(dr["Status"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                Call_Recived_date = Convert.ToString(dr["Call_Recived_date"]),
                                Call_Request_date = Convert.ToString(dr["Call_Request_Date"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Project_Name = Convert.ToString(dr["Project_Name"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Po_Number = Convert.ToString(dr["Po_Number"]),
                                Po_No_SSJob = Convert.ToString(dr["Po_No_SSJob"]),

                                Planned_Date = Convert.ToString(dr["Planned_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                V1 = Convert.ToString(dr["V1"]),
                                V2 = Convert.ToString(dr["V2"]),
                                P1 = Convert.ToString(dr["P1"]),
                                P2 = Convert.ToString(dr["P2"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                InspectedItems = Convert.ToString(dr["InspectedItems"]),
                                Product = Convert.ToString(dr["Product"]),
                                Reason = Convert.ToString(dr["Reason"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lst1 = lstFeedback;
            return objMCR.lst1;
        }

        #endregion


        #region export to excel OpeReport
        [HttpGet]
        public ActionResult ExportIndexOpeReport()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 8;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MISOPEReport> grid = CreateExportableGrid();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                sheet.Cells["A2"].Value = "Name";
                sheet.Cells["A3"].Value = "Employee Code";
                sheet.Cells["A4"].Value = "Cost Centre";

                sheet.Cells["B2"].Value = Session["fullName"].ToString();
                sheet.Cells["B3"].Value = Session["EmpCode"].ToString();
                sheet.Cells["B4"].Value = Session["costcentre"].ToString();

                int colcount = 0;
                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[row - 1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                    colcount++;
                }
                //int rowcount = 2;
                double OPETotal = 0;
                foreach (IGridRow<MISOPEReport> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                    {
                        if (gridRow.Model.trans == "1")
                        {
                            sheet.Cells[row, 1, row, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[row, 1, row, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            if (column.Title.ToString().Contains("Total") == false && column.Title.ToString().Contains("Man Day") == false && column.Title.ToString().Contains("OPE Claim") == false)
                            {
                                sheet.Cells[row, col++].Value = string.Empty;
                            }
                            else
                            {
                                if (column.Title.ToString().Contains("Total"))
                                {
                                    sheet.Cells[row, col++].Value = "OPE";
                                }
                                else
                                {
                                    sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                                }
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                        }
                    }
                    if (gridRow.Model.trans == "1")
                    {
                        OPETotal = OPETotal + Convert.ToDouble(gridRow.Model.OPERate);
                    }
                    row++;


                }

                sheet.Cells[row, 1, row, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;

                sheet.Cells[row, 1, row, colcount].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                sheet.Cells[row, 18].Value = OPETotal;



                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
                // Save the document in PDF format

            }
        }

        private IGrid<MISOPEReport> CreateExportableGrid()
        {

            IGrid<MISOPEReport> grid = new Grid<MISOPEReport>(GetData());


            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(model => model.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");

            grid.Columns.Add(model => model.ActivityType).Titled("Activity Type");
            grid.Columns.Add(model => model.Job).Titled("Job");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.Job_Location).Titled("Job Location");
            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");

            grid.Columns.Add(model => model.StartTime).Titled("On Site Hrs");
            grid.Columns.Add(model => model.EndTime).Titled("Off Site Hrs");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.ODTime).Titled("Outdoor Time");
            grid.Columns.Add(model => model.TotalTime).Titled("Total Time");
            grid.Columns.Add(model => model.Manday).Titled("Man Day(1/0.5)");
            grid.Columns.Add(model => model.OPERate).Titled("OPE Claim");



            grid.Pager = new GridPager<MISOPEReport>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMOR.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<MISOPEReport> GetData()
        {

            int intTotalTime = 0;
            int intOPERate = 0;
            int objTotalTime = 0;
            int objOPERate = 0;
            int objManday = 0;
            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                objMOR.FromDate = Convert.ToString(Session["FromDate"]);
                objMOR.ToDate = Convert.ToString(Session["ToDate"]);
            }
            string Month = "", Year = "";
            if (Session["Month"] != null && Session["Year"] != null)
            {
                Month = Convert.ToString(Session["Month"]);
                Year = Convert.ToString(Session["Year"]);
            }
            //if (Session["GetExcelData"] == "Yes")
            //{
            //    ds = objDMOR.GetData(objMOR); // fill dataset  
            //}
            //else
            //{

            //    objMOR.FromDate = Convert.ToString(Session["FromDate"]);
            //    objMOR.ToDate = Convert.ToString(Session["ToDate"]);
            //    ds = objDMOR.GetData(objMOR); // fill dataset  
            //}

            //ds = objDMOR.GetData(objMOR); // fill dataset  
            ds = objDMOR.GetData(Month, Year); // fill dataset  
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    /* intTotalTime = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalTime"]);
                     intOPERate = Convert.ToInt32(ds.Tables[0].Rows[0]["OPERate"]);
                     if (intTotalTime >= 4)
                     {
                         objTotalTime = 4;
                         objOPERate = Convert.ToInt32(intOPERate);
                         objManday = 1;
                     }
                     else
                     {
                         objTotalTime = Convert.ToInt32(intTotalTime);
                         objOPERate = intOPERate / 2;
                         objManday = Convert.ToInt32(0.5);
                     }*/

                    lmd.Add(new MISOPEReport
                    {
                        //Count = ds.Tables[0].Rows.Count,
                        //InspectorName = Convert.ToString(dr["InspectorName"]),
                        //Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        //EmployeeCode = Convert.ToString(dr["EmployeeCode"]),

                        //ActivityType = Convert.ToString(dr["ActivityType"]),
                        //Job = Convert.ToString(dr["Job"]),
                        //Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        //SAP_No = Convert.ToString(dr["SAP_No"]),
                        //Project_Name = Convert.ToString(dr["Project_Name"]),
                        //Job_Location = Convert.ToString(dr["Job_Location"]),
                        //Company_Name = Convert.ToString(dr["Company_Name"]),
                        //StartTime = Convert.ToString(dr["StartTime"]),
                        //EndTime = Convert.ToString(dr["EndTime"]),
                        //TravelTime = Convert.ToString(dr["TravelTime"]),
                        //TotalTime = objTotalTime,
                        //OPERate = objOPERate,
                        //Manday = objManday

                        Count = ds.Tables[0].Rows.Count,
                        Date = Convert.ToString(dr["Date"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Job = Convert.ToString(dr["Job"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        Job_Location = Convert.ToString(dr["Job_Location"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        OPERate = Convert.ToDouble(dr["OPEClaim"]),
                        Manday = Convert.ToDouble(dr["Manday"]),
                        trans = Convert.ToString(dr["trans"]),
                        StartTime = Convert.ToString(dr["startTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        ODTime = Convert.ToString(dr["ODTime"])




                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;
            return objMOR.lst1;
        }
        #endregion

        #region Export to Excel Job wise TimeSheet



        [HttpGet]
        public ActionResult ExportIndexJobWiseTimeSheet()
        {
            // Using EPPlus from nuget
            string visitreportLink = string.Empty;
            string AttachmentLink = string.Empty;
            string FinalAttch = string.Empty;
            string PreviousText = string.Empty;
            string PreviousReport = string.Empty;
            bool rowincremented = false;
            string FinalURLAttachmentLink = string.Empty;



            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MISJobWiseTimeSheet> grid = CreateExportableGridJobWiseTimeSheet();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                string StyleName = "HyperStyle";

                ExcelNamedStyleXml HyperStyle = sheet.Workbook.Styles.CreateNamedStyle(StyleName);
                HyperStyle.Style.Font.UnderLine = true;
                HyperStyle.Style.Font.Size = 12;
                HyperStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);


                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;
                    column.IsEncoded = false;
                }

                foreach (IGridRow<MISJobWiseTimeSheet> gridRow in grid.Rows)
                {
                    col = 1;
                    rowincremented = false;
                    foreach (IGridColumn column in grid.Columns)
                    {
                        if (column.Title.ToString().ToUpper() == "VISITREPORT")
                        {
                            visitreportLink = "https://tiimes.tuv-india.com/IVRReport/" + column.ValueFor(gridRow).ToString();

                            if (PreviousReport != visitreportLink)
                            {
                                ExcelRange Rng = sheet.Cells[row, col++];

                                Rng.Hyperlink = new Uri(visitreportLink, UriKind.Absolute);
                                Rng.Value = column.ValueFor(gridRow).ToString();
                                Rng.StyleName = StyleName;
                                PreviousReport = visitreportLink;
                            }

                        }
                        else if (column.Title.ToString().ToUpper() == "VATTACHMENT")
                        {
                            if (column.ValueFor(gridRow).ToString() != string.Empty)
                            {
                                string text = column.ValueFor(gridRow).ToString();

                                string[] words = text.Split('!');

                                int prevRow = row;
                                int newRow = row;

                                FinalAttch = string.Empty;
                                sheet.Cells.Style.WrapText = true;
                                sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                sheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                                foreach (var word in words)
                                {

                                    ExcelRange Rng = sheet.Cells[row, 31];

                                    Rng.StyleName = StyleName;
                                    string[] FileDetails = word.Split('|');
                                    AttachmentLink = "https://tiimes.tuv-india.com/VisitReport/Download1?d=" + FileDetails[2].ToString().Trim();
                                    Rng.Hyperlink = new Uri(AttachmentLink, UriKind.Absolute);
                                    Rng.Value = FileDetails[0].ToString().Trim();
                                    row++;
                                    rowincremented = true;
                                }

                                if (words.Length > 1)
                                {
                                    for (int colno = 1; colno < col; colno++)
                                    {
                                        //// sheet.MergedCells = [cnt, colno];
                                        sheet.Cells[prevRow, colno, row - 1, colno].Merge = true;
                                    }
                                }


                            }
                        }
                        else
                        {
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                        }
                    }
                    if (!rowincremented)
                    {
                        row++;
                    }


                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");

            }
        }


        private IGrid<MISJobWiseTimeSheet> CreateExportableGridJobWiseTimeSheet()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<MISJobWiseTimeSheet> grid = new Grid<MISJobWiseTimeSheet>(GetDataJobWiseTimeSheet());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.callstatus).Titled("Call Status");
            grid.Columns.Add(model => model.CreatedDateTime).Titled("Call Creation Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Call Created By");
            grid.Columns.Add(model => model.CoordinatorEmail).Titled("Coordinator Email");
            grid.Columns.Add(model => model.CoordinatorNo).Titled("Coordinator Contact Deatil");
            grid.Columns.Add(model => model.CallType).Titled("Single/Continuous Call");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.SurveyorName).Titled("Surveyor Name");
            grid.Columns.Add(model => model.TUV_Email_ID).Titled("TUV Email ID");
            grid.Columns.Add(model => model.MobileNo).Titled("Mobile No");

            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.EBranch_Name).Titled("Executing Branch");
            grid.Columns.Add(model => model.JOBNO).Titled("Job No");
            grid.Columns.Add(model => model.CustomerPO).Titled("Customer PO");
            grid.Columns.Add(model => model.ClientName).Titled("Customer Name");


            grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.V1).Titled("Sub Job Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Job Vendor Name");
            grid.Columns.Add(model => model.P1).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");

            grid.Columns.Add(model => model.StartTime).Titled("On Site Hrs.");
            grid.Columns.Add(model => model.EndTime).Titled("Off Site Hrs.");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.TravelExpense).Titled("Travel Expenses");
            grid.Columns.Add(model => model.TotalTime).Titled("Total Time");

            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.VisitReport).Titled("VisitReport");
            grid.Columns.Add(model => model.VAttachment).Titled("VAttachment");







            grid.Pager = new GridPager<MISJobWiseTimeSheet>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMISJWTS.lstJWTS1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<MISJobWiseTimeSheet> GetDataJobWiseTimeSheet()
        {


            int intTotalTime = 0;
            int intOPERate = 0;
            int objTotalTime = 0;
            int objOPEClaim = 0;
            int objManday = 0;

            DataSet dsJobWiseTimeSheet = new DataSet();
            // dsJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet();




            List<MISJobWiseTimeSheet> lJWTS = new List<MISJobWiseTimeSheet>();




            if (Session["FromDate"] == null && Session["ToDate"] == null)
            {
                dsJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet();// fill dataset  
            }
            else
            {

                //U.FromDate = Session["FromDate"].ToString();
                //U.ToDate = Session["ToDate"].ToString();
                dsJobWiseTimeSheet = objDMOR.JOBWiseTimeSheet(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));// fill dataset  
            }



            if (dsJobWiseTimeSheet.Tables.Count > 0)
            {
                foreach (DataRow dr in dsJobWiseTimeSheet.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    intTotalTime = Convert.ToInt32(dsJobWiseTimeSheet.Tables[0].Rows[0]["TotalTime"]);

                    if (intTotalTime >= 4)
                    {
                        objTotalTime = 4;
                        objOPEClaim = 275;

                    }
                    else
                    {
                        objTotalTime = Convert.ToInt32(intTotalTime);
                        objOPEClaim = Convert.ToInt32(137.5);

                    }

                    lJWTS.Add(new MISJobWiseTimeSheet
                    {
                        Count = dsJobWiseTimeSheet.Tables[0].Rows.Count,
                        SurveyorName = Convert.ToString(dr["SurveyorName"]),
                        CreatedDateTime = Convert.ToString(dr["CreatedDateTime"]),
                        Call_No = Convert.ToString(dr["Call_No"]),
                        JOBNO = Convert.ToString(dr["JOBNO"]),
                        Sub_Job = Convert.ToString(dr["Sub_Job"]),
                        Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                        SAP_No = Convert.ToString(dr["SAP_No"]),
                        Project_Name = Convert.ToString(dr["Project_Name"]),
                        StartTime = Convert.ToString(dr["StartTime"]),
                        EndTime = Convert.ToString(dr["EndTime"]),
                        TravelTime = Convert.ToString(dr["TravelTime"]),
                        TravelExpense = Convert.ToString(dr["TravelExpense"]),
                        TotalTime = Convert.ToString(dr["TotalTime"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        Description = Convert.ToString(dr["Description"]),
                        VRNumber = Convert.ToString(dr["VRNumber"]),
                        VisitReport = Convert.ToString(dr["VisitReport"]),
                        VRNo = Convert.ToString(dr["VRNo"]),
                        Attachment = Convert.ToString(dr["Attachment"]),
                        OPEClaim = objOPEClaim,
                        V1 = Convert.ToString(dr["V1"]),
                        V2 = Convert.ToString(dr["V2"]),
                        P1 = Convert.ToString(dr["P1"]),
                        P2 = Convert.ToString(dr["P2"]),
                        ActType = Convert.ToString(dr["ActType"]),
                        VAttachment = Convert.ToString(dr["VAttachment"]),
                        CustomerPO = Convert.ToString(dr["CustomerPO"]),
                        ClientName = Convert.ToString(dr["ClientName"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                        EBranch_Name = Convert.ToString(dr["ExecBranch"]),
                        CallType = Convert.ToString(dr["CallType"]),
                        PK_IVR_ID = Convert.ToString(dr["PK_IVR_ID"]),
                        TUV_Email_ID = Convert.ToString(dr["TUV_Email_ID"]),
                        MobileNo = Convert.ToString(dr["MobileNo"]),
                        callstatus = Convert.ToString(dr["Status"]),
                        CoordinatorEmail = Convert.ToString(dr["CoordinatorEmail"]),
                        CoordinatorNo = Convert.ToString(dr["CoordinatorNo"]),


                    });
                }
            }
            ViewData["JobWiseTimeSheet"] = lJWTS;


            objMISJWTS.lstJWTS1 = lJWTS;
            return objMISJWTS.lstJWTS1;
        }

        #endregion        

        #region export to excel OpeReport
        [HttpGet]
        public ActionResult ExportOpeAppReport()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MISOPEReport> grid = CreateExporOPEAppGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<MISOPEReport> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<MISOPEReport> CreateExporOPEAppGrid()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<MISOPEReport> grid = new Grid<MISOPEReport>(GetOPEAppData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(model => model.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");

            grid.Columns.Add(model => model.StartTime).Titled("On Site Hrs");
            grid.Columns.Add(model => model.EndTime).Titled("Off Site Hrs");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.ODTime).Titled("OD Time");




            grid.Columns.Add(model => model.TotalTime).Titled("Total Time");
            grid.Columns.Add(model => model.Manday).Titled("Man Day(1/0.5)");
            grid.Columns.Add(model => model.OPERate).Titled("OPE Claim");




            grid.Pager = new GridPager<MISOPEReport>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMOR.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<MISOPEReport> GetOPEAppData()
        {
            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataSet ds = new DataSet();

            //if (Session["GetExcelData"] == "Yes")
            //{
            //    ds = objDMOR.GetOPEAPPData(objMOR); // fill dataset  
            //}
            //else
            //{
            //    objMOR.FromDate = Convert.ToString(Session["FromDate"]);
            //    objMOR.ToDate = Convert.ToString(Session["ToDate"]);
            //    ds = objDMOR.GetOPEAPPData(objMOR); // fill dataset  
            //}

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                objMOR.FromDate = Convert.ToString(Session["FromDate"]);
                objMOR.ToDate = Convert.ToString(Session["ToDate"]);
            }
            ds = objDMOR.GetOPEAPPData(objMOR); // fill dataset  

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {


                    lmd.Add(new MISOPEReport
                    {
                        EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),

                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        TotalTime = Convert.ToDouble(dr["TotalTime"]),
                        Manday = Convert.ToDouble(dr["ManDay"]),
                        OPERate = Convert.ToDouble(dr["OPERate"])




                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;

            return objMOR.lst1;
        }
        #endregion

        #region Project Status export To excel

        [HttpGet]
        public ActionResult ExportIndexProjectStatus(Projectstatus objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Projectstatus> grid = ProjStatusExportableGrid(objMCR);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Projectstatus> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<Projectstatus> ProjStatusExportableGrid(Projectstatus objMCR)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Projectstatus> grid = new Grid<Projectstatus>(GetDataProjectStatus(objMCR));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.Job).Titled("Job");
            grid.Columns.Add(c => c.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(c => c.Company_Name).Titled("Client Name");
            grid.Columns.Add(c => c.Description).Titled("Project Name");
            grid.Columns.Add(c => c.call_no).Titled("Call Number");
            grid.Columns.Add(c => c.ReqNo).Titled("Requisition No");
            grid.Columns.Add(c => c.VendorName).Titled("Vendor Name");
            grid.Columns.Add(c => c.VendorPO).Titled("Vendor PO No");
            grid.Columns.Add(c => c.Sub_Vendor_Name).Titled("Sub Vendor Name");
            grid.Columns.Add(c => c.SubVendorPO).Titled("PO Number on Sub Vendor");
            grid.Columns.Add(c => c.Job_Location).Titled("Inspection Location");
            grid.Columns.Add(c => c.NotificationNo).Titled("Notification No");
            grid.Columns.Add(c => c.callreceivedDate).Titled("Call Received Date");
            grid.Columns.Add(c => c.PropDateofInspFrom).Titled("Call Requested Date");
            grid.Columns.Add(c => c.ActualDateofInpFrom).Titled("Planned Date");
            grid.Columns.Add(c => c.ActualDateofInpTo).Titled("Actual Visit Date");
            grid.Columns.Add(c => c.callType).Titled("Call Type");
            grid.Columns.Add(c => c.status).Titled("Call Status");
            grid.Columns.Add(c => c.OrderType).Titled("Order Type");
            //grid.Columns.Add(c => c.NoOfDays).Titled("No of Days");
            grid.Columns.Add(c => c.Delay).Titled("Delay of Inspection");
            grid.Columns.Add(c => c.Inspector).Titled("Inspector Name");
            grid.Columns.Add(c => c.branch).Titled("Executing Branch");
            grid.Columns.Add(c => c.VisitReport).Titled("Inspection Visit Report");
            grid.Columns.Add(c => c.TagNo).Titled("Item Code");
            grid.Columns.Add(c => c.AreaofConcern).Titled("Details of Area of Concern/Punch Point");
            grid.Columns.Add(c => c.PendingAct).Titled("Pending Activities");
            grid.Columns.Add(c => c.NC).Titled("Non Confirmaties Raised");
            grid.Columns.Add(c => c.Discipline).Titled("Product Category / Discipline");
            grid.Columns.Add(c => c.ProdDescription).Titled("Product Description");
            grid.Columns.Add(c => c.CanIRNIssued).Titled("Can IRN be Issued");
            grid.Columns.Add(c => c.IssuesPONumber).Titled("PO Item Numbers");
            grid.Columns.Add(c => c.ReleaseNote).Titled("Release Note");

            grid.Columns.Add(c => c.POSrNo).Titled("PO Sr.No");
            grid.Columns.Add(c => c.POIRNItemCode).Titled("PO Item Numbers");
            grid.Columns.Add(c => c.OrgBranch).Titled("Originating Branch");







            grid.Pager = new GridPager<Projectstatus>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstProject.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Projectstatus> GetDataProjectStatus(Projectstatus objMCR)
        {

            DataSet dsCallRegister = new DataSet();
            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                dsCallRegister = objDMOR.ProjectStatus(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {
                dsCallRegister = objDMOR.ProjectStatus();
            }



            List<Projectstatus> lstFeedback = new List<Projectstatus>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new Projectstatus
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Job_Location = Convert.ToString(dr["Inspection Location"]),
                                Job = Convert.ToString(dr["Job"]),
                                Description = Convert.ToString(dr["Project Name"]),
                                call_no = Convert.ToString(dr["call_no"]),
                                Sub_Job = Convert.ToString(dr["sub_job"]),
                                Company_Name = Convert.ToString(dr["Client Name"]),
                                VendorName = Convert.ToString(dr["VendorName"]),
                                VendorPO = Convert.ToString(dr["VendorPoNo"]),
                                Sub_Vendor_Name = Convert.ToString(dr["Sub Vendor Name"]),
                                SubVendorPO = Convert.ToString(dr["Sub Vendor PO"]),
                                ReqNo = Convert.ToString(dr["Requisition No"]),
                                PropDateofInspFrom = Convert.ToString(dr["Proposed Date of Inspection From"]),
                                PropDateofInspTo = Convert.ToString(dr["Proposed Date of Inspection To"]),
                                ActualDateofInpFrom = Convert.ToString(dr["Actual Visit Date of Inspection From"]),
                                ActualDateofInpTo = Convert.ToString(dr["Actual Visit Date of Inspection To"]),
                                callreceivedDate = Convert.ToString(dr["Notification Received Date"]),
                                NoOfDays = Convert.ToString(dr["No of Days"]),
                                Delay = Convert.ToString(dr["Delay of Inspection"]),
                                Inspector = Convert.ToString(dr["Surveyor Name"]),
                                VisitReport = Convert.ToString(dr["Inspection Report"]),
                                ReleaseNote = Convert.ToString(dr["Release Note"]),
                                TagNo = Convert.ToString(dr["Tag No"]),
                                AreaofConcern = Convert.ToString(dr["Details of Area of Concern/Punch Point"]),
                                NC = Convert.ToString(dr["Non Confirmaties Raised"]),
                                PendingAct = Convert.ToString(dr["Pending Activities"]),
                                IssuesPONumber = Convert.ToString(dr["Issued PO Item Numbers"]),
                                CanIRNIssued = Convert.ToString(dr["Can IRN be Issued"]),
                                NotificationNo = Convert.ToString(dr["Notification No"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                AllPONumbers = Convert.ToString(dr["AllPONumbers"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                ProdDescription = Convert.ToString(dr["ProdDescription"]),
                                callType = Convert.ToString(dr["callType"]),
                                status = Convert.ToString(dr["status"]),
                                branch = Convert.ToString(dr["branch"]),
                                OrderType = Convert.ToString(dr["OrderType"]),
                                OrgBranch = Convert.ToString(dr["OrgBranch"]),


                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            return objMCR.lstProject;
        }

        #endregion

        #region Debit Credit export To excel


        [HttpGet]
        public ActionResult ExportIndexDebitCredit(DebitCredit objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 1;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                // IGrid<DebitCredit> grid = DebitCreditExportableGrid(objMCR);

                DataSet grid = DebitCreditExportableGrid(objMCR);

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                /*
                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<DebitCredit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                */

                string pCallNo = string.Empty;
                string pCallDate = string.Empty;
                string pESOCode = string.Empty;
                bool rowIncrease = false;


                for (int dsCol = 0; dsCol < grid.Tables[0].Columns.Count; dsCol++)
                {
                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                    sheet.Cells[row, col].Value = grid.Tables[0].Columns[dsCol].ColumnName.ToString().ToUpper();
                    col++;
                }
                col = 1;
                row++;

                for (int dsrow = 0; dsrow < grid.Tables[0].Rows.Count; dsrow++)
                {
                    col = 1;
                    for (int dsCol1 = 0; dsCol1 < grid.Tables[0].Columns.Count; dsCol1++)
                    {
                        if (grid.Tables[0].Rows[dsrow]["ROW1"].ToString() == "1")
                        {
                            if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() != "ROW1")
                            {
                                if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() == "ESOCODE")
                                {
                                    sheet.Cells[row, col, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                    sheet.Cells[row, col, row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                            }
                            rowIncrease = true;
                        }
                        else if (grid.Tables[0].Rows[dsrow]["ROW1"].ToString() == "2")
                        {
                            if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() != "ROW1")
                            {
                                if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() == "ESOCODE")
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    // sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                            }
                            rowIncrease = true;
                        }
                        else if (grid.Tables[0].Rows[dsrow]["ROW1"].ToString() == "3")
                        {
                            if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() != "ROW1")
                            {
                                if (grid.Tables[0].Columns[dsCol1].ColumnName.ToUpper() == "ESOCODE")
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Pink);
                                    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                                }
                                rowIncrease = true;
                            }

                        }

                        //else
                        //{
                        //    //sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    //sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green);
                        //    sheet.Cells[row, col].Value = grid.Tables[0].Rows[dsrow][dsCol1].ToString();
                        //    rowIncrease = true;
                        //}
                        col++;

                    }


                    pCallNo = grid.Tables[0].Rows[dsrow]["Call_No"].ToString();
                    pCallDate = grid.Tables[0].Rows[dsrow]["Date"].ToString();
                    pESOCode = grid.Tables[0].Rows[dsrow]["TSOCOde"].ToString();

                    if (rowIncrease)
                    {
                        row++;
                    }

                }



                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private DataSet DebitCreditExportableGrid(DebitCredit objMCR)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            // IGrid<DebitCredit> grid = new Grid<DebitCredit>(GetDataDebitCredit(objMCR));

            DataSet grid = GetDataDebitCredit(objMCR);

            /* grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.Call_No).Titled("Call No");
            grid.Columns.Add(c => c.Date).Titled("Date");
            grid.Columns.Add(c => c.UserName).Titled("User Name");
            grid.Columns.Add(c => c.ExecutingBranch).Titled("Executing Branch");
            grid.Columns.Add(c => c.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(c => c.TSalesOrderNo).Titled("TIIMES Sales Order No");
            grid.Columns.Add(c => c.ESalesOrderNo).Titled("SAP Sales Order No");

            grid.Columns.Add(c => c.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(c => c.OriginatingBranch).Titled("Originating Branch");
            grid.Columns.Add(c => c.OnSiteHours).Titled("On Site Hours");
            grid.Columns.Add(c => c.OffSiteHours).Titled("Off Site Hours");
            grid.Columns.Add(c => c.TotalChargeble).Titled("Total Chargeble");
            grid.Columns.Add(c => c.EmployeeCostcentre).Titled("Employee Costcentre");
            grid.Columns.Add(c => c.ESOCode).Titled("SAP SO CostCenter");
            grid.Columns.Add(c => c.TSOCode).Titled("TIIMES SO CostCenter");

            grid.Columns.Add(c => c.Client_Name).Titled("Client Name");
            grid.Columns.Add(c => c.OnSiteHourRate).Titled("On site Hours Rate");
            grid.Columns.Add(c => c.AmountDRCR).Titled("Amount DR/CR");
            grid.Columns.Add(c => c.Description).Titled("Description");
            grid.Columns.Add(c => c.CreditBranch).Titled("Credit Branch");
            grid.Columns.Add(c => c.DebitBranch).Titled("Debit Branch");

            grid.Pager = new GridPager<DebitCredit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstDebitCredit.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            */

            return grid;
        }

        public DataSet GetDataDebitCredit(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                dsCallRegister = objDMOR.DebitCredit(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.DebitCredit();
            }



            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                UserName = Convert.ToString(dr["UserName"]),
                                ExecutingBranch = Convert.ToString(dr["ExecutingBranch"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                OriginatingBranch = Convert.ToString(dr["OriginatingBranch"]),
                                OnSiteHours = Convert.ToString(dr["OnSiteHours"]),
                                OffSiteHours = Convert.ToString(dr["OffSiteHours"]),
                                TotalChargeble = Convert.ToString(dr["TotalChargeble"]),
                                EmployeeCostcentre = Convert.ToString(dr["EmployeeCostcentre"]),
                                SOCostCenter = Convert.ToString(dr["SOCostCenter"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                OnSiteHourRate = Convert.ToString(dr["OnSiteHourRate"]),
                                AmountDRCR = Convert.ToString(dr["AmountDRCR"]),
                                Description = Convert.ToString(dr["Description"]),
                                CreditBranch = Convert.ToString(dr["CreditBranch"]),
                                DebitBranch = Convert.ToString(dr["DebitBranch"]),
                                //  EMaterial = Convert.ToString(dr["EMaterial"]),
                                ESalesOrderNo = Convert.ToString(dr["ESalesOrderNo"]),
                                ESOCode = Convert.ToString(dr["ESOCode"]),
                                //   TMaterial = Convert.ToString(dr["TMaterial"]),
                                TSalesOrderNo = Convert.ToString(dr["TSalesOrderNo"]),
                                TSOCode = Convert.ToString(dr["TSOCode"]),





                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            //    return objMCR.lstDebitCredit;
            return dsCallRegister;
        }

        #endregion

        #region TOYO MIS export To excel

        [HttpGet]
        public ActionResult ExportIndexTOYOMIS(TOYOMIS objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TOYOMIS> grid = TOYOMISExportableGrid(objMCR);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<TOYOMIS> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<TOYOMIS> TOYOMISExportableGrid(TOYOMIS objMCR)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<TOYOMIS> grid = new Grid<TOYOMIS>(GetDataTOYOMIS(objMCR));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.client).Titled("Client");
            grid.Columns.Add(c => c.Job).Titled("Job Number");
            grid.Columns.Add(c => c.SubJob).Titled("Sub Job Number");
            grid.Columns.Add(c => c.Project).Titled("Project Name");
            grid.Columns.Add(c => c.VendorName).Titled("Vendor Name");
            grid.Columns.Add(c => c.SubVendorName).Titled("Sub Vendor Name");
            grid.Columns.Add(c => c.Requisition_No).Titled("Purchase Order No");
            grid.Columns.Add(c => c.Inspection_Location).Titled("Inspection Location");
            grid.Columns.Add(c => c.Discipline).Titled("Discipline");
            grid.Columns.Add(c => c.PO_Quantity).Titled("PO Quantity");
            grid.Columns.Add(c => c.Ammend_Quantity).Titled("Ammend Quantity");
            grid.Columns.Add(c => c.Offered_Quanity).Titled("Offered Quanity");
            grid.Columns.Add(c => c.Notification_Received_Date).Titled("Notification Received Date");
            grid.Columns.Add(c => c.Proposed_Date_of_Inspection_From).Titled("Proposed_Date_of_Inspection_From");
            grid.Columns.Add(c => c.Proposed_Date_of_Inspection_To).Titled("Proposed_Date of Inspection To");
            grid.Columns.Add(c => c.Actual_Visit_Date_of_Inspection_From).Titled("Actual_Visit_Date_of Inspection From");
            grid.Columns.Add(c => c.Actual_Visit_Date_of_Inspection_To).Titled("Actual_Visit_Date_of_Inspection_To");
            grid.Columns.Add(c => c.No_of_Days).Titled("No_of_Days");
            grid.Columns.Add(c => c.Delay_of_Inspection).Titled("Delay of Inspection");
            grid.Columns.Add(c => c.Surveyor_Name).Titled("Surveyor Name");
            grid.Columns.Add(c => c.InspectionActivity).Titled("Inspection Activity");
            grid.Columns.Add(c => c.Inspection_Report).Titled("Inspection Visit Report No & Date");
            grid.Columns.Add(c => c.CanIRNbeIssued).Titled("Can IRN be Issued");
            grid.Columns.Add(c => c.POIRNItemCode).Titled("Item Code");
            grid.Columns.Add(c => c.POSrNo).Titled("Item Sr.No");
            grid.Columns.Add(c => c.Release_Note).Titled("Release Note No");
            grid.Columns.Add(c => c.Final_IRN_Issued_Date).Titled("Final IRN Issued Date");
            grid.Columns.Add(c => c.Details_of_Area_of_Concern).Titled("Details of Area of Concern");
            grid.Columns.Add(c => c.IRN_Quantity).Titled("IRN Release Quantity");
            grid.Columns.Add(c => c.Pending_Activities).Titled("Pending Activities");
            //grid.Columns.Add(c => c.Areas_of_Concers_if_Any).Titled("Areas of Concers if Any");
            grid.Columns.Add(c => c.Updated_Remark).Titled("Updated Remark");
            grid.Columns.Add(c => c.Report).Titled("Report");
            grid.Columns.Add(c => c.CreationDate).Titled("Created Date");









            grid.Pager = new GridPager<TOYOMIS>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstProject.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<TOYOMIS> GetDataTOYOMIS(TOYOMIS objMCR)
        {

            DataSet dsCallRegister = new DataSet();
            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                dsCallRegister = objDMOR.TOYOMIS(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {
                dsCallRegister = objDMOR.TOYOMIS();
            }



            List<TOYOMIS> lstFeedback = new List<TOYOMIS>();

            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new TOYOMIS
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                client = Convert.ToString(dr["client"]),
                                VendorName = Convert.ToString(dr["VendorName"]),
                                SubVendorName = Convert.ToString(dr["SubVendorName"]),
                                Requisition_No = Convert.ToString(dr["Requisition_No"]),
                                Inspection_Location = Convert.ToString(dr["Inspection_Location"]),
                                Discipline = Convert.ToString(dr["Discipline"]),
                                PO_Quantity = Convert.ToString(dr["PO_Quantity"]),
                                Ammend_Quantity = Convert.ToString(dr["Ammend_Quantity"]),
                                Offered_Quanity = Convert.ToString(dr["Offered_Quanity"]),
                                Notification_Received_Date = Convert.ToString(dr["Notification_Received_Date"]),
                                Proposed_Date_of_Inspection_From = Convert.ToString(dr["Proposed_Date_of_Inspection_From"]),
                                Proposed_Date_of_Inspection_To = Convert.ToString(dr["Proposed_Date_of_Inspection_To"]),
                                Actual_Visit_Date_of_Inspection_From = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_From"]),
                                Actual_Visit_Date_of_Inspection_To = Convert.ToString(dr["Actual_Visit_Date_of_Inspection_To"]),
                                No_of_Days = Convert.ToString(dr["No_of_Days"]),
                                Delay_of_Inspection = Convert.ToString(dr["Delay_of_Inspection"]),
                                Surveyor_Name = Convert.ToString(dr["Surveyor_Name"]),
                                Inspection_Report = Convert.ToString(dr["Inspection_Report"]),
                                Release_Note = Convert.ToString(dr["Release_Note"]),
                                Final_IRN_Issued_Date = Convert.ToString(dr["Final_IRN_Issued_Date"]),
                                Details_of_Area_of_Concern = Convert.ToString(dr["Details_of_Area_of_Concern"]),
                                IRN_Quantity = Convert.ToString(dr["IRN_Quantity"]),
                                Pending_Activities = Convert.ToString(dr["Pending_Activities"]),
                                // Areas_of_Concers_if_Any = Convert.ToString(dr["Areas_of_Concers_if_Any"]),
                                Updated_Remark = Convert.ToString(dr["Updated_Remark"]),
                                Job = Convert.ToString(dr["job"]),
                                Project = Convert.ToString(dr["project"]),
                                POIRNItemCode = Convert.ToString(dr["POIRNItemCode"]),
                                POSrNo = Convert.ToString(dr["POSrNo"]),
                                CanIRNbeIssued = Convert.ToString(dr["CanIRNbeIssued"]),
                                InspectionActivity = Convert.ToString(dr["InspectionActivity"]),
                                SubJob = Convert.ToString(dr["Sub_job"]),
                                Report = Convert.ToString(dr["Inspection_Report"]),
                                CreationDate = Convert.ToString(dr["IVR_CreateDate"]),


                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lstProject = lstFeedback;
            return objMCR.lstProject;
        }

        #endregion

        public ActionResult AttendanceSheet()
        {
            try
            {

                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                Session["GetExcelData"] = "Yes";

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    NonInspectionActivity n = new NonInspectionActivity();

                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetAdminAttendanceSheet(n);
                }

                DataTable DtActivityMaster = new DataTable();

                DtActivityMaster = objNIA.GetActivityMaster();

                objModel.dtActivityMaster = DtActivityMaster;

                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult AttendanceSheet(NonInspectionActivity n, FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData.Keep();


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetAdminAttendanceSheet(n); // fill dataset  
                }

                DataTable DtActivityMaster = new DataTable();

                DtActivityMaster = objNIA.GetActivityMaster();

                objModel.dtActivityMaster = DtActivityMaster;


                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }

            return View(objModel);
        }


        #region export Attendance to excel 

        [HttpGet]
        public ActionResult ExportAttIndex(NonInspectionActivity c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;

                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;

                int WOLeaveCount = 0;
                int PHLeaveCount = 0;
                int SelectDays = 0;
                int WorkingDays = 0;
                int WKDLeaveCount = 0;
                int NACount = 0;
                double filledPercentage = 0;
                string DayName = string.Empty;
                string strCheck = string.Empty;

                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = CreateAttExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.ColumnName.ToString();
                    sheet.Column(col++).Width = 18;
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NOA COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPGRADE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LPG"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLING"


                        )
                        {
                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString() == "CL")
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "SL")
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "PL" || grid.Rows[Nrow][Ncol].ToString() == "A33")
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("48"))
                            {
                                NACount++;

                            }

                            nullcount++;
                        }
                    }

                    col = 1;
                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {
                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPGRADE"                             
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "LPG"

                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "NOA COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                sheet.Cells[row, col].Value = NACount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "PL" || grid.Rows[Nrow][Ncol1].ToString() == "CL" || grid.Rows[Nrow][Ncol1].ToString() == "SL" || grid.Rows[Nrow][Ncol1].ToString() == "A33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                if (grid.Rows[Nrow][Ncol1].ToString() == "A33")
                                {
                                    sheet.Cells[row, col].Value = "AUTO PL";
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                // sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                sheet.Cells[row, col].Value = "Weekly Off";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                                //  sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                sheet.Cells[row, col].Value = "Public Holiday";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLING")
                            {

                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                SelectDays = SelectDays = Convert.ToInt32(grid.Rows[Nrow]["SELECTED PERIOD DAYS"].ToString());

                                if (SelectDays == nullcount && WorkingDays < nullcount)
                                {
                                    strCheck = "Correct";
                                }
                                else
                                {
                                    strCheck = "Incorrect";
                                }
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = strCheck.ToString();


                            }
                            else
                            {
                                if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                                    sheet.Cells[row, col].Value = string.Empty;
                                else
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }
                    row++;
                    nullcount = 0;
                    LeaveCount = 0;
                    OTHLeaveCount = 0;

                    CLeaveCount = 0;
                    SLeaveCount = 0;
                    PLeaveCount = 0;

                    WOLeaveCount = 0;
                    PHLeaveCount = 0;
                    WKDLeaveCount = 0;
                    SelectDays = 0;

                    filledPercentage = 0;
                    WorkingDays = 0;
                    NACount = 0;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }

           

        }

        private DataTable CreateAttExportableGrid(NonInspectionActivity c)
        {

            ///// IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetAttData(c));


            DataTable grid = GetAttData(c);


            return grid;
        }

        public DataTable GetAttData(NonInspectionActivity c)
        {
            DataSet ds = new DataSet();
            FormCollection fc = new FormCollection();

            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                c.StartDate = Convert.ToString(TempData["FromD"]);
                c.EndDate = Convert.ToString(TempData["ToD"]);

                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();

                ds = objNIA.GetAdminAttendanceSheet(c); // fill dataset  
            }

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  


            if (ds.Tables.Count > 0)
            {


            }
            return ds.Tables[0];
        }


        #endregion

        #region UserData 

        public ActionResult UserData()
        {
            try
            {


                DataSet ds = new DataSet();
                List<ProfileUser> lstUserDashBoard = new List<ProfileUser>();
                Session["GetExcelData"] = "Yes";


                NonInspectionActivity n = new NonInspectionActivity();
                ProfileUser ProfileUsers = new ProfileUser();

                TempData.Keep();

                ds = objNIA.GetUserDataSheet();


                if (ds.Tables.Count > 0)
                {
                    /* foreach (DataRow dr in ds.Tables[0].Rows)
                     {
                         lstUserDashBoard.Add
                         (
                             new ProfileUser
                             {

                                 FirstName = Convert.ToString(dr["FirstName"]),
                                 LastName = Convert.ToString(dr["LastName"]),
                                 DateOfJoining = Convert.ToString(dr["DateOfJoining"]),
                                 Branch = Convert.ToString(dr["Branch"]),
                                 EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                                 EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                 Designation = Convert.ToString(dr["Designation"]),
                                 TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                 DOB = Convert.ToString(dr["DOB"]),
                                 PanNo = Convert.ToString(dr["PanNo"]),
                                 MobileNo = Convert.ToString(dr["MobileNo"]),
                                 Gender = Convert.ToString(dr["Gender"]),
                                /// ExperienceInMonth = Convert.ToString(dr["ExperienceInMonth"]),
                                 TotalyearofExprience = Convert.ToString(dr["TotalyearofExprience"]),
                                 TUVIStampNo = Convert.ToString(dr["TUVIStampNo"]),
                                 Qualification = Convert.ToString(dr["Qualification"]),
                                 ProfCertificates = Convert.ToString(dr["ProfCertificates"]),
                                 Additional_Qualification = Convert.ToString(dr["Qualification"]),
                                 ///Attachments = Convert.ToString(dr["Attachments"]),
                                 ProfQualAtta = Convert.ToString(dr["ProfCertificates"]),
                                 BPhoto = (byte[])(dr["Photo"]),
                                 //Photo = Convert.ToString(dr["Photo"]),
                                 //PhotoExists = Convert.ToString(dr["PhotoExists"]),
                                 Signature = Convert.ToString(dr["Sign"]),
                                 CV = Convert.ToString(dr["CV"]),
                                 PAN = Convert.ToString(dr["PAN"]),
                                 ETR = Convert.ToString(dr["ETR"]),
                                 FMR = Convert.ToString(dr["FMR"]),

                                 ExpBeforeTUV = Convert.ToString(dr["ExpBeforeTUV"]),
                                 Age = Convert.ToString(dr["Age"]),
                                 ShoesSize = Convert.ToString(dr["ShoesSize"]),
                                 ShirtSize = Convert.ToString(dr["ShirtSize"]),
                                 EmergencyMobile_No = Convert.ToString(dr["EmergencyMobile_No"]),
                                 currentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                             }
                         );
                     }*/

                }

                //   objProfile.LstUserData = lstUserDashBoard;
                objProfile.dtUserData = ds.Tables[0];
                ViewData["UserData"] = lstUserDashBoard;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objProfile);
        }

        [HttpPost]
        public ActionResult UserData(NonInspectionActivity n, FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData.Keep();
                ProfileUser ProfileUsers = new ProfileUser();
                List<ProfileUser> lstUserDashBoard = new List<ProfileUser>();
                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();


                ds = objNIA.GetUserDataSheet(); // fill dataset  


                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lstUserDashBoard.Add(
                            new ProfileUser
                            {

                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                DateOfJoining = Convert.ToString(dr["DateOfJoining"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                DOB = Convert.ToString(dr["DOB"]),
                                PanNo = Convert.ToString(dr["PanNo"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Gender = Convert.ToString(dr["Gender"]),
                                // ExperienceInMonth = Convert.ToString(dr["ExperienceInMonth"]),
                                TotalyearofExprience = Convert.ToString(dr["TotalyearofExprience"]),
                                TUVIStampNo = Convert.ToString(dr["TUVIStampNo"]),
                                Qualification = Convert.ToString(dr["Qualification"]),
                                ProfCertificates = Convert.ToString(dr["ProfCertificates"]),
                                Additional_Qualification = Convert.ToString(dr["Qualification"]),
                                /// Attachments = Convert.ToString(dr["Attachments"]),
                                ProfQualAtta = Convert.ToString(dr["ProfCertificates"]),
                                Photo = Convert.ToString(dr["Photo"]),
                                Signature = Convert.ToString(dr["Sign"]),
                                CV = Convert.ToString(dr["CV"]),
                                PAN = Convert.ToString(dr["PAN"]),
                                ETR = Convert.ToString(dr["ETR"]),
                                FMR = Convert.ToString(dr["FMR"]),

                                ExpBeforeTUV = Convert.ToString(dr["ExpBeforeTUV"]),
                                Age = Convert.ToString(dr["Age"]),
                                ShoesSize = Convert.ToString(dr["ShoesSize"]),
                                ShirtSize = Convert.ToString(dr["ShirtSize"]),
                                EmergencyMobile_No = Convert.ToString(dr["EmergencyMobile_No"]),
                                currentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                            }
                        );
                    }
                }

                objProfile.LstUserData = lstUserDashBoard;
                ViewData["UserData"] = lstUserDashBoard;


            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }

            return View(objProfile);
        }

        [HttpGet]
        public ActionResult ExportUserData()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ProfileUser> grid = CreateUserDataExportableGrid();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ProfileUser> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                //added by nikita on 18122023
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "UserDetailDashboard" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }

        private IGrid<ProfileUser> CreateUserDataExportableGrid()
        {

            IGrid<ProfileUser> grid = new Grid<ProfileUser>(GetUserData());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            //added by nikita on 18122023
            grid.Columns.Add(model => model.FirstName).Titled("First Name");
            grid.Columns.Add(model => model.MiddleName).Titled("Middle Name");
            grid.Columns.Add(model => model.LastName).Titled("Last Name");
            grid.Columns.Add(model => model.DateOfJoining).Titled("Date Of Joining");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Cost_Center).Titled("Cost Center");
            grid.Columns.Add(model => model.EmployementCategory).Titled("Employement Category");
            grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");
            grid.Columns.Add(model => model.EmployeeCode).Titled("HR Employee Code");
            grid.Columns.Add(model => model.SAP_Emp_code).Titled("SAP Employee Code");
            grid.Columns.Add(model => model.SAP_VendorCode).Titled("SAP Vendor Code");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
            grid.Columns.Add(model => model.TuvEmailId).Titled("TUV Email-ID");
            grid.Columns.Add(model => model.OBSTYPE).Titled("OBS Type");
            grid.Columns.Add(model => model.DOB).Titled("Date Of Birth");
            grid.Columns.Add(model => model.EmailID).Titled("Personal Email ID");
            grid.Columns.Add(model => model.MobileNo).Titled("Mobile No");
            grid.Columns.Add(model => model.EmergencyMobile_No).Titled("Emergency Mobile No");
            grid.Columns.Add(model => model.ResidenceNo).Titled("Residence Number");
            grid.Columns.Add(model => model.OfficeNo).Titled("TUVI Landline Number");
            grid.Columns.Add(model => model.ExpBeforeTUV).Titled("Exprience before Joining TUVI");
            grid.Columns.Add(model => model.TotalyearofExprience).Titled("Total year of Exprience");
            grid.Columns.Add(model => model.ResidenceAddress).Titled("Residing Address");
            grid.Columns.Add(model => model.PermanentAddress).Titled("Permanent Address");
            grid.Columns.Add(model => model.ZipCode).Titled("Residing Address PIN code");
            grid.Columns.Add(model => model.PerAddrPin).Titled("Permanent Address PIN code");
            grid.Columns.Add(model => model.BloodGroup).Titled("Blood Group");
            grid.Columns.Add(model => model.ShoesSize).Titled("Shoes Size");
            grid.Columns.Add(model => model.ShirtSize).Titled("Shirt Size");
            grid.Columns.Add(model => model.TUVIStampNo).Titled("TUVI Stamp No");
            grid.Columns.Add(model => model.currentAssignment).Titled("current Assignment");
            grid.Columns.Add(model => model.Qualification).Titled("Qualification");
            grid.Columns.Add(model => model.University).Titled("University");
            grid.Columns.Add(model => model.YearOfPassing).Titled("Year Of Passing");
            grid.Columns.Add(model => model.LastYearPerc).Titled("Last Year %/CGPA");
            grid.Columns.Add(model => model.AggregatePerc).Titled("Aggregate %/CGPA");
            grid.Columns.Add(model => model.FileName).Titled("Certification");
            grid.Columns.Add(model => model.EyeTest).Titled("Eye Test (YES/NO)");
            grid.Columns.Add(model => model.ProfCertificates).Titled("Professional Qualification");
            grid.Columns.Add(model => model.CV).Titled("CV");


            grid.Pager = new GridPager<ProfileUser>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objProfile.LstUserData.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ProfileUser> GetUserData()
        {
            Session["GetExcelData"] = null;
            ProfileUser ProfileUsers = new ProfileUser();
            List<ProfileUser> lstUserDashBoard = new List<ProfileUser>();
            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
            DataSet ds = new DataSet();


            ds = objNIA.GetUserDataSheet(); // fill dataset  


            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lstUserDashBoard.Add(
                        new ProfileUser
                        {
                            //added by nikita on 18122023
                            FirstName = Convert.ToString(dr["FirstName"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            MiddleName = Convert.ToString(dr["MiddleName"]),
                            Cost_Center = Convert.ToString(dr["CostCenter_Id"]),
                            EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                            SAP_Emp_code = Convert.ToString(dr["SAP_Emp_code"]),
                            SAP_VendorCode = Convert.ToString(dr["SAP_VendorCode"]),
                            OBSTYPE = Convert.ToString(dr["OBSName"]),
                            EmailID = Convert.ToString(dr["EmailID"]),
                            ResidenceNo = Convert.ToString(dr["ResidenceNo"]),
                            EmergencyMobile_No = Convert.ToString(dr["EmergencyMobile_No"]),
                            OfficeNo = Convert.ToString(dr["OfficeNo"]),
                            currentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                            ResidenceAddress = Convert.ToString(dr["Address1"]),
                            PermanentAddress = Convert.ToString(dr["Address2"]),
                            ZipCode = Convert.ToString(dr["SiteAddr"]),
                            PerAddrPin = Convert.ToString(dr["PermanantPin"]),
                            BloodGroup = Convert.ToString(dr["BloodGroup"]),
                            ShoesSize = Convert.ToString(dr["ShoesSize"]),
                            ShirtSize = Convert.ToString(dr["ShirtSize"]),
                            AggregatePerc = Convert.ToString(dr["AggregatePerc"]),
                            YearOfPassing = Convert.ToString(dr["YearOfPassing"]),
                            FileName = Convert.ToString(dr["FileName"]),
                            University = Convert.ToString(dr["University"]),
                            EyeTest = Convert.ToString(dr["EyeTest"]),
                            CV = Convert.ToString(dr["CV"]),
                            DateOfJoining = Convert.ToString(dr["DateOfJoining"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                            DOB = Convert.ToString(dr["DOB"]),
                            Age = Convert.ToString(dr["Age"]),
                            PanNo = Convert.ToString(dr["PanNo"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            ExperienceInMonth = Convert.ToString(dr["ExperienceInMonth"]),
                            ExpBeforeTUV = Convert.ToString(dr["ExpBeforeTUV"]),
                            TotalyearofExprience = Convert.ToString(dr["TotalyearofExprience"]),
                            TUVIStampNo = Convert.ToString(dr["TUVIStampNo"]),
                            Qualification = Convert.ToString(dr["Qualification"]),
                            ProfCertificates = Convert.ToString(dr["ProfCertificates"]),
                            Additional_Qualification = Convert.ToString(dr["Qualification"]),
                            //Attachments = Convert.ToString(dr["Attachments"]),

                        }
                        );
                }
            }
            objProfile.LstUserData = lstUserDashBoard;
            ViewData["UserData"] = lstUserDashBoard;

            return lstUserDashBoard;
        }

        //public void Download(String p, String d)
        //{



        //    DataTable DTDownloadFile = new DataTable();
        //    List<FileDetails> lstEditFileDetails = new List<FileDetails>();
        //    DTDownloadFile = objNIA.GetFileContent(Convert.ToInt32(d));

        //    string fileName = string.Empty;
        //    string contentType = string.Empty;
        //    byte[] bytes = null;

        //    if (DTDownloadFile.Rows.Count > 0)
        //    {
        //        bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
        //        fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
        //    }

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.Charset = "";
        //    /// Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetCacheability(HttpCacheability.Public);
        //    Response.ContentType = contentType;
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    Response.BinaryWrite(bytes);
        //    Response.Flush();
        //    Response.End();

        //}

        public FileResult Download(String p, string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objNIA.GetFileContent(Convert.ToInt32(d));

            if (DTDownloadFile.Rows.Count > 0)
            {
                //fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                //Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            //DateTime date = Convert.ToDateTime(Date);
            //int year = date.Year;
            //int Month = date.Month;

            //int intC = Convert.ToInt32(Month);
            //string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);


            //Build the File Path.
            //string path = Server.MapPath("~/Content/JobDocument/") + d;
            //var savePath = Path.Combine(Server.MapPath("~/IVRIRNSupportDocs/"), a + item.FileName);

            string path = Server.MapPath("~/Content/Sign/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", FileName);
        }

        #endregion

        public ActionResult UnderConstruction()
        {
            return View();
        }

        //Added By Satish Pawar On 06 June 2023


        public ActionResult OPE_Report()
        {
            return View();
        }

        public JsonResult Get_OPE_SearchData(string Month, string Year)
        {
            string JsonString = "";
            //MISOPEReport objMOR =new MISOPEReport();
            DataSet ds = new DataSet();

            Session["Month"] = Month;
            Session["Year"] = Year;

            //ds = objDMOR.GetData(objMOR); // fill dataset  
            ds = objDMOR.GetData(Month, Year); // fill dataset  
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendForApproval(string OP_Number)
        {
            string JsonString = "";
            DataSet ds = new DataSet();
            ds = objDMOR.SendForApproval(OP_Number); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OPE_Approval()
        {
            return View();
        }
        public JsonResult GetOPApprovalList()
        {
            DataSet ds = new DataSet();
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId_ = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

            if (Role == "Expenses approver"  || UserId_ == "TUV0003107")
            {
                ds = objDMOR.GetOPApprovalList_Finance("");

            }
            else
            {
                ds = objDMOR.GetOPApprovalList("");
            }
            //ds = objDMOR.GetOPApprovalList("");
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOPBulk_ApprovalList(string Data1, string _OP_Number)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(Data1);
            DataSet ds1 = new DataSet();
            ds1.Tables.Add(dt);

            DataSet ds = new DataSet();
            ds = objDMOR.GetOPBulk_ApprovalList(ds1, "", _OP_Number);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }



        public ActionResult OPE_History(string OP_Number)
        {
            ViewBag.OP_Number = OP_Number;
            return View();
        }
        public JsonResult GetOP_HistoryList(string OP_Number)
        {
            DataSet ds = new DataSet();
            ds = objDMOR.GetOPHistoryList(OP_Number);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OPE_Approval_History(string OP_Number)
        {
            ViewBag.OP_Number = OP_Number;
            return View();
        }
        public JsonResult GetOP_Approval_HistoryList(string OP_Number)
        {
            DataSet ds = new DataSet();
            ds = objDMOR.GetOPHistoryList(OP_Number);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Save_OP_Approval_HistoryList(string OP_Number)
        {
            DataSet ds = new DataSet();
            ds = objDMOR.GetOPHistoryList(OP_Number);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public JsonResult ExportToExcel1(string OPE_Number)
        //{
        //    try
        //    {
        //        DataSet dsHtml = new DataSet();
        //        dsHtml = objDMOR.OPE_Data(OPE_Number);
        //        string htmlContent = "";
        //        string input = OPE_Number;
        //        string[] parts = input.Split('/');
        //        //string month = parts[1];
        //        string year = parts[2];


        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            // Create the worksheet
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
        //            sheet.Cells["A1"].Value = "TUV India Private Ltd.";
        //            var cell = sheet.Cells["A1"];
        //            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        //            cell.Style.Font.Size = 14; // Replace 12 with your desired font size
        //            cell.Style.Font.Bold = true;

        //            sheet.Cells["A3"].Value = "Out of Pocket Expenses Report:";
        //            var cell3 = sheet.Cells["A3"];
        //            // Replace 12 with your desired font size
        //            cell3.Style.Font.Bold = true;
        //            //sheet.Cells["B3"].Value = 
        //            //sheet.Cells["C3"].Value = "Month:";
        //            //var cell4 = sheet.Cells["C3"];
        //            //cell4.Style.Font.Bold = true;

        //            //sheet.Cells["D3"].Value = month;
        //            sheet.Cells["E3"].Value = "Year:";
        //            var cell5 = sheet.Cells["E3"];
        //            cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
        //            sheet.Cells["f3"].Value = "20" + year;

        //            sheet.Cells["G3"].Value = "Submission Date:";
        //            var cell6 = sheet.Cells["G3"];
        //            cell6.Style.Font.Bold = true;
        //            sheet.Cells["H3"].Value = DateTime.Now.ToShortDateString();


        //            sheet.Cells["A5"].Value = "Out of Pocket Voucher Number:";
        //            var cell2 = sheet.Cells["A5"];
        //            cell2.Style.Font.Size = 13; // Replace 12 with your desired font size
        //            cell2.Style.Font.Bold = true;

        //            //sheet.Cells["B5"].Value = Session["VoucherNo"].ToString();
        //            sheet.Cells["B5"].Value = OPE_Number;
        //            sheet.Cells["A7"].Value = "Branch:";
        //            var cell7 = sheet.Cells["A7"];
        //            cell7.Style.Font.Bold = true;
        //            sheet.Cells["B7"].Value = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();

        //            sheet.Cells["A8"].Value = "Name of Employee:";
        //            var cell8 = sheet.Cells["A8"];
        //            cell8.Style.Font.Bold = true;
        //            sheet.Cells["B8"].Value = dsHtml.Tables[0].Rows[0]["InspectorName"].ToString();

        //            sheet.Cells["A9"].Value = "HR Employee Code:";
        //            var cell9 = sheet.Cells["A9"];
        //            cell9.Style.Font.Bold = true;
        //            sheet.Cells["B9"].Value = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();
        //            sheet.Cells["A10"].Value = "SAP EMp Code:";
        //            var cell10 = sheet.Cells["A10"];
        //            cell10.Style.Font.Bold = true;
        //            sheet.Cells["B10"].Value = dsHtml.Tables[2].Rows[0]["SAPEmpCode"].ToString();
        //            sheet.Cells["A11"].Value = "Cost Centre:";
        //            var cell11 = sheet.Cells["A11"];
        //            cell11.Style.Font.Bold = true;
        //            sheet.Cells["B11"].Value = dsHtml.Tables[2].Rows[0]["Cost_center"].ToString();
        //            sheet.Cells["E5"].Value = "printed on:";
        //            var cell12 = sheet.Cells["E5"];
        //            cell12.Style.Font.Bold = true;
        //            sheet.Cells["F5"].Value = DateTime.Now.ToShortDateString();
        //            sheet.Cells["A12"].Value = "Current Residing Address:";
        //            var cell13 = sheet.Cells["A12"];
        //            cell13.Style.Font.Bold = true;
        //            //sheet.Cells["B12"].Value = dsHtml.Tables[0].Rows[0]["Address1"].ToString();
        //            //sheet.Cells["B10"].Value = 

        //            sheet.Cells["A14"].Value = "Details:-";
        //            var cell14 = sheet.Cells["A14"];
        //            cell14.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell14.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        //            cell14.Style.Font.Size = 14; // Replace 12 with your desired font size
        //            cell14.Style.Font.Bold = true;

        //            sheet.Cells["A15"].Value = "Date";
        //            sheet.Cells["B15"].Value = "ActivityType";
        //            sheet.Cells["C15"].Value = "Job No";
        //            sheet.Cells["D15"].Value = "Sub Job No";
        //            sheet.Cells["E15"].Value = "SAP No";
        //            sheet.Cells["F15"].Value = "Project Name";
        //            sheet.Cells["G15"].Value = "Job Location";
        //            sheet.Cells["H15"].Value = "Customer Name";
        //            sheet.Cells["I15"].Value = "Total Time";
        //            sheet.Cells["J15"].Value = "Manday";
        //            sheet.Cells["K15"].Value = "OPE Claim";

        //            var range = sheet.Cells["A15:K15"];

        //            // Apply formatting to the range
        //            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        //            range.Style.Font.Size = 12;
        //            range.Style.Font.Bold = true;
        //            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Top.Color.SetColor(Color.Black);
        //            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Left.Color.SetColor(Color.Black);
        //            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Right.Color.SetColor(Color.Black);
        //            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            decimal RemainingTotal = 0;
        //            decimal OwnVehicalTotal = 0;
        //            int j = 0;
        //            for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
        //            {
        //                sheet.Cells[16 + i, 1].Value = dsHtml.Tables[0].Rows[i]["Date"].ToString();
        //                sheet.Cells[16 + i, 2].Value = dsHtml.Tables[0].Rows[i]["ActivityType"].ToString();
        //                sheet.Cells[16 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Job"].ToString();
        //                sheet.Cells[16 + i, 4].Value = dsHtml.Tables[0].Rows[i]["Sub_jOb"].ToString();
        //                sheet.Cells[16 + i, 5].Value = dsHtml.Tables[0].Rows[i]["SAP_No"].ToString();
        //                sheet.Cells[16 + i, 6].Value = dsHtml.Tables[0].Rows[i]["Project_Name"].ToString();
        //                sheet.Cells[16 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Job_Location"].ToString();
        //                sheet.Cells[16 + i, 8].Value = dsHtml.Tables[0].Rows[i]["Company_Name"].ToString();
        //                sheet.Cells[16 + i, 9].Value = dsHtml.Tables[0].Rows[i]["TotalTime"].ToString();
        //                sheet.Cells[16 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Manday"].ToString();
        //                sheet.Cells[16 + i, 11].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();

        //                RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());

        //                var range_ = sheet.Cells["A" + (16 + i) + ":K" + (16 + i)];
        //                range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Top.Color.SetColor(Color.Black);
        //                range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Left.Color.SetColor(Color.Black);
        //                range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Right.Color.SetColor(Color.Black);
        //                range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Bottom.Color.SetColor(Color.Black);



        //                j = 16 + i;
        //            }
        //            decimal _GrandTotal = RemainingTotal;

        //            sheet.Cells["J" + (j + 1)].Value = "Grand Total:";
        //            //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

        //            sheet.Cells["K" + (j + 1)].Value = _GrandTotal;
        //            //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


        //            var range1 = sheet.Cells["J" + (j + 1) + ":K" + (j + 1)];
        //            range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Top.Color.SetColor(Color.Black);
        //            range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Left.Color.SetColor(Color.Black);
        //            range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Right.Color.SetColor(Color.Black);
        //            range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            sheet.Cells.AutoFitColumns();

        //            // Convert the Excel package to a byte array
        //            byte[] excelBytes = excelPackage.GetAsByteArray();

        //            // Set the response content type and headers
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=" + OPE_Number.Replace(" - ", "_") + ".xlsx");

        //            // Write the Excel data to the response
        //            Response.BinaryWrite(excelBytes);
        //            Response.Flush();
        //            Response.End();
        //        }

        //        return Json("", JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    // return Json("", JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public JsonResult ExportToExcel1(string OPE_Number)
        {
            try
            {
                DataSet dsHtml = new DataSet();
                dsHtml = objDMOR.OPE_Data(OPE_Number);
                string htmlContent = "";
                string input = OPE_Number;
                string[] parts = input.Split('/');
                //string month = parts[1];
                string year = parts[2];


                using (var excelPackage = new ExcelPackage())
                {
                    // Create the worksheet
                    //nikita adding code for excel sheet 1  added by nikita on 27122023
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    sheet.Cells["A1"].Value = "OPE CLAIM";
                    var cell = sheet.Cells["A1"];
                    cell.Style.Font.Size = 14; // Replace 12 with your desired font size
                    cell.Style.Font.Bold = true;


                    sheet.Cells["A3"].Value = "Inspector Name:";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell3 = sheet.Cells["A3"];
                    sheet.Cells["B3"].Value = dsHtml.Tables[0].Rows[0]["InspectorName"].ToString();

                    //sheet.Cells["D3"].Value = month;
                    sheet.Cells["A5"].Value = "Designation:";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell5 = sheet.Cells["A5"];
                    //cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
                    //sheet.Cells["f3"].Value = "20" + year;
                    sheet.Cells["B5"].Value = dsHtml.Tables[0].Rows[0]["Designation"].ToString();

                    sheet.Cells["A7"].Value = "Branch Name";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell6 = sheet.Cells["A7"];
                    //cell6.Style.Font.Bold = true;
                    //sheet.Cells["H3"].Value = DateTime.Now.ToShortDateString();
                    sheet.Cells["B7"].Value = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();


                    sheet.Cells["A9"].Value = "HR Employee Code";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell2 = sheet.Cells["A9"];
                    //cell2.Style.Font.Size = 13; // Replace 12 with your desired font size
                    //cell2.Style.Font.Bold = true;
                    sheet.Cells["B9"].Value = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();

                    //sheet.Cells["B5"].Value = Session["VoucherNo"].ToString();
                    //sheet.Cells["B5"].Value = OPE_Number;
                    //sheet.Cells["A7"].Value = "Branch:";
                    //var cell7 = sheet.Cells["A7"];
                    ////cell7.Style.Font.Bold = true;
                    //sheet.Cells["B7"].Value = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();

                    sheet.Cells["A9"].Value = "HR Employee Code:";
                    var cell9 = sheet.Cells["A9"];
                    //sheet.Cells["B2:G2"].Merge = true;

                    //cell9.Style.Font.Bold = true;
                    sheet.Cells["B9"].Value = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();

                    sheet.Cells["A11"].Value = "SAP Employee Code";
                    var cell8 = sheet.Cells["A11"];
                    //sheet.Cells["B2:G2"].Merge = true;

                    //cell8.Style.Font.Bold = true;
                    sheet.Cells["B11"].Value = dsHtml.Tables[0].Rows[0]["SapEmpCode"].ToString();


                    sheet.Cells["A13"].Value = "Claim Period";
                    var cell10 = sheet.Cells["A13"];
                    //sheet.Cells["B2:G2"].Merge = true;
                    //cell10.Style.Font.Bold = true;
                    sheet.Cells["B13"].Value = dsHtml.Tables[1].Rows[0]["DateRange"].ToString(); //period

                    sheet.Cells["A15"].Value = "No. of days (OPE claim)";
                    var cell100 = sheet.Cells["A15"];
                    //sheet.Cells["B2:G2"].Merge = true;

                    //cell100.Style.Font.Bold = true;
                    sheet.Cells["B15"].Value = dsHtml.Tables[1].Rows[0]["Working"].ToString(); //period

                    sheet.Cells["A17"].Value = "Total Amount (in Rs)";
                    var cell11 = sheet.Cells["A17"];
                    //sheet.Cells["B17"].Value = ;
                    //sheet.Cells["B2:G2"].Merge = true;

                    sheet.Cells["B17"].Value = dsHtml.Tables[1].Rows[0]["OPE_Claim"].ToString(); //period


                    //cell11.Style.Font.Bold = true;
                    //sheet.Cells["A17"].Value = dsHtml.Tables[0].Rows[0][""].ToString(); //total amount column name

                    sheet.Cells["A19"].Value = "Approved by";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell111 = sheet.Cells["A19"];
                    //cell11.Style.Font.Bold = true;
                    //sheet.Cells["A19"].Value = dsHtml.Tables[0].Rows[0][""].ToString(); //Approved column name

                    sheet.Cells["A21"].Value = "Signature Of Manager";
                    //sheet.Cells["B2:G2"].Merge = true;

                    var cell111__ = sheet.Cells["21"];
                    //cell11.Style.Font.Bold = true;
                    //sheet.Cells["A19"].Value = dsHtml.Tables[0].Rows[0][""].ToString(); //Approved column name

                    sheet.Cells["A2"].Value = "";
                    //sheet.Cells["B2:G2"].Merge = true;

                    sheet.Cells["A28"].Value = "Signature Of Claimant";
                    var cellA = sheet.Cells["A28"];
                    //cellA.Style.Font.Bold = true;


                    int numRows = 19;
                    int numCols = 2;

                    // Define the range of cells
                    var range_1 = sheet.Cells[2, 1, 2 + numRows - 1, 1 + numCols - 1];


                    //int numRows = 19;
                    //int numCols = 2; // B column represents 2 columns

                    //// Assuming 'sheet' is your worksheet object
                    //int startRow = 2;
                    //int startCol = 1;

                    //// Define the range of cells
                    //var range_1 = sheet.Cells[startRow, startCol, startRow + numRows - 1, startCol + numCols - 1];
                    // Set border styles and colors for the entire range
                    range_1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Top.Color.SetColor(Color.Black);
                    range_1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Left.Color.SetColor(Color.Black);
                    range_1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Right.Color.SetColor(Color.Black);
                    range_1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Bottom.Color.SetColor(Color.Black);
                    range_1.Style.Font.Name = "Arial";
                    sheet.Cells.AutoFitColumns();
                    var sheet1 = excelPackage.Workbook.Worksheets.Add("Sheet2");
                    sheet1.Cells["A1"].Value = "TUV India Private Limited";
                    sheet1.Cells["A1:C1"].Merge = true;
                    sheet1.Cells["A1"].Style.WrapText = true;
                    var cellSS1 = sheet1.Cells["A1"];
                    //cellSS1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //cellSS1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    cellSS1.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cellSS1.Style.Font.Bold = true;
                    sheet1.Cells["D1"].Value = "Out of Pocket Expenses Report";
                    sheet1.Cells["D1:E1"].Merge = true;
                    sheet1.Cells["D1"].Style.WrapText = true;
                    var cell1_ = sheet1.Cells["D1"];
                    //cell1_.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //cell1_.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    cell1_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell1_.Style.Font.Bold = true;
                    sheet1.Cells["A2"].Value = "Number";
                    var cell2_ = sheet1.Cells["A2"];
                    cell2_.Style.Font.Size = 13; // Replace 12 with your desired font size
                    cell2_.Style.Font.Bold = true;
                    sheet1.Cells["B2"].Value = OPE_Number;

                    sheet1.Cells["A3"].Value = "Printed on:";
                    var cell6_ = sheet1.Cells["A3"];
                    cell6_.Style.Font.Bold = true;
                    //cell6_.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    sheet1.Cells["B3"].Value = DateTime.Now.ToShortDateString();

                    //sheet.Cells["D3"].Value = month;




                    sheet1.Cells["A4"].Value = "Year:";
                    var cell5_ = sheet1.Cells["A4"];
                    //cell5_.Style.Font.Bold = true;//Session["fullName"].ToString();
                    sheet1.Cells["B4"].Value = "20" + year;

                    sheet1.Cells["A5"].Value = "Period:";
                    var cell500_ = sheet1.Cells["A5"];
                    //cell500_.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //cell500_.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    sheet1.Cells["B5"].Value = dsHtml.Tables[1].Rows[0]["DateRange"].ToString();  //Period

                    //cell500_.Style.Font.Bold = true;//Session["fullName"].ToString();
                    //sheet.Cells["B5"].Value = dsHtml.Tables[0].Rows[0][""].ToString();  //Period

                    sheet1.Cells["A6"].Value = "Month:";
                    var cell5123_ = sheet1.Cells["A6"];
                    //cell5123_.Style.Fill.PatternType = ExcelFillStyle.Solid;

                    //cell5123_.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    //cell5123_.Style.Font.Bold = true;//Session["fullName"].ToString();
                    sheet1.Cells["B6"].Value = dsHtml.Tables[0].Rows[0]["MonthName"].ToString();  //month




                    sheet1.Cells["A7"].Value = "Branch:";
                    var cell7__ = sheet1.Cells["A7"];
                    //cell7__.Style.Font.Bold = true;
                    sheet1.Cells["B7"].Value = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();

                    sheet1.Cells["D2"].Value = "Name of Employee:";
                    var cell8__ = sheet1.Cells["D2"];
                    sheet1.Cells["E2"].Value = dsHtml.Tables[0].Rows[0]["InspectorName"].ToString();

                    sheet1.Cells["D3"].Value = "HR Employee Code:";
                    var cell9__ = sheet1.Cells["D3"];
                    //cell9.Style.Font.Bold = true;
                    sheet1.Cells["E3"].Value = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();

                    sheet1.Cells["D4"].Value = "SAP EMp Code:";
                    var cell10_ = sheet1.Cells["D4"];
                    //cell10.Style.Font.Bold = true;
                    sheet1.Cells["E4"].Value = dsHtml.Tables[0].Rows[0]["SapEmpCode"].ToString();

                    sheet1.Cells["D5"].Value = "Cost Centre:";
                    var cell11__ = sheet1.Cells["D5"];
                    //cell11__.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //cell11__.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    sheet1.Cells["E5"].Value = dsHtml.Tables[0].Rows[0]["Cost_Center"].ToString();

                    sheet1.Cells["D6"].Value = "Days:";
                    var cell123__ = sheet1.Cells["D6"];
                    //cell11.Style.Font.Bold = true;
                    sheet1.Cells["E6"].Value = dsHtml.Tables[1].Rows[0]["Working"].ToString(); //Days

                    sheet1.Cells["D7"].Value = "Amount:";
                    var cell1203__ = sheet1.Cells["D7"];
                    //cell11.Style.Font.Bold = true;
                    sheet1.Cells["E7"].Value = dsHtml.Tables[1].Rows[0]["OPE_Claim"].ToString(); //Amount

                    sheet1.Cells["A8"].Value = "Details:";
                    var cell14 = sheet1.Cells["A8"];
                    //cell14.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //cell14.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                    cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
                    //cell14.Style.Font.Bold = true;

                    sheet1.Cells["A9"].Value = "Date";
                    sheet1.Cells["B9"].Value = "ActivityType";
                    sheet1.Cells["C9"].Value = "Sub Job Number";
                    sheet1.Cells["D9"].Value = "SAP Number";
                    sheet1.Cells["E9"].Value = "Vendor Name";
                    sheet1.Cells["F9"].Value = "Job Loaction";
                    sheet1.Cells["G9"].Value = "Customer Name";
                    sheet1.Cells["H9"].Value = "On-site Time(Hrs.)";
                    sheet1.Cells["I9"].Value = "Amount (INR)";


                    var range = sheet1.Cells["A9:I9"];

                    // Apply formatting to the range
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range.Style.Font.Size = 12;
                    range.Style.Font.Bold = true;
                    //range.Style.wi = true;

                    range.Style.WrapText = true; // Set the "Wrap Text" property
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);

                    decimal RemainingTotal = 0;
                    decimal OwnVehicalTotal = 0;
                    int j = 0;
                    for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                    {
                        sheet1.Cells[10 + i, 1].Value = dsHtml.Tables[0].Rows[i]["Date"].ToString();
                        sheet1.Cells[10 + i, 2].Value = dsHtml.Tables[0].Rows[i]["ActivityType"].ToString();
                        sheet1.Cells[10 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Sub_jOb"].ToString();
                        sheet1.Cells[10 + i, 4].Value = dsHtml.Tables[0].Rows[i]["SAP_No"].ToString();
                        sheet1.Cells[10 + i, 5].Value = dsHtml.Tables[0].Rows[i]["Vendor_Name"].ToString();
                        sheet1.Cells[10 + i, 6].Value = dsHtml.Tables[0].Rows[i]["Job_Location"].ToString();
                        sheet1.Cells[10 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Company_Name"].ToString();
                        sheet1.Cells[10 + i, 8].Value = dsHtml.Tables[0].Rows[i]["StartTime"].ToString();
                        sheet1.Cells[10 + i, 9].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();
                        //sheet.Cells[10 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Manday"].ToString();
                        //sheet.Cells[10 + i, 11].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();

                        RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());

                        var range_ = sheet1.Cells["A" + (10 + i) + ":I" + (10 + i)];
                        range_.Style.WrapText = true; // Set the "Wrap Text" property
                        range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Top.Color.SetColor(Color.Black);
                        range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Left.Color.SetColor(Color.Black);
                        range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Right.Color.SetColor(Color.Black);
                        range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Bottom.Color.SetColor(Color.Black);



                        j = 10 + i;
                    }
                    decimal _GrandTotal = RemainingTotal;

                    sheet1.Cells["H" + (j + 1)].Value = "Grand Total:";
                    //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

                    sheet1.Cells["I" + (j + 1)].Value = _GrandTotal;
                    //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


                    var range1 = sheet1.Cells["H" + (j + 1) + ":I" + (j + 1)];
                    range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Top.Color.SetColor(Color.Black);
                    range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Left.Color.SetColor(Color.Black);
                    range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Right.Color.SetColor(Color.Black);
                    range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Bottom.Color.SetColor(Color.Black);

                    sheet1.Cells.AutoFitColumns();
                    sheet1.Protection.IsProtected = true;
                    sheet1.Protection.AllowSelectLockedCells = true; // Allow users to select locked cells

                    sheet.Protection.IsProtected = true;
                    sheet.Protection.AllowSelectLockedCells = true; // Allow users to select locked cells


                    // Convert the Excel package to a byte array
                    byte[] excelBytes_ = excelPackage.GetAsByteArray();

                    // Set the response content type and headers
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + OPE_Number.Replace(" - ", "_") + ".xlsx");

                    // Write the Excel data to the response
                    Response.BinaryWrite(excelBytes_);
                    Response.Flush();
                    Response.End();
                }

                return Json("", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw;
            }
            // return Json("", JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ExportToExcel1(string OPE_Number)
        //{
        //    try
        //    {
        //        DataSet dsHtml = new DataSet();
        //        dsHtml = objDMOR.OPE_Data(OPE_Number);
        //        string htmlContent = "";
        //        string input = OPE_Number;
        //        string[] parts = input.Split('-');
        //        string month = parts[1];
        //        string year = parts[2];


        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            // Create the worksheet
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
        //            sheet.Cells["A1"].Value = "TUV India Private Ltd.";
        //            var cell = sheet.Cells["A1"];
        //            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        //            cell.Style.Font.Size = 14; // Replace 12 with your desired font size
        //            cell.Style.Font.Bold = true;

        //            sheet.Cells["A3"].Value = "Out of Pocket Expenses Report:";
        //            var cell3 = sheet.Cells["A3"];
        //            // Replace 12 with your desired font size
        //            cell3.Style.Font.Bold = true;
        //            //sheet.Cells["B3"].Value = 
        //            sheet.Cells["C3"].Value = "Month:";
        //            var cell4 = sheet.Cells["C3"];
        //            cell4.Style.Font.Bold = true;

        //            sheet.Cells["D3"].Value = month;
        //            sheet.Cells["E3"].Value = "Year:";
        //            var cell5 = sheet.Cells["E3"];
        //            cell5.Style.Font.Bold = true;//Session["fullName"].ToString();
        //            sheet.Cells["f3"].Value = "20" + year;

        //            sheet.Cells["G3"].Value = "Submission Date:";
        //            var cell6 = sheet.Cells["G3"];
        //            cell6.Style.Font.Bold = true;
        //            sheet.Cells["H3"].Value = DateTime.Now.ToShortDateString();


        //            sheet.Cells["A5"].Value = "Out of Pocket Voucher Number:";
        //            var cell2 = sheet.Cells["A5"];
        //            cell2.Style.Font.Size = 13; // Replace 12 with your desired font size
        //            cell2.Style.Font.Bold = true;

        //            //sheet.Cells["B5"].Value = Session["VoucherNo"].ToString();
        //            sheet.Cells["B5"].Value = OPE_Number;
        //            sheet.Cells["A7"].Value = "Branch:";
        //            var cell7 = sheet.Cells["A7"];
        //            cell7.Style.Font.Bold = true;
        //            sheet.Cells["B7"].Value = dsHtml.Tables[2].Rows[0]["Branch"].ToString();

        //            sheet.Cells["A8"].Value = "Name of Employee:";
        //            var cell8 = sheet.Cells["A8"];
        //            cell8.Style.Font.Bold = true;
        //            sheet.Cells["B8"].Value = dsHtml.Tables[2].Rows[0]["EmployeeName"].ToString();

        //            sheet.Cells["A9"].Value = "HR Employee Code:";
        //            var cell9 = sheet.Cells["A9"];
        //            cell9.Style.Font.Bold = true;
        //            sheet.Cells["B9"].Value = dsHtml.Tables[2].Rows[0]["EmployeeCode"].ToString();
        //            sheet.Cells["A10"].Value = "SAP EMp Code:";
        //            var cell10 = sheet.Cells["A10"];
        //            cell10.Style.Font.Bold = true;
        //            sheet.Cells["B10"].Value = dsHtml.Tables[2].Rows[0]["SAPEmpCode"].ToString();
        //            sheet.Cells["A11"].Value = "Cost Centre:";
        //            var cell11 = sheet.Cells["A11"];
        //            cell11.Style.Font.Bold = true;
        //            sheet.Cells["B11"].Value = dsHtml.Tables[2].Rows[0]["Cost_center"].ToString();
        //            sheet.Cells["E5"].Value = "printed on:";
        //            var cell12 = sheet.Cells["E5"];
        //            cell12.Style.Font.Bold = true;
        //            sheet.Cells["F5"].Value = DateTime.Now.ToShortDateString();
        //            sheet.Cells["A12"].Value = "Current Residing Address:";
        //            var cell13 = sheet.Cells["A12"];
        //            cell13.Style.Font.Bold = true;
        //            sheet.Cells["B12"].Value = dsHtml.Tables[2].Rows[0]["Address1"].ToString();
        //            //sheet.Cells["B10"].Value = 

        //            sheet.Cells["A14"].Value = "Details:-";
        //            var cell14 = sheet.Cells["A14"];
        //            cell14.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell14.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        //            cell14.Style.Font.Size = 14; // Replace 12 with your desired font size
        //            cell14.Style.Font.Bold = true;

        //            sheet.Cells["A15"].Value = "Date";
        //            sheet.Cells["B15"].Value = "ActivityType";
        //            sheet.Cells["C15"].Value = "Job No";
        //            sheet.Cells["D15"].Value = "Sub Job No";
        //            sheet.Cells["E15"].Value = "SAP No";
        //            sheet.Cells["F15"].Value = "Project Name";
        //            sheet.Cells["G15"].Value = "Job Location";
        //            sheet.Cells["H15"].Value = "Customer Name";
        //            sheet.Cells["I15"].Value = "Total Time";
        //            sheet.Cells["J15"].Value = "Manday";
        //            sheet.Cells["K15"].Value = "OPE Claim";

        //            var range = sheet.Cells["A15:K15"];

        //            // Apply formatting to the range
        //            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        //            range.Style.Font.Size = 12;
        //            range.Style.Font.Bold = true;
        //            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Top.Color.SetColor(Color.Black);
        //            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Left.Color.SetColor(Color.Black);
        //            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Right.Color.SetColor(Color.Black);
        //            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            decimal RemainingTotal = 0;
        //            decimal OwnVehicalTotal = 0;
        //            int j = 0;
        //            for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
        //            {
        //                sheet.Cells[16 + i, 1].Value = dsHtml.Tables[0].Rows[i]["Date"].ToString();
        //                sheet.Cells[16 + i, 2].Value = dsHtml.Tables[0].Rows[i]["ActivityType"].ToString();
        //                sheet.Cells[16 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Job"].ToString();
        //                sheet.Cells[16 + i, 4].Value = dsHtml.Tables[0].Rows[i]["Sub_jOb"].ToString();
        //                sheet.Cells[16 + i, 5].Value = dsHtml.Tables[0].Rows[i]["SAP_No"].ToString();
        //                sheet.Cells[16 + i, 6].Value = dsHtml.Tables[0].Rows[i]["Project_Name"].ToString();
        //                sheet.Cells[16 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Job_Location"].ToString();
        //                sheet.Cells[16 + i, 8].Value = dsHtml.Tables[0].Rows[i]["Company_Name"].ToString();
        //                sheet.Cells[16 + i, 9].Value = dsHtml.Tables[0].Rows[i]["TotalTime"].ToString();
        //                sheet.Cells[16 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Manday"].ToString();
        //                sheet.Cells[16 + i, 11].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();

        //                RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());

        //                var range_ = sheet.Cells["A" + (16 + i) + ":K" + (16 + i)];
        //                range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Top.Color.SetColor(Color.Black);
        //                range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Left.Color.SetColor(Color.Black);
        //                range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Right.Color.SetColor(Color.Black);
        //                range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Bottom.Color.SetColor(Color.Black);



        //                j = 16 + i;
        //            }
        //            decimal _GrandTotal = RemainingTotal;

        //            sheet.Cells["J" + (j + 1)].Value = "Grand Total:";
        //            //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

        //            sheet.Cells["K" + (j + 1)].Value = _GrandTotal;
        //            //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


        //            var range1 = sheet.Cells["J" + (j + 1) + ":K" + (j + 1)];
        //            range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Top.Color.SetColor(Color.Black);
        //            range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Left.Color.SetColor(Color.Black);
        //            range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Right.Color.SetColor(Color.Black);
        //            range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            //var range2 = sheet.Cells["F" + (j + 2) + ":G" + (j + 2)];
        //            //range2.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //range2.Style.Border.Top.Color.SetColor(Color.Black);
        //            //range2.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //range2.Style.Border.Left.Color.SetColor(Color.Black);
        //            //range2.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            //range2.Style.Border.Right.Color.SetColor(Color.Black);
        //            //range2.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //range2.Style.Border.Bottom.Color.SetColor(Color.Black);


        //            //sheet.Cells["A" + (j + 4)].Value = "Type";
        //            //sheet.Cells["B" + (j + 4)].Value = "Cost Center";
        //            //sheet.Cells["C" + (j + 4)].Value = "Job Number";
        //            //sheet.Cells["D" + (j + 4)].Value = "SAP No";
        //            //sheet.Cells["E" + (j + 4)].Value = "Customer Name";
        //            //sheet.Cells["F" + (j + 4)].Value = "Amount In INR";

        //            //var _range = sheet.Cells["A" + (j + 4) + ":F" + (j + 4)];

        //            //// Apply formatting to the range
        //            //_range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            //_range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        //            //_range.Style.Font.Size = 12;
        //            //_range.Style.Font.Bold = true;
        //            //_range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //_range.Style.Border.Top.Color.SetColor(Color.Black);
        //            //_range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //_range.Style.Border.Left.Color.SetColor(Color.Black);
        //            //_range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            //_range.Style.Border.Right.Color.SetColor(Color.Black);
        //            //_range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //_range.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            //decimal GrandTotal = 0;
        //            //int k = 0;
        //            //int l = j + 5;
        //            //for (int i = 0; i < dsHtml.Tables[1].Rows.Count; i++)
        //            //{
        //            //    sheet.Cells[l + i, 1].Value = dsHtml.Tables[1].Rows[i]["Type"].ToString();
        //            //    sheet.Cells[l + i, 2].Value = dsHtml.Tables[1].Rows[i]["Cost_center"].ToString();
        //            //    sheet.Cells[l + i, 3].Value = dsHtml.Tables[1].Rows[i]["SubJob_No"].ToString();
        //            //    sheet.Cells[l + i, 4].Value = dsHtml.Tables[1].Rows[i]["SAP_No"].ToString();
        //            //    sheet.Cells[l + i, 5].Value = dsHtml.Tables[1].Rows[i]["Company_Name"].ToString();
        //            //    sheet.Cells[l + i, 6].Value = dsHtml.Tables[1].Rows[i]["TotalAmount"].ToString();

        //            //    var range_ = sheet.Cells["A" + (l + i) + ":F" + (l + i)];
        //            //    range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //    range_.Style.Border.Top.Color.SetColor(Color.Black);
        //            //    range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //    range_.Style.Border.Left.Color.SetColor(Color.Black);
        //            //    range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            //    range_.Style.Border.Right.Color.SetColor(Color.Black);
        //            //    range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //    range_.Style.Border.Bottom.Color.SetColor(Color.Black);
        //            //    k = l + i;
        //            //    GrandTotal = GrandTotal + Convert.ToDecimal(dsHtml.Tables[1].Rows[i]["TotalAmount"].ToString());
        //            //}
        //            //sheet.Cells["E" + (k + 1)].Value = "Amount In INR";
        //            //sheet.Cells["F" + (k + 1)].Value = GrandTotal;

        //            //var rangeTotal = sheet.Cells["E" + (k + 1) + ":F" + (k + 1)];
        //            //rangeTotal.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            //rangeTotal.Style.Border.Top.Color.SetColor(Color.Black);
        //            //rangeTotal.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            //rangeTotal.Style.Border.Left.Color.SetColor(Color.Black);
        //            //rangeTotal.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            //rangeTotal.Style.Border.Right.Color.SetColor(Color.Black);
        //            //rangeTotal.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            //rangeTotal.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            sheet.Cells.AutoFitColumns();

        //            // Convert the Excel package to a byte array
        //            byte[] excelBytes = excelPackage.GetAsByteArray();

        //            // Set the response content type and headers
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=" + OPE_Number.Replace(" - ", "_") + ".xlsx");

        //            // Write the Excel data to the response
        //            Response.BinaryWrite(excelBytes);
        //            Response.Flush();
        //            Response.End();
        //        }

        //        return Json("", JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    // return Json("", JsonRequestBehavior.AllowGet);
        //}


        #region export Attendance to excel 

        [HttpGet]
        public ActionResult GenerateAttExport(NonInspectionActivity c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;

                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;

                int WOLeaveCount = 0;
                int PHLeaveCount = 0;
                int WKDLeaveCount = 0;
                int NACount = 0;
                int SelectDays = 0;
                int WorkingDays = 0;
                double filledPercentage = 0;
                string DayName = string.Empty;
                string strCheck = string.Empty;

                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = CreateAttExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.ColumnName.ToString();
                    sheet.Column(col++).Width = 18;
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPGRADE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NOA COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LPG"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLING"

                        )
                        {
                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString().Contains("CL"))
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("SL"))
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("PL") || grid.Rows[Nrow][Ncol].ToString().Contains("A33"))
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString().Contains("48"))
                            {
                                NACount++;
                               
                            }

                            nullcount++;
                        }
                    }

                    col = 1;
                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {
                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPGRADE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "LPG"

                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains("PL") || grid.Rows[Nrow][Ncol1].ToString().Contains("CL") || grid.Rows[Nrow][Ncol1].ToString().Contains("SL") || grid.Rows[Nrow][Ncol1].ToString().Contains("A33"))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                if (grid.Rows[Nrow][Ncol1].ToString().Contains("A33"))
                                {
                                    sheet.Cells[row, col].Value = "AUTO PL";
                                }
                                else
                                {
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                                }


                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                sheet.Cells[row, col].Value = "WO";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                                sheet.Cells[row, col].Value = "PH";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains("48"))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                                sheet.Cells[row, col].Value = "NoA";
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                                sheet.Cells[row, col].Value = "P";
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "NOA COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                sheet.Cells[row, col].Value = NACount.ToString();
                            }

                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLING")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                SelectDays = SelectDays = Convert.ToInt32(grid.Rows[Nrow]["SELECTED PERIOD DAYS"].ToString());


                                if (SelectDays == nullcount && WorkingDays < nullcount)
                                {
                                    strCheck = "Correct";
                                }
                                else
                                {
                                    strCheck = "Incorrect";
                                }
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = strCheck.ToString();


                            }
                            else
                            {
                                if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                                    sheet.Cells[row, col].Value = string.Empty;
                                else
                                    sheet.Cells[row, col].Value = "P"; // grid.Rows[Nrow][Ncol1].ToString();
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }
                    row++;
                    nullcount = 0;
                    LeaveCount = 0;
                    OTHLeaveCount = 0;

                    CLeaveCount = 0;
                    SLeaveCount = 0;
                    PLeaveCount = 0;

                    WOLeaveCount = 0;
                    PHLeaveCount = 0;
                    WKDLeaveCount = 0;
                    NACount = 0;
                    SelectDays = 0;

                    filledPercentage = 0;
                    WorkingDays = 0;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }

        }

        
        #endregion


        //added By shrutika salve on 23/06/2023
        //new model Call_History

        public ActionResult CallHistory()
        {

            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();
            //DTComplaintDashBoard = Dalobj.GetReportsDashBoard();


            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {

                obj1.FromDate = Convert.ToString(TempData["FromDate"]);
                obj1.ToDate = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                DTComplaintDashBoard = Dalobj.GetReports(obj1);

            }
            else
            {

                DTComplaintDashBoard = Dalobj.GetReportsDashBoard();
            }
            string showdata = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                pk_ivr_id = Convert.ToInt32(dr["PK_IVR_ID"]),
                                Iv_pk_call_id = Convert.ToInt32(dr["Iv_pk_call_id"]),
                                Call_No = Convert.ToString(dr["CALL_NO"]),
                                SubSubJobNo = Convert.ToString(dr["SUB_JOB"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                End_Customer = Convert.ToString(dr["End_Customer"]),
                                Project_Name = Convert.ToString(dr["project_Name"]),
                                TopSubVendorName = Convert.ToString(dr["vendor_name"]),
                                TopSubVendorPONo = Convert.ToString(dr["Vendor_Po_No"]),
                                VendorName = Convert.ToString(dr["TopVendor"]),
                                VendorPONo = Convert.ToString(dr["TopVendorPO"]),
                                DocumentRelatedToInspectionCall = Convert.ToString(dr["DocumentRelatedToInspectionCall"]),
                                checkIFCustomerSpecific1 = Convert.ToString(dr["checkIFcustomerSpecific"]),
                                chkARC1 = Convert.ToString(dr["chkARC"]),
                                IVRConclusion = Convert.ToString(dr["IVRConclusion"]),
                                IRNConclusion = Convert.ToString(dr["IRNConclusion"]),
                                CreatedIVrInTiimes = Convert.ToString(dr["CreatedIVrInTiimes"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                Job_Location = Convert.ToString(dr["Inspection_Location"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                DECName = Convert.ToString(dr["DECName"]),
                                DECNumber = Convert.ToString(dr["DECNumber"]),
                                CreatedBy = Convert.ToString(dr["CoordinatorName"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                Branch_Name = Convert.ToString(dr["BRANCH_NAME"]),
                                Excuting_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["InspectorName"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Call_Recived_date = Convert.ToString(dr["CALL_RECIVED_DATE"]),
                                //CALLRECEIVETIME = Convert.ToString(dr["CallReceiveTime"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                Actual_Visit_Date = Convert.ToString(dr["ACTUAL_VISIT_DATE"]),
                                EXECUTIONDELAYDAY = Convert.ToString(dr["EXECUTIONDELAYDAY"]),
                                EXECUTISTATUS = Convert.ToString(dr["ExecutionStatus"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                Status = Convert.ToString(dr["Status"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                CallClosureDate = Convert.ToString(dr["CallClosureDate"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastDateOgInspection"]),
                                IsVisitReportGenerated = Convert.ToString(dr["IsVisitReportGenerated"]),
                                IVR = Convert.ToString(dr["IVR"]),
                                IVRReportNo = Convert.ToString(dr["IVRReportNo"]),
                                IVRCreateDate = Convert.ToString(dr["IVRCreateDate"]),
                                IVRModifiedDate = Convert.ToString(dr["IVRModifiedDate"]),
                                IVRDownloadDateTime = Convert.ToString(dr["IVRDownloadDate"]),
                                IVRDownloadModifiedDate = Convert.ToString(dr["IVRDownloadModifiedDate"]),
                                IVRDistributionDelay = Convert.ToString(dr["IVRDISTRIBUTIONDELAY"]),
                                IRN = Convert.ToString(dr["IRN"]),
                                IRNReportNo = Convert.ToString(dr["IRNReportNo"]),
                                IRNCreateDate = Convert.ToString(dr["IRNCreateDate"]),
                                IRNModifiedDate = Convert.ToString(dr["IRNModifiedDate"]),
                                IRNDownloadDatetime = Convert.ToString(dr["IRNDownloadDate"]),
                                IRNDownloadModifiedDate = Convert.ToString(dr["IRNDownloadModifiedDate"]),
                                IRNDistributionDelay = Convert.ToString(dr["IRNDistributionDelay"]),
                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["List"] = lstComplaintDashBoard;
            obj1.lstDashBoard = lstComplaintDashBoard;


            return View(obj1);

        }



        [HttpPost]
        public ActionResult CallHistory(CallsModel Lm)
        {
            List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
            DataSet ds = new DataSet();


            TempData["FromDate"] = Lm.FromDate;
            TempData["ToDate"] = Lm.ToDate;
            TempData.Keep();


            return RedirectToAction("CallHistory", "MISOPEReport");
            //return Json(obj1.JsonRequestBehavior.AllowGet);

            return View(obj1);


        }



        //Excel Upload Data//


        public ActionResult ExportIndex2(CallsModel U)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGrid(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                using (ExcelRange range = sheet.Cells["A1:BB1"])
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

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Call_History" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
        }
        private IGrid<CallsModel> CreateExportableGrid(CallsModel U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetData2(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.SubSubJobNo).Titled("sub-sub job Number");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.DocumentRelatedToInspectionCall).Titled("Document Related To Inspection Call");
            grid.Columns.Add(model => model.Company_Name).Titled("Company Name");
            grid.Columns.Add(model => model.End_Customer).Titled("End Customer");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.TopSubVendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.TopSubVendorPONo).Titled("Po Number");
            grid.Columns.Add(model => model.VendorName).Titled("sub Vendor Name");
            grid.Columns.Add(model => model.VendorPONo).Titled("sub Po Number");
            grid.Columns.Add(model => model.checkIFCustomerSpecific1).Titled("check IF Customer Specific");
            grid.Columns.Add(model => model.chkARC1).Titled("chkARC");//DocumentRelatedToInspectionCall
            grid.Columns.Add(model => model.ItemsToBeInpsected).Titled("Items To Be Inpsected");
            grid.Columns.Add(model => model.StageOfInspection).Titled("Stage Of Inspection");
            grid.Columns.Add(model => model.Job_Location).Titled("Job Location");
            grid.Columns.Add(model => model.Urgency).Titled("Urgency");
            grid.Columns.Add(model => model.DECName).Titled("DECName");
            grid.Columns.Add(model => model.DECNumber).Titled("DECNumber");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.PresentDay).Titled("PresentDay");
            grid.Columns.Add(model => model.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(model => model.Excuting_Branch).Titled("Excuting Branch");
            grid.Columns.Add(model => model.VendorName).Titled("Vendor Name");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("Call Recived date");
            grid.Columns.Add(model => model.Call_Recived_date).Titled("CALL RECEIVE TIME");
            grid.Columns.Add(model => model.RequstedInspectedDate).Titled("Call Request Date");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.EXECUTIONDELAYDAY).Titled("EXECUTION DELAY DAY");
            grid.Columns.Add(model => model.EXECUTISTATUS).Titled("EXECUTI STATUS");
            grid.Columns.Add(model => model.ExtendCall_Status).Titled("Extend Call Status");
            grid.Columns.Add(model => model.Call_Type).Titled("Call_ Type");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.Reasion).Titled("Reasion");
            grid.Columns.Add(model => model.CallClosureDate).Titled("CallClosureDate");
            grid.Columns.Add(model => model.LastInspectorName_DateOfInspection).Titled("lastDateOfInspection");
            grid.Columns.Add(model => model.IsVisitReportGenerated).Titled("IsVisitReportGenerated");
            grid.Columns.Add(model => model.IVR).Titled("IVR");
            grid.Columns.Add(model => model.IVRReportNo).Titled("IVRReportNo");
            grid.Columns.Add(model => model.IVRCreateDate).Titled("IVR Create Date");
            grid.Columns.Add(model => model.IVRModifiedDate).Titled("IVR Modified Date");
            grid.Columns.Add(model => model.IVRDownloadDateTime).Titled("IVR Download");
            grid.Columns.Add(model => model.IVRDownloadModifiedDate).Titled("IVR Modified Download");
            grid.Columns.Add(model => model.IVRDistributionDelay).Titled("IVR Distribution Delay");
            grid.Columns.Add(model => model.IVRConclusion).Titled("IVR Conclusion");
            grid.Columns.Add(model => model.IRN).Titled("IRN");
            grid.Columns.Add(model => model.IRNReportNo).Titled("IRNReportNo");
            grid.Columns.Add(model => model.IRNCreateDate).Titled("IRNCreateDate");
            grid.Columns.Add(model => model.IRNModifiedDate).Titled("IRNModifiedDate");
            grid.Columns.Add(model => model.IRNDownloadDatetime).Titled("IRNDownloadDatetime");
            grid.Columns.Add(model => model.IRNDownloadModifiedDate).Titled("IRNDownloadModifiedDate");
            grid.Columns.Add(model => model.IRNDistributionDelay).Titled("IRN Distribution Delay");
            grid.Columns.Add(model => model.IRNConclusion).Titled("IRN Conclusion");// IVRConclusion
            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj1.lstDashBoard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<CallsModel> GetData2(CallsModel U)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<CallsModel> lstCallDashBoard = new List<CallsModel>();

            if (TempData["FromDate"] != null && TempData["ToDate"] != null)
            {

                obj1.FromDate = Convert.ToString(TempData["FromDate"]);
                obj1.ToDate = Convert.ToString(TempData["ToDate"]);
                TempData.Keep();
                DTCallDashBoard = Dalobj.GetReports(obj1);

            }
            else
            {

                DTCallDashBoard = Dalobj.GetReportsDashBoard();
            }

            
            //DTCallDashBoard = objDalCreateUser.GetDetails();
           // DTCallDashBoard = Dalobj.GetReportsDashBoard();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new CallsModel
                            {

                                PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                                pk_ivr_id = Convert.ToInt32(dr["PK_IVR_ID"]),
                                Iv_pk_call_id = Convert.ToInt32(dr["Iv_pk_call_id"]),
                                Call_No = Convert.ToString(dr["CALL_NO"]),
                                SubSubJobNo = Convert.ToString(dr["SUB_JOB"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                End_Customer = Convert.ToString(dr["End_Customer"]),
                                Project_Name = Convert.ToString(dr["project_Name"]),
                                TopSubVendorName = Convert.ToString(dr["vendor_name"]),
                                TopSubVendorPONo = Convert.ToString(dr["Vendor_Po_No"]),
                                VendorName = Convert.ToString(dr["TopVendor"]),
                                VendorPONo = Convert.ToString(dr["TopVendorPO"]),
                                checkIFCustomerSpecific1 = Convert.ToString(dr["checkIFcustomerSpecific"]),
                                DocumentRelatedToInspectionCall = Convert.ToString(dr["DocumentRelatedToInspectionCall"]),
                                chkARC1 = Convert.ToString(dr["chkARC"]),
                                ItemsToBeInpsected = Convert.ToString(dr["Itemtobeinspected"]),
                                StageOfInspection = Convert.ToString(dr["stageInspection"]),
                                Job_Location = Convert.ToString(dr["Inspection_Location"]),
                                Urgency = Convert.ToString(dr["Urgency"]),
                                DECName = Convert.ToString(dr["DECName"]),
                                DECNumber = Convert.ToString(dr["DECNumber"]),
                                CreatedBy = Convert.ToString(dr["CoordinatorName"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                Branch_Name = Convert.ToString(dr["BRANCH_NAME"]),
                                Excuting_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["InspectorName"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                Call_Recived_date = Convert.ToString(dr["CALL_RECIVED_DATE"]),
                                //CALLRECEIVETIME = Convert.ToString(dr["CallReceiveTime"]),
                                Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                                Actual_Visit_Date = Convert.ToString(dr["ACTUAL_VISIT_DATE"]),
                                EXECUTIONDELAYDAY = Convert.ToString(dr["EXECUTIONDELAYDAY"]),
                                EXECUTISTATUS = Convert.ToString(dr["ExecutionStatus"]),
                                ExtendCall_Status = Convert.ToString(dr["ExtendCall_Status"]),
                                Call_Type = Convert.ToString(dr["Call_Type"]),
                                Status = Convert.ToString(dr["Status"]),
                                Reasion = Convert.ToString(dr["Reasion"]),
                                IVRConclusion = Convert.ToString(dr["IVRConclusion"]),
                                IRNConclusion = Convert.ToString(dr["IRNConclusion"]),
                                CallClosureDate = Convert.ToString(dr["CallClosureDate"]),
                                LastInspectorName_DateOfInspection = Convert.ToString(dr["LastDateOgInspection"]),
                                IsVisitReportGenerated = Convert.ToString(dr["IsVisitReportGenerated"]),
                                IVR = Convert.ToString(dr["IVR"]),
                                IVRReportNo = Convert.ToString(dr["IVRReportNo"]),
                                IVRCreateDate = Convert.ToString(dr["IVRCreateDate"]),
                                IVRModifiedDate = Convert.ToString(dr["IVRModifiedDate"]),
                                IVRDownloadDateTime = Convert.ToString(dr["IVRDownloadDate"]),
                                IVRDownloadModifiedDate = Convert.ToString(dr["IVRDownloadModifiedDate"]),
                                IVRDistributionDelay = Convert.ToString(dr["IVRDISTRIBUTIONDELAY"]),
                                IRN = Convert.ToString(dr["IRN"]),
                                IRNReportNo = Convert.ToString(dr["IRNReportNo"]),
                                IRNCreateDate = Convert.ToString(dr["IRNCreateDate"]),
                                IRNModifiedDate = Convert.ToString(dr["IRNModifiedDate"]),
                                IRNDownloadDatetime = Convert.ToString(dr["IRNDownloadDate"]),
                                IRNDownloadModifiedDate = Convert.ToString(dr["IRNDownloadModifiedDate"]),
                                IRNDistributionDelay = Convert.ToString(dr["IRNDistributionDelay"]),




                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["Data"] = lstCallDashBoard;

            obj1.lstDashBoard = lstCallDashBoard;


            return obj1.lstDashBoard;
        }





        public ActionResult CallNumberHistory(int PK_Call_ID)
        {
            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();
            DTComplaintDashBoard = Dalobj.GetCallDetailsData(PK_Call_ID);
            string showdata = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {
                                PK_Call_ID = Convert.ToInt32(dr["Pk_call_id"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifiedDate"]),
                                ModifyBy = Convert.ToString(dr["ModifiedBy"]),
                                Description = Convert.ToString(dr["description"]),
                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["List1"] = lstComplaintDashBoard;
            obj1.lstDashBoard1 = lstComplaintDashBoard;

            return View(obj1);
            //return Json(obj1, JsonRequestBehavior.AllowGet);

        }


        public ActionResult IVRNoHistory(int? Pk_Ivr_id)
        {

            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();
            DTComplaintDashBoard = Dalobj.GetIVRDetailsData(Convert.ToInt32(Pk_Ivr_id));


            string showdata = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {
                                pk_ivr_id = Convert.ToInt32(dr["Pk_ivr_Id"]),
                                Report_No = Convert.ToString(dr["Report"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifiedDate"]),
                                ModifyBy = Convert.ToString(dr["ModifiedBy"]),
                                Description = Convert.ToString(dr["description"]),
                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["List1"] = lstComplaintDashBoard;
            obj1.lstDashBoard1 = lstComplaintDashBoard;

            return View(obj1);
            //return Json(obj1, JsonRequestBehavior.AllowGet);

        }

        public ActionResult IRNNoHistory(int? Pk_Ivr_idID)
        {
            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();
            DTComplaintDashBoard = Dalobj.GetIRNDetailsData(Convert.ToInt32(Pk_Ivr_idID));


            string showdata = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {
                                pk_ivr_id = Convert.ToInt32(dr["Pk_ivr_id"]),
                                Report_No = Convert.ToString(dr["Report"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifiedDate"]),
                                ModifyBy = Convert.ToString(dr["ModifiedBy"]),
                                Description = Convert.ToString(dr["description"]),
                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["List1"] = lstComplaintDashBoard;
            obj1.lstDashBoard1 = lstComplaintDashBoard;

            return View(obj1);
            //return Json(obj1, JsonRequestBehavior.AllowGet);

        }



        //////Call Excecution status added by shrutika salve 06/07/2023
        ////public ActionResult CallExecutionStatus()
        ////{
        ////    DataSet dsBindBranch = new DataSet();
        ////    CallsModel tc = new CallsModel();
        ////    List<BranchName> lstBranch = new List<BranchName>();
        ////    dsBindBranch = Dalobj.BindBranch();

        ////    if (dsBindBranch.Tables[0].Rows.Count > 0)
        ////    {
        ////        lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
        ////                     select new BranchName()
        ////                     {
        ////                         Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
        ////                         Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

        ////                     }).ToList();
        ////    }



        ////    IEnumerable<SelectListItem> BranchItems;
        ////    BranchItems = new SelectList(lstBranch, "Code", "Name");

        ////    ViewBag.ProjectTypeItems = BranchItems;
        ////    ViewData["BranchName"] = BranchItems;




        ////    DataSet DTUserDashBoard1 = new DataSet();
        ////    List<UNameCode> lstUserList = new List<UNameCode>();
        ////    DTUserDashBoard1 = Dalobj.GetUserList();

        ////    if (DTUserDashBoard1.Tables[0].Rows.Count > 0)//All Items to be Inspected
        ////    {
        ////        lstUserList = (from n in DTUserDashBoard1.Tables[0].AsEnumerable()
        ////                       select new UNameCode()
        ////                       {
        ////                           Name = n.Field<string>(DTUserDashBoard1.Tables[0].Columns["FullName"].ToString()),
        ////                           Code = n.Field<string>(DTUserDashBoard1.Tables[0].Columns["PK_UserID"].ToString())
        ////                       }).ToList();
        ////    }

        ////    IEnumerable<SelectListItem> Items;
        ////    Items = new SelectList(lstUserList, "Code", "Name");
        ////    ViewBag.Employees = Items;
        ////    ViewData["Employees"] = Items;
        ////    //get inspector name role 
        ////    DataSet DTUserDashBoard = new DataSet();
        ////    List<UNameCode> lstUser = new List<UNameCode>();
        ////    DTUserDashBoard = Dalobj.GetinspectionList();

        ////    if (DTUserDashBoard.Tables[0].Rows.Count > 0)//All Items to be Inspected
        ////    {
        ////        lstUser = (from n in DTUserDashBoard.Tables[0].AsEnumerable()
        ////                   select new UNameCode()
        ////                   {
        ////                       Name = n.Field<string>(DTUserDashBoard.Tables[0].Columns["FullName"].ToString()),
        ////                       Code = n.Field<string>(DTUserDashBoard.Tables[0].Columns["PK_UserID"].ToString())
        ////                   }).ToList();
        ////    }

        ////    IEnumerable<SelectListItem> Items1;
        ////    Items1 = new SelectList(lstUserList, "Code", "Name");
        ////    ViewBag.Emp = Items1;
        ////    ViewData["Emp"] = Items1;


        ////    DataSet BranchName = new DataSet();
        ////    string UserId = Session["UserID"].ToString();
        ////    BranchName = Dalobj.BranchName(UserId);

        ////    if (BranchName.Tables.Count > 0 && BranchName.Tables[0].Rows.Count > 0)
        ////    {
        ////        // Access the value from the first row and 'BranchName' column
        ////        string branchName = BranchName.Tables[0].Rows[0]["Br_Id"].ToString();

        ////        // Store the value in the 'branchName' variable
        ////        obj1.Excuting_Branch = branchName;
        ////    }

        ////    CallsModel call = new CallsModel();

        ////    call.FromDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
        ////    call.ToDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");


        ////    obj1.FromDate = call.FromDate; // Assigning the value to another variable
        ////    obj1.ToDate = call.ToDate;
        ////    obj1.OriginatingBranch = call.OriginatingBranch;
        ////    obj1.CoordinatorName = call.CoordinatorName;
        ////    obj1.Inspector = call.Inspector;

        ////    DataSet DSJobMasterByQtId = new DataSet();
        ////    DSJobMasterByQtId = Dalobj.GetDataList(obj1.FromDate, obj1.ToDate, obj1.Excuting_Branch, obj1.OriginatingBranch, obj1.CoordinatorName, obj1.Inspector, obj1.Status);
        ////    if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
        ////    {

        ////        obj1.TotalCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["total_call"]);
        ////        obj1.OpenCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OpenCount"]);
        ////        obj1.AssignedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["AssignedCount"]);
        ////        obj1.NotDoneCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["NotDoneCount"]);
        ////        obj1.CancelledCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["CancelledCount"]);
        ////        obj1.ClosedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ClosedCount"]);
        ////    }



        ////    return View(obj1);
        ////}

        ////[HttpPost]
        ////public ActionResult CallDetails(CallsModel CM)
        ////{
        ////    List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
        ////    DataSet ds = new DataSet();

        ////    //ds = DALobj.GetDataByDate(LD); // fill dataset  


        ////    //Session["FromDate"] = LD.FromD;
        ////    //Session["ToDate"] = LD.ToD;

        ////    TempData["FromDate"] = CM.FromDate;
        ////    TempData["ToDate"] = CM.ToDate;
        ////    TempData["ExcecutionBranch"] = CM.Excuting_Branch;
        ////    TempData.Keep();


        ////    return RedirectToAction("CallExecutionStatus");
        ////    return View(obj1);




        ////}


        //Call Excecution status added by shrutika salve 06/07/2023
        public ActionResult CallExecutionStatus()
        {
            DataSet dsBindBranch = new DataSet();
            CallsModel tc = new CallsModel();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = Dalobj.BindBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

                             }).ToList();
            }



            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");

            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;




            DataSet DTUserDashBoard1 = new DataSet();
            List<UNameCode> lstUserList = new List<UNameCode>();
            DTUserDashBoard1 = Dalobj.GetUserList();

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
            //get inspector name role 
            DataSet DTUserDashBoard = new DataSet();
            List<UNameCode> lstUser = new List<UNameCode>();
            DTUserDashBoard = Dalobj.GetinspectionList();

            if (DTUserDashBoard.Tables[0].Rows.Count > 0)//All Items to be Inspected
            {
                lstUser = (from n in DTUserDashBoard.Tables[0].AsEnumerable()
                           select new UNameCode()
                           {
                               Name = n.Field<string>(DTUserDashBoard.Tables[0].Columns["FullName"].ToString()),
                               Code = n.Field<string>(DTUserDashBoard.Tables[0].Columns["PK_UserID"].ToString())
                           }).ToList();
            }

            IEnumerable<SelectListItem> Items1;
            Items1 = new SelectList(lstUserList, "Code", "Name");
            ViewBag.Emp = Items1;
            ViewData["Emp"] = Items1;


            DataSet BranchName = new DataSet();
            string UserId = Session["UserID"].ToString();
            BranchName = Dalobj.BranchName(UserId);

            if (BranchName.Tables.Count > 0 && BranchName.Tables[0].Rows.Count > 0)
            {
                // Access the value from the first row and 'BranchName' column
                string branchName = BranchName.Tables[0].Rows[0]["Br_Id"].ToString();

                // Store the value in the 'branchName' variable
                obj1.Excuting_Branch = branchName;
            }
            DataSet DSJobMasterByQtId = new DataSet();
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();
            if (Session["FromDate"] != null && Session["ToDate"] != null && Session["ExcecutionBranch"] != null || Session["OriginatingBranch"] != null || Session["CoordinatorName"] != null || Session["Inspector"] != null)
            {
                obj1.FromDate = Session["FromDate"].ToString(); // Assigning the value to another variable
                obj1.ToDate = Session["ToDate"].ToString();
                //obj1.OriginatingBranch = Session["OriginatingBranch"].ToString();
                if (Session["OriginatingBranch"] != null)
                {
                    obj1.OriginatingBranch = Session["OriginatingBranch"].ToString();
                    
                    
                }
                else
                {
                    obj1.OriginatingBranch = string.Empty;
                }
                if(Session["CoordinatorName"] != null)
                {
                    obj1.CoordinatorName = Session["CoordinatorName"].ToString();
                }else
                {
                    obj1.CoordinatorName = string.Empty;
                }
                if(Session["Inspector"] != null)
                {
                    obj1.Inspector = Session["Inspector"].ToString();
                }

                else
                {
                    obj1.Inspector = string.Empty;
                }



                DSJobMasterByQtId = Dalobj.GetDataList(obj1.FromDate, obj1.ToDate, obj1.Excuting_Branch, obj1.OriginatingBranch, obj1.CoordinatorName, obj1.Inspector, obj1.Status);
                ds = Dalobj.GetDatacallsearch(obj1.FromDate, obj1.ToDate, obj1.Excuting_Branch, obj1.OriginatingBranch, obj1.CoordinatorName, obj1.Inspector, obj1.Status);
            }
            else
            {
                CallsModel call = new CallsModel();

                call.FromDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                call.ToDate = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
                obj1.FromDate = call.FromDate; // Assigning the value to another variable
                obj1.ToDate = call.ToDate;
                obj1.OriginatingBranch = call.OriginatingBranch;
                obj1.CoordinatorName = call.CoordinatorName;
                obj1.Inspector = call.Inspector;
                DSJobMasterByQtId = Dalobj.GetDataList(obj1.FromDate, obj1.ToDate, obj1.Excuting_Branch, obj1.OriginatingBranch, obj1.CoordinatorName, obj1.Inspector, obj1.Status);
                ds = Dalobj.GetDatacallsearch(obj1.FromDate, obj1.ToDate, obj1.Excuting_Branch, obj1.OriginatingBranch, obj1.CoordinatorName, obj1.Inspector, obj1.Status);
            }

            if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
            {

                obj1.TotalCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["total_call"]);
                obj1.OpenCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OpenCount"]);
                obj1.AssignedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["AssignedCount"]);
                obj1.NotDoneCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["NotDoneCount"]);
                obj1.CancelledCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["CancelledCount"]);
                obj1.ClosedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ClosedCount"]);
            }





            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lmd.Add(new CallsModel
                    {
                        PK_Call_ID = Convert.ToInt32(dr["pk_call_id"]),
                        Call_No = Convert.ToString(dr["Call_No"]),
                        Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                        Status = Convert.ToString(dr["Status"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        VendorName = Convert.ToString(dr["Vendor_Name"]),
                        Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        CoordinatorName = Convert.ToString(dr["CreatedBy"]),
                        Inspector = Convert.ToString(dr["Inspector"]),
                        InspectorMobile = Convert.ToString(dr["inspectorMobileNo"]),
                        IVRReportNo = Convert.ToString(dr["Report"]),
                    });
                }
            }


            ViewData["callData"] = lmd;
            obj1.lstDashBoard1 = lmd;

            return View(obj1);
        }

        [HttpPost]
        public ActionResult CallExecutionStatus(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            //ds = DALobj.GetDataByDate(LD); // fill dataset  


            //Session["FromDate"] = LD.FromD;
            //Session["ToDate"] = LD.ToD;

            Session["FromDate"] = CM.FromDate;
            Session["ToDate"] = CM.ToDate;
            Session["ExcecutionBranch"] = CM.Excuting_Branch;
            Session["OriginatingBranch"] = CM.OriginatingBranch;
            Session["CoordinatorName"] = CM.CoordinatorName;
            Session["Inspector"] = CM.Inspector;


            return RedirectToAction("CallExecutionStatus");
            return View(obj1);




        }



        [HttpPost]
        public JsonResult GetdataCopy(string FromDate, string Todate, string Excuting_Branch, string OriginatingBranch, string CoordinatorName, string inspectorname, string status)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = Todate;
            Session["Executing_Branch"] = Excuting_Branch;
            try
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DSJobMasterByQtId = Dalobj.GetDataList(FromDate, Todate, Excuting_Branch, OriginatingBranch, CoordinatorName, inspectorname, status);
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {

                    obj1.TotalCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["total_call"]);
                    obj1.OpenCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["OpenCount"]);
                    obj1.AssignedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["AssignedCount"]);
                    obj1.NotDoneCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["NotDoneCount"]);
                    obj1.CancelledCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["CancelledCount"]);
                    obj1.ClosedCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["ClosedCount"]);
                }


            }

            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            //return Json(obj1,JsonRequestBehavior.AllowGet);
            return Json(obj1, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public ActionResult Getdatacallsearch1(string fromdate, string todate, string Excuting_Branch, string OriginatingBranch, string CoordinatorName, string inspectorname, string status)
        {
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();

            ds = Dalobj.GetDatacallsearch(fromdate, todate, Excuting_Branch, OriginatingBranch, CoordinatorName, inspectorname, status);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lmd.Add(new CallsModel
                    {
                        PK_Call_ID = Convert.ToInt32(dr["pk_call_id"]),
                        Call_No = Convert.ToString(dr["Call_No"]),
                        Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                        Status = Convert.ToString(dr["Status"]),
                        Company_Name = Convert.ToString(dr["Company_Name"]),
                        VendorName = Convert.ToString(dr["Vendor_Name"]),
                        Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                        Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                        CoordinatorName = Convert.ToString(dr["CreatedBy"]),
                        Inspector = Convert.ToString(dr["Inspector"]),
                        InspectorMobile = Convert.ToString(dr["inspectorMobileNo"]),
                        IVRReportNo = Convert.ToString(dr["Report"]),
                    });
                }
            }

            return Json(lmd, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult OnchageMethod(string BranchId1)
        {
            DataSet data = new DataSet();
            data = Dalobj.Branch(BranchId1);
            string jsonString = "";
            if (data != null)
            {
                jsonString = JsonConvert.SerializeObject(data.Tables[0]);
            }


            return Json(jsonString, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Onchageinspector(string BranchId)
        {
            DataSet data = new DataSet();
            data = Dalobj.inspectorName(BranchId);
            string jsonString = "";
            if (data != null)
            {
                jsonString = JsonConvert.SerializeObject(data.Tables[0]);
            }


            return Json(jsonString, JsonRequestBehavior.AllowGet);

        }

        //invoicing Data added shrutika salve 14/07/2023
        public ActionResult invoicingData()
        {
            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            //get inspector name role 
            DataSet DTUserDashBoard = new DataSet();
            List<UNameCode> lstUser = new List<UNameCode>();
            DTUserDashBoard = Dalobj.GetinspectionList();

            if (DTUserDashBoard.Tables[0].Rows.Count > 0)//All Items to be Inspected
            {
                lstUser = (from n in DTUserDashBoard.Tables[0].AsEnumerable()
                           select new UNameCode()
                           {
                               Name = n.Field<string>(DTUserDashBoard.Tables[0].Columns["FullName"].ToString()),
                               Code = n.Field<string>(DTUserDashBoard.Tables[0].Columns["PK_UserID"].ToString())
                           }).ToList();
            }

            IEnumerable<SelectListItem> Items1;
            Items1 = new SelectList(lstUser, "Code", "Name");
            ViewBag.Emp = Items1;
            ViewData["Emp"] = Items1;


            if (Session["PO_Number"] != null & Session["po_Date"] != null)
            {
                obj1.PO_Number = Convert.ToString(Session["PO_Number"]);
                obj1.po_Date = Convert.ToString(Session["po_Date"]);
            }

            DataTable Invoice = new DataTable();
            if (Session["FromDate1"] != null && Session["ToDate1"] != null || Session["Job1"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate1"].ToString());
                obj1.ToDateI = Convert.ToString(Session["ToDate1"].ToString());
                obj1.Job = Convert.ToString(Session["Job1"]?.ToString()) ?? "";
                //obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                obj1.Inspector = Convert.ToString(Session["inpector"]?.ToString()) ?? "";

                DTComplaintDashBoard = Dalobj.InvoiceData(obj1);

            }
            else
            {

            }

            string showData = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {

                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {


                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {

                                Actual_Visit_Date = Convert.ToString(dr["VisitDate"]),
                                Call_No = Convert.ToString(dr["Call_no"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Job = Convert.ToString(dr["job"]),
                                ExecutingBranch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["inspectorName"]),
                                Onsite = Convert.ToString(dr["StartTime"]),
                                Offsite = Convert.ToString(dr["EndTime"]),
                                TravelTime = Convert.ToString(dr["TravelTime"]),
                                IVR = Convert.ToString(dr["Report"]),
                                //insopectionRecord = Convert.ToString(dr["FileName1"]),
                                Status = Convert.ToString(dr["Status"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                VendorName = Convert.ToString(dr["subvendor"]),
                                TopSubVendorName = Convert.ToString(dr["subsubvendor"]),
                                VendorPONo = Convert.ToString(dr["subVendorPO"]),
                                TopSubVendorPONo = Convert.ToString(dr["subsubVendor_Po_No"]),
                                MandayRate = Convert.ToString(dr["MandayRate"]),
                                RefDocument = Convert.ToString(dr["RefDocument"]),
                                Attachment = Convert.ToString(dr["VAttachment"]),
                                insopectionRecord = Convert.ToString(dr["DetailsDocument"]),
                                //added by shrutika salve 14062024
                                CustomerRepresentative = Convert.ToString(dr["CustomerRepresentativeName"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),


                            });
                    }
                    ViewData["List"] = lstComplaintDashBoard;
                    obj1.lstComplaintDashBoard1 = lstComplaintDashBoard;

                }

                else
                {
                    ViewData["List"] = Session["lstComplaintDashBoard"];
                    obj1.lstComplaintDashBoard1 = lstComplaintDashBoard;

                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            return View(obj1);

        }



        [HttpPost]
        public ActionResult invoicingData(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            //ds = DALobj.GetDataByDate(LD); // fill dataset  


            //Session["FromDate"] = LD.FromD;
            //Session["ToDate"] = LD.ToD;

            Session["FromDate1"] = CM.FromDateI;
            Session["ToDate1"] = CM.ToDateI;
            Session["Job1"] = CM.Job;
            Session["inpector"] = CM.Inspector;



            return RedirectToAction("invoicingData");


            return View();
        }



        public ActionResult ExportIndex1(CallsModel U)
        {
            // Using EPPlus from nuget
            string visitreportLink = string.Empty;
            string AttachmentLink = string.Empty;
            string FinalAttch = string.Empty;
            string PreviousText = string.Empty;
            string PreviousReport = string.Empty;
            bool rowincremented = false;
            string FinalURLAttachmentLink = string.Empty;



            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGrid1(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                // sheet = package.Workbook.Worksheets.Add("Sheet1");
                //sheet.Cells["A1"].Value = "TUV India Private Ltd.";
                //var cell = sheet.Cells["A1"];
                //cell.Style.Fill.PatternType = ExcelFillStyle.Solid;


                //sheet.Cells["A2"].Value = "Period:";
                //var cell3 = sheet.Cells["A3"];
                //// Replace 12 with your desired font size
                //cell3.Style.Font.Bold = true;
                //sheet.Cells["B2"].Value = Session["FromDate1"].ToString();
                //sheet.Cells["C3"].Value = Session["ToDate1"].ToString();

                string StyleName = "HyperStyle";

                ExcelNamedStyleXml HyperStyle = sheet.Workbook.Styles.CreateNamedStyle(StyleName);
                HyperStyle.Style.Font.UnderLine = true;
                HyperStyle.Style.Font.Size = 12;
                HyperStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);





                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 20;
                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    rowincremented = false;
                    foreach (IGridColumn column in grid.Columns)
                    {
                        if (column.Title.ToString().ToUpper() == "VISIT REPORT")
                        {
                            visitreportLink = "https://tiimes.tuv-india.com/IVRReport/" + column.ValueFor(gridRow).ToString();


                            ExcelRange Rng = sheet.Cells[row, col++];

                            Rng.Hyperlink = new Uri(visitreportLink, UriKind.Absolute);
                            Rng.Value = column.ValueFor(gridRow).ToString();
                            Rng.StyleName = StyleName;
                            PreviousReport = visitreportLink;



                        }
                        else if (column.Title.ToString().ToUpper() == "INSOPECTION RECORD")
                        {
                            if (column.ValueFor(gridRow).ToString() != string.Empty)
                            {
                                string text = column.ValueFor(gridRow).ToString();

                                string[] words = text.Split('!');

                                int prevRow = row;
                                int newRow = row;

                                FinalAttch = string.Empty;
                                sheet.Cells.Style.WrapText = true;
                                sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                                sheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                                foreach (var word in words)
                                {
                                    ExcelRange Rng = sheet.Cells[row, 23];

                                    Rng.StyleName = StyleName;
                                    string[] FileDetails = word.Split('|');

                                    // Split the value to get the filename and the part before the first underscore
                                    string filename = FileDetails[0].Trim();
                                    int underscoreIndex = filename.IndexOf('_');
                                    string fileId = FileDetails[2].Trim();
                                    //string fileId = filename.Substring(0, underscoreIndex);


                                    AttachmentLink = "https://tiimes.tuv-india.com/VisitReport/Download1?d=" + fileId;

                                    Rng.Hyperlink = new Uri(AttachmentLink, UriKind.Absolute);
                                    Rng.Value = filename;
                                    row++;
                                    rowincremented = true;
                                }
                            }
                        }

                        
                        else
                        {
                            sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
                        }
                    }
                    if (!rowincremented)
                    {
                        row++;
                    }


                }

                return File(package.GetAsByteArray(), "application/unknown", "InvoiceData_" + DateTime.Now.ToShortDateString() + ".xlsx");

            }
        }

        //public ActionResult ExportIndex1(CallsModel U)
        //{

        //    string visitreportLink = string.Empty;
        //    string AttachmentLink = string.Empty;
        //    string FinalAttch = string.Empty;
        //    string PreviousText = string.Empty;
        //    string PreviousReport = string.Empty;
        //    bool rowincremented = false;
        //    string FinalURLAttachmentLink = string.Empty;
        //    // Using EPPlus from nuget
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        Int32 row = 2;
        //        Int32 col = 1;

        //        package.Workbook.Worksheets.Add("Data");
        //        IGrid<CallsModel> grid = CreateExportableGrid1(U);
        //        ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //        string StyleName = "HyperStyle";

        //        ExcelNamedStyleXml HyperStyle = sheet.Workbook.Styles.CreateNamedStyle(StyleName);
        //        HyperStyle.Style.Font.UnderLine = true;
        //        HyperStyle.Style.Font.Size = 12;
        //        HyperStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);


        //        foreach (IGridColumn column in grid.Columns)
        //        {
        //            sheet.Cells[1, col].Value = column.Title;
        //            sheet.Column(col++).Width = 18;
        //            column.IsEncoded = false;
        //        }


        //        foreach (IGridRow<CallsModel> gridRow in grid.Rows)
        //        {
        //            col = 1;
        //            rowincremented = false;
        //            foreach (IGridColumn column in grid.Columns)
        //            {
        //                if (column.Title.ToString().ToUpper() == "insopectionRecord")
        //                {
        //                    visitreportLink = "https://tiimes.tuv-india.com/IVRReport/" + column.ValueFor(gridRow).ToString();

        //                    if (PreviousReport != visitreportLink)
        //                    {
        //                        ExcelRange Rng = sheet.Cells[row, col++];

        //                        Rng.Hyperlink = new Uri(visitreportLink, UriKind.Absolute);
        //                        Rng.Value = column.ValueFor(gridRow).ToString();
        //                        Rng.StyleName = StyleName;
        //                        PreviousReport = visitreportLink;
        //                    }

        //                }
        //                else if (column.Title.ToString().ToUpper() == "IVR")
        //                {
        //                    if (column.ValueFor(gridRow).ToString() != string.Empty)
        //                    {
        //                        string text = column.ValueFor(gridRow).ToString();

        //                        string[] words = text.Split('!');

        //                        int prevRow = row;
        //                        int newRow = row;

        //                        FinalAttch = string.Empty;
        //                        sheet.Cells.Style.WrapText = true;
        //                        sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        //                        sheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


        //                        foreach (var word in words)
        //                        {

        //                            ExcelRange Rng = sheet.Cells[row, 31];

        //                            Rng.StyleName = StyleName;
        //                            string[] FileDetails = word.Split('|');
        //                            AttachmentLink = "https://tiimes.tuv-india.com/VisitReport/Download1?d=" + FileDetails[2].ToString().Trim();
        //                            Rng.Hyperlink = new Uri(AttachmentLink, UriKind.Absolute);
        //                            Rng.Value = FileDetails[0].ToString().Trim();
        //                            row++;
        //                            rowincremented = true;
        //                        }

        //                        if (words.Length > 1)
        //                        {
        //                            for (int colno = 1; colno < col; colno++)
        //                            {
        //                                //// sheet.MergedCells = [cnt, colno];
        //                                sheet.Cells[prevRow, colno, row - 1, colno].Merge = true;
        //                            }
        //                        }


        //                    }
        //                }
        //                else
        //                {
        //                    sheet.Cells[row, col++].Value = column.ValueFor(gridRow);
        //                }
        //            }
        //            if (!rowincremented)
        //            {
        //                row++;
        //            }


        //        }



        //        return File(package.GetAsByteArray(), "application/unknown", "InvoiceData" + DateTime.Now.ToShortDateString() + ".xlsx");
        //    }
        //}


        private IGrid<CallsModel> CreateExportableGrid1(CallsModel U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetData(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Job).Titled("Job Number");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub job Number");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Number");
            grid.Columns.Add(model => model.VendorName).Titled("sub Vendor Name");
            grid.Columns.Add(model => model.TopSubVendorName).Titled("sub/sub Vendor Name");
            grid.Columns.Add(model => model.VendorPONo).Titled("sub Vendor PO Number");
            grid.Columns.Add(model => model.TopSubVendorPONo).Titled("sub/sub Vendor PO Number");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.SAPEmpCode).Titled("SAP Employee Code");
            grid.Columns.Add(model => model.TUVEmpCode).Titled("TUV EMployee Code");
            grid.Columns.Add(model => model.ExecutingBranch).Titled("Executing Branch");
            grid.Columns.Add(model => model.MandayRate).Titled("Manday Rate in INR");
            grid.Columns.Add(model => model.Onsite).Titled("Onsite Time");
            grid.Columns.Add(model => model.Offsite).Titled("Offsite Time");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.Status).Titled("call status");
            grid.Columns.Add(model => model.PresentDay).Titled("Full/Half Day");
            grid.Columns.Add(model => model.IVR).Titled("Visit Report");
            grid.Columns.Add(model => model.insopectionRecord).Titled("insopection Record");




            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj1.lstComplaintDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<CallsModel> GetData(CallsModel U)
        {

            DataTable DTCallDashBoard = new DataTable();
            List<CallsModel> lstCallDashBoard = new List<CallsModel>();
            if (Session["FromDate1"] != null && Session["ToDate1"] != null || Session["Job1"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate1"].ToString());
                obj1.ToDateI = Convert.ToString(Session["ToDate1"].ToString());
                obj1.Job = Convert.ToString(Session["Job1"]?.ToString()) ?? "";
                //obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                obj1.Inspector = Convert.ToString(Session["inpector"]?.ToString()) ?? "";

                DTCallDashBoard = Dalobj.InvoiceDataExcel(obj1);
            }

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new CallsModel
                            {


                                FromDateI = Convert.ToString(dr["FromDate"]),
                                ToDateI = Convert.ToString(dr["ToDate"]),
                                Job = Convert.ToString(dr["job"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["VisitDate"]),
                                Call_No = Convert.ToString(dr["Call_no"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                //VendorName = Convert.ToString(dr["Vendor_Name"]),
                                ExecutingBranch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["inspectorName"]),
                                Onsite = Convert.ToString(dr["StartTime"]),
                                Offsite = Convert.ToString(dr["EndTime"]),
                                TravelTime = Convert.ToString(dr["TravelTime"]),
                                IVR = Convert.ToString(dr["Report"]),
                                insopectionRecord = Convert.ToString(dr["VAttachment"]),
                                Status = Convert.ToString(dr["Status"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                VendorName = Convert.ToString(dr["subvendor"]),
                                TopSubVendorName = Convert.ToString(dr["subsubvendor"]),
                                VendorPONo = Convert.ToString(dr["subVendorPO"]),
                                TopSubVendorPONo = Convert.ToString(dr["subsubVendor_Po_No"]),
                                MandayRate = Convert.ToString(dr["MandayRate"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),



                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["List"] = lstCallDashBoard;

            obj1.lstComplaintDashBoard1 = lstCallDashBoard;


            return obj1.lstComplaintDashBoard1;
        }


        [HttpPost]
        public JsonResult Getdata(string Job)
        {
            try
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DSJobMasterByQtId = Dalobj.GetReportList(Job);
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {
                    obj1.PO_Number = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_PoNo_PoDate"]);

                    obj1.po_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_PoDate"]);

                }
                Session["PO_Number"] = obj1.PO_Number;
                Session["po_Date"] = obj1.po_Date;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json(obj1, JsonRequestBehavior.AllowGet);
        }

        //Added Call analysis for Originating Branch  27072023
        public ActionResult OriginatingBranchSummary()
        {

            DataTable OriginatingBranchSummary = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            if (Session["From"] != null && Session["To"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["From"].ToString());
                obj1.ToDateI = Convert.ToString(Session["To"].ToString());
                OriginatingBranchSummary = Dalobj.OriginatingBranchSummary(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (OriginatingBranchSummary.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in OriginatingBranchSummary.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            Branch_Name = Convert.ToString(dr["Branch_Name"]),
                            TotalCalls = Convert.ToString(dr["TotalCalls"]),
                            Ontime = Convert.ToString(dr["OnTimeCount"]),
                            OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
                            DelayCount = Convert.ToString(dr["DelayCount"]),
                            DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
                            NoactionbyCoordinators = Convert.ToString(dr["NoactionbyCoordinators"]),
                            NoactionbyCoordinatorsPercentage = Convert.ToString(dr["NoactionPercentageCoordinators"]),
                            NoactionbyInspectors = Convert.ToString(dr["NoactionbyInspectors"]),
                            NoactionbyInspectorsPercentage = Convert.ToString(dr["NoactionPercentageinspector"]),
                            WrongReceiveDateCount = Convert.ToString(dr["WrongReceiveDateCount"]),
                            WrongReceiveDateCountPercentage = Convert.ToString(dr["WrongReceiveDatePercentage"]),
                            WrongRequestDateCount = Convert.ToString(dr["WrongRequestDateCount"]),
                            WrongRequestDateCountPercentage = Convert.ToString(dr["WrongRequestDatePercentage"]),
                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;


                    CallsModel grandTotal = new CallsModel();
                    grandTotal.Branch_Name = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
                    //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard.Count;
                    grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
                    grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalNoactionbyCoordinators = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyCoordinators));
                    grandTotal.GrandTotalNoactionbyCoordinatorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyCoordinatorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalNoactionbyInspectors = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyInspectors));
                    grandTotal.GrandTotalNoactionbyInspectorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyInspectorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalWrongReceiveDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongReceiveDateCount));
                    grandTotal.GrandTotalWrongReceiveDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongReceiveDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalWrongRequestDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongRequestDateCount));
                    grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;

                    ViewData["GrandTotal"] = grandTotal;

                    CallsModel viewModel = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    return View(viewModel);
                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };



                    return View(obj1);
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View();
        }



        [HttpPost]
        public ActionResult OriginatingBranchSummary(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();


            Session["From"] = CM.FromDate;
            Session["To"] = CM.ToDate;




            return RedirectToAction("OriginatingBranchSummary");


            return View();
        }

        //Added Call analysis for executing Branch  28072023
        public ActionResult ExecutingBranchSummary()
        {

            DataTable executingBranchSummary = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            if (Session["From2"] != null && Session["To2"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["From2"].ToString());
                obj1.ToDateI = Convert.ToString(Session["To2"].ToString());
                executingBranchSummary = Dalobj.ExecutingBranchSummary(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {


                if (executingBranchSummary.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in executingBranchSummary.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            Branch_Name = Convert.ToString(dr["Branch_Name"]),
                            TotalCalls = Convert.ToString(dr["executedCalls"]),
                            Own = Convert.ToString(dr["OwnBranchCount"]),
                            OwnPercentage = Convert.ToString(dr["OwnBranchPercentage"]),
                            Other = Convert.ToString(dr["OtherBranchCount"]),
                            OtherPercentage = Convert.ToString(dr["OtherBranchPercentage"]),
                            Ontime = Convert.ToString(dr["OnTimeCount"]),
                            OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
                            OntimeOwnCalls = Convert.ToString(dr["OnTimeOwnCalls"]),
                            OntimeOwnCallsPercentage = Convert.ToString(dr["OnTimeOwnPercentage"]),
                            OntimeOtherCalls = Convert.ToString(dr["OnTimeOtherBranchesCall"]),
                            OntimeOtherCallsPercentage = Convert.ToString(dr["OnTimeOtherBranchesPercentage"]),
                            DelayCount = Convert.ToString(dr["DelayCount"]),
                            DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
                            DelayedOwnCalls = Convert.ToString(dr["OwnBranchDelay"]),
                            DelayedOwnCallsPercentage = Convert.ToString(dr["OwnBranchDelayPercentage"]),
                            DelayedOtherCalls = Convert.ToString(dr["OtherBranchDelay"]),
                            DelayedOtherCallsPercentage = Convert.ToString(dr["OtherBranchDelayPercentage"]),
                            //added by shrutika salve 05/12/2023
                            competantinspectorassigned = Convert.ToString(dr["competantinspectorassigned"]),
                            competantinspectorassignedPercentage = Convert.ToString(dr["competantinspectorassignedPercentage"]),
                            inspectorassigned = Convert.ToString(dr["Inspectorassigcount"]),
                            inspectorassignedPercentage = Convert.ToString(dr["InspectorassigcountPercentage"]),
                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;


                    CallsModel grandTotal = new CallsModel();
                    grandTotal.Branch_Name = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandOwn = lstComplaintDashBoard1.Sum(item => int.Parse(item.Own));
                    //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard.Count;
                    // grandTotal.GrandOwnPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OwnPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.OwnPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandOwnPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OwnPercentage)).Select(item => float.Parse(item.OwnPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandOther = lstComplaintDashBoard1.Sum(item => int.Parse(item.Other));
                    //grandTotal.GrandOtherPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OtherPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.OtherPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandOtherPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OtherPercentage)).Select(item => float.Parse(item.OtherPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
                    // grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandOntimeOwnCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.OntimeOwnCalls));
                    // grandTotal.GrandOntimeOwnCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OntimeOwnCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.OntimeOwnCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandOntimeOwnCallsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OntimeOwnCallsPercentage)).Select(item => float.Parse(item.OntimeOwnCallsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandOntimeOtherCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.OntimeOtherCalls));
                    //grandTotal.GrandOntimeOtherCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OntimeOtherCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.OntimeOtherCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandOntimeOtherCallsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OntimeOtherCallsPercentage)).Select(item => float.Parse(item.OntimeOtherCallsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
                    //grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayCountPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandDelayedOwnCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayedOwnCalls));
                    //grandTotal.GrandDelayedOwnCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOwnCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOwnCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandDelayedOwnCallsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayedOwnCallsPercentage)).Select(item => float.Parse(item.DelayedOwnCallsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandDelayedOtherCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayedOtherCalls));

                    //grandTotal.GrandDelayedOtherCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOtherCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOtherCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandDelayedOtherCallsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayedOtherCallsPercentage)).Select(item => float.Parse(item.DelayedOtherCallsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                                                                                                                                                                                                                                                                             //added by shrutika salve 05122023
                    grandTotal.GrandTotalcompetantinspectorassigned = lstComplaintDashBoard1.Sum(item => int.Parse(item.competantinspectorassigned));
                    //grandTotal.GrandDelayedOwnCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOwnCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOwnCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalcompetantinspectorassignedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.competantinspectorassignedPercentage)).Select(item => float.Parse(item.competantinspectorassignedPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalinspectorassigned = lstComplaintDashBoard1.Sum(item => int.Parse(item.inspectorassigned));

                    //grandTotal.GrandDelayedOtherCallsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOtherCallsPercentage)) == 0 ? 0 : lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayedOtherCallsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalinspectorassignedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.inspectorassignedPercentage)).Select(item => float.Parse(item.inspectorassignedPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    ViewData["GrandTotal"] = grandTotal;

                    CallsModel viewModel = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };




                    return View(viewModel);
                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };



                    return View(obj1);
                }


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View();
        }



        [HttpPost]
        public ActionResult ExecutingBranchSummary(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();


            Session["From2"] = CM.FromDate;
            Session["To2"] = CM.ToDate;




            return RedirectToAction("ExecutingBranchSummary");


            return View();
        }


        ////added by shrutika salve 30/07/2023    
        //public ActionResult CoordinatorIndividualPerformance()
        //{

        //    DataTable CoordinatorIndividual = new DataTable();
        //    List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


        //    if (Session["From3"] != null && Session["To3"] != null)
        //    {

        //        obj1.FromDateI = Convert.ToString(Session["From3"].ToString());
        //        obj1.ToDateI = Convert.ToString(Session["To3"].ToString());
        //        CoordinatorIndividual = Dalobj.CoordinatorIndividual(obj1);
        //    }
        //    else
        //    {

        //    }

        //    string showData = string.Empty;
        //    try
        //    {

        //        if (CoordinatorIndividual.Rows.Count > 0)
        //        {
        //            List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

        //            foreach (DataRow dr in CoordinatorIndividual.Rows)
        //            {
        //                lstComplaintDashBoard1.Add(new CallsModel
        //                {
        //                    CoordinatorName = Convert.ToString(dr["FullName"]),
        //                    Branch_Name = Convert.ToString(dr["BranchName"]),
        //                    TotalCalls = Convert.ToString(dr["Totalcalls"]),
        //                    Ontime = Convert.ToString(dr["OnTimeCount"]),
        //                    OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
        //                    DelayCount = Convert.ToString(dr["DelayCount"]),
        //                    DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
        //                    NoactionbyCoordinators = Convert.ToString(dr["NoactionbyCoordinators"]),
        //                    NoactionbyCoordinatorsPercentage = Convert.ToString(dr["NoactionPercentageCoordinators"]),
        //                    NoactionbyInspectors = Convert.ToString(dr["NoactionbyInspectors"]),
        //                    NoactionbyInspectorsPercentage = Convert.ToString(dr["NoactionPercentageinspector"]),
        //                    WrongReceiveDateCount = Convert.ToString(dr["WrongReceiveDateCount"]),
        //                    WrongReceiveDateCountPercentage = Convert.ToString(dr["WrongReceiveDatePercentage"]),
        //                    WrongRequestDateCount = Convert.ToString(dr["WrongRequestDateCount"]),
        //                    WrongRequestDateCountPercentage = Convert.ToString(dr["WrongRequestDatePercentage"]),
        //                    Performance = Convert.ToString(dr["Performance"]),
        //                    PerformanceCountPercentage = Convert.ToString(dr["Performance1"]),

        //                });
        //            }

        //            ViewData["ListData"] = lstComplaintDashBoard1;


        //            CallsModel grandTotal = new CallsModel();
        //            grandTotal.CoordinatorName = "Grand Total";
        //            grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
        //            grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
        //            //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard.Count;
        //            //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
        //            //grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalNoactionbyCoordinators = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyCoordinators));
        //            // grandTotal.GrandTotalNoactionbyCoordinatorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyCoordinatorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalNoactionbyCoordinatorsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.NoactionbyCoordinatorsPercentage)).Select(item => float.Parse(item.NoactionbyCoordinatorsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalNoactionbyInspectors = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyInspectors));
        //            // grandTotal.GrandTotalNoactionbyInspectorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyInspectorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalNoactionbyInspectorsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.NoactionbyInspectorsPercentage)).Select(item => float.Parse(item.NoactionbyInspectorsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalWrongReceiveDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongReceiveDateCount));
        //            //grandTotal.GrandTotalWrongReceiveDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongReceiveDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalWrongReceiveDateCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.WrongReceiveDateCountPercentage)).Select(item => float.Parse(item.WrongReceiveDateCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalWrongRequestDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongRequestDateCount));
        //            //grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
        //            grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.WrongRequestDateCountPercentage)).Select(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

        //            // Now grandTotal.GrandTotalWrongRequestDateCountPercentage contains the calculated average

        //            grandTotal.GrandTotalPerformance = lstComplaintDashBoard1.Sum(item => int.Parse(item.Performance));
        //            grandTotal.GrandTotalPerformancePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PerformanceCountPercentage)).Select(item => float.Parse(item.PerformanceCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average


        //            ViewData["GrandTotal"] = grandTotal;

        //            CallsModel viewModel = new CallsModel
        //            {
        //                lstComplaintDashBoard1 = lstComplaintDashBoard,
        //                GrandTotal = grandTotal
        //            };

        //            return View(viewModel);
        //        }
        //        else
        //        {

        //            CallsModel obj1 = new CallsModel
        //            {
        //                lstComplaintDashBoard1 = new List<CallsModel>(),
        //                GrandTotal = new CallsModel()
        //            };


        //            return View(obj1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }

        //    return View();
        //}




        //[HttpPost]
        //public ActionResult CoordinatorIndividualPerformance(CallsModel CM)
        //{
        //    List<CallsModel> lmd = new List<CallsModel>();
        //    DataSet ds = new DataSet();


        //    Session["From3"] = CM.FromDate;
        //    Session["To3"] = CM.ToDate;




        //    return RedirectToAction("CoordinatorIndividualPerformance");


        //    return View();
        //}


        //added by shrutika salve 30/07/2023    
        public ActionResult CoordinatorIndividualPerformance()
        {

            DataTable CoordinatorIndividual = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            if (Session["From3"] != null && Session["To3"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["From3"].ToString());
                obj1.ToDateI = Convert.ToString(Session["To3"].ToString());
                CoordinatorIndividual = Dalobj.CoordinatorIndividual(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (CoordinatorIndividual.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in CoordinatorIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            CoordinatorName = Convert.ToString(dr["FullName"]),
                            Branch_Name = Convert.ToString(dr["BranchName"]),
                            TotalCalls = Convert.ToString(dr["Totalcalls"]),
                            Ontime = Convert.ToString(dr["OnTimeCount"]),
                            OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
                            DelayCount = Convert.ToString(dr["DelayCount"]),
                            DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
                            NoactionbyCoordinators = Convert.ToString(dr["NoactionbyCoordinators"]),
                            NoactionbyCoordinatorsPercentage = Convert.ToString(dr["NoactionPercentageCoordinators"]),
                            NoactionbyInspectors = Convert.ToString(dr["NoactionbyInspectors"]),
                            NoactionbyInspectorsPercentage = Convert.ToString(dr["NoactionPercentageinspector"]),
                            WrongReceiveDateCount = Convert.ToString(dr["WrongReceiveDateCount"]),
                            WrongReceiveDateCountPercentage = Convert.ToString(dr["WrongReceiveDatePercentage"]),
                            WrongRequestDateCount = Convert.ToString(dr["WrongRequestDateCount"]),
                            WrongRequestDateCountPercentage = Convert.ToString(dr["WrongRequestDatePercentage"]),
                            //added by shrutika salve 07112023
                            Accuracytime = Convert.ToString(dr["Accuracytime"]),
                            Accuracytimeper = Convert.ToString(dr["Accuracytimeper"]),
                            Performance = Convert.ToString(dr["Performance1"]),
                            PerformanceCountPercentage = Convert.ToString(dr["Performance"]),
                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;


                    CallsModel grandTotal = new CallsModel();
                    grandTotal.CoordinatorName = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
                    //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard.Count;
                    //grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
                    //grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalNoactionbyCoordinators = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyCoordinators));
                    // grandTotal.GrandTotalNoactionbyCoordinatorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyCoordinatorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalNoactionbyCoordinatorsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.NoactionbyCoordinatorsPercentage)).Select(item => float.Parse(item.NoactionbyCoordinatorsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalNoactionbyInspectors = lstComplaintDashBoard1.Sum(item => int.Parse(item.NoactionbyInspectors));
                    // grandTotal.GrandTotalNoactionbyInspectorsPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.NoactionbyInspectorsPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalNoactionbyInspectorsPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.NoactionbyInspectorsPercentage)).Select(item => float.Parse(item.NoactionbyInspectorsPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalWrongReceiveDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongReceiveDateCount));
                    //grandTotal.GrandTotalWrongReceiveDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongReceiveDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalWrongReceiveDateCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.WrongReceiveDateCountPercentage)).Select(item => float.Parse(item.WrongReceiveDateCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalWrongRequestDateCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.WrongRequestDateCount));
                    //grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.WrongRequestDateCountPercentage)).Select(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    // Now grandTotal.GrandTotalWrongRequestDateCountPercentage contains the calculated average
                    grandTotal.GrandAccuracytime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Accuracytime));
                    //grandTotal.GrandTotalWrongRequestDateCountPercentage = lstComplaintDashBoard1.Sum(item => float.Parse(item.WrongRequestDateCountPercentage.TrimEnd('%'))) / lstComplaintDashBoard1.Count;
                    grandTotal.GrandAccuracytimeper = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.Accuracytimeper)).Select(item => float.Parse(item.Accuracytimeper.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    // Now grandTotal.GrandTotalWrongRequestDateCountPercentage contains the calculated average

                    //grandTotal.GrandTotalPerformance = lstComplaintDashBoard1.Sum(item => int.Parse(item.Performance));
                    grandTotal.GrandTotalPerformancePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PerformanceCountPercentage)).Select(item => float.Parse(item.PerformanceCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    ViewData["GrandTotal"] = grandTotal;

                    CallsModel viewModel = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    return View(viewModel);
                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }




        [HttpPost]
        public ActionResult CoordinatorIndividualPerformance(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();


            Session["From3"] = CM.FromDate;
            Session["To3"] = CM.ToDate;




            return RedirectToAction("CoordinatorIndividualPerformance");


            return View();
        }



        ////added by shrutika salve 07082023

        //public ActionResult Branchdata()
        //{

        //    DataSet dsBindBranch = new DataSet();
        //    CallsModel tc = new CallsModel();
        //    List<BranchName> lstBranch = new List<BranchName>();
        //    dsBindBranch = Dalobj.BindOriginatingBranch();

        //    if (dsBindBranch.Tables[0].Rows.Count > 0)
        //    {
        //        lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
        //                     select new BranchName()
        //                     {
        //                         Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
        //                         Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

        //                     }).ToList();
        //    }



        //    IEnumerable<SelectListItem> BranchItems;
        //    BranchItems = new SelectList(lstBranch, "Code", "Name");

        //    ViewBag.ProjectTypeItems = BranchItems;
        //    ViewData["BranchName"] = BranchItems;

        //    DataTable Branchdata = new DataTable();
        //    List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();




        //    if (Session["FromDate"] != null && Session["Todate"] != null && Session["OriginatingBranch"] != null)
        //    {

        //        obj1.FromDateI = Convert.ToString(Session["FromDate"].ToString());
        //        obj1.ToDateI = Convert.ToString(Session["Todate"].ToString());
        //        obj1.OriginatingBranch = Convert.ToString(Session["OriginatingBranch"].ToString());
        //        Branchdata = Dalobj.Branchdata(obj1);
        //    }
        //    else
        //    {

        //    }

        //    string showData = string.Empty;
        //    try
        //    {

        //        if (Branchdata.Rows.Count > 0)
        //        {
        //            List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

        //            foreach (DataRow dr in Branchdata.Rows)
        //            {
        //                lstComplaintDashBoard1.Add(new CallsModel
        //                {
        //                    ExecutingBranch = Convert.ToString(dr["Branch_Name"]),

        //                    TotalCalls = Convert.ToString(dr["TotalCalls"]),
        //                    Ontime = Convert.ToString(dr["OnTimeCount"]),
        //                    OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
        //                    DelayCount = Convert.ToString(dr["DelayCount"]),
        //                    DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
        //                });
        //            }

        //            ViewData["Listdetails"] = lstComplaintDashBoard1;


        //            CallsModel grandTotal = new CallsModel();
        //            grandTotal.OpenCalls = Session["Totalcalls"].ToString();
        //            grandTotal.ExecutingBranch = "Grand Total";
        //            grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
        //            grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
        //            grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
        //            grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
        //            grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

        //            ViewData["Grand"] = grandTotal;

        //            CallsModel obj1 = new CallsModel
        //            {
        //                lstComplaintDashBoard1 = lstComplaintDashBoard,
        //                GrandTotal = grandTotal

        //            };
        //            obj1.OpenCalls = Session["Totalcalls"].ToString();
        //            obj1.OriginatingBranch = Session["OriginatingBranch"].ToString();
        //            return View(obj1);

        //        }
        //        else
        //        {

        //            CallsModel obj1 = new CallsModel
        //            {
        //                lstComplaintDashBoard1 = new List<CallsModel>(),
        //                GrandTotal = new CallsModel()
        //            };


        //            return View(obj1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }




        //    return View();
        //}

        public ActionResult Branchdata()
        {

            DataSet dsBindBranch = new DataSet();
            CallsModel tc = new CallsModel();
            List<BranchName> lstBranch = new List<BranchName>();
            dsBindBranch = Dalobj.BindOriginatingBranch();

            if (dsBindBranch.Tables[0].Rows.Count > 0)
            {
                lstBranch = (from n in dsBindBranch.Tables[0].AsEnumerable()
                             select new BranchName()
                             {
                                 Name = n.Field<string>(dsBindBranch.Tables[0].Columns["Branch_Name"].ToString()),
                                 Code = n.Field<Int32>(dsBindBranch.Tables[0].Columns["Br_Id"].ToString())

                             }).ToList();
            }



            IEnumerable<SelectListItem> BranchItems;
            BranchItems = new SelectList(lstBranch, "Code", "Name");

            ViewBag.ProjectTypeItems = BranchItems;
            ViewData["BranchName"] = BranchItems;

            DataTable Branchdata = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();




            if (Session["FromDate"] != null && Session["Todate"] != null && Session["OriginatingBranch"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate"].ToString());
                obj1.ToDateI = Convert.ToString(Session["Todate"].ToString());
                obj1.OriginatingBranch = Convert.ToString(Session["OriginatingBranch"].ToString());
                Branchdata = Dalobj.Branchdata(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (Branchdata.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in Branchdata.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            ExecutingBranch = Convert.ToString(dr["Branch_Name"]),

                            TotalCalls = Convert.ToString(dr["TotalCalls"]),
                            Ontime = Convert.ToString(dr["OnTimeCount"]),
                            OnTimePercentage = Convert.ToString(dr["OnTimePercentage"]),
                            DelayCount = Convert.ToString(dr["DelayCount"]),
                            DelayCountPercentage = Convert.ToString(dr["DelayTimePercentage"]),
                        });
                    }

                    ViewData["Listdetails"] = lstComplaintDashBoard1;


                    CallsModel grandTotal = new CallsModel();
                    grandTotal.OpenCalls = Session["Totalcalls"].ToString();
                    grandTotal.ExecutingBranch = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
                    grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
                    grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    ViewData["Grand"] = grandTotal;

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal

                    };
                    obj1.OpenCalls = Session["Totalcalls"].ToString();
                    obj1.OriginatingBranch = Session["OriginatingBranch"].ToString();
                    return View(obj1);

                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }




            return View();
        }




        [HttpPost]
        public ActionResult Branchdata(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();
            DataSet ds = new DataSet();


            Session["FromDate"] = CM.FromDate;
            Session["Todate"] = CM.ToDate;
            Session["OriginatingBranch"] = CM.OriginatingBranch;




            return RedirectToAction("Branchdata");


            return View();
        }




        //added by shrutika salve 13092023

        //added by shrutika salve 11092023


        public ActionResult CoordinatorIndividualPerformanceExecuting()
        {
            DataTable CoordinatorIndividual = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            if (Session["FromDate4"] != null && Session["Todate4"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate4"].ToString());
                obj1.ToDateI = Convert.ToString(Session["Todate4"].ToString());
                CoordinatorIndividual = Dalobj.CoordinatorPerformance_ExecutingBranch(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (CoordinatorIndividual.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in CoordinatorIndividual.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            CoordinatorName = Convert.ToString(dr["FullName"]),
                            Branch_Name = Convert.ToString(dr["BranchName"]),
                            TotalCalls = Convert.ToString(dr["Totalcall"]),
                            Ontime = Convert.ToString(dr["OnTimeCount"]),
                            OnTimePercentage = Convert.ToString(dr["OnTimeCountPercentage"]),
                            DelayCount = Convert.ToString(dr["DelayCount"]),
                            DelayCountPercentage = Convert.ToString(dr["DelayCountPercentage"]),
                            competantinspectorassigned = Convert.ToString(dr["competantinspectorassigned"]),
                            competantinspectorassignedPercentage = Convert.ToString(dr["competantinspectorassignedPercentage"]),
                            competantinspectorNotassigned = Convert.ToString(dr["competantinspectorNotassigned"]),
                            competantinspectorNotassignedPercentage = Convert.ToString(dr["competantinspectorNotassignedPercentage"]),
                            Performance = Convert.ToString(dr["Performance1"]),
                        });
                    }

                    ViewData["ListData"] = lstComplaintDashBoard1;


                    CallsModel grandTotal = new CallsModel();
                    grandTotal.CoordinatorName = "Grand Total";
                    grandTotal.GrandTotalTotalCalls = lstComplaintDashBoard1.Sum(item => int.Parse(item.TotalCalls));
                    grandTotal.GrandTotalOntime = lstComplaintDashBoard1.Sum(item => int.Parse(item.Ontime));
                    grandTotal.GrandTotalOnTimePercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.OnTimePercentage)).Select(item => float.Parse(item.OnTimePercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalDelayCount = lstComplaintDashBoard1.Sum(item => int.Parse(item.DelayCount));
                    grandTotal.GrandTotalDelayCountPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.DelayCountPercentage)).Select(item => float.Parse(item.DelayCountPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalcompetantinspectorassigned = lstComplaintDashBoard1.Sum(item => int.Parse(item.competantinspectorassigned));
                    grandTotal.GrandTotalcompetantinspectorassignedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.competantinspectorassignedPercentage)).Select(item => float.Parse(item.competantinspectorassignedPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandTotalcompetantinspectorNotassigned = lstComplaintDashBoard1.Sum(item => int.Parse(item.competantinspectorNotassigned));
                    grandTotal.GrandTotalcompetantinspectorNotassignedPercentage = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.competantinspectorNotassignedPercentage)).Select(item => float.Parse(item.competantinspectorNotassignedPercentage.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average


                    //grandTotal.GrandTotalPerformance = lstComplaintDashBoard1.Sum(item => int.Parse(item.Performance));
                    grandTotal.GrandTotalPerformancePercentage = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Performance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Performance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandTotalPerformancePercentage = (float)Math.Round(grandTotal.GrandTotalPerformancePercentage, 2);

                    ViewData["GrandTotal"] = grandTotal;

                    CallsModel viewModel = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };

                    return View(viewModel);
                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }


        [HttpPost]
        public ActionResult CoordinatorIndividualPerformanceExecuting(CallsModel CM)
        {

            Session["FromDate4"] = CM.FromDate;
            Session["Todate4"] = CM.ToDate;


            return RedirectToAction("CoordinatorIndividualPerformanceExecuting");

        }






        //[HttpPost]
        //public ActionResult Branchdata(CallsModel CM)
        //{
        //    List<CallsModel> lmd = new List<CallsModel>();
        //    DataSet ds = new DataSet();


        //    Session["FromDate"] = CM.FromDate;
        //    Session["Todate"] = CM.ToDate;
        //    Session["OriginatingBranch"] = CM.OriginatingBranch;




        //    return RedirectToAction("Branchdata");


        //    return View();
        //}



        [HttpPost]
        public JsonResult GetdetailsCopy(string FromDate, string Todate, string OriginatingBranch)
        {
            Session["OriginatingBranch"] = OriginatingBranch;


            try
            {
                DataSet DSJobMasterByQtId = new DataSet();
                DSJobMasterByQtId = Dalobj.GetDatadeatils(FromDate, Todate, OriginatingBranch);
                if (DSJobMasterByQtId.Tables[0].Rows.Count > 0)
                {

                    obj1.OpenCalls = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["total_call"]);

                }
                Session["Totalcalls"] = obj1.OpenCalls;

            }

            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }


            return Json(obj1, JsonRequestBehavior.AllowGet);

        }

        public ActionResult OPESummary()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstYearList = new List<JobMasters>();

            int TotalPrice = 0;
            int TotalColPrice = 0;

            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;

            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            dtFinancialYear = objDMOR.GetYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {
                    lstYearList.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["fyear"])

                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstYearList, "fYear", "fYear");
            }



            dtMonthList = objJob.GetMonthList();

            if (dtMonthList.Rows.Count > 0)
            {
                foreach (DataRow dr in dtMonthList.Rows)
                {
                    lstMonthList.Add
                    (
                        new JobMasters
                        {
                            strMonthName = Convert.ToString(dr["monthname"]),
                            monthID = Convert.ToInt32(dr["Number"])
                        }
                    );

                }
                ViewBag.MonthList = new SelectList(lstMonthList, "monthID", "strMonthName");
            }


            Session["GetExcelData"] = "Yes";
            if (Session["Month"] != null && Session["Year"] != null)
            {

                objMOR.Month = Convert.ToString(Session["Month"]);
                objMOR.Year = Convert.ToString(Session["Year"]);
            }

            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataTable ds = new DataTable();

            ds = objDMOR.OPESummary(objMOR.Month, objMOR.Year); // fill dataset  

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new MISOPEReport
                    {
                        OP_Number = Convert.ToString(dr["OP_Number"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        // NextApproval = Convert.ToString(dr["NextApproval"]),
                        status = Convert.ToString(dr["Status"]),
                        WorkingDays = Convert.ToString(dr["WorkingDays"]),
                        AmountClaim = Convert.ToString(dr["AmountClaim"]),
                        ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),





                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;

            objMOR.FromDate = null;
            objMOR.ToDate = null;

            return View(objMOR);

        }



        [HttpPost]
        public ActionResult OPESummary(MISOPEReport objMCR)
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstYearList = new List<JobMasters>();
            int TotalPrice = 0;
            int TotalColPrice = 0;

            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            Session["Month"] = objMCR.Month;
            Session["year"] = objMCR.Year;


            string Fyear = string.Empty;
            string month = string.Empty;

            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            dtFinancialYear = objDMOR.GetYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {
                    lstYearList.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["fyear"])

                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstYearList, "fYear", "fYear");
            }


            dtMonthList = objJob.GetMonthList();

            if (dtMonthList.Rows.Count > 0)
            {
                foreach (DataRow dr in dtMonthList.Rows)
                {
                    lstMonthList.Add
                    (
                        new JobMasters
                        {
                            strMonthName = Convert.ToString(dr["monthname"]),
                            monthID = Convert.ToInt32(dr["Number"])
                        }
                    );

                }
                ViewBag.MonthList = new SelectList(lstMonthList, "monthID", "strMonthName");
            }


            //Session["GetExcelData"] = "Yes";
            //if (Session["Month"] != null && Session["Year"] != null)
            //{

            //    objMCR.Month = Convert.ToString(Session["Month"]);
            //    objMCR.Year = Convert.ToString(Session["Year"]);
            //}


            List<MISOPEReport> lmd = new List<MISOPEReport>();
            DataTable ds = new DataTable();

            ds = objDMOR.OPESummary(objMCR.Month, objMCR.Year); // fill dataset  

            if (ds.Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new MISOPEReport
                    {
                        OP_Number = Convert.ToString(dr["OP_Number"]),
                        InspectorName = Convert.ToString(dr["InspectorName"]),
                        Branch_Name = Convert.ToString(dr["Branch"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        //NextApproval = Convert.ToString(dr["NextApproval"]),
                        status = Convert.ToString(dr["Status"]),
                        WorkingDays = Convert.ToString(dr["WorkingDays"]),
                        AmountClaim = Convert.ToString(dr["AmountClaim"]),
                        ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),




                    });
                }
            }
            ViewData["OPEReport"] = lmd;
            objMOR.lst1 = lmd;


            return View(objMOR);
        }


        [HttpGet]
        public ActionResult ExportIndexOPESummary(MISOPEReport objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<MISOPEReport> grid = OPESummaryExportableGrid(objMCR);

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<MISOPEReport> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<MISOPEReport> OPESummaryExportableGrid(MISOPEReport objMCR)
        {

            IGrid<MISOPEReport> grid = new Grid<MISOPEReport>(GetDataOPESummary(objMCR));

            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(c => c.OP_Number).Titled("OPE Number");
            grid.Columns.Add(c => c.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(c => c.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(c => c.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(c => c.WorkingDays).Titled("Working Days");
            grid.Columns.Add(c => c.AmountClaim).Titled("Amount Claim");
            grid.Columns.Add(c => c.ApprovedAmount).Titled("Approved Amount");
            grid.Columns.Add(c => c.status).Titled("Status");

            grid.Pager = new GridPager<MISOPEReport>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<MISOPEReport> GetDataOPESummary(MISOPEReport objMCR)
        {

            DataTable dsCallRegister = new DataTable();

            if (Session["Month"] != null && Session["Year"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.OPESummary(Convert.ToString(Session["Month"]), Convert.ToString(Session["year"]));
            }
            else
            {
                dsCallRegister = objDMOR.OPESummary(objMCR.Month, objMCR.Year);
            }



            List<MISOPEReport> lstFeedback = new List<MISOPEReport>();

            try
            {
                if (dsCallRegister.Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Rows)
                    {
                        lstFeedback.Add(
                            new MISOPEReport
                            {
                                OP_Number = Convert.ToString(dr["OP_Number"]),
                                InspectorName = Convert.ToString(dr["InspectorName"]),
                                Branch_Name = Convert.ToString(dr["Branch"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),

                                status = Convert.ToString(dr["Status"]),
                                WorkingDays = Convert.ToString(dr["WorkingDays"]),
                                AmountClaim = Convert.ToString(dr["AmountClaim"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),


                            });
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CallRegister"] = lstFeedback;
            objMCR.lst1 = lstFeedback;
            return objMCR.lst1;
        }



        private bool IsPostBack()
        {
            bool isPost = false;

            if (Request.QueryString["Page"] != null)
            {
                isPost = true;
            }
            else
            {
                isPost = string.Compare(Request.HttpMethod, "POST", StringComparison.CurrentCultureIgnoreCase) == 0;
            }
            if (Request.UrlReferrer == null)
                return false;

            bool isSameUrl = string.Compare(Request.Url.AbsolutePath, Request.UrlReferrer.AbsolutePath, StringComparison.CurrentCultureIgnoreCase) == 0;


            return isPost && isSameUrl;
        }


        //code added by nikita on 09102023 
        [HttpGet]
        public ActionResult TravelExpense()
        {
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            try
            {
                DataTable DataTable = new DataTable();
                DataTable = Dalobj.GetExpenseDetails();
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                ApprovalOne = Convert.ToString(dr["ApprovalOne"]),
                                ApprovalTwo = Convert.ToString(dr["ApprovalTwo"]),
                                TransferToFI = Convert.ToString(dr["TransferToFI"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                EndCity = Convert.ToString(dr["EndCity"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                PCH_Approval = Convert.ToString(dr["PCH_Approval"]),
                                CH_Approval = Convert.ToString(dr["CH_Approval"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                PkCallId = Convert.ToString(dr["PK_Call_ID"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Job_Number = Convert.ToString(dr["Job_Number"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                FkId = Convert.ToString(dr["FkId"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                obsname = Convert.ToString(dr["OBSName"]),
                                Fi_Approval = Convert.ToString(dr["TransferToFI"]),
                            }
                            );
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return View(Expense);
        }


        [HttpPost]
        public ActionResult TravelExpense(string FromDate, string ToDate, string SAP_No)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            Session["Sap_Number"] = SAP_No;
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            try
            {
                DataTable DataTable = new DataTable();
                DataTable = Dalobj.GetExpenseDetailsDatewise(FromDate, ToDate, SAP_No);
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                ApprovalOne = Convert.ToString(dr["ApprovalOne"]),
                                ApprovalTwo = Convert.ToString(dr["ApprovalTwo"]),
                                TransferToFI = Convert.ToString(dr["TransferToFI"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                EndCity = Convert.ToString(dr["EndCity"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                PCH_Approval = Convert.ToString(dr["PCH_Approval"]),
                                CH_Approval = Convert.ToString(dr["CH_Approval"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                PkCallId = Convert.ToString(dr["PK_Call_ID"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Job_Number = Convert.ToString(dr["Job_Number"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                FkId = Convert.ToString(dr["FkId"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                ExpenseagainstGenertaed_No = Convert.ToString(dr["sap_no_costcenter"]),
                                obsname = Convert.ToString(dr["OBSName"]),
                                Fi_Approval = Convert.ToString(dr["TransferToFI"]),

                            }
                            );
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return View(Expense);
        }

        //Export To Excel for Travel Expense 



        [HttpGet]
        public ActionResult ExportIndexTravelExpense(TravelExpense c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TravelExpense> grid = CreateExportableGridTravelExpense(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<TravelExpense> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                Session["FromDate"] = null;

                Session["ToDate"] = null;
                Session["Sap_Number"] = null;
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "TravelExpense" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<TravelExpense> CreateExportableGridTravelExpense(TravelExpense c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<TravelExpense> grid = new Grid<TravelExpense>(GetDataTravelExpense(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.EmployeeName).Titled("Employee Name");
            grid.Columns.Add(model => model.RoleName).Titled("RoleName");
            grid.Columns.Add(model => model.CountryName).Titled("Country Name");
            grid.Columns.Add(model => model.Tuv_Email_Id).Titled("Tuv Email Id");
            grid.Columns.Add(model => model.MobileNo).Titled("MobileNo");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
            grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            grid.Columns.Add(model => model.EmpCategoray).Titled("Emp Categoray");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.SAPEmpCode).Titled("SAP Emp Code");
            grid.Columns.Add(model => model.SubJobNo).Titled("Sub JobNo");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.CountryName).Titled("Country Name");
            grid.Columns.Add(model => model.Cost_Center).Titled("User Cost Center");
            grid.Columns.Add(model => model.obsname).Titled("OBS Name");
            grid.Columns.Add(model => model.Date).Titled("Date");
            grid.Columns.Add(model => model.City).Titled("City");
            grid.Columns.Add(model => model.ExpenseType).Titled("Expense Type");
            grid.Columns.Add(model => model.Amount).Titled("Amount");
            grid.Columns.Add(model => model.Kilometer).Titled("Kilometer");
            grid.Columns.Add(model => model.Currency).Titled("Currency");
            grid.Columns.Add(model => model.ExchRate).Titled("Exch Rate");
            grid.Columns.Add(model => model.TotalAmount).Titled("TotalAmount");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.VoucherGenerated).Titled("Voucher Generated");
            grid.Columns.Add(model => model.VoucherNo).Titled("Voucher No");
            grid.Columns.Add(model => model.IsSendForApproval).Titled("Is Send For Approval");
            grid.Columns.Add(model => model.IsSendForApprovalAmount).Titled("Is Send For Approval Amount");
            grid.Columns.Add(model => model.ApprovalOne).Titled("Approval One");
            grid.Columns.Add(model => model.ApprovalTwo).Titled("Approval Two");
            grid.Columns.Add(model => model.PCH_Approval).Titled("PCH Approval");
            grid.Columns.Add(model => model.CH_Approval).Titled("CH Approval");
            grid.Columns.Add(model => model.Fi_Approval).Titled("FI Approval Name");
            grid.Columns.Add(model => model.Type).Titled("Main Category");
            grid.Columns.Add(model => model.subCategory).Titled("sub Category");
            grid.Columns.Add(model => model.ExpenseagainstGenertaed).Titled("Expense Against Generated SAP NO/User Costcenter");
            grid.Columns.Add(model => model.ExpenseagainstGenertaed_No).Titled("Expense Against Generated SAP NO/User Costcenter No");
            grid.Columns.Add(model => model.Attachment).Titled("Attachment");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date");
            grid.Columns.Add(model => model.ModifiedBy).Titled("Modified By");
            grid.Columns.Add(model => model.ModifiedDate).Titled("Modified Date");
            grid.Columns.Add(model => model.ApprovedAmount).Titled("Approved Amount");
            grid.Columns.Add(model => model.PkCallId).Titled("PK CallId");
            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.IsActive).Titled("IsActive");




            grid.Pager = new GridPager<TravelExpense>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = Expense.lstTravelExpense.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<TravelExpense> GetDataTravelExpense(TravelExpense c)
        {
            DataTable DataTable = new DataTable();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            string FromDate = string.Empty;
            string ToDate = string.Empty;
            string Sap_Number = string.Empty;


            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);
                Sap_Number = Convert.ToString(Session["Sap_Number"]);

                DataTable = Dalobj.GetExpenseDetailsDatewise(FromDate, ToDate, Sap_Number);

            }
            else
            {
                DataTable = Dalobj.GetExpenseDetails();

            }

            try
            {
                if (DataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in DataTable.Rows)
                    {
                        lstexpense.Add(
                            new TravelExpense
                            {
                                EmployeeName = Convert.ToString(dr["EmployeeName"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                ExpenseType = Convert.ToString(dr["ExpenseType"]),
                                Currency = Convert.ToString(dr["Currency"]),
                                Description = Convert.ToString(dr["Description"]),
                                VoucherGenerated = Convert.ToString(dr["VoucherGenerated"]),
                                CountryName = Convert.ToString(dr["CountryName"]),
                                City = Convert.ToString(dr["City"]),
                                Amount = Convert.ToString(dr["Amount"]),
                                ExchRate = Convert.ToString(dr["ExchRate"]),
                                ApprovalOne = Convert.ToString(dr["ApprovalOne"]),
                                ApprovalTwo = Convert.ToString(dr["ApprovalTwo"]),
                                TransferToFI = Convert.ToString(dr["TransferToFI"]),
                                Type = Convert.ToString(dr["Type"]),
                                TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                Attachment = Convert.ToString(dr["Attachment"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                Kilometer = Convert.ToString(dr["Kilometer"]),
                                SubJobNo = Convert.ToString(dr["SubJobNo"]),
                                PCH_Approval = Convert.ToString(dr["PCH_Approval"]),
                                CH_Approval = Convert.ToString(dr["CH_Approval"]),
                                IsActive = Convert.ToString(dr["IsActive"]),
                                IsSendForApproval = Convert.ToString(dr["IsSendForApproval"]),
                                UIN = Convert.ToString(dr["UIN"]),
                                VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                PkCallId = Convert.ToString(dr["PK_Call_ID"]),
                                IsSendForApprovalAmount = Convert.ToString(dr["IsSendForApproval_Date"]),
                                SAP_No = Convert.ToString(dr["SAP_No"]),
                                Job_Number = Convert.ToString(dr["Job_Number"]),
                                Call_No = Convert.ToString(dr["Call_No"]),
                                Date = Convert.ToString(dr["Date"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmpCategoray = Convert.ToString(dr["EmpCategoray"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                FkId = Convert.ToString(dr["FkId"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                subCategory = Convert.ToString(dr["subcategory_IVR"]),
                                ExpenseagainstGenertaed = Convert.ToString(dr["Expenses_against_SAP_No_costcenter"]),
                                ExpenseagainstGenertaed_No = Convert.ToString(dr["sap_no_costcenter"]),
                                obsname = Convert.ToString(dr["OBSName"]),
                                Fi_Approval = Convert.ToString(dr["TransferToFI"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["TravelExpense"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return Expense.lstTravelExpense;
        }

        //added by shrutika salve 11102023

        public ActionResult OBSWiseUtilisationSummary()
        {

            ViewData["RoleNameItems"] = Dalobs.UserRoleget();


            DataSet dsAuditorName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<NameCode> lstAuditorNamee = new List<NameCode>();
            dsAuditorName = Dalobs.BindAuditorName();

            if (dsAuditorName.Tables[0].Rows.Count > 0)
            {
                lstAuditorNamee = (from n in dsAuditorName.Tables[0].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(dsAuditorName.Tables[0].Columns["RoleName"].ToString()),
                                       Code = n.Field<int>(dsAuditorName.Tables[0].Columns["UserRoleID"].ToString())

                                   }).ToList();
            }

            IEnumerable<SelectListItem> AuditorName;
            AuditorName = new SelectList(lstAuditorNamee, "Code", "Name");
            ViewBag.ProjectTypeItems = AuditorName;
            ViewData["AuditorName"] = AuditorName;
            ViewData["AuditorName"] = lstAuditorNamee;



            DataSet dsName = new DataSet();
            //List<AuditorName> lstAuditorNamee = new List<AuditorName>();
            List<NameCode1> lstamee = new List<NameCode1>();
            dsName = Dalobs.Employeecategory();

            if (dsName.Tables[0].Rows.Count > 0)
            {
                lstamee = (from n in dsName.Tables[0].AsEnumerable()
                           select new NameCode1()
                           {
                               Name = n.Field<string>(dsName.Tables[0].Columns["Name"].ToString()),
                               Code = n.Field<string>(dsName.Tables[0].Columns["id"].ToString())

                           }).ToList();
            }

            IEnumerable<SelectListItem> EmployeeCategory;
            EmployeeCategory = new SelectList(lstamee, "Code", "Name");
            ViewBag.EmployeeCategory = EmployeeCategory;
            ViewData["EmployeeCategory"] = EmployeeCategory;
            ViewData["EmployeeCategory"] = lstamee;



            var Data2 = Dalobs.EmployementCategory();
            ViewBag.EmployementCategory = new SelectList(Data2, "Id", "EmployementCategory");




            DataTable OBSWiseUtilisationSummary = new DataTable();
            List<ObswiseUtilisationSummary> lstOBS = new List<ObswiseUtilisationSummary>();


            if (Session["FromDate"] != null && Session["Todate"] != null || Session["UserRole"] != null || Session["Employee"] != null)
            {

                OBS.FromDate = Convert.ToString(Session["FromDate"].ToString());
                OBS.ToDate = Convert.ToString(Session["Todate"].ToString());
                OBS.UserRole = Convert.ToString(Session["UserRole"]);
                OBS.Id = Convert.ToString(Session["Employee"]);


                OBSWiseUtilisationSummary = Dalobs.OBSWise(OBS);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {







                if (OBSWiseUtilisationSummary.Rows.Count > 0)
                {
                    List<ObswiseUtilisationSummary> lstComplaintDashBoard1 = new List<ObswiseUtilisationSummary>();

                    foreach (DataRow dr in OBSWiseUtilisationSummary.Rows)
                    {
                        lstComplaintDashBoard1.Add(new ObswiseUtilisationSummary
                        {
                            branchName = Convert.ToString(dr["branchName"]),
                            //ExpectedWork = Convert.ToInt32(dr["ExpectedWork"]),
                            //PTIEMPCount = Convert.ToInt32(dr["PTIEMPCount"]),
                            //PTIexceptedWork = Convert.ToInt32(dr["PTIexceptedWork"]),
                            //PTIactuallywork = Convert.ToInt32(dr["PTIactuallywork"]),
                            //PercentagePTIexceptedWork= Convert.ToInt32(dr["PTIactuallywork"]),
                            ExpectedWork = dr["ExpectedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ExpectedWork"]),
                            PTIEMPCount = dr["PTIEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PTIEMPCount"]),
                            PTIexceptedWork = dr["PTIexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PTIexceptedWork"]),
                            PTIactuallywork = dr["PTIactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PTIactuallywork"]),
                            PercentagePTIexceptedWork = dr["PercentagePTIexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentagePTIexceptedWork"])),


                            //ISIMEMPCount = Convert.ToInt32(dr["ISIMEMPCount"]),
                            //ISIMexceptedWork = Convert.ToInt32(dr["ISIMexceptedWork"]),
                            //ISIMactuallywork = Convert.ToInt32(dr["ISIMactuallywork"]),
                            //PercentageISIMexceptedWork = Convert.ToInt32(dr["PTIactuallywork"]),
                            //ISETEMPCount = Convert.ToInt32(dr["ISETEMPCount"]),
                            ISIMEMPCount = dr["ISIMEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIMEMPCount"]),
                            ISIMexceptedWork = dr["ISIMexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIMexceptedWork"]),
                            ISIMactuallywork = dr["ISIMactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIMactuallywork"]),
                            PercentageISIMexceptedWork = dr["PercentageISIMexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageISIMexceptedWork"])),
                            ISETEMPCount = dr["ISETEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISETEMPCount"]),


                            ISIMSPEMPCount = dr["ISIM_SP_EMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_EMPCount"]),
                            ISIMSPexceptedWork = dr["ISIM_SP_exceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_exceptedWork"]),
                            ISIMSPactuallywork = dr["ISIM_SP_Count"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISIM_SP_Count"]),
                            PercentageISIMSPexceptedWork = dr["PercentageISIM_S_P_exceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageISIM_S_P_exceptedWork"])),

                            TotalPT_ISIMEMPCount = dr["GrandTotalPTI_ISIMEMPEMPcount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotalPTI_ISIMEMPEMPcount"]),
                            TotalPT_ISIMexceptedWork = dr["GrandTotalPTI_ISIMexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotalPTI_ISIMexceptedWork"]),
                            TotalPT_ISIMactuallywork = dr["GrandTotalPTI_ISIMEMPactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["GrandTotalPTI_ISIMEMPactuallywork"]),
                            PercentageTotalPT_ISIMexceptedWork = dr["PercentagePTI_ISIMEMP"] == DBNull.Value ? null : Convert.ToString((dr["PercentagePTI_ISIMEMP"])),



                            //ISETexceptedWork = Convert.ToInt32(dr["ISETexceptedWork"]),
                            //ISETMactuallywork = Convert.ToInt32(dr["ISETactuallywork"]),
                            //PercentageISETexceptedWork = Convert.ToInt32(dr["PercentageISETexceptedWork"]),
                            //ISGBEMPCount = Convert.ToInt32(dr["ISGBEMPCount"]),
                            //ISGBexceptedWork = Convert.ToInt32(dr["ISGBexceptedWork"]),
                            //ISGBactuallywork = Convert.ToInt32(dr["ISGBactuallywork"]),
                            ISETexceptedWork = dr["ISETexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISETexceptedWork"]),
                            ISETMactuallywork = dr["ISETactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISETactuallywork"]),
                            PercentageISETexceptedWork = dr["PercentageISETexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageISETexceptedWork"])),
                            ISGBEMPCount = dr["ISGBEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGBEMPCount"]),
                            ISGBexceptedWork = dr["ISGBexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGBexceptedWork"]),
                            ISGBactuallywork = dr["ISGBactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGBactuallywork"]),

                            //PercentageISGBexceptedWork = Convert.ToInt32(dr["PercentageISGBexceptedWork"]),
                            //ISGREMPCount = Convert.ToInt32(dr["ISGREMPCount"]),
                            //ISGRexceptedWork = Convert.ToInt32(dr["ISGRexceptedWork"]),
                            //ISGRactuallywork = Convert.ToInt32(dr["ISGRactuallywork"]),
                            //PercentageISGRexceptedWork = Convert.ToInt32(dr["PercentageISGRexceptedWork"]),
                            //PCGEMPCount = Convert.ToInt32(dr["PCGEMPCount"]),
                            //PCGexceptedWork = Convert.ToInt32(dr["PCGexceptedWork"]),
                            //PCGactuallywork = Convert.ToInt32(dr["PCGactuallywork"]),

                            PercentageISGBexceptedWork = dr["PercentageISGBexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageISGBexceptedWork"])),
                            ISGREMPCount = dr["ISGREMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGREMPCount"]),
                            ISGRexceptedWork = dr["ISGRexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGRexceptedWork"]),
                            ISGRactuallywork = dr["ISGRactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ISGRactuallywork"]),
                            PercentageISGRexceptedWork = dr["PercentageISGRexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageISGRexceptedWork"])),
                            PCGEMPCount = dr["PCGEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCGEMPCount"]),
                            PCGexceptedWork = dr["PCGexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCGexceptedWork"]),
                            PCGactuallywork = dr["PCGactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCGactuallywork"]),

                            //PercentagePCGexceptedWork = Convert.ToInt32(dr["PercentagePCGexceptedWork"]),
                            //PCEMPCount = Convert.ToInt32(dr["PCEMPCount"]),
                            //PCexceptedWork = Convert.ToInt32(dr["PCexceptedWork"]),
                            //PCactuallywork = Convert.ToInt32(dr["PCactuallywork"]),
                            //PercentagePCexceptedWork = Convert.ToInt32(dr["PercentagePCexceptedWork"]),
                            //DVSEMPCount = Convert.ToInt32(dr["DVSEMPCount"]),
                            //DVSexceptedWork = Convert.ToInt32(dr["DVSexceptedWork"]),
                            //DVSactuallywork = Convert.ToInt32(dr["DVSactuallywork"]),
                            //PercentageDVSexceptedWork = Convert.ToInt32(dr["PercentageDVSexceptedWork"]),
                            //EMPcount = Convert.ToInt32(dr["EMPcount"]),
                            PercentagePCGexceptedWork = dr["PercentagePCGexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentagePCGexceptedWork"])),
                            PCEMPCount = dr["PCEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCEMPCount"]),
                            PCexceptedWork = dr["PCexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCexceptedWork"]),
                            PCactuallywork = dr["PCactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PCactuallywork"]),
                            PercentagePCexceptedWork = dr["PercentagePCexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentagePCexceptedWork"])),
                            DVSEMPCount = dr["DVSEMPCount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVSEMPCount"]),
                            DVSexceptedWork = dr["DVSexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVSexceptedWork"]),
                            DVSactuallywork = dr["DVSactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DVSactuallywork"]),
                            PercentageDVSexceptedWork = dr["PercentageDVSexceptedWork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageDVSexceptedWork"])),
                            EMPcount = dr["EMPcount"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EMPcount"]),
                            EMPexceptedWork = dr["EMPexceptedWork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EMPexceptedWork"]),
                            EMPactuallywork = dr["EMPactuallywork"] == DBNull.Value ? 0 : Convert.ToInt32(dr["EMPactuallywork"]),
                            PercentageEMPexceptedWork = dr["PercentageEMPactuallywork"] == DBNull.Value ? null : Convert.ToString((dr["PercentageEMPactuallywork"])),


                        });
                    }



                    ViewData["ListData"] = lstComplaintDashBoard1;

                    Session["FromDatetohold"] = OBS.FromDate;
                    Session["Todatetohold"] = OBS.ToDate;
                    Session["UserRoletohold"] = OBS.UserRole;
                    Session["Employeetohold"] = OBS.EmployementCategory;


                    ObswiseUtilisationSummary grandTotal = new ObswiseUtilisationSummary();
                    grandTotal.branchName = "Grand Total";
                    grandTotal.GrandExpectedWork = lstComplaintDashBoard1.Sum(item => (item.ExpectedWork));
                    grandTotal.GrandPTIEMPCount = lstComplaintDashBoard1.Sum(item => (item.PTIEMPCount));
                    grandTotal.GrandPTIexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PTIexceptedWork));
                    grandTotal.GrandPTIactuallywork = lstComplaintDashBoard1.Sum(item => (item.PTIactuallywork));
                    //grandTotal.GrandPercentagePTIexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentagePTIexceptedWork));
                    grandTotal.GrandPercentagePTIexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentagePTIexceptedWork)).Select(item => float.Parse(item.PercentagePTIexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandISIMEMPCount = lstComplaintDashBoard1.Sum(item => (item.ISIMEMPCount));
                    grandTotal.GrandISIMexceptedWork = lstComplaintDashBoard1.Sum(item => (item.ISIMexceptedWork));
                    grandTotal.GrandISIMactuallywork = lstComplaintDashBoard1.Sum(item => (item.ISIMactuallywork));
                    //grandTotal.GrandPercentageISIMexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageISIMexceptedWork));


                    grandTotal.GrandPercentageISIMexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageISIMexceptedWork)).Select(item => float.Parse(item.PercentageISIMexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average
                    grandTotal.GrandISETEMPCount = lstComplaintDashBoard1.Sum(item => (item.ISETEMPCount));
                    grandTotal.GrandISETexceptedWork = lstComplaintDashBoard1.Sum(item => (item.ISETexceptedWork));
                    grandTotal.GrandISETMactuallywork = lstComplaintDashBoard1.Sum(item => (item.ISETMactuallywork));
                    //grandTotal.GrandPercentageISETexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageISETexceptedWork));



                    grandTotal.GrandPercentageISETexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageISETexceptedWork)).Select(item => float.Parse(item.PercentageISETexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average


                    grandTotal.GrandISETSPEMPCount = lstComplaintDashBoard1.Sum(item => (item.ISIMSPEMPCount));
                    grandTotal.GrandISETSPexceptedWork = lstComplaintDashBoard1.Sum(item => (item.ISIMSPexceptedWork));
                    grandTotal.GrandISETMSPactuallywork = lstComplaintDashBoard1.Sum(item => (item.ISIMSPactuallywork));
                    //grandTotal.GrandPercentageISETexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageISETexceptedWork));



                    grandTotal.GrandPercentageISETSPexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageISIMSPexceptedWork)).Select(item => float.Parse(item.PercentageISIMSPexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandTotalPT_ISIMEMPCount = lstComplaintDashBoard1.Sum(item => (item.TotalPT_ISIMEMPCount));
                    grandTotal.GrandTotalPT_ISIMexceptedWork = lstComplaintDashBoard1.Sum(item => (item.TotalPT_ISIMexceptedWork));
                    grandTotal.GrandTotalPT_ISIMactuallywork = lstComplaintDashBoard1.Sum(item => (item.TotalPT_ISIMactuallywork));
                    grandTotal.GrandPercentageTotalPT_ISIMexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageTotalPT_ISIMexceptedWork)).Select(item => float.Parse(item.PercentageTotalPT_ISIMexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average


                    grandTotal.GrandISGBEMPCount = lstComplaintDashBoard1.Sum(item => (item.ISGBEMPCount));
                    grandTotal.GrandISGBexceptedWork = lstComplaintDashBoard1.Sum(item => (item.ISGBexceptedWork));
                    grandTotal.GrandISGBactuallywork = lstComplaintDashBoard1.Sum(item => (item.ISGBactuallywork));
                    //grandTotal.GrandPercentageISGBexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageISGBexceptedWork));
                    grandTotal.GrandPercentageISGBexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageISGBexceptedWork)).Select(item => float.Parse(item.PercentageISGBexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandISGREMPCount = lstComplaintDashBoard1.Sum(item => (item.ISGREMPCount));
                    grandTotal.GrandISGRexceptedWork = lstComplaintDashBoard1.Sum(item => (item.ISGRexceptedWork));
                    grandTotal.GrandISGRactuallywork = lstComplaintDashBoard1.Sum(item => (item.ISGRactuallywork));
                    //grandTotal.GrandPercentageISGRexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageISGRexceptedWork));
                    grandTotal.GrandPercentageISGRexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageISGRexceptedWork)).Select(item => float.Parse(item.PercentageISGRexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandPCGEMPCount = lstComplaintDashBoard1.Sum(item => (item.PCGEMPCount));
                    grandTotal.GrandPCGexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PCGexceptedWork));
                    grandTotal.GrandPCGactuallywork = lstComplaintDashBoard1.Sum(item => (item.PCGactuallywork));
                    //grandTotal.GrandPercentagePCGexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentagePCGexceptedWork));
                    grandTotal.GrandPercentagePCGexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentagePCGexceptedWork)).Select(item => float.Parse(item.PercentagePCGexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandPCEMPCount = lstComplaintDashBoard1.Sum(item => (item.PCEMPCount));
                    grandTotal.GrandPCexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PCexceptedWork));
                    grandTotal.GrandPCactuallywork = lstComplaintDashBoard1.Sum(item => (item.PCactuallywork));
                    //grandTotal.GrandPercentagePCexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentagePCexceptedWork));
                    grandTotal.GrandPercentagePCexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentagePCexceptedWork)).Select(item => float.Parse(item.PercentagePCexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandDVSEMPCount = lstComplaintDashBoard1.Sum(item => (item.DVSEMPCount));
                    grandTotal.GrandDVSexceptedWork = lstComplaintDashBoard1.Sum(item => (item.DVSexceptedWork));
                    grandTotal.GrandDVSactuallywork = lstComplaintDashBoard1.Sum(item => (item.DVSactuallywork));
                    //grandTotal.GrandPercentageDVSexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageDVSexceptedWork));
                    grandTotal.GrandPercentageDVSexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageDVSexceptedWork)).Select(item => float.Parse(item.PercentageDVSexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average

                    grandTotal.GrandEMPcount = lstComplaintDashBoard1.Sum(item => (item.EMPcount));
                    grandTotal.GrandEMPexceptedWork = lstComplaintDashBoard1.Sum(item => (item.EMPexceptedWork));
                    grandTotal.GrandEMPactuallywork = lstComplaintDashBoard1.Sum(item => (item.EMPactuallywork));
                    //grandTotal.GrandPercentageDVSexceptedWork = lstComplaintDashBoard1.Sum(item => (item.PercentageDVSexceptedWork));
                    grandTotal.GrandPercentageEMPexceptedWork = lstComplaintDashBoard1.Where(item => !string.IsNullOrWhiteSpace(item.PercentageEMPexceptedWork)).Select(item => float.Parse(item.PercentageEMPexceptedWork.TrimEnd('%'))).DefaultIfEmpty(0).Average(); // Calculate the average


                    ViewData["GrandTotal"] = grandTotal;

                    ObswiseUtilisationSummary viewModel = new ObswiseUtilisationSummary
                    {
                        lstComplaintDashBoard1 = lstOBS,
                        GrandTotal = grandTotal
                    };

                    viewModel.FromDate = Convert.ToString(Session["FromDatetohold"]);
                    viewModel.ToDate = Convert.ToString(Session["Todatetohold"]);
                    viewModel.UserRole = Convert.ToString(Session["UserRoletohold"]);
                    viewModel.EmployementCategory = Convert.ToString(Session["Employeetohold"]);


                    DataSet ds = new DataSet();
                    ds = Dalobs.GetAdminAttendanceSheet(OBS);

                    if (ds.Tables.Count > 0)
                    {
                        viewModel.dtUserAttendance = ds.Tables[0];
                    }



                    return View(viewModel);
                }
                else
                {

                    ObswiseUtilisationSummary obj1 = new ObswiseUtilisationSummary
                    {
                        lstComplaintDashBoard1 = new List<ObswiseUtilisationSummary>(),
                        GrandTotal = new ObswiseUtilisationSummary()
                    };


                    return View(OBS);
                }

            }


            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            return View(OBS);


            return View();
        }


        [HttpPost]
        public ActionResult OBSWiseUtilisationSummary(ObswiseUtilisationSummary CM, FormCollection fc)
        {
            string ProList = string.Join(",", fc["BrAuditee1"]);
            CM.UserRole = ProList;

            string itemList = string.Join(",", fc["BrAuditee"]);
            CM.Id = itemList;

            Session["FromDate"] = CM.FromDate;
            Session["Todate"] = CM.ToDate;
            Session["UserRole"] = CM.UserRole;
            Session["Employee"] = CM.Id;


            return RedirectToAction("OBSWiseUtilisationSummary");
            return View();

        }



        [HttpGet]
        public ActionResult ExportOBS(ObswiseUtilisationSummary c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;

                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;

                int WOLeaveCount = 0;
                int PHLeaveCount = 0;
                int SelectDays = 0;
                int WorkingDays = 0;
                int WKDLeaveCount = 0;
                int Exceptedworkingdays = 0;
                double filledPercentage = 0;
                int holidaydays = 0;
                double Tstilledpercantage = 0;
                double PercentageOfWork = 0;
                string DayName = string.Empty;

                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = CreateAttExportableGridOBS(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.ColumnName.ToString();
                    sheet.Column(col++).Width = 18;
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EXCEPTED WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPLOYEMENTCATEGORY"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "FKROLEID"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPGRADE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "HOLIDAYDAYS"
                             && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "%"
                          && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "UTILISATION"


                        )
                        {
                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString() == "CL")
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "SL")
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "PL")
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }

                            nullcount++;
                        }
                    }

                    col = 1;
                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {
                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            //&& grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPLOYEMENTCATEGORY"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "FKROLEID"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "HOLIDAYDAYS"

                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            //added by shrutika salve 10102023
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "EXCEPTED WORKING DAYS")
                            {
                                Exceptedworkingdays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = Exceptedworkingdays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "HOLIDAYDAYS")
                            {
                                holidaydays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = holidaydays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "UTILISATION")
                            {
                                PercentageOfWork = (Convert.ToDouble(WorkingDays) * 100 / Convert.ToDouble(Exceptedworkingdays));
                                string formatted = PercentageOfWork.ToString("0.00");
                                sheet.Cells[row, col].Value = formatted.ToString();


                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "%")
                            {
                                Tstilledpercantage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                string formattedPercentage = Tstilledpercantage.ToString("0.00");
                                sheet.Cells[row, col].Value = formattedPercentage.ToString();


                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "PL" || grid.Rows[Nrow][Ncol1].ToString() == "CL" || grid.Rows[Nrow][Ncol1].ToString() == "SL")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                                sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }
                            else
                            {
                                if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                                    sheet.Cells[row, col].Value = string.Empty;
                                else
                                    sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            }

                        }
                        else
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }
                    row++;
                    nullcount = 0;
                    LeaveCount = 0;
                    OTHLeaveCount = 0;

                    CLeaveCount = 0;
                    SLeaveCount = 0;
                    PLeaveCount = 0;

                    WOLeaveCount = 0;
                    PHLeaveCount = 0;
                    WKDLeaveCount = 0;
                    SelectDays = 0;
                    Exceptedworkingdays = 0;

                    filledPercentage = 0;
                    WorkingDays = 0;
                    holidaydays = 0;
                    Tstilledpercantage = 0;
                    PercentageOfWork = 0;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }



        }

        private DataTable CreateAttExportableGridOBS(ObswiseUtilisationSummary c)
        {

            ///// IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetAttData(c));


            DataTable grid = GetAttDataOBS(c);


            return grid;
        }

        public DataTable GetAttDataOBS(ObswiseUtilisationSummary c)
        {
            DataSet ds = new DataSet();
            FormCollection fc = new FormCollection();

            if (Session["FromDate"] != null && Session["Todate"] != null || Session["UserRole"] != null || Session["Employee"] != null)
            {

                OBS.FromDate = Convert.ToString(Session["FromDate"].ToString());
                OBS.ToDate = Convert.ToString(Session["Todate"].ToString());
                OBS.UserRole = Convert.ToString(Session["UserRole"]);
                OBS.Id = Convert.ToString(Session["Employee"]);



                ds = Dalobs.GetAdminAttendanceSheet(OBS); // fill dataset  
            }

            if (ds.Tables.Count > 0)
            {


            }
            return ds.Tables[0];
        }

        //added by shrutika salve 02/11/2023

        [HttpGet]
        public ActionResult FinalCoodinatorperformance()
        {
            DataTable Finaldata = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            if (Session["FromDate"] != null && Session["Todate"] != null)
            {

                obj1.FromDate = Convert.ToString(Session["FromDate"].ToString());
                obj1.ToDate = Convert.ToString(Session["Todate"].ToString());
                Finaldata = Dalobj.FinalCoordinatorPerformance(obj1);
            }
            else
            {

            }

            string showData = string.Empty;
            try
            {

                if (Finaldata.Rows.Count > 0)
                {
                    List<CallsModel> lstComplaintDashBoard1 = new List<CallsModel>();

                    foreach (DataRow dr in Finaldata.Rows)
                    {
                        lstComplaintDashBoard1.Add(new CallsModel
                        {
                            CoordinatorName = Convert.ToString(dr["FullName"]),
                            Branch_Name = Convert.ToString(dr["BranchName"]),
                            OriginatingBranch = Convert.ToString(dr["OriginatingBranchPerformance"]),
                            Excuting_Branch = Convert.ToString(dr["ExecutingBranchPerformance"]),
                            TSFilledPerformance = Convert.ToString(dr["TSFilledPercentage"]),
                            TrainingCount = Convert.ToString(dr["PercentageTrainingAVG"]),
                            profileperformance = Convert.ToString(dr["profilePerformancePercentage"]),
                            LessionLearntperformance = Convert.ToString(dr["ReadPercentage"]),
                            finalperformance = Convert.ToString(dr["FinalPerformance"]),
                            KPI = Convert.ToString(dr["KPI"]),


                        });
                    }

                    ViewData["FinalCoodinator"] = lstComplaintDashBoard1;
                    CallsModel grandTotal = new CallsModel();

                    grandTotal.CoordinatorName = "Grand Total";
                    grandTotal.GrandOriginatingBranchPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.OriginatingBranch))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.OriginatingBranch.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandExecutingBranchPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.Excuting_Branch))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.Excuting_Branch.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandTSFilledPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.TSFilledPerformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.TSFilledPerformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandFinalPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.finalperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.finalperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandFinalPerformance = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.finalperformance))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.finalperformance.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandKPI = lstComplaintDashBoard1
                    .Where(item => !string.IsNullOrWhiteSpace(item.KPI))
                    .Select(item =>
                    {
                        try
                        {
                            return float.Parse(item.KPI.TrimEnd('%'));
                        }
                        catch (FormatException)
                        {

                            return 0;
                        }
                    })
                    .DefaultIfEmpty(0)
                    .Average();

                    grandTotal.GrandTrainingCount = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.TrainingCount))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.TrainingCount.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();

                    grandTotal.GrandprofileCount = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.profileperformance))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.profileperformance.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();

                    grandTotal.GrandLessonsLearntCount = lstComplaintDashBoard1
                   .Where(item => !string.IsNullOrWhiteSpace(item.LessionLearntperformance))
                   .Select(item =>
                   {
                       try
                       {
                           return float.Parse(item.LessionLearntperformance.TrimEnd('%'));
                       }
                       catch (FormatException)
                       {

                           return 0;
                       }
                   })
                   .DefaultIfEmpty(0)
                   .Average();




                    ViewData["GrandTotal"] = grandTotal;

                    CallsModel viewModel = new CallsModel
                    {
                        lstComplaintDashBoard1 = lstComplaintDashBoard,
                        GrandTotal = grandTotal
                    };
                    viewModel.FromDate = Convert.ToString(Session["FromDate"]);
                    viewModel.ToDate = Convert.ToString(Session["Todate"]);


                    return View(viewModel);
                }
                else
                {

                    CallsModel obj1 = new CallsModel
                    {
                        lstComplaintDashBoard1 = new List<CallsModel>(),
                        GrandTotal = new CallsModel()
                    };


                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View(obj1);
        }

        [HttpPost]
        public ActionResult FinalCoodinatorperformance(CallsModel CM)
        {
            Session["FromDate"] = CM.FromDate;
            Session["Todate"] = CM.ToDate;

            return RedirectToAction("FinalCoodinatorperformance");

        }


        [HttpGet]
        public ActionResult EstimatedVSactualManpower()
        {
            DataTable DTEstimated = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();
            if (Session["FromdateEstimated"] != null && Session["ToateEstimated"] != null)
            {
                obj1.FromDate = Session["FromdateEstimated"].ToString();
                obj1.ToDate = Session["ToateEstimated"].ToString();
                DTEstimated = Dalobj.EstimatedVSactual(obj1); ;
            }
            else
            {

            }

            string showdata = string.Empty;
            try
            {
                if (DTEstimated.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEstimated.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {

                                Call_No = Convert.ToString(dr["Call_No"]),
                                Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                CoordinatorName = Convert.ToString(dr["CoordinatorName"]),
                                Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["Inspector"]),
                                Estimatedbycoordinator = Convert.ToString(dr["Estimatedbycoordinator"]),
                                actualbyinspector = Convert.ToString(dr["actualbyinspector"]),
                                Accuracytime = Convert.ToString(dr["Accuracytime"]),
                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["Listdata"] = lstComplaintDashBoard;
            obj1.lstComplaintDashBoard1 = lstComplaintDashBoard;

            return View(obj1);
        }

        [HttpPost]
        public ActionResult EstimatedVSactualManpower(InspectorPerformance IP)
        {
            Session["FromdateEstimated"] = IP.FromDate;
            Session["ToateEstimated"] = IP.ToDate;
            return RedirectToAction("EstimatedVSactualManpower");

        }
        //added by nikita on 20122023


        [HttpGet]
        public ActionResult Export_IndexOpeReport_(string data)
        {
            try
            {
                
                DataSet dsHtml = objDMOR.GetAccountantExportData(data);

                //    using (var excelPackage = new ExcelPackage())
                //    {
                //        var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                //        sheet.Cells["A1"].Value = "";
                //        var cell = sheet.Cells["A2"];
                //        cell.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell.Style.Font.Bold = true;
                //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["B1"].Value = "";
                //        var cell12 = sheet.Cells["B1"];
                //        cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell12.Style.Font.Bold = true;
                //        cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["C1"].Value = "";
                //        var cell13 = sheet.Cells["C1"];
                //        cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell13.Style.Font.Bold = true;
                //        cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["D1"].Value = "";
                //        var cell14 = sheet.Cells["D1"];
                //        cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell14.Style.Font.Bold = true;
                //        cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["E1"].Value = "";
                //        var cell15 = sheet.Cells["E1"];
                //        cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell15.Style.Font.Bold = true;
                //        cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["F1"].Value = "Out of Pocket Expenses Report";
                //        var cell16 = sheet.Cells["F1"];
                //        cell16.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell16.Style.Font.Bold = true;
                //        cell16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell16.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["G1"].Value = "";
                //        var cell17 = sheet.Cells["G1"];
                //        cell17.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell17.Style.Font.Bold = true;
                //        cell17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell17.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["H1"].Value = "";
                //        var cell18 = sheet.Cells["H1"];
                //        cell18.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell18.Style.Font.Bold = true;
                //        cell18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell18.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["A2"].Value = "Number";
                //        var cell19 = sheet.Cells["A2"];
                //        cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell19.Style.Font.Bold = true;
                //        cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["B2"].Value = "Inspector Name";
                //        var cell20 = sheet.Cells["B2"];
                //        cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell20.Style.Font.Bold = true;
                //        cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                //        sheet.Cells["C2"].Value = "Branch";
                //        var cell2 = sheet.Cells["C2"];
                //        cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell2.Style.Font.Bold = true;
                //        cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                //        sheet.Cells["D2"].Value = "Employee Code";
                //        var cell_ = sheet.Cells["D2"];
                //        cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell_.Style.Font.Bold = true;
                //        cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        //sap employee code
                //        sheet.Cells["E2"].Value = "Sap Employee Code";
                //        var cell2_ = sheet.Cells["E2"];
                //        cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell2_.Style.Font.Bold = true;
                //        cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                //        sheet.Cells["F2"].Value = "Cost Center";
                //        var cell1 = sheet.Cells["F2"];
                //        cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell1.Style.Font.Bold = true;
                //        cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["G2"].Value = "Total Amount(INR)";
                //        var cell10 = sheet.Cells["G2"];
                //        cell10.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell10.Style.Font.Bold = true;
                //        cell10.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell10.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["H2"].Value = "Approved Amount (INR)";
                //        var cell11 = sheet.Cells["H2"];
                //        cell11.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell11.Style.Font.Bold = true;
                //        cell11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell11.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["I2"].Value = " Branch QA Remark";
                //        var cell5 = sheet.Cells["I2"];
                //        cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell5.Style.Font.Bold = true;
                //        cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["J2"].Value = "ApprovalOne-Two Remark";
                //        var cell6 = sheet.Cells["J2"];
                //        cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell6.Style.Font.Bold = true;
                //        cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["K2"].Value = "PCH Remark";
                //        var cell7 = sheet.Cells["K2"];
                //        cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell7.Style.Font.Bold = true;
                //        cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["L2"].Value = "Admin QA Remark";
                //        var cell8 = sheet.Cells["L2"];
                //        cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell8.Style.Font.Bold = true;
                //        cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                //        sheet.Cells["M2"].Value = "CH Remark";
                //        var cell9 = sheet.Cells["M2"];
                //        cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
                //        cell9.Style.Font.Bold = true;
                //        cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                //        cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                //        int j = 0;
                //        decimal RemainingTotal = 0;
                //        decimal OwnVehicalTotal = 0;
                //        decimal RemainingTotalamount = 0;
                //        for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                //        {
                //            sheet.Cells[3 + i, 1].Value = dsHtml.Tables[0].Rows[i]["op_number"].ToString();
                //            sheet.Cells[3 + i, 2].Value = dsHtml.Tables[0].Rows[i]["InspectorName"].ToString();
                //            sheet.Cells[3 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Branch_Name"].ToString();
                //            sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["EmployeeCode"].ToString();
                //            sheet.Cells[3 + i, 5].Value = dsHtml.Tables[0].Rows[i]["SapEmpCode"].ToString();

                //            sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["CostCentre"].ToString();
                //            sheet.Cells[3 + i, 7].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();
                //            sheet.Cells[3 + i, 8].Value = dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString();
                //            //sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["RoleName"].ToString();
                //            sheet.Cells[3 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Branch_QA_Description"].ToString();
                //            sheet.Cells[3 + i, 10].Value = dsHtml.Tables[0].Rows[i]["ApprovalOne_Two_Description"].ToString();
                //            sheet.Cells[3 + i, 11].Value = dsHtml.Tables[0].Rows[i]["Pch_Description"].ToString();
                //            sheet.Cells[3 + i, 12].Value = dsHtml.Tables[0].Rows[i]["AdminOA_Description"].ToString();
                //            sheet.Cells[3 + i, 13].Value = dsHtml.Tables[0].Rows[i]["Ch_Approval_description"].ToString();

                //            RemainingTotalamount = RemainingTotalamount + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());

                //            RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString());
                //            if (sheet.Cells[3 + i, 7].Value.ToString() != sheet.Cells[3 + i, 8].Value.ToString())   //added by nikita on 15012024
                //            {
                //                using (var range = sheet.Cells[3 + i, 1, 3 + i, 13])
                //                {
                //                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);
                //                }
                //            }
                //            var range_ = sheet.Cells["A" + (3 + i) + ":M" + (3 + i)];
                //            range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //            range_.Style.Border.Top.Color.SetColor(Color.Black);
                //            range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //            range_.Style.Border.Left.Color.SetColor(Color.Black);
                //            range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //            range_.Style.Border.Right.Color.SetColor(Color.Black);
                //            range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //            range_.Style.Border.Bottom.Color.SetColor(Color.Black);
                //            j = 3 + i;
                //        }
                //        decimal _GrandTotal = RemainingTotal;
                //        decimal _GrandTotalAmount = RemainingTotalamount;

                //        sheet.Cells["F" + (j + 1)].Value = "Grand Total:";
                //        //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

                //        sheet.Cells["H" + (j + 1)].Value = _GrandTotal;
                //        sheet.Cells["G" + (j + 1)].Value = _GrandTotalAmount;

                //        //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


                //        var range1 = sheet.Cells["F" + (j + 1) + ":H" + (j + 1)];
                //        range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //        range1.Style.Border.Top.Color.SetColor(Color.Black);
                //        range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //        range1.Style.Border.Left.Color.SetColor(Color.Black);
                //        range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //        range1.Style.Border.Right.Color.SetColor(Color.Black);
                //        range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //        range1.Style.Border.Bottom.Color.SetColor(Color.Black);

                //        var range_1 = sheet.Cells["A2:M2"];
                //        range_1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                //        range_1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                //        range_1.Style.Font.Size = 12;
                //        range_1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                //        range_1.Style.Border.Top.Color.SetColor(Color.Black);
                //        range_1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                //        range_1.Style.Border.Left.Color.SetColor(Color.Black);
                //        range_1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //        range_1.Style.Border.Right.Color.SetColor(Color.Black);
                //        range_1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                //        range_1.Style.Border.Bottom.Color.SetColor(Color.Black);

                //        sheet.Cells.AutoFitColumns();
                //        byte[] excelBytes = excelPackage.GetAsByteArray();
                //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //        Response.AddHeader("Content-Disposition", "attachment; filename=OPReport" + DateTime.Now + "_.xlsx");
                //        Response.BinaryWrite(excelBytes);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}
                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    sheet.Cells["A1"].Value = "";
                    var cell = sheet.Cells["A2"];
                    cell.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["B1"].Value = "";
                    var cell12 = sheet.Cells["B1"];
                    cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell12.Style.Font.Bold = true;
                    cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["C1"].Value = "";
                    var cell13 = sheet.Cells["C1"];
                    cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell13.Style.Font.Bold = true;
                    cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["D1"].Value = "";
                    var cell14 = sheet.Cells["D1"];
                    cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell14.Style.Font.Bold = true;
                    cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["E1"].Value = "";
                    var cell15 = sheet.Cells["E1"];
                    cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell15.Style.Font.Bold = true;
                    cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["F1"].Value = "Out of Pocket Expenses Report";
                    var cell16 = sheet.Cells["F1"];
                    cell16.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell16.Style.Font.Bold = true;
                    cell16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell16.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["G1"].Value = "";
                    var cell17 = sheet.Cells["G1"];
                    cell17.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell17.Style.Font.Bold = true;
                    cell17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell17.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["H1"].Value = "";
                    var cell18 = sheet.Cells["H1"];
                    cell18.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell18.Style.Font.Bold = true;
                    cell18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell18.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["A2"].Value = "Number";
                    var cell19 = sheet.Cells["A2"];
                    cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell19.Style.Font.Bold = true;
                    cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["B2"].Value = "Month";
                    var cell19_ = sheet.Cells["B2"];
                    cell19_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell19_.Style.Font.Bold = true;
                    cell19_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell19_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["C2"].Value = "Inspector Name";
                    var cell20 = sheet.Cells["C2"];
                    cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell20.Style.Font.Bold = true;
                    cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    sheet.Cells["D2"].Value = "Branch";
                    var cell2 = sheet.Cells["D2"];
                    cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell2.Style.Font.Bold = true;
                    cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["E2"].Value = "Employee Code";
                    var cell_ = sheet.Cells["E2"];
                    cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell_.Style.Font.Bold = true;
                    cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    //sap employee code
                    sheet.Cells["F2"].Value = "Sap Employee Code";
                    var cell2_ = sheet.Cells["F2"];
                    cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell2_.Style.Font.Bold = true;
                    cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    sheet.Cells["G2"].Value = "Cost Center";
                    var cell1 = sheet.Cells["G2"];
                    cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell1.Style.Font.Bold = true;
                    cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["H2"].Value = "Total Amount(INR)";
                    var cell10 = sheet.Cells["H2"];
                    cell10.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell10.Style.Font.Bold = true;
                    cell10.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell10.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["I2"].Value = "Approved Amount (INR)";
                    var cell11 = sheet.Cells["I2"];
                    cell11.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell11.Style.Font.Bold = true;
                    cell11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell11.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["J2"].Value = " Branch QA Remark";
                    var cell5 = sheet.Cells["J2"];
                    cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell5.Style.Font.Bold = true;
                    cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["K2"].Value = "ApprovalOne-Two Remark";
                    var cell6 = sheet.Cells["K2"];
                    cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell6.Style.Font.Bold = true;
                    cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["L2"].Value = "PCH Remark";
                    var cell7 = sheet.Cells["L2"];
                    cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell7.Style.Font.Bold = true;
                    cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["M2"].Value = "Admin QA Remark";
                    var cell8 = sheet.Cells["M2"];
                    cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell8.Style.Font.Bold = true;
                    cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet.Cells["N2"].Value = "CH Remark";
                    var cell9 = sheet.Cells["N2"];
                    cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
                    cell9.Style.Font.Bold = true;
                    cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



                    int j = 0;
                    decimal RemainingTotal = 0;
                    decimal OwnVehicalTotal = 0;
                    decimal RemainingTotalamount = 0;
                    for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                    {
                        sheet.Cells[3 + i, 1].Value = dsHtml.Tables[0].Rows[i]["op_number"].ToString();
                        sheet.Cells[3 + i, 2].Value = dsHtml.Tables[0].Rows[i]["MonthName"].ToString();

                        sheet.Cells[3 + i, 3].Value = dsHtml.Tables[0].Rows[i]["InspectorName"].ToString();
                        sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["Branch_Name"].ToString();
                        sheet.Cells[3 + i, 5].Value = dsHtml.Tables[0].Rows[i]["EmployeeCode"].ToString();
                        sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["SapEmpCode"].ToString();

                        sheet.Cells[3 + i, 7].Value = dsHtml.Tables[0].Rows[i]["CostCentre"].ToString();

                        sheet.Cells[3 + i, 8].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();
                        sheet.Cells[3 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString();
                        if (sheet.Cells[3 + i, 8].Value.ToString() != sheet.Cells[3 + i, 9].Value.ToString())   //added by nikita on 15012024
                        {
                            using (var range = sheet.Cells[3 + i, 1, 3 + i, 14])
                            {
                                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSalmon);
                            }
                        }
                        //sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["RoleName"].ToString();
                        sheet.Cells[3 + i, 10].Value = dsHtml.Tables[0].Rows[i]["Branch_QA_Description"].ToString();
                        sheet.Cells[3 + i, 11].Value = dsHtml.Tables[0].Rows[i]["ApprovalOne_Two_Description"].ToString();
                        sheet.Cells[3 + i, 12].Value = dsHtml.Tables[0].Rows[i]["Pch_Description"].ToString();
                        sheet.Cells[3 + i, 13].Value = dsHtml.Tables[0].Rows[i]["AdminOA_Description"].ToString();
                        sheet.Cells[3 + i, 14].Value = dsHtml.Tables[0].Rows[i]["Ch_Approval_description"].ToString();

                        RemainingTotalamount = RemainingTotalamount + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString());

                        RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString());
                        var range_ = sheet.Cells["A" + (3 + i) + ":N" + (3 + i)];
                        range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Top.Color.SetColor(Color.Black);
                        range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Left.Color.SetColor(Color.Black);
                        range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Right.Color.SetColor(Color.Black);
                        range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Bottom.Color.SetColor(Color.Black);
                        j = 3 + i;
                    }
                    decimal _GrandTotal = RemainingTotal;
                    decimal _GrandTotalAmount = RemainingTotalamount;

                    sheet.Cells["G" + (j + 1)].Value = "Grand Total:";
                    //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

                    sheet.Cells["H" + (j + 1)].Value = _GrandTotal;
                    sheet.Cells["I" + (j + 1)].Value = _GrandTotalAmount;

                    //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


                    var range1 = sheet.Cells["G" + (j + 1) + ":I" + (j + 1)];
                    range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Top.Color.SetColor(Color.Black);
                    range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Left.Color.SetColor(Color.Black);
                    range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Right.Color.SetColor(Color.Black);
                    range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range1.Style.Border.Bottom.Color.SetColor(Color.Black);

                    var range_1 = sheet.Cells["A2:N2"];
                    range_1.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range_1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range_1.Style.Font.Size = 12;
                    range_1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Top.Color.SetColor(Color.Black);
                    range_1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Left.Color.SetColor(Color.Black);
                    range_1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Right.Color.SetColor(Color.Black);
                    range_1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range_1.Style.Border.Bottom.Color.SetColor(Color.Black);

                    sheet.Cells.AutoFitColumns();
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=OPReport" + DateTime.Now + "_.xlsx");
                    Response.BinaryWrite(excelBytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View();

        }
        //[HttpGet]
        //public ActionResult Export_IndexOpeReport_(string data)
        //{
        //    try
        //    {
        //        //DataTable dt = JsonConvert.DeserializeObject<DataTable>(data);

        //        //DataSet sapData = new DataSet();
        //        //sapData.Tables.Add(data);
        //        //string Result = string.Empty;
        //        DataSet dsHtml = objDMOR.GetAccountantExportData(data);
        //        //DataTable dt = JsonConvert.DeserializeObject<DataTable>(data);

        //        //DataSet sapData = new DataSet();
        //        //sapData.Tables.Add(dt);
        //        //string Result = string.Empty;

        //        //IGrid<MISOPEReport> dsHtml = new Grid<MISOPEReport>(GetData_(data));
        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

        //            sheet.Cells["A1"].Value = "";
        //            var cell = sheet.Cells["A2"];
        //            cell.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell.Style.Font.Bold = true;
        //            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["B1"].Value = "";
        //            var cell12 = sheet.Cells["B1"];
        //            cell12.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell12.Style.Font.Bold = true;
        //            cell12.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell12.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["C1"].Value = "";
        //            var cell13 = sheet.Cells["C1"];
        //            cell13.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell13.Style.Font.Bold = true;
        //            cell13.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell13.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["D1"].Value = "";
        //            var cell14 = sheet.Cells["D1"];
        //            cell14.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell14.Style.Font.Bold = true;
        //            cell14.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell14.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["E1"].Value = "";
        //            var cell15 = sheet.Cells["E1"];
        //            cell15.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell15.Style.Font.Bold = true;
        //            cell15.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell15.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["F1"].Value = "Out of Pocket Expenses Report";
        //            var cell16 = sheet.Cells["F1"];
        //            cell16.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell16.Style.Font.Bold = true;
        //            cell16.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell16.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["G1"].Value = "";
        //            var cell17 = sheet.Cells["G1"];
        //            cell17.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell17.Style.Font.Bold = true;
        //            cell17.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell17.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["H1"].Value = "";
        //            var cell18 = sheet.Cells["H1"];
        //            cell18.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell18.Style.Font.Bold = true;
        //            cell18.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell18.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["A2"].Value = "Number";
        //            var cell19 = sheet.Cells["A2"];
        //            cell19.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell19.Style.Font.Bold = true;
        //            cell19.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell19.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["B2"].Value = "Inspector Name";
        //            var cell20 = sheet.Cells["B2"];
        //            cell20.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell20.Style.Font.Bold = true;
        //            cell20.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell20.Style.VerticalAlignment = ExcelVerticalAlignment.Center;



        //            sheet.Cells["C2"].Value = "Branch";
        //            var cell2 = sheet.Cells["C2"];
        //            cell2.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell2.Style.Font.Bold = true;
        //            cell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell2.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


        //            sheet.Cells["D2"].Value = "Employee Code";
        //            var cell_ = sheet.Cells["D2"];
        //            cell_.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell_.Style.Font.Bold = true;
        //            cell_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["E2"].Value = "Cost Center";
        //            var cell1 = sheet.Cells["E2"];
        //            cell1.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell1.Style.Font.Bold = true;
        //            cell1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell1.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["F2"].Value = "Department";
        //            var cell2_ = sheet.Cells["F2"];
        //            cell2_.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell2_.Style.Font.Bold = true;
        //            cell2_.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell2_.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


        //            sheet.Cells["G2"].Value = " Branch QA Remark";
        //            var cell5 = sheet.Cells["G2"];
        //            cell5.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell5.Style.Font.Bold = true;
        //            cell5.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell5.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["H2"].Value = "ApprovalOne-Two Remark";
        //            var cell6 = sheet.Cells["H2"];
        //            cell6.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell6.Style.Font.Bold = true;
        //            cell6.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell6.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["I2"].Value = "PCH Remark";
        //            var cell7 = sheet.Cells["I2"];
        //            cell7.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell7.Style.Font.Bold = true;
        //            cell7.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell7.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["J2"].Value = "Admin QA Remark";
        //            var cell8 = sheet.Cells["J2"];
        //            cell8.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell8.Style.Font.Bold = true;
        //            cell8.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell8.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["K2"].Value = "CH Remark";
        //            var cell9 = sheet.Cells["K2"];
        //            cell9.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell9.Style.Font.Bold = true;
        //            cell9.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell9.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["L2"].Value = "Total Amount(INR)";
        //            var cell10 = sheet.Cells["L2"];
        //            cell10.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell10.Style.Font.Bold = true;
        //            cell10.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell10.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            sheet.Cells["M2"].Value = "Approved Amount (INR)";
        //            var cell11 = sheet.Cells["M2"];
        //            cell11.Style.Font.Size = 12; // Replace 12 with your desired font size
        //            cell11.Style.Font.Bold = true;
        //            cell11.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            cell11.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //            int j = 0;
        //            decimal RemainingTotal = 0;
        //            decimal OwnVehicalTotal = 0;
        //            for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
        //            {
        //                sheet.Cells[3 + i, 1].Value = dsHtml.Tables[0].Rows[i]["op_number"].ToString();
        //                sheet.Cells[3 + i, 2].Value = dsHtml.Tables[0].Rows[i]["InspectorName"].ToString();
        //                sheet.Cells[3 + i, 3].Value = dsHtml.Tables[0].Rows[i]["Branch_Name"].ToString();
        //                sheet.Cells[3 + i, 4].Value = dsHtml.Tables[0].Rows[i]["EmployeeCode"].ToString();
        //                sheet.Cells[3 + i, 5].Value = dsHtml.Tables[0].Rows[i]["CostCentre"].ToString();
        //                sheet.Cells[3 + i, 6].Value = dsHtml.Tables[0].Rows[i]["RoleName"].ToString();
        //                sheet.Cells[3 + i, 7].Value = dsHtml.Tables[0].Rows[i]["Branch_QA_Description"].ToString();
        //                sheet.Cells[3 + i, 8].Value = dsHtml.Tables[0].Rows[i]["ApprovalOne_Two_Description"].ToString();
        //                sheet.Cells[3 + i, 9].Value = dsHtml.Tables[0].Rows[i]["Pch_Description"].ToString();
        //                sheet.Cells[3 + i, 10].Value = dsHtml.Tables[0].Rows[i]["AdminOA_Description"].ToString();
        //                sheet.Cells[3 + i, 11].Value = dsHtml.Tables[0].Rows[i]["Ch_Approval_description"].ToString();
        //                sheet.Cells[3 + i, 12].Value = dsHtml.Tables[0].Rows[i]["OPEClaim"].ToString();
        //                sheet.Cells[3 + i, 13].Value = dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString();

        //                RemainingTotal = RemainingTotal + Convert.ToDecimal(dsHtml.Tables[0].Rows[i]["Current_Approved_Amount"].ToString());
        //                var range_ = sheet.Cells["A" + (3 + i) + ":M" + (3 + i)];
        //                range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Top.Color.SetColor(Color.Black);
        //                range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Left.Color.SetColor(Color.Black);
        //                range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Right.Color.SetColor(Color.Black);
        //                range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Bottom.Color.SetColor(Color.Black);
        //                j = 3 + i;
        //            }
        //            decimal _GrandTotal = RemainingTotal;

        //            sheet.Cells["L" + (j + 1)].Value = "Grand Total:";
        //            //sheet.Cells["F" + (j + 2)].Value = "Remaining Expenses total:";

        //            sheet.Cells["M" + (j + 1)].Value = _GrandTotal;
        //            //sheet.Cells["G" + (j + 2)].Value = RemainingTotal;


        //            var range1 = sheet.Cells["L" + (j + 1) + ":M" + (j + 1)];
        //            range1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Top.Color.SetColor(Color.Black);
        //            range1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Left.Color.SetColor(Color.Black);
        //            range1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Right.Color.SetColor(Color.Black);
        //            range1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range1.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            var range_1 = sheet.Cells["A2:M2"];
        //            range_1.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            range_1.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        //            range_1.Style.Font.Size = 12;
        //            range_1.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //            range_1.Style.Border.Top.Color.SetColor(Color.Black);
        //            range_1.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //            range_1.Style.Border.Left.Color.SetColor(Color.Black);
        //            range_1.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //            range_1.Style.Border.Right.Color.SetColor(Color.Black);
        //            range_1.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            range_1.Style.Border.Bottom.Color.SetColor(Color.Black);

        //            sheet.Cells.AutoFitColumns();
        //            byte[] excelBytes = excelPackage.GetAsByteArray();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=OPReport" + DateTime.Now + "_.xlsx");
        //            Response.BinaryWrite(excelBytes);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return View();

        //}

        //added by nikita on 25122023

        public ActionResult GetOPListApproval()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetOPList_Approval()
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objDMOR.GetOPList_Approved(UserId, Role);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
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


        //ADDED BY NIKITA ON 25122023

        [HttpGet]
        public JsonResult Op_Voucher_History_Description(string VoucherNo)
        {
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            DataSet ds = objDMOR.BindOp_description(VoucherNo, UserId);
            //string json = JsonConvert.SerializeObject(ds.Tables[0]);
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


        #region Debit Credit export To excel


        [HttpGet]
        public ActionResult ExportDebitCreditSummary(DebitCredit objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<DebitCredit> grid = DebitCreditSummaryExportableGrid(objMCR);



                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];


                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<DebitCredit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<DebitCredit> DebitCreditSummaryExportableGrid(DebitCredit objMCR)
        {

            IGrid<DebitCredit> grid = new Grid<DebitCredit>(GetSummaryDataDebitCredit(objMCR));


            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.Costcentre).Titled("Cost Centre");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.CrAmount).Titled("Credit Amount");
            grid.Columns.Add(c => c.DrAmount).Titled("Debit Amount");


            grid.Pager = new GridPager<DebitCredit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstDebitCredit.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<DebitCredit> GetSummaryDataDebitCredit(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.DebitCreditSummaryDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.DebitCreditSummary();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                Costcentre = Convert.ToString(dr["Costcentre"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                CrAmount = Convert.ToString(dr["CrAmount"]),
                                DrAmount = Convert.ToString(dr["DrAmount"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;

            return lstFeedback;
        }

        #endregion


        [HttpGet]
        public ActionResult MismatchReport(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.MismatchReportDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.MismatchReport();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {

                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCenter"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;
            return View(objMCR);
        }




        [HttpPost]
        public ActionResult MismatchReport(string FromDate, string ToDate, DebitCredit objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.MismatchReportDate(FromDate, ToDate);
            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCenter"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),

                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }


        #region Mismatch Report export To excel


        [HttpGet]
        public ActionResult ExportMismatchReport(DebitCredit objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<DebitCredit> grid = ExportMismatchReportExportableGrid(objMCR);



                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];


                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<DebitCredit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<DebitCredit> ExportMismatchReportExportableGrid(DebitCredit objMCR)
        {

            IGrid<DebitCredit> grid = new Grid<DebitCredit>(GetExportMismatchReport(objMCR));


            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.SalesOrderNo).Titled("SalesOrder No");
            grid.Columns.Add(c => c.Client_Name).Titled("Client Name");
            grid.Columns.Add(c => c.ObsName).Titled("OBS Name");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.Costcentre).Titled("Cost Centre");
            grid.Columns.Add(c => c.CreatedDate).Titled("Created Date");


            grid.Pager = new GridPager<DebitCredit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstDebitCredit.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<DebitCredit> GetExportMismatchReport(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.MismatchReportDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.MismatchReport();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCenter"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;

            return lstFeedback;
        }

        #endregion


        [HttpGet]
        public ActionResult IncorrectSalesOrder(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.IncorrectSalesOrderDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.IncorrectSalesOrder();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {

                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SAP_No"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                JobNumber = Convert.ToString(dr["Job_Number"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCentre"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;
            return View(objMCR);
        }




        [HttpPost]
        public ActionResult IncorrectSalesOrder(string FromDate, string ToDate, DebitCredit objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.IncorrectSalesOrderDate(FromDate, ToDate);
            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SAP_No"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                JobNumber = Convert.ToString(dr["Job_Number"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCentre"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),

                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }


        #region Incorrect SalesOrder Report export To excel


        [HttpGet]
        public ActionResult ExportIncorrectSalesOrder(DebitCredit objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<DebitCredit> grid = ExportIncorrectSalesOrderExportableGrid(objMCR);



                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];


                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<DebitCredit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<DebitCredit> ExportIncorrectSalesOrderExportableGrid(DebitCredit objMCR)
        {

            IGrid<DebitCredit> grid = new Grid<DebitCredit>(GetExportIncorrectSalesOrder(objMCR));


            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.SalesOrderNo).Titled("SalesOrder No");
            grid.Columns.Add(c => c.Client_Name).Titled("Client Name");
            grid.Columns.Add(c => c.ObsName).Titled("OBS Name");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.JobNumber).Titled("Job Number");
            grid.Columns.Add(c => c.Costcentre).Titled("Cost Centre");
            grid.Columns.Add(c => c.CreatedDate).Titled("Created Date");
            grid.Columns.Add(c => c.CreatedBy).Titled("Created By");


            grid.Pager = new GridPager<DebitCredit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstDebitCredit.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<DebitCredit> GetExportIncorrectSalesOrder(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.IncorrectSalesOrderDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.IncorrectSalesOrder();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SAP_No"]),
                                Client_Name = Convert.ToString(dr["Client_Name"]),
                                JobNumber = Convert.ToString(dr["Job_Number"]),
                                ObsName = Convert.ToString(dr["OBSName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                Costcentre = Convert.ToString(dr["CostCentre"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;

            return lstFeedback;
        }

        #endregion


        [HttpGet]
        public ActionResult MultipleCostCentre(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.MultipleCostCentreDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.MultipleCostCentre();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {

                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),

                                Branch = Convert.ToString(dr["Branch_Name"]),


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;
            return View(objMCR);
        }




        [HttpPost]
        public ActionResult MultipleCostCentre(string FromDate, string ToDate, DebitCredit objMCR)
        {
            Session["GetExcelData"] = null;

            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            DataSet dsCallRegister = new DataSet();
            dsCallRegister = objDMOR.MultipleCostCentreDate(FromDate, ToDate);
            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),

                                Branch = Convert.ToString(dr["Branch_Name"]),

                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    //  return View();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            // ViewData["CallRegister"] = lstFeedback;
            objMCR.lstDebitCredit = lstFeedback;
            TempData["Result"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TempData.Keep();
            return View(objMCR);
        }


        #region Incorrect SalesOrder Report export To excel


        [HttpGet]
        public ActionResult ExportMultipleCostCentre(DebitCredit objMCR)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<DebitCredit> grid = ExportMultipleCostCentreExportableGrid(objMCR);



                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];


                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<DebitCredit> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<DebitCredit> ExportMultipleCostCentreExportableGrid(DebitCredit objMCR)
        {

            IGrid<DebitCredit> grid = new Grid<DebitCredit>(GetExportMultipleCostCentre(objMCR));


            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(c => c.SalesOrderNo).Titled("SalesOrder No");
            grid.Columns.Add(c => c.Client_Name).Titled("Client Name");
            
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            


            grid.Pager = new GridPager<DebitCredit>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objMCR.lstDebitCredit.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<DebitCredit> GetExportMultipleCostCentre(DebitCredit objMCR)
        {

            DataSet dsCallRegister = new DataSet();

            if (Session["FromDate"] != null && Session["FromDate"] != null)
            {
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

                dsCallRegister = objDMOR.MultipleCostCentreDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            else
            {

                dsCallRegister = objDMOR.MultipleCostCentre();
            }




            List<DebitCredit> lstFeedback = new List<DebitCredit>();
            try
            {
                if (dsCallRegister.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsCallRegister.Tables[0].Rows)
                    {
                        lstFeedback.Add(
                            new DebitCredit
                            {
                                Count = dsCallRegister.Tables[0].Rows.Count,
                                SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                                Client_Name = Convert.ToString(dr["ClientName"]),
                              
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                


                            });
                    }
                    ViewData["CallRegister"] = dsCallRegister.Tables[0];
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            objMCR.lstDebitCredit = lstFeedback;

            return lstFeedback;
        }

        #endregion

        //added by nikita on 28022024
        public ActionResult GeneratePDF(string OPE_Number)
        {
            //string OPE_Number = "MUM/OPE/23-24/005955";
            DataSet dsHtml = new DataSet();
            dsHtml = objDMOR.OPE_Data(OPE_Number);
            string input = OPE_Number;
            string[] parts = input.Split('/');
            string year = parts[2];




            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            System.Text.StringBuilder strs = new System.Text.StringBuilder();
            string body = string.Empty;


            decimal _GrandTotal_ = 0;
            DataTable dsGetStamp = new DataTable();
            dsGetStamp = dsHtml.Tables[0];
            string StrOpenConcern = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/OpePdf.html")))
            {
                body = reader.ReadToEnd();
            }
            if (dsGetStamp.Rows.Count > 0)
            {
                foreach (DataRow dr in dsGetStamp.Rows)
                {


                    string Date = Convert.ToString(dr["Date"]);
                    string ActivityType = Convert.ToString(dr["ActivityType"]);
                    string Description = Convert.ToString(dr["Description"]);
                    string SAP_No = Convert.ToString(dr["SAP_No"]);
                    string StartTime = Convert.ToString(dr["StartTime"]);
                    string Hours = "H";

                    string OPEClaim = Convert.ToString(dr["OPEClaim"]);



                    StrOpenConcern = StrOpenConcern + "<tr><td style='text-align:center;line-height:2;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + Date + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";word-break:break-word;overflow-wrap:break-word;white-space:normal;'>" + ActivityType + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";word-break:break-word;overflow-wrap:break-word;white-space:normal;'>" + Description + "</td><td style='text-align:left; padding-left:1%;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + SAP_No + "</td><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + StartTime + "</td><td style='text-align:center;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + Hours + "</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";'>" + OPEClaim + "</td></tr>";

                     //_GrandTotal_ += Convert.ToDecimal(OPEClaim);

                }
                var _GrandTotal_1 = dsHtml.Tables[2].Rows[0]["Manday"].ToString();
                var mandayDecimal = Convert.ToDecimal(_GrandTotal_1);
                var GetTotal = mandayDecimal * 275;
                //decimal _GrandTotal = //_GrandTotal_;

                // Append the total rows outside of the loop
                StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td style='text-align:center;line-height:2;font-size:15px;color:black;font-family:\"TNG Pro\";font-Weight:bold;'>Grand Total</td><td style='text-align:right; padding-right:1%;font-size:15px;color:black;font-family:\"TNG Pro\";font-Weight:bold;'>" + GetTotal + "</td></tr>";

            }
            body = body.Replace("[ExpData]", StrOpenConcern);

            string _Header = string.Empty;
            string _footer = string.Empty;
            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/OPE-header.html"));
            _Header = _readHeader_File.ReadToEnd();
            string Empcode = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();
            _Header = _Header.Replace("[Employeecode]", Empcode);

            string Period = dsHtml.Tables[1].Rows[0]["DateRange"].ToString();
            _Header = _Header.Replace("[Period]", Period);

            string MonthName = dsHtml.Tables[0].Rows[0]["MonthName"].ToString();
            _Header = _Header.Replace("[Month]", MonthName);

            string Year = "20" + year.ToString();
            _Header = _Header.Replace("[Year]", Year);



            string Working = dsHtml.Tables[0].Rows[0]["Working"].ToString();
            _Header = _Header.Replace("[Days]", Working);

            string salesorderno = dsHtml.Tables[0].Rows[0]["SAP_No"].ToString();
            _Header = _Header.Replace("[Sales Order No]", salesorderno);

            string SAP_Emp_Code = dsHtml.Tables[0].Rows[0]["SapEmpCode"].ToString();
            _Header = _Header.Replace("[Sapempcode]", SAP_Emp_Code);

            string WorkingMandays = dsHtml.Tables[2].Rows[0]["Manday"].ToString();
            _Header = _Header.Replace("[WorkingMandays]", WorkingMandays);

            _Header = _Header.Replace("[Printed]", DateTime.Now.ToShortDateString());
            string Ope = OPE_Number.ToString();
            _Header = _Header.Replace("[Ope No]", Ope);

            string replacedvoucherNo = OPE_Number.Replace("/", "_");

            string costcenter = dsHtml.Tables[0].Rows[0]["Cost_Center"].ToString();
            _Header = _Header.Replace("[costcenter]", costcenter);

            string InspectorName = dsHtml.Tables[0].Rows[0]["InspectorName"].ToString();
            _Header = _Header.Replace("[Employee]", InspectorName);


            string Branch = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();
            _Header = _Header.Replace("[Branch]", Branch);

            string Amount = dsHtml.Tables[1].Rows[0]["OPE_Claim"].ToString();
            _Header = _Header.Replace("[Amount]", Amount);

            string ManDays = dsHtml.Tables[2].Rows[0]["Manday"].ToString();
            _Header = _Header.Replace("[Amount]", Amount);

            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/OPE-footer.html"));
            _footer = _readFooter_File.ReadToEnd();


            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.MaxPageLoadTime = 360;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.SecurityOptions.CanAssembleDocument = true;
            converter.Options.SecurityOptions.CanCopyContent = true;
            converter.Options.SecurityOptions.CanEditAnnotations = true;
            converter.Options.SecurityOptions.CanEditContent = true;
            converter.Options.SecurityOptions.CanFillFormFields = true;
            converter.Options.SecurityOptions.CanPrint = true;



            // header settings
            converter.Options.DisplayHeader = true ||
                true || true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 150;

            PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true || true || true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 60;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);
            PdfTextSection text1 = new PdfTextSection(45, 15, "Page: {page_number} of {total_pages}  ", new System.Drawing.Font("TNG Pro", 8));
            converter.Footer.Add(text1);
            SelectPdf.PdfDocument doc_ = converter.ConvertHtmlString(body);
            int PageCount = doc_.Pages.Count;

            string path = Server.MapPath("~/QuotationHtml");
            PdfDocument doc = converter.ConvertHtmlString(body);
            doc.Save(path + '\\' + replacedvoucherNo + ".pdf");
            doc.Close();

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + '\\' + replacedvoucherNo + ".pdf");
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, replacedvoucherNo + ".pdf");

        }

        public ActionResult OPE_ApprovedHistory()
        {
            return View();
        }

        //code added by nikita on 09102023 
        [HttpGet]
        public ActionResult ExpenseVoucherStatus()
        {
            Session["GetExcelData"] = "Yes";
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();

            try
            {
                Session["GetExcelData"] = null;
              DataTable DataTable = new DataTable();
                //if (Session["FromDate"] != null || Session["ToDate"] != null || Session["Voucherno"] != null)
                //{
                //    string FromDate = Session["FromDate"].ToString();
                //    string ToDate = Session["ToDate"].ToString();
                //    string Voucherno = Session["Voucherno"].ToString();
                //    DataTable = Dalobj.GetOPEVoucherstatusList(FromDate, ToDate, Voucherno);
                //    if (DataTable.Rows.Count > 0)
                //    {
                //        foreach (DataRow dr in DataTable.Rows)
                //        {
                //            lstexpense.Add(
                //                new TravelExpense
                //                {
                //                    EmployeeName = Convert.ToString(dr["InspectorName"]),
                //                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                //                    //EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                //                    VoucherNo = Convert.ToString(dr["OP_Number"]),
                //                    BranchName = Convert.ToString(dr["Branch_Name"]),
                //                    TotalAmount = Convert.ToString(dr["AmountClaim"]),
                //                    ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                //                    Days = Convert.ToString(dr["WorkingDay"]),
                //                    Month = Convert.ToString(dr["MonthsName"]),

                //                    status = Convert.ToString(dr["Status"]),

                //                }
                //                );
                //        }
                //    }
                //}
                //else
                //{
                    DataTable = Dalobj.GetVoucherstatusList();
                    if (DataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataTable.Rows)
                        {
                            lstexpense.Add(
                                new TravelExpense
                                {
                                    EmployeeName = Convert.ToString(dr["Name"]),
                                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                    EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                                    VoucherNo = Convert.ToString(dr["VoucherNo"]),
                                    BranchName = Convert.ToString(dr["Branch_Name"]),
                                    TotalAmount = Convert.ToString(dr["TotalAmount"]),
                                    ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                    Days = Convert.ToString(dr["DaysPresent"]),
                                    Month = Convert.ToString(dr["Month_Name"]),

                                    status = Convert.ToString(dr["Status"]),
                                    year = Convert.ToString(dr["Year"]),
                                }
                                );
                        }
                    }
                //}

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return View(Expense);
        }




        [HttpPost]
        public ActionResult ExpenseVoucherStatus(string FromDate, string ToDate, string voucherno)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();


            DataTable DataTable = new DataTable();
            DataTable = Dalobj.GetVoucherstatusListDatewise(FromDate, ToDate, voucherno);
            if (DataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in DataTable.Rows)
                {
                    lstexpense.Add(
                        new TravelExpense
                        {
                            EmployeeName = Convert.ToString(dr["Name"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                            VoucherNo = Convert.ToString(dr["VoucherNo"]),
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            TotalAmount = Convert.ToString(dr["TotalAmount"]),
                            ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                            Days = Convert.ToString(dr["DaysPresent"]),
                            Month = Convert.ToString(dr["Month_Name"]),

                            status = Convert.ToString(dr["Status"]),
                            year = Convert.ToString(dr["Year"]),
                        }
                        );
                }
            }
            //else
            //{
            //    return RedirectToAction("ExpenseVoucherStatus");
            ////}

            else
            {


                TempData["Result"] = "No Record Found";
                TempData.Keep();
                Expense.lstTravelExpense = lstexpense;
                return View(Expense);

            }
            TempData["Result"] = "No Record Found";
            TempData.Keep();
            Expense.lstTravelExpense = lstexpense;
            return View(Expense);
        }


        [HttpGet]
        public ActionResult OPEVoucherStatus()
        {
            Session["GetExcelData"] = "Yes";
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();
            DataTable DataTable = new DataTable();
            try
            {
                Session["GetExcelData"] = null;
                if (Session["FromDate"] != null || Session["ToDate"] != null || Session["Voucherno"]!=null)
                {
                    string FromDate = Session["FromDate"].ToString();
                    string ToDate = Session["ToDate"].ToString();
                    string Voucherno = Session["Voucherno"].ToString();
                    DataTable = Dalobj.GetOPEVoucherstatusList(FromDate, ToDate, Voucherno);
                    if (DataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataTable.Rows)
                        {
                            lstexpense.Add(
                                new TravelExpense
                                {
                                    EmployeeName = Convert.ToString(dr["InspectorName"]),
                                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                    //EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                                    VoucherNo = Convert.ToString(dr["OP_Number"]),
                                    BranchName = Convert.ToString(dr["Branch_Name"]),
                                    TotalAmount = Convert.ToString(dr["AmountClaim"]),
                                    ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                    Days = Convert.ToString(dr["WorkingDay"]),
                                    Month = Convert.ToString(dr["MonthsName"]),

                                    status = Convert.ToString(dr["Status"]),

                                }
                                );
                        }
                    }
                }
                else
                {
                   
                    DataTable = Dalobj.GetOPEVoucherstatusList();
                    if (DataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataTable.Rows)
                        {
                            lstexpense.Add(
                                new TravelExpense
                                {
                                    EmployeeName = Convert.ToString(dr["InspectorName"]),
                                    EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                //EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                                   VoucherNo = Convert.ToString(dr["OP_Number"]),
                                    BranchName = Convert.ToString(dr["Branch_Name"]),
                                    TotalAmount = Convert.ToString(dr["AmountClaim"]),
                                    ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                                    Days = Convert.ToString(dr["WorkingDay"]),
                                    Month = Convert.ToString(dr["MonthsName"]),

                                    status = Convert.ToString(dr["Status"]),

                                }
                                );
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            ViewData["UserList"] = lstexpense;
            Expense.lstTravelExpense = lstexpense;

            return View(Expense);
        }




        [HttpPost]
        public ActionResult OPEVoucherStatus(string FromDate, string ToDate, string voucherno)
        {
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            Session["Voucherno"] = voucherno;
            TravelExpense Expense = new TravelExpense();
            List<TravelExpense> lstexpense = new List<TravelExpense>();


            DataTable DataTable = new DataTable();
            DataTable = Dalobj.GetOPEVoucherstatusList(FromDate, ToDate, voucherno);
            if (DataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in DataTable.Rows)
                {
                    lstexpense.Add(
                        new TravelExpense
                        {
                            EmployeeName = Convert.ToString(dr["InspectorName"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            //EmployeementCategoray = Convert.ToString(dr["EmployementCategory"]),
                            VoucherNo = Convert.ToString(dr["OP_Number"]),
                            BranchName = Convert.ToString(dr["Branch_Name"]),
                            TotalAmount = Convert.ToString(dr["AmountClaim"]),
                            ApprovedAmount = Convert.ToString(dr["ApprovedAmount"]),
                            Days = Convert.ToString(dr["WorkingDay"]),
                            Month = Convert.ToString(dr["MonthsName"]),

                            status = Convert.ToString(dr["Status"]),


                        }
                        );
                }
            }
         
            else
            {


                TempData["Result"] = "No Record Found";
                TempData.Keep();
                Expense.lstTravelExpense = lstexpense;
                return View(Expense);

            }

            TempData["Result"] = "No Record Found";
            TempData.Keep();
            Expense.lstTravelExpense = lstexpense;
            return View(Expense);

        }


        public ActionResult Calender()
        {
            try
            {


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                Session["GetExcelData"] = "Yes";

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {

                    NonInspectionActivity n = new NonInspectionActivity();

                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.UserID = Convert.ToString(TempData["HfUserID"]);
                    objModel.UserName = Convert.ToString(TempData["HfUserName"]);

                    TempData.Keep();

                    ds = objNIA.GetUserCalendarDateWise(n, objModel.UserID);
                }





                //ds = objNIA.GetCalenderData(); // fill dataset  

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            Location = Convert.ToString(dr["Location"]),

                            DateSE = Convert.ToString(dr["DateSE"]),
                            Name = Convert.ToString(dr["UserName"]),
                            Branch = Convert.ToString(dr["Branch_Name"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            Code = Convert.ToString(dr["SOCode"]),

                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),

                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),
                            TravelTime = Convert.ToDouble(dr["TravelTime"]),

                            Attachment = Convert.ToString(dr["Attachment"]),
                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            SAP_No = Convert.ToString(dr["SAP_No"])

                        });
                    }
                    ViewData["NonInspectionActivityList"] = lmd;
                    objModel.NIADashBoard = lmd;

                }
                else
                {
                    ViewData["NonInspectionActivityList"] = lmd;
                    objModel.NIADashBoard = lmd;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult Calender(NonInspectionActivity n, FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData["HfUserID"] = Convert.ToString(fc["HfUserID"]);
                TempData["HfUserName"] = Convert.ToString(fc["HfUserName"]);
                TempData.Keep();


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    n.FromD = Convert.ToString(TempData["FromD"]);
                    n.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    objModel.UserName = Convert.ToString(TempData["HfUserName"]);
                    objModel.UserID = Convert.ToString(TempData["HfUserID"]);
                    TempData.Keep();
                    ds = objNIA.GetUserCalendarDateWise(n, objModel.UserID); // fill dataset 
                }
                else
                {
                    ds = objNIA.GetUserCalendar(n, Convert.ToString(fc["HfUserName"])); // fill dataset  
                }


                lmd.Clear();

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                    {
                        lmd.Add(new NonInspectionActivity
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            ActivityType = Convert.ToString(dr["ActivityType"]),
                            Location = Convert.ToString(dr["Location"]),

                            DateSE = Convert.ToString(dr["DateSE"]),
                            Name = Convert.ToString(dr["UserName"]),
                            Branch = Convert.ToString(dr["Branch_Name"]),
                            EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                            Code = Convert.ToString(dr["SOCode"]),

                            ServiceCode = Convert.ToString(dr["ServiceCode"]),
                            Description = Convert.ToString(dr["Description"]),
                            StartTime = Convert.ToDouble(dr["StartTime"]),
                            EndTime = Convert.ToDouble(dr["EndTime"]),

                            TravelTime = Convert.ToDouble(dr["TravelTime"]),
                            Attachment = Convert.ToString(dr["Attachment"]),
                            JobNumber = Convert.ToString(dr["JobNumber"]),
                            SAP_No = Convert.ToString(dr["SAP_No"])

                        });
                    }
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }
            //return View(lmd.ToList());
            return View(objModel);
        }


        #region export to excel 

        [HttpGet]
        public ActionResult ExportIndex(NonInspectionActivity c)
        {
            // Using EPPlus from nuget
            Session["GetExcelData"] = "Yes";
            //Session["FromDate"] = c.FromD;
            // Session["ToDate"] = c.ToD;
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<NonInspectionActivity> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<NonInspectionActivity> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "Calender-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
                //return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<NonInspectionActivity> CreateExportableGrid(NonInspectionActivity c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.DateSE).Titled("Date");
            grid.Columns.Add(model => model.Name).Titled("Name");
            grid.Columns.Add(model => model.Branch).Titled("Branch Name");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Hr-Emp-Code");
            grid.Columns.Add(model => model.ActivityType).Titled("Activity Type");
            grid.Columns.Add(model => model.JobNumber).Titled("Job Number");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP Number");
            grid.Columns.Add(model => model.StartTime).Titled("Outdoor Time/OnSite Time");
            grid.Columns.Add(model => model.EndTime).Titled("Office Time/Offsite Time");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.Location).Titled("Location");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.ServiceCode).Titled("Service Code");
            grid.Columns.Add(model => model.Id).Titled("Id");

            grid.Pager = new GridPager<NonInspectionActivity>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objModel.NIADashBoard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<NonInspectionActivity> GetData(NonInspectionActivity c)
        {
            DataSet DTFeedback = new DataSet();
            FormCollection fc = new FormCollection();

            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                NonInspectionActivity n = new NonInspectionActivity();

                c.FromD = Convert.ToString(TempData["FromD"]);
                c.ToD = Convert.ToString(TempData["ToD"]);
                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                objModel.UserID = Convert.ToString(TempData["HfUserID"]);
                objModel.UserName = Convert.ToString(TempData["HfUserName"]);

                TempData.Keep();

                DTFeedback = objNIA.GetUserCalendarDateWise(c, objModel.UserID);
            }
            else
            {
             //   DTFeedback = objNIA.GetUserCalendar(c, Convert.ToString(fc["HfUserName"])); // fill dataset  
            }




            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  


            if (DTFeedback.Tables.Count > 0)
            {
                foreach (DataRow dr in DTFeedback.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
                {
                    lmd.Add(new NonInspectionActivity
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ActivityType = Convert.ToString(dr["ActivityType"]),
                        Location = Convert.ToString(dr["Location"]),
                        StartDate = Convert.ToString(dr["startdate"]),
                        EndDate = Convert.ToString(dr["enddate"]),
                        DateSE = Convert.ToString(dr["DateSE"]),
                        Name = Convert.ToString(dr["UserName"]),
                        Branch = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        Code = Convert.ToString(dr["SOCode"]),

                        ServiceCode = Convert.ToString(dr["ServiceCode"]),
                        Description = Convert.ToString(dr["Description"]),

                        StartTime = Convert.ToDouble(dr["StartTime"]),
                        EndTime = Convert.ToDouble(dr["EndTime"]),
                        TravelTime = Convert.ToDouble(dr["TravelTime"]),

                        Attachment = Convert.ToString(dr["Attachment"]),
                        JobNumber = Convert.ToString(dr["JobNumber"]),
                        SAP_No = Convert.ToString(dr["SAP_No"])

                    });
                }
                ViewData["NonInspectionActivityList"] = lmd;
                objModel.NIADashBoard = lmd;
                //return View(lmd.ToList());

            }
            return objModel.NIADashBoard;
        }


        #endregion


        //public ActionResult GeneratePDF(string OPE_Number)
        //{
        //    //string OPE_Number = "MUM/OPE/23-24/005955";
        //    DataSet dsHtml = new DataSet();
        //    dsHtml = objDMOR.OPE_Data(OPE_Number);
        //    string htmlContent = "";
        //    string input = OPE_Number;
        //    string[] parts = input.Split('/');
        //    //string month = parts[1];
        //    string year = parts[2];




        //    SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
        //    System.Text.StringBuilder strs = new System.Text.StringBuilder();
        //    string body = string.Empty;


        //    decimal _GrandTotal_ = 0;
        //    DataTable dsGetStamp = new DataTable();
        //    dsGetStamp = dsHtml.Tables[0];
        //    string StrOpenConcern = string.Empty;
        //    using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/OpePdf.html")))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    if (dsGetStamp.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dsGetStamp.Rows)
        //        {


        //            string Date = Convert.ToString(dr["Date"]);
        //            string ActivityType = Convert.ToString(dr["ActivityType"]);
        //            string Description = Convert.ToString(dr["Description"]);
        //            string SAP_No = Convert.ToString(dr["SAP_No"]);
        //            //string Vendor_Name = Convert.ToString(dr["Vendor_Name"]);
        //            //string Job_Location = Convert.ToString(dr["Job_Location"]);
        //            //string Company_Name = Convert.ToString(dr["Company_Name"]);

        //            string StartTime = Convert.ToString(dr["StartTime"]);
        //            string Hours = "H";

        //            string OPEClaim = Convert.ToString(dr["OPEClaim"]);



        //            StrOpenConcern = StrOpenConcern + "<tr><td>" + Date + "</td><td>" + ActivityType + "</td><td>" + Description + "</td><td>" + SAP_No + "</td><td>" + StartTime + "</td><td style='margin-left:10px;'>" + Hours + "</td><td>" + OPEClaim + "</td></tr>";

        //            //if (ExpenseName == "Own Bike" || ExpenseName == "Own Car")
        //            //{
        //            //    OwnVehicalTotal += Convert.ToDecimal(Amount);
        //            //}
        //            //else
        //            //{
        //            //    RemainingTotal += Convert.ToDecimal(Amount);
        //            //}
        //            //decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //            //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";
        //            _GrandTotal_ += Convert.ToDecimal(OPEClaim);

        //        }
        //        decimal _GrandTotal = _GrandTotal_;

        //        // Append the total rows outside of the loop
        //        StrOpenConcern += "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";

        //    }
        //    body = body.Replace("[ExpData]", StrOpenConcern);

        //    //decimal RemainingTotal_ = 0;
        //    //decimal GrandTotal_ = 0;
        //    //string StrOpenConcern_ = string.Empty;

        //    //DataTable dsGetStamp1 = new DataTable();
        //    //dsGetStamp1 = dsHtml.Tables[1];
        //    //if (dsGetStamp1.Rows.Count > 0)
        //    //{
        //    //    foreach (DataRow dr in dsGetStamp1.Rows)
        //    //    {

        //    //        string Type = Convert.ToString(dr["Type"]);
        //    //        string Cost_center = Convert.ToString(dr["Cost_center"]);
        //    //        string SubJob_No = Convert.ToString(dr["SubJob_No"]);
        //    //        string SAP_No = Convert.ToString(dr["SAP_No"]);
        //    //        string Company_Name = Convert.ToString(dr["Company_Name"]);
        //    //        string TotalAmount = Convert.ToString(dr["TotalAmount"]);

        //    //        StrOpenConcern_ = StrOpenConcern_ + "<tr><td>" + Type + "</td><td>" + Cost_center + "</td><td>" + SubJob_No + "</td><td>" + SAP_No + "</td><td>" + Company_Name + "</td><td>" + TotalAmount + "</td></tr>";
        //    //        RemainingTotal_ += Convert.ToDecimal(TotalAmount);

        //    //        //decimal _GrandTotal = OwnVehicalTotal + RemainingTotal;

        //    //        //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + _GrandTotal + "</td></tr>";
        //    //        //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>OwnVehicalTotal</td><td>" + OwnVehicalTotal + "</td></tr>";
        //    //        //StrOpenConcern = StrOpenConcern + "<tr><td></td><td></td><td></td><td></td><td></td><td>RemainingTotal</td><td>" + RemainingTotal + "</td></tr>";

        //    //    }
        //    //    GrandTotal_ = RemainingTotal_;

        //    //    // Append the total rows outside of the loop
        //    //    StrOpenConcern_ += "<tr><td></td><td></td><td></td><td></td><td>Grand Total</td><td>" + GrandTotal_ + "</td></tr>";


        //    //}

        //    //body = body.Replace("[CostExpData]", StrOpenConcern_);

        //    //DataTable dsGetStamp_ = new DataTable();
        //    //dsGetStamp_ = dsHtml.Tables[2];


        //    string Empcode = dsHtml.Tables[0].Rows[0]["EmployeeCode"].ToString();
        //    body = body.Replace("[Employeecode]", Empcode);

        //    string Period = dsHtml.Tables[1].Rows[0]["DateRange"].ToString();
        //    body = body.Replace("[Period]", Period);

        //    string MonthName = dsHtml.Tables[0].Rows[0]["MonthName"].ToString();
        //    body = body.Replace("[Month]", MonthName);

        //    string Year = "20" + year.ToString();
        //    body = body.Replace("[Year]", Year);



        //    //string Working = dsHtml.Tables[0].Rows[0]["Working"].ToString();
        //    //body = body.Replace("[Days]", Working);

        //    string salesorderno = dsHtml.Tables[0].Rows[0]["SAP_No"].ToString();
        //    body = body.Replace("[Sales Order No]", salesorderno);

        //    string SAP_Emp_Code = dsHtml.Tables[0].Rows[0]["SapEmpCode"].ToString();
        //    body = body.Replace("[Sapempcode]", SAP_Emp_Code);



        //    body = body.Replace("[Printed]", DateTime.Now.ToShortDateString());
        //    string Ope = OPE_Number.ToString();
        //    body = body.Replace("[Ope No]", Ope);

        //    string replacedvoucherNo = OPE_Number.Replace("/", "_");

        //    string costcenter = dsHtml.Tables[0].Rows[0]["Cost_Center"].ToString();
        //    body = body.Replace("[costcenter]", costcenter);

        //    string InspectorName = dsHtml.Tables[0].Rows[0]["InspectorName"].ToString();
        //    body = body.Replace("[Employee]", InspectorName);


        //    string Branch = dsHtml.Tables[0].Rows[0]["Branch_Name"].ToString();
        //    body = body.Replace("[Branch]", Branch);

        //    string Amount = dsHtml.Tables[1].Rows[0]["OPE_Claim"].ToString();
        //    body = body.Replace("[Amount]", Amount);
        //    //body = body.Replace("[Voucher No]", StrOpenConcern);


        //    //PdfPageSize pageSize = PdfPageSize.A4;
        //    //PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
        //    //HtmlToPdf converter = new HtmlToPdf();

        //    //SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
        //    //int PageCount = doc1.Pages.Count;
        //    //body = body.Replace("[PageCount]", ObjModelQuotationMast.AddEnclosures + ' ' + "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
        //    //strs.Append(body);
        //    PdfPageSize pageSize = PdfPageSize.A4;
        //    PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
        //    HtmlToPdf converter = new HtmlToPdf();
        //    SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
        //    int PageCount = doc1.Pages.Count;
        //    body = body.Replace("[PageCount]", "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
        //    strs.Append(body);


        //    converter.Options.MaxPageLoadTime = 240;
        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.SecurityOptions.CanAssembleDocument = true;
        //    converter.Options.SecurityOptions.CanCopyContent = true;
        //    converter.Options.SecurityOptions.CanEditAnnotations = true;
        //    converter.Options.SecurityOptions.CanEditContent = true;
        //    converter.Options.SecurityOptions.CanFillFormFields = true;
        //    converter.Options.SecurityOptions.CanPrint = true;

        //    string path = Server.MapPath("~/QuotationHtml");
        //    PdfDocument doc = converter.ConvertHtmlString(body);
        //    doc.Save(path + '\\' + replacedvoucherNo + ".pdf");
        //    doc.Close();

        //    byte[] fileBytes = System.IO.File.ReadAllBytes(path + '\\' + replacedvoucherNo + ".pdf");


        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, replacedvoucherNo + ".pdf");

        //}


        public ActionResult ProcessMeasures()
        {

            DataTable DTProcess = new DataTable();
            ProcessMeasures pro = new ProcessMeasures();
            List<ProcessMeasures> lstComplaintDashBoard = new List<ProcessMeasures>();
            if (Session["ProcessFromdate"] != null && Session["ProcessToate"] != null)
            {
                pro.FromDate = Session["ProcessFromdate"].ToString();
                pro.ToDate = Session["ProcessToate"].ToString();
                DTProcess = Dalobj.ProcessMeasures(pro);
            }
            else
            {
                // Getting the first day of last month
                DateTime firstDayOfLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);

                // Getting the last day of last month
                DateTime lastDayOfLastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);

                pro.FromDate = firstDayOfLastMonth.ToString("dd/MM/yyyy");
                pro.ToDate = lastDayOfLastMonth.ToString("dd/MM/yyyy");



                DTProcess = Dalobj.ProcessMeasures(pro);
            }

            string showdata = string.Empty;
            try
            {
                if (DTProcess.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTProcess.Rows)
                    {
                        lstComplaintDashBoard.Add(
                            new ProcessMeasures
                            {

                                BranchName = Convert.ToString(dr["Branch_Name"]),
                                TotalEmp = Convert.ToString(dr["totalemp"]),
                                inspector = Convert.ToString(dr["inspector"]),
                                OtherEmp = Convert.ToString(dr["Others"]),
                                OnsiteoneMonitoring = Convert.ToString(dr["onsitemonitoringstone"]),
                                offsitethreeMonitoring = Convert.ToString(dr["offsitemonitoringthree"]),
                                ComplaintTotal = Convert.ToString(dr["ComplaintTotal"]),
                                openComplaints = Convert.ToString(dr["openComplaints"]),
                                AttributeComplaints = Convert.ToString(dr["AttributeComplaints"]),
                                IVRCount = Convert.ToString(dr["IVRCount"]),
                                IVRrevCount = Convert.ToString(dr["IVRRevCount"]),
                                IVRcreationOnTimeCount = Convert.ToString(dr["IVRcreationOnTimeCount"]),
                                IVRcreationOnTimeCountper = Convert.ToString(dr["IVRcreationOnTimeCountper"]),
                                IRNCount = Convert.ToString(dr["IRNCount"]),
                                IRNRevCount = Convert.ToString(dr["IRNRevCount"]),
                                IRNcreationOnTimeCount = Convert.ToString(dr["IRNcreationOnTimeCount"]),
                                IRNcreationOnTimeCountPer = Convert.ToString(dr["IRNcreationOnTimeCountPer"]),
                                EVRCount = Convert.ToString(dr["EVRCount"]),

                                EVRRevCount = Convert.ToString(dr["EVRRevCount"]),
                                NCROpen = Convert.ToString(dr["NCROpen"]),
                                NCRClose = Convert.ToString(dr["NCRClose"]),
                                executedCalls = Convert.ToString(dr["executedCalls"]),
                                executedOwnBranch = Convert.ToString(dr["executedOwnBranch"]),
                                executedOtherBranch = Convert.ToString(dr["executedOtherBranch"]),
                                executedOnTimeCount = Convert.ToString(dr["executedOnTimeCount"]),
                                executedOnTimePercentage = Convert.ToString(dr["executedOnTimePercentage"]),
                                executedOnTimeOwnCalls = Convert.ToString(dr["executedOnTimeOwnCalls"]),

                                executedOnTimeOwnPercentage = Convert.ToString(dr["executedOnTimeOwnPercentage"]),
                                executedOnTimeOtherBranchesCall = Convert.ToString(dr["executedOnTimeOtherBranchesCall"]),
                                executedOnTimeOtherBranchesPercentage = Convert.ToString(dr["executedOnTimeOtherBranchesPercentage"]),
                                OriginatingTotalCall = Convert.ToString(dr["OriginatingTotalCall"]),
                                OriginatingOnTimeCount = Convert.ToString(dr["OriginatingOnTimeCount"]),
                                OriginatingOnTimePercentage = Convert.ToString(dr["OriginatingOnTimePercentage"]),
                                FeedbackRec = Convert.ToString(dr["FeedbackRec"]),
                                Feedback = Convert.ToString(dr["Feedback"]),
                                TotalMOnitoringRecords = Convert.ToString(dr["TotalMOnitoringRecords"]),

                                OnsiteCount = Convert.ToString(dr["OnsiteCount"]),
                                OffSiteMonitoring = Convert.ToString(dr["OffSiteMonitoring"]),
                                Mentoring = Convert.ToString(dr["Mentoring"]),
                                MonitoringOfMonitors = Convert.ToString(dr["MonitoringOfMonitors"]),
                                specialMonitoring = Convert.ToString(dr["specialMonitoring"]),
                                IVRRevper = Convert.ToString(dr["IVRRevCountper"]),
                                IRNRevper = Convert.ToString(dr["IRNRevCountPer"]),
                                offsitemonitoringOne = Convert.ToString(dr["Offsitemonitoringone"]),
                                TotalNCR = Convert.ToString(dr["TotalNCR"]),
                                SafetyReport = Convert.ToString(dr["safetyreport"]),

                            });


                    }
                }



                else
                {

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            pro.lstComplaint = lstComplaintDashBoard;

            return View(pro);

        }


        [HttpPost]
        public ActionResult ProcessMeasures(ProcessMeasures IP)
        {
            Session["ProcessFromdate"] = IP.FromDate;
            Session["ProcessToate"] = IP.ToDate;
            return RedirectToAction("ProcessMeasures");

        }
        public JsonResult GetOPApprovalListButtonwise(string Branch)
        {
            DataSet ds = new DataSet();
            string Role = Session["RoleName"].ToString(); //"Approval";

            string UserId_ = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            ds = objDMOR.GetOPApprovalListButtonwise(Branch);
            string JsonString = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                //JsonString = JsonConvert.SerializeObject(ds.Tables[1]);


            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateSendForapproval(string OP_Number)
        {
            DataTable dt = new DataTable();
            dt = objDMOR.valiadateAprroval(OP_Number);
            string result = "";
            result = JsonConvert.SerializeObject(dt);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetDataInExcel(string fromDate, string toDate)
        {
            try
            {
                string role = Session["RoleName"].ToString(); // "Approval";
                string userId = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

                DataSet dsHtml = objDMOR.GetDatewiseApprovedlist(fromDate, toDate, role, userId);

                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    int headerRow = 1;
                    for (int col = 0; col < dsHtml.Tables[0].Columns.Count; col++)
                    {
                        sheet.Cells[headerRow, col + 1].Value = dsHtml.Tables[0].Columns[col].ColumnName;
                        var headerCell = sheet.Cells[headerRow, col + 1];
                        headerCell.Style.Font.Bold = true;
                        headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        headerCell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        headerCell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    }
                    int dataRowStart = headerRow + 1;
                    for (int row = 0; row < dsHtml.Tables[0].Rows.Count; row++)
                    {
                        for (int col = 0; col < dsHtml.Tables[0].Columns.Count; col++)
                        {
                            sheet.Cells[dataRowStart + row, col + 1].Value = dsHtml.Tables[0].Rows[row][col].ToString();
                        }
                    }

                    sheet.Cells.AutoFitColumns();
                    byte[] excelBytes = excelPackage.GetAsByteArray();

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", "attachment; filename=ApprovedList.xlsx");

                    Response.BinaryWrite(excelBytes);
                    Response.Flush();
                    Response.End();
                }

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public JsonResult DelayRemark(string OP_Number, string Remarks)
        {
            string JsonString = "";
            Session["OP_Number1"] = null;


            DataSet ds = new DataSet();
            ds = objDMOR.OP_DelayRemark(OP_Number, Remarks); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delayflag(string OP_Number)
        {
            string JsonString = "";
            Session["OP_Number1"] = OP_Number.ToString();
            DataSet ds = new DataSet();
            ds = objDMOR.Op_delayFlag(OP_Number); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRemark(string OP_Number)
        {
            string JsonString = "";
            DataSet ds = new DataSet();
            ds = objDMOR.GetRemark(OP_Number); // fill dataset  
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                JsonString = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                JsonString = JsonConvert.SerializeObject("Error");
            }
            return Json(JsonString, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LeaveAttendanceSheet()
        {
            try
            {

                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                Session["GetExcelData"] = "Yes";

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    NonInspectionActivity n = new NonInspectionActivity();

                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetLeaveAttendanceSheet(n);
                }
                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(objModel);
        }

        [HttpPost]
        public ActionResult LeaveAttendanceSheet(NonInspectionActivity n, FormCollection fc)
        {
            try
            {
                Session["GetExcelData"] = null;

                TempData["FromD"] = n.FromD;
                TempData["ToD"] = n.ToD;
                TempData.Keep();


                List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  
                DataSet ds = new DataSet();

                if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
                {
                    n.StartDate = Convert.ToString(TempData["FromD"]);
                    n.EndDate = Convert.ToString(TempData["ToD"]);

                    objModel.FromD = Convert.ToString(TempData["FromD"]);
                    objModel.ToD = Convert.ToString(TempData["ToD"]);
                    TempData.Keep();

                    ds = objNIA.GetLeaveAttendanceSheet(n); // fill dataset  
                }

                if (ds.Tables.Count > 0)
                {
                    objModel.dtUserAttendance = ds.Tables[0];
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;

            }

            return View(objModel);
        }

        #region export Leave Attendance to excel 

        [HttpGet]
        public ActionResult LeaveExportAttIndex(NonInspectionActivity c)
        {

            Session["GetExcelData"] = "Yes";

            using (ExcelPackage package = new ExcelPackage())
            {

                Int32 col = 1;
                Int32 row = 1;
                int LeaveCount = 0;
                int nullcount = 0;

                int OTHLeaveCount = 0;

                int CLeaveCount = 0;
                int SLeaveCount = 0;
                int PLeaveCount = 0;

                int WOLeaveCount = 0;
                int PHLeaveCount = 0;
                int SelectDays = 0;
                int WorkingDays = 0;
                int WKDLeaveCount = 0;
                double filledPercentage = 0;
                string DayName = string.Empty;

                package.Workbook.Worksheets.Add("Data");
                /// IGrid<NonInspectionActivity> grid = CreateAttExportableGrid(c);

                DataTable grid = LeaveCreateAttExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() == "MOBILENO"
                        || column.ColumnName.ToString().ToUpper() == "NAME"
                        || column.ColumnName.ToString().ToUpper() == "EMAIL"
                            || column.ColumnName.ToString().ToUpper() == "TUV EMPLOYEE CODE"
                            || column.ColumnName.ToString().ToUpper() == "SAP EMPLOYEE CODE"
                            || column.ColumnName.ToString().ToUpper() == "BRANCH"
                            || column.ColumnName.ToString().ToUpper() == "DESIGNATION"
                            || column.ColumnName.ToString().ToUpper() == "SAP VENDOR CODE"
                            || column.ColumnName.ToString().ToUpper() == "MAINBRANCH"
                            || column.ColumnName.ToString().ToUpper() == "ROLE"
                            || column.ColumnName.ToString().ToUpper() == "COSTCENTRE"
                            || column.ColumnName.ToString().ToUpper() == "DOJ"
                            || column.ColumnName.ToString().ToUpper() == "OBSTYPE"
                            || column.ColumnName.ToString().ToUpper() == "CURRENTASSIGNMENT"
                            || column.ColumnName.ToString().ToUpper() == "SPECIALSERVICES"
                            || column.ColumnName.ToString().ToUpper() == "EMPCATEGORY"
                            || column.ColumnName.ToString().ToUpper() == "LEAVE COUNT"
                            || column.ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT"
                            || column.ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT"
                            || column.ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT"
                            || column.ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT"
                            || column.ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT"
                            || column.ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT"
                            || column.ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                    {
                        sheet.Cells[1, col].Value = column.ColumnName.ToString();
                        sheet.Column(col++).Width = 18;
                    }
                }
                row++;
                for (int Nrow = 0; Nrow < grid.Rows.Count; Nrow++)
                {

                    for (int Ncol = 0; Ncol < grid.Columns.Count; Ncol++)
                    {
                        if (grid.Rows[Nrow][Ncol] != DBNull.Value &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMAIL" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TS FILLED DAYS" &&
                            grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WORKING DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "EMPGRADE"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "OTHER LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "SICK LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "CASUAL LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PRIVILEGE LEAVE COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKLY OFF COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "PUBLIC HOLIDAY COUNT"
                            && grid.Columns[Ncol].ColumnName.ToString().ToUpper() != "WEEKEND LEAVE COUNT"


                        )
                        {
                            DayName = System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(DateTime.Parse(grid.Columns[Ncol].ColumnName.ToString()).DayOfWeek);

                            if (grid.Rows[Nrow][Ncol].ToString() == "CL")
                            {
                                CLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "SL")
                            {
                                SLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "PL" || grid.Rows[Nrow][Ncol].ToString() == "A33")
                            {
                                PLeaveCount++;
                                LeaveCount++;
                                if ((DayName.ToUpper() == "SUNDAY" || DayName.ToUpper() == "SATURDAY"))
                                {
                                    WKDLeaveCount++;
                                }
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "33") // Weekly Off Count
                            {
                                WOLeaveCount++;
                                OTHLeaveCount++;
                            }
                            else if (grid.Rows[Nrow][Ncol].ToString() == "35") // Public Holidays
                            {
                                PHLeaveCount++;
                                OTHLeaveCount++;
                            }

                            nullcount++;
                        }
                    }

                    col = 1;
                    for (int Ncol1 = 0; Ncol1 < grid.Columns.Count; Ncol1++)
                    {
                        if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "NAME" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MOBILENO" &&
                            grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMAIL"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "TUV EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP EMPLOYEE CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SELECTED PERIOD DAYS"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "BRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DESIGNATION"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SAP VENDOR CODE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "MAINBRANCH"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "ROLE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "COSTCENTRE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "DOJ"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "OBSTYPE"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "CURRENTASSIGNMENT"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "SPECIALSERVICES"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPCATEGORY"
                            && grid.Columns[Ncol1].ColumnName.ToString().ToUpper() != "EMPGRADE"



                        )
                        {

                            if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = LeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = OTHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = CLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = SLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WORKING DAYS")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WorkingDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WOLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT")
                            {
                                WorkingDays = nullcount - (LeaveCount + OTHLeaveCount);
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = PHLeaveCount.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SELECTED PERIOD DAYS")
                            {
                                SelectDays = Convert.ToInt32(grid.Rows[Nrow][Ncol1].ToString());
                                sheet.Cells[row, col].Value = SelectDays.ToString();
                            }
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TS FILLED DAYS")
                            {
                                filledPercentage = (Convert.ToDouble(nullcount) / Convert.ToDouble(SelectDays)) * 100;
                                if (filledPercentage > 72)
                                {
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                                else
                                {
                                    sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCyan);
                                    sheet.Cells[row, col].Value = nullcount.ToString();
                                }
                            }
                            /*
                          else if (grid.Rows[Nrow][Ncol1].ToString() == "PL" || grid.Rows[Nrow][Ncol1].ToString() == "CL" || grid.Rows[Nrow][Ncol1].ToString() == "SL" || grid.Rows[Nrow][Ncol1].ToString() == "A33")
                          {
                              sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                              if (grid.Rows[Nrow][Ncol1].ToString() == "A33")
                              {
                                  sheet.Cells[row, col].Value = "AUTO PL";
                              }
                              else
                              {
                                  sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                              }
                          }

                          else if (grid.Rows[Nrow][Ncol1].ToString() == "33")
                          {
                              sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                              // sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                              sheet.Cells[row, col].Value = "Weekly Off";
                          }
                          else if (grid.Rows[Nrow][Ncol1].ToString() == "35")
                          {
                              sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PeachPuff);
                              //  sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                              sheet.Cells[row, col].Value = "Public Holiday";
                          }

                          else if (grid.Rows[Nrow][Ncol1].ToString() == "INSP")
                          {
                              sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                              sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                          }
                          else if (grid.Rows[Nrow][Ncol1].ToString().Contains(","))
                          {
                              sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                              sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Honeydew);
                              sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                          }
                          */
                            else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                            {
                                sheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                                sheet.Cells[row, col].Value = WKDLeaveCount.ToString();
                            }

                            //{
                            //    if (grid.Rows[Nrow][Ncol1] == DBNull.Value)
                            //        sheet.Cells[row, col].Value = string.Empty;
                            //    else
                            //        sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                            //}

                        }
                        else if (grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "NAME"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "MOBILENO"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "EMAIL"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "TUV EMPLOYEE CODE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SAP EMPLOYEE CODE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "BRANCH"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "DESIGNATION"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SAP VENDOR CODE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "MAINBRANCH"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "ROLE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "COSTCENTRE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "DOJ"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OBSTYPE"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CURRENTASSIGNMENT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SPECIALSERVICES"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "EMPCATEGORY"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "LEAVE COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "OTHER LEAVE COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "SICK LEAVE COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "CASUAL LEAVE COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PRIVILEGE LEAVE COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKLY OFF COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "PUBLIC HOLIDAY COUNT"
                            || grid.Columns[Ncol1].ColumnName.ToString().ToUpper() == "WEEKEND LEAVE COUNT")
                        {
                            sheet.Cells[row, col].Value = grid.Rows[Nrow][Ncol1].ToString();
                        }
                        col++;

                    }
                    row++;
                    nullcount = 0;
                    LeaveCount = 0;
                    OTHLeaveCount = 0;

                    CLeaveCount = 0;
                    SLeaveCount = 0;
                    PLeaveCount = 0;

                    WOLeaveCount = 0;
                    PHLeaveCount = 0;
                    WKDLeaveCount = 0;
                    SelectDays = 0;

                    filledPercentage = 0;
                    WorkingDays = 0;
                }
                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }



        }

        private DataTable LeaveCreateAttExportableGrid(NonInspectionActivity c)
        {

            DataTable grid = LeaveGetAttData(c);
            return grid;
        }

        public DataTable LeaveGetAttData(NonInspectionActivity c)
        {
            DataSet ds = new DataSet();
            FormCollection fc = new FormCollection();

            if ((TempData["FromD"] != null && TempData["FromD"] != string.Empty) && (TempData["ToD"] != null && TempData["ToD"] != string.Empty))
            {
                c.StartDate = Convert.ToString(TempData["FromD"]);
                c.EndDate = Convert.ToString(TempData["ToD"]);

                objModel.FromD = Convert.ToString(TempData["FromD"]);
                objModel.ToD = Convert.ToString(TempData["ToD"]);
                TempData.Keep();

                ds = objNIA.GetLeaveAttendanceSheet(c); // fill dataset  
            }

            List<NonInspectionActivity> lmd = new List<NonInspectionActivity>();  // creating list of model.  


            if (ds.Tables.Count > 0)
            {


            }
            return ds.Tables[0];
        }


        #endregion



        [HttpGet]
        public ActionResult TimeSheetData()
        {
            DataTable DTComplaintDashBoard = new DataTable();
            List<CallsModel> lstComplaintDashBoard = new List<CallsModel>();


            //get inspector name role 
            DataSet DTUserDashBoard = new DataSet();
            List<UNameCode> lstUser = new List<UNameCode>();
            DTUserDashBoard = Dalobj.GetinspectionList();

            if (DTUserDashBoard.Tables[0].Rows.Count > 0)//All Items to be Inspected
            {
                lstUser = (from n in DTUserDashBoard.Tables[0].AsEnumerable()
                           select new UNameCode()
                           {
                               Name = n.Field<string>(DTUserDashBoard.Tables[0].Columns["FullName"].ToString()),
                               Code = n.Field<string>(DTUserDashBoard.Tables[0].Columns["PK_UserID"].ToString())
                           }).ToList();
            }

            IEnumerable<SelectListItem> Items1;
            Items1 = new SelectList(lstUser, "Code", "Name");
            ViewBag.Emp = Items1;
            ViewData["Emp"] = Items1;


            if (Session["PO_Number"] != null & Session["po_Date"] != null)
            {
                obj1.PO_Number = Convert.ToString(Session["PO_Number"]);
                obj1.po_Date = Convert.ToString(Session["po_Date"]);
            }

            DataTable Invoice = new DataTable();
            if (Session["FromDate2"] != null && Session["ToDate2"] != null || Session["Job2"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate2"].ToString());
                obj1.ToDateI = Convert.ToString(Session["ToDate2"].ToString());
                obj1.Job = Convert.ToString(Session["Job2"]?.ToString()) ?? "";
                //obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                obj1.Inspector = Convert.ToString(Session["inpector2"]?.ToString()) ?? "";

                DTComplaintDashBoard = Dalobj.TimeData(obj1);

            }
            else
            {

            }

            string showData = string.Empty;
            try
            {
                if (DTComplaintDashBoard.Rows.Count > 0)
                {

                    foreach (DataRow dr in DTComplaintDashBoard.Rows)
                    {


                        lstComplaintDashBoard.Add(
                            new CallsModel
                            {

                                Actual_Visit_Date = Convert.ToString(dr["VisitDate"]),
                                Call_No = Convert.ToString(dr["Call_no"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                Job = Convert.ToString(dr["job"]),
                                ExecutingBranch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["inspectorName"]),
                                Onsite = Convert.ToString(dr["StartTime"]),
                                Offsite = Convert.ToString(dr["EndTime"]),
                                TravelTime = Convert.ToString(dr["TravelTime"]),
                                IVR = Convert.ToString(dr["Report"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                Status = Convert.ToString(dr["Status"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                VendorName = Convert.ToString(dr["subvendor"]),
                                TopSubVendorName = Convert.ToString(dr["subsubvendor"]),
                                VendorPONo = Convert.ToString(dr["subVendorPO"]),
                                TopSubVendorPONo = Convert.ToString(dr["subsubVendor_Po_No"]),
                                MandayRate = Convert.ToString(dr["MandayRate"]),
                                //RefDocument = Convert.ToString(dr["RefDocument"]),
                                // Attachment = Convert.ToString(dr["VAttachment"]),
                                //insopectionRecord = Convert.ToString(dr["DetailsDocument"]),
                                //added by shrutika salve 14062024
                                CustomerRepresentative = Convert.ToString(dr["CustomerRepresentativeName"]),



                            });
                    }
                    ViewData["List"] = lstComplaintDashBoard;
                    obj1.lstComplaintDashBoard1 = lstComplaintDashBoard;

                }

                else
                {
                    ViewData["List"] = Session["lstComplaintDashBoard"];
                    obj1.lstComplaintDashBoard1 = lstComplaintDashBoard;

                    return View(obj1);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            return View(obj1);
        }

        [HttpPost]
        public ActionResult TimeSheetData(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
            DataSet ds = new DataSet();

            //ds = DALobj.GetDataByDate(LD); // fill dataset  


            //Session["FromDate"] = LD.FromD;
            //Session["ToDate"] = LD.ToD;

            Session["FromDate2"] = CM.FromDateI;
            Session["ToDate2"] = CM.ToDateI;
            Session["Job2"] = CM.Job;
            Session["inpector2"] = CM.Inspector;



            return RedirectToAction("TimeSheetData");


            return View();
        }


        public ActionResult ExportIndex5(CallsModel U)
        {
            // Using EPPlus from nuget
            string visitreportLink = string.Empty;
            string AttachmentLink = string.Empty;
            string FinalAttch = string.Empty;
            string PreviousText = string.Empty;
            string PreviousReport = string.Empty;
            bool rowincremented = false;
            string FinalURLAttachmentLink = string.Empty;



            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGrid5(U);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                // sheet = package.Workbook.Worksheets.Add("Sheet1");
                //sheet.Cells["A1"].Value = "TUV India Private Ltd.";
                //var cell = sheet.Cells["A1"];
                //cell.Style.Fill.PatternType = ExcelFillStyle.Solid;


                //sheet.Cells["A2"].Value = "Period:";
                //var cell3 = sheet.Cells["A3"];
                //// Replace 12 with your desired font size
                //cell3.Style.Font.Bold = true;
                //sheet.Cells["B2"].Value = Session["FromDate1"].ToString();
                //sheet.Cells["C3"].Value = Session["ToDate1"].ToString();

                string StyleName = "HyperStyle";

                ExcelNamedStyleXml HyperStyle = sheet.Workbook.Styles.CreateNamedStyle(StyleName);
                HyperStyle.Style.Font.UnderLine = true;
                HyperStyle.Style.Font.Size = 12;
                HyperStyle.Style.Font.Color.SetColor(System.Drawing.Color.Blue);





                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 20;
                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallsModel> gridRow in grid.Rows)
                {
                    col = 1;
                    rowincremented = false;
                    foreach (IGridColumn column in grid.Columns)
                    {

                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    }
                    if (!rowincremented)
                    {
                        row++;
                    }


                }

                return File(package.GetAsByteArray(), "application/unknown", "InvoiceData_" + DateTime.Now.ToShortDateString() + ".xlsx");

            }
        }

        private IGrid<CallsModel> CreateExportableGrid5(CallsModel U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetData5(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Job).Titled("Job Number");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub job Number");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Number");
            grid.Columns.Add(model => model.VendorName).Titled("sub Vendor Name");
            grid.Columns.Add(model => model.TopSubVendorName).Titled("sub/sub Vendor Name");
            grid.Columns.Add(model => model.VendorPONo).Titled("sub Vendor PO Number");
            grid.Columns.Add(model => model.TopSubVendorPONo).Titled("sub/sub Vendor PO Number");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.TUVEmpCode).Titled("TUV Emp Code");
            grid.Columns.Add(model => model.SAPEmpCode).Titled("SAP Emp Code");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.ExecutingBranch).Titled("Executing Branch");
            grid.Columns.Add(model => model.MandayRate).Titled("Manday Rate in INR");
            grid.Columns.Add(model => model.Onsite).Titled("Onsite Time");
            grid.Columns.Add(model => model.Offsite).Titled("Offsite Time");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.Status).Titled("call status");
            grid.Columns.Add(model => model.PresentDay).Titled("Full/Half Day");
            grid.Columns.Add(model => model.IVR).Titled("Visit Report");
            //grid.Columns.Add(model => model.insopectionRecord).Titled("insopection Record");




            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = obj1.lstComplaintDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }



        //get excel Data//

        public List<CallsModel> GetData5(CallsModel U)
        {

            DataTable DTCallDashBoard = new DataTable();
            List<CallsModel> lstCallDashBoard = new List<CallsModel>();
            if (Session["FromDate2"] != null && Session["ToDate2"] != null || Session["Job2"] != null)
            {

                obj1.FromDateI = Convert.ToString(Session["FromDate2"].ToString());
                obj1.ToDateI = Convert.ToString(Session["ToDate2"].ToString());
                obj1.Job = Convert.ToString(Session["Job2"]?.ToString()) ?? "";
                //obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                obj1.Inspector = Convert.ToString(Session["inpector2"]?.ToString()) ?? "";

                DTCallDashBoard = Dalobj.TimeDataExcel(obj1);
            }

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new CallsModel
                            {


                                FromDateI = Convert.ToString(dr["FromDate"]),
                                ToDateI = Convert.ToString(dr["ToDate"]),
                                Job = Convert.ToString(dr["job"]),
                                SAP_Number = Convert.ToString(dr["SAP_No"]),
                                Company_Name = Convert.ToString(dr["Company_Name"]),
                                Actual_Visit_Date = Convert.ToString(dr["VisitDate"]),
                                Call_No = Convert.ToString(dr["Call_no"]),
                                Sub_Job = Convert.ToString(dr["Sub_Job"]),
                                //VendorName = Convert.ToString(dr["Vendor_Name"]),
                                ExecutingBranch = Convert.ToString(dr["Executing_Branch"]),
                                Inspector = Convert.ToString(dr["inspectorName"]),
                                Onsite = Convert.ToString(dr["StartTime"]),
                                Offsite = Convert.ToString(dr["EndTime"]),
                                TravelTime = Convert.ToString(dr["TravelTime"]),
                                IVR = Convert.ToString(dr["Report"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                //insopectionRecord = Convert.ToString(dr["VAttachment"]),
                                Status = Convert.ToString(dr["Status"]),
                                PresentDay = Convert.ToString(dr["Present"]),
                                VendorName = Convert.ToString(dr["subvendor"]),
                                TopSubVendorName = Convert.ToString(dr["subsubvendor"]),
                                VendorPONo = Convert.ToString(dr["subVendorPO"]),
                                TopSubVendorPONo = Convert.ToString(dr["subsubVendor_Po_No"]),
                                MandayRate = Convert.ToString(dr["MandayRate"]),




                            }
                            );
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["List"] = lstCallDashBoard;

            obj1.lstComplaintDashBoard1 = lstCallDashBoard;


            return obj1.lstComplaintDashBoard1;
        }


        public ActionResult MISFollowUP()
        {
            Session["UserLoginID"] = User.Identity.IsAuthenticated;
            string UserRole = Convert.ToString(Session["role"]);
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALQuotationMast.GetFollowUpData();
            ViewData["QuotationMaster"] = lstQuotationMast;
            ObjModelQuotationMast.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(ObjModelQuotationMast);
        }


        [HttpGet]
        public ActionResult ExportIndex_FollowUp(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuotationMaster> grid = CreateExportableGrid(Type);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<QuotationMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd/MM/yyyy" + '-' + "HH:mm:ss");

                string filename = "QuotationMaster-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<QuotationMaster> CreateExportableGrid(String Type)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<QuotationMaster> grid = new Grid<QuotationMaster>(GetData(Type));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            //grid.Columns.Add(model => model.Quotation_Description).Titled("Description");
            //grid.Columns.Add(model => model.Reference).Titled("Reference");
            //grid.Columns.Add(model => model.Enquiry).Titled("Enquiry");
            //grid.Columns.Add(model => model.ExpiryDate).Titled("Expiry Date");
            //grid.Columns.Add(model => model.StatusType).Titled("Status");
            //grid.Columns.Add(model => model.ApprovalStatus).Titled("Approval Status");

            grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            /// .Encoded(false).RenderedAs(o => Html.ActionLink(o.QuotationNumber, "Quotation", new { PK_QM_ID = o.PK_QTID }, 
            /// new { title = "Quotation Number New" })).Filterable(true);

           
            grid.Columns.Add(model => model.InspectionLocation).Titled("Inspection Location");
            grid.Columns.Add(model => model.StatusType).Titled("Status");

           
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.CreatedDate).Titled("Created Date");
         
            grid.Columns.Add(model => model.ProjectName).Titled("OBS Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Service Portfolio Type");
            grid.Columns.Add(model => model.SubServiceType).Titled("Service Type");
            grid.Columns.Add(model => model.Followupddate).Titled("Followup date");
            grid.Columns.Add(model => model.Description).Titled("Followup Discription");
            grid.Columns.Add(model => model.followcnt).Titled("Followup Number");

            



            grid.Pager = new GridPager<QuotationMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ObjModelQuotationMast.lstQuotationMasterDashBoard1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<QuotationMaster> GetData(String Type)
        {

            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALQuotationMast.GetFollowUpData();

            ObjModelQuotationMast.lstQuotationMasterDashBoard1 = lstQuotationMast;


            return ObjModelQuotationMast.lstQuotationMasterDashBoard1;
        }



        //[HttpGet]
        //public JsonResult ExportToExcelSummary()
        //{
        //    try
        //    {
        //        DataSet dsHtml = new DataSet();
        //        dsHtml = objDMOR.GetMRNData();

        //        using (var excelPackage = new ExcelPackage())
        //        {
        //            var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

        //            // Title
        //            sheet.Cells["A1:E1"].Merge = true;
        //            sheet.Cells["A1"].Value = "Enquiry regretted/ Offer Lost Analysis (Month – May 2025)";
        //            var titleCell = sheet.Cells["A1"];
        //            titleCell.Style.Font.Size = 16;
        //            titleCell.Style.Font.Bold = true;
        //            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            // Header
        //            sheet.Cells["A3"].Value = "Enquiry/Quotation No";
        //            var cell = sheet.Cells["A3"];
        //            cell.Style.Font.Size = 12;
        //            cell.Style.Font.Bold = true;
        //            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

        //            sheet.Cells["B3"].Value = "Client";
        //            var cell1 = sheet.Cells["B3"];
        //            cell1.Style.Font.Size = 12;
        //            cell1.Style.Font.Bold = true;
        //            cell1.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell1.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

        //            sheet.Cells["C3"].Value = "Offer Value (Lacs)";
        //            var cell2 = sheet.Cells["C3"];
        //            cell2.Style.Font.Size = 12;
        //            cell2.Style.Font.Bold = true;
        //            cell2.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell2.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

        //            sheet.Cells["D3"].Value = "Reason for Regret/Loss";
        //            var cell3 = sheet.Cells["D3"];
        //            cell3.Style.Font.Size = 12;
        //            cell3.Style.Font.Bold = true;
        //            cell3.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell3.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);

        //            sheet.Cells["E3"].Value = "Action Plan";
        //            var cell4 = sheet.Cells["E3"];
        //            cell4.Style.Font.Size = 12;
        //            cell4.Style.Font.Bold = true;
        //            cell4.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            cell4.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        //            int j = 0;

        //            for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
        //            {
        //                int row = 4 + i;

        //                sheet.Cells[row, 1].Value = dsHtml.Tables[0].Rows[i]["EnquiryNumber"].ToString();
        //                sheet.Cells[row, 2].Value = dsHtml.Tables[0].Rows[i]["CompanyName"].ToString();                        
        //                sheet.Cells[row, 3].Value = dsHtml.Tables[0].Rows[i]["EstimatedAmount"].ToString();                      
        //                sheet.Cells[row, 4].Value = dsHtml.Tables[0].Rows[i]["RegretReason"].ToString();                        
        //                var range_ = sheet.Cells[$"A{row}:E{row}"];
        //                range_.Style.WrapText = true;
        //                range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //                // Alternate row color
        //                //if (i % 2 == 1)
        //                //{
        //                //    range_.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                //    range_.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
        //                //}

        //                j = 10 + i;
        //            }

        //            for (int i = 1; i <= 5; i++)
        //            {
        //                sheet.Column(i).Style.WrapText = false;
        //                sheet.Column(i).AutoFit();
        //                sheet.Column(i).Style.WrapText = true;
        //            }

        //            if (dsHtml.Tables.Count > 1 && dsHtml.Tables[1].Rows.Count > 0)
        //            {
        //                var sheet2 = excelPackage.Workbook.Worksheets.Add("SummaryDetails");


        //                sheet2.Cells[1, 1, 1, dsHtml.Tables[1].Columns.Count].Merge = true;
        //                sheet2.Cells[1, 1].Value = "Open Enquiry Analysis (Month – May 2025)";
        //                var titleCell2 = sheet2.Cells[1, 1];
        //                titleCell2.Style.Font.Size = 16;
        //                titleCell2.Style.Font.Bold = true;
        //                titleCell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                // Header Row Example
        //                for (int col = 0; col < dsHtml.Tables[1].Columns.Count; col++)
        //                {
        //                    var headerCell = sheet2.Cells[3, col + 1];
        //                    headerCell.Value = dsHtml.Tables[1].Columns[col].ColumnName;
        //                    headerCell.Style.Font.Bold = true;
        //                    headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    headerCell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        //                }

        //                // Data Rows
        //                for (int row = 0; row < dsHtml.Tables[1].Rows.Count; row++)
        //                {
        //                    for (int col = 0; col < dsHtml.Tables[1].Columns.Count; col++)
        //                    {
        //                        sheet2.Cells[row + 2, col + 1].Value = dsHtml.Tables[1].Rows[row][col].ToString();
        //                    }
        //                }

        //                // AutoFit Columns
        //                for (int i = 1; i <= dsHtml.Tables[1].Columns.Count; i++)
        //                {
        //                    sheet2.Column(i).AutoFit();
        //                }
        //            }








        //            byte[] excelBytes = excelPackage.GetAsByteArray();
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=" + "testMRM" + ".xlsx");
        //            Response.BinaryWrite(excelBytes);
        //            Response.Flush();
        //            Response.End();
        //        }

        //        return Json("", JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }            
        //}
        [HttpGet]
        public ActionResult ExportToExcelSummary()
        {
            try
            {
                DataSet dsHtml = objDMOR.GetMRNData();

                using (var excelPackage = new ExcelPackage())
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                    // Dynamic month
                    string monthYear = DateTime.Now.ToString("MMMM yyyy");

                    // Title
                    sheet.Cells["A1:E1"].Merge = true;
                    sheet.Cells["A1"].Value = $"Enquiry regretted/ Offer Lost Analysis (Month – {monthYear})";
                    var titleCell = sheet.Cells["A1"];
                    titleCell.Style.Font.Size = 16;
                    titleCell.Style.Font.Bold = true;
                    titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Header row
                    StyleHeader(sheet.Cells["A3"], "Enquiry/Quotation No");
                    StyleHeader(sheet.Cells["B3"], "Client");
                    StyleHeader(sheet.Cells["C3"], "Offer Value (Lacs)");
                    StyleHeader(sheet.Cells["D3"], "Reason for Regret/Loss");
                    StyleHeader(sheet.Cells["E3"], "Action Plan");

                   
                    for (int i = 0; i < dsHtml.Tables[0].Rows.Count; i++)
                    {
                        int row = 4 + i;

                        sheet.Cells[row, 1].Value = dsHtml.Tables[0].Rows[i]["EnquiryNumber"].ToString();
                        sheet.Cells[row, 2].Value = dsHtml.Tables[0].Rows[i]["CompanyName"].ToString();
                        sheet.Cells[row, 3].Value = dsHtml.Tables[0].Rows[i]["EstimatedAmount"].ToString();
                        sheet.Cells[row, 4].Value = dsHtml.Tables[0].Rows[i]["RegretReason"].ToString();

                        var range_ = sheet.Cells[$"A{row}:E{row}"];
                        range_.Style.WrapText = true;
                        range_.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range_.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        // Optional: Add alternating row color
                        // if (i % 2 == 1)
                        // {
                        //     range_.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //     range_.Style.Fill.BackgroundColor.SetColor(Color.LightYellow);
                        // }
                    }

                    // Autofit columns
                    //for (int i = 1; i <= 5; i++)
                    //{
                    //    sheet.Column(i).Style.WrapText = false;
                    //    sheet.Column(i).AutoFit();
                    //    sheet.Column(i).Style.WrapText = true;
                    //}
                    foreach (var ws in excelPackage.Workbook.Worksheets)
                    {
                        AutoFitOrWrapAllColumns(ws, wrapThreshold: 50); // You can adjust threshold
                    }

                    // Handle second table if present
                    if (dsHtml.Tables.Count > 1 && dsHtml.Tables[1].Rows.Count > 0)
                    {
                        var sheet2 = excelPackage.Workbook.Worksheets.Add("SummaryDetails");

                        // Title
                        sheet2.Cells[1, 1, 1, dsHtml.Tables[1].Columns.Count].Merge = true;
                        sheet2.Cells[1, 1].Value = $"Open Enquiry Analysis (Month – {monthYear})";
                        var titleCell2 = sheet2.Cells[1, 1];
                        titleCell2.Style.Font.Size = 16;
                        titleCell2.Style.Font.Bold = true;
                        titleCell2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        // Header
                        for (int col = 0; col < dsHtml.Tables[1].Columns.Count; col++)
                        {
                            var headerCell = sheet2.Cells[3, col + 1];
                            headerCell.Value = dsHtml.Tables[1].Columns[col].ColumnName;
                            headerCell.Style.Font.Bold = true;
                            headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerCell.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
                        }

                        // Data
                        for (int row = 0; row < dsHtml.Tables[1].Rows.Count; row++)
                        {
                            for (int col = 0; col < dsHtml.Tables[1].Columns.Count; col++)
                            {
                                sheet2.Cells[row + 4, col + 1].Value = dsHtml.Tables[1].Rows[row][col].ToString();
                            }
                        }

                        //for (int i = 1; i <= dsHtml.Tables[1].Columns.Count; i++)
                        //{

                        //        sheet2.Column(i).AutoFit();
                        //    sheet.Column(i).Style.WrapText = true;

                        //}
                        foreach (var ws in excelPackage.Workbook.Worksheets)
                        {
                            AutoFitOrWrapAllColumns(ws, wrapThreshold: 50); // You can adjust threshold
                        }
                    }




                 
                    byte[] excelBytes = excelPackage.GetAsByteArray();
                    string fileName = "Enquiry_Summary.xlsx";
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    return File(excelBytes, contentType, fileName);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Failed to export Excel file.");
            }
        }

      
        private void StyleHeader(ExcelRange cell, string text)
        {
            cell.Value = text;
            cell.Style.Font.Size = 12;
            cell.Style.Font.Bold = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(Color.LightSkyBlue);
        }
        private void AutoFitOrWrapAllColumns(ExcelWorksheet sheet, int wrapThreshold = 50)
        {
            if (sheet.Dimension == null)
                return;

            int startRow = sheet.Dimension.Start.Row;
            int endRow = sheet.Dimension.End.Row;
            int startCol = sheet.Dimension.Start.Column;
            int endCol = sheet.Dimension.End.Column;

            for (int col = startCol; col <= endCol; col++)
            {
                bool shouldWrap = false;

                for (int row = startRow; row <= endRow; row++)
                {
                    string text = sheet.Cells[row, col].Text;
                    if (!string.IsNullOrWhiteSpace(text) && text.Length > wrapThreshold)
                    {
                        shouldWrap = true;
                        break;
                    }
                }

                if (shouldWrap)
                {
                    sheet.Column(col).Style.WrapText = true;
                    sheet.Column(col).Width = 50;
                }
                else
                {
                    sheet.Column(col).AutoFit();
                    sheet.Column(col).Style.WrapText = false;
                }
            }
        }


        public ActionResult MISNoninspectionactivity()
        {
            return View();
        }

    }
}