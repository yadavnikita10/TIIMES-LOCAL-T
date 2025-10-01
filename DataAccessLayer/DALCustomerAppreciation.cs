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
   
    public class DALCustomerAppreciation
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

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

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CustomerAppreciationForm", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CustomerAppreciationUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
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

        public string DeleteFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CustomerAppreciationUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 4);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@PK_ID", FileID);
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

        public string Delete(int AuditId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CustomerAppreciationForm", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
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

        public DataTable GetUserName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_CustomerAppreciationForm", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 9);
                CMDSearchNameCode.Parameters.AddWithValue("@EmployeeName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }





    }
}