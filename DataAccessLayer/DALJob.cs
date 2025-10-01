using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;

namespace TuvVision.DataAccessLayer
{
    public class DALJob
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection1"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

      // SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection1"].ConnectionString);

        #region  JOB Master
        public DataTable GetJOBList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }




        public string InsertUpdateJOb(JobMasters CPM, string IPath, string IPath1)
        {
            int ReturnId = 0;
            int ReturnId1 = 0;
            string Result = string.Empty;
            string Result1 = string.Empty;
            var roleid = Convert.ToString(System.Web.HttpContext.Current.Session["RoleID"]);
            con.Open();
            con1.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.PK_JOB_ID == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CMP_ID", CPM.FK_CMP_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Job_Number", CPM.Job_Number);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Quotation_Of_Order", CPM.Quotation_Of_Order);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Enquiry_Of_Order", CPM.Enquiry_Of_Order);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Branch", CPM.Branch);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@End_User", CPM.End_User);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@subserviceType", CPM.subserviceType);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PortfolioType", CPM.PortfolioType);


                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Job_type", CPM.Job_type);
                    //                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    // CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Contract_reviewList", CPM.Contract_reviewList);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);

                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);


                    CMDInsertUpdateJOB.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    if (CPM.Customer_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    {

                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    }


                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Balance", CPM.Balance);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@InvoiceAmount", CPM.InvoiceAmount);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PortID", CPM.PortID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Currency", CPM.Currency);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    //added by shrutika salve 08012023
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@IfConcernsDisplayOfPDF", CPM.checkIFConcernDisplay);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ExchangeRate", CPM.DExchangeRate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.DTotalAmount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderTypeId", CPM.OrderTypeId);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@JobReviewCheckListDescription", CPM.CheckListDescription);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@JobReviewCheckList", CPM.CommaSeparatedCheckListId);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@TermsCondition", CPM.TermsCondition);  //added by nikita on 18062024
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@legalReview", CPM.LegalReview);//added by nikita on 18062024
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Comments", CPM.Legalcomment);//added by nikita on 18062024
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@JobExecuted", CPM.JobCreated);//added by nikita on 18062024
                    //added by shrutika salve 22052024
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ClientContact", CPM.Client_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ChkIfShareReportVendor", CPM.chkDoNotshareVendor); //added by nikita on 21012025

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAPItem_No", CPM.SAPItem_No);
                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (CPM.checkIFCustomerSpecificReportNo == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFCustomerSpecificReportNo", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFCustomerSpecificReportNo", "0");
                    }
                    if (CPM.ItemDescriptionDynamic == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ItemDescriptionDynamic", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ItemDescriptionDynamic", "0");
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ReportType", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ReportType", "0");
                    }
                    if (CPM.checkIFInOutTimeOnPDF == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFInOutTimeOnPDF", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFInOutTimeOnPDF", "0");
                    }
                    if (CPM.chkIfTravelExpenseOnPDF == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@chkIfTravelExpenseOnPDF", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@chkIfTravelExpenseOnPDF", "0");
                    }
                    if (CPM.chkIfEndUserEditable == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@chkIfEndUserEditable", "1");
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@chkIfEndUserEditable", "0");
                    }
                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@JobBlock", CPM.JObBlock);

                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);

                    //data insert to 39 server
                    SqlCommand InsertUpdateJOB = new SqlCommand("SP_InsertDataJobSubJobAndCompany", con1);
                    InsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    InsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    InsertUpdateJOB.Parameters.AddWithValue("@CMP_ID", CPM.FK_CMP_ID);
                    InsertUpdateJOB.Parameters.AddWithValue("@Job_Number", CPM.Job_Number);
                    InsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Description);
                    InsertUpdateJOB.Parameters.AddWithValue("@Quotation_Of_Order", CPM.Quotation_Of_Order);
                    InsertUpdateJOB.Parameters.AddWithValue("@Enquiry_Of_Order", CPM.Enquiry_Of_Order);
                    InsertUpdateJOB.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    InsertUpdateJOB.Parameters.AddWithValue("@Branch", CPM.Branch);
                    InsertUpdateJOB.Parameters.AddWithValue("@End_User", CPM.End_User);

                    InsertUpdateJOB.Parameters.AddWithValue("@subserviceType", CPM.subserviceType);
                    InsertUpdateJOB.Parameters.AddWithValue("@PortfolioType", CPM.PortfolioType);


                    InsertUpdateJOB.Parameters.AddWithValue("@Job_type", CPM.Job_type);
                    //                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    // CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);
                    InsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);

                    InsertUpdateJOB.Parameters.AddWithValue("@Contract_reviewList", CPM.Contract_reviewList);
                    InsertUpdateJOB.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);

                    if (IPath != null && IPath != "")
                    {
                        InsertUpdateJOB.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        InsertUpdateJOB.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }

                    InsertUpdateJOB.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);


                    InsertUpdateJOB.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    InsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    InsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    if (CPM.Customer_PoDate != null)
                    {
                        InsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    InsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    InsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    {

                        InsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        InsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    }


                    InsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    InsertUpdateJOB.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    InsertUpdateJOB.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    InsertUpdateJOB.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    InsertUpdateJOB.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    InsertUpdateJOB.Parameters.AddWithValue("@Balance", CPM.Balance);
                    InsertUpdateJOB.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    InsertUpdateJOB.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    InsertUpdateJOB.Parameters.AddWithValue("@InvoiceAmount", CPM.InvoiceAmount);

                    InsertUpdateJOB.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    InsertUpdateJOB.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    InsertUpdateJOB.Parameters.AddWithValue("@PortID", CPM.PortID);
                    InsertUpdateJOB.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    InsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    InsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    InsertUpdateJOB.Parameters.AddWithValue("@Currency", CPM.Currency);
                    InsertUpdateJOB.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
                    InsertUpdateJOB.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    InsertUpdateJOB.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    InsertUpdateJOB.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);
                    InsertUpdateJOB.Parameters.AddWithValue("@ExchangeRate", CPM.DExchangeRate);
                    InsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.DTotalAmount);
                    InsertUpdateJOB.Parameters.AddWithValue("@OrderTypeId", CPM.OrderTypeId);
                    InsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    InsertUpdateJOB.Parameters.AddWithValue("@JobReviewCheckListDescription", CPM.CheckListDescription);
                    InsertUpdateJOB.Parameters.AddWithValue("@JobReviewCheckList", CPM.CommaSeparatedCheckListId);
                    InsertUpdateJOB.Parameters.AddWithValue("@SAPItem_No", CPM.SAPItem_No);


                    InsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;


                    Result1 = InsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId1 = Convert.ToInt32(InsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result1 = Convert.ToString(ReturnId1);
                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@End_User", CPM.End_User);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    if (CPM.Customer_PoDate != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "dd/MM/yyyy", theCultureInfo));

                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Balance", CPM.Balance);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    // CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", CPM.Customer_PoDate);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ClientContact", CPM.Client_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);


                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PortID", CPM.PortID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Currency", CPM.Currency);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    //added by shrutika salve 08012023
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IfConcernsDisplayOfPDF", CPM.checkIFConcernDisplay);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@JobReviewCheckListDescription", CPM.CheckListDescriptionN);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@JobReviewCheckList", CPM.CommaSeparatedCheckListId);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SAPItem_No", CPM.SAPItem_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@TermsCondition", CPM.TermsCondition);//added by nikita on 18062024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@legalReview", CPM.LegalReview);//added by nikita on 18062024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Comments", CPM.Legalcomment);//added by nikita on 18062024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@JobExecuted", CPM.JobCreated);//added by nikita on 18062024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ChkIfShareReportVendor", CPM.chkDoNotshareVendor); //added by nikita on 21012025

                    if (roleid == "60") //added by nikita on 18062024
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@LegalApprovedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    }
                    if (CPM.checkIFCustomerSpecificReportNo == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFCustomerSpecificReportNo", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFCustomerSpecificReportNo", "0");
                    }
                    if (CPM.ItemDescriptionDynamic == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ItemDescriptionDynamic", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ItemDescriptionDynamic", "0");
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ReportType", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ReportType", "0");
                    }
                    if (CPM.checkIFInOutTimeOnPDF == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFInOutTimeOnPDF", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFInOutTimeOnPDF", "0");
                    }
                    if (CPM.chkIfTravelExpenseOnPDF == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@chkIfTravelExpenseOnPDF", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@chkIfTravelExpenseOnPDF", "0");
                    }
                    if (CPM.chkIfEndUserEditable == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@chkIfEndUserEditable", "1");
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@chkIfEndUserEditable", "0");
                    }
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@JobBlock", CPM.JObBlock);
                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();

                    //data update to 39 server

                    SqlCommand InsertUpdateBranch = new SqlCommand("SP_InsertDataJobSubJobAndCompany", con1);
                    InsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    InsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    InsertUpdateBranch.Parameters.AddWithValue("@Description", CPM.Description);
                    InsertUpdateBranch.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    InsertUpdateBranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    InsertUpdateBranch.Parameters.AddWithValue("@End_User", CPM.End_User);
                    InsertUpdateBranch.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    if (CPM.Customer_PoDate != null)
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    InsertUpdateBranch.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    InsertUpdateBranch.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "dd/MM/yyyy", theCultureInfo));

                    }

                    InsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);
                    InsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    InsertUpdateBranch.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    InsertUpdateBranch.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);

                    InsertUpdateBranch.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    InsertUpdateBranch.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    InsertUpdateBranch.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    InsertUpdateBranch.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    InsertUpdateBranch.Parameters.AddWithValue("@Balance", CPM.Balance);
                    InsertUpdateBranch.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    InsertUpdateBranch.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    // CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", CPM.Customer_PoDate);


                    if (IPath != null && IPath != "")
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }
                    InsertUpdateBranch.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    InsertUpdateBranch.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    InsertUpdateBranch.Parameters.AddWithValue("@PortID", CPM.PortID);
                    InsertUpdateBranch.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    InsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    InsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    InsertUpdateBranch.Parameters.AddWithValue("@Currency", CPM.Currency);
                    InsertUpdateBranch.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
                    InsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    InsertUpdateBranch.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    InsertUpdateBranch.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    InsertUpdateBranch.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);
                    InsertUpdateBranch.Parameters.AddWithValue("@JobReviewCheckListDescription", CPM.CheckListDescriptionN);
                    InsertUpdateBranch.Parameters.AddWithValue("@JobReviewCheckList", CPM.CommaSeparatedCheckListId);
                    InsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    InsertUpdateBranch.Parameters.AddWithValue("@SAPItem_No", CPM.SAPItem_No);

                    Result1 = InsertUpdateBranch.ExecuteNonQuery().ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
                if (con1.State != ConnectionState.Closed)
                {
                    con1.Close();
                }
            }
            return Result;
        }



        public DataTable GetConsumeCount(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 11);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;

        }

        public DataSet EditJob(int? PK_JOB_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataTable EditListOfsubJob(int? Br_Id)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        #region
        public DataSet GetQutationDetails(int? PK_QT_ID)//Getting Data of Qutation Details
        {
            DataSet DTGetEnquiryDtls = new DataSet();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_QT_ID ", PK_QT_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }
        #endregion


        #endregion

        

        public DataSet CheckQutationdata(int? PK_QT_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@PK_QT_ID", PK_QT_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        //#region  Manoj Code
        //public DataTable GetEnquiryDetals(int? EQ_ID)//Getting Data of Enquiry Details
        //{
        //    DataTable DTGetEnquiryDtls = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_QuotationMaster", con);
        //        CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 3);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //        SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
        //        SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTGetEnquiryDtls.Dispose();
        //    }
        //    return DTGetEnquiryDtls;

        //}


        //#endregion


        public int DeleteJob(int? PK_JOB_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_JobMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 7);
                CMDContactDelete.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDContactDelete.ExecuteNonQuery();
                if (Result != 0)
                {
                    return Result;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

            }
            return Result;
        }


        #region Job No Set

        public DataTable JobNoSet(JobMasters CPM)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditContact.Parameters.AddWithValue("@Branch", CPM.Branch);
                CMDEditContact.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataSet GetBranchid(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Branch);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataSet GetBranchCode(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "6N");
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Branch);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataSet GetServicesTypeID(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Service_type);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        #endregion


        public DataTable JobNoUniqueId()
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 9);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        //**********************Manoj Added code for Uploading File in database on 13 March 2020 FOR ATTACHMENT
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_JOBID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    item.FileName = Convert.ToString(JOB_ID) + '_' + item.FileName;
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent);
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListJobMasterUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        public DataTable EditUploadedFile(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", PK_JOB_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        public DataTable GetFileExt(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetFileExtenstion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetFileExtenstion.Dispose();
            }
            return DTGetFileExtenstion;
        }

        //****************************************************************Ending Code Related File Uploaded*******************

        //**********************Manoj Added code for Uploading File in database on 12 March 2020 FOR FORMAT OF REPORT FILE
        public string InsertFileFormat(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_JOBID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                foreach (var item in lstFileUploaded)
                {
                    item.FileName = Convert.ToString(JOB_ID) + '_' + item.FileName;
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListJobMastUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        public DataTable EditUploadedFileFormat(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", BR_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFileFormat(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }

        public DataTable GetFileExtension(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetFileExtenstion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetFileExtenstion.Dispose();
            }
            return DTGetFileExtenstion;
        }


        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public DataTable GetFileContent1(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public DataTable DownloadSubJobAttachment(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "4N");
                CMDEditUploadedFile.Parameters.AddWithValue("@FileID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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
        //****************************************************************Ending Code Related File Uploaded*******************


        public DataTable GetCallSummary(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 13);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataTable GetAddMandays(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 16);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }



        public string InsertUpdateAddMandays(JobMasters CPM)
        {
            int ReturnId = 0;
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.PK_ADDPOID == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 14);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Add_PoNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.Add_POAmt);
                    if (CPM.Add_PoValidity != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Add_PoValidity, "dd/MM/yyyy", theCultureInfo));
                    }
                    if (CPM.Add_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Add_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }


                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Add_Mandays);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Reason", CPM.Add_PoReason);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Add_MandayDesc);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.Add_SAPNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OldSAP_No", CPM.SAP_No);

                    if (CPM.Add_SAPDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@SAPDate", DateTime.ParseExact(CPM.Add_SAPDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);
                }
                else
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 15);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_ADDPOID", CPM.PK_ADDPOID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Add_PoNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.Add_POAmt);
                    if (CPM.Add_PoValidity != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Add_PoValidity, "dd/MM/yyyy", theCultureInfo));
                    }

                    if (CPM.Add_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Add_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Add_Mandays);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Reason", CPM.Add_PoReason);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.Add_SAPNumber);
                    if (CPM.Add_SAPDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@SAPDate", DateTime.ParseExact(CPM.Add_SAPDate, "dd/MM/yyyy", theCultureInfo));
                    }
                    // CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Add_MandayDesc);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();

                }



            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }

        public DataTable CheckAddPOFile(int? PK_JOB_ID, int? pk_AddPO_Id)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", PK_JOB_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@pk_ADDPOID", pk_AddPO_Id);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public string InsertAddPOFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID, int AddPOID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_JOBID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent);
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 6);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListJobMasterUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@pk_ADDPOID", AddPOID);

                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }


        public DataTable GetDataByDateWise(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 18);
                CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", _date1);
                CMDGetEnquriy.Parameters.AddWithValue("@DateTo", _date2);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public List<JobMasters> GetJobHistoryDashBoard()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 19);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new JobMasters
                           {
                               Count = DTEMDashBoard.Rows.Count,
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
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }


        public DataTable GetDataHistoryByDateWise(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                string _date1 = string.Empty;
                string _date2 = string.Empty;
                string[] FromDt = FromDate.Split('/');
                string[] ToDt = ToDate.Split('/');
                _date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                _date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 20);
                CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", _date1);
                CMDGetEnquriy.Parameters.AddWithValue("@DateTo", _date2);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public DataSet GetProbableInvoicing()// Binding Sales Masters DashBoard of Master Page 
        {
            DataSet DTEMDashBoard = new DataSet();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 23);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);

                SDAGetEnquiry.Fill(DTEMDashBoard);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }


        public DataSet GetDatewiseProbableInvoicing(string FYear, string sMonth)// Binding Sales Masters DashBoard of Master Page 
        {
            DataSet DTEMDashBoard = new DataSet();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 24);
                CMDGetEnquriy.Parameters.AddWithValue("@FYear", FYear);
                CMDGetEnquriy.Parameters.AddWithValue("@FYMonth", sMonth);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);

                SDAGetEnquiry.Fill(DTEMDashBoard);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public DataTable GetFinancialYear()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 21);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new JobMasters
                           {
                               Count = DTEMDashBoard.Rows.Count,
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
                //  DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public DataTable GetMonthList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 22);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                //  DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public List<JobMasters> GetJobDashBoard()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 17);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new JobMasters
                           {
                               Count = DTEMDashBoard.Rows.Count,
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
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataTable GetRevisionDetails(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 25);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);

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

        public DataTable GetJobCheckList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "27");
                // CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }

        //Added BY Satish Pawar On 15 May 2023
        //public string SaveFeedbackEmailHistory(string JobNo, string Email_IDs,string Feedback_Link,string CompanyName, string pkuserid)
        //{
            
        //    string Result = "";
        //    string Result1 = "";
        //    con.Open();
        //    con1.Open();
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        //    try
        //    {

        //        string[] emailArray = Email_IDs.Split(';');
        //        string[] Pk_userid = pkuserid.Split(';');



        //        for (int i = 0; i < emailArray.Length; i++)
        //        {
        //            string email = emailArray[i];
        //            string pkUserId = Pk_userid[i];

        //            string UpdatedFeedbackLink = Feedback_Link + "&id=" + pkUserId;

        //            SqlCommand CMD_SaveFeedbackEmailHistory = new SqlCommand("SP_SaveFeedbackEmailHistory", con);
        //            CMD_SaveFeedbackEmailHistory.CommandType = CommandType.StoredProcedure;
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@SP_Type", "1");
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@JobNo", JobNo);
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@EmailID", email.Trim()); // Trim to remove leading/trailing spaces
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@Feedback_Link", UpdatedFeedbackLink);
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CouterValue", pkUserId.Trim());
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CompanyName", CompanyName);

        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //            CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@Result", SqlDbType.VarChar).Direction = ParameterDirection.Output;

        //            CMD_SaveFeedbackEmailHistory.ExecuteNonQuery();
        //            Result = Convert.ToString(CMD_SaveFeedbackEmailHistory.Parameters["@Result"].Value.ToString());

        //            SqlCommand CMD = new SqlCommand("SP_SaveFeedbackEmailHistory", con1);
        //            CMD.CommandType = CommandType.StoredProcedure;
        //            CMD.Parameters.AddWithValue("@SP_Type", "1");
        //            CMD.Parameters.AddWithValue("@JobNo", JobNo);
        //            CMD.Parameters.AddWithValue("@EmailID", email.Trim()); // Trim to remove leading/trailing spaces
        //            CMD.Parameters.AddWithValue("@Feedback_Link", UpdatedFeedbackLink);
        //            CMD.Parameters.AddWithValue("@CouterValue", pkUserId.Trim());
        //            CMD.Parameters.AddWithValue("@CompanyName", CompanyName);
        //            CMD.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //            CMD.Parameters.AddWithValue("@Result", SqlDbType.VarChar).Direction = ParameterDirection.Output;

        //            CMD.ExecuteNonQuery();
        //            Result1 = Convert.ToString(CMD.Parameters["@Result"].Value.ToString());


        //        }




        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //        if (con1.State != ConnectionState.Closed)
        //        {
        //            con1.Close();
        //        }
        //    }
        //    return Result;
        //}

        //added by shrutika salve 27112023

        public DataTable GetBranchOrderValuepoAmd(string StrYear, string strMonth)
        {
            DataTable DTEmployeeAttendanceSearchByDate = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MisReportMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "27");
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

        public DataTable GetDataOrderBookingRegister(string FromDate, string ToDate)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {


                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 29);
                CMDGetEnquriy.Parameters.AddWithValue("@DateFrom", FromDate);
                CMDGetEnquriy.Parameters.AddWithValue("@DateTo", ToDate);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }


        public List<JobMasters> GetJobDashBoardBookingorder()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<JobMasters> lstEnquiryDashB = new List<JobMasters>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 30);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new JobMasters
                           {
                               Count = DTEMDashBoard.Rows.Count,
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
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataTable GetJobdata(string job,string pkuserid)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_GetJobFeedbacklinkData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                CMDGetDdlLst.Parameters.AddWithValue("@JobNo", job);
                CMDGetDdlLst.Parameters.AddWithValue("@pk_userid", pkuserid);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                // Note: No need to dispose of the DataTable explicitly here
            }
            return DSGetddlList;
        }

        public DataSet UserEmailid()//Checking User Login
        {
            DataSet DTLoginDtl = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_UserLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 16);
                cmd.Parameters.AddWithValue("@UserName", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDALoginDtl = new SqlDataAdapter(cmd);
                SDALoginDtl.Fill(DTLoginDtl);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTLoginDtl.Dispose();
            }
            return DTLoginDtl;
        }


        //added by shrutika salve 10042023
        public string Insertdata(string PK_JOB_ID, string ClientContact, string TUVEmailNo)
        {
            string Result = "";

            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_EnquiryMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", "UpdateQuery");
                CMDInsert.Parameters.AddWithValue("@ClientContact", ClientContact);
                CMDInsert.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDInsert.Parameters.AddWithValue("@Tuv_Email", TUVEmailNo);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {

                string msg = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }


        public DataSet GetRecord(string PK_JOB_ID)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", "57A");
                //cmd.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                cmd.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsClientFeedbackHistory);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsClientFeedbackHistory.Dispose();
            }
            return DsClientFeedbackHistory;
        }



        //added by shrutika salve 12042024



       


        public DataSet VendorDetais(string pk_jobid)
        {
            string Result = string.Empty;
            DataSet DataResult = new DataSet();
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster ", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 58);
                cmd.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DataResult);
                //Result = cmd.ExecuteNonQuery().ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return DataResult;
        }


        public DataSet GetRecordData(string PK_JOB_ID)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_EnquiryMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", "59");
                //cmd.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                cmd.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsClientFeedbackHistory);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsClientFeedbackHistory.Dispose();
            }
            return DsClientFeedbackHistory;
        }


        public string CheckData(int? pkjobid, string Ponumber)// Binding Sales Masters DashBoard of Master Page 
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.Parameters.AddWithValue("@pk_job_id", pkjobid);
                CMDGetEnquriy.Parameters.AddWithValue("@vendorPO", Ponumber);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

            return Result.ToString();
        }

        public DataTable GetUserName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 43);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }


        public DataSet GetLegaldata_job()
        {
            DataSet DTScripName = new DataSet();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("GetJobLegalData", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                //CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 1);

                //CMDSearchNameCode.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }
        public DataTable GetStatus(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "58");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_JOB_ID", EQ_ID);
                //CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }

        public DataTable GetLegalMail()
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "99");
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDGetEnquiryDtls);
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

        //added by nikita on 19062024
        public DataTable GetJobNumber(int? EQ_ID)
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", "57");
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_JOB_ID", EQ_ID);
                //CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }


        //added by shrutika salve 27032024
        public string ChcekCustomer(string SapNo, string pk_QI, string pk_jobid)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 100);
                CMDGetEnquriy.Parameters.AddWithValue("@SAP_No", SapNo);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                CMDGetEnquriy.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                //CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }



        public string UpdateCustomer(string SapNo, string pk_QI, string pk_jobid)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 101);
                CMDGetEnquriy.Parameters.AddWithValue("@SAP_No", SapNo);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                CMDGetEnquriy.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                Result = CMDGetEnquriy.ExecuteNonQuery().ToString();
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }

        public DataSet ShowCustomerNameAsPerSap(string SapNo, string pk_QI, string pk_jobid)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 113);
                CMDGetDdlLst.Parameters.AddWithValue("@SAP_No", SapNo);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


        //added by shrutika salve 27032024
        public string getCompanyName(string pk_QI)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 105);              
                CMDGetEnquriy.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                //CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }


        public DataTable GetSearchJobNumber(string JobNumber)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_JobMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 106);
                CMDSearchNameCode.Parameters.AddWithValue("@Job_Number", JobNumber);
               // CMDSearchNameCode.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }

        public string UpdateBlockJob(string pk_jobid,string Reason)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 107);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid); 
                CMDGetEnquriy.Parameters.AddWithValue("@unblockJobReason", Reason);
                CMDGetEnquriy.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                //CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }


        public string GetSapNumber(string Name)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 108);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", Name);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }


        public string ChcekCostCentre(string SapNo)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 109);
                CMDGetEnquriy.Parameters.AddWithValue("@SAP_No", SapNo);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", UserIDs);
                //CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }


        public DataSet TakeCostCenter(string SapNo)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 110);
                CMDGetDdlLst.Parameters.AddWithValue("@SAP_No", SapNo);
                //CMDGetDdlLst.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


        public DataSet GetSapData(string pk_jobid)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 111);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", pk_jobid);
                //CMDGetDdlLst.Parameters.AddWithValue("@PK_QT_ID", pk_QI);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }

        public string GetSubJob(string Name)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 112);
                CMDGetEnquriy.Parameters.AddWithValue("@Job_Number", Name);

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return Result.ToString();
        }

        //Added BY Satish Pawar On 15 May 2023
        public string SaveFeedbackEmailHistory(string JobNo, string Email_IDs, string Feedback_Link, string CompanyName, string pkuserid)
        {

            string Result = "";
            con.Open();
            con1.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                string[] emailArray = Email_IDs.Split(';');
                string[] Pk_userid = pkuserid.Split(';');



                for (int i = 0; i < emailArray.Length; i++)
                {
                    string email = emailArray[i];
                    string pkUserId = Pk_userid[i];

                    string UpdatedFeedbackLink = Feedback_Link + "&id=" + pkUserId;

                    SqlCommand CMD_SaveFeedbackEmailHistory = new SqlCommand("SP_SaveFeedbackEmailHistory", con);
                    CMD_SaveFeedbackEmailHistory.CommandType = CommandType.StoredProcedure;
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@SP_Type", "1");
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@JobNo", JobNo);
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@EmailID", email.Trim()); // Trim to remove leading/trailing spaces
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@Feedback_Link", UpdatedFeedbackLink);
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CouterValue", pkUserId.Trim());
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CompanyName", CompanyName);

                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@Result", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    CMD_SaveFeedbackEmailHistory.ExecuteNonQuery();
                    Result = Convert.ToString(CMD_SaveFeedbackEmailHistory.Parameters["@Result"].Value.ToString());

                    SqlCommand CMD = new SqlCommand("SP_SaveFeedbackEmailHistory", con1);
                    CMD.CommandType = CommandType.StoredProcedure;
                    CMD.Parameters.AddWithValue("@SP_Type", "1");
                    CMD.Parameters.AddWithValue("@JobNo", JobNo);
                    CMD.Parameters.AddWithValue("@EmailID", email.Trim()); // Trim to remove leading/trailing spaces
                    CMD.Parameters.AddWithValue("@Feedback_Link", UpdatedFeedbackLink);
                    CMD.Parameters.AddWithValue("@CouterValue", pkUserId.Trim());
                    CMD.Parameters.AddWithValue("@CompanyName", CompanyName);
                    CMD.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMD.Parameters.AddWithValue("@Result", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                    CMD.ExecuteNonQuery();
                    Result = Convert.ToString(CMD.Parameters["@Result"].Value.ToString());


                }




            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }


    }
}