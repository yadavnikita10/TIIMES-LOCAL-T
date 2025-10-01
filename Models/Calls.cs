using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class Calls
    {
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public List<Calls> lstCallDashBoard1 { get; set; }
        public int Count { get; set; }
        public int PK_Call_ID { get;set; }
        [Required]
        public string[] Product_item_Collecton { get; set; }
        [Required]
        public string Product_Name { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public string PO_Date { get; set; }
        [Required]
        public string Last_Updated { get; set; }
        [Required]
        public string Assighned_To { get; set; }
        [Required]
        public string PO_Number { get; set; }
        [Required]

        public string Company_Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public string Status { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Originating Branch Name"), MaxLength(500)]
        [Display(Name = "Originating Branch")]
        public string Originating_Branch { get; set; }
        [Required]
        public string Type { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Contact Name"), MaxLength(500)]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Required]
      //  [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public string Call_received_date { get; set; }
        [Required]
        public string Sub_Type { get; set; }
        public string Job { get; set; }
        [Required]
      //  [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public string Call_Request_Date { get; set; }
        [Required]
        public string Source { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Vendor's Name"), MaxLength(500)]
        [Display(Name = "Vendor Name")]
        public string Vendor_Name { get; set; }
        public string Sub_Job { get; set; }
        /// public Nullable<System.DateTime> Planned_Date { get; set; }
        public string Planned_Date { get; set; }
        [Required]
        public string Urgency { get; set; }
        [Required]
        public bool COmpetancy_Check { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter End Customers Name"), MaxLength(500)]
        [Display(Name = "End Customer")]
        public string End_Customer { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Category"), MaxLength(500)]
        [Display(Name = "Category")]
        public string Category { get; set; }
     
        [Required(ErrorMessage = "Please Enter Sub-Category"), MaxLength(500)]
        [Display(Name = "Sub-Category")]
        public string Sub_Category { get; set; }
        [Required]
        public bool impartiality_check { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Project Name"), MaxLength(500)]
        [Display(Name = "Project Name")]
        public string Project_Name { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Job Location"), MaxLength(500)]
        [Display(Name = "Job Location")]
        public string Job_Location { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Final Inspection"), MaxLength(500)]
        [Display(Name = "Final Inspection")]
        public string Final_Inspection { get; set; }
        [Required]
        public bool IsWeek_Month_Call { get; set; }
        public Nullable<System.DateTime> From_Date { get; set; }
        public Nullable<System.DateTime> To_Date { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Inspectors Name"), MaxLength(500)]
        [Display(Name = "Inspector")]
        public string Inspector { get; set; }
        [DataType(DataType.Text)]
        //////[Required(ErrorMessage = "Please Enter Email"), MaxLength(500)]
        [Display(Name = "Client Email")]
        public string Client_Email { get; set; }
        [DataType(DataType.Text)]
        /////[Required(ErrorMessage = "Please Enter Email"), MaxLength(500)]
        [Display(Name = "Vendor Email")]
        public string Vendor_Email { get; set; }
        [DataType(DataType.Text)]
        /////[Required(ErrorMessage = "Please Enter Branch"), MaxLength(500)]
        [Display(Name = "Tuv Branch")]
        public string Tuv_Branch { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Branch"), MaxLength(500)]
        [Display(Name = "Executing Branch")]
        public string Executing_Branch { get; set; }
     
        [Required(ErrorMessage = "Please Enter Sub-Job")]
      
        public Nullable<int>  Sub_Job_ID { get; set; }

        [Required(ErrorMessage = "Please Enter Job ID")]
        [Display(Name = "Job ID")]
        public Nullable<int> Job_ID { get; set; }
   
        [Required(ErrorMessage = "Please Enter QT ID")]
        [Display(Name = "QT ID")]
        public Nullable<int> QT_ID { get; set; }

        public Nullable<System.DateTime> Create_Date { get; set; }
        [Required]
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Modify_Date { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Modify By")]
        [Display(Name = "Modify By")]
        public string Modify_By { get; set; }
        [Required]
        public string Delete_Status { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter call No"), MaxLength(500)]
        [Display(Name = "call No")]
        public string Call_No { get; set; }
        [Required]
        public string Actual_Visit_Date { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> From_time { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public Nullable<System.DateTime> To_time { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public Nullable<System.DateTime> Call_Type { get; set; }
        public int Product_ID { get; set; }
        public string ProductList { get; set; }

        public string Attachment { get; set; }

        public string VirtualCallStatus { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string UserRole { get; set; }

        public string AssignStatus { get; set; }

    }
    public class NameCodeProduct
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }
    }

    public class NameCodeStage
    {
        public string Text { get; set; }
        public string Value { get; set; }

    }
}