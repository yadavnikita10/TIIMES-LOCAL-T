using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using TuvVision.BusinessEntities;
using TuvVision.BusinessServices.Interface;
using TuvVision.Models;

namespace TuvVision.BusinessServices.Implementation
{
    public class NewsDetailss : INewsDetails
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        public int AddNewsDetails_New(News_VM _vminfo)
        {
            int i = 0;
            if (_vminfo.PK_NewsId == 0)
            {
                try
                {
                    SqlCommand addnewsinfo = new SqlCommand("SP_DisplayNews", con);
                    addnewsinfo.CommandType = CommandType.StoredProcedure;
                    addnewsinfo.Parameters.AddWithValue("@SP_Type", 1);
                    addnewsinfo.Parameters.AddWithValue("@NewsImage", _vminfo.NewsImage);
                    addnewsinfo.Parameters.AddWithValue("@NewsEvent", _vminfo.NewsEvent);
                    addnewsinfo.Parameters.AddWithValue("@Title", _vminfo.Title);
                    addnewsinfo.Parameters.AddWithValue("@ShortDescription", _vminfo.StortDescription);
                    addnewsinfo.Parameters.AddWithValue("@HtmlContent", _vminfo.HtmlContent);
                    addnewsinfo.Parameters.AddWithValue("@NewsFrom", DateTime.Now);
                    addnewsinfo.Parameters.AddWithValue("@NewsTo", DateTime.Now);
                    addnewsinfo.Parameters.AddWithValue("@Status", _vminfo.Status);
                    addnewsinfo.Parameters.AddWithValue("@CreatedBy", _vminfo.CreatedBy);
                    addnewsinfo.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    addnewsinfo.Parameters.Add("@GetPkNewsId", SqlDbType.VarChar, 30);
                    addnewsinfo.Parameters["@GetPkNewsId"].Direction = ParameterDirection.Output;
                    con.Open();
                    i = addnewsinfo.ExecuteNonQuery();
                    System.Web.HttpContext.Current.Session["NWIDs"] = Convert.ToString(addnewsinfo.Parameters["@GetPkNewsId"].Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if(con.State!=ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else if (_vminfo.PK_NewsId > 0)
            {
                try
                {
                    SqlCommand updatenewsdetails = new SqlCommand("SP_DisplayNews", con);
                    updatenewsdetails.CommandType = CommandType.StoredProcedure;
                    updatenewsdetails.Parameters.AddWithValue("@SP_Type", 3);
                    updatenewsdetails.Parameters.AddWithValue("@PK_NewsId", _vminfo.PK_NewsId);
                    updatenewsdetails.Parameters.AddWithValue("@NewsImage", _vminfo.NewsImage);
                    updatenewsdetails.Parameters.AddWithValue("@NewsEvent", _vminfo.NewsEvent);
                    updatenewsdetails.Parameters.AddWithValue("@Title", _vminfo.Title);
                    updatenewsdetails.Parameters.AddWithValue("@ShortDescription", _vminfo.StortDescription);
                    updatenewsdetails.Parameters.AddWithValue("@HtmlContent", _vminfo.HtmlContent);
                    updatenewsdetails.Parameters.AddWithValue("@NewsFrom", DateTime.Now);
                    updatenewsdetails.Parameters.AddWithValue("@NewsTo", DateTime.Now);
                    updatenewsdetails.Parameters.AddWithValue("@Status", _vminfo.Status);
                    updatenewsdetails.Parameters.AddWithValue("@CreatedBy", _vminfo.CreatedBy);
                    updatenewsdetails.Parameters.AddWithValue("@CreatedDate", _vminfo.CreatedDate);
                    con.Open();
                    i = updatenewsdetails.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return i;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        }
        public IEnumerable<News_VM> GetAllNewsDetails()
        {
            List<News_VM> NewsList = new List<News_VM>();
            try
            {
                SqlCommand getnewsdetails = new SqlCommand("SP_DisplayNews", con);
                getnewsdetails.CommandType = CommandType.StoredProcedure;
                getnewsdetails.Parameters.AddWithValue("@SP_Type", 2);
                con.Open();
                SqlDataReader dr = getnewsdetails.ExecuteReader();
                while (dr.Read())
                {
                    News_VM _vmnews = new News_VM();
                    _vmnews.PK_NewsId = Convert.ToInt32(dr["PK_NewsId"]);
                    _vmnews.NewsImage = dr["NewsImage"].ToString();
                    _vmnews.Title = dr["Title"].ToString();
                    _vmnews.StortDescription = dr["ShortDescription"].ToString();
                    _vmnews.HtmlContent = dr["HtmlContent"].ToString();
                    _vmnews.NewsFrom = Convert.ToDateTime(dr["NewsFrom"]);
                    _vmnews.NewsTo = Convert.ToDateTime(dr["NewsTo"]);
                    _vmnews.Status = Convert.ToInt32(dr["Status"]); //Status== 1 ? "Active" : Status == 2 ? "Inactive",
                  //  _vmnews.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    _vmnews.strCreatedDate = Convert.ToString(dr["CreatedDate"]);
                    NewsList.Add(_vmnews);
                }
                return NewsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        public News_VM GetNewsDataById(int? Id)
        {
            News_VM _vmgetbyid = new News_VM();
            try
            {
                SqlCommand getnewsbyid = new SqlCommand("SP_DisplayNews", con);
                getnewsbyid.CommandType = CommandType.StoredProcedure;
                getnewsbyid.Parameters.AddWithValue("@SP_Type", 5);
                getnewsbyid.Parameters.AddWithValue("@PK_NewsId", Id);
                con.Open();
                SqlDataReader dr = getnewsbyid.ExecuteReader();
                while (dr.Read())
                {
                    _vmgetbyid.PK_NewsId = Convert.ToInt32(dr["PK_NewsId"]);
                    _vmgetbyid.NewsImage = dr["NewsImage"].ToString();
                    _vmgetbyid.NewsEvent = dr["NewsEvent"].ToString();
                    _vmgetbyid.Title = dr["Title"].ToString();
                    _vmgetbyid.StortDescription = dr["ShortDescription"].ToString();
                    _vmgetbyid.HtmlContent = dr["HtmlContent"].ToString();
                    _vmgetbyid.NewsFrom = Convert.ToDateTime(dr["NewsFrom"]);
                    _vmgetbyid.NewsTo = Convert.ToDateTime(dr["NewsTo"]);
                    _vmgetbyid.Status = Convert.ToInt32(dr["Status"]);
                    _vmgetbyid.CreatedBy = dr["CreatedBy"].ToString();
                    _vmgetbyid.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                }
                return _vmgetbyid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public bool DeleteNewsById(int? Id)
        {
            try
            {
                SqlCommand deletenewsbyid = new SqlCommand("SP_DisplayNews", con);
                deletenewsbyid.CommandType = CommandType.StoredProcedure;
                deletenewsbyid.Parameters.AddWithValue("@SP_Type", 4);
                deletenewsbyid.Parameters.AddWithValue("@PK_NewsId", Id);
                deletenewsbyid.Parameters.AddWithValue("@Status", 0);
                con.Open();
                int i = deletenewsbyid.ExecuteNonQuery();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }
        public IEnumerable<News_VM> GetAllNewsDetailsA()
        {
            List<News_VM> NewsList = new List<News_VM>();
            try
            {
                SqlCommand getnewsdetails = new SqlCommand("SP_DisplayNews", con);
                getnewsdetails.CommandType = CommandType.StoredProcedure;
                getnewsdetails.Parameters.AddWithValue("@SP_Type", 6);
                con.Open();
                SqlDataReader dr = getnewsdetails.ExecuteReader();
                while (dr.Read())
                {
                    News_VM _vmnews = new News_VM();
                    _vmnews.PK_NewsId = Convert.ToInt32(dr["PK_NewsId"]);
                    _vmnews.NewsImage = dr["NewsImage"].ToString();
                    _vmnews.NewsEvent = dr["NewsEvent"].ToString();
                    _vmnews.Title = dr["Title"].ToString();
                    _vmnews.StortDescription = dr["ShortDescription"].ToString();
                    _vmnews.HtmlContent = dr["HtmlContent"].ToString();
                    _vmnews.NewsFrom = Convert.ToDateTime(dr["NewsFrom"]);
                    _vmnews.NewsTo = Convert.ToDateTime(dr["NewsTo"]);
                    _vmnews.Status = Convert.ToInt32(dr["Status"]); //Status== 1 ? "Active" : Status == 2 ? "Inactive",
                    _vmnews.strCreatedDate = Convert.ToString(dr["CreatedDate"]);
                    NewsList.Add(_vmnews);
                }
                return NewsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
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
                DTUploadFile.Columns.Add(new DataColumn("FK_NWID", typeof(int)));
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
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_NWUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListNWUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_NWUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_NWID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_NWUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_NWUploadedFile", con);
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

        public DataTable UpdateUserFlag()
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_DisplayNews", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 7);                
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        public DataTable GetEventImagesCAPP(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CustomerAppreciationForm", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditUploadedFile.Parameters.AddWithValue("@Id", ID);
                
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

        #endregion

    }
}