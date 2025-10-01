using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class MonitorRecordData
    {
        public List<MonitorRecordData> listingmonitoringrecord { get; set; }

        //added by nikita 
        public int Count { get; set; }

        public int Mentoring_Count { get; set; }

        public int Monitoring_of_monitors_Count { get; set; }

        public int Offsite_Monitoring_Count { get; set; }

        public int Onsite_Monitoring_Count { get; set; }


        public string Tuv_Email_Id { get; set; }

        public string EmployeeName { get; set; }

        public string Brach_Name { get; set; }

        public string MobileNo { get; set; }

        public string Designation { get; set; }

        public string IsMentor { get; set; }

        public string Mentoring { get; set; }

        public string OffsiteMonitoring { get; set; }

        public string OnsiteMonitoring { get; set; }

        public string MonitoringOfmonitors { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }


    }
}