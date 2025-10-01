using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TuvVision.Models
{
    public class QuotationMaster
    {
        public List<QuotationMaster> lstQuotationMasterDashBoard1 { get; set; }
        public List<QuotationMaster> JobDashBoard { get; set; }
        public List<QuotationMaster> lstQuotationMasterOrderType { get; set; }
        //Code Added by Manoj For Multiple File Uploaded
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public int Count { get; set; }
        public int PK_QTID { get; set; }
        public string QuotationNumber { get; set; }
        public int PK_CSID { get; set; }
        public int EQ_ID { get; set; }
        public int PK_ContID { get; set; }
        public int CMP_ID { get; set; }
        public int BR_ID { get; set; }
        public int ServiceType { get; set; }
        public int ProjectType { get; set; }

        public string PortfolioType { get; set; }


        public string SubServiceType { get; set; }

        public string Enquiry { get; set; }
        public string Quotation_Description { get; set; }
        public string EndCustomer { get; set; }
        public string Reference { get; set; }
        public string ExpiryDate { get; set; }
        public Int32 Status { get; set; }
        public string Statusname { get; set; }
        public string GST { get; set; }
        

        
        public string Remark { get; set; }
        public string Attachment { get; set; }
        public string JobList { get; set; }
        public string HeaderDetails { get; set; }
        public string Subject { get; set; }
        public string ScopeOfWork { get; set; }
        public string Deliverable { get; set; }
        public string Commercials { get; set; }
        public string FeesStructure { get; set; }
        public string PaymentTerms { get; set; }
        public string KeyNotes { get; set; }
        public string AddEnclosures { get; set; }
        public string ApprovalStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string QuotationCompanyName { get; set; }
        public int QuotationBranch { get; set; }
        public int QuotationType { get; set; }
        public string EnquiryNumber { get; set; }
        public string StatusType { get; set; }
        public string CommunicationProtocol { get; set; }
        public string Coordinators { get; set; }
        public string EscalationMatrix { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Associates { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string EmailDate { get; set; }
        public string DearSir { get; set; }
        public string BranchName { get; set; }
        public string ServType { get; set; }
        public string ProjectName { get; set; }
        public string Revise { get; set; }
        public string QTType { get; set; }
        public string FaitFully { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }

        public string EstimatedAmount { get; set; }
        public int PKJobId { get; set; }
        public string JobNo { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string Job_type { get; set; }

        public int FK_CMP_ID { get; set; }

        public string ddlQuotationNumber { get; set; }

        public string Enquiry_Description { get; set; }

        public string CompanyAddress { get; set; }

        public string Validity { get; set; }

        public string FromAddress { get; set; }

        public string Ref { get; set; }

        public string ThirdPartyInspectionService { get; set; }
        public string ThankYouLetter { get; set; }

        public string QuotationPDF { get; set; }


        public string Designation { get; set; }

        public string CostSheetApproveStatus { get; set; } // Domestic Costsheet Status
        
        public string ICostSheetApproveStatus { get; set; } // International Costsheet Status

        public string CostSheetCLStatus { get; set; }
        public string ICostSheetCLStatus { get; set; }


        public string Signature { get; set; }

        public string DashboardType { get; set; }

        public string InspectionLocation { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public int DomStatus { get; set; }
        public int IntStatus { get; set; }

        public string DEstimatedAmount { get; set; }
        public string Dcurrency { get; set; }
        public string DExchangeRate { get; set; }
        public string DTotalAmount { get; set; }
        public string IEstimatedAmount { get; set; }
        public string Icurrency { get; set; }
        public string IExchangeRate { get; set; }
        public string ITotalAmount { get; set; }

        public string OrderRate { get; set; }
        public string Estimate_ManMonth { get; set; }
        public string Estimate_ManDays_ManMonth { get; set; }
        public string OrderType { get; set; }

        public string IOrderType { get; set; }
        public string IOrderRate { get; set; }
        public string IEstimate_ManDays_ManMonth { get; set; }
        public string IDistance { get; set; }
        public string Type { get; set; }
        public string IEstimate_ManMonth { get; set; }

        public bool chkArc { get; set; }

        public int check { get; set; }

        public string Exclusion { get; set; }
        public string Distance { get; set; }

        public string LostReason { get; set; }


        public bool ExclusionCheckBox { get; set; }

        public string SendForApprovel { get; set; }

        public string JobCreationType { get; set; }

        public string IGST { get; set; }
        public string CSSentforApproval { get; set; }
        public string ICSSentforApproval { get; set; }
        


    }

    public class DCurrency
    {
        
        public string OrderType { get; set; }

        public string OrderRate { get; set; }

        public string Estimate_ManDays { get; set; }

        public string Estimate_ManMonth { get; set; }

        public string Distance { get; set; }

        public string EstimatedAmount { get; set; }

        public string Currency { get; set; }

        public string ExchangeRate { get; set; }

        public string DTotalAmount { get; set; }
        
    }

    public class ICurrency
    {

        public string IOrderType { get; set; }

        public string IOrderRate { get; set; }

        public string IEstimate_ManDays { get; set; }

        public string IEstimate_ManMonth { get; set; }

        public string IDistance { get; set; }

        public string IEstimatedAmount { get; set; }

        public string InternationalCurrency { get; set; }

        public string IExchangeRate { get; set; }

        public string ITotalAmount { get; set; }

    }



}