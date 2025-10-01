using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class AppealMaster
    {
        
        public List<AppealMaster> lstAppeal1 { get; set; }
        public int Count { get; set; }
        public int Appeal_ID { get; set; }
        public string Created_by { get; set; }
        public string Attachment { get; set; }
        public string Branch { get; set; }
        public string Created_Date { get; set; }
        public string Modified_By { get; set; }
        public string Modified_Date { get; set; }
        public string Status { get; set; }
        public string Date_of_Appeal { get; set; }
        public string Appeal_Referance_No { get; set; }
        public string Appellant { get; set; }
        public string Details_Of_Appeal { get; set; }
        public string TUV_Control_No { get; set; }
        public string Review_And_Analysis { get; set; }
        public string Disposal_Action { get; set; }
        public string Disposal_By { get; set; }
        public string Date_Of_Disposal { get; set; }
        public string Remarks { get; set; }
        public string DocumentAttached { get; set; }

        public String FromDate { get; set; }

        public string ToDate { get; set; }
        public string Mode_Of_Appeal { get; set; }
        public string Appeal_Date { get; set; }
        public string CompanyName { get; set; }
        public int CMP_ID { get; set; }  
    }
    public class inscode
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }
    }
}