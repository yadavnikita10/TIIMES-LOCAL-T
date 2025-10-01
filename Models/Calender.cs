using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Calender
    {
        public List<Calender> lst1 { get; set; }
        public int Count { get; set; }
        public String FromDate { get; set; }

        public string ToDate { get; set; }

        public string ActivityType { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
    }
}