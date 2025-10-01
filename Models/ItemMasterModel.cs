using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class ItemMasterModel
    {
        public int Id { get; set; }
        public string inventoryCode { get; set; }
        public string inventoryName { get; set; }
        public decimal inventoryPrice { get; set; }
        public string inventoryAvailability { get; set; }
        public string inventoryTamilName { get; set; }
        public string inventoryImageURL { get; set; }
    }
}