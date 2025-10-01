using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class Users
    {
        public string PK_UserID { get; set; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter First Name"), MaxLength(30)]
        [Display(Name = "First Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed.")]
        public string FirstName { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Last Name"), MaxLength(30)]
        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed.")]
        public string LastName { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter User Name"), MaxLength(30)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string EmailID { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please Select Date Of Joinning Name")]
        [Display(Name = "Date of Joining Name")]
        public string DateOfJoining { get; set; }
        public string Qualification { get; set; }
        [Required]
        public string EmployeeGrade { get; set; }
        public string EmployeeCode { get; set; }
        public string SAP_VendorCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int attempts { get; set; }

        public int FK_RoleID { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Please Select Date Of Birth Name")]
        [Display(Name = "Date of Birth Name")]
        public string DOB { get; set; }
        public string Department { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile No")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
       // [MaxLength(10), MinLength(10)]
        public string MobileNo { get; set; }
        public string ResidenceNo { get; set; }
        public string OfficeNo { get; set; }
        public string Designation { get; set; }
        public string LanguageSpoken { get; set; }
        public string ZipCode { get; set; }
        public int FK_BranchID { get; set; }
        public string Employee_Type { get; set; }
        public string DeleteStatus { get; set; }
        public string Signature { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string FilePath { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select User Role"), MaxLength(30)]
        [Display(Name = "User Role")]
        public string UserRole { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Branch "), MaxLength(30)]
        [Display(Name = "Branch")]
        public string Branch { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public static List<Users> lstUser { get; set; }
        public List<NameCode> lstGroupRoleName { get; set; }
        public List<GroupUserRoles> GroupsRoleName { get; set; }
        //Added by Sagar Panigrahi
        [DataType(DataType.EmailAddress)]
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string TuvEmailId { get; set; }
        [Required]
        public string ReportingOne { get; set; }
        public string ReportingTwo { get; set; }
        public string TransferDate { get; set; }
        public string FullName { get; set; }
        //public EmployeeGrade employeeGrade { get; set; }
        public UserType userType { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        [Display(Name = "Employee Grade")]
        public int EmployeeGradeId { get; set; }
        public string BloodGroup { get; set; }
        public string ShoesSize { get; set; }
        public string ShirtSize { get; set; }
        public int CostCenter { get; set; }

        public string PanNo { get; set; }
        public string AadharNo { get; set; }
        public string TotalyearofExprience { get; set; }
        public string Allergies { get; set; }
        public string Medical_History { get; set; }
        public string Additional_Qualification { get; set; }
        public string EmergencyMobile_No { get; set; }
        public string Salutation { get; set; }

        public string Fax_No { get; set; }
        public string Marital_Status { get; set; }
        public string Image { get; set; }
        public string CV { get; set; }



    }
    public enum EmployeeGrade
    {
        M1=1,
        M2=2,
        M3=3,
        M4=4,
        M5=5,
        M6=6,
        M7=7,
        EmpanelledEmployee=8,
        SpecialContractEmployee=9
    }
    public class GroupUserRoles
    {
        public string Text { get; set; }
        public Int32 Value { get; set; }
        public bool Selected { get; set; }
    }
    public class ProfileUser
    {
        public List<ProfileUser> LstDashboard { get; set; }
        public int Count { get; set; }

        public string FirstName { get; set; }

        public string ImgFilePath { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string EmailID { get; set; }
        ////[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public string DateOfJoining { get; set; }
        public string Qualification { get; set; }
        public string EmployeeGrade { get; set; }


        public string EmployeeCode { get; set; }
        public string SAP_VendorCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool IsActive { get; set; }
        public int FK_RoleID { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
       //// [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public string DOB { get; set; }

        public string Dateofbirth { get; set; }
        public string Department { get; set; }

        public string MobileNo { get; set; }
        public string ResidenceNo { get; set; }
        public string OfficeNo { get; set; }
        public string Designation { get; set; }
        public string LanguageSpoken { get; set; }
        public string ZipCode { get; set; }
        public int FK_BranchID { get; set; }
        public string Employee_Type { get; set; }
        public string DeleteStatus { get; set; }
        public string Signature { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string FilePath { get; set; }

        public string UserRole { get; set; }

        public string Branch { get; set; }





        
        public string PK_UserID { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Flag { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<System.DateTime> RelievingDate { get; set; }
        public string Service_Type { get; set; }
      //  public Nullable<decimal> Cost_center { get; set; }
        public string Image { get; set; }
        public string CV { get; set; }
        public string Reporting2 { get; set; }
        public string Vendor_Code { get; set; }
        public string Fax_No { get; set; }
        public string Marital_Status { get; set; }
        public string BloodGroup { get; set; }
        public string ShoesSize { get; set; }
        public string ShirtSize { get; set; }
        public string PanNo { get; set; }
        public string AadharNo { get; set; }
        public string TotalyearofExprience { get; set; }
        public string Allergies { get; set; }
        public string Medical_History { get; set; }
        public string Additional_Qualification { get; set; }
        public string EmergencyMobile_No { get; set; }
        public string Salutation { get; set; }

        public int Pk_CC_Id { get; set; }
        public string Branch_Group { get; set; }
        public string CostCenter_Id { get; set; }
        public string Cost_Center { get; set; }

        //Added by Sagar Panigrahi
        [DataType(DataType.EmailAddress)]
        [Required]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string TuvEmailId { get; set; }
        [Required]
        public string ReportingOne { get; set; }
        public string ReportingTwo { get; set; }
        public string FullName { get; set; }
    }
    public enum UserType
    {
        SuperAdmin=1,
        Admin=2,
        BranchAdmin=3
    }
}