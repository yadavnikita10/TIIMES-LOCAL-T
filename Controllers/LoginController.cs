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
using Newtonsoft.Json;
using SelectPdf;

namespace TuvVision.Controllers
{
    [SessionTimeout(typeof(LoginController))]
    public class LoginController : Controller//Manoj Sharma NEW with CSS Integrated Code All Here
    {
        DALLogin objDalLogin = new DALLogin();
        DALLogout objDalLogout = new DALLogout();
        Login ObjModelLogin = new Login();
        Users ObjModelUser = new Users();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserLogin()



        {

            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(Login login)
        {

            int FirstTimeLogin = 0;
            System.Data.DataSet ds = new System.Data.DataSet();
            string UsrName = string.Empty;
            string Password = string.Empty;
            string costcentre = string.Empty;
            string fullName = string.Empty;
            string LoginName = string.Empty;
            string EmpCode = string.Empty;
            string Result = string.Empty;
            string EmployeeType = string.Empty;
            string UserID = string.Empty;
            bool IsActive = false;
            bool isLocked = false;
            int attempts = 0;
            string BranchId = "0";
            byte[] IData = null;
            string FRM = "";
            string strPhoto = string.Empty;
            //Session.Clear();
            //Session.Abandon();
            //Session.RemoveAll();

            string NewsRead = "0";
            string GalleryRead = "0";
            string LPGGayCylinder = "NO";
            string IBRCompetent = "NO";
            string FK_Role = "";
            Session["UserLoginID"] = login.UserName;
            DataTable DTCheckLogin = new DataTable();
            DataTable DTCheckPassword = new DataTable();
            DataTable DTCheckEncryptedPassword = new DataTable();

            string EPassword = "";
            string Pass = "";


            ds = objDalLogin.LoginDetails(login);


            if (ds != null && ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                var value = ds.Tables[4].Rows[0]["FRM"];

                if (value != DBNull.Value)
                {
                    FRM = Convert.ToString(value);
                }
            }



            if (ds.Tables[0].Rows.Count > 0)
            {
                DTCheckLogin = ds.Tables[0];
                Session["SessionLoggedUserID"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);

                IBRCompetent = Convert.ToString(DTCheckLogin.Rows[0]["IBRCompetent"]);
                FK_Role = Convert.ToString(DTCheckLogin.Rows[0]["FK_RoleID"]);

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["ChangedPassword"]) == 1)
                {
                    //DTCheckLogin = ds.Tables[0];

                    DTCheckPassword = ds.Tables[1];
                    DTCheckEncryptedPassword = ds.Tables[1];
                    if (DTCheckPassword.Rows.Count > 0)
                    {

                        EPassword = Convert.ToString(ds.Tables[1].Rows[0]["Password"]);
                        Pass = Decrypt(EPassword);

                    }

                    if (DTCheckLogin.Rows.Count > 0)
                    {
                        String TempA = ds.Tables[0].Rows[0]["SendTempMsg"].ToString();
                        if (TempA == "Yes")
                        {
                            TempData["TempAlert"] = "Yes";
                            TempData.Keep();
                        }
                        else
                        {

                        }

                        if (ds.Tables[2].Rows.Count > 0) /////Attempts
                        {
                            isLocked = Convert.ToBoolean(ds.Tables[2].Rows[0]["isLocked"]);
                            attempts = Convert.ToInt32(ds.Tables[2].Rows[0]["attempts"]);
                        }

                        IsActive = Convert.ToBoolean(DTCheckLogin.Rows[0]["IsActive"]);
                        LPGGayCylinder = Convert.ToString(DTCheckLogin.Rows[0]["LPGGasCylinder"]);

                        if (isLocked == true)
                        {
                            TempData["LoginFailed"] = "Account have been Locked, Please contact your Administrator !";
                            return View();
                        }
                        else if (LPGGayCylinder.ToUpper() == "YES")
                        {
                            TempData["LoginFailed"] = "You do not have accress to TIIMES as you are allocated for BPCL Project !";
                            return View();

                        }
                        //else if (DTCheckPassword.Rows.Count == 0)
                        else if (Pass != login.Password)
                        {

                            TempData["LoginFailed"] = "Incorrect Password, Please try again !";
                            ObjModelUser.attempts = attempts + 1;
                            ObjModelUser.IsLocked = false;
                            if (ObjModelUser.attempts == 5)
                            {
                                ObjModelUser.IsLocked = true;
                            }

                            Result = objDalLogin.UpdateUserLogin(ObjModelUser);
                            return View();

                        }
                        else if (IsActive == true) //------if user is active 
                        {

                            ///changes by Savio S for single login on 5/2/2024
                            /// Location changed by Rohini

                            Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
                            Session["LastActivity"] = DateTime.Now;
                            Session["SessionLoggedUserID"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);

                            string OBSID = string.Empty;
                            string UserAgent = GetBrowserName(Request.UserAgent);

                            string IP = login.ipAddr;

                            if (IP == null)
                            {
                                IP = "0.0.0.0";
                            }
                            var host = "Unavailabe";
                            var InfoString = Convert.ToString(host) + "|" + IP + "|" + UserAgent;
                            var check = global.checkLogIn(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "0", Convert.ToString(Session["SessionID"]), InfoString);

                            if (check.Rows.Count > 0)
                            {
                                TempData["SerializedModel"] = JsonConvert.SerializeObject(login);
                                return RedirectToAction("KillOtherSession");
                            }

                            global.SessionModule(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "1", Convert.ToString(Session["SessionID"]), InfoString);
                            global.ActivityPing(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "Session Started");


                            ///changes by Savio S for single login on 5/2/2024

                            UsrName = Convert.ToString(DTCheckLogin.Rows[0]["UserName"]);
                            Password = Pass;//Convert.ToString(DTCheckLogin.Rows[0]["Password"]);
                            FirstTimeLogin = Convert.ToInt32(DTCheckLogin.Rows[0]["Flag"]);
                            BranchId = Convert.ToString(DTCheckLogin.Rows[0]["FK_BranchID"]);
                            if (ds.Tables[3].Rows.Count > 0) ////
                            {
                                //// IData = (byte[])(ds.Tables[3].Rows[0]["IData"]);

                                strPhoto = Convert.ToString(ds.Tables[3].Rows[0]["IData"]);

                            }
                            costcentre = Convert.ToString(DTCheckLogin.Rows[0]["Cost_Center"]);
                            EmpCode = Convert.ToString(DTCheckLogin.Rows[0]["EmployeeCode"]);
                            fullName = Convert.ToString(DTCheckLogin.Rows[0]["Fullname"]);
                            LoginName = Convert.ToString(DTCheckLogin.Rows[0]["LoginName"]);
                            EmployeeType = Convert.ToString(DTCheckLogin.Rows[0]["Employee_Type"]);
                            UserID = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);
                            OBSID = Convert.ToString(DTCheckLogin.Rows[0]["OBSID"]);
                            Session["UserOBSID"] = OBSID;
                            Session["F"] = Convert.ToString(DTCheckLogin.Rows[0]["F"]);
                            Session["L"] = Convert.ToString(DTCheckLogin.Rows[0]["L"]);
                            Session["EmpCode_"] = Convert.ToString(DTCheckLogin.Rows[0]["EmployementCategory"]);
                            String LessionLeantRead = null;
                            Session["FirstTimeLogin"] = FirstTimeLogin;
                            Session["UserBranchId"] = BranchId;
                            Session["UserID"] = UserID;

                            Session["costcentre"] = costcentre;
                            Session["EmpCode"] = EmpCode;
                            Session["fullName"] = fullName;
                            Session["LoginName"] = LoginName;
                            Session["EmployeeType"] = EmployeeType;
                            Session["IData"] = strPhoto;
                            //Session["ISA"] = Convert.ToString(DTCheckLogin.Rows[0]["ISA"]);
                            ObjModelUser.IsLocked = false;
                            ObjModelUser.attempts = 0;

                            ds = objDalLogin.NewsGalleryDetails(Session["UserID"].ToString());

                            NewsRead = ds.Tables[0].Rows[0][0].ToString();
                            GalleryRead = ds.Tables[1].Rows[0][0].ToString();
                            LessionLeantRead = ds.Tables[2].Rows[0][0].ToString();

                            Session["NewsRead"] = NewsRead;
                            Session["GalleryRead"] = GalleryRead;
                            Session["LessionLeantRead"] = LessionLeantRead;

                            Result = objDalLogin.UpdateUserLogin(ObjModelUser);
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;


                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                        }
                        else if (IsActive == false) //----------If User deactivated then Below meassage will appar
                        {
                            TempData["LoginFailed"] = "Account haven't been activated yet, Please contact your Admin Department!";
                            return View();
                        }
                    }
                    else if (DTCheckLogin.Rows.Count == 0) //----------If User not available in sysytem in showing this message 
                    {

                        TempData["LoginFailed"] = "Incorrect Credentials. Please check your Username and Try again..!";


                        return View();
                    }
                    DataTable DTCheckRole = new DataTable();
                    DTCheckRole = objDalLogin.CheckRole(login);
                    if (DTCheckRole.Rows.Count > 0)
                    {
                        Session["RoleName"] = Convert.ToString(DTCheckRole.Rows[0]["RoleName"]);
                        Session["RoleID"] = Convert.ToString(DTCheckRole.Rows[0]["UserRoleID"]);
                        //Added By Satish Pawar on 09 June 2023
                        Session["IsApprover"] = Convert.ToString(DTCheckRole.Rows[0]["IsApprover"]);
                        Session["IsOPE"] = Convert.ToString(DTCheckRole.Rows[0]["OPE"]);
                    }
                    //added by nikita on 08042024
                    DataTable GetObsType = new DataTable();
                    string pkuserid = Session["UserID"].ToString();
                    GetObsType = objDalLogin.CheckObs(pkuserid);
                    Session["obsId"] = Convert.ToString(GetObsType.Rows[0]["SOCode"]);
                    Session["obsName"] = Convert.ToString(GetObsType.Rows[0]["OBSName"]);
                    Session["IsMentor"] = Convert.ToString(GetObsType.Rows[0]["IsMentor"]);

                    //end

                    //added by nikita on 19042024
                    DataTable specialservice = new DataTable();
                    specialservice = objDalLogin.Specialservice(pkuserid);
                    Session["specialservice"] = Convert.ToString(specialservice.Rows[0]["specialServices"]);

                    //end
                    string IsApprover = Session["IsApprover"].ToString();
                    string IsOPE = Session["IsOPE"].ToString();
                    string RoleName = Session["RoleName"].ToString();

                    //Added By Sagar Panigrahi
                    var List = objDalLogin.GetAccessMenuSubMenu();
                    var splittedMenuId = List.FkMenuId.Split(',').Select(int.Parse).ToList();
                    var splittedSubMenuId = List.FkSubMenuId.Split(',').Select(int.Parse).ToList();
                    var splittedSubSubMenuId = List.FkSubSubMenuIds.Split(',').Select(int.Parse).ToList();

                    DataTable DTMenuRightList = new DataTable();
                    List<MenuRights> lstMenuLst = new List<MenuRights>();
                    lstMenuLst = objDalLogin.GetMenuRights();
                    var DistinctMenu = lstMenuLst.GroupBy(s => s.MenuID).Select(s => s.First()).Where(s => s.MenuName != "NA").ToList();
                    var MenuList = new List<MenuListDTO>();
                    bool blnDashboard = false;
                    foreach (var singleMenu in DistinctMenu)
                    {
                        var Menu = new MenuListDTO();
                        Menu.MainMenuId = singleMenu.MenuID.Value;
                        Menu.MainMenuName = singleMenu.MenuName;
                        Menu.Action = singleMenu.MainMenuAction; // Added By Sagar Panigrahi
                        Menu.Controller = singleMenu.MainMenuController; // Added By Sagar Panigrahi


                        //var distinctSubMenu = lstMenuLst.GroupBy(s => s.SubMenuID).Select(s => s.First()).Where(s => s.FK_MainMenuID == Menu.MainMenuId && s.SubMenuName != "NA").ToList();
                        var distinctSubMenu = lstMenuLst.GroupBy(s => s.SubMenuID).Select(s => s.First()).Where(s => s.FK_MainMenuID == Menu.MainMenuId && s.SubMenuName != "NA").OrderBy(s => s.OrderNo).ToList();

                        {
                            foreach (var singleSubMenu in distinctSubMenu)
                            {
                                var SubMenuList = new SubMenuList();
                                SubMenuList.Id = singleSubMenu.SubMenuID.Value;
                                SubMenuList.Name = singleSubMenu.SubMenuName;
                                SubMenuList.Action = singleSubMenu.SubMenuAction; // Changed By Sagar Panigrahi
                                SubMenuList.Controller = singleSubMenu.SubMenuController; // Changed By Sagar Panigrahi
                                var distinctSubSubMenu = lstMenuLst.GroupBy(s => s.SubSubMenuID).Select(s => s.First()).Where(s => s.FK_SubMenuID == singleSubMenu.SubMenuID && s.SubSubMenuName != "NA").ToList();
                                foreach (var singleSubSubMenu in distinctSubSubMenu)
                                {
                                    var SubSubMenuList = new SubSubMenuList();//vaibhav 17/07/2024
                                    SubSubMenuList.SubSubMenuId = singleSubSubMenu.SubSubMenuID.Value;
                                    SubSubMenuList.SubSubMenuName = singleSubSubMenu.SubSubMenuName;
                                    SubSubMenuList.Action = singleSubSubMenu.SubSubMenuAction; // Changed By Sagar Panigrahi
                                    SubSubMenuList.Controller = singleSubSubMenu.SubSubMenuController; // Changed By Sagar Panigrahi

                                    //Actual Comment Start
                                    //var DistinctChild = lstMenuLst.GroupBy(s => s.ChiledMenu).Select(s => s.First()).Where(s => s.ChiledMenu != "NA" && s.SubSubMenuName == singleSubSubMenu.SubSubMenuName).ToList();
                                    //foreach (var singleChild in DistinctChild)
                                    //{
                                    //    var keyValue = new ChildMenuList();
                                    //    keyValue.ChildName = singleChild.ChiledMenu;
                                    //    keyValue.Action = singleChild.Action;
                                    //    keyValue.Controller = singleChild.Controller;
                                    //    bool isInchildList = splittedSubSubMenuId.IndexOf(singleChild.ChildMenuID.Value) != -1;
                                    //    if(isInchildList == true)
                                    //    {
                                    //        SubSubMenuList.ChildMenu.Add(keyValue);
                                    //    }
                                    //}
                                    //Actual Comment End

                                    bool isInSubSubMenuList = splittedSubSubMenuId.IndexOf(SubSubMenuList.SubSubMenuId) != -1;
                                    if (isInSubSubMenuList == true)
                                    {
                                        SubMenuList.SubSubmenuList.Add(SubSubMenuList);
                                    }
                                }
                                bool isInSubMenuList = splittedSubMenuId.IndexOf(SubMenuList.Id) != -1;
                                if (isInSubMenuList == true)
                                {
                                    Menu.SubMenuList.Add(SubMenuList);
                                }
                            }
                        }
                        bool isInMainMenuList = splittedMenuId.IndexOf(Menu.MainMenuId) != -1;
                        if (isInMainMenuList == true)
                        {
                            if (Menu.MainMenuName.Contains("Dashboard"))
                            {
                                blnDashboard = true;
                            }
                            MenuList.Add(Menu);




                        }
                    }


                    #region Vaibhav 17/07/2024

                    if (Session["IsMentor"].ToString() == "Yes")
                    {
                        if (Session["RoleName"].ToString() != "QHSE")
                        {
                            foreach (var menu in MenuList)
                            {
                                if (menu.MainMenuName == "My Activity")
                                {

                                    List<SubMenuList> _subMenuList_VS = new List<SubMenuList>();
                                    var subMenuListOPE = new SubMenuList();
                                    var subMenuListOPE1 = new SubMenuList();
                                    var subMenuListOPE2 = new SubMenuList();

                                    subMenuListOPE.Action = "AllIvrReports";
                                    subMenuListOPE.Controller = "VisitReport";
                                    subMenuListOPE.Id = 86;
                                    subMenuListOPE.Name = "Inspection Visit Reports";
                                    _subMenuList_VS.Add(subMenuListOPE);
                                    menu.SubMenuList.Add(subMenuListOPE);

                                    subMenuListOPE1.Action = "AllIRNReports";
                                    subMenuListOPE1.Controller = "InspectionReleaseNote";
                                    subMenuListOPE1.Id = 86;
                                    subMenuListOPE1.Name = "Inspection Release Notes";
                                    _subMenuList_VS.Add(subMenuListOPE1);
                                    menu.SubMenuList.Add(subMenuListOPE1);

                                    subMenuListOPE2.Action = "ListNCR";
                                    subMenuListOPE2.Controller = "NCR";
                                    subMenuListOPE2.Id = 86;
                                    subMenuListOPE2.Name = "Non Conformity Reports";
                                    _subMenuList_VS.Add(subMenuListOPE2);
                                    menu.SubMenuList.Add(subMenuListOPE2);


                                }
                            }
                        }


                    }


                    #endregion

                    //Added By Satish Pawar on 09 June 2023


                    if (IsApprover == "Yes")
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {
                            if (menu.MainMenuName == "Expense")
                            {

                                existsMenu = "Yes";

                                List<SubMenuList> _subMenuList = new List<SubMenuList>();


                                if (IsOPE.ToUpper() == "YES")
                                {
                                    var subMenuListOPE = new SubMenuList();
                                    subMenuListOPE.Action = "OPE_Report";
                                    subMenuListOPE.Controller = "MISOPEReport";
                                    subMenuListOPE.Id = 27;
                                    subMenuListOPE.Name = "OPE Report";
                                    //subMenuList.SubSubmenuList = 0;
                                    _subMenuList.Add(subMenuListOPE);
                                    menu.SubMenuList.Add(subMenuListOPE);
                                }
                                if (RoleName != "QHSE")
                                {
                                    var subMenuList1 = new SubMenuList();
                                    subMenuList1.Action = "GenerateVoucherApproval";
                                    subMenuList1.Controller = "GenerateVoucher";
                                    subMenuList1.Id = 74;
                                    subMenuList1.Name = "Expense Approval";
                                    _subMenuList.Add(subMenuList1);
                                    menu.SubMenuList.Add(subMenuList1);

                                }
                                var subMenuList = new SubMenuList();
                                subMenuList.Action = "OPE_Approval";
                                subMenuList.Controller = "MISOPEReport";
                                subMenuList.Id = 72;
                                subMenuList.Name = "OPE Approval";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuList);
                                menu.SubMenuList.Add(subMenuList);


                                var subMenuList12 = new SubMenuList(); //10112025

                                subMenuList12.Action = "GetOpApproval";
                                subMenuList12.Controller = "OPE";
                                subMenuList12.Id = 107;
                                subMenuList12.Name = "New OPE Approval";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuList12);

                                menu.SubMenuList.Add(subMenuList12);

                                //added by nikita on 15032024
                                var subMenuListVoucher = new SubMenuList();
                                subMenuListVoucher.Action = "GenerateVoucher";
                                subMenuListVoucher.Controller = "GenerateVoucher";
                                subMenuListVoucher.Id = 73;
                                subMenuListVoucher.Name = "Expense Report";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuListVoucher);
                                menu.SubMenuList.Add(subMenuListVoucher);

