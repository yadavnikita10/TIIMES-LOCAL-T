using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class DalOPEAutomated
    {


        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataSet GetOPApprovalList(string UserId,string Month,string Year)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_ApprovalListData_Automated", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "23");
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetData(string Month, string Year)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        public DataSet GetOPApprovalList_Finance(string UserId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "Finance");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataSet GetOPApprovalListButtonwise(string Branch,string month,string year)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_ApprovalListDataButtonwise_Automated", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "23");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.Parameters.AddWithValue("@BranchOpnumber", Branch);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
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
        public DataSet GetOPBulk_ApprovalList(DataSet dsBulkApprove, string UserId, string OP_Number)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_BulUpload_Automated", con);
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.AddWithValue("@xmlData", dsBulkApprove.GetXml());
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.CommandTimeout = 1000000;
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
        public DataTable GetStatusOpApproval()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetOPApprovalFolderStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataSet GetAccountantExportData(string month,string year)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OPExportData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9A");
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 600;
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