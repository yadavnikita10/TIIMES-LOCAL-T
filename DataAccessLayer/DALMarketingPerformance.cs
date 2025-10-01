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
    public class DALMarketingPerformance
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable GetData(MarketingPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MarketingKPI", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                //cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
                cmd.Parameters.AddWithValue("@FromD", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToD", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        public DataTable GetDataKPI(MarketingPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MarketingKPIFinal", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                //cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@FromD", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToD", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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


        public DataTable GetQHSEPerformance(MarketingPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QHSEPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                //cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                cmd.Parameters.AddWithValue("@FromD", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToD", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

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

        public DataTable OperationPerformance(OperationPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_OperationPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                //cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
                cmd.Parameters.AddWithValue("@FromD", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToD", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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


        public DataTable GetBranchPerformance(BranchPerformance BP)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_BranchPerformanceDisplay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@SelectedValue", BP.SelectedValue);
                //cmd.Parameters.AddWithValue("@FromD", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToD", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));



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



        public DataSet GetDates() //--Get All DATA
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_BranchPerformanceDisplay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '2');
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