using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class DocumentRevieweModel
    {
       
            public int PK_DR_ID { get; set; }
            public int abcid { get; set; }
            public string Description { get; set; }
            public Nullable<int> PK_IVR_ID { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string Status { get; set; }
            public Nullable<int> PK_CALL_ID { get; set; }
             public string Type { get; set; }

    }
}