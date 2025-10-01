using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class BranchMasters
    {
        public List<BranchMasters> lstCompanyDashBoard1 { get; set; }
        public int Br_Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Branch Name")]
        [Display(Name = "Branch Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Only alphabets allowed.")]
        //[Remote("doesBranchNameExist", "Branch", AdditionalFields = "Br_Id", HttpMethod = "Get", ErrorMessage = "This Branch name already exists.")]
        public string Branch_Name { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Branch Code")]
        [Display(Name = "Branch Code")]
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed.")]
        //[Remote("doesBranchCodeExist", "Branch", AdditionalFields = "Br_Id", HttpMethod = "Get", ErrorMessage = "This Branch code already exists.")]
        public string Branch_Code { get; set; }
        public string Manager { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Service Code")]
        [Display(Name = "Company Name")]
        public string Service_Code { get; set; }
        public string Sequence_Number { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Address")]
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Country")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Only Alphabets allowed.")]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter State Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Only Alphabets allowed.")]
        [Display(Name = "State")]
        public string State { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Postal Code must be numeric")]
        public string Postal_Code { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email_Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Status { get; set; }
        public string DeleteStatus { get; set; }
        public string CreatedBy { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter City Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Only Alphabets allowed.")]
        [Display(Name = "City Name")]
        public string CityName { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Branch Admin"), MaxLength(30)]
        [Display(Name = "Branch Admin")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Only Alphabets allowed.")]
        public string BranchAdmin { get; set; }
        public string Domesticinter { get; set; }
        public string Attachment { get; set; }
        public string Coordinator_Email_Id { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

    }

    public class CallReasonMaster
    {
        public List<CallReasonMaster> lstCompanyDashBoard1 { get; set; }
        public int CReason_Id { get; set; }
        public string Reason { get; set; }
    }
}