using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TuvVision.Models;


namespace TuvVision.DataAccessLayer
{
    public class DALActivity
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public string InsertUpdateActivity(NonInspectionActivity CPM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if(CPM.Id ==0)
                {
                    SqlCommand cmd = new SqlCommand("SP_Activities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@ActivityType", "Inspection Activity");
                    cmd.Parameters.AddWithValue("@Location", CPM.Location);
                    //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact( CPM.StartDate,"dd/MM/yyyy",theCultureInfo));
                    //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact( CPM.EndDate,"dd/MM/yyyy",theCultureInfo));
                    cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@Description", CPM.Description);
                    cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                    cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                    cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                    cmd.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    cmd.Parameters.AddWithValue("@CallID", CPM.CallId);
                    cmd.Parameters.AddWithValue("@Type", "IA");
                    cmd.Parameters.AddWithValue("@JobNumber", CPM.Sub_Job);
                    cmd.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {//Update
                    SqlCommand cmd = new SqlCommand("SP_Activities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '4');
                    cmd.Parameters.AddWithValue("@Id", CPM.Id);
                    cmd.Parameters.AddWithValue("@ActivityType", "Inspection Activity");
                    cmd.Parameters.AddWithValue("@Description", CPM.Description);
                    //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(CPM.StartDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(CPM.EndDate, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@Location", CPM.Location);
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                    cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);

                    cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                    cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                    cmd.Parameters.AddWithValue("@Type", CPM.Type);
                    cmd.Parameters.AddWithValue("@CallID", CPM.CallId);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
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
        public bool DeleteActivity(int id)
        {
            SqlCommand CMDDeleteAcvt = new SqlCommand("SP_Activities", con);
            CMDDeleteAcvt.CommandType = CommandType.StoredProcedure;
            CMDDeleteAcvt.Parameters.AddWithValue("@SP_Type", "5");
            CMDDeleteAcvt.Parameters.AddWithValue("@Id", id);
            con.Open();
            int i = CMDDeleteAcvt.ExecuteNonQuery();
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

        public string Delete(int id) //--Delete Code
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Activities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@Id", id);
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
        public DataTable GetActivity(int ID, int PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetActivityData = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Activities", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "7");
                CMDGetDdlLst.Parameters.AddWithValue("@Id", ID);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetActivityData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetActivityData.Dispose();
            }
            return DSGetActivityData;
        }

        public DataSet GetData(int? PK_Call_ID) //--Get All DATA
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Activities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');
                cmd.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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

        public DataSet GetDate(int? PK_Call_ID) //--Get All DATA
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Activities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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

        public DataSet GetDataByDate(NonInspectionActivity n)  // Search datewise 
        {
            DataSet ds = new DataSet();
            try
            {

                string DateF = n.FromD;
                IFormatProvider culture = new CultureInfo("en-US", true);
                DateTime DateFrom = DateTime.ParseExact(DateF, "yyyy-MM-dd", culture);

                string DateT = n.ToD;
                IFormatProvider culture1 = new CultureInfo("en-US", true);
                DateTime DateTo = DateTime.ParseExact(DateT, "yyyy-MM-dd", culture1);


                SqlCommand cmd = new SqlCommand("SP_Activities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@DateF", DateF);
                cmd.Parameters.AddWithValue("@DateTo", DateT);
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
        public string InsertUpdateActivityExpediting(NonInspectionActivity CPM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.Id == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_Activities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@ActivityType", "Inspection Activity");
                    cmd.Parameters.AddWithValue("@Location", CPM.Location);
                    //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact( CPM.StartDate,"dd/MM/yyyy",theCultureInfo));
                    //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact( CPM.EndDate,"dd/MM/yyyy",theCultureInfo));
                    cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@Description", CPM.Description);
                    cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                    cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                    cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                    cmd.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    cmd.Parameters.AddWithValue("@CallID", CPM.CallId);
                    cmd.Parameters.AddWithValue("@Type", "IA");
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {//Update
                    SqlCommand cmd = new SqlCommand("SP_Activities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '4');
                    cmd.Parameters.AddWithValue("@Id", CPM.Id);
                    cmd.Parameters.AddWithValue("@ActivityType", "Inspection Activity");
                    cmd.Parameters.AddWithValue("@Description", CPM.Description);
                    //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(CPM.StartDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(CPM.EndDate, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@Location", CPM.Location);
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                    cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                    cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                    cmd.Parameters.AddWithValue("@Type", CPM.Type);
                    cmd.Parameters.AddWithValue("@CallID", CPM.CallId);
                    Result = cmd.ExecuteNonQuery().ToString();
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