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
    public class DALInternalAuditReport
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
        public DataTable GetAuditReportDashBoard()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 1);
                //CMDCallDash.Parameters.AddWithValue("@FKAuditId", FKAuditId);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch(Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }

       
        public string  InsertUpdateforAuditReport(Internal_Audit_Report IntAudit,string Ipath,string IPathSupportingDocument)
        {
            string result = string.Empty;
            int ReturnId = 0;
            con.Open();
            if (IntAudit.Internal_Audit_Id != null)
            {
                if (IntAudit.Total_NCR_raised == 0)
                {
                    IntAudit.Total_NCR_raised = 1;
                }
                else
                {
                    IntAudit.Total_NCR_raised = IntAudit.Total_NCR_raised + 1;
                }
                try
                {
                    SqlCommand CMDAudit = new SqlCommand("Sp_Internal_Audit_Report", con);
                    CMDAudit.CommandType = CommandType.StoredProcedure;
                    CMDAudit.CommandTimeout = 100000;
                    CMDAudit.Parameters.AddWithValue("@SP_Type", 4);
                    CMDAudit.Parameters.AddWithValue("@Internal_Audit_Id", IntAudit.Internal_Audit_Id);
                    CMDAudit.Parameters.AddWithValue("@AuditorId", IntAudit.AuditorId);
                    CMDAudit.Parameters.AddWithValue("@Branch", IntAudit.Branch);
                    CMDAudit.Parameters.AddWithValue("@Auditor", IntAudit.Auditor);
                    CMDAudit.Parameters.AddWithValue("@ExAuditor", IntAudit.ExAuditor);
                    //DateTime.ParseExact(IntAudit.Date_Of_Audit, "dd/MM/yyyy", null));
                    //CMDAudit.Parameters.AddWithValue("@Date_Of_Audit", IntAudit.Date_Of_Audit);
                    //CMDAudit.Parameters.AddWithValue("@Date_Of_Audit", DateTime.ParseExact(IntAudit.SDate_Of_Audit, "dd/MM/yyyy", null));
                    CMDAudit.Parameters.AddWithValue("@Auditee", IntAudit.Auditee);
                    CMDAudit.Parameters.AddWithValue("@Nominated_By_Management_Remark", IntAudit.Nominated_By_Management_Remark);
                    CMDAudit.Parameters.AddWithValue("@Nominated_By_Management_NCR", IntAudit.Nominated_By_Management_NCR);
                    CMDAudit.Parameters.AddWithValue("@Findings_From_PreviousAudit_Remark", IntAudit.Findings_From_PreviousAudit_Remark);
                    CMDAudit.Parameters.AddWithValue("@Findings_From_PreviousAudit_NCR", IntAudit.Findings_From_PreviousAudit_NCR);
                    CMDAudit.Parameters.AddWithValue("@Customer_Complaints_Remarks", IntAudit.Customer_Complaints_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Customer_Complaints_NCR", IntAudit.Customer_Complaints_NCR);
                    CMDAudit.Parameters.AddWithValue("@CustomerFeedBacknAnalysis_Remark", IntAudit.CustomerFeedBacknAnalysis_Remark);
                    CMDAudit.Parameters.AddWithValue("@CustomerFeedBacknAnalysis_NCR", IntAudit.CustomerFeedBacknAnalysis_NCR);
                    CMDAudit.Parameters.AddWithValue("@Process_Measures_Remarks", IntAudit.Process_Measures_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Process_Measures_NCR", IntAudit.Process_Measures_NCR);
                    CMDAudit.Parameters.AddWithValue("@Training_AppraisalnKRAs_Remarks", IntAudit.Training_AppraisalnKRAs_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Training_AppraisalnKRAs_NCR", IntAudit.Training_AppraisalnKRAs_NCR);
                    CMDAudit.Parameters.AddWithValue("@Enquiry_Management_Process_Remarks", IntAudit.Enquiry_Management_Process_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Enquiry_Management_Process_NCR", IntAudit.Enquiry_Management_Process_NCR);
                    CMDAudit.Parameters.AddWithValue("@Quotation_Process_Remarks", IntAudit.Quotation_Process_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Quotation_Process_NCR", IntAudit.Quotation_Process_NCR);
                    CMDAudit.Parameters.AddWithValue("@OrderReviewnAccepatanceContractreview_Remarks", IntAudit.OrderReviewnAccepatanceContractreview_Remarks);
                    CMDAudit.Parameters.AddWithValue("@OrderReviewnAccepatanceContractreview_NCR", IntAudit.OrderReviewnAccepatanceContractreview_NCR);
                    CMDAudit.Parameters.AddWithValue("@OrganisationStructure_Remarks", IntAudit.OrganisationStructure_Remarks);
                    CMDAudit.Parameters.AddWithValue("@OrganisationStructure_NCR", IntAudit.OrganisationStructure_NCR);
                    CMDAudit.Parameters.AddWithValue("@Control_Of_Documents_Remarks", IntAudit.Control_Of_Documents_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Control_Of_Documents_NCR", IntAudit.Control_Of_Documents_NCR);
                    CMDAudit.Parameters.AddWithValue("@Control_Of_Records_Remarks", IntAudit.Control_Of_Records_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Control_Of_Records_NCR", IntAudit.Control_Of_Records_NCR);
                    CMDAudit.Parameters.AddWithValue("@Competancy_Mapping_Remarks", IntAudit.Competancy_Mapping_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Competancy_Mapping_NCR", IntAudit.Competancy_Mapping_NCR);
                    CMDAudit.Parameters.AddWithValue("@TrainingRecords_Effectiveness_Remarks", IntAudit.TrainingRecords_Effectiveness_Remarks);
                    CMDAudit.Parameters.AddWithValue("@TrainingRecords_Effectiveness_NCR", IntAudit.TrainingRecords_Effectiveness_NCR);
                    CMDAudit.Parameters.AddWithValue("@Impartiality_Confidentiality_Remarks", IntAudit.Impartiality_Confidentiality_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Impartiality_Confidentiality_NCR", IntAudit.Impartiality_Confidentiality_NCR);
                    CMDAudit.Parameters.AddWithValue("@InspectionCordinationnInspectionProcess_Remarks", IntAudit.InspectionCordinationnInspectionProcess_Remarks);
                    CMDAudit.Parameters.AddWithValue("@InspectionCordinationnInspectionProcess_NCR", IntAudit.InspectionCordinationnInspectionProcess_NCR);
                    CMDAudit.Parameters.AddWithValue("@Onsite_Monitoring_Remarks", IntAudit.Onsite_Monitoring_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Onsite_Monitoring_NCR", IntAudit.Onsite_Monitoring_NCR);
                    CMDAudit.Parameters.AddWithValue("@Document_Work_Review_Remarks", IntAudit.Document_Work_Review_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Document_Work_Review_NCR", IntAudit.Document_Work_Review_NCR);
                    CMDAudit.Parameters.AddWithValue("@Safety_Of_Personnel_Remarks", IntAudit.Safety_Of_Personnel_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Safety_Of_Personnel_NCR", IntAudit.Safety_Of_Personnel_NCR);
                    CMDAudit.Parameters.AddWithValue("@Planning_opportunities_Remarks", IntAudit.Planning_opportunities_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Planning_opportunities_NCR", IntAudit.Planning_opportunities_NCR);
                    CMDAudit.Parameters.AddWithValue("@Department", IntAudit.Department);
                    CMDAudit.Parameters.AddWithValue("@Plan_ref_No", IntAudit.Plan_ref_No);
                    CMDAudit.Parameters.AddWithValue("@Total_NCR_raised", IntAudit.Total_NCR_raised);
                    CMDAudit.Parameters.AddWithValue("@Academic_records_Remarks", IntAudit.Academic_records_Remarks);
                    CMDAudit.Parameters.AddWithValue("@Academic_records_NCR", IntAudit.Academic_records_NCR);
                    CMDAudit.Parameters.AddWithValue("@NDE_certifications_Remark", IntAudit.NDE_certifications_Remark);
                    CMDAudit.Parameters.AddWithValue("@NDE_certifications_NCR", IntAudit.NDE_certifications_NCR);
                    CMDAudit.Parameters.AddWithValue("@Visual_Activity_Remarks", IntAudit.Visual_Activity_Remarks);
                    CMDAudit.Parameters.AddWithValue("@PDF", IntAudit.PDF);
                    CMDAudit.Parameters.AddWithValue("@Visual_activity_NCR", IntAudit.Visual_activity_NCR);
                    CMDAudit.Parameters.AddWithValue("@AdditionalRemarks", IntAudit.AdditionalRemarks);
                    if (Ipath != null && Ipath != "")
                    {
                        CMDAudit.Parameters.AddWithValue("@NCDocument", Ipath);
                    }
                    if (IPathSupportingDocument != null && IPathSupportingDocument != "")
                    {
                        CMDAudit.Parameters.AddWithValue("@SupportingDocument", IPathSupportingDocument);
                    }
                    //CMDAudit.Parameters.AddWithValue("@DateOfAudit_TODate", IntAudit.DateOfAudit_TODate);
                    //CMDAudit.Parameters.AddWithValue("@DateOfAudit_TODate", DateTime.ParseExact(IntAudit.SDateOfAudit_TODate, "dd/MM/yyyy", null));
                    CMDAudit.Parameters.AddWithValue("@IsAuditCompleted", IntAudit.IsAuditCompleted);
                    CMDAudit.Parameters.AddWithValue("@AreFindingsClose", IntAudit.AreFindingsClose);

                    CMDAudit.Parameters.AddWithValue("@CriticalNCCount", IntAudit.CriticalNCCount);
                    CMDAudit.Parameters.AddWithValue("@MajorNCCount", IntAudit.MajorNCCount);
                    CMDAudit.Parameters.AddWithValue("@MinorNCCount", IntAudit.MinorNCCount);
                    CMDAudit.Parameters.AddWithValue("@OBSERVATIONCount", IntAudit.OBSERVATIONCount);
                    CMDAudit.Parameters.AddWithValue("@ConcernCount", IntAudit.ConcernCount); 
                    CMDAudit.Parameters.AddWithValue("@OFICount", IntAudit.OFICount);
                    CMDAudit.Parameters.AddWithValue("@TotalFindings", IntAudit.TotalFindings);
                    CMDAudit.Parameters.AddWithValue("@CostCenter", IntAudit.CostCenter);



                    SqlDataAdapter adaudit = new SqlDataAdapter(CMDAudit);
                    result = CMDAudit.ExecuteNonQuery().ToString();



                    ReturnId = Convert.ToInt32(IntAudit.Internal_Audit_Id);

                }
                catch(Exception ex)
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
                    SqlCommand CMDinsert = new SqlCommand("Sp_Internal_Audit_Report",con);
                    CMDinsert.CommandType = CommandType.StoredProcedure;
                    CMDinsert.CommandTimeout = 1000000;
                    CMDinsert.Parameters.AddWithValue("@SP_Type", 2);
                    CMDinsert.Parameters.AddWithValue("@Internal_Audit_Id", IntAudit.Internal_Audit_Id);
                    CMDinsert.Parameters.AddWithValue("@Branch", IntAudit.Branch);
                    CMDinsert.Parameters.AddWithValue("@Auditor", IntAudit.Auditor);
                    CMDinsert.Parameters.AddWithValue("@ExAuditor", IntAudit.ExAuditor);
                    CMDinsert.Parameters.AddWithValue("@AuditorId", IntAudit.AuditorId);
                    //CMDinsert.Parameters.AddWithValue("@Date_Of_Audit", IntAudit.Date_Of_Audit);
                    //CMDinsert.Parameters.AddWithValue("@Date_Of_Audit", DateTime.ParseExact(IntAudit.SDate_Of_Audit, "dd/MM/yyyy", null));
                    CMDinsert.Parameters.AddWithValue("@Auditee", IntAudit.Auditee);
                    CMDinsert.Parameters.AddWithValue("@Nominated_By_Management_Remark", IntAudit.Nominated_By_Management_Remark);
                    CMDinsert.Parameters.AddWithValue("@Nominated_By_Management_NCR", IntAudit.Nominated_By_Management_NCR);
                    CMDinsert.Parameters.AddWithValue("@Findings_From_PreviousAudit_Remark", IntAudit.Findings_From_PreviousAudit_Remark);
                    CMDinsert.Parameters.AddWithValue("@Findings_From_PreviousAudit_NCR", IntAudit.Findings_From_PreviousAudit_NCR);
                    CMDinsert.Parameters.AddWithValue("@Customer_Complaints_Remarks", IntAudit.Customer_Complaints_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Customer_Complaints_NCR", IntAudit.Customer_Complaints_NCR);
                    CMDinsert.Parameters.AddWithValue("@CustomerFeedBacknAnalysis_Remark", IntAudit.CustomerFeedBacknAnalysis_Remark);
                    CMDinsert.Parameters.AddWithValue("@CustomerFeedBacknAnalysis_NCR", IntAudit.CustomerFeedBacknAnalysis_NCR);
                    CMDinsert.Parameters.AddWithValue("@Process_Measures_Remarks", IntAudit.Process_Measures_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Process_Measures_NCR", IntAudit.Process_Measures_NCR);
                    CMDinsert.Parameters.AddWithValue("@Training_AppraisalnKRAs_Remarks", IntAudit.Training_AppraisalnKRAs_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Training_AppraisalnKRAs_NCR", IntAudit.Training_AppraisalnKRAs_NCR);
                    CMDinsert.Parameters.AddWithValue("@Enquiry_Management_Process_Remarks", IntAudit.Enquiry_Management_Process_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Enquiry_Management_Process_NCR", IntAudit.Enquiry_Management_Process_NCR);
                    CMDinsert.Parameters.AddWithValue("@Quotation_Process_Remarks", IntAudit.Quotation_Process_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Quotation_Process_NCR", IntAudit.Quotation_Process_NCR);
                    CMDinsert.Parameters.AddWithValue("@OrderReviewnAccepatanceContractreview_Remarks", IntAudit.OrderReviewnAccepatanceContractreview_Remarks);
                    CMDinsert.Parameters.AddWithValue("@OrderReviewnAccepatanceContractreview_NCR", IntAudit.OrderReviewnAccepatanceContractreview_NCR);
                    CMDinsert.Parameters.AddWithValue("@OrganisationStructure_Remarks", IntAudit.OrganisationStructure_Remarks);
                    CMDinsert.Parameters.AddWithValue("@OrganisationStructure_NCR", IntAudit.OrganisationStructure_NCR);
                    CMDinsert.Parameters.AddWithValue("@Control_Of_Documents_Remarks", IntAudit.Control_Of_Documents_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Control_Of_Documents_NCR", IntAudit.Control_Of_Documents_NCR);
                    CMDinsert.Parameters.AddWithValue("@Control_Of_Records_Remarks", IntAudit.Control_Of_Records_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Control_Of_Records_NCR", IntAudit.Control_Of_Records_NCR);
                    CMDinsert.Parameters.AddWithValue("@Competancy_Mapping_Remarks", IntAudit.Competancy_Mapping_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Competancy_Mapping_NCR", IntAudit.Competancy_Mapping_NCR);
                    CMDinsert.Parameters.AddWithValue("@TrainingRecords_Effectiveness_Remarks", IntAudit.TrainingRecords_Effectiveness_Remarks);
                    CMDinsert.Parameters.AddWithValue("@TrainingRecords_Effectiveness_NCR", IntAudit.TrainingRecords_Effectiveness_NCR);
                    CMDinsert.Parameters.AddWithValue("@Impartiality_Confidentiality_Remarks", IntAudit.Impartiality_Confidentiality_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Impartiality_Confidentiality_NCR", IntAudit.Impartiality_Confidentiality_NCR);
                    CMDinsert.Parameters.AddWithValue("@InspectionCordinationnInspectionProcess_Remarks", IntAudit.InspectionCordinationnInspectionProcess_Remarks);
                    CMDinsert.Parameters.AddWithValue("@InspectionCordinationnInspectionProcess_NCR", IntAudit.InspectionCordinationnInspectionProcess_NCR);
                    CMDinsert.Parameters.AddWithValue("@Onsite_Monitoring_Remarks", IntAudit.Onsite_Monitoring_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Onsite_Monitoring_NCR", IntAudit.Onsite_Monitoring_NCR);
                    CMDinsert.Parameters.AddWithValue("@Document_Work_Review_Remarks", IntAudit.Document_Work_Review_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Document_Work_Review_NCR", IntAudit.Document_Work_Review_NCR);
                    CMDinsert.Parameters.AddWithValue("@Safety_Of_Personnel_Remarks", IntAudit.Safety_Of_Personnel_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Safety_Of_Personnel_NCR", IntAudit.Safety_Of_Personnel_NCR);
                    CMDinsert.Parameters.AddWithValue("@Planning_opportunities_Remarks", IntAudit.Planning_opportunities_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Planning_opportunities_NCR", IntAudit.Planning_opportunities_NCR);
                    CMDinsert.Parameters.AddWithValue("@Department", IntAudit.Department);
                    CMDinsert.Parameters.AddWithValue("@Plan_ref_No", IntAudit.Plan_ref_No);
                    CMDinsert.Parameters.AddWithValue("@Total_NCR_raised", IntAudit.Total_NCR_raised);
                    CMDinsert.Parameters.AddWithValue("@Academic_records_Remarks", IntAudit.Academic_records_Remarks);
                    CMDinsert.Parameters.AddWithValue("@Academic_records_NCR", IntAudit.Academic_records_NCR);
                    CMDinsert.Parameters.AddWithValue("@NDE_certifications_Remark", IntAudit.NDE_certifications_Remark);
                    CMDinsert.Parameters.AddWithValue("@NDE_certifications_NCR", IntAudit.NDE_certifications_NCR);
                    CMDinsert.Parameters.AddWithValue("@Visual_Activity_Remarks", IntAudit.Visual_Activity_Remarks);
                    CMDinsert.Parameters.AddWithValue("@PDF", IntAudit.PDF);
                    CMDinsert.Parameters.AddWithValue("@Visual_activity_NCR", IntAudit.Visual_activity_NCR);
                    CMDinsert.Parameters.AddWithValue("@AdditionalRemarks", IntAudit.AdditionalRemarks);
                   
                    CMDinsert.Parameters.AddWithValue("@FKAuditId", IntAudit.AuditId);
                    if (Ipath != null && Ipath != "")
                    {
                        CMDinsert.Parameters.AddWithValue("@NCDocument", Ipath);
                    }
                    if (IPathSupportingDocument != null && IPathSupportingDocument != "")
                    {
                        CMDinsert.Parameters.AddWithValue("@SupportingDocument", IPathSupportingDocument);
                    }
                    //CMDinsert.Parameters.AddWithValue("@DateOfAudit_TODate", IntAudit.DateOfAudit_TODate);
                    //CMDinsert.Parameters.AddWithValue("@DateOfAudit_TODate", DateTime.ParseExact(IntAudit.SDateOfAudit_TODate, "dd/MM/yyyy", null));

                    CMDinsert.Parameters.AddWithValue("@IsAuditCompleted", IntAudit.IsAuditCompleted);
                    CMDinsert.Parameters.AddWithValue("@AreFindingsClose", IntAudit.AreFindingsClose);

                    CMDinsert.Parameters.AddWithValue("@CriticalNCCount", IntAudit.CriticalNCCount);
                    CMDinsert.Parameters.AddWithValue("@MajorNCCount", IntAudit.MajorNCCount);
                    CMDinsert.Parameters.AddWithValue("@MinorNCCount", IntAudit.MinorNCCount);
                    CMDinsert.Parameters.AddWithValue("@OBSERVATIONCount", IntAudit.OBSERVATIONCount);
                    CMDinsert.Parameters.AddWithValue("@ConcernCount", IntAudit.ConcernCount);
                    CMDinsert.Parameters.AddWithValue("@OFICount", IntAudit.OFICount);
                    CMDinsert.Parameters.AddWithValue("@TotalFindings", IntAudit.TotalFindings);
                    CMDinsert.Parameters.Add("@GetIARID", SqlDbType.Int).Direction = ParameterDirection.Output;
                    CMDinsert.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDinsert.Parameters.AddWithValue("@CostCenter", IntAudit.CostCenter); 
                     result = CMDinsert.ExecuteNonQuery().ToString();

                    ReturnId = Convert.ToInt32(CMDinsert.Parameters["@GetIARID"].Value);
                    System.Web.HttpContext.Current.Session["IARIDs"] = ReturnId;
                }
                catch(Exception ex)
                {
                    string message = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return Convert.ToString(ReturnId);
            return result;
        }


        public int InsertAuditDate(Internal_Audit_Report TSM)
        {
            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            int ReturnScheduleId = 0;
            string Result = string.Empty;
            con.Open();
            try
            {
                
                    SqlCommand cmd = new SqlCommand("SP_NonInspectionActivities", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", "26");
                    cmd.Parameters.AddWithValue("@DateSe", DateTime.ParseExact(TSM.SDate_Of_Audit, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@TrainingStartTime", TSM.Date_Of_Audit);
                    cmd.Parameters.AddWithValue("@StartTime",  TSM.OnSiteTime);
                cmd.Parameters.AddWithValue("@EndTime", TSM.OffSiteTime);
                cmd.Parameters.AddWithValue("@TravelTime", TSM.TravelTime);

                    cmd.Parameters.AddWithValue("@Call_No", TSM.FkAuditReportId);
                


                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();

                    //ReturnScheduleId = Convert.ToInt32(cmd.Parameters["@ReturnScheduleId"].Value.ToString());
               


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
            return ReturnScheduleId;
        }


        public string InsertActivityMasterWithData(Internal_Audit_Report IntAudit)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Internal_Audit_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '7');
                cmd.Parameters.AddWithValue("@FkActivityId", IntAudit.PKActivityTypeId);
                cmd.Parameters.AddWithValue("@FkAuditReportId", IntAudit.FkAuditReportId);
                cmd.Parameters.AddWithValue("@Remarks", IntAudit.Remarks);
                cmd.Parameters.AddWithValue("@CriticalNC", IntAudit.CriticalNC);
                cmd.Parameters.AddWithValue("@MajorNC", IntAudit.MajorNC);
                cmd.Parameters.AddWithValue("@MinorNC", IntAudit.MinorNC);
                cmd.Parameters.AddWithValue("@Observation", IntAudit.Observation);
                cmd.Parameters.AddWithValue("@Concern", IntAudit.Concern);
                cmd.Parameters.AddWithValue("@OFI", IntAudit.OFI);

                cmd.Parameters.AddWithValue("@EvidenceChecked", IntAudit.EvidenceChecked);
                cmd.Parameters.AddWithValue("@RCA", IntAudit.RCA);
                cmd.Parameters.AddWithValue("@CA", IntAudit.CA);
                cmd.Parameters.AddWithValue("@Correction", IntAudit.Correction);
                cmd.Parameters.AddWithValue("@Auditorremarks", IntAudit.Auditorremarks);


                
  
  
  
  

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
        public string UpdatePDF(Internal_Audit_Report IntAudit)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Internal_Audit_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '8');
               
                cmd.Parameters.AddWithValue("@PDF", IntAudit.PDF);
                cmd.Parameters.AddWithValue("@Internal_Audit_Id", IntAudit.FkAuditReportId);
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


        public DataTable EditReport(int? Auditid)
        {
            DataTable dtedit = new DataTable();
            try
            {
                SqlCommand CMDedit = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDedit.CommandType = CommandType.StoredProcedure;
                CMDedit.CommandTimeout = 100000;
                CMDedit.Parameters.AddWithValue("@SP_Type", 3);
                CMDedit.Parameters.AddWithValue("@Internal_Audit_Id", Auditid);
                SqlDataAdapter ADedit = new SqlDataAdapter(CMDedit);
                ADedit.Fill(dtedit);
            }
            catch(Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                dtedit.Dispose();
            }
            return dtedit;
        }



        public DataTable GetReportForPDF(String Auditid)
        {
            DataTable dtedit = new DataTable();
            try
            {
                SqlCommand CMDedit = new SqlCommand("SP_Audit", con);
                CMDedit.CommandType = CommandType.StoredProcedure;
                CMDedit.CommandTimeout = 100000;
                CMDedit.Parameters.AddWithValue("@SP_Type", "11");
                CMDedit.Parameters.AddWithValue("@AuditId", Auditid);
                SqlDataAdapter ADedit = new SqlDataAdapter(CMDedit);
                ADedit.Fill(dtedit);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                dtedit.Dispose();
            }
            return dtedit;
        }

        public int DeleteReport(int id)
        {
            int result = 0;
            try
            {
                con.Open();
                SqlCommand CMDdel = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDdel.CommandType = CommandType.StoredProcedure;
                CMDdel.CommandTimeout = 100000;
                CMDdel.Parameters.AddWithValue("@SP_Type", 5);
                CMDdel.Parameters.AddWithValue("@Internal_Audit_Id", id);
                result = CMDdel.ExecuteNonQuery();
                if (result != 0)
                {
                    return result;
                }
            }
            catch(Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public DataTable GetSignature(string sign)
        {
            DataTable dtsign = new DataTable();
            try
            {
                SqlCommand CMDedit = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDedit.CommandType = CommandType.StoredProcedure;
                CMDedit.CommandTimeout = 100000;
                CMDedit.Parameters.AddWithValue("@SP_Type", 6);
                CMDedit.Parameters.AddWithValue("@AuditorId", sign);
                SqlDataAdapter ADedit = new SqlDataAdapter(CMDedit);
                ADedit.Fill(dtsign);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                dtsign.Dispose();
            }
            return dtsign;
        }
        //*************************************** Get Signature By ID ******************************************
        //public Internal_Audit_Report GetSignature(string sign)
        //{
        //    Internal_Audit_Report _vmSignById = new Internal_Audit_Report();
        //    try
        //    {
        //        SqlCommand CMDsign = new SqlCommand("Sp_Internal_Audit_Report", con);
        //        CMDsign.CommandType = CommandType.StoredProcedure;
        //        CMDsign.Parameters.AddWithValue("@SP_Type", 6);
        //        CMDsign.Parameters.AddWithValue("@AuditorId", sign);
        //        con.Open();
        //        SqlDataReader dr = CMDsign.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            _vmSignById.AuditorSignature = Convert.ToString(dr["Signature"]);
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        string Error = Ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return _vmSignById;
        //}

        #region Added By Ankush for FileUpload   
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_IARID", typeof(int)));
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
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_IARUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListIARUploadedFile", DTUploadFile);
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
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_IARUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_IARID", ID);
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
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_IARUploadedFile", con);
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
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_IARUploadedFile", con);
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
        public string InsertFileAttachment1(List<FileDetails> lstFileUploaded, int ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_IARID", typeof(int)));
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
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_IARUploadedFile1", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListIARUploadedFile", DTUploadFile);
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
        public DataTable EditUploadedFile1(int? ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_IARUploadedFile1", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_IARID", ID);
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

        public string DeleteUploadedFile1(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_IARUploadedFile1", con);
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
        public DataTable GetFileExt1(string FileID)
        {
            DataTable DTGetFileExtenstion = new DataTable();
            try
            {
                SqlCommand CMDGetExtenstion = new SqlCommand("SP_IARUploadedFile1", con);
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


        public DataSet GetCostSheet()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "12");
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


        #endregion


        public DataTable GetDates(int? Auditid)
        {
            DataTable dtedit = new DataTable();
            try
            {
                SqlCommand CMDedit = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDedit.CommandType = CommandType.StoredProcedure;
                CMDedit.CommandTimeout = 100000;
                CMDedit.Parameters.AddWithValue("@SP_Type", 9);
                CMDedit.Parameters.AddWithValue("@PK_Call_ID", Auditid);
                CMDedit.Parameters.AddWithValue("@CreatedBy", UserIDs);

                SqlDataAdapter ADedit = new SqlDataAdapter(CMDedit);
                ADedit.Fill(dtedit);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                dtedit.Dispose();
            }
            return dtedit;
        }

        public DataTable GetScheduleDates(int? Auditid)
        {
            DataTable dtedit = new DataTable();
            try
            {
                SqlCommand CMDedit = new SqlCommand("Sp_Internal_Audit_Report", con);
                CMDedit.CommandType = CommandType.StoredProcedure;
                CMDedit.CommandTimeout = 100000;
                CMDedit.Parameters.AddWithValue("@SP_Type", 10);
                CMDedit.Parameters.AddWithValue("@PK_Call_ID", Auditid);
                //CMDedit.Parameters.AddWithValue("@CreatedBy", UserIDs);

                SqlDataAdapter ADedit = new SqlDataAdapter(CMDedit);
                ADedit.Fill(dtedit);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
            }
            finally
            {
                dtedit.Dispose();
            }
            return dtedit;
        }

        //added by rajvi 3feb2025
        public DataTable GetCertificateDataFinal(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("GetCertificateRecords", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 3);
                CMDCallDash.Parameters.AddWithValue("@Pk_id", id);// Using the emailId passed dynamically
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            return dt;
        }

        public DataTable GetFinalNDTQualifications()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("GetNDTQualRecords", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                // CMDCallDash.Parameters.AddWithValue("@EmployeeRollNo", rollno); // Using the emailId passed dynamically
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            return dt;
        }

        public DataTable GetCertificateData()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("GetCertificateRecords", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 1);
                CMDCallDash.Parameters.AddWithValue("@Pk_id", "");// Using the emailId passed dynamically
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            return dt;
        }



    }
}