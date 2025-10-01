using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Globalization;

namespace TuvVision.DataAccessLayer
{
    public class DALIRN
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);


        #region  Inspection Report


        public DataTable GetInspectionList()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
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




        public string InsertUpdateInspectionvisit(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            string id = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_IVR_ID == 0)//Insert
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_IRNReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    if (CPM.UserBranch != 0)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.UserBranch);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    }

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Notification_Name_No_Date", CPM.Notification_Name_No_Date);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Date_Of_Inspection", CPM.Date_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Project_Name_Location", CPM.Project_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address_Of_Inspection", CPM.Address_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@End_user_Name", CPM.End_user_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Vendor_Name_Location", CPM.Vendor_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_Vendor_Name", CPM.Sub_Vendor_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No_SubVendor", CPM.Po_No_SubVendor);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Kick_Off_Pre_Inspection", CPM.Kick_Off_Pre_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Material_identification", CPM.Material_identification);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Interim_Stages", CPM.Interim_Stages);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_review", CPM.Document_review);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Re_inspection", CPM.Re_inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Conclusion", "All items were inspected within the scope defined in PO, approved QAP/ITP/QCP & specifications - found to meet the specified requirements. " + CPM.Conclusion);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Signatures", CPM.Signatures);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Report_No", CPM.Report_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRId", CPM.arrey);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRIRNAttachment", CPM.IVRIRNAttachment);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReviseReason", CPM.ReviseReason);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Emails_Distribution", CPM.Emails_Distribution);
                    

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@OrderStatus", CPM.OrderStatus);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_Order_Status", CPM.Sub_Order_Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ddlReviseReason", CPM.ddlReviseReason);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    //added by satish yadav for autosuggestion
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IsAutoSuggestion", CPM.IsAutoSuggestion);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Product_Item", CPM.Productlst.Trim());
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IsComfirmation", CPM.IsComfirmation);

                    //end autosuggestion code here
                    CMDInsertUpdatebranch.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                     id = CMDInsertUpdatebranch.Parameters["@ReturnID"].Value.ToString();

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_IRNReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Notification_Name_No_Date", CPM.Notification_Name_No_Date);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_Inspection", CPM.Date_Of_Inspection);
                    
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_No_SubVendor", CPM.Po_No_SubVendor);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IVRIRNAttachment", CPM.IVRIRNAttachment);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ReviseReason", CPM.ReviseReason);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Emails_Distribution", CPM.Emails_Distribution);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ClientEmail", CPM.client_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@VendorEmail", CPM.Vendor_Email);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@TUVEmail", CPM.Tuv_Branch);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CustomerSpecificReportNumber", CPM.CustomerSpecificReportNumber);

                    if (CPM.arrey==null)
                    {

                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@IVRId", CPM.arrey);
                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ddlReviseReason", CPM.ddlReviseReason);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Signatures", CPM.Signatures);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Type", CPM.Type);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", CPM.CreatedBy);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedDate", CPM.CreatedDate);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Report_No", CPM.Report_No);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@IVRId", CPM.arrey);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderStatus", CPM.OrderStatus);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Order_Status", CPM.Sub_Order_Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    //added by satish yadav for autosuggestion
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IsAutoSuggestion", CPM.IsAutoSuggestion);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Product_Item", CPM.Productlst.Trim());
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IsComfirmation", CPM.IsComfirmation);

                    //Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
                    //CMDInsertUpdateBranch.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    id = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
                    //id = CMDInsertUpdateBranch.Parameters["@ReturnID"].Value.ToString();
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
            return id;
        }
        public string UpdateReviseReport(int IvrID, string DAName, string DANo, string PANo, string SPANo, string RAReason)
        {
            string id = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDUpdateReviseReport = new SqlCommand("SP_IRNReport", con);
                CMDUpdateReviseReport.CommandType = CommandType.StoredProcedure;
                CMDUpdateReviseReport.Parameters.AddWithValue("@SP_Type", "30");
                CMDUpdateReviseReport.Parameters.AddWithValue("@PK_IVR_ID", Convert.ToInt32(IvrID));
                CMDUpdateReviseReport.Parameters.AddWithValue("@DEC_PMC_EPC_Name", Convert.ToString(DAName));
                CMDUpdateReviseReport.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", Convert.ToString(DANo));
                CMDUpdateReviseReport.Parameters.AddWithValue("@Po_No", Convert.ToString(PANo));
                CMDUpdateReviseReport.Parameters.AddWithValue("@Po_No_SubVendor", Convert.ToString(SPANo));
                CMDUpdateReviseReport.Parameters.AddWithValue("@ReviseReason", Convert.ToString(RAReason));
                CMDUpdateReviseReport.Parameters.AddWithValue("@Date_Of_Inspection", DateTime.Now.ToString("dd/MM/yyyy"));
                id = CMDUpdateReviseReport.ExecuteNonQuery().ToString();
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

        public string InsertUpdateConclousion(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            string id = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_IVR_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_IRNReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Notification_Name_No_Date", CPM.Notification_Name_No_Date);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Date_Of_Inspection", CPM.Date_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Project_Name_Location", CPM.Project_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address_Of_Inspection", CPM.Address_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@End_user_Name", CPM.End_user_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Vendor_Name_Location", CPM.Vendor_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_Vendor_Name", CPM.Sub_Vendor_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No_SubVendor", CPM.Po_No_SubVendor);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Kick_Off_Pre_Inspection", CPM.Kick_Off_Pre_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Material_identification", CPM.Material_identification);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Interim_Stages", CPM.Interim_Stages);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_review", CPM.Document_review);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Re_inspection", CPM.Re_inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Conclusion", CPM.Conclusion);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Signatures", CPM.Signatures);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Report_No", CPM.Report_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Waivers", CPM.Waivers);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DClient", CPM.client);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DVendor", CPM.vendot);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DTUV", CPM.TUV);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@POTotalCheckBox", CPM.POTotalCheckBox);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_IRNReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 5);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Conclusion", CPM.Conclusion);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Order_Status", CPM.Sub_Order_Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OrderStatus", CPM.OrderStatus);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Waivers", CPM.Waivers);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DClient", CPM.client);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DVendor", CPM.vendot);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DTUV", CPM.TUV);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@POTotalCheckBox", CPM.POTotalCheckBox);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    //CMDInsertUpdateBranch.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateBranch.ExecuteNonQuery().ToString();
                    //id = CMDInsertUpdateBranch.Parameters["@ReturnID"].Value.ToString();
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
        public string CopyInsertdataInspectionvisit(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            string id = string.Empty;
            con.Open();
            try
            {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_IRNReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    if (CPM.UserBranch != null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.UserBranch);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    }
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Notification_Name_No_Date", CPM.Notification_Name_No_Date);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Date_Of_Inspection", CPM.Date_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Project_Name_Location", CPM.Project_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Address_Of_Inspection", CPM.Address_Of_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@End_user_Name", CPM.End_user_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Vendor_Name_Location", CPM.Vendor_Name_Location);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_Vendor_Name", CPM.Sub_Vendor_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_No_SubVendor", CPM.Po_No_SubVendor);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Kick_Off_Pre_Inspection", CPM.Kick_Off_Pre_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Material_identification", CPM.Material_identification);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Interim_Stages", CPM.Interim_Stages);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_review", CPM.Document_review);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Re_inspection", CPM.Re_inspection);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Conclusion", CPM.Conclusion);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Signatures", CPM.Signatures);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Report_No", CPM.Report_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRId", CPM.arrey);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRIRNAttachment", CPM.IVRIRNAttachment);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReviseReason", CPM.ReviseReason);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Emails_Distribution", CPM.Emails_Distribution);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@OrderStatus", CPM.OrderStatus);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sub_Order_Status", CPM.Sub_Order_Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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


        public DataSet EditInspectionVisitReport(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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


        //public int DeleteBranch(int? Br_Id)
        //{
        //    int Result = 0;
        //    con.Open();
        //    try
        //    {
        //        SqlCommand CMDContactDelete = new SqlCommand("SP_BranchMaster", con);
        //        CMDContactDelete.CommandType = CommandType.StoredProcedure;
        //        CMDContactDelete.CommandTimeout = 100000;
        //        CMDContactDelete.Parameters.AddWithValue("@SP_Type", 5);
        //        CMDContactDelete.Parameters.AddWithValue("@Br_Id", Br_Id);
        //        CMDContactDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
        //        Result = CMDContactDelete.ExecuteNonQuery();
        //        if (Result != 0)
        //        {
        //            return Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }

        //    }
        //    return Result;
        //}


        public DataSet GetCallDetails(int? PK_Call_ID)//Getting Data of Qutation Details
        {
            DataSet DTGetCallDtls = new DataSet();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_Call_ID ", PK_Call_ID);
                // CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiryDtls = new SqlDataAdapter(CMDGetEnquiryDtls);
                SDAGetEnquiryDtls.Fill(DTGetCallDtls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetCallDtls.Dispose();
            }
            return DTGetCallDtls;

        }



        public DataSet EditInspectionVisitReportByPKCallID(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_IRNReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
                CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                CMDEditContact.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                SDAEditContact.Fill(DTEditContact);
               string id = CMDEditContact.Parameters["@ReturnID"].Value.ToString();
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

        #region  Item Description
        public DataTable GetitemDescription(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ItemDescription", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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
        public DataTable GetLatestIRN(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetLatestIRN = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_IRNReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 100);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetLatestIRN);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetLatestIRN.Dispose();
            }
            return DSGetLatestIRN;
        }

        public DataSet GetitemDescriptionById(int? PK_ItemD_Id)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ItemDescription", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_ItemD_Id", PK_ItemD_Id);
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

        public string InsertUpdateItemDescription(ItemDescriptionModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_ItemD_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ItemDescription", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Item_No", CPM.Po_Item_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ItemCode_Description", CPM.ItemCode_Description);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Quantity", CPM.Po_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Offered_Quantity", CPM.Offered_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Item_Code", CPM.Item_Code);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Accepted_Quantity", CPM.Accepted_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Cumulative_Accepted_Qty", CPM.Cumulative_Accepted_Qty);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@POTotalCheckBox", CPM.POTotalCheckBox);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Unit", CPM.Unit);


                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ItemDescription", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_ItemD_Id", CPM.PK_ItemD_Id);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Po_Item_No", CPM.Po_Item_No);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ItemCode_Description", CPM.ItemCode_Description);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Po_Quantity", CPM.Po_Quantity);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Offered_Quantity", CPM.Offered_Quantity);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);

                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Item_Code", CPM.Item_Code);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Accepted_Quantity", CPM.Accepted_Quantity);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Cumulative_Accepted_Qty", CPM.Cumulative_Accepted_Qty);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Unit", CPM.Unit);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@POTotalCheckBox", CPM.POTotalCheckBox);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public int DeleteItemDescription(int? PK_ItemD_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_ItemDescription", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 6);
                CMDContactDelete.Parameters.AddWithValue("@PK_ItemD_Id", PK_ItemD_Id);
                //CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string InsertItemDescription(ItemDescriptionModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ItemDescription", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Item_No", CPM.Po_Item_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ItemCode_Description", CPM.ItemCode_Description);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Po_Quantity", CPM.Po_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Offered_Quantity", CPM.Offered_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Item_Code", CPM.Item_Code);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Accepted_Quantity", CPM.Accepted_Quantity);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Cumulative_Accepted_Qty", CPM.Cumulative_Accepted_Qty);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Unit", CPM.Unit);


                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
               

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

        #region   Refrance Documents
        public DataTable GetReferenceDocuments(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Reference_Documents", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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


        public string InsertUpdateReferenceDocuments(ReferenceDocumentsModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_RD_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_Reference_Documents", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "2N");
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_No", CPM.Document_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_Name", CPM.Document_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Approval_Status", CPM.Approval_Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@VendorDocumentNumber", Convert.ToString(CPM.VendorDocumentNumber));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_Reference_Documents", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_RD_ID", CPM.PK_RD_ID);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Document_Name", CPM.Document_Name);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Document_No", CPM.Document_No);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Approval_Status", CPM.Approval_Status);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@VendorDocumentNumber", Convert.ToString(CPM.VendorDocumentNumber));
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

        public DataSet GetRefranceDocuments(int? PK_RD_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Reference_Documents", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_RD_ID", PK_RD_ID);
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


        public int DeleteRefranceDocuments(int? PK_RD_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_Reference_Documents", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 7);
                CMDContactDelete.Parameters.AddWithValue("@PK_RD_ID", PK_RD_ID);
                //CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertReferenceDocuments(ReferenceDocumentsModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
               
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_Reference_Documents", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_No", CPM.Document_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_Name", CPM.Document_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Approval_Status", CPM.Approval_Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
               

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




        #region    Inspection Activites
        public DataTable GetInspectionActivities(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionActivites", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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


        public string InsertUpdateInspectionActivities(InspectionActivitiesModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_IA_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionActivites", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Stages_Witnessed", CPM.Stages_Witnessed);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_InspectionActivites", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IA_ID", CPM.PK_IA_ID);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Stages_Witnessed", CPM.Stages_Witnessed);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetInspectionActivitiesByPKIAID(int? PK_IA_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionActivites", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IA_ID", PK_IA_ID);
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


        public int DeleteInspectionActivities(int? PK_IA_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_InspectionActivites", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 8);
                CMDContactDelete.Parameters.AddWithValue("@PK_IA_ID", PK_IA_ID);
                //CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string InsertInspectionActivities(InspectionActivitiesModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
               
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionActivites", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Stages_Witnessed", CPM.Stages_Witnessed);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
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



        #region    Document Reviewe
        public DataTable GetDocumentRevieweModelByCall_Id(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_DocumentsReviewed", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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


        public string InsertUpdateDocumentReviewe(DocumentRevieweModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_DR_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_DocumentsReviewed", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_DocumentsReviewed", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_DR_ID", CPM.PK_DR_ID);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetDocumentRevieweModelById(int? PK_DR_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionActivites", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 7);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_DR_ID", PK_DR_ID);
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


        public int DeleteDocumentReviewe(int? PK_DR_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_DocumentsReviewed", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 7);
                CMDContactDelete.Parameters.AddWithValue("@PK_DR_ID", PK_DR_ID);
                //CMDContactDelete.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public string InsertDocumentReviewe(DocumentRevieweModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
               
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_DocumentsReviewed", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
               

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


        #region   Equipments Details
        public DataTable GetEquipmentDetailsModelByCall_Id(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EquipmentsDetails", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string InsertUpdateEquipmentDetails(EquipmentDetailsModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_DOE_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_EquipmentsDetails", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Name_Of_Equipments", CPM.Name_Of_Equipments);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Range", CPM.Range);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Id", CPM.Id);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CalibrationValid_Till_date", CPM.CalibrationValid_Till_date);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Certification_No_Date", CPM.Certification_No_Date);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_EquipmentsDetails", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_DOE_Id", CPM.PK_DOE_Id);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Name_Of_Equipments", CPM.Name_Of_Equipments);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Range", CPM.Range);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Id", CPM.Id);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CalibrationValid_Till_date", CPM.CalibrationValid_Till_date);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Certification_No_Date", CPM.Certification_No_Date);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetEquipmentDetailsById(int? PK_DOE_Id)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_EquipmentsDetails", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_DOE_Id", PK_DOE_Id);
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


        #region   ReportImage Details
        public DataTable GetReportImageByCall_Id(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportImages", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string InsertUpdateReportImage(ReportImageModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_IP_Id == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ReportImages", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Heading", CPM.Heading);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Image", CPM.Image);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ReportImages", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Heading", CPM.Heading);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Image", CPM.Image);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IP_Id", CPM.PK_IP_Id);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetReportImageById(int? PK_IP_Id)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportImages", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IP_Id", PK_IP_Id);
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



        #region  IVR Report
        public DataTable GetReportByCall_Id(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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

        public DataTable GetCountByCall_Id(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 202);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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

        public DataTable GetReportBySubjob_Id(string SubJob_No)//Get All DropDownlist 
        {
            DataTable DSGetSrList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 20);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetSrList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetSrList.Dispose();
            }
            return DSGetSrList;
        }

        public DataTable GetReportByUser()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataTable GetReportByUserMIS()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "6MIS");
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataSet GetInspectionName(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataSet DSGetInspectionName = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "vai1");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetInspectionName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetInspectionName.Dispose();
            }
            return DSGetInspectionName;
        }

        public string InsertUpdateReport(ReportModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_RM_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ReportsMaster", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Report", CPM.Report);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReportName", CPM.ReportName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ProjectName", CPM.ProjectName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ClientName", CPM.ClientName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@VendorName", CPM.VendorName);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@isComfirmation", CPM.isComfirmation);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdatebranch.Parameters.Add("@GetReportId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();

                    int getreportid = Convert.ToInt32(CMDInsertUpdatebranch.Parameters["@GetReportId"].Value);
                    System.Web.HttpContext.Current.Session["GetReportId"] = getreportid;
                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ReportsMaster", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Report", CPM.Report);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ReportName", CPM.ReportName);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_RM_ID", CPM.PK_RM_ID);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@isComfirmation", CPM.isComfirmation);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetReportById(int? PK_RM_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_RM_ID", PK_RM_ID);
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

        //#region  Signature And Stamp
        //public DataSet GetUserSignatureStamp()
        //{
        //    DataSet DTEditContact = new DataSet();
        //    try
        //    {
        //        SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
        //        CMDEditContact.CommandType = CommandType.StoredProcedure;
        //        CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
        //       // CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
        //        CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
        //        SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
        //        SDAEditContact.Fill(DTEditContact);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEditContact.Dispose();
        //    }
        //    return DTEditContact;
        //}

        //#endregion

        public DataTable GetReportListBySubjobno(string SubJob_No, string Po_No)//Get All DropDownlist 
        {

            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJob_No);
                CMDGetDdlLst.Parameters.AddWithValue("@Po_No", Po_No);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        #region  IRN Search data

        public DataSet IrnSearchdata(string Client_Name, string Vendor_Name_Location, string Project_Name_Location, string Report_No)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditContact.Parameters.AddWithValue("@Client_Name", Client_Name);
                CMDEditContact.Parameters.AddWithValue("@Vendor_Name_Location", Vendor_Name_Location);
                CMDEditContact.Parameters.AddWithValue("@Project_Name_Location", Project_Name_Location);
                CMDEditContact.Parameters.AddWithValue("@Report_No", Report_No);

                //CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet Edit(int? id)
        {
            DataSet DTEditContact = new DataSet();
            string ids = string.Empty;
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_IRNReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 1);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", id);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
               
                CMDEditContact.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                SDAEditContact.Fill(DTEditContact);
                ids = CMDEditContact.Parameters["@ReturnID"].Value.ToString();
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

        public DataSet DownloadPrint(int? id)
        {
            DataSet DTEditContact = new DataSet();
            string ids = string.Empty;
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_IRNReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "1N");
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", id);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);

                CMDEditContact.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                SDAEditContact.Fill(DTEditContact);
                ids = CMDEditContact.Parameters["@ReturnID"].Value.ToString();
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
        

        #region Branch List
        public List<BranchMasters> GetBranchList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchMasters> lstEnquiryDashB = new List<BranchMasters>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 5);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new BranchMasters
                           {
                               Br_Id = Convert.ToInt32(dr["Br_Id"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
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
        #endregion

        public DataTable GetIRNReportById(int? PK_RM_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_RM_ID", PK_RM_ID);
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

        public DataTable GetIRNReportByIdForBindTable(int? PK_RM_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "5NN");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_RM_ID", PK_RM_ID);
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

        public DataSet GetBranchId()
        {
            DataSet DTEditUsers = new DataSet();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 11);
                //CMDEditUsers.Parameters.AddWithValue("@PK_UserID", PK_UserID);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }



        public DataSet GetReportByLastId(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 8);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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


        public DataSet GetCallId(int PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_IRNReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
               
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                //SDAEditContact.Fill(DTEditContact);

                CMDEditContact.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                SDAEditContact.Fill(DTEditContact);
                string id = CMDEditContact.Parameters["@ReturnID"].Value.ToString();
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


        public List<ReportModel> GetAllReportList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 23);
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ReportModel
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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


        


        public DataSet GetReportByCallid(string IRNReport)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ReportsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 11);
                CMDEditContact.Parameters.AddWithValue("@ReportName", IRNReport);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDEditContact);
                SDAEditContact.Fill(DTEditContact);

                //CMDEditContact.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
                //SDAEditContact.Fill(DTEditContact);
                //string id = CMDEditContact.Parameters["@ReturnID"].Value.ToString();
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


        public DataSet GetSubjobDetails(string SubJobNo)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobNewSP", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", SubJobNo);
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

        public DataSet GetSubjobDetailsByPo(string PoNo)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_SubJobNewSP", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@Vendor_Po_No", PoNo);
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

        #region irn Copy

       
        public List<ReportModel> GetReportList(string ReportName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ReportsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 105);
                CMDGetEnquriy.Parameters.AddWithValue("@ReportName", ReportName);
               
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ReportModel
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                               PK_RM_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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


        public List<ReportModel> GetReportbyProjectNameList(string ProjectName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ReportsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 102);
                CMDGetEnquriy.Parameters.AddWithValue("@ProjectName", ProjectName);
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ReportModel
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                               PK_RM_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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

        public List<ReportModel> GetReportbyVendorNameList(string VendorName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ReportsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 103);
                CMDGetEnquriy.Parameters.AddWithValue("@VendorName", VendorName);
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ReportModel
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                               PK_RM_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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

        public List<ReportModel> GetReportbyClientNameList( string ClientName)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_ReportsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 104);
                CMDGetEnquriy.Parameters.AddWithValue("@ClientName", ClientName);
                // CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new ReportModel
                           {
                               ReportName = Convert.ToString(dr["ReportName"]),
                               //PK_CALL_ID = Convert.ToInt32(dr["PK_CALL_ID"]),
                               PK_RM_ID = Convert.ToInt32(dr["PK_CALL_ID"]),

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
        #endregion


        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_IRNID", typeof(int)));
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
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_IRNUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListIRNUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_IRNUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_IRNID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_IRNUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_IRNUploadedFile", con);
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

        #region Added By Ankush for FileUpload   
        public string InsertConFileAttachment(List<FileDetails> lstFileUploaded, int ID, int defid)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_IRNID", typeof(int)));
                //DTUploadFile.Columns.Add(new DataColumn("EnquiryNumber", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));

                foreach (var item in lstFileUploaded)
                {
                    //DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                    DTUploadFile.Rows.Add(ID, Convert.ToString(ID) + "_" + item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_CONUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@FK_RM_ID", defid);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListCONUploadedFile", DTUploadFile);
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
        public DataTable EditConUploadedFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CONUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_IRNID", ID);
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
        public string DeleteConUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CONUploadedFile", con);
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
        public DataTable GetConFileExt(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_CONUploadedFile", con);
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

        public DataSet AllPreviousOpenConcern(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "48New_IRN");
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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

        public DataSet EarlierIssuedQuantity(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 49);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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

        public DataSet NCR(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 50);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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


        public string CloseConcern(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 30);

                CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Reason", CPM.Reason);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                //added by shrutika salve 30092023
                CMDInsertUpdatebranch.Parameters.AddWithValue("@mitigateddate", CPM.Mitigateddate);
                //end
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();



            }
            catch (Exception ex)
            {

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
        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CONUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_IRNID", EQ_ID);
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

        //added by shrutika salve on 16/06/2023

        public string UpdateDownloadDate(int? Pk_IVR_Id, string SubJobNo)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                DateTime today = DateTime.Now;
                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 61);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", Pk_IVR_Id);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@IRNDownloadDate", today.ToString("MM/dd/yyyy HH:mm:ss"));
                CMDInsertUpdatebranch.Parameters.AddWithValue("@userId", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDInsertUpdatebranch.Parameters.AddWithValue("@subjob_no", SubJobNo);
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


            }
            catch (Exception ex)
            {

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

//added by shrutika salve 17082023

        public List<BranchMasters> GetBranch()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchMasters> lstEnquiryDashB = new List<BranchMasters>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 31);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new BranchMasters
                           {
                               Br_Id = Convert.ToInt32(dr["Br_Id"]),
                               Branch_Name = Convert.ToString(dr["Branch_Name"]),
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


        //added by shrutika salve 18082023

        public DataTable areaofconcerncheck(int pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 32);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", pk_call_id);
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


        public DataTable Datecheck(int pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetdate = new DataTable();
            try
            {
                SqlCommand CMDGetdateList = new SqlCommand("SP_DownloadIRN", con);
                CMDGetdateList.CommandType = CommandType.StoredProcedure;
                //CMDGetdateList.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetdateList.Parameters.AddWithValue("@pk_ivrid", pk_call_id);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetdateList);
                SDAGetDdlLst.Fill(DSGetdate);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetdate.Dispose();
            }
            return DSGetdate;
        }


        public string Updateareaconcern(InspectionvisitReportModel IR)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                DateTime today = DateTime.Now;
                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 63);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", IR.abcid);
                if (IR.AreasOfConcern == true)
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IsIssuePending", "1");
                }
                else
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IsIssuePending", "0");
                }
                Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


            }
            catch (Exception ex)
            {

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


        public DataTable areaofdetails(int? PK_IVR_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 65);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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
        //added by nikita on 06112023
        public DataTable GetReportByUser_()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_ReportMasterRevised_IVR_IRN", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 6);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        //added by nikita on 23102024

        public DataTable GetAttachmentsData(int? pk_ivr_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "6");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_ivr_id);
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
        public DataTable GetEmaildata(int? pk_ivr_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "5");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_ivr_id);
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

        public DataTable GetDetails(string pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "2");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
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
        public DataTable UpdateMailFlag(int? pk_call_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "7");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
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


        public DataTable GetAttachmentsData_(int? pk_ivr_id)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "6");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_ivr_id);
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


        public DataTable Updatetable_vendor_Email(int? pk_call_id, string Email, string type)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "9");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@Type", type);
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


        public DataTable UpdateClientTable(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "10");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);
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


        public DataTable UpdateTuvMail(int? pk_call_id, string Email)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@User", Email);
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


        public DataTable UpdateTUVEmial(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@userid", Email);
                //CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);
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

        public DataTable DeleteEmail(int? pk_call_id, string EmailType, string Name, string Email)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_DeleteEmailData_IRN", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                if (EmailType == "TUV")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "1");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "Client_Email")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "2");
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                }
                else if (EmailType == "SubSub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "4");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);

                }
                else if (EmailType == "Sub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "3");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);

                }
                else if (EmailType == "SubSubSub Vendor")
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "5");
                    CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                    CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                    CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Name);

                }

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
        public DataTable SaveData_(string pk_call_id, string emails, string attachments, string mailsubject)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "16");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@Semd_Mails_To_Email", emails);
                CMDGetDdlLst.Parameters.AddWithValue("@Attachment_Send_To_client", attachments);
                CMDGetDdlLst.Parameters.AddWithValue("@MailSubject", mailsubject);
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

        public DataTable updateClient(int? pk_call_id, string Email, string ClientName)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "12");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", ClientName);

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

        public DataTable updateVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "13");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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

        public DataTable updateSubVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "14");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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


        public DataTable updateSubSubVendor(int? pk_call_id, string Email, string Vendor_Name_Location)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_GetIRNData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "15");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", pk_call_id);
                CMDGetDdlLst.Parameters.AddWithValue("@NewEmail", Email);
                CMDGetDdlLst.Parameters.AddWithValue("@ClientName", Vendor_Name_Location);

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

        public DataTable updateIscomfirmation(int? Pk_IVR_ID, ReportModel CPM)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_IRNReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "101");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_IVR_ID", Pk_IVR_ID);
                CMDGetDdlLst.Parameters.AddWithValue("@IsComfirmation", CPM.isComfirmation);
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

        public DataTable GetAllIRNReport()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 93);
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        //public string InsertFirstDownloadDate(int? PK_RM_ID)
        //{
        //    string Result = string.Empty;
        //    string id = string.Empty;
        //    con.Open();
        //    try
        //    {

        //        SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_IRNReport", con);
        //        CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
        //        CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "102");

        //        CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_RM_ID", PK_RM_ID);
        //        CMDInsertUpdatebranch.Parameters.Add("@ReturnID", SqlDbType.Int).Direction = ParameterDirection.Output;
        //        Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
        //        id = CMDInsertUpdatebranch.Parameters["@ReturnID"].Value.ToString();


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return id;
        //}
        public string InsertFirstDownloadDate(int? PK_RM_ID)
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_IRNReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 102);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_RM_ID", PK_RM_ID);
                //CMDGetEnquriy.Parameters.AddWithValue("@unblockJobReason", Reason);
                //.Parameters.AddWithValue("@Createdby", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDGetEnquriy);
                SDADashBoardData.Fill(DTDashBoard);
                if (DTDashBoard.Rows.Count > 0)
                {
                    Result = DTDashBoard.Rows[0][0].ToString();
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

            return Result.ToString();
        }

        public DataTable GetAllIRNReportdate(ReportModel R)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 94);
                //CMDGetDdlLst.Parameters.AddWithValue("@FDate", R.FromDate);
                //CMDGetDdlLst.Parameters.AddWithValue("@TDate", R.ToDate);
                CMDGetDdlLst.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(R.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(R.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


       


        public DataTable UpdateIRNData(int? PK_IVR_ID, string PoDescription)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "91");
                CMDGetDdlLst.Parameters.AddWithValue("@POItem_Des", PoDescription);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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
        public DataTable GetPOItemNo(int? PK_IVR_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "90N");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_IVR_ID);
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

        public DataSet AllPreviousCloseConcernIRN(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "IRNClose");
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public DataTable GetIRNReportdate(ReportModel R)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 96);
                //CMDGetDdlLst.Parameters.AddWithValue("@FDate", R.FromDate);
                //CMDGetDdlLst.Parameters.AddWithValue("@TDate", R.ToDate);
                CMDGetDdlLst.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(R.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(R.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataTable GetIRNReportdateMIS(ReportModel R)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 600;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 97);
                //CMDGetDdlLst.Parameters.AddWithValue("@FDate", R.FromDate);
                //CMDGetDdlLst.Parameters.AddWithValue("@TDate", R.ToDate);
                CMDGetDdlLst.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(R.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(R.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public DataSet GetIRNdata(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "90NNN");
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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
    }
}


