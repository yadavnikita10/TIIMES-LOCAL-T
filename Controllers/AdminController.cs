using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Globalization;

using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TuvVision.Models;
using Vision.DatahandlingLayer.Implementation;
using Vision.DatahandlingLayer.Interface;
using Vision.ViewModel;

using System.Configuration;
using System.Data.Entity;
//using Vision.CustomFilter;
using Newtonsoft.Json;
using System.Data;

using System.Drawing.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Net.Mail;

namespace TuvVision.Controllers
{
    public class AdminController : Controller
    {
        AdminRepository Admin = new AdminRepository();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public string Jsonoutput { get; private set; }
        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        public AdminController()
        {
        }
        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

      



        #region Login Mainager

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        #region  Login And Forgot Code 


        #endregion
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    //var s = AuthenticationManager.User.Identity;
                    //var ss = s.GetUserId();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Admin");
        }



        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Admin");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }






        #endregion 

        #region forgot password

      
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            ViewBag.RoleId = 0;
            if (User.Identity.IsAuthenticated)
            {
                var UserName = AuthenticationManager.User.Identity;
                var UID = UserName.GetUserId();
                ViewBag.RoleId = (Admin.GetUserByID(UID)).U_Role_ID;
            }
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                var user1 = Admin.CheckEmailExistsforforgetpassword(model.Email);
                if (user1 == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPassword", "Admin");
                }
                else
                {

                    string code = await UserManager.GeneratePasswordResetTokenAsync(user1);
                    var callbackUrl = Url.Action("ResetPassword", "Admin", new { userId = user1, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user1, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");


                    string body = "<b>Please find the Password Reset Link. </b><br/><a href=\"" + callbackUrl + "\">Click here to reset password.</a>";



                    SendPasswordResetEmail(callbackUrl, body, model.Email);
                    return RedirectToAction("ForgotPasswordConfirmation", "Admin");

                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return RedirectToAction("ForgotPasswordConfirmation", "Admin");

            // If we got this far, something failed, redisplay form

        }


        public void SendPasswordResetEmail(string href, string body, string email)
        {
            try
            {
                MailMessage msg = new MailMessage();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                string adminEmialId = ConfigurationManager.AppSettings["MailFrom"].ToString();
                // string MailBCC = ConfigurationManager.AppSettings["MailBCC"].ToString();
                //string siteURL = ConfigurationManager.AppSettings["siteURL"].ToString();


                string strBodyText = "";
                strBodyText += "<BR>";
                strBodyText += "<BR> Dear User<BR>";
                //   strBodyText += "<BR>Use  the below Link to rest your password<BR>";
                //   strBodyText += "<BR>"+href+"<BR>";
                strBodyText += "<BR>" + body + "<BR>";
                strBodyText += "<BR> Thank You.";
                strBodyText += "<BR> Admin";
                strBodyText += "<BR> ";

                msg.From = new MailAddress(adminEmialId);
                msg.To.Add(email);

                msg.Subject = "Password Reset";
                msg.Body = strBodyText;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;

                SmtpClient client = new SmtpClient(smtpHost);

                client.Send(msg);
            }
            catch (Exception ex)
            {


            }


        }


        #endregion

        #region

        #region ForgotPasswordConfirmation

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model, string email, string code)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            var user1 = Admin.CheckEmailExistsforforgetpassword(model.Email);
            if (user1 == null)
            {
                // Don't reveal that the user does not exist
                //return RedirectToAction("ResetPasswordConfirmation", "Admin");
                TempData["Message"] = "I";
                return View();
            }
            var result = await UserManager.ResetPasswordAsync(user1, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Admin");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion



        #endregion
        // GET: Admin
        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                Session["role"] = UserManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();

                var UID = User.Identity.GetUserId();
                var UserStatus = Admin.GetUserLoginStatus(UID);
                ViewBag.RoleId = (Admin.GetUserByID(UID)).U_Role_ID;
                return View();
            }
            else
            {

                return RedirectToAction("Login", "Admin");
            }

        }


