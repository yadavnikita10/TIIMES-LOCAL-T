using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace TuvVision.DataAccessLayer
{
    public class DALLogout
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        public void LastLoginHistory(string LastClientIP)
        {
            con.Open();
            try
            {
                SqlCommand CMDLastLoginID = new SqlCommand("SP_UserLastLoginHistory", con);
                CMDLastLoginID.CommandType = CommandType.StoredProcedure;
                CMDLastLoginID.CommandTimeout = 10000000;
                CMDLastLoginID.Parameters.AddWithValue("@SP_Type", 1);
                CMDLastLoginID.Parameters.AddWithValue("@UserLoginID", Convert.ToInt32(System.Web.HttpContext.Current.Session["UserIDs"]));
                CMDLastLoginID.Parameters.AddWithValue("@UserName", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                CMDLastLoginID.Parameters.AddWithValue("@LastLoginIP", LastClientIP);
                CMDLastLoginID.ExecuteNonQuery();
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
        }
        //public DataSet CheckValidUser()
        //{
        //    DataSet DSCheckUser = new DataSet();
        //    try
        //    {
        //        SqlCommand CMDCheckUser = new SqlCommand("SP_ChangePassword", con);
        //        CMDCheckUser.CommandType = CommandType.StoredProcedure;
        //        CMDCheckUser.Parameters.AddWithValue("@SP_Type", '1');
        //        CMDCheckUser.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //        SqlDataAdapter SDACheckPassword = new SqlDataAdapter(CMDCheckUser);
        //        SDACheckPassword.Fill(DSCheckUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DSCheckUser.Dispose();
        //    }
        //    return DSCheckUser;
        //}

        public DataSet CheckValidUser(string Pk_userid = null)
        {
            DataSet DSCheckUser = new DataSet();
            try
            {
                SqlCommand CMDCheckUser = new SqlCommand("SP_ChangePassword", con);
                CMDCheckUser.CommandType = CommandType.StoredProcedure;
                CMDCheckUser.Parameters.AddWithValue("@SP_Type", '1');
                if (Pk_userid == null)
                {
                    CMDCheckUser.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                }
                else
                {
                    CMDCheckUser.Parameters.AddWithValue("@LoginID", Pk_userid);
                }

                SqlDataAdapter SDACheckPassword = new SqlDataAdapter(CMDCheckUser);
                SDACheckPassword.Fill(DSCheckUser);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSCheckUser.Dispose();
            }
            return DSCheckUser;
        }
        //public Int32 ChangePassword(string NewPassword)
        //{
        //    int result = 0;
        //    con.Open();
        //    try
        //    {
        //        SqlCommand cmdChange = new SqlCommand("SP_ChangePassword", con) { CommandType = CommandType.StoredProcedure };
        //        cmdChange.Parameters.AddWithValue("@SP_Type", '2');
        //        cmdChange.Parameters.AddWithValue("@ChangePassword", NewPassword);
        //        cmdChange.Parameters.AddWithValue("@EncryptChangePassword", Encrypt(NewPassword));
        //        cmdChange.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
        //        result = cmdChange.ExecuteNonQuery();
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
        //    return result;
        //}

        public Int32 ChangePassword(string NewPassword, string Pk_userid = null)
        {
            int result = 0;
            con.Open();
            try
            {
                SqlCommand cmdChange = new SqlCommand("SP_ChangePassword", con) { CommandType = CommandType.StoredProcedure };
                cmdChange.Parameters.AddWithValue("@SP_Type", '2');
                cmdChange.Parameters.AddWithValue("@ChangePassword", NewPassword);
                cmdChange.Parameters.AddWithValue("@EncryptChangePassword", Encrypt(NewPassword));
                if (Pk_userid == null)
                {
                    cmdChange.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                }
                else
                {
                    cmdChange.Parameters.AddWithValue("@LoginID", Pk_userid);
                }
                result = cmdChange.ExecuteNonQuery();
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

        private static string Encrypt(string clearText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

    }
}