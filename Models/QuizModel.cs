using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class QuizModel
    {
        public List<QuizModel> lstFeedback { get; set; }

        public List<QuizModel> lstHeaderQuetion { get; set; }
        public List<QuizModel> lstFooterAnswer { get; set; }

        public int Count { get; set; }
        public int QuizID { get; set; }
        public int TopicName { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Answer { get; set; }
        public int TrainingID { get; set; }
        public string UserID { get; set; }
        public decimal Percent { get; set; }
        public List<QuizQuestion> QuizQuestion { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string E1 { get; set; }
        public string F1 { get; set; }
        public string G1 { get; set; }
        public string PK_TrainingScheduleId { get; set; }
        public string QuizeEndDate { get; set; }
        public string QuizeEndTime1 { get; set; }
        public string QuizStartDate { get; set; }
        public string Branch_Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }


        public int QuestionId { get; set; }
        public string ReattendQuizStartDate { get; set; }
        public string ReattendQuizEndDate { get; set; }
        public string ReattendQuizTotalQuestion { get; set; }
        public string Attempt { get; set; }
        public string QTimeInMinutes { get; set; }
    }
    public class QuizQuestion
    {
        public int TQuizID { get; set; }
        public int TrainingID { get; set; }
        public string TQuestion { get; set; }
        public string TOption1 { get; set; }
        public string TOption2 { get; set; }
        public string TOption3 { get; set; }
        public string TOption4 { get; set; }
    }
    public class TopicList
    {
        public int TopicID { get; set; }
        public string Name { get; set; }
    }
}