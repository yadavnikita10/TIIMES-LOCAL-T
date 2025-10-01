using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace TuvVision.Models
{
    public class JobMasters
    {
        public List<JobMasters> lstCompanyDashBoard1 { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<FileDetails> FileDetailsFormat { get; set; }
        public List<JobMasters> lstQuotationMasterOrderType { get; set; }
        public List<JobMasters> lstQuotationMasterOrderTypeI { get; set; }
        public int Count { get; set; }
        public int PK_JOB_ID { get; set; }
        public string Job_Number { get; set; }
        public string Description { get; set; }
        public string Quotation_Of_Order { get; set; }
        public string Enquiry_Of_Order { get; set; }
        public string Client_Name { get; set; }
        public string Branch { get; set; }
        public string End_User { get; set; }
        public string subserviceType { get; set; }
        public string PortfolioType { get; set; }
        public string Service_type { get; set; }

        public string Consumed { get; set; }
        public string Estimate_ManHR { get; set; }
        public string PONo { get; set; }
        public string QMAmount { get; set; }
        public string EQEstAmt { get; set; }
        public string POAmount { get; set; }

        public string  OBSID { get; set; }
        public string  ServiceID { get; set; }
        public string PortID { get; set; }

        public string PortCode { get; set; }
        public string ServiceCode { get; set; }

        public string Job_type { get; set; }
        public string Customer_PoNo_PoDate { get; set; }
        public Nullable<decimal> Customer_PO_Amount { get; set; }
        public string Po_Validity { get; set; }
        public string Estimate_ManDays_ManMonth { get; set; }
        public string Estimate_ManHours { get; set; }
        public string Contract_reviewList { get; set; }
        public string Special_Notes { get; set; }
        public string formats_Of_Report { get; set; }
        public string Attachment { get; set; }
        public Nullable<int> Br_Id { get; set; }
        public Nullable<int> EQ_ID { get; set; }
        public Nullable<int> CMP_ID { get; set; }
        public Nullable<int> PK_QT_ID { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string Status { get; set; }

        public string V1 { get; set; }
        public string V2 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        

        public string GstDetails_BillingAddress { get; set; }
        public string SAP_No { get; set; }
        public string OrderStatus { get; set; }
        public string DECName { get; set; }
        public string DECNumber { get; set; }
        public string Currency { get; set; }

        public string Estimate_ManMonth { get; set; }
        public string Customer_PoDate { get; set; }



        public Nullable<decimal> FirstYear { get; set; }
        public Nullable<decimal> SecondYear { get; set; }
        public Nullable<decimal> ThirdYear { get; set; }
        public Nullable<decimal> FourthYear { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string OrderType { get; set; }
        public string OrderRate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }

        public string JobDate { get; set; }
        public Nullable<decimal> pendingAmount { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }

        public string Branch_Code { get; set; }
        public string JobNoUniqueId { get; set; }

        public int FK_CMP_ID { get; set; }

        public string GSTDetail { get; set; }



        public string SalesOrderNo { get; set; }
        public string CUSTSAPID { get; set; }
        public string MATID { get; set; }
        public string Qty { get; set; }
        public string NetAmount { get; set; }
        public string refno { get; set; }
        public string INVNO { get; set; }
        public string text3 { get; set; }
        public string rptNo { get; set; }
        public string PC { get; set; }
        public string InvDate { get; set; }
        public string SubProjectName { get; set; }
       

        public bool chkARC { get; set; }

        public int check { get; set; }

        public bool checkIFCustomerSpecific { get; set; }

        public string Exclusion { get; set; }
        public string Distance { get; set; }

        public bool POAwaited { get; set; }

        public string EstimatedAmount { get; set; }

        public string DReadOnly { get; set; }

        public string IReadOnly { get; set; }

        public bool RadioButton { get; set; }
        public bool IRadioButton { get; set; }

        public string OrderTypeId { get; set; }
        public string IOrderTypeId { get; set; }

        public string CostSheetApproved { get; set; }
        public string CostsheetApproval { get; set; }
        public string JobCreatedBy { get; set; }
        public string EnqRecDate { get; set; }
        public string QMCreatedBy { get; set; }
        public string QMCreatedDate { get; set; }
        public List<JobMasters> lst1 { get; set; }
        public List<JobMasters> lstTotalSum { get; set; }

        public List<JobMasters> lst1FinYear { get; set; }

        public string FYear { get; set; }
        public string strYear { get; set; }
        public int monthID { get; set; }
        public string strMonthName { get; set; }
        public string FinYearPrev1 { get; set; }
        public string FinYearPrev2 { get; set; }

        public string EstimatedMandays { get; set; }


        public string ProjectName { get; set; }

        public string ProposedMandays { get; set; }
        public string ConsumedMandays { get; set; }
        public string ConsumedInvoicing { get; set; }
        public string ProposedInvoicing { get; set; }

        public string ConsumedInvoicingSum { get; set; }
        public string ProposedInvoicingSum { get; set; }
        public string Customer_PO_AmountSum { get; set; }
        public string RemainingMandays { get; set; }
        public string br_ID1 { get; set; }

        #region Sub Job List
        //public List<SubJobs> lstSubJobDashBoard1 { get; set; }
        //public List<SubJobs> ListDashboard { get; set; }

        public List<JobMasters> lstSubJob { get; set; }
        public int PK_SubJob_Id { get; set; }
        public string ItemsToBeInpsected { get; set; }
        public string Inspector { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string Call_No { get; set; }
        public Nullable<int> PK_QTID { get; set; }
       
        public string Project_Name { get; set; }
        public string Company_Name { get; set; }
        public string Control_Number { get; set; }
      
        public string vendor_name { get; set; }
        public string Vendor_Po_No { get; set; }
        public int PK_Call_ID { get; set; }
       
       
        public string SubJob_No { get; set; }
        public string SubSubJob_No { get; set; }
     
        public string Client_Email { get; set; }
        public string Vendor_Email { get; set; }
        public string Tuv_Email { get; set; }
        public string Client_Contact { get; set; }
        public string Vendor_Contact { get; set; }
        public string Sub_Vendor_Contact { get; set; }
       
        public string Calls_List { get; set; }
        public string IRN_List { get; set; }
        public string Visit_Report_List { get; set; }
       
        public string DeletStatus { get; set; }
        public string Type { get; set; }

        //public string Date_of_Po { get; set; }
        public string Sub_Job { get; set; }

        public string Date_of_Po { get; set; }

        public string StringDate_of_Po { get; set; }
        #endregion



        #region Changes for order type
        public string DEstimatedAmount { get; set; }
        public string Dcurrency { get; set; }
        public string DExchangeRate { get; set; }
        public string DTotalAmount { get; set; }
        public string IEstimatedAmount { get; set; }
        public string Icurrency { get; set; }
        public string IExchangeRate { get; set; }
        public string ITotalAmount { get; set; }
        public string IOrderType { get; set; }
        public string IOrderRate { get; set; }
        public string IEstimate_ManDays_ManMonth { get; set; }
        public string IDistance { get; set; }
        

        public string IEstimate_ManMonth { get; set; }
        #endregion


        public string Consume { get; set; }
        public string Remaining { get; set; }
        public string ProposedCall { get; set; }
        public string POMandays { get; set; }

        public string InspectionLocation { get; set; }
        public string chkARCType { get; set; }
        public List<CallsModel> lstCallSummary { get; set; }

        /*******Additional PO Details ************/

        public int PK_ADDPOID { get; set; }
        public Nullable<decimal> Add_POAmt { get; set; }
        public string Add_PoValidity { get; set; }
        public string Add_PoNumber { get; set; }
        public string Add_MandayDesc { get; set; }
        public string Add_PoDate { get; set; }
        public string Add_Mandays { get; set; }        
        public bool Add_PoAmd { get; set; }
        public string Add_PoReason { get; set; }
        public List<JobMasters> lstAddMandays { get; set; }
        public string Remark { get; set; }
        public string ARC { get; set; }
        public string SAPItem_No { get; set; }


        public List<JobMasters> LCheckList { get; set; }
        public string CheckListId { get; set; }
        public string CheckListName { get; set; }
        public Boolean chkbox { get; set; }
        public string CheckListDescription { get; set; }
        public string CommaSeparatedCheckListId { get; set; }
        public string CheckListDescriptionN { get; set; }
        /******************************************/
        public string strPOAwaited { get; set; }

        [DataType(DataType.Text)]
        //  [Required(ErrorMessage = "Please Select ")]
        public string ContactNames { get; set; }
        public string ContactCompanyName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public string Fax_No { get; set; }
        public bool IsMainContact { get; set; }
        public string ContactStatus { get; set; }
        public string Address { get; set; }
        public string TitleName { get; set; }

        public string PoAmmendment { get; set; }
        public bool checkIFCustomerSpecificReportNo { get; set; }
        //#region ContactDetails
        ////**********************************************************Contact Details**********************************************

        //public int PK_ContID { get; set; }

        //public string Email { get; set; }
        //public string Mobile { get; set; }
        //public string Fax_No { get; set; }
        //public string ContactStatus { get; set; }
        //public string Address { get; set; }

        ////[DataType(DataType.PhoneNumber)]
        ////[Display(Name = "CONTACT_NUMBER ")]
        ////[Required(ErrorMessage = "CONTACT_NUMBER Required!")]
        ////[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered CONTACT_NUMBER format is not valid.")]
        //public string HomePhone { get; set; }
        //public bool IsMainContact { get; set; }
        //public string TitleName { get; set; }

        //[DataType(DataType.Text)]
        ////  [Required(ErrorMessage = "Please Select ")]
        //public string ContactNames { get; set; }
        //public string ContactCompanyName { get; set; }

        //public static List<CompanyMaster> lstUser { get; set; }
        //public string RegretStatus { get; set; }
        //public string RegretReason { get; set; }
        //public DateTime? Opendate { get; set; }

        //public string Owner { get; set; }

        //public string DashboardType { get; set; }



        //public bool chkArc { get; set; }


        //public string AutoOrderRate { get; set; }

        //public string RefDate { get; set; }


        //public int RegretId { get; set; }
        //public int RegretActionTakenId { get; set; }
        //public String RegretActionTaken { get; set; }

        //public string ContactNo { get; set; }

        //public string ModifiedDateS { get; set; }
        //public string OpendateS { get; set; }
        //public string EstCloseS { get; set; }
        //public string TEstimatedAmount { get; set; }
        //public string Domesticlocation { get; set; }
        //public string Intlocation { get; set; }


        //public string LeadGivenBy { get; set; }
        //public string NotesbyLeads { get; set; }
        //public string PkUserID { get; set; }
        //public string RegretReasonDescription { get; set; }


        //public string DRemark { get; set; }
        //public string IRemark { get; set; }
        //public string QuoGenerated { get; set; }
        //public string Quotation { get; set; }
        //public string JobNumber { get; set; }

        public bool checkIFConcernDisplay { get; set; }
        public bool ItemDescriptionDynamic { get; set; }
        public bool checkIFExpeditingReport { get; set; } //#endregion
    }
}
 
 
 
 
 
 