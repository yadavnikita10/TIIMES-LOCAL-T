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
    public class DALBranchMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        #region  Branch master


        public DataTable GetBranchList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_BranchMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
               
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
        public string InsertUpdateBranch(BranchMasters CPM, string IPath)
        {
            string Result = string.Empty;
            int BranchID;
            con.Open();
            try
            {
                if (CPM.Br_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_BranchMaster", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch_Name", CPM.Branch_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch_Code", CPM.Branch_Code);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Manager", CPM.Manager);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Service_Code", CPM.Service_Code);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sequence_Number", CPM.Sequence_Number);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address1", CPM.Address1);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address2", CPM.Address2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address3", CPM.Address3);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Country", CPM.Country);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@State", CPM.State);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Postal_Code", CPM.Postal_Code);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Email_Id", CPM.Email_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CityName", CPM.CityName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@BranchAdmin", CPM.BranchAdmin);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Dom_Inter", CPM.Domesticinter);
                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Coordinator_Email_Id", CPM.Coordinator_Email_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                   
                    //Code Added by Manoj Sharma for Uploading Multiple Image
                    SqlParameter RequestID = CMDInsertUpdatebranch.Parameters.Add("@PK_MAX_BRID", SqlDbType.VarChar, 100);
                    CMDInsertUpdatebranch.Parameters["@PK_MAX_BRID"].Direction = ParameterDirection.Output;

                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();

                    BranchID = Convert.ToInt32(CMDInsertUpdatebranch.Parameters["@PK_MAX_BRID"].Value);
                    System.Web.HttpContext.Current.Session["BranchIDs"] = BranchID;

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_BranchMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch_Name", CPM.Branch_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch_Code", CPM.Branch_Code);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Manager", CPM.Manager);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Service_Code", CPM.Service_Code);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sequence_Number", CPM.Sequence_Number);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Address1", CPM.Address1);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Address2", CPM.Address2);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Address3", CPM.Address3);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Country", CPM.Country);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@State", CPM.State);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Postal_Code", CPM.Postal_Code);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Email_Id", CPM.Email_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CityName", CPM.CityName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Br_Id", CPM.Br_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@BranchAdmin", CPM.BranchAdmin);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Dom_Inter", CPM.Domesticinter);
                    if (IPath != null && IPath != "")
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", IPath);
                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Coordinator_Email_Id", CPM.Coordinator_Email_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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


        public DataSet EditBranch(int? Br_Id)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_BranchMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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


        public int DeleteBranch(int? Br_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_BranchMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDContactDelete.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDContactDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        #endregion

      //**********************Manoj Added code for Uploading File in database on 12 March 2020
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int BR_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_QTID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(BR_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_BranchUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListBranchUploadedFile", DTUploadFile);
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
        public DataTable EditUploadedFile(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_BranchUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_BRID", BR_ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_BranchUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_BranchUploadedFile", con);
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



    }
}