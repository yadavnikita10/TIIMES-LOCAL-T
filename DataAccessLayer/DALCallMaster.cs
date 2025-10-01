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
    public class DALCallMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataTable GetCallDashBoard() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_Virtual_Calls", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 1);
                CMDCallDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
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

        public DataTable GetCompanyName(string prefix) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_Virtual_Calls", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 10);
                CMDCallDash.Parameters.AddWithValue("@Company_Name", prefix);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
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

        public DataTable GetCallDashBoardByDate(Calls UserCalls) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_Virtual_Calls", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 8);
                //CMDCallDash.Parameters.AddWithValue("@FromDate", UserCalls.FromDate);
                //CMDCallDash.Parameters.AddWithValue("@ToDate", UserCalls.ToDate);
                //CMDCallDash.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(UserCalls.FromDate, "dd/MM/yyyy", null));
                //CMDCallDash.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(UserCalls.ToDate, "dd/MM/yyyy", null));

                CMDCallDash.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(UserCalls.FromDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDCallDash.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(UserCalls.ToDate, "dd/MM/yyyy", provider).ToString("MM/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));




                CMDCallDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
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
        //public DataTable GetDrpList()
        //{
        //    DataTable DTDashBoard = new DataTable();
        //    List<Calls> lst = new List<Calls>();

        //    try
        //    {
        //        SqlCommand CMDCallDash = new SqlCommand("sp_Virtual_Calls", con);
        //        CMDCallDash.CommandType = CommandType.StoredProcedure;
        //        CMDCallDash.CommandTimeout = 100000;
        //        CMDCallDash.Parameters.AddWithValue("@SP_Type", 6);
        //        SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
        //        SDADashBoardData.Fill(DTDashBoard);


        //    }

        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    return DTDashBoard;
        //}

        #region
        public List<Calls> GetDrpList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Calls> lstEnquiryDashB = new List<Calls>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("sp_Virtual_Calls", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 6);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Calls
                           {
                               Product_ID = Convert.ToInt32(dr["Product_ID"]),
                               Product_Name = Convert.ToString(dr["Name"]),
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

        public DataTable getlistforEdit()
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("sp_Virtual_Calls", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", 6);

            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }
        public DataTable getlistforEdit1()
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("sp_Virtual_Calls", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", "6New");

            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }


        public DataTable getJoblistforEdit(int? PK_SubJob_Id)
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("sp_CallsMaster", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", 63);
            CMDGetEdit.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);


            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }
        #endregion



        public DataTable GetProductDashBoard()
        {
            DataTable DTProductDashBoard = new DataTable();
            try
            {

                SqlCommand CMDProductDash = new SqlCommand("Sp_ProductMaster", con);
                CMDProductDash.CommandType = CommandType.StoredProcedure;
                CMDProductDash.CommandTimeout = 100000;
                CMDProductDash.Parameters.AddWithValue("@Sp_Type", 1);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDProductDash);
                SDADashBoardData.Fill(DTProductDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTProductDashBoard.Dispose();
            }
            return DTProductDashBoard;
        }
        public string InsertUpdateCallData(Calls ca, string IPath)
        {
            string result = string.Empty;
            int ReturnId = 0;
            con.Open();
            if (ca.PK_Call_ID != 0)
            {
                try
                {
                    SqlCommand CMDInsertCall = new SqlCommand("sp_Virtual_Calls", con);
                    CMDInsertCall.CommandType = CommandType.StoredProcedure;
                    CMDInsertCall.Parameters.AddWithValue("@SP_Type", '5');
                    CMDInsertCall.Parameters.AddWithValue("@Quantity", ca.Quantity);
                    CMDInsertCall.Parameters.AddWithValue("@Description", ca.Description);
                    CMDInsertCall.Parameters.AddWithValue("@Company_Name", ca.Company_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Po_Number", ca.PO_Number);
                    CMDInsertCall.Parameters.AddWithValue("@Last_Updated", ca.Last_Updated);
                    CMDInsertCall.Parameters.AddWithValue("@Assighned_To", ca.Assighned_To);
                    CMDInsertCall.Parameters.AddWithValue("@Po_Date", ca.PO_Date);
                    CMDInsertCall.Parameters.AddWithValue("@Contact_Name", ca.ContactName);
                    CMDInsertCall.Parameters.AddWithValue("@Project_Name", ca.Project_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Job_Location", ca.Job_Location);
                    //CMDInsertCall.Parameters.AddWithValue("@Status", ca.Status);
                    //CMDInsertCall.Parameters.AddWithValue("@Call_Recived_date", ca.Call_received_date);
                    // CMDInsertCall.Parameters.AddWithValue("@Call_Request_Date", ca.Call_Request_Date);



                    if (ca.Call_Request_Date != null)
                    {
                        CMDInsertCall.Parameters.AddWithValue("@Call_Request_Date", DateTime.ParseExact(ca.Call_Request_Date, "dd/MM/yyyy", null));
                    }

                    if (ca.Planned_Date != null)
                    {
                        CMDInsertCall.Parameters.AddWithValue("@Planned_Date", DateTime.ParseExact(ca.Planned_Date, "dd/MM/yyyy", null));
                    }
                    if (ca.Call_received_date != null)
                    {
                        CMDInsertCall.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(ca.Call_received_date, "dd/MM/yyyy", null));
                    }
                    CMDInsertCall.Parameters.AddWithValue("@category", ca.Category);
                    CMDInsertCall.Parameters.AddWithValue("@Source", ca.Source);
                    CMDInsertCall.Parameters.AddWithValue("@Urgency", ca.Urgency);
                    CMDInsertCall.Parameters.AddWithValue("@Sub_Category", ca.Sub_Category);
                    CMDInsertCall.Parameters.AddWithValue("@Type", ca.Type);
                    CMDInsertCall.Parameters.AddWithValue("@Vendor_Name", ca.Vendor_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Client_Email", ca.Client_Email);
                    CMDInsertCall.Parameters.AddWithValue("@Vendor_Email", ca.Vendor_Email);
                    CMDInsertCall.Parameters.AddWithValue("@PK_Call_ID", ca.PK_Call_ID);
                    CMDInsertCall.Parameters.AddWithValue("@Tuv_Branch", ca.Tuv_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@Product_item", ca.ProductList);
                    CMDInsertCall.Parameters.AddWithValue("@Sub_Job", ca.Sub_Job);
                    CMDInsertCall.Parameters.AddWithValue("@Attachment", IPath);
                    CMDInsertCall.Parameters.AddWithValue("@VirtualCallStatus", ca.VirtualCallStatus);
                    CMDInsertCall.Parameters.AddWithValue("@Executing_Branch", ca.Executing_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@Originating_Branch", ca.Originating_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@Job", ca.Job);
                    CMDInsertCall.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                    result = CMDInsertCall.ExecuteNonQuery().ToString();
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

                    //string s = ca.Call_received_date;
                    //DateTime a = DateTime.Parse(s);
                    //ca.Call_received_date = Convert.ToString(a);

                    SqlCommand CMDInsertCall = new SqlCommand("sp_Virtual_Calls", con);
                    CMDInsertCall.CommandType = CommandType.StoredProcedure;
                    CMDInsertCall.Parameters.AddWithValue("@SP_Type", '3');
                    CMDInsertCall.Parameters.AddWithValue("@Quantity", ca.Quantity);
                    CMDInsertCall.Parameters.AddWithValue("@Description", ca.Description);
                    CMDInsertCall.Parameters.AddWithValue("@Company_Name", ca.Company_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Po_Number", ca.PO_Number);
                    CMDInsertCall.Parameters.AddWithValue("@Last_Updated", ca.Last_Updated);
                    CMDInsertCall.Parameters.AddWithValue("@Assighned_To", ca.Assighned_To);
                    CMDInsertCall.Parameters.AddWithValue("@Po_Date", ca.PO_Date);
                    CMDInsertCall.Parameters.AddWithValue("@Contact_Name", ca.ContactName);
                    CMDInsertCall.Parameters.AddWithValue("@Project_Name", ca.Project_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Job_Location", ca.Job_Location);
                    CMDInsertCall.Parameters.AddWithValue("@Status", ca.Status);
                    //CMDInsertCall.Parameters.AddWithValue("@Call_Recived_date", ca.Call_received_date);
                    //CMDInsertCall.Parameters.AddWithValue("@Call_Request_Date", ca.Call_Request_Date);

                    if (ca.Call_Request_Date != null)
                    {
                        CMDInsertCall.Parameters.AddWithValue("@Call_Request_Date", DateTime.ParseExact(ca.Call_Request_Date, "dd/MM/yyyy", null));
                    }
                    if (ca.Call_received_date != null)
                    {
                        CMDInsertCall.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(ca.Call_received_date, "dd/MM/yyyy", null));
                    }

                    CMDInsertCall.Parameters.AddWithValue("@category", ca.Category);
                    CMDInsertCall.Parameters.AddWithValue("@Source", ca.Source);
                    CMDInsertCall.Parameters.AddWithValue("@Urgency", ca.Urgency);
                    CMDInsertCall.Parameters.AddWithValue("@Sub_Category", ca.Sub_Category);
                    CMDInsertCall.Parameters.AddWithValue("@Type", ca.Type);
                    CMDInsertCall.Parameters.AddWithValue("@Vendor_Name", ca.Vendor_Name);
                    CMDInsertCall.Parameters.AddWithValue("@Client_Email", ca.Client_Email);
                    CMDInsertCall.Parameters.AddWithValue("@Vendor_Email", ca.Vendor_Email);
                    CMDInsertCall.Parameters.AddWithValue("@Tuv_Branch", ca.Tuv_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@Call_No", ca.Call_No);
                    CMDInsertCall.Parameters.AddWithValue("@Product_item", ca.ProductList);
                    CMDInsertCall.Parameters.AddWithValue("@Sub_Job", ca.Sub_Job);
                    CMDInsertCall.Parameters.AddWithValue("@Attachment", IPath);
                    CMDInsertCall.Parameters.AddWithValue("@VirtualCallStatus", ca.VirtualCallStatus);
                    CMDInsertCall.Parameters.AddWithValue("@Executing_Branch", ca.Executing_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@Originating_Branch", ca.Originating_Branch);
                    CMDInsertCall.Parameters.AddWithValue("@AssignStatus", ca.AssignStatus);
                    CMDInsertCall.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);

                    CMDInsertCall.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    result = CMDInsertCall.ExecuteNonQuery().ToString();

                    ReturnId = Convert.ToInt32(CMDInsertCall.Parameters["@ReturnId"].Value.ToString());
                    System.Web.HttpContext.Current.Session["VCIDs"] = ReturnId;
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

        public DataTable EditCalls(int? PK_Call_ID)
        {
            DataTable DTEditCalls = new DataTable();
            try
            {
                SqlCommand CMDEditCalls = new SqlCommand("sp_Virtual_Calls", con);
                CMDEditCalls.CommandType = CommandType.StoredProcedure;
                CMDEditCalls.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditCalls.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditCalls);
                SDAEditUsers.Fill(DTEditCalls);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditCalls.Dispose();
            }
            return DTEditCalls;
        }



        public DataSet ChkRole()
        {
            DataSet DTEditCalls = new DataSet();
            try
            {
                SqlCommand CMDEditCalls = new SqlCommand("sp_Virtual_Calls", con);
                CMDEditCalls.CommandType = CommandType.StoredProcedure;
                CMDEditCalls.Parameters.AddWithValue("@SP_Type", 9);

                CMDEditCalls.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditCalls);
                SDAEditUsers.Fill(DTEditCalls);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditCalls.Dispose();
            }
            return DTEditCalls;
        }

        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID, InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_VCID", typeof(int)));

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
                    // DTUploadFile.Rows.Add(ID,CPM.PK_IVR_ID +"_"+ item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent);
                    DTUploadFile.Rows.Add(ID, CPM.PK_IVR_ID + "_" + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListVCUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_VCID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_VCUploadedFile", con);
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

        public DataSet ChkLeave(string ActualVisitDate, string Inspector)
        {
            DataSet DTGetTUVEmail = new DataSet();
            try
            {
                SqlCommand GetBREmail = new SqlCommand("SP_CallsMaster", con);
                GetBREmail.CommandType = CommandType.StoredProcedure;
                GetBREmail.Parameters.AddWithValue("@SP_Type", 60);

                /// GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(ActualVisitDate, "dd/MM/yyyy", null));

                GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(DateTime.ParseExact(ActualVisitDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                GetBREmail.Parameters.AddWithValue("@CreatedBy", Inspector);
                SqlDataAdapter dr = new SqlDataAdapter(GetBREmail);
                dr.Fill(DTGetTUVEmail);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetTUVEmail.Dispose();
            }
            return DTGetTUVEmail;
        }




        public DataSet ChkLeaveForCountinuous(string FromDate, string ToDate, string ActualVisitDate, string Inspector)
        {
            DataSet DTGetTUVEmail = new DataSet();
            try
            {
                SqlCommand GetBREmail = new SqlCommand("SP_CallsMaster", con);
                GetBREmail.CommandType = CommandType.StoredProcedure;
                GetBREmail.Parameters.AddWithValue("@SP_Type", 61);

                GetBREmail.Parameters.AddWithValue("@From_Date", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                GetBREmail.Parameters.AddWithValue("@To_Date", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(DateTime.ParseExact(ActualVisitDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                GetBREmail.Parameters.AddWithValue("@CreatedBy", Inspector);
                SqlDataAdapter dr = new SqlDataAdapter(GetBREmail);
                dr.Fill(DTGetTUVEmail);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetTUVEmail.Dispose();
            }
            return DTGetTUVEmail;
        }

        public List<CallsModel> GetInspectorSchedule()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CallsModel> lstEnquiryDashB = new List<CallsModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;

                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 65);


                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new CallsModel
                           {
                               //Count = DTEMDashBoard.Rows.Count,
                               //ContactNo = Convert.ToString(dr["ContactNo"]),

                               //added by nikita on 08-09-2023
                               Inspector = Convert.ToString(dr["Inspector"]),
                               Call_No = Convert.ToString(dr["Call_No"]),
                               InspectionLocation = Convert.ToString(dr["InspectionLocation"]),
                               CurrentAssignment = Convert.ToString(dr["CurrentAssignment"]),
                               EstimatedHours = Convert.ToString(dr["EstimatedHours"]),
                               InspectorAddress = Convert.ToString(dr["InspectorAddress"]),
                               VendorName = Convert.ToString(dr["Vendor_Name"]),
                               SubVendorName = Convert.ToString(dr["Sub_Vendor_Name"]),
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

        public DataTable GetInspectorScheduleFromDate(string FromDate)
        {
            DataTable DTEditCalls = new DataTable();
            try
            {
                SqlCommand CMDEditCalls = new SqlCommand("SP_CallsMaster", con);
                CMDEditCalls.CommandType = CommandType.StoredProcedure;
                CMDEditCalls.Parameters.AddWithValue("@SP_Type", 66);
                //CMDEditCalls.Parameters.AddWithValue("@Actual_Visit_Date", FromDate);
                CMDEditCalls.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                CMDEditCalls.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditCalls);
                SDAEditUsers.Fill(DTEditCalls);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditCalls.Dispose();
            }
            return DTEditCalls;
        }

        public DataTable getstageEdit()
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("sp_Virtual_Calls", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", 11);

            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }

        public DataSet MainBranch()// Binding Sales Masters DashBoard of Master Page 
        {
            DataSet DTBranchName = new DataSet();
            con.Open();
            try
            {
                SqlCommand CMDGet = new SqlCommand("sp_Virtual_Calls", con);
                CMDGet.CommandType = CommandType.StoredProcedure;
                CMDGet.CommandTimeout = 1000000;
                CMDGet.Parameters.AddWithValue("@SP_Type", 13);
                CMDGet.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDALoginDtl = new SqlDataAdapter(CMDGet);
                SDALoginDtl.Fill(DTBranchName);

            }

            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBranchName.Dispose();
            }
            return DTBranchName;
        }
    




}
}