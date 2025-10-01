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
    public class DalMisReportQHSE
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable GetAuditReportDashBoard()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("MisReport", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 1);
                //CMDCallDash.Parameters.AddWithValue("@FKAuditId", FKAuditId);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }



        //code for list of compentency Evaluation 

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        //code for mentee and mentor list

        public DataSet GetMentorList()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("MisReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 3);
                //CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }
        public DataSet GetDataMentor(String UserId1)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 4);
                cmd.Parameters.AddWithValue("@UserID", UserId1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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
        public DataSet EditBranch(int? Br_Id)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("MisReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditContact.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataTable GetDetails() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("MisReport", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 4);
                CMDCallDash.Parameters.AddWithValue("@UserId", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        public List<MonitorRecordData> GetMonitorrecord()
        {
            DataTable ds = new DataTable();
            List<MonitorRecordData> lstmonitoringrecord = new List<MonitorRecordData>();

            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 7);
                //cmd.Parameters.AddWithValue("@UserID", UserIDs);

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
        public DataTable GetDataByDate(string FromDate, string ToDate)
        {
            DataTable ds = new DataTable();
            try
            {


                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 6);
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

        public DataSet GetDataReport()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
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
        public DataTable dtGetNAME()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CompentencyMetrixView", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
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


        public DataSet GetDataSafetyIncident()
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("MisReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
                //CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                //CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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
        public DataSet GetDataN()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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

        public DataSet GetMasterDataN()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
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
        public DataTable dtGetNAME1()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
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