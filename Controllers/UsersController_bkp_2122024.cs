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
using OfficeOpenXml;
using SelectPdf;
using NonFactors.Mvc.Grid;
using System.Net;
using System.Security.Cryptography;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Net.Mail;
using System.Net.Mime;

namespace TuvVision.Controllers
{
    public class UsersController : Controller
    {
        DALUsers objDalCreateUser = new DALUsers();

        Users ObjModelUsers = new Users();
        ProfileUser ProfileUsers = new ProfileUser();

        DALNonInspectionActivity objNIA = new DALNonInspectionActivity();
        DALEnquiryMaster objDalEnquiryMaster = new DALEnquiryMaster();
        DALLibraryMaster OBJAppl = new DALLibraryMaster();
        DataTable DTEditUser = new DataTable();
        string AttachmentType = "";
        // GET: CreateUsers
        public ActionResult UserDashBoard()
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTUserDashBoard = new DataTable();
            //List<MenuRights> lstMenu = new List<MenuRights>();
            //lstMenu = objdalMenuRights.GetMenuRights();
            //Session.Add("MenuList", lstMenu);
            List<ProfileUser> lstUserDashBoard = new List<ProfileUser>();
            DTUserDashBoard = objDalCreateUser.GetUserMis();
            try
            {
                if (DTUserDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTUserDashBoard.Rows)
                    {
                        lstUserDashBoard.Add(
                            new ProfileUser
                            {
                                //PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                //UserName = Convert.ToString(dr["UserName"]),
                                //FirstName = Convert.ToString(dr["FirstName"]),
                                //LastName = Convert.ToString(dr["LastName"]),
                                //EmailID = Convert.ToString(dr["EmailID"]),
                                //MobileNo = Convert.ToString(dr["MobileNo"]),
                                //IsActive = Convert.ToBoolean(dr["IsActive"]),
                                //ImgFilePath = Convert.ToString("~/Content/Uploads/Images/"),
                                PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Image = Convert.ToString(dr["Image"]),
                                EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                                SAP_VendorCode = Convert.ToString(dr["SAP_VendorCode"]),
                                DateOfJoining = Convert.ToString(dr["DateOfJoining"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                Address2 = Convert.ToString(dr["Address2"]),
                                Address1 = Convert.ToString(dr["Address1"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                Additional_Qualification = Convert.ToString(dr["Additional_Qualification"]),
                                Qualification = Convert.ToString(dr["Qualification"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                EmergencyMobile_No = Convert.ToString(dr["EmergencyMobile_No"]),
                                EmailID = Convert.ToString(dr["EmailID"]),
                                TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                DOB = Convert.ToString(dr["DOB"]),
                                Gender = Convert.ToString(dr["Gender"]),
                                BloodGroup = Convert.ToString(dr["BloodGroup"]),
                                ShoesSize = Convert.ToString(dr["ShoesSize"]),
                                ShirtSize = Convert.ToString(dr["ShirtSize"]),
                                PanNo = Convert.ToString(dr["PanNo"]),
                                AadharNo = Convert.ToString(dr["AadharNo"]),
                                CV = Convert.ToString(dr["CV"]),
                                ActiveStatus = Convert.ToString(dr["IsActive"]),
                                UserRole = Convert.ToString(dr["Role"]),
                                //FK_RoleID = Convert.ToInt32(dr["FK_RoleID"]),
                                FK_RoleID = Convert.ToString(dr["FK_RoleID"]),

                                SAPEmployeeCode = Convert.ToString(dr["SAPEmpCode"]),
                                EduQualAtta = Convert.ToString(dr["EduQualAtta"]),
                                ProfQualAtta = Convert.ToString(dr["ProfQualAtta"]),
                                TUVIStampNo = Convert.ToString(dr["TUVIStampNo"]),
                                Verified = Convert.ToString(dr["isVerified"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                strModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                //added by shrutika salve 11082023
                                MainBranch = Convert.ToString(dr["mainBranch"]),
                                OBSTYPE = Convert.ToString(dr["OBSName"]),
                                ProffesionalQualificationDetails = Convert.ToString(dr["ProffesionalDetails"]),
                                UserAttachment1 = Convert.ToString(dr["UserAttachment"]),
                                EyeTest = Convert.ToString(dr["EyeTest"]),
                                DisplayName = Convert.ToString(dr["DisplayName"]),
                                FormFilled = Convert.ToString(dr["FormFilled"])

                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["UserList"] = lstUserDashBoard;
            ProfileUsers.LstDashboard = lstUserDashBoard;

            return View(ProfileUsers);
        }


        [HttpGet]
        public ActionResult CreateUser(string UserID)
        {

            if (UserID == "Admin" || UserID == "Superadmin")
            {
                UserID = null;
            }
            else if (UserID != null)
            {
                UserID = UserID;
            }
            else
            {
                UserID = Convert.ToString(Session["UserIDs"]);
            }
            Session["UserID"] = UserID;
            int LoginUserRoleId = 0;
            LoginUserRoleId = objDalCreateUser.GetLoginRoleID();
            ObjModelUsers.LoginUserRoleId = LoginUserRoleId;

            var Data1 = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");



            var varYear = objDalCreateUser.BindYear();//Bind Year
            ViewBag.Year = new SelectList(varYear, "Year", "YearName");

            #region Bind Year of Passing
            //var Data2 = objDalCreateUser.BindYearOfPassing();
            //ViewBag.YearOfPassing = new SelectList(Data2, "YearOfPassing", "YearOfPassingName");

            DataSet DSYearOfPassing = new DataSet();
            List<NameCode> lstYear = new List<NameCode>();
            DSYearOfPassing = objDalCreateUser.BindYearOfPassing();

            if (DSYearOfPassing.Tables[0].Rows.Count > 0)//All Currency 
            {
                lstYear = (from n in DSYearOfPassing.Tables[0].AsEnumerable()
                           select new NameCode()
                           {
                               //Name = n.Field<string>(DSYearOfPassing.Tables[0].Columns["YearOfPassingName"].ToString()),
                               //Code = n.Field<Int32>(DSYearOfPassing.Tables[0].Columns["intYearOfPassing"].ToString())

                               Name = n.Field<string>(DSYearOfPassing.Tables[0].Columns["YearOfPassingName"].ToString()),
                               Code = n.Field<Int32>(DSYearOfPassing.Tables[0].Columns["intYearOfPassing"].ToString())
                           }).ToList();
            }
            ViewBag.YearOfPassing = lstYear;
            #endregion

            var dtDocType = objDalCreateUser.GetDocumentType();

            ViewBag.DocTypeList = new SelectList(dtDocType, "PK_id", "DocName");


            DataTable DTUserDashBoard = new DataTable();
            List<Users> lstUserDashBoard = new List<Users>();
            DTUserDashBoard = objDalCreateUser.GetUserDashBoard();

            if (DTUserDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTUserDashBoard.Rows)
                {
                    lstUserDashBoard.Add(
                        new Users
                        {
                            PK_UserID = Convert.ToString(dr["PK_UserID"]),
                            FullName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                        }
                        );
                }
            }

            ViewBag.Employees = new SelectList(lstUserDashBoard, "PK_UserID", "FullName");

            DataTable DTOrgChange = new DataTable();
            List<Users> lstOrgChangeBoard = new List<Users>();
            DTOrgChange = objDalCreateUser.GetOrgChangeData();

            if (DTOrgChange.Rows.Count > 0)
            {
                foreach (DataRow dr in DTOrgChange.Rows)
                {
                    lstOrgChangeBoard.Add(
                        new Users
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Orgdescription = Convert.ToString(dr["description"]),
                        }
                        );
                }
            }

            ViewBag.OrgChange = new SelectList(lstOrgChangeBoard, "id", "Orgdescription");

            DataSet DSGetEditAllddllst = new DataSet();

            #region Bind university
            List<NameCode> lstUniversity = new List<NameCode>();
            DataSet DSBindUniversity = new DataSet();
            //var varUniversity = objDalCreateUser.BindUniversity();
            //ViewBag.University = new SelectList(varUniversity, "University", "UniversityName");
            //ViewBag.University = new SelectList(varUniversity, "Code", "Name");

            DSBindUniversity = objDalCreateUser.BindUniversity();

            if (DSBindUniversity.Tables[0].Rows.Count > 0)
            {



                lstUniversity = (from n in DSBindUniversity.Tables[0].AsEnumerable()
                                 select new NameCode()
                                 {
                                     Name = n.Field<string>(DSBindUniversity.Tables[0].Columns["UniversityName"].ToString()),
                                     Code = n.Field<Int32>(DSBindUniversity.Tables[0].Columns["UniversityId"].ToString())
                                 }).ToList();

            }
            ViewBag.University = lstUniversity;
            #endregion

            #region Bind Employee Grade


            List<NameCode> lstBindGrade = new List<NameCode>();
            List<NameCode> lstBindGradeNew = new List<NameCode>();
            DSGetEditAllddllst = objDalCreateUser.GetGradeMaster();

            if (DSGetEditAllddllst.Tables[1].Rows.Count > 0)
            {



                lstBindGrade = (from n in DSGetEditAllddllst.Tables[1].AsEnumerable()
                                select new NameCode()
                                {
                                    Name = n.Field<string>(DSGetEditAllddllst.Tables[0].Columns["EmployeeGrade"].ToString()),
                                    Code = n.Field<Int32>(DSGetEditAllddllst.Tables[0].Columns["Id"].ToString())
                                }).ToList();

            }
            if (DSGetEditAllddllst.Tables[0].Rows.Count > 0)
            {



                lstBindGradeNew = (from n in DSGetEditAllddllst.Tables[0].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSGetEditAllddllst.Tables[0].Columns["EmployeeGrade"].ToString()),
                                       Code = n.Field<Int32>(DSGetEditAllddllst.Tables[0].Columns["Id"].ToString())
                                   }).ToList();

            }
            //IEnumerable<SelectListItem> ProjectGradeItems;
            //ProjectGradeItems = new SelectList(lstBindGrade, "Code", "Name");
            //ViewData["EmployeeGrade"] = ProjectGradeItems;
            ViewBag.EmployeeGrade = lstBindGrade;
            ViewBag.EmployeeGradeOld = lstBindGradeNew;
            //ViewBag.EmployeeGrade = new SelectList(lstBindGrade, "EmployeeGradeId", "EmployeeGrade");

            #endregion

            DSGetEditAllddllst = objDalEnquiryMaster.GetEditAllddlLst();
            List<NameCode> lstEditProjectType = new List<NameCode>();

            DataSet DSGetEmployeeType = new DataSet();
            DSGetEmployeeType = objDalEnquiryMaster.GetEmployeeTypeLst();
            List<NameCode> lstEmployeetType = new List<NameCode>();

            if (DSGetEmployeeType.Tables[0].Rows.Count > 0)
            {
                lstEmployeetType = (from n in DSGetEmployeeType.Tables[0].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSGetEmployeeType.Tables[0].Columns["ProjectName"].ToString()),
                                        Code = n.Field<Int32>(DSGetEmployeeType.Tables[0].Columns["PK_ID"].ToString())
                                    }).ToList();
            }

            IEnumerable<SelectListItem> EmployeeType;
            EmployeeType = new SelectList(lstEmployeetType, "Code", "Name");
            ViewBag.ProjectTypeItems = EmployeeType;
            ViewData["ProjectTypeItems"] = EmployeeType;

            if (UserID != "" && UserID != null)
            {

                DataSet CostCenter = objNIA.GetServiceCode();
                ObjModelUsers.CostCenter = Convert.ToInt32(CostCenter.Tables[0].Rows[0]["CostCenter"].ToString());

                DTEditUser = objDalCreateUser.EditUsers(UserID);
                if (DTEditUser.Rows.Count > 0)
                {
                    ObjModelUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0]["PK_UserID"]);
                    ObjModelUsers.FirstName = Convert.ToString(DTEditUser.Rows[0]["FirstName"]);
                    ObjModelUsers.LastName = Convert.ToString(DTEditUser.Rows[0]["LastName"]);
                    ObjModelUsers.EmailID = Convert.ToString(DTEditUser.Rows[0]["EmailID"]);
                    if (DTEditUser.Rows[0]["DateOfJoining"] != null)
                    {
                        ObjModelUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                    }
                    //ObjModelUsers.EmployeeGrade = Convert.ToString(DTEditUser.Rows[0]["EmployeeGrade"]);
                    ObjModelUsers.EmployeeGradeId = Convert.ToInt32(DTEditUser.Rows[0]["EmployeeGrade"]);
                    ObjModelUsers.EmployeeCode = Convert.ToString(DTEditUser.Rows[0]["EmployeeCode"]);
                    ObjModelUsers.SAP_VendorCode = Convert.ToString(DTEditUser.Rows[0]["SAP_VendorCode"]);
                    ObjModelUsers.Address1 = Convert.ToString(DTEditUser.Rows[0]["Address1"]);
                    ObjModelUsers.Address2 = Convert.ToString(DTEditUser.Rows[0]["Address2"]);
                    ObjModelUsers.IsActive = Convert.ToBoolean(DTEditUser.Rows[0]["IsActive"]);
                    ObjModelUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["FK_BranchID"]);
                    ObjModelUsers.UserRole = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);
                    ObjModelUsers.PKId = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);

                    if (DTEditUser.Rows[0]["DOB"] != null)
                    {
                        ObjModelUsers.DOB = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                    }
                    ObjModelUsers.Gender = Convert.ToString(DTEditUser.Rows[0]["Gender"]);
                    ObjModelUsers.MobileNo = Convert.ToString(DTEditUser.Rows[0]["MobileNo"]);
                    ObjModelUsers.ResidenceNo = Convert.ToString(DTEditUser.Rows[0]["ResidenceNo"]);
                    ObjModelUsers.OfficeNo = Convert.ToString(DTEditUser.Rows[0]["OfficeNo"]);
                    ObjModelUsers.Designation = Convert.ToString(DTEditUser.Rows[0]["Designation"]);
                    ObjModelUsers.LanguageSpoken = Convert.ToString(DTEditUser.Rows[0]["LanguageSpoken"]);
                    ObjModelUsers.ZipCode = Convert.ToString(DTEditUser.Rows[0]["ZipCode"]);
                    ObjModelUsers.Signature = Convert.ToString(DTEditUser.Rows[0]["Signature"]);
                    ObjModelUsers.Qualification = Convert.ToString(DTEditUser.Rows[0]["Qualification"]);
                    ObjModelUsers.UserName = Convert.ToString(DTEditUser.Rows[0]["UserName"]);
                    ObjModelUsers.ReportingOne = Convert.ToString(DTEditUser.Rows[0]["Reporting_Person_One"]);
                    ObjModelUsers.ReportingTwo = Convert.ToString(DTEditUser.Rows[0]["Reporting_Person_Two"]);
                    ObjModelUsers.TuvEmailId = Convert.ToString(DTEditUser.Rows[0]["Tuv_Email_Id"]);
                    ObjModelUsers.Employee_Type = Convert.ToString(DTEditUser.Rows[0]["Employee_Type"]);
                    ObjModelUsers.BloodGroup = Convert.ToString(DTEditUser.Rows[0]["BloodGroup"]);
                    ObjModelUsers.ShoesSize = Convert.ToString(DTEditUser.Rows[0]["ShoesSize"]);
                    ObjModelUsers.ShirtSize = Convert.ToString(DTEditUser.Rows[0]["ShirtSize"]);
                    ObjModelUsers.IsLocked = Convert.ToBoolean(DTEditUser.Rows[0]["isLocked"]);
                    ObjModelUsers.CostCenter = Convert.ToInt32(DTEditUser.Rows[0]["CostCenter_ID"]);

                    ObjModelUsers.Marital_Status = Convert.ToString(DTEditUser.Rows[0]["Marital_Status"]);
                    ObjModelUsers.AadharNo = Convert.ToString(DTEditUser.Rows[0]["AadharNo"]);
                    ObjModelUsers.PanNo = Convert.ToString(DTEditUser.Rows[0]["PANNO"]);
                    ObjModelUsers.Allergies = Convert.ToString(DTEditUser.Rows[0]["Allergies"]);
                    ObjModelUsers.EmergencyMobile_No = Convert.ToString(DTEditUser.Rows[0]["EmergencyMobile_No"]);
                    ObjModelUsers.Medical_History = Convert.ToString(DTEditUser.Rows[0]["Medical_History"]);
                    ObjModelUsers.Fax_No = Convert.ToString(DTEditUser.Rows[0]["Fax_No"]);
                    ObjModelUsers.CV = Convert.ToString(DTEditUser.Rows[0]["CV"]);
                    ObjModelUsers.Image = Convert.ToString(DTEditUser.Rows[0]["Image"]);
                    ObjModelUsers.TotalyearofExprience = Convert.ToString(DTEditUser.Rows[0]["TotalyearofExprience"]);
                    ObjModelUsers.ExperienceInMonth = Convert.ToString(DTEditUser.Rows[0]["ExperienceInMonth"]);
                    ObjModelUsers.SAPEmployeeCode = Convert.ToString(DTEditUser.Rows[0]["SAPEmployeeCode"]);
                    ObjModelUsers.Additional_Qualification = Convert.ToString(DTEditUser.Rows[0]["Additional_Qualification"]);
                    ObjModelUsers.EmployeeGradeIdOld = Convert.ToInt32(DTEditUser.Rows[0]["EmployeeGradeOld"]);
                    ObjModelUsers.MiddleName = Convert.ToString(DTEditUser.Rows[0]["MiddleName"]);
                    ObjModelUsers.TUVUIN = Convert.ToString(DTEditUser.Rows[0]["TUVUIN"]);

                    ObjModelUsers.PermanantPin = Convert.ToString(DTEditUser.Rows[0]["PermanantPin"]);
                    ObjModelUsers.TUVIStampNo = Convert.ToString(DTEditUser.Rows[0]["TUVIStampNo"]);
                    ObjModelUsers.OPE = Convert.ToString(DTEditUser.Rows[0]["OPE"]);

                    ObjModelUsers.UserPrimaryKey = Convert.ToString(DTEditUser.Rows[0]["UserPrimaryKey"]);
                    ObjModelUsers.ReasonForUpdate = Convert.ToString(DTEditUser.Rows[0]["ReasonForUpdate"]);
                    ObjModelUsers.OrgChangeDate = Convert.ToString(DTEditUser.Rows[0]["OrgChangeDate"]);
                    ObjModelUsers.EmployementCategory = Convert.ToString(DTEditUser.Rows[0]["EmployementCategory"]);
                    ObjModelUsers.Course = Convert.ToString(DTEditUser.Rows[0]["Course"]);
                    ObjModelUsers.Degree = Convert.ToString(DTEditUser.Rows[0]["Degree"]);
                    ObjModelUsers.MajorFieldOfStudy = Convert.ToString(DTEditUser.Rows[0]["MajorFieldOfStudy"]);
                    //ObjModelUsers.University = Convert.ToInt32(DTEditUser.Rows[0]["University"]);
                    ObjModelUsers.OtherUniversity = Convert.ToString(DTEditUser.Rows[0]["OtherUniversity"]);
                    ObjModelUsers.CurrentAssignment = Convert.ToString(DTEditUser.Rows[0]["CurrentAssignment"]);
                    ObjModelUsers.SiteDetail = Convert.ToString(DTEditUser.Rows[0]["SiteDetail"]);
                    ObjModelUsers.ItemToBeInspected = Convert.ToString(DTEditUser.Rows[0]["ItemToBeInspected"]);
                    ObjModelUsers.After7Days = Convert.ToString(DTEditUser.Rows[0]["After7Days"]);
                    ObjModelUsers.isVerified = Convert.ToBoolean(DTEditUser.Rows[0]["isVerified"]);
                    //added by shrutika salve 10082023
                    ObjModelUsers.MainBranch = Convert.ToString(DTEditUser.Rows[0]["mainBranch"]);
                    ObjModelUsers.OBSTYPE = Convert.ToString(DTEditUser.Rows[0]["OBSName"]);
                    ObjModelUsers.SitePin = Convert.ToString(DTEditUser.Rows[0]["SiteAddrPin"]);

                    ObjModelUsers.PFUANNumber = Convert.ToString(DTEditUser.Rows[0]["PFUANNumber"]);
                    //added by shrutika salve 28022024
                    ObjModelUsers.CoreField = Convert.ToString(DTEditUser.Rows[0]["CoreFieldOfStudy"]);
                    //added by nikita on 18042024
                    ObjModelUsers.specialservices = Convert.ToString(DTEditUser.Rows[0]["specialservices"]);
                    ObjModelUsers.DisplayName = Convert.ToString(DTEditUser.Rows[0]["DisplayName"]);
                    ObjModelUsers.LpgGasCylinder = Convert.ToString(DTEditUser.Rows[0]["LpgGasCylinder"]);
                    var TransferDate = Convert.ToString(DTEditUser.Rows[0]["Transfer_Date"]);

                    if (!string.IsNullOrEmpty(TransferDate))
                    {
                        ObjModelUsers.TransferDate = Convert.ToString(DTEditUser.Rows[0]["Transfer_Date"]);
                    }


                    ViewData["ListPKSubJobChecked"] = Convert.ToString(DTEditUser.Rows[0]["Employee_Type"]);

                    ViewData["ListBranchchecked"] = Convert.ToString(DTEditUser.Rows[0]["FK_BranchID"]);

                    ViewData["ListLangchecked"] = Convert.ToString(DTEditUser.Rows[0]["LanguageSpoken"]);

                    ViewData["ListItemToBeInspectedchecked"] = Convert.ToString(DTEditUser.Rows[0]["ItemToBeInspected"]);
                }

                DataTable DTUserHistory = new DataTable();
                List<Users> lstUserHistory = new List<Users>();

                DTUserHistory = objDalCreateUser.GetUserHistory(UserID);

                if (DTUserHistory.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTUserHistory.Rows)
                    {
                        lstUserHistory.Add(
                            new Users
                            {
                                // FullName = Convert.ToString(dr["Name"]),
                                // EmailID = Convert.ToString(dr["EmailID"]),
                                EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                SAP_VendorCode = Convert.ToString(dr["SAP_VendorCode"]),
                                Status = Convert.ToString(dr["IsActive"]),
                                UserRole = Convert.ToString(dr["FK_RoleID"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                FK_BranchID = Convert.ToString(dr["FK_BranchID"]),
                                Employee_Type = Convert.ToString(dr["Employee_Type"]),
                                CostCenterName = Convert.ToString(dr["Cost_Center"]),
                                TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                ReportingOne = Convert.ToString(dr["Reporting_Person_One"]),
                                ReportingTwo = Convert.ToString(dr["Reporting_Person_Two"]),
                                LockedStatus = Convert.ToString(dr["isLocked"]),
                                SAPEmployeeCode = Convert.ToString(dr["SAPEmpCode"]),
                                OPE = Convert.ToString(dr["OPE"]),
                                ReasonForUpdate = Convert.ToString(dr["ReasonForUpdate"]),
                                EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                                OrgChangeDate = Convert.ToString(dr["OrgChangeDate"]),

                            }
                        );
                    }
                }
                ObjModelUsers.dtUserHistory = DTUserHistory;

                ViewData["UserList"] = lstUserHistory;
                ObjModelUsers.lstUserHistory = lstUserHistory;

                #region get Education qualification data
                DataTable dtGetEduQualification = new DataTable();
                List<Users> lstDOrderType = new List<Users>();

                dtGetEduQualification = objDalCreateUser.GetEduQualification(ObjModelUsers.UserPrimaryKey);
                if (dtGetEduQualification.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtGetEduQualification.Rows)
                    {
                        lstDOrderType.Add(
                           new Users
                           {


                               Course = Convert.ToString(dr["Course"]),
                               Degree = Convert.ToString(dr["Degree"]),
                               MajorFieldOfStudy = Convert.ToString(dr["MajorFieldOfStudy"]),
                               UniversityName = Convert.ToString(dr["University"]),
                               LastYearCGPA = Convert.ToString(dr["LastYearPerc"]),
                               AggregateCGPA = Convert.ToString(dr["AggregatePerc"]),
                               YearOfPassing = Convert.ToString(dr["YearOfPassing"]),

                           }
                         );
                    }
                    ViewBag.lstDOrderType = lstDOrderType;
                    Session["EducationDetails"] = lstDOrderType;
                }
                #endregion

                #region get Education qualification data
                DataTable dtProfessionalCerts = new DataTable();
                List<Users> lstProfCerts = new List<Users>();

                dtProfessionalCerts = objDalCreateUser.GetProfessionalCerts(ObjModelUsers.UserPrimaryKey);
                if (dtProfessionalCerts.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtProfessionalCerts.Rows)
                    {
                        lstProfCerts.Add(
                           new Users
                           {
                               CertName = Convert.ToString(dr["CertName"]),
                               CertNo = Convert.ToString(dr["CertNo"]),
                               CertIssueDate = Convert.ToString(dr["CertIssueDate"]),
                               CertValidTill = Convert.ToString(dr["CertValidTill"]),
                           }
                         );
                    }
                    ViewBag.lstProfCerts = lstProfCerts;
                    Session["lstProfCerts"] = ViewBag.lstProfCerts;
                }
                #endregion


                #region Document Attachment 
                DataTable dtAttach = new DataTable();
                List<Users> lstDTPAN = new List<Users>();

                dtAttach = objDalCreateUser.GetAtt(ObjModelUsers.UserPrimaryKey);

                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                           new Users
                           {
                               //  PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),

                               //    IDS = Convert.ToString(dr["FileID"]),
                               AttType = Convert.ToString(dr["AttType"]),
                           }
                         );
                    }

