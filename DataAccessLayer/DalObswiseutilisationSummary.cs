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
    public class DalObswiseutilisationSummary
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public List<ObswiseUtilisationSummary> UserRoleget()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMuserRole = new DataTable();
            List<ObswiseUtilisationSummary> lstGetuserRole = new List<ObswiseUtilisationSummary>();
            try
            {
                SqlCommand CMDgetUserRole = new SqlCommand("Sp_UtilisationOfManpower_Alldata", con);
                CMDgetUserRole.CommandType = CommandType.StoredProcedure;
                CMDgetUserRole.CommandTimeout = 1000000;
                CMDgetUserRole.Parameters.AddWithValue("@SP_Type", 1);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDgetUserRole);
                SDAGetEnquiry.Fill(DTEMuserRole);
                if (DTEMuserRole.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMuserRole.Rows)
                    {
                        lstGetuserRole.Add(
                           new ObswiseUtilisationSummary
                           {
                               UserRole = Convert.ToString(dr["UserRoleID"]),
                               RoleName = Convert.ToString(dr["RoleName"]),
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
                DTEMuserRole.Dispose();
            }
            return lstGetuserRole;
        }


        

        public List<ObswiseUtilisationSummary> EmployementCategory()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMuserEmployee = new DataTable();
            List<ObswiseUtilisationSummary> lstGetuserEmploye = new List<ObswiseUtilisationSummary>();
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
                           new ObswiseUtilisationSummary
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


        //added by shrutika salve 27/07/2023
        public DataTable OBSWise(ObswiseUtilisationSummary CM)
        {
            DataTable ds = new DataTable();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

            

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UtilisationOfManpower", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", CM.FromDate);
                cmd.Parameters.AddWithValue("@EndDate", CM.ToDate);
                //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@Userole", CM.UserRole);
                cmd.Parameters.AddWithValue("@Employee",CM.Id);
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


        public DataSet GetAdminAttendanceSheet(ObswiseUtilisationSummary n)
        {
            DataSet ds = new DataSet();
            CultureInfo provider = CultureInfo.InvariantCulture;
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                


                // Parse the input date string


                // Format the parsed date into the desired output format

                SqlCommand cmd = new SqlCommand("SP_Attendance_OBSUlitisation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Sp_Type", '2');
                //cmd.Parameters.AddWithValue("@StartDate", DateTime.ParseExact(DateTime.ParseExact(n.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@EndDate", DateTime.ParseExact(DateTime.ParseExact(n.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@StartDate", n.FromDate);
                cmd.Parameters.AddWithValue("@EndDate", n.ToDate);
                cmd.Parameters.AddWithValue("@UserRole", n.UserRole);
                cmd.Parameters.AddWithValue("@EmployeementCategory",n.Id);
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

    }
}