using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class TravelExpense
    {
        //  page created by nikita on 09102023
        public List<TravelExpense> lstTravelExpense { get; set; }
        public List<TravelExpense> lstUserRole { get; set; }

        public string EmployeeName { get; set; }
        public string RoleName { get; set; }
        public string ExpenseType { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string VoucherGenerated { get; set; }
        public string CountryName { get; set; }
        public string City { get; set; }
        public string Amount { get; set; }
        public string ExchRate { get; set; }
        public string ApprovalOne { get; set; }
        public string ApprovalTwo { get; set; }
        public string TransferToFI { get; set; }
        public string Type { get; set; }
        public string TotalAmount { get; set; }
        public string Attachment { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string Obsname { get; set; }
        public string ApprovedAmount { get; set; }
        public string EndCity { get; set; }
        public string Kilometer { get; set; }
        public string obsname { get; set; }
        public string Fi_Approval { get; set; }
        public string SubJobNo { get; set; }
        public string PCH_Approval { get; set; }
        public string PCH_ApprovalAmount { get; set; }
        public string CH_ApprovalAmount { get; set; }
        public string CH_Approval { get; set; }

        public string IsActive { get; set; }
        public string IsSendForApproval { get; set; }

        public string UIN { get; set; }
        public string VoucherNo { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public string CreatedDate { get; set; }


        public string PkCallId { get; set; }
        public string IsSendForApprovalAmount { get; set; }

      
        //  added by nikita on 17102023
        public string Name { get; set; }
        public int code { get; set; }

        public string SAP_No { get; set; }
        public string Job_Number { get; set; }
        public string Call_No { get; set; }
        public string Date { get; set; }
        public string SAPEmpCode { get; set; }
        public string EmpCategoray { get; set; }
        public string Cost_Center { get; set; }
        public string EmployeeCode { get; set; }
        public string FkId { get; set; }
        public string createdby { get; set; }
        public string MobileNo { get; set; }
        public string Tuv_Email_Id { get; set; }
        public string Designation { get; set; }

        public string BranchName { get; set; }


        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string EmployeementCategoray { get; set; }
        public string UserRole { get; set; }




        public string subCategory { get; set; }
        public string ExpenseagainstGenertaed { get; set; }
        public string ExpenseagainstGenertaed_No { get; set; }



        //added by nikita on 02052024

        public string Days { get; set; }
        public string Month { get; set; }
        public string status { get; set; }
        public string year { get; set; }

    }
}