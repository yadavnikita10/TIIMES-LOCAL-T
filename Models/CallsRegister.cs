using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CallsRegister
    {

        public List<CallsRegister> listingCallRegister { get; set; }

        public string Job { get; set; }
        public int Count { get; set; }

        public string client { get; set; }

        public string call_no { get; set; }
        public string Call_Received_Date { get; set; }

        public string Call_Request_Date { get; set; }
        public string Actual_Visit_Date { get; set; }

        public string CreatedBy { get; set; }
        public string Originating_Branch { get; set; }

        public string Continuous_Call { get; set; }
        public string Visit_Report_No { get; set; }

        public string createddate { get; set; }
        public string Executing_Branch { get; set; }
        public string inspector { get; set; }

        public string status { get; set; }
        public string iscompetant { get; set; }

        public string CallAssignBy { get; set; }
        public string CallAssignDate  { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string End_Customer { get; set; }
        public string Vendor_Name { get; set; }

        //added by shrutika salve 29012023

        public string ProjectName { get; set; }
        public string DECPMCName { get; set; }
        public string itemTobeInspected { get; set; }
        public string stageOfinspection { get; set; }
        public string primaryMaterial { get; set; }
        public string StageDescription { get; set; }
        public string itemQty { get; set; }
        public string EstimatedTimeinHours { get; set; }


        public string FormFilled { get; set; }
        public string IsVerified { get; set; }

        public string Finalinsepection { get; set; }

        public string ManMonthsAssi { get; set; }


        public string subendusername { get; set; }

        public string subsubendusername { get; set; }

        public string vendorLoaction { get; set; }

        public string Reason { get; set; }

        public string callcancelby { get; set; }

        public string CallCancelDate { get; set; }

        public string SAP_NO { get; set; }



    }
}