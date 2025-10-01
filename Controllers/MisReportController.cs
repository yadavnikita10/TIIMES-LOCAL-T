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
using System.Text;
using System.IO;
using OfficeOpenXml;
using NonFactors.Mvc.Grid;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System.Globalization;

namespace TuvVision.Controllers
{
    public class MisReportController : Controller
    {


        TechnicalCompetencyEvaluation objMTCE = new TechnicalCompetencyEvaluation();
        BranchMasters ObjModelCompany = new BranchMasters();//added by nikita on 14092023
        Users ObjModelUsers = new Users();//added by nikita on 14092023
        MonitorRecordData monitoringrecordata = new MonitorRecordData();//added by nikita on 14092023
        Internal_Audit_Report InterAud = new Internal_Audit_Report();//addded by nikita on 14092023
        DalMisReportQHSE DalMisReportQHSE = new DalMisReportQHSE();//addded by nikita on 14092023
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        DALMisReport objDALMisReport = new DALMisReport();
        EnquiryMaster objEM = new EnquiryMaster();
        QuotationMaster objQM = new QuotationMaster();
        NonInspectionActivity objNIA = new NonInspectionActivity();
        CallsModel objCM = new CallsModel();
        JobMasters objJM = new JobMasters();
        EmployeeAttendance objEA = new EmployeeAttendance();
        DALSAPInvoice ObjSAPInv = new DALSAPInvoice();
        InspectorData objInspData = new InspectorData();
        DALTrainingSchedule objDTS = new DALTrainingSchedule();
        CallAnalysis objCallAnalysis = new CallAnalysis();
        DALJob objJob = new DALJob();
        DalCallHistory Dalobj = new DalCallHistory();
        TrainingScheduleModel objTSM = new TrainingScheduleModel();

        // GET: MisReport
        public ActionResult EnquiryReport(string Type)
        {
            Session["GetExcelData"] = "Yes";
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            lstEnquiryMast = objDalEnquiryMaster.GetEnquiryListDashBoard(Type);
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }

