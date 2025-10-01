using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision
{
    public static class GlobalFilterCollectionExtensions
    {
        public static void AddSessionTimeout(this GlobalFilterCollection filters)
        {
            filters.Add(new SessionTimeoutAttribute());
        }
    }
}