                                // end
                                //menu.SubMenuList = _subMenuList;


                                //MenuList.Add(menu);
                                //break;
                            }
                            if (menu.MainMenuName == "My Activity")
                            {

                                //List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                                //var subMenuList2 = new SubMenuList();
                                //subMenuList2.Action = "LeaveManagementApprover";
                                //subMenuList2.Controller = "Leave";
                                //subMenuList2.Id = 78;
                                //subMenuList2.Name = "Approve Leave";

                                //_subMenuList1.Add(subMenuList2);

                                //menu.SubMenuList.Add(subMenuList2);

                                List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                                var subMenuList3 = new SubMenuList();
                                subMenuList3.Id = 103;
                                subMenuList3.Name = "Branch Admins";
                                _subMenuList1.Add(subMenuList3);
                                menu.SubMenuList.Add(subMenuList3);


                                var SubsubMenuList1 = new SubSubMenuList();
                                SubsubMenuList1.Action = "LeaveManagementApprover";
                                SubsubMenuList1.Controller = "Leave";
                                SubsubMenuList1.SubSubMenuId = 286;
                                SubsubMenuList1.SubSubMenuName = "Approve Leave";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList1);

                                var SubsubMenuList2 = new SubSubMenuList();
                                SubsubMenuList2.Action = "CreateAllUser";
                                SubsubMenuList2.Controller = "Users";
                                SubsubMenuList2.SubSubMenuId = 287;
                                SubsubMenuList2.SubSubMenuName = "Manage All Users";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                                //added by shrutika salve 06-062025

