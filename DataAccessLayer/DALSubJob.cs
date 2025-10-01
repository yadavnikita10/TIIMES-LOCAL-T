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
    public class DALSubJob
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        string UserLoginID = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);

        SqlConnection con1 = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection1"].ConnectionString);
        #region  JOB Master

        public DataTable GetSubJOBList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", UserLoginID);
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

		#region  SubSub Job Data
        public DataTable GetSubSubJobList(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_subjobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 10);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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
        public DataSet GetSubJOBListExport()
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", UserLoginID);
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


        public string InsertUpdateSubJOb(SubJobs CPM)
        {
            string Result = string.Empty;
            string Result1 = string.Empty;
            con.Open();
           con1.Open();
            try
            {
                if (CPM.PK_SubJob_Id == 0)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Control_Number", CPM.Control_Number);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_QTID", CPM.PK_QTID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po",DateTime.ParseExact( CPM.Date_Of_PoDateTime,"dd/MM/yyyy",null));
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ReportType", "Expediting");
                    }
                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubJob_No", CPM.SubSubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Attachment", CPM.Attachment); 
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@orderstatus", CPM.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", UserLoginID);
					CMDInsertUpdateJOB.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@vendor_contactAllDetails", CPM.Vendor_ContactAll);
                    

                    CMDInsertUpdateJOB.Parameters.Add("@GetPkJobId", SqlDbType.VarChar, 30);
                    CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Direction = ParameterDirection.Output;                    

                    CMDInsertUpdateJOB.ExecuteNonQuery().ToString();

                    // Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@PK_JOB_ID"].Value);
                    Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Value);

                    System.Web.HttpContext.Current.Session["GetPkJobId"] = Result;
                    System.Web.HttpContext.Current.Session["SJIDs"] = Result;

                    //insert data 39 server

                    SqlCommand InsertUpdateJOB = new SqlCommand("SP_InsertDataJobSubJobAndCompany", con1);
                    InsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    InsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 3);
                    InsertUpdateJOB.Parameters.AddWithValue("@Control_Number", CPM.Control_Number);
                    InsertUpdateJOB.Parameters.AddWithValue("@PK_QTID", CPM.PK_QTID);
                    InsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    InsertUpdateJOB.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    InsertUpdateJOB.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    InsertUpdateJOB.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                    InsertUpdateJOB.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    InsertUpdateJOB.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po",DateTime.ParseExact( CPM.Date_Of_PoDateTime,"dd/MM/yyyy",null));
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        InsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    InsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    InsertUpdateJOB.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    InsertUpdateJOB.Parameters.AddWithValue("@SubSubJob_No", CPM.SubSubJob_No);
                    InsertUpdateJOB.Parameters.AddWithValue("@Status", CPM.Status);
                    InsertUpdateJOB.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    InsertUpdateJOB.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    InsertUpdateJOB.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    InsertUpdateJOB.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    InsertUpdateJOB.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    InsertUpdateJOB.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    InsertUpdateJOB.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    InsertUpdateJOB.Parameters.AddWithValue("@Type", CPM.Type);
                    InsertUpdateJOB.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    InsertUpdateJOB.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    InsertUpdateJOB.Parameters.AddWithValue("@orderstatus", CPM.Status);
                    InsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    InsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    InsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", UserLoginID);
                    InsertUpdateJOB.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    InsertUpdateJOB.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    InsertUpdateJOB.Parameters.Add("@GetPkJobId", SqlDbType.VarChar, 30);
                    InsertUpdateJOB.Parameters["@GetPkJobId"].Direction = ParameterDirection.Output;

                    InsertUpdateJOB.ExecuteNonQuery().ToString();

                    // Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@PK_JOB_ID"].Value);
                    Result1 = Convert.ToString(InsertUpdateJOB.Parameters["@GetPkJobId"].Value);

                    System.Web.HttpContext.Current.Session["GetPkJobId"] = Result;
                    System.Web.HttpContext.Current.Session["SJIDs"] = Result;

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ReportType", "Expediting");
                    }
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", UserLoginID);
					CMDInsertUpdateBranch.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);

                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();

                    //update data 39 server

                    SqlCommand InsertUpdateBranch = new SqlCommand("SP_InsertDataJobSubJobAndCompany", con1);
                    InsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    InsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 5);
                    InsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    InsertUpdateBranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    InsertUpdateBranch.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    InsertUpdateBranch.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        InsertUpdateBranch.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    InsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    InsertUpdateBranch.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    InsertUpdateBranch.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    InsertUpdateBranch.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);

                    InsertUpdateBranch.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    InsertUpdateBranch.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    InsertUpdateBranch.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    InsertUpdateBranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    InsertUpdateBranch.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    InsertUpdateBranch.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    InsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", UserLoginID);
                    InsertUpdateBranch.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    InsertUpdateBranch.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    InsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    InsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    InsertUpdateBranch.Parameters.AddWithValue("@vendor_contactAllDetails", CPM.Vendor_ContactAll);

                    Result1 = InsertUpdateBranch.ExecuteNonQuery().ToString();

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
                if (con1.State != ConnectionState.Closed)
                {
                    con1.Close();
                }
            }
            return Result;
        }

        public string InsertUpdateSubSubJOb(SubJobs CPM, int insupdate)
        {
            string Result = string.Empty;
            string Result1 = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_SubJob_Id > 0 && insupdate == 1)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Control_Number", CPM.Control_Number);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_QTID", CPM.PK_QTID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    // CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po",DateTime.ParseExact( CPM.Date_Of_PoDateTime,"dd/MM/yyyy",null));
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@ReportType", "Expediting");
                    }

                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubJob_No", CPM.SubSubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@orderstatus", CPM.Status);



                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", UserLoginID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSub_Vendor_Contact", CPM.SubSub_Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Subvendor_contactAllDetails", CPM.Vendor_ContactAll);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubvendor_contactAllDetails", CPM.subVendor_ContactAll);

                    //added by shrutika salve 20052024
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Subvendor_Email", CPM.subvendorEmailid);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubvendor_Email", CPM.subsubvendorEmailid);


                    CMDInsertUpdateJOB.Parameters.Add("@GetPkJobId", SqlDbType.VarChar, 30);
                    CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Direction = ParameterDirection.Output;

                    CMDInsertUpdateJOB.ExecuteNonQuery().ToString();

                    // Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@PK_JOB_ID"].Value);
                    Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Value);

                    System.Web.HttpContext.Current.Session["GetPkJobId"] = Result;
                    System.Web.HttpContext.Current.Session["SJIDs"] = Result;

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    if (CPM.checkIFExpeditingReport == true)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@ReportType", "Expediting");
                    }
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", UserLoginID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSub_Vendor_Contact", CPM.SubSub_Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Subvendor_contactAllDetails", CPM.Vendor_ContactAll);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSubvendor_contactAllDetails", CPM.subVendor_ContactAll);

                    //added by shrutika salve 20052024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Subvendor_Email", CPM.subvendorEmailid);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSubvendor_Email", CPM.subsubvendorEmailid);
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

        public DataSet EditSubJob(int? PK_SubJob_Id)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", UserLoginID);
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


        public DataTable EditListOfsubJob(int? Br_Id)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@Br_Id", Br_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", UserLoginID);
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

        #region
        public DataTable GetJobDataById(int? PK_JOB_ID)//Getting Data of Qutation Details
        {
            DataTable DTGetEnquiryDtls = new DataTable();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_SubJobMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 11);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
               CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", UserLoginID);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetEnquiryDtls.Dispose();
            }
            return DTGetEnquiryDtls;

        }

        public DataTable getDatajob(int?jobid)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDGetData = new SqlCommand("SP_SubJobMaster", con);
                CMDGetData.CommandType = CommandType.StoredProcedure;
                CMDGetData.Parameters.AddWithValue("@SP_Type",11);
                CMDGetData.Parameters.AddWithValue("@PK_JOB_ID", jobid);
                CMDGetData.Parameters.AddWithValue("@UserID", UserLoginID);
                SqlDataAdapter SDAGetData = new SqlDataAdapter(CMDGetData);
                SDAGetData.Fill(dt);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        #endregion


        #endregion

        public DataSet CheckQutationdata(int? PK_JOB_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_JobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditContact.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy",UserLoginID);
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

		 #region  Calls data
        public DataTable GetSubSubCallsList(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 59);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable GetSubJobCountBydata(int? PK_JOB_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 302);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_JOB_ID);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);
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

        public DataTable GetSubJobNoDetail(int? PK_JOB_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 303);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_JOB_ID);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);
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

        public DataTable GetJobDetailsCountBydata(int? PK_JOB_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 31);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                // CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", UserLoginID);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);               
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

        public DataTable GetJobCountBydata(int? PK_JOB_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 301);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);
                // CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", UserLoginID);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);
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


        public DataTable GetdataBySubJobNo(string SubJob_No)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "701");
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                //CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);

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

        //#region  Manoj Code
        //public DataTable GetEnquiryDetals(int? EQ_ID)//Getting Data of Enquiry Details
        //{
        //    DataTable DTGetEnquiryDtls = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_QuotationMaster", con);
        //        CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 3);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@EQ_ID", EQ_ID);
        //        CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //        SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
        //        SDAGetEnquiryDtls.Fill(DTGetEnquiryDtls);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTGetEnquiryDtls.Dispose();
        //    }
        //    return DTGetEnquiryDtls;

        //}


        //#endregion

        public List<SubJobs> GetSubJobNo()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<SubJobs> lstEnquiryDashB = new List<SubJobs>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_SubJobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 8);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new SubJobs
                           {
                               PK_SubJob_Id = Convert.ToInt32(dr["PK_SubJob_Id"]),
                               SubJob_No = Convert.ToString(dr["SubJob_No"]),

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


        public int DeleteSubJob(int? PK_SubJob_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_SubJobMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 9);
                CMDContactDelete.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetCompanyName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 6);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetCompanyAddress(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 26);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
                //CMDSearchNameCode.Parameters.AddWithValue("", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        #region  Calls data
        public DataTable GetCallsList(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 18);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        #region IVR IRN

        public DataTable GetReportByCall_Id(string SubJob_No)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 12);
                ///CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 13);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                CMDGetDdlLst.CommandTimeout = 300;
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

		public DataTable GetReportByJob(string SubJob_No)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 109);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                CMDGetDdlLst.CommandTimeout = 600;
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

        public DataTable GetIRNReportByCall_Id(string SubJob_No)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 13);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                CMDGetDdlLst.CommandTimeout = 300;
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

        public DataTable GetIRNReportByCallPage(int? PK_CALL_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "13N");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_CALL_ID", PK_CALL_ID);
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

        public DataTable GetNCRreports(string callId)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 16);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", callId);
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
        public DataTable GetNCRreportsByCallPage(string callId)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 111);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", callId);
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
        public DataTable GetNCRReportByCall_Id(string SubJob_No)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 14);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
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

        public DataSet GetSubJobNoByID(string SubJob_No)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ReportsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 15);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                
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

        #endregion

        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID,int? PK_Call_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_SJID", typeof(int)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("PK_call_id", typeof(int)));
                foreach (var item in lstFileUploaded)
                {
                    if (PK_Call_ID == 0)
                    {
                        item.FileName = Convert.ToString(ID) + '_' + item.FileName;
                    }
                    else
                    {
                        item.FileName = Convert.ToString(PK_Call_ID) + '_' + item.FileName;
                    }

                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent,PK_Call_ID);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListSJUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SJID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_SJUploadedFile", con);
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

        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", EQ_ID);
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
        #endregion


        public DataTable GetIRNReportByJobNo(string JobNo)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 110);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", JobNo);
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

        public DataTable GetCallSummary(int? PK_SubJob_Id)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 12);
                CMDEditContact.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
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

        public DataTable GetSiteCompanyAddress(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 45);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
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


        //  Added By shrutika salve for FileUpload   
        public string DetailsInsertFileAttachment(List<FileDetails> lstFileUploaded, int ID, int? PK_Call_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_SJID", typeof(int)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("PK_call_id", typeof(int)));
                foreach (var item in lstFileUploaded)
                {
                    if (PK_Call_ID == 0)
                    {
                        item.FileName = Convert.ToString(ID) + '_' + item.FileName;
                    }
                    else
                    {
                        item.FileName = Convert.ToString(PK_Call_ID) + '_' + item.FileName;
                    }

                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, PK_Call_ID);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_DetailsUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DetailsUploadedFile", DTUploadFile);
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



        public List<NameStatusEngineering> GetDDLStatus(string PK_JOB_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_SubJobMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "13");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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


        public List<NameStatusEngineering> GetMaterialStatus(string pk_jobno)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "21");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobno);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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


        public string Insertdata(string Status)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 14);
                CMDInsert.Parameters.AddWithValue("@Item", Status);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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

       


        public string InsertdataMaterial(string Status)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 15);
                CMDInsert.Parameters.AddWithValue("@Item", Status);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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


        public List<NameStatusEngineering> GetManufacturingStatus(string pk_jobno)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "25");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobno);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public string InsertdataManufacturing(string Status)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 16);
                CMDInsert.Parameters.AddWithValue("@Item", Status);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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
        public string InsertdataFinal(string Status)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 17);
                CMDInsert.Parameters.AddWithValue("@Item", Status);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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



        //Added BY Shrutika salve 12012023
        public string SaveStageWiseData(string status, string Value, string stage, string statusid, string PK_JOB_ID, string statusActionRequirdby)
        {

            string Result = "";
            con.Open();

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                SqlCommand CMD_SaveFeedbackEmailHistory = new SqlCommand("SP_ExpeditingReport", con);
                CMD_SaveFeedbackEmailHistory.CommandType = CommandType.StoredProcedure;
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@SP_Type", "40");
                //CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@pk_QT_id", pk_qt_id);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@stage", stage);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@Statusid", statusid);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@StatusName", status);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@StatuValue", Value);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@pk_jobid ", PK_JOB_ID);
                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@ActionReq ", statusActionRequirdby);

                CMD_SaveFeedbackEmailHistory.Parameters.AddWithValue("@createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));



                CMD_SaveFeedbackEmailHistory.ExecuteNonQuery();



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


        public string deleteRecordEng(string PK_QT_ID, string Stageid, string Statusid, string statusName)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 41);
                CMDInsert.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                CMDInsert.Parameters.AddWithValue("@Stages", Stageid);
                CMDInsert.Parameters.AddWithValue("@Statusid", Statusid);
                CMDInsert.Parameters.AddWithValue("@StatusName", statusName);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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

        public string deleteRecord(string Stageid, string Statusid, string statusName, string PK_JOB_ID)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 41);
                //CMDInsert.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                CMDInsert.Parameters.AddWithValue("@Stages", Stageid);
                CMDInsert.Parameters.AddWithValue("@Statusid", Statusid);
                CMDInsert.Parameters.AddWithValue("@StatusName", statusName);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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

        public string deleteRecordFinalStage(string statusName, string pk_jobid)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 44);
                CMDInsert.Parameters.AddWithValue("@Stage", statusName);
                CMDInsert.Parameters.AddWithValue("@PK_call_id", pk_jobid);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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


        public string deleteRecordDocument(string statusName, string pk_jobid)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_ExpeditingReport", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 45);
                CMDInsert.Parameters.AddWithValue("@Stage", statusName);
                CMDInsert.Parameters.AddWithValue("@PK_call_id", pk_jobid);

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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

        public List<NameStatusEngineering> GetddlFinalStageStatusMaster(string pk_jobno)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "29");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobno);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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




        public DataSet GetRecord(string PK_JOB_ID)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", 42);
                //cmd.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                cmd.Parameters.AddWithValue("@pk_jobid  ", PK_JOB_ID);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsClientFeedbackHistory);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsClientFeedbackHistory.Dispose();
            }
            return DsClientFeedbackHistory;
        }

        public List<NameStatusEngineering> GetFinalstage(string PK_JOB_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "28");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", PK_JOB_ID);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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


        public List<NameStatusEngineering> GetDocument(string pk_jobno)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NameStatusEngineering> lstEnquiryDashB = new List<NameStatusEngineering>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ExpeditingReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "43");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_JOB_ID", pk_jobno);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NameStatusEngineering
                           {
                               Value = Convert.ToInt32(dr["Value"]),
                               Text = Convert.ToString(dr["Text"]),
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

        public string InsertdataFinalstage(string Status, string PK_JOB_ID)
        {
            string Result = "";
            con.Open();
            try
            {

                IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 18);
                CMDInsert.Parameters.AddWithValue("@Item", Status);
                CMDInsert.Parameters.AddWithValue("@Pk_call_id", PK_JOB_ID);
                CMDInsert.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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



        public string InsertdataDocument(string Document, string PK_JOB_ID)
        {
            string Result = "";
            try
            {
                con.Open();
                SqlCommand CMDInsert = new SqlCommand("SP_SubJobMaster", con);
                CMDInsert.CommandType = CommandType.StoredProcedure;
                CMDInsert.Parameters.AddWithValue("@SP_Type", 19);
                CMDInsert.Parameters.AddWithValue("@Item", Document);
                CMDInsert.Parameters.AddWithValue("@Pk_call_id", PK_JOB_ID);
                CMDInsert.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));


                CMDInsert.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
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

        public DataSet GetFinalRecord(string PK_JOB_ID)
        {
            DataSet DsClientFeedbackHistory = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpeditingReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", 70);
                //cmd.Parameters.AddWithValue("@pk_QT_id", PK_QT_ID);
                cmd.Parameters.AddWithValue("@PK_JOB_ID  ", PK_JOB_ID);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsClientFeedbackHistory);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsClientFeedbackHistory.Dispose();
            }
            return DsClientFeedbackHistory;
        }

        //17/05/2024

        #region SubSubSub Job
        public DataSet EditSubSubJob(int? PK_SubJob_Id)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", UserLoginID);
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

        public DataTable GetSubSubJobCountBydata(int? PK_JOB_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_SubJobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 302);
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 20);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_JOB_ID);

                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDGetDdlLst);
                SDAEditContact.Fill(DSGetddlList);
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

        public string InsertUpdateSubSubSubJOb(SubJobs CPM, int insupdate)
        {
            string Result = string.Empty;
            string Result1 = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_SubJob_Id > 0 && insupdate == 1)
                {
                    SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", "21");
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Control_Number", CPM.Control_Number);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_QTID", CPM.PK_QTID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Service_type", CPM.Service_type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    // CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po",DateTime.ParseExact( CPM.Date_Of_PoDateTime,"dd/MM/yyyy",null));
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }

                    //CMDInsertUpdateJOB.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SAP_No", CPM.SAP_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubJob_No", CPM.SubSubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubSubJob_No", CPM.SubSubSubJob_No);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@orderstatus", CPM.Status);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSub_Vendor_Contact", CPM.SubSub_Vendor_Contact);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubvendor_contactAllDetails", CPM.subVendor_ContactAll);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Subvendor_Email", CPM.subvendorEmailid);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubSubvendor_Email", CPM.subsubvendorEmailid);




                    CMDInsertUpdateJOB.Parameters.AddWithValue("@CreatedBy", UserLoginID);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);

                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);

                    CMDInsertUpdateJOB.Parameters.Add("@GetPkJobId", SqlDbType.VarChar, 30);
                    CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Direction = ParameterDirection.Output;

                    CMDInsertUpdateJOB.ExecuteNonQuery().ToString();

                    // Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@PK_JOB_ID"].Value);
                    Result = Convert.ToString(CMDInsertUpdateJOB.Parameters["@GetPkJobId"].Value);

                    System.Web.HttpContext.Current.Session["GetPkJobId"] = Result;
                    System.Web.HttpContext.Current.Session["SJIDs"] = Result;

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_SubJobMaster", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 4);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Po_No", CPM.Vendor_Po_No);
                    if (CPM.Date_Of_PoDateTime != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_of_Po", DateTime.ParseExact(CPM.Date_Of_PoDateTime, "dd/MM/yyyy", null));
                    }
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_PoDateTime", CPM.Date_Of_PoDateTime);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@vendor_name", CPM.vendor_name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Tuv_Email", CPM.Tuv_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorPO_Amount", CPM.VendorPO_Amount);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorAddress", CPM.VendorAddress);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ModifyBy", UserLoginID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@EndUserName", CPM.End_User);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJobProjectName", CPM.SubJobProjectName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECName", CPM.DECName);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DECNumber", CPM.DECNumber);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSub_Vendor_Contact", CPM.SubSub_Vendor_Contact);
                    //added by shrutika salve 20052024
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Subvendor_Email", CPM.subvendorEmailid);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSubvendor_Email", CPM.subsubvendorEmailid);
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

        public DataTable GetSubSubSubJobListToDisplayOnCreateSubSub(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_subjobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 22);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataTable GetSubSubSubCallsList(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 80);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        public DataTable GetVendorDetails(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 60);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", CompanyName);
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


        public DataTable GetVendorEmailId(string CompanyName, string vendorcompany)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 61);
                CMDSearchNameCode.Parameters.AddWithValue("@ContactName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", vendorcompany);
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


        public DataTable GetVendorDetails(string CompanyName, string vendorcompany)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_EnquiryMaster", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 62);
                CMDSearchNameCode.Parameters.AddWithValue("@ContactName", CompanyName);
                CMDSearchNameCode.Parameters.AddWithValue("@CompanyName", vendorcompany);
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
        //added by nikita 
        public DataTable Validatecall_(int? PK_SubJob_Id)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_subjobMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "Call");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetDdlLst.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        //added by nikita on 10112024
        public DataTable GetInspectorList(string CompanyName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CompanyMaster> lstEnquiryDashB = new List<CompanyMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_EnquiryMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "74AA");
                CMDGetEnquriy.Parameters.AddWithValue("@CompanyName", CompanyName);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return DTEMDashBoard;
        }
    }
}