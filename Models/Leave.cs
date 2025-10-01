using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class Leave
    {

    public List<Leave> objLeave1 { get; set; }
        // [Required(ErrorMessage = "Please enter From Date")]
        public string FromD { get; set; }

       // [Required(ErrorMessage = "Please enter To Date")]
        public string ToD { get; set; }

        [Required(ErrorMessage ="Please Select Start Date")]
        public string StartDate { get; set; }

        [Required(ErrorMessage = "Please Select End Date")]
        public string EndDate { get; set; }

        public string DateSE { get; set; } //Individual Date

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Select Activity Type")]
        public string ActivityType { get; set; }

        public string LeaveAddedBy { get; set; }

        public int Id { get; set; }

        public string Attachment { get; set; }        
        public bool ChkLTILeave { get; set; }
        public string LTILeave { get; set; }
        public string Reason { get; set; }

        public List<Leave> LeaveDashBoard { get; set; }
        public string ApproveName { get; set; }
        public string ApproveDate { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public int ? Year { get; set; }
         public int? BranchID { get; set; }
        public System.Data.DataTable dtLeave { get; set; }
        public List<Leave> lst1 { get; set; }
        public string ProjectType { get; set; }
        public string CH { get; set; }
        public string PCHName { get; set; }
        public string Approver1 { get; set; }
        public string Approver2 { get; set; }
        public string UniversalID { get; set; }
        public string AutoPL { get; set; }
        public string Message { get; set; }
        public string ChkLTILeavestring { get; set; }
        public string uniqueNumber { get; set; }
        public string LeaveCount { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }
        public virtual ICollection<FileDetails> FileDetails1 { get; set; }
        //Added BY Satish Pawar on 07 June 2023
        public string BranchQA { get; set; }
        public string AdminQA { get; set; }
        public bool ChkHalfDayLeave { get; set; }
    }

   

}