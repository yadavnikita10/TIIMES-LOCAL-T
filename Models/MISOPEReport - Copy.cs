using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class MISOPEReport
    {
        public List<MISOPEReport> lst1 { get; set; }
        public int Count { get; set; }
        //public DateTime? FromDate { get; set; }

        //public DateTime? ToDate { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public string Date { get; set; }
        public string InspectorName { get; set; }
        public string Branch_Name { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeGrade { get; set; }
        public string ActivityType { get; set; }
        public string Job { get; set; }
        public string Sub_Job { get; set; }
        public string SAP_No { get; set; }
        public string Project_Name { get; set; }
        public string Job_Location { get; set; }
        public string Company_Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TravelTime { get; set; }
        public Double TotalTime { get; set; }
        public Double OPERate { get; set; }

        public Double Manday { get; set; }
        public string trans { get; set; }
        public string ODTime { get; set; }
        public string Id { get; set; }

    }

    public class MISJobWiseTimeSheet
    {
        public List<MISJobWiseTimeSheet> lstJWTS1 { get; set; }
        public int Count { get; set; }
        public string CreatedDateTime { get; set; }
        public string SurveyorName { get; set; }
        public string Call_No { get; set; }
        public string JOBNO { get; set; }
        public string Sub_Job { get; set; }
        public string Vendor_Name { get; set; }
        public string SAP_No { get; set; }
        public string Project_Name { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TravelTime { get; set; }
        public string TotalTime { get; set; }
        public string Originating_Branch { get; set; }
        public string Branch_Name { get; set; }
        public string Description { get; set; }
        public string TravelExpense { get; set; }
        public string VRNumber { get; set; }
        public string VisitReport { get; set; }
        public int OPEClaim { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string VRNo { get; set; }
        public string Attachment { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string ActType { get; set; }
    }

    public class MISCallRegister
    {
        public List<MISCallRegister> lst1 { get; set; }
        public int Count { get; set; }

        public string  CreatedDate { get; set; }
        public string  Call_No              { get; set; }
        public string  ClientName           { get; set; }
        public string  Project_Name         { get; set; }
        public string  Vendor_Name          { get; set; }
        public string  Sub_Job              { get; set; }
        public string  SAP_No               { get; set; }
        public string  Po_Number            { get; set; }
        public string  Po_No_SSJob          { get; set; }
        public string  Originating_Branch   { get; set; }
        public string  Executing_Branch     { get; set; }
        public string  InspectedItems       { get; set; }
        public string  Status               { get; set; }
        public string  Call_Recived_date    { get; set; }
        public string  Call_Request_date    { get; set; }
        public string  ActualVisitDate      { get; set; }
        public string Inspector             { get; set; }
        public string Job_Location          { get; set; }
        public string FromDate              { get; set; }
        public string ToDate                { get; set; }
        public string Product               { get; set; }
        public string Reason                { get; set; }
        public string V1                    { get; set; }
        public string V2                    { get; set; }
        public string P1                    { get; set; }
        public string P2                    { get; set; }
        public string Type                  { get; set; }

        public string PK_Call_ID { get; set; }
        public string Company_Name { get; set; }
        public string Contact_Name { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string Urgency { get; set; }
        public string Planned_Date { get; set; }
        public string ExtendCall_Status { get; set; }
        public string Call_Type { get; set; }
        public string CreatedBy { get; set; }
        public int delay { get; set; }
    }


    public class Projectstatus
    {
        public List<Projectstatus> lstProject { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Job_Location { get; set; }
        public string Job { get; set; }
        public string Description { get; set; }
        public string call_no { get; set; }
        public string Sub_Job { get; set; }
        public string Company_Name { get; set; }
        public string VendorName { get; set; }
        public string Sub_Vendor_Name { get; set; }
        public string SubVendorPO { get; set; }
        public string VendorPO { get; set; }
        public string NotificationNo { get; set; }
        public string Discipline { get; set; }
        public string ReqNo { get; set; }
        public string PropDateofInspFrom { get; set; }
        public string PropDateofInspTo { get; set; }
        public string ActualDateofInpFrom { get; set; }
        public string ActualDateofInpTo { get; set; }
        public string NoOfDays { get; set; }
        public string Delay { get; set; }
        public string Inspector { get; set; }
        public string VisitReport { get; set; }
        public string ReleaseNote { get; set; }
        public string TagNo { get; set; }
        public string AreaofConcern { get; set; }
        public string NC { get; set; }
        public string PendingAct { get; set; }
        public string IssuesPONumber { get; set; }
        public string CanIRNIssued { get; set; }
        public string callreceivedDate { get; set; }
        public string callType { get; set; }
        public string status { get; set; }
        public string branch { get; set; }
        public string OrderType { get; set; }
        public string OrgBranch { get; set; }
        


        public string AllPONumbers { get; set; }
        public string POSrNo { get; set; }
        public string POIRNItemCode { get; set; }
        public string ProdDescription { get; set; }

    }

    public class DebitCredit
    {
        public List<DebitCredit> lstDebitCredit { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Call_No { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string ExecutingBranch { get; set; }
        public string EmployeeCode { get; set; }
        public string SalesOrderNo { get; set; }
        public string Sub_Job { get; set; }
        public string OriginatingBranch { get; set; }
        public string OnSiteHours { get; set; }
        public string OffSiteHours { get; set; }
        public string TotalChargeble { get; set; }
        public string EmployeeCostcentre { get; set; }
        public string SOCostCenter { get; set; }
        public string Client_Name { get; set; }
        public string OnSiteHourRate { get; set; }
        public string AmountDRCR { get; set; }
        public string Description { get; set; }
        public string CreditBranch { get; set; }
        public string DebitBranch { get; set; }

    }

    public class TOYOMIS
    {
        public List<TOYOMIS> lstProject { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string client { get; set; }
        public string VendorName { get; set; }
        public string SubVendorName { get; set; }
        public string Requisition_No { get; set; }
        public string Inspection_Location { get; set; }
        public string Discipline { get; set; }
        public string PO_Quantity { get; set; }
        public string Ammend_Quantity { get; set; }
        public string Offered_Quanity { get; set; }
        public string Notification_Received_Date { get; set; }
        public string Proposed_Date_of_Inspection_From { get; set; }
        public string Proposed_Date_of_Inspection_To { get; set; }
        public string Actual_Visit_Date_of_Inspection_From { get; set; }
        public string Actual_Visit_Date_of_Inspection_To { get; set; }
        public string No_of_Days { get; set; }
        public string Delay_of_Inspection { get; set; }
        public string Surveyor_Name { get; set; }
        public string Inspection_Report { get; set; }
        public string Release_Note { get; set; }
        public string Final_IRN_Issued_Date { get; set; }
        public string Details_of_Area_of_Concern { get; set; }
        public string IRN_Quantity { get; set; }
        public string Pending_Activities { get; set; }
        public string Areas_of_Concers_if_Any { get; set; }
        public string Updated_Remark { get; set; }

        public string Job { get; set; }
        public string Project { get; set; }
        public string CanIRNbeIssued { get; set; }
        public string POSrNo { get; set; }
        public string POIRNItemCode { get; set; }
        public string InspectionActivity { get; set; }
        public string SubJob { get; set; }
        public string Report { get; set; }
        public string CreationDate { get; set; }

    }
}