                    ObjModelUsers.UserAttachment = lstDTPAN;
                    ViewData["DocAttachments"] = lstDTPAN;
                }
                #endregion


                #region Get Eye Test Report
                DataTable dtAttachEye = new DataTable();
                List<Users> lstDTPANEye = new List<Users>();

                dtAttachEye = objDalCreateUser.GetEyeTest(ObjModelUsers.UserPrimaryKey);

                if (dtAttachEye.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttachEye.Rows)
                    {
                        lstDTPANEye.Add(
                           new Users
                           {
                               PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),
                               AttType = Convert.ToString(dr["attType"]),
                               CertIssueDate = Convert.ToString(dr["CertIssueDate"]),
                               CreatedBy = Convert.ToString(dr["CreatedBy"]),
                               CertValidTill = Convert.ToString(dr["CertValidTill"]),
                           }
                         );
                    }
                    
                    ObjModelUsers.EyeTestReport = lstDTPANEye;
                    ViewData["DocAttachmentsEyeTest"] = lstDTPANEye;
                    ViewBag.lstEYE = lstDTPANEye;
                }
                #endregion


                #region Get Photo
                DataTable dtPhoto = new DataTable();
                List<Users> lstPhoto = new List<Users>();

                dtPhoto = objDalCreateUser.GetPhoto(ObjModelUsers.UserPrimaryKey);

                if (dtPhoto.Rows.Count > 0)
                {
                    //foreach (DataRow dr in dtAttach.Rows)
                    //{
                    //    lstDTPAN.Add(
                    //       new Users
                    //       {
                    //           //  PK_ID = Convert.ToInt32(dr["PK_ID"]),
                    //           FileName = Convert.ToString(dr["FileName"]),

                    //           //    IDS = Convert.ToString(dr["FileID"]),
                    //           AttType = Convert.ToString(dr["AttType"]),
                    //       }
                    //     );
                    //}

                    // ObjModelUsers.UserAttachment = lstDTPAN;
                    ObjModelUsers.StrIData = Convert.ToString(dtPhoto.Rows[0]["IData"]);
                    //Session["IData"] = (byte[])(dtPhoto.Rows[0]["IData"]);
                    ViewData["DocAttachments"] = lstDTPAN;
                }
                #endregion

                #region Get Signature
                DataTable dtSignature = new DataTable();
                List<Users> lstSignature = new List<Users>();

                dtPhoto = objDalCreateUser.GetSignature(ObjModelUsers.UserPrimaryKey);

                if (dtPhoto.Rows.Count > 0)
                {

                    //                    ObjModelUsers.SignatureData = (byte[])(dtPhoto.Rows[0]["IDataSignature"]);
                    ObjModelUsers.StrSignatureData = Convert.ToString(dtPhoto.Rows[0]["IDataSignature"]);


                }
                #endregion

                List<NameCode> lstEditUserList = new List<NameCode>();
                List<NameCode> lstEditBranchList = new List<NameCode>();
                List<NameCode> lstLangList = new List<NameCode>();
                List<NameCode> lstItemsToBeInspected = new List<NameCode>();
                List<NameCode> lstCoreField = new List<NameCode>();
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCreateUser.GetDdlLst();
                if (DSEditGetList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstEditBranchList = (from n in DSEditGetList.Tables[0].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSEditGetList.Tables[0].Columns["Branch_Name"].ToString()),
                                             Code = n.Field<Int32>(DSEditGetList.Tables[0].Columns["Br_Id"].ToString())
                                         }).ToList();
                }
                IEnumerable<SelectListItem> BranchNameItems;
                BranchNameItems = new SelectList(lstEditBranchList, "Code", "Name");
                ViewData["BranchNameItems"] = BranchNameItems;

                if (DSEditGetList.Tables[1].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstEditUserList = (from n in DSEditGetList.Tables[1].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSEditGetList.Tables[1].Columns["RoleName"].ToString()),
                                           Code = n.Field<Int32>(DSEditGetList.Tables[1].Columns["UserRoleID"].ToString())

                                       }).ToList();
                }
                IEnumerable<SelectListItem> UserNameItems;
                UserNameItems = new SelectList(lstEditUserList, "Code", "Name");
                ViewData["RoleNameItems"] = UserNameItems;

                if (DSEditGetList.Tables[3].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstLangList = (from n in DSEditGetList.Tables[3].AsEnumerable()
                                   select new NameCode()
                                   {
                                       Name = n.Field<string>(DSEditGetList.Tables[3].Columns["LangName"].ToString()),
                                       Code = n.Field<Int32>(DSEditGetList.Tables[3].Columns["Id"].ToString())
                                   }).ToList();
                }
                IEnumerable<SelectListItem> LangNameItems;
                LangNameItems = new SelectList(lstLangList, "Code", "Name");
                ViewData["LangNameItems"] = LangNameItems;
                Session["LangNameItems"] = LangNameItems;

                if (DSEditGetList.Tables[4].Rows.Count > 0)//All Items to be Inspected
                {
                    lstItemsToBeInspected = (from n in DSEditGetList.Tables[4].AsEnumerable()
                                             select new NameCode()
                                             {
                                                 Name = n.Field<string>(DSEditGetList.Tables[4].Columns["Name"].ToString()),
                                                 Code = n.Field<Int32>(DSEditGetList.Tables[4].Columns["Product_ID"].ToString())
                                             }).ToList();
                }
                IEnumerable<SelectListItem> Items;
                Items = new SelectList(lstItemsToBeInspected, "Code", "Name");
                ViewData["AllItemToBeInspected"] = Items;

                //added by shrutika salve 28022024
                if (DSEditGetList.Tables[5].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                {
                    lstCoreField = (from n in DSEditGetList.Tables[5].AsEnumerable()
                                    select new NameCode()
                                    {
                                        Name = n.Field<string>(DSEditGetList.Tables[5].Columns["CoreFieldName"].ToString()),
                                        Code = n.Field<Int32>(DSEditGetList.Tables[5].Columns["Pk_fieldid"].ToString())
                                    }).ToList();
                }


                IEnumerable<SelectListItem> item2;
                item2 = new SelectList(lstCoreField, "Code", "Name");
                ViewData["item2"] = item2;
                // ViewBag.CoreField = item2;

                Session["UserProfile"] = ObjModelUsers;
                return View(ObjModelUsers);
            }
            else
            {
                DTEditUser = objDalCreateUser.EditUsers(Convert.ToString(Session["UserIDs"]));
                if (DTEditUser.Rows.Count > 0)
                {

                    ObjModelUsers.PKId = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);
                }
                DataSet DSGetList = new DataSet();
                List<NameCode> lstRoleList = new List<NameCode>();
                List<NameCode> lstBranchList = new List<NameCode>();
                List<NameCode> lstLangList = new List<NameCode>();
                List<NameCode> lstItemsToBeInspected = new List<NameCode>();
                //added by shrutrika salve 28022024
                List<NameCode> lstCoreField = new List<NameCode>();

                DSGetList = objDalCreateUser.GetDdlLst();
                if (DSGetList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetList.Tables[0].Rows.Count > 0)//Dynamic Binding Branch  DropDwonlist
                    {
                        lstBranchList = (from n in DSGetList.Tables[0].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetList.Tables[0].Columns["Branch_Name"].ToString()),
                                             Code = n.Field<Int32>(DSGetList.Tables[0].Columns["Br_Id"].ToString())

                                         }).ToList();
                    }
                    ViewBag.BranchName = lstBranchList;
                    IEnumerable<SelectListItem> BranchNameItems;
                    BranchNameItems = new SelectList(lstBranchList, "Code", "Name");
                    ViewData["BranchNameItems"] = BranchNameItems;

                    if (DSGetList.Tables[1].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstRoleList = (from n in DSGetList.Tables[1].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSGetList.Tables[1].Columns["RoleName"].ToString()),
                                           Code = n.Field<Int32>(DSGetList.Tables[1].Columns["UserRoleID"].ToString())

                                       }).ToList();
                    }
                    ViewBag.UserRole = lstRoleList;

                    if (DSGetList.Tables[3].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstLangList = (from n in DSGetList.Tables[3].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSGetList.Tables[3].Columns["LangName"].ToString()),
                                           Code = n.Field<Int32>(DSGetList.Tables[3].Columns["Id"].ToString())
                                       }).ToList();
                    }


                    IEnumerable<SelectListItem> LangNameItems;
                    LangNameItems = new SelectList(lstLangList, "Code", "Name");
                    ViewData["LangNameItems"] = LangNameItems;

                    if (DSGetList.Tables[4].Rows.Count > 0)//All Items to be Inspected
                    {
                        lstItemsToBeInspected = (from n in DSGetList.Tables[4].AsEnumerable()
                                                 select new NameCode()
                                                 {
                                                     Name = n.Field<string>(DSGetList.Tables[4].Columns["Name"].ToString()),
                                                     Code = n.Field<Int32>(DSGetList.Tables[4].Columns["Product_ID"].ToString())
                                                 }).ToList();
                    }
                    IEnumerable<SelectListItem> Items;
                    Items = new SelectList(lstItemsToBeInspected, "Code", "Name");
                    ViewData["AllItemToBeInspected"] = Items;
                    //added by shrutika salve 28022024
                    if (DSGetList.Tables[5].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstCoreField = (from n in DSGetList.Tables[5].AsEnumerable()
                                        select new NameCode()
                                        {
                                            Name = n.Field<string>(DSGetList.Tables[5].Columns["CoreFieldName"].ToString()),
                                            Code = n.Field<Int32>(DSGetList.Tables[5].Columns["Pk_fieldid"].ToString())
                                        }).ToList();
                    }


                    IEnumerable<SelectListItem> item2;
                    item2 = new SelectList(lstCoreField, "Code", "Name");
                    //ViewData["item2"] = item2;
                    ViewBag.CoreField = item2;


                }
            }
            return View(ObjModelUsers);
        }

        public ActionResult CreateCV()
        {
            ObjModelUsers = Session["UserProfile"] as Users;
            ObjModelUsers.FullName = ObjModelUsers.FirstName + " " + ObjModelUsers.MiddleName + " " + ObjModelUsers.LastName;
            DateTime now = DateTime.Now;
            DateTime DOB = Convert.ToDateTime(ObjModelUsers.DOB);
            int DOByears = now.Year - DOB.Year;
            int DOBmonths = now.Month - DOB.Month;

            // Adjust for the possibility of negative months
            if (DOBmonths < 0)
            {
                DOByears--;
                DOBmonths += 12;
            }
            ObjModelUsers.AgeCalculator = DOByears + " Years and " + DOBmonths + " Months";

            DateTime DOJ = Convert.ToDateTime(ObjModelUsers.DateOfJoining);
            int DOJyears = now.Year - DOJ.Year;
            int DOJmonths = now.Month - DOJ.Month;

            // Adjust for the possibility of negative months
            if (DOJmonths < 0)
            {
                DOJyears--;
                DOJmonths += 12;
            }

            string FinalMonth = string.Empty;
            if (DOJmonths <= 9)
            {
                FinalMonth = "0" + Convert.ToString(DOJmonths);
            }
            else
            {
                FinalMonth = Convert.ToString(DOJmonths);
            }

            ObjModelUsers.TUVTotalyearofExperience = DOJyears + " Years and " + FinalMonth + " Months";
            ViewBag.lstDOrderType = Session["EducationDetails"];
            ViewBag.lstProfCerts = Session["lstProfCerts"];

            DataSet DSGetUserCV = new DataSet();
            DSGetUserCV = objDalCreateUser.GetUserCV(Convert.ToString(Session["UserIDs"]));

            string UpdatedOn = null;
            string UpdatedBy = string.Empty;
            string DownloadOn = null;
            string Inspection = string.Empty;
            string ExpertiseSummary = string.Empty;
            bool IsInspectionCheck = true;

            if (DSGetUserCV.Tables[0].Rows.Count != 0)
            {
                UpdatedOn = DSGetUserCV.Tables[0].Rows[0].ItemArray[5].ToString();
                DownloadOn = DSGetUserCV.Tables[0].Rows[0].ItemArray[6].ToString();
                UpdatedBy = DSGetUserCV.Tables[0].Rows[0].ItemArray[7].ToString();
                IsInspectionCheck = Convert.ToBoolean(DSGetUserCV.Tables[0].Rows[0].ItemArray[8]);
                Inspection = DSGetUserCV.Tables[0].Rows[0].ItemArray[2].ToString();
                ExpertiseSummary = DSGetUserCV.Tables[0].Rows[0].ItemArray[3].ToString();
            }
            else
            {
                UpdatedOn = "00.00.000";
                DownloadOn = "00.00.000";
                UpdatedBy = "";
                IsInspectionCheck = false;
                Inspection = "";
                ExpertiseSummary = "";
            }

            ObjModelUsers.CVUpdatedOn = UpdatedOn;
            ObjModelUsers.CVDownloadedOn = DownloadOn;
            ObjModelUsers.Inspection = Inspection;
            ObjModelUsers.ExpertiseSummary = ExpertiseSummary;
            ObjModelUsers.CVUpdatedBy = UpdatedBy;
            ObjModelUsers.IsInspectionCheck = IsInspectionCheck;

            List<Employment> lstEmployment = new List<Employment>();
            DataTable EmploymentDetails = new DataTable();
            EmploymentDetails = DSGetUserCV.Tables[1];
            Session["EmploymentDetails"] = EmploymentDetails;

            List<ProjectCS> lstProject = new List<ProjectCS>();
            DataTable ProjectDetails = new DataTable();
            ProjectDetails = DSGetUserCV.Tables[2];
            Session["ProjectDetails"] = ProjectDetails;

            List<Training> lstTraining = new List<Training>();
            DataTable TrainingDetails = new DataTable();
            TrainingDetails = DSGetUserCV.Tables[3];
            Session["TrainingDetails"] = TrainingDetails;

            List<Training> lstTUVTraining = new List<Training>();
            DataTable TUVTrainingDetails = new DataTable();
            TUVTrainingDetails = DSGetUserCV.Tables[4];
            Session["TUVTrainingDetails"] = TUVTrainingDetails;

            if (EmploymentDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in EmploymentDetails.Rows)
                {
                    lstEmployment.Add(
                       new Employment
                       {

                           Period = Convert.ToString(dr["Period"]),
                           EmployerName = Convert.ToString(dr["EmployerName"]),
                           Location = Convert.ToString(dr["Location"]),
                           Designation = Convert.ToString(dr["Designation"]),
                           Responsibilities = Convert.ToString(dr["Responsibilities"]),
                       }
                     );
                }
            }
            ViewBag.lstEmployment = lstEmployment;


            if (ProjectDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in ProjectDetails.Rows)
                {
                    lstProject.Add(
                       new ProjectCS
                       {
                           Project = Convert.ToString(dr["Project"]),
                           ActivityItemInspected = Convert.ToString(dr["ActivityItemInspected"]),
                           Codes = Convert.ToString(dr["Codes"]),
                           EndUserName = Convert.ToString(dr["EndUserName"])
                       }
                     );
                }
            }
            ViewBag.lstProject = lstProject;

            if (TrainingDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in TrainingDetails.Rows)
                {
                    lstTraining.Add(
                       new Training
                       {
                           //ArrangedBy = Convert.ToString(dr["ArrangedBy"]),
                           Date = Convert.ToString(dr["Date"]),
                           Hours = Convert.ToString(dr["Hours"]),
                           Topic = Convert.ToString(dr["Topic"])
                       }
                     );
                }
            }
            ViewBag.lstTraining = lstTraining;

            if (TUVTrainingDetails.Rows.Count > 0)
            {
                foreach (DataRow dr in TUVTrainingDetails.Rows)
                {
                    lstTUVTraining.Add(
                       new Training
                       {
                           //ArrangedBy = Convert.ToString(dr["ArrangedBy"]),
                           Date = Convert.ToString(dr["Date"]),
                           Hours = Convert.ToString(dr["Hours"]),
                           Topic = Convert.ToString(dr["Topic"])
                       }
                     );
                }
            }
            ViewBag.lstTUVTraining = lstTUVTraining;

            List<CustomerAppreciation> lstCustomerAppreciation = new List<CustomerAppreciation>();
            DataTable CustomerAppreciation = new DataTable();
            CustomerAppreciation = DSGetUserCV.Tables[5];
            Session["CustomerAppreciation"] = CustomerAppreciation;

            if (CustomerAppreciation.Rows.Count > 0)
            {
                foreach (DataRow dr in CustomerAppreciation.Rows)
                {
                    lstCustomerAppreciation.Add(
                       new CustomerAppreciation
                       {
                           AchievementDate = Convert.ToString(dr["AchievementDate"]),
                           Description = Convert.ToString(dr["Description"])
                       }
                     );
                }
            }
            ViewBag.lstCustomerAppreciation = lstCustomerAppreciation;

            Session["UserProfile"] = ObjModelUsers;
            return View(ObjModelUsers);
        }

        [HttpPost]
        public ActionResult DownloadCV(string language, string Age, string TotalEXP, string Inspection, bool IsInspectionCheck, string ExpertiseSummary, string[][] EmploymentDetails, string[][] ProjectDetails, string[][] TrainingDetails)
        {
            string UserID = Session["UserID"] as string;
            SelectPdf.GlobalProperties.LicenseKey = "uZKImYuMiJmImYuIl4mZioiXiIuXgICAgA==";
            string body = string.Empty;
            System.Text.StringBuilder strs = new System.Text.StringBuilder();

            using (StreamReader reader = new StreamReader(Server.MapPath("~/QuotationHtml/CVHtml.html")))
            {
                body = reader.ReadToEnd();
            }
            ObjModelUsers = Session["UserProfile"] as Users;
            List<Users> lstEducation = new List<Users>();
            lstEducation = Session["EducationDetails"] as List<Users>;
            List<Users> lstProfessional = new List<Users>();
            lstProfessional = Session["lstProfCerts"] as List<Users>;
            DataTable DTP = Session["lstProfCerts"] as DataTable;

            if (lstEducation != null)
            {
                string EducationStr = string.Empty;
                foreach (var Edu in lstEducation)
                {
                    string Degree = Edu.Degree;
                    if (Degree == "BEng")
                    {
                        Degree = "Bachelor of Engineering";
                    }
                    else if (Degree == "BTech") { Degree = "Bachelor of Technology"; }
                    else if (Degree == "BA") { Degree = "Bachelor of Arts"; }
                    else if (Degree == "BSc") { Degree = "Bachelor of Science"; }
                    else if (Degree == "BCom") { Degree = "Bachelor of Commerce"; }
                    else if (Degree == "Tech") { Degree = "Technical"; }
                    else if (Degree == "NTech") { Degree = "Non Technical"; }
                    else if (Degree == "ME") { Degree = "Master of Engineering"; }

                    string YearOfPassing = Edu.YearOfPassing;
                    string MajorFieldOfStudy = Edu.MajorFieldOfStudy;
                    string Course = Edu.Course;
                    string University = Edu.UniversityName;
                    if (Course != "S.S.C" || Course != "H.S.C")
                    {
                        EducationStr = EducationStr + "<div class=\"col-md-12 row\"><span>" + YearOfPassing + " - " + Course + " " + Degree + " in " + MajorFieldOfStudy + " from Institute/University " + University + ".</span></div>";
                    }
                }
                body = body.Replace("[EducationalQualification]", EducationStr);
            }
            else
            {
                body = body.Replace("[EducationalQualification]", "");
            }

            if (lstProfessional != null)
            {
                string ProfessionalStr = string.Empty;
                foreach (var prof in lstProfessional)
                {
                    string Certification = prof.CertName;
                    string validTill = prof.CertValidTill;

                    ProfessionalStr = ProfessionalStr + "<div class=\"col-md-12 row\">" + Certification + " (Valid till " + validTill + ")</div>";
                }
                
                body = body.Replace("[ProfessionalQualification]", ProfessionalStr);
            }
            else
            {
                body = body.Replace("[ProfessionalQualification]", "");
            }
            ObjModelUsers.FullName = Regex.Replace(ObjModelUsers.FullName.ToString(), @"\b[a-z]", m => m.Value.ToUpper());
            body = body.Replace("[Name]", ObjModelUsers.FullName.ToString());
            //string ImageName = ObjModelUsers.StrIData.ToString();
            string ImageName = string.Empty;
            if (ObjModelUsers.StrIData != null)
            {
                ImageName = ObjModelUsers.StrIData.ToString();
            }
            else
            {
                ImageName = "NoImage.jpg";
            }
            string path = "Content/Sign/";
            string filePath = AppDomain.CurrentDomain.BaseDirectory + path.Replace("/", "\\") + ImageName;
            string CompressfilePath = AppDomain.CurrentDomain.BaseDirectory + "CompressFiles\\" + ImageName;
            if (!System.IO.File.Exists(CompressfilePath))
            {
                Image originalImage = Image.FromFile(filePath);

                Size newSize = new Size(500, 300);

                Image resizedImage = CommonControl.ResizeImage(originalImage, newSize);

                // Save the resized image with JPEG compression and quality settings

                CommonControl.SaveImageWithCompression(resizedImage, CompressfilePath, 99); // 99 is the quality level (0-100)

                // Dispose the original and resized images after use
                originalImage.Dispose();
                resizedImage.Dispose();
            }

            //string I = "<img src = '" + CompressfilePath + "' style='height: 220px;width: 190px;margin-top: 35px;float:right' align='center'>";
            string I = "<img src = 'https://tiimes.tuv-india.com/CompressFiles/" + ImageName + "' style='height: 220px;width: 190px;margin-top: 35px;float:right' align='center'>";
            body = body.Replace("[ProfileImage]", I);
            body = body.Replace("[DoB]", ObjModelUsers.DOB.ToString());
            body = body.Replace("[Age]", Age);
            body = body.Replace("[Nationality]", "Indian");
            body = body.Replace("[Location]", ObjModelUsers.MainBranch.ToString());
            body = body.Replace("[Gender]", ObjModelUsers.Gender.ToString());
            body = body.Replace("[MaritalStatus]", ObjModelUsers.Marital_Status.ToString());
            body = body.Replace("[Languages]", language);
            body = body.Replace("[TUVDOJ]", ObjModelUsers.DateOfJoining.ToString());
            body = body.Replace("[ExpTUV]", ObjModelUsers.TUVTotalyearofExperience.ToString());
            body = body.Replace("[TotalExp]", TotalEXP.Replace("y", "Y").Replace("m", "M"));
            body = body.Replace("[Inspection]", Inspection.Replace("\n", "<br />"));
            body = body.Replace("[ExpertiseSummary]", ExpertiseSummary);
            body = body.Replace("[UpdatedOn]", ObjModelUsers.CVUpdatedOn.ToString());
            body = body.Replace("[DownloadedOn]", ObjModelUsers.CVDownloadedOn.ToString());

            DataTable DTEmploymentDetails = new DataTable();
            DTEmploymentDetails = ConvertToDataTable(EmploymentDetails);
            string StrEmployment = string.Empty;

            foreach (DataRow row in DTEmploymentDetails.Rows)
            {

                StrEmployment = StrEmployment + "<div class=\"col-md-12 row\"><div class=\"col-md-3\"><b>Period</b></div><b>:</b><div class=\"col-md-8\">"
                    + row["Period"].ToString() + "</div></div><div class=\"col-md-12 row\"><div class=\"col-md-3\"><b>Employer Name</b></div><b>:</b><div class=\"col-md-8\">"
                    + row["EmployerName"].ToString() + "</div></div><div class=\"col-md-12 row\"><div class=\"col-md-3\"><b>Location</b></div><b>:</b><div class=\"col-md-8\">"
                    + row["Location"].ToString() + "</div></div><div class=\"col-md-12 row\"><div class=\"col-md-3\"><b>Designation</b></div><b>:</b><div class=\"col-md-8\">"
                    + row["Designation"].ToString() + "</div></div><div class=\"col-md-12 row\"><div class=\"col-md-3\"><b>Responsibilities</b></div><b>:</b><div class=\"col-md-8 Res\">"
                    + row["Responsibilities"].ToString()+ "</div></div><br/>";
            }

            body = body.Replace("[EmploymentDetails]", StrEmployment);

            DataTable DTProjectDetails = new DataTable();
            DTProjectDetails = ConvertToDataTable(ProjectDetails);
            string StrProject = string.Empty;

            StrProject = "<div class=\"col-md-12\"><table id=\"ProjectTable\" cellpadding=\"1\" cellspacing=\"1\" border=\"1\" width=\"101% \"><thead><tr><th style=\"width: 20px\">Item Details</th><th style=\"width: 20px\">Code / Standard / Specification / MOC</th><th style=\"width: 20px\">Customer / End Customer / Project Name</th><th style=\"width: 20px\">Manufacturer Name and Location</th></tr></thead>" + ""
                    + "<tbody>";

            foreach (DataRow row in DTProjectDetails.Rows)
            {
                StrProject = StrProject + "<tr><td>" + row["ActivityItemInspected"].ToString().Replace("\n", "<br / >") + "</td><td>" + row["Codes"].ToString().Replace("\n", "<br / >")
                    + " </td><td> " + row["Project"].ToString().Replace("\n", "<br / >") + " </td><td> " + row["EndUserName"].ToString().Replace("\n", "<br / >") + "</td></tr>";
            }

            StrProject = StrProject + "</tbody></table></div>";

            body = body.Replace("[ProjectDetails]", StrProject);

            DataTable DTTrainingDetails = new DataTable();
            DTTrainingDetails = ConvertToDataTable(TrainingDetails);
            string StrTraining = string.Empty;

            //StrTraining = "<div><div style=\"border: 1px solid;\"><p style=\"margin-left:20px;margin-top:5px;\"><b>Self - Arranged</b></p></div><table width=\"100%\"><thead><tr><th>Date</th><th>Hours</th><th>Topic</th></tr></thead><tbody>";
            StrTraining = "<div><table width=\"100%\"><thead><tr><th style=\"width:134px;\">Date</th style=\"width:81px;\"><th>Hours</th><th>Topic</th></tr></thead><tbody>";

            foreach (DataRow row in DTTrainingDetails.Rows)
            {
                StrTraining = StrTraining + "<tr><td>" + row["Date"].ToString() + " </td><td> " + row["Hours"].ToString() + "</td><td>" + row["Topic"].ToString() + " </td></tr>";
            }
            //StrTraining = StrTraining + "</tbody></table></div>";
            //body = body.Replace("[TrainingDetails]", StrTraining);

            DataTable TUVTrainingDetails = new DataTable();
            TUVTrainingDetails = Session["TUVTrainingDetails"] as DataTable;
            string StrTUVTraining = string.Empty;

            //StrTUVTraining = "<div style=\"border:1px solid\"><p style=\"margin-left:20px;margin-top:5px;\"><b>Arranged by TUVI</b></p></div><table width=\"100%\"><thead><tr><th>Date</th><th>Hours</th><th>Topi</th></tr></thead><tbody>";
            StrTUVTraining = "<div><table width=\"100%\"><thead><tr><th style=\"width:134px;\">Date</th style=\"width:81px;\"><th>Hours</th><th>Topic</th></tr></thead><tbody>";

            foreach (DataRow row in TUVTrainingDetails.Rows)
            {
                StrTraining = StrTraining + "<tr><td>" + row["Date"].ToString() + " </td><td> " + row["Hours"].ToString() + "</td><td>" + row["Topic"].ToString() + " </td></tr>";
            }
            //StrTUVTraining = StrTUVTraining + "</tbody></table></div>";
            //body = body.Replace("[TUVTrainingDetails]", StrTUVTraining);

            StrTraining = StrTraining + "</tbody></table></div>";
            body = body.Replace("[TrainingDetails]", StrTraining);

            DataTable AchievementDetails = new DataTable();
            AchievementDetails = Session["CustomerAppreciation"] as DataTable;
            string StrAchievement = string.Empty;

            StrAchievement = "<div class=\"col-md-12\"><table cellpadding=\"1\" cellspacing=\"1\" border=\"1\" width=\"101% \"><thead><tr><th>Date</th><th>Description</th></tr></thead>" + ""
                    + "<tbody>";

            foreach (DataRow row in AchievementDetails.Rows)
            {
                StrAchievement = StrAchievement + "<tr><td>" + row["AchievementDate"].ToString() + " </td><td> " + row["Description"].ToString() + "</td></tr>";
            }

            StrAchievement = StrAchievement + "</tbody></table></div>";

            body = body.Replace("[Achievement]", StrAchievement);

            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            HtmlToPdf converter = new HtmlToPdf();


            #region Count Page No
            SelectPdf.PdfDocument doc1 = converter.ConvertHtmlString(body);
            int PageCount = doc1.Pages.Count;
            body = body.Replace("[PageCount]", "(Refer Page " + Convert.ToString(PageCount) + " Of " + Convert.ToString(PageCount) + " )");
            strs.Append(body);

            converter.Options.MaxPageLoadTime = 240;
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;

            string _Header = string.Empty;
            string _footer = string.Empty;

            StreamReader _readHeader_File = new StreamReader(Server.MapPath("~/QuotationHtml/cv-header.html"));
            _Header = _readHeader_File.ReadToEnd();
            _Header = _Header.Replace("[logo]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/logo.svg");
            _Header = _Header.Replace("[Name]", ObjModelUsers.FullName.ToString());
            _Header = _Header.Replace("[Designation]", ObjModelUsers.Designation.ToString());
            _Header = _Header.Replace("[TotalExp]", TotalEXP.Replace("y", "Y").Replace("m", "M"));

            StreamReader _readFooter_File = new StreamReader(Server.MapPath("~/QuotationHtml/cv-footer.html"));
            _footer = _readFooter_File.ReadToEnd();
            _footer = _footer.Replace("[LogoFooter]", ConfigurationManager.AppSettings["Web"].ToString() + "/AllJsAndCss/images/FTUEV-NORD-GROUP_Logo_Electric-Blue.svg");

            // header settings
            converter.Options.DisplayHeader = true ||
                true || true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = 85;

            PdfHtmlSection headerHtml = new PdfHtmlSection(_Header, string.Empty);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;

            converter.Header.Add(headerHtml);

            // footer settings
            converter.Options.DisplayFooter = true || true || true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = 85;

            PdfHtmlSection footerHtml = new PdfHtmlSection(_footer, string.Empty);
            footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Footer.Add(footerHtml);

            #region Footer Code

            PdfTextSection text1 = new PdfTextSection(30, 60, "Page: {page_number} of {total_pages}", new System.Drawing.Font("TNG Pro", 9, FontStyle.Bold));

            converter.Footer.Add(text1);

            converter.Options.SecurityOptions.CanAssembleDocument = true;
            converter.Options.SecurityOptions.CanCopyContent = true;
            converter.Options.SecurityOptions.CanEditAnnotations = true;
            converter.Options.SecurityOptions.CanEditContent = true;
            converter.Options.SecurityOptions.CanFillFormFields = true;
            converter.Options.SecurityOptions.CanPrint = true;

            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(body);

            if (IsInspectionCheck != true || Inspection == "")
            {
                HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='InspectionDiv']");
                HtmlNode HRToRemove = htmldoc.DocumentNode.SelectSingleNode("//hr[@id='InspectionHr']");

                if (divToRemove != null)
                {
                    divToRemove.Remove();
                    HRToRemove.Remove();
                }

                body = htmldoc.DocumentNode.OuterHtml;
            }

            if (DTProjectDetails.Rows.Count == 0)
            {
                HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='ProjectDiv']");
                HtmlNode HRToRemove = htmldoc.DocumentNode.SelectSingleNode("//hr[@id='ProjectHr']");

                if (divToRemove != null)
                {
                    divToRemove.Remove();
                    HRToRemove.Remove();
                }

                body = htmldoc.DocumentNode.OuterHtml;
            }

            if (DTTrainingDetails.Rows.Count == 0 && TUVTrainingDetails.Rows.Count == 0)
            {
                HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='TrainingDiv']");
                HtmlNode HRToRemove = htmldoc.DocumentNode.SelectSingleNode("//hr[@id='TrainingHr']");

                if (divToRemove != null)
                {
                    divToRemove.Remove();
                    HRToRemove.Remove();
                }

                body = htmldoc.DocumentNode.OuterHtml;
            }

            if (AchievementDetails.Rows.Count == 0)
            {
                HtmlNode divToRemove = htmldoc.DocumentNode.SelectSingleNode("//div[@id='AchievementDiv']");
                HtmlNode HRToRemove = htmldoc.DocumentNode.SelectSingleNode("//hr[@id='AchievementHr']");
                if (divToRemove != null) { divToRemove.Remove(); HRToRemove.Remove(); }
                body = htmldoc.DocumentNode.OuterHtml;
            }

            PdfDocument doc = converter.ConvertHtmlString(body);

            HtmlDocument doc2 = new HtmlDocument();
            doc2.LoadHtml(body);

            // Find and remove the specified style element
            HtmlNode styleNode = doc2.DocumentNode.SelectSingleNode("//style[contains(text(),'hr {')]");
            if (styleNode != null)
            {
                styleNode.Remove();
            }

            body = doc2.DocumentNode.OuterHtml;

            string result = Regex.Replace(body, "<.*?>", String.Empty).Replace("\r", "").Replace("\n", "").Replace("&amp;", "&");
            SaveCVHTML(result, UserID);

            string ReportName = "TUVIPL " + ObjModelUsers.MainBranch + " " + ObjModelUsers.FullName + ".pdf";
            string Savepath = Server.MapPath("~/CV");
            doc.Save(Savepath + '\\' + ReportName);
            doc.Close();
            Session["ReportName"] = ReportName;


            #endregion

            #endregion

            objDalCreateUser.UpdateCVDownloadDate(UserID, Session["ReportName"].ToString());

            return Json(true, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveCVHTML(string Html, string UserID)
        {
            objDalCreateUser.SaveCVHTML(Html, UserID);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CV()
        {
            string path = Server.MapPath("~/CV");
            string ReportName = Convert.ToString(Session["ReportName"]);
            // Check if the file exists
            if (!System.IO.File.Exists(path + '\\' + ReportName))
            {
                return Json("404", JsonRequestBehavior.AllowGet);
            }

            // Read the file content into a byte array
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + '\\' + ReportName);

            // Return the file as a byte array
            return File(fileBytes, "application/pdf", ReportName);
        }

        public ActionResult RefetchInspectionData()
        {
            try
            {
                string UserID = Convert.ToString(Session["UserIDs"]);

                string Inspection = objDalCreateUser.RefetchInspectionData(UserID);
                //string Inspection = JsonConvert.SerializeObject(result);
                return Json(Inspection, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }

        }

        //added by nikita on 28/09/2023

        public ActionResult GetPch(string brid)
        {
            try
            {

                DALUsers dalusers = new DALUsers();
                var result = dalusers.GetPchName(brid);
                string Name = JsonConvert.SerializeObject(result);
                return Json(Name, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }

        }



        [HttpPost]

        public ActionResult CreateUser(Users URS, HttpPostedFileBase File, FormCollection fc, List<Users> DArray, List<Users> DArrayPFC, List<Users> DArrayEYE)
        {
            string Result = string.Empty;
            string Result1 = string.Empty;
            string strPKUserID = string.Empty;
            DataTable dtPkUserID = new DataTable();

            try
            {
                var Data1 = objDalCreateUser.GetCostCenterList();
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
                DataSet CostCenter = objNIA.GetServiceCode();
                ObjModelUsers.CostCenter = Convert.ToInt32(CostCenter.Tables[0].Rows[0]["CostCenter"].ToString());

                string[] Brsplit;
                string ListBranch = string.Empty;
                ListBranch = string.Join(",", fc["BrAuditeeName"]);
                Brsplit = ListBranch.Split(',');
                ViewData["ListBranchchecked"] = ListBranch;
                // URS.FK_BranchID = ListBranch;
                // URS.Branch = ListBranch;

                string[] Langsplit;
                string ListLang = string.Empty;
                ListLang = string.Join(",", fc["LangAuditee"]);
                Langsplit = ListLang.Split(',');
                ViewData["ListLangchecked"] = ListLang;

                string[] split;
                string ListEmpType = string.Empty;

                ListEmpType = string.Join(",", fc["AuditeeName"]);
                split = ListEmpType.Split(',');
                ViewData["ListPKSubJobChecked"] = ListEmpType;
                //  URS.Employee_Type = ListEmpType;
                DataSet dsCertCount = new DataSet();
                DataSet dsVerified = new DataSet();
                int QualDetCnt = 0;
                int DArrayPFCCnt = 0;
                int chkVerified = 1;
                int chkVerifiedExist = 1;
                if (URS.PK_UserID != "" && URS.PK_UserID != null)
                {
                    dsCertCount = objDalCreateUser.GetCertificatesCount(URS.PK_UserID);

                    if (dsCertCount.Tables.Count > 0)
                    {
                        DArrayPFCCnt = Convert.ToInt32(dsCertCount.Tables[0].Rows[0]["Cnt"]);
                        QualDetCnt = Convert.ToInt32(dsCertCount.Tables[1].Rows[0]["Cnt"]);

                    }

                    Result = objDalCreateUser.InsertUpdateUsers(URS);

                    dsVerified = objDalCreateUser.GetVerifiedFiled(URS.PK_UserID);

                    if (dsVerified.Tables.Count > 0)
                    {
                        chkVerifiedExist = Convert.ToInt32(dsVerified.Tables[0].Rows[0]["isVerified"]);
                    }

                    if (Result != "" && Result != null)
                    {
                        #region Qualification Detail

                        if (chkVerifiedExist != 0)
                        {
                            if (DArrayPFC != null)
                            {
                                if (DArrayPFCCnt != DArrayPFC.Count)
                                {
                                    chkVerified = 0;
                                }
                            }
                            if (DArray != null)
                            {
                                if (QualDetCnt != DArray.Count)
                                {
                                    chkVerified = 0;
                                }
                            }


                        }
                        else
                        {
                            chkVerified = 0;
                        }

                        if (DArray != null)
                        {
                            foreach (var d in DArray)
                            {

                                URS.Course = d.Course;
                                URS.Degree = d.Degree;
                                URS.MajorFieldOfStudy = d.MajorFieldOfStudy;
                                URS.UniversityName = d.UniversityName;
                                URS.OtherUniversity = d.OtherUniversity;
                                URS.LastYearCGPA = d.LastYearCGPA;
                                URS.AggregateCGPA = d.AggregateCGPA;
                                URS.YearOfPassing = d.YearOfPassing;


                                Result1 = objDalCreateUser.InsertQualificationDetail(URS, Convert.ToInt32(Result), chkVerified);
                            }
                        }



                        #endregion

                        #region proffesional Certificates
                        if (DArrayPFC != null)
                        {
                            foreach (var d in DArrayPFC)
                            {

                                URS.CertName = d.CertName;
                                URS.CertNo = d.CertNo;
                                URS.CertIssueDate = d.CertIssueDate;
                                URS.CertValidTill = d.CertValidTill;

                                //Result1 = objDalCreateUser.InsertProfsionalCerts(URS, Convert.ToInt32(Result), chkVerified);
                                string checkResult = objDalCreateUser.CheckCertNameExistence(URS);

                                if (checkResult == "0")
                                {
                                    return Json(new { success = 2, responseText = "Code matched" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    Result1 = objDalCreateUser.InsertProfsionalCerts(URS, Convert.ToInt32(Result), chkVerified);
                                }
                            }
                        }

                        #endregion

                        #region Eye Test Report
                        if (DArrayEYE != null)
                        {
                            foreach (var d in DArrayEYE)
                            {

                                URS.PK_ID = d.PK_ID;
                                URS.CreatedBy1 = d.CreatedBy1;
                                URS.CertIssueDate = d.CertIssueDate;
                                //URS.CertValidTill = d.CertValidTill;
                                Result1 = objDalCreateUser.InsertEyeTest(URS, Convert.ToInt32(Result), chkVerified);





                            }
                        }

                        #endregion


                        TempData["UpdateUsers"] = Result;

                        if (URS.LoginUserRoleId == 36)
                        {
                            dtPkUserID = objDalCreateUser.GetPKUserID(Convert.ToInt32(Result));

                            if (dtPkUserID.Rows.Count > 0)
                            {
                                strPKUserID = Convert.ToString(dtPkUserID.Rows[0]["PK_UserID"]);


                                string Appr1  = Convert.ToString(dtPkUserID.Rows[0]["Apr1Mail"]);
                                string Appr2  = Convert.ToString(dtPkUserID.Rows[0]["Apr2Mail"]);
                                string PCH =  Convert.ToString(dtPkUserID.Rows[0]["PCHEmail"]);
                                string BranchQA  = Convert.ToString(dtPkUserID.Rows[0]["BranchQA"]);
                                string Username = Convert.ToString(dtPkUserID.Rows[0]["UserName"]);
                                string UserEmail = Convert.ToString(dtPkUserID.Rows[0]["Tuv_Email_Id"]);

                               // SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);

                            }

                            return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = strPKUserID }) });
                            ////return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = Convert.ToInt32(Result) }) });
                        }
                        else
                        {
                            return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = URS.PK_UserID }) });
                        }


                    }
                }
                else
                {
                    chkVerified = 0;

                    string Pass = Encrypt("Pass@123");
                    URS.Password = Pass;
                    Result = objDalCreateUser.InsertUpdateUsers(URS);


                    if (Result != null && Result != "")
                    {
                        #region Qualification Detail
                        if (DArray != null)
                        {
                            foreach (var d in DArray)
                            {

                                URS.Course = d.Course;
                                URS.Degree = d.Degree;
                                URS.MajorFieldOfStudy = d.MajorFieldOfStudy;
                                URS.University = d.University;
                                URS.OtherUniversity = d.OtherUniversity;
                                URS.LastYearCGPA = d.LastYearCGPA;
                                URS.AggregateCGPA = d.AggregateCGPA;

                                Result1 = objDalCreateUser.InsertQualificationDetail(URS, Convert.ToInt32(Result), chkVerified);
                            }
                        }

                        #endregion

                        #region proffesional Certificates
                        if (DArrayPFC != null)
                        {
                            foreach (var d in DArrayPFC)
                            {

                                URS.CertName = d.CertName;
                                URS.CertNo = d.CertNo;
                                URS.CertIssueDate = d.CertIssueDate;
                                URS.CertValidTill = d.CertValidTill;

                                Result1 = objDalCreateUser.InsertProfsionalCerts(URS, Convert.ToInt32(Result), chkVerified);
                            }
                        }

                        #endregion


                        TempData["InsertUsers"] = Result;


                        dtPkUserID = objDalCreateUser.GetPKUserID(Convert.ToInt32(Result));

                        if (dtPkUserID.Rows.Count > 0)
                        {
                            strPKUserID = Convert.ToString(dtPkUserID.Rows[0]["PK_UserID"]);

                            string Appr1 = Convert.ToString(dtPkUserID.Rows[0]["Apr1Mail"]);
                            string Appr2 = Convert.ToString(dtPkUserID.Rows[0]["Apr2Mail"]);
                            string PCH = Convert.ToString(dtPkUserID.Rows[0]["PCHEmail"]);
                            string BranchQA = Convert.ToString(dtPkUserID.Rows[0]["BranchQA"]);
                            string Username = Convert.ToString(dtPkUserID.Rows[0]["UserName"]);
                            string UserEmail = Convert.ToString(dtPkUserID.Rows[0]["Tuv_Email_Id"]);
                            if (Convert.ToString(dtPkUserID.Rows[0]["EmployementCategory"]) == "2" || Convert.ToString(dtPkUserID.Rows[0]["EmployementCategory"]) == "3")
                            {
                                SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
                            }
                            else if (Convert.ToString(dtPkUserID.Rows[0]["EmployementCategory"]) == "5" || Convert.ToString(dtPkUserID.Rows[0]["EmployementCategory"]) == "1" || Convert.ToString(dtPkUserID.Rows[0]["EmployementCategory"]) == "6")
                            {
                                SendInductionMailOtherThanPayroll(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
                            }
                            else
                            {

                            }
                            
                        }
                        return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = strPKUserID }) });
                        /*   if (URS.LoginUserRoleId == 36)
                           {
                               // return Json(new { result = "Redirect", url = Url.Action("UserDashBoard", "Users") });
                               return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = Convert.ToInt32(Result) }) });
                           }
                           else
                           {
                               return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = URS.PK_UserID }) });
                           }
   */
                        //  return Json(new { result = "Redirect", url = Url.Action("CreateUser", "Users", new { @UserID = URS.PK_UserID }) });
                        //return RedirectToAction("CreateUser");
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return RedirectToAction("UserDashBoard");
            //return RedirectToAction("CreateUser");

        }


        public JsonResult TemporaryFilePathUserSignature()//Photo Uploading Functionality For Adding TemporaryFilePathFollowupReport
        {
            try
            {
                var IPath = string.Empty;
                FormCollection fc = new FormCollection();
                string filePath = string.Empty;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                    int fileSize = file.ContentLength;
                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.FileName.ToUpper().EndsWith(".JPEG") || file.FileName.ToUpper().EndsWith(".JPG") || file.FileName.ToUpper().EndsWith(".GIF") || file.FileName.ToUpper().EndsWith(".PNG"))
                        {
                            string fileName = file.FileName;
                            filePath = Path.Combine(Server.MapPath("~/UsersSignature/"), filePath);
                            var K = "/UsersSignature/" + fileName;
                            IPath = K.TrimStart('~');
                            file.SaveAs(Server.MapPath(IPath));
                            Session["UserSigFileName"] = fileName;
                            Session["UserSigFilePath"] = IPath;
                            TempData["File"] = file;
                            TempData.Keep();
                        }
                        else
                        {
                            ViewBag.Error = "Please Select only jpeg, jpg, gif and png File";
                        }
                    }

                }

                return Json(IPath, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //added by nikita on 29092023
        public ActionResult PasswordReset(string userid)
        {

            try
            {
                DALUsers dalusers = new DALUsers();
                string Password = "Pass@123";
                var result = dalusers.PasswordReset(userid, Password);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Something went wrong", JsonRequestBehavior.AllowGet);

            }
        }


        [HttpGet]
        public ActionResult Delete(string UserID)
        {
            int Result = 0;
            try
            {
                Result = objDalCreateUser.DeleteUser(UserID);
                if (Result != 0)
                {
                    TempData["DeleteUser"] = Result;
                    return RedirectToAction("UserDashBoard", "Users");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }
        [HttpGet]
        public ActionResult CheckingUserNameExist(string ExistingUserName)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistUsertName = new DataTable();
            try
            {
                DTExistUsertName = objDalCreateUser.CheckExistingProductName(ExistingUserName);
                if (DTExistUsertName.Rows.Count > 0)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult CheckingHrEmployeeCode(string ExistingUserName)//Checking Existing User Name
        {
            string Result = string.Empty;
            DataTable DTExistUsertName = new DataTable();
            try
            {
                DTExistUsertName = objDalCreateUser.CheckExistingHrEmployeeCode(ExistingUserName);
                if (DTExistUsertName.Rows.Count > 0)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }


        #region   Profile Edit Update And My Profile Display Code 

        /*
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            string UserID = Convert.ToString(Session["UserIDs"]);
            string dateofb = null;


            DataTable DTUserDashBoard = new DataTable();
            List<Users> lstUserDashBoard = new List<Users>();
            DTUserDashBoard = objDalCreateUser.GetUserDashBoard();

            if (DTUserDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTUserDashBoard.Rows)
                {
                    lstUserDashBoard.Add(
                        new Users
                        {
                            PK_UserID = Convert.ToString(dr["PK_UserID"]),
                            FullName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                        }
                        );
                }
            }
            ViewBag.Employees = new SelectList(lstUserDashBoard, "PK_UserID", "FullName");
            if (UserID != "" && UserID != null)
            {
                DataTable DTEditUser = new DataTable();
                DTEditUser = objDalCreateUser.EditProfile(UserID);
                if (DTEditUser.Rows.Count > 0)
                {
                    ProfileUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0]["PK_UserID"]);
                    ProfileUsers.FirstName = Convert.ToString(DTEditUser.Rows[0]["FirstName"]);
                    ProfileUsers.LastName = Convert.ToString(DTEditUser.Rows[0]["LastName"]);
                    ProfileUsers.EmailID = Convert.ToString(DTEditUser.Rows[0]["EmailID"]);
                    //if (ProfileUsers.DateOfJoining != null)
                    //{
                      //  ProfileUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                    //}
                    ProfileUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                    ProfileUsers.EmployeeGrade = Convert.ToString(DTEditUser.Rows[0]["EmployeeGrade"]);
                    ProfileUsers.EmployeeCode = Convert.ToString(DTEditUser.Rows[0]["EmployeeCode"]);
                    ProfileUsers.SAP_VendorCode = Convert.ToString(DTEditUser.Rows[0]["SAP_VendorCode"]);
                    ProfileUsers.Address1 = Convert.ToString(DTEditUser.Rows[0]["Address1"]);
                    ProfileUsers.Address2 = Convert.ToString(DTEditUser.Rows[0]["Address2"]);

                    ProfileUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["FK_BranchID"]);
                    ProfileUsers.UserRole = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);

                    dateofb = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                    ProfileUsers.Dateofbirth = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                    ProfileUsers.DOB = Convert.ToString(dateofb);


                    ProfileUsers.Gender = Convert.ToString(DTEditUser.Rows[0]["Gender"]);
                    ProfileUsers.MobileNo = Convert.ToString(DTEditUser.Rows[0]["MobileNo"]);
                    ProfileUsers.ResidenceNo = Convert.ToString(DTEditUser.Rows[0]["ResidenceNo"]);
                    ProfileUsers.OfficeNo = Convert.ToString(DTEditUser.Rows[0]["OfficeNo"]);
                    ProfileUsers.Designation = Convert.ToString(DTEditUser.Rows[0]["Designation"]);
                    ProfileUsers.LanguageSpoken = Convert.ToString(DTEditUser.Rows[0]["LanguageSpoken"]);
                    ProfileUsers.ZipCode = Convert.ToString(DTEditUser.Rows[0]["ZipCode"]);
                    ProfileUsers.Signature = Convert.ToString(DTEditUser.Rows[0]["Signature"]);
                    //ProfileUsers.Signature = "test-gal-1.JPG";
                    ProfileUsers.Qualification = Convert.ToString(DTEditUser.Rows[0]["Qualification"]);
                    ProfileUsers.UserName = Convert.ToString(DTEditUser.Rows[0]["UserName"]);
                    //ObjModelUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0][""]);

                    ProfileUsers.CV = Convert.ToString(DTEditUser.Rows[0]["CV"]);
                    ProfileUsers.Image = Convert.ToString(DTEditUser.Rows[0]["Image"]);
                    ProfileUsers.PanNo = Convert.ToString(DTEditUser.Rows[0]["PanNo"]);
                    ProfileUsers.AadharNo = Convert.ToString(DTEditUser.Rows[0]["AadharNo"]);
                    ProfileUsers.Allergies = Convert.ToString(DTEditUser.Rows[0]["Allergies"]);
                    ProfileUsers.Additional_Qualification = Convert.ToString(DTEditUser.Rows[0]["Additional_Qualification"]);
                    ProfileUsers.Medical_History = Convert.ToString(DTEditUser.Rows[0]["Medical_History"]);
                    ProfileUsers.ShirtSize = Convert.ToString(DTEditUser.Rows[0]["ShirtSize"]);
                    ProfileUsers.ShoesSize = Convert.ToString(DTEditUser.Rows[0]["ShoesSize"]);
                    ProfileUsers.Marital_Status = Convert.ToString(DTEditUser.Rows[0]["Marital_Status"]);
                    ProfileUsers.BloodGroup = Convert.ToString(DTEditUser.Rows[0]["BloodGroup"]);


                    ProfileUsers.Pk_CC_Id = Convert.ToInt32(DTEditUser.Rows[0]["CostCenter_Id"]);
                    //ProfileUsers.Cost_Center = Convert.ToString(DTEditUser.Rows[0]["CostCenter_Id"]);
                    ProfileUsers.TotalyearofExprience = Convert.ToString(DTEditUser.Rows[0]["TotalyearofExprience"]);
                    
                    ProfileUsers.EmergencyMobile_No = Convert.ToString(DTEditUser.Rows[0]["EmergencyMobile_No"]);
                    ProfileUsers.Fax_No = Convert.ToString(DTEditUser.Rows[0]["Fax_No"]);
                    ProfileUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["Branch"]);

                }
                List<NameCode> lstEditUserList = new List<NameCode>();
                List<NameCode> lstEditBranchList = new List<NameCode>();
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCreateUser.GetDdlLst();

                var Data1 = objDalCreateUser.GetCostCenterList();
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");


                return View(ProfileUsers);
            }
            else
            {
                DataSet DSGetList = new DataSet();
                List<NameCode> lstRoleList = new List<NameCode>();
                List<NameCode> lstBranchList = new List<NameCode>();
                DSGetList = objDalCreateUser.GetDdlLst();
                if (DSGetList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstBranchList = (from n in DSGetList.Tables[0].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetList.Tables[0].Columns["Branch_Name"].ToString()),
                                             Code = n.Field<Int32>(DSGetList.Tables[0].Columns["Br_Id"].ToString())

                                         }).ToList();
                    }
                    ViewBag.BranchName = lstBranchList;
                    if (DSGetList.Tables[1].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstRoleList = (from n in DSGetList.Tables[1].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSGetList.Tables[1].Columns["RoleName"].ToString()),
                                           Code = n.Field<Int32>(DSGetList.Tables[1].Columns["UserRoleID"].ToString())

                                       }).ToList();
                    }
                    ViewBag.UserRole = lstRoleList;
                }
            }
            var Data = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data, "Pk_CC_Id", "Cost_Center");

            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(ProfileUser URS, HttpPostedFileBase File)
        {
            string Result = string.Empty;
            try
            {
                string UserID = Convert.ToString(Session["UserIDs"]);
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCreateUser.GetDdlLst();

                var Data1 = objDalCreateUser.GetCostCenterList();
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
                if (UserID != "" && UserID != null)
                {
                    //if (URS.Image != null)
                    //{
                    //    URS.Image = CommonControl.FileUpload("Content/Uploads/Images/", URS.Image);
                    //}
                    //if (URS.Signature != null)
                    //{
                    //    URS.Signature = CommonControl.FileUpload("Content/Uploads/Images/", URS.Signature);
                    //}
                    //if (URS.CV != null)
                    //{
                    //    URS.CV = CommonControl.FileUpload("Content/Uploads/Images/", URS.CV);
                    //}
                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            URS.Image = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection);
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                URS.Image = "NoImage.gif";
                            }
                        }
                    }

                    HttpPostedFileBase Imagesection1;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner1"])))
                    {
                        Imagesection1 = Request.Files["img_Banner1"];
                        if (Imagesection1 != null && Imagesection1.FileName != "")
                        {
                            URS.CV = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection1);
                        }
                        else
                        {
                            if (Imagesection1.FileName != "")
                            {
                                URS.CV = "NoImage.gif";
                            }
                        }
                    }

                    HttpPostedFileBase Imagesection2;//Signatur
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner2"])))
                    {
                        Imagesection2 = Request.Files["img_Banner2"];
                        if (Imagesection2 != null && Imagesection2.FileName != "")
                        {
                            URS.Signature = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection2);
                        }
                        else
                        {
                            if (Imagesection2.FileName != "")
                            {
                                URS.Signature = "NoImage.gif";
                            }
                        }
                    }

                    URS.PK_UserID = UserID;
                    DataSet CostCenter = objNIA.GetServiceCode();
                    //objModel.ServiceCode = CostCenter.Tables[0].Rows[0]["CostCenter"].ToString();
                    URS.Pk_CC_Id = Convert.ToInt32(CostCenter.Tables[0].Rows[0]["CostCenter"]);
                    Result = objDalCreateUser.Updatprofile(URS);
                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateUsers"] = Result;
                        return RedirectToAction("Myprofile");
                    }

                }
                else
                {
                    if (URS.Image != null)
                    {
                        URS.Image = CommonControl.FileUpload("Content/Uploads/Images/", URS.Image);
                    }
                    if (URS.Signature != null)
                    {
                        URS.Signature = CommonControl.FileUpload("Content/Uploads/Images/", URS.Signature);
                    }
                    if (URS.CV != null)
                    {
                        URS.CV = CommonControl.FileUpload("Content/Uploads/Images/", URS.CV);
                    }

                    Result = objDalCreateUser.Updatprofile(URS);
                    if (Result != null && Result != "")
                    {

                        TempData["InsertUsers"] = Result;
                        return RedirectToAction("Myprofile");
                    }

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View();
        }
        */

        [HttpGet]
        public ActionResult UpdateProfile()
        {
            string UserID = Convert.ToString(Session["UserIDs"]);
            string dateofb = null;


            DataTable DTUserDashBoard = new DataTable();
            List<Users> lstUserDashBoard = new List<Users>();
            DTUserDashBoard = objDalCreateUser.GetUserDashBoard();

            if (DTUserDashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DTUserDashBoard.Rows)
                {
                    lstUserDashBoard.Add(
                        new Users
                        {
                            PK_UserID = Convert.ToString(dr["PK_UserID"]),
                            FullName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),
                        }
                        );
                }
            }
            ViewBag.Employees = new SelectList(lstUserDashBoard, "PK_UserID", "FullName");

            DataTable DTOrgChange = new DataTable();
            List<Users> lstOrgChangeBoard = new List<Users>();
            DTOrgChange = objDalCreateUser.GetOrgChangeData();

            if (DTOrgChange.Rows.Count > 0)
            {
                foreach (DataRow dr in DTOrgChange.Rows)
                {
                    lstOrgChangeBoard.Add(
                        new Users
                        {
                            Id = Convert.ToInt32(dr["id"]),
                            Orgdescription = Convert.ToString(dr["description"]),
                        }
                        );
                }
            }

            ViewBag.OrgChange = new SelectList(lstOrgChangeBoard, "id", "Orgdescription");

            if (UserID != "" && UserID != null)
            {
                DataTable DTEditUser = new DataTable();
                DTEditUser = objDalCreateUser.EditProfile(UserID);
                if (DTEditUser.Rows.Count > 0)
                {
                    ProfileUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0]["PK_UserID"]);
                    ProfileUsers.FirstName = Convert.ToString(DTEditUser.Rows[0]["FirstName"]);
                    ProfileUsers.LastName = Convert.ToString(DTEditUser.Rows[0]["LastName"]);
                    ProfileUsers.EmailID = Convert.ToString(DTEditUser.Rows[0]["EmailID"]);
                    //if (ProfileUsers.DateOfJoining != null)
                    //{
                    //  ProfileUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                    //}
                    ProfileUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                    ProfileUsers.EmployeeGrade = Convert.ToString(DTEditUser.Rows[0]["EmployeeGrade"]);
                    ProfileUsers.EmployeeCode = Convert.ToString(DTEditUser.Rows[0]["EmployeeCode"]);
                    ProfileUsers.SAP_VendorCode = Convert.ToString(DTEditUser.Rows[0]["SAP_VendorCode"]);
                    ProfileUsers.Address1 = Convert.ToString(DTEditUser.Rows[0]["Address1"]);
                    ProfileUsers.Address2 = Convert.ToString(DTEditUser.Rows[0]["Address2"]);

                    ProfileUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["FK_BranchID"]);
                    ProfileUsers.UserRole = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);

                    dateofb = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                    ProfileUsers.Dateofbirth = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                    ProfileUsers.DOB = Convert.ToString(dateofb);


                    ProfileUsers.Gender = Convert.ToString(DTEditUser.Rows[0]["Gender"]);
                    ProfileUsers.MobileNo = Convert.ToString(DTEditUser.Rows[0]["MobileNo"]);
                    ProfileUsers.ResidenceNo = Convert.ToString(DTEditUser.Rows[0]["ResidenceNo"]);
                    ProfileUsers.OfficeNo = Convert.ToString(DTEditUser.Rows[0]["OfficeNo"]);
                    ProfileUsers.Designation = Convert.ToString(DTEditUser.Rows[0]["Designation"]);
                    ProfileUsers.LanguageSpoken = Convert.ToString(DTEditUser.Rows[0]["LanguageSpoken"]);
                    ProfileUsers.ZipCode = Convert.ToString(DTEditUser.Rows[0]["ZipCode"]);
                    ProfileUsers.Signature = Convert.ToString(DTEditUser.Rows[0]["Signature"]);
                    //ProfileUsers.Signature = "test-gal-1.JPG";
                    ProfileUsers.Qualification = Convert.ToString(DTEditUser.Rows[0]["Qualification"]);
                    ProfileUsers.UserName = Convert.ToString(DTEditUser.Rows[0]["UserName"]);
                    //ObjModelUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0][""]);

                    ProfileUsers.CV = Convert.ToString(DTEditUser.Rows[0]["CV"]);
                    ProfileUsers.Image = Convert.ToString(DTEditUser.Rows[0]["Image"]);
                    ProfileUsers.PanNo = Convert.ToString(DTEditUser.Rows[0]["PanNo"]);
                    ProfileUsers.AadharNo = Convert.ToString(DTEditUser.Rows[0]["AadharNo"]);
                    ProfileUsers.Allergies = Convert.ToString(DTEditUser.Rows[0]["Allergies"]);
                    ProfileUsers.Additional_Qualification = Convert.ToString(DTEditUser.Rows[0]["Additional_Qualification"]);
                    ProfileUsers.Medical_History = Convert.ToString(DTEditUser.Rows[0]["Medical_History"]);
                    ProfileUsers.ShirtSize = Convert.ToString(DTEditUser.Rows[0]["ShirtSize"]);
                    ProfileUsers.ShoesSize = Convert.ToString(DTEditUser.Rows[0]["ShoesSize"]);
                    ProfileUsers.Marital_Status = Convert.ToString(DTEditUser.Rows[0]["Marital_Status"]);
                    ProfileUsers.BloodGroup = Convert.ToString(DTEditUser.Rows[0]["BloodGroup"]);


                    ProfileUsers.Pk_CC_Id = Convert.ToInt32(DTEditUser.Rows[0]["CostCenter_Id"]);
                    //ProfileUsers.Cost_Center = Convert.ToString(DTEditUser.Rows[0]["CostCenter_Id"]);
                    ProfileUsers.TotalyearofExprience = Convert.ToString(DTEditUser.Rows[0]["TotalyearofExprience"]);

                    ProfileUsers.EmergencyMobile_No = Convert.ToString(DTEditUser.Rows[0]["EmergencyMobile_No"]);
                    ProfileUsers.Fax_No = Convert.ToString(DTEditUser.Rows[0]["Fax_No"]);
                    ProfileUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["Branch"]);


                }
                List<NameCode> lstEditUserList = new List<NameCode>();
                List<NameCode> lstEditBranchList = new List<NameCode>();
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCreateUser.GetDdlLst();

                var Data1 = objDalCreateUser.GetCostCenterList();
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");


                return View(ProfileUsers);
            }
            else
            {
                DataSet DSGetList = new DataSet();
                List<NameCode> lstRoleList = new List<NameCode>();
                List<NameCode> lstBranchList = new List<NameCode>();
                DSGetList = objDalCreateUser.GetDdlLst();
                if (DSGetList.Tables[0].Rows.Count > 0)
                {
                    if (DSGetList.Tables[0].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstBranchList = (from n in DSGetList.Tables[0].AsEnumerable()
                                         select new NameCode()
                                         {
                                             Name = n.Field<string>(DSGetList.Tables[0].Columns["Branch_Name"].ToString()),
                                             Code = n.Field<Int32>(DSGetList.Tables[0].Columns["Br_Id"].ToString())

                                         }).ToList();
                    }
                    ViewBag.BranchName = lstBranchList;
                    if (DSGetList.Tables[1].Rows.Count > 0)//Dynamic Binding Analyst  Sectore Code DropDwonlist
                    {
                        lstRoleList = (from n in DSGetList.Tables[1].AsEnumerable()
                                       select new NameCode()
                                       {
                                           Name = n.Field<string>(DSGetList.Tables[1].Columns["RoleName"].ToString()),
                                           Code = n.Field<Int32>(DSGetList.Tables[1].Columns["UserRoleID"].ToString())

                                       }).ToList();
                    }
                    ViewBag.UserRole = lstRoleList;
                }
            }
            var Data = objDalCreateUser.GetCostCenterList();
            ViewBag.SubCatlist = new SelectList(Data, "Pk_CC_Id", "Cost_Center");

            return View();
        }




        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(ProfileUser URS, HttpPostedFileBase File, FormCollection fc)
        {
            string Result = string.Empty;
            try
            {
                string UserID = Convert.ToString(Session["UserIDs"]);
                DataSet DSEditGetList = new DataSet();
                DSEditGetList = objDalCreateUser.GetDdlLst();

                var Data1 = objDalCreateUser.GetCostCenterList();
                ViewBag.SubCatlist = new SelectList(Data1, "Pk_CC_Id", "Cost_Center");
                if (UserID != "" && UserID != null)
                {

                    HttpPostedFileBase Imagesection;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner"])))
                    {
                        Imagesection = Request.Files["img_Banner"];
                        if (Imagesection != null && Imagesection.FileName != "")
                        {
                            URS.Image = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection);
                        }
                        else
                        {
                            if (Imagesection.FileName != "")
                            {
                                URS.Image = "NoImage.gif";
                            }
                        }
                    }

                    HttpPostedFileBase Imagesection1;
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner1"])))
                    {
                        Imagesection1 = Request.Files["img_Banner1"];

                        if (Imagesection1 != null && Imagesection1.FileName != "")
                        {
                            /// URS.CV = CommonControl.FileUpload("Content/Uploads/Images/", Imagesection1);
                            DataSet dsResult = new DataSet();

                            string[] BranchList = URS.Branch.Split(',');
                            foreach (string brname in BranchList)
                            {
                                dsResult = OBJAppl.CheckLibraryFoler(brname);
                                string LibID = dsResult.Tables[0].Rows[0]["Lib_ID"].ToString();

                                URS.CV = CommonControl.FileUpload("Libraryfiles/", Imagesection1);
                                LibraryDocumentModel LB = new LibraryDocumentModel();
                                var InputFileName = Path.GetFileName(Imagesection1.FileName);
                                var ServerSavePath = Path.Combine(Server.MapPath("~/LibraryFiles/") + InputFileName);
                                LB.Lib_Id = Convert.ToInt32(LibID);
                                LB.PDF = InputFileName;
                                LB.UserID = URS.PK_UserID;
                                var InputFileName1 = Path.GetFileName(Imagesection1.FileName);
                                Result = OBJAppl.InsertUpdateCVData(LB);
                            }


                        }
                        else
                        {
                            if (Imagesection1.FileName != "")
                            {
                                URS.CV = "NoImage.gif";
                            }
                        }
                    }

                    HttpPostedFileBase Imagesection2;//Signatur
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.Files["img_Banner2"])))
                    {
                        Imagesection2 = Request.Files["img_Banner2"];
                        if (Imagesection2 != null && Imagesection2.FileName != "")
                        {
                            URS.Signature = CommonControl.FileUpload("Content/Sign/", Imagesection2);
                        }
                        else
                        {
                            if (Imagesection2.FileName != "")
                            {
                                URS.Signature = "NoImage.gif";
                            }
                        }
                    }

                    URS.PK_UserID = UserID;
                    DataSet CostCenter = objNIA.GetServiceCode();
                    //objModel.ServiceCode = CostCenter.Tables[0].Rows[0]["CostCenter"].ToString();
                    URS.Pk_CC_Id = Convert.ToInt32(CostCenter.Tables[0].Rows[0]["CostCenter"]);
                    Result = objDalCreateUser.Updatprofile(URS);
                    if (Result != "" && Result != null)
                    {
                        TempData["UpdateUsers"] = Result;
                        return RedirectToAction("Myprofile");
                    }

                }
                else
                {
                    if (URS.Image != null)
                    {
                        URS.Image = CommonControl.FileUpload("Content/Sign/", URS.Image);
                    }
                    if (URS.Signature != null)
                    {
                        URS.Signature = CommonControl.FileUpload("Content/Sign/", URS.Signature);
                    }
                    if (URS.CV != null)
                    {
                        URS.CV = CommonControl.FileUpload("Content/Sign/", URS.CV);
                    }

                    Result = objDalCreateUser.Updatprofile(URS);
                    if (Result != null && Result != "")
                    {

                        TempData["InsertUsers"] = Result;
                        return RedirectToAction("Myprofile");
                    }

                }

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            return View();
        }

        [HttpGet]
        public ActionResult Myprofile()
        {
            try
            {
                string UserID = Convert.ToString(Session["UserIDs"]);

                if (UserID != "" || UserID != null || UserID != "0")
                {
                    DataTable DTEditUser = new DataTable();
                    DTEditUser = objDalCreateUser.EditProfile(UserID);
                    if (DTEditUser.Rows.Count > 0)
                    {
                        ProfileUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0]["PK_UserID"]);
                        ProfileUsers.FirstName = Convert.ToString(DTEditUser.Rows[0]["FirstName"]);
                        ProfileUsers.LastName = Convert.ToString(DTEditUser.Rows[0]["LastName"]);
                        ProfileUsers.EmailID = Convert.ToString(DTEditUser.Rows[0]["EmailID"]);
                        ProfileUsers.TuvEmailId = Convert.ToString(DTEditUser.Rows[0]["Tuv_Email_Id"]);
                        //if (ProfileUsers.DateOfJoining != null)
                        //{
                        //    ProfileUsers.DateOfJoining = Convert.ToDateTime(DTEditUser.Rows[0]["DateOfJoining"]);
                        //}
                        ProfileUsers.EmployeeGrade = Convert.ToString(DTEditUser.Rows[0]["EmployeeGrade"]);
                        ProfileUsers.EmployeeCode = Convert.ToString(DTEditUser.Rows[0]["EmployeeCode"]);
                        ProfileUsers.SAP_VendorCode = Convert.ToString(DTEditUser.Rows[0]["SAP_VendorCode"]);
                        ProfileUsers.Address1 = Convert.ToString(DTEditUser.Rows[0]["Address1"]);
                        ProfileUsers.Address2 = Convert.ToString(DTEditUser.Rows[0]["Address2"]);

                        ProfileUsers.Branch = Convert.ToString(DTEditUser.Rows[0]["Branch"]);
                        ProfileUsers.FK_BranchID = Convert.ToString(DTEditUser.Rows[0]["FK_BranchID"]);
                        ProfileUsers.UserRole = Convert.ToString(DTEditUser.Rows[0]["FK_RoleID"]);
                        //if (ProfileUsers.DOB != null)
                        //{
                        //    ProfileUsers.DOB = Convert.ToDateTime(DTEditUser.Rows[0]["DOB"]);
                        //}
                        ProfileUsers.Gender = Convert.ToString(DTEditUser.Rows[0]["Gender"]);
                        ProfileUsers.MobileNo = Convert.ToString(DTEditUser.Rows[0]["MobileNo"]);
                        ProfileUsers.ResidenceNo = Convert.ToString(DTEditUser.Rows[0]["ResidenceNo"]);
                        ProfileUsers.OfficeNo = Convert.ToString(DTEditUser.Rows[0]["OfficeNo"]);
                        ProfileUsers.Designation = Convert.ToString(DTEditUser.Rows[0]["Designation"]);
                        ProfileUsers.LanguageSpoken = Convert.ToString(DTEditUser.Rows[0]["LanguageSpoken"]);
                        ProfileUsers.ZipCode = Convert.ToString(DTEditUser.Rows[0]["ZipCode"]);
                        ProfileUsers.Signature = Convert.ToString(DTEditUser.Rows[0]["Signature"]);
                        ProfileUsers.Qualification = Convert.ToString(DTEditUser.Rows[0]["Qualification"]);
                        ProfileUsers.UserName = Convert.ToString(DTEditUser.Rows[0]["UserName"]);
                        //ObjModelUsers.PK_UserID = Convert.ToString(DTEditUser.Rows[0][""]);

                        ProfileUsers.CV = Convert.ToString(DTEditUser.Rows[0]["CV"]);
                        ProfileUsers.Image = Convert.ToString(DTEditUser.Rows[0]["Image"]);
                        ProfileUsers.PanNo = Convert.ToString(DTEditUser.Rows[0]["PanNo"]);
                        ProfileUsers.AadharNo = Convert.ToString(DTEditUser.Rows[0]["AadharNo"]);
                        ProfileUsers.Allergies = Convert.ToString(DTEditUser.Rows[0]["Allergies"]);
                        ProfileUsers.Additional_Qualification = Convert.ToString(DTEditUser.Rows[0]["Additional_Qualification"]);
                        ProfileUsers.Medical_History = Convert.ToString(DTEditUser.Rows[0]["Medical_History"]);
                        ProfileUsers.ShirtSize = Convert.ToString(DTEditUser.Rows[0]["ShirtSize"]);
                        ProfileUsers.ShoesSize = Convert.ToString(DTEditUser.Rows[0]["ShoesSize"]);
                        ProfileUsers.Marital_Status = Convert.ToString(DTEditUser.Rows[0]["Marital_Status"]);
                        ProfileUsers.BloodGroup = Convert.ToString(DTEditUser.Rows[0]["BloodGroup"]);

                        ProfileUsers.CostCenter_Id = Convert.ToString(DTEditUser.Rows[0]["CostCenter_Id"]);


                        ProfileUsers.TotalyearofExprience = Convert.ToString(DTEditUser.Rows[0]["TotalyearofExprience"]);
                        ProfileUsers.DateOfJoining = Convert.ToString(DTEditUser.Rows[0]["DateOfJoining"]);
                        ProfileUsers.DOB = Convert.ToString(DTEditUser.Rows[0]["DOB"]);
                        ProfileUsers.EmergencyMobile_No = Convert.ToString(DTEditUser.Rows[0]["EmergencyMobile_No"]);
                        ProfileUsers.Fax_No = Convert.ToString(DTEditUser.Rows[0]["Fax_No"]);
                        ProfileUsers.Cost_Center = Convert.ToString(DTEditUser.Rows[0]["Cost_CenterName"]);


                    }
                    List<NameCode> lstEditUserList = new List<NameCode>();
                    List<NameCode> lstEditBranchList = new List<NameCode>();
                    DataSet DSEditGetList = new DataSet();
                    DSEditGetList = objDalCreateUser.GetDdlLst();




                    return View(ProfileUsers);
                }


                //  return View();
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        #endregion

        #region
        [HttpGet]
        public ActionResult ExportIndex(ProfileUser c)
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                Int32 row = 2;
                Int32 col = 1;

                package.Workbook.Worksheets.Add("Data");
                IGrid<ProfileUser> grid = CreateExportableGrid(c);
                ExcelWorksheet sheet = package.Workbook.Worksheets["Data"];

                foreach (IGridColumn column in grid.Columns)
                {
                    sheet.Cells[1, col].Value = column.Title;
                    sheet.Column(col++).Width = 18;

                    column.IsEncoded = false;
                }

                foreach (IGridRow<ProfileUser> gridRow in grid.Rows)
                {
                    col = 1;
                    foreach (IGridColumn column in grid.Columns)
                        sheet.Cells[row, col++].Value = column.ValueFor(gridRow);

                    row++;
                }

                return File(package.GetAsByteArray(), "application/unknown", "Export.xlsx");
            }
        }
        private IGrid<ProfileUser> CreateExportableGrid(ProfileUser c)
        {
            //IGrid<SubJobs> grid = new Grid<SubJobs>(repository.GetData());
            IGrid<ProfileUser> grid = new Grid<ProfileUser>(GetData(c));
            grid.ViewContext = new ViewContext { HttpContext = HttpContext };


            grid.Columns.Add(model => model.EmployeeCode).Titled("Employee Code");
            grid.Columns.Add(model => model.PK_UserID).Titled("PK_UserId");
            grid.Columns.Add(model => model.FirstName + model.LastName).Titled("Name");
            grid.Columns.Add(model => model.DisplayName).Titled("DisplayName");
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");
            grid.Columns.Add(model => model.SAP_VendorCode).Titled("SAP Vendor Code");
            grid.Columns.Add(model => model.SAPEmployeeCode).Titled("SAP Employee Code");
            grid.Columns.Add(model => model.DateOfJoining).Titled("Date Of Joining");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
            grid.Columns.Add(model => model.UserRole).Titled("User Role");
            grid.Columns.Add(model => model.Address2).Titled("Address 2");
            grid.Columns.Add(model => model.Address1).Titled("Address 1");
            grid.Columns.Add(model => model.Cost_Center).Titled("Cost Center");
            grid.Columns.Add(model => model.Additional_Qualification).Titled("Additional Qualification");
            grid.Columns.Add(model => model.Qualification).Titled("Qualification");
            grid.Columns.Add(model => model.MobileNo).Titled("Mobile No");
            grid.Columns.Add(model => model.EmergencyMobile_No).Titled("Emergency Mobile No");
            grid.Columns.Add(model => model.EmailID).Titled("Email-ID");
            grid.Columns.Add(model => model.TuvEmailId).Titled("TUV Email-ID");
            grid.Columns.Add(model => model.DOB).Titled("Date Of Birth");
            grid.Columns.Add(model => model.Gender).Titled("Gender");
            grid.Columns.Add(model => model.BloodGroup).Titled("Blood Group");
            grid.Columns.Add(model => model.ShoesSize).Titled("Shoes Size");
            grid.Columns.Add(model => model.ShirtSize).Titled("Shirt Size");
            grid.Columns.Add(model => model.PanNo).Titled("Pan No");
            grid.Columns.Add(model => model.TUVIStampNo).Titled("Stamp No");
            grid.Columns.Add(model => model.AadharNo).Titled("Aadhar No");
            grid.Columns.Add(model => model.CV).Titled("CV");
            grid.Columns.Add(model => model.ActiveStatus).Titled("Status");
            //Comment By shrutika Salve 11/08/2023
            grid.Columns.Add(model => model.MainBranch).Titled("Main Branch");
            grid.Columns.Add(model => model.OBSTYPE).Titled("OBS Type");
            grid.Columns.Add(model => model.ProffesionalQualificationDetails).Titled("Professional Qualification  Details");
            grid.Columns.Add(model => model.UserAttachment1).Titled("User Attachment Details");
            grid.Columns.Add(model => model.EyeTest).Titled("Eye Test Report Details");
            grid.Columns.Add(model => model.SiteAddrPin).Titled("Site Addr Pin");
            grid.Columns.Add(model => model.PerAddrPin).Titled("Per Addr Pin");
            grid.Columns.Add(model => model.EmployementCategory).Titled("Employement Category");
            grid.Columns.Add(model => model.CostCenter_Id).Titled("Cost Center No");
            grid.Columns.Add(model => model.EmployementCategoryCode).Titled("EmployementCategoryCode");

            grid.Columns.Add(model => model.ResidenceAddress).Titled("ResidenceAddress");
            grid.Columns.Add(model => model.ResiPin).Titled("Residence Pin");
            grid.Columns.Add(model => model.Image).Titled("Photo");
            grid.Columns.Add(model => model.FormFilled).Titled("FormFilled");
            grid.Pager = new GridPager<ProfileUser>(grid);
            grid.Processors.Add(grid.Pager);
            grid.Pager.RowsPerPage = ProfileUsers.LstDashboard.Count;

            foreach (IGridColumn column in grid.Columns)
            {
                column.Filter.IsEnabled = true;
                column.Sort.IsEnabled = true;
            }

            return grid;
        }

        public List<ProfileUser> GetData(ProfileUser c)
        {
            Session["GetExcelData"] = "Yes";
            DataTable DTUserDashBoard = new DataTable();
            //List<MenuRights> lstMenu = new List<MenuRights>();
            //lstMenu = objdalMenuRights.GetMenuRights();
            //Session.Add("MenuList", lstMenu);
            List<ProfileUser> lstUserDashBoard = new List<ProfileUser>();
            DTUserDashBoard = objDalCreateUser.GetUserMis();
            try
            {
                if (DTUserDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTUserDashBoard.Rows)
                    {
                        lstUserDashBoard.Add(
                            new ProfileUser
                            {
                                //PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                //UserName = Convert.ToString(dr["UserName"]),
                                //FirstName = Convert.ToString(dr["FirstName"]),
                                //LastName = Convert.ToString(dr["LastName"]),
                                //EmailID = Convert.ToString(dr["EmailID"]),
                                //MobileNo = Convert.ToString(dr["MobileNo"]),
                                //IsActive = Convert.ToBoolean(dr["IsActive"])
                                PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                FirstName = Convert.ToString(dr["FirstName"]),
                                LastName = Convert.ToString(dr["LastName"]),
                                Branch = Convert.ToString(dr["Branch"]),
                                Image = Convert.ToString(dr["Image"]),
                                EmployeeGrade = Convert.ToString(dr["EmployeeGrade"]),
                                SAP_VendorCode = Convert.ToString(dr["SAP_VendorCode"]),
                                DateOfJoining = Convert.ToString(dr["DateOfJoining"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                Address2 = Convert.ToString(dr["Address2"]),
                                Address1 = Convert.ToString(dr["Address1"]),
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),
                                Additional_Qualification = Convert.ToString(dr["Additional_Qualification"]),
                                Qualification = Convert.ToString(dr["Qualification"]),
                                MobileNo = Convert.ToString(dr["MobileNo"]),
                                EmergencyMobile_No = Convert.ToString(dr["EmergencyMobile_No"]),
                                EmailID = Convert.ToString(dr["EmailID"]),
                                TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                DOB = Convert.ToString(dr["DOB"]),
                                Gender = Convert.ToString(dr["Gender"]),
                                BloodGroup = Convert.ToString(dr["BloodGroup"]),
                                ShoesSize = Convert.ToString(dr["ShoesSize"]),
                                ShirtSize = Convert.ToString(dr["ShirtSize"]),
                                PanNo = Convert.ToString(dr["PanNo"]),
                                AadharNo = Convert.ToString(dr["AadharNo"]),
                                CV = Convert.ToString(dr["CV"]),
                                //IsActive = Convert.ToBoolean(dr["IsActive"]),
                                ActiveStatus = Convert.ToString(dr["IsActive"]),
                                SAPEmployeeCode = Convert.ToString(dr["SAPEmpCode"]),
                                //added by shrutika salve 11082023
                                MainBranch = Convert.ToString(dr["mainBranch"]),
                                OBSTYPE = Convert.ToString(dr["OBSName"]),
                                ProffesionalQualificationDetails = Convert.ToString(dr["ProffesionalDetails"]),
                                UserAttachment1 = Convert.ToString(dr["UserAttachment"]),
                                EyeTest = Convert.ToString(dr["EyeTest"]),
                                SiteAddrPin = Convert.ToString(dr["SiteAddr"]),
                                PerAddrPin = Convert.ToString(dr["PerAddr"]),
                                EmployementCategory = Convert.ToString(dr["EmployementCategory"]),
                                CostCenter_Id = Convert.ToString(dr["CostCenter_Id"]),
                                EmployementCategoryCode = Convert.ToString(dr["EmployementCategoryCode"]),
                                UserRole = Convert.ToString(dr["Role"]),
                                DisplayName = Convert.ToString(dr["DisplayName"]),
                                TUVIStampNo = Convert.ToString(dr["TUVIStampNo"]),
                                ResidenceAddress = Convert.ToString(dr["Address1"]),
                                ResiPin = Convert.ToString(dr["ZipCode"]),
                                FormFilled = Convert.ToString(dr["FormFilled"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["UserList"] = lstUserDashBoard;
            ProfileUsers.LstDashboard = lstUserDashBoard;

            return ProfileUsers.LstDashboard;
        }

        #endregion




        #region cost Sheet Filter By Auto complete
        public JsonResult GetAutoCompleteData(string InspectorName)
        {
            DataTable DTCallDashBoard = new DataTable();
            List<CostCenterModel> lstCallDashBoard = new List<CostCenterModel>();

            DTCallDashBoard = objDalCreateUser.GetCostCanterByJason();

            try
            {
                if (DTCallDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTCallDashBoard.Rows)
                    {
                        lstCallDashBoard.Add(
                            new CostCenterModel
                            {
                                Cost_Center = Convert.ToString(dr["Cost_Center"]),


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            var getList = from n in lstCallDashBoard
                          where n.Cost_Center.StartsWith(InspectorName)
                          select new { n.Cost_Center };
            return Json(getList);
        }
        #endregion

        public JsonResult TemporaryFilePathDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            var CVIPath = string.Empty;

            string[] splitedGrp;
            List<string> Selected = new List<string>();
            FormCollection fc = new FormCollection();

            //Adding New Code 13 March 2020
            string strExtension = string.Empty;


            List<FileDetails> DOCUploaded = new List<FileDetails>();
            List<FileDetails> DOCUploaded1 = new List<FileDetails>();

            if (Session["DocsUploaded"] != null)
            {
                DOCUploaded = Session["DocsUploaded"] as List<FileDetails>;
            }
            if (Session["DocsEyeTestUploaded"] != null)
            {
                DOCUploaded1 = Session["DocsEyeTestUploaded"] as List<FileDetails>;
            }
            //---Adding end Code
            try
            {


                string filePath = string.Empty;
                string CVfilePath = string.Empty;
                string NewfileName = string.Empty;

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    strExtension = Path.GetExtension(files.FileName);

                    if (files != null && files.ContentLength > 0)
                    {

                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.ToUpper().EndsWith(".XLSX") || files.FileName.ToUpper().EndsWith(".XLS") || files.FileName.ToUpper().EndsWith(".PDF") || files.FileName.ToUpper().EndsWith(".JPEG") || files.FileName.ToUpper().EndsWith(".JPG") || files.FileName.ToUpper().EndsWith(".PNG") || files.FileName.ToUpper().EndsWith(".GIF") || files.FileName.ToUpper().EndsWith(".DOC") || files.FileName.ToUpper().EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            string FilewithoutExt = Path.GetFileNameWithoutExtension(fileName);
                            NewfileName = FilewithoutExt + "_" + Session["UserID"].ToString() + Path.GetExtension(fileName);
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = Path.GetFileNameWithoutExtension(NewfileName);
                            fileDetail.Extension = Path.GetExtension(NewfileName);


                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;
                            //DOCUploaded.Add(fileDetail);

                            if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD")
                            {
                                DOCUploaded.Add(fileDetail);
                            }
                            else if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD1")
                            {
                                DOCUploaded1.Add(fileDetail);
                            }
                            //-----------------------------------------------------
                            //filePath = Path.Combine(Server.MapPath("~/Content/Uploads/Images/"), NewfileName);
                            //var K = "/Content/Uploads/Images/" + NewfileName;
                            //IPath = K;





                            //CVfilePath = Path.Combine(Server.MapPath("~/LibraryFiles/"), fileName);
                            //var KCV = "~/LibraryFiles/" + fileName;
                            //CVIPath = KCV;

                            filePath = Path.Combine(Server.MapPath("~/Content/Uploads/Images/"), NewfileName.Replace(",", ""));
                            var K = "/Content/Uploads/Images/" + NewfileName.Replace(",", "");
                            IPath = K;

                            CVfilePath = Path.Combine(Server.MapPath("~/LibraryFiles/"), fileName.Replace(",", ""));
                            var KCV = "~/LibraryFiles/" + fileName.Replace(",", "");
                            CVIPath = KCV;


                            if (files.FileName.ToUpper().EndsWith(".JPEG") || files.FileName.ToUpper().EndsWith(".JPG") || files.FileName.ToUpper().EndsWith(".GIF") || files.FileName.ToUpper().EndsWith(".PNG"))
                            {
                                // files.SaveAs(filePath);
                            }

                            if (files.FileName.ToUpper().EndsWith(".DOC") || files.FileName.ToUpper().EndsWith(".DOCX"))
                            {
                                // files.SaveAs(CVfilePath);
                            }

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list"] = Selected;
                        }
                        else
                        {
                            // ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                }

                //Session["DocsUploaded"] = DOCUploaded;
                if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD")
                {
                    Session["DocsUploaded"] = DOCUploaded;
                }
                else if (Request.Files.Keys[0].ToString().ToUpper() == "DOCUPLOAD1")
                {
                    Session["DocsEyeTestUploaded"] = DOCUploaded1;
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TemporaryFilePathEyeTestDocumentAttachment()//Photo Uploading Functionality For Adding TemporaryFilePathDocumentAttachment
        {
            var IPath = string.Empty;
            string[] splitedGrp;
            List<string> Selected = new List<string>();
            FormCollection fc = new FormCollection();

            //Adding New Code 13 March 2020
            string strExtension = string.Empty;


            List<FileDetails> DOCUploaded = new List<FileDetails>();

            if (Session["DocsEyeTestUploaded"] != null)
            {
                DOCUploaded = Session["DocsEyeTestUploaded"] as List<FileDetails>;
            }
            //---Adding end Code
            try
            {


                string filePath = string.Empty;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase files = Request.Files[i]; //Uploaded file
                    int fileSize = files.ContentLength;
                    strExtension = Path.GetExtension(files.FileName);

                    if (files != null && files.ContentLength > 0)
                    {

                        if (files.FileName.ToUpper().EndsWith(".MSG") || files.FileName.EndsWith(".xlsx") || files.FileName.EndsWith(".xls") || files.FileName.EndsWith(".pdf") || files.FileName.EndsWith(".PDF") || files.FileName.EndsWith(".JPEG") || files.FileName.EndsWith(".jpeg") || files.FileName.EndsWith(".jpg") || files.FileName.EndsWith(".JPG") || files.FileName.EndsWith(".png") || files.FileName.EndsWith(".PNG") || files.FileName.EndsWith(".gif") || files.FileName.EndsWith(".doc") || files.FileName.EndsWith(".DOC") || files.FileName.EndsWith(".docx") || files.FileName.EndsWith(".DOCX"))
                        {
                            string fileName = files.FileName;
                            //Adding New Code as per new requirement 12 March 2020, Manoj Sharma
                            FileDetails fileDetail = new FileDetails();
                            fileDetail.FileName = Path.GetFileNameWithoutExtension(fileName);
                            fileDetail.Extension = Path.GetExtension(fileName);


                            fileDetail.Id = Guid.NewGuid();

                            BinaryReader br = new BinaryReader(files.InputStream);
                            byte[] bytes = br.ReadBytes((Int32)files.ContentLength);
                            fileDetail.FileContent = bytes;
                            DOCUploaded.Add(fileDetail);


                            //-----------------------------------------------------
                            filePath = Path.Combine(Server.MapPath("~/Content/JobDocument/"), fileDetail.Id + fileDetail.Extension);
                            var K = "~/Content/JobDocument/" + fileName;
                            IPath = K;

                            // files.SaveAs(filePath);

                            var ExistingUploadFile = IPath;
                            splitedGrp = ExistingUploadFile.Split(',');
                            foreach (var single in splitedGrp)
                            {
                                Selected.Add(single);
                            }
                            Session["list1"] = Selected;
                        }
                        else
                        {
                            ViewBag.Error = "Please Select XLSX or PDF File";
                        }
                    }
                }

                Session["DocsEyeTestUploaded"] = DOCUploaded;
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            return Json(IPath, JsonRequestBehavior.AllowGet);
        }


        public void DownloadFormat(String p, String d)
        {
            /// return File(Path.Combine(Server.MapPath("~/Files/Documents/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);


            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCreateUser.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }
        public FileResult DownloadFormat1(string d)
        {

            string FileName = "";
            string Date = "";

            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCreateUser.GetFileContent(Convert.ToInt32(d));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                ////  bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }




            //string myDate = "05/11/2010";


            string path = Server.MapPath("~/Content/Sign/" + fileName);
            // string path = Server.MapPath("~/Content/") + d;

            //Read the File data into Byte Array.
            bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }


        public ActionResult Show(int id)
        {
            DataTable DTDownloadFile = new DataTable();
            List<FileDetails> lstEditFileDetails = new List<FileDetails>();
            DTDownloadFile = objDalCreateUser.GetFileContent(Convert.ToInt32(id));

            string fileName = string.Empty;
            string contentType = string.Empty;
            byte[] bytes = null;

            if (DTDownloadFile.Rows.Count > 0)
            {
                bytes = ((byte[])DTDownloadFile.Rows[0]["FileContent"]);
                fileName = DTDownloadFile.Rows[0]["FileName"].ToString();
            }

            var imageData = fileName;


            return File(imageData, "image/jpg");
        }



        [HttpPost]
        public JsonResult DeleteFileFormat(string id, string UserID)
        {
            string Results = string.Empty;
            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();
            if (String.IsNullOrEmpty(id))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { result = "Error" });
                //return Json(new { result = "ERROR", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });
            }
            try
            {
                //   Guid guid = new Guid(id);
                //DTGetDeleteFile = objDalCompany.GetFileExt(id);
                //if (DTGetDeleteFile.Rows.Count > 0)
                //{
                //    fileDetails.Extension = Convert.ToString(DTGetDeleteFile.Rows[0]["Extenstion"]);
                //}
                if (id != null && id != "")
                {
                    Results = objDalCreateUser.DeleteUploadedFile(id);
                    //var path = Path.Combine(Server.MapPath("~/Content/JobDocument/"), id + fileDetails.Extension);
                    //if (System.IO.File.Exists(path))
                    //{
                    //    System.IO.File.Delete(path);
                    //}
                    return Json(new { result = "OK", JsonRequestBehavior.AllowGet });
                    //return Json(new { result = "OK", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });
                }
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR", Message = ex.Message });
                return Json(new { result = "ERROR", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });
            }
            return Json(new { result = "SUCCESS", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });
            //return Json(new { result = "ERROR", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });
        }


        [HttpPost]
        public JsonResult InsertDocument(string DocType, string UserID, string Year)
        {
            string Results = string.Empty;
            int intUserID = 0;

            //if(Year!="")
            //{
            //    DocType = "9";
            //}
            //else
            //{
            //    DocType = DocType;
            //}

            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();

            List<FileDetails> DOCUploaded = new List<FileDetails>();


            string Result = string.Empty;
            string RetValue = string.Empty;
            bool validFile = false;

            intUserID = objDalCreateUser.GetUserID(UserID);
            try
            {
                DOCUploaded = Session["DocsUploaded"] as List<FileDetails>;

                if (DOCUploaded != null && DOCUploaded.Count > 0)
                {
                    foreach (var item in DOCUploaded)
                    {
                        if (DocType == "3")
                        {
                            if (item.Extension.ToString().ToUpper().EndsWith(".DOC") || item.Extension.ToString().ToUpper().EndsWith(".DOCX"))
                            {
                                validFile = true;
                            }
                        }
                        else if (DocType == "1" || DocType == "2")
                        {
                            if (item.Extension.ToString().ToUpper().EndsWith(".JPG") || item.Extension.ToString().ToUpper().EndsWith(".JPEG") || item.Extension.ToString().ToUpper().EndsWith(".PNG"))
                            {
                                validFile = true;
                            }
                        }
                        else if (item.Extension.ToString().ToUpper().EndsWith(".PDF") || item.Extension.ToString().ToUpper().EndsWith(".JPEG") || item.Extension.ToString().ToUpper().EndsWith(".PNG"))
                        {
                            validFile = true;
                        }

                    }

                    if (validFile)
                    {
                        RetValue = objDalCreateUser.InsertUserAttachment(DOCUploaded, Convert.ToInt32(intUserID), DocType, Year);
                        CommonControl objCommonControl = new CommonControl();
                        objCommonControl.SaveSign(DOCUploaded, Convert.ToInt32(intUserID));
                        Session["DocsUploaded"] = null;
                    }
                    else
                    {
                        Session["DocsUploaded"] = null;
                        //return Json("TYPEERROR", JsonRequestBehavior.AllowGet);
                        return Json(new { result = "TYPEERROR" });
                    }
                }

                if (String.IsNullOrEmpty(RetValue))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    //return Json("ERROR", JsonRequestBehavior.AllowGet);
                    return Json(new { result = "TYPEERROR" });
                }


                #region Bind Documents
                DataTable dtAttach = new DataTable();
                List<Users> lstDTPAN = new List<Users>();

                //  dtAttach = objDalCreateUser.GetAtt(UserID.ToString());
                dtAttach = objDalCreateUser.GetAtt(intUserID.ToString());
                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                            new Users
                            {

                                FileName = Convert.ToString(dr["FileName"]),

                                AttType = Convert.ToString(dr["AttType"]),
                            }
                            );
                    }

                    ObjModelUsers.UserAttachment = lstDTPAN;
                    ViewData["DocAttachments"] = lstDTPAN;
                }
                #endregion

                #region Get Eye Test Report
                DataTable dtAttachEye = new DataTable();
                List<Users> lstDTPANEye = new List<Users>();

                dtAttachEye = objDalCreateUser.GetEyeTest(intUserID.ToString());

                if (dtAttachEye.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttachEye.Rows)
                    {
                        lstDTPANEye.Add(
                           new Users
                           {
                               //  PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),

                               //    IDS = Convert.ToString(dr["FileID"]),
                               AttType = Convert.ToString(dr["AttType"]),
                               Year = Convert.ToString(dr["Year"]),
                           }
                         );
                    }

                    ObjModelUsers.EyeTestReport = lstDTPANEye;
                    ViewData["DocAttachmentsEyeTest"] = lstDTPANEye;
                }
                #endregion



                return Json(new { result = "SUCCESS", JsonRequestBehavior.AllowGet });
                //return Json(new { result = "SUCCESS", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });

            }
            catch (Exception ex)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult InsertDocumentEyeTest(string DocType, string UserID, string Year)
        {
            string Results = string.Empty;
            int intUserID = 0;

            //if (Year != "")
            //{
            DocType = "9";
            //}
            //else
            //{
            //    DocType = DocType;
            //}

            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();

            List<FileDetails> DOCUploaded = new List<FileDetails>();

            string Result = string.Empty;
            string RetValue = string.Empty;
            bool validFile = false;

            intUserID = objDalCreateUser.GetUserID(UserID);
            try
            {
                DOCUploaded = Session["DocsEyeTestUploaded"] as List<FileDetails>;

                if (DOCUploaded != null && DOCUploaded.Count > 0)
                {
                    foreach (var item in DOCUploaded)
                    {
                        if (DocType == "3")
                        {
                            if (item.Extension.ToString().ToUpper().EndsWith(".DOC") || item.Extension.ToString().ToUpper().EndsWith(".DOCX"))
                            {
                                validFile = true;
                            }
                        }
                        else if (DocType == "1" || DocType == "2")
                        {
                            if (item.Extension.ToString().ToUpper().EndsWith(".JPG") || item.Extension.ToString().ToUpper().EndsWith(".JPEG") || item.Extension.ToString().ToUpper().EndsWith(".PNG") || item.Extension.ToString().ToUpper().EndsWith(".GIFF"))
                            {
                                validFile = true;
                            }
                        }
                        else if (item.Extension.ToString().ToUpper().EndsWith(".PDF") || item.Extension.ToString().ToUpper().EndsWith(".JPEG") || item.Extension.ToString().ToUpper().EndsWith(".PNG") || item.Extension.ToString().ToUpper().EndsWith(".GIFF"))
                        {
                            validFile = true;
                        }

                    }

                    if (validFile)
                    {
                        RetValue = objDalCreateUser.InsertUserAttachmentEyeDoc(DOCUploaded, Convert.ToInt32(intUserID), DocType, Year);
                        CommonControl objCommonControl = new CommonControl();
                        objCommonControl.SaveSign(DOCUploaded, Convert.ToInt32(intUserID));
                        Session["DocsEyeTestUploaded"] = null;
                    }
                    else
                    {
                        Session["DocsEyeTestUploaded"] = null;
                        //return Json("TYPEERROR", JsonRequestBehavior.AllowGet);
                        return Json(new { result = "TYPEERROR" });
                    }
                }

                if (String.IsNullOrEmpty(RetValue))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    //return Json("ERROR", JsonRequestBehavior.AllowGet);
                    return Json(new { result = "TYPEERROR" });
                }


                #region Bind Documents
                DataTable dtAttach = new DataTable();
                List<Users> lstDTPAN = new List<Users>();

                //  dtAttach = objDalCreateUser.GetAtt(UserID.ToString());
                dtAttach = objDalCreateUser.GetAtt(intUserID.ToString());
                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                            new Users
                            {

                                FileName = Convert.ToString(dr["FileName"]),

                                AttType = Convert.ToString(dr["AttType"]),
                            }
                            );
                    }

                    ObjModelUsers.UserAttachment = lstDTPAN;
                    ViewData["DocAttachments"] = lstDTPAN;
                }
                #endregion

                #region Get Eye Test Report
                DataTable dtAttachEye = new DataTable();
                List<Users> lstDTPANEye = new List<Users>();

                dtAttachEye = objDalCreateUser.GetEyeTest(intUserID.ToString());

                if (dtAttachEye.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttachEye.Rows)
                    {
                        lstDTPANEye.Add(
                           new Users
                           {
                               //  PK_ID = Convert.ToInt32(dr["PK_ID"]),
                               FileName = Convert.ToString(dr["FileName"]),

                               //    IDS = Convert.ToString(dr["FileID"]),
                               AttType = Convert.ToString(dr["AttType"]),
                               //Year = Convert.ToString(dr["Year"]),
                           }
                         );
                    }

                    ObjModelUsers.EyeTestReport = lstDTPANEye;
                    ViewData["DocAttachmentsEyeTest"] = lstDTPANEye;
                }
                #endregion



                //return Json("SUCCESS", JsonRequestBehavior.AllowGet);
                return Json(new { result = "SUCCESS", JsonRequestBehavior.AllowGet });
                // return Json(new { result = "SUCCESS", url = Url.Action("CreateUser", "Users", new { @UserID = UserID }) });

            }
            catch (Exception ex)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetDegreeName(string strDegree)
        {
            string Results = string.Empty;


            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();

            List<FileDetails> DOCUploaded = new List<FileDetails>();

            string Result = string.Empty;
            string RetValue = string.Empty;



            try
            {

                DataTable dtAttach = new DataTable();
                List<Users> lstDTPAN = new List<Users>();

                dtAttach = objDalCreateUser.GetDegreeNames(strDegree);
                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAttach.Rows)
                    {
                        lstDTPAN.Add(
                            new Users
                            {
                                BranchName = Convert.ToString(dr["name"]),
                                DBrID = Convert.ToString(dr["id"]),
                            }
                        );
                    }
                }

                return Json(lstDTPAN, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult ValidateDocs(string UserID)
        {
            string Results = string.Empty;
            int intUserID = 0;

            FileDetails fileDetails = new FileDetails();
            DataTable DTGetDeleteFile = new DataTable();

            List<FileDetails> DOCUploaded = new List<FileDetails>();

            string Result = string.Empty;
            string RetValue = string.Empty;


            intUserID = objDalCreateUser.GetUserID(UserID);
            try
            {
                RetValue = objDalCreateUser.CheckDocuments(Convert.ToInt32(intUserID));

                if (RetValue == "NOEXISTS")
                {
                    return Json(new { result = "NOEXISTS" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "EXISTS" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json(new { result = "ERROR" }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetCertName(string Prefix)
        {
            DataTable DTResult = new DataTable();
            List<Users> lstAutoComplete = new List<Users>();

            if (Prefix != null && Prefix != "")
            {
                DTResult = objDalCreateUser.GetCertName(Prefix);

                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new Users
                           {
                               PKId = Convert.ToString(dr["PK_ID"]),
                               CertName = Convert.ToString(dr["CertName"])

                           }
                         );
                    }
                    Session["CertNamesNames"] = Convert.ToString(DTResult.Rows[0]["CertName"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
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


        private static string Decrypt(string cipherText)
        {
            string encryptionKey = "MAKV2SPBNI99212";
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


        public JsonResult CheckBranchDetails(string ValCostCentre)
        {
            DataTable DTResult = new DataTable();
            List<Users> lstAutoComplete = new List<Users>();

            if (ValCostCentre != null && ValCostCentre != "")
            {
                DTResult = objDalCreateUser.CheckBranchDetails(ValCostCentre);

                if (DTResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTResult.Rows)
                    {
                        lstAutoComplete.Add(
                           new Users
                           {
                               PKId = Convert.ToString(dr["br_ID"]),
                               CertName = Convert.ToString(dr["CostCentre"]),
                               BranchName = Convert.ToString(dr["Branch_Name"]),
                               Employee_Type = Convert.ToString(dr["EmpType"])

                           }
                         );
                    }
                    Session["BranchDetails"] = Convert.ToString(DTResult.Rows[0]["CostCentre"]);
                    return Json(lstAutoComplete, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertCV(string Inspection, string ExpertiseSummary, bool IsInspectionCheck, string[][] EmploymentDetails, string[][] ProjectDetails, string[][] TrainingDetails, string[][] AchievementDetails)
        {
            try
            {
                string UserID = Convert.ToString(Session["UserID"]);
                DataTable Employmentdata = ConvertToDataTable(EmploymentDetails);
                DataTable Projectdata = ConvertToDataTable(ProjectDetails);
                DataTable Trainingdata = ConvertToDataTable(TrainingDetails);
                DataTable Achievementdata = ConvertToDataTable(AchievementDetails);

                objDalCreateUser.InsertCVData(UserID, Inspection, ExpertiseSummary, IsInspectionCheck, Employmentdata, Projectdata, Trainingdata, Achievementdata);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        static DataTable ConvertToDataTable(string[][] stringArray)
        {
            DataTable dataTable = new DataTable();

            if (stringArray.Length > 0)
            {
                foreach (string header in stringArray[0])
                {
                    if (header != null && header != "Edit" && header != "Delete")
                    {
                        dataTable.Columns.Add(header.Replace(" ", ""));
                    }

                }

                for (int i = 1; i < stringArray.Length; i++)
                {
                    DataRow row = dataTable.NewRow();
                    for (int j = 0; j < stringArray[i].Length - 2; j++)
                    {
                        row[j] = stringArray[i][j];
                    }
                    if (row.ItemArray[0].ToString() != "" && row.ItemArray[0] != null)
                    {
                        dataTable.Rows.Add(row);
                    }
                }
            }

            return dataTable;
        }



        public ActionResult CVDocumentsDetails(string FolderName, Users U)
        {
            DataTable DTAppeal = new DataTable();
            List<Users> lstAppeal = new List<Users>();
            Users ProfileUsers = new Users();
            #region Library
            DataSet DSEditQutationTabledata = new DataSet();
            DSEditQutationTabledata = OBJAppl.GetUserRoll();
            if (DSEditQutationTabledata.Tables[0].Rows.Count > 0)
            {
                U.FK_RoleID = Convert.ToInt32(DSEditQutationTabledata.Tables[0].Rows[0]["FK_RoleID"]);
            }
            #endregion

            DTAppeal = objDalCreateUser.GetCVData();
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new Users
                            {

                                PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                //LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                UserName = Convert.ToString(dr["Name"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                OBSTYPE = Convert.ToString(dr["OBSName"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                Qualification = Convert.ToString(dr["Qualification"]),
                                TotalyearofExprience = Convert.ToString(dr["TotalyearofExprience"]),
                                CV = Convert.ToString(dr["PDF"]),
                                CVUpdate = Convert.ToString(dr["CVUpdate"]),
                                CoreField = Convert.ToString(dr["Corefieldname"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CVListData"] = lstAppeal;
            ProfileUsers.lstPQ = lstAppeal;
            //IVRNew.Lib_Id = LDM.Lib_Id;
            ProfileUsers.FK_RoleID = U.FK_RoleID;
            return View(ProfileUsers);

        }

        public ActionResult SearchTermInCV(string SearchTerm)
        {
            DataTable DTAppeal = new DataTable();
            List<Users> lstAppeal = new List<Users>();
            Users ProfileUsers = new Users();

            DTAppeal = objDalCreateUser.SearchTermInCV(SearchTerm);
            try
            {
                if (DTAppeal.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTAppeal.Rows)
                    {
                        lstAppeal.Add(
                            new Users
                            {

                                PK_UserID = Convert.ToString(dr["PK_UserID"]),
                                //LP_Id = Convert.ToInt32(dr["LP_Id"]),
                                UserName = Convert.ToString(dr["Name"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                OBSTYPE = Convert.ToString(dr["OBSName"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                Qualification = Convert.ToString(dr["Qualification"]),
                                TotalyearofExprience = Convert.ToString(dr["TotalyearofExperience"]),
                                CV = Convert.ToString(dr["PDF"]),
                                CVUpdate = Convert.ToString(dr["CVUpdate"]),
                                CoreField = Convert.ToString(dr["Corefieldname"]),
                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            ViewData["CVListData"] = lstAppeal;
            ProfileUsers.lstPQ = lstAppeal;

            //return View("CVDocumentsDetails", ProfileUsers);
            return Json(lstAppeal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProCertValidDate()
        {

            DataTable DashBoard = new DataTable();
            List<ProCertValidDate> lstCompanyDashBoard = new List<ProCertValidDate>();
            DashBoard = objDalCreateUser.GetDashboard();
            ProCertValidDate ObjModelsubJob = new ProCertValidDate();
            try
            {
                if (DashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DashBoard.Rows)
                    {
                        lstCompanyDashBoard.Add(
                            new ProCertValidDate
                            {
                                UserName = Convert.ToString(dr["UserName"]),
                                Branch = Convert.ToString(dr["Branch_Name"]),
                                EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                                SapVendorCode = Convert.ToString(dr["Sap_vendorCode"]),
                                MobileNumber = Convert.ToString(dr["MobileNo"]),
                                Designation = Convert.ToString(dr["Designation"]),
                                TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                                SapEmployeeCode = Convert.ToString(dr["SAPEmpCode"]),
                                EmploymentCategory = Convert.ToString(dr["Description"]),
                                Role = Convert.ToString(dr["Userrole"]),
                                Cert1 = Convert.ToString(dr["API 1169 - Pipeline Construction Inspector"]),
                                Cert2 = Convert.ToString(dr["API 1184 - Pipeline Facility Construction Inspector"]),
                                Cert3 = Convert.ToString(dr["API 510 - Pressure Vessel Inspector"]),
                                Cert4 = Convert.ToString(dr["API 570 - Piping Inspector"]),
                                Cert5 = Convert.ToString(dr["API 571 - Corrosion and Materials"]),
                                Cert6 = Convert.ToString(dr["API 577 - Welding Inspection and Metallurgy"]),
                                Cert7 = Convert.ToString(dr["API 580 - Risk Based Inspection"]),
                                Cert8 = Convert.ToString(dr["API 653 - Aboveground Storage Tank Inspector"]),
                                Cert9 = Convert.ToString(dr["API 936 - Refractory Personnel"]),
                                Cert10 = Convert.ToString(dr["API AQ1 - Auditor - Q1"]),
                                Cert11 = Convert.ToString(dr["API AQ2 - Auditor - Q2"]),
                                Cert12 = Convert.ToString(dr["API IAQ1 - Internal Auditor - Q1"]),
                                Cert13 = Convert.ToString(dr["API IAQ2 - Internal Auditor - Q2"]),
                                Cert14 = Convert.ToString(dr["API LAQ1 - Lead Auditor - Q1"]),
                                Cert15 = Convert.ToString(dr["API LAQ2 - Lead Auditor - Q2"]),
                                Cert16 = Convert.ToString(dr["API LSP - Long Seam Pipeline"]),
                                Cert17 = Convert.ToString(dr["API QUPA - Qualification of UT Examiners (Phased Array)"]),
                                Cert18 = Convert.ToString(dr["API QUSE - Qualification of UT Examiners (Sizing)"]),
                                Cert19 = Convert.ToString(dr["API QUSEPA - Qualification of UT Examiners (Crack Sizing)"]),
                                Cert20 = Convert.ToString(dr["API QUTE - Qualification of UT Examiners (Detection)"]),
                                Cert21 = Convert.ToString(dr["API QUTETM - Qualification of UT Examiners (Thickness Measurement)"]),
                                Cert22 = Convert.ToString(dr["API SIEE - Source Inspector - Electrical Equipment"]),
                                Cert23 = Convert.ToString(dr["API SIFE - Source Inspector - Fixed Equipment"]),
                                Cert24 = Convert.ToString(dr["API SIRE - Source Inspector - Rotating Equipment"]),
                                Cert25 = Convert.ToString(dr["API TES - Tank Entry Supervisor"]),
                                Cert26 = Convert.ToString(dr["AWS-CAWI"]),
                                Cert27 = Convert.ToString(dr["AWS-CWI"]),
                                Cert28 = Convert.ToString(dr["AWS-SCWI"]),
                                Cert29 = Convert.ToString(dr["CSWIP 3.0"]),
                                Cert30 = Convert.ToString(dr["CSWIP 3.1"]),
                                Cert31 = Convert.ToString(dr["CSWIP 3.2"]),
                                Cert32 = Convert.ToString(dr["CSWIP 3.2.2"]),
                                Cert33 = Convert.ToString(dr["BINDT/PCN Level II - Ultrasonic Testing (UT)"]),
                                Cert34 = Convert.ToString(dr["BINDT/PCN Level II - Magnetic Particle Testing (MT)"]),
                                Cert35 = Convert.ToString(dr["BINDT/PCN Level II - Dye Penetrant Testing (DPT)"]),
                                Cert36 = Convert.ToString(dr["BINDT/PCN Level II - Radiography Film Interpretation (RTFI)"]),
                                Cert37 = Convert.ToString(dr["IWI-Comprehensive"]),
                                Cert38 = Convert.ToString(dr["IWI-Standard"]),
                                Cert39 = Convert.ToString(dr["IWI-Basic"]),
                                Cert40 = Convert.ToString(dr["NWICS-Basic"]),
                                Cert41 = Convert.ToString(dr["NWICS – Standard"]),
                                Cert42 = Convert.ToString(dr["NWICS – Advanced"]),
                                Cert43 = Convert.ToString(dr["ASNT NDT Level III - Acoustic Emission testing (AE)"]),
                                Cert44 = Convert.ToString(dr["ASNT NDT Level III - Electromagnetic Testing (ET)"]),
                                Cert45 = Convert.ToString(dr["ASNT NDT Level III - Infrared/Thermal (IR)"]),
                                Cert46 = Convert.ToString(dr["ASNT NDT Level III - Leak Testing (LT)"]),
                                Cert47 = Convert.ToString(dr["ASNT NDT Level III - Magnetic Flux Leakage (ML)"]),
                                Cert48 = Convert.ToString(dr["ASNT NDT Level III - Magnetic Particle Testing (MT)"]),
                                Cert49 = Convert.ToString(dr["ASNT NDT Level III - Liquid Penetrant Testing (PT)"]),
                                Cert50 = Convert.ToString(dr["ASNT NDT Level III - Radiographic Testing (RT)"]),
                                Cert51 = Convert.ToString(dr["ASNT NDT Level III - Ultrasonic Testing (UT)"]),
                                Cert52 = Convert.ToString(dr["ASNT NDT Level III - Visual Testing (VT)"]),
                                Cert53 = Convert.ToString(dr["ASNT NDT Level III - Phased Array Ultrasonic Testing (PAUT)"]),
                                Cert54 = Convert.ToString(dr["ASNT NDT Level III - Time of Flight Diffraction Testing (ToFD)"]),
                                Cert55 = Convert.ToString(dr["ASNT NDT Level III - Eddy Current Testing (ECT)"]),
                                Cert56 = Convert.ToString(dr["ASNT NDT Level II - Acoustic Emission testing (AE)"]),
                                Cert57 = Convert.ToString(dr["ASNT NDT Level II - Electromagnetic Testing (ET)"]),
                                Cert58 = Convert.ToString(dr["ASNT NDT Level II - Infrared/Thermal (IR)"]),
                                Cert59 = Convert.ToString(dr["ASNT NDT Level II - Leak Testing (LT)"]),
                                Cert60 = Convert.ToString(dr["ASNT NDT Level II - Magnetic Flux Leakage (ML)"]),
                                Cert61 = Convert.ToString(dr["ASNT NDT Level II - Magnetic Particle Testing (MT)"]),
                                Cert62 = Convert.ToString(dr["ASNT NDT Level II - Liquid Penetrant Testing (PT)"]),
                                Cert63 = Convert.ToString(dr["ASNT NDT Level II - Radiographic Testing (RT)"]),
                                Cert64 = Convert.ToString(dr["ASNT NDT Level II - Ultrasonic Testing (UT)"]),
                                Cert65 = Convert.ToString(dr["ASNT NDT Level II - Visual Testing (VT)"]),
                                Cert66 = Convert.ToString(dr["ASNT NDT Level II - Phased Array Ultrasonic Testing (PAUT)"]),
                                Cert67 = Convert.ToString(dr["ASNT NDT Level II - Time of Flight Diffraction Testing (ToFD)"]),
                                Cert68 = Convert.ToString(dr["ASNT NDT Level II - Eddy Current Testing (ECT)"]),
                                Cert69 = Convert.ToString(dr["NACE CIP Level 1"]),
                                Cert70 = Convert.ToString(dr["NACE CIP Level 2"]),
                                Cert71 = Convert.ToString(dr["NACE CIP Level 3"]),
                                Cert72 = Convert.ToString(dr["NACE Cathodic Protection Technician Level 2"]),
                                Cert73 = Convert.ToString(dr["SSPC PCI Level 1"]),
                                Cert74 = Convert.ToString(dr["SSPC PCI Level 2"]),
                                Cert75 = Convert.ToString(dr["SSPC PCI Level 3"]),
                                Cert76 = Convert.ToString(dr["FROSIO Inspector Level I   (white)"]),
                                Cert77 = Convert.ToString(dr["FROSIO Inspector Level II  (green)"]),
                                Cert78 = Convert.ToString(dr["FROSIO Inspector Level III (red)"]),
                                Cert79 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 1"]),
                                Cert80 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 2"]),
                                Cert81 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 3"]),
                                Cert82 = Convert.ToString(dr["ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2"]),
                                Cert83 = Convert.ToString(dr["ICorr/PFPNet Fire Protection Coatings Inspector Level 3"]),
                                Cert84 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 3"]),
                                Cert85 = Convert.ToString(dr["ICorr Hot Dip Galvanizing Inspector Course"]),
                                Cert86 = Convert.ToString(dr["ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2"]),
                                Cert87 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 1"]),
                                Cert88 = Convert.ToString(dr["ICorr Insulation Inspector – Level 2"]),
                                Cert89 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 2"]),
                                Cert90 = Convert.ToString(dr["ICorr Pipeline Coatings Inspector – Level 2"]),
                                Cert91 = Convert.ToString(dr["ISO 9001:2015 Lead Auditor"]),
                                Cert92 = Convert.ToString(dr["ISO 9001:2015 Internal Auditor"]),
                                Cert93 = Convert.ToString(dr["ISO 14001:2015 Lead Auditor"]),
                                Cert94 = Convert.ToString(dr["ISO 14001:2015 Internal Auditor"]),
                                Cert95 = Convert.ToString(dr["ISO 45001:2018 Lead Auditor"]),
                                Cert96 = Convert.ToString(dr["ISO 45001:2018  Internal Auditor"]),
                                Cert97 = Convert.ToString(dr["Integrated Management System Lead Auditor"]),
                                Cert98 = Convert.ToString(dr["Integrated Management System Internal Auditor"]),
                                Cert99 = Convert.ToString(dr["AS 9100:2016 Rev. D Lead  Auditor"]),
                                Cert100 = Convert.ToString(dr["AS 9100:2016 Rev. D Internal Auditor"]),
                                Cert101 = Convert.ToString(dr["SA 8000:2014  Lead Auditor"]),
                                Cert102 = Convert.ToString(dr["SA 8000:2014  Internal Auditor"]),
                                Cert103 = Convert.ToString(dr["IATF 16949 : 2016 Lead Auditor"]),
                                Cert104 = Convert.ToString(dr["IATF 16949 : 2016 Internal Auditor"]),
                                Cert105 = Convert.ToString(dr["ISO/IEC 17025:2017 Lead Auditor"]),
                                Cert106 = Convert.ToString(dr["ISO/IEC 17025:2017 Internal Auditor"]),
                                Cert107 = Convert.ToString(dr["Lean Six Sigma Green Belt"]),
                                Cert108 = Convert.ToString(dr["FSSC Ver. 5.1 Lead Auditor"]),
                                Cert109 = Convert.ToString(dr["FSSC Ver. 5.1 Internal Auditor"]),
                                Cert110 = Convert.ToString(dr[" ISO 22000:2018 Lead Auditor"]),
                                Cert111 = Convert.ToString(dr["ISO 22000:2018 Internal Auditor"]),
                                Cert112 = Convert.ToString(dr["ISO 27001:2013 Lead Auditor"]),
                                Cert113 = Convert.ToString(dr["ISO 27001:2013 Internal Auditor"]),
                                Cert114 = Convert.ToString(dr["Chartered Engineer"]),
                                Cert115 = Convert.ToString(dr["Chartered Accountant"]),
                                Cert116 = Convert.ToString(dr["ICWA"]),
                                Cert117 = Convert.ToString(dr["Charted Financial Analyst"]),
                                Cert118 = Convert.ToString(dr["IBR Competent Inspector"]),
                                Cert119 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER SMPV (U) RULES"]),
                                Cert120 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER PETROLEUM RULES"]),
                                Cert121 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER EXPLOSIVE RULES"]),
                                Cert122 = Convert.ToString(dr["CPR qualified Auditor"]),
                                Cert123 = Convert.ToString(dr["ISO 3834 Qualified Auditor"]),
                                Cert124 = Convert.ToString(dr["AD 2000 HP0 Qualified Auditor"]),
                                Cert125 = Convert.ToString(dr["EN 15085 Qualified Auditor"]),
                                Cert126 = Convert.ToString(dr["EN 1090 Qualified Auditor"]),
                                Cert127 = Convert.ToString(dr["VdTUV-Merkblatt 1153 Qualified Auditor"]),
                                Cert128 = Convert.ToString(dr["PESR Qualified Auditor"]),
                                Cert129 = Convert.ToString(dr["PESR Qualified Inspector"]),
                                Cert130 = Convert.ToString(dr["DIN 2303 Qualified Auditor"]),
                                Cert131 = Convert.ToString(dr["NORSOK Qualified Auditor"]),
                                Cert132 = Convert.ToString(dr["TPED Qualified Auditor"]),
                                Cert133 = Convert.ToString(dr["TPED Qualified Inspector"]),
                                Cert134 = Convert.ToString(dr["PED PDGR Authorisation for 1100"]),
                                Cert135 = Convert.ToString(dr["PED PDGR Authorisation for 1110"]),
                                Cert136 = Convert.ToString(dr["PED PDGR Authorisation for 1120"]),
                                Cert137 = Convert.ToString(dr["PED PDGR Authorisation for 1130"]),
                                Cert138 = Convert.ToString(dr["PED PDGR Authorisation for 1140"]),
                                Cert139 = Convert.ToString(dr["PED PDGR Authorisation for 1150"]),
                                Cert140 = Convert.ToString(dr["PED PDGR Authorisation for 1160"]),
                                Cert141 = Convert.ToString(dr["PED PDGR Authorisation for 1180"]),
                                Cert142 = Convert.ToString(dr["PED PDGR Authorisation for 1500"]),
                                Cert143 = Convert.ToString(dr["PED PDGR Authorisation for 1510"]),
                                Cert144 = Convert.ToString(dr["PED PDGR Authorisation for 1520"]),
                                Cert145 = Convert.ToString(dr["PED PDGR Authorisation for 1530"]),
                                Cert146 = Convert.ToString(dr["PED PDGR Authorisation for 1540"]),
                                Cert147 = Convert.ToString(dr["PED PDGR Authorisation for 1550"]),
                                Cert148 = Convert.ToString(dr["PED PDGR Authorisation for 1560"]),
                                Cert149 = Convert.ToString(dr["PED PDGR Authorisation for 3000"]),
                                Cert150 = Convert.ToString(dr["PED PDGR Authorisation for 3100"]),
                                Cert151 = Convert.ToString(dr["PED PDGR Authorisation for 4000"]),
                                Cert152 = Convert.ToString(dr["PED PDGR Authorisation for 4100"]),
                                Cert153 = Convert.ToString(dr["PED PDGR Authorisation for 4200"]),
                                Cert154 = Convert.ToString(dr["PED PDGR Authorisation for 4210"]),
                                Cert155 = Convert.ToString(dr["PED PDGR Authorisation for 4220"]),
                                Cert156 = Convert.ToString(dr["PED PDGR Authorisation for 4230"]),
                                Cert157 = Convert.ToString(dr["PED PDGR Authorisation for 4400"]),
                                Cert158 = Convert.ToString(dr["PED PDGR Authorisation for 5000"]),
                                Cert159 = Convert.ToString(dr["PED PDGR Authorisation for 6000"]),
                                Cert160 = Convert.ToString(dr["PED PDGR Authorisation for 6100"]),
                                Cert161 = Convert.ToString(dr["International Welding Engineer (IWE - IIW)"]),
                                Cert162 = Convert.ToString(dr["International Welding Technologist (IWT - IIW)"]),
                                Cert163 = Convert.ToString(dr["International Welding Specialist (IWS - IIW)"]),
                                Cert164 = Convert.ToString(dr["NBBI AI- Authorized Inspector"]),
                                Cert165 = Convert.ToString(dr["NBBI R - Repair Inspector (NBIC)"]),
                                Cert166 = Convert.ToString(dr["NBBI B - Inspector Supervisor"]),
                                Cert167 = Convert.ToString(dr["NBBI N - Nuclear Inspector "]),
                                Cert168 = Convert.ToString(dr["NBBII - Nuclear Inspector "]),
                                Cert169 = Convert.ToString(dr["NBBI NS - Nuclear Inspector Supervisor "]),
                                Cert170 = Convert.ToString(dr["NBBI C - Nuclear Inspector (Concrete) "]),
                                Cert171 = Convert.ToString(dr["NBBI NSC - Nuclear Inspector Supervisor (Concrete)"]),
                                Cert172 = Convert.ToString(dr["NBBI NSI - Nuclear In-service Inspector Supervisor"]),
                                Cert173 = Convert.ToString(dr["ARAMCO approved Inspector"])


                            }
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }

            ObjModelsubJob.lst2 = lstCompanyDashBoard;


            return View(ObjModelsubJob);
        }

        public ActionResult ExportIndex2()
        {
            // Using EPPlus from nuget
            using (ExcelPackage package = new ExcelPackage())
            {
                int row = 2;
                int col = 1;

                var worksheet = package.Workbook.Worksheets.Add("Data");

                // Style for header row
                using (ExcelRange range = worksheet.Cells["A1:GA1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Assuming you have a method to get data
                var data = GetProCertValidDateData();

                var headers = new Dictionary<string, string>()
{
    { "Cert1", "API 1169 - Pipeline Construction Inspector" },
    { "Cert2", "API 1184 - Pipeline Facility Construction Inspector" },
    { "Cert3", "API 510 - Pressure Vessel Inspector" },
    { "Cert4", "API 570 - Piping Inspector" },
    // Add more headers as needed
    { "Cert5", "API 571 - Corrosion and Materials"},
    { "Cert6", "API 577 - Welding Inspection and Metallurgy" },
    { "Cert7","API 580 - Risk Based Inspection"},
    { "Cert8","API 653 - Aboveground Storage Tank Inspector"},
    { "Cert9","API 936 - Refractory Personnel"},
    { "Cert10","API AQ1 - Auditor - Q1"},
    { "Cert11","API AQ2 - Auditor - Q2"},
    { "Cert12","API IAQ1 - Internal Auditor - Q1"},
    { "Cert13","API IAQ2 - Internal Auditor - Q2"},
    // Continues in the same format

    {"Cert14", "API LAQ1 - Lead Auditor - Q1" },
    {"Cert15", "API LAQ2 - Lead Auditor - Q2" },
    {"Cert16", "API LSP - Long Seam Pipeline" },
    {"Cert17", "API QUPA - Qualification of UT Examiners (Phased Array)" },
    {"Cert18", "API QUSE - Qualification of UT Examiners (Sizing)" },
    {"Cert19", "API QUSEPA - Qualification of UT Examiners (Crack Sizing)"},
    {"Cert20", "API QUTE - Qualification of UT Examiners (Detection)" },
    {"Cert21", "API QUTETM - Qualification of UT Examiners (Thickness Measurement)" },
    {"Cert22", "API SIEE - Source Inspector - Electrical Equipment" },
    {"Cert23", "API SIFE - Source Inspector - Fixed Equipment" },
    {"Cert24", "API SIRE - Source Inspector - Rotating Equipment" },
    {"Cert25", "API TES - Tank Entry Supervisor" },
    {"Cert26", "AWS-CAWI" },
    {"Cert27", "AWS-CWI" },
    {"Cert28", "AWS-SCWI" },
    {"Cert29", "CSWIP 3.0"},
    {"Cert30", "CSWIP 3.1"},
    {"Cert31", "CSWIP 3.2" },
    {"Cert32", "CSWIP 3.2.2" },
    {"Cert33", "BINDT/PCN Level II - Ultrasonic Testing (UT)" },
    {"Cert34", "BINDT/PCN Level II - Magnetic Particle Testing (MT)" },
    {"Cert35", "BINDT/PCN Level II - Dye Penetrant Testing (DPT)" },
    {"Cert36", "BINDT/PCN Level II - Radiography Film Interpretation (RTFI)" },
    {"Cert37", "IWI-Comprehensive" },
    {"Cert38", "IWI-Standard" },
    {"Cert39", "IWI-Basic" },
    {"Cert40", "NWICS-Basic" },
    {"Cert41", "NWICS – Standard" },
    {"Cert42", "NWICS – Advanced"},
    {"Cert43", "ASNT NDT Level III - Acoustic Emission testing (AE)"},
    {"Cert44", "ASNT NDT Level III - Electromagnetic Testing (ET)"},
    {"Cert45", "ASNT NDT Level III - Infrared/Thermal (IR)"},
    {"Cert46", "ASNT NDT Level III - Leak Testing (LT)"},
    {"Cert47", "ASNT NDT Level III - Magnetic Flux Leakage (ML)"},
    {"Cert48", "ASNT NDT Level III - Magnetic Particle Testing (MT)"},
    {"Cert49", "ASNT NDT Level III - Liquid Penetrant Testing (PT)"},
    {"Cert50", "ASNT NDT Level III - Radiographic Testing (RT)"},
    {"Cert51", "ASNT NDT Level III - Ultrasonic Testing (UT)"},
    {"Cert52", "ASNT NDT Level III - Visual Testing (VT)"},
    {"Cert53", "ASNT NDT Level III - Phased Array Ultrasonic Testing (PAUT)" },
    {"Cert54", "ASNT NDT Level III - Time of Flight Diffraction Testing (ToFD)" },
    {"Cert55", "ASNT NDT Level III - Eddy Current Testing (ECT)"},
    {"Cert56", "ASNT NDT Level II - Acoustic Emission testing (AE)"},
    {"Cert57", "ASNT NDT Level II - Electromagnetic Testing (ET)"},
    {"Cert58", "ASNT NDT Level II - Infrared/Thermal (IR)"},
    {"Cert59", "ASNT NDT Level II - Leak Testing (LT)"},
    {"Cert60", "ASNT NDT Level II - Magnetic Flux Leakage (ML)"},
    {"Cert61", "ASNT NDT Level II - Magnetic Particle Testing (MT)"},
    {"Cert62", "ASNT NDT Level II - Liquid Penetrant Testing (PT)"},
    {"Cert63", "ASNT NDT Level II - Radiographic Testing (RT)"},
    {"Cert64", "ASNT NDT Level II - Ultrasonic Testing (UT)"},
    {"Cert65", "ASNT NDT Level II - Visual Testing (VT)"},
    {"Cert66", "ASNT NDT Level II - Phased Array Ultrasonic Testing (PAUT)" },
    {"Cert67", "ASNT NDT Level II - Time of Flight Diffraction Testing (ToFD)" },
    {"Cert68", "ASNT NDT Level II - Eddy Current Testing (ECT)"},
    {"Cert69", "NACE CIP Level 1"},
    {"Cert70", "NACE CIP Level 2"},
    {"Cert71", "NACE CIP Level 3"},
    {"Cert72", "NACE Cathodic Protection Technician Level 2"},
    {"Cert73", "SSPC PCI Level 1"},
    {"Cert74", "SSPC PCI Level 2"},
    {"Cert75", "SSPC PCI Level 3"},
    {"Cert76", "FROSIO Inspector Level I   (white)"},
    {"Cert77", "FROSIO Inspector Level II  (green)"},
    {"Cert78", "FROSIO Inspector Level III (red)"},
    {"Cert79", "BGAS-CSWIP Painting Inspector - Grade 1"},
    {"Cert80", "BGAS-CSWIP Painting Inspector - Grade 2"},
    {"Cert81", "BGAS-CSWIP Painting Inspector - Grade 3"},
    {"Cert82", "ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2" },
    {"Cert83", "ICorr/PFPNet Fire Protection Coatings Inspector Level 3"},
    {"Cert84", "ICorr Protective Coatings Inspector – Level 3"},
    {"Cert85", "ICorr Hot Dip Galvanizing Inspector Course"},
    {"Cert86", "ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2" },
    {"Cert87", "ICorr Protective Coatings Inspector – Level 1"},
    {"Cert88", "ICorr Insulation Inspector – Level 2"},
    {"Cert89", "ICorr Protective Coatings Inspector – Level 2"},
    {"Cert90", "ICorr Pipeline Coatings Inspector – Level 2"},
    {"Cert91", "ISO 9001:2015 Lead Auditor"},
    {"Cert92", "ISO 9001:2015 Internal Auditor"},
    {"Cert93", "ISO 14001:2015 Lead Auditor"},
    {"Cert94", "ISO 14001:2015 Internal Auditor"},
    {"Cert95", "ISO 45001:2018 Lead Auditor"},
    {"Cert96", "ISO 45001:2018  Internal Auditor"},
    {"Cert97", "Integrated Management System Lead Auditor"},
    {"Cert98", "Integrated Management System Internal Auditor"},
    {"Cert99", "AS 9100:2016 Rev. D Lead  Auditor"},
    {"Cert100", "AS 9100:2016 Rev. D Internal Auditor"},
    {"Cert101", "SA 8000:2014  Lead Auditor"},
    {"Cert102", "SA 8000:2014  Internal Auditor"},
    {"Cert103", "IATF 16949 : 2016 Lead Auditor"},
    {"Cert104", "IATF 16949 : 2016 Internal Auditor"},
    {"Cert105", "ISO/IEC 17025:2017 Lead Auditor"},
    {"Cert106", "ISO/IEC 17025:2017 Internal Auditor"},
    {"Cert107", "Lean Six Sigma Green Belt"},
    {"Cert108", "FSSC Ver. 5.1 Lead Auditor"},
    {"Cert109", "FSSC Ver. 5.1 Internal Auditor"},
    {"Cert110", " ISO 22000:2018 Lead Auditor"},
    {"Cert111", "ISO 22000:2018 Internal Auditor"},
    {"Cert112", "ISO 27001:2013 Lead Auditor"},
    {"Cert113", "ISO 27001:2013 Internal Auditor"},
    {"Cert114", "Chartered Engineer"},
    {"Cert115", "Chartered Accountant"},
    {"Cert116", "ICWA"},
    {"Cert117", "Charted Financial Analyst"},
    {"Cert118", "IBR Competent Inspector"},
    {"Cert119", "PESO COMPETENT PERSON UNDER SMPV (U) RULES"},
    {"Cert120", "PESO COMPETENT PERSON UNDER PETROLEUM RULES"},
    {"Cert121", "PESO COMPETENT PERSON UNDER EXPLOSIVE RULES"},
    {"Cert122", "CPR qualified Auditor"},
    {"Cert123", "ISO 3834 Qualified Auditor"},
    {"Cert124", "AD 2000 HP0 Qualified Auditor"},
    {"Cert125", "EN 15085 Qualified Auditor"},
    {"Cert126", "EN 1090 Qualified Auditor"},
    {"Cert127", "VdTUV-Merkblatt 1153 Qualified Auditor"},
    {"Cert128", "PESR Qualified Auditor"},
    {"Cert129", "PESR Qualified Inspector"},
    {"Cert130", "DIN 2303 Qualified Auditor"},
    {"Cert131", "NORSOK Qualified Auditor"},
    {"Cert132", "TPED Qualified Auditor"},
    {"Cert133", "TPED Qualified Inspector"},
    {"Cert134", "PED PDGR Authorisation for 1100"},
    {"Cert135", "PED PDGR Authorisation for 1110"},
    {"Cert136", "PED PDGR Authorisation for 1120"},
    {"Cert137", "PED PDGR Authorisation for 1130"},
    {"Cert138", "PED PDGR Authorisation for 1140"},
    {"Cert139", "PED PDGR Authorisation for 1150"},
    {"Cert140", "PED PDGR Authorisation for 1160"},
    {"Cert141", "PED PDGR Authorisation for 1180"},
    {"Cert142", "PED PDGR Authorisation for 1500"},
    {"Cert143", "PED PDGR Authorisation for 1510"},
    {"Cert144", "PED PDGR Authorisation for 1520"},
    {"Cert145", "PED PDGR Authorisation for 1530"},
    {"Cert146", "PED PDGR Authorisation for 1540"},
    {"Cert147", "PED PDGR Authorisation for 1550"},
    {"Cert148", "PED PDGR Authorisation for 1560"},
    {"Cert149", "PED PDGR Authorisation for 3000"},
    {"Cert150", "PED PDGR Authorisation for 3100"},
    {"Cert151", "PED PDGR Authorisation for 4000"},
    {"Cert152", "PED PDGR Authorisation for 4100"},
    {"Cert153", "PED PDGR Authorisation for 4200"},
    {"Cert154", "PED PDGR Authorisation for 4210"},
    {"Cert155", "PED PDGR Authorisation for 4220"},
    {"Cert156", "PED PDGR Authorisation for 4230"},
    {"Cert157", "PED PDGR Authorisation for 4400"},
    {"Cert158", "PED PDGR Authorisation for 5000"},
    {"Cert159", "PED PDGR Authorisation for 6000"},
    {"Cert160", "PED PDGR Authorisation for 6100"},
    {"Cert161", "International Welding Engineer (IWE - IIW)"},
    {"Cert162", "International Welding Technologist (IWT - IIW)"},
    {"Cert163", "International Welding Specialist (IWS - IIW)"},
    {"Cert164", "NBBI AI- Authorized Inspector"},
    {"Cert165", "NBBI R - Repair Inspector (NBIC)"},
    {"Cert166", "NBBI B - Inspector Supervisor"},
    {"Cert167", "NBBI N - Nuclear Inspector "},
    {"Cert168", "NBBII - Nuclear Inspector "},
    {"Cert169", "NBBI NS - Nuclear Inspector Supervisor "},
    {"Cert170", "NBBI C - Nuclear Inspector (Concrete) "},
    {"Cert171", "NBBI NSC - Nuclear Inspector Supervisor (Concrete)"},
    {"Cert172", "NBBI NSI - Nuclear In-service Inspector Supervisor"},
    {"Cert173", "ARAMCO approved Inspector"},

};

                var properties = typeof(ProCertValidDate).GetProperties();

                // Set headers dynamically

                foreach (var prop in properties)
                {
                    if (headers.ContainsKey(prop.Name))
                    {
                        worksheet.Cells[1, col].Value = headers[prop.Name];
                    }
                    else
                    {
                        worksheet.Cells[1, col].Value = prop.Name; // Default to property name if no custom header
                    }
                    worksheet.Column(col).Width = 18; // Set column width
                    col++;
                }

                // Set data

                foreach (var item in data)
                {
                    col = 1;
                    foreach (var prop in properties)
                    {
                        var cell = worksheet.Cells[row, col];
                        var propValue = prop.GetValue(item);

                        // Handle date formatting and coloring for certification dates
                        if (prop.Name.StartsWith("Cert"))
                        {
                            DateTime certDate;
                            if (DateTime.TryParseExact(propValue?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out certDate))
                            {
                                // Set date value
                                cell.Value = certDate;

                                DateTime currentDate = DateTime.Today;
                                DateTime oneMonthLaterDate = currentDate.AddMonths(1);

                                // Calculate difference in days
                                int diffDays = (certDate - currentDate).Days;

                                // Apply formatting based on the difference in days
                                if (diffDays <= 30 && diffDays >= 0)
                                {
                                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(Color.Orange);
                                }
                                else if (certDate < currentDate)
                                {
                                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(Color.Red);
                                }
                                // No formatting for other dates

                                // Set date format for the cell
                                cell.Style.Numberformat.Format = "dd/MM/yyyy";
                            }
                            else
                            {
                                // Set non-date values directly if the value is not a valid date
                                cell.Value = propValue;
                            }
                        }
                        else
                        {
                            // Set non-date values directly
                            cell.Value = propValue;
                        }

                        col++;
                    }
                    row++;
                }

                // Return the Excel file as a byte array
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProCertValidDate" + DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
        }


        private List<ProCertValidDate> GetProCertValidDateData()
        {
            DataTable DashBoard = objDalCreateUser.GetDashboard();
            List<ProCertValidDate> lstCompanyDashBoard = new List<ProCertValidDate>();

            if (DashBoard.Rows.Count > 0)
            {
                foreach (DataRow dr in DashBoard.Rows)
                {
                    lstCompanyDashBoard.Add(new ProCertValidDate
                    {
                        UserName = Convert.ToString(dr["UserName"]),
                        Branch = Convert.ToString(dr["Branch_Name"]),
                        EmployeeCode = Convert.ToString(dr["EmployeeCode"]),
                        SapVendorCode = Convert.ToString(dr["Sap_vendorCode"]),
                        MobileNumber = Convert.ToString(dr["MobileNo"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        TuvEmailId = Convert.ToString(dr["Tuv_Email_Id"]),
                        SapEmployeeCode = Convert.ToString(dr["SAPEmpCode"]),
                        EmploymentCategory = Convert.ToString(dr["Description"]),
                        Role = Convert.ToString(dr["Userrole"]),
                        Cert1 = Convert.ToString(dr["API 1169 - Pipeline Construction Inspector"]),
                        Cert2 = Convert.ToString(dr["API 1184 - Pipeline Facility Construction Inspector"]),
                        Cert3 = Convert.ToString(dr["API 510 - Pressure Vessel Inspector"]),
                        Cert4 = Convert.ToString(dr["API 570 - Piping Inspector"]),
                        Cert5 = Convert.ToString(dr["API 571 - Corrosion and Materials"]),
                        Cert6 = Convert.ToString(dr["API 577 - Welding Inspection and Metallurgy"]),
                        Cert7 = Convert.ToString(dr["API 580 - Risk Based Inspection"]),
                        Cert8 = Convert.ToString(dr["API 653 - Aboveground Storage Tank Inspector"]),
                        Cert9 = Convert.ToString(dr["API 936 - Refractory Personnel"]),
                        Cert10 = Convert.ToString(dr["API AQ1 - Auditor - Q1"]),
                        Cert11 = Convert.ToString(dr["API AQ2 - Auditor - Q2"]),
                        Cert12 = Convert.ToString(dr["API IAQ1 - Internal Auditor - Q1"]),
                        Cert13 = Convert.ToString(dr["API IAQ2 - Internal Auditor - Q2"]),
                        Cert14 = Convert.ToString(dr["API LAQ1 - Lead Auditor - Q1"]),
                        Cert15 = Convert.ToString(dr["API LAQ2 - Lead Auditor - Q2"]),
                        Cert16 = Convert.ToString(dr["API LSP - Long Seam Pipeline"]),
                        Cert17 = Convert.ToString(dr["API QUPA - Qualification of UT Examiners (Phased Array)"]),
                        Cert18 = Convert.ToString(dr["API QUSE - Qualification of UT Examiners (Sizing)"]),
                        Cert19 = Convert.ToString(dr["API QUSEPA - Qualification of UT Examiners (Crack Sizing)"]),
                        Cert20 = Convert.ToString(dr["API QUTE - Qualification of UT Examiners (Detection)"]),
                        Cert21 = Convert.ToString(dr["API QUTETM - Qualification of UT Examiners (Thickness Measurement)"]),
                        Cert22 = Convert.ToString(dr["API SIEE - Source Inspector - Electrical Equipment"]),
                        Cert23 = Convert.ToString(dr["API SIFE - Source Inspector - Fixed Equipment"]),
                        Cert24 = Convert.ToString(dr["API SIRE - Source Inspector - Rotating Equipment"]),
                        Cert25 = Convert.ToString(dr["API TES - Tank Entry Supervisor"]),
                        Cert26 = Convert.ToString(dr["AWS-CAWI"]),
                        Cert27 = Convert.ToString(dr["AWS-CWI"]),
                        Cert28 = Convert.ToString(dr["AWS-SCWI"]),
                        Cert29 = Convert.ToString(dr["CSWIP 3.0"]),
                        Cert30 = Convert.ToString(dr["CSWIP 3.1"]),
                        Cert31 = Convert.ToString(dr["CSWIP 3.2"]),
                        Cert32 = Convert.ToString(dr["CSWIP 3.2.2"]),
                        Cert33 = Convert.ToString(dr["BINDT/PCN Level II - Ultrasonic Testing (UT)"]),
                        Cert34 = Convert.ToString(dr["BINDT/PCN Level II - Magnetic Particle Testing (MT)"]),
                        Cert35 = Convert.ToString(dr["BINDT/PCN Level II - Dye Penetrant Testing (DPT)"]),
                        Cert36 = Convert.ToString(dr["BINDT/PCN Level II - Radiography Film Interpretation (RTFI)"]),
                        Cert37 = Convert.ToString(dr["IWI-Comprehensive"]),
                        Cert38 = Convert.ToString(dr["IWI-Standard"]),
                        Cert39 = Convert.ToString(dr["IWI-Basic"]),
                        Cert40 = Convert.ToString(dr["NWICS-Basic"]),
                        Cert41 = Convert.ToString(dr["NWICS – Standard"]),
                        Cert42 = Convert.ToString(dr["NWICS – Advanced"]),
                        Cert43 = Convert.ToString(dr["ASNT NDT Level III - Acoustic Emission testing (AE)"]),
                        Cert44 = Convert.ToString(dr["ASNT NDT Level III - Electromagnetic Testing (ET)"]),
                        Cert45 = Convert.ToString(dr["ASNT NDT Level III - Infrared/Thermal (IR)"]),
                        Cert46 = Convert.ToString(dr["ASNT NDT Level III - Leak Testing (LT)"]),
                        Cert47 = Convert.ToString(dr["ASNT NDT Level III - Magnetic Flux Leakage (ML)"]),
                        Cert48 = Convert.ToString(dr["ASNT NDT Level III - Magnetic Particle Testing (MT)"]),
                        Cert49 = Convert.ToString(dr["ASNT NDT Level III - Liquid Penetrant Testing (PT)"]),
                        Cert50 = Convert.ToString(dr["ASNT NDT Level III - Radiographic Testing (RT)"]),
                        Cert51 = Convert.ToString(dr["ASNT NDT Level III - Ultrasonic Testing (UT)"]),
                        Cert52 = Convert.ToString(dr["ASNT NDT Level III - Visual Testing (VT)"]),
                        Cert53 = Convert.ToString(dr["ASNT NDT Level III - Phased Array Ultrasonic Testing (PAUT)"]),
                        Cert54 = Convert.ToString(dr["ASNT NDT Level III - Time of Flight Diffraction Testing (ToFD)"]),
                        Cert55 = Convert.ToString(dr["ASNT NDT Level III - Eddy Current Testing (ECT)"]),
                        Cert56 = Convert.ToString(dr["ASNT NDT Level II - Acoustic Emission testing (AE)"]),
                        Cert57 = Convert.ToString(dr["ASNT NDT Level II - Electromagnetic Testing (ET)"]),
                        Cert58 = Convert.ToString(dr["ASNT NDT Level II - Infrared/Thermal (IR)"]),
                        Cert59 = Convert.ToString(dr["ASNT NDT Level II - Leak Testing (LT)"]),
                        Cert60 = Convert.ToString(dr["ASNT NDT Level II - Magnetic Flux Leakage (ML)"]),
                        Cert61 = Convert.ToString(dr["ASNT NDT Level II - Magnetic Particle Testing (MT)"]),
                        Cert62 = Convert.ToString(dr["ASNT NDT Level II - Liquid Penetrant Testing (PT)"]),
                        Cert63 = Convert.ToString(dr["ASNT NDT Level II - Radiographic Testing (RT)"]),
                        Cert64 = Convert.ToString(dr["ASNT NDT Level II - Ultrasonic Testing (UT)"]),
                        Cert65 = Convert.ToString(dr["ASNT NDT Level II - Visual Testing (VT)"]),
                        Cert66 = Convert.ToString(dr["ASNT NDT Level II - Phased Array Ultrasonic Testing (PAUT)"]),
                        Cert67 = Convert.ToString(dr["ASNT NDT Level II - Time of Flight Diffraction Testing (ToFD)"]),
                        Cert68 = Convert.ToString(dr["ASNT NDT Level II - Eddy Current Testing (ECT)"]),
                        Cert69 = Convert.ToString(dr["NACE CIP Level 1"]),
                        Cert70 = Convert.ToString(dr["NACE CIP Level 2"]),
                        Cert71 = Convert.ToString(dr["NACE CIP Level 3"]),
                        Cert72 = Convert.ToString(dr["NACE Cathodic Protection Technician Level 2"]),
                        Cert73 = Convert.ToString(dr["SSPC PCI Level 1"]),
                        Cert74 = Convert.ToString(dr["SSPC PCI Level 2"]),
                        Cert75 = Convert.ToString(dr["SSPC PCI Level 3"]),
                        Cert76 = Convert.ToString(dr["FROSIO Inspector Level I   (white)"]),
                        Cert77 = Convert.ToString(dr["FROSIO Inspector Level II  (green)"]),
                        Cert78 = Convert.ToString(dr["FROSIO Inspector Level III (red)"]),
                        Cert79 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 1"]),
                        Cert80 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 2"]),
                        Cert81 = Convert.ToString(dr["BGAS-CSWIP Painting Inspector - Grade 3"]),
                        Cert82 = Convert.ToString(dr["ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2"]),
                        Cert83 = Convert.ToString(dr["ICorr/PFPNet Fire Protection Coatings Inspector Level 3"]),
                        Cert84 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 3"]),
                        Cert85 = Convert.ToString(dr["ICorr Hot Dip Galvanizing Inspector Course"]),
                        Cert86 = Convert.ToString(dr["ICorr Passive Fire Protection (PFP) Coating Inspector (Epoxy) Level 2"]),
                        Cert87 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 1"]),
                        Cert88 = Convert.ToString(dr["ICorr Insulation Inspector – Level 2"]),
                        Cert89 = Convert.ToString(dr["ICorr Protective Coatings Inspector – Level 2"]),
                        Cert90 = Convert.ToString(dr["ICorr Pipeline Coatings Inspector – Level 2"]),
                        Cert91 = Convert.ToString(dr["ISO 9001:2015 Lead Auditor"]),
                        Cert92 = Convert.ToString(dr["ISO 9001:2015 Internal Auditor"]),
                        Cert93 = Convert.ToString(dr["ISO 14001:2015 Lead Auditor"]),
                        Cert94 = Convert.ToString(dr["ISO 14001:2015 Internal Auditor"]),
                        Cert95 = Convert.ToString(dr["ISO 45001:2018 Lead Auditor"]),
                        Cert96 = Convert.ToString(dr["ISO 45001:2018  Internal Auditor"]),
                        Cert97 = Convert.ToString(dr["Integrated Management System Lead Auditor"]),
                        Cert98 = Convert.ToString(dr["Integrated Management System Internal Auditor"]),
                        Cert99 = Convert.ToString(dr["AS 9100:2016 Rev. D Lead  Auditor"]),
                        Cert100 = Convert.ToString(dr["AS 9100:2016 Rev. D Internal Auditor"]),
                        Cert101 = Convert.ToString(dr["SA 8000:2014  Lead Auditor"]),
                        Cert102 = Convert.ToString(dr["SA 8000:2014  Internal Auditor"]),
                        Cert103 = Convert.ToString(dr["IATF 16949 : 2016 Lead Auditor"]),
                        Cert104 = Convert.ToString(dr["IATF 16949 : 2016 Internal Auditor"]),
                        Cert105 = Convert.ToString(dr["ISO/IEC 17025:2017 Lead Auditor"]),
                        Cert106 = Convert.ToString(dr["ISO/IEC 17025:2017 Internal Auditor"]),
                        Cert107 = Convert.ToString(dr["Lean Six Sigma Green Belt"]),
                        Cert108 = Convert.ToString(dr["FSSC Ver. 5.1 Lead Auditor"]),
                        Cert109 = Convert.ToString(dr["FSSC Ver. 5.1 Internal Auditor"]),
                        Cert110 = Convert.ToString(dr[" ISO 22000:2018 Lead Auditor"]),
                        Cert111 = Convert.ToString(dr["ISO 22000:2018 Internal Auditor"]),
                        Cert112 = Convert.ToString(dr["ISO 27001:2013 Lead Auditor"]),
                        Cert113 = Convert.ToString(dr["ISO 27001:2013 Internal Auditor"]),
                        Cert114 = Convert.ToString(dr["Chartered Engineer"]),
                        Cert115 = Convert.ToString(dr["Chartered Accountant"]),
                        Cert116 = Convert.ToString(dr["ICWA"]),
                        Cert117 = Convert.ToString(dr["Charted Financial Analyst"]),
                        Cert118 = Convert.ToString(dr["IBR Competent Inspector"]),
                        Cert119 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER SMPV (U) RULES"]),
                        Cert120 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER PETROLEUM RULES"]),
                        Cert121 = Convert.ToString(dr["PESO COMPETENT PERSON UNDER EXPLOSIVE RULES"]),
                        Cert122 = Convert.ToString(dr["CPR qualified Auditor"]),
                        Cert123 = Convert.ToString(dr["ISO 3834 Qualified Auditor"]),
                        Cert124 = Convert.ToString(dr["AD 2000 HP0 Qualified Auditor"]),
                        Cert125 = Convert.ToString(dr["EN 15085 Qualified Auditor"]),
                        Cert126 = Convert.ToString(dr["EN 1090 Qualified Auditor"]),
                        Cert127 = Convert.ToString(dr["VdTUV-Merkblatt 1153 Qualified Auditor"]),
                        Cert128 = Convert.ToString(dr["PESR Qualified Auditor"]),
                        Cert129 = Convert.ToString(dr["PESR Qualified Inspector"]),
                        Cert130 = Convert.ToString(dr["DIN 2303 Qualified Auditor"]),
                        Cert131 = Convert.ToString(dr["NORSOK Qualified Auditor"]),
                        Cert132 = Convert.ToString(dr["TPED Qualified Auditor"]),
                        Cert133 = Convert.ToString(dr["TPED Qualified Inspector"]),
                        Cert134 = Convert.ToString(dr["PED PDGR Authorisation for 1100"]),
                        Cert135 = Convert.ToString(dr["PED PDGR Authorisation for 1110"]),
                        Cert136 = Convert.ToString(dr["PED PDGR Authorisation for 1120"]),
                        Cert137 = Convert.ToString(dr["PED PDGR Authorisation for 1130"]),
                        Cert138 = Convert.ToString(dr["PED PDGR Authorisation for 1140"]),
                        Cert139 = Convert.ToString(dr["PED PDGR Authorisation for 1150"]),
                        Cert140 = Convert.ToString(dr["PED PDGR Authorisation for 1160"]),
                        Cert141 = Convert.ToString(dr["PED PDGR Authorisation for 1180"]),
                        Cert142 = Convert.ToString(dr["PED PDGR Authorisation for 1500"]),
                        Cert143 = Convert.ToString(dr["PED PDGR Authorisation for 1510"]),
                        Cert144 = Convert.ToString(dr["PED PDGR Authorisation for 1520"]),
                        Cert145 = Convert.ToString(dr["PED PDGR Authorisation for 1530"]),
                        Cert146 = Convert.ToString(dr["PED PDGR Authorisation for 1540"]),
                        Cert147 = Convert.ToString(dr["PED PDGR Authorisation for 1550"]),
                        Cert148 = Convert.ToString(dr["PED PDGR Authorisation for 1560"]),
                        Cert149 = Convert.ToString(dr["PED PDGR Authorisation for 3000"]),
                        Cert150 = Convert.ToString(dr["PED PDGR Authorisation for 3100"]),
                        Cert151 = Convert.ToString(dr["PED PDGR Authorisation for 4000"]),
                        Cert152 = Convert.ToString(dr["PED PDGR Authorisation for 4100"]),
                        Cert153 = Convert.ToString(dr["PED PDGR Authorisation for 4200"]),
                        Cert154 = Convert.ToString(dr["PED PDGR Authorisation for 4210"]),
                        Cert155 = Convert.ToString(dr["PED PDGR Authorisation for 4220"]),
                        Cert156 = Convert.ToString(dr["PED PDGR Authorisation for 4230"]),
                        Cert157 = Convert.ToString(dr["PED PDGR Authorisation for 4400"]),
                        Cert158 = Convert.ToString(dr["PED PDGR Authorisation for 5000"]),
                        Cert159 = Convert.ToString(dr["PED PDGR Authorisation for 6000"]),
                        Cert160 = Convert.ToString(dr["PED PDGR Authorisation for 6100"]),
                        Cert161 = Convert.ToString(dr["International Welding Engineer (IWE - IIW)"]),
                        Cert162 = Convert.ToString(dr["International Welding Technologist (IWT - IIW)"]),
                        Cert163 = Convert.ToString(dr["International Welding Specialist (IWS - IIW)"]),
                        Cert164 = Convert.ToString(dr["NBBI AI- Authorized Inspector"]),
                        Cert165 = Convert.ToString(dr["NBBI R - Repair Inspector (NBIC)"]),
                        Cert166 = Convert.ToString(dr["NBBI B - Inspector Supervisor"]),
                        Cert167 = Convert.ToString(dr["NBBI N - Nuclear Inspector "]),
                        Cert168 = Convert.ToString(dr["NBBII - Nuclear Inspector "]),
                        Cert169 = Convert.ToString(dr["NBBI NS - Nuclear Inspector Supervisor "]),
                        Cert170 = Convert.ToString(dr["NBBI C - Nuclear Inspector (Concrete) "]),
                        Cert171 = Convert.ToString(dr["NBBI NSC - Nuclear Inspector Supervisor (Concrete)"]),
                        Cert172 = Convert.ToString(dr["NBBI NSI - Nuclear In-service Inspector Supervisor"]),
                        Cert173 = Convert.ToString(dr["ARAMCO approved Inspector"])
                    });
                }
            }

            return lstCompanyDashBoard;
        }


        #region   SendInductionMail
        //SendInductionMail(Appr1, Appr2, PCH, BranchQA, Username, UserEmail);
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



        #endregion

        #region   SendInductionMailOtherThanPayroll
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



        #endregion


    }
}