using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;

namespace TuvVision.DataAccessLayer
{
    public class DALJob
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        #region  JOB Master
        public DataTable GetJOBList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_JobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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




        public string InsertUpdateJOb(JobMasters CPM, string IPath, string IPath1)
        {
            int ReturnId = 0;
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.PK_JOB_ID == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CMP_ID", CPM.FK_CMP_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Job_Number", CPM.Job_Number);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Quotation_Of_Order", CPM.Quotation_Of_Order);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Enquiry_Of_Order", CPM.Enquiry_Of_Order);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Branch", CPM.Branch);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@End_User", CPM.End_User);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@subserviceType", CPM.subserviceType);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PortfolioType", CPM.PortfolioType);
                    

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Job_type", CPM.Job_type);
//                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                   // CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Contract_reviewList", CPM.Contract_reviewList);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);
                    
                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);


                    CMDInsertUpdateJOB.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    if (CPM.Customer_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    { 
                        
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy") , "dd/MM/yyyy", theCultureInfo));
                    }																																								  
                                      
                    
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Balance", CPM.Balance);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@InvoiceAmount", CPM.InvoiceAmount);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PortID", CPM.PortID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Currency", CPM.Currency);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
					CMDInsertUpdateJOB.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@ExchangeRate", CPM.DExchangeRate);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.DTotalAmount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@OrderTypeId", CPM.OrderTypeId);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;


                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);
                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@End_User", CPM.End_User);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    if (CPM.Customer_PoDate != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Customer_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Customer_PoNo_PoDate);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PO_Amount", CPM.Customer_PO_Amount);

                    if (CPM.POAwaited == false)
                    {                        
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Po_Validity, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(DateTime.Today.AddDays(15).ToString("dd/MM/yyyy"), "dd/MM/yyyy", theCultureInfo));
                     
                    }

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Estimate_ManDays_ManMonth);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManMonth", CPM.Estimate_ManMonth);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@GstDetails_BillingAddress", CPM.GstDetails_BillingAddress);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Special_Notes", CPM.Special_Notes);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@FirstYear", CPM.FirstYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SecondYear", CPM.SecondYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ThirdYear", CPM.ThirdYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@FourthYear", CPM.FourthYear);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Balance", CPM.Balance);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderType", CPM.OrderType);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderRate", CPM.OrderRate);
                    // CMDInsertUpdateBranch.Parameters.AddWithValue("@Customer_PoDate", CPM.Customer_PoDate);
                    

                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    if (IPath1 != null && IPath1 != "")
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@formats_Of_Report", IPath1);
                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OBSID", CPM.OBSID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ServiceID", CPM.ServiceID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PortID", CPM.PortID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Orderstatus", CPM.OrderStatus);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Currency", CPM.Currency);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubProjectName", CPM.SubProjectName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
					CMDInsertUpdateBranch.Parameters.AddWithValue("@chkARC", CPM.chkARC);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@checkIFCustomerSpecific", CPM.checkIFCustomerSpecific);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@POAwaited", CPM.POAwaited);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Estimate_ManHR", CPM.Estimate_ManHours);
                    
                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
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

        

        public DataTable GetConsumeCount(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 11);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;

        }

        public DataSet EditJob(int? PK_JOB_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataTable EditListOfsubJob(int? Br_Id)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        #region
        public DataSet GetQutationDetails(int? PK_QT_ID)//Getting Data of Qutation Details
        {
            DataSet DTGetEnquiryDtls = new DataSet();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_JobMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_QT_ID ", PK_QT_ID);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }
        #endregion


        #endregion

        public DataSet CheckQutationdata(int? PK_QT_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@PK_QT_ID", PK_QT_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        //#region  Manoj Code
        //public DataTable GetEnquiryDetals(int? EQ_ID)//Getting Data of Enquiry Details
        //{
        //    DataTable DTGetEnquiryDtls = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_QuotationMaster", con);
        //        CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 3);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //        SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
        //        SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTGetEnquiryDtls.Dispose();
        //    }
        //    return DTGetEnquiryDtls;

        //}


        //#endregion


        public int DeleteJob(int? PK_JOB_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_JobMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 7);
                CMDContactDelete.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        #region Job No Set

        public DataTable JobNoSet(JobMasters CPM)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditContact.Parameters.AddWithValue("@Branch", CPM.Branch);
                CMDEditContact.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }


        public DataSet GetBranchid(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Branch);
               
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataSet GetBranchCode(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "6N");
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Branch);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataSet GetServicesTypeID(JobMasters CPM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditContact.Parameters.AddWithValue("@Branch_Name", CPM.Service_type);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        #endregion


        public DataTable JobNoUniqueId()
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 9);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        //**********************Manoj Added code for Uploading File in database on 13 March 2020 FOR ATTACHMENT
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_JOBID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent);
                }


                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListJobMasterUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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
        public DataTable EditUploadedFile(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", PK_JOB_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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
        public DataTable GetFileExt(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_JobMasterUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetFileExtenstion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetFileExtenstion.Dispose();
            }
            return DTGetFileExtenstion;
        }

        //****************************************************************Ending Code Related File Uploaded*******************
        
            //**********************Manoj Added code for Uploading File in database on 12 March 2020 FOR FORMAT OF REPORT FILE
        public string InsertFileFormat(List<FileDetails> lstFileUploaded, int JOB_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_JOBID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(JOB_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListJobMastUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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
        public DataTable EditUploadedFileFormat(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", BR_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFileFormat(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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

        public DataTable GetFileExtension(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAGetExtenstion = new SqlDataAdapter(CMDGetExtenstion);
                SDAGetExtenstion.Fill(DTGetFileExtenstion);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetFileExtenstion.Dispose();
            }
            return DTGetFileExtenstion;
        }


        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        public DataTable DownloadSubJobAttachment(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "4N");
                CMDEditUploadedFile.Parameters.AddWithValue("@FileID", EQ_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", UserIDs);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        //****************************************************************Ending Code Related File Uploaded*******************


        public DataTable GetCallSummary(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 13);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        public DataTable GetAddMandays(int? PK_JOB_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 16);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditContact.Dispose();
            }
            return DTEditContact;
        }

        

        public string InsertUpdateAddMandays(JobMasters CPM)
        {
            int ReturnId = 0;
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.PK_ADDPOID == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 14);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Add_PoNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.Add_POAmt);
                    if(CPM.Add_PoValidity !=null)
                    {                        
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Add_PoValidity, "dd/MM/yyyy", theCultureInfo));
                    }
                    if (CPM.Add_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Add_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }

                    
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Add_Mandays);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Reason", CPM.Add_PoReason);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Add_MandayDesc);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));                    
                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);
                }
                else
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_JobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 15);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_ADDPOID", CPM.PK_ADDPOID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoNo_PoDate", CPM.Add_PoNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Amount", CPM.Add_POAmt);
                    if (CPM.Add_PoValidity != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Po_Validity", DateTime.ParseExact(CPM.Add_PoValidity, "dd/MM/yyyy", theCultureInfo));
                    }

                    if (CPM.Add_PoDate != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Customer_PoDate", DateTime.ParseExact(CPM.Add_PoDate, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", CPM.Add_Mandays);
                  //  CMDInsertUpdateJOB.Parameters.AddWithValue("@Reason", CPM.Add_PoReason);
                   // CMDInsertUpdateJOB.Parameters.AddWithValue("@Description", CPM.Add_MandayDesc);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
                   
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
    }
}