using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;

namespace TuvVision.Models
{
    public class EnquiryMaster
    {
        public List<EnquiryMaster> lst1 { get; set; }
        public int Count { get; set; }
        public string DisplayCompanyName { get; set; }

        public string ContactStatus { get; set; }
        public List<EnquiryMaster> lstEnquiryMast { get; set; }
        public List<QuotationMaster> lstQuotationMasterOrderType { get; set; }

        public List<NameCode> lstBranch { get; set; }
        public List<NameCode> lstServiceType { get; set; }
        public List<NameCode> lstProjectType { get; set; }

        public List<NameCode> lstOBSType { get; set; }
        public List<NameCode> lstServicePortType { get; set; }
        public List<NameCode> lstSubServiceType { get; set; }


        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<FileDetails> FileDetails_ { get; set; }  //added by nikita on 25062024
        public virtual ICollection<FileDetails> FileDetailsformats { get; set; }  //added by nikita on 25062024


        public int EQ_ID { get; set; }
        public string EnquiryNumber { get; set; }
        public string Date_ { get; set; }

        public string EmployeeName { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Select ")]

        [Required]
        public string EnquiryDescription { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Company Name ")]
        public string CompanyName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter End Customer Name ")]
        public string EndCustomer { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? EstClose { get; set; }
        public String EstClose { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Enquiry Reference No")]
        public string EnquiryReferenceNo { get; set; }

        [Required(ErrorMessage = "Please Enter Estimated Amount")]
        public decimal EstimatedAmount { get; set; }
        public string Notes { get; set; }
        public string AccountManager { get; set; }
        public string Branch { get; set; }
        [DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Select Source")]
        public string Source { get; set; }
        public string Type { get; set; }
        /*
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Business Type")]
        public string Type { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ProjectType")]
        public string ProjectType { get; set; }
        */
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select OBS Type")]
        public string ProjectType { get; set; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Service Portfolio")]
        public string PortfolioType { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Service Type")]
        public string SubServiceType { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Status { get; set; }


        [DataType(DataType.Text)]
        //  [Required(ErrorMessage = "Please Select Contact ")]
        public string ContactName { get; set; }
        public string[] Citys { get; set; }
        public string[] Countrys { get; set; }

        public string DocumentAttached { get; set; }
        public string Ped { get; set; }

        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }

        //***************************************************Company Master Details**************************************************
        public int CMP_ID { get; set; }
        public string Contact { get; set; }
        public string Company_Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Work_Phone { get; set; }
        public string CompanyType { get; set; }
        public string Sub_Type { get; set; }
        public string Title { get; set; }
        public string Home_Page { get; set; }
        public string Pan_No { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Fax_No { get; set; }
        public string Address_Account { get; set; }
        public string Main { get; set; }
        public int Ind_ID { get; set; }
        public string Fax_Account { get; set; }
        public string Branch_Description { get; set; }
        public int Br_Id { get; set; }
        public string CompanyStatus { get; set; }
        public int CG_ID { get; set; }
        public string DeleteStatus { get; set; }
        public string CompanyNames { get; set; }
        public string InspectionLocation { get; set; }
        public int SubType { get; set; }
        public List<NameCode> lstPedType { get; set; }
        public int OtherType { get; set; }
        public List<NameCode> lstOtherType { get; set; }
        public int EnergyType { get; set; }
        public List<NameCode> lstEnergyType { get; set; }
        public string City { get; set; }
        public List<NameCode> lstCity { get; set; }
        public string Country { get; set; }
        public List<NameCode> lstCountry { get; set; }
        //**********************************************************Contact Details**********************************************

        public int PK_ContID { get; set; }
        //[DataType(DataType.PhoneNumber)]
        //[Display(Name = "CONTACT_NUMBER ")]
        //[Required(ErrorMessage = "CONTACT_NUMBER Required!")]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered CONTACT_NUMBER format is not valid.")]
        public string HomePhone { get; set; }
        public bool IsMainContact { get; set; }
        public string TitleName { get; set; }

        [DataType(DataType.Text)]
        //  [Required(ErrorMessage = "Please Select ")]
        public string ContactNames { get; set; }
        public string ContactCompanyName { get; set; }
        public string FK_CMP_ID { get; set; }
        public static List<CompanyMaster> lstUser { get; set; }
        public string RegretStatus { get; set; }
        public string RegretReason { get; set; }
        public DateTime? Opendate { get; set; }

        public string Owner { get; set; }

        public string DashboardType { get; set; }

        public string DEstimatedAmount { get; set; }
        public string Dcurrency { get; set; }
        public string DExchangeRate { get; set; }
        public string DTotalAmount { get; set; }

        public string IEstimatedAmount { get; set; }
        public string Icurrency { get; set; }
        public string IExchangeRate { get; set; }
        public string ITotalAmount { get; set; }
        public string CompanyAddress { get; set; }


        public string OrderRate { get; set; }
        public string Estimate_ManMonth { get; set; }
        public string Estimate_ManDays_ManMonth { get; set; }
        public string OrderType { get; set; }

        public string IOrderType { get; set; }
        public string IOrderRate { get; set; }
        public string IEstimate_ManDays_ManMonth { get; set; }
        public string IDistance { get; set; }
        public string Distance { get; set; }
        public string IEstimate_ManMonth { get; set; }

        public bool chkArc { get; set; }
        //added by nikita on 11062024

        public string Legalcomment { get; set; }
        public bool Quotationviewed { get; set; }
        public bool LegalReview { get; set; }
        public bool Budgetary { get; set; }
        public string LegalAttachment { get; set; }

        public string AutoOrderRate { get; set; }

        public string RefDate { get; set; }
        public string Remark { get; set; }

        public int RegretId { get; set; }
        public int RegretActionTakenId { get; set; }
        public String RegretActionTaken { get; set; }

        public string ContactNo { get; set; }
        public string ARC { get; set; }
        public string ModifiedDateS { get; set; }
        public string OpendateS { get; set; }
        public string EstCloseS { get; set; }
        public string TEstimatedAmount { get; set; }
        public string Domesticlocation { get; set; }
        public string Intlocation { get; set; }
        public string ProjectName { get; set; }

        public string LeadGivenBy { get; set; }
        public string NotesbyLeads { get; set; }
        public string PkUserID { get; set; }
        public string RegretReasonDescription { get; set; }

        public List<EnquiryMaster> LCheckList { get; set; }
        public string CheckListId { get; set; }
        public string CheckListName { get; set; }
        public Boolean chkbox { get; set; }

        public string CheckListDescription { get; set; }
        public string DRemark { get; set; }
        public string IRemark { get; set; }
        public string QuoGenerated { get; set; }
        public string Quotation { get; set; }
        public string JobNumber { get; set; }
        public string QCreatedDate { get; set; }

        //Vaibhav---21 June
        public List<EnquiryMaster> lstConflictJobs { get; set; }
        public List<EnquiryMaster> lstConflictJobsApproval { get; set; }
        public string Reason { get; set; }
        public string PCHApproval { get; set; }
        public string CHApproval { get; set; }
        public string QAApproval { get; set; }
        public string SessionId { get; set; }
        public string PCHID { get; set; }
        public string CHID { get; set; }
        public string BranchQAID { get; set; }
        public string ConflictType { get; set; }
        public bool ConflictConfirmation { get; set; }
        public string ConflictSituation { get; set; }
        public string GetDuplicateEnquiry { get; set; }
        public int PK_Job_Id { get; set; }
        public string AllA { get; set; }
        //Vaibhav---21 June End
        public string ConflictApprovalName { get; set; }
        public string ApprovedNotApproved { get; set; }
        public string PCHName { get; set; }
        public string CHName { get; set; }
        public string ConflictEnquiryCreatedbyEmail { get; set; }
        public string OffAddrPin { get; set; }
        public string CompanyCode { get; set; }
        public string CustomerSearch { get; set; }
        public string GST_NO { get; set; }
        public int New_Cmp { get; set; }

        public string State { get; set; }

        public string VATRegistrationNo { get; set; }
        public string oldcustomername { get; set; }

        public string JobCount { get; set; }

        public string QuotationHide { get; set; }
        public string CostSheetA { get; set; }

        public string ENQDuplicate { get; set; }

        public string DeleteReason { get; set; }
        public string BudgetoryText { get; set; }

        public string BudgetaryPk_QTid { get; set; }
        public string CityName { get; set; }
    }
    public class NameCode
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }
        public int pk_cc_id { get; set; }
        public string CCid { get; set; }
    }

    public class UNameCode
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public SelectList SectionModel { get; set; }
    }
    //Added By Satish Pawar On 11 May 2023
    public class EmailCode
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }
    }

    public class NameCode1
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public SelectList SectionModel { get; set; }
    }

}