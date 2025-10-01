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
    public class DALExpenseItem
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        CultureInfo provider = CultureInfo.InvariantCulture;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public string InsertFileAttachment(List<FileDetails> lstFileUploaded, int EXP_ID)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_EXPID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("VoucherNo", typeof(string)));

                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(EXP_ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListQuotationUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    Result = CMDSaveUploadedFile.ExecuteNonQuery().ToString();
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

        public DataTable EditUploadedFile(int? QT_ID)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@FK_ExpID", QT_ID);
                CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        //Delete Uploaded File From Database Code Added by Manoj Sharma 7 March 2020
        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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

        //Modified By Satish Pawar On 22 May 2023
        public string Insert(ExpenseItem E, string IPath, string pkCcId,string pk_cc_id_)
        {
            string Result = string.Empty;
            con.Open();
            int EX_ID = 0;

            string obsId = System.Web.HttpContext.Current.Session["obsId"].ToString();
            string roleId = System.Web.HttpContext.Current.Session["RoleID"].ToString();
            string activityCode = System.Web.HttpContext.Current.Session["Activitycode"].ToString();

            IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
            try
            {
                if (E.PKExpenseId > 0)
                {
                    SqlCommand cmd = new SqlCommand(" ", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '2');
                    cmd.Parameters.AddWithValue("@PKExpenseId", E.PKExpenseId);
                    cmd.Parameters.AddWithValue("@Type", E.Type);
                    cmd.Parameters.AddWithValue("@FkId", E.FKId);
                    cmd.Parameters.AddWithValue("@Country", E.Country);
                    cmd.Parameters.AddWithValue("@City", E.City);
                    cmd.Parameters.AddWithValue("@ExpenseType", E.ExpenseType);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(E.Date, "dd/MM/yyyy", theCultureInfo));
                    cmd.Parameters.AddWithValue("@Currency", E.Currency);
                    cmd.Parameters.AddWithValue("@Amount", E.Amount);
                    //if (E.ExpenseType == "Own Car" || E.ExpenseType == "Own Bike")
                    //{
                    //    cmd.Parameters.AddWithValue("@Amount", "0.00");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@Amount", E.Amount);
                    //}
                    cmd.Parameters.AddWithValue("@SapNumber", E.SAPNo);
                    cmd.Parameters.AddWithValue("@ExchRate", E.ExchRate);
                    cmd.Parameters.AddWithValue("@TotalAmount", E.TotalAmount);
                    cmd.Parameters.AddWithValue("@Description", E.Description);
                    cmd.Parameters.AddWithValue("@Attachment", IPath);
                    //cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
                    //Added By Satish Pawar On 22 May 2023
                    if (E.Type == "Onsite" || E.Type == "MOM")
                    {
                        cmd.Parameters.AddWithValue("@SubJobNo", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
                    }
                    cmd.Parameters.AddWithValue("@Kilometer", E.KM);

                    cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    SqlParameter RequestID = cmd.Parameters.Add("@PK_MAX_EXID", SqlDbType.VarChar, 100);
                    cmd.Parameters["@PK_MAX_EXID"].Direction = ParameterDirection.Output;

                    Result = cmd.ExecuteNonQuery().ToString();

                    EX_ID = Convert.ToInt32(cmd.Parameters["@PK_MAX_EXID"].Value);
                    System.Web.HttpContext.Current.Session["EXID"] = EX_ID;
                }
                else
                {
                    //string formattedDate = E.Date.ToString("dd-MM-yyyy");

                    SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SP_Type", '1');
                    cmd.Parameters.AddWithValue("@Type", E.Type);
                    cmd.Parameters.AddWithValue("@FkId", E.FKId);
                    cmd.Parameters.AddWithValue("@Country", E.Country);
                    cmd.Parameters.AddWithValue("@City", E.City);
                    cmd.Parameters.AddWithValue("@ExpenseType", E.ExpenseType);
                    cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(E.Date, "dd/MM/yyyy", theCultureInfo));
                    //cmd.Parameters.AddWithValue("@Date", E.Date);
                    cmd.Parameters.AddWithValue("@Currency", E.Currency);
                    cmd.Parameters.AddWithValue("@Amount", E.Amount);
                    //if (E.ExpenseType == "Own Car" || E.ExpenseType == "Own Bike")
                    //{
                    //    cmd.Parameters.AddWithValue("@Amount", "0.00");
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@Amount", E.Amount);
                    //}
                    cmd.Parameters.AddWithValue("@SapNumber", E.SAPNo);
                    cmd.Parameters.AddWithValue("@ExchRate", E.ExchRate);
                    cmd.Parameters.AddWithValue("@TotalAmount", E.TotalAmount);
                    cmd.Parameters.AddWithValue("@Description", E.Description);
                    cmd.Parameters.AddWithValue("@Attachment", IPath);
                    //cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
                    //Added By Satish Pawar On 22 May 2023
                    if (E.Type == "Onsite" || E.Type == "MOM")
                    {
                        cmd.Parameters.AddWithValue("@SubJobNo", "");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
                    }
                    cmd.Parameters.AddWithValue("@Kilometer", E.KM);
                    cmd.Parameters.AddWithValue("@UIN", E.UIN);
                    cmd.Parameters.AddWithValue("@PK_Call_ID", E.PK_Call_Id);
                    cmd.Parameters.AddWithValue("@costcenter_", E.CostCenter);

                    //cmd.Parameters.AddWithValue("@Pk_CC_Id", pkCcId);
                    if (obsId == "21" || roleId == "1" || roleId == "46" || roleId == "47" || roleId == "48" || roleId == "49" || roleId == "51" || roleId == "53")
                    {
                        if (activityCode == "1" || activityCode == "02" || activityCode == "03" || activityCode == "30" || activityCode == "31" || activityCode == "34" || activityCode == "32" || activityCode == "27")
                        {
                            cmd.Parameters.AddWithValue("@Pk_CC_Id", pk_cc_id_);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Pk_CC_Id", pkCcId);
                    }

                    cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    SqlParameter RequestID = cmd.Parameters.Add("@PK_MAX_EXID", SqlDbType.VarChar, 100);
                    cmd.Parameters["@PK_MAX_EXID"].Direction = ParameterDirection.Output;

                    Result = cmd.ExecuteNonQuery().ToString();
                    EX_ID = Convert.ToInt32(cmd.Parameters["@PK_MAX_EXID"].Value);
                    System.Web.HttpContext.Current.Session["EXID"] = EX_ID;
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

        //public string Insert(ExpenseItem E, string IPath, string pkCcId)
        //{
        //    string Result = string.Empty;
        //    con.Open();
        //    int EX_ID = 0;
        //    IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        //    try
        //    {
        //        if (E.PKExpenseId > 0)
        //        {
        //            SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@SP_Type", '2');
        //            cmd.Parameters.AddWithValue("@PKExpenseId", E.PKExpenseId);
        //            cmd.Parameters.AddWithValue("@Type", E.Type);
        //            cmd.Parameters.AddWithValue("@FkId", E.FKId);
        //            cmd.Parameters.AddWithValue("@Country", E.Country);
        //            cmd.Parameters.AddWithValue("@City", E.City);
        //            cmd.Parameters.AddWithValue("@ExpenseType", E.ExpenseType);
        //            cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(E.Date, "dd/MM/yyyy", theCultureInfo));
        //            cmd.Parameters.AddWithValue("@Currency", E.Currency);
        //            cmd.Parameters.AddWithValue("@Amount", E.Amount);
        //            cmd.Parameters.AddWithValue("@ExchRate", E.ExchRate);
        //            cmd.Parameters.AddWithValue("@TotalAmount", E.TotalAmount);
        //            cmd.Parameters.AddWithValue("@Description", E.Description);
        //            cmd.Parameters.AddWithValue("@Attachment", IPath);
        //            //cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
        //            //Added By Satish Pawar On 22 May 2023
        //            if (E.Type == "Onsite" || E.Type == "MOM")
        //            {
        //                cmd.Parameters.AddWithValue("@SubJobNo", "");
        //            }
        //            else
        //            {
        //                cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
        //            }
        //            cmd.Parameters.AddWithValue("@Kilometer", E.KM);

        //            cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //            SqlParameter RequestID = cmd.Parameters.Add("@PK_MAX_EXID", SqlDbType.VarChar, 100);
        //            cmd.Parameters["@PK_MAX_EXID"].Direction = ParameterDirection.Output;

        //            Result = cmd.ExecuteNonQuery().ToString();

        //            EX_ID = Convert.ToInt32(cmd.Parameters["@PK_MAX_EXID"].Value);
        //            System.Web.HttpContext.Current.Session["EXID"] = EX_ID;
        //        }
        //        else
        //        {
        //            //string formattedDate = E.Date.ToString("dd-MM-yyyy");

        //            SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@SP_Type", '1');
        //            cmd.Parameters.AddWithValue("@Type", E.Type);
        //            cmd.Parameters.AddWithValue("@FkId", E.FKId);
        //            cmd.Parameters.AddWithValue("@Country", E.Country);
        //            cmd.Parameters.AddWithValue("@City", E.City);
        //            cmd.Parameters.AddWithValue("@ExpenseType", E.ExpenseType);
        //            cmd.Parameters.AddWithValue("@Date", DateTime.ParseExact(E.Date, "dd/MM/yyyy", theCultureInfo));
        //            //cmd.Parameters.AddWithValue("@Date", E.Date);
        //            cmd.Parameters.AddWithValue("@Currency", E.Currency);
        //            cmd.Parameters.AddWithValue("@Amount", E.Amount);
        //            cmd.Parameters.AddWithValue("@ExchRate", E.ExchRate);
        //            cmd.Parameters.AddWithValue("@TotalAmount", E.TotalAmount);
        //            cmd.Parameters.AddWithValue("@Description", E.Description);
        //            cmd.Parameters.AddWithValue("@Attachment", IPath);
        //            //cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
        //            //Added By Satish Pawar On 22 May 2023
        //            if (E.Type == "Onsite" || E.Type == "MOM")
        //            {
        //                cmd.Parameters.AddWithValue("@SubJobNo", "");
        //            }
        //            else
        //            {
        //                cmd.Parameters.AddWithValue("@SubJobNo", E.SubJobNo);
        //            }
        //            cmd.Parameters.AddWithValue("@Kilometer", E.KM);
        //            cmd.Parameters.AddWithValue("@UIN", E.UIN);
        //            cmd.Parameters.AddWithValue("@PK_Call_ID", E.PK_Call_Id);
        //            cmd.Parameters.AddWithValue("@Pk_CC_Id", pkCcId);
        //            cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

        //            SqlParameter RequestID = cmd.Parameters.Add("@PK_MAX_EXID", SqlDbType.VarChar, 100);
        //            cmd.Parameters["@PK_MAX_EXID"].Direction = ParameterDirection.Output;

        //            Result = cmd.ExecuteNonQuery().ToString();
        //            EX_ID = Convert.ToInt32(cmd.Parameters["@PK_MAX_EXID"].Value);
        //            System.Web.HttpContext.Current.Session["EXID"] = EX_ID;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        if (con.State != ConnectionState.Closed)
        //        {
        //            con.Close();
        //        }
        //    }
        //    return Result;
        //}

        public DataSet GetExpenseItem(ExpenseItem ET)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@Type", ET.Type);
                cmd.Parameters.AddWithValue("@FKId", ET.FKId);
                cmd.Parameters.AddWithValue("@SubJobNo", ET.SubJobNo);
                cmd.Parameters.AddWithValue("@ActivityCode", ET.ActivityType);
                
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

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

        public List<Country> GetCountryList()
        {
            List<Country> lstCountry = new List<Country>();
            DataTable dtCountry = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 11);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));




                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtCountry);
                if (dtCountry.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCountry.Rows)
                    {
                        lstCountry.Add(
                           new Country
                           {
                               Id = Convert.ToInt32(dr["PK_ID"]),
                               CountryName = Convert.ToString(dr["CountryName"]),

                           }
                         );
                    }
                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dtCountry.Dispose();
            }
            return lstCountry;
        }



        public DataSet GetDataById(int PKExpenseId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "4");
                cmd.Parameters.AddWithValue("@PKExpenseId", PKExpenseId);
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

        public string Delete(int PKExpenseId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '5');
                cmd.Parameters.AddWithValue("@PKExpenseId", PKExpenseId);
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

        //Added BY Satish Pawar On 17 May 2023
        public DataSet GetMasters()
        {
            //List<Country> lstCountry = new List<Country>();
            //DataTable dtCountry = new DataTable();
            DataSet dsMaster = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 17);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dtCountry);
                da.Fill(dsMaster);
                //if (dtCountry.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtCountry.Rows)
                //    {
                //        lstCountry.Add(
                //           new Country
                //           {
                //               Id = Convert.ToInt32(dr["PK_ID"]),
                //               CountryName = Convert.ToString(dr["CountryName"]),

                //           }
                //         );
                //    }
                //}

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsMaster.Dispose();
            }
            return dsMaster;
        }

        public DataSet GetType(string Type,int? FKId)
        {
            DataSet dsMaster = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 18);
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@FKId", FKId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dtCountry);
                da.Fill(dsMaster);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsMaster.Dispose();
            }
            return dsMaster;
        }

        //Added By Satish Pawar on 25 May 2023
        public DataSet GetVoucherList(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_VoucherDetails_ID_testing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public DataSet GetVoucherApprovalList(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_VoucherApprovalDetails_ID_Testing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        //public DataSet Get_VoucherDetails_History_ID(string VoucherNo)
        //{
        //    DataSet DsVoucher = new DataSet();
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("SP_Get_VoucherDetails_History_ID", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 1000000000;
        //        cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
        //        cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

        //        SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
        //        SDAScripName.Fill(DsVoucher);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DsVoucher.Dispose();
        //    }
        //    return DsVoucher;
        //}

        public DataSet Get_VoucherDetails_History_ID(string VoucherNo, string IsApproval)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_VoucherDetails_History_ID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@isApproval", IsApproval);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        //added by nikita on 21112023
        public DataSet Get_VoucherDetails_History_ID_Description(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_BindDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                //cmd.Parameters.AddWithValue("@isApproval", IsApproval);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        public DataSet SendForApproval(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Voucher_SendForApprovaltesting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }


        public DataSet Approval(ExpenseItem expenseItem, string ApprovedAmounttotal)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Voucher_Approval_testing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", expenseItem.VoucherNo);
                cmd.Parameters.AddWithValue("@PKExpenseId", expenseItem.PKExpenseId);
                cmd.Parameters.AddWithValue("@TotalAmount", expenseItem.TotalAmount);
                cmd.Parameters.AddWithValue("@ApprovedAmount", expenseItem.ApprovedAmount);
                cmd.Parameters.AddWithValue("@ApprovedAmount_voucher", ApprovedAmounttotal);
                cmd.Parameters.AddWithValue("@Remarks", expenseItem.Remarks);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public string BulkApproval(DataSet dsexpenseItem)
        {
            DataSet DsVoucher = new DataSet();
            string Result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("Bulk_Voucher", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@xmlData", dsexpenseItem.GetXml());
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                cmd.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                DsVoucher.Dispose();
            }
            return Result;
        }

        //added by nikita on 25112023
        public DataSet CheckUser(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Expense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@sp_Type", '1');
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public DataSet GetVoucher(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Expense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@sp_Type", '2');
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        public DataSet GetVoucherAccounts(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Expense", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@sp_Type", '3');
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        //added by nikita on 2211023

        public DataSet GetVoucherList_Approved(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Approved_Voucherlist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        //added by nikita on 05122023

        public DataSet GetData_(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetDataFor_VoucherHistory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@voucherno", VoucherNo);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        //added by nikita on 21012024
        public DataSet Getuservoucherlist(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_GetUserVoucherList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        //added by nikita on 11032024
        public DataTable Checked(string Dates, int? FKId)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_checkdata", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Dates", Dates);
                cmd.Parameters.AddWithValue("@fkid", FKId);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(dt);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }


        //added by nikita on 27032024

        public DataSet GetVoucherList_Approvedforsap(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Getsapdata", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }
        public DataSet GetVoucherList_Uploadedsap(string VoucherNo, string Role)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Approved_Voucherlist_sap_uploaded", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.Parameters.AddWithValue("@CreatedBy", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        //added by nikita on 22042024

        public string UpdateSapDataFlag(DataSet dsexpenseItem)
        {
            DataSet DsVoucher = new DataSet();
            string Result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_SapUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", dsexpenseItem.GetXml());
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                Result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                con.Close();
                DsVoucher.Dispose();
            }
            return Result;
        }


        //added by nikita on 30042024
        public DataSet GetCost_center(string Type, string Pk_CC_Id)
        {
            DataSet dsMaster = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "35A");
                cmd.Parameters.AddWithValue("@Type", Type);
                cmd.Parameters.AddWithValue("@FKId", Pk_CC_Id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dtCountry);
                da.Fill(dsMaster);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsMaster.Dispose();
            }
            return dsMaster;
        }
        public string UpdateFileAttachment(List<FileDetails> lstFileUploaded, int EXP_ID)
        {
            string result = string.Empty;
            con.Open();
            SqlTransaction transaction = con.BeginTransaction(); // Begin a transaction to handle multiple operations as a single unit
            try
            {
                foreach (var file in lstFileUploaded)
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ExpenseUploadedFile", con, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SP_Type", 5);
                        cmd.Parameters.AddWithValue("@FileName", file.FileName);
                        cmd.Parameters.AddWithValue("@FileID", file.Id);
                        cmd.Parameters.AddWithValue("@FK_ExpID", EXP_ID);
                        cmd.Parameters.AddWithValue("@Extenstion", file.Extension); // Assuming FileDetails has a property named Extension
                        cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                        cmd.ExecuteNonQuery();
                    }
                }
                transaction.Commit(); // Commit the transaction if all commands succeed
                result = "Success";
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
        public string DeleteUploadedFile(int FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@FileID", FileID);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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
        public DataTable validate_data(string Voucherno)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "40");
                cmd.Parameters.AddWithValue("@VoucherNo_", Voucherno);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(dt);
            }
            catch (Exception ex)
            {
                string error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }

            return dt;
        }
        //added by nikita on 2211023

        public DataSet ValidateVoucher(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_ValidateVoucherAttachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@Voucherno", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public DataSet GetDatewiseApprovedlist(string fromdate, string Todate, string Role, string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Approved_Voucherlist_dateWise", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", DateTime.ParseExact(DateTime.ParseExact(fromdate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@ToDate", DateTime.ParseExact(DateTime.ParseExact(Todate, "dd/mm/yyyy", provider).ToString("dd/mm/yyyy"), "dd/MM/yyyy", theCultureInfo));
                cmd.Parameters.AddWithValue("@CreatedBy", UserId);
                cmd.Parameters.AddWithValue("@Role", Role);
                cmd.CommandTimeout = 100000;
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
        public DataSet GetVoucherMaxDate(string VoucherNo)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", "41");
                cmd.Parameters.AddWithValue("@VoucherNo_", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public DataSet Voucher_DelayRemark(string Voucherno, string Remarks)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "42");
                cmd.Parameters.AddWithValue("@VoucherNo_", Voucherno);
                cmd.Parameters.AddWithValue("@DelayRemarks", Remarks);
                cmd.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
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

        public DataSet Voucher_delayFlag(string Voucherno)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "43");
                cmd.Parameters.AddWithValue("@VoucherNo_", Voucherno);
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
        public string DeleteUploadedFile_(string VoucherNo)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 3);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                //CMDDeleteUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDDeleteUploadedFile.ExecuteNonQuery().ToString();
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

        public string InsertFileAttachment_(List<FileDetails> lstFileUploaded, string voucherno)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("FK_EXPID", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("Voucherno", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(null, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, voucherno);
                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTListQuotationUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    CMDSaveUploadedFile.ExecuteNonQuery().ToString();
                    Result = "true";
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
        public DataTable EditUploadedFile_(string voucherno)
        {
            DataTable DTEditUploadedFile = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_ExpenseUploadedFile", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUploadedFile.Parameters.AddWithValue("@VoucherNo", voucherno);
                //CMDEditUploadedFile.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DTEditUploadedFile);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUploadedFile.Dispose();
            }
            return DTEditUploadedFile;
        }
        public string Delete_(int PKExpenseId)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseUploadedFile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", '3');
                cmd.Parameters.AddWithValue("@PK_ID", PKExpenseId);
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
        public DataSet GetCost_center_id(string Pk_CC_Id)
        {
            DataSet dsMaster = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "45");
                cmd.Parameters.AddWithValue("@FKId", Pk_CC_Id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMaster);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsMaster.Dispose();
            }
            return dsMaster;
        }

        public DataSet GetCostCentre_ExpId(int? FkID)
        {
            DataSet dsMaster = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_ExpenseItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "46");
                cmd.Parameters.AddWithValue("@FKId", FkID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsMaster);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dsMaster.Dispose();
            }
            return dsMaster;
        }

        public DataTable RemoveExpenseApproval(string voucherno)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Voucher_RemoveApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", "1");
                cmd.Parameters.AddWithValue("@Voucherno", voucherno);
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            catch(Exception ex)
            {
                string Message = ex.Message;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }


        public DataSet Getuservoucherlist_Remove(string VoucherNo,string userid)
        {
            DataSet DsVoucher = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Voucher_RemoveApprovallist", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                cmd.Parameters.AddWithValue("@CreatedBy", userid);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

        public DataTable GetApprovalId(string VoucherNo)
        {
            DataTable DsVoucher = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Voucher_RemoveApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 1000000000;
                cmd.Parameters.AddWithValue("@SP_Type", 2);
                cmd.Parameters.AddWithValue("@VoucherNo", VoucherNo);
                SqlDataAdapter SDAScripName = new SqlDataAdapter(cmd);
                SDAScripName.Fill(DsVoucher);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DsVoucher.Dispose();
            }
            return DsVoucher;
        }

    }
}