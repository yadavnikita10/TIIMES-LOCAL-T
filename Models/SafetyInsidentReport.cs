using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class SafetyInsidentReport
    {
        public int PKId { get; set; }
        public string TiimesUIN { get; set; }
        public string Branch { get; set; }
        public string DateofReport { get; set; }
        public string DateOfIncident { get; set; }
        public string TypeOfIncident { get; set; }
        public string NameOfInjuredPerson { get; set; }
        public string IPHomeAddress { get; set; }
        public string LocationofIncident { get; set; }
        public string TypeOfInjury { get; set; }
        public string MedicalTreatmentDetails { get; set; }
        public string DescriptionOfIncident { get; set; }
        public string RootCauseAnalysis { get; set; }
        public string Correction { get; set; }
        public string CorrectiveAction { get; set; }
        public string MandaysLost { get; set; }
        public bool RiskAndOpportunities { get; set; }
        public bool AIHIRAReviewed { get; set; }
        public bool ShareLessonsLearnt { get; set; }
        public string Status { get; set; }
        public string FormFilledBy { get; set; }
        public string CreationDate { get; set; }
        public string Modifiedby { get; set; }
        public string ModificationDate { get; set; }

        public string SRiskAndOpportunities { get; set; }
        public string SAIHIRAReviewed { get; set; }
        public string SShareLessonsLearnt { get; set; }
        public string FileType { get; set; }
        public virtual ICollection<FileDetails> IncidentPhotographs { get; set; }
        public virtual ICollection<FileDetails> LessonsLearntPhotographs { get; set; }
        public virtual ICollection<FileDetails> OtherAttachment { get; set; }
        public string LessonsLearned { get; set; }
    }
}