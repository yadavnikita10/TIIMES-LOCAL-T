using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Web;
using System.Security.Cryptography;

namespace TuvVision.DataAccessLayer
{
    public class DALLogin
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        public DataSet LoginDetails(Login login)//Checking User Login
        {
            DataSet DTLoginDtl = new DataSet();
            try
            {
                SqlCommand CMDLoginDtl = new SqlCommand("SP_UserLogin", con);
                CMDLoginDtl.CommandType = CommandType.StoredProcedure;
                CMDLoginDtl.CommandTimeout = 100000;
                CMDLoginDtl.Parameters.AddWithValue("@SP_Type", 14);
                CMDLoginDtl.Parameters.AddWithValue("@LoginID", login.UserName);
                CMDLoginDtl.Parameters.AddWithValue("@Password", login.Password);
                SqlDataAdapter SDALoginDtl = new SqlDataAdapter(CMDLoginDtl);
                SDALoginDtl.Fill(DTLoginDtl);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTLoginDtl.Dispose();
            }
            return DTLoginDtl;
        }

        public string UpdateUserLogin(Users u)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDUpdateFlag = new SqlCommand("SP_UserLogin", con);
                CMDUpdateFlag.CommandType = CommandType.StoredProcedure;
                CMDUpdateFlag.Parameters.AddWithValue("@SP_Type", 11);
                CMDUpdateFlag.Parameters.AddWithValue("@flag", u.IsLocked);
                CMDUpdateFlag.Parameters.AddWithValue("@attempts", u.attempts);

