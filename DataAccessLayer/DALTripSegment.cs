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
    public class DALTripSegment
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public string Insert(TripSegment TS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (TS.PKExpenseId > 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@City", TS.StartCity);
                    cmd.Parameters.AddWithValue("@EndCity", TS.EndCity);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(TS.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@ExpenseType", TS.ExpenseType);
                    cmd.Parameters.AddWithValue("@Kilometer", TS.Kilometer);
                    cmd.Parameters.AddWithValue("@Description", TS.Description);
                    cmd.Parameters.AddWithValue("@TotalAmount", TS.TotalAmount);
                    cmd.Parameters.AddWithValue("@FKId", TS.FKId);
                    cmd.Parameters.AddWithValue("@Type", TS.Type);
                    cmd.Parameters.AddWithValue("@PKExpenseId", TS.PKExpenseId);
                    cmd.Parameters.AddWithValue("@SubJobNo", TS.SubJobNo);
                    
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@City", TS.StartCity);
                    cmd.Parameters.AddWithValue("@EndCity", TS.EndCity);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(TS.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@ExpenseType", TS.ExpenseType);
                    cmd.Parameters.AddWithValue("@Kilometer", TS.Kilometer);
                    cmd.Parameters.AddWithValue("@Description", TS.Description);
                    cmd.Parameters.AddWithValue("@TotalAmount", TS.TotalAmount);
                    cmd.Parameters.AddWithValue("@FKId", TS.FKId);
                    cmd.Parameters.AddWithValue("@Type", TS.Type);
                    cmd.Parameters.AddWithValue("@SubJobNo", TS.SubJobNo);
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

        public DataSet GetEmployeeGrade()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetTripSegment(TripSegment objTS)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@FKId", objTS.FKId);
                cmd.Parameters.AddWithValue("@Type", objTS.Type);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetDataById(int PKExpenseId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@PKExpenseId", PKExpenseId);
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

        public string Delete(int PKExpenseId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_TripSegment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PKExpenseId", PKExpenseId);
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

    }
}