using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuvVision.Models;
using System.Data;
using TuvVision.DataAccessLayer;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;

using System.Configuration;

using System.Data.OleDb;
using System.Xml;
using System.Text.RegularExpressions;
using System.Drawing;
using SelectPdf;
using HtmlAgilityPack;
using System.Reflection;

namespace TuvVision.Controllers
{
    public class CreateInternationalUserController : Controller
    {
        InternationalUsers Dalusers = new InternationalUsers();
        DalInternaltionUsers objusers = new DalInternaltionUsers();
        DALUsers objDalCreateUser = new DALUsers();
        Users ObjModelUsers = new Users();
       
        // GET: CreateInternationalUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateInternationalUser(string Firstname, string lastname, string DOB, string Role, string CoreStudy)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string Pass = Encrypt("Pass@123");
            var EncryptedPass = Pass;
            dt = objusers.CreateUser(Firstname, lastname, DOB, Role, EncryptedPass, CoreStudy);
            return Json("Success", JsonRequestBehavior.AllowGet);            //if (dt.Rows.Count > 0)
            //{
            //    return Json(new { success = true, message = "User created successfully!" });

            //}
            //else
            //{
            //    return Json(new { success = false, message = "Something Went Wrong!" });
            //}
        }
        [HttpGet]
        public ActionResult GetData()
        {
            DataSet ds = new DataSet();
            ds = objusers.GetData();
            string json = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                json = JsonConvert.SerializeObject(ds.Tables[0]);
            }
            else
            {
                json = JsonConvert.SerializeObject("Error");
            }
            return Json(json, JsonRequestBehavior.AllowGet);
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
        public ActionResult UserActivateDeactivate(string PK_UserID, string isActive)
        {
            DataSet ds = new DataSet();
            ds = objusers.Activate_Deactivate_User(PK_UserID, isActive);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateCV(string id)
        {
            //ObjModelUsers = Session["UserProfile"] as Users;
            //ObjModelUsers.FullName = ObjModelUsers.FirstName + " " + ObjModelUsers.MiddleName + " " + ObjModelUsers.LastName;

            DateTime now = DateTime.Now;
            DataTable ds = new DataTable();
            ds = objusers.GetDataForCV(id);
            ObjModelUsers.FullName = ds.Rows[0]["Name"].ToString();
            ObjModelUsers.DateOfJoining = ds.Rows[0]["DateOfJoining"].ToString();


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
            //ObjModelUsers.TUVTotalyearofExperience = DOJyears + " Years and " + FinalMonth + " Months";
            //ViewBag.lstDOrderType = Session["EducationDetails"];
            //ViewBag.lstProfCerts = Session["lstProfCerts"];

            DataSet DSGetUserCV = new DataSet();
            DSGetUserCV = objDalCreateUser.GetUserCV(id);

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



    }
}


