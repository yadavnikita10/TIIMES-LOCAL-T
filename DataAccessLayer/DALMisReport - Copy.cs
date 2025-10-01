using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TuvVision.Models;
using System.Web.Mvc;

namespace TuvVision.DataAccessLayer
{

    
    public class DALMisReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable GenerateInvoiceData(string FromDate, string ToDate)// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTQMDashBoard = new DataTable();
            List<JobMasters> lstQuotationMastDashB = new List<JobMasters>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 12);
                CMDQMDashBoard.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDQMDashBoard.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return DTQMDashBoard;
        }

        public List<QuotationMaster> QuotaionMastertDashBoard()// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTQMDashBoard = new DataTable();
            List<QuotationMaster> lstQuotationMastDashB = new List<QuotationMaster>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_MisReportMaster", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 9);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
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
                               BranchName=Convert.ToString(dr["Branch_Name"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                               EstimatedAmount=Convert.ToString(dr["EstimatedAmount"]),
                             //  ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                               DApprovalStatus = Convert.ToString(dr["DCostSheetApproveStatus"]),
                               IApprovalStatus = Convert.ToString(dr["ICostSheetApproveStatus"]),
                               StatusType = Convert.ToString(dr["Status"]),
                               QTType = Convert.ToString(dr["QuotationType"]),                               
                               Name = Convert.ToString(dr["Name"]),
                               CreatedBy=Convert.ToString(dr["OwnerName"]),
                               JobNo=Convert.ToString(dr["JobNo"])                               
                           }
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }
        //Record Search By From Date and To Date Wise, Code by Manoj Sharma 17 Dec 2019
        public DataTable GetQMSearchRecordByDate(string FromDate, string ToDate)
        {
            DataTable DTQMSearchByDate = new DataTable();
            try
            {
               /* string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];*/

                SqlCommand CMDQMSearchByDate = new SqlCommand("SP_MisReportMaster", con);
                CMDQMSearchByDate.CommandType = CommandType.StoredProcedure;
                CMDQMSearchByDate.CommandTimeout = 1000000;
                CMDQMSearchByDate.Parameters.AddWithValue("@SP_Type", 10);
                CMDQMSearchByDate.Parameters.AddWithValue("@DateFrom", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDQMSearchByDate.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDQMSearchByDate.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMSearchByDate);
                SDAGetEnquiry.Fill(DTQMSearchByDate);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMSearchByDate.Dispose();
            }
            return DTQMSearchByDate;
        }


        public List<NonInspectionActivity> timesheetReportDashBoard()// Binding QuotationMAster DashBoard of Master Page 
        {
            int abc = 0;
            string totaltime = null;
            DataTable DTQMDashBoard = new DataTable();
            List<NonInspectionActivity> lstQuotationMastDashB = new List<NonInspectionActivity>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_MisReportMaster", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 1);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
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

                        lstQuotationMastDashB.Add(
                           new NonInspectionActivity
                           {
                               Count = DTQMDashBoard.Rows.Count,
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
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }
        //Get Record By From Date And To Date Code By Manoj Sharma 17 Dec 2019

        public DataTable GetTMSearchRecordByDate(string FromDate, string ToDate)
        {
            DataTable DTTMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDQMDashBoard = new SqlCommand("SP_MisReportMaster", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 3);
                CMDQMDashBoard.Parameters.AddWithValue("@DateFrom", _date1);
                CMDQMDashBoard.Parameters.AddWithValue("@DateTo", _date2);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDASearchByDate = new SqlDataAdapter(CMDQMDashBoard);
                SDASearchByDate.Fill(DTTMDashBoard);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTTMDashBoard.Dispose();
            }
            return DTTMDashBoard;
        }
        public List<CallsModel> PendingIVRMIS()// Binding QuotationMAster DashBoard of Master Page 
        {

            DataTable DTQMDashBoard = new DataTable();
            List<CallsModel> lstQuotationMastDashB = new List<CallsModel>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 2);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new CallsModel
                           {
                               Count = DTQMDashBoard.Rows.Count,
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
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }
        //Searching Reocrd by Date wise From Date and To Date code by Manoj Sharma 17 Dec 2019
        public DataTable GetSearchRecordByDateWisePendingIVRMIS(string FromDate, string ToDate)
        {
            DataTable DTQMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 5);
                CMDQMDashBoard.Parameters.AddWithValue("@DateFrom", _date1);
                CMDQMDashBoard.Parameters.AddWithValue("@DateTo", _date2);
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return DTQMDashBoard;
        }
        public DataTable DealyIVRMIS()// Binding QuotationMAster DashBoard of Master Page 
        {

            DataTable DTQMDashBoard = new DataTable();
            List<CallsModel> lstQuotationMastDashB = new List<CallsModel>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 3);
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);

            /*    if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new CallsModel
                           {
                               Count = DTQMDashBoard.Rows.Count,
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
                               CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                               ReportDate = Convert.ToDateTime(dr["CreatedDate"]),
                               //ActualReportDealydate = Convert.ToDateTime(dr["Actual_Visit_Date"]).AddDays(2),

                           }
                         );
                    }
                }*/
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return DTQMDashBoard;
        }

        //Searching Record by From Date and To Date wise code by Manoj Sharma 17 Dec 2019
        public DataTable DealyIVRMISSearchByDate(string FromDate, string ToDate)
        {
            DataTable DTDelayIVRSearchByDate = new DataTable();

            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 6);
                //CMDQMDashBoard.Parameters.AddWithValue("@FromDate", FromDate);
                //CMDQMDashBoard.Parameters.AddWithValue("@ToDate", ToDate);

                CMDQMDashBoard.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDQMDashBoard.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADelayIVRSearchByDate = new SqlDataAdapter(CMDQMDashBoard);
                SDADelayIVRSearchByDate.Fill(DTDelayIVRSearchByDate);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDelayIVRSearchByDate.Dispose();
            }
            return DTDelayIVRSearchByDate;
        }

        public List<CallsModel> DealyIRNMIS()// Binding QuotationMAster DashBoard of Master Page 
        {

            DataTable DTQMDashBoard = new DataTable();
            List<CallsModel> lstQuotationMastDashB = new List<CallsModel>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 8);
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new CallsModel
                           {
                               Count = DTQMDashBoard.Rows.Count,
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
                               //Branch_Name = Convert.ToString(dr["Branch_Name"]),
                               Executing_Branch = Convert.ToString(dr["Executing_Branch"]),
                               ProductList = Convert.ToString(dr["Inspected_Items"]),
                               Status = Convert.ToString(dr["Status"]),
                               //Contact_Name = Convert.ToString(dr["Contact_Name"]),
                               Actual_Visit_Date = Convert.ToString(dr["Actual_Visit_Date"]),
                               Job_Location = Convert.ToString(dr["InspectionLocation"]),
                               Inspector = Convert.ToString(dr["Visiting_InspectorName"]),
                               //Report_No = Convert.ToString(dr["Report_No"]),
                               CreatedDate = Convert.ToString(dr["IRNSubmissionDate"]),
                              // ReportDate = Convert.ToDateTime(dr["CreatedDate"]),
                               DelayInINRSubmission=Convert.ToInt32(dr["DelayInIRNSubmissionDay"]),

                           }
                         );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }
        public DataTable DealyIRNMISSearchByDate(string FromDate, string ToDate)
        {
            DataTable DTDelayIVRSearchByDate = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 9);
                CMDQMDashBoard.Parameters.AddWithValue("@DateFrom", _date1);
                CMDQMDashBoard.Parameters.AddWithValue("@DateTo", _date2);
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADelayIVRSearchByDate = new SqlDataAdapter(CMDQMDashBoard);
                SDADelayIVRSearchByDate.Fill(DTDelayIVRSearchByDate);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDelayIVRSearchByDate.Dispose();
            }
            return DTDelayIVRSearchByDate;
        }

        public List<JobMasters> InvoiceinstructionMIS()// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTQMDashBoard = new DataTable();
            List<JobMasters> lstQuotationMastDashB = new List<JobMasters>();
            try
            {
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_IVRMIS", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 4);
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new JobMasters
                           {
                               Count = DTQMDashBoard.Rows.Count,
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
                               Currency = Convert.ToString(dr["Currency"]),
                               Customer_PoNo_PoDate = Convert.ToString(dr["Customer_PoNo_PoDate"]),
                               PK_JOB_ID = Convert.ToInt32(dr["PK_JOB_ID"]),
                               CreatedDate = Convert.ToString(dr["CreatedDate"]),
                               JobDate = Convert.ToString(dr["CreatedDate"]),
                               pendingAmount = Convert.ToDecimal(dr["Customer_PO_Amount"]) - Convert.ToDecimal(dr["InvoiceAmount"]),
                           }
                         );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }
        //Searching Record By From Date And To Date code By Manoj Sharma 17 Dec 2019
        public DataTable InvoiceinstructionMISSearchByDate(string FromDate, string ToDate)
        {
            DataTable DTIIMSearchByDate = new DataTable();
            try
            {
                //string _date1 = string.Empty;
                //string _date2 = string.Empty;
                //string[] FromDt = FromDate.Split('/');
                //string[] ToDt = ToDate.Split('/');
                //_date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                //_date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDIIMSearchByDate = new SqlCommand("SP_IVRMIS", con);
                CMDIIMSearchByDate.CommandType = CommandType.StoredProcedure;
                CMDIIMSearchByDate.CommandTimeout = 1000000;
                CMDIIMSearchByDate.Parameters.AddWithValue("@SP_Type", 7);
                CMDIIMSearchByDate.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDIIMSearchByDate.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                // CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAIIMSearchByDate = new SqlDataAdapter(CMDIIMSearchByDate);
                SDAIIMSearchByDate.Fill(DTIIMSearchByDate);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTIIMSearchByDate.Dispose();
            }
            return DTIIMSearchByDate;
        }
        //Employee Atta

        public List<EmployeeAttendance> EmployeeAttendanceMIS()// Binding Employee Attendance MIS Report 
        {

            DataTable DTEmployeeAttendanceMIS = new DataTable();
            List<EmployeeAttendance> lstEmployeeAttendanceMIS = new List<EmployeeAttendance>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");
                
                cmd.Parameters.AddWithValue("@User", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DTEmployeeAttendanceMIS);            
            
            
                //SqlCommand CMDEmployeeAttendanceMIS = new SqlCommand("SP_IVRMIS", con);
                //CMDEmployeeAttendanceMIS.CommandType = CommandType.StoredProcedure;
                //CMDEmployeeAttendanceMIS.CommandTimeout = 1000000;
                //CMDEmployeeAttendanceMIS.Parameters.AddWithValue("@SP_Type", 10);
                //CMDEmployeeAttendanceMIS.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                //SqlDataAdapter SDAEmployeeAttendanceMIS = new SqlDataAdapter(CMDEmployeeAttendanceMIS);
                //SDAEmployeeAttendanceMIS.Fill(DTEmployeeAttendanceMIS);

                if (DTEmployeeAttendanceMIS.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceMIS.Rows)
                    {
                        lstEmployeeAttendanceMIS.Add(
                           new EmployeeAttendance
                           {
                               //Count = DTEmployeeAttendanceMIS.Rows.Count,
                               //PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               //BranchName = Convert.ToString(dr["BranchName"]),
                               //NameOfEmployee = Convert.ToString(dr["EmployeeName"]),
                               //EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                               //EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                               //DateOfActivity = Convert.ToString(dr["DateOfActivity"]),
                               //ActivityType = Convert.ToString(dr["ActivityType"]),
                               //SubJobNumber = Convert.ToString(dr["SubJobNumber"]),
                               //Description = Convert.ToString(dr["Description"]),
                               ///ActivityHours = Convert.ToInt32(dr["ActivityHours"]),
                               
                           //    Date = Convert.ToString(dr["Date"]),
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
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceMIS.Dispose();
            }
            return lstEmployeeAttendanceMIS;
        }
        public DataTable EmployeeAttendanceSearchByDate(string FromDate, string ToDate)
        {
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            try
            {
                
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");                
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@User", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DTEmployeeAttendanceSearchByDate);

                //SqlCommand CMDEmployeeAttendanceSearchByDate = new SqlCommand("SP_IVRMIS", con);
                //CMDEmployeeAttendanceSearchByDate.CommandType = CommandType.StoredProcedure;
                //CMDEmployeeAttendanceSearchByDate.CommandTimeout = 1000000;
                //CMDEmployeeAttendanceSearchByDate.Parameters.AddWithValue("@SP_Type", 11);
                //CMDEmployeeAttendanceSearchByDate.Parameters.AddWithValue("@DateFrom", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //CMDEmployeeAttendanceSearchByDate.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //CMDEmployeeAttendanceSearchByDate.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                //SqlDataAdapter SDAEmployeeAttendanceSearchByDate = new SqlDataAdapter(CMDEmployeeAttendanceSearchByDate);
                //SDAEmployeeAttendanceSearchByDate.Fill(DTEmployeeAttendanceSearchByDate);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceSearchByDate.Dispose();
            }
            return DTEmployeeAttendanceSearchByDate;
        }


        public DataTable InspectorDataByDate(string FromDate, string ToDate)
        {
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@DateFrom", FromDate);
                cmd.Parameters.AddWithValue("@DateTo", ToDate);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(DTEmployeeAttendanceSearchByDate);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceSearchByDate.Dispose();
            }
            return DTEmployeeAttendanceSearchByDate;
        }

        public List<InspectorData> InspectorData()// Binding Employee Attendance MIS Report 
        {

            DataTable DTEmployeeAttendanceMIS = new DataTable();
            List<InspectorData> lstEmployeeAttendanceMIS = new List<InspectorData>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "13");
                // cmd.Parameters.AddWithValue("@FromDate", string.Empty);
                // cmd.Parameters.AddWithValue("@ToDate", string.Empty);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DTEmployeeAttendanceMIS);


                if (DTEmployeeAttendanceMIS.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceMIS.Rows)
                    {
                        lstEmployeeAttendanceMIS.Add(
                           new InspectorData
                           {
                               User = Convert.ToString(dr["User"]),
                               TUv_Email_ID = Convert.ToString(dr["TUv_Email_ID"]),
                               MobileNo = Convert.ToString(dr["MobileNo"]),
                               Branch = Convert.ToString(dr["Branch"]),
                               EmpType = Convert.ToString(dr["EmpType"]),
                               CallsAssigned = Convert.ToString(dr["CallsAssigned"]),
                               ClosedCalls = Convert.ToString(dr["ClosedCalls"]),
                               VisitReports = Convert.ToString(dr["VisitReports"]),
                           }
                         );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceMIS.Dispose();
            }
            return lstEmployeeAttendanceMIS;
        }

        public DataTable CallAnalysisByDate(string FromDate, string ToDate)
        {
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "14");
                cmd.Parameters.AddWithValue("@DateFrom", FromDate);
                cmd.Parameters.AddWithValue("@DateTo", ToDate);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(DTEmployeeAttendanceSearchByDate);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceSearchByDate.Dispose();
            }
            return DTEmployeeAttendanceSearchByDate;
        }

        public List<CallAnalysis> CallAnalysis()// Binding Employee Attendance MIS Report 
        {

            DataTable DTEmployeeAttendanceMIS = new DataTable();
            List<CallAnalysis> lstEmployeeAttendanceMIS = new List<CallAnalysis>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "15");

                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DTEmployeeAttendanceMIS);


                if (DTEmployeeAttendanceMIS.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEmployeeAttendanceMIS.Rows)
                    {
                        lstEmployeeAttendanceMIS.Add(
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
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceMIS.Dispose();
            }
            return lstEmployeeAttendanceMIS;
        }


        public SelectList GetMonths(int? iSelectedYear)
        {

            List<SelectListItem> ddlMonths = new List<SelectListItem>();

            int CurrentMonth = 1; //January  
            int CountMonth = 12; //January  
            try
            {
                var months = Enumerable.Range(1, 12).Select(i => new
                {
                    A = i,
                    B = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                });

                if (iSelectedYear == DateTime.Now.Year)
                {
                    CurrentMonth = DateTime.Now.Month;

                    months = Enumerable.Range(1, CountMonth).Select(i => new
                    {
                        A = i,
                        B = DateTimeFormatInfo.CurrentInfo.GetMonthName(i)
                    });
                }

                foreach (var item in months)
                {
                    ddlMonths.Add(new SelectListItem { Text = item.B.ToString(), Value = item.A.ToString() });
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {

            }
            return new SelectList(ddlMonths, "Value", "Text", CurrentMonth);

        }
        //DropDown : GetYears() will fill Year DropDown and Return List.  
        public SelectList GetYears(int? iSelectedYear)
        {
            int CurrentYear = DateTime.Now.Year;
            int yearstart = CurrentYear - 5;
            List<SelectListItem> ddlYears = new List<SelectListItem>();

            try
            {
                for (int i = yearstart; i <= CurrentYear; i++)
                {
                    ddlYears.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {

            }
            return new SelectList(ddlYears, "Value", "Text", iSelectedYear);

        }

        public DataTable GetBranchOrderValue(string StrYear, string strMonth)
        {

            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@FinYear", StrYear);
                cmd.Parameters.AddWithValue("@Month", strMonth);

                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(DTEmployeeAttendanceSearchByDate);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmployeeAttendanceSearchByDate.Dispose();
            }
            return DTEmployeeAttendanceSearchByDate;

        }

        public DataSet GetUserDataSheet()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }


        public DataSet GetLessionLerntHist()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet ViewLessonLearntUserHstory()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataSet ViewLessonLearntUserDetailHstory(int? EQ_ID)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataTable ViewLessonLearntUserDetailHstoryExcel(int? EQ_ID)
        {
            DataTable ds = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }

        public DataTable GetFileContent(int? EQ_ID, string TableType)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_MisReportMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;

                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditUploadedFile.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@Attachment", TableType);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        public DataSet UpdateLessionLeantData(int? EQ_ID)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "8");
                cmd.Parameters.AddWithValue("@EQ_ID", EQ_ID);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
    }
}