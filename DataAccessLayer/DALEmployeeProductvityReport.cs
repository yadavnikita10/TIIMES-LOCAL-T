using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TuvVision.DataAccessLayer
{
    public class DALEmployeeProductvityReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public DataSet GetDdlBranchLst()//Binding All Dropdownlist
        {
            DataSet DSBranchDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ManDaySummeryReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSBranchDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSBranchDdlList.Dispose();
            }
            return DSBranchDdlList;
        }
        public DataSet GetExcelEmployeeProductvityReport(DateTime? FromDate, DateTime? ToDate, string BranchName, string InspectorID)
        {
            DataSet DTEPReport = new DataSet();
            try
            {
                SqlCommand CMDEPReport = new SqlCommand("SP_ManDaySummeryReport", con);
                CMDEPReport.CommandType = CommandType.StoredProcedure;
                CMDEPReport.Parameters.AddWithValue("@SP_Type", 3);
                CMDEPReport.Parameters.AddWithValue("@FromDate", FromDate);
                CMDEPReport.Parameters.AddWithValue("@ToDate", ToDate);
                if (BranchName == "" || BranchName==null)
                {
                    CMDEPReport.Parameters.AddWithValue("@BranchName", null);
                }
                else
                {
                    CMDEPReport.Parameters.AddWithValue("@BranchName", BranchName);
                }
                if(InspectorID=="" || InspectorID==null)
                {
                    CMDEPReport.Parameters.AddWithValue("@InspectorID", null);
                }
                else
                {
                    CMDEPReport.Parameters.AddWithValue("@InspectorID", InspectorID);
                }
                //CMDEPReport.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEPReport = new SqlDataAdapter(CMDEPReport);
                SDAEPReport.Fill(DTEPReport);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                DTEPReport.Dispose();
            }
            return DTEPReport;
        }

    }
}