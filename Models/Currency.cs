using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class Currency
    {
        public List<Currency> lstCurrencyName { get; set; }
        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public bool        IsDefault { get; set; }
        

    }

    public class CurrencyName
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public SelectList SectionModel { get; set; }


    }
}