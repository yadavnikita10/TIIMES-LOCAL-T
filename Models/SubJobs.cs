using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace TuvVision.Models
{
    public class SubJobs
    {
        public List<SubJobs> lstSubJobDashBoard1 { get; set; }
        public List<SubJobs> ListDashboard { get; set; }
        public List<SubJobs> callListDashboard { get; set; }

        public int PK_SubJob_Id { get; set; }
        public string ItemsToBeInpsected { get; set; }
        public string Inspector { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string Call_No { get; set; }
        public Nullable<int> PK_QTID { get; set; }
        public Nullable<int> EQ_ID { get; set; }
        public Nullable<int> PK_JOB_ID { get; set; }
        public string Project_Name { get; set; }
        public string Company_Name { get; set; }
        public string Control_Number { get; set; }
        public string Service_type { get; set; }
        public string vendor_name { get; set; }
        public string Pvendor_name { get; set; }
        public string Vendor_Po_No { get; set; }
        public int PK_Call_ID { get; set; }
        //  public Nullable<System.DateTime> Po_Date { get; set; }
        public string SAP_No { get; set; }
        public string SubJob_No { get; set; }
        public string SubSubJob_No { get; set; }
        public string Status { get; set; }
        public string Client_Email { get; set; }
        public string Vendor_Email { get; set; }
        public string Tuv_Email { get; set; }
        public string Client_Contact { get; set; }
        public string Vendor_Contact { get; set; }
        public string Sub_Vendor_Contact { get; set; }
        public string VendorAddress { get; set; }

        public string Attachment { get; set; }
        public string Calls_List { get; set; }
        public string IRN_List { get; set; }
        public string Visit_Report_List { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string DeletStatus { get; set; }
        public string Type { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string V3 { get; set; }
        public string P3 { get; set; }
        //public string Date_of_Po { get; set; }
        public string Sub_Job { get; set; }

        public string Date_of_Po { get; set; }

        public string StringDate_of_Po { get; set; }

        public string Date_Of_PoDateTime { get; set; }
        public Nullable<decimal> VendorPO_Amount { get; set; }
        
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        public string Consume { get; set; }
        public string Remaining { get; set; }
        public string ProposedCall { get; set; }
        public string POMandays { get; set; }
        

        public bool chkArc { get; set; }
        public string End_User { get; set; }
        public string SubJobProjectName { get; set; }
        public string CostsheetApproval { get; set; }
        public string DECName { get; set; }
        public string DECNumber { get; set; }
        public bool checkIFCustomerSpecific { get; set; }
        public String TopPoNo { get; set; }

        public List<CallsModel> lstCallSummary { get; set; }

        public int pkID { get; set; }
        public bool checkIFExpeditingReport { get; set; }

        public string CustomerBlock { get; set; }


        public bool checkIFConcernDisplay { get; set; }
        public bool ItemDescriptionDynamic { get; set; }
        public bool chkDoNotshareVendor { get; set; }
        public bool checkIFCustomerSpecificReportNo { get; set; }

        
        //17/05
        public string SubSubSubJob_No { get; set; }
        public string SubSubVendorName { get; set; }
        public string SubSubVendorPO { get; set; }
        public string SubSubVendorDate { get; set; }
        public string SubSub_Vendor_Contact { get; set; }


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

        public string AddressType { get; set; }

        public string Address_Account { get; set; }

        public string SiteAddrPin { get; set; }

        //added by shrutika salve 18052024

        public string Vendor_ContactAll { get; set; }

        public string subVendor_ContactAll { get; set; }

        public string subvendorEmailid { get; set; }

        public string subsubvendorEmailid { get; set; }

        public string Rowid { get; set; }
        public List<string> Vendor_ContactDetails { get; set; }

        public string JobBlock { get; set; }
        public bool IVRAssignment { get; set; }


    }
}