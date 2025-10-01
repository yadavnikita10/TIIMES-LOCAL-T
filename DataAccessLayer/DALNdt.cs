using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DALNdt
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        static string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"].ToString();

        public DataTable InsertNDTDataToUsercreation(DataTable finalResult)
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();

            con.Open();
            try
            {
                dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
                dt.Columns.Add(new DataColumn("LastName", typeof(string)));
                dt.Columns.Add(new DataColumn("Branch", typeof(string)));
                dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
                dt.Columns.Add(new DataColumn("RollNo", typeof(string)));
                dt.Columns.Add(new DataColumn("MT_Perc", typeof(string)));
                dt.Columns.Add(new DataColumn("MT_Cert_Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("MT_Pass", typeof(string)));
                dt.Columns.Add(new DataColumn("PT_Perc", typeof(string)));
                dt.Columns.Add(new DataColumn("PT_Cert_Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("PT_Pass", typeof(string)));
                dt.Columns.Add(new DataColumn("VT_Perc", typeof(string)));
                dt.Columns.Add(new DataColumn("VT_Cert_Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("VT_Pass", typeof(string)));
                dt.Columns.Add(new DataColumn("UT_Perc", typeof(string)));
                dt.Columns.Add(new DataColumn("UT_Cert_Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("UT_Pass", typeof(string)));
                dt.Columns.Add(new DataColumn("RT_Perc", typeof(string)));
                dt.Columns.Add(new DataColumn("RT_Cert_Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("RT_Pass", typeof(string)));
                dt.Columns.Add(new DataColumn("Image", typeof(string)));

                foreach (DataRow dtRow in finalResult.Rows)
                {

                    var FirstName = dtRow["FirstName"].ToString();
                    var LastName = dtRow["LastName"].ToString();
                    var Branch = dtRow["Branch"].ToString();
                    var EmployeeCode = dtRow["EmpCode"].ToString();
                    var RollNo = dtRow["RollNo"].ToString();
                    var MT_Perc = dtRow["MT_Perc"].ToString();
                    var MT_Cert_Date = dtRow["MT_Cert_Date"].ToString();
                    var MT_Pass = dtRow["MT_Pass"].ToString();
                    var PT_Perc = dtRow["PT_Perc"].ToString();
                    var PT_Cert_Date = dtRow["PT_Cert_Date"].ToString();
                    var PT_Pass = dtRow["PT_Pass"].ToString();
                    var VT_Perc = dtRow["VT_Perc"].ToString();
                    var VT_Cert_Date = dtRow["VT_Cert_Date"].ToString();
                    var VT_Pass = dtRow["VT_Pass"].ToString();
                    var UT_Perc = dtRow["UT_Perc"].ToString();
                    var UT_Cert_Date = dtRow["UT_Cert_Date"].ToString();
                    var UT_Pass = dtRow["UT_Pass"].ToString();
                    var RT_Perc = dtRow["RT_Perc"].ToString();
                    var RT_Cert_Date = dtRow["RT_Cert_Date"].ToString();
                    var RT_Pass = dtRow["RT_Pass"].ToString();
                    var Image = dtRow["Image"].ToString();

                    DataRow newRow = dt.NewRow();
                    DateTime parsedDate;
                    newRow["FirstName"] = FirstName;  // Assign value to "RollNo" column
                    newRow["LastName"] = LastName;  // Assign value to "MT_Perc" column
                    newRow["Branch"] = Branch;  // Assign value to "RollNo" column
                    newRow["EMpCode"] = EmployeeCode;
                    newRow["RollNo"] = RollNo;  // Assign value to "RollNo" column
                    newRow["MT_Perc"] = MT_Perc;  // Assign value to "MT_Perc" column
                    //newRow["MT_Cert_Date"] = DateTime.Parse(MT_Cert_Date); 
                    newRow["MT_Cert_Date"] = DateTime.TryParse(MT_Cert_Date?.ToString(), out parsedDate) ? (object)parsedDate : DBNull.Value;
                    newRow["MT_Pass"] = MT_Pass;
                    newRow["PT_Perc"] = PT_Perc;
                    newRow["PT_Cert_Date"] = DateTime.TryParse(PT_Cert_Date?.ToString(), out parsedDate) ? (object)parsedDate : DBNull.Value;
                    //newRow["PT_Cert_Date"] = DateTime.Parse(PT_Cert_Date);  // Assign value to "RollNo" column
                    newRow["PT_Pass"] = PT_Pass;
                    newRow["VT_Perc"] = VT_Perc;
                    newRow["VT_Cert_Date"] = DateTime.TryParse(VT_Cert_Date?.ToString(), out parsedDate) ? (object)parsedDate : DBNull.Value;  // Assign value to "RollNo" column
                    newRow["VT_Pass"] = VT_Pass;
                    newRow["UT_Perc"] = UT_Perc;
                    newRow["UT_Cert_Date"] = DateTime.TryParse(UT_Cert_Date?.ToString(), out parsedDate) ? (object)parsedDate : DBNull.Value;
                    newRow["UT_Pass"] = UT_Pass;
                    newRow["RT_Perc"] = RT_Perc;
                    newRow["RT_Cert_Date"] = DateTime.TryParse(RT_Cert_Date?.ToString(), out parsedDate) ? (object)parsedDate : DBNull.Value;
                    newRow["RT_Pass"] = RT_Pass;
                    newRow["Image"] = Image;

                    // Add the new row to the DataTable
                    dt.Rows.Add(newRow);

                }

                if (finalResult.Rows.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_InsertNDTQualifications", con);
                    con.FireInfoMessageEventOnUserErrors = false;
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@NDTData", dt);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                }


            }
            catch (Exception ex)
            {
                // Log the error (if necessary) and return an empty DataTable
                string error = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return dt;
        }




    }
}