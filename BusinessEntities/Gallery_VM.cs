using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.BusinessEntities
{
    public class Gallery_VM
    {
        [DisplayName("Sr. No")]
        public int PK_GalleryId { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        [DisplayName("Event Date")]
        [DataType(DataType.Date)DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> EventDate { get; set; }
        public string Gallery { get; set; }
        [DisplayName("Created Date")]
        public Nullable<DateTime> CreatedDate { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        public List<string> GallaryImages { get; set; }

    }
}