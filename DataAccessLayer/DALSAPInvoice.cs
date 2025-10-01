using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.Web.Mvc;
using System.Web;

namespace TuvVision.DataAccessLayer
{

    public class DALSAPInvoice
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        string UserIDs = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);


        public string UpdateSAPInvoice(SAPInvoice SAP)
        {
            string result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDSAPInvoice = new SqlCommand("SP_InvoicingInstruction", con);
                CMDSAPInvoice.CommandType = CommandType.StoredProcedure;
                CMDSAPInvoice.Parameters.AddWithValue("@SP_Type", 14);
                CMDSAPInvoice.Parameters.AddWithValue("@InvoiceNumber", SAP.InvoiceNumber);
                CMDSAPInvoice.Parameters.AddWithValue("@SAPNumber", SAP.SAPInvNo);
                CMDSAPInvoice.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));             
                    
                
                result = CMDSAPInvoice.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}