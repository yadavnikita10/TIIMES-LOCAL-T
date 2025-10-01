using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CompentencyMetrixView
    {
        public string SurveyorId { get; set; }
       public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Candidate Name")]
        public string CandidateName { get; set; }

        public string CandidateId { get; set; }
       public string Location                    { get; set; }
       public string EducationalQualification  { get; set; }
        public string AdditionalQualification   { get; set; }
       public string Designation                 { get; set; }
       public string EmailId                     { get; set; }
       public string CellPhoneNumber             { get; set; }
        //public DateTime JoiningDate                 { get; set; }
        public DateTime JoiningDate { get; set; }
        public string JoiningDt { get; set; }
        public string TotalExperienceInYears      { get; set; }
       public string NumberOfYearsWithTUVIndia { get; set; }

        public int CompentencyMetrixMasterId { get; set; }

        public String CompentencyMetrixMasterName { get; set; }

        public String CheckBoxValue { get; set; }

        public string TotalTUVExperience { get; set; }
        public string JoiningDate1 { get; set; }

        public string OverAllExp { get; set; }

        public string Success { get; set; }

        public string PK_userId { get; set; }
        public string TiimesRole { get; set; }
        public string EmployeeCategory { get; set; }
        public string StampNumber { get; set; }
        public string AutharizeLevel { get; set; }
        public string strFormFilled { get; set; }
        public string ProfileDataCompleted { get; set; }
        public string OBSName { get; set; }
        public string TechniqalCompetencyVerify { get; set; }

        public string CreatedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovalStatus { get; set; }
        public string BranchID { get; set; }
        public string CoreFieldOfStudy { get; set; }
        public string FormFilled { get; set; }
        public string CompetentInScope { get; set; }
    }
}