using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ReportImageModel
    {
       
            public int PK_IP_Id { get; set; }
            public string Heading { get; set; }
            public string Image { get; set; }
            public string Status { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<int> PK_IVR_ID { get; set; }
            public Nullable<int> PK_CALL_ID { get; set; }
        public Nullable<int> abcid { get; set; }
        public string Path { get; set; }

        public string Type { get; set; }
        public bool chkbox { get; set; }
        public List<ReportImageModel> Activity { get; set; }
    }
}