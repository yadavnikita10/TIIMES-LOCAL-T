using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ItemDescriptionModel
    {
        
            public int PK_ItemD_Id { get; set; }
            public int abcid { get; set; }
            public string Po_Item_No { get; set; }
            public string ItemCode_Description { get; set; }
            public string Po_Quantity { get; set; }
            public string Offered_Quantity { get; set; }
            public Nullable<int> PK_IVR_ID { get; set; }
            public Nullable<System.DateTime> Created_date { get; set; }
            public string Created_By { get; set; }
            public string Status { get; set; }
            public string Type { get; set; }
            public Nullable<int> PK_CALL_ID { get; set; }

            public string Item_Code { get; set; }
            public string Accepted_Quantity { get; set; }
            public string Cumulative_Accepted_Qty { get; set; }
            public string Unit { get; set; }
            public Boolean POTotalCheckBox { get; set; }
            public string Report_No { get; set; }
        public string HeatNumber { get; set; }
        public string TotalQuantity { get; set; }
        public string HeatNoShow { get; set; }

    }
}