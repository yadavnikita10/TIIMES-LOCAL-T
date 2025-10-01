using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class CompentencyMatrixMaster
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter ")]
        public string Name { get; set; }
        public string Item { get; set; }
        public string ProjectName { get; set; }

        public string BranchName { get; set; }

        public string Branchid { get; set; }

        public int inspectorid { get; set; }

        public string inspectorName { get; set; }
    }
}