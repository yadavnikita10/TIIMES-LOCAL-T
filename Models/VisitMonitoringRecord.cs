using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuvVision.Models
{
    public class VisitMonitoringRecord
    {
        public List<VisitMonitoringRecord> Getsection { get; set; }
        public List<VisitMonitoringRecord> GetPersonsDiary { get; set; }
        public List<VisitMonitoringRecord> lstData { get; set; }
        public List<VisitMonitoringRecord> lstinspectorName { get; set; }
        public string Person { get; set; }
        public string inspectingAuthority { get; set; }
        public string Manufacturer { get; set; }
        public string Section { get; set; }
        public int Pk_id { get; set; }
        public string IVRRefNo { get; set; }
        public string DateofInspection { get; set; }
        public string DiaryNo { get; set; }
        public string SRNO { get; set; }
        public string Signature { get; set; }
        public string VendorName { get; set; }
        public string VendorName_location { get; set; }
        public string LeadGivenBy { get; set; }
        public string Inspectors { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string pk_call_id { get; set; }
        public string Additionalcomment { get; set; }
        public string Inspection_Authority_ReferenceNo { get; set; }
        public string Boliers_Maker_No { get; set; }
        public string Activities_Events { get; set; }
        public string Form_no_Certificate_No { get; set; }
        public string Quality_offered_Final_Inspection { get; set; }
        public string Size { get; set; }
        public string No_For_lot_invoicing { get; set; }
        public bool IsComfirmation { get; set; }
        public bool BolierOwner { get; set; }
        public bool BoilerManufacturer { get; set; }
        public bool BoilerComponentManufacturer { get; set; }
        public string JobNumber { get; set; }
        public string Downloadfile { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Sapnumber { get; set; }
        public int PK_IVR_ID { get; set; }
        public int ID_ { get; set; }
        public bool ShopApproved { get; set; }
        public string ShopApproved_Number { get; set; }
        public string Validity { get; set; }
        public string CertificateNo { get; set; }
        public string IsComfirmation_Date { get; set; }
        public string Activities_Events_Value { get; set; }
    }
}