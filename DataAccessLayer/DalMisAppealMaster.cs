using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TuvVision.DataAccessLayer
{
    public class DalMisAppealMaster
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataTable GetAppealDashBoard()
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("MisReport", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type","22");
                CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
                ad.Fill(DTAppeal);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAppeal.Dispose();
            }
            return DTAppeal;
        }

        //public DataTable GetAppealDashBoardWithDate(AppealMaster a)
        //{
        //    string DateF = a.FromDate;
        //    //IFormatProvider culture = new CultureInfo("en-US", true);
        //    //DateTime DateFrom = DateTime.ParseExact(DateF, "yyyy-MM-dd", culture);

        //    string DateT = (a.ToDate);
        //    //IFormatProvider culture1 = new CultureInfo("en-US", true);
        //    //DateTime DateTo = DateTime.ParseExact(DateT, "yyyy-MM-dd", culture1);


        //    DataTable DTAppeal = new DataTable();
        //    try
        //    {
        //        SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
        //        CMDAppeal.CommandType = CommandType.StoredProcedure;
        //        CMDAppeal.Parameters.AddWithValue("@SP_Type", 8);
        //        //CMDAppeal.Parameters.AddWithValue("@FromDate", DateF);
        //        CMDAppeal.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateF, "dd/MM/yyyy", theCultureInfo));
        //        //CMDAppeal.Parameters.AddWithValue("@ToDate", DateT);
        //        CMDAppeal.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateT, "dd/MM/yyyy", theCultureInfo));
        //        CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
        //        CMDAppeal.CommandTimeout = 100000;
        //        SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
        //        ad.Fill(DTAppeal);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTAppeal.Dispose();
        //    }
        //    return DTAppeal;
        //}
    }
}