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
    public class DALAudit
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);

        static string strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"].ToString();

        public DataSet BindBranch()
        {
            DataSet DTBindBranch = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Audit", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(DTBindBranch);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTBindBranch.Dispose();
            }
            return DTBindBranch;

        }

        public DataSet BindAuditorName()
        {
            DataSet dsAuditorName = new DataSet();
            try
            {
                SqlCommand CMDGetBranch = new SqlCommand("SP_Audit", con);
                CMDGetBranch.CommandType = CommandType.StoredProcedure;
                CMDGetBranch.Parameters.AddWithValue("@SP_Type", 2);
                SqlDataAdapter da = new SqlDataAdapter(CMDGetBranch);
                da.Fill(dsAuditorName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsAuditorName.Dispose();
            }
            return dsAuditorName;

        }

        public string Insert(Audit SM)
        {
            string Result = string.Empty;

            string strAuditee = "";
            string strAuditor = "";
            con.Open();
            try
            {
                if (SM.AuditId > 0)//Update
                {
                    //SM.AuditorName = String.Join(",", SM.listAuditorName.ToArray());
                    //SM.Auditee = String.Join(",", SM.listAuditee.ToArray());
                    SqlCommand cmd = new SqlCommand("SP_Audit", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '8');

                    cmd.Parameters.AddWithValue("@Branch", SM.Branch);
                    cmd.Parameters.AddWithValue("@TypeOfAudit", SM.TypeOfAudit);
                    cmd.Parameters.AddWithValue("@AuditStandard", SM.AuditStandard);
                    cmd.Parameters.AddWithValue("@AuditorName", SM.AuditorName);
                    cmd.Parameters.AddWithValue("@ExAuditor", SM.ExAuditor);
                    cmd.Parameters.AddWithValue("@Auditee", SM.Auditee);
                    //cmd.Parameters.AddWithValue("@ProposeDate", SM.ProposeDate);
                    cmd.Parameters.AddWithValue("@ProposeDate", DateTime.ParseExact(SM.ProposeDate, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@Costcenter", SM.CostCenter);  //added by nikita on 11012024

                    //cmd.Parameters.AddWithValue("@ScheduleDate", SM.ScheduleDate);
                    cmd.Parameters.AddWithValue("@ScheduleDate", DateTime.ParseExact(SM.ScheduleDate, "dd/MM/yyyy", null));

                    cmd.Parameters.AddWithValue("@Remark", SM.Remark);

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    cmd.Parameters.AddWithValue("@AuditId", SM.AuditId);
                    Result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {

                    //SM.AuditorName = String.Join(",", SM.listAuditorName.ToArray());
                    //SM.Auditee = String.Join(",", SM.listAuditee.ToArray());


                    SqlCommand cmd = new SqlCommand("SP_Audit", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '3');

                    cmd.Parameters.AddWithValue("@Branch", SM.Branch);
                    cmd.Parameters.AddWithValue("@TypeOfAudit", SM.TypeOfAudit);
                    cmd.Parameters.AddWithValue("@AuditStandard", SM.AuditStandard);
                    cmd.Parameters.AddWithValue("@AuditorName", SM.AuditorName);
                    cmd.Parameters.AddWithValue("@ExAuditor", SM.ExAuditor);
                    cmd.Parameters.AddWithValue("@Auditee", SM.Auditee);
                    //cmd.Parameters.AddWithValue("@ProposeDate", SM.ProposeDate);
                    cmd.Parameters.AddWithValue("@ProposeDate", DateTime.ParseExact(SM.ProposeDate, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@Costcenter", SM.CostCenter);  //added by nikita on 11012024

                    //if (SM.ScheduleDate= "1/1/0001 12:00:00 AM")
                    //{

                    //}
                    //else
                    //{
                    // cmd.Parameters.AddWithValue("@ScheduleDate", SM.ScheduleDate);
                    // }
                    cmd.Parameters.AddWithValue("@ScheduleDate", DateTime.ParseExact(SM.ScheduleDate, "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@Remark", SM.Remark);
                    cmd.Parameters.AddWithValue("@AuditNumber", SM.AuditNumber);

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    Result = cmd.ExecuteNonQuery().ToString();
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
            return Result;
        }

        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '4');
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


        public DataSet GetDataById(int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "5");
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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
        public DataSet GetDataByIdForReport(int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "7");
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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

        public DataSet GetActivityType()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "9");
                //cmd.Parameters.AddWithValue("@AuditId", AuditId);
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

        public DataSet GetActivityTypeById(int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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
        public DataSet GetActivityTypeByIdForPDF(int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "11");
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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

        public string Delete(int AuditId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '6');
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
                Result = cmd.ExecuteNonQuery().ToString();

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
            return Result;
        }

        public DataSet GetActivityTypeForPDF(int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Audit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "10");
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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


        //added by nikita on 12012024
        public DataSet GetAuditCostcenter(string stype, int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetAuditCostCneter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", stype);
                cmd.Parameters.AddWithValue("@AuditId", AuditId);
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
        //added by nikita on 12012024
        public DataSet GetMonitoringCostcenter(string Type, int? id)
        {
            DataSet ds = new DataSet();
            try
            {
                if (Type == "MOM")
                {
                    SqlCommand cmd = new SqlCommand("Sp_GetAuditCostCneter", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '3');
                    cmd.Parameters.AddWithValue("@fkId", id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Sp_GetAuditCostCneter", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '4');
                    cmd.Parameters.AddWithValue("@fkId", id);
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
        //added by nikita on 05/02/2024
        public DataSet GetMonitoring(string stype, int? AuditId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetAuditCostCneter", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", stype);
                cmd.Parameters.AddWithValue("@fkId", AuditId);
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