                //CMDUpdateFlag.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                CMDUpdateFlag.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                Result = CMDUpdateFlag.ExecuteNonQuery().ToString();
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


        
        public List<MenuRights> GetMenuRights()//Checking User MenuRights
        {
            DataTable DTMenuRghtDtl = new DataTable();
            List<MenuRights> lstMenuLst = new List<MenuRights>();
            
            try
            {
                SqlCommand CMDMenuRghtDtl = new SqlCommand("SP_UserLogin", con);
                CMDMenuRghtDtl.CommandType = CommandType.StoredProcedure;
                CMDMenuRghtDtl.Parameters.AddWithValue("@SP_Type", 6);
                //CMDMenuRghtDtl.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                //CMDMenuRghtDtl.Parameters.AddWithValue("@RoleName", Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"]));
                SqlDataAdapter SDAMenuRgtDtl = new SqlDataAdapter(CMDMenuRghtDtl);
                SDAMenuRgtDtl.Fill(DTMenuRghtDtl);
                if (DTMenuRghtDtl.Rows.Count > 0)
                {
                    lstMenuLst = (from n in DTMenuRghtDtl.AsEnumerable()
                                  select new MenuRights
                                  {
                                      MenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_MainMenuID"])),
                                      MenuName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["MenuName"])),
                                      SubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_SubID"])),
                                      SubMenuName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubMenuName"])),
                                      FK_MainMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["FK_MainMenuID"])),
                                      SubSubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_SubSubMenuID"])),
                                      SubSubMenuName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubSubMenuName"])),
                                      FK_SubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["FK_SubMenuID"])),
                                      //UrlName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["UrlName"])), //Commented by Sagar Panigrahi
                                      //Action = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["Action"])), //Commented by Sagar Panigrahi
                                      //Controller = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["Controller"])), //Commented by Sagar Panigrahi
                                      MainMenuController= Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["MainMenuController"])), // Added By Sagar Panigrahi
                                      MainMenuAction= Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["MainMenuAction"])), // Added By Sagar Panigrahi
                                      SubMenuController = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubMenuController"])), // Added By Sagar Panigrahi
                                      SubMenuAction =Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubMenuAction"])), // Added By Sagar Panigrahi
                                      SubSubMenuController = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubSubMenuController"])), // Added By Sagar Panigrahi
                                      SubSubMenuAction = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubSubMenuAction"])), // Added By Sagar Panigrahi
                                      OrderNo = Convert.ToInt32(n.Field<string>(DTMenuRghtDtl.Columns["OrderNo"])), // Added By Sagar Panigrahi
                                  }).ToList();

                    System.Web.HttpContext.Current.Session["MenuList"] = lstMenuLst;
                }
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            finally
            {
                   DTMenuRghtDtl.Dispose();
            }
            return lstMenuLst;
        }
        public List<MenuRights> MainMenuList()
        {
            DataTable DTMainMenu = new DataTable();
            List<MenuRights> lstMainMenu = new List<MenuRights>();
            
            try
            {
                SqlCommand CMDMenuRghtDtl = new SqlCommand("SP_UserLogin", con);
                CMDMenuRghtDtl.CommandType = CommandType.StoredProcedure;
                CMDMenuRghtDtl.Parameters.AddWithValue("@SP_Type", 7);
                //CMDMenuRghtDtl.Parameters.AddWithValue("@LoginID", login.UserID);
                SqlDataAdapter SDAMenuRgtDtl = new SqlDataAdapter(CMDMenuRghtDtl);
                SDAMenuRgtDtl.Fill(DTMainMenu);
                if (DTMainMenu.Rows.Count > 0)
                {
                    lstMainMenu = (from n in DTMainMenu.AsEnumerable()
                                   select new MenuRights
                                   {
                                       MenuID = Convert.ToInt32(n.Field<Int32>(DTMainMenu.Columns["PK_MainMenuID"])),
                                       MenuName = Convert.ToString(n.Field<string>(DTMainMenu.Columns["MenuName"])),
                                       ChildMenuID = Convert.ToInt32(n.Field<Int32>(DTMainMenu.Columns["ChildMenuID"])),
                                   }).ToList();

                    System.Web.HttpContext.Current.Session["MainMenuLst"] = lstMainMenu;
                }
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            finally
            {
                DTMainMenu.Dispose();
            }
            return lstMainMenu;
        }
        public DataTable CheckRole(Login login)//Checking User Role
        {
            DataTable DTCheckRoleDtl = new DataTable();
            try
            {
                SqlCommand CMDCheckRole = new SqlCommand("SP_UserLogin", con);
                CMDCheckRole.CommandType = CommandType.StoredProcedure;
                CMDCheckRole.CommandTimeout = 100000;
                CMDCheckRole.Parameters.AddWithValue("@SP_Type", 2);
                CMDCheckRole.Parameters.AddWithValue("@LoginID", login.UserName);
                SqlDataAdapter SDACheckRole = new SqlDataAdapter(CMDCheckRole);
                SDACheckRole.Fill(DTCheckRoleDtl);
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            finally
            {
                DTCheckRoleDtl.Dispose();
            }
            return DTCheckRoleDtl;
        }
        //public List<MenuRights> MainMenuList()
        //{
        //    DataTable DTMainMenu = new DataTable();
        //    List<MenuRights> lstMainMenu = new List<MenuRights>();
        //    con.Open();
        //    try
        //    {
        //        SqlCommand CMDMenuRghtDtl = new SqlCommand("SP_UserLogin", con);
        //        CMDMenuRghtDtl.CommandType = CommandType.StoredProcedure;
        //        CMDMenuRghtDtl.Parameters.AddWithValue("@SP_Type", 6);
        //        //CMDMenuRghtDtl.Parameters.AddWithValue("@LoginID", login.UserID);
        //        SqlDataAdapter SDAMenuRgtDtl = new SqlDataAdapter(CMDMenuRghtDtl);
        //        SDAMenuRgtDtl.Fill(DTMainMenu);
        //        if (DTMainMenu.Rows.Count > 0)
        //        {
        //            lstMainMenu = (from n in DTMainMenu.AsEnumerable()
        //                           select new MenuRights
        //                           {
        //                               MenuID = Convert.ToInt32(n.Field<Int32>(DTMainMenu.Columns["MenuID"])),
        //                               MenuName = Convert.ToString(n.Field<string>(DTMainMenu.Columns["MenuName"])),
        //                           }).ToList();

        //            System.Web.HttpContext.Current.Session["MainMenuLst"] = lstMainMenu;
        //            //objModelLog.lstMenu = lstMenuLst;
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
        //        DTMainMenu.Dispose();
        //    }
        //    return lstMainMenu;
        //}

        //***********************************************************Forgot Password Settings Here*********************************************
        public DataTable CheckEmail(string UserEmail,string userName)
        {
            string CheckEmail = string.Empty;
            DataTable DTEmailCheck = new DataTable();
            try
            {
                SqlCommand CMDEmailCheck = new SqlCommand("SP_UserLogin", con);
                CMDEmailCheck.CommandType = CommandType.StoredProcedure;
                CMDEmailCheck.CommandTimeout = 100000;
                CMDEmailCheck.Parameters.AddWithValue("@SP_Type", 5);
                CMDEmailCheck.Parameters.AddWithValue("@EmailID", UserEmail);
                CMDEmailCheck.Parameters.AddWithValue("@UserName", userName);
                SqlDataAdapter SDAEmailCheck = new SqlDataAdapter(CMDEmailCheck);
                SDAEmailCheck.Fill(DTEmailCheck);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                    DTEmailCheck.Dispose();
            }
            return DTEmailCheck;
        }

        public DataTable CheckAttempts(string userName)
        {
            string CheckEmail = string.Empty;
            DataTable DTEmailCheck = new DataTable();
            try
            {
                SqlCommand CMDEmailCheck = new SqlCommand("SP_UserLogin", con);
                CMDEmailCheck.CommandType = CommandType.StoredProcedure;
                CMDEmailCheck.CommandTimeout = 100000;
                CMDEmailCheck.Parameters.AddWithValue("@SP_Type", 12);                
                CMDEmailCheck.Parameters.AddWithValue("@UserName", userName);
                SqlDataAdapter SDAEmailCheck = new SqlDataAdapter(CMDEmailCheck);
                SDAEmailCheck.Fill(DTEmailCheck);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEmailCheck.Dispose();
            }
            return DTEmailCheck;
        }

        public int SendingEMail(string UserEmailID,string UserName)
        {
            int Result = 0;
            string ResetRandomPassword = Encrypt("Pass@123");
            con.Open();
            try
            {
                SqlCommand CMDResetPassword = new SqlCommand("SP_UserLogin", con);
                CMDResetPassword.CommandType = CommandType.StoredProcedure;
                CMDResetPassword.CommandTimeout = 100000;
                CMDResetPassword.Parameters.AddWithValue("@SP_Type", 4);
                CMDResetPassword.Parameters.AddWithValue("@EmailID", UserEmailID);
                CMDResetPassword.Parameters.AddWithValue("@UserName", UserName);
                CMDResetPassword.Parameters.AddWithValue("@ResetPassword", ResetRandomPassword);//Random Generated Password Method Call Here
                CMDResetPassword.Parameters.AddWithValue("@Password", "Pass@123");
                Result = CMDResetPassword.ExecuteNonQuery();
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

        public string GeneratePassword()
        {
            string PasswordLength = "12";
            string NewPassword = "";
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "~,!,@,#,$,%,^,&,*,+,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewPassword = IDString;
            }
            return NewPassword;
        }

        public string FirstTimeUserLogin(int Flag)//FIRST TIME LOGIN FLAG UPDATION
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDUpdateFlag = new SqlCommand("SP_UserLogin", con);
                CMDUpdateFlag.CommandType = CommandType.StoredProcedure;
                CMDUpdateFlag.Parameters.AddWithValue("@SP_Type",8);
                CMDUpdateFlag.Parameters.AddWithValue("@Flag",Flag);
                CMDUpdateFlag.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                Result = CMDUpdateFlag.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                if(con.State!=ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            return Result;
        }
        //Added by Sagar Panigrahi
        public AccessRightsIds GetAccessMenuSubMenu()
        {
            try
            {
                //Added By Sagar Panigrahi for Getting User menu,submenu & its childmenu based on role
                DataTable DTMenuRghtDt = new DataTable();
                var AccessRights = new AccessRightsIds();
                SqlCommand CMDMenuRghtDtl = new SqlCommand("SP_UserLogin", con);
                CMDMenuRghtDtl.CommandType = CommandType.StoredProcedure;
                CMDMenuRghtDtl.Parameters.AddWithValue("@SP_Type", 9);
                var RoleIs = Convert.ToString(System.Web.HttpContext.Current.Session["RoleID"]);
                CMDMenuRghtDtl.Parameters.AddWithValue("@RoleID", Convert.ToString(System.Web.HttpContext.Current.Session["RoleID"]));
                SqlDataAdapter SDAMenuRgtDtl = new SqlDataAdapter(CMDMenuRghtDtl);
                SDAMenuRgtDtl.Fill(DTMenuRghtDt);
                for (int index = 0; index < DTMenuRghtDt.Rows.Count; index++)
                {
                    AccessRights.FkMenuId = Convert.ToString(DTMenuRghtDt.Rows[index]["FK_MenuName"]);
                    AccessRights.FkSubMenuId = Convert.ToString(DTMenuRghtDt.Rows[index]["FK_SUB_MenuNames"]);
                    AccessRights.FkSubSubMenuIds = Convert.ToString(DTMenuRghtDt.Rows[index]["FK_Child_MenuNames"]);
                }
                return AccessRights;
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        public DataSet NewsGalleryDetails(string loginID)
        {
            DataSet DTLoginDtl = new DataSet();
            try
            {
                SqlCommand CMDLoginDtl = new SqlCommand("SP_UserLogin", con);
                CMDLoginDtl.CommandType = CommandType.StoredProcedure;
                CMDLoginDtl.CommandTimeout = 100000;
                CMDLoginDtl.Parameters.AddWithValue("@SP_Type", 13);
                CMDLoginDtl.Parameters.AddWithValue("@LoginID", loginID);

                SqlDataAdapter SDALoginDtl = new SqlDataAdapter(CMDLoginDtl);
                SDALoginDtl.Fill(DTLoginDtl);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTLoginDtl.Dispose();
            }
            return DTLoginDtl;
        }
        //added by nikita on 08042024

        public DataTable CheckObs(string login)
        {
            DataTable DTCheckRoleDtl = new DataTable();
            try
            {
                SqlCommand CMDCheckRole = new SqlCommand("SP_UserLogin", con);
                CMDCheckRole.CommandType = CommandType.StoredProcedure;
                CMDCheckRole.CommandTimeout = 100000;
                CMDCheckRole.Parameters.AddWithValue("@SP_Type", 15);
                CMDCheckRole.Parameters.AddWithValue("@LoginID", login);
                SqlDataAdapter SDACheckRole = new SqlDataAdapter(CMDCheckRole);
                SDACheckRole.Fill(DTCheckRoleDtl);
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            finally
            {
                DTCheckRoleDtl.Dispose();
            }
            return DTCheckRoleDtl;
        }

        //added by nikita on 19042024

        public DataTable Specialservice(string login)//Checking User Role
        {
            DataTable DTCheckRoleDtl = new DataTable();
            try
            {
                SqlCommand CMDCheckRole = new SqlCommand("SP_UserLogin", con);
                CMDCheckRole.CommandType = CommandType.StoredProcedure;
                CMDCheckRole.CommandTimeout = 100000;
                CMDCheckRole.Parameters.AddWithValue("@SP_Type", 17);
                CMDCheckRole.Parameters.AddWithValue("@UserName", login);
                SqlDataAdapter SDACheckRole = new SqlDataAdapter(CMDCheckRole);
                SDACheckRole.Fill(DTCheckRoleDtl);
            }
            catch (Exception ex)
            {

                string Error = ex.Message.ToString();
            }
            finally
            {
                DTCheckRoleDtl.Dispose();
            }
            return DTCheckRoleDtl;
        }
    }
}