using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class BranchTarget
    {

        public int Id { get; set; }

        public string PreviousFYOrderBooking { get; set; }
        public string PreviousFYInvoicing { get; set; }

        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public string OrderBookingTarget { get; set; }
        [MaxLength(12)]
        [MinLength(1)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Must be numeric")]
        public string InvoicingTarget { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }

        public string EstimatedSales { get; set; }
        public string EstimatedEBIT { get; set; }
        public string EstimatedCost_Sales_EBIT { get; set; }
        public string CostTarget { get; set; }
        public string CollectionTarget { get; set; }
        public string Outstanding { get; set; }
        public string DSO { get; set; }
        public string ActualSales { get; set; }
        public string Actual_EBIT { get; set; }
        public string Actual_Cost_SalesEBIT { get; set; }
        public string ActualCost { get; set; }
        public string ActualInvoicing { get; set; }
        public string ActualCollection { get; set; }
        public string EstimatedInvoicingByTiimes { get; set; }
        public string OrderBooking { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDateTime { get; set; }
        [Required]
        public string Year { get; set; }
        public string Month { get; set; }
        [Required]
        public string ServiceCode { get; set; }//Cost center
        public SelectList Lmonths { get; set; }
        public SelectList Ldays { get; set; }

        public int PK_ID { get; set; }
        public string PortfolioName { get; set; }
         
                               

        public List<BranchTarget> BTarget { get; set; }

    }

    public class IndividualTarget
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        [Required]
        public string Year { get; set; }
        public string EmployeeId { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public string OrderBookingTarget { get; set; }
        public string MarketingVisits { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDateTime { get; set; }
        public List<IndividualTarget> ITarget { get; set; }

        public string RoleName { get; set; }
        public string ReportingPersonTwo { get; set; }


    }



}