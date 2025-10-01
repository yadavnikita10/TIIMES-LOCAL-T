using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class MonitoringOfMonitors
    {
        public List<MonitoringOfMonitors> lstCallDashBoard4 { get; set; }


        public List<ModelOffSiteMonitoring> lst1 { get; set; }
        public List<ModelOffSiteMonitoring> lstComplaintDashBoard { get; set; }
        public List<ModelOffSiteMonitoring> lstCallDashBoard1 { get; set; }

        public string TypeOfmonitoring { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string UIN { get; set; }

        public string Report_No { get; set; }

        public string Date { get; set; }

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

        public string QuestionNo { get; set; }

        public string Question { get; set; }
        public string OptButton { get; set; }

        public string FreeText { get; set; }
        
        public bool Checkbox1 { get; set; }
        
        public string checkbox { get; set; }
        [Required(ErrorMessage = "please Fill Answer")]

        public string Ans { get; set; }

        public string cheboxans { get; set; }
        [Required(ErrorMessage = "please Fill Answer")]

        public string FreeText1 { get; set; }

        public string Reporting_manager_comments { get; set; }
        
        public string insertcheckbox { get; set; }

        public string Qid { get; set; }

        public string Yes { get; set; }

        public string No { get; set; }

        public string NA { get; set; }

        public int PK_CALL_ID { get; set; }

        public string Date_of_PO { get; set; }
        public string Sub_Vendor_Name { get; set; }
        public string Po_No_SubVendor { get; set; }
        public string SubSubVendorDate_of_PO { get; set; }

        public string DEC_PMC_EPC_Name { get; set; }

        public string DEC_PMC_EPC_Assignment_No { get; set; }
        public string Po_No { get; set; }


        public string inspectorId { get; set; }


        public string itemDescription { get; set; }

        public string ReportNo { get; set; }

        public string MonitorinspectorName { get; set; }

        public string InspectorLevelauthorisation { get; set; }


        //added by shrutika salve 07-06-2023
        public string inspectorCommetName { get; set; }

        public string InspectorCommentDate { get; set; }

        public string ManagerCommentName { get; set; }

        public string ManagerCommentDate { get; set; }

        public string CreatedDate { get; set; }

        public int Id { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        //added by shrutika salve 28092023

        public int Status { get; set; }

        public int rating { get; set; }

        public int RatingAvg { get; set; }
        public List<MonitoringOfMonitors> lstQuestionrate { get; set; }



        public string Qidrate { get; set; }
        public string RateQuestion { get; set; }

        public bool chkMan { get; set; }

        public string CurrentAssignment { get; set; }

        public bool specialMonitoring { get; set; }
    }
}