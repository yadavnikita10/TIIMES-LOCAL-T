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
    public class DalCallHistory
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

        public DataTable GetReportsDashBoard() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_Call", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                CMDReportDash.CommandTimeout = 100000;
                //CMDReportDash.Parameters.AddWithValue("@SP_Type", "13");
                CMDReportDash.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        public DataTable GetReports(CallsModel CM) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_CallsearchData", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                CMDReportDash.CommandTimeout = 100000;
                //CMDReportDash.Parameters.AddWithValue("@SP_Type", "13");
                CMDReportDash.Parameters.AddWithValue("@DateF", Convert.ToDateTime(CM.FromDate));
                CMDReportDash.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(CM.ToDate));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        public DataTable GetCallDetailsData(int refId) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_Call_NumberHistory", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                //CMDReportDash.CommandTimeout = 100000;
                CMDReportDash.Parameters.AddWithValue("@SP_Type", "1");
                CMDReportDash.Parameters.AddWithValue("@refID", refId);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }

        //get IVR Number History
        public DataTable GetIVRDetailsData(int refId) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_Call_NumberHistory", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                //CMDReportDash.CommandTimeout = 100000;
                CMDReportDash.Parameters.AddWithValue("@SP_Type", "2");
                CMDReportDash.Parameters.AddWithValue("@refID", refId);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        //get IVR Number History
        public DataTable GetIRNDetailsData(int refId) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_Call_NumberHistory", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                //CMDReportDash.CommandTimeout = 100000;
                CMDReportDash.Parameters.AddWithValue("@SP_Type", "3");
                CMDReportDash.Parameters.AddWithValue("@refID", refId);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        public DataTable GetData() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDReportDash = new SqlCommand("SP_Call", con);
                //SP_CALLHISTORY
                CMDReportDash.CommandType = CommandType.StoredProcedure;
                CMDReportDash.CommandTimeout = 100000;
                //CMDReportDash.Parameters.AddWithValue("@SP_Type", "13");
                CMDReportDash.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDReportDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }

        //added by shrutika salve (Executive Call Status) 
        public DataSet BindBranch()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.CommandTimeout = 1000000000;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(DTBindBranch);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBindBranch.Dispose();
            }
            return DTBindBranch;

        }

        public DataSet GetUserList()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 9);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }



        public DataSet GetinspectionList()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 10);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }


        public DataSet BranchName(string UserId)
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 8);
                CMDGetDdlLst.Parameters.AddWithValue("@Userid", UserId);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public DataSet GetDataList(string FromDate, string Todate, string Excuting_Branch, string OriginatingBranch, string CoordinatorName, string inspectorname, string status)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@FromDate", FromDate);
                CMDGetDdlLst.Parameters.AddWithValue("@ToDate", Todate);
                CMDGetDdlLst.Parameters.AddWithValue("@inspector", inspectorname);
                CMDGetDdlLst.Parameters.AddWithValue("@Executing_Branch", Excuting_Branch);
                CMDGetDdlLst.Parameters.AddWithValue("@Originating_Branch", OriginatingBranch);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", CoordinatorName);
                CMDGetDdlLst.Parameters.AddWithValue("@Status", status);
                CMDGetDdlLst.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }
        public DataSet GetDatacallsearch(String FromDate, String ToDate, String Executing_Branch, String OriginatingBranch, String CoordinatorName, String inspectorname, String status)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 6);
                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@inspector", inspectorname);
                cmd.Parameters.AddWithValue("@Originating_Branch", OriginatingBranch);
                cmd.Parameters.AddWithValue("@CreatedBy", CoordinatorName);
                cmd.Parameters.AddWithValue("@Executing_Branch", Executing_Branch);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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

        public DataSet Branch(String Branch)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 2);
                cmd.Parameters.AddWithValue("@Executing_Branch", Branch);
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


        public DataSet inspectorName(string Branch)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 7);
                cmd.Parameters.AddWithValue("@Executing_Branch", Branch);
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


        //Added Shrutika salve for invoice data
        public DataTable InvoiceData(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 12);
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@Job", CM.Job);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@inspector", CM.Inspector);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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


        public DataTable InvoiceDataExcel(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 12);
                cmd.Parameters.AddWithValue("@Job", CM.Job);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@inspector", CM.Inspector);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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



        public DataSet GetReportList(string job)//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Executivecallstatus", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 13);
                CMDGetDdlLst.Parameters.AddWithValue("@job", job);

                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return DSGetddlList;
        }


        //added by shrutika salve 27/07/2023
        public DataTable OriginatingBranchSummary(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("OriginatingBranchSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
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


        //added by shrutika salve 28/07/2023
        public DataTable ExecutingBranchSummary(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("executingBranchSummary_shrutika", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
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


        //added by shrutika salve 27/07/2023
        public DataTable CoordinatorIndividual(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("CoordinatorPerformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
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


        //added by shrutika salve 07082023 
        public DataSet BindOriginatingBranch()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("sp_BranchDetails", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.CommandTimeout = 100000;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(DTBindBranch);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBindBranch.Dispose();
            }
            return DTBindBranch;

        }



        public DataSet GetDatadeatils(string Fromdate, string Todate, string OriginatingBranch)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_BranchDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 2);
                cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                cmd.Parameters.AddWithValue("@ToDate", Todate);
                cmd.Parameters.AddWithValue("@Originating_Branch", OriginatingBranch);
                cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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

        //added by shrutika salve 27/07/2023
        public DataTable Branchdata(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_BranchDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 3);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@Originating_Branch", CM.OriginatingBranch);
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


        //added by shrutika salve 12/09/2023
        public DataTable CoordinatorPerformance_ExecutingBranch(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("CoordinatorPerformance_ExecutingBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
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

        //added by nikita on 06092023
        public DataTable GetExpenseDetails()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", 1);
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



        //added by nikita on 06092023 for date Filter
        public DataTable GetExpenseDetailsDatewise(string Fromdate, string ToDate, string SapNumber)
        {
            DataTable ds = new DataTable();
            try
            {
                //SqlCommand cmd = new SqlCommand("Sp_TravelExpense", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Sp_Type", "2");
                ////cmd.Parameters.AddWithValue("@FromDate", Fromdate);
                ////cmd.Parameters.AddWithValue("@ToDate", ToDate);
                //cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(Fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                //cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                ////cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.SelectCommand.CommandTimeout = 100000;
                //da.Fill(ds);
                SqlCommand cmd = new SqlCommand("Sp_TravelExpense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(SapNumber))
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "3");
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(Fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@SAPNumber", SapNumber);
                    cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));

                }
                else if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(ToDate) && string.IsNullOrEmpty(SapNumber))
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "2");
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(Fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));        
                    cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));

                }
                else if (string.IsNullOrEmpty(Fromdate) && string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(SapNumber))
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "4");
                    cmd.Parameters.AddWithValue("@SAPNumber", SapNumber);            
                    cmd.Parameters.AddWithValue("@Userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));

                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 100000;
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

        //added by shrutikasalve 02112023
        public DataTable FinalCoordinatorPerformance(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("finalcoordinatorperformance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@FromD", CM.FromDate);
                cmd.Parameters.AddWithValue("@ToD", CM.ToDate);
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

        //added by shrutikasalve 02112023
        public DataTable EstimatedVSactual(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_EstimatedVSactualManpower", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", CM.FromDate);
                cmd.Parameters.AddWithValue("@EndDate", CM.ToDate);
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

        //added by nikita on 20052024
        public DataTable GetVoucherstatusList()
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("Sp_TravelExpenseVoucherstatus", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }

        public DataTable GetVoucherstatusListDatewise(string FromDate, string ToDate, string voucherno)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                using (SqlCommand CMDGetEnquriy = new SqlCommand("Sp_TravelExpenseVoucherstatus", con))
                {
                    CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                    CMDGetEnquriy.CommandTimeout = 1000000;

                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        if (!string.IsNullOrEmpty(voucherno))
                        {
                            CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 3);
                            CMDGetEnquriy.Parameters.AddWithValue("@Fromdate", FromDate);
                            CMDGetEnquriy.Parameters.AddWithValue("@Todate", ToDate);
                            CMDGetEnquriy.Parameters.AddWithValue("@VoucherNo", voucherno);
                        }
                        else
                        {
                            CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 2);
                            CMDGetEnquriy.Parameters.AddWithValue("@Fromdate", FromDate);
                            CMDGetEnquriy.Parameters.AddWithValue("@Todate", ToDate);
                        }
                    }
                    else if (!string.IsNullOrEmpty(voucherno))
                    {
                        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 4);
                        CMDGetEnquriy.Parameters.AddWithValue("@VoucherNo", voucherno);
                    }
                    else
                    {
                        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 1);
                    }

                    SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                    SDAGetEnquiry.Fill(DTEMDashBoard);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                // Log the error or handle it as necessary
            }
            return DTEMDashBoard;
        }
        public DataTable GetOPEVoucherstatusList()
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetstatus = new SqlCommand("Sp_OPEVoucherstatus", con);
                CMDGetstatus.CommandType = CommandType.StoredProcedure;
                CMDGetstatus.CommandTimeout = 1000000;
                CMDGetstatus.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetstatus);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }


        public DataTable GetOPEVoucherstatusList(string FromDate, string ToDate, string OPENumber)
        {
            DataTable DTEMDashBoard = new DataTable();
            try
            {
                using (SqlCommand CMDGetstatus = new SqlCommand("Sp_OPEVoucherstatus", con))
                {
                    CMDGetstatus.CommandType = CommandType.StoredProcedure;
                    CMDGetstatus.CommandTimeout = 1000000;

                    if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                    {
                        if (!string.IsNullOrEmpty(OPENumber))
                        {
                            CMDGetstatus.Parameters.AddWithValue("@SP_Type", 3);
                            CMDGetstatus.Parameters.AddWithValue("@Fromdate", FromDate);
                            CMDGetstatus.Parameters.AddWithValue("@Todate", ToDate);
                            CMDGetstatus.Parameters.AddWithValue("@OP_Number", OPENumber);
                        }
                        else
                        {
                            CMDGetstatus.Parameters.AddWithValue("@SP_Type", 2);
                            CMDGetstatus.Parameters.AddWithValue("@Fromdate", FromDate);
                            CMDGetstatus.Parameters.AddWithValue("@Todate", ToDate);
                        }
                    }
                    else if (!string.IsNullOrEmpty(OPENumber))
                    {
                        CMDGetstatus.Parameters.AddWithValue("@SP_Type", 4);
                        CMDGetstatus.Parameters.AddWithValue("@OP_Number", OPENumber);
                    }
                    else
                    {
                        CMDGetstatus.Parameters.AddWithValue("@SP_Type", 1);
                    }

                    SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetstatus);
                    SDAGetEnquiry.Fill(DTEMDashBoard);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
        
            }
            return DTEMDashBoard;
        }

        public DataTable ProcessMeasures(ProcessMeasures CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ProcessMeasures", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromD", CM.FromDate);
                cmd.Parameters.AddWithValue("@ToD", CM.ToDate);
                cmd.CommandTimeout = 120;
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
        //added by nikita on 06092023
        public DataTable GetExpenseDetails_Approved()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpense_Approved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Sp_Type", 1);
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



        //added by nikita on 06092023 for date Filter
        public DataTable GetExpenseDetailsDatewise_Approved(string Fromdate, string ToDate, string SAP_No)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_TravelExpense_Approved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(SAP_No))
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "3");
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(Fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    cmd.Parameters.AddWithValue("@SAPNumber", SAP_No);

                }
                else if (!string.IsNullOrEmpty(Fromdate) && !string.IsNullOrEmpty(ToDate))
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "2");
                    cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(Fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                }

                else
                {
                    cmd.Parameters.AddWithValue("@Sp_Type", "4");
                    cmd.Parameters.AddWithValue("@SAPNumber", SAP_No);
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                }

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
        public DataTable InvoiceData_international(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 14);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        //Added Shrutika salve for invoice data
        public DataTable TimeData(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 15);
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@Job", CM.Job);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@inspector", CM.Inspector);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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


        public DataTable TimeDataExcel(CallsModel CM)
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Executivecallstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 15);
                cmd.Parameters.AddWithValue("@Job", CM.Job);
                cmd.Parameters.AddWithValue("@FromDate", CM.FromDateI);
                cmd.Parameters.AddWithValue("@Todate", CM.ToDateI);
                cmd.Parameters.AddWithValue("@inspector", CM.Inspector);
                cmd.Parameters.AddWithValue("@userid", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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