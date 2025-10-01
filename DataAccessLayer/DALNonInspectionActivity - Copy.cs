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
    public class DALNonInspectionActivity
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        static string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"].ToString();

        public string Insert(NonInspectionActivity CPM)
        {
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '1');
                cmd.Parameters.AddWithValue("@ActivityType", CPM.ActivityType);
                cmd.Parameters.AddWithValue("@Location", CPM.Location);               
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                cmd.Parameters.AddWithValue("@Description", CPM.Description);
                cmd.Parameters.AddWithValue("@JobNumber", CPM.JobNumber);
                cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                cmd.Parameters.AddWithValue("@ODTime", CPM.ODTime);
                cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                cmd.Parameters.AddWithValue("@wfhcheckbox", CPM.WFHCheckbox);
                cmd.Parameters.AddWithValue("@Ratings", CPM.Rating);
                cmd.Parameters.AddWithValue("@Call_No", CPM.CallNumber);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = cmd.ExecuteNonQuery().ToString();

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

        public DataSet GetServiceCode()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
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
        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetCalenderData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetDataByDate(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                // It throws Argument null exception  
                //string dateTime1 = DateTime.ParseExact(n.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy");
                //string dateTime2 = DateTime.ParseExact(n.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy");

                //string DateF = n.FromD;

                //string DateT = n.ToD;



                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@DateF",DateTime.ParseExact(DateTime.ParseExact(n.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(n.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetCalenderDataByDate(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            try
            {

                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@DateF", DateTime.ParseExact(DateTime.ParseExact(n.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(n.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet GetDataById(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@ID", id);
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

        public string Update(NonInspectionActivity N, int id)
        {
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                
               SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@Id", N.Id);
                cmd.Parameters.AddWithValue("@ActivityType", N.ActivityType);
                cmd.Parameters.AddWithValue("@Description", N.Description);
                //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(N.StartDate, "dd/MM/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(N.EndDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(N.DateSE, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@Location", N.Location);
                cmd.Parameters.AddWithValue("@ServiceCode", N.ServiceCode);
                cmd.Parameters.AddWithValue("@TravelTime", N.TravelTime);
                cmd.Parameters.AddWithValue("@StartTime", N.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", N.EndTime);
                cmd.Parameters.AddWithValue("@ODTime", N.ODTime);
                cmd.Parameters.AddWithValue("@JobNumber", N.JobNumber);
                cmd.Parameters.AddWithValue("@Attachment", N.Attachment);
                cmd.Parameters.AddWithValue("@TotalTime", N.TotalTime);
                cmd.Parameters.AddWithValue("@wfhcheckbox", N.WFHCheckbox);
                cmd.Parameters.AddWithValue("@Call_No", N.CallNumber);
                cmd.Parameters.AddWithValue("@Ratings", N.Rating);
                Result = cmd.ExecuteNonQuery().ToString();

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

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@Id",id);
                Result = cmd.ExecuteNonQuery().ToString();

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
        public DataSet ValidateTT(string date)
        {
            /*string _dtcon = Convert.ToString(date);
            string[] _dtchn = _dtcon.Split('-');
            string _dtcor = Convert.ToString(_dtchn[1]) + "-" + Convert.ToString(_dtchn[0]) + "-" + Convert.ToString(_dtchn[2]);
            DateTime ActualDate = Convert.ToDateTime(_dtcor);*/

            DataSet ValidateTotal = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date,"dd/MM/yyyy",null));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ValidateTotal);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ValidateTotal.Dispose();
            }
            return ValidateTotal;
        }

        public DataTable ValidateJob(string strJobNo)
        {
            DataTable ValidateSubJob = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@JobNumber", strJobNo);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ValidateSubJob);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ValidateSubJob.Dispose();
            }
            return ValidateSubJob;
        }

        public DataTable DuplicateCall(string DupCmpNm)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_CallsMaster", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 62);
                DupCompany.Parameters.AddWithValue("@Call_No", DupCmpNm);
                SqlDataAdapter dr = new SqlDataAdapter(DupCompany);
                dr.Fill(DTGetCompanyList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetCompanyList.Dispose();
            }
            return DTGetCompanyList;
        }


    }
}