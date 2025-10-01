using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class TripSegment
    {
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public string Date { get; set; }
        public string ExpenseType { get; set; }
        public Double Kilometer { get; set; }
        public string Description { get; set; }
        public Double TotalAmount { get; set; }
        public int  FKId  { get; set; }
        public string Type { get; set; }

        public int PKExpenseId { get; set; }

        public int PK_Call_Id { get; set; }

        public string SubJobNo { get; set; }


    }
}