using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Audit
    {
        public List<Audit> lmd1 { get; set; }
        public int Count { get; set; }
        public int AuditId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ")]
        public string Branch { get; set; }
        public string TypeOfAudit { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ")]
        public string AuditStandard { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ")]
        public string AuditorName { get; set; }

        public List<string> listAuditorName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ")]
        public string Auditee { get; set; }

        public string ProductList { get; set; }
        public List<string> listAuditee { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        //public DateTime? ProposeDate { get; set; }
        public string ProposeDate { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        //public DateTime? ScheduleDate { get; set; }

        public string ScheduleDate { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public List<ListOfData> ListOfData { get; set; }

        public string Status { get; set; }

        public string DAuditorName { get; set; }
        public string DAuditorCode { get; set; }
        public string ExAuditor { get; set; }
        public string CostCenter { get; set; }   //added by nikita on 08012024

        public string ActualAuditDateTo { get; set; }
        public string ActualAuditDateFrom { get; set; }
        public string TotalFindings { get; set; }
        public string AreFindingsClose { get; set; }
        public string PDF { get; set; }

        public string SProposeDateFrom { get; set; }
        public string SProposeDateTo { get; set; }
        public string SActualDateFrom { get; set; }
        public string SActualDateTo { get; set; }
        public string SaveButtonVisible { get; set; }
        public string AuditNumber { get; set; }
        

    }

    public class ListOfData
    {
        public string LAuditee { get; set; }
        public string LAuditorName { get; set; }


    }

    public class AuditorName
    {
        public string DAuditorName { get; set; }
        public string DAuditorCode { get; set; }
        //public SelectList SectionModel { get; set; }


    }

}