using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;
using System.IO;
using System.Globalization;

namespace TuvVision.DataAccessLayer
{
    public class DALCalls
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        #region  Call Master


        public DataTable GetCallsList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 1);
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

        public int InsertExtendCalls(CallsModel CPM)
        {
            int ReturnId = 0;
            string Result = string.Empty;
            con.Open();
            try
            {

                SqlCommand CMDInsertUpdateCalls = new SqlCommand("SP_CallExtend", con);
                CMDInsertUpdateCalls.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateCalls.Parameters.AddWithValue("@SP_Type", 2);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Status", CPM.Status);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Originating_Branch", CPM.Originating_Branch);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Type", CPM.Type);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Contact_Name", CPM.Contact_Name);
          //      CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(CPM.CallReceiveTime, "dd/MM/yyyy", null));
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(CPM.CallReceiveTime.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                if (CPM.Call_Request_Date != null)
                {
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Request_Date", DateTime.ParseExact(CPM.Call_Request_Date, "dd/MM/yyyy", null));
                }
                if (CPM.Planned_Date != null)
                {
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Planned_Date", DateTime.ParseExact(CPM.Planned_Date, "dd/MM/yyyy", null));
                }

                CMDInsertUpdateCalls.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(CPM.Actual_Visit_Date, "dd/MM/yyyy", null));
                if (CPM.From_Date != null)
                {
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@From_Date", DateTime.ParseExact(CPM.From_Date, "dd/MM/yyyy", null));
                }
                if (CPM.To_Date != null)
                {
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@To_Date", DateTime.ParseExact(CPM.To_Date, "dd/MM/yyyy", null));
                }


                CMDInsertUpdateCalls.Parameters.AddWithValue("@Executing_Branch", CPM.Br_Id);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Type", CPM.Sub_Type);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Job", CPM.Job);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Source", CPM.Source);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Name", CPM.Vendor_Name);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Job", CPM.Sub_Job);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Urgency", CPM.Urgency);
                //CMDInsertUpdateCalls.Parameters.AddWithValue("@Competency_Check", CPM.Competency_Check);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@End_Customer", CPM.End_Customer);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Category", CPM.Category);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Category", CPM.Sub_Category);
                //CMDInsertUpdateCalls.Parameters.AddWithValue("@Empartiality_Check", CPM.Empartiality_Check);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Job_Location", CPM.Job_Location);
                //CMDInsertUpdateCalls.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@isWeek_Month_Call", CPM.isWeek_Month_Call);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@Inspector", CPM.FirstName);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Tuv_Branch", CPM.Tuv_Branch);
                //CMDInsertUpdateCalls.Parameters.AddWithValue("@Executing_Branch", CPM.Executing_Branch);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_No", CPM.Call_No);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@From_Time", CPM.From_Time);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@To_Time", CPM.To_Time);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Type", CPM.Call_Type);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@AssignStatus", CPM.AssignStatus);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@PO_Number", CPM.PO_Number);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Po_No_SSJob", CPM.Po_No_SSJob);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@Competency_Check", CPM.CompetencyCheck);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Empartiality_Check", CPM.ImpartialityCheck);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Final_Inspection", CPM.FinalInspection);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@Homecheckbox", CPM.Homecheckbox);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendorcheckbox", CPM.Vendorcheckbox);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@ClientEmailcheckbox", CPM.ClientEmailcheckbox);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.ChkMultipleSubJobNo);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkContinuousCall", CPM.ChkContinuousCall);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Product_item", CPM.ProductList);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Description", CPM.Description);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@Quantity", CPM.Quantity);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@ExecutingService", CPM.ExecutingService);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@EstimatedHours", CPM.EstimatedHours);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@CallReceiveTime", CPM.CallReceiveTime);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@ExtendCallID", CPM.PK_Call_ID);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@isCompetant", CPM.inspectorCompetant);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@MandayRate", CPM.MandayRate);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@PrimaryMaterial", CPM.PrimaryMaterial);
                //added by shrutika salve 04042023
                CMDInsertUpdateCalls.Parameters.AddWithValue("@isinspectorApproved", CPM.inspectorapproved);
                //added by shrutika salve 14062024
                CMDInsertUpdateCalls.Parameters.AddWithValue("@CustomerRepresentativeName", CPM.CustomerRepresentative);
                CMDInsertUpdateCalls.Parameters.AddWithValue("@CallsRemarks", CPM.CallRemarks);

                CMDInsertUpdateCalls.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDInsertUpdateCalls.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                Result = CMDInsertUpdateCalls.ExecuteNonQuery().ToString();
                ReturnId = Convert.ToInt32(CMDInsertUpdateCalls.Parameters["@ReturnId"].Value.ToString());


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
            return ReturnId;
        }

        public DataTable GetCallDetails(int callId)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 55);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", callId);
                
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

        public DataTable GetUserDetails(string PInspector)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 57);
                CMDGetDdlLst.Parameters.AddWithValue("@Inspector", PInspector);

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


        public string InsertUpdateCalls(CallsModel CPM)
        {
            int ReturnId = 0;
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (CPM.PK_Call_ID == 0)
                {
                    SqlCommand CMDInsertUpdateCalls = new SqlCommand("SP_CallExtend", con);
                    CMDInsertUpdateCalls.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@SP_Type", 1);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Originating_Branch", CPM.Originating_Branch);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Contact_Name", CPM.Contact_Name);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(CPM.CallReceiveTime.Substring(0, 10), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    if (CPM.Call_Request_Date != null)
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Request_Date", DateTime.ParseExact(CPM.Call_Request_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    if (CPM.Planned_Date != null)
                    {
                        ///CMDInsertUpdateCalls.Parameters.AddWithValue("@Planned_Date", DateTime.ParseExact(CPM.Planned_Date, "dd/MM/yyyy", theCultureInfo));
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@Planned_Date", DateTime.ParseExact(CPM.Planned_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                    }

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(CPM.Actual_Visit_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    if (CPM.From_Date != null)
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@From_Date", DateTime.ParseExact(CPM.From_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }
                    if (CPM.To_Date != null)
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@To_Date", DateTime.ParseExact(CPM.To_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                    }

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Executing_Branch", CPM.Br_Id);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Type", CPM.Sub_Type);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Job", CPM.Job);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Source", CPM.Source);
                    if (CPM.SubSubJobNo != null)
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Job", CPM.SubSubJobNo);
                    }
                    else
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Job", CPM.Sub_Job);
                    }

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Urgency", CPM.Urgency);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Competency_Check", CPM.Competency_Check);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@End_Customer", CPM.End_Customer);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Category", CPM.Category);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Category", CPM.Sub_Category);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Empartiality_Check", CPM.Empartiality_Check);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Job_Location", CPM.Job_Location);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    //added by shrutika salve 27032024
                    if (CPM.FinalInspectionValue == "1")
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@FinalInspection", 1);

                    }
                    else
                    {
                        CMDInsertUpdateCalls.Parameters.AddWithValue("@FinalInspection", 0);
                    }
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@isWeek_Month_Call", CPM.isWeek_Month_Call);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Inspector", CPM.FirstName);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Tuv_Branch", CPM.Tuv_Branch);
                    //CMDInsertUpdateCalls.Parameters.AddWithValue("@Executing_Branch", CPM.Executing_Branch);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_JOB_ID", CPM.PK_JOB_ID);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_No", CPM.Call_No);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@From_Time", CPM.From_Time);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@To_Time", CPM.To_Time);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Call_Type", CPM.Call_Type);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@AssignStatus", CPM.AssignStatus);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Attachment", CPM.Attachment);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Product_item", CPM.ProductList);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Quantity", CPM.Quantity);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendor_Name", CPM.Vendor_Name); /////Sub Sub vendor (/1/1)
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Po_No_SSJob", CPM.Po_No_SSJob);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@TopSubVendorName", CPM.TopSubVendorName); //////Sub Vendor (/1)
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@TopSubVendorPONo", CPM.TopSubVendorPONo);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@PO_Number", CPM.PO_Number);


                    CMDInsertUpdateCalls.Parameters.AddWithValue("@JobList", CPM.JobList);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkMultipleJobNo", CPM.ChkMultipleJobNo);




                    CMDInsertUpdateCalls.Parameters.AddWithValue("@CompetencyCheck", CPM.CompetencyCheck);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ImpartialityCheck", CPM.ImpartialityCheck);
                    //CMDInsertUpdateCalls.Parameters.AddWithValue("@FinalInspection", CPM.FinalInspection);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Homecheckbox", CPM.Homecheckbox);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Vendorcheckbox", CPM.Vendorcheckbox);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ClientEmailcheckbox", CPM.ClientEmailcheckbox);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.ChkMultipleSubJobNo);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkContinuousCall", CPM.ChkContinuousCall);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ExecutingService", CPM.ExecutingService);

                    CMDInsertUpdateCalls.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@EstimatedHours", CPM.EstimatedHours);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@CallReceiveTime", CPM.CallReceiveTime);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@isCompetant", CPM.inspectorCompetant);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@MandayRate", CPM.MandayRate);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@PrimaryMaterial", CPM.PrimaryMaterial);
                    //added by shrutika salve 08022024
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ExpeditingType", CPM.ExpeditingType);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@ChkManMonthsAssignmentCall", CPM.ManMonths);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@Weekdays", CPM.WeekDays);

                    //added by shrutika salve 04042023
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@isinspectorApproved", CPM.inspectorapproved);

                    //added by shrutika salve 14062024
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@CustomerRepresentativeName", CPM.CustomerRepresentative);
                    CMDInsertUpdateCalls.Parameters.AddWithValue("@CallsRemarks", CPM.CallRemarks);


                    CMDInsertUpdateCalls.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    Result = CMDInsertUpdateCalls.ExecuteNonQuery().ToString();
                    ReturnId = Convert.ToInt32(CMDInsertUpdateCalls.Parameters["@ReturnId"].Value.ToString());

                    Result = Convert.ToString(ReturnId);

                }
                else
                {
                    SqlCommand CMDInsertUpdateCall = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateCall.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateCall.Parameters.AddWithValue("@SP_Type", 7);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Company_Name", CPM.Company_Name);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Status", CPM.Status);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@AssignStatus", CPM.AssignStatus);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Originating_Branch", CPM.Originating_Branch);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Type", CPM.Type);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Contact_Name", CPM.Contact_Name);
                    //CMDInsertUpdateCall.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(CPM.Call_Recived_date, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Call_Recived_date", DateTime.ParseExact(CPM.CallReceiveTime.Substring(0, 10), "dd/MM/yyyy", theCultureInfo));
                    if (CPM.Call_Request_Date != null)
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@Call_Request_Date", DateTime.ParseExact(CPM.Call_Request_Date, "dd/MM/yyyy", theCultureInfo));
                    }
                    if (CPM.Planned_Date != null)
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@Planned_Date", DateTime.ParseExact(CPM.Planned_Date, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(CPM.Actual_Visit_Date, "dd/MM/yyyy", theCultureInfo));
                    if (CPM.From_Date != null)
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@From_Date", DateTime.ParseExact(CPM.From_Date, "dd/MM/yyyy", theCultureInfo));
                    }
                    if (CPM.To_Date != null)
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@To_Date", DateTime.ParseExact(CPM.To_Date, "dd/MM/yyyy", theCultureInfo));
                    }

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Excuting_Branch", CPM.Br_Id);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Sub_Type", CPM.Sub_Type);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Job", CPM.Job);

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Source", CPM.Source);

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Vendor_Name", CPM.Vendor_Name);


                    CMDInsertUpdateCall.Parameters.AddWithValue("@Sub_Job", CPM.Sub_Job);

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Urgency", CPM.Urgency);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Competency_Check", CPM.Competency_Check);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@End_Customer", CPM.End_Customer);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Category", CPM.Category);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Sub_Category", CPM.Sub_Category);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Empartiality_Check", CPM.Empartiality_Check);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Project_Name", CPM.Project_Name);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Job_Location", CPM.Job_Location);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Final_Inspection", CPM.Final_Inspection);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@isWeek_Month_Call", CPM.isWeek_Month_Call);


                    CMDInsertUpdateCall.Parameters.AddWithValue("@Inspector", CPM.FirstName);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Client_Email", CPM.Client_Email);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Vendor_Email", CPM.Vendor_Email);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Tuv_Branch", CPM.Tuv_Branch);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Executing_Branch", CPM.Br_Id);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@PK_SubJob_Id", CPM.PK_SubJob_Id);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@PK_QT_ID", CPM.PK_QT_ID);


                    CMDInsertUpdateCall.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@From_Time", CPM.From_Time);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@To_Time", CPM.To_Time);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Call_Type", CPM.Call_Type);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Attachment", CPM.Attachment);

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Client_Contact", CPM.Client_Contact);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Sub_Vendor_Contact", CPM.Sub_Vendor_Contact);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Vendor_Contact", CPM.Vendor_Contact);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Po_No_SSJob", CPM.Po_No_SSJob);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Product_item", CPM.ProductList);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Description", CPM.Description);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Quantity", CPM.Quantity);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@CompetencyCheck", CPM.CompetencyCheck);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ImpartialityCheck", CPM.ImpartialityCheck);
                    //CMDInsertUpdateCall.Parameters.AddWithValue("@FinalInspection", CPM.FinalInspection);
                    if (CPM.FinalInspectionValue == "1")
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@FinalInspection", 1);

                    }
                    else
                    {
                        CMDInsertUpdateCall.Parameters.AddWithValue("@FinalInspection", 0);
                    }

                    CMDInsertUpdateCall.Parameters.AddWithValue("@JobList", CPM.JobList);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ChkMultipleJobNo", CPM.ChkMultipleJobNo);

                    CMDInsertUpdateCall.Parameters.AddWithValue("@Homecheckbox", CPM.Homecheckbox);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Vendorcheckbox", CPM.Vendorcheckbox);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ClientEmailcheckbox", CPM.ClientEmailcheckbox);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ChkMultipleSubJobNo", CPM.ChkMultipleSubJobNo);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ChkContinuousCall", CPM.ChkContinuousCall);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ExecutingService", CPM.ExecutingService);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@EstimatedHours", CPM.EstimatedHours);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    //CMDInsertUpdateCall.CommandTimeout = 0;
                    CMDInsertUpdateCall.Parameters.AddWithValue("@chkCallCancelled", CPM.chkCallCancelled);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@Reasion", CPM.Reasion);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@StReasion", CPM.statusReason);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@CallReceiveTime", CPM.CallReceiveTime);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@isCompetant", CPM.inspectorCompetant);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@MandayRate", CPM.MandayRate);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@PrimaryMaterial", CPM.PrimaryMaterial);
                    //added by shrutika salve 04042023
                    CMDInsertUpdateCall.Parameters.AddWithValue("@isinspectorApproved", CPM.inspectorapproved);
                    //added by shrutika salve 14062024
                    CMDInsertUpdateCall.Parameters.AddWithValue("@CustomerRepresentativeName", CPM.CustomerRepresentative);
                    CMDInsertUpdateCall.Parameters.AddWithValue("@CallsRemarks", CPM.CallRemarks);

                    //added by shrutika salve 08022024
                    CMDInsertUpdateCall.Parameters.AddWithValue("@ExpeditingType", CPM.ExpeditingType);
                    Result = CMDInsertUpdateCall.ExecuteNonQuery().ToString();
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
                LogFile(ex.Message, "InsertCall");

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

        public string UpdateExtendCalls(CallsModel CPM)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDInsertUpdateCall = new SqlCommand("SP_CallExtend", con);
                CMDInsertUpdateCall.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateCall.Parameters.AddWithValue("@SP_Type", 7);
                CMDInsertUpdateCall.Parameters.AddWithValue("@Call_No", CPM.Call_No);
                CMDInsertUpdateCall.Parameters.AddWithValue("@Attachment", CPM.Attachment);
                CMDInsertUpdateCall.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                CMDInsertUpdateCall.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDInsertUpdateCall.ExecuteNonQuery().ToString();


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
        //Manoj Sharma Latest Code
        public DataSet EditCall(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        


        public DataSet EditCallByInspector(int? PK_Call_ID)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 24);
                CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                // CMDEditContact.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
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

        public DataSet GetQutationDetails(int? PK_SubJob_Id)//Getting Data of Qutation Details
        {
            DataSet DTGetEnquiryDtls = new DataSet();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 4);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@PK_SubJob_Id ", PK_SubJob_Id);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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



        public DataSet GetVendorPo(string Sub_Job)//Getting Data of Qutation Details
        {
            DataSet DTGetEnquiryDtls = new DataSet();
            try
            {
                SqlCommand CMDGetEnquiryDtls = new SqlCommand("SP_SubJobNewSP", con);
                CMDGetEnquiryDtls.CommandType = CommandType.StoredProcedure;
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SP_Type", 1);
                CMDGetEnquiryDtls.Parameters.AddWithValue("@SubJob_No ", Sub_Job);
                // CMDGetEnquiryDtls.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public List<BranchMasters> GetBranchList_(int? PK_SubJob_Id,int? PK_Call_ID)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchMasters> lstEnquiryDashB = new List<BranchMasters>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 5);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_SubJob_Id", PK_SubJob_Id);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);

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

        #region Bind Executing Branch
        public List<BranchMasters> GetExecutingList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<BranchMasters> lstEnquiryDashB = new List<BranchMasters>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                //CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "5N");
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "5");
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        #region Bind sub job 
        public DataSet BindSubJobByControlNo(String vendorName, string subtype, string jobnumber)
        {
            DataSet dsSubJobByControlNo = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_CallsMaster", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 53);
                CMDGetBranch.Parameters.AddWithValue("@Vendor_Name", vendorName);
                CMDGetBranch.Parameters.AddWithValue("@Type", subtype);
                CMDGetBranch.Parameters.AddWithValue("@JobNumber", jobnumber);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(dsSubJobByControlNo);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsSubJobByControlNo.Dispose();
            }
            return dsSubJobByControlNo;

        }
        #endregion


        #region bind format of report
        public DataTable EditUploadedFileFormat1(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_JobMastUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "2N");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_JOBID", BR_ID);
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


        #region bind Sub job Files
        public DataTable EditSubJobFiles(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "5");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_SJID", BR_ID);
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

        public DataTable GetDownloadSubJobFileFromCalls(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "6");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", BR_ID);
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

        public DataTable GetDownloadSubSubJobFileFromCalls(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "8");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", BR_ID);
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

        public DataTable GetDownloadSubSubSubJobFileFromCalls(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "9");
                CMDEditUploadedFile.Parameters.AddWithValue("@PK_ID", BR_ID);
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

        public DataTable DownloadSubJobFileFromCalls(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_SJUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "7");
                CMDEditUploadedFile.Parameters.AddWithValue("@FileID", BR_ID);
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

        #region bind Sub job Files
        public DataTable EditCallFiles(int? BR_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "2");
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CALLID", BR_ID);
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

        public DataTable dtGetExecutingList() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", "5N");
                CMDCallDash.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }
        public DataTable GetInspectorById(int? FK_BranchID) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 6);
                CMDCallDash.Parameters.AddWithValue("@FK_BranchID", FK_BranchID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        public DataTable GetSubJobNo() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 8);
                // CMDCallDash.Parameters.AddWithValue("@FK_BranchID", FK_BranchID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        public List<Users> GetInspectorList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 9);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

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

        public List<Users> GetInspectorListForLeaveManagement()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "9N");
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

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


        public List<Users> GetInspectorListByExecutingBranch()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 9);
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + Convert.ToString(dr["LastName"]),

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
        public IEnumerable<Users> GetInsByExBr(int brid)
        {
            List<Users> InsList = new List<Users>();
            try
            {
                SqlCommand CMDGetList = new SqlCommand("SP_CallsMaster", con);
                CMDGetList.CommandType = CommandType.StoredProcedure;
                CMDGetList.Parameters.AddWithValue("@SP_Type", 52);
                CMDGetList.Parameters.AddWithValue("@Br_Id", brid);
                con.Open();
                SqlDataReader dr = CMDGetList.ExecuteReader();
                while (dr.Read())
                {
                    Users _vmIns = new Users();
                    _vmIns.PK_UserID = Convert.ToString(dr["PK_UserID"]);
                    _vmIns.FirstName = Convert.ToString(dr["InspectorName"]);

                    InsList.Add(_vmIns);
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
            return InsList;
        }

        public DataSet GetBranchName()
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 10);
                // CMDEditContact.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
                //var Session= Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]);
                CMDEditContact.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
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


        public DataTable GetCallmanagmentsList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                #region previous call SP 
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 11);
                #endregion
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11N");
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

        //public DataTable GetCallmanagmentsListByCondition(CallsModel CM)
        public DataTable GetCallmanagmentsListByCondition(CallsModel CM)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                #region previous call SP 
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 11);
                #endregion
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "11NN");
                //CMDGetDdlLst.Parameters.AddWithValue("@Fromdate", CM.FromDate);
                //CMDGetDdlLst.Parameters.AddWithValue("@ToDate", CM.ToDate);
                if (CM.FromDate != null && CM.ToDate != null)
                {
                    CMDGetDdlLst.Parameters.AddWithValue("@Fromdate", DateTime.ParseExact(DateTime.ParseExact(CM.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    CMDGetDdlLst.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(CM.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                }
                

                
                CMDGetDdlLst.Parameters.AddWithValue("@Call_No", CM.Call_No);
                CMDGetDdlLst.Parameters.AddWithValue("@Branch", CM.Excuting_Branch );
                CMDGetDdlLst.Parameters.AddWithValue("@originating_Branch", CM.Originating_Branch);

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

        /*  public DataTable GetCallsListWithDate(CallsModel a)
          {
              DateTime dtf = Convert.ToDateTime(a.FromDate);
              DateTime dtt = Convert.ToDateTime(a.ToDate);

              DataTable DTCallsList = new DataTable();
              try
              {
                  SqlCommand CMDCallList = new SqlCommand("SP_CallsMaster", con);
                  CMDCallList.CommandType = CommandType.StoredProcedure;
                  CMDCallList.Parameters.AddWithValue("@SP_Type", "1AS");
                  CMDCallList.Parameters.AddWithValue("@CreatedDate", dtf);
                  CMDCallList.Parameters.AddWithValue("@ModifyDate", dtt);
                  CMDCallList.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                  //CMDCallList.CommandTimeout = 100000;
                  SqlDataAdapter ad = new SqlDataAdapter(CMDCallList);
                  ad.Fill(DTCallsList);
              }
              catch (Exception ex)
              {
                  string Error = ex.Message.ToString();
              }
              finally
              {
                  DTCallsList.Dispose();
              }
              return DTCallsList;
          }

      */

        public DataTable GetCallsListWithDate(CallsModel a)
        {
            DateTime dtf = new DateTime();
            DateTime dtt = new DateTime();
          /*  if (a.FromDate != null && a.ToDate != null)
            {

                dtf = Convert.ToDateTime(a.FromDate);
                dtt = Convert.ToDateTime(a.ToDate);
            }
            */


            DataTable DTCallsList = new DataTable();
            try
            {
                SqlCommand CMDCallList = new SqlCommand("SP_CallsMaster", con);
                CMDCallList.CommandType = CommandType.StoredProcedure;
                CMDCallList.Parameters.AddWithValue("@SP_Type", "1AS");
                CMDCallList.Parameters.AddWithValue("@CreatedDate", DateTime.ParseExact(DateTime.ParseExact(a.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDCallList.Parameters.AddWithValue("@ModifyDate", DateTime.ParseExact(DateTime.ParseExact(a.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                CMDCallList.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                //CMDCallList.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDCallList);
                ad.Fill(DTCallsList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTCallsList.Dispose();
            }
            return DTCallsList;
        }

        public DataSet GetCallmanagmentsListByConditionTest(CallsModel CM)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", "11NN");
                CMDEditContact.Parameters.AddWithValue("@Fromdate", CM.FromDate);
                CMDEditContact.Parameters.AddWithValue("@ToDate", CM.ToDate);
                CMDEditContact.Parameters.AddWithValue("@Call_No", CM.Call_No);
                CMDEditContact.Parameters.AddWithValue("@Branch", CM.Excuting_Branch);
                CMDEditContact.Parameters.AddWithValue("@originating_Branch", CM.Originating_Branch);

                CMDEditContact.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
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


        public string AssigntUpdate(CallsModel URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (URS.PK_Call_ID != 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 12);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AssignStatus", URS.AssignStatus);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.Inspector);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_Call_ID", URS.PK_Call_ID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifyBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                }
                else
                {
                    //SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    //CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);


                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    //Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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


        public DataTable GetCallAssignList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 0;
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 13);
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "13N");
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
        public DataTable GetAssignmentDashBoardWithDate(CallsModel a)
        {
          //  DateTime dtf = Convert.ToDateTime(a.From_Date);
            //DateTime dtt = Convert.ToDateTime(a.To_Date);
            string DateF =  a.FromDate;
            string DateT = a.ToDate;
            string CallNo = a.Call_No;
            int BrId = a.Br_Id;
            int ExBr_Id = Convert.ToInt32( a.Originating_Branch);

            DataTable DTAssignment = new DataTable();
            try
            {
                SqlCommand CMDAssignment = new SqlCommand("SP_CallsMaster", con);
                CMDAssignment.CommandType = CommandType.StoredProcedure;
                CMDAssignment.Parameters.AddWithValue("@SP_Type", "13NN");
                //CMDAssignment.Parameters.AddWithValue("@FromDate", DateF);
                //CMDAssignment.Parameters.AddWithValue("@ToDate", DateT);
                if (a.FromDate != null && a.ToDate != null)
                {
                    CMDAssignment.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(a.FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                    CMDAssignment.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(a.ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                }
                CMDAssignment.Parameters.AddWithValue("@Call_No", CallNo);
                //CMDAssignment.Parameters.AddWithValue("@ToDate", BrId);
                CMDAssignment.Parameters.AddWithValue("@Br_Id", ExBr_Id);
                CMDAssignment.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAssignment.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDAssignment);
                ad.Fill(DTAssignment);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAssignment.Dispose();
            }
            return DTAssignment;
        }

        public string UpdateCallAssignBydate(CallsModel URS)
        {
            string Result = string.Empty;
            DateTime strDate = DateTime.ParseExact(URS.Actual_Visit_Date, "dd/MM/yyyy", provider);
            con.Open();
            try
            {
         //     LogFile(strDate.ToString(), "strDate");
                if (URS.PK_Call_ID != 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.CommandTimeout = 0;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 15);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AssignStatus", URS.AssignStatus);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_Call_ID", URS.PK_Call_ID);
                    //added by shrutika salve 19032024
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@isCompetant", URS.inspectorCompetant);
                    //added by shrutika salve 04042024
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@isinspectorApproved", URS.inspectorapproved);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@Actual_Visit_Date", URS.Actual_Visit_Date);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@Actual_Visit_Date", URS.Actual_Visit_Date);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(URS.Actual_Visit_Date, "dd/MM/yyyy", provider));


                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Status", URS.Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.Inspector);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                }
                else
                {
                    //SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    //CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);


                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    //Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                }
                

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
              //  LogFile(ex.Message , "UpdateCallAssignment");
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

        public DataSet GetEmailDetails(int CallId)
        {
            DataSet Result = new DataSet();
            con.Open();
            try
            {
                if (CallId != 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.CommandTimeout = 0;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 54);                    
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_Call_ID", CallId);                    
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifyBy", System.Web.HttpContext.Current.Session["UserIDs"]);
                    
                    SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDInsertUpdateUsers);
                    SDAEditContact.Fill(Result);
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

        public DataSet EditUserDetails(string Inspector)
        {
            DataSet DTEditContact = new DataSet();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 16);
                CMDEditContact.Parameters.AddWithValue("@Inspector", Inspector);
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


        public DataTable GetCallsListByInspector()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 17);
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

        public int DeleteCalls(int? PK_Call_ID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDContactDelete = new SqlCommand("SP_CallsMaster", con);
                CMDContactDelete.CommandType = CommandType.StoredProcedure;
                CMDContactDelete.CommandTimeout = 100000;
                CMDContactDelete.Parameters.AddWithValue("@SP_Type", 19);
                CMDContactDelete.Parameters.AddWithValue("@PK_Call_ID", PK_Call_ID);
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


        public string UpdateReason(CallsModel URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (URS.PK_Call_ID != 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CallsMaster", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 20);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Status", URS.Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AssignStatus", URS.AssignStatus);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@IsVisitReportGenerated", URS.IsVisitReportGenerated);
                    
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Inspector", URS.Inspector);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_Call_ID", URS.PK_Call_ID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Reasion", URS.Reasion);
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

        public DataSet GetEmailByBranch(int brid)
        {
            DataSet DTGetTUVEmail = new DataSet();
            try
            {
                SqlCommand GetBREmail = new SqlCommand("SP_CallsMaster", con);
                GetBREmail.CommandType = CommandType.StoredProcedure;
                GetBREmail.Parameters.AddWithValue("@SP_Type", 50);
                GetBREmail.Parameters.AddWithValue("@Br_Id", brid);

                SqlDataAdapter dr = new SqlDataAdapter(GetBREmail);
                dr.Fill(DTGetTUVEmail);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetTUVEmail.Dispose();
            }
            return DTGetTUVEmail;
        }
        // Code Added By Manoj Sharma'
        public DataSet CheckValidCall(string JobNumber)
        {
            DataSet DSCheckValid = new DataSet();
            try
            {
                SqlCommand CMDCheckValid = new SqlCommand("SP_CallsMaster", con);
                CMDCheckValid.CommandType = CommandType.StoredProcedure;
                CMDCheckValid.Parameters.AddWithValue("@SP_Type", 51);
                CMDCheckValid.Parameters.AddWithValue("@JobNumber", JobNumber);
                SqlDataAdapter SDACheckValid = new SqlDataAdapter(CMDCheckValid);
                SDACheckValid.Fill(DSCheckValid);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSCheckValid.Dispose();
            }
            return DSCheckValid;
        }


        public DataSet GetInspectorName(int callId)
        {
            DataSet DSCheckValid = new DataSet();
            try
            {
                SqlCommand CMDCheckValid = new SqlCommand("SP_CallsMaster", con);
                CMDCheckValid.CommandType = CommandType.StoredProcedure;
                CMDCheckValid.Parameters.AddWithValue("@SP_Type", 56);
                CMDCheckValid.Parameters.AddWithValue("@PK_Call_ID", callId);
                SqlDataAdapter SDACheckValid = new SqlDataAdapter(CMDCheckValid);
                SDACheckValid.Fill(DSCheckValid);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSCheckValid.Dispose();
            }
            return DSCheckValid;
        }

        public DataSet GetInspectorDetails(string inspector)
        {
            DataSet DSCheckValid = new DataSet();
            try
            {
                SqlCommand CMDCheckValid = new SqlCommand("SP_CallsMaster", con);
                CMDCheckValid.CommandType = CommandType.StoredProcedure;
                CMDCheckValid.Parameters.AddWithValue("@SP_Type", 57);
                CMDCheckValid.Parameters.AddWithValue("@Inspector", inspector);
                SqlDataAdapter SDACheckValid = new SqlDataAdapter(CMDCheckValid);
                SDACheckValid.Fill(DSCheckValid);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSCheckValid.Dispose();
            }
            return DSCheckValid;
        }

        

        //**********************Manoj Added code for Uploading File in database on 12 March 2020
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int CALL_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_CALLID", typeof(int)));
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
                    DTUploadFile.Rows.Add(CALL_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now,item.FileContent);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListCallMasterUploadedFile", DTUploadFile);
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
        public DataTable EditUploadedFile(int? CALL_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_CALLID", CALL_ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_CallMasterUploadedFile", con);
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


        public DataTable GetFileContent(int? EQ_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();

            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CallMasterUploadedFile", con);
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


        #region
        public DataTable GetCallmanagmentsListAfterSearch(CallsModel vcm)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                #region previous call SP 
                //CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 11);
                #endregion
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "vaibhav");
                CMDGetDdlLst.Parameters.AddWithValue("@From_Date", vcm.FromDate);
                CMDGetDdlLst.Parameters.AddWithValue("@To_Date", vcm.ToDate);
                CMDGetDdlLst.Parameters.AddWithValue("@Call_No", vcm.Call_No);
                CMDGetDdlLst.Parameters.AddWithValue("@Tuv_Branch", vcm.Br_Id);

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

        public DataSet ChkLeave(string ActualVisitDate, string Inspector)
        {
            DataSet DTGetTUVEmail = new DataSet();
            try
            {
                SqlCommand GetBREmail = new SqlCommand("SP_CallsMaster", con);
                GetBREmail.CommandType = CommandType.StoredProcedure;
                GetBREmail.Parameters.AddWithValue("@SP_Type", 60);

                /// GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(ActualVisitDate, "dd/MM/yyyy", null));

                GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(DateTime.ParseExact(ActualVisitDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                GetBREmail.Parameters.AddWithValue("@CreatedBy", Inspector);
                SqlDataAdapter dr = new SqlDataAdapter(GetBREmail);
                dr.Fill(DTGetTUVEmail);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetTUVEmail.Dispose();
            }
            return DTGetTUVEmail;
        }

        public DataSet ChkLeaveForCountinuous(string FromDate, string ToDate, string ActualVisitDate, string Inspector)
        {
            DataSet DTGetTUVEmail = new DataSet();
            try
            {
                SqlCommand GetBREmail = new SqlCommand("SP_CallsMaster", con);
                GetBREmail.CommandType = CommandType.StoredProcedure;
                GetBREmail.Parameters.AddWithValue("@SP_Type", 61);

                GetBREmail.Parameters.AddWithValue("@From_Date", DateTime.ParseExact(DateTime.ParseExact(FromDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                GetBREmail.Parameters.AddWithValue("@To_Date", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));
                GetBREmail.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(DateTime.ParseExact(ActualVisitDate, "dd/mm/yyyy", provider).ToString("mm/dd/yyyy"), "MM/dd/yyyy", theCultureInfo));

                GetBREmail.Parameters.AddWithValue("@CreatedBy", Inspector);
                SqlDataAdapter dr = new SqlDataAdapter(GetBREmail);
                dr.Fill(DTGetTUVEmail);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetTUVEmail.Dispose();
            }
            return DTGetTUVEmail;
        }
		
		public DataTable GetCallDetailsNew(int callId, string ArrPK_Call_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "55N");
                CMDGetDdlLst.Parameters.AddWithValue("@PK_Call_ID", callId);
                CMDGetDdlLst.Parameters.AddWithValue("@ArrPK_Call_ID", ArrPK_Call_ID);
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
		
        public void LogFile(string strMessage, string strMethodName)
        {
            try
            {
                string strLogPath = ConfigurationManager.AppSettings["LogPath"].ToString();

                string strFileName = "LogFile_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt";

                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", strLogPath, strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMethodName + ":" + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();

            }
            catch (Exception ex)
            {

            }
        }

        public string GetPlannedDate(string pkCallID) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            string strPlannedDate = string.Empty;
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 68);
                CMDCallDash.Parameters.AddWithValue("@PK_Call_ID", pkCallID);
                
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);

                strPlannedDate = DTDashBoard.Rows[0]["PlannedDate"].ToString();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return strPlannedDate;
        }

        public DataTable GetInspectorTS(string InspectorName,string plannedDate) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 69);
                CMDCallDash.Parameters.AddWithValue("@Inspector", InspectorName);
                CMDCallDash.Parameters.AddWithValue("@TSDate", plannedDate);                
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        public DataTable CallReviseData(int? PK_Call_ID)
        {
            DataTable DTEditContact = new DataTable();
            try
            {
                SqlCommand CMDEditContact = new SqlCommand("SP_CallsMaster", con);
                CMDEditContact.CommandType = CommandType.StoredProcedure;
                CMDEditContact.Parameters.AddWithValue("@SP_Type", 70);
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

        //public string InsertUpdateAddMandays(CallsModel CPM)
        //{
        //    int ReturnId = 0;
        //    string Result = string.Empty;
        //    con.Open();
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        //    try
        //    {

        //            SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_CallsMaster", con);
        //            CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
        //            CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 67);
        //            CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);



        //            CMDInsertUpdateJOB.Parameters.AddWithValue("@Reasion", CPM.Reason);
        //            CMDInsertUpdateJOB.Parameters.AddWithValue("@ActionSelected", CPM.ActionSelected);
        //            if (CPM.NewPlannedDate != null)
        //            {
        //                CMDInsertUpdateJOB.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(CPM.NewPlannedDate, "dd/MM/yyyy", theCultureInfo));
        //            }


        //            CMDInsertUpdateJOB.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //          //  CMDInsertUpdateJOB.Parameters.Add("@ReturnId", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            Result = CMDInsertUpdateJOB.ExecuteNonQuery().ToString();
        //            //ReturnId = Convert.ToInt32(CMDInsertUpdateJOB.Parameters["@ReturnId"].Value.ToString());
        //            //Result = Convert.ToString(ReturnId);

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
        //added by nikita on 23052024
        public string InsertUpdateAddMandays(CallsModel CPM)
        {
           
            string Result = string.Empty;
            DataTable dtResult = new DataTable();

            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_CallsMaster", con);
                CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 67);
                CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_Call_ID", CPM.PK_Call_ID);
                CMDInsertUpdateJOB.Parameters.AddWithValue("@Reasion", CPM.Reason);
                CMDInsertUpdateJOB.Parameters.AddWithValue("@ActionSelected", CPM.ActionSelected);
                if (CPM.NewPlannedDate != null)
                {
                    CMDInsertUpdateJOB.Parameters.AddWithValue("@Actual_Visit_Date", DateTime.ParseExact(CPM.NewPlannedDate, "dd/MM/yyyy", theCultureInfo));
                }
                CMDInsertUpdateJOB.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDInsertUpdateJOB);
                SDAEditContact.Fill(dtResult);
                string firstColumnValue = dtResult.Rows[0][0] != DBNull.Value ? dtResult.Rows[0][0].ToString() : string.Empty;
                string secondColumnValue = dtResult.Rows[0][1] != DBNull.Value ? dtResult.Rows[0][1].ToString() : string.Empty;
                string OpeSendForapproval = dtResult.Rows[0][2] != DBNull.Value ? dtResult.Rows[0][2].ToString() : string.Empty;
                string ExpenseSendForapproval = dtResult.Rows[0][3] != DBNull.Value ? dtResult.Rows[0][3].ToString() : string.Empty;

                Result = firstColumnValue + "," + secondColumnValue + "," + OpeSendForapproval + "," + ExpenseSendForapproval;
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
        public string CheckSingleCall(string CallID)
        {

            DataTable dtResult = new DataTable();
            string Result = string.Empty;
            con.Open();
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {

                SqlCommand CMDInsertUpdateJOB = new SqlCommand("SP_CallsMaster", con);
                CMDInsertUpdateJOB.CommandType = CommandType.StoredProcedure;
                CMDInsertUpdateJOB.Parameters.AddWithValue("@SP_Type", 71);
                CMDInsertUpdateJOB.Parameters.AddWithValue("@PK_Call_ID", CallID);
                SqlDataAdapter SDAEditContact = new SqlDataAdapter(CMDInsertUpdateJOB);
                SDAEditContact.Fill(dtResult);
                Result = dtResult.Rows[0][0].ToString();


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

        public DataTable GetMentorName(string pkCallID) //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            string strPlannedDate = string.Empty;
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("sp_CallsMaster", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 72);
                CMDCallDash.Parameters.AddWithValue("@PK_Call_ID", pkCallID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);

             

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }

        
        public List<CallReasonMaster> GetReasonList(string Type)// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CallReasonMaster> lstEnquiryDashB = new List<CallReasonMaster>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 73);
                CMDGetEnquriy.Parameters.AddWithValue("@Source", Type);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new CallReasonMaster
                           {
                               CReason_Id = Convert.ToInt32(dr["CReason_ID"]),
                               Reason = Convert.ToString(dr["Reason"]),
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


        public string UpdateDeclareStatus(string pkCallID)// Binding Sales Masters DashBoard of Master Page 
        {
            int Result = 0;

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 73);                
                CMDGetEnquriy.Parameters.AddWithValue("@PK_Call_ID", pkCallID);
                Result = CMDGetEnquriy.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                Result = 0;
            }
            return Result.ToString();
        }


        public DataTable GetScopeDetails(string ProdList,string stageof)// Binding Sales Masters DashBoard of Master Page 
        {
            string Result = string.Empty;
            DataTable DTDashBoard = new DataTable();

            try
            {

                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 75);
                CMDGetEnquriy.Parameters.AddWithValue("@Product_item", ProdList);
                CMDGetEnquriy.Parameters.AddWithValue("@ProductStage", stageof);
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


            return DTDashBoard;
        }


        public string GetCompetancy(string Name,string Scope)// Binding Sales Masters DashBoard of Master Page 
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 76);
                CMDGetEnquriy.Parameters.AddWithValue("@Inspector", Name);
                CMDGetEnquriy.Parameters.AddWithValue("@Product_item", Scope);

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
            
            return Result.ToString();
        }

        public DataTable GetExpeditingCallsListByInspector()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 77);
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

        //added by shrutika salve 28032024

        public string GetFormFilled(string Name)// Binding Sales Masters DashBoard of Master Page 
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 80);
                CMDGetEnquriy.Parameters.AddWithValue("@Inspector", Name);


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

            return Result.ToString();
        }

        public string GetcustomerApproved(string inspectorName, string CustomerName)// Binding Sales Masters DashBoard of Master Page 
        {
            string Result = string.Empty;

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 81);
                CMDGetEnquriy.Parameters.AddWithValue("@Customer", CustomerName);
                CMDGetEnquriy.Parameters.AddWithValue("@Inspector", inspectorName);

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

            return Result.ToString();
        }
        public DataTable CheckExpenseApproval(int? PK_Call_ID)
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", "79N");
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
        //added by nikita on 20052024

        public DataTable GetSatusForMail(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("Sp_OPE_Mail_status", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@Pk_call_id", PK_JOB_ID);

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

        public DataSet GetDatainspectorcall()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_CallsMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "81A");
                //cmd.Parameters.AddWithValue("@UserID", UserId1);
                cmd.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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

        public DataTable GetcancelsList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_CallcancelData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
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


        public DataTable CallsDataSearch(string fromDate, string ToDate)
        {
            DataTable dsCalls = new DataTable();
            try
            {
                SqlCommand cmdCall = new SqlCommand("Sp_CallcancelData", con);
                cmdCall.CommandType = CommandType.StoredProcedure;
                cmdCall.Parameters.AddWithValue("@SP_Type", "1");

                cmdCall.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmdCall.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));




                SqlDataAdapter daCall = new SqlDataAdapter(cmdCall);
                daCall.Fill(dsCalls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsCalls.Dispose();
            }
            return dsCalls;
        }



        public DataTable GetNotDoneList()
        {
            DataTable DSGetddlList = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_CallcancelData", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
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


        public DataTable CallNotDoneSearch(string fromDate, string ToDate)
        {
            DataTable dsCalls = new DataTable();
            try
            {
                SqlCommand cmdCall = new SqlCommand("Sp_CallcancelData", con);
                cmdCall.CommandType = CommandType.StoredProcedure;
                cmdCall.Parameters.AddWithValue("@SP_Type", "3");

                cmdCall.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmdCall.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(ToDate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));




                SqlDataAdapter daCall = new SqlDataAdapter(cmdCall);
                daCall.Fill(dsCalls);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsCalls.Dispose();
            }
            return dsCalls;
        }


        public DataSet DataCount()//Get All DropDownlist 
        {

            DataSet DSGetddlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CallsMaster", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 82);

                CMDGetDdlLst.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
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

        public DataTable GetConsumedMandaysData(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ConsumedMandays_mail", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                CMDEditUploadedFile.Parameters.AddWithValue("@Pk_Job_id", PK_JOB_ID);

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



        public DataTable UpdateMailflag(int? PK_JOB_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ConsumedMandays_mail", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@Pk_Job_id", PK_JOB_ID);

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

    }
}