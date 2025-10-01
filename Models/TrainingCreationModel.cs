using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class TrainingCreationModel
    {
        public int Id {get;set;}

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Training Topic")]
        public string TrainingTopic { get; set; }

        //public List<TrainingTopic> d { get; set; }

       
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Evaluation Method")]
        public string EvaluationMethod { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Branch ")]
        public int BranchId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Branch Name")]
        public string BranchName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Category")]
        public string Category { get; set; }
        public string Other { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Training Type")]
        public string TrainType { get; set; }
        public string Remarks { get; set; }

        public string CreatedBy { get; set; }
        public string ProposedDate { get; set; }
        public string CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDateTime { get; set; }
        public List<TrainingTopic> n { get; set; }
    }

    public class BranchName
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }


    }
    public class TrainingTopic
    {
        public string lTopicName { get; set; }
        
    }
}