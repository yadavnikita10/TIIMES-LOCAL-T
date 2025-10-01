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
    public class DALCompanyMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection1"].ConnectionString);
        public DataTable GetCompanyDashBoard() //Company DashBoard List
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
        public DataSet GetAllDdlLst()//Get All DropDownlist 
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                //CMDGetDdlLst.Parameters.AddWithValue("",Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public int InsertUpdateCompany(CompanyMaster CM)
        {
            int CompanyId = 0;
            int CompanyId1 = 0;
            string Result = string.Empty;
            con.Open();
            con1.Open();
            try
            {
                if (CM.CMP_ID != 0 && CM.CMP_ID != null)
                {
                    SqlCommand CMDInsertUpdateCompany = new SqlCommand("SP_CompanyMaster", con);
                    CMDInsertUpdateCompany.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@CMP_ID", CM.CMP_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Company_Name", CM.Company_Name);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Email", CM.Email);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Website", CM.Website);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Work_Phone", CM.Work_Phone);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Mobile", CM.Mobile);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Title", CM.TitleName);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Ind_ID", CM.Ind_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Address", CM.Address);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Address_Account", CM.Address_Account);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Main", CM.Main);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Br_Id", CM.BranchName);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Fax_No", CM.Fax_No);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Branch_Description", CM.Branch_Description);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Pan_No", CM.Pan_No);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Status", CM.Status);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@CG_ID", CM.CG_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Home_Page", CM.Home_Page);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Fax_Account", CM.Fax_Account);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Contact", CM.Contact);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@OffAddrPin", CM.OffAddrPin);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SiteAddrPin", CM.SiteAddrPin);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@InspectionLocation", CM.InspectionLocation);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@isvendor", CM.vendorid);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateCompany.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateCompany.ExecuteNonQuery().ToString();
                    CompanyId = Convert.ToInt32(CMDInsertUpdateCompany.Parameters["@ReturnCompanyId"].Value.ToString());

                    SqlCommand InsertUpdateCompany = new SqlCommand("[SP_InsertDataJobSubJobAndCompany]", con1);
                    InsertUpdateCompany.CommandType = CommandType.StoredProcedure;
                    InsertUpdateCompany.Parameters.AddWithValue("@SP_Type", 6);
                    InsertUpdateCompany.Parameters.AddWithValue("@CMP_ID", CM.CMP_ID);
                    InsertUpdateCompany.Parameters.AddWithValue("@Company_Name", CM.Company_Name);
                    InsertUpdateCompany.Parameters.AddWithValue("@Email", CM.Email);
                    InsertUpdateCompany.Parameters.AddWithValue("@Website", CM.Website);
                    InsertUpdateCompany.Parameters.AddWithValue("@Work_Phone", CM.Work_Phone);
                    InsertUpdateCompany.Parameters.AddWithValue("@Mobile", CM.Mobile);
                    InsertUpdateCompany.Parameters.AddWithValue("@Title", CM.TitleName);
                    InsertUpdateCompany.Parameters.AddWithValue("@Ind_ID", CM.Ind_ID);
                    InsertUpdateCompany.Parameters.AddWithValue("@Address", CM.Address);
                    InsertUpdateCompany.Parameters.AddWithValue("@Address_Account", CM.Address_Account);
                    InsertUpdateCompany.Parameters.AddWithValue("@Main", CM.Main);
                    InsertUpdateCompany.Parameters.AddWithValue("@Br_Id", CM.BranchName);
                    InsertUpdateCompany.Parameters.AddWithValue("@Fax_No", CM.Fax_No);
                    InsertUpdateCompany.Parameters.AddWithValue("@Branch_Description", CM.Branch_Description);
                    InsertUpdateCompany.Parameters.AddWithValue("@Pan_No", CM.Pan_No);
                    InsertUpdateCompany.Parameters.AddWithValue("@Status", CM.Status);
                    InsertUpdateCompany.Parameters.AddWithValue("@CG_ID", CM.CG_ID);
                    InsertUpdateCompany.Parameters.AddWithValue("@Home_Page", CM.Home_Page);
                    InsertUpdateCompany.Parameters.AddWithValue("@Fax_Account", CM.Fax_Account);
                    InsertUpdateCompany.Parameters.AddWithValue("@Contact", CM.Contact);
                    InsertUpdateCompany.Parameters.AddWithValue("@OffAddrPin", CM.OffAddrPin);
                    InsertUpdateCompany.Parameters.AddWithValue("@SiteAddrPin", CM.SiteAddrPin);
                    InsertUpdateCompany.Parameters.AddWithValue("@InspectionLocation", CM.InspectionLocation);                  
                    InsertUpdateCompany.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    InsertUpdateCompany.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = InsertUpdateCompany.ExecuteNonQuery().ToString();
                    CompanyId1 = Convert.ToInt32(InsertUpdateCompany.Parameters["@ReturnCompanyId"].Value.ToString());

                }
                else
                {
                    SqlCommand CMDInsertUpdateCompany = new SqlCommand("SP_CompanyMaster", con);
                    CMDInsertUpdateCompany.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Company_Name", CM.Company_Name);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Email", CM.Email);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Website", CM.Website);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Work_Phone", CM.Work_Phone);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Mobile", CM.Mobile);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Title", CM.TitleName);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Ind_ID", CM.Ind_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Address", CM.Address);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Address_Account", CM.Address_Account);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Main", CM.Main);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Br_Id", CM.BranchName);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Fax_No", CM.Fax_No);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Branch_Description", CM.Branch_Description);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Pan_No", CM.Pan_No);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Status", CM.Status);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@CG_ID", CM.CG_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Home_Page", CM.Home_Page);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Fax_Account", CM.Fax_Account);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Contact", CM.Contact);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@OffAddrPin", CM.OffAddrPin);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SiteAddrPin", CM.SiteAddrPin);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@InspectionLocation", CM.InspectionLocation);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@New_Cmp", CM.New_Cmp);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@isvendor", CM.vendorid);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateCompany.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateCompany.ExecuteNonQuery().ToString();
                    CompanyId = Convert.ToInt32(CMDInsertUpdateCompany.Parameters["@ReturnCompanyId"].Value.ToString());

                    SqlCommand InsertUpdateCompany = new SqlCommand("[SP_InsertDataJobSubJobAndCompany]", con1);
                    InsertUpdateCompany.CommandType = CommandType.StoredProcedure;
                    InsertUpdateCompany.Parameters.AddWithValue("@SP_Type", 6);
                    InsertUpdateCompany.Parameters.AddWithValue("@Company_Name", CM.Company_Name);
                    InsertUpdateCompany.Parameters.AddWithValue("@Email", CM.Email);
                    InsertUpdateCompany.Parameters.AddWithValue("@Website", CM.Website);
                    InsertUpdateCompany.Parameters.AddWithValue("@Work_Phone", CM.Work_Phone);
                    InsertUpdateCompany.Parameters.AddWithValue("@Mobile", CM.Mobile);
                    InsertUpdateCompany.Parameters.AddWithValue("@Title", CM.TitleName);
                    InsertUpdateCompany.Parameters.AddWithValue("@Ind_ID", CM.Ind_ID);
                    InsertUpdateCompany.Parameters.AddWithValue("@Address", CM.Address);
                    InsertUpdateCompany.Parameters.AddWithValue("@Address_Account", CM.Address_Account);
                    InsertUpdateCompany.Parameters.AddWithValue("@Main", CM.Main);
                    InsertUpdateCompany.Parameters.AddWithValue("@Br_Id", CM.BranchName);
                    InsertUpdateCompany.Parameters.AddWithValue("@Fax_No", CM.Fax_No);
                    InsertUpdateCompany.Parameters.AddWithValue("@Branch_Description", CM.Branch_Description);
                    InsertUpdateCompany.Parameters.AddWithValue("@Pan_No", CM.Pan_No);
                    InsertUpdateCompany.Parameters.AddWithValue("@Status", CM.Status);
                    InsertUpdateCompany.Parameters.AddWithValue("@CG_ID", CM.CG_ID);
                    InsertUpdateCompany.Parameters.AddWithValue("@Home_Page", CM.Home_Page);
                    InsertUpdateCompany.Parameters.AddWithValue("@Fax_Account", CM.Fax_Account);
                    InsertUpdateCompany.Parameters.AddWithValue("@Contact", CM.Contact);
                    InsertUpdateCompany.Parameters.AddWithValue("@OffAddrPin", CM.OffAddrPin);
                    InsertUpdateCompany.Parameters.AddWithValue("@SiteAddrPin", CM.SiteAddrPin);
                    InsertUpdateCompany.Parameters.AddWithValue("@InspectionLocation", CM.InspectionLocation);
                    InsertUpdateCompany.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    InsertUpdateCompany.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = InsertUpdateCompany.ExecuteNonQuery().ToString();
                    CompanyId1 = Convert.ToInt32(InsertUpdateCompany.Parameters["@ReturnCompanyId"].Value.ToString());
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
                //if (con1.State != ConnectionState.Closed)
                //{
                //    con1.Close();
                //}
            }
            return CompanyId;
        }
        public DataSet EditCompany(int? PK_CMPID)
        {
            DataSet DSEditCompany = new DataSet();
            try
            {
                SqlCommand CMDEditCompany = new SqlCommand("SP_CompanyMaster", con);
                CMDEditCompany.CommandType = CommandType.StoredProcedure;
                CMDEditCompany.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditCompany.Parameters.AddWithValue("@CMP_ID", PK_CMPID);
                CMDEditCompany.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDEditCompany.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditCompany);
                SDAEditUsers.Fill(DSEditCompany);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSEditCompany.Dispose();
            }
            return DSEditCompany;
        }
        public int DeleteCompany(int? PK_CMPID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDCompanyDelete = new SqlCommand("SP_CompanyMaster", con);
                CMDCompanyDelete.CommandType = CommandType.StoredProcedure;
                CMDCompanyDelete.CommandTimeout = 100000;
                CMDCompanyDelete.Parameters.AddWithValue("@SP_Type", 6);
                CMDCompanyDelete.Parameters.AddWithValue("@CMP_ID", PK_CMPID);
                CMDCompanyDelete.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDCompanyDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDCompanyDelete.ExecuteNonQuery();
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
        public DataSet GetContactDdlList()//Get All DropDownlist 
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                //CMDGetDdlLst.Parameters.AddWithValue("",Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public string InsertUpdateContact(CompanyMaster CPM, string CompanyName)
        {
            string Result = string.Empty;
            string CompanyId = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_ContID != 0 )
                {
                    SqlCommand CMDInsertUpdateContact = new SqlCommand("SP_CompanyMaster", con);
                    CMDInsertUpdateContact.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateContact.Parameters.AddWithValue("@SP_Type", 13);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@PK_ContID", CPM.PK_ContID);
                    if(CompanyName !=null && CompanyName !="")
                    {
                        CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", CompanyName);
                    }
                    else
                    {
                        CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", CPM.CompanyName);
                    }
                    CMDInsertUpdateContact.Parameters.AddWithValue("@ContactName", CPM.ContactName);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", CPM.FK_CMP_ID);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Title", CPM.TitleName);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@HomePhone", CPM.HomePhone);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Mobile", CPM.Mobile);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Address", CPM.Address);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@FaxNo", CPM.Fax_No);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@PanNumber", CPM.Pan_No);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@IsMainContact", CPM.IsMainContact);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@ContactStatus", CPM.ContactStatus);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Email", CPM.Email);
                   // CMDInsertUpdateContact.Parameters.AddWithValue("@vendorid", CPM.vendorid);
                    //CMDInsertUpdateContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    CMDInsertUpdateContact.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                    //CompanyId =CMDInsertUpdateContact.Parameters["@ReturnCompanyId"].Value.ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateContact = new SqlCommand("SP_CompanyMaster", con);
                    CMDInsertUpdateContact.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateContact.Parameters.AddWithValue("@SP_Type", 5);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@ContactName", CPM.ContactName);
                    if (CompanyName != null && CompanyName != "")
                    {
                        CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", CompanyName);
                    }
                    else
                    {
                        CMDInsertUpdateContact.Parameters.AddWithValue("@CompanyName", CPM.CompanyName);
                    }
                    
                    CMDInsertUpdateContact.Parameters.AddWithValue("@FK_CMP_ID", CPM.FK_CMP_ID);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Title", CPM.TitleName);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@HomePhone", CPM.HomePhone);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Mobile", CPM.Mobile);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Address", CPM.Address);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@FaxNo", CPM.Fax_No);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@PanNumber", CPM.Pan_No);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@IsMainContact", CPM.IsMainContact);
                    CMDInsertUpdateContact.Parameters.AddWithValue("@ContactStatus", CPM.ContactStatus);
                    CMDInsertUpdateContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    CMDInsertUpdateContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateContact.Parameters.AddWithValue("@Email", CPM.Email);
                    //CMDInsertUpdateContact.Parameters.AddWithValue("@vendorid", CPM.vendorid);
                    Result = CMDInsertUpdateContact.ExecuteNonQuery().ToString();
                    CompanyId = CMDInsertUpdateContact.Parameters["@ReturnCompanyId"].Value.ToString();
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
            return CompanyId;
        }

        public DataTable ChkContactDetailExist(CompanyMaster CPM, string CompanyName)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CompanyMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 32);
                CMDEditContact.Parameters.AddWithValue("@ContactName", CPM.ContactName);
                CMDEditContact.Parameters.AddWithValue("@Mobile", CPM.Mobile);
                CMDEditContact.Parameters.AddWithValue("@Email", CPM.Email);
                CMDEditContact.Parameters.AddWithValue("@CompanyName", CPM.CompanyName);
                CMDEditContact.Parameters.AddWithValue("@FK_CMP_ID", CPM.FK_CMP_ID);

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


        public DataTable ChkContactDetailExistFromEnquiry(EnquiryMaster CPM, string CompanyName)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CompanyMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "16");
                CMDEditContact.Parameters.AddWithValue("@ContactName", CPM.ContactNames);
                CMDEditContact.Parameters.AddWithValue("@Mobile", CPM.Mobile);
                CMDEditContact.Parameters.AddWithValue("@Email", CPM.Email);
                CMDEditContact.Parameters.AddWithValue("@CompanyName", CPM.CompanyName);

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

        public DataTable EditContact(int? PK_ContID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CompanyMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditContact.Parameters.AddWithValue("@PK_ContID", PK_ContID);
                CMDEditContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public int DeleteContact(int? PK_ContID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_CompanyMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 10);
                CMDContactDelete.Parameters.AddWithValue("@PK_ContID", PK_ContID);
                CMDContactDelete.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDContactDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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
        public DataTable GetContactDashBoard()
        {
            DataTable DTGeContacttList = new DataTable();
            try
            {
                SqlCommand CMDGetContactList = new SqlCommand("SP_CompanyMaster",con);
                CMDGetContactList.CommandType = CommandType.StoredProcedure;
                CMDGetContactList.Parameters.AddWithValue("@SP_Type", 29);              
               // CMDGetContactList.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDGetContactList.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetContactList = new SqlDataAdapter(CMDGetContactList);
                SDAGetContactList.Fill(DTGeContacttList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGeContacttList.Dispose();
            }
            return DTGeContacttList;
        }
        public CompanyMaster GetAddressByName(string companyname)
        {
            con.Open();
            CompanyMaster _vmcompany = new CompanyMaster();
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_CompanyMaster", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", 14);
                GetAddress.Parameters.AddWithValue("@Company_Name", companyname);
                
                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {
                    _vmcompany.Address = dr["Address"].ToString();
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
        public DataTable DuplicateCompany(string DupCmpNm)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_CompanyMaster", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 15);
                DupCompany.Parameters.AddWithValue("@Company_Name", DupCmpNm);
                SqlDataAdapter dr = new SqlDataAdapter(DupCompany);
                dr.Fill(DTGetCompanyList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetCompanyList.Dispose();
            }
            return DTGetCompanyList;
        }

        public DataTable GetSiteAdressDet(int? CompanyID)
        {
            DataTable DTGetCompanyList = new DataTable();
            try
            {
                SqlCommand DupCompany = new SqlCommand("SP_CompanyMaster", con);
                DupCompany.CommandType = CommandType.StoredProcedure;
                DupCompany.Parameters.AddWithValue("@SP_Type", 17);
                DupCompany.Parameters.AddWithValue("@CMP_ID", CompanyID);
                SqlDataAdapter dr = new SqlDataAdapter(DupCompany);
                dr.Fill(DTGetCompanyList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetCompanyList.Dispose();
            }
            return DTGetCompanyList;
        }

        public int InsertUpdateSiteAddress(int CMP_ID, string SiteAdd, string SiteAddrPin,string AddressType)
        {
            int CompanyId = 0;
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CMP_ID != 0 && CMP_ID != null)
                {
                    SqlCommand CMDInsertUpdateCompany = new SqlCommand("SP_CompanyMaster", con);
                    CMDInsertUpdateCompany.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SP_Type", 18);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@CMP_ID", CMP_ID);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@Address_Account", SiteAdd);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@SiteAddrPin", SiteAddrPin);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@AddressType", AddressType);
                    CMDInsertUpdateCompany.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateCompany.ExecuteNonQuery().ToString();
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
            return CompanyId;
        }

        public string DeleteSiteAddress(string pkID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CompanyMaster", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 19);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SiteID", pkID);
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

        public DataSet GetCompanyAddr(string Company_Name)//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "20");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        public DataSet GetCompanyAddrForUpdate(string Company_Name)//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "21");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        //Added By Satish Pawar on 11 May 2023
        public DataSet GetCompanyEmailId(string Company_Name)
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "22");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }


        public DataSet GetCompanyAddrVendor(string Company_Name)//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "26");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }
        public DataSet GetCompanyAddrVendor_(string Company_Name)//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CompanyMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "24");
                CMDGetDdlLst.Parameters.AddWithValue("@Company_Name", Company_Name);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }


        public DataTable GetSapContactDashBoard(string CilientCode)
        {
            DataTable DTGeContacttList = new DataTable();
            try
            {
                SqlCommand CMDGetContactList = new SqlCommand("SP_CompanyMaster", con);
                CMDGetContactList.CommandType = CommandType.StoredProcedure;
                CMDGetContactList.Parameters.AddWithValue("@SP_Type", 31);
                CMDGetContactList.Parameters.AddWithValue("@ClientCode", CilientCode);
               // CMDGetContactList.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                CMDGetContactList.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetContactList = new SqlDataAdapter(CMDGetContactList);
                SDAGetContactList.Fill(DTGeContacttList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGeContacttList.Dispose();
            }
            return DTGeContacttList;
        }


        public DataTable GetcompanyContact(string PK_ContID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CompanyMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 30);
                CMDEditContact.Parameters.AddWithValue("@ClientCode", PK_ContID);
               // CMDEditContact.Parameters.Add("@ReturnCompanyId", SqlDbType.Int).Direction = ParameterDirection.Output;
                //CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetSearchCompanyName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_InactiveCustomer", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 1);
                CMDSearchNameCode.Parameters.AddWithValue("@SAPCustomer", CompanyName);
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


        public DataTable UpdateCustomer(string sapcustomer)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_InactiveCustomer", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 2);
                CMDSearchNameCode.Parameters.AddWithValue("@SAPCustomer", sapcustomer);
                CMDSearchNameCode.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataTable GetCompanyData()
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_InactiveCustomer", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 3);
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


        public DataTable GetCustomerJobData(string sapcustomer)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_InactiveCustomer", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 4);
                CMDSearchNameCode.Parameters.AddWithValue("@SAPCustomer", sapcustomer);
                //CMDSearchNameCode.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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