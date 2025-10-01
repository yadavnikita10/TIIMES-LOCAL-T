using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class EmployeeProductvityReport
    {
        public List<EmployeeProductvityReport> lst1 { get; set; }
        public int Count { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public string InspectorID { get; set; }
        public string BranchName { get; set; }


        public string Branch_Name { get; set; }
        public string Location_Name { get; set; }
        public string Employee_Name { get; set; }
        public string TPI { get; set; }
        public string PED { get; set; }
        public string ASME { get; set; }
        public string Energy { get; set; }
        public string OTHERS { get; set; }
        public string RAILWAY { get; set; }
        public string IBR { get; set; }
        public string EXPEDITING_SERVICES { get; set; }
        public string TRCU { get; set; }
        public string PESO { get; set; }
        public string PNGRB { get; set; }
        public string VENDOR_EVALUATION { get; set; }
        public string DESIGN_APPRAISAL { get; set; }
        public string ASSET_INTEGRITY { get; set; }
        public string QUANTITY_VERIFICATION { get; set; }
        public string RENEWABLE { get; set; }
        public string NUCLEAR { get; set; }
        public string CONVENTIONAL_POWER_PLANT { get; set; }
        public string TotalCount { get; set; }
        public string ActivityType { get; set; }
        public string TotalHoursCount { get; set; }
        public string LeveCount { get; set; }

    }


    public class N
    {
        public string Name { get; set; }
        public string Code { get; set; }
        //public SelectList SectionModel { get; set; }
    }



}