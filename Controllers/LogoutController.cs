using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;

namespace TuvVision.Controllers
{
    public class LogoutController : Controller
    {
        DALLogout objDalLogout = new DALLogout();
        Logout objModelLogout = new Logout();
        // GET: Logout
        public ActionResult Index()
        {
            return View();
        }
     //   [HttpPost]
        public ActionResult Logout()
        {
            Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
            //Session["LastActivity"] = DateTime.Now;

            global.ActivityPing(Convert.ToString(Session["SessionLoggedUserID"]), Convert.ToString(Session["SessionID"]), "Session Logged Off");
            global.SessionModule(Convert.ToString(Session["UserLoginID"]), Convert.ToString(Session["SessionLoggedUserID"]), "2", Convert.ToString(Session["SessionID"]), null);

            string LastClientIP = string.Empty;
            LastClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (LastClientIP == "" || LastClientIP == null)
            {
                LastClientIP = Request.ServerVariables["REMOTE_ADDR"];
            }
            objDalLogout.LastLoginHistory(LastClientIP);
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {

                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {

                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

            return RedirectToAction("UserLogin", "Login");
            //return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangedPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChangedPassword(Login login, string OldPassword, string NewPassword)
        {
            login.UserID = Convert.ToString(System.Web.HttpContext.Current.Session["LoginID"]);
            int result;
            try
            {
                //DataSet dsCheck = objDalLogout.CheckValidUser();
                
                //string pwd1 = dsCheck.Tables[0].Rows[0]["UserPassword"].ToString();

                //string pwd = Decrypt(pwd1);

                //if (pwd != OldPassword)
                //{
                //    return Json(new { result = "Redirect", url = Url.Action("ChangedPassword", "Logout")});
                //}
                if (login.NewPassword != login.ConfirmPassword)
                {
                    return Json(new { result = "RedirectPage", url = Url.Action("ChangedPassword", "Logout") });
                }
                else if (login.ConfirmPassword != "" && login.ConfirmPassword != null)
                {
                    result = objDalLogout.ChangePassword(NewPassword);
                    if (result != 0)
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        private static string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";

            //cipherText = "6nZ/9nf24gFaMKPCHhnhVHqP04P61OJXUFdXwqVUqz4=";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public ActionResult ThankYou()
        {
            return View();
        }
    }
}