using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Web.Mvc;


namespace TuvVision.Models
{
    public class Library
    {
       
            public int Lib_Id { get; set; }
            public string FolderName { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifyDate { get; set; }
            public string ModifyBy { get; set; }
            public Nullable<int> PK_SubSubID { get; set; }
            public Nullable<int> Rollid { get; set; }
           

        

    }
}