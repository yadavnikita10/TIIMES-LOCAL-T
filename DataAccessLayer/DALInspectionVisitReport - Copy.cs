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
    public class DALInspectionVisitReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);



        #region  Inspection Report
        public string DeleteConUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FK_VCID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string DeleteConUploadedFile1(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", "3N");
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FK_VCID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_VCUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetConFileExt1(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_SJUploadedFile", con);
                CMDGetExtenstion.CommandType = CommandType.StoredProcedure;
                CMDGetExtenstion.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetExtenstion.Parameters.AddWithValue("@FileID", FileID);
                CMDGetExtenstion.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
            con.Open();
            try
            {
                if (CPM.PK_IVR_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    if (CPM.Br_Id != null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.Br_Id);
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

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReviseReason", CPM.ReviseReason);

                    if (CPM.Date_of_PO == null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@date_of_po", CPM.Date_of_PO);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@date_of_po", DateTime.ParseExact(CPM.Date_of_PO, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    if (CPM.SubSubVendorDate_of_PO == null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@SubSubVendorDate_of_PO", CPM.SubSubVendorDate_of_PO);
                    }
                    else
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@SubSubVendorDate_of_PO", DateTime.ParseExact(CPM.SubSubVendorDate_of_PO, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CallIDs", CPM.CallIDs);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 3);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    if (CPM.Br_Id != null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch", CPM.Br_Id);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@Branch", CPM.Branch);
                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Notification_Name_No_Date", CPM.Notification_Name_No_Date);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Date_Of_Inspection", CPM.Date_Of_Inspection);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Project_Name_Location", CPM.Project_Name_Location);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Address_Of_Inspection", CPM.Address_Of_Inspection);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Client_Name", CPM.Client_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@End_user_Name", CPM.End_user_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Vendor_Name_Location", CPM.Vendor_Name_Location);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_No", CPM.Po_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Sub_Vendor_Name", CPM.Sub_Vendor_Name);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Po_No_SubVendor", CPM.Po_No_SubVendor);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Kick_Off_Pre_Inspection", CPM.Kick_Off_Pre_Inspection);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Material_identification", CPM.Material_identification);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Interim_Stages", CPM.Interim_Stages);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Document_review", CPM.Document_review);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Re_inspection", CPM.Re_inspection);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Conclusion", CPM.Conclusion);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Signatures", CPM.Signatures);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Type", CPM.Type);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", CPM.CreatedBy);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedDate", CPM.CreatedDate);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Report_No", CPM.Report_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ReviseReason", CPM.ReviseReason);
                    if (CPM.Date_of_PO == null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@date_of_po", CPM.Date_of_PO);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@date_of_po", DateTime.ParseExact(CPM.Date_of_PO, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    if (CPM.SubSubVendorDate_of_PO == null)
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSubVendorDate_of_PO", CPM.SubSubVendorDate_of_PO);
                    }
                    else
                    {
                        CMDInsertUpdateBranch.Parameters.AddWithValue("@SubSubVendorDate_of_PO", DateTime.ParseExact(CPM.SubSubVendorDate_of_PO, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CallIDs", CPM.CallIDs);

                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Name", CPM.DEC_PMC_EPC_Name);
                    //CMDInsertUpdateBranch.Parameters.AddWithValue("@DEC_PMC_EPC_Assignment_No", CPM.DEC_PMC_EPC_Assignment_No);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string InsertUpdateConclousion(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (CPM.PK_IVR_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Sap_And_Controle_No", CPM.Sap_And_Controle_No);
                    if (CPM.Br_Id != null)
                    {
                        CMDInsertUpdatebranch.Parameters.AddWithValue("@Branch", CPM.Br_Id);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@InspectiobRecord_Remark", CPM.InspectiobRecord_Remark);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@OtherSpecifyRecords", CPM.OtherSpecifyRecords);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRIRNAttachment", CPM.IVRIRNAttachment);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CanIRNbeissued", CPM.CanIRNbeissued);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@IssuedPOItemNumbers", CPM.IssuedPOItemNumbers);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@MasterListOfcalibratedInstruments", CPM.MasterListOfcalibratedInstruments);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DTUVIndiaClientEndUser", CPM.DTUVIndiaClientEndUser);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DTUVIndiaExecuting_Originating_Branch", CPM.DTUVIndiaExecuting_Originating_Branch);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DVendor_Sub_Vendor", CPM.DVendor_Sub_Vendor);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@TempInspectionPhotosNo", CPM.TempInspectionPhotosNo);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@TempMaster_List_Of_calibrated_Instruments", CPM.TempMaster_List_Of_calibrated_Instruments);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@date_of_PO", CPM.Date_of_PO);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ExpenseCheckBox", CPM.ExpenseCheckBox);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Expenses", CPM.Expenses);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReasonID", CPM.ReasonIDs);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@TiimeCheckBox", CPM.TiimeCheckBox);

                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdatebranch.Parameters["@ReturnId"].Value.ToString());
                    Result = Convert.ToString(ReturnId);

                }
                else
                {
                    SqlCommand CMDInsertUpdateBranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdateBranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@SP_Type", 7);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Conclusion", CPM.Conclusion);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Pending_Activites", CPM.Pending_Activites);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Identification_Of_Inspected", CPM.Identification_Of_Inspected);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Non_Conformities_raised", CPM.Non_Conformities_raised);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_records", CPM.Inspection_records);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Inspection_Photo", CPM.Inspection_Photo);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Other_Specify", CPM.Other_Specify);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@InspectiobRecord_Remark", CPM.InspectiobRecord_Remark);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@OtherSpecifyRecords", CPM.OtherSpecifyRecords);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IVRIRNAttachment", CPM.IVRIRNAttachment);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Attachment", CPM.Attachment);

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CanIRNbeissued", CPM.CanIRNbeissued);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@IssuedPOItemNumbers", CPM.IssuedPOItemNumbers);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@MasterListOfcalibratedInstruments", CPM.MasterListOfcalibratedInstruments);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DTUVIndiaClientEndUser", CPM.DTUVIndiaClientEndUser);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DTUVIndiaExecuting_Originating_Branch", CPM.DTUVIndiaExecuting_Originating_Branch);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@DVendor_Sub_Vendor", CPM.DVendor_Sub_Vendor);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@TempInspectionPhotosNo", CPM.TempInspectionPhotosNo);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@TempMaster_List_Of_calibrated_Instruments", CPM.TempMaster_List_Of_calibrated_Instruments);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@date_of_PO", CPM.Date_of_PO);


                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ExpenseCheckBox", CPM.ExpenseCheckBox);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@Expenses", CPM.Expenses);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@ReasonID", CPM.ReasonIDs);
                    CMDInsertUpdateBranch.Parameters.AddWithValue("@TiimeCheckBox", CPM.TiimeCheckBox);


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


        #region areas of concern
        public string InsertAreasOfConcern(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (CPM.PkId != "" && CPM.PkId != null)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 24);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 23);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Pk_Call_Id", CPM.PK_Call_ID);
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }

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
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string ReopenConcern(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 45);

                CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);

                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string UpdateAreasOfConcern(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 24);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);
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

        public DataTable GetAreasOfConcern(InspectionvisitReportModel Conclusion)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 22);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", Conclusion.PK_Call_ID);
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

        public DataTable GetTop1AreasOfConcern(InspectionvisitReportModel Conclusion)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 25);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", Conclusion.PK_Call_ID);
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


        public string DeleteConcern(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (CPM.PkId != "" && CPM.PkId != null)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 26);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }


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


        #endregion

        #region Pending Activities 6 April
        public string InsertPendingActivities(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                if (CPM.PkId != "" && CPM.PkId != null)//Update
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 24);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PkId", CPM.PkId);
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }
                else
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 27);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Areas_Of_Concerns", CPM.Areas_Of_Concerns);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Pk_Call_Id", CPM.PK_Call_ID);
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();
                }

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



        public DataTable GetPendingActivity(InspectionvisitReportModel Conclusion)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 28);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", Conclusion.PK_Call_ID);
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

        public DataTable GetTop1GetPendingActivity(InspectionvisitReportModel Conclusion)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 29);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_Id", Conclusion.PK_Call_ID);
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






        public DataSet EditInspectionVisitReport(int? PK_IVR_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 4);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", PK_IVR_ID);
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
        //        CMDContactDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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



        public DataSet EditInspectionVisitReportByPKCallID(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 19);
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

        public DataSet AllPreviousConcern(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 21);
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


        public DataSet AllPreviousCloseConcern(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 40);
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

        public DataSet NCR(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 46);
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

        public DataSet PrintVisitReport(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "19Print");
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
        #endregion

        public DataSet getsubJobdate(string subjobNo)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 20);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", subjobNo);
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
        #region  Item Description
        public DataTable GetitemDescription(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ItemDescription", con);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Unit", CPM.Unit);


                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


                CMDInsertUpdatebranch.Parameters.AddWithValue("@Created_By", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public int DeleteItemDescriptionData(int? PK_ItemD_Id)
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
        #endregion

        #region   Refrance Documents
        public DataTable GetReferenceDocuments(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Reference_Documents", con);
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

        public DataTable EditConUploadedFile(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_VCID", ID);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_No", CPM.Document_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Document_Name", CPM.Document_Name);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Approval_Status", CPM.Approval_Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@VendorDocumentNumber", CPM.VendorDocumentNumber);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@VendorDocumentNumber", CPM.VendorDocumentNumber);
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

        public DataSet GetRefranceDocuments(int? PK_RD_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_Reference_Documents", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
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
                CMDInsertUpdatebranch.Parameters.AddWithValue("@VendorDocumentNumber", CPM.VendorDocumentNumber);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public int DeleteRefranceDocumentData(int? PK_RD_ID)
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

        #endregion




        #region    Inspection Activites
        public DataTable GetInspectionActivities(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionActivites", con);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
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

        public DataSet GetInspectionActivitiesByPKIAID(int? PK_IA_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionActivites", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
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
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public int DeleteInspectionActivitiesData(int? PK_IA_ID)
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

        #endregion



        #region    Document Reviewe
        public DataTable GetDocumentRevieweModelByCall_Id(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_DocumentsReviewed", con);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet GetDocumentRevieweModelById(int? PK_DR_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_DocumentsReviewed", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
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
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public int DeleteDocumentRevieweData(int? PK_DR_ID)
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
        #endregion


        #region Get Visit Time
        public DataTable GetVisitTime(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DTGetVisitTime = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "66");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DTGetVisitTime);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetVisitTime.Dispose();
            }
            return DTGetVisitTime;
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@NABLseenote1", CPM.NABLseenote1);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@NonNABLseenote2", CPM.NonNABLseenote2);


                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@NABLseenote1", CPM.NABLseenote1);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@NonNABLseenote2", CPM.NonNABLseenote2);

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


        public string InsertEquipmentDetails(EquipmentDetailsModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
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
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public int DeleteEquipmentDetailsData(int? PK_DOE_Id)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_EquipmentsDetails", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDContactDelete.Parameters.AddWithValue("@PK_DOE_Id", PK_DOE_Id);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        public string UpdateHeading(ReportImageModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ReportImages", con);
                CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 6);
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Heading", CPM.Heading);

                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_IP_Id", CPM.PK_IP_Id);
                CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDInsertUpdateItemDescription.ExecuteNonQuery().ToString();


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
        public bool DeleteImage(int id)
        {
            SqlCommand CMDDeleteAcvt = new SqlCommand("SP_ItemDescription", con);
            CMDDeleteAcvt.CommandType = CommandType.StoredProcedure;
            CMDDeleteAcvt.Parameters.AddWithValue("@SP_Type", "8");
            CMDDeleteAcvt.Parameters.AddWithValue("@PK_ItemD_Id", id);
            con.Open();
            int i = CMDDeleteAcvt.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string InsertUpdatevisitreportattachment(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (CPM.PK_Call_ID == 0)
                {
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_ReportImages", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 5);


                    CMDInsertUpdatebranch.Parameters.AddWithValue("@VisitReportAttachment", CPM.VisitReportAttachment);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_Call_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ReportImages", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 5);

                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@VisitReportAttachment", CPM.VisitReportAttachment);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_Call_ID);
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
                CMDGetDdlLst.CommandTimeout = 0;
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
        public DataTable GetReportByUser()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 6000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 3);
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
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SubJob_No", CPM.SubJob_No);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", CPM.PK_CALL_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@ReportNo", CPM.ReportNo);

                    //CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    Result = CMDInsertUpdatebranch.ExecuteNonQuery().ToString();


                }
                else
                {
                    SqlCommand CMDInsertUpdateItemDescription = new SqlCommand("SP_ReportsMaster", con);
                    CMDInsertUpdateItemDescription.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@SP_Type", 7);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@Report", CPM.Report);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@ReportName", CPM.ReportName);
                    CMDInsertUpdateItemDescription.Parameters.AddWithValue("@PK_RM_ID", CPM.PK_RM_ID);
                    // CMDInsertUpdateItemDescription.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataSet GetSrNo(string SubJobNo)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 106);
                CMDGetDdlLst.Parameters.AddWithValue("@SubJob_No", SubJobNo);
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


        public DataSet GetReportNo(string ReportName)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 107);
                CMDGetDdlLst.Parameters.AddWithValue("@ReportName", ReportName);
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
        //        CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataSet Edit(int? id)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 22);
                CMDEditContact.Parameters.AddWithValue("@PK_IVR_ID", id);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataSet GetReportByLastId(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 8);
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


        public string UpdateStatus(int? PK_Call_ID, string status)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (PK_Call_ID != 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 21);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Status", status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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

        // public List<ReportModel> StringGetReportList(List<int> PK_CALL_ID)// Binding Sales Masters DashBoard of Master Page 
        public List<ReportModel> StringGetReportList(string PK_CALL_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "23N");
                CMDGetEnquriy.Parameters.AddWithValue("@PK_CALL_ID1", PK_CALL_ID);
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


        public List<ReportModel> GetReportList(int PK_CALL_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 23);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_CALL_ID", PK_CALL_ID);
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


        public DataSet GetCallId(string SubJobNo, string ProjectName, string Vendor, string Client)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 12);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", SubJobNo);
                CMDEditContact.Parameters.AddWithValue("@Project_Name_Location", ProjectName);
                CMDEditContact.Parameters.AddWithValue("@Vendor_Name_Location", Vendor);
                CMDEditContact.Parameters.AddWithValue("@Client_Name", Client);
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

        public DataTable DtGetCallId(string SubJobNo, string ProjectName, string Vendor, string Client)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 47);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", SubJobNo);
                //CMDEditContact.Parameters.AddWithValue("@Project_Name_Location", ProjectName);
                //CMDEditContact.Parameters.AddWithValue("@Vendor_Name_Location", Vendor);

                CMDEditContact.Parameters.AddWithValue("@Client_Name", Client);
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


        public DataSet IVRByCallID(string ReportName)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_ReportsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 9);
                CMDEditContact.Parameters.AddWithValue("@ReportName", ReportName);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        #region Copy Function
        public string CopyInsert(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 31);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@Report_No", CPM.ReportNoName);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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
        #endregion

        #region Clear Function
        public string Clear(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 32);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
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
        #endregion



        #region IRN Copy and Clear
        public DataTable GetReportNoIRN(string SubJobNo, string VisitReport, string Type)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 12);
                CMDEditContact.Parameters.AddWithValue("@SubJob_No", SubJobNo);
                CMDEditContact.Parameters.AddWithValue("@Client_Name", VisitReport);
                CMDEditContact.Parameters.AddWithValue("@Vendor_Name_Location", Type);

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

        public List<ReportModel> StringGetReportListForIRNNew(string Sub_JobNo, string VisitReport, String Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<ReportModel> lstEnquiryDashB = new List<ReportModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                if (Type == "IRN")
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "34");
                }
                else
                {
                    CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "33");
                }

                CMDGetEnquriy.Parameters.AddWithValue("@SubJob_No", Sub_JobNo);
                CMDGetEnquriy.Parameters.AddWithValue("@VisitReport", VisitReport);

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
                               PK_RM_ID = Convert.ToInt32(dr["PK_IVR_ID"]),
                               // PK_CALL_ID = Convert.ToInt32(dr["PK_IVR_ID"]),

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

        public string CopyIRNInsert(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                if (CPM.Type == "IVR")
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 35);
                }
                else if (CPM.Type == "IRN")
                {
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 36);
                }
                else
                {

                }


                CMDInsertUpdatebranch.Parameters.AddWithValue("@ReportNo", CPM.ReportNoName);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
                //CMDInsertUpdatebranch.Parameters.AddWithValue("@Type", CPM.Type);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public string ClearIRN(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 37);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_IVR_ID", CPM.PK_IVR_ID);
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
        #endregion


        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
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

        public DataTable GetFileContentRefDoc(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                //SqlCommand CMDEditUploadedFile = new SqlCommand("SP_VCUploadedFile", con);
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "2NN");
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

        #region 
        public InspectionvisitReportModel GetPONo(string PK_Call_Id)
        {
            con.Open();
            InspectionvisitReportModel _vmcompany = new InspectionvisitReportModel();
            try
            {
                SqlCommand GetAddress = new SqlCommand("SP_InspectionVisitReport", con);
                GetAddress.CommandType = CommandType.StoredProcedure;
                GetAddress.Parameters.AddWithValue("@SP_Type", 38);
                GetAddress.Parameters.AddWithValue("@PK_Call_ID", PK_Call_Id);

                SqlDataReader dr = GetAddress.ExecuteReader();
                while (dr.Read())
                {
                    _vmcompany.Po_No = dr["PONO"].ToString();

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
        #endregion


        #region Bind Measurement
        public DataSet Measurement()
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 39);
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


        #region Areas of concern
        public DataTable GetAreasOfConcern(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 41);
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
        #endregion

        #region
        public DataTable GetPendingActivity(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 42);
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
        #endregion
        #region CheckPreviousActivity
        public DataTable CheckPreviousActivity(string date)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "43");
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDGetDdlLst.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
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

        public DataTable CheckPreviousActivityWithCallId(string date, int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "44");
                CMDGetDdlLst.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDGetDdlLst.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
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

        public DataTable CheckIfLeavePresent(string date)
        {


            DataTable ValidateTotal = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "20");
                cmd.Parameters.AddWithValue("@DateSE", DateTime.ParseExact(date, "dd/MM/yyyy", null));
                //cmd.Parameters.AddWithValue("@CreatedBy", objL.FirstName);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ValidateTotal);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ValidateTotal.Dispose();
            }
            return ValidateTotal;
        }
        #endregion


        public DataTable GetEquipmentName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_InspectionVisitReport", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 51);
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

        public string UpdateARCFlagFirstClick(int? PK_CALL_ID)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 52);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", PK_CALL_ID);
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

        public DataTable EditConSubUploadedFile(int? ID, int? PK_call_id)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SJID", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_call_id", PK_call_id);
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

        public DataTable EditConSubUploadedFile1(int? ID, int? PK_call_id)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "2N");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SJID", ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_call_id", PK_call_id);
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

        public DataTable getReasonlistforEdit()
        {
            DataTable dt = new DataTable();
            SqlCommand CMDGetEdit = new SqlCommand("SP_InspectionVisitReport", con);
            CMDGetEdit.CommandType = CommandType.StoredProcedure;
            CMDGetEdit.CommandTimeout = 1000000;
            CMDGetEdit.Parameters.AddWithValue("@SP_Type", 53);

            SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEdit);
            SDAGetEnquiry.Fill(dt);
            return dt;
        }

        public DataTable GetAllReportByUser()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_ReportsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 10);
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

        #region
        public List<InspectionvisitReportModel> GetDrpList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<InspectionvisitReportModel> lstEnquiryDashB = new List<InspectionvisitReportModel>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 53);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new InspectionvisitReportModel
                           {
                               ReasonID = Convert.ToInt32(dr["Code"]),
                               ReasonName = Convert.ToString(dr["Name"]),
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

        #region  Get All concern for Export to Excel
        public DataTable GetDataForExportToExcelAllConcern(int? PK_Call_ID)//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 54);
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


        #endregion


        //added by shrutika salve on 16/06/2023

        public string UpdateDownloadDate(int? PK_CALL_ID)
        {
            string Result = string.Empty;
            int ReturnId = 0;
            con.Open();
            try
            {
                DateTime today = DateTime.Now;
                SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", 60);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_CALL_ID", PK_CALL_ID);
                CMDInsertUpdatebranch.Parameters.AddWithValue("@IVRDownloadDate", today.ToString("MM/dd/yyyy HH:mm:ss"));
                CMDInsertUpdatebranch.Parameters.AddWithValue("@userId", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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

        public DataTable GetDates(int? PK_Call_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_InspectionVisitReport", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 62);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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

        public string InsertUpdateInspectionvisitTime(InspectionvisitReportModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                
                    SqlCommand CMDInsertUpdatebranch = new SqlCommand("SP_InspectionVisitReport", con);
                    CMDInsertUpdatebranch.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@SP_Type", "64");
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@DateSe", CPM.DateSe);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Intime", CPM.Intime);
                    CMDInsertUpdatebranch.Parameters.AddWithValue("@Outtime", CPM.Outtime);
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

        public DataTable checksign()//Get All DropDownlist 
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_InspectionVisitReport", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 67);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


    }
}