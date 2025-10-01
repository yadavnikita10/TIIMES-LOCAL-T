using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ReferenceDocumentsModel
    {
       
            public int PK_RD_ID { get; set; }
            public int abcid { get; set; }
            public string Document_Name { get; set; }
            public string Document_No { get; set; }
            public string Approval_Status { get; set; }
            public Nullable<int> PK_IVR_ID { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public string Status { get; set; }
            public Nullable<int> PK_CALL_ID { get; set; }
           public string Type { get; set; }

             public string VendorDocumentNumber { get; set; }

				 public List<ReferenceDocumentsModel> RD { get; set; }

        public string Visible { get; set; }											 
    }
}