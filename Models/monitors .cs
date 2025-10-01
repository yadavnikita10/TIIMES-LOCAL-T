using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class monitors
    {
        public List<monitors> lstComplaintDashBoard1 { get; set; }
        public List<monitors> monitoringDashBoard { get; set; }
       

        public string TypeOfmonitoring { get; set; }
        public bool Check { get; set; }
        public string Check1 { get; set; }
        public bool Check2 { get; set; }
        public string che2 { get; set; }
        public bool check3 { get; set; }
        public string che3 { get; set; }
        public string UserId { get; set; }
        public string otp { get; set; }

        public string inspectorId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string UIN { get; set; }
        
        public string Call_No { get; set; }
        public string pk_call_id { get; set; }

        public string Date { get; set; }
        public string BranchName { get; set; }

        public string Inspector_Name { get; set; }

        public string InspectorComment { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedDate { get; set; }
        public string Manager { get; set; }

        public string Inspector_Level_of_authorisation { get; set; }

        public string Scope { get; set; }

        public string Monitor_Name { get; set; }

        public string Monitor_level_of_authorisation { get; set; }

        public string TUVI_control_number { get; set; }

        public string Customer_Name { get; set; }

        public string EndCustomerName { get; set; }

        public string ProjectName { get; set; }
       
        public string Vendor_Name { get; set; }
       
        public string Vendor_Location { get; set; }

        public string Item_Inspected { get; set; }
        
        public string on_site_time { get; set; }
        
        public string off_site_time { get; set; }
        
        public string travel_time { get; set; }
       
       
        public string Reference_Document { get; set; }
      
        public string Details_of_inspection_activity { get; set; }

        public String Yes { get; set; }

        public string No { get; set; }

        public string NA { get; set; }

        public string comment { get; set; }
        public int PK_IAFScopeId { get; set; }

        public string IAFScopeName { get; set; }


        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public string Q4 { get; set; }

        
        public string Date_of_PO { get; set; }
        public string Sub_Vendor_Name { get; set; }
        public string Po_No_SubVendor { get; set; }
        public string SubSubVendorDate_of_PO { get; set; }

        public string DEC_PMC_EPC_Name { get; set; }

        public string DEC_PMC_EPC_Assignment_No { get; set; }
        public string Po_No { get; set; }

        public string QuestionNo { get; set; }

        public string Question { get; set; }
        public string OptButton { get; set; }

        public string FreeText { get; set; }

       
        public bool Checkbox1 { get; set; }
        
        public string checkbox { get; set; }
        public virtual ICollection<monitors> ATT1 { get; set; }

        public List<monitors> lstCallDashBoard1 { get; set; }

        [Required(ErrorMessage = "please Fill Answer")]
        public string Ans { get; set; }
        public string Observations { get; set; }

        public string Declaration { get; set; }
        
        public string Comments_by_Inspector { get; set; }

        
        public string Reporting_manager_comments { get; set; }
        public string Qid { get; set; }
        public string comment1 { get; set; }
        public string training_topic { get; set; }
        public string UINId { get; set; }
        
        public string insertcheckbox { get; set; }
        [Required(ErrorMessage = "please Fill Answer")]
        public string FreeText1 { get; set; }

        public string BranchManager { get; set; }

        public string itemDescription { get; set; }
        public string CreatedDate { get; set; }

        public string inspectorCommetName { get; set; }
        public string InspectorCommentDate { get; set; }

        public string ManagerCommentName { get; set; }
        public string ManagerCommentDate { get; set; }

        public int Id { get; set; }

        //added by shrutika salve 28092023

        public int Status { get; set; }

        public int rating { get; set; }

        public int RatingAvg { get; set; }
        public List<monitors> lstQuestionrate { get; set; }



        public string Qidrate { get; set; }
        public string RateQuestion { get; set; }
        public string pk_inspectionId { get; set; }//added by satish

        public class MNameCode
        {
            public string Name { get; set; }
            public int Code { get; set; }
            public SelectList SectionModel { get; set; }
        }

       
        public string cheboxans { get; set; }

        public string freetextans { get; set; }

        //added by shrutika salve 07092023

        public virtual ICollection<FileDetails> FileDetails { get; set; }

        public string monitoringid { get; set; }

        public bool chkMan { get; set; }


        public string chkManData { get; set; }

        public string CurrentAssignment { get; set; }

        public bool specialMonitoring { get; set; }

        public string specialMonitoringdata { get; set; }

        public string SpecialServicesData { get; set; }

    }
}
