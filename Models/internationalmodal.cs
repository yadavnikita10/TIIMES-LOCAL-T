using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class internationalmodal
    {

        public class UserData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Marital_Status { get; set; }
            public string DOB { get; set; }
            public string MiddleName { get; set; } //added by nikita on 26092023
            public string Nationality { get; set; }
            public string Location { get; set; }
            public string Inspection { get; set; }
            public string ExpertiseSummary { get; set; }
            public string Gender { get; set; }
            public string Agency { get; set; }
            public string Image { get; set; }
            public string TotalyearofExprience { get; set; }
            public string TUVTotalyearofExperience { get; set; }
            public string DateOfJoining { get; set; }
            public string Language { get; set; }
        }
        public class Education_
        {
            public string Course { get; set; }
            public string Degree { get; set; }
            public string FOS { get; set; }
            public string University { get; set; }
            public string LastYearCGPA { get; set; }
            public string Aggregate { get; set; }
            public string Year { get; set; }

        }
        public class Proffestional_
        {
            public string certification { get; set; }
            public string certificationNO { get; set; }
            public string issuedate { get; set; }
            public string validtill { get; set; }
            //public string LastYearCGPA { get; set; }
            //public string Aggregate { get; set; }
            //public string Year { get; set; }

        }

        public class EmployeementDetails_
        {
            public string periodfrom { get; set; }
            public string employeername { get; set; }
            public string location { get; set; }
            public string designation { get; set; }
            public string Responsibilities { get; set; }
            //public string LastYearCGPA { get; set; }
            //public string Aggregate { get; set; }
            //public string Year { get; set; }

        }

        public class ProjectDetails_
        {
            public string ItemDetails { get; set; }
            public string code { get; set; }
            public string Endcustomer { get; set; }
            public string manufacturername { get; set; }
        }
        public class TrainingDetails_
        {
            public string date { get; set; }
            public string hours { get; set; }
            public string topic { get; set; }
        }

        public class AchievementDetails_
        {
            public string archievementdate { get; set; }
            public string description { get; set; }
        }
        public class GetData
        {
            public List<Education_> Education { get; set; }
            public List<Proffestional_> Proffestional { get; set; }
            public Users GetUserDetails { get; set; }
            public List<EmployeementDetails_> EmployeementDetails { get; set; }
            public List<ProjectDetails_> ProjectDetails { get; set; }
            public List<TrainingDetails_> TrainingDetails { get; set; }
            public List<AchievementDetails_> AchievementDetails { get; set; }
        }

        //public class UserData
        //{
        //    public internationalmodal requestData { get; set; }
        //}
        //public class ExternalCVRequest
        //{
        //    public List<GetData> detailedData { get; set; }
        //    public Users externalcv { get; set; }
        //}
    }
}