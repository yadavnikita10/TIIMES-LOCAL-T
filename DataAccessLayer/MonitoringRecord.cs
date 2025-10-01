using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class MonitoringRecord
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        //added by nikita 
        public DataTable GetDataByDate(string FromDate, string ToDate)
        {
            DataTable ds = new DataTable();
            try
            {


                SqlCommand cmd = new SqlCommand("SP_MonitoringRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@UserID", UserIDs);
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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
        public List<MonitorRecordData> GetMonitorrecord()
        {
            DataTable ds = new DataTable();
            List<MonitorRecordData> lstmonitoringrecord = new List<MonitorRecordData>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MonitoringRecord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@UserID", UserIDs);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                if (ds.Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Rows)
                    {
                        lstmonitoringrecord.Add(
                           new MonitorRecordData
                           {
                               Count = ds.Rows.Count,

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


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return lstmonitoringrecord;
        }

    }
}