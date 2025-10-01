using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class BDSMActivity
    {
        public List<BDSMActivity> listingBDSMActivityrecord { get; set; }

        public int Count { get; set; }  //added by nikita on 09-09-2023

        public string EmployeeName { get; set; }

        public string HREmployeeCode { get; set; }

        public string Tuv_Email_Id { get; set; }

        public string MobileNo { get; set; }

        public string Designation { get; set; }

        public string Id { get; set; }

        public string TravelTime { get; set; }

        public string ActivityDate { get; set; }

        public string EndDate { get; set; }

        public string ActivityType { get; set; }

        public string StartTime { get; set; }

        public string SAPEmpCode { get; set; }

        public string EmployeeCategory { get; set; }

        public string CostCenter { get; set; }


        public string EndTime { get; set; }

        public string Description { get; set; }


        public string New_ExistingCustomer { get; set; }

        public string Dom_Inter_Visit { get; set; }

        public string Location { get; set; }


        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public List<BDSMActivity> lst1 { get; set; }


    }
}