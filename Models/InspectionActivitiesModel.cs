using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class InspectionActivitiesModel
    {
        public int? PK_IA_ID { get; set; }
        public int abcid { get; set; }
        public string Stages_Witnessed { get; set; }
        public int? PK_IVR_ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
        public int? PK_CALL_ID { get; set; }
        public string Type { get; set; }
        //Added by Mohjjam Dange_18112025 
        public string Original_Stages_Witnessed { get; set; }
        public List<InspectionActivitiesModel> StageList { get; set; } = new List<InspectionActivitiesModel>();
        public int? Sequence_No { get; set; }
        public int? Old_Sequence_No { get; set; }
        public string QAP_Clause_Number { get; set; }
        public string Old_QAP_Clause_Number { get; set; }
        public int PK_RM_ID { get; set; }
        public int IsGrammerCheck { get; set; }
        //End by Mohjjam Dange_18112025 

        public bool AddIMGONIRN { get; set; }


        //  public int PK_IA_ID { get; set; }
        //  public int abcid { get; set; }
        //  public string Stages_Witnessed { get; set; }
        //  public Nullable<int> PK_IVR_ID { get; set; }
        //  public Nullable<System.DateTime> CreatedDate { get; set; }
        //  public string CreatedBy { get; set; }
        //  public string Status { get; set; }
        //  public Nullable<int> PK_CALL_ID { get; set; }
        //public string Type { get; set; }
    }
}