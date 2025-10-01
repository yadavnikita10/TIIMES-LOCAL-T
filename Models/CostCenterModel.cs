using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CostCenterModel
    {
        public int Pk_CC_Id { get; set; }
        public string Cost_Center { get; set; }
        public string profit_Ctr { get; set; }
        public string Branch { get; set; }
        public string Branch_Group { get; set; }
        public string Product_Group { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string DocName { get; set; }
        public int PK_id { get; set; }
    }

    public class ConclusionMaster
    {
        public int Id { get; set; }
        public string Conclusion { get; set; }

    }

    public class Nationality
    {
        public int Nationality_ID { get; set; }
        public string Nationality_ { get; set; }

    }
}