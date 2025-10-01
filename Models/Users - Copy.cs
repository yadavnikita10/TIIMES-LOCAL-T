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
        public List<Users> lstEducationQualification { get; set; }
        public List<Users> lstPQ { get; set; }
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
        public string SAPEmployeeCode { get; set; }
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
        //[Range(0, 6, ErrorMessage = "Please enter valid integer Number")]
        public string ZipCode { get; set; }
        /////public int FK_BranchID { get; set; }
        public string FK_BranchID { get; set; }
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
        public int EmployeeGradeIdOld { get; set; }
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
        public string MiddleName { get; set; }
        public string TUVUIN { get; set; }


        public string PermanantPin { get; set; }
        public string TUVIStampNo { get; set; }
        public string OPE { get; set; }

        public int PKUserRoleId { get; set; }
        public string PKId { get; set; }

        public virtual ICollection<Users> UserAttachment { get; set; }

        public string UserPrimaryKey { get; set; }
        public string ReasonForUpdate { get; set; }
        public string EmployementCategory { get; set; }

        public string Course { get; set; }
        public string Degree { get; set; }
        public string MajorFieldOfStudy { get; set; }
        //public string University { get; set; }
        public string OtherUniversity { get; set; }



        public int University { get; set; }
        public string UniversityName { get; set; }

        public string LastYearCGPA { get; set; }
        public string AggregateCGPA { get; set; }
        //public Array arrEducationQualification { get; set; }
        public List<string> arrEducationQualification { get; set; }
        //public List<Users> arrEducationQualification { get; set; }
        public int DocType { get; set; }
        public int PK_ID { get; set; }
        public string FileName { get; set; }

        public string IDS { get; set; }
        public string AttType { get; set; }
        public string CurrentAssignment { get; set; }
        public string SiteDetail { get; set; }

        public string BranchName { get; set; }
        public string DBrID { get; set; }
        public string OrgChangeDate { get; set; }
        public string CostCenterName { get; set; }
        public string LockedStatus { get; set; }
        public string Status { get; set; }
        public int LoginUserRoleId { get; set; }
        public List<Users> lstUserHistory { get; set; }
        public System.Data.DataTable dtUserHistory { get; set; }

        public string CertName { get; set; }
        public string CertNo { get; set; }
        public string CertIssueDate { get; set; }
        public string CertValidTill { get; set; }
        public List<Users> lstProfCertificates { get; set; }
        public byte[] IData { get; set; }
        public byte[] SignatureData { get; set; }
        public string ExperienceInMonth { get; set; }
        public string DynamicCalTotalYearExp { get; set; }
        public string ItemToBeInspected { get; set; }
        public string Year { get; set; }
        public string YearName { get; set; }
        public virtual ICollection<Users> EyeTestReport { get; set; }
        public string After7Days { get; set; }
        public string YearOfPassing { get; set; }
        public string YearOfPassingName { get; set; }

        public string AgeCalculator { get; set; }

        public int intYearOfPassing { get; set; }
        public bool isVerified { get; set; }
        public string PFUANNumber { get; set; }

        public string StrIData { get; set; }
        public string StrSignatureData { get; set; }


        public List<Users> lstCallDashBoard1 { get; set; }
        public String MentorName { get; set; }

        public int Id { get; set; }

        public string MentorListName { get; set; }

        public string ManteeName { get; set; }

        public string IsMentor { get; set; }

      

        public string CreatedBy1 { get; set; }

        public DateTime? CreatedDate1 { get; set; }

        public string ModifiedBy1 { get; set; }

        public DateTime? ModifiedDate1 { get; set; }
        public string BrAuditee { get; set; }
        public List<Users> LeaveDashBoard { get; set; }
        //added by shrutika salve 10/08/2023
        public string MainBranch { get; set; }
        public string OBSTYPE { get; set; }
        public string SitePin { get; set; }
        public string Orgdescription { get; set; }

    }
    public enum EmployeeGrade
    {
        M1 = 1,
        M2 = 2,
        M3 = 3,
        M4 = 4,
        M5 = 5,
        M6 = 6,
        M7 = 7,
        EmpanelledEmployee = 8,
        SpecialContractEmployee = 9
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
        public string SAPEmployeeCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool IsActive { get; set; }
        public string FK_RoleID { get; set; }
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
        //// public int FK_BranchID { get; set; }
        public string FK_BranchID { get; set; }
        public string Employee_Type { get; set; }
        public string DeleteStatus { get; set; }
        public string Signature { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string FilePath { get; set; }

        public string UserRole { get; set; }

        public string Branch { get; set; }




        public string EmployementCategoryCode { get; set; } //added by nikita on 26092023
        public string ReasonForUpdate { get; set; } //added by nikita on 26092023
        public string OrgChangeDate { get; set; } //added by nikita on 26092023
        public string MiddleName { get; set; } //added by nikita on 26092023
        public string Reporting_Person_One { get; set; } //added by nikita on 26092023
        public string PFUANNumber { get; set; } //added by nikita on 26092023
        public string IsLocked { get; set; }//added by nikita on 26092023
        public string Cost_center { get; set; }//added by nikita on 26092023


        public string Email { get; set; } //added by nikita on 26092023
        public string Dateofjoining { get; set; } //added by nikita on 26092023


        public string SAP_Emp_code { get; set; } //added by nikita on 26092023
        public string OPE { get; set; } //added by nikita on 26092023
        public string PortfolioAccess { get; set; }//added by nikita on 26092023
        public string ResidenceAddress { get; set; }//added by nikita on 26092023
        public string PermanentAddress { get; set; }//added by nikita on 26092023

        public string Course { get; set; }//added by nikita on 26092023
        public string MajorFieldOfStudy { get; set; }//added by nikita on 26092023
        public string University { get; set; }//added by nikita on 26092023
        public string OtherUniversity { get; set; }//added by nikita on 26092023
        public string LastYearPerc { get; set; }//added by nikita on 26092023
        public string Degree { get; set; }//added by nikita on 26092023
        public string YearOfPassing { get; set; }//added by nikita on 26092023
        public string AggregatePerc { get; set; }//added by nikita on 26092023
        public string UploadedDocument { get; set; }//added by nikita on 26092023
        public string FileName { get; set; }//added by nikita on 26092023
        public string CertIssueDate { get; set; }//added by nikita on 26092023
        public string CertValidDate { get; set; }//added by nikita on 26092023
        public string certNo { get; set; }//added by nikita on 26092023
     //public string FK_RoleID { get; set; }//added by nikita on 26092023
        public string Fk_UserId { get; set; } //added by nikita on 26092023


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

        [MaxLength(10), MinLength(10)]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}$", ErrorMessage = "Invalid Pan Number.")]
        public string PanNo { get; set; }

        [MaxLength(12), MinLength(12)]
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
        public string TUVIStampNo { get; set; }

        public string EduQualAtta { get; set; }
        public string ProfQualAtta { get; set; }


        public string EmployementCategory { get; set; }
        public string ExperienceInMonth { get; set; }

        public string ProfCertificates { get; set; }
        public string Attachments { get; set; }
        public List<ProfileUser> LstUserData { get; set; }

        public string Photo { get; set; }
        public string PAN { get; set; }
        public string ETR { get; set; }
        public string FMR { get; set; }

        public string Age { get; set; }
        public string ExpBeforeTUV { get; set; }
        public string currentAssignment { get; set; }

        public System.Data.DataTable dtUserData { get; set; }
        public string Verified { get; set; }
        public string strModifiedDate { get; set; }
        public string ActiveStatus { get; set; }

        public string EncryptedPassword { get; set; }
        public string DecryptedPassword { get; set; }


        //shrutika salve 11/08/2023
        //public string EducationalQualificationDetails { get; set; }
        public string ProffesionalQualificationDetails { get; set; }
        public string UserAttachment1 { get; set; }
        public string EyeTest { get; set; }
        public string MainBranch { get; set; }
        public string OBSTYPE { get; set; }
        public string SitePin { get; set; }
        public string SiteAddrPin { get; set; }
        public string PerAddrPin { get; set; }
        //public string EmployementCategoryCode { get; set; }

        //added by shrutika salve 07122023
        public string PerformancePercentage { get; set; }

        public string profileDetails { get; set; }
        public string EducationDetails { get; set; }
        

    }
    public enum UserType
    {
        SuperAdmin = 1,
        Admin = 2,
        BranchAdmin = 3
    }
   // public class mantor { 
  // public List<Mantor> lstEducationQualification { get; set; }
   // public string PK_UserID { get; set; }

   // public int ID { get; set; }

   // }

}