using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class InvoiceInstructionModel
    {

        
            public int INV_ID { get; set; }
            public Nullable<int> PK_JOB_ID { get; set; }
        public string ServiceCode { get; set; }
        public bool ChkMultipleSubJobNo { get; set; }
        public List<InvoiceList> ListDashboard { get; set; }
        public List<InvoiceList> IVRListData { get; set; }

        public Nullable<decimal> PoAmount { get; set; }
            public Nullable<decimal> InvoiceAmount { get; set; }
            public Nullable<decimal> TotalAmount { get; set; }
            public Nullable<decimal> Balance { get; set; }
            public string InvoiceDate { get; set; }
        public string rptType { get; set; }
        public string InvoiceNumber { get; set; }
        public string Invoicetext { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
     

        public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifyDate { get; set; }
            public string ModifyBy { get; set; }
            public string Status { get; set; }
            public Nullable<int> PK_SubJob_Id { get; set; }
            public string OrderType { get; set; }
            public string OrderRate { get; set; }
            public string Job_Number { get; set; }
            public Nullable<decimal> InvoiceAmount2 { get; set; }
            public string JobNo { get; set; }
            public string SubJobNo { get; set; }
        public string SAPInvNo { get; set; }
        public string GSTDetail { get; set; }
    }

    public class InvoiceList
    {
        

        
        public bool IVRChekbox { get; set; }

        public string Inspectiondate { get; set; }
        public string reportName { get; set; }
        public string Id { get; set; }
        public string CreatedBy { get; set; }

    }

    public class MISOrderStatus
    {
        public List<MISOrderStatus> lst1 { get; set; }
        public int Count { get; set; }
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public String Job_Number { get; set; }
        public String Client_Name { get; set; }
        public String Customer_PO_Amount { get; set; }
        public String Balance { get; set; }

        public String InvoiceAmount { get; set; }


public String         IRNnumber   { get; set; }
public String IRNCreatedDate      { get; set; }
public String Control_number      { get; set; }
public String Originating_Branch  { get; set; }
public String executing_branch    { get; set; }
public String Inspection_dates    { get; set; }
public String Project_name        { get; set; }
public String Client_name         { get; set; }
public String Vendor_name         { get; set; }
public String PO_Number_date      { get; set; }
public String Sub_vendor_name     { get; set; }
public String Inspector_names     { get; set; }
public String PO_status           { get; set; }
public String IRN_Creation_date  { get; set; }
public String Po_amount           { get; set; }
public String Invoiced_amount     { get; set; }
public String Pending_amount { get; set; }


    }
}