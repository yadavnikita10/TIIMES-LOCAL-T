using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{

    public class IVRDelay
    {
        public string FromD { get; set; }
        public string ToD { get; set; }

        public List<IVRDelay> lstDelay { get; set; }
        public string Call_No { get; set; }
        public string Call_CreatedDate { get; set; }
        public string Call_CreatedBy { get; set; }
        public string Actual_Visit_Date { get; set; }
        public string CallType { get; set; }
        public string Sub_JobNo { get; set; }
        public string Customer_Name { get; set; }
        public string ProjectName { get; set; }
        public string VendorName { get; set; }
        public string SubVendorName { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string Originating_Branch { get; set; }
        public string Excuting_Branch { get; set; }
        public string InspectorName { get; set; }
        public string ContinuousDate { get; set; }
        public int Count { get; set; }

    }

    public class IRNDelay
    {
        public List<IRNDelay> lstIRNDelay { get; set; }
        public string InspectorName { get; set; }
        public string InspectorEmail { get; set; }
        public string ReportNo { get; set; }
        public string Call_No { get; set; }
        public string Date_Of_Inspection { get; set; }
        public string Vendor_name { get; set; }
        public string CreatedDate { get; set; }
        public string FromD { get; set; }
        public string ToD { get; set; }


    }


    }