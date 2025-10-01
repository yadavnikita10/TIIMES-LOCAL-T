using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class IAFScopeMaster
    {
        public List<IAFScopeMaster> lstlmd1 { get; set; }
        public int Count { get; set; }
        public int PK_IAFScopeId { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        public string IAFScopeName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select")]
        public string IAFScopeNumber { get; set; }
    }
}