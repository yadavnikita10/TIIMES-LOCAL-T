using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class BDSPiechart
    {
        public List<BDSPiechart> lstBDSMActivityrecord { get; set; }
        public string ActivityType { get; set; }

        public int ActivityCount { get; set; }

        public int FromDate { get; set; }

        public int ToDate { get; set; }

        public string ExistingCustomerVisit { get; set; }

        public string MarketingOffsitefromoffice { get; set; }

        public string MarketingOnsite { get; set; }

        //public DataPoint(string label, double y)
        //{
        //    this.Label = label;
        //    this.Y = y;
        //}

        ////Explicitly setting the name to be used while serializing to JSON.
        //[DataMember(Name = "label")]
        //public string Label = "";

        ////Explicitly setting the name to be used while serializing to JSON.
        //[DataMember(Name = "y")]
        //public Nullable<double> Y = null;
    }
}