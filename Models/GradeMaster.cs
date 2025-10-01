using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class GradeMaster
    {
        public List<GradeMaster> lmd1 { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Employee Grade")]
        public string EmployeeGrade { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Car Rate")]
        public string CarRate        { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Motor Bike Rate")]
        public string MotorBikeRate { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select OPE Rate")]
        public string OPERate { get; set; }

        

    }
}