        #region Menu Access To Roll
        public ActionResult TestRolSet()
        {
            var rl = Admin.GetRoleByAction("0", "GetActiveRole");
            ViewBag.RoleList = new SelectList(rl, "ID", "Name");
            ViewData["AllMenucat"] = Admin.GetallMenueCat();
            return View();
        }
        //[Authorize]
        //[RoleFilter(ActionID = 121)]
        [HttpGet]
        public ActionResult RoleSetupforUser()
        {
            var rl = Admin.GetRoleByAction("0", "GetActiveRoleFormenue");
            ViewBag.RoleList = new SelectList(rl, "ID", "Name");
            ViewData["AllMenucat"] = Admin.GetallMenueCat();
            return View();
        }


        [HttpGet]
        public JsonResult GetUserLeftmenueList(string UserID)
        {

            return Json(Admin.getuserLeftmenu(UserID), JsonRequestBehavior.AllowGet);

        }
        public JsonResult UpdateRoleleftmenu(string UID, string LeftMenuID)
        {
            Admin.UpdateRoleMenue(UID, LeftMenuID);
            return Json(new { success = true, wish = true });
        }
        #endregion


        #region Set User 
        [HttpGet]
        //[Authorize(Roles = "Super Administrator, Administrator")]
        public ActionResult AddUpdateUsers(string ID)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserName = AuthenticationManager.User.Identity;
                var UID = UserName.GetUserId();
                ViewBag.RoleId = (Admin.GetUserByID(UID)).U_Role_ID;
                if (ViewBag.RoleId == 1)
                {
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                }
                else
                {
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                }


                //var Temple = Admin.GetTempleByAction("0", "GetActiveTemple");
                //ViewBag.TempleList = new SelectList(Temple, "Temple_ID", "Temple_Name");

                //var country = Admin.GetAllCountries();
                //ViewBag.countryList = new SelectList(country, "name", "name");

                if (ID == null)
                {
                    return View();
                }

