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
    public class DALAppealMaster
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataTable GetAppealDashBoard()
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 1);
                CMDAppeal.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.CommandTimeout = 100000;
                SqlDataAdapter ad = new SqlDataAdapter(CMDAppeal);
                ad.Fill(DTAppeal);
            }
            catch(Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTAppeal.Dispose();
            }
            return DTAppeal;
        }

        public DataTable GetAppealDashBoardWithDate(AppealMaster a)
        {
            string DateF = a.FromDate;
            //IFormatProvider culture = new CultureInfo("en-US", true);
            //DateTime DateFrom = DateTime.ParseExact(DateF, "yyyy-MM-dd", culture);

            string DateT = (a.ToDate);
            //IFormatProvider culture1 = new CultureInfo("en-US", true);
            //DateTime DateTo = DateTime.ParseExact(DateT, "yyyy-MM-dd", culture1);


            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 8);
                //CMDAppeal.Parameters.AddWithValue("@FromDate", DateF);
                CMDAppeal.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateF, "dd/MM/yyyy", theCultureInfo));
                //CMDAppeal.Parameters.AddWithValue("@ToDate", DateT);
                CMDAppeal.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateT, "dd/MM/yyyy", theCultureInfo));
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
        public DataTable GetAppealauto()
         {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 7);
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
        public DataTable getautocompletedata()
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 6);
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

        public string InsertUpdateAppealData(AppealMaster APMas)
        {
            string result = string.Empty;
            con.Open();
            if (APMas.Appeal_ID != 0)
            {
                try
                {
                    SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                    CMDAppeal.CommandType = CommandType.StoredProcedure;
                    CMDAppeal.Parameters.AddWithValue("@SP_Type", 4);
                    CMDAppeal.Parameters.AddWithValue("@Appeal_ID", APMas.Appeal_ID);
                    CMDAppeal.Parameters.AddWithValue("@Created_by", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDAppeal.Parameters.AddWithValue("@Created_Date",DateTime.Now);
                    CMDAppeal.Parameters.AddWithValue("@Modified_By", System.Web.HttpContext.Current.Session["UserIDs"]);
                    CMDAppeal.Parameters.AddWithValue("@Modified_Date",DateTime.Now);
                    CMDAppeal.Parameters.AddWithValue("@status", APMas.Status);
                    // CMDAppeal.Parameters.AddWithValue("@Appeal_Date", APMas.Date_of_Appeal);
                    CMDAppeal.Parameters.AddWithValue("@Date_of_Appeal", DateTime.ParseExact(APMas.Date_of_Appeal, "dd/MM/yyyy", theCultureInfo));
                    CMDAppeal.Parameters.AddWithValue("@Appeal_Referance_No", APMas.Appeal_Referance_No);
                    CMDAppeal.Parameters.AddWithValue("@Appeliant", APMas.Appellant);
                    CMDAppeal.Parameters.AddWithValue("@Details_Of_Appeal", APMas.Details_Of_Appeal);
                    CMDAppeal.Parameters.AddWithValue("@TUV_Control_No", APMas.TUV_Control_No);
                    CMDAppeal.Parameters.AddWithValue("@Review_And_Analysis", APMas.Review_And_Analysis);
                    CMDAppeal.Parameters.AddWithValue("@Disposal_Action", APMas.Disposal_Action);
                    CMDAppeal.Parameters.AddWithValue("@Disposal_By", APMas.Disposal_By);
                    //CMDAppeal.Parameters.AddWithValue("@Date_Of_Disposal", APMas.Date_Of_Disposal);
                    CMDAppeal.Parameters.AddWithValue("@Date_Of_Disposal", DateTime.ParseExact(APMas.Date_Of_Disposal, "dd/MM/yyyy", theCultureInfo));
                    CMDAppeal.Parameters.AddWithValue("@Remarks", APMas.Remarks);
                    CMDAppeal.Parameters.AddWithValue("@Attachment", APMas.Attachment);
                    CMDAppeal.Parameters.AddWithValue("@Branch", APMas.Branch);
                    CMDAppeal.Parameters.AddWithValue("@Mode_Of_Appeal", APMas.Mode_Of_Appeal);
                    result = CMDAppeal.ExecuteNonQuery().ToString();
                    System.Web.HttpContext.Current.Session["GetAppealID"] = APMas.Appeal_ID;

                }
              catch(Exception ex)
                {
                    string Error = ex.Message.ToString();
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 2);
                //CMDAppeal.Parameters.AddWithValue("@Appeal_ID", APMas.Appeal_ID);
                CMDAppeal.Parameters.AddWithValue("@Created_by", System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.Parameters.AddWithValue("@Created_Date",DateTime.Now);
                CMDAppeal.Parameters.AddWithValue("@Modified_By",System.Web.HttpContext.Current.Session["UserIDs"]);
                CMDAppeal.Parameters.AddWithValue("@Modified_Date", "");
                CMDAppeal.Parameters.AddWithValue("@status", APMas.Status);
                //CMDAppeal.Parameters.AddWithValue("@Appeal_Date", APMas.Date_of_Appeal);
                CMDAppeal.Parameters.AddWithValue("@Appeal_Date", DateTime.ParseExact(APMas.Date_of_Appeal, "dd/MM/yyyy", theCultureInfo));
                CMDAppeal.Parameters.AddWithValue("@Appeal_Referance_No", APMas.Appeal_Referance_No);
                CMDAppeal.Parameters.AddWithValue("@Appeliant", APMas.Appellant);
                CMDAppeal.Parameters.AddWithValue("@Details_Of_Appeal", APMas.Details_Of_Appeal);
                CMDAppeal.Parameters.AddWithValue("@TUV_Control_No", APMas.TUV_Control_No);
                CMDAppeal.Parameters.AddWithValue("@Review_And_Analysis", APMas.Review_And_Analysis);
                CMDAppeal.Parameters.AddWithValue("@Disposal_Action", APMas.Disposal_Action);
                CMDAppeal.Parameters.AddWithValue("@Disposal_By", APMas.Disposal_By);
                //CMDAppeal.Parameters.AddWithValue("@Date_Of_Disposal", APMas.Date_Of_Disposal);
                CMDAppeal.Parameters.AddWithValue("@Date_Of_Disposal", DateTime.ParseExact(APMas.Date_Of_Disposal, "dd/MM/yyyy", theCultureInfo));
                CMDAppeal.Parameters.AddWithValue("@Remarks", APMas.Remarks);
                CMDAppeal.Parameters.AddWithValue("@Attachment", APMas.Attachment);
                CMDAppeal.Parameters.AddWithValue("@Branch", APMas.Branch);
                CMDAppeal.Parameters.AddWithValue("@Mode_Of_Appeal", APMas.Mode_Of_Appeal);

                CMDAppeal.Parameters.Add("@GetAppealID", SqlDbType.Int).Direction = ParameterDirection.Output;

                CMDAppeal.ExecuteNonQuery().ToString();
                result = Convert.ToString(CMDAppeal.Parameters["@GetAppealID"].Value);
                System.Web.HttpContext.Current.Session["GetAppealID"] = result;
            }
            return result;
        }
        public DataTable EditAppeal(int? Appeal_ID)
        {
            DataTable DTAppeal = new DataTable();
            try
            {
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 3);
                CMDAppeal.Parameters.AddWithValue("@Appeal_ID", Appeal_ID);
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
        public int DeleteAppeal(int Appeal_ID)
        {
            int result = 0;
            con.Open();
            try
            {
              
                SqlCommand CMDAppeal = new SqlCommand("SP_AppealMaster", con);
                CMDAppeal.CommandType = CommandType.StoredProcedure;
                CMDAppeal.Parameters.AddWithValue("@SP_Type", 5);
                CMDAppeal.Parameters.AddWithValue("@Appeal_ID", Appeal_ID);
                result = CMDAppeal.ExecuteNonQuery();
                if (result != 0)
                {
                    return result;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }

            }
            return result;
        }
        public DataSet GetCompanyList(string prefix)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_AppealMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 10);
                cmd.Parameters.AddWithValue("@CompanyName", prefix);
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
    }
}
