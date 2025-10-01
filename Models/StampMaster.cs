using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class StampMaster
    {
        public int Id { get; set; }

        
        public string Image { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Image Name")]
        public string ImageName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Quantity")]
        public string Quantity { get; set; }

        public string Attachment { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Type")]
        public string Type { get; set; }

        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }

    }
}