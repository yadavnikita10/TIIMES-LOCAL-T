using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ModelOffSiteMonitoring
    {

        public List<ModelOffSiteMonitoring> lst1 { get; set; }
        public List<ModelOffSiteMonitoring> lstComplaintDashBoard1 { get; set; }
      public List<ModelOffSiteMonitoring> lstCallDashBoard1 { get; set; } 
        

        public string UIN { get; set; }

        public string IVR { get; set; }

        public string IRN { get; set; }

        public string TUVIConrolNumber { get; set; }

        public string Date { get; set; }

        public string InspectorName { get; set; }

        public string InspectrorLevelofauthorisation { get; set; }

        public string Scope { get; set; }

        public string MonitorName { get; set; }
        public string Monitorlevelofauthorisation { get; set; }
        public string TUVIcontrolnumber { get; set; }

        public string CustomerName { get; set; }

        public string EndCustomerName { get; set; }

        public string ProjectName { get; set; }

        public string VendorName { get; set; }

        public string VendorLocation { get; set; }

        public string ItemInspected { get; set; }
        public string on_site_time { get; set; }

        public string off_site_time { get; set; }

        public string travel_time { get; set; }
        public string copy_Report { get; set; }

        public int PK_IAFScopeId { get; set; }

        public string IAFScopeName { get; set; }

        public string ReferenceDocument { get; set; }

        public string ReportName { get; set; }

        public int PK_CALL_ID { get; set; }

        public string Type { get; set; }

        public string Qid { get; set; }

        [Required(ErrorMessage = "please Fill Answer")]

        public string Ans { get; set; }

        public string insertcheckbox { get; set; }

        public string FreeText { get; set; }

        public bool Checkbox1 { get; set; }

        public string QuestionNo { get; set; }

        public string Question { get; set; }
        public string OptButton { get; set; }

        public string checkbox { get; set; }

        public string cheboxans { get; set; }
        [Required(ErrorMessage = "please Fill Answer")]
        public string FreeText1 { get; set; }

        public string InspectorComment { get; set; }

        public string Reporting_manager_comments { get; set; }

        public string Item_Inspected { get; set; }

        public string Yes { get; set; }
        public string No { get; set; }
        public string NA { get; set; }

        public int PK_RM_ID { get; set; }

        public string Date_of_PO { get; set; }
        public string Sub_Vendor_Name { get; set; }
        public string Po_No_SubVendor { get; set; }
        public string SubSubVendorDate_of_PO { get; set; }

        public string DEC_PMC_EPC_Name { get; set; }

        public string DEC_PMC_EPC_Assignment_No { get; set; }
        public string Po_No { get; set; }

        public string inspectorId { get; set; }


        public string PK_IVR_ID { get; set; }
        public string itemDescription { get; set; }

        public string Reference_Document { get; set; }

        public string Report { get; set; }

        public string CreatedDate { get; set; }


        //added by shrutika salve 07-06-2023
        public string inspectorCommetName { get; set; }
       
        public string InspectorCommentDate { get; set; }

        public string ManagerCommentName { get; set; }
       
        public string ManagerCommentDate { get; set; }

        //added by shrutika salve 07092023

        public virtual ICollection<FileDetails> FileDetails { get; set; }

        //added by shrutika salve 28092023

        public int Status { get; set; }

        public int rating { get; set; }

        public int RatingAvg { get; set; }
        public List<ModelOffSiteMonitoring> lstQuestionrate { get; set; }



        public string Qidrate { get; set; }
        public string RateQuestion { get; set; }

        public int id { get; set; }

        public bool chkMan { get; set; }

        public string CurrentAssignment { get; set; }






    }
}