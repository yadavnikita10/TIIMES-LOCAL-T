using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALCustomerFeedback
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataTable GetFeedbackDashBoard()
        {
            DataTable DTFeedBack = new DataTable();
            try
            {
                SqlCommand CMDFeedBack = new SqlCommand("SP_CustomerFeedback", con);
                CMDFeedBack.CommandType = CommandType.StoredProcedure;
                CMDFeedBack.Parameters.AddWithValue("@SP_Type", 1);
                CMDFeedBack.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDFeedBack.CommandTimeout = 100000;
                SqlDataAdapter Ad = new SqlDataAdapter(CMDFeedBack);
                Ad.Fill(DTFeedBack);

            }
            catch(Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTFeedBack.Dispose();
            }
            return DTFeedBack;
        }

        public DataTable GetFeedbackDashBoardByDate(CustomerFeedback a)
        {
            DataTable DTFeedBack = new DataTable();
            try
            {
                string DateF = a.FromDate;
                //IFormatProvider culture = new CultureInfo("en-US", true);
                //DateTime DateFrom = DateTime.ParseExact(DateF, "yyyy-MM-dd", culture);

                string DateT = a.ToDate;
                //IFormatProvider culture1 = new CultureInfo("en-US", true);
                //DateTime DateTo = DateTime.ParseExact(DateT, "yyyy-MM-dd", culture1);


                SqlCommand CMDFeedBack = new SqlCommand("SP_CustomerFeedback", con);
                CMDFeedBack.CommandType = CommandType.StoredProcedure;
                CMDFeedBack.Parameters.AddWithValue("@SP_Type", 3);
                CMDFeedBack.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateF, "dd/MM/yyyy", theCultureInfo));
                CMDFeedBack.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateT, "dd/MM/yyyy", theCultureInfo));
                //CMDFeedBack.Parameters.AddWithValue("@FromDate", DateF);
                //CMDFeedBack.Parameters.AddWithValue("@ToDate", DateT);
                CMDFeedBack.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDFeedBack.CommandTimeout = 100000;
                SqlDataAdapter Ad = new SqlDataAdapter(CMDFeedBack);
                Ad.Fill(DTFeedBack);

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTFeedBack.Dispose();
            }
            return DTFeedBack;
        }

        public string InsertFeedBack(CustomerFeedback CustFeed)
         {
            string result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDFeedBack = new SqlCommand("SP_CustomerFeedback", con);
                CMDFeedBack.CommandType = CommandType.StoredProcedure;
                CMDFeedBack.Parameters.AddWithValue("@SP_Type", 2);
                CMDFeedBack.Parameters.AddWithValue("@Created_by", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDFeedBack.Parameters.AddWithValue("@Created_Date", CustFeed.Created_Date);
                // CMDFeedBack.Parameters.AddWithValue("@Status", CustFeed.Status);
                CMDFeedBack.Parameters.AddWithValue("@Name_Of_organisation", CustFeed.Name_Of_organisation);
                CMDFeedBack.Parameters.AddWithValue("@Name_Of_Respondent", CustFeed.Name_Of_Respondent);
                CMDFeedBack.Parameters.AddWithValue("@Desighnation_Of_Respondent", CustFeed.Desighnation_Of_Respondent);
                CMDFeedBack.Parameters.AddWithValue("@Order_No", CustFeed.Order_No);
                CMDFeedBack.Parameters.AddWithValue("@Enquiry_response", CustFeed.Enquiry_response);
                CMDFeedBack.Parameters.AddWithValue("@Quotation_Time_Frame_feedback", CustFeed.Quotation_Time_Frame_feedback);
                CMDFeedBack.Parameters.AddWithValue("@Requirement_Understanding", CustFeed.Requirement_Understanding);
                CMDFeedBack.Parameters.AddWithValue("@Quotation_information", CustFeed.Quotation_information);
                CMDFeedBack.Parameters.AddWithValue("@Quotation_Submit_Response", CustFeed.Quotation_Submit_Response);
                CMDFeedBack.Parameters.AddWithValue("@Email_call_ResponseTime", CustFeed.Email_call_ResponseTime);
                CMDFeedBack.Parameters.AddWithValue("@Requested_Call_schedule", CustFeed.Requested_Call_schedule);
                CMDFeedBack.Parameters.AddWithValue("@Confirmation_Reception", CustFeed.Confirmation_Reception);
                CMDFeedBack.Parameters.AddWithValue("@Change_in_schedule_Response", CustFeed.Change_in_schedule_Response);
                CMDFeedBack.Parameters.AddWithValue("@Communication_satisfaction", CustFeed.Communication_satisfaction);
                CMDFeedBack.Parameters.AddWithValue("@Behaiviour_of_Inspector", CustFeed.Behaiviour_of_Inspector);
                CMDFeedBack.Parameters.AddWithValue("@implementation_of_safety_requirements_Of_Inspector", CustFeed.implementation_of_safety_requirements_Of_Inspector);
                CMDFeedBack.Parameters.AddWithValue("@quality_of_inspection", CustFeed.quality_of_inspection);
                CMDFeedBack.Parameters.AddWithValue("@efficiency_with_time", CustFeed.efficiency_with_time);
                CMDFeedBack.Parameters.AddWithValue("@Maintanance_Of_confidentiality_and_Integrity", CustFeed.Maintanance_Of_confidentiality_and_Integrity);
                CMDFeedBack.Parameters.AddWithValue("@inspection_report_or_Releasenote_Time", CustFeed.inspection_report_or_Releasenote_Time);
                CMDFeedBack.Parameters.AddWithValue("@Expectation_Meet", CustFeed.Expectation_Meet);
                CMDFeedBack.Parameters.AddWithValue("@report_for_number_of_errors", CustFeed.report_for_number_of_errors);
                CMDFeedBack.Parameters.AddWithValue("@association_with_TUV_India", CustFeed.association_with_TUV_India);
                CMDFeedBack.Parameters.AddWithValue("@Suggestions", CustFeed.Suggestions);
                CMDFeedBack.Parameters.AddWithValue("@Score_Achieved", CustFeed.Score_Achieved);
                CMDFeedBack.Parameters.AddWithValue("@Score_percentage", CustFeed.Score_percentage);
                CMDFeedBack.Parameters.AddWithValue("@Client_Location", CustFeed.Client_Location);
                result = CMDFeedBack.ExecuteNonQuery().ToString();
            }
            catch(Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        //*************************************** Delete Feedback - Code By Ankush ******************************************
        public bool DeleteFeedback(int id)
        {
            SqlCommand CMDDeleteFeedback = new SqlCommand("SP_CustomerFeedback", con);
            CMDDeleteFeedback.CommandType = CommandType.StoredProcedure;
            CMDDeleteFeedback.Parameters.AddWithValue("@SP_Type", 5);
            CMDDeleteFeedback.Parameters.AddWithValue("@FeedBack_ID", id);
            con.Open();
            int i = CMDDeleteFeedback.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}