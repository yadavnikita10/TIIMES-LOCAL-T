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
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        CultureInfo provider = CultureInfo.InvariantCulture;

        public string Insert(NonInspectionActivity CPM,int? Id)
        {
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            string Result = string.Empty;
            con.Open();
            try
            {
                if(Id>0)
                {
                    SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '4');
                    cmd.Parameters.AddWithValue("@Id", CPM.Id);
                    cmd.Parameters.AddWithValue("@ActivityType", CPM.ActivityType);
                    cmd.Parameters.AddWithValue("@Description", CPM.Description);
                    //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(CPM.StartDate, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(CPM.EndDate, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(CPM.DateSE, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@Location", CPM.Location);
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@TravelTime", CPM.TravelTime);
                    cmd.Parameters.AddWithValue("@StartTime", CPM.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", CPM.EndTime);
                    cmd.Parameters.AddWithValue("@ODTime", CPM.ODTime);
                    cmd.Parameters.AddWithValue("@JobNumber", CPM.JobNumber);
                    cmd.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    cmd.Parameters.AddWithValue("@TotalTime", CPM.TotalTime);
                    cmd.Parameters.AddWithValue("@wfhcheckbox", CPM.WFHCheckbox);
                    cmd.Parameters.AddWithValue("@Call_No", CPM.CallNumber);
                    cmd.Parameters.AddWithValue("@Ratings", CPM.Rating);
                    cmd.Parameters.AddWithValue("@New_ExistingCustomer", CPM.NewExistingCustomer);
                    cmd.Parameters.AddWithValue("@Dom_Inter_Visit", CPM.DomesticInternationVisit);
                    cmd.Parameters.AddWithValue("@CostCenter", CPM.CostCenter);  //added by nikita on 29042024
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
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
                    cmd.Parameters.AddWithValue("@New_ExistingCustomer", CPM.NewExistingCustomer);
                    cmd.Parameters.AddWithValue("@Dom_Inter_Visit", CPM.DomesticInternationVisit);
                    cmd.Parameters.AddWithValue("@CostCenter", CPM.CostCenter);  //added by nikita on 29042024
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
                cmd.Parameters.AddWithValue("@New_ExistingCustomer", N.NewExistingCustomer);
                cmd.Parameters.AddWithValue("@Dom_Inter_Visit", N.DomesticInternationVisit);
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

        public DataTable ValidateJob(string strJobNo,string ActType)
        {
            DataTable ValidateSubJob = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                cmd.Parameters.AddWithValue("@JobNumber", strJobNo);
                cmd.Parameters.AddWithValue("@ActivityType", ActType);
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

        public DataTable ValidateControlNo(string strJobNo)
        {
            DataTable ValidateSubJob = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "23");
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

        public DataSet GetNonInspectionDataByDate(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            try
            {

                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "21");
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

        public DataSet GetNoninspectionData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "22");
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



        public DataSet GetAttendanceSheet(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Attendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", '1');
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(DateTime.ParseExact(n.StartDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(DateTime.ParseExact(n.EndDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet GetAdminAttendanceSheet(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Attendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", '2');
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(DateTime.ParseExact(n.StartDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(DateTime.ParseExact(n.EndDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataTable GetActivityMaster()
        {
            DataTable ds = new DataTable();


            try
            {
                SqlCommand cmd = new SqlCommand("SP_Attendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", '3');
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

        public DataSet GetUserDataSheet()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                //commented by nikita on 18122023
                //SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Sp_Type", "18");
                 SqlCommand cmd = new SqlCommand("Sp_Get_GeUserdetails_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "2");
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {

                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_MISOPEReport", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 19);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetServiceName()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "25");
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


        public DataTable chkHoliday(string fromDate, string toDate)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_NonInspectionActivities", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 27);
                //DupCompany.Parameters.AddWithValue("@fromDate", fromDate);
                //DupCompany.Parameters.AddWithValue("@toDate", toDate);

                if (fromDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@StartDate1", "");
                }
                if (toDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(DateTime.ParseExact(toDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@EndDate1", "");
                }

                DupCompany.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable WeeklyOff(string fromDate, string toDate)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_NonInspectionActivities", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 28);
                //DupCompany.Parameters.AddWithValue("@fromDate", fromDate);
                //DupCompany.Parameters.AddWithValue("@toDate", toDate);

                if (fromDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    //DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(fromDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@StartDate1", "");
                }
                if (toDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    //DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(toDate, "MM/dd/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(DateTime.ParseExact(toDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@EndDate1", "");
                }

                DupCompany.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable dtchkIfSunday(string fromDate, string toDate)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_NonInspectionActivities", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 29);
                //DupCompany.Parameters.AddWithValue("@fromDate", fromDate);
                //DupCompany.Parameters.AddWithValue("@toDate", toDate);

                if (fromDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@StartDate1", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@StartDate1", "");
                }
                if (toDate != null)
                {
                    //DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    DupCompany.Parameters.AddWithValue("@EndDate1", DateTime.ParseExact(DateTime.ParseExact(toDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                }
                else
                {
                    DupCompany.Parameters.AddWithValue("@EndDate1", "");
                }

                DupCompany.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        public DataTable ValidateSalesOrderNo(string strJobNo)
        {
            DataTable ValidateSubJob = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "30");
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

        //added by ruchita on 08052024

        //added by ruchita 25 apr 08/05/2024
        public DataTable GetUserDetails(string UserId)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetUserDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserId);
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

        //added by nikita on 06052024
        public DataTable CheckSendForApproval(string pkcallid)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_Check_OPE_SendForApproval", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@id", pkcallid);
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

        public DataSet GetUserCalendarDateWise(NonInspectionActivity n, string pkUserID)
        {
            DataSet ds = new DataSet();
            try
            {

                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "31");
                cmd.Parameters.AddWithValue("@DateF", DateTime.ParseExact(DateTime.ParseExact(n.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(n.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.Parameters.AddWithValue("@CreatedBy", pkUserID);
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

        public DataSet GetUserCalendar(NonInspectionActivity n, string pkUserID)
        {
            DataSet ds = new DataSet();
            try
            {

                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "32");
                cmd.Parameters.AddWithValue("@DateF", DateTime.ParseExact(DateTime.ParseExact(n.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(n.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.Parameters.AddWithValue("@CreatedBy", pkUserID);

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


        public DataSet GetLeaveAttendanceSheet(NonInspectionActivity n)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Attendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", '4');
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(DateTime.ParseExact(n.StartDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(DateTime.ParseExact(n.EndDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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
    }
}