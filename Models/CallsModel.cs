using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CallsModel
    {
        public List<CallsModel> lstInspectorSchedule { get; set; }
        public string InspectorAddress { get; set; }

        public virtual ICollection<FileDetails> FileDetailsFormat1 { get; set; }
        public virtual ICollection<FileDetails> SubJobFileDetails { get; set; }
        public virtual ICollection<FileDetails> SubSubJobFileDetails { get; set; }
        public virtual ICollection<FileDetails> SubSubSubJobFileDetails { get; set; }
        public virtual ICollection<FileDetails> CallFileDetails { get; set; }

        public string DSubJob_No { get; set; }
        public int DPK_SubJob_Id { get; set; }
        
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }

        /*public string Call_Recived_date { get; set; }
        public string Call_Request_Date { get; set; }
        public string Planned_Date { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }*/
        public int OrgID { get; set; }
        public int EXEBRId { get; set; }
        
        public string Call_Recived_date { get; set; }
        public string Call_Request_Date { get; set; }
        public string Planned_Date { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string FromDateNew { get; set; }
        public string ToDateNew { get; set; }



        public List<CallsModel> lst1 { get; set; }
        public int Count { get; set; }
        public List<CallsModel> CADashboard { get; set; }
        public int CACount { get; set; }
        public List<CallsModel> ListDashboard { get; set; }
        public List<CallsModel> lstCallsModel1 { get; set; }
        public int PK_Call_ID { get; set; }
        public string Company_Name { get; set; }
        public string LastInspectorName_DateOfInspection { get; set; }
        public string Status { get; set; }
        public string Originating_Branch { get; set; }
        public string Coordinatorname_ { get; set; }    //added by nikita on 12122023
        public string CoordinatorMobileNo_ { get; set; } //added by nikita on 12122023
        public string ExecutingService { get; set; }
        public string CoordinatorEmail_ { get; set; }
        public string Type { get; set; }
        public string Contact_Name { get; set; }
        public string MainBranch { get; set; }

        public string JobType { get; set; }
        public string ServiceType { get; set; }
        public string OBSId { get; set; }


        public string Excuting_Branch { get; set; }
        public string Sub_Type { get; set; }
        public string Job { get; set; }
        

        public string Source { get; set; }
        public string Vendor_Name { get; set; }
        public string Sub_Job { get; set; }
        public string FirstSubJob { get; set; }

        public string Urgency { get; set; }
        public string Special_Notes { get; set; }
        
        public string Competency_Check { get; set; }
        public string End_Customer { get; set; }
        public string Category { get; set; }
        public string Sub_Category { get; set; }
        public string Empartiality_Check { get; set; }
        public string Project_Name { get; set; }
        public string Job_Location { get; set; }
        public string Final_Inspection { get; set; }
        public string isWeek_Month_Call { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public string Inspector { get; set; }
        public string Client_Email { get; set; }
        public string Vendor_Email { get; set; }
        public string Tuv_Branch { get; set; }
        public string Executing_Branch { get; set; }
        public string EB_Email { get; set; }
        public Nullable<int> PK_SubJob_Id { get; set; }
        public Nullable<int> PK_JOB_ID { get; set; }
        public Nullable<int> PK_QT_ID { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string DeleteStatus { get; set; }
        public string Call_No { get; set; }
        
        public string From_Time { get; set; }
        public string To_Time { get; set; }
        public string Call_Type { get; set; }
        public string SubSubJobNo { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string AI { get; set; }
        public bool chkDoNotshareVendor { get; set; }

        public string Branch_Name { get; set; }
        public int Br_Id { get; set; }
        public int ExBr_Id { get; set; }
        public bool ClientEmailcheckbox { get; set; }
        public bool Vendorcheckbox { get; set; }
        public bool Homecheckbox { get; set; }
        public string FirstName { get; set; }


        public string UserBranchName { get; set; }
        public string AssignStatus { get; set; }
        public string Attachment { get; set; }

        public string Client_Contact { get; set; }
        public string Vendor_Contact { get; set; }
        public string Sub_Vendor_Contact { get; set; }
        public string Sub_Sub__Vendor_Contact { get; set; }
        public string PO_Number { get; set; }
        public List<abc> CallListData { get; set; }
        public string Reasion { get; set; }

        public string SAP_Number { get; set; }
        public string Sub_Job_Number { get; set; }
        public string Inspection_CallIntimationReceivedOn { get; set; }
        public string RequstedInspectedDate { get; set; }

        public string  Inspector_To_Be_Assign { get; set; }
        public string  PONumberOnVender { get; set; }
        public string  OriginatingBranch { get; set; }
        public string  StageOfInspection { get; set; }
        public string  InspectionLocation{ get; set; }
        public string  ItemsToBeInpsected { get; set; }
        public string Unassign_Inspector { get; set; }

        public string PONumberOnSubVender { get; set; }

        public string ApplyDate { get; set; }
        public string formats_Of_Report { get; set; }
        public string Po_No_SSJob { get; set; }

        public string Report_No { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Select ")]
        public string ProductList { get; set; }
        public DateTime? ActualReportDealydate { get; set; }
        public DateTime? ReportDate { get; set; }

        //public DateTime? FromDateNew { get; set; }
        //public DateTime? ToDateNew { get; set; }

        


        
        public string ExtendCall_Status { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string MailSend_ConsumedMandays { get; set; }
        public int DelayInINRSubmission { get; set; }

        
        public string Product_Name { get; set; }

        public bool ChkMultipleSubJobNo { get; set; }
        public bool ChkContinuousCall { get; set; }
        public bool chkExcludeSundays { get; set; }


        public string IsVisitReportGenerated { get; set; }

        public string TopSubVendorName { get; set; }

        public string TopSubVendorPONo { get; set; }

        public string VendorName { get; set; }
        public string SubVendorName { get; set; }
        public string VendorPONo { get; set; }
        public string SubVendorPONo { get; set; }
       
        public string ExecutingBranch { get; set; }

        public string POAmountGreaterThan { get; set; }

        public List<Test> CallTest { get; set; }

        public bool CompetencyCheck { get; set; }

        public bool ImpartialityCheck { get; set; }

        public bool FinalInspection { get; set; }

       // [Range(1, 14, ErrorMessage = "Value must be between 1 to 14")]
     ///   public int EstimatedHours { get; set; }
        public string EstimatedHours { get; set; }
        public string ClientAddress { get; set; }
        public string VendorAddress { get; set; }
        public bool chkCallCancelled { get; set; }
        public bool checkIFCustomerSpecific { get; set; }

        public string TopvendorPODate { get; set; }
        public string SubVendorPODate { get; set; }
        public string DECNumber { get; set; }
        public string DECName { get; set; }

        public string CoordinatorName { get; set; }
        public string CoordinatorContactDetail { get; set; }
        public string SAP_no { get; set; }
        public bool chkARC { get; set; }

        public string OpenCalls { get; set; }
        public string ClosedCalls { get; set; }
        public string AssignedCalls { get; set; }
        public string NotDoneCalls { get; set; }
        public string CancelledCalls { get; set; }
        public string TotalCalls { get; set; }

        public bool ChkMultipleJobNo { get; set; }
        public string DJob_No { get; set; }
        public int DPK_Job_Id { get; set; }
        public string JobList { get; set; }
        public string MandaysConsumedValidity { get; set; }
        public int Mailsend_consumedMandays { get; set; }
        public string CurrentAssignment { get; set; }
        //public string CallReceiveTime { get; set; }
        public string statusReason { get; set; }
        public string NewPlannedDate { get; set; }
        public string ActionSelected { get; set; }
        public List<CallsModel> lstAddCallDetails { get; set; }

        public int PK_AddCallID { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public string DeleteVisible { get; set; }


        //added by shrutika salve on 12/06/2023
        //public string CALLRECEIVETIME { get; set; }

        public string EXECUTIONDELAYDAY { get; set; }

        public string IVRDistributionDelay { get; set; }
        public string IRNDistributionDelay { get; set; }
        public string CallCancelledBy { get; set; }
        public List<CallsModel> lstDashBoard { get; set; }
        public List<CallsModel> lstDashBoard1 { get; set; }

        public string CallReceiveTime { get; set; }
       // public string CallReceiveTime { get; set; }
        public string IVRCreateDate { get; set; }

        public string IRNCreateDate { get; set; }

        public string IVRModifiedDate { get; set; }
        public string IRNModifiedDate { get; set; }
        public string IVR { get; set; }
        public string IRN { get; set; }
        public string PresentDay { get; set; }

        public string CallId { get; set; }

        public string IVRDownloadDateTime { get; set; }
        public string IVRDownloadModifiedDate { get; set; }
        public string IVRDistrubutionDateTime { get; set; }
        public string IRNDownloadDatetime { get; set; }
        public string IRNDownloadModifiedDate { get; set; }
        public string IRNDistrubutionDateTime { get; set; }
        public string CallClosureDate { get; set; }

        public string IVRReportNo { get; set; }
        public string IRNReportNo { get; set; }
        //added by shrutika salve 03/07/2023
        public int pk_ivr_id { get; set; }
        public int Iv_pk_call_id { get; set; }
        public string EXECUTISTATUS { get; set; }
        public string InspectorMobile { get; set; }


        public string checkIFCustomerSpecific1 { get; set; }
        public string chkARC1 { get; set; }
        public string CreatedIVrInTiimes { get; set; }
        public string IVRConclusion { get; set; }
        public string IRNConclusion { get; set; }
        public string DocumentRelatedToInspectionCall { get; set; }

        public List<CallsModel> lstComplaintDashBoard1 { get; set; }


        //added by shrutika salve 17/07/2023

        public string Onsite { get; set; }
        public string Offsite { get; set; }
        public string TravelTime { get; set; }
        public string insopectionRecord { get; set; }

        public string FromDateI { get; set; }
        public string ToDateI { get; set; }
        public string inspectorCompetant { get; set; }
        public string MandayRate { get; set; }
        public string RefDocument { get; set; }

        public string po_Date { get; set; }
        public bool checkIFExpeditingReport { get; set; }
        public string ExpeditingType { get; set; }

        //added By shrutika salve 27/07/2023

        public string Ontime { get; set; }
        public string OnTimePercentage { get; set; }
        public string DelayCount { get; set; }
        public string DelayCountPercentage { get; set; }
        public string NoactionbyCoordinators { get; set; }
        public string NoactionbyCoordinatorsPercentage { get; set; }
        public string NoactionbyInspectors { get; set; }
        public string NoactionbyInspectorsPercentage { get; set; }
        public string WrongReceiveDateCountPercentage { get; set; }
        public string WrongRequestDateCountPercentage { get; set; }
        public string Performance { get; set; }
        public string PerformanceCountPercentage { get; set; }

        //added by shrutika salve 28072023

        public int GrandTotalTotalCalls { get; set; }
        public int GrandTotalOntime { get; set; }
        public float GrandTotalOnTimePercentage { get; set; }
        public int GrandTotalDelayCount { get; set; }
        public float GrandTotalDelayCountPercentage { get; set; }
        public int GrandTotalNoactionbyCoordinators { get; set; }
        public float GrandTotalNoactionbyCoordinatorsPercentage { get; set; }
        public int GrandTotalNoactionbyInspectors { get; set; }
        public float GrandTotalNoactionbyInspectorsPercentage { get; set; }
        public int GrandTotalWrongReceiveDateCount { get; set; }
        public float GrandTotalWrongReceiveDateCountPercentage { get; set; }
        public int GrandTotalWrongRequestDateCount { get; set; }
        public float GrandTotalPerformance { get; set; }
        public float GrandTotalWrongRequestDateCountPercentage { get; set; }
        public float GrandTotalPerformancePercentage { get; set; }
        public CallsModel GrandTotal { get; set; }

        //added by shrutika salve 12/09/2023
        public string competantinspectorassigned { get; set; }
        public string competantinspectorassignedPercentage { get; set; }
        public string competantinspectorNotassigned { get; set; }
        public string competantinspectorNotassignedPercentage { get; set; }


        //added by shrutika salve 12092023

        public int GrandTotalcompetantinspectorassigned { get; set; }
        public float GrandTotalcompetantinspectorassignedPercentage { get; set; }
        public int GrandTotalcompetantinspectorNotassigned { get; set; }
        public float GrandTotalcompetantinspectorNotassignedPercentage { get; set; }


        public string inspectorassigned { get; set; }
        public string inspectorassignedPercentage { get; set; }
        public int GrandTotalinspectorassigned { get; set; }
        public float GrandTotalinspectorassignedPercentage { get; set; }





        //added By shrutika salve 27/07/2023

        public string Own { get; set; }
        public string OwnPercentage { get; set; }
        public string Other { get; set; }
        public string OtherPercentage { get; set; }
        public string OntimeOwnCalls { get; set; }
        public string OntimeOwnCallsPercentage { get; set; }
        public string OntimeOtherCalls { get; set; }
        public string OntimeOtherCallsPercentage { get; set; }
        public string DelayedOwnCalls { get; set; }
        public string DelayedOwnCallsPercentage { get; set; }
        public string DelayedOtherCalls { get; set; }
        public string DelayedOtherCallsPercentage { get; set; }

        //added by shrutika salve 27/07/2023

        public int GrandOwn { get; set; }
        public float GrandOwnPercentage { get; set; }
        public int GrandOther { get; set; }
        public float GrandOtherPercentage { get; set; }
        public int GrandOntimeOwnCalls { get; set; }
        public float GrandOntimeOwnCallsPercentage { get; set; }
        public int GrandOntimeOtherCalls { get; set; }
        public float GrandOntimeOtherCallsPercentage { get; set; }
        public int GrandDelayedOwnCalls { get; set; }
        public float GrandDelayedOwnCallsPercentage { get; set; }
        public int GrandDelayedOtherCalls { get; set; }
        public float GrandDelayedOtherCallsPercentage { get; set; }
        public string WrongReceiveDateCount { get; set; }
        public string WrongReceiveDate { get; set; }
        public string WrongRequestDateCount { get; set; }
        public string WrongRequestDate { get; set; }

        //added by shrutika salve 02/11/2023
        public string TSFilledPerformance { get; set; }
        public string finalperformance { get; set; }
        public string KPI { get; set; }

        public float GrandOriginatingBranchPerformance { get; set; }
        public float GrandExecutingBranchPerformance { get; set; }
        public float GrandFinalPerformance { get; set; }
        public float GrandKPI { get; set; }
        public float GrandTSFilledPerformance { get; set; }

        //added by shrutika salve 07112023
        public string Estimatedbycoordinator { get; set; }
        public string actualbyinspector { get; set; }
        public string Accuracytime { get; set; }
        public string Accuracytimeper { get; set; }


        public int GrandAccuracytime { get; set; }
        public float GrandAccuracytimeper { get; set; }

        public string TrainingCount { get; set; }
        public float GrandTrainingCount { get; set; }
        //added by shrutika salve 11122023
        public string profileperformance { get; set; }
        public string LessionLearntperformance { get; set; }
        public float GrandprofileCount { get; set; }
        public float GrandLessonsLearntCount { get; set; }
        public string PrimaryMaterial { get; set; }
        public string ReportType { get; set; }

        //added by shrutika salve 19032024
        public bool checkIFConcernDisplay { get; set; }
        public bool ItemDescriptionDynamic { get; set; }
        public bool checkIFCustomerSpecificReportNo { get; set; }
        public bool checkIFExpeditingReportvalue { get; set; }


        public bool checkInouttime { get; set; }
        public bool checkusereditableonpdf { get; set; }

        //added by shrutika salve 20032024
        public bool ManMonths { get; set; }

        public string WeekDays { get; set; }

        public string FinalInspectionValue { get; set; }

        public string TCEFilled { get; set; }

        public string inspectorapproved { get; set; }

        public string ManDays { get; set; }

        public string SubSubVendorName { get; set; }
        public string SubSubVendorPO { get; set; }
        public string SubSubVendorDate { get; set; }
        public string SJobType { get; set; }
        public string SubSubSubJob_No { get; set; }
        public string V3 { get; set; }
        public string P3 { get; set; }

        public string PK_UserID { get; set; }

        public string ActualDay { get; set; }

        public string CustomerRepresentative { get; set; }
        public string CallRemarks { get; set; }

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
        public string SAPEmpCode { get; set; }
        public string InspectionLocation_ { get; set; }
        public string TUVEmpCode { get; set; }
        public bool chkIfTravelExpenseOnPDF { get; set; }


        public string ActionHidden { get; set; }
        public string JobBlock { get; set; }
        
    }

    public class abc
    {
       
        public int PK_Call_ID { get; set; }
        public string Company_Name { get; set; }
        public string Status { get; set; }
        public string Originating_Branch { get; set; }
        public string Type { get; set; }
        public string Contact_Name { get; set; }
        public string Call_Recived_date { get; set; }
        public string Excuting_Branch { get; set; }
        public string Sub_Type { get; set; }
        public string Job { get; set; }
        public string Call_Request_Date { get; set; }
        public string Source { get; set; }
        public string Vendor_Name { get; set; }
        public string Sub_Job { get; set; }
        public string Planned_Date { get; set; }
        public string Urgency { get; set; }
        public string Competency_Check { get; set; }
        public string End_Customer { get; set; }
        public string Category { get; set; }
        public string Sub_Category { get; set; }
        public string Empartiality_Check { get; set; }
        public string Project_Name { get; set; }
        public string Job_Location { get; set; }
        public string Final_Inspection { get; set; }
        public string isWeek_Month_Call { get; set; }
        public string From_Date { get; set; }
        public string To_Date { get; set; }
        public string Inspector { get; set; }
        public string Client_Email { get; set; }
        public string Vendor_Email { get; set; }
        public string Tuv_Branch { get; set; }
        public string Executing_Branch { get; set; }
        public Nullable<int> PK_SubJob_Id { get; set; }
        public Nullable<int> PK_JOB_ID { get; set; }
        public Nullable<int> PK_QT_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public string DeleteStatus { get; set; }
        public string Call_No { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string From_Time { get; set; }
        public string To_Time { get; set; }
        public string Call_Type { get; set; }


        public string Branch_Name { get; set; }
        public int Br_Id { get; set; }

        public bool ClientEmailcheckbox { get; set; }
        public bool Vendorcheckbox { get; set; }
        public bool Homecheckbox { get; set; }
        public string FirstName { get; set; }


        public string UserBranchName { get; set; }
        public string AssignStatus { get; set; }



        public string SAP_Number{ get; set; }
        public string Sub_Job_Number { get; set; }
        public string Inspection_CallIntimationReceivedOn { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public string RequstedInspectedDate{ get; set; }
      //  public string Actual_Visit_Date                       { get; set; }
        public string LastInspectorName_DateOfInspection  { get; set; }
        public string Inspector_To_Be_Assign{ get; set; }
        public string CallStatus { get; set; }
        public string PONumberOnVender  { get; set; }
        public string PONumberOnSubVender { get; set; }
        public string OriginatingBranch { get; set; }
        public string StageOfInspection { get; set; }
        public string InspectionLocation{ get; set; }
        public string ItemsToBeInpsected { get; set; }
        public string Reasion { get; set; }
        public string ExtendCall_Status { get; set; }
        public string VisitDate { get; set; }
        public int DelayInINRSubmission { get; set; }

        //added by shrutika salve  19032024
        public string inspectorCompetant { get; set; }

        public string TCEFilled { get; set; }
        public string inspectorapproved { get; set; }
    }
    public class EmployeeAttendance
    {
        public List<EmployeeAttendance> lst1 { get; set; }
        public int Count { get; set; }
        public string PK_UserID { get; set; }
        public string BranchName { get; set; }
        public string NameOfEmployee { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeCode { get; set; }
        public string DateOfActivity { get; set; }
        public string ActivityType { get; set; }
        public string SubJobNumber { get; set; }
        public string Description { get; set; }
        public int ActivityHours { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }


        public string Date { get; set; }
        public string InspectorName { get; set; }
        public string Branch_Name { get; set; }
        
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
        public string Availibity { get; set; }
        public string Availibity22 { get; set; }
        public string CL { get; set; }
        public string SL { get; set; }


    }

    public class Item
{

        public string [] Name  { get; set; }
        public string [] Date { get; set; }
        public String [] PK_Call_ID { get; set; }
    }

    public class ItemTest
    {

        public string Name { get; set; }
        public string Date { get; set; }
        public String PK_Call_ID { get; set; }
        public string iscompedent { get; set; }
        public string isinspectorApproved { get; set; }
    }

    public class Test
    {

        public string Name { get; set; }
        public string Date { get; set; }
        public String PK_Call_ID { get; set; }
    }

    public class InspectorData
    {
        public List<InspectorData> lst1 { get; set; }
        public int Count { get; set; }
        public string User { get; set; }
        public string TUv_Email_ID { get; set; }
        public string MobileNo { get; set; }
        public string Branch { get; set; }
        public string EmpType { get; set; }
        public string CallsAssigned { get; set; }
        public string ClosedCalls { get; set; }
        public string VisitReports { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


    }

    public class CallAnalysis
    {
        public List<CallAnalysis> lst1 { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Job { get; set; }
        public string client { get; set; }
        public string call_no { get; set; }
        public string Call_Received_Date { get; set; }
        public string Call_Request_Date { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Executing_Branch { get; set; }
        public string Originating_Branch { get; set; }
        public string Continuous_Call { get; set; }
        public string Inspector { get; set; }
        public string Status { get; set; }
        public string Visit_Report_No { get; set; }


    }


    public class CallsModel1
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Call_No { get; set; }
        public string Excuting_Branch { get; set; }
        public string Originating_Branch { get; set; }
       
    }

    public class ProcessMeasures
    {
        public string BranchName { get; set; }
        public string TotalEmp { get; set; }
        public string inspector { get; set; }
        public string OtherEmp { get; set; }
        public string OnsiteoneMonitoring { get; set; }
        public string offsitethreeMonitoring { get; set; }
        public string ComplaintTotal { get; set; }
        public string openComplaints { get; set; }
        public string AttributeComplaints { get; set; }
        public string IVRCount { get; set; }
        public string IVRrevCount { get; set; }
        public string IVRRevper { get; set; }
        public string IVRcreationOnTimeCount { get; set; }
        public string IVRcreationOnTimeCountper { get; set; }
        public string IRNCount { get; set; }
        public string IRNRevCount { get; set; }
        public string IRNRevper { get; set; }
        public string IRNcreationOnTimeCount { get; set; }
        public string IRNcreationOnTimeCountPer { get; set; }
        public string EVRCount { get; set; }
        public string EVRRevCount { get; set; }
        public string NCROpen { get; set; }
        public string NCRClose { get; set; }
        public string executedCalls { get; set; }
        public string executedOwnBranch { get; set; }
        public string executedOtherBranch { get; set; }
        public string executedOnTimeCount { get; set; }
        public string executedOnTimePercentage { get; set; }
        public string executedOnTimeOwnCalls { get; set; }
        public string executedOnTimeOwnPercentage { get; set; }
        public string executedOnTimeOtherBranchesCall { get; set; }
        public string executedOnTimeOtherBranchesPercentage { get; set; }
        public string OriginatingTotalCall { get; set; }
        public string OriginatingOnTimeCount { get; set; }
        public string OriginatingOnTimePercentage { get; set; }
        public string FeedbackRec { get; set; }
        public string Feedback { get; set; }
        public string TotalMOnitoringRecords { get; set; }
        public string OnsiteCount { get; set; }
        public string OffSiteMonitoring { get; set; }
        public string Mentoring { get; set; }
        public string MonitoringOfMonitors { get; set; }
        public string specialMonitoring { get; set; }
        public List<ProcessMeasures> lstComplaint { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        public string offsitemonitoringOne { get; set; }
        public string TotalNCR { get; set; }
        public string SafetyReport { get; set; }
    }



}