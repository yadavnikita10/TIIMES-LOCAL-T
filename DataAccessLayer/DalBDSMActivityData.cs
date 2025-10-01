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
    public class DalBDSMActivityData
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);



        //public DataTable GetData()
        //{
        //    DataTable ds = new DataTable();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "1");

        //        cmd.CommandTimeout = 1000000;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //    return ds;
        //}

//added by nikita on 09092023
        public List<BDSMActivity> GetData()
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BDSMActivity> lstEnquiryDashB = new List<BDSMActivity>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_BdsmActivity", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SpType", 1);
                //CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new BDSMActivity
                           {
                               Count = DTEMDashBoard.Rows.Count,
                               EmployeeName = Convert.ToString(dr["EmployeeName"]),
                               HREmployeeCode = Convert.ToString(dr["HREmployeeCode"]),
                               SAPEmpCode = Convert.ToString(dr["SAPEmpCode"]),
                               EmployeeCategory = Convert.ToString(dr["EmployeeCategory"]),
                               Tuv_Email_Id = Convert.ToString(dr["Tuv_Email_Id"]),
                               MobileNo = Convert.ToString(dr["MobileNo"]),
                               Designation = Convert.ToString(dr["Designation"]),
                               CostCenter = Convert.ToString(dr["Cost_Center"]),
                               Id = Convert.ToString(dr["Id"]),
                               TravelTime = Convert.ToString(dr["TravelTime"]),
                               ActivityType = Convert.ToString(dr["ActivityType"]),
                               ActivityDate = Convert.ToString(dr["ActivityDate"]),
                               StartTime = Convert.ToString(dr["StartTime"]),
                               EndTime = Convert.ToString(dr["EndTime"]),
                               Location = Convert.ToString(dr["Location"]),
                               New_ExistingCustomer = Convert.ToString(dr["New_ExistingCustomer"]),
                               Dom_Inter_Visit = Convert.ToString(dr["Dom_Inter_Visit"]),
                               Description = Convert.ToString(dr["Description"]),
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

        public DataTable BdsmActivityDataSearch(string fromDate, string ToDate)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "30");
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                cmd.CommandTimeout = 1000000;
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


        //end of code











        public DataTable Piechart()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "2");
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
        //public DataTable Data()
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "3");

        //        //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}

        public DataTable Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "3");
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
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

        //public DataTable BindData()
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "12");
                
        //        //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}


        public DataTable BindData(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "12");
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

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


        public DataTable NewExistingPiechart(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "5");
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
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
        //public DataTable NewExistingPiechart()
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "5");
              
        //        //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable DomesticInternationalPiechart(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "6");
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
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
        //public DataTable DomesticInternationalPiechart()
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "6");
               
        //        //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}

        public DataTable BindDataTotext()
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "4");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        //public DataTable BindEmpWiseFiltereddata(string PK_UserID)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "13");
        //        cmd.Parameters.AddWithValue("@PK_UserId", PK_UserID);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable BindEmpWiseFiltereddata(string PK_UserID,string Fromdate,string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "13");
                cmd.Parameters.AddWithValue("@PK_UserId", PK_UserID);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable BranchData(int BranchId,string Fromdate,string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "8");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        //public DataTable GetBranch_EmployeeWise(string PK_UserId)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "9");
        //        cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable GetBranch_EmployeeWise(string PK_UserId, string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "9");
                cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable Bind_Employee_Pie(string PK_UserId,string Fromdate,string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "10");
                cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }


        //public DataTable Bind_Employee_Pie(string PK_UserId)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "10");
        //        cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable Bind_NEwExitFiltereData_Pie(string PK_UserId,string Fromdate,string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "14");
                cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        //public DataTable Bind_NEwExitFiltereData_Pie(string PK_UserId)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "14");
        //        cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable Bind_Dom_Inter_FiltereData_Pie(string PK_UserId,string Fromdate,string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "15");
                cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        //public DataTable Bind_Dom_Inter_FiltereData_Pie(string PK_UserId)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "15");
        //        cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable From_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "16");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable Bind_data_From_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "21");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable BranchFilteringCountdata(int BranchId, string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "17");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        //public DataTable BranchFilteringCountdata(int BranchId)
        //{
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@SpType", "17");
        //        cmd.Parameters.AddWithValue("@BranchId", BranchId);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        string error = ex.Message;
        //    }
        //    return dt;

        //}
        public DataTable Pie_Bind_data_From_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "18");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Pie_New_Existing_data_From_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "20");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Pie_Dom_Inter_From_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "19");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }


        public DataTable Bind_Filter_Branch_dataFrom_ToDate_Data(string Fromdate, string ToDate)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "22");
                //cmd.Parameters.AddWithValue("@PK_UserId", PK_UserId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }

        public DataTable Bind_Branch_OnClick_Piedata(string Fromdate, string ToDate,int BranchId)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "23");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Bind_Branch_OnClick_Exist_New_data(string Fromdate, string ToDate, int BranchId)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "24");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return dt;

        }
        public DataTable Bind_Branch_OnClick_Dom_Inter_New_data(string Fromdate, string ToDate, int BranchId)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BdsmActivity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SpType", "25");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

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

