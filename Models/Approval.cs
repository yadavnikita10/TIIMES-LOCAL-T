using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Approval
    {
        public List<Approval> LstDashboard { get; set; }
        public int Count { get; set; }
        public int LVoucherId { get; set; }
        public string LFKExpenseId { get; set; }
        public string Date { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

       /// [DataType(DataType.Text)]
       // [Required(ErrorMessage = "Please Select Inspector")]
        public string Inspector { get; set; }

        public int PKExpenseId { get; set; }

        public string ActivityType { get; set; }
        public string ApprovalStatus1 { get; set; }
        public string ApprovalStatus2 { get; set; }
        public Double TotalAmount { get; set; }
        public Double AprovelAmount { get; set; }

        public string Reporting_Person_One { get; set; }

        public string Reporting_Person_Two { get; set; }

        public List<ApprovalList> ApprovalList { get; set; }

        public int ApprovedAmount { get; set; }

        public string TransferToFI { get; set; }

        public string Remarks { get; set; }

        public List<Approval> Activity { get; set; }

        public bool chkbox { get; set; }



    }

    public class ApprovalList
    {
        
        public string LActivityType { get; set; }
        public string LApprovalStatus1 { get; set; }
        public string LApprovalStatus2 { get; set; }
        public Double LTotalAmount { get; set; }
        public int LAprovelAmount { get; set; }

        public int LPKExpenseId { get; set; }

        public bool Checkbox1 { get; set; }

       public string LColor { get; set; }
        public string LTransferToFI { get; set; }

        public int LVoucherId { get; set; }

        public string LFKExpenseId { get; set; }

        public DateTime LCreatedDate { get; set; }



       public DateTime LDate { get; set; }
        public string  LExpenseType { get; set; }
        public string  LCountry     { get; set; }
        public string LCurrency     { get; set; }
        
         public int LExchRate       { get; set; }
        public string LDescription  { get; set; }
       
        



    }



    }