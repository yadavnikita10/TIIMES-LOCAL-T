using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using TuvVision.Models;

using System.Linq;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class DalTravelExpensedashboard
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        public DataTable Piechart()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "1");
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable ExpensePiechart()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "2");
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable FilterExpensePiechart(string costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "3");
                cmd.Parameters.AddWithValue("@Costcenter", costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }


        public DataTable BranchPiechart()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "4");
                //cmd.Parameters.AddWithValue("@Costcenter", costcenter);

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Employeedata()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "5");

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Employeementpiedata()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "6");

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable Typepiedata()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "7");

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable ToatlData()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "8");

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable FilterFirsttable(string Costcenter,string Fromdate,string Todate,string EmployeementCategoray,string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "9");
                cmd.Parameters.AddWithValue("@Costcenter",Costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                //cmd.Parameters.AddWithValue("@Pk_UserId", Pk_UserId);

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable FilterSecondtable(string Costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "10");
                cmd.Parameters.AddWithValue("@Costcenter", Costcenter);
                //cmd.Parameters.AddWithValue("@Costcenter", Costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable FilterEmplyoment(string Costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "11");
                cmd.Parameters.AddWithValue("@Costcenter", Costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable FilterTypePie(string Costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "12");
                cmd.Parameters.AddWithValue("@Costcenter", Costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable filtercostcenteronEmployeeClick(string Costcenter,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "14");
                cmd.Parameters.AddWithValue("@Pk_UserId", Costcenter);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable filterEmployeeExpenseTypeonEmployeeClick(string Pk_userid,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "15");
                cmd.Parameters.AddWithValue("@Pk_UserId", Pk_userid);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable filterEmployeeTypeonEmployeeClick(string Pk_userid, string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "16");
                cmd.Parameters.AddWithValue("@Pk_UserId", Pk_userid);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        

        public DataTable filterEmployeementTypeonEmployeeClick(string Pk_userid,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "17");
                cmd.Parameters.AddWithValue("@Pk_UserId", Pk_userid);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable filterCostcenteronBranchClick(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "18");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        } 

         public DataTable filterEmployeeonBranchClick(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "19");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable filterTotalAmountonBranchClick(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "20");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable filterExpenseBranchClick_(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "23");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        public DataTable filterEmployementBranchClick_(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "22");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable filterTypeBranchClick_(string Branchcode,string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "21");
                cmd.Parameters.AddWithValue("@BranchCode", Branchcode);
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        //15102023
        public DataTable filterBranchTableOnClick(string Pk_userid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "24");
                cmd.Parameters.AddWithValue("@Pk_UserId", Pk_userid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataSet MultiEmployeeDropdown()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "26");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return ds;
        }

        public DataSet UserRole_()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "27");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return ds;
        }
        public DataTable Datewisecostercenter(string Fromdate,string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "28");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable DatewiseBranch(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "29");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable DatewiseEmployee(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "30");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        public DataTable DatewiseExpense(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "32");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        public DataTable DatewiseType(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "33");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        public DataTable DatewiseEmployeement(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "34");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

        public DataTable DatewiseTotal(string Fromdate, string Todate, string EmployeementCategoray, string UserRole)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpenseDashboard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "31");
                cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                cmd.Parameters.AddWithValue("@todate", Todate);
                cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }



        public DataTable TravelExpese_obswise()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("TravelExpenseObsWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "1");
                //cmd.Parameters.AddWithValue("@fromdate", Fromdate);
                //cmd.Parameters.AddWithValue("@todate", Todate);
                //cmd.Parameters.AddWithValue("@Employement", EmployeementCategoray);
                //cmd.Parameters.AddWithValue("@Role", UserRole);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }
        public DataTable TravelExpese_obswise_Datewise(Travel T)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("TravelExpenseObsWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", "2");
                cmd.Parameters.AddWithValue("@fromdate", T.Fromd);
                cmd.Parameters.AddWithValue("@todate", T.Tod);
                cmd.Parameters.AddWithValue("@Employement", T.UserRole);
                cmd.Parameters.AddWithValue("@Role", T.EmpCategoray);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;
        }

    }
}