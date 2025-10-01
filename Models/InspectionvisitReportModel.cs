using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class InspectionvisitReportModel
    {
        public List<InspectionvisitReportModel> lstVisitDateINOUTTime { get; set; }
        public List<InspectionvisitReportModel> lst1 { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<FileDetails> RefFileDetails { get; set; }
        public virtual ICollection<FileDetails> DetailsFileDetails { get; set; }
        public int PK_IVR_ID { get; set; }
        public int abcid { get; set; }
        public string Sap_And_Controle_No { get; set; }
        public string Branch { get; set; }
        public string Notification_Name_No_Date { get; set; }
        public string Date_Of_Inspection { get; set; }
        public string Project_Name_Location { get; set; }
        public string Address_Of_Inspection { get; set; }
        public string Client_Name { get; set; }
        public string End_user_Name { get; set; }
        public string DEC_PMC_EPC_Name { get; set; }
        public string DEC_PMC_EPC_Assignment_No { get; set; }
        public string Vendor_Name_Location { get; set; }
        public string Po_No { get; set; }
        public string Sub_Vendor_Name { get; set; }
        public string Po_No_SubVendor { get; set; }
        public string Date_of_PO { get; set; }
        public string SubVendorPODate { get; set; }
        public string FinalConfirmationVisible { get; set; }

        public string CMP_ID { get; set; }    //added by nikita on 22102024


        public string POItems { get; set; }
        public string PODescription { get; set; }

        public bool Kick_Off_Pre_Inspection { get; set; } //====================Convert To bool
        public bool Material_identification { get; set; } //====================Convert To bool
        public bool Interim_Stages { get; set; }   //====================Convert To bool
        public bool Document_review { get; set; }  //====================Convert To bool
        public bool Final_Inspection { get; set; }  //====================Convert To bool
        public bool Re_inspection { get; set; }  //====================Convert To bool
        public string Conclusion { get; set; }
        public string Pending_Activites { get; set; }
        public string Identification_Of_Inspected { get; set; }
        public string Areas_Of_Concerns { get; set; }
        public string Non_Conformities_raised { get; set; }
        public string Signatures { get; set; }
        public string Type { get; set; }
        public string CreatedBy { get; set; }
        public string GetDataAfterSave { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Status { get; set; }
        public string SubJob_No { get; set; }
        public string Report_No { get; set; }
        public bool Inspection_records { get; set; } //============Convert To Bool
        public bool Inspection_Photo { get; set; }  //============Convert To Bool
        public bool Other_Specify { get; set; }  //============Convert To Bool
        public Nullable<int> PK_Call_ID { get; set; }
        public Nullable<int> PK_SubJob_Id { get; set; }
        public string PK_UserID { get; set; }


        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string Onsite_hours { get; set; }
        public string OffSite_Hours { get; set; }
        public string Travel_Time { get; set; }
        public string Attachment { get; set; }
        public string OrderStatus { get; set; }
        public string Sub_Order_Status { get; set; }
        public string Call_No { get; set; }

        public string Name { get; set; }
        public string ReportCreatedDate { get; set; }

        public Nullable<int> Br_Id { get; set; }
        public string Branch_Name { get; set; }

        public string arrey { get; set; }
        public string ReportName { get; set; }
        public string SearchType { get; set; }
        public string ReviseReason { get; set; }

        public string IVRIRNAttachment { get; set; }
        public string Emails_Distribution { get; set; }


        public string InspectiobRecord_Remark { get; set; }
        public string OtherSpecifyRecords { get; set; }

        public string DownloadPrint { get; set; }

        public string Waivers { get; set; }

        public string client_Email { get; set; }
        public string Vendor_Email { get; set; }
        public string Tuv_Branch { get; set; }

        public bool client_EmailSend { get; set; }
        public bool Vendor_EmailSend { get; set; }
        public bool Tuv_BranchSend { get; set; }


        public string NCRNo { get; set; }

        public string PDF { get; set; }


        public string ReportNoName { get; set; }

        public string VisitReportAttachment { get; set; }

        public string Identification_Of_Inspected_AfterSave { get; set; }



        public bool MasterListOfcalibratedInstruments { get; set; }
        public string CanIRNbeissued { get; set; }
        public string IssuedPOItemNumbers { get; set; }


        public bool DTUVIndiaClientEndUser { get; set; }
        public bool DTUVIndiaExecuting_Originating_Branch { get; set; }
        public bool DVendor_Sub_Vendor { get; set; }
        public string TempInspectionPhotosNo { get; set; }
        public string TempMaster_List_Of_calibrated_Instruments { get; set; }


        public bool TUVIndiaClientEndUser { get; set; }
        public string TUVIndiaExecuting_Originating_Branch { get; set; }
        public string Vendor_Sub_Vendor { get; set; }


        public int defid { get; set; }
        public bool client { get; set; }
        public bool vendot { get; set; }
        public bool TUV { get; set; }

        public int UserBranch { get; set; }

        public string ReviseReportNoForPDF { get; set; }

        public string ReportNoForPDF { get; set; }
        public string SAPNo { get; set; }
        public string SubType { get; set; }
		 public bool chkARC { get; set; }

        public int IVRCount { get; set; }
        public bool ExpenseCheckBox { get; set; }
        public bool chkDoNotshareVendor { get; set; }

        public bool TiimeCheckBox { get; set; }

        public string Expenses { get; set; }

        public string PkId { get; set; }

        public string serviceid { get; set; }

        public Boolean POTotalCheckBox { get; set; }

        public List<InspectionvisitReportModel> lstConclusion { get; set; }

        //public int PO_QuantityTotal { get; set; }
        //public int Offered_QuantityTotal { get; set; }
        //public int Accepted_QuantityTotal { get; set; }
        //public int Cumulative_Accepted_QtyTotal { get; set; }


        public double PO_QuantityTotal { get; set; }
        public double Offered_QuantityTotal { get; set; }
        public double Accepted_QuantityTotal { get; set; }
        public double Cumulative_Accepted_QtyTotal { get; set; }

        public string Reason { get; set; }
        public List<InspectionvisitReportModel> Concern { get; set; }


        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public double TravelTime { get; set; }
        public double Description { get; set; }
        public List<NonInspectionActivity> Activity { get; set; }
			   public string ReopenBy { get; set; }
        public string NCRID { get; set; }

        public string SubSubVendorDate_of_PO { get; set; }
        public string ARCFirstPrint { get; set; }
        public string DownloadPDF { get; set; }

        
        public string ReasonName { get; set; }
        public int ReasonID { get; set; }
        public string ReasonIDs { get; set; }
        public string ProductList { get; set; }
        public string IsReviseReport { get; set; }
        public string UnitNameOnPDF { get; set; }
        public string ClosedBy { get; set; }
        public string SubOrderStatusForHide { get; set; }
        public string CallIDs { get; set; }
        public bool AreasOfConcern { get; set; }
        public string StrUserBranch { get; set; }
        public string DateSe { get; set; }

        public bool IsComfirmation { get; set; }  //====================Convert To bool


        public string acceptedStages { get; set; }
        public string acceptedItems { get; set; }
        public string NonacceptedStages { get; set; }
        public string NonacceptedItems { get; set; }
        public string ddlReviseReason { get; set; }
        
        public string strIsIssuePending { get; set; }
        public Boolean IsIssuePending { get; set; }
        public string Mitigateddate { get; set; }
        public string IsCustomerSpecificReportNumber { get; set; }
        public string CustomerSpecificReportNumber { get; set; }



        public string PDF_Type { get; set; }
        public string PDF_Description { get; set; }
        public string PDF_MitigatedBy { get; set; }
        public string PDF_MitigatedDate { get; set; }
        public string PDF_RaisedBy { get; set; }
        public string PDF_ReportNo { get; set; }
        public string PDF_PreviousComment { get; set; }
        public string PDF_IfConcernsDisplayOfPDF { get; set; }
        public string PDF_Status { get; set; }
        public string HeatNumber { get; set; }
        public string TotalQuantity { get; set; }

        public string ItemDescriptionDynamic { get; set; }
        public string ShowCount { get; set; }


        public string PO_QuantityTotal1 { get; set; }
        public string Offered_QuantityTotal1 { get; set; }
        public string Accepted_QuantityTotal1 { get; set; }
        public string Cumulative_Accepted_QtyTotal1 { get; set; }
        public string DisplayTotalQuantity { get; set; }

        public string SubSubSubVendorName { get; set; }
        public string SubSubSubPoNo { get; set; }
        public string SubSubSubPoDate { get; set; }
        //added by satish yadav for autosuggestion
        public string Productlst { get; set; }
        public bool IsAutoSuggestion { get; set; }

        //added by shrutika salve 09082024
        public string Category { get; set; }

        public string InOutTimeMandatory { get; set; }

        public string chkIfTravelExpenseOnPDF { get; set; }
        public string ChkCustomerSpcFormat { get; set; }
        public string chkIfEndUserEditable { get; set; }

        

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


        ///added by nikita 
        ///
        public string ContactNames_Client { get; set; }
        public string ContactCompanyName_Client { get; set; }
        public string Title_Client { get; set; }
        public string Email_Client { get; set; }

        public string HomePhone_Client { get; set; }
        public string Mobile_Client { get; set; }
        public string Fax_No_Client { get; set; }
        public bool IsMainContact_Client { get; set; }
        public string ContactStatus_Client { get; set; }
        public string Address_Client { get; set; }
        public string TitleName_Client { get; set; }
        public string Tuv_Email { get; set; }
        public string MailSubject { get; set; }
        public string IpAddress { get; set; }
        public string DownloadLocation { get; set; }
        public string ReportDownloadDate { get; set; }
        public string Id { get; set; }

    }
}