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

        public List<QuotationMaster> lstQuotationMasterDashBoard1 { get; set; }
        public List<QuotationMaster> JobDashBoard { get; set; }
        public List<QuotationMaster> lstQuotationMasterOrderType { get; set; }

        public string Costsheet { get; set; }
        public string RefNo { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByID { get; set; }
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
        public int CostSheetType { get; set; }
        public string InspectionLocation { get; set; }
        public string QtnNo { get; set; }
        public string SendForApprovel { get; set; }

        public string SenderComment { get; set; }
        public string PCHComment { get; set; }
        public string CHComment { get; set; }

        public string CommentBoxVisible { get; set; }

        public string EComment { get; set; }

        public string DEstimatedAmount { get; set; }
        public string Dcurrency { get; set; }
        public string DExchangeRate { get; set; }
        public string DTotalAmount { get; set; }
        public string IEstimatedAmount { get; set; }
        public string Icurrency { get; set; }
        public string IExchangeRate { get; set; }
        public string ITotalAmount { get; set; }
        public string OrderRate { get; set; }
        public string Estimate_ManMonth { get; set; }
        public string Estimate_ManDays_ManMonth { get; set; }
        public string OrderType { get; set; }

        public string IOrderType { get; set; }
        public string IOrderRate { get; set; }
        public string IEstimate_ManDays_ManMonth { get; set; }
        public string IDistance { get; set; }
        public string Type { get; set; }
        public string IEstimate_ManMonth { get; set; }
        public string Distance { get; set; }
        public string PK_UserId { get; set; }
        public string IFileChoosen { get; set; }
        public string ICLFileChoosen { get; set; }
        public string DFileChoosen { get; set; }
        public string CLDFileChoosen { get; set; }



    }
}