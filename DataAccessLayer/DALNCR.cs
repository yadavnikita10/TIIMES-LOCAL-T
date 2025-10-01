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
    public class DALNCR
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture; //added by nikita on 03102023
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);//added by nikita on 03102023

        public DataSet GetNCRDataById(int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@Id", Id);
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
        public DataSet ChkNCRDataById(int Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "8");
                cmd.Parameters.AddWithValue("@Id", Id);
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

        public DataSet ChkNCRDataByIVRID(int IVRId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                cmd.Parameters.AddWithValue("@IVRId", IVRId);
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

        public DataSet dsgetDataFromVisitReport(int IVRId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "6");
                cmd.Parameters.AddWithValue("@IVRId", IVRId);
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

        public int Insert(NCR N,string IPath)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (N.Id > 0)
                {
                    SqlCommand cmd = new SqlCommand("SP_NCR", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@Id", N.Id);
                    cmd.Parameters.AddWithValue("@NCRNo", N.NCRNo);
                    cmd.Parameters.AddWithValue("@TUVControlNo", N.TUVControlNo);
                    cmd.Parameters.AddWithValue("@ProjectName", N.ProjectName);
                    cmd.Parameters.AddWithValue("@Client", N.Client);
                    cmd.Parameters.AddWithValue("@VenderSubVendor", N.VenderSubVendor);
                    cmd.Parameters.AddWithValue("@ItemEquipment", N.ItemEquipment);
                    cmd.Parameters.AddWithValue("@ReferenceDocument", N.ReferenceDocument);
                    cmd.Parameters.AddWithValue("@DescriptionOfTheNonconformity", N.DescriptionOfTheNonconformity);
                    cmd.Parameters.AddWithValue("@NCRRaisedBy", N.NCRRaisedBy);
                    //cmd.Parameters.AddWithValue("@Date", N.Date);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(N.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@Attachment", IPath);
                    cmd.Parameters.AddWithValue("@Pdf", N.Pdf);
                    cmd.Parameters.AddWithValue("@IVRId", N.IVRId);
                    cmd.Parameters.AddWithValue("@Status", N.Status);
                    cmd.Parameters.AddWithValue("@SubVendorName", N.SubVendorName);
                    cmd.Parameters.AddWithValue("@IsConfirmation", N.IsComfirmation);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@CustomerSpecificReportNumber", N.CustomerSpecificReportNumber);
                    cmd.Parameters.AddWithValue("@ReviseReason", N.ddlReviseReason);
                    cmd.Parameters.AddWithValue("@ReviseDescription", N.ReviseReason);
                    cmd.Parameters.AddWithValue("@AttachedDocument", N.AttachedDoucment);
                    cmd.Parameters.AddWithValue("@ClosedReason", N.ClosedReason);

                    cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value.ToString());
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("SP_NCR", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@NCRNo", N.NCRNo);
                    cmd.Parameters.AddWithValue("@TUVControlNo", N.TUVControlNo);
                    cmd.Parameters.AddWithValue("@ProjectName", N.ProjectName);
                    cmd.Parameters.AddWithValue("@Client", N.Client);
                    cmd.Parameters.AddWithValue("@VenderSubVendor", N.VenderSubVendor);
                    cmd.Parameters.AddWithValue("@ItemEquipment", N.ItemEquipment);
                    cmd.Parameters.AddWithValue("@ReferenceDocument", N.ReferenceDocument);
                    cmd.Parameters.AddWithValue("@DescriptionOfTheNonconformity", N.DescriptionOfTheNonconformity);
                    cmd.Parameters.AddWithValue("@NCRRaisedBy", N.NCRRaisedBy);
                    //cmd.Parameters.AddWithValue("@Date", N.Date);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(N.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    cmd.Parameters.AddWithValue("@Attachment", IPath);
                    cmd.Parameters.AddWithValue("@Pdf", N.Pdf);
                    cmd.Parameters.AddWithValue("@IVRId", N.IVRId);
                    cmd.Parameters.AddWithValue("@Status", N.Status);
                    cmd.Parameters.AddWithValue("@SubVendorName", N.SubVendorName);
                    cmd.Parameters.AddWithValue("@IsConfirmation", N.IsComfirmation);
                    cmd.Parameters.AddWithValue("@AttachedDocument", N.AttachedDoucment);
                    cmd.Parameters.AddWithValue("@ClosedReason", N.ClosedReason);
                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));


                    cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value.ToString());

                    System.Web.HttpContext.Current.Session["NCRIDs"] = ReturnId;


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
            //return Result;
            return ReturnId;
        }


        public string InserPDF(string ReportName,int Id,string GNCRNO)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                
                    SqlCommand cmd = new SqlCommand("SP_NCR", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '7');
                    cmd.Parameters.AddWithValue("@Id", Id);
                    
                    cmd.Parameters.AddWithValue("@Pdf", ReportName);
                cmd.Parameters.AddWithValue("@NCRNo", GNCRNO);
                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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
        public DataSet GetGetData_()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "25");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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

        //added by nikita on 03102023
        public DataSet GetData_()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "22");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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


        public DataSet GetDataByBranchID(string BranchId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);

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

        public DataSet CheckUserRole()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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

        public string Delete(int Id)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@Id", Id);
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

        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_NCRID", typeof(int)));
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
                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_NCRUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListNCRUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_NCRUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_NCRID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_NCRUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_NCRUploadedFile", con);
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_NCRUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
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


        public int ReviseNCR(int? Id)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
              
                    SqlCommand cmd = new SqlCommand("SP_NCR", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "23");
                    cmd.Parameters.AddWithValue("@IVRId", Id);
                //cmd.Parameters.AddWithValue("@AttachedDocument", Id);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = cmd.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(cmd.Parameters["@ReturnId"].Value.ToString());
               
                


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
            //return Result;
            return ReturnId;
        }

        public string InsertFirstDownloadDate(int? PK_RM_ID)
        {
            string Result = string.Empty;
            string id = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_NCR", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "24");

                CMDInsertUpdatebranch.Parameters.AddWithValue("@Id", PK_RM_ID);
                CMDInsertUpdatebranch.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                id = CMDInsertUpdatebranch.Parameters["@ReturnID"].Value.ToString();


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
            return id;
        }

        public DataSet GetDataByBranchID(string BranchId,NCR obj)
        {
            DataSet ds = new DataSet();
           
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "26");
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(obj.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(obj.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet GetDataDatewise(NCR obj)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 27);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(obj.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(obj.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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

        public DataSet GetMISDataByBranchID(NCR obj)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_NCR", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "28");
               // cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(obj.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(obj.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
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