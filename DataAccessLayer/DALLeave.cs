using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALLeave
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);


        public string Insert(Leave objL)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "13");
                cmd.Parameters.AddWithValue("@ActivityType", objL.ActivityType);
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(objL.DateSE, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@LeaveAddedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@Attachment", objL.Attachment + ',');
                cmd.Parameters.AddWithValue("@Reason", objL.Reason);
                cmd.Parameters.AddWithValue("@ChkLTILeave", objL.ChkLTILeave);
                cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);//Leave added for
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

        public string Update(Leave objL)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "16");
                cmd.Parameters.AddWithValue("@ActivityType", objL.ActivityType);


                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(DateTime.ParseExact(objL.DateSE, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));


                cmd.Parameters.AddWithValue("@LeaveAddedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@Attachment", objL.Attachment + ',');
                cmd.Parameters.AddWithValue("@Reason", objL.Reason);
                cmd.Parameters.AddWithValue("@ChkLTILeave", objL.ChkLTILeave);

                cmd.Parameters.AddWithValue("@Id", objL.Id);
                cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);//Leave added for
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

        public DataSet GetLeaveDashboard()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "14");
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
                cmd.Parameters.AddWithValue("@SP_Type", "3");
                cmd.Parameters.AddWithValue("@Id", id);
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

        public string Delete(int id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
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

        public DataSet GetDataByDate(Leave LD)
        {
            DataSet ds = new DataSet();
            try
            {


                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "18");
                cmd.Parameters.AddWithValue("@DateF", DateTime.ParseExact(DateTime.ParseExact(LD.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(LD.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataTable CheckValidLeave(string date, Leave objL)
        {


            DataTable ValidateTotal = new DataTable();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "19");
                //cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(DateTime.ParseExact(objL.StartDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(DateTime.ParseExact(date, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable CheckIfLeavePresent(string date, Leave objL)
        {


            DataTable ValidateTotal = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "20");
                /// cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(DateTime.ParseExact(date, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetBranchMaster()
        {
            DataTable dt = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetBranchMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }

        public string InserHolodayListBranch(DataTable dt, Leave OBJLeave)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("sp_InsertHolidayList", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                ///CMDInsertUpdatebranch.Parameters.AddWithValue("@udt_InsertHolidayList", dt);

                CMDInsertUpdatebranch.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Year", OBJLeave.Year);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "1");
                SqlParameter tvparam = CMDInsertUpdatebranch.Parameters.AddWithValue("@tbl_Holiday_Master", dt);

                tvparam.SqlDbType = SqlDbType.Structured;
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
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
        public string DeleteHolidayList(Leave ObjLeave)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("sp_InsertHolidayList", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;


                CMDInsertUpdatebranch.Parameters.AddWithValue("@Year", ObjLeave.Year);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "4");










                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
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

        public DataSet GetHolidayDetail()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertHolidayList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");

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

        public DataSet GetHolidayListDetail(Leave ObjLeave)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("sp_InsertHolidayList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3");
                cmd.Parameters.AddWithValue("Year", ObjLeave.Year);
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

        public SelectList GetYears(int? iSelectedYear)
        {
            int CurrentYear = DateTime.Now.Year;
            int yearstart = CurrentYear - 5;
            List<SelectListItem> ddlYears = new List<SelectListItem>();

            try
            {
                for (int i = yearstart; i <= CurrentYear; i++)
                {
                    ddlYears.Add(new SelectListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {

            }
            return new SelectList(ddlYears, "Value", "Text", iSelectedYear);
        }


        public string InsertUserLeaveApplication(Leave objL)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                //SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                SqlCommand cmd = new SqlCommand("Sp_Insert_Leave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "14");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@ActivityType", objL.ActivityType);
                cmd.Parameters.AddWithValue("@Reason", objL.Reason);
                cmd.Parameters.AddWithValue("@ChkLTILeave", objL.ChkLTILeave);
               cmd.Parameters.AddWithValue("@Id", objL.Id);
                cmd.Parameters.AddWithValue("@UniversalID", objL.UniversalID);
                cmd.Parameters.AddWithValue("@uniqueNumber", objL.uniqueNumber);

                cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = cmd.ExecuteNonQuery().ToString();
                 objL.Id = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value.ToString());


                string EnquiryID = Convert.ToString(cmd.Parameters["@ReturnId"].Value);
                System.Web.HttpContext.Current.Session["Id"] = EnquiryID;
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

        public string InsertUserLeaveApplicationHalfday(Leave objL)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                //SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                SqlCommand cmd = new SqlCommand("Sp_Insert_Leave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "15");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@ActivityType", objL.ActivityType);
                cmd.Parameters.AddWithValue("@Reason", objL.Reason);
                cmd.Parameters.AddWithValue("@ChkLTILeave", objL.ChkLTILeave);
                cmd.Parameters.AddWithValue("@Id", objL.Id);
                cmd.Parameters.AddWithValue("@UniversalID", objL.UniversalID);
                cmd.Parameters.AddWithValue("@uniqueNumber", objL.uniqueNumber);

                cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = cmd.ExecuteNonQuery().ToString();
                objL.Id = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value.ToString());


                string EnquiryID = Convert.ToString(cmd.Parameters["@ReturnId"].Value);
                System.Web.HttpContext.Current.Session["Id"] = EnquiryID;
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
        public DataSet GetUserLeaveApplication(string ID)
        {
            
DataSet ds = new DataSet();
            string userid = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                //SqlCommand cmd = new SqlCommand("Sp_vaibhav", con);
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "15");
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                //cmd.Parameters.AddWithValue("ID", Convert.ToInt32(ID));
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

        public DataSet EditUserLeaveApplication(string id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3");
                cmd.Parameters.AddWithValue("@UniversalID", id);
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

        public string DeleteUserLeaveApplication(string id)

        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@UniversalID", id);
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

        public DataSet ValidateLeaveApplicationDate(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }
        public DataSet CheckCallMasterDetail(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@startdate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@enddate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }
        public DataSet WeeklyoffCount(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }

        public DataSet LeaveValidate(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }

        public DataSet OnlyLeaveValidate(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9N");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }

        public DataSet GetEmployeeDataSheet()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataSet GetEmployeeDataSheetAll()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "8");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public string ChangeApproval(string ID, string comment)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 2);
                CMDStatus.Parameters.AddWithValue("@universalID", ID);
                CMDStatus.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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
        public string RejectApprover(string ID, string comment)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 3);
                CMDStatus.Parameters.AddWithValue("@universalID", ID);
                CMDStatus.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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

        public DataTable GetApprovalRejectEmployeeDataSheet()
        {
            DataTable dt = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }

        public string MoveToApprovalList(string ID, string comment)
        {
            string Result = string.Empty;
            try
            {
                SqlCommand CMDStatus = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                CMDStatus.CommandType = CommandType.StoredProcedure;
                CMDStatus.Parameters.AddWithValue("@SP_Type", 5);
                CMDStatus.Parameters.AddWithValue("@universalID", ID);
                CMDStatus.Parameters.AddWithValue("@comment", comment);
                con.Open();
                Result = CMDStatus.ExecuteNonQuery().ToString();
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

        public List<BranchMasters> GetBranchList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchMasters> lstEnquiryDashB = new List<BranchMasters>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 5);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new BranchMasters
                           {
                               Br_Id = Convert.ToInt32(dr["Br_Id"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataSet GetAllddlLst()//Binding All Dropdownlist
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 57);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        public DataTable GetClusterHead(string FirstName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("Sp_GetEmployeeLeaveDetail", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 6);
                CMDSearchNameCode.Parameters.AddWithValue("@EmployeeName", FirstName);

                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;

        }

        public string InsertApprovalList(Leave ObjLeave)
        {
            string result = string.Empty;
 
            con.Open();
            
                try
                {
                    SqlCommand cmd = new SqlCommand("Sp_InsertApprovalList", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@BRID", ObjLeave.BranchID);
                    cmd.Parameters.AddWithValue("@OBSID", ObjLeave.ProjectType);
                    cmd.Parameters.AddWithValue("@CH", ObjLeave.CH);
                    cmd.Parameters.AddWithValue("@PCH", ObjLeave.PCHName);
                cmd.Parameters.AddWithValue("@ApprovalName1", ObjLeave.Approver1);
                cmd.Parameters.AddWithValue("@ApprovalName2", ObjLeave.Approver2);
                cmd.Parameters.AddWithValue("@BranchQA", ObjLeave.BranchQA);
                cmd.Parameters.AddWithValue("@AdminQA", ObjLeave.AdminQA);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                 
                result = cmd.ExecuteNonQuery().ToString();

            }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            return result;

        }
        public DataTable EditUploadedFile(string  EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("Sp_InsertUserLeave", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditUploadedFile.Parameters.AddWithValue("@UniversalID", EQ_ID);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public DataTable EditUploadedFile1(string EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("Sp_InsertUserLeave", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditUploadedFile.Parameters.AddWithValue("@UniversalID", EQ_ID);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
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

        public DataSet GetLeaveApprovalDashboard()
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertApprovalList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
               
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

        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, string EQ_ID ,string Type,int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_UniversalID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("TableType", typeof(string)));
                
                foreach (var item in lstFileUploaded)
                {
                    item.FileName = Convert.ToString(ID) + '_' + item.FileName;
                    DTUploadFile.Rows.Add(EQ_ID, null, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent ,Type);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("Sp_InsertUserLeave", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 6);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@UniversalID", EQ_ID);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListEnquiryUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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

        public DataTable GetFileContent(string EQ_ID, string TableType)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("Sp_GetEmployeeLeaveDetail ", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;

                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditUploadedFile.Parameters.AddWithValue("@universalID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@Attachment", TableType);
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

        public DataSet HolidayCount(Leave objL)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "16");
                cmd.Parameters.AddWithValue("@startdate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@enddate", DateTime.ParseExact(objL.ToD, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }

        public DataSet GetLeaveDetails(int LeaveID)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
                cmd.Parameters.AddWithValue("@id", LeaveID);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }

        public DataSet GetApproverDetails(string LeaveID)
        {
            DataSet ds = new DataSet();
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertUserLeave", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
                cmd.Parameters.AddWithValue("@UniversalID", LeaveID);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

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
            return ds;
        }


        //added by nikita on 23052024
        public string InsertUserLeaveForAbscond(Leave objL)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Abscond", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "15");
                cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(objL.StartDate, "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@ActivityType", objL.ActivityType);
                cmd.Parameters.AddWithValue("@USERID", objL.FirstName);
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
        public DataTable GetSatusForMail(string firstname)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("sp_Abscond", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 16);
                CMDEditUploadedFile.Parameters.AddWithValue("@USERID", firstname);
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


        public DataSet GetDataByDate_(Leave LD)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Abscond", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
                cmd.Parameters.AddWithValue("@DateF", DateTime.ParseExact(DateTime.ParseExact(LD.FromD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(LD.ToD, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataSet GetAbscondDashboard()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Abscond", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "18");
                //cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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