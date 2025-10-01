using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.BusinessEntities
{
    public class News_VM
    {
        [DisplayName("ID")]
        public int PK_NewsId { get; set; }
        [DisplayName("Image")]
        public string NewsImage { get; set; }
        public string NewsEvent { get; set; }
        public string Title { get; set; }
        [DisplayName("Short Description")]
        public string StortDescription { get; set; }
        [DisplayName("News Description")]
        public string HtmlContent { get; set; }
        [DisplayName("From Date")]
        public Nullable<DateTime> NewsFrom { get; set; }
        [DisplayName("To Date")]
        public Nullable<DateTime> NewsTo { get; set; }

        public int Status { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Created Date")]
        public Nullable<DateTime> CreatedDate { get; set; }
        public string  strCreatedDate { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        
    }
}