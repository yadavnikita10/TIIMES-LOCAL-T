using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class LessionLearntModel
    {
        public String Uniquenumber { get; set; }
        public string From { get; set; }
        public string Dicription { get; set; }
        public string DicriptionView { get; set; }
        public string RCA { get; set; }
        public string LessonsLearnt  { get; set; }
        public string LessonsLearntview { get; set; }
        public string Sharedon { get; set; }
        public string Declaration { get; set; }
        public int LessionLeantID { get; set; }
        public System.Data.DataTable dtLessionLeant { get; set; }
        public string Name  { get; set; }
        public string EmployeeCode { get; set; }
        public string Branch { get; set; }
        public string Tuv_Email_Id { get; set; }

        public string MobileNo { get; set; }
        public string RoleName { get; set; }
        public string Viewedon { get; set; }
       public List<LessionLearntModel> LstLessionLearntModel { get; set; }
        public string StrLessionLeantID { get; set; }


    }
}