
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;

namespace TuvVision.Models
{
    public class TrainingScheduleModel
    {

        public List<TrainingScheduleModel> lstQuotationMasterDashBoard1 { get; set; }
        public List<TrainingScheduleModel> lstExportQuizeResult { get; set; }
        public List<TrainingScheduleModel> lstTrainingRecord { get; set; }
        
        public int PK_TrainingScheduleId { get; set; }
        public int PK_ATID { get; set; }
        public int FFID { get; set; }
        public string FeedbackFile { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        public int TrainingId { get; set; }

        public string TrainingName { get; set; }

        public string tfid { get; set; }
        public string TrainerId { get; set; }

        [DataType(DataType.Text)]
        
        public string TrainerName { get; set; }

        
        public string TraineeId { get; set; }


        [DataType(DataType.Text)]
       /// [Required(ErrorMessage = "Please Select")]
        public string TraineeName { get; set; }

        public int BranchId { get; set; }

        public string SBranchId { get; set; }

        public string BranchName { get; set; }
        public string ProposedDate { get; set; }
        public string TrainingStartDate { get; set; }
        public string TrainingEndDate { get; set; }
        public string TrainingStartTime { get; set; }
        public string TrainingEndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDateTime { get; set; }

        public string ExternalTrainer { get; set; }

        public string Attachment { get; set; }

        public string TrainingRecordId { get; set; }

        public List<string> Merits { get; set; }

        public bool IsPresent { get; set; }

        public string AccessorName { get; set; }

        public string AccessorId { get; set; }

        public bool RIsPresent { get; set; }

        public string RTraineeId { get; set; }

        public string test { get; set; }

        public List<TrainingRecordList> TrainingRecordList { get; set; }

        public string RAccessorName { get; set; }

        public string RTrainingScheduleId { get; set; }

        public string TraineeIdForRecord { get; set; }
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
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Branch { get; set; }
        public string TotalHours { get; set; }
        public string Venue { get; set; }
        public string Scope { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public bool chkAllEmployee { get; set; }
        public bool checkBoxBranch { get; set; }
        public string QuizeTimeInHours { get; set; }






        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select OBS Type")]
        public string ProjectType { get; set; }

        [DataType(DataType.Text)]
    [Required(ErrorMessage = "Please Select Category")]
        public string Category { get; set; }

        public string EmployementCategory { get; set; }
        public string UserRole { get; set; }

        [DataType(DataType.Text)]
     
        public string TrainType { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Evaluation Method")]
        public string EvaluationMethod { get; set; }

        public string TypeOfEmployee { get; set; }

        [DataType(DataType.Text)]
        
        public string ETrainType { get; set; }
        public string FileName { get; set; }
        public string MobileNo { get; set; }
        public string QuizeEndDate { get; set; }
        public string QuizeStartDate { get; set; }
        public string QuizeEndTime1 { get; set; }
        public string Link { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        public string SubmitCount { get; set; }
        public string Name { get; set; }

        public bool chkVideoSeen { get; set; }
        public DataTable dtActivityMaster = new DataTable();

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string QuizButtonVisibleOnList { get; set; }

        public string TrainingScheduleFor { get; set; }
        public string TrainingAttended { get; set; }
        public string Pass { get; set; }
        public string Failed { get; set; }
        public string Feedback { get; set; }
        public string EmpType { get; set; }


    }

    public class TrainingTime
    {
        
        public string TrainingStartDate { get; set; }
        public string TrainingStartTime { get; set; }
        public string TrainingEndTime { get; set; }
        public string TotalHours { get; set; }
        public string Fk_TrainingSchedule_Id { get; set; }
    }

        public class TrainingRecordList
{
        public string EmpType { get; set; }
        public int RTrainingScheduleId { get; set; }
        public string RTraineeName { get; set; }
        public string RTraineeId { get; set; }
        //public string RecordTraieeId { get; set; }
        //public string RAccessorName { get; set; }
        public string AccessorCode { get; set; }
        public int PK_ID { get; set; }

        public string AccessorN { get; set; }

        public bool RIsPresent { get; set; }

        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
        public string FeedBack { get; set; }
        public string EmployementCategory { get; set; }
        //public int booldata { get; set; }
        

    }


public class AccessorNameCode
{
    public string AccessorN { get; set; }
    public string AccessorCode { get; set; }
    
}


    public class TrainingRecordById
    {
        public int Count { get; set; }
        public List<TrainingRecordById> lstEnquiryMast { get; set; }
        public string TrainingScheduleId { get; set; }
        public string TraineeName { get; set; }
        public string Branch { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string IsPresent { get; set; }

        public string QuizAttended { get; set; }
        public string Score { get; set; }
        public string Result { get; set; }
        public string FeedBack { get; set; }
        public string Attempt { get; set; }
        public string AttendedDate { get; set; }

    }



}