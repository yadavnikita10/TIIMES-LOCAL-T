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


                }
            }
            return View(ObjModelUsers);
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

        public ActionResult CreateUser(Users URS, HttpPostedFileBase File, FormCollection fc, List<Users> DArray, List<Users> DArrayPFC)
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

                                Result1 = objDalCreateUser.InsertProfsionalCerts(URS, Convert.ToInt32(Result), chkVerified);
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
            grid.Columns.Add(model => model.Branch).Titled("Branch");
            grid.Columns.Add(model => model.EmployeeGrade).Titled("Employee Grade");
            grid.Columns.Add(model => model.SAP_VendorCode).Titled("SAP Vendor Code");
            grid.Columns.Add(model => model.SAPEmployeeCode).Titled("SAP Employee Code");
            grid.Columns.Add(model => model.DateOfJoining).Titled("Date Of Joining");
            grid.Columns.Add(model => model.Designation).Titled("Designation");
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
            grid.Columns.Add(model => model.PanNo).Titled("Pan No");
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
                               Year = Convert.ToString(dr["Year"]),
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






    }
}