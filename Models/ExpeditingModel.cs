using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ExpeditingModel
    {

        public int PK_Expediting_Id { get; set; }
        public List<ExpeditingModel> lst1 { get; set; }
        public string Date_Of_Expediting { get; set; }
        public string PK_call_id { get; set; }
        public string Controle_No { get; set; }
        public string Executing_branch { get; set; }
        public string Executing_branchId { get; set; }
        public string Customer_Name { get; set; }
        public string End_Customer_Name { get; set; }
        public string Project_Name { get; set; }
        public string DEC_PMC_EPC_Name { get; set; }
        public string DEC_PMC_EPC_Assignment_No { get; set; }
        public string NotificationNumber { get; set; }
        public string ExpeditingLocation { get; set; }
        public string VendorName { get; set; }
        public string Po_No { get; set; }
        public string Po_Date { get; set; }
        public string Sub_VendorName { get; set; }
        public string SubPo_No { get; set; }
        public string SubPo_Date { get; set; }
        public string Contractual_DeliveryDate { get; set; }
        public string Expected_DeliveryDate { get; set; }
        public string CurrentOverallPOStatus { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string ModifiedDateTime { get; set; }
        public List<NonInspectionActivity> Activity { get; set; }
        public string CallIDs { get; set; }
        public string Date_Of_Inspection { get; set; }
        public string ReportName { get; set; }
        public string PK_SubJob_Id { get; set; }
        public string Sub_Job_No { get; set; }
        public string Edit { get; set; }
        public string Sap_no { get; set; }

        public bool IsComfirmation { get; set; } //added by nikita on 11/11/2024
        public string CMP_ID { get; set; }

        //added by shrutika salve 28122023

        public string Inspector { get; set; }
        public string InspectionDate { get; set; }
        public string ReportDate { get; set; }
        public string Product_item { get; set; }
        public string Originating_Branch { get; set; }
        public string Call_no { get; set; }
        public int Pk_RM_id { get; set; }
        public string ReportNo { get; set; }

        public string OverAll { get; set; }

        public string LastExpeditingtypeandDate { get; set; }
        public string ExpeditingType { get; set; }

        public string AssignmentNumber { get; set; }

        public List<ExpeditingModel> Emp { get; set; }
        public string Represting { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }

        public bool checkCustomer { get; set; }
        public bool Vendor { get; set; }
        public bool TUVI { get; set; }

        public string closureRemarks { get; set; }

        public string MailSubject { get; set; } //added by nikita on 19092024
        public string ContactNames { get; set; }
        public string ContactCompanyName { get; set; }
        public string Title { get; set; }

        public string HomePhone { get; set; }
        public string Mobile { get; set; }
        public string Fax_No { get; set; }
        public bool IsMainContact { get; set; }
        public string ContactStatus { get; set; }
        public string Address { get; set; }
        public string TitleName { get; set; }
        public string Tuv_Email { get; set; }



    }

    public class ExpItemDescription
    {
        //PK_Item_Detail
        //PO_Item_Number
        //ItemCode
        //ItemDescription
        //Equipment_TagNo
        //Quantity
        //Unit
        //Contractual_DeliveryDateAsPerPO
        //CreatedBy
        //CreatedDateTime
        //Modifiedby
        //ModifiedDateTime
        //PK_Call_ID
        public int PK_Item_Detail { get; set; }
        public int PK_Call_ID { get; set; }
        public string PO_Item_Number { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Equipment_TagNo { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string DeleteStatus { get; set; }
        public string Contractual_DeliveryDateAsPerPO { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Modifiedby { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        //added by shrutika salve 01/01/2024
        
        public string EstimatedDeliveryDate { get; set; }
        public string TotalConcerns { get; set; }
        public string actualprogress { get; set; }
        public string progressStatus { get; set; }

        public string itemName { get; set; }

    }

    public class Progress
    {
        //PK_Stages_Id
        //PK_Call_Id
        //Stages
        //ExpectedStartDate
        //ExpectedEndDate
        //CreatedBy
        //CreatedDate
        //ModifiedBy
        //ModifiedDate

       public int Id { get; set; }
       public string PK_Process_Id      { get; set; }
       public string PK_Call_Id        { get; set; }
       public string Stages            { get; set; }
        [Required(ErrorMessage = "Please enter the Expected Start date.")]
        public string ExpectedStartDate { get; set; }
       [Required(ErrorMessage = "Please enter the Expected End date.")]
        public string ExpectedEndDate   { get; set; }
       public string CreatedBy         { get; set; }
       public string CreatedDate       { get; set; }
       public string ModifiedBy        { get; set; }
       public string ModifiedDate { get; set; }
       public List<Progress> Activity { get; set; }
        [Required(ErrorMessage = "Please enter the Actual start date.")]
        public string Actual_Start_Date { get; set; }
       
        public string Actual_End_Date { get; set; }
       public string Expected_Progress { get; set; }
       public string Actual_Progress { get; set; }
       public string Expected_Progressper { get; set; }
       public string Actual_Progressper { get; set; }
       public string delaydays { get; set; }
       public string item { get; set; }

        public string ConcernClosureper { get; set; }
        public string Concern { get; set; }
        public string ProgressStatus{ get; set; }

        public string Customer { get; set; }
        public string vendor { get; set; }
        public string TUVI { get; set; }
        public string Actionstage { get; set; }

        public string ActionValue { get; set; }

        public string ExpeditingDate { get; set; }

        public string Total { get; set; }

        public string VendorPO { get; set; }

        public string itemnumbar { get; set; }

        public string itemtag { get; set; }

        public string ItemName { get; set; }

        public string pk_subjobid { get; set; }

        public string OntimeCount { get; set; }
        public string DelayCount { get; set; }
        public string earlycount { get; set; }

        public string itemCode { get; set; }
        public string ItemDescription { get; set; }
        public string Unit { get; set; }
        public string Quantity { get; set; }





    }

    public class Engineering
    {
        //PK_Engi_Id
        //PK_Call_Id
        //Item
        //ReferenceDocument
        //Number
        //Status
        //StatusUpdatedOn
        //CreatedBy
        //CreatedDate
        //ModifyBy
        //ModifiedDate
        public List<Engineering> lstEngineering { get; set; }
        public int PK_Engi_Id { get; set; }
        public string PK_Call_Id { get; set; }
        public string Item { get; set; }
        public string ReferenceDocument { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifiedDate { get; set; }
        public string Actual_Progressper { get; set; }
        public string delaydays { get; set; }
        public string QuantityNumber { get; set; }
        public string EstimatedEndDate { get; set; }

    }

    public class NameStatusEngineering
    {
        public string Text { get; set; }
        public int Value { get; set; }
       

        //public SelectList SectionModel { get; set; }
    }

    public class NamePoitemDetails
    {
        public string Text { get; set; }
        public string Valuepo { get; set; }
       

        //public SelectList SectionModel { get; set; }
    }

    public class Material
    {
        public List<Material> lstMaterial { get; set; }
        public int PK_Material_Id      { get; set; }
        public string PK_Call_Id          { get; set; }
        public string Item                { get; set; }
        public string Material_Category   { get; set; }
        public string Description         { get; set; }
        public string UOM                 { get; set; }
        public string Quantity            { get; set; }
        public string Source              { get; set; }
        public string Status              { get; set; }
        public string StatusUpdatedOn     { get; set; }
        public string OrderPlacedOn       { get; set; }
        public string PONumber            { get; set; }
        public string CreatedBy           { get; set; }
        public string CreatedDate         { get; set; }
        public string ModifyBy            { get; set; }
        public string ModifiedDate { get; set; }

        //added by shrutika salve 02012023

        public string PODate { get; set; }
        public string Contractual_DeliveryDateAsPerPO { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string MaterialActual_Progressper { get; set; }
        public string delaydays { get; set; }
        public string EstimatedEndDate { get; set; }
    }

    public class Manufacturing
    {
        public List<Manufacturing> lstManufacturing { get; set; }
        public int PK_Manufacturing_Id { get; set; }
        public string PK_Call_Id { get; set; }
        public string Item { get; set; }
        public string PartName { get; set; }
        public string UOM { get; set; }
        public string Quantity { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedOn{ get; set; }
        public string CreatedBy{ get; set; }
        public string CreatedDate{ get; set; }
        public string ModifyBy{ get; set; }
        public string ModifiedDate{ get; set; }
        public string ManufacturingProgressper { get; set; }
        public string delaydays { get; set; }
        public string EstimatedEndDate { get; set; }

    }

    public class FinalStages
    {
        public List<FinalStages> lstStages { get; set; }
        public int PK_FinalStage_Id { get; set; }
        public string PK_Call_Id { get; set; }
        public string Item { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifiedDate { get; set; }
        public string FinalstageProgressper { get; set; }
        public string delaydays { get; set; }
        public string Document { get; set; }
        public string EstimatedEndDate { get; set; }

    }

    public class Concerns
    {
        public List<Concerns> lstConcerns { get; set; }
        public int PK_Concern_Id { get; set; }
        public string PK_Call_ID { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Item { get; set; }
        public string Stage { get; set; }
        public string Details { get; set; }
        public string MitigationBy { get; set; }
        public string ResponsiblePerson { get; set; }
        public string ExpectedClosureDate { get; set; }
        public string Status { get; set; }
        public string StatusUpdatedOn { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifiedDate { get; set; }

        //added by shrutika salve 28122023
        public string pk_userid { get; set; }
        public string MitigationByUserid { get; set; }
        public string ConcernsstageProgressper { get; set; }
        public string delaydays { get; set; }
        public string ActionReq { get; set; }

        public string PreviousComments { get; set; }

        public string VendorPONumber { get; set; }

    }

    public class Photo
    {
        public int PK_IP_Id { get; set; }
        public string Heading { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> PK_IVR_ID { get; set; }
        public string PK_CALL_ID { get; set; }
        public string Type { get; set; }
        public bool chkbox { get; set; }
        public List<Photo> Activity { get; set; }
        

    }

    public class Observation
    {
        public string PK_Call_ID { get; set; }
        public string Stage { get; set; }
        public string Findings { get; set; }
        public List<Observation> lstStages { get; set; }
        public int PK_Final { get; set; }
        public string CreatedDate { get; set; }
    }

    public class Records
    {
        public virtual ICollection<Records> ReportDetails { get; set; }
        public string AttType { get; set; }
        public string Name { get; set; }
        public string PK_Call_ID { get; set; }
        public string FileName { get; set; }

        


    }

    public class ExpDelays
    {
        public int Id { get; set; }
        public string PK_Call_ID { get; set; }
        public string ExpEndDate { get; set; }
        public string ExpstartDate { get; set; }
        public string ActEndDate { get; set; }
        public string ActstartDate { get; set; }
        public string Stage { get; set; }
        public string ExpDelayBy  { get; set; }
        public string ExpReasonfordelay  { get; set; }
        public string ExpMitigationby  { get; set; }
        public string ExpResponsiblePerson  { get; set; }
        public string ExpComments { get; set; }
        public List<ExpDelays> Activity { get; set; }

        public int pk_expDelayID { get; set; }
        public string item { get; set; }
        public string ActionReq { get; set; }
        public string PreviousComments { get; set; }
        public string PoitemCode { get; set; }
        public string UOM { get; set; }
        public string POQuantity { get; set; }
        public string CDD { get; set; }
        public string EDD { get; set; }

        public string Concerns { get; set; }
        public string engDelay { get; set; }
        public string ProDelay { get; set; }
        public string manuDelay { get; set; }
        public string FinalDelay { get; set; }

        public string engAct { get; set; }
        public string ProAct { get; set; }
        public string manuAct { get; set; }
        public string FinalAct { get; set; }

        public string engExp { get; set; }
        public string ProExp { get; set; }
        public string manuExp { get; set; }
        public string FinalExp { get; set; }

        public string actualprogress { get; set; }
        public string progressStatus { get; set; }



    }

    public class ExpCommercial
    {
        public string PK_Call_Id { get; set; }
        public List<ExpCommercial> Activity { get; set; }
        public string Documents { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public int Id { get; set; }

        public string Date { get; set; }

    }
    public class StatusData
    {
        public string status { get; set; }
        public string statusValue { get; set; }
        public int statusid { get; set; }
        public int Stageid { get; set; }
        public string statusActionRequirdby { get; set; }
        public int PK_JOB_ID { get; set; }
    }

    public class ExpeditingData
    {
        public int PK_call_id { get; set; }
        public string chartData { get; set; }
    }

}