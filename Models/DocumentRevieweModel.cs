using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class DocumentRevieweModel
    {

        public int? PK_DR_ID { get; set; }
        public int abcid { get; set; }
        public string Description { get; set; }
        public Nullable<int> PK_IVR_ID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public Nullable<int> PK_CALL_ID { get; set; }
        public string Type { get; set; }
        //Added by Mohjjam Dange 25112025
        public List<DocumentRevieweModel> DocumentList { get; set; } = new List<DocumentRevieweModel>();
        public string Old_Description { get; set; }
        public int? Sequence_No { get; set; }
        public int? Old_Sequence_No { get; set; }
        public string QAP_Clause_Number { get; set; }
        public string Old_QAP_Clause_Number { get; set; }
        public int PK_RM_ID { get; set; }
        public int IsGrammerCheck { get; set; }
        //End by Mohjjam Dange 25112025

        public bool AddIMGONIRN { get; set; }
    }

}