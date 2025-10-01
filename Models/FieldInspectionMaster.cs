using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class FieldInspectionMaster
    {
        public List<FieldInspectionMaster> lmd1 { get; set; }
        public int Count { get; set; }
        public int PK_FieldInspection { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select InspectionName")]
        public string InspectionName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select IAF Scope Id")]
        public string FK_IAFScopeId { get; set; }

        public string IAFScopeName { get; set; }





    }

//    public class NameCode
//    {
//        public string Name { get; set; }
//        public int Code { get; set; }
        
//    }
}