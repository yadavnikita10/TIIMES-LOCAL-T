using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TuvVision.Models;

namespace TuvVision.DataAccessLayer
{
    public class DalQAMetrix
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        public DataTable dtGetNAME()
        {
            DataTable ds = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_QAMetrixCount_test", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SP_Type", '1');
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
        public Dictionary<string, Dictionary<string, int>> PivotData(DataTable data)
        {
            var pivotedData = new Dictionary<string, Dictionary<string, int>>();

            // Iterate over each row in the DataTable
            foreach (DataRow row in data.Rows)
            {
                var branchName = row["Branch_Name"].ToString();
                var parameters = new Dictionary<string, int>();

                // Iterate over each column (except the Branch_Name column)
                foreach (DataColumn column in data.Columns)
                {
                    if (column.ColumnName != "Branch_Name")
                    {
                        var parameterName = column.ColumnName;
                        var value = Convert.ToInt32(row[column]);
                        parameters.Add(parameterName, value);
                    }
                }

                pivotedData.Add(branchName, parameters);
            }

            return pivotedData;
        }
    
    public DataTable GetDataByDate(string fromDate, string ToDate, QAMetrix SM)
        {
            DataTable ds = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(ToDate) && string.IsNullOrEmpty(SM.CostCenter))
                {

                    SqlCommand cmd = new SqlCommand("sp_QAMetrixCount_Datewise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    //cmd.Parameters.AddWithValue("@obstype", SM.CostCenter);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }

                else if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(ToDate) && !string.IsNullOrEmpty(SM.CostCenter))
                {
                    SqlCommand cmd = new SqlCommand("sp_QAMetrixCount_Datewise_OBSWise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@SP_Type", '2');    
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@obstype", SM.CostCenter);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("sp_QAMetrixCount_OBSWise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@obstype", SM.CostCenter);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return ds;
        }
    }
}