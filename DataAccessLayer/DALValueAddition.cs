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
  public class DALValueAddition
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public ValueAddition GetDataByControllNo(string Control_No)
        {
            con.Open();
            ValueAddition _vmcompany = new ValueAddition();
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_ComplaintRegisterMaster", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", "12");
                GetAddress.Parameters.AddWithValue("@SubJob_No", Control_No);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {
                    
                    _vmcompany.ProjectName = dr["Project_Name"].ToString();
                    _vmcompany.CustomerName = dr["Company_Name"].ToString();
                    _vmcompany.VendorName = dr["VendorName"].ToString();
                    _vmcompany.SubVendorName = dr["SubVendorName"].ToString();
                   
                    
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


        public string Insert(ValueAddition SR)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if(SR.Id>0)
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

        public DataSet GetDataById(int id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
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

        public string InsertFileAttachment(List<FileDetails> lstFileUploaded,int ID,  ValueAddition CPM)
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
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_ValueAdditionUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
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

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
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

        public DataTable GetFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ValueAdditionUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ValueAdditionUploadedFile", con);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_ValueAdditionUploadedFile", con);
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

        public string Delete(int AuditId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ValueAddition", con);
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

        public DataTable GetValueADashBoard() //Company DashBoard List
        {

            DataTable DTCompanyDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCompanyDashBoard = new SqlCommand("SP_CompanyMaster", con);
                CMDCompanyDashBoard.CommandType = CommandType.StoredProcedure;
                CMDCompanyDashBoard.CommandTimeout = 100000;
                CMDCompanyDashBoard.Parameters.AddWithValue("@SP_Type", 1);
                CMDCompanyDashBoard.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDCompanyDashBoard.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCompanyDashBoard);
                SDADashBoardData.Fill(DTCompanyDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTCompanyDashBoard.Dispose();
            }

            return DTCompanyDashBoard;
        }


    }
}