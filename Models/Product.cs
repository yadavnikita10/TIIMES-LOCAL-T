using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class Product
    {
        [Required]
        public string Product_Name { get; set; }
        [Required]
        public string code { get; set; }
      

        [Required]
        public int? Product_ID { get; set; }

        [Required]
        public string Created_By { get; set; }
        [Required]
        public string Created_Date { get; set; }
        [Required]
        public string status { get; set; }
        [Required]
        public string Description { get; set; }

    }
}