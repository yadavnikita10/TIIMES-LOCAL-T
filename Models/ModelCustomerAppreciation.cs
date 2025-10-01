using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ModelCustomerAppreciation
    {
        public List<ModelCustomerAppreciation> lst1 { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public int Id { get; set; }
        public string Branch { get; set; }
        public string Mode { get; set; }
        public string UIIN { get; set; }
        public string Date { get; set; }
        [Required]
        public string JobNumberWithSubJob { get; set; }
        public string ProjectName { get; set; }
        public string PraisingQuote { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string SubVendorName { get; set; }
        public string EmployeeName { get; set; }
        public string DescriptionOfValueAddition { get; set; }
        public string Impact { get; set; }
        //public string EvidencesFileUpload { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string FromD { get; set; }
        public string ToD { get; set; }
        public string Remarks { get; set; }
        public string PraisedBy { get; set; }
        public bool ShareasNews { get; set; }

        public int Count { get; set; } //added by nikita on 10102023


        public virtual ICollection<FileDetails> Attachment { get; set; }
        public virtual ICollection<FileDetails> Attachment1 { get; set; }
        public string CoverImage { get; set; }


    }
}