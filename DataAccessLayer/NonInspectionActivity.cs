using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class NonInspectionActivity
    {

        public List<NonInspectionActivity> lst1 { get; set; }
        public List<NonInspectionActivity> NIADashBoard { get; set; }
        public int Count { get; set; }

        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Activity Type")]
        public string ActivityType { get; set; }
        public string Location { get; set; }


        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
       // [Required(ErrorMessage = "Please Select Start Date")]
       // [Display(Name = "Date of Joining Name")]
        public DateTime StartDate { get; set; }


        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Please Select End Date")]
      //  [Display(Name = "Date of Joining Name")]
        
        public DateTime EndDate { get; set; }
        public DateTime DateSE { get; set; }
        public string ServiceCode { get; set; } 
        public string Description { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string FromD { get; set; }

        public string ToD { get; set; }

        public string Attachment { get; set; }
        public string TravelTime { get; set; }
        public string TotalTime { get; set; }
        public string Type { get; set; }
        public Nullable<int> PK_Call_ID { get; set; }
        public Nullable<int> PK_IVR_ID { get; set; }
        public string CreatedBy { get; set; }


        public string Branch { get; set; }
        public string EmployeeCode { get; set; }
        public string Sub_Job { get; set; }
        public string SAP_No { get; set; }


          public string Vendor_Name { get; set; }
          public string Project_Name { get; set; }
         public string Job_Location { get; set; }
        public string OriganatingBranch { get; set; }
        public string ExcutingBranch { get; set; }

        public string ManDays { get; set; }

        public Nullable<int> total { get; set; }
        public DateTime? Date { get; set; }

        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
    }
}