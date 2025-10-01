using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CustomerFeedback
    {
        public List<CustomerFeedback> lstFeedback1 { get; set; }
        //public List<JobMasters> lstCompanyDashBoard1 { get; set; }
        public int Count { get; set; }
        public int FeedBack_ID { get; set; }
        public string Created_by { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string Created_Date { get; set; }
        public string Status { get; set; }
        public string Name_Of_organisation { get; set; }
        public string Name_Of_Respondent { get; set; }
        public string Desighnation_Of_Respondent { get; set; }
        public string Order_No { get; set; }
        public string Enquiry_response { get; set; }
        public string Quotation_Time_Frame_feedback { get; set; }
        public string Requirement_Understanding { get; set; }
        public string Quotation_information { get; set; }
        public string Quotation_Submit_Response { get; set; }
        public string Email_call_ResponseTime { get; set; }
        public string Requested_Call_schedule { get; set; }
        public string Confirmation_Reception { get; set; }
        public string Change_in_schedule_Response { get; set; }
        public string Communication_satisfaction { get; set; }
        public string Behaiviour_of_Inspector { get; set; }
        public string implementation_of_safety_requirements_Of_Inspector { get; set; }
        public string quality_of_inspection { get; set; }
        public string efficiency_with_time { get; set; }
        public string Maintanance_Of_confidentiality_and_Integrity { get; set; }
        public string inspection_report_or_Releasenote_Time { get; set; }
        public string Expectation_Meet { get; set; }
        public string report_for_number_of_errors { get; set; }
        public string association_with_TUV_India { get; set; }
        public string Suggestions { get; set; }
        public string Score_Achieved { get; set; }
        public string Score_percentage { get; set; }
        public string Client_Location { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }


        public int ScoreAchieved { get; set; }

        public int ScorePercentage { get; set; }

        public string Address_of_Your_Organisation { get;set;}
        public string Any_Further_Expectations_from_us { get; set; }

        public string Name { get; set; }
        public string Branch { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FeedBack { get; set; }
        public string MobileNo { get; set; }
        public string RTraineeName { get; set; }


    }
}