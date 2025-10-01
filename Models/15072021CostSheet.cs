using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TuvVision.Models
{
    public class CostSheet
    {
        public int PK_Cs_Id { get; set; }
        [Required(ErrorMessage = "Please Select Enquiry")]
        public int EQ_ID { get; set; }
        //[DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Select CostSheet"), MaxLength(30)]
        //[Display(Name = "CostSheet")]
        public string Costsheet { get; set; }
        public string RefNo { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public int Orderby { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string Status { get; set; }
        public string CLStatus { get; set; } //Clusterhead Approval
        
        public int EstimatedAmount { get; set; }
        public string Role { get; set; }
        public string Approval { get; set; }
        public string CLApproval { get; set; }
        public int PK_QTID { get; set; }
        public string QtnNo { get; set; }
        public string SendForApprovel { get; set; }

        public string SenderComment { get; set; }
        public string PCHComment { get; set; }
        public string CHComment { get; set; }

        public string CommentBoxVisible { get; set; }

        public string EComment { get; set; }


    }
}