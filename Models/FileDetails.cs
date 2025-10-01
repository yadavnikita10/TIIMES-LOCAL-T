using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class FileDetails
    {
        public Guid Id { get; set; }
        public string IDS { get; set; }
        public string AttachmentType { get; set; } //added by nikita on 25062024
        public int PK_ID { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int EQ_ID { get; set; }
        public int PK_QTID { get; set; }
        public int Br_Id { get; set; }
        public int PK_JOB_ID { get; set; }
        public int PK_Call_ID { get; set; }
        public int NcrId { get; set; }
        public int PK_Complaint_ID { get; set; }
        public int AuditId { get; set; }
        public int PK_SubJob_Id { get; set; }
        public int PK_VCall_ID { get; set; }
        public int PK_IVR_ID { get; set; }
        public virtual InspectionvisitReportModel IRN { get; set; }
        public virtual Calls VirtualCalls { get; set; }
        public virtual SubJobs SubJobs { get; set; }
        public virtual Internal_Audit_Report Internal_Audit_Report { get; set; }
        public virtual ComplaitRegister ComplaitRegister  { get; set; }
        public virtual NCR NCR { get; set; }
        public virtual EnquiryMaster EnquiryMaster { get; set; }
        public virtual QuotationMaster QuotationMaster { get; set; }
        public virtual BranchMaster BranchMaster { get; set; }
        public virtual JobMasters JobMaster { get; set; }
        public virtual CallsModel CallsModel { get; set; }
        public byte[] FileContent { get; set; }
        public int FK_RM_ID { get; set; }

        public string FileSize { get; set; }
    }
}