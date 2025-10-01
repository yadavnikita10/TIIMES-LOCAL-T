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
    public class DALBranchTarget
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);


        public string Insert(BranchTarget objBranchTarget)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (objBranchTarget.Id>0 )//Update
                {

                    SqlCommand cmd = new SqlCommand("SP_Target", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@Id", objBranchTarget.Id);
                    cmd.Parameters.AddWithValue("@Year", objBranchTarget.Year);
                    cmd.Parameters.AddWithValue("@Month", objBranchTarget.Month);
                    cmd.Parameters.AddWithValue("@BranchId", objBranchTarget.BranchId);
                    cmd.Parameters.AddWithValue("@OrderBookingTarget", objBranchTarget.OrderBookingTarget);
                    cmd.Parameters.AddWithValue("@InvoicingTarget", objBranchTarget.InvoicingTarget);
                    cmd.Parameters.AddWithValue("@CostTarget", objBranchTarget.CostTarget);
                    cmd.Parameters.AddWithValue("@CollectionTarget", objBranchTarget.CollectionTarget);

                    cmd.Parameters.AddWithValue("@Outstanding", objBranchTarget.Outstanding);
                    cmd.Parameters.AddWithValue("@DSO", objBranchTarget.DSO);
                    cmd.Parameters.AddWithValue("@ActualSales", objBranchTarget.ActualSales);
                    cmd.Parameters.AddWithValue("@Actual_EBIT", objBranchTarget.Actual_EBIT);
                    cmd.Parameters.AddWithValue("@ActualInvoicing", objBranchTarget.ActualInvoicing);
                    cmd.Parameters.AddWithValue("@ActualCollection", objBranchTarget.ActualCollection);
                 
                    //Outstanding   DSO Actual Sales Actual EBIT Actual Cost(Sales - EBIT)  Actual Invoicing    Actual Collection

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@Branch", objBranchTarget.Branch);
                    cmd.Parameters.AddWithValue("@CostCenter", objBranchTarget.ServiceCode);
                    
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("SP_Target", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@Year", objBranchTarget.Year);
                    cmd.Parameters.AddWithValue("@Branch", objBranchTarget.Branch);
                    cmd.Parameters.AddWithValue("@BranchId", objBranchTarget.BranchId);
                    cmd.Parameters.AddWithValue("@Month", objBranchTarget.Month);
                    cmd.Parameters.AddWithValue("@OrderBookingTarget", objBranchTarget.OrderBookingTarget);
                    cmd.Parameters.AddWithValue("@InvoicingTarget", objBranchTarget.InvoicingTarget);
                    cmd.Parameters.AddWithValue("@CostTarget", objBranchTarget.CostTarget);
                    cmd.Parameters.AddWithValue("@CollectionTarget", objBranchTarget.CollectionTarget);
                    cmd.Parameters.AddWithValue("@CostCenter", objBranchTarget.ServiceCode);
                    cmd.Parameters.AddWithValue("@Outstanding", objBranchTarget.Outstanding);
                    cmd.Parameters.AddWithValue("@DSO", objBranchTarget.DSO);
                    cmd.Parameters.AddWithValue("@ActualSales", objBranchTarget.ActualSales);
                    cmd.Parameters.AddWithValue("@Actual_EBIT", objBranchTarget.Actual_EBIT);
                    cmd.Parameters.AddWithValue("@ActualInvoicing", objBranchTarget.ActualInvoicing);
                    cmd.Parameters.AddWithValue("@ActualCollection", objBranchTarget.ActualCollection);
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

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Target", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
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


        public DataSet GetDataOnMonthSelection(string Year, string Month)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Target", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
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


        public DataSet GetDataOnCostCenterSelection(string Year, string CostCenter)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Target", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@CostCenter", CostCenter);
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


        public List<BranchTarget> GetOBSType()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchTarget> lstEnquiryDashB = new List<BranchTarget>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_Target", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 5);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new BranchTarget
                           {
                               PK_ID = Convert.ToInt32(dr["groupID"]),
                               PortfolioName = Convert.ToString(dr["groupName"]),
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


        public DataSet GetIndividualBranchWiseData(string BranchId,string Year)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Target", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@Branch", BranchId);
                cmd.Parameters.AddWithValue("@Year", Year);

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


        #region Insert Update Individual Target
        public string InsertIndividualTarget(IndividualTarget objIndividualTarget)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (objIndividualTarget.Id > 0)//Update
                {

                    SqlCommand cmd = new SqlCommand("SP_Target", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '8');
                    cmd.Parameters.AddWithValue("@Id", objIndividualTarget.Id);
                    //cmd.Parameters.AddWithValue("@Year", objIndividualTarget.Year);
                    //cmd.Parameters.AddWithValue("@Branch", objIndividualTarget.Branch);
                    //cmd.Parameters.AddWithValue("@BranchId", objIndividualTarget.BranchId);
                    //cmd.Parameters.AddWithValue("@EmployeeId", objIndividualTarget.EmployeeId);
                    cmd.Parameters.AddWithValue("@OrderBookingTarget", objIndividualTarget.OrderBookingTarget);
                    cmd.Parameters.AddWithValue("@MarketingVisits", objIndividualTarget.MarketingVisits);

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    

                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {

                    SqlCommand cmd = new SqlCommand("SP_Target", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '7');
                    cmd.Parameters.AddWithValue("@Year", objIndividualTarget.Year);
                    cmd.Parameters.AddWithValue("@Branch", objIndividualTarget.Branch);
                    cmd.Parameters.AddWithValue("@BranchId", objIndividualTarget.BranchId);
                    cmd.Parameters.AddWithValue("@EmployeeName", objIndividualTarget.EmployeeId);
                    cmd.Parameters.AddWithValue("@OrderBookingTarget", objIndividualTarget.OrderBookingTarget);
                    cmd.Parameters.AddWithValue("@MarketingVisits", objIndividualTarget.MarketingVisits);
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
        #endregion



    }
}