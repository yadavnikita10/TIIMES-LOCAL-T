using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class RangeInspectionMaster
    {
        public List<RangeInspectionMaster> lmd1 { get; set; }
        public int Count { get; set; }
        public int PK_RangeInspectionId { get; set; }
       public string RangeInspection { get; set; }
       public string FK_FieldInspection { get; set; }

       public string FK_FieldInspectionName { get; set; }
        public string MinimumEducationQua { get; set; }
        public string MinimumRequirmentForLevel3 { get; set; }
    }
}