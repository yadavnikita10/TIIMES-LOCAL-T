using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class QuotationServices
    {
        public int PKServiceId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string ServiceName { get; set; }
        public string ServiceImage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public int PKIndustriesId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FkServiceId { get; set; }
        public string IndustryImage { get; set; }
        public string IndustryName { get; set; }


        public int PKProjectId { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FkIndustryId { get; set; }
        public string ProjectImage { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }

    }

   
}