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
    public class DalCustSpecific
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        CultureInfo provider = CultureInfo.InvariantCulture;

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type","13");
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


        public DataSet GetDataValueAddition()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type","14");
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
        public DataSet BindBranch()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Training", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 2);
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

        public DataSet GetInspectorName()
        {
            DataSet DTEditUploadedFile = new DataSet();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CustomerAppreciationForm", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "6");
                ////CMDEditUploadedFile.Parameters.AddWithValue("@FK_CustomerAppreciation", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CustomerAppreciationUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CustomerAppreciation", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataTable GetEventFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CustomerAppreciationUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CustomerAppreciation", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetDataById(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CustomerAppreciationForm", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Id", id);
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

        public string Delete(int AuditId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type","15");
                cmd.Parameters.AddWithValue("@Id", AuditId);
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

        public DataSet GetBranch()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '1');
                cmd.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string Insert(ModelCustomerAppreciation SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.Id > 0)
                {//Update
                    SqlCommand cmd = new SqlCommand("SP_CustomerAppreciationForm", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '3');
                    cmd.Parameters.AddWithValue("@Branch", SR.Branch);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@JobNumberWithSubJob", SR.JobNumberWithSubJob);
                    cmd.Parameters.AddWithValue("@ProjectName", SR.ProjectName);
                    cmd.Parameters.AddWithValue("@CustomerName", SR.CustomerName);
                    cmd.Parameters.AddWithValue("@VendorName", SR.VendorName);
                    cmd.Parameters.AddWithValue("@SubVendorName", SR.SubVendorName);
                    cmd.Parameters.AddWithValue("@EmployeeName", SR.EmployeeName);
                    cmd.Parameters.AddWithValue("@Id", SR.Id);
                    cmd.Parameters.AddWithValue("@PraisingQuote", SR.PraisingQuote);
                    cmd.Parameters.AddWithValue("@Mode", SR.Mode);
                    cmd.Parameters.AddWithValue("@PraisedBy", SR.PraisedBy);
                    cmd.Parameters.AddWithValue("@ShareasNews", SR.ShareasNews);
                    cmd.Parameters.AddWithValue("@CoverImage", SR.CoverImage);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.Add("@GetRetID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(SR.Id);
                    // Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_CustomerAppreciationForm", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@Branch", SR.Branch);
                    cmd.Parameters.AddWithValue("@UIIN", SR.UIIN);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@JobNumberWithSubJob", SR.JobNumberWithSubJob);
                    cmd.Parameters.AddWithValue("@ProjectName", SR.ProjectName);
                    cmd.Parameters.AddWithValue("@CustomerName", SR.CustomerName);
                    cmd.Parameters.AddWithValue("@VendorName", SR.VendorName);
                    cmd.Parameters.AddWithValue("@SubVendorName", SR.SubVendorName);
                    cmd.Parameters.AddWithValue("@EmployeeName", SR.EmployeeName);
                    cmd.Parameters.AddWithValue("@PraisingQuote", SR.PraisingQuote);
                    cmd.Parameters.AddWithValue("@Mode", SR.Mode);
                    cmd.Parameters.AddWithValue("@PraisedBy", SR.PraisedBy);
                    cmd.Parameters.AddWithValue("@ShareasNews", SR.ShareasNews);
                    cmd.Parameters.AddWithValue("@CoverImage", SR.CoverImage);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.Add("@GetRetID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(cmd.Parameters["@GetRetID"].Value);

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
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID, ModelCustomerAppreciation CPM, string attType)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_ValueAddition", typeof(int)));

                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, Convert.ToString(CPM.Id) + "_" + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, attType);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_CustomerAppreciationUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTCustomerAppreciationUploadedFile", DTUploadFile);
                    CMDSaveUploadedFile.CommandTimeout = 120;
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


        public List<Users> GetInspectorListForLeaveManagement()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("MisReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "9N");
                //CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               //PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

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


        public DataSet GetInspectorName_()
        {
            DataSet DTEditUploadedFile = new DataSet();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("MisReport", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "16");
                ////CMDEditUploadedFile.Parameters.AddWithValue("@FK_CustomerAppreciation", ID);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetFile_(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("MisReport", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "17");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_ValueAddition", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetDataById_(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "18");
                cmd.Parameters.AddWithValue("@Id", id);
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



        public string _Insert_(ValueAddition SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (SR.Id > 0)
                {//Update
                    SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '3');
                    cmd.Parameters.AddWithValue("@Branch", SR.Branch);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    cmd.Parameters.AddWithValue("@JobNumberWithSubJob", SR.JobNumberWithSubJob);
                    cmd.Parameters.AddWithValue("@ProjectName", SR.ProjectName);
                    cmd.Parameters.AddWithValue("@CustomerName", SR.CustomerName);
                    cmd.Parameters.AddWithValue("@VendorName", SR.VendorName);
                    cmd.Parameters.AddWithValue("@SubVendorName", SR.SubVendorName);
                    cmd.Parameters.AddWithValue("@EmployeeName", SR.EmployeeName);
                    cmd.Parameters.AddWithValue("@DescriptionOfValueAddition", SR.DescriptionOfValueAddition);
                    cmd.Parameters.AddWithValue("@Impact", SR.Impact);
                    cmd.Parameters.AddWithValue("@Remarks", SR.Remarks);
                    cmd.Parameters.AddWithValue("@Id", SR.Id);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.Add("@GetRetID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(SR.Id);
                    // Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@Branch", SR.Branch);
                    cmd.Parameters.AddWithValue("@UIIN", SR.UIIN);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(SR.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@JobNumberWithSubJob", SR.JobNumberWithSubJob);
                    cmd.Parameters.AddWithValue("@ProjectName", SR.ProjectName);
                    cmd.Parameters.AddWithValue("@CustomerName", SR.CustomerName);
                    cmd.Parameters.AddWithValue("@VendorName", SR.VendorName);
                    cmd.Parameters.AddWithValue("@SubVendorName", SR.SubVendorName);
                    cmd.Parameters.AddWithValue("@EmployeeName", SR.EmployeeName);
                    cmd.Parameters.AddWithValue("@DescriptionOfValueAddition", SR.DescriptionOfValueAddition);
                    cmd.Parameters.AddWithValue("@Impact", SR.Impact);
                    cmd.Parameters.AddWithValue("@Remarks", SR.Remarks);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.Add("@GetRetID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(cmd.Parameters["@GetRetID"].Value);

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



        public string InsertFileAttachment_V(List<FileDetails> lstFileUploaded, int ID, ValueAddition CPM)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_ValueAddition", typeof(int)));

                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, Convert.ToString(CPM.Id) + "_" + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("MisReport", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", "19");
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTValueAdditionUploadedFile", DTUploadFile);
                    CMDSaveUploadedFile.CommandTimeout = 120;
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

        public string Delete_valueAddition(int AuditId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("MisReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type","20");
                cmd.Parameters.AddWithValue("@Id", AuditId);
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



        public DataTable GetComplaintDashBoard() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDComplaintDash = new SqlCommand("SP_ComplaintRegisterMaster", con);
                CMDComplaintDash.CommandType = CommandType.StoredProcedure;
                CMDComplaintDash.CommandTimeout = 100000;
                CMDComplaintDash.Parameters.AddWithValue("@SP_Type","1");
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



        //code for apppealmaster dashboard

        public DataTable GetAppealDashBoard()
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("MisReport", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", "22");
                CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
                ad.Fill(DTAppeal);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAppeal.Dispose();
            }
            return DTAppeal;
        }

        public DataTable GetAppealDashBoardWithDate(AppealMaster a)
        {
            string DateF = a.FromDate;
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //DateTime DateFrom = DateTime.ParseExact(DateF, "yyyy-MM-dd", culture);

            string DateT = (a.ToDate);
            //IFormatProvider culture1 = new CultureInfo("en-US", true);
            //DateTime DateTo = DateTime.ParseExact(DateT, "yyyy-MM-dd", culture1);


            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("MisReport", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", "23");
                //CMDAppeal.Parameters.AddWithValue("@FromDate", DateF);
                CMDAppeal.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateF, "dd/MM/yyyy", theCultureInfo));
                //CMDAppeal.Parameters.AddWithValue("@ToDate", DateT);
                CMDAppeal.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateT, "dd/MM/yyyy", theCultureInfo));
                //CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
                ad.Fill(DTAppeal);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAppeal.Dispose();
            }
            return DTAppeal;
        }


        public DataSet BindBranchAppeal()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Audit", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
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



        public DataTable EditAppeal(int? Appeal_ID)
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("MisReport", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type","24");
                CMDAppeal.Parameters.AddWithValue("@Appeal_ID", Appeal_ID);
                SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
                ad.Fill(DTAppeal);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAppeal.Dispose();
            }
            return DTAppeal;
        }
        public int DeleteAppeal(int Appeal_ID)
        {
            int result = 0;
            con.Open();
            try
            {

                SqlCommand CMDAppeal = new SqlCommand("MisReport", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type","26");
                CMDAppeal.Parameters.AddWithValue("@Appeal_ID", Appeal_ID);
                result = CMDAppeal.ExecuteNonQuery();
                if (result != 0)
                {
                    return result;
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
            return result;
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
                    CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
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
                    CMDComplaint.Parameters.AddWithValue("@LastDateOfInspection", DateTime.ParseExact(CRCom.LastDateOfInspection, "dd/MM/yyyy", theCultureInfo));
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
                    DTUploadFile.Rows.Add(ID, Convert.ToString(ID) + '_' + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
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
        public DataSet BindAuditorName()
        {
            DataSet dsAuditorName = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Audit", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 2);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(dsAuditorName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsAuditorName.Dispose();
            }
            return dsAuditorName;

        }



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
        public List<Users> GetInspectorList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 9);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

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

    }
}