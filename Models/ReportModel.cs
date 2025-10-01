using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ReportModel
    {
        public List<ReportModel> lst1 { get; set; }
        public List<ReportModel> LstDashboard { get; set; }
        public int Count { get; set; }
        public int PK_RM_ID { get; set; }
        public string ReportName { get; set; }
        public int abcid { get; set; }
        public string Report { get; set; }
        public Nullable<int> PK_CALL_ID { get; set; }
        public string Status { get; set; }
        public string CraetedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Type { get; set; }
        public string ImageReport { get; set; }
        public string ProjectName { get; set; }
        public string inspectionDate { get; set; }
        public string SubJob_No { get; set; }
        public Nullable<int> PK_SubJob_Id { get; set; }
        public string Po_No { get; set; }
        public string VendorName { get; set; }
        public string ClientName { get; set; }

        public string ReportNo { get; set; }
        public bool isComfirmation { get; set; }
        public string ddlReviseReason { get; set; }//added by nikita on 03102023
        public string ReviseReason { get; set; }//added by nikita on 03102023

        public string Date_Of_Inspection { get; set; }//added by nikita on 03102023
        public string mitigateddate { get; set; }//added by nikita on 03102023
        public string FromDate { get; set; }//added by nikita on 03102023
        public string ToDate { get; set; }//added by nikita on 03102023



        public string IVRDownloadDate { get; set; } //added by nikita on 25092023
        public int checkIFCustomerSpecific { get; set; } //added by nikita on 25092023
        public string ReportprepareinTiimes { get; set; } //added by nikita on 25092023

        public string Sap_And_Controle_No { get; set; }
        public string Project_Name_Location { get; set; }
        public string Client_Name { get; set; }
        public string Vendor_Name_Location { get; set; }

        public string Inspector { get; set; }
        
        public string ReportDate { get; set; }
        public string Product_item { get; set; }
        public string Originating_Branch { get; set; }
        public string Excuting_Branch { get; set; }
        public string NCR { get; set; }

        public string Call_No { get; set; }

        public string CanIRNbeissued { get; set; }

        public string IssuedPOItemNumbers { get; set; }

        public string Conclusion { get; set; }

        public string Areas_Of_Concerns { get; set; }
        public string Report_No { get; set; }
        public string PendingActivities { get; set; }
        public string ImageName { get; set; }
        public string AttFileName { get; set; }
        public string IVRNo { get; set; }

        public string Edit { get; set; }

        public string Error { get; set; }

        //added by shrutika salve  06092023

        public string refDocument { get; set; }
        public string detailsDocument { get; set; }

        public string checkIFCustomer { get; set; }

        // added by shrutika salve 31/08/2023
        public string lastdateofinspection { get; set; }
        public string IVR { get; set; }
        public string Attachment { get; set; }
        public string insopectionRecord { get; set; }

        public string CustomerName { get; set; }


    }
}