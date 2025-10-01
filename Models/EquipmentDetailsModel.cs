using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class EquipmentDetailsModel
    {
       
            public int PK_DOE_Id { get; set; }
            public string Name_Of_Equipments { get; set; }
            public string Range { get; set; }
            public string Id { get; set; }
            public string CalibrationValid_Till_date { get; set; }
            public string Certification_No_Date { get; set; }
            public string Status { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<int> PK_IVR_ID { get; set; }
            public Nullable<int> PK_CALL_ID { get; set; }
        public Nullable<int> abcid { get; set; }
        public string Type { get; set; }

            public bool NABLseenote1 { get; set; }

            public bool NonNABLseenote2 { get; set; }

        public string SNABLseenote1 { get; set; }

        public string SNonNABLseenote2 { get; set; }

    }
}