        //Record Search By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        [HttpPost]
        public ActionResult EnquiryReport(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDalEnquiryMaster.GetDataByDateWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new EnquiryMaster
                       {
                           RefDate = Convert.ToString(dr["refDate"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           Notes = Convert.ToString(dr["Notes"]),
                           NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                           //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                           ContactNo = Convert.ToString(dr["ContactNo"]),
                           ContactName = Convert.ToString(dr["ContactName"]),
                           ARC = Convert.ToString(dr["ARC"]),
                           ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                           EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           EnquiryDescription = Convert.ToString(dr["Description"]),
                           ProjectName = Convert.ToString(dr["ProjectName"]),
                           Company_Name = Convert.ToString(dr["CompanyName"]),
                           Branch = Convert.ToString(dr["OriginatingBranch"]),
                           OpendateS = Convert.ToString(dr["DateOpened"]),
                           Source = Convert.ToString(dr["source"]),
                           EstCloseS = Convert.ToString(dr["EstClose"]),
                           ProjectType = Convert.ToString(dr["ProjectType"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           //SubServiceType = Convert.ToString(dr["ServiceType"]),
                           Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                           Status = Convert.ToString(dr["status"]),
                           RegretReason = Convert.ToString(dr["RegretReason"]),
                           //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                           Owner = Convert.ToString(dr["OwnerName"]),
                           //DEstimatedAmount ="", //Convert.ToString(dr["DEstimatedAmount"]),
                           //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                           DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                           IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                           // TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                           Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                           Intlocation = Convert.ToString(dr["Intlocation"]),
                           Dcurrency = Convert.ToString(dr["Dcurrency"]),
                           Icurrency = Convert.ToString(dr["ICurrency"]),
                           LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           RegretStatus = Convert.ToString(dr["EStatus"]),
                           CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                           Quotation = Convert.ToString(dr["Quotation"]),
                           JobNumber = Convert.ToString(dr["JobNumber"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objEM.lst1 = lstEnquiryMast;
                return View(objEM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }

        public ActionResult QuotationReport()
        {
            Session["GetExcelData"] = "Yes";
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALMisReport.QuotaionMastertDashBoard();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
            //return View();
        }
        [HttpPost]
        public ActionResult QuotationReport(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();
            DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDate(FromDate, ToDate);
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           ILostReason = Convert.ToString(dr["ILostReason"]),
                           DLostReason = Convert.ToString(dr["DLostReason"]),
                           //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                           IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),//added by nikita 29082023
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),//added by nikita 29082023
                           ModifiedBy = Convert.ToString(dr["ModifiedBy"]),//added by nikita 29082023

                           Type = Convert.ToString(dr["OBSType"])//added by nikita 29082023
                       }
                    );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return View(objQM);
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
        }

        public ActionResult TimeSheetReport()
        {
            Session["GetExcelData"] = "Yes";
            List<NonInspectionActivity> lstQuotationMast = new List<NonInspectionActivity>();
            lstQuotationMast = objDALMisReport.timesheetReportDashBoard();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objNIA.lst1 = lstQuotationMast;
            return View(objNIA);
        }

        [HttpPost]
        public ActionResult TimeSheetReport(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;

            int abc = 0;
            string totaltime = null;
            List<NonInspectionActivity> lstQuotationMast = new List<NonInspectionActivity>();
            DataTable DTSearchRecordByDate = new DataTable();
            DTSearchRecordByDate = objDALMisReport.GetTMSearchRecordByDate(FromDate, ToDate);

            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    abc = Convert.ToInt32(dr["TotalTime"]);

                    if (abc >= 5)
                    {
                        totaltime = "1";
                    }
                    else
                    {
                        totaltime = "0.5";
                    }

                    lstQuotationMast.Add(
                       new NonInspectionActivity
                       {
                           Id = Convert.ToInt32(dr["Id"]),
                           ActivityType = Convert.ToString(dr["ActivityType"]),
                           Date = Convert.ToDateTime(dr["StartDate"]),
                           StartTime = Convert.ToDouble(dr["StartTime"]),
                           EndTime = Convert.ToDouble(dr["EndTime"]),
                           TravelTime = Convert.ToDouble(dr["TravelTime"]),
                           Description = Convert.ToString(dr["Description"]),

                           ManDays = Convert.ToString(totaltime),

                           TotalTime = Convert.ToInt32(dr["TotalTime"]),
                           CreatedBy = Convert.ToString(dr["FirstName"]),
                           Branch = Convert.ToString(dr["Branch_Name"]),
                           EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                           Sub_Job = Convert.ToString(dr["Sub_Job"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),

                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Job_Location = Convert.ToString(dr["Job_Location"]),
                           ExcutingBranch = Convert.ToString(dr["Branch_Name1"]),
                           OriganatingBranch = Convert.ToString(dr["Originating_Branch"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objNIA.lst1 = lstQuotationMast;
                return View(objNIA);
            }

            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objNIA.lst1 = lstQuotationMast;
            return View(objNIA);
        }

        public ActionResult PendingIVR()
        {
            Session["GetExcelData"] = "Yes";
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            lstQuotationMast = objDALMisReport.PendingIVRMIS();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objCM.lst1 = lstQuotationMast;
            return View(objCM);
        }
        [HttpPost]
        public ActionResult PendingIVR(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTSearchRecordByDate = new DataTable();
            DTSearchRecordByDate = objDALMisReport.GetSearchRecordByDateWisePendingIVRMIS(FromDate, ToDate);
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallsModel
                       {
                           PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           Company_Name = Convert.ToString(dr["Company_Name"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Sub_Job = Convert.ToString(dr["Sub_Job"]),
                           SAP_Number = Convert.ToString(dr["SAP_No"]),
                           PO_Number = Convert.ToString(dr["PO_Number"]),
                           Po_No_SSJob = Convert.ToString(dr["PO_No_SSJob"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Branch_Name = Convert.ToString(dr["Branch_Name"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           ProductList = Convert.ToString(dr["Product_item"]),
                           Status = Convert.ToString(dr["Status"]),
                           Contact_Name = Convert.ToString(dr["Contact_Name"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           Job_Location = Convert.ToString(dr["JOb_Location"]),
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Report_No = Convert.ToString(dr["Report_No"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           V1 = Convert.ToString(dr["V1"]),
                           V2 = Convert.ToString(dr["V2"]),
                           P1 = Convert.ToString(dr["P1"]),
                           P2 = Convert.ToString(dr["P2"]),
                           //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCM.lst1 = lstQuotationMast;
                return View(objCM);
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return View(objCM);
        }

        public ActionResult DealyIVR()
        {
            Session["GetExcelData"] = "Yes";
            DataTable dtIVRMIS = new DataTable();
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            try
            { 
             dtIVRMIS = objDALMisReport.DealyIVRMIS();
                if (dtIVRMIS.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtIVRMIS.Rows)
                    {
                        lstQuotationMast.Add(
                           new CallsModel
                           {
                               PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                               Call_No = Convert.ToString(dr["Call_No"]),
                               Company_Name = Convert.ToString(dr["Company_Name"]),
                               Project_Name = Convert.ToString(dr["Project_Name"]),
                               Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                               Sub_Job = Convert.ToString(dr["Sub_Job"]),
                               SAP_Number = Convert.ToString(dr["SAP_No"]),
                               PO_Number = Convert.ToString(dr["PO_Number"]),
                               Po_No_SSJob = Convert.ToString(dr["PO_No_SSJob"]),
                               Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
                               Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                               ProductList = Convert.ToString(dr["Product_item"]),
                               Status = Convert.ToString(dr["Status"]),
                               Contact_Name = Convert.ToString(dr["Contact_Name"]),
                               Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                               Job_Location = Convert.ToString(dr["JOb_Location"]),
                               Inspector = Convert.ToString(dr["Inspector"]),
                               Report_No = Convert.ToString(dr["Report_No"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               Call_Type = Convert.ToString(dr["calltype"]),
                               //ReportDate = Convert.ToString(dr["CreatedDate"]),
                               //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),
                               V1 = Convert.ToString(dr["V1"]),
                               V2 = Convert.ToString(dr["V2"]),
                               P1 = Convert.ToString(dr["P1"]),
                               P2 = Convert.ToString(dr["P2"]),
                               DelayInINRSubmission = Convert.ToInt32(dr["delay"]),

                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    objCM.lst1 = lstQuotationMast;
                    return View(objCM);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            objCM.lst1 = lstQuotationMast;
            
            return View(objCM);
        }


        [HttpPost]
        public ActionResult DealyIVR(CallsModel CM)
        {
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTDelayIVRSearchByDate = new DataTable();
            try
            {
                Session["GetExcelData"] = null;
                Session["FromDate"] = CM.FromDate;
                Session["ToDate"] = CM.ToDate;             
                if (Session["Fromdate"] != null && Session["ToDate"] != null)
                {
                    DTDelayIVRSearchByDate = objDALMisReport.DealyIVRMISSearchByDate(CM.FromDate, CM.ToDate);
                }
                else
                {
                    DTDelayIVRSearchByDate = objDALMisReport.DealyIVRMIS();

                }

                if (DTDelayIVRSearchByDate.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                    {
                        lstQuotationMast.Add(
                           new CallsModel
                           {
                               PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                               Call_No = Convert.ToString(dr["Call_No"]),
                               Company_Name = Convert.ToString(dr["Company_Name"]),
                               Project_Name = Convert.ToString(dr["Project_Name"]),
                               Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                               Sub_Job = Convert.ToString(dr["Sub_Job"]),
                               SAP_Number = Convert.ToString(dr["SAP_No"]),
                               PO_Number = Convert.ToString(dr["PO_Number"]),
                               Po_No_SSJob = Convert.ToString(dr["PO_No_SSJob"]),
                               Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
                               Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                               ProductList = Convert.ToString(dr["Product_item"]),
                               Status = Convert.ToString(dr["Status"]),
                               Contact_Name = Convert.ToString(dr["Contact_Name"]),
                               Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                               Job_Location = Convert.ToString(dr["JOb_Location"]),
                               Inspector = Convert.ToString(dr["Inspector"]),
                               Report_No = Convert.ToString(dr["Report_No"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               Call_Type = Convert.ToString(dr["calltype"]),
                               //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),
                               V1 = Convert.ToString(dr["V1"]),
                               V2 = Convert.ToString(dr["V2"]),
                               P1 = Convert.ToString(dr["P1"]),
                               P2 = Convert.ToString(dr["P2"]),

                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    objCM.lst1 = lstQuotationMast;
                    return View(objCM);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return View(objCM);
        }

        public ActionResult DealyIRN()
        {
            Session["GetExcelData"] = "Yes";
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            lstQuotationMast = objDALMisReport.DealyIRNMIS();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objCM.lst1 = lstQuotationMast;
            return View(objCM);
        }

        [HttpPost]
        public ActionResult DealyIRN(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTDelayIVRSearchByDate = new DataTable();
            DTDelayIVRSearchByDate = objDALMisReport.DealyIRNMISSearchByDate(FromDate, ToDate);
            if (DTDelayIVRSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallsModel
                       {
                           PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           Company_Name = Convert.ToString(dr["ClientName"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Sub_Job = Convert.ToString(dr["SubJob_No"]),
                           SAP_Number = Convert.ToString(dr["SAP_No"]),
                           PO_Number = Convert.ToString(dr["PO_No"]),
                           Po_No_SSJob = Convert.ToString(dr["PO_No_SubVendor"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           ProductList = Convert.ToString(dr["Inspected_Items"]),
                           Status = Convert.ToString(dr["Status"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           Job_Location = Convert.ToString(dr["InspectionLocation"]),
                           Inspector = Convert.ToString(dr["Visiting_InspectorName"]),
                           CreatedDate = Convert.ToString(dr["IRNSubmissionDate"]),
                           DelayInINRSubmission = Convert.ToInt32(dr["DelayInIRNSubmissionDay"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCM.lst1 = lstQuotationMast;
                return View(objCM);
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return View(objCM);
        }
        public ActionResult InvoiceInstructionMis()
        {
            Session["GetExcelData"] = "Yes";
            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            lstQuotationMast = objDALMisReport.InvoiceinstructionMIS();
            ViewData["QuotationMaster"] = lstQuotationMast;

            objJM.lstCompanyDashBoard1 = lstQuotationMast;
            return View(objJM);
        }
        [HttpPost]
        public ActionResult InvoiceInstructionMis(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            DataTable DTIIMSearchByDate = new DataTable();
            DTIIMSearchByDate = objDALMisReport.InvoiceinstructionMISSearchByDate(FromDate, ToDate);
            if (DTIIMSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTIIMSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new JobMasters
                       {
                           Branch = Convert.ToString(dr["Branch"]),
                           Client_Name = Convert.ToString(dr["Client_Name"]),
                           Job_type = Convert.ToString(dr["Job_type"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),
                           Job_Number = Convert.ToString(dr["Job_Number"]),
                           Customer_PO_Amount = Convert.ToDecimal(dr["Customer_PO_Amount"]),
                           InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                           FirstYear = Convert.ToDecimal(dr["FirstYear"]),
                           SecondYear = Convert.ToDecimal(dr["SecondYear"]),
                           ThirdYear = Convert.ToDecimal(dr["ThirdYear"]),
                           Currency = Convert.ToString(dr["Currency"]),
                           FourthYear = Convert.ToDecimal(dr["FourthYear"]),
                           Customer_PoNo_PoDate = Convert.ToString(dr["Customer_PoNo_PoDate"]),
                           PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           JobDate = Convert.ToString(dr["CreatedDate"]),
                           pendingAmount = Convert.ToDecimal(dr["Customer_PO_Amount"]) - Convert.ToDecimal(dr["InvoiceAmount"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lstCompanyDashBoard1 = lstQuotationMast;
                return View(objJM);
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lstCompanyDashBoard1 = lstQuotationMast;
            return View(objJM);
        }



        public ActionResult GenerateInvoice(FormCollection fc)
        {
            string FromDate = string.Empty;
            string ToDate = string.Empty;

            DataTable dtGenerateInvoice = new DataTable();
            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            dtGenerateInvoice = objDALMisReport.GenerateInvoiceData(FromDate, ToDate);
            if (dtGenerateInvoice.Rows.Count > 0)
            {
                foreach (DataRow dr in dtGenerateInvoice.Rows)
                {
                    lstQuotationMast.Add(
                           new JobMasters
                           {
                               Count = dtGenerateInvoice.Rows.Count,
                               SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                               CUSTSAPID = Convert.ToString(dr["CUSTSAPID"]),
                               MATID = Convert.ToString(dr["MATID"]),
                               Qty = Convert.ToString(dr["Qty"]),
                               NetAmount = Convert.ToString(dr["NetAmount"]),
                               refno = Convert.ToString(dr["refno"]),
                               INVNO = Convert.ToString(dr["INVNO"]),
                               text3 = Convert.ToString(dr["text3"]),
                               rptNo = Convert.ToString(dr["rptNo"]),
                               PC = Convert.ToString(dr["PC"]),
                               InvDate = Convert.ToString(dr["InvDate"]),

                           }
                         );
                }

            }
            else
            {

                TempData.Keep();
                objJM.lstCompanyDashBoard1 = lstQuotationMast;
                return View(objJM);
            }
            ViewData["QuotationMaster"] = lstQuotationMast;

            objJM.lstCompanyDashBoard1 = lstQuotationMast;
            return View(objJM);
        }

        [HttpPost]
        public ActionResult GenerateInvoice(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            DataTable DTIIMSearchByDate = new DataTable();
            DTIIMSearchByDate = objDALMisReport.GenerateInvoiceData(FromDate, ToDate);

            if (DTIIMSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTIIMSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                           new JobMasters
                           {
                               Count = DTIIMSearchByDate.Rows.Count,
                               SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                               CUSTSAPID = Convert.ToString(dr["CUSTSAPID"]),
                               MATID = Convert.ToString(dr["MATID"]),
                               Qty = Convert.ToString(dr["Qty"]),
                               NetAmount = Convert.ToString(dr["NetAmount"]),
                               refno = Convert.ToString(dr["refno"]),
                               INVNO = Convert.ToString(dr["INVNO"]),
                               text3 = Convert.ToString(dr["text3"]),
                               rptNo = Convert.ToString(dr["rptNo"]),
                               PC = Convert.ToString(dr["PC"]),
                               InvDate = Convert.ToString(dr["InvDate"]),

                           }
                         );
                }

            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lstCompanyDashBoard1 = lstQuotationMast;
                return View(objJM);
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lstCompanyDashBoard1 = lstQuotationMast;
            return View(objJM);
        }

        public ActionResult SAPInvoice()
        {
            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        public ActionResult SAPInvoice(SAPInvoice SAPinv, HttpPostedFileBase FileUpload)
        {
            string Result = string.Empty;
            var data = AuthenticationManager.User.Identity;
            var UID = data.GetUserId();



            HttpPostedFileBase files = FileUpload;

            if (files.ContentLength > 0 && !string.IsNullOrEmpty(files.FileName) && files.FileName.Contains(".xlsx"))
            {
                try
                {

                    string fileName = files.FileName;
                    string fileContentType = files.ContentType;
                    byte[] fileBytes = new byte[files.ContentLength];
                    var data1 = files.InputStream.Read(fileBytes, 0, Convert.ToInt32(files.ContentLength));
                    var package = new ExcelPackage(files.InputStream);

                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    int noOfCol = workSheet.Dimension.End.Column;
                    int noOfRow = workSheet.Dimension.End.Row;
                    int rowIterator = 1;
                    string strInvoice = string.Empty;

                    for (rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        try
                        {
                            ///Text Field
                            string strText = Convert.ToString(workSheet.Cells[rowIterator, 34].Value);
                            int occurance = strText.IndexOf("INV/");
                            int blankoccurance = strText.IndexOf(' ', occurance);
                            int lenInvoice = blankoccurance - occurance;

                            strInvoice = strText.Substring(occurance, lenInvoice);



                            ///Text Field
                            SAPinv.InvoiceNumber = strInvoice;

                            ///SAP Invoice Number Reference Field
                            SAPinv.SAPInvNo = Convert.ToString(workSheet.Cells[rowIterator, 5].Value);

                            Result = ObjSAPInv.UpdateSAPInvoice(SAPinv);
                            if (Result != "" && Result != null)
                            {
                                TempData["UpdateFeedBack"] = Result;

                            }


                        }
                        catch (Exception ex)
                        {

                            string error = ex.Message.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Message"] = ex.Message;
                    return RedirectToAction("InvoiceInstructionMIS");
                }


            }
            return RedirectToAction("InvoiceInstructionMIS");

        }

        public ActionResult EmployeeAttendance()
        {

            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<EmployeeAttendance> lstEmployeeAttendance = new List<EmployeeAttendance>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);
                DTEmployeeAttendanceSearchByDate = objDALMisReport.EmployeeAttendanceSearchByDate(FromDate, ToDate);
                if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                    {
                        lstEmployeeAttendance.Add(
                           new EmployeeAttendance
                           {
                                InspectorName = Convert.ToString(dr["InspectorName"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
                               EmployeeCode = Convert.ToString(dr["WorkFromHome"]),
                               ActivityType = Convert.ToString(dr["InspectionActivity"]),
                               Job = Convert.ToString(dr["Marketing"]),
                               Sub_Job = Convert.ToString(dr["Training"]),
                               SAP_No = Convert.ToString(dr["QA"]),
                               Project_Name = Convert.ToString(dr["office"]),
                               Job_Location = Convert.ToString(dr["NonClearableActivity"]),
                               Company_Name = Convert.ToString(dr["PL"]),
                               CL = Convert.ToString(dr["CL"]),
                               SL = Convert.ToString(dr["SL"]),
                               StartTime = Convert.ToString(dr["TotalManDays"]),
                               Id = Convert.ToString(dr["Id"]),
                               Availibity = Convert.ToString(Math.Round(((Convert.ToDouble(dr["TotalManDays"]) / 26) * 100))) + "%",
                               Availibity22 = Convert.ToString(Math.Round(((Convert.ToDouble(dr["TotalManDays"]) / 22) * 100))) + "%",

                           }
                         );
                    }
                }
            }
            else
            {

                lstEmployeeAttendance = objDALMisReport.EmployeeAttendanceMIS();
                
            }

            
            ViewData["EmployeeAttendance"] = lstEmployeeAttendance;
            objEA.lst1 = lstEmployeeAttendance;
            return View(objEA);
        }

        [HttpPost]
        public ActionResult EmployeeAttendance(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            List<EmployeeAttendance> lstEmployeeAttendanceSearchByDate = new List<EmployeeAttendance>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            DTEmployeeAttendanceSearchByDate = objDALMisReport.EmployeeAttendanceSearchByDate(FromDate, ToDate);
            if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                {
                    lstEmployeeAttendanceSearchByDate.Add(
                       new EmployeeAttendance
                       {
                           //PK_UserID = Convert.ToString(dr["PK_UserID"]),
                           //BranchName = Convert.ToString(dr["BranchName"]),
                           //NameOfEmployee = Convert.ToString(dr["EmployeeName"]),
                           //EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                           //EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                           //DateOfActivity = Convert.ToString(dr["DateOfActivity"]),
                           //ActivityType = Convert.ToString(dr["ActivityType"]),
                           //SubJobNumber = Convert.ToString(dr["SubJobNumber"]),
                           //Description = Convert.ToString(dr["Description"]),
                           //ActivityHours = Convert.ToInt32(dr["ActivityHours"]),

                           //Date = Convert.ToString(dr["Date"]),
                           InspectorName = Convert.ToString(dr["InspectorName"]),
                           Branch_Name = Convert.ToString(dr["Branch_Name"]),
                           EmployeeCode = Convert.ToString(dr["WorkFromHome"]),
                           ActivityType = Convert.ToString(dr["InspectionActivity"]),
                           Job = Convert.ToString(dr["Marketing"]),
                           Sub_Job = Convert.ToString(dr["Training"]),
                           SAP_No = Convert.ToString(dr["QA"]),
                           Project_Name = Convert.ToString(dr["office"]),
                           Job_Location = Convert.ToString(dr["NonClearableActivity"]),
                           Company_Name = Convert.ToString(dr["PL"]),
                           CL = Convert.ToString(dr["CL"]),
                           SL = Convert.ToString(dr["SL"]),
                           StartTime = Convert.ToString(dr["TotalManDays"]),
                           Id = Convert.ToString(dr["Id"]),
                           Availibity = Convert.ToString( Math.Round( ((Convert.ToDouble(dr["TotalManDays"]) / 26) * 100))) + "%",
                           Availibity22 = Convert.ToString(Math.Round(((Convert.ToDouble(dr["TotalManDays"]) / 22) * 100))) + "%",

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objEA.lst1 = lstEmployeeAttendanceSearchByDate;
                return View(objEA);
            }
            ViewData["EmployeeAttendance"] = lstEmployeeAttendanceSearchByDate;
            TempData["Result"] = null;
            TempData.Keep();
            objEA.lst1 = lstEmployeeAttendanceSearchByDate;
            return View(objEA);
        }


        #region export to excel EnquiryReport
        [HttpGet]
        public ActionResult ExportIndexEnquiryReport(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EnquiryMaster> grid = CreateExportableGridEnquiryReport(Type);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EnquiryMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "EnquiryReport-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<EnquiryMaster> CreateExportableGridEnquiryReport(string Type)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<EnquiryMaster> grid = new Grid<EnquiryMaster>(GetDataEnquiryReport(Type));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.OpendateS).Titled("Open Date").Css("aa");
            //grid.Columns.Add(model => model.EnquiryDescription).Titled("Description");
            //grid.Columns.Add(model => model.ProjectType).Titled("Project Name");
            //grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");
            //grid.Columns.Add(model => model.Type).Titled("Service Type");
            //grid.Columns.Add(model => model.CompanyName).Titled("Client Name");
            //grid.Columns.Add(model => model.Branch).Titled("Originating Branch");
            //grid.Columns.Add(model => model.EstClose).Titled("Est Close Date");
            //grid.Columns.Add(model => model.RegretStatus).Titled("Quoted ( Along with quotation number ) / Regreted");
            //grid.Columns.Add(model => model.RegretReason).Titled("Regret Reason");
            //grid.Columns.Add(model => model.CreatedBy).Titled("Owner");

            //grid.Columns.Add(model => model.DEstimatedAmount).Titled("Domestic Amount");
            //grid.Columns.Add(model => model.IEstimatedAmount).Titled("International Amount");
            //grid.Columns.Add(model => model.TEstimatedAmount).Titled("Total Estimated Amount");

            //added by nikita 31-08-2023
            grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.Source).Titled("Source");
            grid.Columns.Add(c => c.RefDate).Titled("Enquiry receipt date");
            grid.Columns.Add(c => c.EstCloseS).Titled("Estimated closure date");
            grid.Columns.Add(c => c.EnquiryReferenceNo).Titled("Enquiry reference details");
            grid.Columns.Add(c => c.Company_Name).Titled("Customer Name");
            grid.Columns.Add(c => c.EndCustomer).Titled("End customer name");
            grid.Columns.Add(c => c.ProjectType).Titled("OBS Type");
            grid.Columns.Add(c => c.PortfolioType).Titled("Portfolio Type");
            grid.Columns.Add(c => c.Type).Titled("Service Type");
            grid.Columns.Add(c => c.ProjectName).Titled("Project Name");
            grid.Columns.Add(c => c.EnquiryDescription).Titled("Description");
            grid.Columns.Add(c => c.ModifiedBy).Titled("Modified by");
            grid.Columns.Add(c => c.ARC).Titled("ARC Job");
            grid.Columns.Add(c => c.Domesticlocation).Titled("Domestic");
            grid.Columns.Add(c => c.Intlocation).Titled("International");
            grid.Columns.Add(c => c.DEstimatedAmount).Titled("Domestic Amount");
            grid.Columns.Add(c => c.IEstimatedAmount).Titled("International Amount");
            grid.Columns.Add(c => c.Status).Titled("Enquiry Status");
            grid.Columns.Add(c => c.RegretReason).Titled("Regret Reason");
            grid.Columns.Add(c => c.ContactName).Titled("Customer Contact Person Name");
            grid.Columns.Add(c => c.ContactNo).Titled("Customer Contact Person Number");
            grid.Columns.Add(c => c.Owner).Titled("Enquiry Created By");
            grid.Columns.Add(c => c.LeadGivenBy).Titled("Lead Given By");
            grid.Columns.Add(c => c.NotesbyLeads).Titled("Notes by Leads");
            grid.Columns.Add(c => c.OpendateS).Titled("Creation Date");
            grid.Columns.Add(c => c.OrderType).Titled("Order Type");
            grid.Columns.Add(c => c.ModifiedDateS).Titled("Modification Date");
            grid.Columns.Add(c => c.Quotation).Titled("Quotation Number");
            grid.Columns.Add(c => c.JobNumber).Titled("Job Number");


            grid.Pager = new GridPager<EnquiryMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEM.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            return grid;
          
        }

        public List<EnquiryMaster> GetDataEnquiryReport(string Type)
        {



            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();

            string FromDate = string.Empty;
            string ToDate = string.Empty;

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);

                DTSearchByDateWiseData = objDalEnquiryMaster.GetDataByDateWise(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add
                        (
                           new EnquiryMaster
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount ="", //Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               // TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               RegretStatus = Convert.ToString(dr["EStatus"]),
                               CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                               Quotation = Convert.ToString(dr["Quotation"]),
                               JobNumber = Convert.ToString(dr["JobNumber"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                    objEM.lst1 = lstEnquiryMast;
                    //return objEM.lst1;
                }
            }
            else
            {
                lstEnquiryMast = objDalEnquiryMaster.GetEnquiryListDashBoard(Type);
            }

            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;

            return objEM.lst1;
        }
        #endregion


        #region Export to excel Quotation Report

        [HttpGet]
        public ActionResult ExportIndexQuotationMaster()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuotationMaster> grid = CreateExportableGridQuotationMaster();
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

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "QuotationReport-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<QuotationMaster> CreateExportableGridQuotationMaster()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<QuotationMaster> grid = new Grid<QuotationMaster>(GetDataQuotationMaster());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation No");
            ////grid.Columns.Add(model => model.Quotation_Description).Titled("Description");
            //grid.Columns.Add(model => model.CreatedDate).Titled("Quotation Date");
            //grid.Columns.Add(model => model.ExpiryDate).Titled("Validity Date");
            //grid.Columns.Add(model => model.CompanyAddress).Titled("Client Name");
            //grid.Columns.Add(model => model.Quotation_Description).Titled("Project Name");
            //grid.Columns.Add(model => model.EstimatedAmount).Titled("Estimate Amount");
            //grid.Columns.Add(model => model.BranchName).Titled("Branch");
            ////grid.Columns.Add(model => model.ApprovalStatus).Titled("Approval Status");
            //grid.Columns.Add(model => model.DApprovalStatus).Titled("Domestic Approval Status");
            //grid.Columns.Add(model => model.IApprovalStatus).Titled("International Approval Status");
            //grid.Columns.Add(model => model.StatusType).Titled("Status");            
            ////grid.Columns.Add(model => model.Name).Titled("Originating Branch");
            //grid.Columns.Add(model => model.JobNo).Titled("Job No");
            //grid.Columns.Add(model => model.CreatedBy).Titled("Owner");

            grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            grid.Columns.Add(model => model.CreatedDate).Titled("Quotation Date");
            grid.Columns.Add(model => model.OrderType).Titled("Order Type");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Validity Date");
            grid.Columns.Add(model => model.EstimatedAmount).Titled("Estimate Amount");
            grid.Columns.Add(model => model.Quotation_Description).Titled("Project Name");
            grid.Columns.Add(model => model.Name).Titled("Service Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.Type).Titled("OBS Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.CompanyAddress).Titled("Client Name");
            grid.Columns.Add(model => model.BranchName).Titled("Originating Branch");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Est Close Date");
            grid.Columns.Add(model => model.StatusType).Titled("Status");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.DLostReason).Titled("Lost Reason (Domestic)");
            grid.Columns.Add(model => model.ILostReason).Titled("Lost Reason (International)");
            grid.Columns.Add(model => model.ICostSheetApproveStatus).Titled("International Costsheet Approval Status");
            grid.Columns.Add(model => model.DApprovalStatus).Titled("Domestic Costsheet Approval Status");

            grid.Pager = new GridPager<QuotationMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objQM.lstQuotationMasterDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<QuotationMaster> GetDataQuotationMaster()
        {

            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.QuotaionMastertDashBoard();
            }
            else
            {

                //U.FromDate = Session["FromDate"].ToString();
                //U.ToDate = Session["ToDate"].ToString();
                DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }


            
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           Count = DTSearchRecordByDate.Rows.Count,
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           ILostReason = Convert.ToString(dr["ILostReason"]),
                           DLostReason = Convert.ToString(dr["DLostReason"]),
                           //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                           IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),//added by nikita 29082023
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),//added by nikita 29082023
                           Type = Convert.ToString(dr["OBSType"])//added by nikita 29082023
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return objQM.lstQuotationMasterDashBoard1;
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return objQM.lstQuotationMasterDashBoard1;
        }

        #endregion


        #region Export To excel TimeSheetReport

        [HttpGet]
        public ActionResult ExportIndexTimeSheetReport()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<NonInspectionActivity> grid = CreateExportableGridTimeSheetReport();
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

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<NonInspectionActivity> CreateExportableGridTimeSheetReport()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<NonInspectionActivity> grid = new Grid<NonInspectionActivity>(GetDataTimeSheetReport());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Date.Value.ToString("dd/MM/yyyy")).Titled("Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.Location).Titled("Location");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor Name");
            grid.Columns.Add(model => model.ActivityType).Titled("Activity Type");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.StartTime).Titled("On Site Hrs");
            grid.Columns.Add(model => model.EndTime).Titled("Off Site Hrs");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.TotalTime).Titled("Total Time");
            grid.Columns.Add(model => model.OriganatingBranch).Titled("Originating Branch");
            grid.Columns.Add(model => model.ExcutingBranch).Titled("Executing Branch");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.ManDays).Titled("Man Days");


            grid.Pager = new GridPager<NonInspectionActivity>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objNIA.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<NonInspectionActivity> GetDataTimeSheetReport()
        {
            int abc = 0;
            string totaltime = null;
            List<NonInspectionActivity> lstQuotationMast = new List<NonInspectionActivity>();
            DataTable DTSearchRecordByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.timesheetReportDashBoard();
            }
            else
            {
                DTSearchRecordByDate = objDALMisReport.GetTMSearchRecordByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }

            

            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    abc = Convert.ToInt32(dr["TotalTime"]);

                    if (abc >= 5)
                    {
                        totaltime = "1";
                    }
                    else
                    {
                        totaltime = "0.5";
                    }

                    lstQuotationMast.Add(
                       new NonInspectionActivity
                       {
                           Count = DTSearchRecordByDate.Rows.Count,
                           Id = Convert.ToInt32(dr["Id"]),
                           ActivityType = Convert.ToString(dr["ActivityType"]),
                           Date = Convert.ToDateTime(dr["StartDate"]),
                           StartTime = Convert.ToDouble(dr["StartTime"]),
                           EndTime = Convert.ToDouble(dr["EndTime"]),
                           TravelTime = Convert.ToDouble(dr["TravelTime"]),
                           Description = Convert.ToString(dr["Description"]),

                           ManDays = Convert.ToString(totaltime),

                           TotalTime = Convert.ToInt32(dr["TotalTime"]),
                           CreatedBy = Convert.ToString(dr["FirstName"]),
                           Branch = Convert.ToString(dr["Branch_Name"]),
                           EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                           Sub_Job = Convert.ToString(dr["Sub_Job"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),

                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Job_Location = Convert.ToString(dr["Job_Location"]),
                           ExcutingBranch = Convert.ToString(dr["Branch_Name1"]),
                           OriganatingBranch = Convert.ToString(dr["Originating_Branch"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objNIA.lst1 = lstQuotationMast;
                return objNIA.lst1;
            }

            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objNIA.lst1 = lstQuotationMast;
            return objNIA.lst1;
        }

        #endregion

        #region Export To Excel Pending IVR
        [HttpGet]
        public ActionResult ExportIndexPendingIVR()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGridPendingIVR();
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
        private IGrid<CallsModel> CreateExportableGridPendingIVR()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetDataPendingIVR());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Call_No).Titled("Call Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");            
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.V1).Titled("Sub Job Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Job Vendor Name");
            grid.Columns.Add(model => model.SAP_Number).Titled("SAP No");
            grid.Columns.Add(model => model.P1).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Branch_Name).Titled("Executing Branch");
            grid.Columns.Add(model => model.ProductList).Titled("Inspected items");
            grid.Columns.Add(model => model.Status).Titled("Call status");
            grid.Columns.Add(c => "Single days").Titled("Single days/Contune");
            grid.Columns.Add(model => model.Job_Location).Titled("Inspection Location");
            grid.Columns.Add(model => model.Inspector).Titled("Visiting Inspector name");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Report_No).Titled("Visit Report Status");


            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objCM.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetDataPendingIVR()
        {

            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTSearchRecordByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.PendingIVRMIS();
            }
            else
            {                
                DTSearchRecordByDate = objDALMisReport.GetSearchRecordByDateWisePendingIVRMIS(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
            
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallsModel
                       {
                           Count = DTSearchRecordByDate.Rows.Count,
                           PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           Company_Name = Convert.ToString(dr["Company_Name"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Sub_Job = Convert.ToString(dr["Sub_Job"]),
                           SAP_Number = Convert.ToString(dr["SAP_No"]),
                           PO_Number = Convert.ToString(dr["PO_Number"]),
                           Po_No_SSJob = Convert.ToString(dr["PO_No_SSJob"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Branch_Name = Convert.ToString(dr["Branch_Name"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           ProductList = Convert.ToString(dr["Product_item"]),
                           Status = Convert.ToString(dr["Status"]),
                           Contact_Name = Convert.ToString(dr["Contact_Name"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           Job_Location = Convert.ToString(dr["JOb_Location"]),
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Report_No = Convert.ToString(dr["Report_No"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           V1 = Convert.ToString(dr["V1"]),
                           V2 = Convert.ToString(dr["V2"]),
                           P1 = Convert.ToString(dr["P1"]),
                           P2 = Convert.ToString(dr["P2"]),
                           //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCM.lst1 = lstQuotationMast;
                return objCM.lst1;
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return objCM.lst1;
        }
        #endregion


        #region Export to excel DealyIVR
        [HttpGet]
        public ActionResult ExportIndexDealyIVR()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGridDealyIVR();
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
        private IGrid<CallsModel> CreateExportableGridDealyIVR()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetDataDealyIVR());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Call_No).Titled("Call Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub-Sub Job No");
            grid.Columns.Add(model => model.V1).Titled("Sub Job Vendor Name");
            grid.Columns.Add(model => model.V2).Titled("Sub-Sub Job Vendor Name");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.P1).Titled("Sub Job PO No");
            grid.Columns.Add(model => model.P2).Titled("Sub-Sub Job PO No");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Branch_Name).Titled("Executing Branch");
            grid.Columns.Add(model => model.ProductList).Titled("Inspected Items");
            grid.Columns.Add(model => model.Status).Titled("Call Status");
            grid.Columns.Add(model => model.Call_Type).Titled("Single Days/Continue");
            grid.Columns.Add(model => model.Job_Location).Titled("Inspection Location");
            grid.Columns.Add(model => model.Inspector).Titled("Visiting Inspector Name");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(c => "Completed").Titled("Visit Report Status");
            grid.Columns.Add(model => model.ReportDate.Value.ToString("dd/MM/yyyy")).Titled("Report Submission Date");
            grid.Columns.Add(c => "").Titled("Delay In Visit Report Submission");


            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objCM.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetDataDealyIVR()
        {

            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTDelayIVRSearchByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                DTDelayIVRSearchByDate = objDALMisReport.DealyIVRMIS();
            }
            else
            {
                
                DTDelayIVRSearchByDate = objDALMisReport.DealyIVRMISSearchByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }

           
            if (DTDelayIVRSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallsModel
                       {
                           Count = DTDelayIVRSearchByDate.Rows.Count,
                           PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           Company_Name = Convert.ToString(dr["Company_Name"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Sub_Job = Convert.ToString(dr["Sub_Job"]),
                           SAP_Number = Convert.ToString(dr["SAP_No"]),
                           PO_Number = Convert.ToString(dr["PO_Number"]),
                           Po_No_SSJob = Convert.ToString(dr["PO_No_SSJob"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Branch_Name = Convert.ToString(dr["Branch_Name"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           ProductList = Convert.ToString(dr["Product_item"]),
                           Status = Convert.ToString(dr["Status"]),
                           Contact_Name = Convert.ToString(dr["Contact_Name"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           Job_Location = Convert.ToString(dr["JOb_Location"]),
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Report_No = Convert.ToString(dr["Report_No"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           Call_Type = Convert.ToString(dr["calltype"]),
                           //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),
                           V1 = Convert.ToString(dr["V1"]),
                           V2 = Convert.ToString(dr["V2"]),
                           P1 = Convert.ToString(dr["P1"]),
                           P2 = Convert.ToString(dr["P2"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCM.lst1 = lstQuotationMast;
                return objCM.lst1;
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return objCM.lst1;
        }
        #endregion


        #region Export To excel InvoiceInstructionMis
        [HttpGet]
        public ActionResult ExportIndexInvoiceInstructionMis()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridInvoiceInstructionMis();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }


        private IGrid<JobMasters> CreateExportableGridInvoiceInstructionMis()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataInvoiceInstructionMis());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Client_Name).Titled("Client");
            grid.Columns.Add(model => model.Job_type).Titled("Job Type");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.JobDate).Titled("Job Date");
            grid.Columns.Add(model => model.Customer_PO_Amount).Titled("Order Value");
            grid.Columns.Add(model => model.InvoiceAmount).Titled("Invoicing Amount");
            grid.Columns.Add(model => model.pendingAmount).Titled("Pending Amount");
            grid.Columns.Add(model => model.FirstYear).Titled("Probable invoicing for year 1");
            grid.Columns.Add(model => model.SecondYear).Titled("Probable invoicing in year 2");
            grid.Columns.Add(model => model.ThirdYear).Titled("Probable invoicing in year 3");
            grid.Columns.Add(model => model.FourthYear).Titled("Probable invoicing in year 4");
            grid.Columns.Add(model => model.Customer_PoNo_PoDate).Titled("Completed PO Numbers and Sub Job Numbers");


            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lstCompanyDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataInvoiceInstructionMis()
        {

            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            DataTable DTIIMSearchByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.InvoiceinstructionMIS();
            }
            else
            {
                DTIIMSearchByDate = objDALMisReport.InvoiceinstructionMISSearchByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }

            
            if (DTIIMSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTIIMSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new JobMasters
                       {
                           Count = DTIIMSearchByDate.Rows.Count,
                           Branch = Convert.ToString(dr["Branch"]),
                           Client_Name = Convert.ToString(dr["Client_Name"]),
                           Job_type = Convert.ToString(dr["Job_type"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),
                           Job_Number = Convert.ToString(dr["Job_Number"]),
                           Customer_PO_Amount = Convert.ToDecimal(dr["Customer_PO_Amount"]),
                           InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                           FirstYear = Convert.ToDecimal(dr["FirstYear"]),
                           SecondYear = Convert.ToDecimal(dr["SecondYear"]),
                           ThirdYear = Convert.ToDecimal(dr["ThirdYear"]),
                           FourthYear = Convert.ToDecimal(dr["FourthYear"]),
                           Customer_PoNo_PoDate = Convert.ToString(dr["Customer_PoNo_PoDate"]),
                           PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           JobDate = Convert.ToString(dr["CreatedDate"]),
                           pendingAmount = Convert.ToDecimal(dr["Customer_PO_Amount"]) - Convert.ToDecimal(dr["InvoiceAmount"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lstCompanyDashBoard1 = lstQuotationMast;
                return objJM.lstCompanyDashBoard1;
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lstCompanyDashBoard1 = lstQuotationMast;


            return objJM.lstCompanyDashBoard1;
        }

        #endregion



        #region Export To excel InvoiceInstructionMis
        [HttpGet]
        public ActionResult ExportGenerateInvoice()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridGenerateInvoice();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }


        private IGrid<JobMasters> CreateExportableGridGenerateInvoice()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataGenerateInvoice());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.SalesOrderNo).Titled("SalesOrderNo");
            grid.Columns.Add(model => model.CUSTSAPID).Titled("CUSTSAPID");
            grid.Columns.Add(model => model.MATID).Titled("MATID");
            grid.Columns.Add(model => model.Qty).Titled("Qty");
            grid.Columns.Add(model => model.NetAmount).Titled("NetAmount");
            grid.Columns.Add(model => model.refno).Titled("refno");
            grid.Columns.Add(model => model.INVNO).Titled("INVNO");
            grid.Columns.Add(model => model.text3).Titled("text3");
            grid.Columns.Add(model => model.rptNo).Titled("rptNo");
            grid.Columns.Add(model => model.PC).Titled("PC");
            grid.Columns.Add(model => model.InvDate).Titled("InvDate");
                    

                    

            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lstCompanyDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataGenerateInvoice()
        {

            List<JobMasters> lstQuotationMast = new List<JobMasters>();
            DataTable DTIIMSearchByDate = new DataTable();

           
            DTIIMSearchByDate = objDALMisReport.GenerateInvoiceData(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));

            if (DTIIMSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTIIMSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new JobMasters
                       {
                           Count = DTIIMSearchByDate.Rows.Count,
                           SalesOrderNo = Convert.ToString(dr["SalesOrderNo"]),
                           CUSTSAPID = Convert.ToString(dr["CUSTSAPID"]),
                           MATID = Convert.ToString(dr["MATID"]),
                           Qty = Convert.ToString(dr["Qty"]),
                           NetAmount = Convert.ToString(dr["NetAmount"]),
                           refno = Convert.ToString(dr["refno"]),
                           INVNO = Convert.ToString(dr["INVNO"]),
                           text3 = Convert.ToString(dr["text3"]),
                           rptNo = Convert.ToString(dr["rptNo"]),
                           PC = Convert.ToString(dr["PC"]),
                           InvDate = Convert.ToString(dr["InvDate"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lstCompanyDashBoard1 = lstQuotationMast;
                return objJM.lstCompanyDashBoard1;
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lstCompanyDashBoard1 = lstQuotationMast;


            return objJM.lstCompanyDashBoard1;
        }

        #endregion


        #region Export To Excel DealyIRN

        [HttpGet]
        public ActionResult ExportIndexDealyIRN()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallsModel> grid = CreateExportableGridDealyIRN();
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
        private IGrid<CallsModel> CreateExportableGridDealyIRN()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetDataDealyIRN());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Call_No).Titled("Call Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Project_Name).Titled("Project Name");
            grid.Columns.Add(model => model.Vendor_Name).Titled("Vendor");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Job Number");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.PO_Number).Titled("PO Number");
            grid.Columns.Add(model => model.Po_No_SSJob).Titled("PO Number on Sub-Vendor");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            grid.Columns.Add(model => model.ProductList).Titled("Inspected Items");
            grid.Columns.Add(c => "Single days").Titled("Single Days/Continue");
            grid.Columns.Add(model => model.Job_Location).Titled("Inspection Location");
            grid.Columns.Add(model => model.Inspector).Titled("Visiting Inspector Name");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Status).Titled("IRN Status");
            grid.Columns.Add(model => model.CreatedDate).Titled("Report Submission Date");
            grid.Columns.Add(model => model.DelayInINRSubmission).Titled("Delay In Visit Report Submission");





            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objCM.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallsModel> GetDataDealyIRN()
        {

            List<CallsModel> lstQuotationMast = new List<CallsModel>();
            DataTable DTDelayIVRSearchByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {

                lstQuotationMast = objDALMisReport.DealyIRNMIS();
            }
            else
            {

               
                DTDelayIVRSearchByDate = objDALMisReport.DealyIRNMISSearchByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }
           
            
            if (DTDelayIVRSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallsModel
                       {
                           Count = DTDelayIVRSearchByDate.Rows.Count,
                           PK_Call_ID = Convert.ToInt32(dr["PK_Call_ID"]),
                           Call_No = Convert.ToString(dr["Call_No"]),
                           Company_Name = Convert.ToString(dr["ClientName"]),
                           Project_Name = Convert.ToString(dr["Project_Name"]),
                           Vendor_Name = Convert.ToString(dr["Vendor_Name"]),
                           Sub_Job = Convert.ToString(dr["SubJob_No"]),
                           SAP_Number = Convert.ToString(dr["SAP_No"]),
                           PO_Number = Convert.ToString(dr["PO_No"]),
                           Po_No_SSJob = Convert.ToString(dr["PO_No_SubVendor"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           ProductList = Convert.ToString(dr["Inspected_Items"]),
                           Status = Convert.ToString(dr["Status"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           Job_Location = Convert.ToString(dr["InspectionLocation"]),
                           Inspector = Convert.ToString(dr["Visiting_InspectorName"]),
                           CreatedDate = Convert.ToString(dr["IRNSubmissionDate"]),
                           DelayInINRSubmission = Convert.ToInt32(dr["DelayInIRNSubmissionDay"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCM.lst1 = lstQuotationMast;
                return objCM.lst1;
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCM.lst1 = lstQuotationMast;
            return objCM.lst1;
        }

        #endregion


        #region Export to Excel EmployeeAttendance
        [HttpGet]
        public ActionResult ExportIndexEmployeeAttendance()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EmployeeAttendance> grid = CreateExportableGridEmployeeAttendance();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EmployeeAttendance> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<EmployeeAttendance> CreateExportableGridEmployeeAttendance()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<EmployeeAttendance> grid = new Grid<EmployeeAttendance>(GetDataEmployeeAttendance());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //grid.Columns.Add(model => model.BranchName).Titled("Branch Name");
            //grid.Columns.Add(model => model.NameOfEmployee).Titled("Name Of Employee");
            //grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");
            //grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            //grid.Columns.Add(model => model.DateOfActivity).Titled("Date Of Activity");
            //grid.Columns.Add(model => model.ActivityType).Titled("Name Of Activity");
            //grid.Columns.Add(model => model.SubJobNumber).Titled("Sub Job Number/Non Inspection Activity Number");
            //grid.Columns.Add(model => model.Description).Titled("Description");
            //grid.Columns.Add(model => model.ActivityHours).Titled("Activity Hours");

            grid.Columns.Add(c => c.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(c => c.Branch_Name).Titled("Branch Name");
            grid.Columns.Add(c => c.EmployeeCode).Titled("Work From Home");
            grid.Columns.Add(c => c.ActivityType).Titled("InspectionActivity");
            grid.Columns.Add(c => c.Job).Titled("Marketing");
            grid.Columns.Add(c => c.Sub_Job).Titled("Training");
            grid.Columns.Add(c => c.SAP_No).Titled("QA");
            grid.Columns.Add(c => c.Project_Name).Titled("office");
            grid.Columns.Add(c => c.Job_Location).Titled("NonClearableActivity");
            grid.Columns.Add(c => c.Company_Name).Titled("PL");
            grid.Columns.Add(c => c.CL).Titled("CL");
            grid.Columns.Add(c => c.SL).Titled("SL");
            grid.Columns.Add(c => c.StartTime).Titled("TotalManDays");
            grid.Columns.Add(c => c.Availibity).Titled("Utilisation (26 Mandays)");
            grid.Columns.Add(c => c.Availibity22).Titled("Utilisation (22 Mandays)");



            grid.Pager = new GridPager<EmployeeAttendance>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEA.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<EmployeeAttendance> GetDataEmployeeAttendance()
        {
            List<EmployeeAttendance> lstEmployeeAttendanceSearchByDate = new List<EmployeeAttendance>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstEmployeeAttendanceSearchByDate = objDALMisReport.EmployeeAttendanceMIS();
            }
            else
            {


                DTEmployeeAttendanceSearchByDate = objDALMisReport.EmployeeAttendanceSearchByDate(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }

            
            
            if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                {
                    lstEmployeeAttendanceSearchByDate.Add(
                       new EmployeeAttendance
                       {
                           Count = DTEmployeeAttendanceSearchByDate.Rows.Count,
                           InspectorName = Convert.ToString(dr["InspectorName"]),
                           Branch_Name = Convert.ToString(dr["Branch_Name"]),
                           EmployeeCode = Convert.ToString(dr["WorkFromHome"]),
                           ActivityType = Convert.ToString(dr["InspectionActivity"]),
                           Job = Convert.ToString(dr["Marketing"]),
                           Sub_Job = Convert.ToString(dr["Training"]),
                           SAP_No = Convert.ToString(dr["QA"]),
                           Project_Name = Convert.ToString(dr["office"]),
                           Job_Location = Convert.ToString(dr["NonClearableActivity"]),
                           Company_Name = Convert.ToString(dr["PL"]),
                           CL = Convert.ToString(dr["CL"]),
                           SL = Convert.ToString(dr["SL"]),
                           StartTime = Convert.ToString(dr["TotalManDays"]),
                           Id = Convert.ToString(dr["Id"]),
                           Availibity = Convert.ToString(Math.Round(((Convert.ToDouble(dr["TotalManDays"]) / 26) * 100))) + "%",
                           Availibity22 = Convert.ToString(Math.Round(((Convert.ToDouble(dr["TotalManDays"]) / 22) * 100))) + "%",
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objEA.lst1 = lstEmployeeAttendanceSearchByDate;
                return objEA.lst1;
            }
            ViewData["EmployeeAttendance"] = lstEmployeeAttendanceSearchByDate;
            TempData["Result"] = null;
            TempData.Keep();
            objEA.lst1 = lstEmployeeAttendanceSearchByDate;

            return objEA.lst1;
        }

        #endregion

        public ActionResult InspectorData()
        {


            ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
            ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);


            Session["GetExcelData"] = "Yes";

            string FromDate = string.Empty;
            string ToDate = string.Empty;

            List<InspectorData> lstEmployeeAttendance = new List<InspectorData>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);

                DTEmployeeAttendanceSearchByDate = objDALMisReport.InspectorDataByDate(FromDate, ToDate);


                if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                    {


                        lstEmployeeAttendance.Add(
                           new InspectorData
                           {
                               User = Convert.ToString(dr["User"]),
                               TUv_Email_ID = Convert.ToString(dr["TUv_Email_ID"]),
                               MobileNo = Convert.ToString(dr["MobileNo"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               EmpType = Convert.ToString(dr["EmpType"]),
                               CallsAssigned = dr["CallsAssigned"] == null ? "0" : Convert.ToString(dr["CallsAssigned"]),
                               ClosedCalls = dr["ClosedCalls"] == null ? "0" : Convert.ToString(dr["ClosedCalls"]),
                               VisitReports = dr["VisitReports"] == null ? "0" : Convert.ToString(dr["VisitReports"]),
                           }
                        );
                    }
                }
            }
            else
            {

                lstEmployeeAttendance = objDALMisReport.InspectorData();

            }


            ViewData["EmployeeAttendance"] = lstEmployeeAttendance;
            objInspData.lst1 = lstEmployeeAttendance;
            return View(objInspData);
        }

        [HttpPost]
        public ActionResult InspectorData(string FromDate, string ToDate)
        {


            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            List<InspectorData> lstEmployeeAttendanceSearchByDate = new List<InspectorData>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            DTEmployeeAttendanceSearchByDate = objDALMisReport.InspectorDataByDate(FromDate, ToDate);
            if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                {
                    lstEmployeeAttendanceSearchByDate.Add(
                       new InspectorData
                       {

                           User = Convert.ToString(dr["User"]),
                           TUv_Email_ID = Convert.ToString(dr["TUv_Email_ID"]),
                           MobileNo = Convert.ToString(dr["MobileNo"]),
                           Branch = Convert.ToString(dr["Branch"]),
                           EmpType = Convert.ToString(dr["EmpType"]),
                           CallsAssigned = dr["CallsAssigned"] == null ? "0" : Convert.ToString(dr["CallsAssigned"]),
                           ClosedCalls = dr["ClosedCalls"] == null ? "0" : Convert.ToString(dr["ClosedCalls"]),
                           VisitReports = dr["VisitReports"] == null ? "0" : Convert.ToString(dr["VisitReports"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objInspData.lst1 = lstEmployeeAttendanceSearchByDate;
                ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
                ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);

                objInspData.FromDate = Convert.ToString(Session["FromDate"]);
                objInspData.ToDate = Convert.ToString(Session["ToDate"]);
                return View(objInspData);
            }
            ViewData["EmployeeAttendance"] = lstEmployeeAttendanceSearchByDate;
            TempData["Result"] = null;
            TempData.Keep();
            objInspData.lst1 = lstEmployeeAttendanceSearchByDate;

            ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
            ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);

            objInspData.FromDate = Convert.ToString(Session["FromDate"]);
            objInspData.ToDate = Convert.ToString(Session["ToDate"]);

            return View(objInspData);
        }

        public ActionResult CallAnalysis()
        {
            ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
            ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);


            Session["GetExcelData"] = "Yes";

            string FromDate = string.Empty;
            string ToDate = string.Empty;

            List<CallAnalysis> lstEmployeeAttendance = new List<CallAnalysis>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);

                DTEmployeeAttendanceSearchByDate = objDALMisReport.CallAnalysisByDate(FromDate, ToDate);


                if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                    {


                        lstEmployeeAttendance.Add(
                           new CallAnalysis
                           {
                               Job = Convert.ToString(dr["Job"]),
                               client = Convert.ToString(dr["client"]),
                               call_no = Convert.ToString(dr["call_no"]),
                               Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                               Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                               Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                               Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                               Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                               Inspector = Convert.ToString(dr["Inspector"]),
                               Status = Convert.ToString(dr["Status"]),
                               Visit_Report_No = Convert.ToString(dr["Visit Report No"]),
                           }
                        );
                    }
                }
            }
            else
            {

                lstEmployeeAttendance = objDALMisReport.CallAnalysis();

            }


            ViewData["EmployeeAttendance"] = lstEmployeeAttendance;
            objCallAnalysis.lst1 = lstEmployeeAttendance;
            return View(objCallAnalysis);
        }

        [HttpPost]
        public ActionResult CallAnalysis(string FromDate, string ToDate)
        {


            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;


            List<CallAnalysis> lstEmployeeAttendanceSearchByDate = new List<CallAnalysis>();
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            DTEmployeeAttendanceSearchByDate = objDALMisReport.CallAnalysisByDate(FromDate, ToDate);

            if (DTEmployeeAttendanceSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTEmployeeAttendanceSearchByDate.Rows)
                {
                    lstEmployeeAttendanceSearchByDate.Add(
                       new CallAnalysis
                       {

                           Job = Convert.ToString(dr["Job"]),
                           client = Convert.ToString(dr["client"]),
                           call_no = Convert.ToString(dr["call_no"]),
                           Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                           Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           CreatedBy = Convert.ToString(dr["CreatedBy"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Status = Convert.ToString(dr["Status"]),
                           Visit_Report_No = Convert.ToString(dr["Visit Report No"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCallAnalysis.lst1 = lstEmployeeAttendanceSearchByDate;
                ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
                ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);

                objCallAnalysis.FromDate = Convert.ToString(Session["FromDate"]);
                objCallAnalysis.ToDate = Convert.ToString(Session["ToDate"]);
                return View(objCallAnalysis);
            }

            ViewData["EmployeeAttendance"] = lstEmployeeAttendanceSearchByDate;
            TempData["Result"] = null;
            TempData.Keep();

            objCallAnalysis.lst1 = lstEmployeeAttendanceSearchByDate;

            ViewBag.FromDate = objDALMisReport.GetMonths(DateTime.Now.Year);
            ViewBag.ToDate = objDALMisReport.GetYears(DateTime.Now.Year);

            objCallAnalysis.FromDate = Convert.ToString(Session["FromDate"]);
            objCallAnalysis.ToDate = Convert.ToString(Session["ToDate"]);

            return View(objCallAnalysis);
        }

        #region Export To Excel InspectorData

        [HttpGet]
        public ActionResult ExportInspectorData()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<InspectorData> grid = CreateExportableInspectorData();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<InspectorData> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<InspectorData> CreateExportableInspectorData()
        {

            IGrid<InspectorData> grid = new Grid<InspectorData>(GetInpectorData());

            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.User).Titled("Inspector Name");
            grid.Columns.Add(model => model.TUv_Email_ID).Titled("Email ID");
            grid.Columns.Add(model => model.MobileNo).Titled("Mobile No");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.EmpType).Titled("Employee Type");
            grid.Columns.Add(model => model.CallsAssigned).Titled("Call Assigned");
            grid.Columns.Add(model => model.ClosedCalls).Titled("Closed Calls");
            grid.Columns.Add(model => model.VisitReports).Titled("Visit Reports");


            grid.Pager = new GridPager<InspectorData>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objInspData.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<InspectorData> GetInpectorData()
        {

            List<InspectorData> lstQuotationMast = new List<InspectorData>();
            DataTable DTDelayIVRSearchByDate = new DataTable();
            string FromDate = string.Empty;
            string ToDate = string.Empty;

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);
                DTDelayIVRSearchByDate = objDALMisReport.InspectorDataByDate(FromDate, ToDate);
            }
            else
            {
                lstQuotationMast = objDALMisReport.InspectorData();
            }


            if (DTDelayIVRSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new InspectorData
                       {
                           User = Convert.ToString(dr["User"]),
                           TUv_Email_ID = Convert.ToString(dr["TUv_Email_ID"]),
                           MobileNo = Convert.ToString(dr["MobileNo"]),
                           Branch = Convert.ToString(dr["Branch"]),
                           EmpType = Convert.ToString(dr["EmpType"]),
                           CallsAssigned = dr["CallsAssigned"] == DBNull.Value ? "0" : Convert.ToString(dr["CallsAssigned"]),
                           ClosedCalls = dr["ClosedCalls"] == DBNull.Value ? "0" : Convert.ToString(dr["ClosedCalls"]),
                           VisitReports = dr["VisitReports"] == DBNull.Value ? "0" : Convert.ToString(dr["VisitReports"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objInspData.lst1 = lstQuotationMast;
                return objInspData.lst1;
            }

            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objInspData.lst1 = lstQuotationMast;
            return objInspData.lst1;
        }

        #endregion

        #region Export To Excel Call Analysis

        [HttpGet]
        public ActionResult ExportCallAnalysis()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<CallAnalysis> grid = CreateExportableCallAnalysis();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<CallAnalysis> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<CallAnalysis> CreateExportableCallAnalysis()
        {

            IGrid<CallAnalysis> grid = new Grid<CallAnalysis>(GetCallAnalysisData());

            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Job).Titled("Job Number");
            grid.Columns.Add(model => model.client).Titled("Client Name");
            grid.Columns.Add(model => model.call_no).Titled("Call No");
            grid.Columns.Add(model => model.Call_Received_Date).Titled("Call Received Date");
            grid.Columns.Add(model => model.Call_Request_Date).Titled("Call Requested Date");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.CreatedDate).Titled("Call Created Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.Executing_Branch).Titled("Executing Branch");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            grid.Columns.Add(model => model.Continuous_Call).Titled("Continuos Call");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            grid.Columns.Add(model => model.Status).Titled("Status");
            grid.Columns.Add(model => model.Visit_Report_No).Titled("Visit Report No");



            grid.Pager = new GridPager<CallAnalysis>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objCallAnalysis.lst1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<CallAnalysis> GetCallAnalysisData()
        {

            List<CallAnalysis> lstQuotationMast = new List<CallAnalysis>();
            DataTable DTDelayIVRSearchByDate = new DataTable();
            string FromDate = string.Empty;
            string ToDate = string.Empty;

            if (Session["FromDate"] != null && Session["ToDate"] != null)
            {
                FromDate = Convert.ToString(Session["FromDate"]);
                ToDate = Convert.ToString(Session["ToDate"]);
                DTDelayIVRSearchByDate = objDALMisReport.CallAnalysisByDate(FromDate, ToDate);
            }
            else
            {
                lstQuotationMast = objDALMisReport.CallAnalysis();
            }


            if (DTDelayIVRSearchByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTDelayIVRSearchByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new CallAnalysis
                       {
                           Job = Convert.ToString(dr["Job"]),
                           client = Convert.ToString(dr["client"]),
                           call_no = Convert.ToString(dr["call_no"]),
                           Call_Received_Date = Convert.ToString(dr["Call Received Date"]),
                           Call_Request_Date = Convert.ToString(dr["Call_Request_Date"]),
                           Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           CreatedBy = Convert.ToString(dr["CreatedBy"]),
                           Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                           Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                           Continuous_Call = Convert.ToString(dr["Continuous Call"]),
                           Inspector = Convert.ToString(dr["Inspector"]),
                           Status = Convert.ToString(dr["Status"]),
                           Visit_Report_No = Convert.ToString(dr["Visit Report No"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objCallAnalysis.lst1 = lstQuotationMast;
                return objCallAnalysis.lst1;
            }

            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objCallAnalysis.lst1 = lstQuotationMast;
            return objCallAnalysis.lst1;
        }

        #endregion

        #region Regret EnquiryReport
        // GET: MisReport
        public ActionResult RegEnquiryReport()
        {
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }

            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            {
                TempData.Keep();
                FromDate = Convert.ToString(TempData["FromDate"]);
                ToDate = Convert.ToString(TempData["ToDate"]);
                objEM.FromDate = Convert.ToDateTime(FromDate);
                objEM.ToDate = Convert.ToDateTime(ToDate);

                DTSearchByDateWiseData = objDalEnquiryMaster.GetDataByDateWise(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new EnquiryMaster
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),

                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {
                lstEnquiryMast = objDalEnquiryMaster.GetRegEnquiryListDashBoard();
            }


            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }

        //Record Search By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        [HttpPost]
        public ActionResult RegEnquiryReport(string FromDate, string ToDate)
        {

            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }


            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDalEnquiryMaster.GetRegDataByDateWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new EnquiryMaster
                       {
                           Count = DTSearchByDateWiseData.Rows.Count,

                           RefDate = Convert.ToString(dr["refDate"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           Notes = Convert.ToString(dr["Notes"]),
                           NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                           //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                           ContactNo = Convert.ToString(dr["ContactNo"]),
                           ContactName = Convert.ToString(dr["ContactName"]),
                           ARC = Convert.ToString(dr["ARC"]),
                           ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                           EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           EnquiryDescription = Convert.ToString(dr["Description"]),
                           ProjectName = Convert.ToString(dr["ProjectName"]),
                           Company_Name = Convert.ToString(dr["CompanyName"]),
                           Branch = Convert.ToString(dr["OriginatingBranch"]),
                           OpendateS = Convert.ToString(dr["DateOpened"]),
                           Source = Convert.ToString(dr["source"]),
                           EstCloseS = Convert.ToString(dr["EstClose"]),
                           ProjectType = Convert.ToString(dr["ProjectType"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           //SubServiceType = Convert.ToString(dr["ServiceType"]),
                           Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                           Status = Convert.ToString(dr["status"]),
                           RegretReason = Convert.ToString(dr["RegretReason"]),
                           //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                           Owner = Convert.ToString(dr["OwnerName"]),
                           //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),
                           //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                           DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                           IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                           TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                           Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                           Intlocation = Convert.ToString(dr["Intlocation"]),
                           Dcurrency = Convert.ToString(dr["Dcurrency"]),
                           Icurrency = Convert.ToString(dr["ICurrency"]),
                           LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objEM.lst1 = lstEnquiryMast;
                return View(objEM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }


        [HttpGet]
        public ActionResult ExportIndexRegEnquiryReport(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EnquiryMaster> grid = CreateRegExportableGridEnquiryReport();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EnquiryMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "RegEnquiryReport-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<EnquiryMaster> CreateRegExportableGridEnquiryReport()
        {
            IGrid<EnquiryMaster> grid = new Grid<EnquiryMaster>(GetDataRegEnquiryReport());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            //grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            //grid.Columns.Add(model => model.Branch).Titled("Branch");
            //grid.Columns.Add(model => model.Source).Titled("Source");
            //grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            //grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            //grid.Columns.Add(model => model.ProjectType).Titled("OBS Type");
            //grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");
            //grid.Columns.Add(model => model.SubServiceType).Titled("Service Type");
            //grid.Columns.Add(model => model.CreatedBy).Titled("Owner Name");
            //grid.Columns.Add(model => model.EnquiryDescription).Titled("Description");
            //grid.Columns.Add(model => model.TEstimatedAmount).Titled("Total Estimated Amount");
            //grid.Columns.Add(model => model.OpendateS).Titled("Creation Date");
            //grid.Columns.Add(model => model.EstCloseS).Titled("Estimated Closure Date");
            //grid.Columns.Add(model => model.ModifiedDateS).Titled("Modification Date");
            //grid.Columns.Add(model => model.Status).Titled("Enquiry Status");
            //grid.Columns.Add(model => model.RegretReason).Titled("Regret Reason");
            //grid.Columns.Add(model => model.Domesticlocation).Titled("Domestic");
            //grid.Columns.Add(model => model.Intlocation).Titled("International");
            //grid.Columns.Add(model => model.DEstimatedAmount).Titled("Domestic Amount");
            //grid.Columns.Add(model => model.IEstimatedAmount).Titled("International Amount");
            //grid.Columns.Add(model => model.ARC).Titled("ARC Job");
            //grid.Columns.Add(model => model.ContactName).Titled("Contact Name");
            //grid.Columns.Add(model => model.ContactNo).Titled("Contact Number");

            grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.Source).Titled("Source");
            grid.Columns.Add(c => c.RefDate).Titled("Enquiry receipt date");
            grid.Columns.Add(c => c.EstCloseS).Titled("Estimated closure date");
            grid.Columns.Add(c => c.EnquiryReferenceNo).Titled("Enquiry reference details");
            grid.Columns.Add(c => c.Company_Name).Titled("Customer Name");
            grid.Columns.Add(c => c.EndCustomer).Titled("End customer name");
            grid.Columns.Add(c => c.ProjectType).Titled("OBS Type");
            grid.Columns.Add(c => c.PortfolioType).Titled("Portfolio Type");
            grid.Columns.Add(c => c.Type).Titled("Service Type");
            grid.Columns.Add(c => c.ProjectName).Titled("Project Name");
            grid.Columns.Add(c => c.EnquiryDescription).Titled("Description");
            grid.Columns.Add(c => c.ModifiedBy).Titled("Modified by");
            grid.Columns.Add(c => c.ARC).Titled("ARC Job");
            grid.Columns.Add(c => c.Domesticlocation).Titled("Domestic");
            grid.Columns.Add(c => c.Intlocation).Titled("International");
            grid.Columns.Add(c => c.DEstimatedAmount).Titled("Domestic Amount");
            grid.Columns.Add(c => c.IEstimatedAmount).Titled("International Amount");
            grid.Columns.Add(c => c.TEstimatedAmount).Titled("Total Estimated Amount");
            grid.Columns.Add(c => c.Status).Titled("Enquiry Status");
            grid.Columns.Add(c => c.RegretReason).Titled("Regret Reason");
            grid.Columns.Add(c => c.RegretActionTaken).Titled("Regret Action taken");
            grid.Columns.Add(c => c.ContactName).Titled("Customer Contact Person Name");
            grid.Columns.Add(c => c.ContactNo).Titled("Customer Contact Person Number");
            grid.Columns.Add(c => c.Owner).Titled("Enquiry Created By");
            grid.Columns.Add(c => c.LeadGivenBy).Titled("Lead Given By");
            grid.Columns.Add(c => c.NotesbyLeads).Titled("Notes by Leads");
            grid.Columns.Add(c => c.OpendateS).Titled("Creation Date");
            //colCmns.Add(c => c.EstCloseS).Titled("Estimated Closure Date");
            grid.Columns.Add(c => c.ModifiedDateS).Titled("Modification Date");
            //grid.Columns.Add(c => c.Quotation).Titled("Quotation Number");
            //grid.Columns.Add(c => c.JobNumber).Titled("Job Number");






            grid.Pager = new GridPager<EnquiryMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<EnquiryMaster> GetDataRegEnquiryReport()
        {



            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();


            //if (Session["FromDate"] == null && Session["ToDate"] == null)//commented by nikita 
            if (TempData["FromDate"] != null && TempData["ToDate"] != null)//added by nikita

            {
                DTSearchByDateWiseData = objDalEnquiryMaster.GetRegDataByDateWise(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new EnquiryMaster
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               //ContactNo = Convert.ToString(dr["ContactNo"]),
                               //ContactName = Convert.ToString(dr["ContactName"]),
                               //ARC = Convert.ToString(dr["ARC"]),
                               //ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               //EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               //EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               //EnquiryDescription = Convert.ToString(dr["Description"]),
                               //ProjectName = Convert.ToString(dr["ProjectName"]),
                               //Company_Name = Convert.ToString(dr["CompanyName"]),
                               //Branch = Convert.ToString(dr["OriginatingBranch"]),
                               //OpendateS = Convert.ToString(dr["DateOpened"]),
                               //Source = Convert.ToString(dr["source"]),
                               //EstCloseS = Convert.ToString(dr["EstClose"]),
                               //ProjectType = Convert.ToString(dr["ProjectType"]),
                               //PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               //Status = Convert.ToString(dr["status"]),
                               //RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               //TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               //IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               //Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               //Intlocation = Convert.ToString(dr["Intlocation"]),
                               //Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               //Icurrency = Convert.ToString(dr["ICurrency"])

                               //added by mikita 
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               RegretActionTaken = Convert.ToString(dr["EnquiryRegretActionTaken"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),//added by nikita 09082023
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),commenetd by nikita
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),commenetd by nikita
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),//added by nikita 09082023
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),//added by nikita 09082023
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

            }
            else
            {
                lstEnquiryMast = objDalEnquiryMaster.GetRegEnquiryListDashBoard();
            }




            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;

            return objEM.lst1;
        }
        #endregion

        #region JobRegister
        // GET: MisReport
        public ActionResult JobRegister()
        {
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            {
                TempData.Keep();
                FromDate = Convert.ToString(TempData["FromDate"]);
                ToDate = Convert.ToString(TempData["ToDate"]);

                objJM.FromDate = Convert.ToDateTime(FromDate);
                objJM.ToDate = Convert.ToDateTime(ToDate);

                DTSearchByDateWiseData = objJob.GetDataByDateWise(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                               Consumed = Convert.ToString(dr["Consumed"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                               Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               QMAmount = Convert.ToString(dr["QMAmount"]),
                               EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                               POAmount = Convert.ToString(dr["POAmount"]),
                               Job_Number = Convert.ToString(dr["Job_Number"]),
                               Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Description = Convert.ToString(dr["Description"]),
                               Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               End_User = Convert.ToString(dr["End_User"]),
                               Service_type = Convert.ToString(dr["Service_type"]),
                               Job_type = Convert.ToString(dr["Job_type"]),
                               SAP_No = Convert.ToString(dr["SAP_No"]),
                               RemainingMandays = Convert.ToString(dr["RemainingDays"]),
                               JobDate = Convert.ToString(dr["JobCreateDate"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),

                               Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                               Special_Notes = Convert.ToString(dr["Special_Notes"]),
                               OrderStatus = Convert.ToString(dr["orderstatus"]),
                               DECName = Convert.ToString(dr["DECName"]),
                               DECNumber = Convert.ToString(dr["DECNumber"]),
                               JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                               ARC = Convert.ToString(dr["chkARC"]),
                               EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                               QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                               QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                               InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"])
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {

                lstEnquiryMast = objJob.GetJobDashBoard();
            }



            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }

        //Record Search By From Date And To Date wise, Code By Manoj Sharma 17 Dec 2019
        [HttpPost]
        public ActionResult JobRegister(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            //Session["FromDate"] = FromDate;
            //Session["ToDate"] = ToDate;
            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objJob.GetDataByDateWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new JobMasters
                       {
                           Count = DTSearchByDateWiseData.Rows.Count,
                           PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                           Consumed = Convert.ToString(dr["Consumed"]),
                           Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                           Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                           Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                           PONo = Convert.ToString(dr["PONo"]),
                           QMAmount = Convert.ToString(dr["QMAmount"]),
                           EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                           POAmount = Convert.ToString(dr["POAmount"]),
                           Job_Number = Convert.ToString(dr["Job_Number"]),
                           Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                           Po_Validity = Convert.ToString(dr["Po_Validity"]),
                           Description = Convert.ToString(dr["Description"]),
                           Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                           Client_Name = Convert.ToString(dr["Client_Name"]),
                           Branch = Convert.ToString(dr["Branch"]),
                           End_User = Convert.ToString(dr["End_User"]),
                           Service_type = Convert.ToString(dr["Service_type"]),
                           Job_type = Convert.ToString(dr["Job_type"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),
                           RemainingMandays = Convert.ToString(dr["RemainingDays"]),
                           JobDate = Convert.ToString(dr["JobCreateDate"]),
                           OrderRate = Convert.ToString(dr["OrderRate"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),

                           Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                           Special_Notes = Convert.ToString(dr["Special_Notes"]),
                           OrderStatus = Convert.ToString(dr["orderstatus"]),
                           DECName = Convert.ToString(dr["DECName"]),
                           DECNumber = Convert.ToString(dr["DECNumber"]),
                           JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                           ARC = Convert.ToString(dr["chkARC"]),
                           EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                           QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                           QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                           InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"])

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }


        [HttpGet]
        public ActionResult ExportIndexJobRegister(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridJobRegister();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy" + '-' + "HH:mm:ss");

                string filename = "JobRegister-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<JobMasters> CreateExportableGridJobRegister()
        {

            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataJobRegister());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.SAP_No).Titled("Sales Order No");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Client_Name).Titled("Client Name");
            grid.Columns.Add(model => model.PONo).Titled("PO Number");
            grid.Columns.Add(model => model.Customer_PoDate).Titled("Customer PO Date");
            grid.Columns.Add(model => model.Po_Validity).Titled("PO Validity");
            grid.Columns.Add(model => model.POAmount).Titled("PO Amount");
            grid.Columns.Add(model => model.OrderType).Titled("Order Type");
            grid.Columns.Add(model => model.ARC).Titled("ARC");
            grid.Columns.Add(model => model.OrderStatus).Titled("Order Status");

            grid.Columns.Add(model => model.Consumed).Titled("Consumed Mandays");
            grid.Columns.Add(model => model.RemainingMandays).Titled("Remaining Mandays");

            grid.Columns.Add(model => model.Estimate_ManDays_ManMonth).Titled("Mandays as per PO");
            grid.Columns.Add(model => model.Estimate_ManMonth).Titled("Estimated Manmonth");
            grid.Columns.Add(model => model.Estimate_ManHR).Titled("Estimated HR");
            grid.Columns.Add(model => model.OrderRate).Titled("Manday Rate");
            grid.Columns.Add(model => model.JobCreatedBy).Titled("Job Created By");
            grid.Columns.Add(model => model.JobDate).Titled("Job Created Date");
            grid.Columns.Add(model => model.Quotation_Of_Order).Titled("Quotation Number");
            grid.Columns.Add(model => model.QMCreatedBy).Titled("Quotation Created By");
            grid.Columns.Add(model => model.QMCreatedDate).Titled("Quotation Created Date");
            grid.Columns.Add(model => model.QMAmount).Titled("Quotation Amount");
            grid.Columns.Add(model => model.Enquiry_Of_Order).Titled("Enquiry Number");
            grid.Columns.Add(model => model.EnqRecDate).Titled("Enquiry Receipt Date");
            grid.Columns.Add(model => model.EQEstAmt).Titled("Enquiry Amount");
            grid.Columns.Add(model => model.End_User).Titled("End User");
            grid.Columns.Add(model => model.Description).Titled("Project Name");
            grid.Columns.Add(model => model.Job_type).Titled("OBS Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");
            grid.Columns.Add(model => model.Service_type).Titled("Service Type");
            grid.Columns.Add(model => model.DECName).Titled("DEC Name");
            grid.Columns.Add(model => model.DECNumber).Titled("DEC Number");


            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataJobRegister()
        {



            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();


            if (TempData["FromDate"] == null && TempData["ToDate"] == null)

            {
                lstEnquiryMast = objJob.GetJobDashBoard();
            }
            else
            {
                DTSearchByDateWiseData = objJob.GetDataByDateWise(Convert.ToString(TempData["FromDate"]), Convert.ToString(TempData["ToDate"]));
             //   DTSearchByDateWiseData = objJob.GetDataByDateWise(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));//coomenetd by nikita on 09/09/2023

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                               Consumed = Convert.ToString(dr["Consumed"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                               Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               QMAmount = Convert.ToString(dr["QMAmount"]),
                               EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                               POAmount = Convert.ToString(dr["POAmount"]),
                               Job_Number = Convert.ToString(dr["Job_Number"]),
                               Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Description = Convert.ToString(dr["Description"]),
                               Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               End_User = Convert.ToString(dr["End_User"]),
                               Service_type = Convert.ToString(dr["Service_type"]),
                               Job_type = Convert.ToString(dr["Job_type"]),
                               SAP_No = Convert.ToString(dr["SAP_No"]),
                               RemainingMandays = Convert.ToString(dr["RemainingDays"]),
                               JobDate = Convert.ToString(dr["JobCreateDate"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),

                               Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                               Special_Notes = Convert.ToString(dr["Special_Notes"]),
                               OrderStatus = Convert.ToString(dr["orderstatus"]),
                               DECName = Convert.ToString(dr["DECName"]),
                               DECNumber = Convert.ToString(dr["DECNumber"]),
                               JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                               ARC = Convert.ToString(dr["chkARC"]),
                               EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                               QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                               QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                               InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }




            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;

            return objJM.lst1;
        }

        #endregion

        #region JobRevisionHistory

        // GET: MisReport
        public ActionResult JobHistory()
        {
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            {
                TempData.Keep();
                FromDate = Convert.ToString(TempData["FromDate"]);
                ToDate = Convert.ToString(TempData["ToDate"]);

                objJM.FromDate = Convert.ToDateTime(FromDate);
                objJM.ToDate = Convert.ToDateTime(ToDate);

                DTSearchByDateWiseData = objJob.GetDataHistoryByDateWise(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               Job_Number = Convert.ToString(dr["Job"]),
                               Description = Convert.ToString(dr["Jobdescription"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               POAmount = Convert.ToString(dr["PO_Amount"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Add_PoDate = Convert.ToString(dr["PODate"]),
                               Add_Mandays = Convert.ToString(dr["ManDays"]),
                               Add_PoReason = Convert.ToString(dr["Reason"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               ModifyBy = Convert.ToString(dr["ModifyBy"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               Service_type = Convert.ToString(dr["Service_type"]),
                               OBSID = Convert.ToString(dr["ObsType"]),

                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {

                lstEnquiryMast = objJob.GetJobHistoryDashBoard();
            }



            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }

        [HttpPost]
        public ActionResult JobHistory(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            //Session["FromDate"] = FromDate;
            //Session["ToDate"] = ToDate;
            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objJob.GetDataHistoryByDateWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new JobMasters
                       {

                           Count = DTSearchByDateWiseData.Rows.Count,
                           Job_Number = Convert.ToString(dr["Job"]),
                           Description = Convert.ToString(dr["Jobdescription"]),
                           Client_Name = Convert.ToString(dr["Client_Name"]),
                           Branch = Convert.ToString(dr["Branch"]),
                           PONo = Convert.ToString(dr["PONo"]),
                           POAmount = Convert.ToString(dr["PO_Amount"]),
                           Po_Validity = Convert.ToString(dr["Po_Validity"]),
                           Add_PoDate = Convert.ToString(dr["PODate"]),
                           Add_Mandays = Convert.ToString(dr["ManDays"]),
                           Add_PoReason = Convert.ToString(dr["Reason"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           CreatedBy = Convert.ToString(dr["CreatedBy"]),
                           ModifyBy = Convert.ToString(dr["ModifyBy"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           Service_type = Convert.ToString(dr["ServiceType"]),
                           OBSID = Convert.ToString(dr["ObsType"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }
        [HttpGet]
        public ActionResult ExportIndexJobRevisionHistory(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridJobRevisionHistory();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy" + '-' + "HH:mm:ss");

                string filename = "JobHistory-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<JobMasters> CreateExportableGridJobRevisionHistory()
        {

            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataJobRevisionHistory());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.Client_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.PONo).Titled("PO Number");
            grid.Columns.Add(model => model.POAmount).Titled("PO Amount");
            grid.Columns.Add(model => model.Po_Validity).Titled("Po_Validity");
            grid.Columns.Add(model => model.Add_PoDate).Titled("PO Date");
            grid.Columns.Add(model => model.Add_Mandays).Titled("Man Days");
            grid.Columns.Add(model => model.OBSID).Titled("Obs Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");
            grid.Columns.Add(model => model.Service_type).Titled("Service Type");
            grid.Columns.Add(model => model.Add_PoReason).Titled("Reason");
            grid.Columns.Add(model => model.CreatedDate).Titled("Revision Created Date");
            grid.Columns.Add(model => model.CreatedBy).Titled("Revision Created By");
            grid.Columns.Add(model => model.ModifyBy).Titled("Revision Modified By");

            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataJobRevisionHistory()
        {

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();


            if (TempData["FromDate"] == null && TempData["ToDate"] == null) //added by nikita 

            {
                lstEnquiryMast = objJob.GetJobHistoryDashBoard();
            }
            else
            {
                DTSearchByDateWiseData = objJob.GetDataHistoryByDateWise(Convert.ToString(TempData["FromDate"]), Convert.ToString(TempData["ToDate"]));

               
                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               Job_Number = Convert.ToString(dr["Job"]),
                               Description = Convert.ToString(dr["Jobdescription"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               POAmount = Convert.ToString(dr["PO_Amount"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Add_PoDate = Convert.ToString(dr["PODate"]),
                               Add_Mandays = Convert.ToString(dr["ManDays"]),
                               Add_PoReason = Convert.ToString(dr["Reason"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               ModifyBy = Convert.ToString(dr["ModifyBy"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               Service_type = Convert.ToString(dr["ServiceType"]),
                               OBSID = Convert.ToString(dr["ObsType"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }




            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;

            return objJM.lst1;
        }

        #endregion

        #region Probable Invoicing Amount

        // GET: MisReport
        public ActionResult ProbInvoicing()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();

            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataSet DTSearchByDateWiseData = new DataSet();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


            if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) && (TempData["month"] != null && TempData["month"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["FYear"]);
                month = Convert.ToString(TempData["month"]);


                DTSearchByDateWiseData = objJob.GetDatewiseProbableInvoicing(Fyear, month);

                if (DTSearchByDateWiseData.Tables.Count > 0)
                {
                    if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTSearchByDateWiseData.Tables[0].Rows)
                        {
                            lstEnquiryMast.Add(
                               new JobMasters
                               {
                                   Count = DTSearchByDateWiseData.Tables[0].Rows.Count,
                                   Job_Number = Convert.ToString(dr["Job"]),
                                   Branch = Convert.ToString(dr["Branch"]),
                                   CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   EstimatedMandays = Convert.ToString(dr["EstimatedMandays"]),
                                   Client_Name = Convert.ToString(dr["Client_Name"]),
                                   SAP_No = Convert.ToString(dr["Sap_no"]),
                                   ProjectName = Convert.ToString(dr["ProjectName"]),
                                   Customer_PO_AmountSum = Convert.ToString(dr["Customer_PO_Amount"]),
                                   ProposedMandays = Convert.ToString(dr["ProposedMandays"]),
                                   ConsumedMandays = Convert.ToString(dr["ConsumedMandays"]),
                                   ConsumedInvoicing = Convert.ToString(dr["ConsumedInvoicing"]),
                                   ProposedInvoicing = Convert.ToString(dr["ProposedInvoicing"]),
                                   RemainingMandays = Convert.ToString(dr["RemainingMandays"]),
                                   br_ID1 = Convert.ToString(dr["br_ID"]),

                               }
                             );
                        }
                    }

                    if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                    {

                        objJM.ConsumedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ConsumedInvoicing"]);
                        objJM.ProposedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ProposedInvoicing"]);
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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


                DTSearchByDateWiseData = objJob.GetProbableInvoicing();

                if (DTSearchByDateWiseData.Tables.Count > 0)
                {
                    if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTSearchByDateWiseData.Tables[0].Rows)
                        {
                            lstEnquiryMast.Add(
                               new JobMasters
                               {
                                   Count = DTSearchByDateWiseData.Tables[0].Rows.Count,
                                   Job_Number = Convert.ToString(dr["Job"]),
                                   Branch = Convert.ToString(dr["Branch"]),
                                   CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   EstimatedMandays = Convert.ToString(dr["EstimatedMandays"]),
                                   Client_Name = Convert.ToString(dr["Client_Name"]),
                                   SAP_No = Convert.ToString(dr["Sap_no"]),
                                   ProjectName = Convert.ToString(dr["ProjectName"]),
                                   Customer_PO_AmountSum = Convert.ToString(dr["Customer_PO_Amount"]),
                                   ProposedMandays = Convert.ToString(dr["ProposedMandays"]),
                                   ConsumedMandays = Convert.ToString(dr["ConsumedMandays"]),
                                   ConsumedInvoicing = Convert.ToString(dr["ConsumedInvoicing"]),
                                   ProposedInvoicing = Convert.ToString(dr["ProposedInvoicing"]),
                                   RemainingMandays = Convert.ToString(dr["RemainingMandays"]),
                                   br_ID1 = Convert.ToString(dr["br_ID"]),

                               }
                             );
                        }
                    }
                    if (DTSearchByDateWiseData.Tables.Count > 1)
                    {
                        if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                        {

                            objJM.ConsumedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ConsumedInvoicing"]);
                            objJM.ProposedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ProposedInvoicing"]);
                        }
                    }


                }
            }

            objJM.lst1 = lstEnquiryMast;


            return View(objJM);
        }

        [HttpPost]
        public ActionResult ProbInvoicing(string FYear, string monthID)
        {
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();

            dtFinancialYear = objJob.GetFinancialYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {

                    lstFinancialYear.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["FYear"])
                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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
            Session["GetExcelData"] = null;

            if (FYear != string.Empty && monthID != string.Empty)
            {
                TempData["FYear"] = FYear;
                TempData["month"] = monthID;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }




            DataSet DTSearchByDateWiseData = new DataSet();
            DTSearchByDateWiseData = objJob.GetDatewiseProbableInvoicing(FYear, monthID);

            if (DTSearchByDateWiseData.Tables.Count > 0)
            {
                if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Tables[0].Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {

                               Count = DTSearchByDateWiseData.Tables[0].Rows.Count,
                               Job_Number = Convert.ToString(dr["Job"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               OrderRate = Convert.ToString(dr["OrderRate"]),
                               EstimatedMandays = Convert.ToString(dr["EstimatedMandays"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               SAP_No = Convert.ToString(dr["Sap_no"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Customer_PO_AmountSum = Convert.ToString(dr["Customer_PO_Amount"]),
                               ProposedMandays = Convert.ToString(dr["ProposedMandays"]),
                               ConsumedMandays = Convert.ToString(dr["ConsumedMandays"]),
                               ConsumedInvoicing = Convert.ToString(dr["ConsumedInvoicing"]),
                               ProposedInvoicing = Convert.ToString(dr["ProposedInvoicing"]),
                               RemainingMandays = Convert.ToString(dr["RemainingMandays"]),
                               br_ID1 = Convert.ToString(dr["br_ID"]),

                           }
                         );
                    }
                }

                if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                {

                    objJM.ConsumedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ConsumedInvoicing"]);
                    objJM.ProposedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ProposedInvoicing"]);
                }


            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }


            TempData["Result"] = null;
            TempData.Keep();

            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }

        [HttpGet]
        public ActionResult ExportIndexProbInvoicing(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridProbInvoicing();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                if (objJM.ConsumedInvoicingSum != null && objJM.ProposedInvoicingSum != null)
                {

                    int newrow = row++;
                    sheet.Cells[newrow, 12, newrow, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    sheet.Cells[newrow, 12, newrow, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                    sheet.Cells[newrow, 12].Value = "Total Amount : ";
                    sheet.Cells[newrow, 13].Value = objJM.ConsumedInvoicingSum.ToString();
                    sheet.Cells[newrow, 14].Value = objJM.ProposedInvoicingSum.ToString();
                }



                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }

        private IGrid<JobMasters> CreateExportableGridProbInvoicing()
        {

            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataProbInvoicing());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.OrderType).Titled("Order Type");
            grid.Columns.Add(model => model.OrderRate).Titled("Order Rate");
            grid.Columns.Add(model => model.EstimatedMandays).Titled("Estimated Mandays");
            grid.Columns.Add(model => model.Client_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.SAP_No).Titled("SAP No");
            grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            grid.Columns.Add(model => model.Customer_PO_Amount).Titled("Customer PO Amount");
            grid.Columns.Add(model => model.ProposedMandays).Titled("Proposed Mandays");
            grid.Columns.Add(model => model.ConsumedMandays).Titled("Consumed Mandays");
            grid.Columns.Add(model => model.ConsumedInvoicing).Titled("Consumed Invoicing");
            grid.Columns.Add(model => model.ProposedInvoicing).Titled("Proposed Invoicing");
            grid.Columns.Add(model => model.RemainingMandays).Titled("Remaining Mandays");


            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataProbInvoicing()
        {

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataSet DTSearchByDateWiseData = new DataSet();

            string Fyear = string.Empty;
            string month = string.Empty;

            if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) && (TempData["month"] != null && TempData["month"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["FYear"]);
                month = Convert.ToString(TempData["month"]);


                DTSearchByDateWiseData = objJob.GetDatewiseProbableInvoicing(Fyear, month);

                if (DTSearchByDateWiseData.Tables.Count > 0)
                {
                    if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTSearchByDateWiseData.Tables[0].Rows)
                        {
                            lstEnquiryMast.Add(
                               new JobMasters
                               {
                                   Count = DTSearchByDateWiseData.Tables[0].Rows.Count,
                                   Job_Number = Convert.ToString(dr["Job"]),
                                   Branch = Convert.ToString(dr["Branch"]),
                                   CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   EstimatedMandays = Convert.ToString(dr["EstimatedMandays"]),
                                   Client_Name = Convert.ToString(dr["Client_Name"]),
                                   SAP_No = Convert.ToString(dr["Sap_no"]),
                                   ProjectName = Convert.ToString(dr["ProjectName"]),
                                   Customer_PO_AmountSum = Convert.ToString(dr["Customer_PO_Amount"]),
                                   ProposedMandays = Convert.ToString(dr["ProposedMandays"]),
                                   ConsumedMandays = Convert.ToString(dr["ConsumedMandays"]),
                                   ConsumedInvoicing = Convert.ToString(dr["ConsumedInvoicing"]),
                                   ProposedInvoicing = Convert.ToString(dr["ProposedInvoicing"]),
                                   RemainingMandays = Convert.ToString(dr["RemainingMandays"]),
                                   br_ID1 = Convert.ToString(dr["br_ID"]),

                               }
                             );
                        }
                    }

                    if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                    {

                        objJM.ConsumedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ConsumedInvoicing"]);
                        objJM.ProposedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ProposedInvoicing"]);
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {



                DTSearchByDateWiseData = objJob.GetProbableInvoicing();

                if (DTSearchByDateWiseData.Tables.Count > 0)
                {
                    if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in DTSearchByDateWiseData.Tables[0].Rows)
                        {
                            lstEnquiryMast.Add(
                               new JobMasters
                               {
                                   Count = DTSearchByDateWiseData.Tables[0].Rows.Count,
                                   Job_Number = Convert.ToString(dr["Job"]),
                                   Branch = Convert.ToString(dr["Branch"]),
                                   CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                   OrderType = Convert.ToString(dr["OrderType"]),
                                   OrderRate = Convert.ToString(dr["OrderRate"]),
                                   EstimatedMandays = Convert.ToString(dr["EstimatedMandays"]),
                                   Client_Name = Convert.ToString(dr["Client_Name"]),
                                   SAP_No = Convert.ToString(dr["Sap_no"]),
                                   ProjectName = Convert.ToString(dr["ProjectName"]),
                                   Customer_PO_AmountSum = Convert.ToString(dr["Customer_PO_Amount"]),
                                   ProposedMandays = Convert.ToString(dr["ProposedMandays"]),
                                   ConsumedMandays = Convert.ToString(dr["ConsumedMandays"]),
                                   ConsumedInvoicing = Convert.ToString(dr["ConsumedInvoicing"]),
                                   ProposedInvoicing = Convert.ToString(dr["ProposedInvoicing"]),
                                   RemainingMandays = Convert.ToString(dr["RemainingMandays"]),
                                   br_ID1 = Convert.ToString(dr["br_ID"]),

                               }
                             );
                        }
                    }

                    if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                    {

                        objJM.ConsumedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ConsumedInvoicing"]);
                        objJM.ProposedInvoicingSum = Convert.ToString(DTSearchByDateWiseData.Tables[1].Rows[0]["ProposedInvoicing"]);
                    }


                }
            }



            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;

            return objJM.lst1;
        }

        #endregion

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
        public ActionResult UnderConstruction()
        {
            return View();
        }
        /*
        public ActionResult OBSOrderValue()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            int TotalPrice = 0;
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


            if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) || (TempData["month"] != null && TempData["month"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["FYear"]);
                month = Convert.ToString(TempData["month"]);


                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

                ViewData["OBSOrderValue"] = DTSearchByDateWiseData;

            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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

                if (Fyear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        Fyear = CurYear + "-" + NexYear;
                    else
                        Fyear = PreYear + "-" + CurYear;



                }

                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);



                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                }


            }
            ViewData["OBSOrderValue"] = DTSearchByDateWiseData;




            return View(objJM);
        }




        [HttpPost]
        public ActionResult OBSOrderValue(string FYear, string monthID)
        {
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            int TotalPrice = 0;
            dtFinancialYear = objJob.GetFinancialYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {

                    lstFinancialYear.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["FYear"])
                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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
            Session["GetExcelData"] = null;

            if (FYear != string.Empty || monthID != string.Empty)
            {
                TempData["FYear"] = FYear;
                TempData["month"] = monthID;


                TempData.Keep();
            }
            else
            {
                TempData.Clear();

                if (FYear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        FYear = CurYear + "-" + NexYear;
                    else
                        FYear = PreYear + "-" + CurYear;

                }
            }




            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(FYear, monthID);


            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        TotalRow[col] = "Total";
                    }
                    else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                    {
                        for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                        {
                            if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                            {
                                TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                            }
                        }
                        TotalRow[col] = TotalPrice;
                    }
                    else
                    {
                        TotalRow[col] = 0;
                    }
                    TotalPrice = 0;
                }
                DTSearchByDateWiseData.Rows.Add(TotalRow);

            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }


            TempData["Result"] = null;
            TempData.Keep();

            ViewData["OBSOrderValue"] = DTSearchByDateWiseData;

            return View(objJM);
        }


        [HttpGet]
        public ActionResult ExportObsOrderValue(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;
                int GrTotal = 0;
                int GRCol = 0;

                package.Workbook.Worksheets.Add("Data");
                DataTable grid = CreateExportableGridObsOrderValue();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        sheet.Cells[1, col].Value = column.ColumnName.ToString();
                        sheet.Column(col++).Width = 18;
                    }
                }

                for (int gridRow = 0; gridRow < grid.Rows.Count - 1; gridRow++)
                {
                    col = 1;
                    foreach (DataColumn column in grid.Columns)
                    {
                        if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                        {
                            if (grid.Rows[gridRow][column].ToString() == string.Empty)
                            {
                                sheet.Cells[row, col].Value = "0";
                            }
                            else
                            {
                                sheet.Cells[row, col].Value = grid.Rows[gridRow][column].ToString();
                            }
                            col++;
                        }
                    }
                    row++;
                }

                col = 1;

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        if (col != 1)
                        {
                            GrTotal = GrTotal + Convert.ToInt32(grid.Rows[grid.Rows.Count - 1][column].ToString());
                        }

                        if (grid.Rows[grid.Rows.Count - 1][column].ToString() == string.Empty)
                        {
                            sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            sheet.Cells[row, col].Value = "0";
                        }
                        else
                        {
                            sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            sheet.Cells[row, col].Value = grid.Rows[grid.Rows.Count - 1][column].ToString();
                        }
                        col++;
                    }
                }
                row++;
                GRCol = 1;
                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                sheet.Cells[row, GRCol].Value = "Grand Total";

                GRCol++;
                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                sheet.Cells[row, GRCol].Value = GrTotal.ToString();





                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        */

        public ActionResult OBSOrderValue()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            int TotalPrice = 0;
            int TotalColPrice = 0;
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


            if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) || (TempData["month"] != null && TempData["month"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["FYear"]);
                month = Convert.ToString(TempData["month"]);


                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    DataColumn TotalCol = DTSearchByDateWiseData.Columns.Add();

                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);



                    for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                    {
                        for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                        {
                            if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                            {
                                if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                                {
                                    TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                                }
                            }
                        }
                        DTSearchByDateWiseData.Columns.Add(TotalCol);
                        TotalColPrice = 0;
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

                ViewData["OBSOrderValue"] = DTSearchByDateWiseData;

            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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

                if (Fyear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        Fyear = CurYear + "-" + NexYear;
                    else
                        Fyear = PreYear + "-" + CurYear;



                }

                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);



                if (DTSearchByDateWiseData.Rows.Count > 0)
                {

                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    DataColumn TotalCol = new DataColumn();

                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                    DataColumn newCol = new DataColumn("Total", typeof(string));
                    newCol.AllowDBNull = true;
                    DTSearchByDateWiseData.Columns.Add(newCol);
                    for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                    {

                        for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                        {
                            if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                            {
                                if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                                {
                                    TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                                }
                            }

                        }
                        DTSearchByDateWiseData.Rows[row1]["Total"] = TotalColPrice;


                        TotalColPrice = 0;
                    }
                }


            }
            ViewData["OBSOrderValue"] = DTSearchByDateWiseData;




            return View(objJM);
        }

        [HttpPost]
        public ActionResult OBSOrderValue(string FYear, string monthID)
        {
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            int TotalPrice = 0;
            int TotalColPrice = 0;
            dtFinancialYear = objJob.GetFinancialYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {

                    lstFinancialYear.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["FYear"])
                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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
            Session["GetExcelData"] = null;

            if (FYear != string.Empty || monthID != string.Empty)
            {
                TempData["FYear"] = FYear;
                TempData["month"] = monthID;


                TempData.Keep();
            }
            else
            {
                TempData.Clear();

                if (FYear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        FYear = CurYear + "-" + NexYear;
                    else
                        FYear = PreYear + "-" + CurYear;

                }
            }




            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(FYear, monthID);


            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        TotalRow[col] = "Total";
                    }
                    else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                    {
                        for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                        {
                            if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                            {
                                TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                            }
                        }
                        TotalRow[col] = TotalPrice;
                    }
                    else
                    {
                        TotalRow[col] = 0;
                    }
                    TotalPrice = 0;
                }
                DTSearchByDateWiseData.Rows.Add(TotalRow);

                DataColumn newCol = new DataColumn("Total", typeof(string));
                newCol.AllowDBNull = true;
                DTSearchByDateWiseData.Columns.Add(newCol);
                for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                {

                    for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                    {
                        if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                        {
                            if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                            {
                                TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                            }
                        }

                    }
                    DTSearchByDateWiseData.Rows[row1]["Total"] = TotalColPrice;


                    TotalColPrice = 0;
                }

            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }


            TempData["Result"] = null;
            TempData.Keep();

            ViewData["OBSOrderValue"] = DTSearchByDateWiseData;

            return View(objJM);
        }
        [HttpGet]
        public ActionResult ExportObsOrderValue(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;
                int GrTotal = 0;
                int GRCol = 0;
                int GrColTotal = 0;
                System.Globalization.CultureInfo hindi = new System.Globalization.CultureInfo("hi-IN");

                package.Workbook.Worksheets.Add("Data");
                DataTable grid = CreateExportableGridObsOrderValue();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        sheet.Cells[1, col].Value = column.ColumnName.ToString();
                        sheet.Column(col++).Width = 18;
                    }
                }

                for (int gridRow = 0; gridRow < grid.Rows.Count - 1; gridRow++)
                {
                    col = 1;
                    foreach (DataColumn column in grid.Columns)
                    {
                        if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                        {
                            if (grid.Rows[gridRow][column].ToString() == string.Empty)
                            {
                                sheet.Cells[row, col].Value = "0";
                            }
                            else
                            {
                                sheet.Cells[row, col].Value = grid.Rows[gridRow][column].ToString();
                            }
                            col++;
                        }
                    }
                    row++;
                }

                col = 1;

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        if (col != 1)
                        {
                            GrTotal = GrTotal + Convert.ToInt32(grid.Rows[grid.Rows.Count - 1][column].ToString());
                        }

                        if (grid.Rows[grid.Rows.Count - 1][column].ToString() == string.Empty)
                        {
                            sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            sheet.Cells[row, col].Value = "0";
                        }
                        else
                        {
                            if (col != 1)
                            {
                                int parsedAmt = int.Parse(grid.Rows[grid.Rows.Count - 1][column].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                string TotalAmt = string.Format(hindi, "{0:c0}", parsedAmt);
                                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                                sheet.Cells[row, col].Value = TotalAmt.ToString();
                            }
                            else
                            {

                                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                                sheet.Cells[row, col].Value = grid.Rows[grid.Rows.Count - 1][column].ToString();
                            }
                        }
                        col++;
                    }
                }
                // row++;
                GRCol = col++;
                //sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                //sheet.Cells[row, GRCol].Value = "Grand Total";

                //GRCol++;
                //sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                //sheet.Cells[row, GRCol].Value = GrTotal.ToString();

                row = 1;

                int TotalColPrice = 0;

                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                sheet.Cells[row, GRCol].Value = "Total";

                row++;
                for (int row1 = 0; row1 < grid.Rows.Count - 1; row1++)
                {
                    col = 1;
                    for (int col2 = 0; col2 < grid.Columns.Count; col2++)
                    {
                        if (grid.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && grid.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && grid.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                        {
                            if (grid.Rows[row1][grid.Columns[col2].ColumnName] != DBNull.Value && grid.Rows[row1][grid.Columns[col2].ColumnName] != string.Empty)
                            {
                                TotalColPrice = TotalColPrice + Convert.ToInt32(grid.Rows[row1][grid.Columns[col2].ColumnName]);
                            }
                            col++;
                        }

                    }
                    //grid.Columns.Add(TotalCol);
                    col++;


                    sheet.Cells[row, col].Value = TotalColPrice.ToString();
                    GrColTotal = GrColTotal + TotalColPrice;
                    TotalColPrice = 0;

                    row++;
                }
                // row++;
                decimal parsed = decimal.Parse(GrColTotal.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                string GrandAmttext = string.Format(hindi, "{0:c0}", parsed);

                sheet.Cells[row, col, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, col, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aquamarine);
                sheet.Cells[row, col].Value = GrandAmttext.ToString();


                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "OBSwiseJobCreation-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private DataTable CreateExportableGridObsOrderValue()
        {

            DataTable grid = GetDataObsOrderValue();


            return grid;
        }

        public DataTable GetDataObsOrderValue()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            //if (!IsPostBack())   //// False if initial Load
            //{
            //    TempData.Clear();
            //}


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;
            int TotalPrice = 0;




            if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) || (TempData["month"] != null && TempData["month"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["FYear"]);
                month = Convert.ToString(TempData["month"]);


                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);

                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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

                if (Fyear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        Fyear = CurYear + "-" + NexYear;
                    else
                        Fyear = PreYear + "-" + CurYear;



                }

                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValue(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {

                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                }
            }
            ViewData["OBSOrderValue"] = DTSearchByDateWiseData;


            return DTSearchByDateWiseData;
        }

        public ActionResult CompetancyMatrix()
        {

            
            return View();
        }

        public ActionResult PlayVideo(string ID)
        {
            TrainingScheduleModel trg = new TrainingScheduleModel();

            DataSet DTSearchByDateWiseData = new DataSet();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            trg.PK_TrainingScheduleId = Convert.ToInt32(ID);

            DTSearchByDateWiseData = objDALMisReport.GetAttendance(ID);


            if (DTSearchByDateWiseData.Tables.Count > 0)
            {
                if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                {
                    trg.TrainingName = DTSearchByDateWiseData.Tables[0].Rows[0]["TrainingTopic"].ToString();
                    trg.FileName = DTSearchByDateWiseData.Tables[0].Rows[0]["FileName"].ToString();
                }


                if (DTSearchByDateWiseData.Tables[1].Rows.Count > 0)
                {
                    if (DTSearchByDateWiseData.Tables[1].Rows[0]["IsPresent"].ToString() == "1")
                        trg.chkVideoSeen = true;
                    else
                        trg.chkVideoSeen = false;
                }
                else
                {
                    trg.chkVideoSeen = false;
                }
            }
            else
            {
                trg.chkVideoSeen = false;
            }


            return View(trg);
        }


        [HttpPost]
        public ActionResult PlayVideo(TrainingScheduleModel N, FormCollection fc)
        {
            string Result = string.Empty;

            if (N.chkVideoSeen)
            {
                objTSM.RIsPresent = N.chkVideoSeen;
                objTSM.RTraineeId = Convert.ToString(Session["UserID"]);
                objTSM.RTrainingScheduleId = Convert.ToString(N.PK_TrainingScheduleId);
                objTSM.TraineeName = Convert.ToString(Session["fullName"]);
                objTSM.TrainingName = N.TrainingName;
                Result = objDTS.AddTrainingAttendance(objTSM);
            }

//            return View(objTSM);
            return RedirectToAction("PlayVideo", new { ID = Convert.ToString(N.PK_TrainingScheduleId) });
        }

        [HttpGet]
        public ActionResult LessonLearnt()
        {
            LessionLearntModel OBJLessionleant = new LessionLearntModel();

            try
            {


                DataSet ds = new DataSet();

                List<LessionLearntModel> lstLessionLearntModel = new List<LessionLearntModel>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = objDALMisReport.GetUserDataSheet();


                if (ds.Tables.Count > 0)
                {
                    OBJLessionleant.dtLessionLeant = ds.Tables[0];
                    ViewData["LessionLearntModel"] = lstLessionLearntModel;
                }

                //   objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLessionleant);
        }

        /*
        public ActionResult Download1(string d, string TableType)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            string strTable = string.Empty;

            if (TableType.ToUpper() == "COMPLAINT REGISTER")
                strTable = "C";
            else
                strTable = "S";

            DTDownloadFile = objDALMisReport.GetFileContent(Convert.ToInt32(d), strTable);

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

            string path = Server.MapPath("~/Content/JobDocument/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            string downloadPath = Server.MapPath("~/JobDocument/") + FileName;
            System.IO.File.WriteAllBytes(downloadPath, bytes);

            string viewPath = "../JobDocument/" + FileName;
            //Send the File to Download.
            //return Json(viewPath, JsonRequestBehavior.AllowGet);
            return File(bytes, "application/octet-stream", FileName);
        }
        */

        public ActionResult Download1(string d, string TableType)
        {

            string FileName = "";
            string FileID = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            string strTable = string.Empty;

            if (TableType.ToUpper() == "COMPLAINT REGISTER")
                strTable = "C";
            else
                strTable = "S";

            DTDownloadFile = objDALMisReport.GetFileContent(Convert.ToInt32(d), strTable);

            if (DTDownloadFile.Rows.Count > 0)
            {
                //fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                FileName = DTDownloadFile.Rows[0]["FileName"].ToString();
                FileID = DTDownloadFile.Rows[0]["FileID"].ToString();
                Date = DTDownloadFile.Rows[0]["CreatedDate"].ToString();
            }

            //string myDate = "05/11/2010";
            DateTime date = Convert.ToDateTime(Date);
            int year = date.Year;
            int Month = date.Month;

            int intC = Convert.ToInt32(Month);
            string CurrentMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(intC);
            

            string path = string.Empty; // Server.MapPath("~/Content/JobDocument/") + FileName;
            // string path = Server.MapPath("~/Content/") + d;

            if (System.IO.File.Exists(Server.MapPath("~/Content/JobDocument/") + FileName))
            {
                path = Server.MapPath("~/Content/JobDocument/") + FileName;
            }
            else
            {
                path = Server.MapPath("~/Content/JobDocument/") + FileID;
            }

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            string downloadPath = string.Empty;


            if (System.IO.File.Exists(Server.MapPath("~/Content/JobDocument/") + FileName))
            {
                downloadPath = Server.MapPath("~/Content/JobDocument/") + FileName;
            }
            else
            {
                downloadPath = Server.MapPath("~/Content/JobDocument/") + FileID;
            }

            System.IO.File.WriteAllBytes(downloadPath, bytes);


           
            return File(bytes, "application/octet-stream", FileName);
        }

        [HttpPost]
        public JsonResult UpdateLessionLearnt(int ID)
        {
            LessionLearntModel OBJLessionleant = new LessionLearntModel();

            try
            {


                DataSet ds = new DataSet();

                List<LessionLearntModel> lstLessionLearntModel = new List<LessionLearntModel>();
                Session["GetExcelData"] = "Yes";


                string LessionLearntID = null;

                TempData.Keep();

                ds = objDALMisReport.UpdateLessionLeantData(Convert.ToInt32(ID));


                if (ds.Tables.Count > 0)
                {
                    ds = objDALMisReport.GetUserDataSheet();


                    return Json(new { Result = "OK" });

                }

                //   objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "OK" });
        }

        [HttpGet]
        public ActionResult GetLessonLearntHstory()
        {
            LessionLearntModel OBJLessionleant = new LessionLearntModel();

            try
            {


                DataSet ds = new DataSet();

                List<LessionLearntModel> lstLessionLearntModel = new List<LessionLearntModel>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = objDALMisReport.GetLessionLerntHist();


                if (ds.Tables.Count > 0)
                {
                    OBJLessionleant.dtLessionLeant = ds.Tables[0];
                    ViewData["LessionLearntModel"] = lstLessionLearntModel;
                }

                //   objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLessionleant);
        }

        [HttpGet]
        public ActionResult ViewLessonLearntUserHstory()
        {
            LessionLearntModel OBJLessionleant = new LessionLearntModel();

            try
            {


                DataSet ds = new DataSet();

                List<LessionLearntModel> lstLessionLearntModel = new List<LessionLearntModel>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();

                ds = objDALMisReport.ViewLessonLearntUserHstory();


                if (ds.Tables.Count > 0)
                {
                    OBJLessionleant.dtLessionLeant = ds.Tables[0];
                    ViewData["LessionLearntModel"] = lstLessionLearntModel;
                }

              //  objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLessionleant);
        }

        [HttpGet]
        public ActionResult ViewLessonLearntUserDetail(int ID)
        {
            LessionLearntModel OBJLessionleant = new LessionLearntModel();

            try
            {


                DataSet ds = new DataSet();

                List<LessionLearntModel> lstLessionLearntModel = new List<LessionLearntModel>();
                Session["GetExcelData"] = "Yes";




                TempData.Keep();
                OBJLessionleant.LessionLeantID = ID;

               ds = objDALMisReport.ViewLessonLearntUserDetailHstory(ID);



                if (ds.Tables.Count > 0)
                {
                    OBJLessionleant.dtLessionLeant = ds.Tables[0];
                    ViewData["LessionLearntModel"] = lstLessionLearntModel;

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        OBJLessionleant.StrLessionLeantID = ds.Tables[1].Rows[0]["LessionLeantNumber"].ToString();
                        /////ViewData["StrLessionLeantID"] = StrLessionLeantID;
                    }
                }

                

                //   objProfile.LstUserData = lstUserDashBoard;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View(OBJLessionleant);
        }

        [HttpGet]
        public ActionResult GetLessionLearntUselist(int ID)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "LessionLearntuserDetail.xls"));
            Response.ContentType = "application/ms-excel";
            DataTable dt=objDALMisReport.ViewLessonLearntUserDetailHstoryExcel(ID);
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }

        #region Quotation Report With Mandays 
        public ActionResult QuotationReportWithMandays()
        {
            Session["GetExcelData"] = "Yes";
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALMisReport.QuotaionMastertDashBoardWithMandays();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
            //return View();
        }

        [HttpPost]
        public ActionResult QuotationReportWithMandays(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();
            DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDateWithMandays(FromDate, ToDate);
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           //ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           Type = Convert.ToString(dr["OBSType"])
                       }
                    );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return View(objQM);
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
        }

        #endregion


        //Nikita 19 Aug 2023

        public ActionResult RegEnquiryReportOrderTypeWise()

        {
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }

            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            {
                TempData.Keep();
                FromDate = Convert.ToString(TempData["FromDate"]);
                ToDate = Convert.ToString(TempData["ToDate"]);
                objEM.FromDate = Convert.ToDateTime(FromDate);
                objEM.ToDate = Convert.ToDateTime(ToDate);

                DTSearchByDateWiseData = objDalEnquiryMaster.GetRegDataByDateOrderTypeWise(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new EnquiryMaster
                           {
                               //commented by nikita
                               Count = DTSearchByDateWiseData.Rows.Count,

                               //added by nikita
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),//added by nikita 09082023
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),commenetd by nikita
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),commenetd by nikita
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),//added by nikita 09082023
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),//added by nikita 09082023
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {
                lstEnquiryMast = objDalEnquiryMaster.GetRegEnquiryListDashBoardOrderTypeWise();
            }


            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }

        [HttpPost]
        public ActionResult RegEnquiryReportOrderTypeWise(string FromDate, string ToDate)
        {

            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }


            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDalEnquiryMaster.GetRegDataByDateOrderTypeWise(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new EnquiryMaster
                       {
                           Count = DTSearchByDateWiseData.Rows.Count,

                           //ContactNo = Convert.ToString(dr["ContactNo"]),
                           //ContactName = Convert.ToString(dr["ContactName"]),
                           //ARC = Convert.ToString(dr["ARC"]),
                           //ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                           //EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                           //EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           //EnquiryDescription = Convert.ToString(dr["Description"]),
                           //ProjectName = Convert.ToString(dr["ProjectName"]),
                           //Company_Name = Convert.ToString(dr["CompanyName"]),
                           //Branch = Convert.ToString(dr["OriginatingBranch"]),
                           //OpendateS = Convert.ToString(dr["DateOpened"]),
                           //Source = Convert.ToString(dr["source"]),
                           //EstCloseS = Convert.ToString(dr["EstClose"]),
                           //ProjectType = Convert.ToString(dr["ProjectType"]),
                           //PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           //SubServiceType = Convert.ToString(dr["ServiceType"]),
                           //Status = Convert.ToString(dr["status"]),
                           //RegretReason = Convert.ToString(dr["RegretReason"]),
                           //CreatedBy = Convert.ToString(dr["OwnerName"]),
                           //DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                           //IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                           //TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           //IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                           //Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                           //Intlocation = Convert.ToString(dr["Intlocation"]),
                           //Dcurrency = Convert.ToString(dr["Dcurrency"]),
                           //Icurrency = Convert.ToString(dr["ICurrency"])
                           RefDate = Convert.ToString(dr["refDate"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           Notes = Convert.ToString(dr["Notes"]),
                           NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                           //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                           EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                           ContactNo = Convert.ToString(dr["ContactNo"]),
                           ContactName = Convert.ToString(dr["ContactName"]),
                           ARC = Convert.ToString(dr["ARC"]),
                           ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                           EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           EnquiryDescription = Convert.ToString(dr["Description"]),
                           ProjectName = Convert.ToString(dr["ProjectName"]),
                           Company_Name = Convert.ToString(dr["CompanyName"]),
                           Branch = Convert.ToString(dr["OriginatingBranch"]),
                           OpendateS = Convert.ToString(dr["DateOpened"]),
                           Source = Convert.ToString(dr["source"]),
                           EstCloseS = Convert.ToString(dr["EstClose"]),
                           ProjectType = Convert.ToString(dr["ProjectType"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),
                           //SubServiceType = Convert.ToString(dr["ServiceType"]),
                           Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                           Status = Convert.ToString(dr["status"]),
                           RegretActionTaken = Convert.ToString(dr["EnquiryRegretActionTaken"]),
                           RegretReason = Convert.ToString(dr["RegretReason"]),
                           //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                           Owner = Convert.ToString(dr["OwnerName"]),
                           //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),
                           //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),
                           DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                           IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                           TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                           Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           Intlocation = Convert.ToString(dr["Intlocation"]),
                           Dcurrency = Convert.ToString(dr["Dcurrency"]),
                           Icurrency = Convert.ToString(dr["ICurrency"]),
                           LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objEM.lst1 = lstEnquiryMast;
                return View(objEM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;
            return View(objEM);
        }




        [HttpGet]
        public ActionResult ExportIndexRegEnquiryReportOrderType_Wise(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<EnquiryMaster> grid = CreateRegExportableGridEnquiryReportOrderTypeWise();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<EnquiryMaster> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }
                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "RegEnquiryReportOrderTypeWise-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<EnquiryMaster> CreateRegExportableGridEnquiryReportOrderTypeWise()
        {
            IGrid<EnquiryMaster> grid = new Grid<EnquiryMaster>(GetDataRegEnquiryReportOrderWise());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            //grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            //grid.Columns.Add(model => model.Branch).Titled("Branch");
            //grid.Columns.Add(model => model.Source).Titled("Source");
            //grid.Columns.Add(model => model.ProjectName).Titled("Project Name");
            //grid.Columns.Add(model => model.Company_Name).Titled("Client Name");
            //grid.Columns.Add(model => model.ProjectType).Titled("OBS Type");
            //grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");
            //grid.Columns.Add(model => model.SubServiceType).Titled("Service Type");
            //grid.Columns.Add(model => model.CreatedBy).Titled("Owner Name");
            //grid.Columns.Add(model => model.EnquiryDescription).Titled("Description");
            //grid.Columns.Add(model => model.TEstimatedAmount).Titled("Total Estimated Amount");
            //grid.Columns.Add(model => model.OpendateS).Titled("Creation Date");
            //grid.Columns.Add(model => model.EstCloseS).Titled("Estimated Closure Date");
            //grid.Columns.Add(model => model.ModifiedDateS).Titled("Modification Date");
            //grid.Columns.Add(model => model.Status).Titled("Enquiry Status");
            //grid.Columns.Add(model => model.RegretReason).Titled("Regret Reason");
            //grid.Columns.Add(model => model.Domesticlocation).Titled("Domestic");
            //grid.Columns.Add(model => model.Intlocation).Titled("International");
            //grid.Columns.Add(model => model.DEstimatedAmount).Titled("Domestic Amount");
            //grid.Columns.Add(model => model.IEstimatedAmount).Titled("International Amount");
            //grid.Columns.Add(model => model.ARC).Titled("ARC Job");
            //grid.Columns.Add(model => model.ContactName).Titled("Contact Name");
            //grid.Columns.Add(model => model.ContactNo).Titled("Contact Number");

            grid.Columns.Add(model => model.EnquiryNumber).Titled("Enquiry Number");
            grid.Columns.Add(c => c.Branch).Titled("Branch");
            grid.Columns.Add(c => c.Source).Titled("Source");
            grid.Columns.Add(c => c.RefDate).Titled("Enquiry receipt date");
            grid.Columns.Add(c => c.EstCloseS).Titled("Estimated closure date");
            grid.Columns.Add(c => c.EnquiryReferenceNo).Titled("Enquiry reference details");
            grid.Columns.Add(c => c.Company_Name).Titled("Customer Name");
            grid.Columns.Add(c => c.EndCustomer).Titled("End customer name");
            grid.Columns.Add(c => c.ProjectType).Titled("OBS Type");
            grid.Columns.Add(c => c.PortfolioType).Titled("Portfolio Type");
            grid.Columns.Add(c => c.Type).Titled("Service Type");
            grid.Columns.Add(c => c.ProjectName).Titled("Project Name");
            grid.Columns.Add(c => c.EnquiryDescription).Titled("Description");
            grid.Columns.Add(c => c.ModifiedBy).Titled("Modified by");
            grid.Columns.Add(c => c.ARC).Titled("ARC Job");
            grid.Columns.Add(c => c.Domesticlocation).Titled("Domestic");
            grid.Columns.Add(c => c.Intlocation).Titled("International");
            grid.Columns.Add(c => c.DEstimatedAmount).Titled("Domestic Amount");
            grid.Columns.Add(c => c.IEstimatedAmount).Titled("International Amount");
            grid.Columns.Add(c => c.TEstimatedAmount).Titled("Total Estimated Amount");
            grid.Columns.Add(c => c.Status).Titled("Enquiry Status");
            grid.Columns.Add(c => c.RegretReason).Titled("Regret Reason");
            grid.Columns.Add(c => c.RegretActionTaken).Titled("Regret Action taken");
            grid.Columns.Add(c => c.OrderType).Titled("Ordere Type");
            grid.Columns.Add(c => c.ContactName).Titled("Customer Contact Person Name");
            grid.Columns.Add(c => c.ContactNo).Titled("Customer Contact Person Number");
            grid.Columns.Add(c => c.Owner).Titled("Enquiry Created By");
            grid.Columns.Add(c => c.LeadGivenBy).Titled("Lead Given By");
            grid.Columns.Add(c => c.NotesbyLeads).Titled("Notes by Leads");
            grid.Columns.Add(c => c.OpendateS).Titled("Creation Date");
            //colCmns.Add(c => c.EstCloseS).Titled("Estimated Closure Date");
            grid.Columns.Add(c => c.ModifiedDateS).Titled("Modification Date");
            //grid.Columns.Add(c => c.Quotation).Titled("Quotation Number");
            //grid.Columns.Add(c => c.JobNumber).Titled("Job Number");






            grid.Pager = new GridPager<EnquiryMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objEM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            return grid;
        }

        public List<EnquiryMaster> GetDataRegEnquiryReportOrderWise()
        {



            List<EnquiryMaster> lstEnquiryMast = new List<EnquiryMaster>();
            DataTable DTSearchByDateWiseData = new DataTable();


            //if (Session["FromDate"] == null && Session["ToDate"] == null)//commented by nikita 
            if (Session["FromDate"] != null && Session["ToDate"] != null)//added by nikita

            {
                DTSearchByDateWiseData = objDalEnquiryMaster.GetRegDataByDateOrderTypeWise(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new EnquiryMaster
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               //ContactNo = Convert.ToString(dr["ContactNo"]),
                               //ContactName = Convert.ToString(dr["ContactName"]),
                               //ARC = Convert.ToString(dr["ARC"]),
                               //ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               //EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               //EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               //EnquiryDescription = Convert.ToString(dr["Description"]),
                               //ProjectName = Convert.ToString(dr["ProjectName"]),
                               //Company_Name = Convert.ToString(dr["CompanyName"]),
                               //Branch = Convert.ToString(dr["OriginatingBranch"]),
                               //OpendateS = Convert.ToString(dr["DateOpened"]),
                               //Source = Convert.ToString(dr["source"]),
                               //EstCloseS = Convert.ToString(dr["EstClose"]),
                               //ProjectType = Convert.ToString(dr["ProjectType"]),
                               //PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               //Status = Convert.ToString(dr["status"]),
                               //RegretReason = Convert.ToString(dr["RegretReason"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),
                               //DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),
                               //IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),
                               //TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               //IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               //Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               //Intlocation = Convert.ToString(dr["Intlocation"]),
                               //Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               //Icurrency = Convert.ToString(dr["ICurrency"])

                               //added by mikita 
                               RefDate = Convert.ToString(dr["refDate"]),
                               EndCustomer = Convert.ToString(dr["EndCustomer"]),
                               Notes = Convert.ToString(dr["Notes"]),
                               NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                               //NotesbyLeads = Convert.ToString(dr["NotesbyLeads"]),
                               EnquiryReferenceNo = Convert.ToString(dr["EnquiryReferenceNo"]),
                               ContactNo = Convert.ToString(dr["ContactNo"]),
                               ContactName = Convert.ToString(dr["ContactName"]),
                               ARC = Convert.ToString(dr["ARC"]),
                               ModifiedDateS = Convert.ToString(dr["ModifiedDate"]),
                               EQ_ID = Convert.ToInt32(dr["EQ_ID"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               EnquiryDescription = Convert.ToString(dr["Description"]),
                               ProjectName = Convert.ToString(dr["ProjectName"]),
                               Company_Name = Convert.ToString(dr["CompanyName"]),
                               Branch = Convert.ToString(dr["OriginatingBranch"]),
                               OpendateS = Convert.ToString(dr["DateOpened"]),
                               Source = Convert.ToString(dr["source"]),
                               EstCloseS = Convert.ToString(dr["EstClose"]),
                               ProjectType = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               //SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Type = Convert.ToString(dr["ServiceType"]),//added by nikita 09082023
                               Status = Convert.ToString(dr["status"]),
                               RegretReason = Convert.ToString(dr["RegretReason"]),
                               RegretActionTaken = Convert.ToString(dr["EnquiryRegretActionTaken"]),
                               //CreatedBy = Convert.ToString(dr["OwnerName"]),//commented by nikita 09082023
                               Owner = Convert.ToString(dr["OwnerName"]),//added by nikita 09082023
                               //DEstimatedAmount = "", //Convert.ToString(dr["DEstimatedAmount"]),commenetd by nikita
                               //IEstimatedAmount = "",//Convert.ToString(dr["IEstimatedAmount"]),commenetd by nikita
                               DEstimatedAmount = Convert.ToString(dr["DEstimatedAmount"]),//added by nikita 09082023
                               IEstimatedAmount = Convert.ToString(dr["IEstimatedAmount"]),//added by nikita 09082023
                               TEstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                               IExchangeRate = Convert.ToString(dr["IExchangeRate"]),
                               Domesticlocation = Convert.ToString(dr["Domesticlocation"]),
                               OrderType = Convert.ToString(dr["OrderType"]),

                               Intlocation = Convert.ToString(dr["Intlocation"]),
                               Dcurrency = Convert.ToString(dr["Dcurrency"]),
                               Icurrency = Convert.ToString(dr["ICurrency"]),
                               LeadGivenBy = Convert.ToString(dr["LeadGivenBy"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

            }
            else
            {
                lstEnquiryMast = objDalEnquiryMaster.GetRegEnquiryListDashBoardOrderTypeWise();
            }




            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objEM.lst1 = lstEnquiryMast;

            return objEM.lst1;
        }



        //added by nikita 30-08-2023 start

        #region Export to excel Quotation Report WithMandays

        [HttpGet]
        public ActionResult ExportIndexQuotationReportWithMandays()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuotationMaster> grid = CreateExportableGridQuotationReportMandays();
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

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "QuotationReport-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<QuotationMaster> CreateExportableGridQuotationReportMandays()
        {
            IGrid<QuotationMaster> grid = new Grid<QuotationMaster>(GetDataQuotationMasterMandays());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            grid.Columns.Add(model => model.CreatedDate).Titled("Quotation Date");
            grid.Columns.Add(model => model.OrderType).Titled("OrderType");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Validity Date");
            grid.Columns.Add(model => model.EstimatedAmount).Titled("Estimate Amount");
            grid.Columns.Add(model => model.Quotation_Description).Titled("Project Name");
            grid.Columns.Add(model => model.Name).Titled("Service Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.Type).Titled("OBS Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.CompanyAddress).Titled("Client Name");
            grid.Columns.Add(model => model.BranchName).Titled("Originating Branch");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Est Close Date");
            grid.Columns.Add(model => model.StatusType).Titled("Status");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");//added by nikita 29-08-2023

            grid.Pager = new GridPager<QuotationMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objQM.lstQuotationMasterDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<QuotationMaster> GetDataQuotationMasterMandays()
        {

            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.QuotaionMastertDashBoardWithMandays();
            }
            else
            {

                //U.FromDate = Session["FromDate"].ToString();
                //U.ToDate = Session["ToDate"].ToString();
                DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDateWithMandays(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }



            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           Count = DTSearchRecordByDate.Rows.Count,
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                           IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),//added by nikita 30-08-2023
                           Type = Convert.ToString(dr["OBSType"])//added by nikita 30-08-2023
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return objQM.lstQuotationMasterDashBoard1;
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return objQM.lstQuotationMasterDashBoard1;
        }

        #endregion

        //end by nikita



        //code added by nikita for QHSE pages

        [HttpGet]
        public ActionResult AuditreportDashBoard()
        {

            DataTable DTDash = new DataTable();
            DataSet dsGetAudit = new DataSet();
            DTDash = DalMisReportQHSE.GetAuditReportDashBoard();
            List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
            try
            {
                if (DTDash.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDash.Rows)
                    {
                        lstReport.Add(
                            new Internal_Audit_Report
                            {
                                AuditId = Convert.ToInt32(dr["AuditId"]),
                                Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Auditor = Convert.ToString(dr["Auditor"]),
                                ExAuditor = Convert.ToString(dr["ExAuditor"]),
                                Auditee = Convert.ToString(dr["Auditee"]),
                                Department = Convert.ToString(dr["Department"]),

                                ActualAuditDateTo = Convert.ToString(dr["ActualAuditDateTo"]),
                                ProposeDateFrom = Convert.ToString(dr["ProposeDateFrom"]),
                                ProposeDateTo = Convert.ToString(dr["ProposeDateTo"]),

                                Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]),
                                Total_NCR_raised = Convert.ToInt32(dr["TotalFindings"]),
                                PDF = Convert.ToString(dr["PDF"]),
                                NCDocument = Convert.ToString(dr["NCDocument"]),
                                SupportingDocument = Convert.ToString(dr["SupportingDocument"]),
                                IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]),
                                AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),

                                CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]),
                                MajorNCCount = Convert.ToString(dr["MajorNCCount"]),
                                MinorNCCount = Convert.ToString(dr["MinorNCCount"]),
                                OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]),
                                ConcernCount = Convert.ToString(dr["ConcernCount"]),
                                OFICount = Convert.ToString(dr["OFICount"]),


                            });
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            ViewData["dashboardreport"] = lstReport;
            InterAud.lstReport1 = lstReport;
            return View(InterAud);

        }

        //export to excel for AuditReport
        [HttpGet]
        public ActionResult ExportIndexAuditReport()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<Internal_Audit_Report> grid = CreateExportableGridAuditReport();
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<Internal_Audit_Report> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                string filename = "AuditReport-" + formattedDateTime + ".xlsx";
                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<Internal_Audit_Report> CreateExportableGridAuditReport()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Internal_Audit_Report> grid = new Grid<Internal_Audit_Report>(GetDataAuditReport());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.Plan_ref_No).Titled("Plan Ref Serial No.");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.Auditor).Titled("Auditor Name");
            grid.Columns.Add(model => model.ExAuditor).Titled("External Auditor Name");
            grid.Columns.Add(model => model.Auditee).Titled("Auditees");
            grid.Columns.Add(model => model.ProposeDateFrom).Titled("Proposed Date of Audit (From Date)");
            grid.Columns.Add(model => model.ProposeDateTo).Titled("Proposed Date of Audit (To Date)");
            grid.Columns.Add(model => model.ActualAuditDateTo).Titled("Audit Date(s)");
            grid.Columns.Add(model => model.Department).Titled("Process audited");
            grid.Columns.Add(model => model.Total_NCR_raised).Titled("Total Findings");
            grid.Columns.Add(model => model.IsAuditCompleted).Titled("Is Audit Completed");
            grid.Columns.Add(model => model.AreFindingsClose).Titled("Are Findings Close");
            grid.Columns.Add(model => model.NCDocument).Titled("NC Document");


            //grid.Columns.Add(model => model.Date_Of_Audit).Titled("Date of Audit");
            //grid.Columns.Add(model => model.PDF).Titled("PDF");
            //grid.Columns.Add(model => model.SupportingDocument).Titled("Supporting Document");



            grid.Pager = new GridPager<Internal_Audit_Report>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = InterAud.lstReport1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<Internal_Audit_Report> GetDataAuditReport()
        {

            DataTable DTDash = new DataTable();
            DataSet dsGetAudit = new DataSet();
            DTDash = DalMisReportQHSE.GetAuditReportDashBoard();
            List<Internal_Audit_Report> lstReport = new List<Internal_Audit_Report>();
            try
            {
                if (DTDash.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTDash.Rows)
                    {
                        lstReport.Add(
                            new Internal_Audit_Report
                            {
                                Count = DTDash.Rows.Count,
                                AuditId = Convert.ToInt32(dr["AuditId"]),
                                Internal_Audit_Id = Convert.ToInt32(dr["Internal_Audit_Id"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Auditor = Convert.ToString(dr["Auditor"]),

                                ExAuditor = Convert.ToString(dr["ExAuditor"]),
                                Auditee = Convert.ToString(dr["Auditee"]),
                                Department = Convert.ToString(dr["Department"]),
                                ActualAuditDateTo = Convert.ToString(dr["ActualAuditDateTo"]),
                                ProposeDateFrom = Convert.ToString(dr["ProposeDateFrom"]),
                                ProposeDateTo = Convert.ToString(dr["ProposeDateTo"]),
                                Plan_ref_No = Convert.ToString(dr["Plan_ref_No"]),
                                Total_NCR_raised = Convert.ToInt32(dr["TotalFindings"]),
                                PDF = Convert.ToString(dr["PDF"]),
                                NCDocument = Convert.ToString(dr["NCDocument"]),
                                SupportingDocument = Convert.ToString(dr["SupportingDocument"]),
                                IsAuditCompleted = Convert.ToString(dr["IsAuditCompleted"]),
                                AreFindingsClose = Convert.ToString(dr["AreFindingsClose"]),
                                CriticalNCCount = Convert.ToString(dr["CriticalNCCount"]),
                                MajorNCCount = Convert.ToString(dr["MajorNCCount"]),
                                MinorNCCount = Convert.ToString(dr["MinorNCCount"]),
                                OBSERVATIONCount = Convert.ToString(dr["OBSERVATIONCount"]),
                                ConcernCount = Convert.ToString(dr["ConcernCount"]),
                                OFICount = Convert.ToString(dr["OFICount"]),

                            });
                    }
                }




            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            ViewData["dashboardreport"] = lstReport;
            InterAud.lstReport1 = lstReport;

            return InterAud.lstReport1;
        }




        //code for Technical Compentency Evaluation

        [HttpGet]
        public ActionResult ListTechnicalCompetencyEvaluation()
        {
            List<TechnicalCompetencyEvaluation> lmd = new List<TechnicalCompetencyEvaluation>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = DalMisReportQHSE.GetData(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new TechnicalCompetencyEvaluation
                {

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


        [HttpGet]
        public ActionResult ExportIndexListCompentency()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<TechnicalCompetencyEvaluation> grid = CreateExportableGridListCompentency();
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
        private IGrid<TechnicalCompetencyEvaluation> CreateExportableGridListCompentency()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<TechnicalCompetencyEvaluation> grid = new Grid<TechnicalCompetencyEvaluation>(GetDataListCompentency());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            //added by nikita on 11092023
            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.InspectorName).Titled("Inspector Name");
            grid.Columns.Add(model => model.InspectorBranch).Titled("Branch");
            grid.Columns.Add(model => model.Email).Titled("Email");
            grid.Columns.Add(model => model.Mobile).Titled("Mobile");
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

        public List<TechnicalCompetencyEvaluation> GetDataListCompentency()
        {

            List<TechnicalCompetencyEvaluation> lmd = new List<TechnicalCompetencyEvaluation>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = DalMisReportQHSE.GetData(); // fill dataset  

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




        //Mentee and Mentor list

        public ActionResult MentorList(int? Br_Id, string UserID)
        {
            try
            {



                DataSet DTUserDashBoard1 = new DataSet();
                List<UNameCode> lstUserList = new List<UNameCode>();
                DTUserDashBoard1 = DalMisReportQHSE.GetMentorList();

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
                ds = DalMisReportQHSE.GetDataMentor(UserId1);
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
                    DSEditCompany = DalMisReportQHSE.EditBranch(Br_Id);
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




        //Export To index for mentee and mentor list


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

                return File(package.GetAsByteArray(), "application/unknown", "MenteeAndMentorlist_" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
        }
        private IGrid<Users> CreateExportableGrid(Users U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<Users> grid = new Grid<Users>(GetData(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };



            grid.Columns.Add(model => model.ManteeName).Titled("Employee Name");
            grid.Columns.Add(model => model.IsMentor).Titled("Is Mentor");

            grid.Columns.Add(model => model.MentorName).Titled("Mentor Name");

            grid.Columns.Add(model => model.MentorListName).Titled("Mentee's List");


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
            DTCallDashBoard = DalMisReportQHSE.GetDetails();

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
                                MentorListName = Convert.ToString(dr["MantorList"])

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


        //code for Monitoring Record


        [HttpGet]
        public ActionResult MonitoringRecords()


        {
            try
            {

                Session["GetExcelData"] = "Yes";
                List<MonitorRecordData> lstMonitoringRecord = new List<MonitorRecordData>();  // creating list of model.  
                lstMonitoringRecord = DalMisReportQHSE.GetMonitorrecord();
                ViewData["EnquiryMaster"] = lstMonitoringRecord;
                monitoringrecordata.listingmonitoringrecord = lstMonitoringRecord;
                return View(monitoringrecordata);


            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(monitoringrecordata.listingmonitoringrecord);
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
                    monitoringrecordata.FromDate = Convert.ToString(TempData["FromDate"]);
                    monitoringrecordata.ToDate = Convert.ToString(TempData["ToDate"]);
                    TempData.Keep();
                    DTComplaintDashBoard = DalMisReportQHSE.GetDataByDate(FromDate, ToDate);

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
                        monitoringrecordata.listingmonitoringrecord = lstMonitorrecord;


                    }

                }


                monitoringrecordata.listingmonitoringrecord = lstMonitorrecord;

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return View(monitoringrecordata);



        }



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
            grid.Pager.RowsPerPage = monitoringrecordata.listingmonitoringrecord.Count;

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

                DTSearchByDateWiseData = DalMisReportQHSE.GetDataByDate(FromDate, ToDate);

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
                    monitoringrecordata.listingmonitoringrecord = lstMonitoringrecord;
                    //return objEM.lst1;
                }
            }
            else
            {
                lstMonitoringrecord = DalMisReportQHSE.GetMonitorrecord();
            }

            ViewData["EnquiryMaster"] = lstMonitoringrecord;
            TempData["Result"] = null;
            TempData.Keep();
            monitoringrecordata.listingmonitoringrecord = lstMonitoringrecord;

            return monitoringrecordata.listingmonitoringrecord;
        }


        //code for CAIL


        [HttpGet]
        public ActionResult ListCompentencyMetrixViewMis()
        {
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.  
            DataSet ds = new DataSet();


            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Scope (Name)
            dtGetNAME = DalMisReportQHSE.dtGetNAME();
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


            ds = DalMisReportQHSE.GetDataReport(); // fill dataset  



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


        //safety incident report

        [HttpGet]
        public ActionResult ListSafetyInsidentReportMis()
        {
            List<SafetyInsidentReport> lmd = new List<SafetyInsidentReport>();  // creating list of model.  
            DataSet ds = new DataSet();

            ds = DalMisReportQHSE.GetDataSafetyIncident(); // fill dataset  

            foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>  
            {
                lmd.Add(new SafetyInsidentReport
                {


                    PKId = Convert.ToInt32(dr["PKId"]),
                    TiimesUIN = Convert.ToString(dr["UniqueNumber"]),
                    Branch = Convert.ToString(dr["Branch"]),
                    DateofReport = Convert.ToString(dr["DateofReport"]),
                    DateOfIncident = Convert.ToString(dr["DateOfIncident"]),
                    TypeOfIncident = Convert.ToString(dr["TypeOfIncident"]),
                    NameOfInjuredPerson = Convert.ToString(dr["NameOfInjuredPerson"]),
                    IPHomeAddress = Convert.ToString(dr["IPHomeAddress"]),
                    LocationofIncident = Convert.ToString(dr["LocationofIncident"]),
                    TypeOfInjury = Convert.ToString(dr["TypeOfInjury"]),
                    MedicalTreatmentDetails = Convert.ToString(dr["MedicalTreatmentDetails"]),
                    DescriptionOfIncident = Convert.ToString(dr["DescriptionOfIncident"]),
                    RootCauseAnalysis = Convert.ToString(dr["RootCauseAnalysis"]),
                    Correction = Convert.ToString(dr["Correction"]),
                    CorrectiveAction = Convert.ToString(dr["CorrectiveAction"]),
                    MandaysLost = Convert.ToString(dr["MandaysLost"]),
                    SRiskAndOpportunities = Convert.ToString(dr["RiskAndOpportunities"]),
                    SAIHIRAReviewed = Convert.ToString(dr["AIHIRAReviewed"]),
                    SShareLessonsLearnt = Convert.ToString(dr["ShareLessonsLearnt"]),
                    Status = Convert.ToString(dr["Status"]),
                    FormFilledBy = Convert.ToString(dr["FormFilledBy"]),
                    CreationDate = Convert.ToString(dr["CreationDate"]),
                    Modifiedby = Convert.ToString(dr["Modifiedby"]),
                    ModificationDate = Convert.ToString(dr["ModificationDate"]),


                });
            }
            return View(lmd.ToList());

        }

        [HttpGet]
        public ActionResult ListCompentencyMetrixViewNMis()
        {
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.  
            DataSet ds = new DataSet();


            #region Get Scope Name
            DataTable dtGetNAME = new DataTable();

            //Get Scope (Name)
            dtGetNAME = DalMisReportQHSE.dtGetNAME1();
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


            ds = DalMisReportQHSE.GetDataN(); // fill dataset  



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
                    AdditionalQualification = Convert.ToString(dr["ProfCertificates"])
                });
            }
            return View(lmd.ToList());

        }

        //Exporttoexcel

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

                        if (dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "ROLENAME" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDBY" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "FORMFILLED" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDDATE" &&
                            //  dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "PK_USERID" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "MOBILENO" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "TUV_EMAIL_ID" &&
                            dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "EMPLOYEMENTCATEGORY" &&
                            //  dtgridN.Columns[column].ColumnName.ToString().ToUpper().Trim() != "TUVISTAMPNO" &&
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

                            if (dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "ROLENAME" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDBY" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "FORMFILLED" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MODIFIEDDATE" &&
                                //  dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "PK_USERID" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "MOBILENO" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "TUV_EMAIL_ID" &&
                                dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "EMPLOYEMENTCATEGORY" &&
                                //   dtgridN.Columns[colcnt].ColumnName.ToString().ToUpper().Trim() != "TUVISTAMPNO" &&
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

                return File(package.GetAsByteArray(), "application/unknown", "CompentencyMetrix " + DateTime.Now.ToShortDateString() + ".xlsx");
            }




        }

        public DataTable GetData()
        {

            DataSet dsNew = new DataSet();
            List<CompentencyMetrixView> lmd = new List<CompentencyMetrixView>();  // creating list of model.

            dsNew = DalMisReportQHSE.GetDataN(); // fill dataset

            return dsNew.Tables[0];
        }


        public DataSet GetMasterData()
        {

            DataSet dsMasterNew = new DataSet();


            dsMasterNew = DalMisReportQHSE.GetMasterDataN(); // fill dataset  



            return dsMasterNew;
        }

        //added by shrutika salve 23112023
        // GET: MisReport
        public ActionResult OrderBookingRegister()
        {
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";
            string FromDate = string.Empty;
            string ToDate = string.Empty;
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            if ((TempData["FromDate"] != null && TempData["FromDate"] != string.Empty) && (TempData["ToDate"] != null && TempData["ToDate"] != string.Empty))
            {
                TempData.Keep();
                FromDate = Convert.ToString(TempData["FromDate"]);
                ToDate = Convert.ToString(TempData["ToDate"]);

                objJM.FromDate = Convert.ToDateTime(FromDate);
                objJM.ToDate = Convert.ToDateTime(ToDate);

                DTSearchByDateWiseData = objJob.GetDataOrderBookingRegister(FromDate, ToDate);

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                               PoAmmendment = Convert.ToString(dr["PoAmmendment"]),
                               Consumed = Convert.ToString(dr["Consumed"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                               Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               QMAmount = Convert.ToString(dr["QMAmount"]),
                               EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                               POAmount = Convert.ToString(dr["POAmount"]),
                               Job_Number = Convert.ToString(dr["Job_Number"]),
                               Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Description = Convert.ToString(dr["Description"]),
                               Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               End_User = Convert.ToString(dr["End_User"]),
                               Service_type = Convert.ToString(dr["Service_type"]),
                               Job_type = Convert.ToString(dr["Job_type"]),
                               SAP_No = Convert.ToString(dr["SAP_No"]),
                               Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                               Special_Notes = Convert.ToString(dr["Special_Notes"]),
                               OrderStatus = Convert.ToString(dr["orderstatus"]),
                               DECName = Convert.ToString(dr["DECName"]),
                               DECNumber = Convert.ToString(dr["DECNumber"]),
                               JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                               ARC = Convert.ToString(dr["chkARC"]),
                               EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                               QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                               QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                               InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }
            else
            {

                lstEnquiryMast = objJob.GetJobDashBoardBookingorder();
            }



            ViewData["EnquiryMaster"] = lstEnquiryMast;
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }


        [HttpPost]
        public ActionResult OrderBookingRegister(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            //Session["FromDate"] = FromDate;
            //Session["ToDate"] = ToDate;
            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                TempData["FromDate"] = FromDate;
                TempData["ToDate"] = ToDate;
                TempData.Keep();
            }
            else
            {
                TempData.Clear();
            }

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objJob.GetDataOrderBookingRegister(FromDate, ToDate);

            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                {
                    lstEnquiryMast.Add(
                       new JobMasters
                       {
                           Count = DTSearchByDateWiseData.Rows.Count,
                           PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                           Consumed = Convert.ToString(dr["Consumed"]),
                           PoAmmendment = Convert.ToString(dr["PoAmmendment"]),
                           Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                           Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                           Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                           PONo = Convert.ToString(dr["PONo"]),
                           QMAmount = Convert.ToString(dr["QMAmount"]),
                           EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                           POAmount = Convert.ToString(dr["POAmount"]),
                           Job_Number = Convert.ToString(dr["Job_Number"]),
                           Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                           Po_Validity = Convert.ToString(dr["Po_Validity"]),
                           Description = Convert.ToString(dr["Description"]),
                           Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                           Client_Name = Convert.ToString(dr["Client_Name"]),
                           Branch = Convert.ToString(dr["Branch"]),
                           End_User = Convert.ToString(dr["End_User"]),
                           Service_type = Convert.ToString(dr["Service_type"]),
                           Job_type = Convert.ToString(dr["Job_type"]),
                           SAP_No = Convert.ToString(dr["SAP_No"]),
                           Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                           Special_Notes = Convert.ToString(dr["Special_Notes"]),
                           OrderStatus = Convert.ToString(dr["orderstatus"]),
                           DECName = Convert.ToString(dr["DECName"]),
                           DECNumber = Convert.ToString(dr["DECNumber"]),
                           JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                           ARC = Convert.ToString(dr["chkARC"]),
                           EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                           QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                           QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                           InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),

                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }
            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;
            return View(objJM);
        }

        [HttpGet]
        public ActionResult ExportIndexorderBookingRegister(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<JobMasters> grid = CreateExportableGridorderbookingRegister();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<JobMasters> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }


        private IGrid<JobMasters> CreateExportableGridorderbookingRegister()
        {

            IGrid<JobMasters> grid = new Grid<JobMasters>(GetDataorderbookingRegister());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };

            grid.Columns.Add(model => model.Job_Number).Titled("Job Number");
            grid.Columns.Add(model => model.PoAmmendment).Titled("PO Details");
            grid.Columns.Add(model => model.Consumed).Titled("Consumed Mandays");
            grid.Columns.Add(model => model.Estimate_ManDays_ManMonth).Titled("Estimated Man Days");
            grid.Columns.Add(model => model.Estimate_ManMonth).Titled("Estimated Manmonth");
            grid.Columns.Add(model => model.Estimate_ManHR).Titled("Estimated HR");
            grid.Columns.Add(model => model.PONo).Titled("PO Number");
            grid.Columns.Add(model => model.QMAmount).Titled("Quotation Amount");
            grid.Columns.Add(model => model.EQEstAmt).Titled("Enquiry Amount");
            grid.Columns.Add(model => model.POAmount).Titled("PO AMount");
            grid.Columns.Add(model => model.Job_Number).Titled("Job No");
            grid.Columns.Add(model => model.Customer_PoDate).Titled("Customer PO Date");
            grid.Columns.Add(model => model.Po_Validity).Titled("PO Validity");
            grid.Columns.Add(model => model.Description).Titled("Description");
            grid.Columns.Add(model => model.Quotation_Of_Order).Titled("Quotation Number");
            grid.Columns.Add(model => model.OrderType).Titled("Order Type");
            grid.Columns.Add(model => model.Enquiry_Of_Order).Titled("Enquiry Number");
            grid.Columns.Add(model => model.Client_Name).Titled("Client Name");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.End_User).Titled("End User");
            grid.Columns.Add(model => model.Service_type).Titled("Service Type");
            grid.Columns.Add(model => model.Job_type).Titled("Job Type");
            grid.Columns.Add(model => model.SAP_No).Titled("Sales Order No");
            grid.Columns.Add(model => model.Contract_reviewList).Titled("Contract Review List");
            grid.Columns.Add(model => model.Special_Notes).Titled("Special Notes");
            grid.Columns.Add(model => model.Estimate_ManMonth).Titled("Estimated Man month");
            grid.Columns.Add(model => model.OrderStatus).Titled("Order Status");
            grid.Columns.Add(model => model.DECName).Titled("DEC Name");
            grid.Columns.Add(model => model.DECNumber).Titled("DEC Number");
            grid.Columns.Add(model => model.JobCreatedBy).Titled("Job Created By");
            grid.Columns.Add(model => model.ARC).Titled("ARC");
            grid.Columns.Add(model => model.EnqRecDate).Titled("Enquiry Receipt Date");
            grid.Columns.Add(model => model.QMCreatedBy).Titled("Quotation Created By");
            grid.Columns.Add(model => model.QMCreatedDate).Titled("Quotation Created Date");
            grid.Columns.Add(model => model.InvoiceAmount).Titled("Invoicing Amount");

            grid.Pager = new GridPager<JobMasters>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objJM.lst1.Count;


            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<JobMasters> GetDataorderbookingRegister()
        {



            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            DataTable DTSearchByDateWiseData = new DataTable();


            if (TempData["FromDate"] == null && TempData["ToDate"] == null)
            {
                lstEnquiryMast = objJob.GetJobDashBoardBookingorder();
            }
            else
            {
                DTSearchByDateWiseData = objJob.GetDataOrderBookingRegister(Convert.ToString(TempData["FromDate"]), Convert.ToString(TempData["ToDate"]));

                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTSearchByDateWiseData.Rows)
                    {
                        lstEnquiryMast.Add(
                           new JobMasters
                           {
                               Count = DTSearchByDateWiseData.Rows.Count,
                               PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                               Consumed = Convert.ToString(dr["Consumed"]),
                               PoAmmendment = Convert.ToString(dr["PoAmmendment"]),
                               Estimate_ManDays_ManMonth = Convert.ToString(dr["Estimate_ManDays_ManMonth"]),
                               Estimate_ManMonth = Convert.ToString(dr["Estimate_ManMonth"]),
                               Estimate_ManHR = Convert.ToString(dr["Estimate_ManHR"]),
                               PONo = Convert.ToString(dr["PONo"]),
                               QMAmount = Convert.ToString(dr["QMAmount"]),
                               EQEstAmt = Convert.ToString(dr["EQEstAmt"]),
                               POAmount = Convert.ToString(dr["POAmount"]),
                               Job_Number = Convert.ToString(dr["Job_Number"]),
                               Customer_PoDate = Convert.ToString(dr["Customer_PoDate"]),
                               Po_Validity = Convert.ToString(dr["Po_Validity"]),
                               Description = Convert.ToString(dr["Description"]),
                               Quotation_Of_Order = Convert.ToString(dr["Quotation_Of_Order"]),
                               OrderType = Convert.ToString(dr["OrderType"]),
                               Enquiry_Of_Order = Convert.ToString(dr["Enquiry_Of_Order"]),
                               Client_Name = Convert.ToString(dr["Client_Name"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               End_User = Convert.ToString(dr["End_User"]),
                               Service_type = Convert.ToString(dr["Service_type"]),
                               Job_type = Convert.ToString(dr["Job_type"]),
                               SAP_No = Convert.ToString(dr["SAP_No"]),
                               Contract_reviewList = Convert.ToString(dr["Contract_reviewList"]),
                               Special_Notes = Convert.ToString(dr["Special_Notes"]),
                               OrderStatus = Convert.ToString(dr["orderstatus"]),
                               DECName = Convert.ToString(dr["DECName"]),
                               DECNumber = Convert.ToString(dr["DECNumber"]),
                               JobCreatedBy = Convert.ToString(dr["JobCreatedBy"]),
                               ARC = Convert.ToString(dr["chkARC"]),
                               EnqRecDate = Convert.ToString(dr["EnqRecDate"]),
                               QMCreatedBy = Convert.ToString(dr["QMCreatedBy"]),
                               QMCreatedDate = Convert.ToString(dr["QMCreatedDate"]),
                               InvoiceAmount = Convert.ToDecimal(dr["InvoiceAmount"]),
                           }
                         );
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }
            }




            ViewData["EnquiryMaster"] = lstEnquiryMast;
            TempData["Result"] = null;
            TempData.Keep();
            objJM.lst1 = lstEnquiryMast;

            return objJM.lst1;
        }

        //[HttpGet]

        //public ActionResult OBSOrderPOAMDValue()
        //{
        //    DataTable dtFinancialYear = new DataTable();
        //    DataTable dtMonthList = new DataTable();
        //    int TotalPrice = 0;
        //    if (!IsPostBack())   //// False if initial Load
        //    {
        //        TempData.Clear();
        //    }


        //    Session["GetExcelData"] = "Yes";

        //    string Fyear = string.Empty;
        //    string month = string.Empty;

        //    List<JobMasters> lstEnquiryMast = new List<JobMasters>();
        //    List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
        //    List<JobMasters> lstYear = new List<JobMasters>();
        //    List<JobMasters> lstMonthList = new List<JobMasters>();
        //    List<JobMasters> lstFinancialYear = new List<JobMasters>();


        //    DataTable DTSearchByDateWiseData = new DataTable();
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


        //    if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) || (TempData["month"] != null && TempData["month"] != string.Empty))
        //    {
        //        TempData.Keep();
        //        Fyear = Convert.ToString(TempData["FYear"]);
        //        month = Convert.ToString(TempData["month"]);


        //        DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


        //        if (DTSearchByDateWiseData.Rows.Count > 0)
        //        {
        //            DataRow TotalRow = DTSearchByDateWiseData.NewRow();
        //            for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
        //            {
        //                if (col == 0)
        //                {
        //                    TotalRow[col] = "Total";
        //                }
        //                else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
        //                {
        //                    for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
        //                    {
        //                        if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
        //                        {
        //                            TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
        //                        }
        //                    }
        //                    TotalRow[col] = TotalPrice;
        //                }
        //                else
        //                {
        //                    TotalRow[col] = 0;
        //                }
        //                TotalPrice = 0;
        //            }
        //            DTSearchByDateWiseData.Rows.Add(TotalRow);
        //        }
        //        else
        //        {
        //            TempData["Result"] = "No Record Found";
        //            TempData.Keep();
        //        }

        //        ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;

        //    }
        //    else
        //    {

        //        dtFinancialYear = objJob.GetFinancialYear();

        //        if (dtFinancialYear.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtFinancialYear.Rows)
        //            {

        //                lstFinancialYear.Add
        //                (
        //                    new JobMasters
        //                    {
        //                        FYear = Convert.ToString(dr["FYear"])
        //                    }
        //                );

        //            }
        //            ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
        //        }

        //        dtMonthList = objJob.GetMonthList();

        //        if (dtMonthList.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtMonthList.Rows)
        //            {
        //                lstMonthList.Add
        //                (
        //                    new JobMasters
        //                    {
        //                        strMonthName = Convert.ToString(dr["monthname"]),
        //                        monthID = Convert.ToInt32(dr["Number"])
        //                    }
        //                );

        //            }
        //            ViewBag.MonthList = new SelectList(lstMonthList, "monthID", "strMonthName");
        //        }

        //        if (Fyear == string.Empty)
        //        {
        //            int CurrentYear = DateTime.Today.Year;
        //            int PreviousYear = DateTime.Today.Year - 1;
        //            int NextYear = DateTime.Today.Year + 1;
        //            string PreYear = PreviousYear.ToString();
        //            string NexYear = NextYear.ToString();
        //            string CurYear = CurrentYear.ToString();


        //            if (DateTime.Today.Month > 3)
        //                Fyear = CurYear + "-" + NexYear;
        //            else
        //                Fyear = PreYear + "-" + CurYear;



        //        }

        //        DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);



        //        if (DTSearchByDateWiseData.Rows.Count > 0)
        //        {
        //            DataRow TotalRow = DTSearchByDateWiseData.NewRow();
        //            for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
        //            {
        //                if (col == 0)
        //                {
        //                    TotalRow[col] = "Total";
        //                }
        //                else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
        //                {
        //                    for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
        //                    {
        //                        if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
        //                        {
        //                            TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
        //                        }
        //                    }
        //                    TotalRow[col] = TotalPrice;
        //                }
        //                else
        //                {
        //                    TotalRow[col] = 0;
        //                }
        //                TotalPrice = 0;
        //            }
        //            DTSearchByDateWiseData.Rows.Add(TotalRow);
        //        }


        //    }
        //    ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;




        //    return View(objJM);
        //}




        //[HttpPost]
        //public ActionResult OBSOrderPOAMDValue(string FYear, string monthID)
        //{
        //    List<JobMasters> lstYear = new List<JobMasters>();
        //    List<JobMasters> lstMonthList = new List<JobMasters>();
        //    DataTable dtFinancialYear = new DataTable();
        //    DataTable dtMonthList = new DataTable();
        //    List<JobMasters> lstEnquiryMast = new List<JobMasters>();
        //    List<JobMasters> lstFinancialYear = new List<JobMasters>();
        //    List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
        //    int TotalPrice = 0;
        //    dtFinancialYear = objJob.GetFinancialYear();

        //    if (dtFinancialYear.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dtFinancialYear.Rows)
        //        {

        //            lstFinancialYear.Add
        //            (
        //                new JobMasters
        //                {
        //                    FYear = Convert.ToString(dr["FYear"])
        //                }
        //            );

        //        }
        //        ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
        //    }

        //    dtMonthList = objJob.GetMonthList();

        //    if (dtMonthList.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dtMonthList.Rows)
        //        {
        //            lstMonthList.Add
        //            (
        //                new JobMasters
        //                {
        //                    strMonthName = Convert.ToString(dr["monthname"]),
        //                    monthID = Convert.ToInt32(dr["Number"])
        //                }
        //            );

        //        }
        //        ViewBag.MonthList = new SelectList(lstMonthList, "monthID", "strMonthName");
        //    }
        //    Session["GetExcelData"] = null;

        //    if (FYear != string.Empty || monthID != string.Empty)
        //    {
        //        TempData["FYear"] = FYear;
        //        TempData["month"] = monthID;


        //        TempData.Keep();
        //    }
        //    else
        //    {
        //        TempData.Clear();

        //        if (FYear == string.Empty)
        //        {
        //            int CurrentYear = DateTime.Today.Year;
        //            int PreviousYear = DateTime.Today.Year - 1;
        //            int NextYear = DateTime.Today.Year + 1;
        //            string PreYear = PreviousYear.ToString();
        //            string NexYear = NextYear.ToString();
        //            string CurYear = CurrentYear.ToString();


        //            if (DateTime.Today.Month > 3)
        //                FYear = CurYear + "-" + NexYear;
        //            else
        //                FYear = PreYear + "-" + CurYear;

        //        }
        //    }




        //    DataTable DTSearchByDateWiseData = new DataTable();
        //    DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(FYear, monthID);


        //    if (DTSearchByDateWiseData.Rows.Count > 0)
        //    {
        //        DataRow TotalRow = DTSearchByDateWiseData.NewRow();
        //        for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
        //        {
        //            if (col == 0)
        //            {
        //                TotalRow[col] = "Total";
        //            }
        //            else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
        //            {
        //                for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
        //                {
        //                    if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
        //                    {
        //                        TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
        //                    }
        //                }
        //                TotalRow[col] = TotalPrice;
        //            }
        //            else
        //            {
        //                TotalRow[col] = 0;
        //            }
        //            TotalPrice = 0;
        //        }
        //        DTSearchByDateWiseData.Rows.Add(TotalRow);

        //    }
        //    else
        //    {
        //        TempData["Result"] = "No Record Found";
        //        TempData.Keep();
        //        objJM.lst1 = lstEnquiryMast;
        //        return View(objJM);
        //    }


        //    TempData["Result"] = null;
        //    TempData.Keep();

        //    ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;

        //    return View(objJM);
        //}

        //[HttpGet]
        //public ActionResult ExportObsOrderPOAMDValue(string Type)
        //{
        //    // Using EPPlus from nuget
        //    using (ExcelPackage package = new ExcelPackage())
        //    {
        //        Int32 row = 2;
        //        Int32 col = 1;
        //        int GrTotal = 0;
        //        int GRCol = 0;

        //        package.Workbook.Worksheets.Add("Data");
        //        DataTable grid = CreateExportableGridObsOrderPOAMDValue();

        //        ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

        //        foreach (DataColumn column in grid.Columns)
        //        {
        //            if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
        //            {
        //                sheet.Cells[1, col].Value = column.ColumnName.ToString();
        //                sheet.Column(col++).Width = 18;
        //            }
        //        }

        //        for (int gridRow = 0; gridRow < grid.Rows.Count - 1; gridRow++)
        //        {
        //            col = 1;
        //            foreach (DataColumn column in grid.Columns)
        //            {
        //                if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
        //                {
        //                    if (grid.Rows[gridRow][column].ToString() == string.Empty)
        //                    {
        //                        sheet.Cells[row, col].Value = "0";
        //                    }
        //                    else
        //                    {
        //                        sheet.Cells[row, col].Value = grid.Rows[gridRow][column].ToString();
        //                    }
        //                    col++;
        //                }
        //            }
        //            row++;
        //        }

        //        col = 1;

        //        foreach (DataColumn column in grid.Columns)
        //        {
        //            if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
        //            {
        //                if (col != 1)
        //                {
        //                    GrTotal = GrTotal + Convert.ToInt32(grid.Rows[grid.Rows.Count - 1][column].ToString());
        //                }

        //                if (grid.Rows[grid.Rows.Count - 1][column].ToString() == string.Empty)
        //                {
        //                    sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
        //                    sheet.Cells[row, col].Value = "0";
        //                }
        //                else
        //                {
        //                    sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                    sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
        //                    sheet.Cells[row, col].Value = grid.Rows[grid.Rows.Count - 1][column].ToString();
        //                }
        //                col++;
        //            }
        //        }
        //        row++;
        //        GRCol = 1;
        //        sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
        //        sheet.Cells[row, GRCol].Value = "Grand Total";

        //        GRCol++;
        //        sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
        //        sheet.Cells[row, GRCol].Value = GrTotal.ToString();





        //        return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
        //    }
        //}



        //private DataTable CreateExportableGridObsOrderPOAMDValue()
        //{

        //    DataTable grid = GetDataObsOrderPOAMDValue();


        //    return grid;
        //}

        //public DataTable GetDataObsOrderPOAMDValue()
        //{
        //    DataTable dtFinancialYear = new DataTable();
        //    DataTable dtMonthList = new DataTable();
        //    List<JobMasters> lstEnquiryMast = new List<JobMasters>();
        //    List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
        //    List<JobMasters> lstYear = new List<JobMasters>();
        //    List<JobMasters> lstMonthList = new List<JobMasters>();
        //    List<JobMasters> lstFinancialYear = new List<JobMasters>();


        //    DataTable DTSearchByDateWiseData = new DataTable();
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        //    //if (!IsPostBack())   //// False if initial Load
        //    //{
        //    //    TempData.Clear();
        //    //}


        //    Session["GetExcelData"] = "Yes";

        //    string Fyear = string.Empty;
        //    string month = string.Empty;
        //    int TotalPrice = 0;




        //    if ((TempData["FYear"] != null && TempData["FYear"] != string.Empty) || (TempData["month"] != null && TempData["month"] != string.Empty))
        //    {
        //        TempData.Keep();
        //        Fyear = Convert.ToString(TempData["FYear"]);
        //        month = Convert.ToString(TempData["month"]);


        //        DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


        //        if (DTSearchByDateWiseData.Rows.Count > 0)
        //        {
        //            DataRow TotalRow = DTSearchByDateWiseData.NewRow();
        //            for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
        //            {
        //                if (col == 0)
        //                {
        //                    TotalRow[col] = "Total";
        //                }
        //                else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
        //                {
        //                    for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
        //                    {
        //                        if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
        //                        {
        //                            TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
        //                        }
        //                    }
        //                    TotalRow[col] = TotalPrice;
        //                }
        //                else
        //                {
        //                    TotalRow[col] = 0;
        //                }
        //                TotalPrice = 0;
        //            }
        //            DTSearchByDateWiseData.Rows.Add(TotalRow);

        //        }
        //        else
        //        {
        //            TempData["Result"] = "No Record Found";
        //            TempData.Keep();
        //        }

        //    }
        //    else
        //    {

        //        dtFinancialYear = objJob.GetFinancialYear();

        //        if (dtFinancialYear.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtFinancialYear.Rows)
        //            {

        //                lstFinancialYear.Add
        //                (
        //                    new JobMasters
        //                    {
        //                        FYear = Convert.ToString(dr["FYear"])
        //                    }
        //                );

        //            }
        //            ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
        //        }

        //        dtMonthList = objJob.GetMonthList();

        //        if (dtMonthList.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtMonthList.Rows)
        //            {
        //                lstMonthList.Add
        //                (
        //                    new JobMasters
        //                    {
        //                        strMonthName = Convert.ToString(dr["monthname"]),
        //                        monthID = Convert.ToInt32(dr["Number"])
        //                    }
        //                );

        //            }
        //            ViewBag.MonthList = new SelectList(lstMonthList, "monthID", "strMonthName");
        //        }

        //        if (Fyear == string.Empty)
        //        {
        //            int CurrentYear = DateTime.Today.Year;
        //            int PreviousYear = DateTime.Today.Year - 1;
        //            int NextYear = DateTime.Today.Year + 1;
        //            string PreYear = PreviousYear.ToString();
        //            string NexYear = NextYear.ToString();
        //            string CurYear = CurrentYear.ToString();


        //            if (DateTime.Today.Month > 3)
        //                Fyear = CurYear + "-" + NexYear;
        //            else
        //                Fyear = PreYear + "-" + CurYear;



        //        }

        //        DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


        //        if (DTSearchByDateWiseData.Rows.Count > 0)
        //        {

        //            DataRow TotalRow = DTSearchByDateWiseData.NewRow();
        //            for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
        //            {
        //                if (col == 0)
        //                {
        //                    TotalRow[col] = "Total";
        //                }
        //                else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
        //                {
        //                    for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
        //                    {
        //                        if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
        //                        {
        //                            TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
        //                        }
        //                    }
        //                    TotalRow[col] = TotalPrice;
        //                }
        //                else
        //                {
        //                    TotalRow[col] = 0;
        //                }
        //                TotalPrice = 0;
        //            }
        //            DTSearchByDateWiseData.Rows.Add(TotalRow);
        //        }
        //    }
        //    ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;


        //    return DTSearchByDateWiseData;
        //}

        [HttpGet]
        public ActionResult OBSOrderPOAMDValue()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            int TotalPrice = 0;
            int TotalColPrice = 0;
            if (!IsPostBack())   //// False if initial Load
            {
                TempData.Clear();
            }


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;

            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


            if ((TempData["YearOBS"] != null && TempData["YearOBS"] != string.Empty) || (TempData["monthOBS"] != null && TempData["monthOBS"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["YearOBS"]);
                month = Convert.ToString(TempData["monthOBS"]);


                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    DataColumn TotalCol = DTSearchByDateWiseData.Columns.Add();

                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);



                    for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                    {
                        for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                        {
                            if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                            {
                                if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                                {
                                    TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                                }
                            }
                        }
                        DTSearchByDateWiseData.Columns.Add(TotalCol);
                        TotalColPrice = 0;
                    }
                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

                ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;

            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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

                if (Fyear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        Fyear = CurYear + "-" + NexYear;
                    else
                        Fyear = PreYear + "-" + CurYear;



                }

                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);



                if (DTSearchByDateWiseData.Rows.Count > 0)
                {

                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    DataColumn TotalCol = new DataColumn();

                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                    DataColumn newCol = new DataColumn("Total", typeof(string));
                    newCol.AllowDBNull = true;
                    DTSearchByDateWiseData.Columns.Add(newCol);
                    for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                    {

                        for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                        {
                            if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                            {
                                if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                                {
                                    TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                                }
                            }

                        }
                        DTSearchByDateWiseData.Rows[row1]["Total"] = TotalColPrice;


                        TotalColPrice = 0;
                    }
                }


            }
            ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;




            return View(objJM);
        }




        [HttpPost]
        public ActionResult OBSOrderPOAMDValue(string FYear, string monthID)
        {
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            int TotalPrice = 0;
            int TotalColPrice = 0;
            dtFinancialYear = objJob.GetFinancialYear();

            if (dtFinancialYear.Rows.Count > 0)
            {
                foreach (DataRow dr in dtFinancialYear.Rows)
                {

                    lstFinancialYear.Add
                    (
                        new JobMasters
                        {
                            FYear = Convert.ToString(dr["FYear"])
                        }
                    );

                }
                ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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
            Session["GetExcelData"] = null;

            if (FYear != string.Empty || monthID != string.Empty)
            {
                TempData["YearOBS"] = FYear;
                TempData["monthOBS"] = monthID;


                TempData.Keep();
            }
            else
            {
                TempData.Clear();

                if (FYear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        FYear = CurYear + "-" + NexYear;
                    else
                        FYear = PreYear + "-" + CurYear;

                }
            }




            DataTable DTSearchByDateWiseData = new DataTable();
            DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(FYear, monthID);


            if (DTSearchByDateWiseData.Rows.Count > 0)
            {
                DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                {
                    if (col == 0)
                    {
                        TotalRow[col] = "Total";
                    }
                    else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                    {
                        for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                        {
                            if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                            {
                                TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                            }
                        }
                        TotalRow[col] = TotalPrice;
                    }
                    else
                    {
                        TotalRow[col] = 0;
                    }
                    TotalPrice = 0;
                }
                DTSearchByDateWiseData.Rows.Add(TotalRow);

                DataColumn newCol = new DataColumn("Total", typeof(string));
                newCol.AllowDBNull = true;
                DTSearchByDateWiseData.Columns.Add(newCol);
                for (int row1 = 0; row1 < DTSearchByDateWiseData.Rows.Count - 1; row1++)
                {

                    for (int col2 = 0; col2 < DTSearchByDateWiseData.Columns.Count; col2++)
                    {
                        if (DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && DTSearchByDateWiseData.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                        {
                            if (DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName] != string.Empty)
                            {
                                TotalColPrice = TotalColPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[row1][DTSearchByDateWiseData.Columns[col2].ColumnName]);
                            }
                        }

                    }
                    DTSearchByDateWiseData.Rows[row1]["Total"] = TotalColPrice;


                    TotalColPrice = 0;
                }

            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objJM.lst1 = lstEnquiryMast;
                return View(objJM);
            }


            TempData["Result"] = null;
            TempData.Keep();

            ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;

            return View(objJM);
        }








        [HttpGet]
        public ActionResult ExportObsOrderPOAMDValue(string Type)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;
                int GrTotal = 0;
                int GRCol = 0;
                int GrColTotal = 0;
                System.Globalization.CultureInfo hindi = new System.Globalization.CultureInfo("hi-IN");

                package.Workbook.Worksheets.Add("Data");
                DataTable grid = CreateExportableGridObsOrderPOAMDValue();

                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        sheet.Cells[1, col].Value = column.ColumnName.ToString();
                        sheet.Column(col++).Width = 18;
                    }
                }

                for (int gridRow = 0; gridRow < grid.Rows.Count - 1; gridRow++)
                {
                    col = 1;
                    foreach (DataColumn column in grid.Columns)
                    {
                        if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                        {
                            if (grid.Rows[gridRow][column].ToString() == string.Empty)
                            {
                                sheet.Cells[row, col].Value = "0";
                            }
                            else
                            {
                                sheet.Cells[row, col].Value = grid.Rows[gridRow][column].ToString();
                            }
                            col++;
                        }
                    }
                    row++;
                }

                col = 1;

                foreach (DataColumn column in grid.Columns)
                {
                    if (column.ColumnName.ToString().ToUpper() != "OBSID" && column.ColumnName.ToString().ToUpper() != "SPID")
                    {
                        if (col != 1)
                        {
                            GrTotal = GrTotal + Convert.ToInt32(grid.Rows[grid.Rows.Count - 1][column].ToString());
                        }

                        if (grid.Rows[grid.Rows.Count - 1][column].ToString() == string.Empty)
                        {
                            sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                            sheet.Cells[row, col].Value = "0";
                        }
                        else
                        {
                            if (col != 1)
                            {
                                int parsedAmt = int.Parse(grid.Rows[grid.Rows.Count - 1][column].ToString(), System.Globalization.CultureInfo.InvariantCulture);
                                string TotalAmt = string.Format(hindi, "{0:c0}", parsedAmt);
                                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                                sheet.Cells[row, col].Value = TotalAmt.ToString();
                            }
                            else
                            {

                                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue);
                                sheet.Cells[row, col].Value = grid.Rows[grid.Rows.Count - 1][column].ToString();
                            }
                        }
                        col++;
                    }
                }
                // row++;
                GRCol = col++;
                //sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                //sheet.Cells[row, GRCol].Value = "Grand Total";

                //GRCol++;
                //sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                //sheet.Cells[row, GRCol].Value = GrTotal.ToString();

                row = 1;

                int TotalColPrice = 0;

                sheet.Cells[row, 1, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, 1, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.BurlyWood);
                sheet.Cells[row, GRCol].Value = "Total";

                row++;
                for (int row1 = 0; row1 < grid.Rows.Count - 1; row1++)
                {
                    col = 1;
                    for (int col2 = 0; col2 < grid.Columns.Count; col2++)
                    {
                        if (grid.Columns[col2].ColumnName.ToString().ToUpper() != "OBSID" && grid.Columns[col2].ColumnName.ToString().ToUpper() != "SPID" && grid.Columns[col2].ColumnName.ToString().ToUpper() != "PORTFOLIONAME")
                        {
                            if (grid.Rows[row1][grid.Columns[col2].ColumnName] != DBNull.Value && grid.Rows[row1][grid.Columns[col2].ColumnName] != string.Empty)
                            {
                                TotalColPrice = TotalColPrice + Convert.ToInt32(grid.Rows[row1][grid.Columns[col2].ColumnName]);
                            }
                            col++;
                        }

                    }
                    //grid.Columns.Add(TotalCol);
                    col++;


                    sheet.Cells[row, col].Value = TotalColPrice.ToString();
                    GrColTotal = GrColTotal + TotalColPrice;
                    TotalColPrice = 0;

                    row++;
                }
                // row++;
                decimal parsed = decimal.Parse(GrColTotal.ToString(), System.Globalization.CultureInfo.InvariantCulture);

                string GrandAmttext = string.Format(hindi, "{0:c0}", parsed);

                sheet.Cells[row, col, row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                sheet.Cells[row, col, row, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Aquamarine);
                sheet.Cells[row, col].Value = GrandAmttext.ToString();


                DateTime currentDateTime = DateTime.Now;

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "OBSwiseOrderBookingValue-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private DataTable CreateExportableGridObsOrderPOAMDValue()
        {

            DataTable grid = GetDataObsOrderPOAMDValue();


            return grid;
        }

        public DataTable GetDataObsOrderPOAMDValue()
        {
            DataTable dtFinancialYear = new DataTable();
            DataTable dtMonthList = new DataTable();
            List<JobMasters> lstEnquiryMast = new List<JobMasters>();
            List<JobMasters> lstEnquiryMastSum = new List<JobMasters>();
            List<JobMasters> lstYear = new List<JobMasters>();
            List<JobMasters> lstMonthList = new List<JobMasters>();
            List<JobMasters> lstFinancialYear = new List<JobMasters>();


            DataTable DTSearchByDateWiseData = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            //if (!IsPostBack())   //// False if initial Load
            //{
            //    TempData.Clear();
            //}


            Session["GetExcelData"] = "Yes";

            string Fyear = string.Empty;
            string month = string.Empty;
            int TotalPrice = 0;




            if ((TempData["YearOBS"] != null && TempData["YearOBS"] != string.Empty) || (TempData["monthOBS"] != null && TempData["monthOBS"] != string.Empty))
            {
                TempData.Keep();
                Fyear = Convert.ToString(TempData["YearOBS"]);
                month = Convert.ToString(TempData["monthOBS"]);


                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {
                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);

                }
                else
                {
                    TempData["Result"] = "No Record Found";
                    TempData.Keep();
                }

            }
            else
            {

                dtFinancialYear = objJob.GetFinancialYear();

                if (dtFinancialYear.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtFinancialYear.Rows)
                    {

                        lstFinancialYear.Add
                        (
                            new JobMasters
                            {
                                FYear = Convert.ToString(dr["FYear"])
                            }
                        );

                    }
                    ViewBag.FinancialYear = new SelectList(lstFinancialYear, "fYear", "fYear");
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

                if (Fyear == string.Empty)
                {
                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();


                    if (DateTime.Today.Month > 3)
                        Fyear = CurYear + "-" + NexYear;
                    else
                        Fyear = PreYear + "-" + CurYear;



                }

                DTSearchByDateWiseData = objDALMisReport.GetBranchOrderValuepoAmd(Fyear, month);


                if (DTSearchByDateWiseData.Rows.Count > 0)
                {

                    DataRow TotalRow = DTSearchByDateWiseData.NewRow();
                    for (int col = 0; col < DTSearchByDateWiseData.Columns.Count; col++)
                    {
                        if (col == 0)
                        {
                            TotalRow[col] = "Total";
                        }
                        else if (DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "OBSID" && DTSearchByDateWiseData.Columns[col].ColumnName.ToString().ToUpper() != "SPID")
                        {
                            for (int rowno = 0; rowno < DTSearchByDateWiseData.Rows.Count; rowno++)
                            {
                                if (DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != DBNull.Value && DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName] != string.Empty)
                                {
                                    TotalPrice = TotalPrice + Convert.ToInt32(DTSearchByDateWiseData.Rows[rowno][DTSearchByDateWiseData.Columns[col].ColumnName]);
                                }
                            }
                            TotalRow[col] = TotalPrice;
                        }
                        else
                        {
                            TotalRow[col] = 0;
                        }
                        TotalPrice = 0;
                    }
                    DTSearchByDateWiseData.Rows.Add(TotalRow);
                }
            }
            ViewData["OBSOrderPOAMDValue"] = DTSearchByDateWiseData;


            return DTSearchByDateWiseData;
        }
        //added by nikita on 08012024

        public ActionResult LostQuotationReport()
        {
            Session["GetExcelData"] = "Yes";
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            lstQuotationMast = objDALMisReport.QuotaionMastertDashBoard_();
            ViewData["QuotationMaster"] = lstQuotationMast;
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
            //return View();
        }
        [HttpPost]
        public ActionResult LostQuotationReport(string FromDate, string ToDate)
        {
            Session["GetExcelData"] = null;
            Session["FromDate"] = FromDate;
            Session["ToDate"] = ToDate;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            DateTime FromDt = DateTime.ParseExact(FromDate, "dd/MM/yyyy", theCultureInfo);
            DateTime ToDt = DateTime.ParseExact(ToDate, "dd/MM/yyyy", theCultureInfo);
            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();
            DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDate_(FromDate, ToDate);
            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           ILostReason = Convert.ToString(dr["ILostReason"]),
                           DLostReason = Convert.ToString(dr["DLostReason"]),
                           //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                           IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),//added by nikita 29082023
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),//added by nikita 29082023
                           Type = Convert.ToString(dr["OBSType"])//added by nikita 29082023
                       }
                    );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return View(objQM);
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return View(objQM);
        }

        [HttpGet]
        public ActionResult ExportIndexLostQuotationMaster()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<QuotationMaster> grid = CreateExportableGridQuotationMaster_();
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

                string formattedDateTime = currentDateTime.ToString("dd-MM-yyyy HH:mm:ss");

                string filename = "LostQuotationReport-" + formattedDateTime + ".xlsx";

                return File(package.GetAsByteArray(), "application/unknown", filename);
            }
        }
        private IGrid<QuotationMaster> CreateExportableGridQuotationMaster_()
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<QuotationMaster> grid = new Grid<QuotationMaster>(GetDataQuotationMaster_());
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.QuotationNumber).Titled("Quotation Number");
            grid.Columns.Add(model => model.CreatedDate).Titled("Quotation Date");
            grid.Columns.Add(model => model.OrderType).Titled("Order Type");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Validity Date");
            grid.Columns.Add(model => model.EstimatedAmount).Titled("Estimate Amount");
            grid.Columns.Add(model => model.Quotation_Description).Titled("Project Name");
            grid.Columns.Add(model => model.Name).Titled("Service Type");
            grid.Columns.Add(model => model.PortfolioType).Titled("Portfolio Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.Type).Titled("OBS Type");//added by nikita 29-08-2023
            grid.Columns.Add(model => model.CompanyAddress).Titled("Client Name");
            grid.Columns.Add(model => model.BranchName).Titled("Originating Branch");
            grid.Columns.Add(model => model.ExpiryDate).Titled("Est Close Date");
            grid.Columns.Add(model => model.StatusType).Titled("Status");
            grid.Columns.Add(model => model.CreatedBy).Titled("Created By");
            grid.Columns.Add(model => model.DLostReason).Titled("Lost Reason (Domestic)");
            grid.Columns.Add(model => model.ILostReason).Titled("Lost Reason (International)");

            grid.Pager = new GridPager<QuotationMaster>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objQM.lstQuotationMasterDashBoard1.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<QuotationMaster> GetDataQuotationMaster_()
        {

            List<QuotationMaster> lstQuotationMast = new List<QuotationMaster>();
            DataTable DTSearchRecordByDate = new DataTable();

            if (Session["GetExcelData"] == "Yes")
            {
                lstQuotationMast = objDALMisReport.QuotaionMastertDashBoard_();
            }
            else
            {

                //U.FromDate = Session["FromDate"].ToString();
                //U.ToDate = Session["ToDate"].ToString();
                DTSearchRecordByDate = objDALMisReport.GetQMSearchRecordByDate_(Convert.ToString(Session["FromDate"]), Convert.ToString(Session["ToDate"]));
            }



            if (DTSearchRecordByDate.Rows.Count > 0)
            {
                foreach (DataRow dr in DTSearchRecordByDate.Rows)
                {
                    lstQuotationMast.Add(
                       new QuotationMaster
                       {
                           Count = DTSearchRecordByDate.Rows.Count,
                           PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                           QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                           EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                           Quotation_Description = Convert.ToString(dr["Description"]),
                           EndCustomer = Convert.ToString(dr["EndCustomer"]),
                           CompanyAddress = Convert.ToString(dr["CompanyName"]),
                           Enquiry = Convert.ToString(dr["Enquiry"]),
                           Reference = Convert.ToString(dr["Reference"]),
                           BranchName = Convert.ToString(dr["Branch_Name"]),
                           CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                           EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
                           OrderType = Convert.ToString(dr["OrderType"]),
                           ILostReason = Convert.ToString(dr["ILostReason"]),
                           DLostReason = Convert.ToString(dr["DLostReason"]),
                           //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                           DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                           IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                           StatusType = Convert.ToString(dr["Status"]),
                           QTType = Convert.ToString(dr["QuotationType"]),
                           Name = Convert.ToString(dr["ServiceType"]),//added by nikita 29082023
                           CreatedBy = Convert.ToString(dr["OwnerName"]),
                           JobNo = Convert.ToString(dr["JobNo"]),
                           PortfolioType = Convert.ToString(dr["PortfolioType"]),//added by nikita 29082023
                           Type = Convert.ToString(dr["OBSType"])//added by nikita 29082023
                       }
                     );
                }
            }
            else
            {
                TempData["Result"] = "No Record Found";
                TempData.Keep();
                objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
                return objQM.lstQuotationMasterDashBoard1;
                //return View();
            }
            ViewData["QuotationMaster"] = lstQuotationMast;
            TempData["Result"] = null;
            TempData.Keep();
            objQM.lstQuotationMasterDashBoard1 = lstQuotationMast;
            return objQM.lstQuotationMasterDashBoard1;
        }


        public ActionResult PlayLibVideo(string ID)
        {
            TrainingScheduleModel trg = new TrainingScheduleModel();

            DataSet DTSearchByDateWiseData = new DataSet();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            trg.PK_TrainingScheduleId = Convert.ToInt32(ID);

            DTSearchByDateWiseData = objDALMisReport.GetLibDetails(ID);



            if (DTSearchByDateWiseData.Tables.Count > 0)
            {
                if (DTSearchByDateWiseData.Tables[0].Rows.Count > 0)
                {
                    trg.TrainingName = DTSearchByDateWiseData.Tables[0].Rows[0]["TrainingTopic"].ToString();
                    trg.FileName = DTSearchByDateWiseData.Tables[0].Rows[0]["FileName"].ToString();
                }
            }
            else
            {
                trg.TrainingName = string.Empty;
                trg.FileName = string.Empty;
            }


            return View(trg);
        }
        //end of nikita code

        [HttpPost]
        public JsonResult UpdateReadLibVideo(int ID)
        {
            
            try
            {


                DataSet ds = new DataSet();
                TempData.Keep();
                ds = objDALMisReport.UpdateVideoLeantData(Convert.ToInt32(ID));


                if (ds.Tables.Count > 0)
                {                    
                    return Json(new { Result = "OK" });
                }



            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(new { Result = "OK" });
        }
        public ActionResult WorkRegister()
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
                objCM.PO_Number = Convert.ToString(Session["PO_Number"]);
                objCM.po_Date = Convert.ToString(Session["po_Date"]);
            }

            DataTable Invoice = new DataTable();
            if (Session["FromDate1"] != null && Session["ToDate1"] != null || Session["Job1"] != null)
            {

                objCM.FromDateI = Convert.ToString(Session["FromDate1"].ToString());
                objCM.ToDateI = Convert.ToString(Session["ToDate1"].ToString());
                //objCM.Job = Convert.ToString(Session["Job1"]?.ToString()) ?? "";
                ////obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                //objCM.Inspector = Convert.ToString(Session["inpector"]?.ToString()) ?? "";
                DTComplaintDashBoard = Dalobj.InvoiceData_international(objCM);

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
                                OriginatingBranch = Convert.ToString(dr["Originating_Branch"]),
                                InspectionLocation_ = Convert.ToString(dr["InspectionLocation"]),
                                SubSubVendorName = Convert.ToString(dr["subsubvendor_"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),


                            });
                    }
                    ViewData["List"] = lstComplaintDashBoard;
                    objCM.lstComplaintDashBoard1 = lstComplaintDashBoard;
                    //Session["FromDate1"] = null;
                    //Session["ToDate1"] = null;
                    //Session["Job1"] = null;
                    //Session["inpector"] = null;
                }

                else
                {
                    ViewData["List"] = Session["lstComplaintDashBoard"];
                    objCM.lstComplaintDashBoard1 = lstComplaintDashBoard;

                    return View(objCM);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }


            return View(objCM);
        }
        [HttpPost]
        public ActionResult WorkRegister(CallsModel CM)
        {
            List<CallsModel> lmd = new List<CallsModel>();  // creating list of model.  
            DataSet ds = new DataSet();
            Session["FromDate1"] = CM.FromDateI;
            Session["ToDate1"] = CM.ToDateI;
            Session["Job1"] = CM.Job;
            Session["inpector"] = CM.Inspector;



            return RedirectToAction("WorkRegister");
            //return View();
        }

        public ActionResult ExportIndex2(CallsModel U)
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
                                    ExcelRange Rng = sheet.Cells[row, 21];

                                    Rng.StyleName = StyleName;
                                    string[] FileDetails = word.Split('|');

                                    // Split the value to get the filename and the part before the first underscore
                                    string filename = FileDetails[0].Trim();
                                    int underscoreIndex = filename.IndexOf('_');
                                    string fileId = filename.Substring(0, underscoreIndex);


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
                //Session["FromDate1"] = null;
                //Session["ToDate1"] = null;
                //Session["Job1"] = null;
                //Session["inpector"] = null;
                return File(package.GetAsByteArray(), "application/unknown", "WorkRegister_" + DateTime.Now.ToShortDateString() + ".xlsx");

            }
        }

        private IGrid<CallsModel> CreateExportableGrid1(CallsModel U)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<CallsModel> grid = new Grid<CallsModel>(GetData(U));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };
            grid.Columns.Add(model => model.Call_No).Titled("Call No");
            grid.Columns.Add(model => model.Actual_Visit_Date).Titled("Actual Visit Date");
            grid.Columns.Add(model => model.Job).Titled("Job Number");
            //grid.Columns.Add(model => model.Sub_Job).Titled("Sub/Sub job Number");
            grid.Columns.Add(model => model.SAP_Number).Titled("Sap Number");
            grid.Columns.Add(model => model.Company_Name).Titled("Customer Name");
            grid.Columns.Add(model => model.Sub_Job).Titled("Sub Number");
            grid.Columns.Add(model => model.VendorName).Titled("sub Vendor Name");
            grid.Columns.Add(model => model.TopSubVendorName).Titled("sub Vendor Name");
            grid.Columns.Add(model => model.TopSubVendorName).Titled("sub sub Vendor Name");
            //grid.Columns.Add(model => model.VendorPONo).Titled("sub Vendor PO Number");
            //grid.Columns.Add(model => model.TopSubVendorPONo).Titled("sub/sub Vendor PO Number");
            grid.Columns.Add(model => model.Inspector).Titled("Inspector Name");
            //grid.Columns.Add(model => model.SAPEmpCode).Titled("SAP Employee Code");
            //grid.Columns.Add(model => model.TUVEmpCode).Titled("TUV EMployee Code");
            grid.Columns.Add(model => model.ExecutingBranch).Titled("Executing Branch");
            //grid.Columns.Add(model => model.MandayRate).Titled("Manday Rate in INR");
            grid.Columns.Add(model => model.Onsite).Titled("Onsite Time");
            grid.Columns.Add(model => model.Offsite).Titled("Offsite Time");
            grid.Columns.Add(model => model.TravelTime).Titled("Travel Time");
            grid.Columns.Add(model => model.InspectionLocation_).Titled("Inspection Location");
            grid.Columns.Add(model => model.CustomerRepresentative).Titled("Call raised by");
            grid.Columns.Add(model => model.Originating_Branch).Titled("Originating Branch");
            //grid.Columns.Add(model => model.Status).Titled("call status");
            //grid.Columns.Add(model => model.PresentDay).Titled("Full/Half Day");
            grid.Columns.Add(model => model.IVR).Titled("Visit Report");
            grid.Columns.Add(model => model.insopectionRecord).Titled("inspection Record");




            grid.Pager = new GridPager<CallsModel>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = objCM.lstComplaintDashBoard1.Count;

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

                objCM.FromDateI = Convert.ToString(Session["FromDate1"].ToString());
                objCM.ToDateI = Convert.ToString(Session["ToDate1"].ToString());
                //objCM.Job = Convert.ToString(Session["Job1"]?.ToString()) ?? "";
                ////obj1.Inspector = Convert.ToString(Session["inpector"].ToString());
                //objCM.Inspector = Convert.ToString(Session["inpector"]?.ToString()) ?? "";

                DTCallDashBoard = Dalobj.InvoiceData_international(objCM);
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
                                SubSubVendorName = Convert.ToString(dr["subsubvendor_"]),

                                
                                TopSubVendorPONo = Convert.ToString(dr["subsubVendor_Po_No"]),
                                MandayRate = Convert.ToString(dr["MandayRate"]),
                                CustomerRepresentative = Convert.ToString(dr["CustomerRepresentativeName"]),
                                SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                                TUVEmpCode = Convert.ToString(dr["TUVEmpCode"]),
                                Originating_Branch = Convert.ToString(dr["Originating_Branch"]),
                                InspectionLocation_ = Convert.ToString(dr["InspectionLocation"]),
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

            objCM.lstComplaintDashBoard1 = lstCallDashBoard;


            return objCM.lstComplaintDashBoard1;
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
                    objCM.PO_Number = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_PoNo_PoDate"]);

                    objCM.po_Date = Convert.ToString(DSJobMasterByQtId.Tables[0].Rows[0]["Customer_PoDate"]);

                }
                Session["PO_Number"] = objCM.PO_Number;
                Session["po_Date"] = objCM.po_Date;
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }

            return Json(objCM, JsonRequestBehavior.AllowGet);
        }

    }
}