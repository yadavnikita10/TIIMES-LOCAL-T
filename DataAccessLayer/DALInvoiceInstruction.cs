using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALInvoiceInstruction
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        public string InsertUpdateInvoiceInstruction(InvoiceInstructionModel CPM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.INV_ID == 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_InvoicingInstruction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@InvoiceAmount", CPM.InvoiceAmount);
                    cmd.Parameters.AddWithValue("@InvoiceDate",DateTime.ParseExact(CPM.InvoiceDate,"dd/MM/yyyy",null));
                    cmd.Parameters.AddWithValue("@InvoiceNumber", CPM.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    cmd.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    cmd.Parameters.AddWithValue("@PoAmount", CPM.PoAmount);
                    cmd.Parameters.AddWithValue("@TotalAmount", CPM.TotalAmount);
                    cmd.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    cmd.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    cmd.Parameters.AddWithValue("@Balance", CPM.Balance);
                    cmd.Parameters.AddWithValue("@GSTDetail", CPM.GSTDetail);
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.ChkMultipleSubJobNo);
                    cmd.Parameters.AddWithValue("@rptType", CPM.rptType);
                    cmd.Parameters.AddWithValue("@Invoicetext", CPM.Invoicetext);
                    
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_InvoicingInstruction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '3');
                    cmd.Parameters.AddWithValue("@InvoiceAmount", CPM.InvoiceAmount);
                    cmd.Parameters.AddWithValue("@InvoiceDate", DateTime.ParseExact(CPM.InvoiceDate, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@InvoiceNumber", CPM.InvoiceNumber);
                    cmd.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    cmd.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    cmd.Parameters.AddWithValue("@TotalAmount", CPM.TotalAmount);
                    cmd.Parameters.AddWithValue("@Balance", CPM.Balance);

                    cmd.Parameters.AddWithValue("@INV_ID", CPM.INV_ID);
                    cmd.Parameters.AddWithValue("@ServiceCode", CPM.ServiceCode);
                    cmd.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.ChkMultipleSubJobNo);
                    cmd.Parameters.AddWithValue("@rptType", CPM.rptType);
                    cmd.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.Invoicetext);
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

        
        public int GetSubJobCount(int? PK_JOB_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            int count = 0;
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 12);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlList);
                count = Convert.ToInt32(DSGetddlList.Tables[0].Rows[0]["count"]);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlList.Dispose();
            }
            return count;
        }

        public string UpdateBalanceInJobMaster(InvoiceInstructionModel CPM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                    SqlCommand cmd = new SqlCommand("SP_InvoicingInstruction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '9');
                    cmd.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    cmd.Parameters.AddWithValue("@Balance", CPM.Balance);
                    cmd.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string UpdateBalance(int kid, decimal bal) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_InvoicingInstruction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                //cmd.Parameters.AddWithValue("@InvoiceAmount", CPM.TotalAmount);
                cmd.Parameters.AddWithValue("@PK_JOB_ID", kid);
                cmd.Parameters.AddWithValue("@Balance", Convert.ToDecimal(bal));
                cmd.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataSet GetInvoiceInstructionById(int? PK_JOB_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
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

        public string InvoiceIDByJob(int JobId)
        {
            DataSet DSEditContact = new DataSet();
            string InvNo = string.Empty;
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InvoicingInstruction", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 13);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", JobId);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DSEditContact);
                InvNo = DSEditContact.Tables[0].Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSEditContact.Dispose();
            }
            return InvNo;
        }

        public DataTable GetInvoiceInstruction(int? PK_JOB_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
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

        public DataTable GetreportName(string FromDate,string ToDate, string JobNo,string subJobID,string orderType,int? JobID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 15);
                
                  
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedDate", DateTime.ParseExact(FromDate,"dd/MM/yyyy",null));
                CMDGetDdlLst.Parameters.AddWithValue("@ModifyDate", DateTime.ParseExact(ToDate, "dd/MM/yyyy", null));
                CMDGetDdlLst.Parameters.AddWithValue("@JobNumber", JobNo);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", JobID);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJobNo", subJobID);
                CMDGetDdlLst.Parameters.AddWithValue("@rptType", orderType);
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

        public DataTable GetBalanceAmount(int? JobId)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 16);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", JobId);
             
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
        

        public DataSet EditInvoiceInstructionById(int? INV_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@INV_ID", INV_ID);
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

        public DataSet BindSubJobControlNo(int JobNo)
        {
            DataSet dsSubJobByControlNo = new DataSet();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 8);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", JobNo);                
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(dsSubJobByControlNo);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsSubJobByControlNo.Dispose();
            }
            return dsSubJobByControlNo;

        }

        public List<SubJobs> GetSubJobList(int? PK_JOB_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<SubJobs> lstEnquiryDashB = new List<SubJobs>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 8);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
               // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new SubJobs
                           {
                               PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                               SubJob_No = Convert.ToString(dr["SubJob_No"]),

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

        public int DeleteInvoiceInstructionData(int? INV_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_InvoicingInstruction", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDContactDelete.Parameters.AddWithValue("@INV_ID", INV_ID);
                //cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                Result = CMDContactDelete.ExecuteNonQuery();
                if (Result != 0)
                {
                    return Result;
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



        public DataSet dsMisOrderStatus()//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "10");
                //CMDGetDdlLst.Parameters.AddWithValue("@INV_ID", INV_ID);
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

        public DataSet dsMisOrderStatusByDate(MISOrderStatus MOS)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InvoicingInstruction", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "10");
                //CMDGetDdlLst.Parameters.AddWithValue("@INV_ID", INV_ID);
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



    }
}