                                var SubsubMenuList3 = new SubSubMenuList();
                                SubsubMenuList3.Action = "CallsReview";
                                SubsubMenuList3.Controller = "CallsMaster";
                                SubsubMenuList3.SubSubMenuId = 290;
                                SubsubMenuList3.SubSubMenuName = "Approve Attention-Required Calls";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList3);


                                var SubsubMenuList4 = new SubSubMenuList();
                                SubsubMenuList4.Action = "CompentencyApproval";
                                SubsubMenuList4.Controller = "CompentencyMetrixView";
                                SubsubMenuList4.SubSubMenuId = 291;
                                SubsubMenuList4.SubSubMenuName = "Compentency Approval";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList4);

                                //Added by Adesh Sawant on 22/7/2025
                                var SubsubMenuList5 = new SubSubMenuList();
                                SubsubMenuList5.Action = "FlexMessage";
                                SubsubMenuList5.Controller = "CompentencyMetrixView";
                                SubsubMenuList5.SubSubMenuId = 301;
                                SubsubMenuList5.SubSubMenuName = "Add Flash Message";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList5);

                            }

                            //menu.MainMenuName == "";
                        }
                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            menuDTO.Action = "NA";
                            menuDTO.Controller = "NA";
                            menuDTO.MainMenuId = 9;
                            menuDTO.MainMenuName = "Expense";
                            List<SubMenuList> _subMenuList = new List<SubMenuList>();


                            if (IsOPE.ToUpper() == "YES")
                            {
                                var subMenuListOPE = new SubMenuList();
                                subMenuListOPE.Action = "OPE_Report";
                                subMenuListOPE.Controller = "MISOPEReport";
                                subMenuListOPE.Id = 27;
                                subMenuListOPE.Name = "OPE Report";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuListOPE);

                            }


                            if (RoleName != "QHSE")
                            {
                                var subMenuList1 = new SubMenuList();
                                subMenuList1.Action = "GenerateVoucherApproval";
                                subMenuList1.Controller = "GenerateVoucher";
                                subMenuList1.Id = 74;
                                subMenuList1.Name = "Expense Approval";
                                _subMenuList.Add(subMenuList1);

                            }

                            var subMenuListVoucher = new SubMenuList();
                            subMenuListVoucher.Action = "GenerateVoucher";
                            subMenuListVoucher.Controller = "GenerateVoucher";
                            subMenuListVoucher.Id = 73;
                            subMenuListVoucher.Name = "Expense Report";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuListVoucher);
                            //Nikita 19/3
                            menuDTO.SubMenuList = _subMenuList;
                            //MenuList.Add(menuDTO);

                            var subMenuList = new SubMenuList();

                            subMenuList.Action = "OPE_Approval";
                            subMenuList.Controller = "MISOPEReport";
                            subMenuList.Id = 72;
                            subMenuList.Name = "OPE Approval";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuList);

                            menuDTO.SubMenuList = _subMenuList;


                            var subMenuList_ = new SubMenuList();

                            subMenuList_.Action = "GetOpApproval";
                            subMenuList_.Controller = "OPE";
                            subMenuList_.Id = 107;
                            subMenuList_.Name = "New OPE Approval";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuList_);

                            menuDTO.SubMenuList = _subMenuList;


                            MenuList.Add(menuDTO);







                        }


                    }


                    else if (Session["UserID"].ToString() == "TUV00558")
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {

                            if (menu.MainMenuName == "My Activity")
                            {



                                List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                                var subMenuList3 = new SubMenuList();
                                subMenuList3.Id = 103;
                                subMenuList3.Name = "Branch Admins";
                                _subMenuList1.Add(subMenuList3);
                                menu.SubMenuList.Add(subMenuList3);


                                var SubsubMenuList1 = new SubSubMenuList();
                                SubsubMenuList1.Action = "LeaveManagementApprover";
                                SubsubMenuList1.Controller = "Leave";
                                SubsubMenuList1.SubSubMenuId = 286;
                                SubsubMenuList1.SubSubMenuName = "Approve Leave";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList1);

                                var SubsubMenuList2 = new SubSubMenuList();
                                SubsubMenuList2.Action = "CreateAllUser";
                                SubsubMenuList2.Controller = "Users";
                                SubsubMenuList2.SubSubMenuId = 287;
                                SubsubMenuList2.SubSubMenuName = "Manage All Users";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                                //added by shrutika salve 06-062025

                                var SubsubMenuList3 = new SubSubMenuList();
                                SubsubMenuList3.Action = "CallsReview";
                                SubsubMenuList3.Controller = "CallsMaster";
                                SubsubMenuList3.SubSubMenuId = 290;
                                SubsubMenuList3.SubSubMenuName = "Approve Attention-Required Calls";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList3);

                            }

                        }
                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            menuDTO.Action = "NA";
                            menuDTO.Controller = "NA";
                            menuDTO.MainMenuId = 9;
                            menuDTO.MainMenuName = "Expense";

                            List<SubMenuList> _subMenuList = new List<SubMenuList>();

                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucher";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Report";
                            _subMenuList.Add(subMenuList1);

                            if (IsOPE.ToUpper() == "YES")
                            {
                                var subMenuListOPE = new SubMenuList();
                                subMenuListOPE.Action = "OPE_Report";
                                subMenuListOPE.Controller = "MISOPEReport";
                                subMenuListOPE.Id = 27;
                                subMenuListOPE.Name = "OPE Report";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuListOPE);

                            }







                            menuDTO.SubMenuList = _subMenuList;

                            MenuList.Add(menuDTO);
                        }
                    }

                    else if (Session["UserID"].ToString() == "TUV00303" || FK_Role == "46" || FK_Role == "47" || FK_Role == "48" || FK_Role == "49" || FK_Role == "51" || FK_Role == "53")
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {

                            if (menu.MainMenuName == "MIS")
                            {



                                List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                                var subMenuList3 = new SubMenuList();
                                subMenuList3.Id = 104;
                                subMenuList3.Name = "IBR";
                                _subMenuList1.Add(subMenuList3);
                                menu.SubMenuList.Add(subMenuList3);

                                var SubsubMenuList2 = new SubSubMenuList();
                                SubsubMenuList2.Action = "MISVisitMonitoringdata";
                                SubsubMenuList2.Controller = "VisitReport";
                                SubsubMenuList2.SubSubMenuId = 299;
                                SubsubMenuList2.SubSubMenuName = "Mfr. Monitoring Record (IBR)";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                                var SubsubMenuList1 = new SubSubMenuList();
                                SubsubMenuList1.Action = "MISGetPersonalDiaryData";
                                SubsubMenuList1.Controller = "VisitReport";
                                SubsubMenuList1.SubSubMenuId = 297;
                                SubsubMenuList1.SubSubMenuName = "Mfr. Competent Person's Diary(IBR)";
                                subMenuList3.SubSubmenuList.Add(SubsubMenuList1);

                            }

                        }

                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            menuDTO.Action = "NA";
                            menuDTO.Controller = "NA";
                            menuDTO.MainMenuId = 9;
                            menuDTO.MainMenuName = "Expense";

                            List<SubMenuList> _subMenuList = new List<SubMenuList>();

                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucher";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Report";
                            _subMenuList.Add(subMenuList1);

                            if (IsOPE.ToUpper() == "YES")
                            {
                                var subMenuListOPE = new SubMenuList();
                                subMenuListOPE.Action = "OPE_Report";
                                subMenuListOPE.Controller = "MISOPEReport";
                                subMenuListOPE.Id = 27;
                                subMenuListOPE.Name = "OPE Report";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuListOPE);

                            }
                            menuDTO.SubMenuList = _subMenuList;
                            MenuList.Add(menuDTO);
                        }

                    }



                    else if (RoleName == "InspectionO" || RoleName == "OperationO")
                    {

                    }
                    else
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {

                            #region vaibhav
                            //Vaibhav 17/07/2024
                            List<SubMenuList> _subMenuList132 = new List<SubMenuList>();

                            //if (Session["IsMentor"].ToString() == "Yes")
                            //{


                            //    var subMenuListOPE = new SubMenuList();
                            //    subMenuListOPE.Action = "AllIvrReports";
                            //    subMenuListOPE.Controller = "MISInspectionRecords";
                            //    subMenuListOPE.Id = 9;
                            //    subMenuListOPE.Name = "Inspection Visit Reports";
                            //    _subMenuList132.Add(subMenuListOPE);
                            //    menu.SubMenuList.Add(subMenuListOPE);


                            //}
                            #endregion


                            if (menu.MainMenuName == "Expense")
                            {

                                existsMenu = "Yes";

                                List<SubMenuList> _subMenuList = new List<SubMenuList>();

                                var subMenuList1 = new SubMenuList();
                                subMenuList1.Action = "GenerateVoucher";
                                subMenuList1.Controller = "GenerateVoucher";
                                subMenuList1.Id = 73;
                                subMenuList1.Name = "Expense Report";
                                _subMenuList.Add(subMenuList1);
                                menu.SubMenuList.Add(subMenuList1);

                                if (IsOPE.ToUpper() == "YES")
                                {
                                    var subMenuListOPE = new SubMenuList();
                                    subMenuListOPE.Action = "OPE_Report";
                                    subMenuListOPE.Controller = "MISOPEReport";
                                    subMenuListOPE.Id = 27;
                                    subMenuListOPE.Name = "OPE Report";
                                    //subMenuList.SubSubmenuList = 0;
                                    _subMenuList.Add(subMenuListOPE);
                                    menu.SubMenuList.Add(subMenuListOPE);
                                }

                                //menu.SubMenuList = _subMenuList;


                                //MenuList.Add(menu);
                                break;
                            }
                            //menu.MainMenuName == "";
                        }
                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            menuDTO.Action = "NA";
                            menuDTO.Controller = "NA";
                            menuDTO.MainMenuId = 9;
                            menuDTO.MainMenuName = "Expense";


                            List<SubMenuList> _subMenuList = new List<SubMenuList>();

                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucher";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Report";
                            _subMenuList.Add(subMenuList1);


                            if (IsOPE.ToUpper() == "YES")
                            {
                                var subMenuListOPE = new SubMenuList();
                                subMenuListOPE.Action = "OPE_Report";
                                subMenuListOPE.Controller = "MISOPEReport";
                                subMenuListOPE.Id = 27;
                                subMenuListOPE.Name = "OPE Report";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuListOPE);

                            }

                            menuDTO.SubMenuList = _subMenuList;

                            MenuList.Add(menuDTO);
                        }


                    }
                    //===========================
                    //Added By Satish Pawar on 21 June 2023
                    if (RoleName == "Accounts")
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {
                            if (menu.MainMenuName == "Expense")
                            {

                                existsMenu = "Yes";

                                List<SubMenuList> _subMenuList = new List<SubMenuList>();

                                var subMenuList1 = new SubMenuList();
                                subMenuList1.Action = "GenerateVoucherApproval";
                                subMenuList1.Controller = "GenerateVoucher";
                                subMenuList1.Id = 74;
                                subMenuList1.Name = "Expense Approval";
                                _subMenuList.Add(subMenuList1);
                                menu.SubMenuList.Add(subMenuList1);

                                break;
                            }
                            //menu.MainMenuName == "";
                        }
                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            menuDTO.Action = "NA";
                            menuDTO.Controller = "NA";
                            menuDTO.MainMenuId = 9;
                            menuDTO.MainMenuName = "Expense";
                            List<SubMenuList> _subMenuList = new List<SubMenuList>();

                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucherApproval";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Approval";
                            _subMenuList.Add(subMenuList1);

                            menuDTO.SubMenuList = _subMenuList;

                            MenuList.Add(menuDTO);
                        }


                    }
                    //End Added Code By Satish Pawar on 21 June 2023

                    //added by nikita on 28122023
                    if (RoleName == "Expenses approver")
                    {
                        string existsMenu = "No";
                        foreach (var menu in MenuList)
                        {
                            if (menu.MainMenuName == "Expense")
                            {

                                existsMenu = "Yes";

                                List<SubMenuList> _subMenuList = new List<SubMenuList>();

                                var subMenuList1 = new SubMenuList();
                                subMenuList1.Action = "GenerateVoucherApproval";
                                subMenuList1.Controller = "GenerateVoucher";
                                subMenuList1.Id = 74;
                                subMenuList1.Name = "Expense Approval";
                                _subMenuList.Add(subMenuList1);
                                menu.SubMenuList.Add(subMenuList1);

                                //break;
                                var subMenuList = new SubMenuList();
                                subMenuList.Action = "OPE_Approval";
                                subMenuList.Controller = "MISOPEReport";
                                subMenuList.Id = 72;
                                subMenuList.Name = "OPE Approval";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuList);
                                menu.SubMenuList.Add(subMenuList);




                                var subMenuList12 = new SubMenuList(); //10112025

                                subMenuList12.Action = "GetOpApproval";
                                subMenuList12.Controller = "OPE";
                                subMenuList12.Id = 107;
                                subMenuList12.Name = "New OPE Approval";
                                //subMenuList.SubSubmenuList = 0;
                                _subMenuList.Add(subMenuList12);

                                menu.SubMenuList.Add(subMenuList12);

                                break;
                            }
                            //menu.MainMenuName == "";
                        }
                        if (existsMenu == "No")
                        {
                            MenuListDTO menuDTO = new MenuListDTO();
                            //menuDTO.Action = "NA";
                            //menuDTO.Controller = "NA";
                            //menuDTO.MainMenuId = 9;
                            //menuDTO.MainMenuName = "Expense";
                            List<SubMenuList> _subMenuList = new List<SubMenuList>();

                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucherApproval";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Approval";
                            _subMenuList.Add(subMenuList1);

                            menuDTO.SubMenuList = _subMenuList;

                            MenuList.Add(menuDTO);

                            var subMenuList = new SubMenuList();
                            subMenuList.Action = "OPE_Approval";
                            subMenuList.Controller = "MISOPEReport";
                            subMenuList.Id = 72;
                            subMenuList.Name = "OPE Approval";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuList);

                            var subMenuList12 = new SubMenuList(); //10112025

                            subMenuList12.Action = "GetOpApproval";
                            subMenuList12.Controller = "OPE";
                            subMenuList12.Id = 107;
                            subMenuList12.Name = "New OPE Approval";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuList12);



                            MenuList.Add(menuDTO);
                        }


                    }

                    //end of nikita code

                    var sdsdsd = MenuList;

                    Session["NewMenuList"] = MenuList;

                    var GroupBydata = lstMenuLst.GroupBy(car => car.MenuID).Distinct().ToList();

                    var GroupByDataSubMenu = lstMenuLst.GroupBy(cars => cars.SubMenuID).Distinct().ToList();

                    var GroupByDataSubSubMenu = lstMenuLst.GroupBy(carss => carss.SubSubMenuID).Distinct().ToList();

                    Session["GroupBydata"] = GroupBydata as List<IGrouping<int?, MenuRights>>;
                    Session["GroupByDataSubMenu"] = GroupByDataSubMenu as List<IGrouping<int?, MenuRights>>;
                    Session["GroupByDataSubSubMenu"] = GroupByDataSubSubMenu as List<IGrouping<int?, MenuRights>>;

                    Session.Add("MenuList", lstMenuLst);

                    List<MenuRights> lstMainMenuLst = new List<MenuRights>();
                    lstMainMenuLst = objDalLogin.MainMenuList();
                    Session.Add("MainMenuList", lstMainMenuLst);



                    //if (DTCheckLogin.Rows.Count == 0)
                    //{

                    //        ViewBag.UserID = "Entered Invalid User Id....!";
                    //        return View();
                    //}
                    if (UsrName != login.UserName && Password != login.Password)
                    {
                        ViewBag.UserID = "Invalid User...";
                        ViewBag.Password = "Invalid Password...";
                        return View();
                    }


                    if (UsrName.ToUpper() != login.UserName.ToUpper())
                    {
                        ViewBag.UserID = "Invalid User...";
                        return View();
                    }
                    string pwd = Pass;//Convert.ToString(DTCheckLogin.Rows[0]["Password"]);
                    if (pwd != login.Password)
                    {
                        ViewBag.Password = "Invalid Password...";
                        return View();
                    }
                    else
                    {
                        //    FormsAuthentication.SetAuthCookie(login.UserName, true);
                        Session["EmailID"] = Convert.ToString(DTCheckLogin.Rows[0]["EmailID"]);
                        Session["UserIDs"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);
                        Session["LoginID"] = Convert.ToString(login.UserName);
                        Session["UserName"] = Convert.ToString(DTCheckLogin.Rows[0]["UserName"]);
                        login.UserName = Convert.ToString(Session["UserLoginID"]);
                        Session["ID"] = Convert.ToString(DTCheckLogin.Rows[0]["Id"]);
                    }

                    if (login.UserName != "" && login.UserName != null)
                    {
                        ///changes by Savio S for single login on 5/2/2024
                        ///////Commented by Rohini

                        //Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
                        //Session["LastActivity"] = DateTime.Now;
                        //Session["SessionLoggedUserID"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);

                        //string UserAgent = GetBrowserName(Request.UserAgent);

                        //string IP = login.ipAddr;

                        //if (IP == null)
                        //{
                        //    IP = "0.0.0.0";
                        //}

                        //var host = "Unavailabe";
                        ////System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                        ////
                        ////string host = Request.LogonUserIdentity.Name;

                        //var InfoString = Convert.ToString(host) + "|" + IP + "|" + UserAgent;

                        //var check = global.checkLogIn(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "0", Convert.ToString(Session["SessionID"]), InfoString);

                        //if (check.Rows.Count > 0)
                        //{
                        //    TempData["SerializedModel"] = JsonConvert.SerializeObject(login);
                        //    return RedirectToAction("KillOtherSession");
                        //}

                        //global.SessionModule(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "1", Convert.ToString(Session["SessionID"]), InfoString);
                        //global.ActivityPing(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "Session Started");


                        ///changes by Savio S for single login on 5/2/2024
                        ///
                        if (FirstTimeLogin == 0)
                        {
                            int Flag = 1;
                            Result = objDalLogin.FirstTimeUserLogin(Flag);
                            return RedirectToAction("ChangedPassword", "Logout");
                        }
                        else if (FirstTimeLogin == 1)
                        {


                            if (FRM == "0")
                            {
                                //Action = "GetDashBoard";
                                //Controller = "Login";
                                //return Controller + "," + Action;
                                return RedirectToAction("GetDashBoard", "Login");
                            }
                            else if (RoleName == "BD S and M and Operations" || RoleName == "BD S and M and Operations" || RoleName == "Clusterhead Industrial Inspection" || RoleName == "Clusterhead Renewable Building and Construction" || RoleName == "BD S and M International" || RoleName == "BD S and M International" || RoleName == "Clusterhead Special Services" || RoleName == "Clusterhead Business Developement Inspection" || RoleName == "Admin PCH  Reporting person II" || RoleName == "BD S and M" || RoleName == "BD S and M  and Operations and Inspection" || RoleName == "Legal" || RoleName == "Accounts")
                            {
                                return RedirectToAction("Dashboard", "AdminDashboard");
                            }
                            else
                            {
                                return RedirectToAction("Welcome", "Login");
                            }

                            if (blnDashboard)
                            {
                                // return RedirectToAction("Dashboard", "AdminDashboard");
                                return RedirectToAction("Welcome", "Login");
                            }
                            else
                            {
                                return RedirectToAction("Welcome", "Login");
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("UserLogin", "Login");
                    }
                    return RedirectToAction("UserLogin", "Login");
                }
                else
                {
                    return RedirectToAction("ChangedPassword", "Login");
                }
            }
            else
            {
                TempData["LoginFailed"] = "Account is inactive, Please contact your Administrator !";
                return View();
            }
        }

        DALUsers objDalCreateUser = new DALUsers();
        public ActionResult Welcome()
        {
            if (Convert.ToString(Session["UserName"]) == null || Convert.ToString(Session["UserName"]) == "")
            {
                return RedirectToAction("UserLogin", "Login");
            }

            DataTable dt = objDalCreateUser.GetFlashMessage(Session["UserID"].ToString());
            string message = "";
            string time = "";

            if (dt != null && dt.Rows.Count > 0)
            {
                message = dt.Rows[0]["Message"].ToString();
                time = dt.Rows[0]["Timer"].ToString();// Assuming "Message" is your column name
            }
            else
            {
                message = "";
                time = "";
            }

            ViewBag.FlashMessage = message;
            ViewBag.Timer = time;

            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        public JsonResult ForgotPasswordSendMail(string UserEmailID, string UserName)
        {
            int Result = 0;
            bool EmailSent = false;
            DataTable dtforgotttempt = new DataTable();
            dtforgotttempt = objDalLogin.CheckAttempts(UserName);
            string forgotattempt = string.Empty;
            if (dtforgotttempt.Rows.Count > 0)
            {
                forgotattempt = Convert.ToString(dtforgotttempt.Rows[0][0].ToString());

            }
            if (forgotattempt.ToString().ToUpper() == "EXCEED")
            {
                return Json("EXCEED", JsonRequestBehavior.AllowGet);
            }

            Result = objDalLogin.SendingEMail(UserEmailID, UserName);

            Login objForgetPass = new Login();
            DataTable DTEmailCheck = new DataTable();
            DTEmailCheck = objDalLogin.CheckEmail(UserEmailID, UserName);
            if (DTEmailCheck.Rows.Count > 0)
            {
                objForgetPass.UserName = Convert.ToString(DTEmailCheck.Rows[0]["UserName"]);
                objForgetPass.Password = Convert.ToString(DTEmailCheck.Rows[0]["UserPassword"]);
            }
            if (!string.IsNullOrEmpty(objForgetPass.Password))
            {
                // EmailSent=SendMail(objForgetPass.Password, UserEmailID);//Live Email Sending Mail
                EmailSent = SendMail("Pass@123", UserEmailID);
            }
            if (Result != 0 && EmailSent == true)
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public bool SendMail(string password, string emailid)//Live Sending Mail Utiliy
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            StringBuilder MailBody = new StringBuilder();
            try
            {

                MailBody.Append("<Center>");
                MailBody.Append("<table border='2' cellpadding='5' cellspacing='5'>");
                MailBody.Append("<tr><td colspan='3' valign='top' bgcolor=' #FFFFFF' style='font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 12px; color: #192F41; padding: 3px; font-weight: bold; text-transform: uppercase;'><strong>Your Password And Email</strong></td></tr>");
                if (emailid != "")
                {
                    MailBody.Append("<tr bgcolor=' #FFFFFF'><td bgcolor=' #FFFFFF'> Email:</td><td bgcolor=' #FFFFFF'>" + emailid.ToString() + "</td></tr>");

                }
                else
                {
                    MailBody.Append("<tr bgcolor=' #FFFFFF'><td bgcolor=' #FFFFFF'> Email:</td><td bgcolor=' #FFFFFF'>" + "-" + "</td></tr>");

                }
                if (password != "")
                {
                    MailBody.Append("<tr bgcolor=' #FFFFFF'><td bgcolor=' #FFFFFF'>Password :</td><td bgcolor=' #FFFFFF'>" + password.ToString() + "</td></tr>");

                }
                else
                {
                    MailBody.Append("<tr bgcolor=' #FFFFFF'><td bgcolor=' #FFFFFF'>Password :</td><td bgcolor=' #FFFFFF'>" + "-" + "</td></tr>");

                }
                MailBody.Append("</table>");
                MailBody.Append("</Center>");



                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();

                mail.To.Add(emailid);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                mail.Subject = "Your TUV Password";
                mail.IsBodyHtml = true;
                mail.Body = MailBody.ToString();


                client.Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString());
                client.Host = ConfigurationManager.AppSettings["smtpserver"].ToString();
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
                client.EnableSsl = true;
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
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


        static string GetBrowserName(string userAgent)
        {
            if (userAgent.Contains("Edg"))
            {
                return "Microsoft Edge";
            }
            if (userAgent.Contains("Chrome"))
            {
                return "Google Chrome";
            }
            else if (userAgent.Contains("Firefox"))
            {
                return "Mozilla Firefox";
            }
            else if (userAgent.Contains("Safari"))
            {
                return "Apple Safari";
            }
            else if (userAgent.Contains("Edge"))
            {
                return "Microsoft Edge";
            }
            else if (userAgent.Contains("MSIE") || userAgent.Contains("Trident"))
            {
                return "Internet Explorer";
            }
            else
            {
                return "Unknown Browser";
            }
        }
        [HttpGet]
        public ActionResult KillOtherSession()
        {

            return View();



        }

        private static string getUserId(string userName)
        {
            DataTable DTLoginDtl = new DataTable();

            SqlConnection con = new SqlConnection(Convert.ToString(System.Configuration.ConfigurationManager.ConnectionStrings["TuvConnection"]));
            string query = "select * from UserCreation where userName = '" + userName + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(DTLoginDtl);
            con.Close();


            string userid = Convert.ToString(DTLoginDtl.Rows[0]["PK_Userid"]);




            return userid;

        }

        [HttpPost]
        public ActionResult KillOtherSession(string Value)
        {
            if (Value == "YES")
            {
                int ChangePass = 0;
                var serializedModel = TempData["SerializedModel"] as string;
                var model = JsonConvert.DeserializeObject<Login>(serializedModel);

                Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
                var sessionId = Session["SessionID"];
                string userId = getUserId(model.UserName);

                Session["SessionLoggedUserID"] = userId;
                Session["LastActivity"] = DateTime.Now;

                string UserAgent = GetBrowserName(Request.UserAgent);

                string host = Request.LogonUserIdentity.Name;
                host = "Unavailabe";
                string IP = model.ipAddr;


                var InfoString = Convert.ToString(host) + "|" + IP + "|" + UserAgent;


                global.SessionModule(model.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "4", Convert.ToString(Session["SessionID"]), InfoString);

                string ReturnString = NewLogin(model);

                string[] Returns = ReturnString.Split(',');



                return RedirectToAction(Returns[1], Returns[0]);
            }
            else
            {


                return RedirectToAction("UserLogin", "Login");
            }

        }

        public string NewLogin(Login login)
        {
            string Action;
            string Controller;
            int FirstTimeLogin = 0;
            System.Data.DataSet ds = new System.Data.DataSet();
            string UsrName = string.Empty;
            string Password = string.Empty;
            string costcentre = string.Empty;
            string fullName = string.Empty;
            string LoginName = string.Empty;
            string EmpCode = string.Empty;
            string Result = string.Empty;
            string EmployeeType = string.Empty;
            string UserID = string.Empty;
            bool IsActive = false;
            bool isLocked = false;
            int attempts = 0;
            string BranchId = "0";
            byte[] IData = null;
            string strPhoto = string.Empty;
            string IBRCompetent = "NO";
            string FK_Role = "";
            string FRM = "";
            //Session.Clear();
            //Session.Abandon();
            //Session.RemoveAll();

            string NewsRead = "0";
            string GalleryRead = "0";

            Session["UserLoginID"] = login.UserName;
            DataTable DTCheckLogin = new DataTable();
            DataTable DTCheckPassword = new DataTable();
            DataTable DTCheckEncryptedPassword = new DataTable();

            string EPassword = "";
            string Pass = "";


            ds = objDalLogin.LoginDetails(login);

            DTCheckLogin = ds.Tables[0];
            if (ds != null && ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
            {
                var value = ds.Tables[4].Rows[0]["FRM"];

                if (value != DBNull.Value)
                {
                    FRM = Convert.ToString(value);
                }
            }
            IBRCompetent = Convert.ToString(DTCheckLogin.Rows[0]["IBRCompetent"]);
            FK_Role = Convert.ToString(DTCheckLogin.Rows[0]["FK_RoleID"]);
            DTCheckPassword = ds.Tables[1];
            DTCheckEncryptedPassword = ds.Tables[1];
            if (DTCheckPassword.Rows.Count > 0)
            {

                EPassword = Convert.ToString(ds.Tables[1].Rows[0]["Password"]);
                Pass = Decrypt(EPassword);

            }

            if (DTCheckLogin.Rows.Count > 0)
            {
                if (ds.Tables[2].Rows.Count > 0) /////Attempts
                {
                    isLocked = Convert.ToBoolean(ds.Tables[2].Rows[0]["isLocked"]);
                    attempts = Convert.ToInt32(ds.Tables[2].Rows[0]["attempts"]);
                }

                IsActive = Convert.ToBoolean(DTCheckLogin.Rows[0]["IsActive"]);


                if (isLocked == true)
                {
                    TempData["LoginFailed"] = "Account have been Locked, Please contact your Administrator !";
                    //return View();
                }
                //else if (DTCheckPassword.Rows.Count == 0)
                else if (Pass != login.Password)
                {

                    TempData["LoginFailed"] = "Incorrect Password, Please try again !";
                    ObjModelUser.attempts = attempts + 1;
                    ObjModelUser.IsLocked = false;
                    if (ObjModelUser.attempts == 5)
                    {
                        ObjModelUser.IsLocked = true;
                    }

                    Result = objDalLogin.UpdateUserLogin(ObjModelUser);
                    //return View();

                }
                else if (IsActive == true) //------if user is active 
                {
                    UsrName = Convert.ToString(DTCheckLogin.Rows[0]["UserName"]);
                    Password = Pass;//Convert.ToString(DTCheckLogin.Rows[0]["Password"]);
                    FirstTimeLogin = Convert.ToInt32(DTCheckLogin.Rows[0]["Flag"]);
                    BranchId = Convert.ToString(DTCheckLogin.Rows[0]["FK_BranchID"]);
                    if (ds.Tables[3].Rows.Count > 0) ////
                    {
                        //// IData = (byte[])(ds.Tables[3].Rows[0]["IData"]);

                        strPhoto = Convert.ToString(ds.Tables[3].Rows[0]["IData"]);

                    }
                    costcentre = Convert.ToString(DTCheckLogin.Rows[0]["Cost_Center"]);
                    EmpCode = Convert.ToString(DTCheckLogin.Rows[0]["EmployeeCode"]);
                    fullName = Convert.ToString(DTCheckLogin.Rows[0]["Fullname"]);
                    LoginName = Convert.ToString(DTCheckLogin.Rows[0]["LoginName"]);
                    EmployeeType = Convert.ToString(DTCheckLogin.Rows[0]["Employee_Type"]);
                    UserID = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);
                    Session["F"] = Convert.ToString(DTCheckLogin.Rows[0]["F"]);
                    Session["L"] = Convert.ToString(DTCheckLogin.Rows[0]["L"]);

                    String LessionLeantRead = null;
                    Session["FirstTimeLogin"] = FirstTimeLogin;
                    Session["UserBranchId"] = BranchId;
                    Session["UserID"] = UserID;
                    Session["EmpCode_"] = Convert.ToString(DTCheckLogin.Rows[0]["EmployementCategory"]);
                    Session["costcentre"] = costcentre;
                    Session["EmpCode"] = EmpCode;
                    Session["fullName"] = fullName;
                    Session["LoginName"] = LoginName;
                    Session["EmployeeType"] = EmployeeType;
                    Session["IData"] = strPhoto;
                    //Session["ISA"] = Convert.ToString(DTCheckLogin.Rows[0]["ISA"]);
                    ObjModelUser.IsLocked = false;
                    ObjModelUser.attempts = 0;

                    ds = objDalLogin.NewsGalleryDetails(Session["UserID"].ToString());

                    NewsRead = ds.Tables[0].Rows[0][0].ToString();
                    GalleryRead = ds.Tables[1].Rows[0][0].ToString();
                    LessionLeantRead = ds.Tables[2].Rows[0][0].ToString();

                    Session["NewsRead"] = NewsRead;
                    Session["GalleryRead"] = GalleryRead;
                    Session["LessionLeantRead"] = LessionLeantRead;

                    Result = objDalLogin.UpdateUserLogin(ObjModelUser);
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;


                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                }
                else if (IsActive == false) //----------If User deactivated then Below meassage will appar
                {
                    TempData["LoginFailed"] = "Account haven't been activated yet, Please contact your Admin Department!";
                    //return View();
                }
            }
            else if (DTCheckLogin.Rows.Count == 0) //----------If User not available in sysytem in showing this message 
            {

                TempData["LoginFailed"] = "Incorrect Credentials. Please check your Username and Try again..!";


                //return View();
            }
            DataTable DTCheckRole = new DataTable();
            DTCheckRole = objDalLogin.CheckRole(login);
            if (DTCheckRole.Rows.Count > 0)
            {
                Session["RoleName"] = Convert.ToString(DTCheckRole.Rows[0]["RoleName"]);
                Session["RoleID"] = Convert.ToString(DTCheckRole.Rows[0]["UserRoleID"]);
                //Added By Satish Pawar on 09 June 2023
                Session["IsApprover"] = Convert.ToString(DTCheckRole.Rows[0]["IsApprover"]);
                Session["IsOPE"] = Convert.ToString(DTCheckRole.Rows[0]["OPE"]);
            }

            string IsApprover = Session["IsApprover"].ToString();
            string IsOPE = Session["IsOPE"].ToString();
            string RoleName = Session["RoleName"].ToString();

            //added by nikita on 08042024
            DataTable GetObsType = new DataTable();
            string pkuserid = Session["UserID"].ToString();
            GetObsType = objDalLogin.CheckObs(pkuserid);
            Session["obsId"] = Convert.ToString(GetObsType.Rows[0]["SOCode"]);
            Session["obsName"] = Convert.ToString(GetObsType.Rows[0]["OBSName"]);
            Session["IsMentor"] = Convert.ToString(GetObsType.Rows[0]["IsMentor"]);

            //end

            //added by nikita on 19042024
            DataTable specialservice = new DataTable();
            specialservice = objDalLogin.Specialservice(pkuserid);
            Session["specialservice"] = Convert.ToString(specialservice.Rows[0]["specialServices"]);

            //end

            //Added By Sagar Panigrahi
            var List = objDalLogin.GetAccessMenuSubMenu();
            var splittedMenuId = List.FkMenuId.Split(',').Select(int.Parse).ToList();
            var splittedSubMenuId = List.FkSubMenuId.Split(',').Select(int.Parse).ToList();
            var splittedSubSubMenuId = List.FkSubSubMenuIds.Split(',').Select(int.Parse).ToList();

            DataTable DTMenuRightList = new DataTable();
            List<MenuRights> lstMenuLst = new List<MenuRights>();
            lstMenuLst = objDalLogin.GetMenuRights();
            var DistinctMenu = lstMenuLst.GroupBy(s => s.MenuID).Select(s => s.First()).Where(s => s.MenuName != "NA").ToList();
            var MenuList = new List<MenuListDTO>();
            bool blnDashboard = false;
            foreach (var singleMenu in DistinctMenu)
            {
                var Menu = new MenuListDTO();
                Menu.MainMenuId = singleMenu.MenuID.Value;
                Menu.MainMenuName = singleMenu.MenuName;
                Menu.Action = singleMenu.MainMenuAction; // Added By Sagar Panigrahi
                Menu.Controller = singleMenu.MainMenuController; // Added By Sagar Panigrahi


                //var distinctSubMenu = lstMenuLst.GroupBy(s => s.SubMenuID).Select(s => s.First()).Where(s => s.FK_MainMenuID == Menu.MainMenuId && s.SubMenuName != "NA").ToList();
                var distinctSubMenu = lstMenuLst.GroupBy(s => s.SubMenuID).Select(s => s.First()).Where(s => s.FK_MainMenuID == Menu.MainMenuId && s.SubMenuName != "NA").OrderBy(s => s.OrderNo).ToList();

                {
                    foreach (var singleSubMenu in distinctSubMenu)
                    {
                        var SubMenuList = new SubMenuList();
                        SubMenuList.Id = singleSubMenu.SubMenuID.Value;
                        SubMenuList.Name = singleSubMenu.SubMenuName;
                        SubMenuList.Action = singleSubMenu.SubMenuAction; // Changed By Sagar Panigrahi
                        SubMenuList.Controller = singleSubMenu.SubMenuController; // Changed By Sagar Panigrahi
                        var distinctSubSubMenu = lstMenuLst.GroupBy(s => s.SubSubMenuID).Select(s => s.First()).Where(s => s.FK_SubMenuID == singleSubMenu.SubMenuID && s.SubSubMenuName != "NA").ToList();
                        foreach (var singleSubSubMenu in distinctSubSubMenu)
                        {
                            var SubSubMenuList = new SubSubMenuList();
                            SubSubMenuList.SubSubMenuId = singleSubSubMenu.SubSubMenuID.Value;
                            SubSubMenuList.SubSubMenuName = singleSubSubMenu.SubSubMenuName;
                            SubSubMenuList.Action = singleSubSubMenu.SubSubMenuAction; // Changed By Sagar Panigrahi
                            SubSubMenuList.Controller = singleSubSubMenu.SubSubMenuController; // Changed By Sagar Panigrahi

                            //Actual Comment Start
                            //var DistinctChild = lstMenuLst.GroupBy(s => s.ChiledMenu).Select(s => s.First()).Where(s => s.ChiledMenu != "NA" && s.SubSubMenuName == singleSubSubMenu.SubSubMenuName).ToList();
                            //foreach (var singleChild in DistinctChild)
                            //{
                            //    var keyValue = new ChildMenuList();
                            //    keyValue.ChildName = singleChild.ChiledMenu;
                            //    keyValue.Action = singleChild.Action;
                            //    keyValue.Controller = singleChild.Controller;
                            //    bool isInchildList = splittedSubSubMenuId.IndexOf(singleChild.ChildMenuID.Value) != -1;
                            //    if(isInchildList == true)
                            //    {
                            //        SubSubMenuList.ChildMenu.Add(keyValue);
                            //    }
                            //}
                            //Actual Comment End

                            bool isInSubSubMenuList = splittedSubSubMenuId.IndexOf(SubSubMenuList.SubSubMenuId) != -1;
                            if (isInSubSubMenuList == true)
                            {
                                SubMenuList.SubSubmenuList.Add(SubSubMenuList);
                            }
                        }
                        bool isInSubMenuList = splittedSubMenuId.IndexOf(SubMenuList.Id) != -1;
                        if (isInSubMenuList == true)
                        {
                            Menu.SubMenuList.Add(SubMenuList);
                        }
                    }
                }
                bool isInMainMenuList = splittedMenuId.IndexOf(Menu.MainMenuId) != -1;
                if (isInMainMenuList == true)
                {
                    if (Menu.MainMenuName.Contains("Dashboard"))
                    {
                        blnDashboard = true;
                    }
                    MenuList.Add(Menu);
                }
            }


            #region Vaibhav 17/07/2024
            if (Session["IsMentor"].ToString() == "Yes")
            {
                if (Session["RoleName"].ToString() != "QHSE")
                {
                    foreach (var menu in MenuList)
                    {
                        if (menu.MainMenuName == "My Activity")
                        {
                            // List<SubMenuList> _subMenuList_VS = new List<SubMenuList>();
                            // var subMenuListOPE = new SubMenuList();
                            // var subMenuListOPE1 = new SubMenuList();
                            // var subMenuListOPE2 = new SubMenuList();

                            // subMenuListOPE.Action = "AllIvrReports";
                            // subMenuListOPE.Controller = "MISInspectionRecords";
                            // subMenuListOPE.Id = 86;
                            // subMenuListOPE.Name = "Mentor - Inspection Visit Reports";
                            // _subMenuList_VS.Add(subMenuListOPE);
                            // menu.SubMenuList.Add(subMenuListOPE);

                            // subMenuListOPE1.Action = "IRNReports";
                            //subMenuListOPE1.Controller = "MisInspectionReleaseNote";
                            //subMenuListOPE1.Id = 86;
                            // subMenuListOPE1.Name = "Mentor - Inspection Release Notes";
                            // _subMenuList_VS.Add(subMenuListOPE1);
                            // menu.SubMenuList.Add(subMenuListOPE1);

                            // subMenuListOPE2.Action = "ListNCr";
                            // subMenuListOPE2.Controller = "MISNCR";
                            // subMenuListOPE2.Id = 86;
                            // subMenuListOPE2.Name = "Mentor - Non Conformity Reports";
                            // _subMenuList_VS.Add(subMenuListOPE2);
                            // menu.SubMenuList.Add(subMenuListOPE2);



                        }
                    }
                }


            }
            #endregion



            //Added By Satish Pawar on 09 June 2023
            if (IsApprover == "Yes")
            {
                string existsMenu = "No";
                foreach (var menu in MenuList)
                {
                    if (menu.MainMenuName == "Expense")
                    {

                        existsMenu = "Yes";

                        List<SubMenuList> _subMenuList = new List<SubMenuList>();


                        if (IsOPE.ToUpper() == "YES")
                        {
                            var subMenuListOPE = new SubMenuList();
                            subMenuListOPE.Action = "OPE_Report";
                            subMenuListOPE.Controller = "MISOPEReport";
                            subMenuListOPE.Id = 27;
                            subMenuListOPE.Name = "OPE Report";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuListOPE);
                            menu.SubMenuList.Add(subMenuListOPE);
                        }
                        if (RoleName != "QHSE")
                        {
                            var subMenuList1 = new SubMenuList();
                            subMenuList1.Action = "GenerateVoucherApproval";
                            subMenuList1.Controller = "GenerateVoucher";
                            subMenuList1.Id = 74;
                            subMenuList1.Name = "Expense Approval";
                            _subMenuList.Add(subMenuList1);
                            menu.SubMenuList.Add(subMenuList1);

                        }


                        var subMenuList = new SubMenuList();
                        subMenuList.Action = "OPE_Approval";
                        subMenuList.Controller = "MISOPEReport";
                        subMenuList.Id = 72;
                        subMenuList.Name = "OPE Approval";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuList);
                        menu.SubMenuList.Add(subMenuList);


                        var subMenuList12 = new SubMenuList(); //10112025

                        subMenuList12.Action = "GetOpApproval";
                        subMenuList12.Controller = "OPE";
                        subMenuList12.Id = 107;
                        subMenuList12.Name = "New OPE Approval";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuList12);

                        menu.SubMenuList.Add(subMenuList12);

                        //added by nikita on 15032024
                        var subMenuListVoucher = new SubMenuList();
                        subMenuListVoucher.Action = "GenerateVoucher";
                        subMenuListVoucher.Controller = "GenerateVoucher";
                        subMenuListVoucher.Id = 73;
                        subMenuListVoucher.Name = "Expense Report";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuListVoucher);
                        menu.SubMenuList.Add(subMenuListVoucher);





                        //menu.SubMenuList = _subMenuList;


                        //MenuList.Add(menu);
                        //break;
                    }
                    if (menu.MainMenuName == "My Activity")
                    {

                        List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                        var subMenuList3 = new SubMenuList();
                        subMenuList3.Id = 103;
                        subMenuList3.Name = "Branch Admins";
                        _subMenuList1.Add(subMenuList3);
                        menu.SubMenuList.Add(subMenuList3);


                        var SubsubMenuList1 = new SubSubMenuList();
                        SubsubMenuList1.Action = "LeaveManagementApprover";
                        SubsubMenuList1.Controller = "Leave";
                        SubsubMenuList1.SubSubMenuId = 286;
                        SubsubMenuList1.SubSubMenuName = "Approve Leave";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList1);

                        var SubsubMenuList2 = new SubSubMenuList();
                        SubsubMenuList2.Action = "CreateAllUser";
                        SubsubMenuList2.Controller = "Users";
                        SubsubMenuList2.SubSubMenuId = 287;
                        SubsubMenuList2.SubSubMenuName = "Manage All Users";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                        //added by shrutika salve 06-062025

                        var SubsubMenuList3 = new SubSubMenuList();
                        SubsubMenuList3.Action = "CallsReview";
                        SubsubMenuList3.Controller = "CallsMaster";
                        SubsubMenuList3.SubSubMenuId = 290;
                        SubsubMenuList3.SubSubMenuName = "Approve Attention-Required Calls";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList3);


                        var SubsubMenuList4 = new SubSubMenuList();
                        SubsubMenuList4.Action = "CompentencyApproval";
                        SubsubMenuList4.Controller = "CompentencyMetrixView";
                        SubsubMenuList4.SubSubMenuId = 291;
                        SubsubMenuList4.SubSubMenuName = "Compentency Approval";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList4);

                        //Added by Adesh Sawant on 22/7/2025
                        var SubsubMenuList5 = new SubSubMenuList();
                        SubsubMenuList5.Action = "FlexMessage";
                        SubsubMenuList5.Controller = "CompentencyMetrixView";
                        SubsubMenuList5.SubSubMenuId = 301;
                        SubsubMenuList5.SubSubMenuName = "Add Flash Message";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList5);

                    }

                    //menu.MainMenuName == "";
                }
                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";
                    List<SubMenuList> _subMenuList = new List<SubMenuList>();


                    if (IsOPE.ToUpper() == "YES")
                    {
                        var subMenuListOPE = new SubMenuList();
                        subMenuListOPE.Action = "OPE_Report";
                        subMenuListOPE.Controller = "MISOPEReport";
                        subMenuListOPE.Id = 27;
                        subMenuListOPE.Name = "OPE Report";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuListOPE);

                    }

                    var subMenuListVoucher = new SubMenuList();
                    subMenuListVoucher.Action = "GenerateVoucher";
                    subMenuListVoucher.Controller = "GenerateVoucher";
                    subMenuListVoucher.Id = 73;
                    subMenuListVoucher.Name = "Expense Report";
                    //subMenuList.SubSubmenuList = 0;
                    _subMenuList.Add(subMenuListVoucher);
                    if (RoleName != "QHSE")
                    {
                        var subMenuList1 = new SubMenuList();
                        subMenuList1.Action = "GenerateVoucherApproval";
                        subMenuList1.Controller = "GenerateVoucher";
                        subMenuList1.Id = 74;
                        subMenuList1.Name = "Expense Approval";
                        _subMenuList.Add(subMenuList1);

                    }

                    var subMenuList = new SubMenuList();

                    subMenuList.Action = "OPE_Approval";
                    subMenuList.Controller = "MISOPEReport";
                    subMenuList.Id = 72;
                    subMenuList.Name = "OPE Approval";
                    //subMenuList.SubSubmenuList = 0;
                    _subMenuList.Add(subMenuList);


                    menuDTO.SubMenuList = _subMenuList;


                    var subMenuList12 = new SubMenuList(); //10112025

                    subMenuList12.Action = "GetOpApproval";
                    subMenuList12.Controller = "OPE";
                    subMenuList12.Id = 107;
                    subMenuList12.Name = "New OPE Approval";
                    //subMenuList.SubSubmenuList = 0;
                    _subMenuList.Add(subMenuList12);

                    menuDTO.SubMenuList = _subMenuList;


                    MenuList.Add(menuDTO);







                }


            }
            else if (Session["UserID"].ToString() == "TUV00558")
            {
                string existsMenu = "No";
                foreach (var menu in MenuList)
                {
                    if (menu.MainMenuName == "My Activity")
                    {



                        List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                        var subMenuList3 = new SubMenuList();
                        subMenuList3.Id = 103;
                        subMenuList3.Name = "Branch Admins";
                        _subMenuList1.Add(subMenuList3);
                        menu.SubMenuList.Add(subMenuList3);


                        var SubsubMenuList1 = new SubSubMenuList();
                        SubsubMenuList1.Action = "LeaveManagementApprover";
                        SubsubMenuList1.Controller = "Leave";
                        SubsubMenuList1.SubSubMenuId = 286;
                        SubsubMenuList1.SubSubMenuName = "Approve Leave";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList1);

                        var SubsubMenuList2 = new SubSubMenuList();
                        SubsubMenuList2.Action = "CreateAllUser";
                        SubsubMenuList2.Controller = "Users";
                        SubsubMenuList2.SubSubMenuId = 287;
                        SubsubMenuList2.SubSubMenuName = "Manage All Users";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                        //added by shrutika salve 06-062025

                        var SubsubMenuList3 = new SubSubMenuList();
                        SubsubMenuList3.Action = "CallsReview";
                        SubsubMenuList3.Controller = "CallsMaster";
                        SubsubMenuList3.SubSubMenuId = 290;
                        SubsubMenuList3.SubSubMenuName = "Approve Attention-Required Calls";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList3);

                    }
                }

                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";

                    List<SubMenuList> _subMenuList = new List<SubMenuList>();

                    var subMenuList1 = new SubMenuList();
                    subMenuList1.Action = "GenerateVoucher";
                    subMenuList1.Controller = "GenerateVoucher";
                    subMenuList1.Id = 74;
                    subMenuList1.Name = "Expense Report";
                    _subMenuList.Add(subMenuList1);

                    if (IsOPE.ToUpper() == "YES")
                    {
                        var subMenuListOPE = new SubMenuList();
                        subMenuListOPE.Action = "OPE_Report";
                        subMenuListOPE.Controller = "MISOPEReport";
                        subMenuListOPE.Id = 27;
                        subMenuListOPE.Name = "OPE Report";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuListOPE);

                    }







                    menuDTO.SubMenuList = _subMenuList;

                    MenuList.Add(menuDTO);
                }

            }
            else if (Session["UserID"].ToString() == "TUV00303" || FK_Role == "46" || FK_Role == "47" || FK_Role == "48" || FK_Role == "49" || FK_Role == "51" || FK_Role == "53")
            {
                string existsMenu = "No";
                foreach (var menu in MenuList)
                {

                    if (menu.MainMenuName == "MIS")
                    {



                        List<SubMenuList> _subMenuList1 = new List<SubMenuList>();
                        var subMenuList3 = new SubMenuList();
                        subMenuList3.Id = 104;
                        subMenuList3.Name = "IBR";
                        _subMenuList1.Add(subMenuList3);
                        menu.SubMenuList.Add(subMenuList3);

                        var SubsubMenuList2 = new SubSubMenuList();
                        SubsubMenuList2.Action = "MISVisitMonitoringdata";
                        SubsubMenuList2.Controller = "VisitReport";
                        SubsubMenuList2.SubSubMenuId = 299;
                        SubsubMenuList2.SubSubMenuName = "Mfr. Monitoring Record (IBR)";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList2);

                        var SubsubMenuList1 = new SubSubMenuList();
                        SubsubMenuList1.Action = "MISGetPersonalDiaryData";
                        SubsubMenuList1.Controller = "VisitReport";
                        SubsubMenuList1.SubSubMenuId = 297;
                        SubsubMenuList1.SubSubMenuName = "Mfr. Competent Person's Diary(IBR)";
                        subMenuList3.SubSubmenuList.Add(SubsubMenuList1);



                        ////added by shrutika salve 06-062025

                        //var SubsubMenuList3 = new SubSubMenuList();
                        //SubsubMenuList3.Action = "CallsReview";
                        //SubsubMenuList3.Controller = "CallsMaster";
                        //SubsubMenuList3.SubSubMenuId = 290;
                        //SubsubMenuList3.SubSubMenuName = "Approve Attention-Required Calls";
                        //subMenuList3.SubSubmenuList.Add(SubsubMenuList3);

                    }

                }

                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";

                    List<SubMenuList> _subMenuList = new List<SubMenuList>();

                    var subMenuList1 = new SubMenuList();
                    subMenuList1.Action = "GenerateVoucher";
                    subMenuList1.Controller = "GenerateVoucher";
                    subMenuList1.Id = 74;
                    subMenuList1.Name = "Expense Report";
                    _subMenuList.Add(subMenuList1);

                    if (IsOPE.ToUpper() == "YES")
                    {
                        var subMenuListOPE = new SubMenuList();
                        subMenuListOPE.Action = "OPE_Report";
                        subMenuListOPE.Controller = "MISOPEReport";
                        subMenuListOPE.Id = 27;
                        subMenuListOPE.Name = "OPE Report";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuListOPE);

                    }







                    menuDTO.SubMenuList = _subMenuList;

                    MenuList.Add(menuDTO);
                }

            }

            else if (RoleName == "InspectionO" || RoleName == "OperationO")
            {

            }

            else
            {
                #region Vaibhav 17/07/2024
                if (Session["IsMentor"].ToString() == "Yes")
                {
                    if (Session["RoleName"].ToString() != "QHSE")
                    {
                        foreach (var menu in MenuList)
                        {
                            if (menu.MainMenuName == "My Activity")
                            {


                                List<SubMenuList> _subMenuList_VS = new List<SubMenuList>();
                                var subMenuListOPE = new SubMenuList();
                                var subMenuListOPE1 = new SubMenuList();
                                var subMenuListOPE2 = new SubMenuList();

                                subMenuListOPE.Action = "AllIvrReports";
                                subMenuListOPE.Controller = "MISInspectionRecords";
                                subMenuListOPE.Id = 86;
                                subMenuListOPE.Name = "Mentor - Inspection Visit Reports";
                                _subMenuList_VS.Add(subMenuListOPE);
                                menu.SubMenuList.Add(subMenuListOPE);

                                subMenuListOPE1.Action = "IRNReports";
                                subMenuListOPE1.Controller = "MisInspectionReleaseNote";
                                subMenuListOPE1.Id = 86;
                                subMenuListOPE1.Name = "Mentor - Inspection Release Notes";
                                _subMenuList_VS.Add(subMenuListOPE1);
                                menu.SubMenuList.Add(subMenuListOPE1);

                                subMenuListOPE2.Action = "ListNCr";
                                subMenuListOPE2.Controller = "MISNCR";
                                subMenuListOPE2.Id = 86;
                                subMenuListOPE2.Name = "Mentor - Non Conformity Reports";
                                _subMenuList_VS.Add(subMenuListOPE2);
                                menu.SubMenuList.Add(subMenuListOPE2);




                            }
                        }

                    }

                }
                #endregion



                string existsMenu = "No";
                foreach (var menu in MenuList)
                {
                    #region vaibhav
                    //Vaibhav 17/07/2024
                    //List<SubMenuList> _subMenuList132 = new List<SubMenuList>();

                    //if (Session["IsMentor"].ToString() == "Yes")
                    //{
                    //    if (menu.MainMenuName == "MIS")
                    //    {
                    //        var subMenuListOPE = new SubMenuList();
                    //        subMenuListOPE.Action = "AllIvrReports";
                    //        subMenuListOPE.Controller = "MISInspectionRecords";
                    //        subMenuListOPE.Id = 8;
                    //        subMenuListOPE.Name = "Inspection Visit Reports";
                    //        _subMenuList132.Add(subMenuListOPE);
                    //        menu.SubMenuList.Add(subMenuListOPE);
                    //    }




                    //}
                    #endregion

                    if (menu.MainMenuName == "Expense")
                    {



                        existsMenu = "Yes";

                        List<SubMenuList> _subMenuList = new List<SubMenuList>();

                        var subMenuList1 = new SubMenuList();
                        subMenuList1.Action = "GenerateVoucher";
                        subMenuList1.Controller = "GenerateVoucher";
                        subMenuList1.Id = 74;
                        subMenuList1.Name = "Expense Report";
                        _subMenuList.Add(subMenuList1);
                        menu.SubMenuList.Add(subMenuList1);

                        if (IsOPE.ToUpper() == "YES")
                        {
                            var subMenuListOPE = new SubMenuList();
                            subMenuListOPE.Action = "OPE_Report";
                            subMenuListOPE.Controller = "MISOPEReport";
                            subMenuListOPE.Id = 27;
                            subMenuListOPE.Name = "OPE Report";
                            //subMenuList.SubSubmenuList = 0;
                            _subMenuList.Add(subMenuListOPE);
                            menu.SubMenuList.Add(subMenuListOPE);
                        }

                        //menu.SubMenuList = _subMenuList;


                        //MenuList.Add(menu);
                        break;
                    }
                    //menu.MainMenuName == "";
                }
                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";

                    List<SubMenuList> _subMenuList = new List<SubMenuList>();

                    var subMenuList1 = new SubMenuList();
                    subMenuList1.Action = "GenerateVoucher";
                    subMenuList1.Controller = "GenerateVoucher";
                    subMenuList1.Id = 74;
                    subMenuList1.Name = "Expense Report";
                    _subMenuList.Add(subMenuList1);

                    if (IsOPE.ToUpper() == "YES")
                    {
                        var subMenuListOPE = new SubMenuList();
                        subMenuListOPE.Action = "OPE_Report";
                        subMenuListOPE.Controller = "MISOPEReport";
                        subMenuListOPE.Id = 27;
                        subMenuListOPE.Name = "OPE Report";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuListOPE);

                    }







                    menuDTO.SubMenuList = _subMenuList;

                    MenuList.Add(menuDTO);
                }


            }
            //===========================
            //Added By Satish Pawar on 21 June 2023
            if (RoleName == "Accounts")
            {
                string existsMenu = "No";
                foreach (var menu in MenuList)
                {
                    if (menu.MainMenuName == "Expense")
                    {

                        existsMenu = "Yes";

                        List<SubMenuList> _subMenuList = new List<SubMenuList>();

                        var subMenuList1 = new SubMenuList();
                        subMenuList1.Action = "GenerateVoucherApproval";
                        subMenuList1.Controller = "GenerateVoucher";
                        subMenuList1.Id = 74;
                        subMenuList1.Name = "Expense Approval";
                        _subMenuList.Add(subMenuList1);
                        menu.SubMenuList.Add(subMenuList1);

                        break;
                    }
                    //menu.MainMenuName == "";
                }
                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";
                    List<SubMenuList> _subMenuList = new List<SubMenuList>();

                    var subMenuList1 = new SubMenuList();
                    subMenuList1.Action = "GenerateVoucherApproval";
                    subMenuList1.Controller = "GenerateVoucher";
                    subMenuList1.Id = 74;
                    subMenuList1.Name = "Expense Approval";
                    _subMenuList.Add(subMenuList1);

                    menuDTO.SubMenuList = _subMenuList;

                    MenuList.Add(menuDTO);
                }


            }
            //End Added Code By Satish Pawar on 21 June 2023

            //added by nikita on 28122023
            if (RoleName == "Expenses approver")
            {
                string existsMenu = "No";
                foreach (var menu in MenuList)
                {
                    if (menu.MainMenuName == "Expense")
                    {

                        existsMenu = "Yes";

                        List<SubMenuList> _subMenuList = new List<SubMenuList>();

                        var subMenuList1 = new SubMenuList();
                        subMenuList1.Action = "GenerateVoucherApproval";
                        subMenuList1.Controller = "GenerateVoucher";
                        subMenuList1.Id = 74;
                        subMenuList1.Name = "Expense Approval";
                        _subMenuList.Add(subMenuList1);
                        menu.SubMenuList.Add(subMenuList1);

                        //break;
                        var subMenuList = new SubMenuList();
                        subMenuList.Action = "OPE_Approval";
                        subMenuList.Controller = "MISOPEReport";
                        subMenuList.Id = 72;
                        subMenuList.Name = "OPE Approval";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuList);
                        menu.SubMenuList.Add(subMenuList);


                        var subMenuList12 = new SubMenuList(); //10112025

                        subMenuList12.Action = "GetOpApproval";
                        subMenuList12.Controller = "OPE";
                        subMenuList12.Id = 107;
                        subMenuList12.Name = "New OPE Approval";
                        //subMenuList.SubSubmenuList = 0;
                        _subMenuList.Add(subMenuList12);
                        menu.SubMenuList.Add(subMenuList12);








                        break;


                    }
                    //menu.MainMenuName == "";
                }
                if (existsMenu == "No")
                {
                    MenuListDTO menuDTO = new MenuListDTO();
                    menuDTO.Action = "NA";
                    menuDTO.Controller = "NA";
                    menuDTO.MainMenuId = 9;
                    menuDTO.MainMenuName = "Expense";
                    List<SubMenuList> _subMenuList = new List<SubMenuList>();

                    var subMenuList1 = new SubMenuList();
                    subMenuList1.Action = "GenerateVoucherApproval";
                    subMenuList1.Controller = "GenerateVoucher";
                    subMenuList1.Id = 74;
                    subMenuList1.Name = "Expense Approval";
                    _subMenuList.Add(subMenuList1);

                    menuDTO.SubMenuList = _subMenuList;

                    MenuList.Add(menuDTO);

                    var subMenuList = new SubMenuList();
                    subMenuList.Action = "OPE_Approval";
                    subMenuList.Controller = "MISOPEReport";
                    subMenuList.Id = 72;
                    subMenuList.Name = "OPE Approval";
                    //subMenuList.SubSubmenuList = 0;
                    _subMenuList.Add(subMenuList);


                    var subMenuList12 = new SubMenuList(); //10112025

                    subMenuList12.Action = "GetOpApproval";
                    subMenuList12.Controller = "OPE";
                    subMenuList12.Id = 107;
                    subMenuList12.Name = "New OPE Approval";
                    //subMenuList.SubSubmenuList = 0;
                    _subMenuList.Add(subMenuList12);




                    MenuList.Add(menuDTO);
                }


            }

            //end of nikita code

            var sdsdsd = MenuList;

            Session["NewMenuList"] = MenuList;

            var GroupBydata = lstMenuLst.GroupBy(car => car.MenuID).Distinct().ToList();

            var GroupByDataSubMenu = lstMenuLst.GroupBy(cars => cars.SubMenuID).Distinct().ToList();

            var GroupByDataSubSubMenu = lstMenuLst.GroupBy(carss => carss.SubSubMenuID).Distinct().ToList();

            Session["GroupBydata"] = GroupBydata as List<IGrouping<int?, MenuRights>>;
            Session["GroupByDataSubMenu"] = GroupByDataSubMenu as List<IGrouping<int?, MenuRights>>;
            Session["GroupByDataSubSubMenu"] = GroupByDataSubSubMenu as List<IGrouping<int?, MenuRights>>;

            Session.Add("MenuList", lstMenuLst);

            List<MenuRights> lstMainMenuLst = new List<MenuRights>();
            lstMainMenuLst = objDalLogin.MainMenuList();
            Session.Add("MainMenuList", lstMainMenuLst);



            //if (DTCheckLogin.Rows.Count == 0)
            //{

            //        ViewBag.UserID = "Entered Invalid User Id....!";
            //        return View();
            //}
            if (UsrName != login.UserName && Password != login.Password)
            {
                ViewBag.UserID = "Invalid User...";
                ViewBag.Password = "Invalid Password...";
                //return View();
            }


            if (UsrName.ToUpper() != login.UserName.ToUpper())
            {
                ViewBag.UserID = "Invalid User...";
                //return View();
            }
            string pwd = Pass;//Convert.ToString(DTCheckLogin.Rows[0]["Password"]);
            if (pwd != login.Password)
            {
                ViewBag.Password = "Invalid Password...";
                //return View();
            }
            else
            {
                //    FormsAuthentication.SetAuthCookie(login.UserName, true);
                Session["EmailID"] = Convert.ToString(DTCheckLogin.Rows[0]["EmailID"]);
                Session["UserIDs"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);
                Session["LoginID"] = Convert.ToString(login.UserName);
                Session["UserName"] = Convert.ToString(DTCheckLogin.Rows[0]["UserName"]);
                login.UserName = Convert.ToString(Session["UserLoginID"]);
                Session["ID"] = Convert.ToString(DTCheckLogin.Rows[0]["Id"]);
            }

            if (login.UserName != "" && login.UserName != null)
            {
                Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
                Session["LastActivity"] = DateTime.Now;

                string UserAgent = GetBrowserName(Request.UserAgent);

                string IP = login.ipAddr;

                if (IP == null)
                {
                    IP = "0.0.0.0";
                }

                var host = "Unavailabe";
                //System.Environment.GetEnvironmentVariable("COMPUTERNAME");
                //
                //string host = Request.LogonUserIdentity.Name;

                var InfoString = Convert.ToString(host) + "|" + IP + "|" + UserAgent;
                ///changes by Savio S for single login on 5/2/2024
                global.SessionModule(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "1", Convert.ToString(Session["SessionID"]), InfoString);
                global.ActivityPing(login.UserName, Convert.ToString(Session["SessionLoggedUserID"]), "Session Started");
                ///changes by Savio S for single login on 5/2/2024

                if (FirstTimeLogin == 0)
                {
                    int Flag = 1;
                    Result = objDalLogin.FirstTimeUserLogin(Flag);
                    Action = "ChangedPassword";
                    Controller = "Logout";
                    return Controller + "," + Action;
                    //return RedirectToAction("ChangedPassword", "Logout");
                }
                else if (FirstTimeLogin == 1)
                {
                    if (FRM == "0")
                    {
                        Action = "GetDashBoard";
                        Controller = "Login";
                        return Controller + "," + Action;
                    }
                    else if (RoleName == "BD S and M and Operations" || RoleName == "BD S and M and Operations" || RoleName == "Clusterhead Industrial Inspection" || RoleName == "Clusterhead Renewable Building and Construction" || RoleName == "BD S and M International" || RoleName == "BD S and M International" || RoleName == "Clusterhead Special Services" || RoleName == "Clusterhead Business Developement Inspection" || RoleName == "Admin PCH  Reporting person II" || RoleName == "BD S and M" || RoleName == "BD S and M  and Operations and Inspection" || RoleName == "Legal" || RoleName == "Accounts")
                    {
                        Action = "Dashboard";
                        Controller = "AdminDashboard";
                        return Controller + "," + Action;
                        //return RedirectToAction("Dashboard", "AdminDashboard");
                    }
                    else
                    {
                        Action = "Welcome";
                        Controller = "Login";
                        return Controller + "," + Action;
                        //return RedirectToAction("Welcome", "Login");
                    }

                    if (blnDashboard)
                    {
                        Action = "Welcome";
                        Controller = "Login";
                        return Controller + "," + Action;
                        // return RedirectToAction("Dashboard", "AdminDashboard");
                        //return RedirectToAction("Welcome", "Login");
                    }
                    else
                    {
                        Action = "Welcome";
                        Controller = "Login";
                        return Controller + "," + Action;
                        //return RedirectToAction("Welcome", "Login");
                    }
                }
            }
            else
            {
                Action = "UserLogin";
                Controller = "Login";
                return Controller + "," + Action;
                //return RedirectToAction("UserLogin", "Login");
            }
            Action = "UserLogin";
            Controller = "Login";
            return Controller + "," + Action;
            //return RedirectToAction("UserLogin", "Login");
        }


        public int UpdateActivityTime()
        {
            DateTime lastActivity = Convert.ToDateTime(Session["LastActivity"]);
            TimeSpan timeSinceLastActivity = DateTime.Now - lastActivity;
            var value = (int)timeSinceLastActivity.TotalSeconds;

            Session["LastActivity"] = DateTime.Now;

            return value;
        }

        ///this returns LastActivity Time of the system and if more than 20 min then logsout and sends the value back.
        public int returnLastActivity()
        {
            DateTime lastActivity = Convert.ToDateTime(Session["LastActivity"]);
            TimeSpan timeSinceLastActivity = DateTime.Now - lastActivity;

            var value = (int)timeSinceLastActivity.TotalSeconds;
            if (value > 1080 && value < 1200)///between 18 and 20 min.
            {
                global.ActivityPing(Convert.ToString(Session["SessionLoggedUserID"]), Convert.ToString(Session["SessionID"]), "Session Idle");
            }

            if (value > 1200)///1200 = 20mins
            {
                LogOut();
            }

            return value;
        }

        public int LogOut()
        {

            //if (FormsAuthentication.IsEnabled)
            //{
            //    FormsAuthentication.SignOut();
            //}
            global.SessionModule(Convert.ToString(Session["UserLoginID"]), Convert.ToString(Session["SessionLoggedUserID"]), "3", Convert.ToString(Session["SessionID"]), null);
            global.ActivityPing(Convert.ToString(Session["SessionLoggedUserID"]), Convert.ToString(Session["SessionID"]), "Session Terminated");


            return 1;
        }

        [HttpGet]
        public ActionResult InductionTool()
        {
            string script = string.Empty;
            string strUrl = ConfigurationManager.AppSettings["InductionPath"].ToString();
            strUrl = strUrl + "?u=" + Session["ID"].ToString();

            try
            {
                script = $"<script>window.open('{strUrl}', '_blank');</script>";

            }
            catch (Exception ex)
            {
                LogFile1(ex.Message.ToString(), "InductionLogin");
            }

            return Content("<script>window.open('https://TIIMES.TUV-india.com/TUVInduction?u=613','_blank');</script>");

        }

        [HttpGet]
        public ActionResult TicketTool()
        {
            string strTicketUrl = ConfigurationManager.AppSettings["TicketTool"].ToString();
            Response.Redirect(strTicketUrl);
            return View();
        }


        public void LogFile1(string strMessage, string strMethodName)
        {
            try
            {
                string strLogPath = ConfigurationManager.AppSettings["LogPath"].ToString();

                string strFileName = "LogFile_" + DateTime.Now.Date.ToString("dd_MM_yyyy") + ".txt";

                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", strLogPath, strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.WriteLine(strMethodName + ":" + strMessage);
                objStreamWriter.Close();
                objFilestream.Close();

            }
            catch (Exception ex)
            {

            }
        }



        [HttpGet]
        public ActionResult ChangedPassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChangedPassword(Login login, string NewPassword)
        {
            Session["SessionID"] = System.Web.HttpContext.Current.Session.SessionID;
            Session["LastActivity"] = DateTime.Now;
            var Pk_userid = Session["SessionLoggedUserID"].ToString();
            //Session["SessionLoggedUserID"] = Convert.ToString(DTCheckLogin.Rows[0]["PK_UserID"]);
            login.UserID = Convert.ToString(System.Web.HttpContext.Current.Session["LoginID"]);
            int result;
            try
            {
                //DataSet dsCheck = objDalLogout.CheckValidUser(Pk_userid);

                //string pwd1 = dsCheck.Tables[0].Rows[0]["UserPassword"].ToString();

                //string pwd = Decrypt(pwd1);

                //if (pwd != OldPassword)
                //{
                //    return Json(new { result = "Redirect", url = Url.Action("ChangedPassword", "Logout") });
                //}
                if (login.NewPassword != login.ConfirmPassword)
                {
                    return Json(new { result = "RedirectPage", url = Url.Action("ChangedPassword", "Logout") });
                }
                else if (login.ConfirmPassword != "" && login.ConfirmPassword != null)
                {
                    result = objDalLogout.ChangePassword(NewPassword, Pk_userid);
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

        public ActionResult GetDashBoard()
        {
            var Pk_userid = Session["SessionLoggedUserID"].ToString();
            DataTable dt = new DataTable();
            dt = objDalLogout.GetData(Pk_userid);
            Declaration dec = new Declaration();
            dec.Name = dt.Rows[0]["Name"].ToString();
            dec.Datediff = dt.Rows[0]["datediff"].ToString();
            dec.Grade = dt.Rows[0]["EmployeeGrade"].ToString();
            dec.Pk_userid = Pk_userid;
            return View(dec);
        }

        public ActionResult GetData()
        {
            string Json1;
            var Pk_userid = Session["SessionLoggedUserID"].ToString();
            DataTable dt = new DataTable();
            dt = objDalLogout.GetData(Pk_userid);
            Declaration dec = new Declaration();
            dec.Name = dt.Rows[0]["Name"].ToString();
            if (dt.Rows.Count > 0)
            {
                Json1 = JsonConvert.SerializeObject(dt);
            }
            else
            {
                Json1 = JsonConvert.SerializeObject("error");
            }
            return Json(Json1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveData()
        {
            try
            {
                // 1. Get the JSON string from FormData
                string jsonData = Request.Form["rowData"];
                string place = Request.Form["place"];
                string Location = Request.Form["Location"];
                string TimeField = Request.Form["TimeField"];
                string Pk_userid = Request.Form["Pk_userid"];
                bool chkDeclaration = Convert.ToBoolean(Request.Form["chkDeclaration"]);
                bool chkRead_Understand = Convert.ToBoolean(Request.Form["chkRead_Understand"]);

                if (Pk_userid != "")
                {
                    if (string.IsNullOrWhiteSpace(jsonData))
                    {
                        return Json(new { success = false, message = "No data received." });
                    }

                    // 2. Deserialize the JSON into a list of objects
                    List<Declaration> workExperiences = JsonConvert.DeserializeObject<List<Declaration>>(jsonData);

                    // 3. Handle the file (selfieImage)
                    HttpPostedFileBase selfieImage = Request.Files["selfieImage"];
                    string filePath = "";
                    if (selfieImage != null && selfieImage.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(selfieImage.FileName);
                        var filname = Pk_userid + '_' + fileName;
                        filePath = Server.MapPath("~/Uploads/" + filname); // Make sure "Uploads" folder exists
                        selfieImage.SaveAs(filePath);
                    }

                    // 4. Save the workExperiences list to the database (pseudo code)
                    DataTable dt = new DataTable();

                    foreach (var item in workExperiences)
                    {
                        dt = objDalLogout.SaveDeclarationData(item, filePath, place, Location, TimeField, chkDeclaration, chkRead_Understand, Pk_userid);
                    }
                    // _db.SaveChanges();
                    string path = GeneratePDF();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Something Went Wrong" });
                }

            }
            catch (Exception ex)
            {
                // Log the error if needed
                return Json(new { success = false, message = ex.Message });
            }
        }

        public string GeneratePDF()
        {
            StringBuilder tableRows = new StringBuilder();
            var Pk_userid = Session["SessionLoggedUserID"].ToString();
            DataTable dt = new DataTable();
            dt = objDalLogout.GetData_PDF(Pk_userid);
            Declaration model = new Declaration();
            if (dt.Rows.Count > 0)
            {

                model.image = Convert.ToString(dt.Rows[0]["image"]);
                model.Address = Convert.ToString(dt.Rows[0]["address1"]);
                model.Name = Convert.ToString(dt.Rows[0]["Name"]);
                model.Branch = Convert.ToString(dt.Rows[0]["Branch_Name"]);
                model.TillDate = Convert.ToString(dt.Rows[0]["Createddate"]);
                model.TimeField = Convert.ToString(dt.Rows[0]["Time_"]);
                model.Datediff = dt.Rows[0]["Createddate"].ToString();
            }
            string bodyHtml = System.IO.File.ReadAllText(Server.MapPath("~/QuotationHtml/Declaration.html"));
            string headerHtml = System.IO.File.ReadAllText(Server.MapPath("~/QuotationHtml/Declaration-Header.html"));
            string footerHtml = System.IO.File.ReadAllText(Server.MapPath("~/QuotationHtml/Declaration-Footer.html"));

            if (dt.Rows.Count > 0)
            {
                int slNo = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    string Designation = dr["Designation"].ToString();
                    string DOJ = dr["DOJ"].ToString();
                    string Organization = dr["Organization"].ToString();
                    string TillDate = dr["TillDate"].ToString();
                    string Full_Part_Time = dr["Full_Part_Time"].ToString();
                    string NatureOfWork = dr["NatureOfWork"].ToString();
                    tableRows.Append("<tr>");
                    tableRows.Append($"<td style='text-align:center;font-size:18px;vertical-align:top;'>{slNo}</td>");
                    tableRows.Append($"<td style='text-align:left;font-size:18px;width:170px;vertical-align:top;'>{DOJ}</td>");
                    tableRows.Append($"<td style='text-align:left;font-size:18px;vertical-align:top;width:170px;'>{TillDate}</td>");
                    //tableRows.Append($"<td style='text-align:left;font-size:11px;'>{date}</td>");
                    tableRows.Append($"<td style='text-align:left;font-size:18px;vertical-align:top;'>{Organization}</td>");
                    tableRows.Append($"<td style='text-align:left;font-size:18px;vertical-align:top;'>{NatureOfWork}</td>");
                    tableRows.Append($"<td style='text-align:left;font-size:18px;vertical-align:top;'>{Full_Part_Time}</td>");
                    //tableRows.Append($"<td>{person}</td>");
                    slNo++;
                    //if (!string.IsNullOrWhiteSpace(status) && !string.IsNullOrWhiteSpace(comment))
                    //{
                }
                //}
                //else
                //{
                //    tableRows.Append("<td></td>");
                //    tableRows.Append("<td></td>");
                //}
                tableRows.Append("</tr>");
            }


            // Replace placeholder in body HTML




            bodyHtml = bodyHtml.Replace("[ExpData]", tableRows.ToString());

            var tableRows_ = @"
            <tr>
                <td style='vertical-align:top; font-size:18px;width:5%'><strong>23.&nbsp;</strong></td>
                <td><p style='font-size:18px;font-family:'TNG Pro';'>I shall declare to TUV India, those assignments undertaken by me or I wish to undertake, apart from the work assignments from TUV India, from time to time. TUV India shall have the right to judge if there is any breach of terms as defined in the procedure for Independence, Impartiality, Integrity & confidentiality - QP / IB / 01.</p></td>
            </tr>
            <tr>
                <td style='vertical-align:top; font-size:18px;'><strong>24.&nbsp;</strong></td>
                <td><p style='font-size:18px;font-family:'TNG Pro';'>I shall never approach clients of TUV India for my company's or my own assignments.</p></td>
            </tr>";
            if (Convert.ToString(Session["EmpCode_"]) == "10" || Convert.ToString(Session["EmpCode_"]) == "11")
            {

                bodyHtml = bodyHtml.Replace("[exportdata1]", tableRows_);

            }
            else
            {
                bodyHtml = bodyHtml.Replace("[exportdata1]", "");
            }

            bodyHtml = bodyHtml.Replace("[Name]", model.Name);
            bodyHtml = bodyHtml.Replace("[Place]", model.Address);
            bodyHtml = bodyHtml.Replace("[Location]", model.Branch);
            bodyHtml = bodyHtml.Replace("[Date]", model.TillDate);
            bodyHtml = bodyHtml.Replace("[Time]", model.TimeField);
            headerHtml = headerHtml.Replace("[Date]", model.Datediff);
            headerHtml = headerHtml.Replace("[Name]", model.Name);


            if (model.image == "")
            {

            }
            else
            {
                string base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(model.image));
                string imgSrc = $"data:image/png;base64,{base64Image}";
                //string base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(model.image));
                //string imgSrc = $"data:image/png;base64,{base64Image}";
                bodyHtml = bodyHtml.Replace("[Picture]", imgSrc);
            }
            // Set SelectPdf license key if needed
            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA=="; // Your license key

            HtmlToPdf converter = new HtmlToPdf
            {
                Options =
        {
            PdfPageSize = PdfPageSize.A4,
            PdfPageOrientation = PdfPageOrientation.Portrait,
            //MarginTop = 70,
            //MarginBottom = 60,
            DisplayHeader = true,
            DisplayFooter = true,
           MarginLeft = 24,  // Changed from 30
        MarginRight = 25  // Changed from 20
            //MarginTop=70
        }
            };


            converter.Header.Height = 70;
            PdfHtmlSection headerSection = new PdfHtmlSection(headerHtml, string.Empty)
            {
                AutoFitHeight = HtmlToPdfPageFitMode.AutoFit,
            };
            converter.Header.Add(headerSection);
            // Footer section
            converter.Footer.Height = 50;

            // ✅ Wrap the footer HTML in a padded div (via code)
            string wrappedFooterHtml = $"<div style='padding-left:0px; padding-right:0px;'>{footerHtml}</div>";

            PdfHtmlSection footerSection = new PdfHtmlSection(wrappedFooterHtml, string.Empty)
            {
                AutoFitHeight = HtmlToPdfPageFitMode.AutoFit
            };
            converter.Footer.Add(footerSection);

            // ✅ Align the footer text with the left margin (30px)
            converter.Footer.Add(new PdfTextSection(
                15, 15, // x = 30 matches left margin
                "Page:",
                new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold)
            ));

            converter.Footer.Add(new PdfTextSection(
                50, 15, // Slightly to the right of "Page:"

                "{page_number} of {total_pages} ",
                new System.Drawing.Font("'TNG Pro'", 9, System.Drawing.FontStyle.Regular)
            ));

            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(bodyHtml);

            doc.Security.CanPrint = true;
            doc.Security.CanEditContent = false;
            doc.Security.CanCopyContent = false;
            doc.Security.CanAssembleDocument = false;
            doc.Security.CanEditAnnotations = false;
            doc.Security.CanFillFormFields = false;
            byte[] pdfBytes = doc.Save();
            doc.Close();
            var folderPath = Server.MapPath("~/Content/Uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string fileName = model.Name + "_Declaration.pdf";
            string filePath = Path.Combine(folderPath, fileName);
            System.IO.File.WriteAllBytes(filePath, pdfBytes);

            return filePath;
        }


        public ActionResult GetEditorCheck()
        {
            return View();
        }
    }
}