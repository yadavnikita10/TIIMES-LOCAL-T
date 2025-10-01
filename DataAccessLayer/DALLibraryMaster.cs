using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALLibraryMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);


        public DataTable GetLibraryDashBoard(int PK_SubSubID)
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_LibraryMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 1);
                CMDAppeal.Parameters.AddWithValue("@PK_SubSubID", PK_SubSubID);
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
        public DataTable GetLibraryDataByID(int? Id)
        {
            DataTable DTLibrary = new DataTable();
            try
            {
                SqlCommand CMDFolder = new SqlCommand("SP_LibraryMaster", con);
                CMDFolder.CommandType = CommandType.StoredProcedure;
                CMDFolder.Parameters.AddWithValue("@SP_Type", 4);
                CMDFolder.Parameters.AddWithValue("@Lib_Id", Id);
                CMDFolder.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDFolder);
                ad.Fill(DTLibrary);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTLibrary.Dispose();
            }
            return DTLibrary;
        }
        public string InsertUpdateLibraryData(Library APMas)
        {
            string result = string.Empty;
            con.Open();
            if (APMas.Lib_Id != 0)
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryMaster", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 3);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@FolderName", APMas.FolderName);
                    CMDAppeal.Parameters.AddWithValue("@PK_SubSubID", APMas.PK_SubSubID);
                    CMDAppeal.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserName"]);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            else
            {
                try
                { 
                SqlCommand CMDAppeal = new SqlCommand("SP_LibraryMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 2);
                CMDAppeal.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserName"]);
               
                CMDAppeal.Parameters.AddWithValue("@FolderName", APMas.FolderName);
                CMDAppeal.Parameters.AddWithValue("@PK_SubSubID", APMas.PK_SubSubID);
                result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            return result;
        }

        public int DeleteLibraryData(int? Lib_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_LibraryMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDContactDelete.Parameters.AddWithValue("@Lib_Id", Lib_Id);
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]));
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


        public DataTable GetLibraryDocsDashBoard(int Lib_Id)
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 1);
                CMDAppeal.Parameters.AddWithValue("@Lib_Id", Lib_Id);
                CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserID"]);
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

        public string InsertUpdateLibraryDocsData(LibraryDocumentModel APMas)
        {
            string result = string.Empty;
            con.Open();
            if (APMas.LP_Id != 0)
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 3);
                    CMDAppeal.Parameters.AddWithValue("@LP_Id", APMas.LP_Id);
                    CMDAppeal.Parameters.AddWithValue("@PDF", APMas.PDF);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@Created_by", System.Web.HttpContext.Current.Session["UserName"]);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            else
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 2);
                    CMDAppeal.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@PDF", APMas.PDF);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@UserID", APMas.UserID);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            return result;
        }

        public DataSet GetUserRoll()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_LibraryDocuments", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public int DeleteLibraryDocsData(int? LP_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_LibraryDocuments", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 6);
                CMDContactDelete.Parameters.AddWithValue("@LP_Id", LP_Id);
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserName"]));
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


        public DataSet CheckLibraryFoler(string BrName)
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_LibraryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@FolderName", BrName);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public string InsertUpdateCVData(LibraryDocumentModel APMas)
        {
            string result = string.Empty;
            con.Open();
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 6);
                    CMDAppeal.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@PDF", APMas.PDF);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@UserID", APMas.UserID);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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


        //public DataTable GetCV(int Lib_Id,string FolderName)
        //{
        //    DataTable DTAppeal = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
        //        CMDAppeal.CommandType = CommandType.StoredProcedure;
        //        CMDAppeal.Parameters.AddWithValue("@SP_Type", 7);
        //        //CMDAppeal.Parameters.AddWithValue("@Lib_Id", Lib_Id);
        //        CMDAppeal.Parameters.AddWithValue("@BranchName", FolderName);
        //        CMDAppeal.CommandTimeout = 100000;
        //        SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
        //        ad.Fill(DTAppeal);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTAppeal.Dispose();
        //    }
        //    return DTAppeal;
        //}

        public DataTable GetCV(int Lib_Id, string FolderName)
        {
            DataTable DSGetActivityData = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_LibraryDocuments", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "10");
                CMDGetDdlLst.Parameters.AddWithValue("@BranchName", FolderName);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetActivityData);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetActivityData.Dispose();
            }
            return DSGetActivityData;
        }

        public DataSet GetEditAllddlLst()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_LibraryMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 9);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public string InsertUpdateTutorial(Library APMas)
        {
            string result = string.Empty;
            con.Open();

            if (APMas.Lib_Id != 0)
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryMaster", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 11);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@FolderName", APMas.FolderName);


                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.PK_SubSubID);
                    CMDAppeal.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserName"]);

                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            else
            {
                try
                {

                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryMaster", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 10);
                    CMDAppeal.Parameters.AddWithValue("@FolderName", APMas.FolderName);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.PK_SubSubID);
                    CMDAppeal.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserName"]);

                    result = CMDAppeal.ExecuteNonQuery().ToString();


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
            }
            return result;
        }


        public string InsertUpdateTDocsData(LibraryDocumentModel APMas)
        {
            string result = string.Empty;
            con.Open();
            if (APMas.LP_Id != 0)
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 3);
                    CMDAppeal.Parameters.AddWithValue("@LP_Id", APMas.LP_Id);
                    CMDAppeal.Parameters.AddWithValue("@PDF", APMas.PDF);
                    CMDAppeal.Parameters.AddWithValue("@Roleid", APMas.UserRole);
                    CMDAppeal.Parameters.AddWithValue("@EmpCat", APMas.EmployementCategory);
                    CMDAppeal.Parameters.AddWithValue("@InspLoc", APMas.Location);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@Created_by", System.Web.HttpContext.Current.Session["UserName"]);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            else
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 2);
                    CMDAppeal.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserName"]);
                    CMDAppeal.Parameters.AddWithValue("@PDF", APMas.PDF);
                    CMDAppeal.Parameters.AddWithValue("@Roleid", APMas.UserRole);
                    CMDAppeal.Parameters.AddWithValue("@EmpCat", APMas.EmployementCategory);
                    CMDAppeal.Parameters.AddWithValue("@InspLoc", APMas.Location);
                    CMDAppeal.Parameters.AddWithValue("@Lib_Id", APMas.Lib_Id);
                    CMDAppeal.Parameters.AddWithValue("@UserID", APMas.UserID);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
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
            }
            return result;
        }


        public DataTable GetLibraryVideoDashBoard(int Lib_Id)
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_LibraryDocuments", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 11);
                CMDAppeal.Parameters.AddWithValue("@Lib_Id", Lib_Id);
                CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserID"]);
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
    }
}