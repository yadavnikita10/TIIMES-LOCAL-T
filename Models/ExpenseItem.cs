using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ExpenseItem
    {
        public List<ExpenseItem> LstDashboard { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public int Count { get; set; }
        public int PKExpenseId { get; set; }
        public string Type { get; set; }
        public int FKId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string EndCity { get; set; }
        public string ExpenseType { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }

        public string pk_cc_id { get; set; } //added by nikita on 03052024

        
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Amount")]
        public double Amount { get; set; }
        public double INRAmount { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select ExchRate")]
        public double ExchRate { get; set; }
        public double TotalAmount { get; set; }
        public string Description { get; set; }
        public string Attachment { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }



        public List<ExpenseItemList> ExpenseItemList { get; set; }


        public string VoucherGenerated { get; set; }
        public string ActivityType { get; set; }
        public string ApprovalStatus1 { get; set; }
        public string ApprovalStatus2 { get; set; }
        public Double ApprovedAmount { get; set; }

        public int VoucherId { get; set; }

        public string FKExpenseId { get; set; }

        public string TransferToFI { get; set; }

        public string SubJobNo { get; set; }

        public int PK_Call_Id { get; set; }
        //ApprovalStatus1 = Convert.ToString(dr["ApprovalStatus1"]),
        //                ApprovalStatus2 = Convert.ToString(dr["ApprovalStatus2"]),
        //                TotalAmount = Convert.ToDouble(dr["VTotalAmount"]),
        //                AprovelAmount = Convert.ToDouble(dr["VApprovedAmount"]),
        //                TransferToFI = Convert.ToString(dr["TransferToFI"]),

        //Added By Satish Pawar On 17 May 2023
        public string UIN { get; set; }
        public string SubUIN { get; set; }
        public string KM { get; set; }
        public string SAPNo { get; set; }
        public string VoucherNo { get; set; }
        public string IsSendForApproval { get; set; }
        public string Month_Name { get; set; } //added by nikita on 21012024
        public string Remarks { get; set; }
        public bool chkbox { get; set; }

        //Change by shrutika salve 12/06/2023
        public string VendorName { get; set; }
        public string Address { get; set; }
        public string month { get; set; }
        public string Year { get; set; }
        public string Branch { get; set; }
        public double INRAmount1 { get; set; }
        public string CostCenter { get; set; }
        public string CompanyName { get; set; }
        public List<ExpenseItem> LstCostDistribution { get; set; }

        //Added By Satish Pawar on 05 July 2023 
        public int AuditId { get; set; }



        public string Name { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployementCategory { get; set; }
        public string PK_UserID { get; set; }
        public string DaysPresent { get; set; }
        public string Status { get; set; }
        public string IsVisibleCheckBox { get; set; }
        public List<ExpenseItem> lst1 { get; set; }

    }


    public class ExpenseItemList
    {
        public bool Checkbox1 { get; set; }

        public int LPKExpenseId { get; set; }
        public string LExpenseType { get; set; }
        public string LDate { get; set; }

        public Double LTotalAmount { get; set; }



    }


    public class ExpenseMode
    {
        public int ID { get; set; }
        public string Mode { get; set; }
    }


    public class ExpensesCurrency
    {
        public int ID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CarRatePerKM
    {
        public string ID { get; set; }
        public string CarRate { get; set; }
        public string MotorBikeRate { get; set; }
    }
}