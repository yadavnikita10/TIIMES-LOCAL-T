using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class DALQuotationMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public List<QuotationMaster> QuotaionMastertDashBoard(string Type)// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTQMDashBoard = new DataTable();
            List<QuotationMaster> lstQuotationMastDashB = new List<QuotationMaster>();
            try
            {   
                SqlCommand CMDQMDashBoard = new SqlCommand("SP_QuotationMaster", con);
                CMDQMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDQMDashBoard.CommandTimeout = 1000000;
                if(Type== "NoOfQuotationInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 25);
                }
                else if(Type == "QuotationApprovedByPCH")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 26);
                }
                else if (Type == "QuotationApprovedByPCHInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 27);
                }
                else if (Type == "QuotationApprovedByClusterHead")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 28);
                }
                else if (Type == "QuotationApprovedByClusterHeadInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 29);
                }
                else if (Type == "QuotationWon")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 30);
                }
                else if (Type == "QuotationWonInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 31);
                }
                else if (Type == "QuotationLost")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 32);
                }
                else if (Type == "QuotationLostInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 33);
                }
                else if (Type == "QuotationPendingForApprovel")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 34);
                }
                else if (Type == "QuotationPendingForApprovelInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 35);
                }
                else if (Type == "QuotationOpen")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 36);
                }
                else if (Type == "QuotationOpenInLast15Days")
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 37);
                }
                
                else
                {
                    CMDQMDashBoard.Parameters.AddWithValue("@SP_Type", 9);
                }
                CMDQMDashBoard.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDQMDashBoard);
                SDAGetEnquiry.Fill(DTQMDashBoard);
                if (DTQMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTQMDashBoard.Rows)
                    {
                        lstQuotationMastDashB.Add(
                           new QuotationMaster
                           {
                               PK_QTID = Convert.ToInt32(dr["PK_QTID"]),
                               QuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                               EnquiryNumber = Convert.ToString(dr["EnquiryNumber"]),
                               Quotation_Description = Convert.ToString(dr["Description"]),
                               Reference = Convert.ToString(dr["Reference"]),
                               Enquiry = Convert.ToString(dr["Enquiry"]),
                               ExpiryDate = Convert.ToString(dr["ExpiryDate"]),
                               ApprovalStatus = Convert.ToString(dr["ApprovalStatus"]),
                               StatusType = Convert.ToString(dr["Status"]),
                               QTType = Convert.ToString(dr["QuotationType"]),
                               ProjectName = Convert.ToString(dr["ProjectType"]),
                               PortfolioType = Convert.ToString(dr["PortfolioType"]),
                               SubServiceType = Convert.ToString(dr["ServiceType"]),
                               Count = DTQMDashBoard.Rows.Count,
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               EstimatedAmount = Convert.ToString(dr["EstimatedAmount"]),
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
                DTQMDashBoard.Dispose();
            }
            return lstQuotationMastDashB;
        }

        public DataSet GetAllddlLst()//Binding All Dropdownlist Quotation
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        
        public DataTable GetQuotationNo(int? PK_QTId)//Getting Data of Enquiry Details
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_QuotationMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 23);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_QTID", PK_QTId);
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

        public DataTable GetEnquiryDetals(int? EQ_ID)//Getting Data of Enquiry Details
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_QuotationMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 3);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
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
        public DataTable GetCLApprovalStatus(int? PK_QTID)//Revised Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", 24);
                CMDEditQM.Parameters.AddWithValue("@PK_QTID", PK_QTID);
                CMDEditQM.Parameters.AddWithValue("@QuotationNumber", PK_QTID);
                CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;
        }
        
        public DataTable RevisedQuotationMast(int?PK_QTID)//Revised Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type",5);
                CMDEditQM.Parameters.AddWithValue("@PK_QTID", PK_QTID);
                CMDEditQM.Parameters.AddWithValue("@QuotationNumber", PK_QTID); 
                CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;
        }
        public DataTable QuotationCount(string QtnNo)//Revised Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", "50");
                CMDEditQM.Parameters.AddWithValue("@QuotationNumber", QtnNo);
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;
        }
        public DataTable EditQuotationMast(int? QTId)//Edit Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditQM.Parameters.AddWithValue("@PK_QTID", QTId);
                CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;

        }

        public DataSet GetEditAllddlLst()//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        #region Code Coment by Sagar



        //public string InsertAndUpdateQuotation(QuotationMaster QM, string IPath)//Inserting Record in QuotationMaster
        //{
        //    string Result = string.Empty;
        //    string QuotationNumber = string.Empty;
        //    int QT_ID;
        //    DataTable DTGetMAXId = new DataTable();
        //    con.Open();
        //    try
        //    {
        //        if (QM.PK_QTID != 0)
        //        {
        //            SqlCommand CMDRevisedQuotation = new SqlCommand("SP_QuotationMaster", con);//Updating Record In Database
        //            CMDRevisedQuotation.CommandType = CommandType.StoredProcedure;
        //            CMDRevisedQuotation.Parameters.AddWithValue("@SP_Type", 4);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Status", QM.Status);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@ExpiryDate", QM.ExpiryDate);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@GST", QM.GST);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);

        //            if (IPath != null && IPath != "")
        //            {
        //                CMDRevisedQuotation.Parameters.AddWithValue("@Attachment", IPath);
        //            }
        //            else
        //            {
        //                CMDRevisedQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
        //            }
        //            CMDRevisedQuotation.Parameters.AddWithValue("@TO", QM.To);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@CC", QM.CC);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
        //            CMDRevisedQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);

        //            SqlParameter RequestID = CMDRevisedQuotation.Parameters.Add("@PK_MAX_QTID", SqlDbType.VarChar, 100);
        //            CMDRevisedQuotation.Parameters["@PK_MAX_QTID"].Direction = ParameterDirection.Output;

        //            CMDRevisedQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //            CMDRevisedQuotation.ExecuteNonQuery().ToString();
        //            Result = Convert.ToString(CMDRevisedQuotation.Parameters["@CompanyName"].Value);
        //            QuotationNumber = Convert.ToString(CMDRevisedQuotation.Parameters["@QuotationNumber"].Value);
        //            System.Web.HttpContext.Current.Session["QuotationNumber"] = QuotationNumber;
        //            QT_ID = Convert.ToInt32(CMDRevisedQuotation.Parameters["@PK_MAX_QTID"].Value);
        //            System.Web.HttpContext.Current.Session["QTID"] = QT_ID;

        //        }
        //        if(QM.QuotationNumber!="" && QM.QuotationNumber!=null)
        //        {

        //            SqlCommand CMDUpdateQuotation = new SqlCommand("SP_QuotationMaster", con);//Updating Record In Database
        //            CMDUpdateQuotation.CommandType = CommandType.StoredProcedure;
        //            CMDUpdateQuotation.Parameters.AddWithValue("@SP_Type", 8);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Status", QM.Status);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@ExpiryDate", QM.ExpiryDate);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@GST", QM.GST);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);

        //            if (IPath != null && IPath != "")
        //            {
        //                CMDUpdateQuotation.Parameters.AddWithValue("@Attachment", IPath);
        //            }
        //            else
        //            {
        //                CMDUpdateQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
        //            }
        //            CMDUpdateQuotation.Parameters.AddWithValue("@TO", QM.To);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@CC", QM.CC);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);
        //            CMDUpdateQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //            CMDUpdateQuotation.ExecuteNonQuery().ToString();
        //            Result = Convert.ToString(CMDUpdateQuotation.Parameters["@QuotationNumber"].Value);
        //            System.Web.HttpContext.Current.Session["QuotationNumber"] = QM.QuotationNumber;
        //        }
        //        else
        //        {
        //            SqlCommand CMDInsertQuotation = new SqlCommand("SP_QuotationMaster", con);//Saving Record in Database
        //            CMDInsertQuotation.CommandType = CommandType.StoredProcedure;
        //            CMDInsertQuotation.Parameters.AddWithValue("@SP_Type", 4);
        //            CMDInsertQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
        //            CMDInsertQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
        //            CMDInsertQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
        //            CMDInsertQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
        //            CMDInsertQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Status", QM.Status);
        //            CMDInsertQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
        //            CMDInsertQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
        //            CMDInsertQuotation.Parameters.AddWithValue("@ExpiryDate", QM.ExpiryDate);
        //            CMDInsertQuotation.Parameters.AddWithValue("@GST", QM.GST);
        //            CMDInsertQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
        //            CMDInsertQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
        //            CMDInsertQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
        //            CMDInsertQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
        //            CMDInsertQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
        //            CMDInsertQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
        //            CMDInsertQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
        //            CMDInsertQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
        //            CMDInsertQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);
        //            if (IPath != null && IPath != "")
        //            {
        //                CMDInsertQuotation.Parameters.AddWithValue("@Attachment", IPath);
        //            }
        //            else
        //            {
        //                CMDInsertQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
        //            }
        //            CMDInsertQuotation.Parameters.AddWithValue("@TO", QM.To);
        //            CMDInsertQuotation.Parameters.AddWithValue("@CC", QM.CC);
        //            CMDInsertQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
        //            CMDInsertQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
        //            CMDInsertQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);
        //            //Adding New Fields End 07 June 2019

        //            SqlParameter RequestID = CMDInsertQuotation.Parameters.Add("@PK_MAX_QTID", SqlDbType.VarChar, 100);
        //            CMDInsertQuotation.Parameters["@PK_MAX_QTID"].Direction = ParameterDirection.Output;

        //            CMDInsertQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //            CMDInsertQuotation.ExecuteNonQuery().ToString();
        //            Result = Convert.ToString(CMDInsertQuotation.Parameters["@CompanyName"].Value);

        //            QuotationNumber = Convert.ToString(CMDInsertQuotation.Parameters["@QuotationNumber"].Value);
        //            System.Web.HttpContext.Current.Session["QuotationNumber"] = QuotationNumber;
        //            QT_ID = Convert.ToInt32(CMDInsertQuotation.Parameters["@PK_MAX_QTID"].Value);
        //            System.Web.HttpContext.Current.Session["QTID"] = QT_ID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return Result;
        //}

        #endregion
        #region Added By Ankush - Job No against Quotation
        public List<QuotationMaster> GetJobNoForQuotation(string QtnId)// Binding QuotationMAster DashBoard of Master Page 
        {
            DataTable DTJMDashBoard = new DataTable();
            List<QuotationMaster> GetJobByQuotation = new List<QuotationMaster>();
            try
            {
                SqlCommand CMDJMDashBoard = new SqlCommand("SP_QuotationMaster", con);
                CMDJMDashBoard.CommandType = CommandType.StoredProcedure;
                CMDJMDashBoard.CommandTimeout = 1000000;
                CMDJMDashBoard.Parameters.AddWithValue("@SP_Type", 15);
                CMDJMDashBoard.Parameters.AddWithValue("@QuotationNumber", QtnId);
                SqlDataAdapter SDAGetJobNo = new SqlDataAdapter(CMDJMDashBoard);
                SDAGetJobNo.Fill(DTJMDashBoard);
                if (DTJMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTJMDashBoard.Rows)
                    {
                        GetJobByQuotation.Add(
                           new QuotationMaster
                           {
                               PKJobId = Convert.ToInt32(dr["PK_JOB_ID"]),
                               JobNo = Convert.ToString(dr["Job_Number"]),
                               ClientName = Convert.ToString(dr["Client_Name"]),
                               Description = Convert.ToString(dr["Description"]),
                               Job_type = Convert.ToString(dr["Job_type"]),
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
                DTJMDashBoard.Dispose();
            }
            return GetJobByQuotation;
        }
        //public DataTable JobDetailsByID(int? JobId)//Revised Quotaion Details
        //{
        //    DataTable DTEditQM = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
        //        CMDEditQM.CommandType = CommandType.StoredProcedure;
        //        CMDEditQM.Parameters.AddWithValue("@SP_Type", 5);
        //        CMDEditQM.Parameters.AddWithValue("@PK_JOB_ID", JobId);
        //        CMDEditQM.Parameters.AddWithValue("@QuotationNumber", PK_QTID);
        //        CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //        SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
        //        SDAEditQM.Fill(DTEditQM);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEditQM.Dispose();
        //    }
        //    return DTEditQM;
        //}


        #endregion
        public DataTable GetPrintQuotationDtls(QuotationMaster QM)//Quotation Printing Details 
        {
            DataTable DTPrintQuotationDtls = new DataTable();
            try
            {
                SqlCommand CMDPrintQMDtls = new SqlCommand("SP_QuotationMaster", con);
                CMDPrintQMDtls.CommandType = CommandType.StoredProcedure;
                CMDPrintQMDtls.Parameters.AddWithValue("@SP_Type",6);
                CMDPrintQMDtls.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
                CMDPrintQMDtls.Parameters.AddWithValue("@PK_QTID", QM.PK_QTID);
                CMDPrintQMDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAPrintQMDtls = new SqlDataAdapter(CMDPrintQMDtls);
                SDAPrintQMDtls.Fill(DTPrintQuotationDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return DTPrintQuotationDtls;

        }


        #region Delete Code By Rahul 
        public int DeleteQuotation(int? PK_QTID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDCompanyDelete = new SqlCommand("SP_QuotationMaster", con);
                CMDCompanyDelete.CommandType = CommandType.StoredProcedure;
                CMDCompanyDelete.CommandTimeout = 100000;
                CMDCompanyDelete.Parameters.AddWithValue("@SP_Type", 10);
                CMDCompanyDelete.Parameters.AddWithValue("@PK_QTID", PK_QTID);
                //CMDCompanyDelete.Parameters.AddWithValue("@Status", 0);
                CMDCompanyDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDCompanyDelete.ExecuteNonQuery();
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
        #endregion



        #region Code By Sagar
        public string InsertAndUpdateQuotation(QuotationMaster QM, string IPath)//Inserting Record in QuotationMaster
        {
            string Result = string.Empty;
            string QuotationNumber = string.Empty;
            int QT_ID;
            DataTable DTGetMAXId = new DataTable();
            con.Open();
            try
            {
                if (QM.PK_QTID != 0 && QM.Revise !=null /*|| QM.Revise!=""*/) // ===========Revise
                {
                    SqlCommand CMDRevisedQuotation = new SqlCommand("SP_QuotationMaster", con);//Updating Record In Database
                    CMDRevisedQuotation.CommandType = CommandType.StoredProcedure;
                    CMDRevisedQuotation.Parameters.AddWithValue("@SP_Type", 11);
                   // CMDRevisedQuotation.Parameters.AddWithValue("@FK_CMP_ID", QM.FK_CMP_ID);
                    CMDRevisedQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
                    CMDRevisedQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
                    CMDRevisedQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
                    CMDRevisedQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
                    //CMDRevisedQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Type", QM.SubServiceType);

                    CMDRevisedQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Status", QM.Status);
                    CMDRevisedQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
                    CMDRevisedQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ExpiryDate", DateTime.ParseExact(QM.ExpiryDate, "dd/MM/yyyy", theCultureInfo));
                    CMDRevisedQuotation.Parameters.AddWithValue("@GST", QM.GST);
                    CMDRevisedQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
                    CMDRevisedQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
                    CMDRevisedQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
                    CMDRevisedQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
                    CMDRevisedQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
                    CMDRevisedQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
                    CMDRevisedQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);
                    CMDRevisedQuotation.Parameters.AddWithValue("@QuotationType", "Revision");
                    

                    if (IPath != null && IPath != "")
                    {
                        CMDRevisedQuotation.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    else
                    {
                        CMDRevisedQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
                    }
                    CMDRevisedQuotation.Parameters.AddWithValue("@TO", QM.To);
                    CMDRevisedQuotation.Parameters.AddWithValue("@CC", QM.CC);
                    CMDRevisedQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
                    CMDRevisedQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);

                    CMDRevisedQuotation.Parameters.AddWithValue("@FK_CMP_ID", QM.FK_CMP_ID);
                    CMDRevisedQuotation.Parameters.AddWithValue("@CompanyAddress", QM.CompanyAddress);
                    CMDRevisedQuotation.Parameters.AddWithValue("@Validity", QM.Validity);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ThirdPartyInspectionService", QM.ThirdPartyInspectionService);
                    CMDRevisedQuotation.Parameters.AddWithValue("@ThankYouLetter", QM.ThankYouLetter);
                    CMDRevisedQuotation.Parameters.AddWithValue("@PortfolioType", QM.PortfolioType);
                    CMDRevisedQuotation.Parameters.AddWithValue("@SubServiceType", QM.SubServiceType);

                    ///Added Amount
                    CMDRevisedQuotation.Parameters.AddWithValue("@EstimatedAmount", QM.EstimatedAmount);
                    CMDRevisedQuotation.Parameters.AddWithValue("@distance", QM.Distance);
                    CMDRevisedQuotation.Parameters.AddWithValue("@LostReason", QM.LostReason);
                    CMDRevisedQuotation.Parameters.AddWithValue("@DomStatus", QM.DomStatus);
                    CMDRevisedQuotation.Parameters.AddWithValue("@IntStatus", QM.IntStatus);

                    CMDRevisedQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    SqlParameter RequestID = CMDRevisedQuotation.Parameters.Add("@PK_MAX_QTID", SqlDbType.VarChar, 100);
                    CMDRevisedQuotation.Parameters["@PK_MAX_QTID"].Direction = ParameterDirection.Output;

                  
                    CMDRevisedQuotation.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDRevisedQuotation.Parameters["@CompanyName"].Value);
                    QuotationNumber = Convert.ToString(CMDRevisedQuotation.Parameters["@QuotationNumber"].Value);
                    System.Web.HttpContext.Current.Session["QuotationNumber"] = QuotationNumber;
                    QT_ID = Convert.ToInt32(CMDRevisedQuotation.Parameters["@PK_MAX_QTID"].Value);
                    System.Web.HttpContext.Current.Session["QTID"] = QT_ID;

                }
                #region update
                if (QM.QuotationNumber != "" && QM.QuotationNumber != null && QM.Revise == null || QM.Revise == "") //=============Update
                {

                    SqlCommand CMDUpdateQuotation = new SqlCommand("SP_QuotationMaster", con);//Updating Record In Database
                    CMDUpdateQuotation.CommandType = CommandType.StoredProcedure;
                    CMDUpdateQuotation.Parameters.AddWithValue("@SP_Type", 8);
                    CMDUpdateQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
                    CMDUpdateQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
                    CMDUpdateQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
                    CMDUpdateQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Status", QM.Status);
                    CMDUpdateQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
                    CMDUpdateQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
                    ///CMDUpdateQuotation.Parameters.AddWithValue("@ExpiryDate", QM.ExpiryDate);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ExpiryDate", DateTime.ParseExact(QM.ExpiryDate, "dd/MM/yyyy", theCultureInfo));
                    CMDUpdateQuotation.Parameters.AddWithValue("@GST", QM.GST);
                    CMDUpdateQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
                    CMDUpdateQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
                    CMDUpdateQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
                    CMDUpdateQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
                    CMDUpdateQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
                    CMDUpdateQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
                    CMDUpdateQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);

                    if (IPath != null && IPath != "")
                    {
                        CMDUpdateQuotation.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    else
                    {
                        CMDUpdateQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
                    }
                    CMDUpdateQuotation.Parameters.AddWithValue("@TO", QM.To);
                    CMDUpdateQuotation.Parameters.AddWithValue("@CC", QM.CC);

                    CMDUpdateQuotation.Parameters.AddWithValue("@CompanyAddress", QM.CompanyAddress);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Validity", QM.Validity);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ThirdPartyInspectionService", QM.ThirdPartyInspectionService);
                    CMDUpdateQuotation.Parameters.AddWithValue("@ThankYouLetter", QM.ThankYouLetter);
                    CMDUpdateQuotation.Parameters.AddWithValue("@PortfolioType", QM.PortfolioType);
                    CMDUpdateQuotation.Parameters.AddWithValue("@SubServiceType", QM.SubServiceType);

                    CMDUpdateQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
                    CMDUpdateQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);
                    ///Added Amount
                    CMDUpdateQuotation.Parameters.AddWithValue("@EstimatedAmount", QM.EstimatedAmount);

                    //14
                    CMDUpdateQuotation.Parameters.AddWithValue("@InspectionLocation", QM.InspectionLocation);
                    CMDUpdateQuotation.Parameters.AddWithValue("@City", QM.City);
                    CMDUpdateQuotation.Parameters.AddWithValue("@Country", QM.Country);

                   

                    CMDUpdateQuotation.Parameters.AddWithValue("@chkArc", QM.chkArc);
                 
                    CMDUpdateQuotation.Parameters.AddWithValue("@Exclusion", QM.Exclusion);
                  
                    CMDUpdateQuotation.Parameters.AddWithValue("@ExclusionCheckBox", QM.ExclusionCheckBox);
                    CMDUpdateQuotation.Parameters.AddWithValue("@LostReason", QM.LostReason);

                    CMDUpdateQuotation.Parameters.AddWithValue("@DomStatus", QM.DomStatus);
                    CMDUpdateQuotation.Parameters.AddWithValue("@IntStatus", QM.IntStatus);

                    CMDUpdateQuotation.Parameters.AddWithValue("@FK_QM_ID", Convert.ToString(QM.PK_QTID));
                    
                    CMDUpdateQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDUpdateQuotation.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDUpdateQuotation.Parameters["@QuotationNumber"].Value);
                    System.Web.HttpContext.Current.Session["QuotationNumber"] = QM.QuotationNumber;
                }
                #endregion
                if(QM.PK_QTID == 0 && QM.Revise == null && QM.QuotationNumber == null)
                {
                    SqlCommand CMDInsertQuotation = new SqlCommand("SP_QuotationMaster", con);//Saving Record in Database
                    CMDInsertQuotation.CommandType = CommandType.StoredProcedure;
                    CMDInsertQuotation.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertQuotation.Parameters.AddWithValue("@QuotationNumber", QM.QuotationNumber);
                    CMDInsertQuotation.Parameters.AddWithValue("@EQ_ID", QM.EQ_ID);
                    CMDInsertQuotation.Parameters.AddWithValue("@Quotation_Description", QM.Quotation_Description);
                    CMDInsertQuotation.Parameters.AddWithValue("@CompanyName", QM.QuotationCompanyName);
                    CMDInsertQuotation.Parameters.AddWithValue("@EndCustomer", QM.EndCustomer);
                    CMDInsertQuotation.Parameters.AddWithValue("@Reference", QM.Reference);
                    CMDInsertQuotation.Parameters.AddWithValue("@ProjectType", QM.ProjectType);
                    CMDInsertQuotation.Parameters.AddWithValue("@Branch", QM.QuotationBranch);
                    // CMDInsertQuotation.Parameters.AddWithValue("@Type", QM.ServiceType);
                    CMDInsertQuotation.Parameters.AddWithValue("@Type", QM.SubServiceType);
                    
                    CMDInsertQuotation.Parameters.AddWithValue("@Remark", QM.Remark);
                    CMDInsertQuotation.Parameters.AddWithValue("@Status", QM.Status);
                    CMDInsertQuotation.Parameters.AddWithValue("@PK_CSID", QM.PK_CSID);
                    CMDInsertQuotation.Parameters.AddWithValue("@PK_ContID", QM.PK_ContID);
                    CMDInsertQuotation.Parameters.AddWithValue("@Enquiry", QM.Enquiry);
                    //CMDInsertQuotation.Parameters.AddWithValue("@ExpiryDate", QM.ExpiryDate);
                    if (QM.ExpiryDate != null)
                    {
                        CMDInsertQuotation.Parameters.AddWithValue("@ExpiryDate", DateTime.ParseExact(QM.ExpiryDate, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertQuotation.Parameters.AddWithValue("@GST", QM.GST);
                    CMDInsertQuotation.Parameters.AddWithValue("@JobList", QM.JobList);
                    CMDInsertQuotation.Parameters.AddWithValue("@HeaderDetails", QM.HeaderDetails);
                    CMDInsertQuotation.Parameters.AddWithValue("@Subject", QM.Subject);
                    CMDInsertQuotation.Parameters.AddWithValue("@ScopeOfWork", QM.ScopeOfWork);
                    CMDInsertQuotation.Parameters.AddWithValue("@Deliverable", QM.Deliverable);
                    CMDInsertQuotation.Parameters.AddWithValue("@Commercials", QM.Commercials);
                    CMDInsertQuotation.Parameters.AddWithValue("@FeesStructure", QM.FeesStructure);
                    CMDInsertQuotation.Parameters.AddWithValue("@PaymentTerms", QM.PaymentTerms);
                    CMDInsertQuotation.Parameters.AddWithValue("@KeyNotes", QM.KeyNotes);
                    CMDInsertQuotation.Parameters.AddWithValue("@AddEnclosures", QM.AddEnclosures);
                    CMDInsertQuotation.Parameters.AddWithValue("@ApprovalStatus", QM.ApprovalStatus);
                    CMDInsertQuotation.Parameters.AddWithValue("@EnquiryNumber", QM.EnquiryNumber);
                    if (IPath != null && IPath != "")
                    {
                        CMDInsertQuotation.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    else
                    {
                        CMDInsertQuotation.Parameters.AddWithValue("@Attachment", QM.Attachment);
                    }
                    CMDInsertQuotation.Parameters.AddWithValue("@TO", QM.To);
                    CMDInsertQuotation.Parameters.AddWithValue("@CC", QM.CC);
                    CMDInsertQuotation.Parameters.AddWithValue("@CommunicationProtocol", QM.CommunicationProtocol);
                    CMDInsertQuotation.Parameters.AddWithValue("@Coordinators", QM.Coordinators);
                    CMDInsertQuotation.Parameters.AddWithValue("@EscalationMatrix", QM.EscalationMatrix);
                    CMDInsertQuotation.Parameters.AddWithValue("@FK_CMP_ID", QM.FK_CMP_ID);

                    CMDInsertQuotation.Parameters.AddWithValue("@CompanyAddress", QM.CompanyAddress);
                    CMDInsertQuotation.Parameters.AddWithValue("@Validity", QM.Validity);

                    CMDInsertQuotation.Parameters.AddWithValue("@ThirdPartyInspectionService", QM.ThirdPartyInspectionService);
                    CMDInsertQuotation.Parameters.AddWithValue("@ThankYouLetter", QM.ThankYouLetter);
                    //Adding New Fields End 07 June 2019
                    CMDInsertQuotation.Parameters.AddWithValue("@PortfolioType", QM.PortfolioType);
                    CMDInsertQuotation.Parameters.AddWithValue("@SubServiceType", QM.SubServiceType);
                    ///Added Amount
                    CMDInsertQuotation.Parameters.AddWithValue("@EstimatedAmount", QM.EstimatedAmount);

                    CMDInsertQuotation.Parameters.AddWithValue("@InspectionLocation", QM.InspectionLocation);
                    CMDInsertQuotation.Parameters.AddWithValue("@City", QM.City);
                    CMDInsertQuotation.Parameters.AddWithValue("@Country", QM.Country);

                    CMDInsertQuotation.Parameters.AddWithValue("@DEstimatedAmount", QM.DEstimatedAmount);
                    CMDInsertQuotation.Parameters.AddWithValue("@Dcurrency", QM.Dcurrency);
                    CMDInsertQuotation.Parameters.AddWithValue("@DExchangeRate", QM.DExchangeRate);
                    CMDInsertQuotation.Parameters.AddWithValue("@DTotalAmount", QM.DTotalAmount);
                    CMDInsertQuotation.Parameters.AddWithValue("@IEstimatedAmount", QM.IEstimatedAmount);
                    CMDInsertQuotation.Parameters.AddWithValue("@Icurrency", QM.Icurrency);
                    CMDInsertQuotation.Parameters.AddWithValue("@IExchangeRate", QM.IExchangeRate);
                    CMDInsertQuotation.Parameters.AddWithValue("@ITotalAmount", QM.ITotalAmount);
                    CMDInsertQuotation.Parameters.AddWithValue("@distance", QM.Distance);
                    //if(QM.chkArc == false)
                    //{
                    //    int a = 0;
                    //    QM.chkArc == a;
                    //}
                    //else
                    //{

                    //}
                    CMDInsertQuotation.Parameters.AddWithValue("@chkArc", QM.chkArc);
                    CMDInsertQuotation.Parameters.AddWithValue("@OrderRate", QM.OrderRate);
                    CMDInsertQuotation.Parameters.AddWithValue("@Estimate_ManMonth", QM.Estimate_ManMonth);
                    CMDInsertQuotation.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", QM.Estimate_ManDays_ManMonth);
                    CMDInsertQuotation.Parameters.AddWithValue("@OrderType", QM.OrderType);
                    CMDInsertQuotation.Parameters.AddWithValue("@Exclusion", QM.Exclusion);
                    CMDInsertQuotation.Parameters.AddWithValue("@LostReason", QM.LostReason);

                    CMDInsertQuotation.Parameters.AddWithValue("@DomStatus", QM.DomStatus);
                    CMDInsertQuotation.Parameters.AddWithValue("@IntStatus", QM.IntStatus);

                    if (QM.ExclusionCheckBox == true)
                    {
                        CMDInsertQuotation.Parameters.AddWithValue("@ExclusionCheckBox", "1");
                    }
                    else
                    {
                        CMDInsertQuotation.Parameters.AddWithValue("@ExclusionCheckBox", "0");
                    }
                    SqlParameter RequestID = CMDInsertQuotation.Parameters.Add("@PK_MAX_QTID", SqlDbType.VarChar, 100);
                    CMDInsertQuotation.Parameters["@PK_MAX_QTID"].Direction = ParameterDirection.Output;

                    SqlParameter RequestID1 = CMDInsertQuotation.Parameters.Add("@Q", SqlDbType.VarChar, 100);
                    CMDInsertQuotation.Parameters["@Q"].Direction = ParameterDirection.Output;

                    CMDInsertQuotation.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDInsertQuotation.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDInsertQuotation.Parameters["@CompanyName"].Value);

                    QuotationNumber = Convert.ToString(CMDInsertQuotation.Parameters["@Q"].Value);
                    System.Web.HttpContext.Current.Session["QuotationNumber"] = QuotationNumber;
                    QT_ID = Convert.ToInt32(CMDInsertQuotation.Parameters["@PK_MAX_QTID"].Value);
                    System.Web.HttpContext.Current.Session["QTID"] = QT_ID;
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



        public string InsertUpdateOrderType(QuotationMaster QM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "26");

                cmd.Parameters.AddWithValue("@OrderType", QM.OrderType);
                cmd.Parameters.AddWithValue("@OrderRate", QM.OrderRate);
                cmd.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", QM.Estimate_ManDays_ManMonth);
                cmd.Parameters.AddWithValue("@Estimate_ManMonth", QM.Estimate_ManMonth);
                cmd.Parameters.AddWithValue("@Distance", QM.Distance);
                cmd.Parameters.AddWithValue("@EstimatedAmount", QM.DEstimatedAmount);
                cmd.Parameters.AddWithValue("@Dcurrency", QM.Dcurrency);
                cmd.Parameters.AddWithValue("@DExchangeRate", QM.DExchangeRate);
                cmd.Parameters.AddWithValue("@DTotalAmount", QM.DTotalAmount);

                cmd.Parameters.AddWithValue("@PK_QTID", QM.PK_QTID);
                cmd.Parameters.AddWithValue("@TypeDI", QM.Type);


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

        public string InsertUpdateIOrderType(QuotationMaster QM) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "29");

                cmd.Parameters.AddWithValue("@OrderType", QM.IOrderType);
                cmd.Parameters.AddWithValue("@OrderRate", QM.IOrderRate);
                cmd.Parameters.AddWithValue("@Estimate_ManDays_ManMonth", QM.IEstimate_ManDays_ManMonth);
                cmd.Parameters.AddWithValue("@Estimate_ManMonth", QM.IEstimate_ManMonth);
                cmd.Parameters.AddWithValue("@Distance", QM.IDistance);
                cmd.Parameters.AddWithValue("@EstimatedAmount", QM.IEstimatedAmount);
                cmd.Parameters.AddWithValue("@Dcurrency", QM.Icurrency);
                cmd.Parameters.AddWithValue("@DExchangeRate", QM.IExchangeRate);
                cmd.Parameters.AddWithValue("@DTotalAmount", QM.ITotalAmount);
                cmd.Parameters.AddWithValue("@PK_QTID", QM.PK_QTID);
                cmd.Parameters.AddWithValue("@TypeDI", QM.Type);

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
        #endregion

        #region Code By Rahul 19-Aug-2019 (Copy Quotation)

        public DataTable CopyQuotation(string QuotationNumber , string Quotation_Description ,string Enquiry ,string CompanyName , string EndCustomer)//Edit Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", 12);
                CMDEditQM.Parameters.AddWithValue("@QuotationNumber", QuotationNumber);
                CMDEditQM.Parameters.AddWithValue("@Quotation_Description", Quotation_Description);
                CMDEditQM.Parameters.AddWithValue("@Enquiry", Enquiry);
                CMDEditQM.Parameters.AddWithValue("@CompanyName", CompanyName);
                CMDEditQM.Parameters.AddWithValue("@EndCustomer", EndCustomer);
               // CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;

        }


        #endregion


        #region New Copy Quotation
        public DataTable NewCopyQuotation(string QuotationNumber)//Edit Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", 20);
                CMDEditQM.Parameters.AddWithValue("@QuotationNumber", QuotationNumber);
                
                // CMDEditQM.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;

        }

        #endregion

        public string UpdateStatus(string AQntNo, string MQntNo) //=======Insert And update 
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "51");
                cmd.Parameters.AddWithValue("@QuotationNumber", MQntNo);
                cmd.Parameters.AddWithValue("@Reference", AQntNo);                
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

        //**********************Manoj Added code for Uploading File in database
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int QT_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_QTID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("QuotationNumber", typeof(string)));
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
                    DTUploadFile.Rows.Add(QT_ID, null, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_QuotationUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListQuotationUploadedFile", DTUploadFile);
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

        public DataTable DOrderType(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "27");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_QTID", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        public DataTable IOrderType(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "28");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_QTID", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        #region Bind Order Type on Job Page
        public DataTable DOrderTypeJob(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "30");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_QTID", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        public DataTable IOrderTypeJob(int? QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationMaster", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "31");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_QTID", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }
        #endregion


        public DataTable EditUploadedFile(int? QT_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_QTID", QT_ID);
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_QuotationUploadedFile", con);
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

        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_QuotationUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_QuotationUploadedFile", con);
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


        public DataSet GetClientRecord(string prefix)//Binding All Dropdownlist Quotation
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 16);
                CMDGetDdlLst.Parameters.AddWithValue("@CompanyName", prefix);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }


        public DataSet GetQuotationDescription(string prefix)//Binding All Dropdownlist Quotation
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 17);
                CMDGetDdlLst.Parameters.AddWithValue("@QuotationDescription", prefix);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        public DataSet GetEnquiryDescription(string prefix)//Binding All Dropdownlist Quotation
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 19);
                CMDGetDdlLst.Parameters.AddWithValue("@EnquiryDescription", prefix);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        //public DataSet GetQuotationDescription(string prefix)//Binding All Dropdownlist Quotation
        //{
        //    DataSet DSGetDdlList = new DataSet();
        //    try
        //    {
        //        SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationMaster", con);
        //        CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
        //        CMDGetDdlLst.CommandTimeout = 900000000;
        //        CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 18);
        //        CMDGetDdlLst.Parameters.AddWithValue("@QuotationDescription", prefix);
        //        SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
        //        SDAGetAllDdlLst.Fill(DSGetDdlList);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DSGetDdlList.Dispose();
        //    }
        //    return DSGetDdlList;
        //}


        public List<QuotationMaster> GetQuotationDescription(string ClientName, string QuotationDescription,string QuotationNumberSearch)
        {
            DataTable DTEMDashBoard = new DataTable();
            List<QuotationMaster> lstEnquiryDashB = new List<QuotationMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_QuotationMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "18");
                CMDGetEnquriy.Parameters.AddWithValue("@CompanyName", ClientName);
                CMDGetEnquriy.Parameters.AddWithValue("@QuotationDescription", QuotationDescription);
                CMDGetEnquriy.Parameters.AddWithValue("@QuotationNumberSearch", QuotationNumberSearch);
                //CMDGetEnquriy.Parameters.AddWithValue("@EnquiryDescription", EnquiryDescription);
                
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new QuotationMaster
                           {
                               ddlQuotationNumber = Convert.ToString(dr["QuotationNumber"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                               //PK_RM_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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


        public string InsertUpdateReport(QuotationMaster P)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDInsertCustDetail = new SqlCommand("SP_QuotationMaster", con);
                CMDInsertCustDetail.CommandType = CommandType.StoredProcedure;
                CMDInsertCustDetail.Parameters.AddWithValue("@SP_Type", 21);
                
                CMDInsertCustDetail.Parameters.AddWithValue("@QuotationPDF", P.QuotationPDF);
                CMDInsertCustDetail.Parameters.AddWithValue("@PK_QTID", P.PK_QTID);

                CMDInsertCustDetail.ExecuteNonQuery().ToString();
                

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

        public DataSet GetOtherServiceType(int QtID)
        {
            DataSet DSGetDdlList = new DataSet();

            
            con.Open();
            try
            {
                SqlCommand CMDInsertCustDetail = new SqlCommand("SP_QuotationMaster", con);
                CMDInsertCustDetail.CommandType = CommandType.StoredProcedure;
                CMDInsertCustDetail.Parameters.AddWithValue("@SP_Type", 22);                
                CMDInsertCustDetail.Parameters.AddWithValue("@PK_QTID", QtID);
                SqlDataAdapter SDAGetOtherType = new SqlDataAdapter(CMDInsertCustDetail);
                SDAGetOtherType.Fill(DSGetDdlList);


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
            return DSGetDdlList;
        }

        public DataTable ChkForJob(string PK_QTID)//Edit Quotaion Details
        {
            DataTable DTEditQM = new DataTable();
            try
            {
                SqlCommand CMDEditQM = new SqlCommand("SP_QuotationMaster", con);
                CMDEditQM.CommandType = CommandType.StoredProcedure;
                CMDEditQM.Parameters.AddWithValue("@SP_Type", 32);
                CMDEditQM.Parameters.AddWithValue("@PK_QTID", PK_QTID);

                SqlDataAdapter SDAEditQM = new SqlDataAdapter(CMDEditQM);
                SDAEditQM.Fill(DTEditQM);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditQM.Dispose();
            }
            return DTEditQM;

        }

    }
}