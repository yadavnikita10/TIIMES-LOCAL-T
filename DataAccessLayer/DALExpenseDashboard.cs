using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class DALExpenseDashboard
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        //Added By Satish Pawar on 26 Jul 2023
        public DataSet GetExpenseReport()
        {
            DataSet DsExpenseReport = new DataSet();
            try
            {
                SqlCommand CMDExpenseReport = new SqlCommand("SP_ExpenseItem", con);
                CMDExpenseReport.CommandType = CommandType.StoredProcedure;
                CMDExpenseReport.CommandTimeout = 900000000;
                CMDExpenseReport.Parameters.AddWithValue("@SP_Type", "19");
                SqlDataAdapter SDAExpenseReport = new SqlDataAdapter(CMDExpenseReport);
                SDAExpenseReport.Fill(DsExpenseReport);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsExpenseReport.Dispose();
            }
            return DsExpenseReport;
        }
        public DataSet GetExpenseReportDetails()
        {
            DataSet DsExpenseReportDetails = new DataSet();
            try
            {
                SqlCommand CMDExpenseReportDetails = new SqlCommand("SP_ExpenseItem", con);
                CMDExpenseReportDetails.CommandType = CommandType.StoredProcedure;
                CMDExpenseReportDetails.CommandTimeout = 900000000;
                CMDExpenseReportDetails.Parameters.AddWithValue("@SP_Type", "20");
                SqlDataAdapter SDAExpenseReportDetails = new SqlDataAdapter(CMDExpenseReportDetails);
                SDAExpenseReportDetails.Fill(DsExpenseReportDetails);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsExpenseReportDetails.Dispose();
            }
            return DsExpenseReportDetails;
        }
        
    }
}