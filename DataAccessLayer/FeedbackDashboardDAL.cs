using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class FeedbackDashboardDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public DataSet GetDashboard()
        {
            DataSet DsFeedbackDashboard = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                //cmd.Parameters.AddWithValue("@StageName", Stage);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsFeedbackDashboard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsFeedbackDashboard.Dispose();
            }
            return DsFeedbackDashboard;
        }

        public DataSet ClientFeedbackHistory(string ClientJobId)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ClientFeedbackHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@ClientJobId", ClientJobId);

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

        public DataSet GetDashboardDate(string fromdate, string Todate)
        {
            DataSet DsFeedbackDashboard = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetDashboard", con);
                cmd.Parameters.AddWithValue("@SP_Type", 2);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@ToDate", Todate);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsFeedbackDashboard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsFeedbackDashboard.Dispose();
            }
            return DsFeedbackDashboard;
        }

        public DataTable GetCustomerFeedback()
        {
            DataTable ds = new DataTable();
            try
            {


                SqlCommand cmd = new SqlCommand("SP_CustomerFeedbackRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@SP_Type", "9");
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

        public DataSet GetData(string fromdate, string Todate)
        {
            DataSet DsFeedbackDashboard = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetDashboard", con);
                cmd.Parameters.AddWithValue("@SP_Type", 1);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@FromDate", fromdate);
                cmd.Parameters.AddWithValue("@ToDate", Todate);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsFeedbackDashboard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsFeedbackDashboard.Dispose();
            }
            return DsFeedbackDashboard;
        }

    }
}