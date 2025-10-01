using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class LibraryDocumentModel
    {
        public int LP_Id { get; set; }
        public string PDF { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public Nullable<int> Lib_Id { get; set; }
        public Nullable<int> Rollid { get; set; }
        public string FolderName { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }

        public string EmployementCategory { get; set; }
        public string Location { get; set; }
        public string UserRole { get; set; }

    }
}