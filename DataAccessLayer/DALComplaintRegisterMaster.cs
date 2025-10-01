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
    public class DALComplaintRegisterMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable GetComplaintDashBoard() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 1);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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
        public DataTable GetUserBranch() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 21);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataTable GetComplaintDashBoardByDate(ComplaitRegister a)

        {
            string DateF = a.FromDate;


            string DateT = a.ToDate;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 8);
                CMDComplaintDash.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(DateF, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDComplaintDash.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(DateT, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                /// CMDComplaintDash.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateT, "dd/MM/yyyy", theCultureInfo));
                //CMDComplaintDash.Parameters.AddWithValue("@FromDate", DateF);
                //CMDComplaintDash.Parameters.AddWithValue("@ToDate", DateT);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataTable GetInspector(string BranchName) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 7);
                CMDComplaintDash.Parameters.AddWithValue("@BranchForInspector", BranchName);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataTable GetComplaint() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", "15N");
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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
       

        public DataTable AutoCompletDAta() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 6);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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
        public DataTable getcontrolNodata()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ComplaintRegisterMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 100000;
                cmd.Parameters.AddWithValue("@SP_Type", 9);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);

            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public string InsertUpdateComplaintData(ComplaitRegister CRCom)
        {
            string result = string.Empty;
            int ReturnId = 0;
            con.Open();
            if (CRCom.PK_Complaint_ID != 0)
            {
                try
                {
                    SqlCommand CMDComplaint = new SqlCommand("SP_ComplaintRegisterMaster", con);
                    CMDComplaint.CommandType = CommandType.StoredProcedure;
                    CMDComplaint.Parameters.AddWithValue("@SP_Type", '4');
                    CMDComplaint.Parameters.AddWithValue("@PK_Complaint_ID", CRCom.PK_Complaint_ID);
                    CMDComplaint.Parameters.AddWithValue("@Attachment", "NA"); //CRCom.Attachment
                    CMDComplaint.Parameters.AddWithValue("@Complaint_No", CRCom.Complaint_No);
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Date", DateTime.ParseExact(CRCom.Complaint_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Add(DateTime.Now.TimeOfDay));
                    // DateTime.ParseExact(CRCom.Complaint_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Add(DateTime.Now.TimeOfDay));
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Mode", CRCom.Complaint_Mode);
                    CMDComplaint.Parameters.AddWithValue("@Control_No", CRCom.Control_No);
                    CMDComplaint.Parameters.AddWithValue("@TUV_Client", CRCom.TUV_Client);
                    CMDComplaint.Parameters.AddWithValue("@Originating_Branch", CRCom.Originating_Branch);
                    CMDComplaint.Parameters.AddWithValue("@Executing_Branch", CRCom.Executing_Branch);
                    CMDComplaint.Parameters.AddWithValue("@Inspector_Name", CRCom.Inspector_Name);
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Details", CRCom.Complaint_Details);
                    CMDComplaint.Parameters.AddWithValue("@Correction", CRCom.Correction);
                    CMDComplaint.Parameters.AddWithValue("@Root_Cause", CRCom.Root_Cause);
                    CMDComplaint.Parameters.AddWithValue("@CA_To_Prevent_Recurrance", CRCom.CA_To_Prevent_Recurrance);
                    CMDComplaint.Parameters.AddWithValue("@Effectiveness_Of_Implementation_Of_CA", CRCom.Effectiveness_Of_Implementation_Of_CA);
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_Aknowledgement", DateTime.ParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", theCultureInfo));
                    DateTime dt;
                    if (DateTime.TryParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Aknowledgement", DateTime.ParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {

                        CMDComplaint.Parameters.Add("@Date_Of_Aknowledgement", SqlDbType.DateTime).Value = DBNull.Value;
                        //CMDComplaint.Parameters.Add("@Date_Of_Aknowledgement", DateTime.TryParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", null, DateTimeStyles.None, out dt));
                    }
                    DateTime dtDate_Of_FinalReply;
                    if (DateTime.TryParseExact(CRCom.Date_Of_FinalReply,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_FinalReply))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", DateTime.ParseExact(CRCom.Date_Of_FinalReply, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", null);
                    }
                    DateTime dtDate_Of_PreliminaryReply;
                    if (DateTime.TryParseExact(CRCom.Date_Of_PreliminaryReply,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_PreliminaryReply))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", DateTime.ParseExact(CRCom.Date_Of_PreliminaryReply, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", null);
                    }
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", DateTime.ParseExact(CRCom.Date_Of_PreliminaryReply, "dd/MM/yyyy", theCultureInfo));
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", DateTime.ParseExact(CRCom.Date_Of_FinalReply, "dd/MM/yyyy", theCultureInfo));
                    DateTime dtDate_Of_Action;
                    if (DateTime.TryParseExact(CRCom.Date_Of_Action,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_Action))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", DateTime.ParseExact(CRCom.Date_Of_Action, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", null);
                    }
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", DateTime.ParseExact(CRCom.Date_Of_PreliminaryReply, "dd/MM/yyyy", theCultureInfo));
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", DateTime.ParseExact(CRCom.Date_Of_FinalReply, "dd/MM/yyyy", theCultureInfo));

                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", DateTime.ParseExact(CRCom.Date_Of_Action, "dd/MM/yyyy", theCultureInfo));
                    CMDComplaint.Parameters.AddWithValue("@Category", CRCom.Category);
                    CMDComplaint.Parameters.AddWithValue("@Remarks", CRCom.Remarks);
                    CMDComplaint.Parameters.AddWithValue("@EndUser", CRCom.EndUser);
                    CMDComplaint.Parameters.AddWithValue("@States_Of_Complaints", CRCom.States_Of_Complaints);
                    //CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
                    DateTime dtLastDateOfInspection;
                    if (DateTime.TryParseExact(CRCom.LastDateOfInspection,
                                              "dd/MM/yyyy", null, DateTimeStyles.None, out dtLastDateOfInspection))
                    {
                        CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", null);
                    }
                    CMDComplaint.Parameters.AddWithValue("@AttributeToFaultiInspection", CRCom.AttributeToFaultiInspection);
                    CMDComplaint.Parameters.AddWithValue("@Vendor", CRCom.Vendor);
                    CMDComplaint.Parameters.AddWithValue("@SubVendor", CRCom.SubVendor);
                    CMDComplaint.Parameters.AddWithValue("@LessonLearned", CRCom.LessonLearned);
                    CMDComplaint.Parameters.AddWithValue("@ActionTaken", CRCom.ActionTaken);
                    CMDComplaint.Parameters.AddWithValue("@ProductCategory", CRCom.ProductList);
                    CMDComplaint.Parameters.AddWithValue("@ProjectName", CRCom.ProjectName);
                    CMDComplaint.Parameters.AddWithValue("@CallNo", CRCom.Call_no);
                    CMDComplaint.Parameters.AddWithValue("@ReferenceDocument", CRCom.ReferenceDocument);
                    CMDComplaint.Parameters.AddWithValue("@ShareLessonsLearnt", CRCom.ShareLessonsLearnt);
                    CMDComplaint.Parameters.AddWithValue("@Hide", CRCom.Hide);
                    CMDComplaint.Parameters.AddWithValue("@VendorPO", CRCom.VendorPO);
                    CMDComplaint.Parameters.AddWithValue("@SubVendorVendorPO", CRCom.SubVendorVendorPO);
                    CMDComplaint.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    result = CMDComplaint.ExecuteNonQuery().ToString();
                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }

            }
            else
            {
                try
                {
                    SqlCommand CMDComplaint = new SqlCommand("SP_ComplaintRegisterMaster", con);
                    CMDComplaint.CommandType = CommandType.StoredProcedure;
                    CMDComplaint.Parameters.AddWithValue("@SP_Type", '2');
                    CMDComplaint.Parameters.AddWithValue("@Complaint_No", CRCom.Complaint_No);
                    CMDComplaint.Parameters.AddWithValue("@Attachment", CRCom.Attachment);
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Date", DateTime.ParseExact(CRCom.Complaint_Date, "dd/MM/yyyy", theCultureInfo));
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Mode", CRCom.Complaint_Mode);
                    CMDComplaint.Parameters.AddWithValue("@Control_No", CRCom.Control_No);
                    CMDComplaint.Parameters.AddWithValue("@Effectiveness_Of_Implementation_Of_CA", CRCom.Effectiveness_Of_Implementation_Of_CA);
                    CMDComplaint.Parameters.AddWithValue("@TUV_Client", CRCom.TUV_Client);
                    CMDComplaint.Parameters.AddWithValue("@Originating_Branch", CRCom.Originating_Branch);
                    CMDComplaint.Parameters.AddWithValue("@Executing_Branch", CRCom.Executing_Branch);
                    CMDComplaint.Parameters.AddWithValue("@Inspector_Name", CRCom.Inspector_Name);
                    CMDComplaint.Parameters.AddWithValue("@Complaint_Details", CRCom.Complaint_Details);
                    CMDComplaint.Parameters.AddWithValue("@Correction", CRCom.Correction);
                    CMDComplaint.Parameters.AddWithValue("@Root_Cause", CRCom.Root_Cause);
                    CMDComplaint.Parameters.AddWithValue("@CA_To_Prevent_Recurrance", CRCom.CA_To_Prevent_Recurrance);
                    // CMDComplaint.Parameters.AddWithValue("@Date_Of_Aknowledgement", DateTime.ParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", theCultureInfo));
                    DateTime dt;
                    if (DateTime.TryParseExact(CRCom.Date_Of_Aknowledgement,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dt))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Aknowledgement", DateTime.ParseExact(CRCom.Date_Of_Aknowledgement, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Aknowledgement", null);
                    }

                    
                   
                    DateTime dtDate_Of_FinalReply;
                    if (DateTime.TryParseExact(CRCom.Date_Of_FinalReply,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_FinalReply))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", DateTime.ParseExact(CRCom.Date_Of_FinalReply, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", null);
                    }
                    DateTime dtDate_Of_PreliminaryReply;
                    if (DateTime.TryParseExact(CRCom.Date_Of_PreliminaryReply,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_PreliminaryReply))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", DateTime.ParseExact(CRCom.Date_Of_PreliminaryReply, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", null);
                    }
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_PreliminaryReply", DateTime.ParseExact(CRCom.Date_Of_PreliminaryReply, "dd/MM/yyyy", theCultureInfo));
                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_FinalReply", DateTime.ParseExact(CRCom.Date_Of_FinalReply, "dd/MM/yyyy", theCultureInfo));
                    DateTime dtDate_Of_Action;
                    if (DateTime.TryParseExact(CRCom.Date_Of_Action,
                                               "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate_Of_Action))
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", DateTime.ParseExact(CRCom.Date_Of_Action, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", null);
                    }

                    //CMDComplaint.Parameters.AddWithValue("@Date_Of_Action", DateTime.ParseExact(CRCom.Date_Of_Action, "dd/MM/yyyy", theCultureInfo));
                    CMDComplaint.Parameters.AddWithValue("@Category", CRCom.Category);
                    CMDComplaint.Parameters.AddWithValue("@Remarks", CRCom.Remarks);
                    CMDComplaint.Parameters.AddWithValue("@EndUser", CRCom.EndUser);
                    CMDComplaint.Parameters.AddWithValue("@States_Of_Complaints", CRCom.States_Of_Complaints);
                    CMDComplaint.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    //CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
                    DateTime dtLastDateOfInspection;
                    if (DateTime.TryParseExact(CRCom.LastDateOfInspection,
                                              "dd/MM/yyyy", null, DateTimeStyles.None, out dtLastDateOfInspection))
                    {
                        CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
                    }
                    else
                    {
                        CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", null);
                    }
                    CMDComplaint.Parameters.AddWithValue("@AttributeToFaultiInspection", CRCom.AttributeToFaultiInspection);
                    CMDComplaint.Parameters.AddWithValue("@Vendor", CRCom.Vendor);
                    CMDComplaint.Parameters.AddWithValue("@SubVendor", CRCom.SubVendor);
                    CMDComplaint.Parameters.AddWithValue("@LessonLearned", CRCom.LessonLearned);
                    CMDComplaint.Parameters.AddWithValue("@ActionTaken", CRCom.ActionTaken);
                    CMDComplaint.Parameters.AddWithValue("@ProductCategory", CRCom.ProductList);
                    CMDComplaint.Parameters.AddWithValue("@ProjectName", CRCom.ProjectName);
                    CMDComplaint.Parameters.AddWithValue("@CallNo", CRCom.Call_no);
                    CMDComplaint.Parameters.AddWithValue("@ReferenceDocument", CRCom.ReferenceDocument);
                    CMDComplaint.Parameters.AddWithValue("@ShareLessonsLearnt", CRCom.ShareLessonsLearnt);
                    CMDComplaint.Parameters.AddWithValue("@Hide", CRCom.Hide);
                    CMDComplaint.Parameters.AddWithValue("@VendorPO", CRCom.VendorPO);
                    CMDComplaint.Parameters.AddWithValue("@SubVendorVendorPO", CRCom.SubVendorVendorPO);

                    CMDComplaint.Parameters.Add("@GetCmpID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    result = CMDComplaint.ExecuteNonQuery().ToString();

                    ReturnId = Convert.ToInt32(CMDComplaint.Parameters["@GetCmpID"].Value);
                    System.Web.HttpContext.Current.Session["CMPIDs"] = ReturnId;
                }
                catch (Exception ex)
                {
                    string error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }

            }

            return result;
        }
        public DataTable EditComplaints(int? PK_Complaint_ID)
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditComplaints.Parameters.AddWithValue("@PK_Complaint_ID", PK_Complaint_ID);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }
        public int DeleteComplaint(int ComplaintID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDComplaintDelete = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDelete.CommandType = CommandType.StoredProcedure;
                CMDComplaintDelete.CommandTimeout = 100000;
                CMDComplaintDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDComplaintDelete.Parameters.AddWithValue("@PK_Complaint_ID", ComplaintID);
                //CMDRoleDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                Result = CMDComplaintDelete.ExecuteNonQuery();
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

        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_CMPID", typeof(int)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, Convert.ToString(ID)+'_'+item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListCMPUploadedFile", DTUploadFile);
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
        public DataTable EditUploadedFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CMPID", ID);
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

        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_CMPUploadedFile", con);
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
        #endregion


        public ComplaitRegister GetDataByControllNo(string Control_No)
        {
            con.Open();
            ComplaitRegister _vmcompany = new ComplaitRegister();
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_ComplaintRegisterMaster", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", 12);
                GetAddress.Parameters.AddWithValue("@SubJob_No", Control_No);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {
                    _vmcompany.EndUser = dr["EndUser"].ToString();
                    _vmcompany.Vendor = dr["VendorName"].ToString();
                    _vmcompany.SubVendor = dr["SubVendorName"].ToString();
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.CompanyName = dr["Company_Name"].ToString();
                    _vmcompany.VendorPO = dr["vendor_Po_No"].ToString();
                    _vmcompany.SubVendorVendorPO = dr["Subvendor_Po_No"].ToString();
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
            return _vmcompany;
        }

        #region Attachemnt Files


        public string InsertAttFileAttachment(List<FileDetails> lstFileUploaded, int ID, string AttType)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_CMPID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("AttType", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, AttType, Convert.ToString(ID) + '_' + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                }

                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_CMPAttUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListCMPAttUploadedFile", DTUploadFile);
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

        public DataTable EditAttUploadedFile(int? ID, string AttType)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CMPAttUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CMPID", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@AttType", AttType);
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

        public string DeleteAttUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
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

        #endregion


        #region Get complaint by Vendor Name and Item
        public DataTable GetComplaintByVendor(string Vendor) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 13);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]); 
                CMDComplaintDash.Parameters.AddWithValue("@Vendor", Vendor);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataTable GetComplaintByItem(string Call_No) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 14);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDComplaintDash.Parameters.AddWithValue("@Vendor", Call_No);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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



        #endregion

        public List<ComplaitRegister> GetComplaintCategory()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ComplaitRegister> lstEnquiryDashB = new List<ComplaitRegister>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 15);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ComplaitRegister
                           {
                               ComplaintCategoryId = Convert.ToString(dr["Id"]),
                               ComplaintCategoryName = Convert.ToString(dr["Name"]),
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


        public DataTable ShowSelectedDashBoard(string showData) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 16);
                CMDComplaintDash.Parameters.AddWithValue("@ShowData", showData);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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


        public DataTable GetExComplaintDashBoard() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 17);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataTable GetExComplaintDashBoardByDate(ComplaitRegister a)
        {
            string DateF = a.FromDate;


            string DateT = a.ToDate;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 18);
                CMDComplaintDash.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(DateF, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDComplaintDash.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(DateT, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public DataSet GetComplaintDetails(int ComplaintID) //User Role DashBoard
        {

            DataSet DTDashBoard = new DataSet();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type", 19);
                CMDComplaintDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDComplaintDash.Parameters.AddWithValue("@PK_Complaint_ID", ComplaintID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDComplaintDash);
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

        public string UpdateClosedMailFlag(int? PK_Complaint_ID)
        {
            DataTable DTEditComplaints = new DataTable();
            string result = string.Empty;
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 20);
                CMDEditComplaints.Parameters.AddWithValue("@PK_Complaint_ID", PK_Complaint_ID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);
                result = "1";

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return result;
        }


        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
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

        public DataTable GetFileContent2(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CMPAttUploadedFile", con);
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


        public string DeleteUploadedFileFormat(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CMPUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 6);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@PK_ID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public string DeleteUploadedFileFormat1(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CMPAttUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 6);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@PK_ID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetOrigBranch()
        {
            DataTable DTEditComplaints = new DataTable();
            try
            {
                SqlCommand CMDEditComplaints = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDEditComplaints.CommandType = CommandType.StoredProcedure;
                CMDEditComplaints.Parameters.AddWithValue("@SP_Type", 22);
                CMDEditComplaints.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditComplaints);
                SDAEditUsers.Fill(DTEditComplaints);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditComplaints.Dispose();
            }
            return DTEditComplaints;
        }



    }
}