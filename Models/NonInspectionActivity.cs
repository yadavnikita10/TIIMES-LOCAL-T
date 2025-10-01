using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;


namespace TuvVision.Models
{
    public class NonInspectionActivity
    {

        public List<NonInspectionActivity> lst1 { get; set; }
        public List<NonInspectionActivity> NIADashBoard { get; set; }
        public int Count { get; set; }

        public int Id { get; set; }

        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Activity Type")]
        public string ActivityType { get; set; }
        public string Location { get; set; }


        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        // [Required(ErrorMessage = "Please Select Start Date")]
        // [Display(Name = "Date of Joining Name")]
        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Start Date")]
        public string StartDate { get; set; }


        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        //[Required(ErrorMessage = "Please Select End Date")]
        //  [Display(Name = "Date of Joining Name")]
        // [DataType(DataType.Date)]
        //[Required(ErrorMessage = "End Date")]
        public string EndDate { get; set; }

        public string DateSE { get; set; }
        public string ServiceCode { get; set; }
        public string Description { get; set; }

        //[Required(ErrorMessage = "On Site Time")]
        [Range(1, 14, ErrorMessage = "Value must be between 1 to 14")]
        //public Nullable<double> StartTime { get; set; }
        public double StartTime { get; set; }

        [Range(0, 12, ErrorMessage = "Value must be between 0 to 12")]
        public double EndTime { get; set; }

        // [Required(ErrorMessage = "Please enter Start Date")]
        public string FromD { get; set; }

        // [Required(ErrorMessage = "Please enter End Date")]
        public string ToD { get; set; }

        public string Attachment { get; set; }

        //[Required(ErrorMessage = "Travel Time")]
        ///  [Range(6, 24, ErrorMessage = "Value must be between 6 to 24")]
        // [Range(0, 8, ErrorMessage = "Value must be between 0 to 8")]
        //public Nullable<double> TravelTime { get; set; }
        public double TravelTime { get; set; }

        public double TotalTime { get; set; }
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

        public string JobNumber { get; set; }
        public string CallNumber { get; set; }

        [Range(0, 16, ErrorMessage = "Value must be between 0 to 16")]
        public double ODTime { get; set; }

        public bool WFHCheckbox { get; set; }
        public List<NonInspectionActivity> Activity { get; set; }

        public string DataEntryView { get; set; }

        public string EstimatedTime { get; set; }
        public string Rating { get; set; }

        public string OBSName { get; set; }
        public string PortfolioName { get; set; }
        public string ServiceName { get; set; }


        //addded by nikita on 29042024
        public string CostCenter { get; set; }
        public string CostCenter_name { get; set; }


        public string issendforapprovalExpense { get; set; }
        public string issendforapprovalope { get; set; }

        public string NewExistingCustomer { get; set; }
        public string DomesticInternationVisit { get; set; }
        public string Full_Half_Day { get; set; }

        public DataTable dtUserAttendance = new DataTable();
        public DataTable dtActivityMaster = new DataTable();
        public string AddExpense { get; set; }
        public string CallId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ActivityTypeName { get; set; }
        public string PK_SubJob_Id { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }


        public string CreatedDatetime { get; set; }
        public string Status { get; set; }
        public string modifiedby { get; set; }
        public string op_number { get; set; }
        public string reason { get; set; }
        public string stage { get; set; }
        public string IsSendForApproval { get; set; }
        public string modifieddate { get; set; }
        public string IsSendForAprrovalDate { get; set; }
    }
}