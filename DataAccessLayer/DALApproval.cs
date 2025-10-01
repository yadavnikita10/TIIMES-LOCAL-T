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
    public class DALApproval
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataSet GetDataByDate(Approval objEIModel)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@FromDate", objEIModel.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", objEIModel.ToDate);
                cmd.Parameters.AddWithValue("@Inspector", objEIModel.Inspector);//objEIModel.Inspector);

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

        public DataSet GetDataByID(string PKExpenseId)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");
                cmd.Parameters.AddWithValue("@PKExpenseId", PKExpenseId);//objEIModel.Inspector);

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


        public DataSet GetName()
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
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

        public DataSet GetApprovalDetail(string InspectorId)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@PK_UserID", InspectorId);
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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


        public string ProvideApproval(Approval E,string Person)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@PKExpenseId", E.PKExpenseId);
                cmd.Parameters.AddWithValue("@Person", Person);
                cmd.Parameters.AddWithValue("@Approved", "Approved");
                cmd.Parameters.AddWithValue("@ApprovedAmount", E.AprovelAmount);
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

        public string ProvideVoucherApproval(Approval E, string Person,int VoucherId,Double Sum)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@VoucherId", VoucherId);
                cmd.Parameters.AddWithValue("@Person", Person);
                cmd.Parameters.AddWithValue("@Approved", "Approved");
                cmd.Parameters.AddWithValue("@ApprovedAmount", Sum);
                //cmd.Parameters.AddWithValue("@ApprovedAmount", E.AprovelAmount);
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

        

        public string UpdateTransferToFI(Approval E, string Person)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PKExpenseId", E.PKExpenseId);
                //cmd.Parameters.AddWithValue("@Person", Person);
               // cmd.Parameters.AddWithValue("@Approved", "Approved");
                //cmd.Parameters.AddWithValue("@ApprovedAmount", E.AprovelAmount);
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

        public string UpdateTransferToFIVoucher(Approval E, string Person,int VoucherId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '9');
                cmd.Parameters.AddWithValue("@VoucherId", VoucherId);
                //cmd.Parameters.AddWithValue("@Person", Person);
                // cmd.Parameters.AddWithValue("@Approved", "Approved");
                //cmd.Parameters.AddWithValue("@ApprovedAmount", E.AprovelAmount);
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


        public DataSet GetVoucherByDate(Approval objEIModel)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "6");
               // cmd.Parameters.AddWithValue("@FromDate", objEIModel.FromDate);
               /// cmd.Parameters.AddWithValue("@ToDate", objEIModel.ToDate);
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objEIModel.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objEIModel.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));


                //cmd.Parameters.AddWithValue("@Remarks", objEIModel.Remarks);
                cmd.Parameters.AddWithValue("@Inspector", objEIModel.Inspector);//objEIModel.Inspector);
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


        public string Update(Approval E,string Person)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_Approval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "14");
                cmd.Parameters.AddWithValue("@VoucherId", E.LVoucherId);
                cmd.Parameters.AddWithValue("@Person", Person);
                cmd.Parameters.AddWithValue("@Approved", "Approved");
                cmd.Parameters.AddWithValue("@Remarks1", E.Remarks);
                cmd.Parameters.AddWithValue("@ApprovedAmount", E.ApprovedAmount);

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