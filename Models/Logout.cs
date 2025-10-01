using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Logout
    {
        public int PK_ID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string EmailID { get; set; }
        public string Branch { get; set; }
        public string RoleName { get; set; }
        public string Active { get; set; }
    }
}