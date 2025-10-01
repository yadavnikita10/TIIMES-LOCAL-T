using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Internal_Audit_Report
    {
        public List<Internal_Audit_Report> lstQuotationMasterDashBoard1 { get; set; }
        public List<Internal_Audit_Report> lstReport1 { get; set; }
        public int Count { get; set; }
        public int? Internal_Audit_Id { get; set; }

        public int AuditId { get; set; }
        public string Branch { get; set; }
        public string Auditor { get; set; }

        public string AuditorId { get; set; }
        public int Plan_Ref_Serial_No { get; set; }
        public string Auditee { get; set; }
        public string Department { get; set; }
        public DateTime Date_Of_Audit { get; set; }
        public string SDate_Of_Audit { get; set; }
        public string Nominated_By_Management_Remark { get; set; }
        public string Nominated_By_Management_NCR { get; set; }
        public string Findings_From_PreviousAudit_Remark { get; set; }
        public string Findings_From_PreviousAudit_NCR { get; set; }
        public string Customer_Complaints_Remarks { get; set; }
        public string Customer_Complaints_NCR { get; set; }
        public string CustomerFeedBacknAnalysis_Remark { get; set; }
        public string CustomerFeedBacknAnalysis_NCR { get; set; }
        public string Process_Measures_Remarks { get; set; }
        public string Process_Measures_NCR { get; set; }
        public string Training_AppraisalnKRAs_Remarks { get; set; }
        public string Training_AppraisalnKRAs_NCR { get; set; }
        public string Enquiry_Management_Process_Remarks { get; set; }
        public string Enquiry_Management_Process_NCR { get; set; }
        public string Quotation_Process_Remarks { get; set; }
        public string Quotation_Process_NCR { get; set; }
        public string OrderReviewnAccepatanceContractreview_Remarks { get; set; }
        public string OrderReviewnAccepatanceContractreview_NCR { get; set; }
        public string OrganisationStructure_Remarks { get; set; }
        public string OrganisationStructure_NCR { get; set; }
        public string Control_Of_Documents_Remarks { get; set; }
        public string Control_Of_Documents_NCR { get; set; }
        public string Control_Of_Records_Remarks { get; set; }
        public string Control_Of_Records_NCR { get; set; }
        public string Competancy_Mapping_Remarks { get; set; }
        public string Competancy_Mapping_NCR { get; set; }
        public string TrainingRecords_Effectiveness_Remarks { get; set; }
        public string TrainingRecords_Effectiveness_NCR { get; set; }
        public string Impartiality_Confidentiality_Remarks { get; set; }
        public string Impartiality_Confidentiality_NCR { get; set; }
        public string InspectionCordinationnInspectionProcess_Remarks { get; set; }
        public string InspectionCordinationnInspectionProcess_NCR { get; set; }
        public string Onsite_Monitoring_Remarks { get; set; }
        public string Onsite_Monitoring_NCR { get; set; }
        public string Document_Work_Review_Remarks { get; set; }
        public string Document_Work_Review_NCR { get; set; }
        public string Safety_Of_Personnel_Remarks { get; set; }
        public string Safety_Of_Personnel_NCR { get; set; }
        public string Planning_opportunities_Remarks { get; set; }
        public string Planning_opportunities_NCR { get; set; }
        //public int Plan_ref_No { get; set; }
        public string Plan_ref_No { get; set; }
        public string Academic_records_Remarks { get; set; }
        public string Academic_records_NCR { get; set; }
        public string NDE_certifications_Remark { get; set; }
        public string NDE_certifications_NCR { get; set; }
        public string Visual_Activity_Remarks { get; set; }
        public string Visual_activity_NCR { get; set; }
        public string Auditors_Signature { get; set; }
        public int Total_NCR_raised { get; set; }
        public int Total_obeservations { get; set; }
        public string PDF { get; set; }
        public string CostCenter_ { get; set; } //added by nikita on 12012024
        public string Pk_CC_Id { get; set; } //added by nikita on 12012024
        public string NCDocument { get; set; }

        public string SupportingDocument { get; set; }

        public DateTime DateOfAudit_TODate { get; set; }
        public string SDateOfAudit_TODate { get; set; }

        public string IsAuditCompleted { get; set; }

        public string AreFindingsClose { get; set; }
        
        public string CriticalNCCount         { get; set; }
        public string MajorNCCount            { get; set; }
        public string MinorNCCount            { get; set; }
        public string OBSERVATIONCount        { get; set; }
        public string ConcernCount            { get; set; }
        public string OFICount { get; set; }
        public string AdditionalRemarks { get; set; }
        public string AuditorSignature { get; set; }
        public string ExAuditor { get; set; }
        public string ActualAuditDateFrom { get; set; }
        public string ActualAuditDateTo { get; set; }
        public string ProposeDateFrom { get; set; }
        public string ProposeDateTo { get; set; }

        public string SaveButtonVisible { get; set; }
        public string LoginBy { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<FileDetails> FileDetails1 { get; set; }

        public List<Internal_Audit_Report> Activity { get; set; }

        public string Remarks { get; set; }



        public string PKActivityTypeId { get; set; }
        public string CriticalNC { get; set; }
        public string MajorNC { get; set; }
        public string MinorNC { get; set; }
        public string Observation { get; set; }
        public string Concern { get; set; }
        public string OFI { get; set; }
        public string None { get; set; }
        public string ActivityNameMaster { get; set; }
        public string FkAuditReportId { get; set; }

        public string Clause1 { get; set; }
        public string Clause2 { get; set; }
        public string TotalFindings { get; set; }

        public List<Internal_Audit_Report> lst1 { get; set; }

        public string EvidenceChecked { get; set; }
        public string RCA { get; set; }
        public string CA { get; set; }
        public string Correction { get; set; }
        public string Auditorremarks { get; set; }
        public string CostCenter { get; set; }

        public string OnSiteTime { get; set; }
        public string OffSiteTime { get; set; }
        public string TravelTime { get; set; }
        public string Test { get; set; }
        public int Id { get; set; }

    }

    public class Internal_Audit_Report1
    {
        public DateTime LDate_Of_Audit { get; set; }
        public DateTime LOnSiteTime { get; set; }
        public DateTime LTravelTime { get; set; }
    }

    //added by rajvi 3feb2025
    public class NDTQualification
    {
        public List<NDTQualification> ndtReport1 { get; set; }
        public List<NDTQualification> lstNdtCertificates { get; set; }
        public string NdtCertName { get; set; }
        public string NdtCertDate { get; set; }
        public string NdtValid { get; set; }
        public int Count { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmpCode { get; set; }
        public string Branch { get; set; }
        public string RollNo { get; set; }
        public string MT_Perc { get; set; }
        public string MT_Cert_Date { get; set; }
        public string PT_Perc { get; set; }
        public string PT_Cert_Date { get; set; }
        public string VT_Perc { get; set; }
        public string VT_Cert_Date { get; set; }
        public string UT_Perc { get; set; }
        public string UT_Cert_Date { get; set; }
        public string RT_Perc { get; set; }
        public string RT_Cert_Date { get; set; }
        public string Image { get; set; }
        public string MT_Pass { get; set; }
        public string PT_Pass { get; set; }
        public string VT_Pass { get; set; }
        public string RT_Pass { get; set; }
        public string UT_Pass { get; set; }
        //public string Valid Till { get; set; }
        //public string DOJ { get; set; }

    }

    public class GenerateCertificate
    {
        public List<GenerateCertificate> certreport { get; set; }
        public int Count { get; set; }


        public string Name { get; set; }
        public string EmpCode { get; set; }
        public string Branch { get; set; }
        public string DateofJoining { get; set; }
        public string MT { get; set; }
        public string PT { get; set; }
        public string VT { get; set; }
        public string UT { get; set; }
        public string RT { get; set; }
        public string EyeTest { get; set; }
        public string EyeTestCertCalid { get; set; }
        public string CertValid { get; set; }
        public string HAsCert { get; set; }
        public string CurrentDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        public string YesCert { get; set; }
        public string YesEye { get; set; }
        public string EyeTestCol { get; set; }
        public string MTCERT { get; set; }
        public string PTCERT { get; set; }
        public string VTCERT { get; set; }
        public string UTCERT { get; set; }
        public string RTCERT { get; set; }
        public string MTimg { get; set; }
        public string UTimg { get; set; }
        public string VTimg { get; set; }
        public string PTimg { get; set; }
        public string RTimg { get; set; }
        public string ID { get; set; }
        public string Method { get; set; }
        public string RollNo { get; set; }
        public string chkbox { get; set; }
        //public string Valid Till { get; set; }
        //public string DOJ { get; set; }

    }
}