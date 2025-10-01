using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TuvVision.Models
{
    public class CompanyMaster
    {
        public List<CompanyMaster> lstCompanyMaster1 { get; set; }
        public List<CompanyMaster> ContactDashBoard { get; set; }
        public List<CompanyMaster> ListDashboard { get; set; }
        public int CMP_ID { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Contact Name")]
        [Display(Name = "Contact Name")]
        public string Contact { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Company Name")]
        [Display(Name = "Company Name")]
        public string Company_Name { get; set; }

        public string companyId { get; set; } /// <summary>
                                              /// //Company With ID
                                              /// </summary>


        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]

        public string Email { get; set; }
        //[Required]
        public string Website { get; set; }
        //[MaxLength(10), MinLength(10)]
        //[Required(ErrorMessage = "Please enter Landline No")]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Work Phone must be numeric")]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Office Number.")]
        public string Work_Phone { get; set; }
        public string Type { get; set; }
        public string Sub_Type { get; set; }
        //public int Title { get; set; }
        public string Title { get; set; }
        //[Required]
        public string Home_Page { get; set; }

        //[Required(ErrorMessage = "Pan Number is required.")]
        [Display(Name = "Pan_No")]
        //[DataType(DataType.PhoneNumber)]
        [MaxLength(10), MinLength(10)]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}$", ErrorMessage = "Invalid Pan Number.")]
        public string Pan_No { get; set; }
        [Required]
        public string Address { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        //[Display(Name = "Mobile Number")]
        ////[DataType(DataType.PhoneNumber)]
        //[MaxLength(10), MinLength(10)]
        //[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]

        public string Mobile { get; set; }
        //[Required]
        public string Fax_No { get; set; }

        ////     [Required(ErrorMessage = "Site Address is required.")]
        public string Address_Account { get; set; }

        public string Main { get; set; }
        public int Ind_ID { get; set; }
        [Required]
        public string Fax_Account { get; set; }
        //[Required]
        public string Branch_Description { get; set; }
        public int Br_Id { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public int CG_ID { get; set; }

        public string CorporateName { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string createDate { get; set; }
        public DateTime? Modified_Date { get; set; }
        public string ModifiedBy { get; set; }
        public string DeleteStatus { get; set; }
        [Required]
        public string BranchName { get; set; }

        public int PK_ContID { get; set; }

        public string HomePhone { get; set; }
        public bool IsMainContact { get; set; }

        public string IsMainContactString { get; set; }
        public string PrimaryContact { get; set; }

        [Required(ErrorMessage = "Please enter Designation")]
        public string TitleName { get; set; }

        [Required(ErrorMessage = "Please enter Contact Name")]
        public string ContactName { get; set; }


        [Required]
        public string CompanyName { get; set; }


        public string FK_CMP_ID { get; set; }

        public static List<CompanyMaster> lstUser { get; set; }
        public string OtherCorporateName { get; set; }

        public string ContactStatus { get; set; }

        public string IndustryName { get; set; }


        public string OffAddrPin { get; set; }


        public string SiteAddrPin { get; set; }

        [Required(ErrorMessage = "Please Select Location")]
        public string InspectionLocation { get; set; }

        public virtual ICollection<CompanyMaster> lstSiteAddress { get; set; }

        public string SiteID { get; set; }

        public string AddressType { get; set; }

        public string SapNo { get; set; }

        public int New_Cmp { get; set; }

        public string CustomerSearch { get; set; }

        public int vendorid { get; set; }


    }
}