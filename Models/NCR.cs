using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class NCR
    {
        public List<NCR> lmd1 { get; set; }
        public int Count { get; set; }
        public int Id { get; set; }
       public string NCRNo                              { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Enter Control Number")]
        public string TUVControlNo                       { get; set; }

        [DataType(DataType.Text)]
        //[Required(ErrorMessage = "Please Enter Project Name")]
        public string ProjectName                        { get; set; }
       public string Client                             { get; set; }
       public string VenderSubVendor                    { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please mention Enter Item Equipment")]
        public string ItemEquipment                      { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please mention Reference Document")]
        public string ReferenceDocument                  { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please mention Description Of The Nonconformity ")]
        public string DescriptionOfTheNonconformity      { get; set; }
        public string AttachedDoucment { get; set; }
        public string NCRRaisedBy                        { get; set; }
        public string NCRClosedBy { get; set; }

        public string ddlReviseReason { get; set; }  //added by nikita 
        public string ReviseReason { get; set; }//added by nikita 
        public string GetRevno { get; set; }//added by nikita 
        public string GetPreviousnumber { get; set; }//added by nikita 
        //public DateTime Date                               { get; set; }
        public String Date { get; set; }

        public string Attachment                         { get; set; }
       public string Pdf                                { get; set; }
       public string IVRId                              { get; set; }
       public string CreatedBy                          { get; set; }
       public DateTime CreatedDate                        { get; set; }
       public string ModifiedBy                         { get; set; }
        public string ModifiedDate                       { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Select Status")]
        public string Status { get; set; }
        public string Branch { get; set; }
        public string PK_Call_Id { get; set; }
        public string countrport { get; set; }
        public string ClosedReason { get; set; }
        public virtual ICollection<FileDetails> FileDetails { get; set; }

        public string SubVendorName { get; set; }
        public string SubJobNo { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string Signature { get; set; }
        public string Edit { get; set; }
        public string Type { get; set; }

        public string SAP { get; set; }
        public string VendorPoNo { get; set; }
        public string SubVendorPoNo { get; set; }
        public string date_of_PO { get; set; }
        public string date_of_POSubVendor { get; set; }
        public string IsCustomerSpecificReportNumber { get; set; }
        public string CustomerSpecificReportNumber { get; set; }

        public string SubSubSubVendor { get; set; }
        public string SubSubSubVendorPO { get; set; }
        public string SubSubSubVendorPODate { get; set; }

        public string V3 { get; set; }
        public string P3 { get; set; }
        public string D3 { get; set; }
        public string SubType { get; set; }
        public bool IsComfirmation { get; set; }
        public string NCRFirstTime { get; set; }
        public string FromDate { get; set; }//added by nikita on 03102023
        public string ToDate { get; set; }//added by nikita on 03102023



    }
}