                else
                {
                    var data = Admin.GetUserByID(ID);
                    return View(data);


                }
            }
            else
            {
                return RedirectToAction("Login");
            }


        }


        [HttpPost]
        //[Authorize(Roles = "Super Administrator, Administrator")]
        public async Task<ActionResult> AddUpdateUsers(AspNetUsersViewModel u, FormCollection fc)
        {
            int i = 0;

            if (User.Identity.IsAuthenticated)
            {

                var data = AuthenticationManager.User.Identity;
                var UID = data.GetUserId();

                //u.Temple_ID = Convert.ToInt32(ConfigurationManager.AppSettings["TempleID"]);


                if (u.Id == null || u.Id == "0")
                {

                    var user = new ApplicationUser
                    {

                        UserName = u.UserName,
                        Email = u.Email_ID,
                       
                        U_Role_ID = Convert.ToInt32(u.U_Role_ID),
                       
                        #region code Added by rahul
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        DOB = u.DOB,
                        Qualification = u.Qualification,
                        TransferDate = u.TransferDate,
                        DateOf_Joining = u.DateOf_Joining,
                        RelievingDate = u.RelievingDate,
                        Password = u.Password,
                        Branch = u.Branch,
                        Address1 = u.Address1,
                        Address2 = u.Address2,
                        ZipCode = u.ZipCode,
                        Signature = u.Signature,
                        EmployeeCode = u.EmployeeCode,
                        Designation = u.Designation,
                        EmployeeGrade = u.EmployeeGrade,
                        OfficeNo = u.OfficeNo,
                        ResiNo = u.ResiNo,
                        MobileNo = u.MobileNo,
                        SAP_Vandor_code = u.SAP_Vandor_code,
                        Active = u.Active,
                        Language_Spoken = u.Language_Spoken,
                        Employment_Type = u.Employment_Type,
                        Reporting_To = u.Reporting_To,
                        DeleteStatus = u.DeleteStatus,
                        Service_Type = u.Service_Type,
                        #endregion

                        Created_By = UID,
                      
                        Created_Date = DateTime.Now,
                        // Signature = CommonControl.FileUpload("Content/Uploads/Images/", u.Signature)

                    };
                    var result = await UserManager.CreateAsync(user, u.Password);

                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                        //Admin.UpdateUser(u);

                        ModelState.Clear();
                        TempData["Message"] = "S";
                        var Udata = Admin.GetUserDetailByUName(u.UserName);
                        UserManager.AddToRole(Udata.Id, Udata.RoleName);

                        //UserManager.AddToRole(u.Id, fc["Role_name"]);
                        UserManager.AddToRole(Udata.Id, Udata.RoleName);

                    }
                    else
                    {
                        i += 1;
                        AddErrors(result);
                    }



                }
                else
                {
                    u.Modified_By = UID;
                    u.Modified_Date = DateTime.Now;
                    //if (u.ImageUpload != null)
                    //{
                    //    u.profilepic = CommonControl.FileUpload("Content/Uploads/Images/", u.ImageUpload);
                    //}

                    Admin.UpdateUser(u);
                    var Udata = Admin.GetUserDetailByUID(u.Id);

                  //  string role = UserManager.GetRoles(u.Id).SingleOrDefault();
                 //   UserManager.RemoveFromRole(u.Id, role);
                    UserManager.AddToRole(Udata.Id, Udata.RoleName);

                    ModelState.Clear();
                    TempData["Message"] = "E";

                }

                if (i > 0)
                {
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");

                    //var Temple = Admin.GetTempleByAction("0", "GetActiveTemple");
                    //ViewBag.TempleList = new SelectList(Temple, "Temple_ID", "Temple_Name");

                    //var country = Admin.GetAllCountries();
                    //ViewBag.countryList = new SelectList(country, "name", "name");

                    return View();
                }
                else
                {
                    return RedirectToAction("Users");
                }


            }
            else
            {
                return RedirectToAction("Login");
            }


        }

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #region Roll Mainagement
        [HttpGet]
        // [Authorize(Roles = "Super Administrator, Administrator")]
        public ActionResult Users()
        {
            if (User.Identity.IsAuthenticated)
            {
                var UID = User.Identity.GetUserId();
                ViewBag.RoleId = (Admin.GetUserByID(UID)).U_Role_ID;
                if (ViewBag.RoleId == 1)
                {
                    Session["role"] = UserManager.GetRoles(User.Identity.GetUserId()).SingleOrDefault();
                    ViewData["UsersList"] = Admin.GetUserByAction("0", "GetAllUsers");
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                }
                else
                {
                    Session["role"] = UserManager.GetRoles(User.Identity.GetUserId()).SingleOrDefault();
                    ViewData["UsersList"] = Admin.GetUserByAction("0", "GetAllUsers");
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                    TempData["ubhaykarUsers"] = "s";
                }


                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public ActionResult Users(FormCollection fc)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UID = User.Identity.GetUserId();
                ViewBag.RoleId = (Admin.GetUserByID(UID)).U_Role_ID;
                if (ViewBag.RoleId == 1)
                {
                    var rl = Admin.GetRoleByAction("0", "GetActiveRole");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                }
                else
                {
                    var rl = Admin.GetRoleByAction("0", "");
                    ViewBag.RoleList = new SelectList(rl, "ID", "Name");
                }
                Session["role"] = UserManager.GetRoles(User.Identity.GetUserId()).SingleOrDefault();
                ViewData["UsersList"] = Admin.GetUserByAction(fc["RoleList"], "GetAllUsersByUserID");


                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //public object CommonControl { get; private set; }   // rahul Code 
        #endregion



        #region test

        public ActionResult Test()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Test(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:

                    //var s = AuthenticationManager.User.Identity;
                    //var ss = s.GetUserId();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


      
        #endregion


        #region   branch 

        [HttpGet]
        [Authorize]
        public ActionResult BranchMaster()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["BranchList"] = Admin.GetAllBranchList();

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }


        }


        [HttpGet]
        [Authorize]
        public ActionResult AddUpdateBranchMaster(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return View();
                }
                else
                {
                    var data = Admin.GetSingleBranchdetail(Convert.ToInt32(id));
                    return View(data);
                }
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }


        [HttpPost]
        [Authorize]
        //[ValidateInput(false)]
        public ActionResult AddUpdateBranchMaster(BranchMasterViewModel u)
        {
            if (User.Identity.IsAuthenticated)
            {

                var data = AuthenticationManager.User.Identity;
                var UID = data.GetUserId();

              


                if (u.Br_Id == null || u.Br_Id == 0)
                {
                    u.CreatedBy = UID;
                    u.Status = "1";
                    Admin.AddUpdateBranch(u);

                    ModelState.Clear();
                    TempData["Message"] = "S";

                }
                else
                {

                    u.CreatedBy = UID;
                    u.Status = "1";
                    Admin.AddUpdateBranch(u);
                    TempData["Message"] = "E";
                }


                return RedirectToAction("BranchMaster", "Admin");
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }

        }

        #endregion



        public ActionResult InActiveCustomer()
        {
            return View();
        }
    }



    
}