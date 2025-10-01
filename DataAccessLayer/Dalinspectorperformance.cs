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
    public class Dalinspectorperformance
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable GetData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_inspectorPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
                cmd.CommandTimeout = 1000000;
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

        //added by shrutika salve 21102023

        public DataTable GetIRNData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_IRNInspectorPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
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

        public DataTable GetNCRData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_NCRInspectorPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDate);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToDate);
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

        //added by shrutika salve 21102023

        public DataTable GetMonitoringData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_MonitoringDetails_inspectorPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.fromD);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToD);
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


        public DataTable GetTsfilledData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TSFilledOperation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.fromD);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToD);
                cmd.Parameters.AddWithValue("@UserRole", CM.UserRole);
                cmd.Parameters.AddWithValue("@EmployeementCategory", CM.Id);
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

        public List<InspectorPerformance> EmployementCategory()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMuserEmployee = new DataTable();
            List<InspectorPerformance> lstGetuserEmploye = new List<InspectorPerformance>();
            try
            {
                SqlCommand CMDgetEmployement = new SqlCommand("Sp_UtilisationOfManpower_Alldata", con);
                CMDgetEmployement.CommandType = CommandType.StoredProcedure;
                CMDgetEmployement.CommandTimeout = 1000000;
                CMDgetEmployement.Parameters.AddWithValue("@SP_Type", 2);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDgetEmployement);
                SDAGetEnquiry.Fill(DTEMuserEmployee);
                if (DTEMuserEmployee.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMuserEmployee.Rows)
                    {
                        lstGetuserEmploye.Add(
                           new InspectorPerformance
                           {
                               Id = Convert.ToString(dr["id"]),
                               EmployementCategory = Convert.ToString(dr["Name"]),
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
                DTEMuserEmployee.Dispose();
            }
            return lstGetuserEmploye;
        }

        public DataSet BindAuditorName()
        {
            DataSet dsAuditorName = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("Sp_UtilisationOfManpower_Alldata", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(dsAuditorName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsAuditorName.Dispose();
            }
            return dsAuditorName;

        }

        public DataSet Employeecategory()
        {
            DataSet dsAuditorName = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("Sp_UtilisationOfManpower_Alldata", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 2);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(dsAuditorName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsAuditorName.Dispose();
            }
            return dsAuditorName;

        }

        //added  by shrutika salve 30102023

        public DataTable GetFinalinspectorData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_ALLFinalinspectorPerformanceALLFinal", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromD", CM.fromD);
                cmd.Parameters.AddWithValue("@ToD", CM.ToD);
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        //added  by shrutika salve 30102023

        public DataTable GetTraningData(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TrainingCount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", CM.fromD);
                cmd.Parameters.AddWithValue("@ToDate", CM.ToD);
                // cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        //added by shrutika salve 06122023
        public DataTable GetLessionLearnt(InspectorPerformance CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_lessionLearnt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Fromdate", CM.fromD);
                cmd.Parameters.AddWithValue("@Todate", CM.ToD);
                // cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        //added by shrutika salve 06122023
        public DataTable Userdetails()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ProfileUpdatePerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        public DataTable InspectorWorkload(InspectorWorkload Iw)
        {
            DataTable ds = new DataTable();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime FromDate = DateTime.ParseExact(Iw.Fromdate, "dd/MM/yyyy", provider);
                string formattedFromDateStr = FromDate.ToString("dd-MMM-yyyy");
                DateTime TODate = DateTime.ParseExact(Iw.Todate, "dd/MM/yyyy", provider);
                string formattedToDateStr = TODate.ToString("dd-MMM-yyyy");

                SqlCommand cmd = new SqlCommand("SP_WorkLoad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
                cmd.Parameters.AddWithValue("@FromDate", formattedFromDateStr);
                cmd.Parameters.AddWithValue("@ToDate", formattedToDateStr);

                // cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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


        public DataTable InspectorWorkloadBranchWise(InspectorWorkload Iw)
        {
            DataTable ds = new DataTable();
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                DateTime FromDate = DateTime.ParseExact(Iw.Fromdate, "dd/MM/yyyy", provider);
                string formattedFromDateStr = FromDate.ToString("dd-MMM-yyyy");
                DateTime TODate = DateTime.ParseExact(Iw.Todate, "dd/MM/yyyy", provider);
                string formattedToDateStr = TODate.ToString("dd-MMM-yyyy");

                SqlCommand cmd = new SqlCommand("SP_WorkLoad", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@FromDate", formattedFromDateStr);
                cmd.Parameters.AddWithValue("@ToDate", formattedToDateStr);

                // cmd.Parameters.AddWithValue("@UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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