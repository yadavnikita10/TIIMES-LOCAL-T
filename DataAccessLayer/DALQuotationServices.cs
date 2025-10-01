using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALQuotationServices
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        #region Services

        

        public string InsertUpdateServiceImage(QuotationServices CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PKServiceId == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 1);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ServiceImage", CPM.ServiceImage);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ServiceName", CPM.ServiceName);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ServiceImage", CPM.ServiceImage);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ServiceName", CPM.ServiceName);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PKServiceId", CPM.PKServiceId);

                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();
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


        public DataSet GetImageById(int? PKServiceId)//Servicess by Id
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 3);
                CMDGetDdlLst.Parameters.AddWithValue("@PKServiceId", PKServiceId);
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

        public DataSet GetAllServicess()
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);

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

        public string Delete(int PKServiceId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationServiceLink", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PKServiceId", PKServiceId);
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

        #region Industries
        public DataSet GetServiceName()//Servicess by Id
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
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


        public DataSet GetAllIndustries()
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);

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


        public DataSet GetIndustryById(int? PKIndustryId)//Servicess by Id
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@PKIndustriesId", PKIndustryId);
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


        public string InsertUpdateIndustry(QuotationServices CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PKIndustriesId == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 8);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IndustryImage", CPM.IndustryImage);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IndustryName", CPM.IndustryName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@FkServiceId", CPM.FkServiceId);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 9);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@IndustryImage", CPM.IndustryImage);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@IndustryName", CPM.IndustryName);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@FkServiceId", CPM.FkServiceId);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PKIndustriesId", CPM.PKIndustriesId);
                    
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();
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

        public string DeleteIndustry(int PKIndustriesId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationServiceLink", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@PKIndustriesId", PKIndustriesId);
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

        public DataSet GetAllIndustryByServicId(string PkServiceId)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 17);
                CMDGetDdlLst.Parameters.AddWithValue("@FkServiceId", PkServiceId);
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
        #endregion


        #region Projects
        public DataSet GetIndustryName()//Get Industry
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 11);
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


        public string InsertUpdateProject(QuotationServices CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PKProjectId == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 12);

              
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@FkIndustryId", CPM.FkIndustryId);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ProjectImage", CPM.ProjectImage);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Title", CPM.Title);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Description", CPM.Description);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_QuotationServiceLink", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 13);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PKProjectId", CPM.PKProjectId);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@FkIndustryId", CPM.FkIndustryId);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ProjectImage", CPM.ProjectImage);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Title", CPM.Title);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Description", CPM.Description);

                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();
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

        public DataSet GetAllProjects()
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 14);

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

        public DataSet GetProjectsById(int PKProjectId)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 15);
                CMDGetDdlLst.Parameters.AddWithValue("@PKProjectId", PKProjectId);
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

        public string DeleteProject(int PKProjectId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_QuotationServiceLink", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "16");
                cmd.Parameters.AddWithValue("@PKProjectId", PKProjectId);
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


        public DataSet GetAllProjectByIndustryId(string FkIndustryId)
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_QuotationServiceLink", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 18);
                CMDGetDdlLst.Parameters.AddWithValue("@FkIndustryId", FkIndustryId);
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
        #endregion



    }
}