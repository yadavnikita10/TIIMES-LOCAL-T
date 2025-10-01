using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO;
using TuvVision.Models;
using TuvVision.DataAccessLayer;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data.OleDb;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace TuvVision.Controllers
{
    public class UserRoleController : Controller
    {
        DALUserRole objDalUserRole = new DALUserRole();
        UserRoles ObjModelUserRole = new UserRoles();
        // GET: UserRole
        public ActionResult RoleDashBoard()
        {
            DataTable DTRoleDashBoard = new DataTable();
            //List<MenuRights> lstMenu = new List<MenuRights>();
            //lstMenu = objdalMenuRights.GetMenuRights();
            //Session.Add("MenuList", lstMenu);
            List<UserRoles> lstRoleDashBoard = new List<UserRoles>();
            DTRoleDashBoard = objDalUserRole.GetRoleDashBoard();
            try
            {
                if (DTRoleDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTRoleDashBoard.Rows)
                    {
                        //var strBuilder = new StringBuilder();
                        //var splittedMenuId = new List<int>();// Added by sagar panigrahi

                        //var userRoles = new UserRoles();
                        //userRoles.UserRoleID = Convert.ToInt32(dr["UserRoleID"]);
                        //userRoles.RoleName = Convert.ToString(dr["RoleName"]);
                        //userRoles.FK_SUB_MenuNames = Convert.ToString(dr["FK_SUB_MenuNames"]);

                        //if (userRoles != null)
                        //{
                        //    if (!string.IsNullOrEmpty(userRoles.FK_SUB_MenuNames))
                        //    {
                        //        splittedMenuId = userRoles.FK_SUB_MenuNames.Split(',').Select(int.Parse).ToList();
                        //    }
                        //}
                        //foreach(var singleMenuId in splittedMenuId)
                        //{
                        //    strBuilder.Append("Sagar");
                        //    userRoles.FK_SUB_MenuNames = strBuilder.ToString();
                        //}


                        lstRoleDashBoard.Add(
                            new UserRoles
                            {
                                UserRoleID = Convert.ToInt32(dr["UserRoleID"]),
                                RoleName = Convert.ToString(dr["RoleName"]),
                                FK_SUB_MenuNames = Convert.ToString(dr["FK_SUB_MenuNames"]),

                            }
                            );

                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["RoleList"] = lstRoleDashBoard;

            return View();
        }
        [HttpGet]
        public ActionResult CreateRole(int? RoleID)
        {
            if (RoleID != 0 && RoleID != null)
            {
                string[] splited;
                List<string> Selected = new List<string>();
                DataSet DSRole = new DataSet();
                List<NameCode> lstEditMenuGrp = new List<NameCode>();
                List<NameCode> lstEditMenuGroup = new List<NameCode>();
                DSRole = objDalUserRole.GetList();

                DataTable DTEditRoleDtls = new DataTable();
                DTEditRoleDtls = objDalUserRole.GetRoleDtls(RoleID);
                if (DTEditRoleDtls.Rows.Count > 0)
                {
                    ObjModelUserRole.UserRoleID = Convert.ToInt32(DTEditRoleDtls.Rows[0]["UserRoleID"]);
                    ObjModelUserRole.RoleName = Convert.ToString(DTEditRoleDtls.Rows[0]["RoleName"]);
                    ObjModelUserRole.Description = Convert.ToString(DTEditRoleDtls.Rows[0]["Description"]);

                    var existingMenus = Convert.ToString(DTEditRoleDtls.Rows[0]["FK_MenuName"]);

                    splited = existingMenus.Split(',');
                    foreach (var single in splited)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.Menus = Selected;

                    var ExistingSubMenus = Convert.ToString(DTEditRoleDtls.Rows[0]["FK_SubMenuNames"]);
                    splited = ExistingSubMenus.Split(',');
                    foreach (var single in splited)
                    {
                        Selected.Add(single);
                    }
                    ViewBag.SubMenus = Selected;
                }
                if (DSRole.Tables[0].Rows.Count > 0)
                {
                    lstEditMenuGrp = (from n in DSRole.Tables[0].AsEnumerable()
                                      select new NameCode
                                      {
                                          Name = Convert.ToString(n.Field<string>(DSRole.Tables[0].Columns["MenuName"])),
                                          Code = Convert.ToInt32(n.Field<Int32>(DSRole.Tables[0].Columns["PK_MainMenuID"]))
                                      }).ToList();
                }
                IEnumerable<SelectListItem> MenuNameGrup;
                MenuNameGrup = new SelectList(lstEditMenuGrp, "Code", "Name");
                ViewBag.MenuNameGrup = MenuNameGrup;

                if (DSRole.Tables[1].Rows.Count > 0)
                {
                    lstEditMenuGrp = (from n in DSRole.Tables[1].AsEnumerable()
                                      select new NameCode
                                      {
                                          Name = Convert.ToString(n.Field<string>(DSRole.Tables[1].Columns["SubMenuName"])),
                                          Code = Convert.ToInt32(n.Field<Int32>(DSRole.Tables[1].Columns["PK_SubId"]))
                                      }).ToList();
                }
                IEnumerable<SelectListItem> SubMenuNameGrup;
                SubMenuNameGrup = new SelectList(lstEditMenuGrp, "Code", "Name");
                ViewBag.SubMenuNameGrup = SubMenuNameGrup;

                return View(ObjModelUserRole);
            }

            else
            {
                DataSet DSRole = new DataSet();
                List<NameCode> lstMenuGrp = new List<NameCode>();
                List<NameCode> lstSubMenuGroup = new List<NameCode>();
                DSRole = objDalUserRole.GetList();
                if (DSRole.Tables[0].Rows.Count > 0)
                {
                    lstMenuGrp = (from n in DSRole.Tables[0].AsEnumerable()
                                  select new NameCode()
                                  {
                                      Name = n.Field<string>(DSRole.Tables[0].Columns["MenuName"].ToString()),
                                      Code = n.Field<Int32>(DSRole.Tables[0].Columns["PK_MainMenuID"].ToString())
                                  }).ToList();

                    ViewBag.MenuName = lstMenuGrp;
                }
                if (DSRole.Tables[1].Rows.Count > 0)
                {
                    lstSubMenuGroup = (from n in DSRole.Tables[1].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSRole.Tables[1].Columns["SubMenuName"].ToString()),
                                           Code = n.Field<Int32>(DSRole.Tables[1].Columns["PK_SubId"].ToString())
                                       }).ToList();

                    ViewBag.SubMenuName = lstSubMenuGroup;
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole(UserRoles UR, string[] MenuName, string[] SubMenuName)
        {
            int Result;
            string MName = string.Empty;
            string SMName = string.Empty;
            if (MenuName != null)
            {
                MName = string.Join(",", MenuName.ToArray().Where(s => !string.IsNullOrEmpty(s)));
                MName = MName.TrimEnd(',');

            }
            if (SubMenuName != null)
            {
                SMName = string.Join(",", SubMenuName.ToArray().Where(s => !string.IsNullOrEmpty(s)));
                SMName = SMName.TrimEnd(',');
            }
            else
            {
                return RedirectToAction("RoleDashBoard", "UserRole");
            }
            try
            {
                if (MName != null || MName != "")
                {
                    if (UR.UserRoleID != 0 && UR.UserRoleID != null)
                    {
                        //Result = objDalUserRole.RoleCreatedUpdate(UR, MName, SMName);
                        Result = objDalUserRole.RoleCreatedUpdate(UR);
                        if (Result != 0)
                        {
                            return Json("UpdateSuccess", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        //Result = objDalUserRole.RoleCreatedUpdate(UR, MName, SMName);
                        Result = objDalUserRole.RoleCreatedUpdate(UR);
                        if (Result != 0)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int RoleID)
        {
            int Result = 0;
            try
            {
                Result = objDalUserRole.DeleteUser(RoleID);
                if (Result != 0)
                {
                    TempData["Result"] = Result;
                    return RedirectToAction("RoleDashBoard", "UserRole");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        //AdditionCode For User Role Live  Code
        public ActionResult UsersRole(int? RoleID)
        {
            try
            {
                var userRole = new UserRoles();
                DataTable DTMenuRightList = new DataTable();
                List<UserRoles> lstMenuLst = new List<UserRoles>();
                List<MainMenuList> Menulst = new List<MainMenuList>();
                lstMenuLst = objDalUserRole.GetMenuRights();
                var distinctMenu = lstMenuLst.GroupBy(s => s.MenuID).Select(s => s.First()).ToList();
                foreach (var singleMenu in distinctMenu)
                {
                    var singleMainMenu = new MainMenuList();
                    singleMainMenu.Id = singleMenu.MenuID.Value;
                    singleMainMenu.Name = singleMenu.MenuNames;
                    var distinctSubMenu = lstMenuLst.GroupBy(d => d.SubMenuID).Select(s => s.First()).Where(s => s.FK_MainMenuID == singleMenu.MenuID).ToList();
                    foreach (var singleSubMenu in distinctSubMenu)
                    {
                        var singleSubMainMenu = new SubMenuLists();
                        singleSubMainMenu.Id = singleSubMenu.SubMenuID.Value;
                        singleSubMainMenu.Name = singleSubMenu.SubMenuName;
                        var distinctChildSubMenu = lstMenuLst.GroupBy(s => s.SubSubMenuID).Select(e => e.First()).Where(s => s.FK_SubMenuID == singleSubMenu.SubMenuID).ToList();
                        foreach (var singleChildSubMenu in distinctChildSubMenu)
                        {
                            var singlechildMainSubMenu = new SubMenuChildList();
                            singlechildMainSubMenu.Id = singleChildSubMenu.SubSubMenuID.Value;
                            singlechildMainSubMenu.Child = singleChildSubMenu.SubSubMenuName;
                            singleSubMainMenu.ChildMenuList.Add(singlechildMainSubMenu);
                        }
                        singleMainMenu.SubMenuList.Add(singleSubMainMenu);
                    }
                    Menulst.Add(singleMainMenu);
                }
                userRole.lstMenuList = Menulst;

                if (RoleID != null)
                {
                    DataSet DSRole = new DataSet();
                    DSRole = objDalUserRole.GetList();
                    List<UserRoles> lstEditMenuLst = new List<UserRoles>();
                    UserRoles usrrole = new UserRoles();
                    DataTable DTEditRoleDtls = new DataTable();
                    DTEditRoleDtls = objDalUserRole.GetRoleDtls(RoleID);
                    if (DTEditRoleDtls.Rows.Count > 0)
                    {
                        userRole.UserRoleID = Convert.ToInt32(DTEditRoleDtls.Rows[0]["UserRoleID"]);
                        userRole.RoleName = Convert.ToString(DTEditRoleDtls.Rows[0]["RoleName"]);
                        userRole.Description = Convert.ToString(DTEditRoleDtls.Rows[0]["Description"]);
                        userRole.MainMenuIds = Convert.ToString(DTEditRoleDtls.Rows[0]["FK_MenuName"]);
                        userRole.SubMenuIds = Convert.ToString(DTEditRoleDtls.Rows[0]["FK_SubMenuNames"]);
                        userRole.SubMenuChildIds = Convert.ToString(DTEditRoleDtls.Rows[0]["FK_Child_MenuNames"]);
                      

                        List<string> SplittedMainMenu = userRole.MainMenuIds.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();
                        ViewBag.SelectedMainMenu = SplittedMainMenu.Select(int.Parse).ToList();

                        List<string> SplittedSubMenu = userRole.SubMenuIds.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();
                        ViewBag.SelectedSubMenu = SplittedSubMenu.Select(int.Parse).ToList();

                        List<string> SplittedSubMenuChild = userRole.SubMenuChildIds.Split(',').Where(s => !string.IsNullOrEmpty(s)).ToList();
                        ViewBag.SelectedSubMenuChild = SplittedSubMenuChild.Select(int.Parse).ToList();
                        return View(userRole);
                    }
                }
                else
                {
                    return View(userRole);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return View();
        }
        [HttpPost]//Live Data Implementation
        public ActionResult UsersRole(UserRoles UR)
        {
            try
            {
                StringBuilder MainMenu = new StringBuilder();
                StringBuilder SubMenu = new StringBuilder();
                StringBuilder SubMenuChild = new StringBuilder();
                foreach (var singleMenu in UR.lstMenuList.Where(s => s.IsChecked == true))
                {
                    if (singleMenu.IsChecked == true)
                    {
                        string MId = Convert.ToString(singleMenu.Id + ",");
                        MainMenu.Append(MId);
                    }
                    foreach (var singleSubMenu in singleMenu.SubMenuList.Where(s => s.IsChecked == true))
                    {
                        if (singleSubMenu.IsChecked == true)
                        {
                            string SMId = Convert.ToString(singleSubMenu.Id + ",");
                            SubMenu.Append(SMId);
                        }
                        foreach (var singleSubMenuChild in singleSubMenu.ChildMenuList.Where(s => s.IsChecked == true))
                        {
                            if (singleSubMenuChild.IsChecked == true)
                            {
                                string SMC = Convert.ToString(singleSubMenuChild.Id + ",");
                                SubMenuChild.Append(SMC);
                            }
                        }
                    }
                }
                
                UR.MainMenuIds = MainMenu.ToString().TrimEnd(',');
                UR.SubMenuIds = SubMenu.ToString().TrimEnd(',');
                UR.SubMenuChildIds = SubMenuChild.ToString().TrimEnd(',');
                int Id = objDalUserRole.RoleCreatedUpdate(UR);
                if (Id == -1)
                {
                    TempData["InsertUpdate"] = Id;
                }
                else
                {
                    TempData["Error"] = Id;
                }
                return RedirectToAction("RoleDashBoard");
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return View();
        }


        public ActionResult CreateUserBulk()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public ActionResult BindExcelDataToGrid(HttpPostedFileBase fileData)
        {
            if (fileData == null || fileData.ContentLength == 0)
            {
                return Json(new { success = false, message = "No file uploaded" }, JsonRequestBehavior.AllowGet);
            }

            try
            {

                string fileExtension = Path.GetExtension(fileData.FileName).ToLower();
                string fileLocation = Path.Combine(Server.MapPath("~/Content/Uploads"), fileData.FileName);

                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                else
                {

                }
                fileData.SaveAs(fileLocation);

                string connectionString = fileExtension == ".xls"
                    ? $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileLocation};Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\""
                    : $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

                DataSet ds = new DataSet();
                using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
                {
                    excelConnection.Open();

                    DataTable dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return Json(new { success = false, message = "No sheets found in the Excel file." }, JsonRequestBehavior.AllowGet);
                    }

                    string sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                    string query = $"SELECT * FROM [{sheetName}]";

                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                    {
                        dataAdapter.Fill(ds);
                    }
                }

                var jsonData = ds.Tables[0].AsEnumerable()
                                           .Select(row => ds.Tables[0].Columns.Cast<DataColumn>()
                                                         .ToDictionary(col => col.ColumnName, col => row[col]?.ToString()))
                                           .ToList();

                return Json(new { success = true, data = jsonData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult InsertDataIntoDataBase(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            if (file == null || file.ContentLength == 0)
            {
                // Handle the case where no file is uploaded
                ViewBag.Message = "No file selected or file is empty.";
                return View();
            }

            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string fileLocation = Server.MapPath("~/Content/Uploads") + file.FileName;

            try
            {
                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    ProcessExcelFile(file, fileLocation, fileExtension);

                    dt = objDalUserRole.InsertDataToUsercreation();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        var status = dt.Rows[0]["Status"].ToString();

                        if (status == "Successful")
                        {
                            TempData["UploadedData"] = dt;
                            SendInductionMail_Bulk();
                        }


                    }

                }
                else if (fileExtension == ".xml")
                {
                    ProcessXmlFile(file, fileLocation);
                }
                else
                {
                    ViewBag.Message = "Invalid file format. Please upload an Excel or XML file.";
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                ViewBag.Message = "File uploaded and data inserted successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (optional) and display a generic error message
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

            //  return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            string jsonResult = JsonConvert.SerializeObject(dt);
            return Json(new { success = true, data = jsonResult }, JsonRequestBehavior.AllowGet);

        }

        private void ProcessExcelFile(HttpPostedFileBase file, string fileLocation, string fileExtension)
        {

            // Ensure file doesn't already exist before saving
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }

            file.SaveAs(fileLocation);
            string connectionString = GetExcelConnectionString(fileExtension, fileLocation);

            using (OleDbConnection excelConnection = new OleDbConnection(connectionString))
            {
                excelConnection.Open();
                DataTable schemaTable = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (schemaTable == null || schemaTable.Rows.Count == 0)
                {
                    throw new Exception("No sheets found in the Excel file.");
                }

                // Get the first sheet from the Excel file
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();
                string query = $"SELECT * FROM [{sheetName}]";

                DataSet ds = new DataSet();
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                {
                    dataAdapter.Fill(ds);
                }

                // Get the column headers dynamically from the first row
                var headers = ds.Tables[0].Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();

                // Map the Excel headers to database columns
                var headerMapping = GetHeaderColumnMapping();

                // Insert the data into the database
                InsertDataIntoDatabase(ds, headers, headerMapping);
            }
        }

        private void ProcessXmlFile(HttpPostedFileBase file, string fileLocation)
        {
            if (System.IO.File.Exists(fileLocation))
            {
                System.IO.File.Delete(fileLocation);
            }

            file.SaveAs(fileLocation);
            using (XmlTextReader xmlReader = new XmlTextReader(fileLocation))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);

                // Assuming the XML file also has column headers (can be customized for your XML format)
                var headers = ds.Tables[0].Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList();
                var headerMapping = GetHeaderColumnMapping();

                InsertDataIntoDatabase(ds, headers, headerMapping);
            }
        }

        private Dictionary<string, string> GetHeaderColumnMapping()
        {
            // Define the mapping from Excel column headers to your database column names
            return new Dictionary<string, string>
    {
        { "FirstName", "FirstName" },
        { "MiddleName", "MiddleName" },
        { "LastName", "LastName" },
         { "DisplayName", "DisplayName" },
          { "DateOfJoining", "DateOfJoining" },
           { "CostCenter_Id", "CostCenter_Id" },
            { "FK_BranchID", "FK_BranchID" },
             { "EmployementCategory", "EmployementCategory" },
              { "EmailID", "EmailID" },
                { "EmployeeGrade", "EmployeeGrade" },
                  { "EmployeeCode", "EmployeeCode" },
                    { "SAPEmpCode", "SAPEmpCode" },
                      { "Designation", "Designation" },
                        { "Tuv_Email_Id", "Tuv_Email_Id" },
                          { "UserName", "UserName" },
                          { "FK_RoleID", "FK_RoleID" },
                          { "SpecialServices", "SpecialServices" },
                          { "LpgGasCylinder", "LpgGasCylinder" },
                          { "OPE", "OPE" },


        // Add other mappings here...
    };
        }

        private void InsertDataIntoDatabase(DataSet ds, List<string> excelHeaders, Dictionary<string, string> headerMapping)
        {
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No data found in the file.");
            }
            string conn = ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString;


            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // Prepare the SQL query with parameterized values
                    var parameters = new List<SqlParameter>();
                    var query = "INSERT INTO UserCreation_bulk (";

                    // Add columns dynamically based on header mapping
                    foreach (var excelColumn in excelHeaders)
                    {
                        if (headerMapping.ContainsKey(excelColumn))
                        {
                            string dbColumn = headerMapping[excelColumn];
                            query += $"{dbColumn},";
                            parameters.Add(new SqlParameter($"@{dbColumn}", row[excelColumn].ToString()));
                        }
                    }

                    // Remove last comma and close the query
                    query = query.TrimEnd(',') + ") VALUES (";
                    foreach (var param in parameters)
                    {
                        query += $"{param.ParameterName},";
                    }
                    query = query.TrimEnd(',') + ")";

                    // Execute the query with parameters
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetExcelConnectionString(string fileExtension, string fileLocation)
        {
            if (fileExtension == ".xls")
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fileLocation};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (fileExtension == ".xlsx")
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={fileLocation};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
            else
            {
                throw new Exception("Invalid Excel file format.");
            }
        }
        public ActionResult GetDataInserted()
        {
            try
            {
                DataTable dt = objDalUserRole.GetInsertedData();
                if (dt.Rows.Count > 0)
                {
                    string json = JsonConvert.SerializeObject(dt);
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonConvert.SerializeObject("Error"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.SerializeObject("Error"), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SendInductionMail_Bulk()
        {
            // Retrieve the DataTable from TempData
            DataTable dt = TempData["UploadedData"] as DataTable;

            if (dt == null)
            {
                // Handle the case where no data was passed
                return View("Error");
            }

            // Process the DataTable as needed (e.g., sending induction emails)
            foreach (DataRow row in dt.Rows)
            {

                DataTable datatable = new DataTable();

                string PK_UserID = row["Pk_userid_"].ToString();


                datatable = objDalUserRole.GetPKUserID(PK_UserID);

                if (datatable.Rows.Count > 0)
                {
                    string Appr1 = Convert.ToString(datatable.Rows[0]["Apr1Mail"]);
                    string Appr2 = Convert.ToString(datatable.Rows[0]["Apr2Mail"]);
                    string PCH = Convert.ToString(datatable.Rows[0]["PCHEmail"]);
                    string BranchQA = Convert.ToString(datatable.Rows[0]["BranchQA"]);
                    string Username = Convert.ToString(datatable.Rows[0]["UserName"]);
                    string UserEmail = Convert.ToString(datatable.Rows[0]["Tuv_Email_Id"]);
                    if (Convert.ToString(datatable.Rows[0]["EmployementCategory"]) == "2" || Convert.ToString(datatable.Rows[0]["EmployementCategory"]) == "3")
                    {
                        SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
                    }
                    else if (Convert.ToString(datatable.Rows[0]["EmployementCategory"]) == "5" || Convert.ToString(datatable.Rows[0]["EmployementCategory"]) == "1" || Convert.ToString(datatable.Rows[0]["EmployementCategory"]) == "6")
                    {
                        SendInductionMailOtherThanPayroll(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
                    }
                    else
                    {
                        SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
                    }
                }

            }

            return View();
        }

        public bool SendInductionMail(string Appr1, string Appr2, string PCH, string BranchQA, string Username, string UserEmail)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //UserEmail = "vaipatil@tuv-nord.com";
            //string CC = "vaipatil@tuv-nord.com";
            string CC = "juliet@tuv-nord.com,pshrikant@tuv-nord.com,rohini@tuv-nord.com,vaipatil@tuv-nord.com" + "," + PCH + "," + Appr1 + "," + Appr2;
            StringBuilder MailBody = new StringBuilder();
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(basePath, "HrInduction.jpg");

                LinkedResource inlineImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
                inlineImage.ContentId = "InductionImage";
                inlineImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);
                inlineImage.TransferEncoding = TransferEncoding.Base64;

                if (UserEmail != "")
                {
                    MailBody.Append("<div style='font-family: Arial, sans-serif; line-height: 1.6;'>");
                    MailBody.Append("<div style='width: 80%; margin: auto; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>");

                    MailBody.Append("<p>Dear Colleague,</p>");
                    MailBody.Append("<p>A Warm Welcome to <span style='color: #001ed2;'>TUV INDIA</span> Private Limited.</p>");

                    MailBody.Append("<div style='margin: 20px 0;'>");
                    MailBody.Append("<p>Your TIIMES Credentials follow below:</p>");
                    MailBody.Append("<p>Link: <a href='https://tiimes.tuv-india.com'>https://tiimes.tuv-india.com</a><br>");
                    MailBody.Append($"ID: {Username}<br>");
                    MailBody.Append("Password: Pass@123</p>");
                    MailBody.Append("</div>");

                    MailBody.Append("<div style='margin: 20px 0;'>");
                    MailBody.Append("<p><span style='color: #B72323;'>Notes:</span></p>");
                    MailBody.Append("<ul style='list-style-type: disc; margin-left: 20px;'>");
                    MailBody.Append("<li>A VPN connection is required to access the site outside the office network.</li>");
                    MailBody.Append("<li>Change the password after login.</li>");
                    MailBody.Append("<li>Please go through the HR Induction Tool process detailed in the image below. Completing the HR Induction Tool is <span style='color: #B72323;'>MANDATORY</span>. You must secure at least 40 marks for the certificate to be generated. Click on the <span style='color: #B72323;'>\"VIEW CERTIFICATE\"</span> icon to generate your certificate.</li>");
                    MailBody.Append("<li>For any technical assistance, please email our IT service team at <a href='mailto:support.india@tuv-nord.com'>support.india@tuv-nord.com</a>.</li>");
                    MailBody.Append("<li>Please update your complete profile and create a CV in TIIMES.</li>");
                    MailBody.Append("</ul>");
                    MailBody.Append("</div>");

                    MailBody.Append("<div style='margin: 20px 0; text-align: center;'>");
                    MailBody.Append("<img src='cid:InductionImage' id='img' style='max-width: 100%; height: auto;' />");
                    MailBody.Append("</div>");
                    MailBody.Append("<p>Warmest regards,</p>");
                    MailBody.Append("<p>Team TUVI</p>");

                    MailBody.Append("</div>");
                    MailBody.Append("</div>");
                }

                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();

                mail.To.Add(UserEmail);
                mail.CC.Add(CC);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                mail.Subject = "TIIMES Software Credentials and HR Induction Process";
                mail.IsBodyHtml = true;

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(MailBody.ToString(), null, MediaTypeNames.Text.Html);
                htmlView.LinkedResources.Add(inlineImage);
                mail.AlternateViews.Add(htmlView);

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





        //SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
        public bool SendInductionMailOtherThanPayroll2(string Appr1, string Appr2, string PCH, string BranchQA, string Username, string UserEmail)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //UserEmail = "vaipatil@tuv-nord.com";
            //string CC = "vaipatil@tuv-nord.com";
            string CC = "pshrikant@tuv-nord.com,rohini@tuv-nord.com,vaipatil@tuv-nord.com" + "," + PCH + "," + Appr1 + "," + Appr2;
            StringBuilder MailBody = new StringBuilder();
            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string imagePath = Path.Combine(basePath, "");

                LinkedResource inlineImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
                inlineImage.ContentId = "InductionImage";
                inlineImage.ContentType = new ContentType(MediaTypeNames.Image.Jpeg);
                inlineImage.TransferEncoding = TransferEncoding.Base64;

                if (UserEmail != "")
                {
                    MailBody.Append("<div style='font-family: Arial, sans-serif; line-height: 1.6;'>");
                    MailBody.Append("<div style='width: 80%; margin: auto; padding: 20px; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>");

                    MailBody.Append("<p>Dear Colleague,</p>");
                    MailBody.Append("<p>A Warm Welcome to <span style='color: #001ed2;'>TUV INDIA</span> Private Limited.</p>");

                    MailBody.Append("<div style='margin: 20px 0;'>");
                    MailBody.Append("<p>Your TIIMES Credentials follow below:</p>");
                    MailBody.Append("<p>Link: <a href='https://tiimes.tuv-india.com'>https://tiimes.tuv-india.com</a><br>");
                    MailBody.Append($"ID: {Username}<br>");
                    MailBody.Append("Password: Pass@123</p>");
                    MailBody.Append("</div>");

                    MailBody.Append("<div style='margin: 20px 0;'>");
                    MailBody.Append("<p><span style='color: #B72323;'>Notes:</span></p>");
                    MailBody.Append("<ul style='list-style-type: disc; margin-left: 20px;'>");
                    MailBody.Append("<li>A VPN connection is required to access the site outside the office network.</li>");
                    MailBody.Append("<li>Change the password after login.</li>");
                    //MailBody.Append("<li>Please go through the HR Induction Tool process detailed in the image below. Completing the HR Induction Tool is <span style='color: #B72323;'>MANDATORY</span>. You must secure at least 40 marks for the certificate to be generated. Click on the <span style='color: #B72323;'>\"VIEW CERTIFICATE\"</span> icon to generate your certificate.</li>");
                    MailBody.Append("<li>For any technical assistance, please email our IT service team at <a href='mailto:support.india@tuv-nord.com'>support.india@tuv-nord.com</a>.</li>");
                    MailBody.Append("<li>Please update your complete profile and create a CV in TIIMES.</li>");
                    MailBody.Append("</ul>");
                    MailBody.Append("</div>");

                    //MailBody.Append("<div style='margin: 20px 0; text-align: center;'>");
                    //MailBody.Append("<img src='cid:InductionImage' id='img' style='max-width: 100%; height: auto;' />");
                    //MailBody.Append("</div>");
                    MailBody.Append("<p>Warmest regards,</p>");
                    MailBody.Append("<p>Team TUVI</p>");

                    MailBody.Append("</div>");
                    MailBody.Append("</div>");
                }

                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();

                mail.To.Add(UserEmail);
                mail.CC.Add(CC);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString());
                mail.Subject = "TIIMES Software Credentials";
                mail.IsBodyHtml = true;

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(MailBody.ToString(), null, MediaTypeNames.Text.Html);
                htmlView.LinkedResources.Add(inlineImage);
                mail.AlternateViews.Add(htmlView);

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


        public bool SendInductionMailOtherThanPayroll(string Appr1, string Appr2, string PCH, string BranchQA, string Username, string UserEmail)
        {
            try
            {
                MailMessage msg = new MailMessage();

                // Fetching email configurations from web.config
                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string smtpHost = ConfigurationManager.AppSettings["SmtpServer"].ToString();

                // Setting the recipient and CC
                //UserEmail = "vaipatil@tuv-nord.com";
                //string CC = "vaipatil@tuv-nord.com";
                string CC = "pshrikant@tuv-nord.com,rohini@tuv-nord.com,vaipatil@tuv-nord.com" + "," + PCH + "," + Appr1 + "," + Appr2;

                // Constructing the email body with string concatenation for Username
                string bodyTxt = $@"
        <html>
        <head><title></title></head>
        <body>
            <div style='font-family:Verdana, Geneva, sans-serif; font-size:12px;'>
                <p>Dear Colleague,</p>
                <p>A Warm Welcome to TUV INDIA Private Limited.</p>
                <p>Your TIIMES Credentials follow below:</p>
                <ul>
                    <li><strong>Link:</strong> <a href='https://tiimes.tuv-india.com'>https://tiimes.tuv-india.com</a></li>
                    <li><strong>ID:</strong> {Username}</li>
                    <li><strong>Password:</strong> Pass@123</li>
                </ul>
                <p>Notes:</p>
                <ul>
                    <li>A VPN connection is required to access the site outside the office network.</li>
                    <li>Change the password after login.</li>
                    <li>For any technical assistance, please email our IT service team at <a href='mailto:support.india@tuv-nord.com'>support.india@tuv-nord.com</a>.</li>
                    <li>Please update your complete profile and create a CV in TIIMES.</li>
                </ul>
                <p>Warmest regards,</p>
                <p>Team TUVI</p>
            </div>
        </body>
        </html>";

                // Configuring mail message details
                msg.To.Add(UserEmail);
                msg.CC.Add(CC);
                msg.From = new MailAddress(MailFrom, "TUV INDIA Private Limited");
                msg.Subject = "TIIMES Software Credentials";
                msg.Body = bodyTxt;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.Normal;

                // SMTP client setup
                SmtpClient client = new SmtpClient
                {
                    Port = int.Parse(ConfigurationManager.AppSettings["Port"].ToString()),
                    Host = ConfigurationManager.AppSettings["SmtpServer"].ToString(),
                    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["User"].ToString(), ConfigurationManager.AppSettings["Password"].ToString()),
                    EnableSsl = true
                };

                // Ensure proper security protocol for TLS
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                // Send the email
                client.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }


    }
}