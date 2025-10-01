using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class ComplaitRegister
    {
        public List<ComplaitRegister> lstComplaintDashBoard1 { get; set; }
        public List<ComplaitRegister> lstByVendor { get; set; }
        public List<ComplaitRegister> lstByItem { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public int PK_Complaint_ID { get; set; }

        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Enter Attachment"), MaxLength(30)]
        public int Br_id { get; set; }
        public string Attachment { get; set; }
        [DataType(DataType.Text)]
       // [Required(ErrorMessage = "Please Enter Complaint No")]
        public string Complaint_No { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "Please Enter Complaint_Date")]
        public string Complaint_Date { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Control No")]
        public string Control_No { get; set; }
        [DataType(DataType.Text)]
       // [Required(ErrorMessage = "Please Enter Complaint Mode")]
        public string Complaint_Mode { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter TUVI Client")]
        public string TUV_Client { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Originating Branch")]
        public string Originating_Branch { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Executing Branch")]
        public string Executing_Branch { get; set; }
        [DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Enter Complaint_Date")]
        public string BranchForInspector { get; set; }
        public string Inspector_Name { get; set; }
        public string PKUserId { get; set; }
        public string allInspector_Name { get; set; }
    
        public string Complaint_Details { get; set; }
        [DataType(DataType.Text)]
       // [Required(ErrorMessage = "Please Enter Complaint Details")]
        public string Correction { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Root_Cause")]
        public string Root_Cause { get; set; }
        [DataType(DataType.Text)]
       // [Required(ErrorMessage = "Please Enter CA To Prevent Recurrance")]
        public string CA_To_Prevent_Recurrance { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Effectiveness Of Implementation Of CA")]
        public string Effectiveness_Of_Implementation_Of_CA { get; set; }
        //[DataType(DataType.Text)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
      //  [Required(ErrorMessage = "Please Enter Date Of Aknowledgement")]

        public string Date_Of_Aknowledgement { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
       // [Required(ErrorMessage = "Please Enter Date Of PreliminaryReply")]
        public string Date_Of_PreliminaryReply { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
       // [Required(ErrorMessage = "Please Enter Date Of FinalReply")]
        public string Date_Of_FinalReply { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
       // [Required(ErrorMessage = "Please Enter Date Of Action")]
        public string Date_Of_Action { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Category")]
        public string Category { get; set; }
        [DataType(DataType.Text)]
      //  [Required(ErrorMessage = "Please Enter Remarks"),]
        public string Remarks { get; set; }

        public string EndUser { get; set; }

        public string States_Of_Complaints { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedDate { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        public string LastDateOfInspection { get; set; }

        public string AttributeToFaultiInspection { get; set; }
        public string Vendor { get; set; }
        public string SubVendor { get; set; }

        public string LessonLearned { get; set; }

        public string ActionTaken { get; set; }

        public string ProductList { get; set; }

        public string ProjectName { get; set; }
        public string CompanyName { get; set; }

        public string ComplaintCategoryId { get; set; }
        public string ComplaintCategoryName { get; set; }

        public string VendorPO { get; set; }
        public string SubVendorVendorPO { get; set; }
        public string Call_no { get; set; }
        public string ReferenceDocument { get; set; }

        public virtual ICollection<FileDetails> ATT1 { get; set; }
        public virtual ICollection<FileDetails> ATT2 { get; set; }
        public virtual ICollection<FileDetails> ATT3 { get; set; }
        public virtual ICollection<FileDetails> ATT4 { get; set; }

        public bool ShareLessonsLearnt { get; set; }
        public String SShareLessonsLearnt { get; set; }

        public bool Hide { get; set; }
        public string showData1 { get; set; }
        public string CloseMailSent { get; set; }
        public string UserBranch { get; set; }

    }
    public class NameCodeins
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public String  PkUserId { get; set; }
        public SelectList SectionModel { get; set; }
    }

  


}