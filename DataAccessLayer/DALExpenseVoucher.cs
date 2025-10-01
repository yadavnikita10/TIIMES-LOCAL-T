using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    
    public class DALExpenseVoucher
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataSet GetDataById(ExpenseItem objEIModel)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
                
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

        public DataSet GetDataByDate(ExpenseItem objEIModel)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "6");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objEIModel.FromDate, "dd/MM/yyyy", theCultureInfo).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objEIModel.ToDate, "dd/MM/yyyy", theCultureInfo).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public string GenerateExpenseVoucher(ExpenseItem E)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                
                    SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '7');
                    cmd.Parameters.AddWithValue("@PKExpenseId", E.PKExpenseId);
                    
                    
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


        public DataSet GetVoucherStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@SP_Type", "8A");
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

        public DataSet Voucher(string FKExpenseId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@FKExpenseId", FKExpenseId);
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

        public string InsertToVoucher(string CommaSepPKExpenseId,Double VoucherTotalAmount)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '9');
                cmd.Parameters.AddWithValue("@FKExpenseId", CommaSepPKExpenseId);
                cmd.Parameters.AddWithValue("@TotalAmount", VoucherTotalAmount);
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

        //Added By shrutika salve on 12/06/2023

        public DataSet Data(string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_test", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                //cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@CreatedBy", UserID);
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

        public DataSet VoucherData(string FKExpenseId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Voucher", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", '8');
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@FKExpenseId", FKExpenseId);
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

        //Added By Satish Pawar on 23 June 2023
        public DataSet GetDataForPdf(string VoucherNo)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Voucher_Export", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
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

        //added by nikita on 30112023


        public DataSet GetSapData(DataSet data)
        {

            DataSet DsVoucher = new DataSet();
            string Result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Sap_Download", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", data.GetXml());
                cmd.Parameters.AddWithValue("@createdBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                DsVoucher.Dispose();
            }
            return DsVoucher;

        }
        //added by nikita on 21012024
        public DataSet Getuservoucherlist(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetUserVoucherList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        //added by nikita on 21032024
        public DataTable Get_VoucherDetails_History_ID_Description_pdf(string VoucherNo)
        {
            DataTable DsVoucher = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("GenerateDescriptionOnPdf", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);                
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }


        public DataSet GetSapDataForEmpanell(DataSet data)
        {
            DataSet DsVoucher = new DataSet();
            string Result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Sap_Download_Empanell", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", data.GetXml());
                cmd.Parameters.AddWithValue("@createdBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
    }
}