using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class MarketingPerformance
    {
        public string PK_UserId { get; set; }
        public string TotalEnquiries { get; set; }
        public string EnquiriesOnTime { get; set; }
        public string OnTimePercentage { get; set; }
        public string QuotationsGeneratedAgainstEnquiries { get; set; }
        public string QuotationsPercentage { get; set; }
        public string TotalQuotationsGenerated { get; set; }
        public string ActionTakenQuotations { get; set; }
        public string ActionTakenQuotationsPercentage { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }  
        public string JobCreated { get; set; }
        public string JobCreatedOnTime { get; set; }
        public string Name { get; set; }
        public string JobPercentage { get; set; }
        public string OverAllPercentage { get; set; }
        public string Branch { get; set; }


        public string profilePerformancePercentage { get; set; }
        public string TSFilledPercentage { get; set; }
        public string AVG { get; set; }
        public string KPI { get; set; }



    }

    public class QHSEPerformance
    {
        public string Branch_Name { get; set; }
        public string No_Of_Complaints { get; set; }
        public string No_Of_Complaints_On_Time { get; set; }
        public string Percentage_On_Time { get; set; }
        public string No_Of_Inspectors { get; set; }
        public string NumberOfTCEFilled { get; set; }
        public string Percentage_TCE_Filled { get; set; }
        public string No_Of_SafetyIncidentReports { get; set; }
        public string No_Of_OnTimeReports { get; set; }
        public string Percentage_OnTimeReports { get; set; }
        public string No_Of_Complaints_ReceivedExe { get; set; }
        public string No_Of_Complaints_Closed { get; set; }
        public string No_Of_Complaints_Open { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }



    }



    public class OperationPerformance
    {
        public string InspectorName { get; set; }
        public string Branch { get; set; }
        public string TSFilledPercentage { get; set; }
        public string ProfilePercentage { get; set; }
        public string AVG { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class BranchPerformance
    {
        public string BranchName { get; set; }
        public string AvgInspectorPer { get; set; }
        public string AvgCordinatorPer { get; set; }
        public string AvgMarketingPer { get; set; }
        public string AvgQHSEPer { get; set; }
        public string AvgOperationPer { get; set; }
        public string GrandFinal { get; set; }

        public string SelectedValue { get; set; }
        public List<SelectListItem> DropdownItems { get; set; }
    }

}