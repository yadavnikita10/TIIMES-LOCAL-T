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
    public class DALMISOPEReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        //public DataSet GetData(MISOPEReport objMOR)
        public DataSet GetData(string Month, string Year)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");

                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Year", Year);


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

        public DataSet GetOPEAPPData(MISOPEReport objMOR)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1A");
                
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(objMOR.FromDate, "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(objMOR.ToDate, "dd/MM/yyyy", null));
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet GetOPEUserData(MISOPEReport objMOR,string user)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "6");

                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@User", user);
                
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

        public DataSet JOBWiseTimeSheet()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "2");
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
        //Getting Record by From Date And To Date, Code By Manoj Sharma 17 Dec 2019
        public DataSet JOBWiseTimeSheet(string FromDate, string ToDate)
        {
            DataSet DSGetRecordByDatewise = new DataSet();
            try
            {
                SqlCommand CMDCallList = new SqlCommand("SP_MISOPEReport", con);
                CMDCallList.CommandType = CommandType.StoredProcedure;
                CMDCallList.Parameters.AddWithValue("@SP_Type", "5");
                CMDCallList.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDCallList.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDCallList.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);

                SqlDataAdapter ad = new SqlDataAdapter(CMDCallList);
                ad.Fill(DSGetRecordByDatewise);


            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetRecordByDatewise.Dispose();
            }
            return DSGetRecordByDatewise;
        }
        public DataSet CallRegister()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "3");
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

        public DataSet ProjectStatus()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "8");
                cmd.CommandTimeout = 30000;
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

        public DataSet ProjectStatus(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet DebitCredit()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.CommandTimeout = 30000;
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

        public DataSet DebitCredit(string FromDate, string ToDate)
        {
            DataSet dsDebit = new DataSet();
            try
            {
                SqlCommand cmdDebit = new SqlCommand("SP_MISOPEReport", con);
                cmdDebit.CommandType = CommandType.StoredProcedure;
                // cmdDebit.Parameters.AddWithValue("@SP_Type", "11");
                cmdDebit.Parameters.AddWithValue("@SP_Type", "31");
                cmdDebit.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmdDebit.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmdDebit.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter dadebit = new SqlDataAdapter(cmdDebit);
                dadebit.Fill(dsDebit);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                 dsDebit.Dispose();
            }
            return dsDebit;
        }


        public DataSet CallRegister(string FromDate, string ToDate)//Getting Record By Code By Manoj Sharma 17 Dec 2019 Searching Record by From Date And To Date
        {

            DataSet DSGetListByParameter = new DataSet();
            try
            {
                //string _date1 = string.Empty;
                //string _date2 = string.Empty;
                //string[] FromDt = FromDate.Split('/');
                //string[] ToDt = ToDate.Split('/');
                //_date1 = FromDt[2] + "-" + FromDt[1] + "-" + FromDt[0];
                //_date2 = ToDt[2] + "-" + ToDt[1] + "-" + ToDt[0];

                SqlCommand CMDGetListByParameter = new SqlCommand("SP_MISOPEReport", con);
                CMDGetListByParameter.CommandType = CommandType.StoredProcedure;
                CMDGetListByParameter.Parameters.AddWithValue("@SP_Type",4);
                CMDGetListByParameter.Parameters.AddWithValue("@DateFrom", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetListByParameter.Parameters.AddWithValue("@DateTo", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetListByParameter.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAetListByParameter = new SqlDataAdapter(CMDGetListByParameter);
                SDAetListByParameter.Fill(DSGetListByParameter);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetListByParameter.Dispose();
            }
            return DSGetListByParameter;
        }

        public DataSet TOYOMIS()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "17");
                cmd.CommandTimeout = 30000;
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

        public DataSet TOYOMIS(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "16");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
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

        //Added By Satish Pawar

        public DataSet GetOPApprovalList(string UserId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_ApprovalListData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "23");
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserId", UserId);               
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

        public DataSet SendForApproval(string OP_Number)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "22");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
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

        public DataSet GetOPBulk_ApprovalList(DataSet dsBulkApprove, string UserId,string OP_Number)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_BulUpload", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", "24");
                cmd.Parameters.AddWithValue("@xmlData", dsBulkApprove.GetXml());
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(objMOR.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserId", UserId);



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

        public DataSet GetOPHistoryList(string OP_Number)
        {
            DataSet ds1 = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds1);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds1.Dispose();
            }
            return ds1;
        }

        public DataSet OPE_Data(string OPE_Number)
        {
            DataSet ds1 = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_Op_ExcelData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "26");
                cmd.Parameters.AddWithValue("@OP_Number", OPE_Number);
                //cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds1);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds1.Dispose();
            }
            return ds1;
        }

        public DataTable OPESummary(string strMonth, string stryear)
        {
            DataTable ds1 = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "28");
                cmd.Parameters.AddWithValue("@Month", strMonth);
                cmd.Parameters.AddWithValue("@Year", stryear);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds1);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds1.Dispose();
            }
            return ds1;
        }

        public DataTable GetYear()
        {
            List<int> years = new List<int>();


            int currentYear = DateTime.Now.Year;
            int startYear = DateTime.Today.Year - 5;
            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("fYear");

            try
            {

                for (int year = startYear; year <= currentYear; year++)
                {
                    DataRow dr = dtyear.NewRow();
                    dr[0] = year.ToString();

                    dtyear.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                years = null;
            }
            finally
            {

            }
            return dtyear;
        }

        public DataSet DebitCreditSummary()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@SP_Type", "29");
                cmd.CommandTimeout = 30000;
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



        public DataSet DebitCreditSummaryDate(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "30");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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
        //added by nikita on 20122023
        public DataSet GetAccountantExportData(string op_number)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9A");
                cmd.Parameters.AddWithValue("@OP_Number", op_number);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 600;
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
        //added by nikita on 25122023

        public DataSet GetOPList_Approved(string UserId, string Userrole)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetOpApprovalList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Userrole);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
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

        public DataSet BindOp_description(string Op_Number, string UserId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp__op_History_BindDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VoucherNo", Op_Number);
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
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
        //added by nikita on 02012024
        public DataSet GetOPApprovalList_Finance(string UserId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "Finance");
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


        public DataSet MismatchReport()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@SP_Type", "32");
                cmd.CommandTimeout = 30000;
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



        public DataSet MismatchReportDate(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "33");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet IncorrectSalesOrder()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@SP_Type", "34");
                cmd.CommandTimeout = 30000;
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



        public DataSet IncorrectSalesOrderDate(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "35");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet MultipleCostCentre()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 30000;
                cmd.Parameters.AddWithValue("@SP_Type", "36");
                cmd.CommandTimeout = 30000;
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



        public DataSet MultipleCostCentreDate(string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "36");
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet GetOPApprovalListButtonwise(string Branch)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_ApprovalListDataButtonwise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "23");
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                cmd.Parameters.AddWithValue("@BranchOpnumber", Branch);

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
        public DataTable valiadateAprroval(string OP_Number)
        {
            DataTable ds1 = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_OP_Data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds1);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds1.Dispose();
            }
            return ds1;
        }
        public DataSet GetDatewiseApprovedlist(string fromdate, string Todate, string Role, string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetOpApprovalList_DateWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@Todate", DateTime.ParseExact(DateTime.ParseExact(Todate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromdate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@Todate", DateTime.ParseExact(DateTime.ParseExact(Todate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@FromDate", fromdate);// DateTime.ParseExact(DateTime.ParseExact(fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@Todate", Todate);//DateTime.ParseExact(DateTime.ParseExact(Todate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));

                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.CommandTimeout = 100000;
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

        public DataSet OP_DelayRemark(string OP_Number, string Remarks)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "38");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
                cmd.Parameters.AddWithValue("@DelayRemarks", Remarks);
                cmd.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataSet Op_delayFlag(string OP_Number)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "39");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
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
        public DataSet GetRemark(string OP_Number)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_MISOPEReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "40");
                cmd.Parameters.AddWithValue("@OP_Number", OP_Number);
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


        public DataSet GetMRNData()
        {
            DataSet ds1 = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_GetMRNData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", "26");
                //cmd.Parameters.AddWithValue("@OP_Number", OPE_Number);
                //cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds1);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds1.Dispose();
            }
            return ds1;
        }
    }
}