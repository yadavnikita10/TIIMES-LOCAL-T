using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class Country
    {

        
        
        public int Id { get; set; }
        public int FKILId { get; set; }


        public string CountryName { get; set; }

       
        public int isactive { get; set; }

        
        
    }
}