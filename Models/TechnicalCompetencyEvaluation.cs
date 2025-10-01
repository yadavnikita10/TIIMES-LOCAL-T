using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class TechnicalCompetencyEvaluation
    {
        public List<TechnicalCompetencyEvaluation> lmd1 { get; set; }
        public int Count { get; set; }
        public int PK_TechnicalCompetencyEvaluation { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        public int FK_RangeInspectionId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        public string FK_RangeInspectionName { get; set; }
        public string EmployeeCode { get; set; }//added by nikita on 11092023

        public string[] AFK_RangeInspectionName { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Autharization Level")]
        public string AutharizationLevel { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Basic Authorization")]
        public string BasicAuthorization { get; set; }
        public string InspectorName { get; set; }

        public string InspectorFullName { get; set; }
        public string InspectorId { get; set; }
        public string InspectorBranch { get; set; }
        public string EfectiveDate { get; set; }
        public string RevisionDate { get; set; }
        public string ReportNo { get; set; }

        public List<RangeOfInspectionList> RangeOfInspectionList { get; set; }
        public string FullName { get; set; }
        public string Signature1 { get; set; }
        public string CreatedBy { get; set; }
        //public Nullable<DateTime> CreatedDate { get; set; }

        public string CreatedDate { get; set; }
        public string DDLTest { get; set; }

        //public DateTime ModifiedDate { get; set; }

        public String ModifiedDate { get; set; }
        public String ModifiedBy { get; set; }

        public String Signature { get; set; }

        public String AdminDesignation { get; set; }

        public String InspectorDesignation { get; set; }

        public string Branch { get; set; }
        public string TiimesRole { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string EmployeeCategory { get; set; }
        public bool isVerified { get; set; }

        public bool isFormFilled { get; set; }
        public string FormFilled { get; set; }
        public string Verified { get; set; }

        //public string Createdby { get; set; }
        public byte[] CreatedSignature { get; set; }

        public string CreatedSignature1 { get; set; }

        public string CreatedDesignation { get; set; }

        public string  ReportingPerson1Name { get; set; }
        //public byte[] ReportingPerson1Signature { get; set; }
        public string ReportingPerson1Signature { get; set; }

        public string ReportingPerson1Designation { get; set; }

        public string VerifiedDateTime { get; set; }

        public string SelectedRadioButton { get; set; }

        public string BasicAuthText { get; set; }

        public string BasicAuthValue { get; set; }

        public string SkillID { get; set; }

        public string PCH { get; set; }
        public string Remarks { get; set; }
    }

    public class Report
    {
       public int PK_IAFScopeId { get; set; }
        public string IAFScopeName { get; set; }
        public string IAFScope { get; set; }

        public int PK_FieldInspection { get; set; }

        public string InspectionName { get; set; }


        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string Signature { get; set; }
        public string RangeInspection { get; set; }

        public int PK_RangeInspectionId { get; set; }
        public string AuthorizationLevel { get; set; }
        public string BasisOfAuthorization { get; set; }
        public string MinimumEducationQua { get; set; }
        public string MinimumRequirmentForLevel3 { get; set; }


    }



        public class EmpName
    {
        public string Name { get; set; }
        public string Code { get; set; }
        //public SelectList SectionModel { get; set; }
    }

   public class RangeOfInspectionList
    {
        public string LFK_RangeInspectionName { get; set; }
        public int    LFK_RangeInspectionId { get; set; }

        public string LIAFScopeNumber { get; set; }

        public string LAutharizationLevel { get; set; }

        public string LAutharizationLevelId { get; set; }
        public string LBasicAuthorization { get; set; }

        public string LisVerified { get; set; }

        // public string[] LBasicAuthorizationName { get; set; }

        //  public List<ListOfData> LBasicAuthorizationName { get; set; }

        public string LBasicAuthorizationName { get; set; }
        public string LInspectorName { get; set; }

        public string LIAFScopeName { get; set; }
        public string LFieldOfInspection { get; set; }

        public List<string> ListBasicAuthorizationName { get; set; }

        public string PK_TechnicalCompetencyEvaluation { get; set; }

        public string MinimumEducationQualification { get; set; }

        public string MinimumRequirementForLevel3 { get; set; }

        public string skillID { get; set; }
        public string Remarks { get; set; }

    }

}