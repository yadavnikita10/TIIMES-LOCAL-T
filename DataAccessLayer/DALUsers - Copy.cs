using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TuvVision.DataAccessLayer
{
    public class DALUsers
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        public DataTable GetUserDashBoard() //User Role DashBoard
        {

            DataTable DTGetRoleDashBoard = new DataTable();
            try
            {
                SqlCommand CMDRoleDashBoard = new SqlCommand("SP_CreateUser", con);
                CMDRoleDashBoard.CommandType = CommandType.StoredProcedure;
                CMDRoleDashBoard.CommandTimeout = 100000;
                CMDRoleDashBoard.Parameters.AddWithValue("@SP_Type", 1);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDRoleDashBoard);
                SDADashBoardData.Fill(DTGetRoleDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetRoleDashBoard.Dispose();
            }

            return DTGetRoleDashBoard;
        }


        //added by nikita on 28/09/2023
        public DataTable GetPchName(string brid) //User PCHName DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("Sp_Get_PCH", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@PK_USERID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                CMDCallDash.Parameters.AddWithValue("@costcenter", brid);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }
        //added by nikita on 26092023
        public DataTable GetUserMis_() //User Role DashBoard
        {

            DataTable DTGetRoleDashBoard = new DataTable();
            try
            {
                //SqlCommand CMDRoleDashBoard = new SqlCommand("Sp_Userdetail", con);
                SqlCommand CMDRoleDashBoard = new SqlCommand("Sp_Userdetail_Data", con);
                CMDRoleDashBoard.CommandType = CommandType.StoredProcedure;
                CMDRoleDashBoard.CommandTimeout = 100000;
                CMDRoleDashBoard.Parameters.AddWithValue("@SP_Type", 2);
                CMDRoleDashBoard.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDRoleDashBoard);
                SDADashBoardData.Fill(DTGetRoleDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetRoleDashBoard.Dispose();
            }

            return DTGetRoleDashBoard;
        }


        //added by nikita on 28092023
        public DataTable PasswordReset(string userid, string Password) //User PCHName DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("Sp_PasswordReset", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@User_Pk_UserId", userid);
                CMDCallDash.Parameters.AddWithValue("@password", Password);
                CMDCallDash.Parameters.AddWithValue("@Encryptpassword", Encrypt(Password));
                CMDCallDash.Parameters.AddWithValue("@Pk_UserId", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));

                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }



        //added by nikita on 29092023
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

        public DataSet GetGradeMaster()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 12);

                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }


        public DataSet GetDdlLst()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 4);
                //CMDGetDdlLst.Parameters.AddWithValue("",Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public DataSet GetObsTypeEmpLst(string empType)
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 13);
                CMDGetDdlLst.Parameters.AddWithValue("@Employee_Type", empType);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        /*
        public string InsertUpdateUsers(Users URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (URS.PK_UserID != null && URS.PK_UserID != "")
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_UserID", URS.PK_UserID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserName", URS.UserName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmailID", URS.EmailID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", DateTime.ParseExact(URS.DateOfJoining, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qualification", URS.Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeGrade", URS.EmployeeGradeId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeCode", URS.EmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAP_VendorCode", URS.SAP_VendorCode);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address1", URS.Address1);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address2", URS.Address2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@IsActive", URS.IsActive);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_RoleID", URS.UserRole);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Gender", URS.Gender);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@Password", URS.Password);
                    if(URS.DOB==null || URS.DOB == "")
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", null);
                    }
                    else
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));
                    }
                    
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MobileNo", URS.MobileNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ResidenceNo", URS.ResidenceNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OfficeNo", URS.OfficeNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Designation", URS.Designation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LanguageSpoken", URS.LanguageSpoken);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ZipCode", URS.ZipCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Signature", URS.FilePath);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_BranchID", URS.Branch);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVEmailId", URS.TuvEmailId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingOne", URS.ReportingOne);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingTwo", URS.ReportingTwo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TransferDate", URS.TransferDate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Employee_Type", URS.Employee_Type);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@BloodGroup", URS.BloodGroup);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShoesSize", URS.ShoesSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShirtSize", URS.ShirtSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Locked", URS.IsLocked);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CostCenter_Id", URS.CostCenter);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Image", URS.Image);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CV", URS.CV);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PanNo", URS.PanNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AadharNo", URS.AadharNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TotalyearofExprience", URS.TotalyearofExprience);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ExperienceInMonth", URS.ExperienceInMonth);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Allergies", URS.Allergies);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Medical_History", URS.Medical_History);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Marital_Status", URS.Marital_Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmergencyMobile_No", URS.EmergencyMobile_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Fax_No", URS.Fax_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Additional_Qualification", URS.Additional_Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MiddleName", URS.MiddleName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVUIN", URS.TUVUIN);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PermanantPin", URS.PermanantPin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVIStampNo", URS.TUVIStampNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OPE", URS.OPE);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReasonForUpdate", URS.ReasonForUpdate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployementCategory", URS.EmployementCategory);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.MajorFieldOfStudy);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OtherUniversity", URS.OtherUniversity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteDetail", URS.SiteDetail);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ItemToBeInspected", URS.ItemToBeInspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAPEmployeeCode", URS.SAPEmployeeCode);

                    if (URS.OrgChangeDate!= null)
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@OrgChangeDate", DateTime.ParseExact(URS.OrgChangeDate, "dd/MM/yyyy", theCultureInfo));


                    SqlParameter RequestID = CMDInsertUpdateUsers.Parameters.Add("@ReturnId", SqlDbType.VarChar, 100);
                    CMDInsertUpdateUsers.Parameters["@ReturnId"].Direction = ParameterDirection.Output;

                    CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDInsertUpdateUsers.Parameters["@ReturnId"].Value);



                }
                else
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_UserID", URS.PK_UserID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserName", URS.UserName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmailID", URS.EmailID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", DateTime.ParseExact(URS.DateOfJoining, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qualification", URS.Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeGrade", URS.EmployeeGradeId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeCode", URS.EmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAP_VendorCode", URS.SAP_VendorCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address1", URS.Address1);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address2", URS.Address2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@IsActive", URS.IsActive);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_RoleID", URS.UserRole);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Gender", URS.Gender);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Password", "Pass@123");
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));
                    if (URS.DOB == null || URS.DOB == "")
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", null);
                    }
                    else
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MobileNo", URS.MobileNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ResidenceNo", URS.ResidenceNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OfficeNo", URS.OfficeNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Designation", URS.Designation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LanguageSpoken", URS.LanguageSpoken);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ZipCode", URS.ZipCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Signature", URS.FilePath);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_BranchID", URS.Branch);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVEmailId", URS.TuvEmailId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingOne", URS.ReportingOne);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingTwo", URS.ReportingTwo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Employee_Type", URS.Employee_Type);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Flag", 0);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@BloodGroup", URS.BloodGroup);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShoesSize", URS.ShoesSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShirtSize", URS.ShirtSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Locked", URS.IsLocked);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CostCenter_Id", URS.CostCenter);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Image", URS.Image);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CV", URS.CV);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PanNo", URS.PanNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AadharNo", URS.AadharNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TotalyearofExprience", URS.TotalyearofExprience);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ExperienceInMonth", URS.ExperienceInMonth);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Allergies", URS.Allergies);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Medical_History", URS.Medical_History);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Marital_Status", URS.Marital_Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmergencyMobile_No", URS.EmergencyMobile_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Fax_No", URS.Fax_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MiddleName", URS.MiddleName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVUIN", URS.TUVUIN);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PermanantPin", URS.PermanantPin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVIStampNo", URS.TUVIStampNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OPE", URS.OPE);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReasonForUpdate", URS.ReasonForUpdate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployementCategory", URS.EmployementCategory);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.MajorFieldOfStudy);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OtherUniversity", URS.OtherUniversity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteDetail", URS.SiteDetail);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ItemToBeInspected", URS.ItemToBeInspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAPEmployeeCode", URS.SAPEmployeeCode); 
                     Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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
        */

        public string InsertUpdateUsers(Users URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (URS.PK_UserID != null && URS.PK_UserID != "")
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_UserID", URS.PK_UserID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserName", URS.UserName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmailID", URS.EmailID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", DateTime.ParseExact(URS.DateOfJoining, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qualification", URS.Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeGrade", URS.EmployeeGradeId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeCode", URS.EmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAP_VendorCode", URS.SAP_VendorCode);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address1", URS.Address1);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address2", URS.Address2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@IsActive", URS.IsActive);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_RoleID", URS.UserRole);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Gender", URS.Gender);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@Password", URS.Password);
                    if (URS.DOB != null)
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MobileNo", URS.MobileNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ResidenceNo", URS.ResidenceNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OfficeNo", URS.OfficeNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Designation", URS.Designation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LanguageSpoken", URS.LanguageSpoken);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ZipCode", URS.ZipCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Signature", URS.FilePath);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_BranchID", URS.Branch);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVEmailId", URS.TuvEmailId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingOne", URS.ReportingOne);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingTwo", URS.ReportingTwo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TransferDate", URS.TransferDate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Employee_Type", URS.Employee_Type);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@BloodGroup", URS.BloodGroup);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShoesSize", URS.ShoesSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShirtSize", URS.ShirtSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Locked", URS.IsLocked);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CostCenter_Id", URS.CostCenter);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Image", URS.Image);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CV", URS.CV);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PanNo", URS.PanNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AadharNo", URS.AadharNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TotalyearofExprience", URS.TotalyearofExprience);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ExperienceInMonth", URS.ExperienceInMonth);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Allergies", URS.Allergies);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Medical_History", URS.Medical_History);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Marital_Status", URS.Marital_Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmergencyMobile_No", URS.EmergencyMobile_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Fax_No", URS.Fax_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Additional_Qualification", URS.Additional_Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MiddleName", URS.MiddleName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVUIN", URS.TUVUIN);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PermanantPin", URS.PermanantPin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVIStampNo", URS.TUVIStampNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OPE", URS.OPE);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReasonForUpdate", URS.ReasonForUpdate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployementCategory", URS.EmployementCategory);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.MajorFieldOfStudy);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OtherUniversity", URS.OtherUniversity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteDetail", URS.SiteDetail);
                    //Added by shrutika salve 10082023
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteAddrPin", URS.SitePin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ItemToBeInspected", URS.ItemToBeInspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAPEmployeeCode", URS.SAPEmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LoginRoleID", URS.LoginUserRoleId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PFUANNumber", URS.PFUANNumber);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                    int chkVerified = 0;
                    if (URS.isVerified == true)
                        chkVerified = 1;
                    else
                        chkVerified = 0;

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified)
                        ;
                    if (URS.OrgChangeDate != null)
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@OrgChangeDate", DateTime.ParseExact(URS.OrgChangeDate, "dd/MM/yyyy", theCultureInfo));


                    SqlParameter RequestID = CMDInsertUpdateUsers.Parameters.Add("@ReturnId", SqlDbType.VarChar, 100);
                    CMDInsertUpdateUsers.Parameters["@ReturnId"].Direction = ParameterDirection.Output;

                    CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDInsertUpdateUsers.Parameters["@ReturnId"].Value);



                }
                else
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_UserID", URS.PK_UserID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FirstName", URS.FirstName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LastName", URS.LastName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserName", URS.UserName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmailID", URS.EmailID);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", DateTime.ParseExact(URS.DateOfJoining, "dd/MM/yyyy", theCultureInfo));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qualification", URS.Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeGrade", URS.EmployeeGradeId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployeeCode", URS.EmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAP_VendorCode", URS.SAP_VendorCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address1", URS.Address1);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address2", URS.Address2);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@IsActive", URS.IsActive);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_RoleID", URS.UserRole);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Gender", URS.Gender);
                    //CMDInsertUpdateUsers.Parameters.AddWithValue("@Password", "Pass@123");
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Password", "Pass@123");
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EncryptedPassword", URS.Password);
                    if (URS.DOB != null)
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MobileNo", URS.MobileNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ResidenceNo", URS.ResidenceNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OfficeNo", URS.OfficeNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Designation", URS.Designation);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LanguageSpoken", URS.LanguageSpoken);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ZipCode", URS.ZipCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Signature", URS.FilePath);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@FK_BranchID", URS.Branch);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVEmailId", URS.TuvEmailId);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingOne", URS.ReportingOne);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReportingTwo", URS.ReportingTwo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Employee_Type", URS.Employee_Type);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Flag", 0);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@BloodGroup", URS.BloodGroup);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShoesSize", URS.ShoesSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShirtSize", URS.ShirtSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Locked", URS.IsLocked);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CostCenter_Id", URS.CostCenter);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Image", URS.Image);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CV", URS.CV);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PanNo", URS.PanNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AadharNo", URS.AadharNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TotalyearofExprience", URS.TotalyearofExprience);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ExperienceInMonth", URS.ExperienceInMonth);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Allergies", URS.Allergies);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Medical_History", URS.Medical_History);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Marital_Status", URS.Marital_Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmergencyMobile_No", URS.EmergencyMobile_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Fax_No", URS.Fax_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MiddleName", URS.MiddleName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVUIN", URS.TUVUIN);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PermanantPin", URS.PermanantPin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TUVIStampNo", URS.TUVIStampNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OPE", URS.OPE);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ReasonForUpdate", URS.ReasonForUpdate);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmployementCategory", URS.EmployementCategory);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.MajorFieldOfStudy);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.University);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OtherUniversity", URS.OtherUniversity);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CurrentAssignment", URS.CurrentAssignment);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteDetail", URS.SiteDetail);
                    //added by shrutika salve 10082023
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SiteAddrPin", URS.SitePin);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ItemToBeInspected", URS.ItemToBeInspected);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SAPEmployeeCode", URS.SAPEmployeeCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PFUANNumber", URS.PFUANNumber);

                    SqlParameter RequestID = CMDInsertUpdateUsers.Parameters.Add("@ReturnId", SqlDbType.VarChar, 100);
                    CMDInsertUpdateUsers.Parameters["@ReturnId"].Direction = ParameterDirection.Output;

                    CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                    Result = Convert.ToString(CMDInsertUpdateUsers.Parameters["@ReturnId"].Value);
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

        public DataTable EditUsers(string PK_UserID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", PK_UserID);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }
        public int DeleteUser(string PK_UserID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDUserDelete = new SqlCommand("SP_CreateUser", con);
                CMDUserDelete.CommandType = CommandType.StoredProcedure;
                CMDUserDelete.CommandTimeout = 100000;
                CMDUserDelete.Parameters.AddWithValue("@SP_Type", 5);
                CMDUserDelete.Parameters.AddWithValue("@PK_UserID", PK_UserID);
                CMDUserDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                Result = CMDUserDelete.ExecuteNonQuery();
                if (Result != 0)
                {
                    return Result;
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

        #region   User Profile Code 

        public DataTable GetProfiledata() //User Role DashBoard
        {

            DataTable DTGetRoleDashBoard = new DataTable();
            try
            {
                SqlCommand CMDRoleDashBoard = new SqlCommand("SP_CreateUser", con);
                CMDRoleDashBoard.CommandType = CommandType.StoredProcedure;
                CMDRoleDashBoard.CommandTimeout = 100000;
                CMDRoleDashBoard.Parameters.AddWithValue("@SP_Type", 6);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDRoleDashBoard);
                SDADashBoardData.Fill(DTGetRoleDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetRoleDashBoard.Dispose();
            }

            return DTGetRoleDashBoard;
        }



        public DataTable EditProfile(string PK_UserID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", PK_UserID);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }





        public string Updatprofile(ProfileUser URS)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                if (URS.PK_UserID != null || URS.PK_UserID != "")
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", 8);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PK_UserID", URS.PK_UserID);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmailID", URS.EmailID);
                    // CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", Convert.ToDateTime(URS.DateOfJoining));
                    if (URS.DateOfJoining != null)
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DateOfJoining", DateTime.ParseExact(URS.DateOfJoining, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Qualification", URS.Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Address1", URS.Address1);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ZipCode", URS.ZipCode);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Signature", URS.Signature);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CostCenter_Id", URS.Pk_CC_Id.ToString());
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@OfficeNo", URS.OfficeNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ResidenceNo", URS.ResidenceNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CV", URS.CV);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Gender", URS.Gender);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Image", URS.Image);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Marital_Status", URS.Marital_Status);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@BloodGroup", URS.BloodGroup);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShoesSize", URS.ShoesSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ShirtSize", URS.ShirtSize);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@PanNo", URS.PanNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AadharNo", URS.AadharNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@TotalyearofExprience", URS.TotalyearofExprience);

                    if (URS.DOB != null)
                    {
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@DOB", DateTime.ParseExact(URS.DOB, "dd/MM/yyyy", theCultureInfo));
                    }
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Medical_History", URS.Medical_History);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Allergies", URS.Allergies);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Additional_Qualification", URS.Additional_Qualification);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@EmergencyMobile_No", URS.EmergencyMobile_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Fax_No", URS.Fax_No);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Cost_center", URS.Cost_Center);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));

                    Result = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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
        #endregion
        public DataTable CheckExistingProductName(string ExistingUserName)
        {
            DataTable DTExistUserName = new DataTable();
            try
            {
                SqlCommand CMDExistsUsertName = new SqlCommand("SP_CreateUser", con);
                CMDExistsUsertName.CommandType = CommandType.StoredProcedure;
                CMDExistsUsertName.CommandTimeout = 1000000;
                CMDExistsUsertName.Parameters.AddWithValue("@SP_Type", 9);
                CMDExistsUsertName.Parameters.AddWithValue("@ExistingUserName", ExistingUserName);
                SqlDataAdapter SDAExistsProName = new SqlDataAdapter(CMDExistsUsertName);
                SDAExistsProName.Fill(DTExistUserName);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();

            }
            finally
            {
                DTExistUserName.Dispose();
            }
            return DTExistUserName;
        }


        public List<CostCenterModel> GetCostCenterList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CostCenterModel> lstEnquiryDashB = new List<CostCenterModel>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 10);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new CostCenterModel
                           {
                               Pk_CC_Id = Convert.ToInt32(dr["Pk_CC_Id"]),
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
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataTable GetCostCanterByJason() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("SP_CreateUser", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 10);
                //CMDCallDash.Parameters.AddWithValue("@FK_BranchID", FK_BranchID);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }


        #region Mis Report
        public DataTable GetUserMis() //User Role DashBoard
        {

            DataTable DTGetRoleDashBoard = new DataTable();
            try
            {
                SqlCommand CMDRoleDashBoard = new SqlCommand("SP_MisReportMaster", con);
                CMDRoleDashBoard.CommandType = CommandType.StoredProcedure;
                CMDRoleDashBoard.CommandTimeout = 100000;
                CMDRoleDashBoard.Parameters.AddWithValue("@SP_Type", 2);
                CMDRoleDashBoard.Parameters.AddWithValue("@UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDRoleDashBoard);
                SDADashBoardData.Fill(DTGetRoleDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTGetRoleDashBoard.Dispose();
            }

            return DTGetRoleDashBoard;
        }

        #endregion




        public string InsertVaccinationAttachment(List<FileDetails> lstFileUploaded, int ID, string strAttachmentType)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("Fk_UserId", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, strAttachmentType);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_UserCreationAttachment", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTUserCreationUploadedFile", DTUploadFile);
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

        public DataTable GetAtt_old(string Id, string AttachmentType)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 2);
                CMDEditUsers.Parameters.AddWithValue("@FK_userid", Id);
                CMDEditUsers.Parameters.AddWithValue("@AttachmentType", AttachmentType);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable GetAtt(string Id)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 5);
                CMDEditUsers.Parameters.AddWithValue("@FK_userid", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable GetEyeTest(string Id)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 8);
                CMDEditUsers.Parameters.AddWithValue("@FK_userid", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable GetPhoto(string Id)
        {
            DataTable DTPhoto = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", "11");
                CMDEditUsers.Parameters.AddWithValue("@FK_userid", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTPhoto);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTPhoto.Dispose();
            }
            return DTPhoto;
        }

        public DataTable GetSignature(string Id)
        {
            DataTable DTPhoto = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", "12");
                CMDEditUsers.Parameters.AddWithValue("@FK_userid", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTPhoto);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTPhoto.Dispose();
            }
            return DTPhoto;
        }


        public string InsertUserAttachment(List<FileDetails> lstFileUploaded, int ID, string DocType, string Year)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("Fk_UserId", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Year", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    // DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, DocType, Year);
                    DTUploadFile.Rows.Add(ID, item.FileName.Replace(",",""), item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, DocType, Year);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_UserCreationAttachment", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTUserCreationUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@Fk_UserId", ID);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@AttachmentType", DocType);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@Year", Year);
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

        public string InsertUserAttachmentEyeDoc(List<FileDetails> lstFileUploaded, int ID, string DocType, string Year)
        {
            string Result = string.Empty;
            DataTable DTUploadFile = new DataTable();
            string UserName = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
            con.Open();
            try
            {
                DTUploadFile.Columns.Add(new DataColumn("Fk_UserId", typeof(int)));
                DTUploadFile.Columns.Add(new DataColumn("FileName", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Extenstion", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("FileID", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("ModifiedDate", typeof(DateTime)));
                DTUploadFile.Columns.Add(new DataColumn("FileContent", typeof(byte[])));
                DTUploadFile.Columns.Add(new DataColumn("AttachmentType", typeof(string)));
                DTUploadFile.Columns.Add(new DataColumn("Year", typeof(string)));
                foreach (var item in lstFileUploaded)
                {
                    DTUploadFile.Rows.Add(ID, item.FileName, item.Extension, item.Id, UserName, DateTime.Now, UserName, DateTime.Now, item.FileContent, DocType, Year);

                }
                if (lstFileUploaded.Count > 0)
                {
                    SqlCommand CMDSaveUploadedFile = new SqlCommand("SP_UserCreationAttachment", con);
                    CMDSaveUploadedFile.CommandType = CommandType.StoredProcedure;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@SP_Type", 1);
                    SqlParameter tvparam = CMDSaveUploadedFile.Parameters.AddWithValue("@DTUserCreationUploadedFile", DTUploadFile);
                    tvparam.SqlDbType = SqlDbType.Structured;
                    CMDSaveUploadedFile.Parameters.AddWithValue("@Fk_UserId", ID);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@AttachmentType", DocType);
                    CMDSaveUploadedFile.Parameters.AddWithValue("@Year", Year);
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


        public DataTable GetFileContent(int Id)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 3);
                CMDEditUsers.Parameters.AddWithValue("@PK_ID", Id);
                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public string DeleteUploadedFile(string FileID)
        {
            string Result = string.Empty;
            con.Open();
            try
            {
                SqlCommand CMDDeleteUploadedFile = new SqlCommand("SP_UserCreationAttachment", con);
                CMDDeleteUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDDeleteUploadedFile.Parameters.AddWithValue("@SP_Type", 4);
                CMDDeleteUploadedFile.Parameters.AddWithValue("@PK_ID", FileID);
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

        //public List<NameCode> BindUniversity()// Binding Sales Masters DashBoard of Master Page 
        //{
        //    DataTable DTEMDashBoard = new DataTable();
        //    List<NameCode> lstEnquiryDashB = new List<NameCode>();
        //    try
        //    {
        //        SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
        //        CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
        //        CMDGetEnquriy.CommandTimeout = 1000000;
        //        CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "14");

        //        SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
        //        SDAGetEnquiry.Fill(DTEMDashBoard);
        //        if (DTEMDashBoard.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in DTEMDashBoard.Rows)
        //            {
        //                lstEnquiryDashB.Add(
        //                   new NameCode
        //                   {
        //                       Code = Convert.ToInt32(dr["UniversityId"]),
        //                       Name = Convert.ToString(dr["UniversityName"]),
        //                   }
        //                 );
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string Error = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        DTEMDashBoard.Dispose();
        //    }
        //    return lstEnquiryDashB;
        //}

        public DataSet BindUniversity()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 14);

                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public List<CostCenterModel> GetDocumentType()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<CostCenterModel> lstEnquiryDashB = new List<CostCenterModel>();

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 15);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new CostCenterModel
                           {
                               PK_id = Convert.ToInt32(dr["PK_id"]),
                               DocName = Convert.ToString(dr["DocName"]),
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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }


        public int GetUserID(string UserID)
        {
            DataTable DTEditUsers = new DataTable();

            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 18);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", UserID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return Convert.ToInt32(DTEditUsers.Rows[0]["Id"].ToString());
        }


        public string InsertQualificationDetail(Users URS, int UserPrimaryKey, int chkVerified)
        {
            string Result1 = string.Empty;
            con.Open();
            try
            {
                if (UserPrimaryKey > 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "16");

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Course", URS.Course);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@Degree", URS.Degree);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MajorFieldOfStudy", URS.MajorFieldOfStudy);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@University", URS.UniversityName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@LastYearPerc", URS.LastYearCGPA);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@AggregatePerc", URS.AggregateCGPA);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@YearOfPassing", URS.YearOfPassing);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserPrimaryKey", Convert.ToString(UserPrimaryKey));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified);


                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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
            return Result1;
        }



        public DataTable GetEduQualification(String QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CreateUser", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "17");
                CMDEditUploadedFile.Parameters.AddWithValue("@FkId", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }




        public DataTable GetDegreeNames(string strDegree)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 6);
                CMDEditUsers.Parameters.AddWithValue("@DegId", strDegree);

                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public string CheckDocuments(int strUserID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 7);
                CMDEditUsers.Parameters.AddWithValue("@Fk_UserId", strUserID);

                //CMDEditUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers.Rows[0][0].ToString();
        }


        public int GetLoginRoleID()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            int RoleID = 0;

            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 19);
                CMDGetEnquriy.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);

                if (DTEMDashBoard.Rows.Count > 0)
                {
                    RoleID = Convert.ToInt32(DTEMDashBoard.Rows[0]["RoleID"]);
                }
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEMDashBoard.Dispose();
            }
            return RoleID;
        }


        public DataTable GetUserHistory(string PK_UserID)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 20);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", PK_UserID);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable GetCertName(string CompanyName)
        {
            DataTable DTScripName = new DataTable();

            try
            {
                SqlCommand CMDSearchNameCode = new SqlCommand("SP_CreateUser", con);
                CMDSearchNameCode.CommandType = CommandType.StoredProcedure;
                CMDSearchNameCode.CommandTimeout = 1000000000;
                CMDSearchNameCode.Parameters.AddWithValue("@SP_Type", 21);
                CMDSearchNameCode.Parameters.AddWithValue("@CertName", CompanyName);

                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDSearchNameCode);
                SDAScripName.Fill(DTScripName);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTScripName.Dispose();
            }
            return DTScripName;
        }

        public string InsertProfsionalCerts(Users URS, int UserPrimaryKey, int chkVerified)
        {
            string Result1 = string.Empty;
            con.Open();
            try
            {
                if (UserPrimaryKey > 0)
                {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("SP_CreateUser", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "22");

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CertName", URS.CertName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CertNo", URS.CertNo);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CertIssueDate", DateTime.ParseExact(URS.CertIssueDate, "dd/MM/yyyy", theCultureInfo));

                    if (URS.CertValidTill != null)
                        CMDInsertUpdateUsers.Parameters.AddWithValue("@CertValidTill", DateTime.ParseExact(URS.CertValidTill, "dd/MM/yyyy", theCultureInfo));


                    CMDInsertUpdateUsers.Parameters.AddWithValue("@UserPrimaryKey", Convert.ToString(UserPrimaryKey));
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@chkVerified", chkVerified);

                    CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
                    Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
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
            return Result1;
        }

        public DataTable GetProfessionalCerts(String QT_ID)
        {
            DataTable DOrderType = new DataTable();
            try
            {
                SqlCommand CMDEditUploadedFile = new SqlCommand("SP_CreateUser", con);
                CMDEditUploadedFile.CommandType = CommandType.StoredProcedure;
                CMDEditUploadedFile.Parameters.AddWithValue("@SP_Type", "23");
                CMDEditUploadedFile.Parameters.AddWithValue("@FkId", QT_ID);
                SqlDataAdapter SDAEditUploadedFile = new SqlDataAdapter(CMDEditUploadedFile);
                SDAEditUploadedFile.Fill(DOrderType);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DOrderType.Dispose();
            }
            return DOrderType;
        }

        public List<Users> BindYear()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CreateUser", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 24);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               Year = Convert.ToString(dr["Year"]),
                               YearName = Convert.ToString(dr["Year"]),
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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataSet BindYearOfPassing()//Geting List Of Enquiry Master record Details Binding Dddl List
        {
            DataSet DSGetDdlList = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_CreateUser", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.CommandTimeout = 900000000;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 25);
                SqlDataAdapter SDAGetAllDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetAllDdlLst.Fill(DSGetDdlList);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetDdlList.Dispose();
            }
            return DSGetDdlList;
        }

        public DataSet GetCertificatesCount(string Id)
        {
            DataSet DTEditUsers = new DataSet();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 9);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }


        public DataTable GetPKUserID(int Id)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_CreateUser", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 27);
                CMDEditUsers.Parameters.AddWithValue("@PKID", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }

        public DataTable CheckExistingHrEmployeeCode(string ExistingUserName)
        {
            DataTable DTExistUserName = new DataTable();
            try
            {
                SqlCommand CMDExistsUsertName = new SqlCommand("SP_CreateUser", con);
                CMDExistsUsertName.CommandType = CommandType.StoredProcedure;
                CMDExistsUsertName.CommandTimeout = 1000000;
                CMDExistsUsertName.Parameters.AddWithValue("@SP_Type", 26);
                CMDExistsUsertName.Parameters.AddWithValue("@ExistingUserName", ExistingUserName);
                SqlDataAdapter SDAExistsProName = new SqlDataAdapter(CMDExistsUsertName);
                SDAExistsProName.Fill(DTExistUserName);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();

            }
            finally
            {
                DTExistUserName.Dispose();
            }
            return DTExistUserName;
        }

        public DataSet GetVerifiedFiled(string Id)
        {
            DataSet DTEditUsers = new DataSet();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("SP_UserCreationAttachment", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", 10);
                CMDEditUsers.Parameters.AddWithValue("@PK_UserID", Id);
                SqlDataAdapter SDAEditUsers = new SqlDataAdapter(CMDEditUsers);
                SDAEditUsers.Fill(DTEditUsers);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditUsers.Dispose();
            }
            return DTEditUsers;
        }


        public List<Users> GetMantorList()// Binding Mantor List DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_CallsMaster", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", "9N");
                CMDGetEnquriy.Parameters.AddWithValue("@UserID", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new Users
                           {
                               PK_UserID = Convert.ToString(dr["PK_UserID"]),
                               FirstName = Convert.ToString(dr["FirstName"]) + " " + Convert.ToString(dr["LastName"]),

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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }

        public DataSet GetMentorList()
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_Mentor", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 2);
                CMDGetDdlLst.Parameters.AddWithValue("@PK_UserID", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }



        public List<Users> InsertMantorList(Users CM)
             { 
            string Result1 = string.Empty;
            //Users obj = new Users();
            DataTable DTExistUserName = new DataTable();
            List<Users> lstEnquiryDashB = new List<Users>();
            con.Open();
            try
            {
                    SqlCommand CMDInsertUpdateUsers = new SqlCommand("Sp_Mentor", con);
                    CMDInsertUpdateUsers.CommandType = CommandType.StoredProcedure;
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@SP_Type", "1");

                   //CMDInsertUpdateUsers.Parameters.AddWithValue("@ID", System.Web.HttpContext.Current.Session["UserIDs"]);
                   CMDInsertUpdateUsers.Parameters.AddWithValue("@MentorName", CM.FullName);
                    CMDInsertUpdateUsers.Parameters.AddWithValue("@MentorListName",CM.BrAuditee);
                   CMDInsertUpdateUsers.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]));
                   CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedBy",CM.ModifiedBy1);
                   CMDInsertUpdateUsers.Parameters.AddWithValue("@ModifiedDate",CM.ModifiedDate1);

                // Result1 = CMDInsertUpdateUsers.ExecuteNonQuery().ToString();
                SqlDataAdapter SDAScripName = new SqlDataAdapter(CMDInsertUpdateUsers);
                SDAScripName.Fill(DTExistUserName);

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
            return lstEnquiryDashB;
        }



        public DataSet GetDataMentor(String UserId1)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Mentor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SP_Type", 6);
                cmd.Parameters.AddWithValue("@UserID",UserId1);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 120;
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


        public DataTable GetDetails() //User Role DashBoard
        {

            DataTable DTDashBoard = new DataTable();
            try
            {
                SqlCommand CMDCallDash = new SqlCommand("Sp_Mentor", con);
                CMDCallDash.CommandType = CommandType.StoredProcedure;
                CMDCallDash.CommandTimeout = 100000;
                CMDCallDash.Parameters.AddWithValue("@SP_Type", 6);
                CMDCallDash.Parameters.AddWithValue("@UserId", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMDCallDash);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }

        public List<monitors> GetScopeList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<monitors> lstEnquiryDashB = new List<monitors>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_MonitoringDetails", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 1);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new monitors
                           {
                               PK_IAFScopeId = Convert.ToInt32(dr["PK_IAFScopeId"]),
                               IAFScopeName = Convert.ToString(dr["IAFScopeName"]),
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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }


        //public bool DeleteDataRecord(int Id)
        //{
        //    bool Result = 0;
        //    con.Open();
        //    try
        //    {
        //        SqlCommand CMDUserDelete = new SqlCommand("Sp_Mentor", con);
        //        CMDUserDelete.CommandType = CommandType.StoredProcedure;
        //        CMDUserDelete.CommandTimeout = 100000;
        //        CMDUserDelete.Parameters.AddWithValue("@SP_Type", 5);
        //        CMDUserDelete.Parameters.AddWithValue("@Id", Id);
        //        CMDUserDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
        //        Result = CMDUserDelete.ExecuteNonQuery();
        //        if (Result != 0)
        //        {
        //            return Result;
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

        public bool DeleteDataRecord(int Id)
        {
            int i = 0;
            SqlCommand CMDDelete = new SqlCommand("Sp_Mentor", con);
            CMDDelete.CommandType = CommandType.StoredProcedure;
            CMDDelete.Parameters.AddWithValue("@SP_Type", 5);
            CMDDelete.Parameters.AddWithValue("@ID", Id);
            CMDDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"]));
            con.Open();
            i = CMDDelete.ExecuteNonQuery();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public DataSet GetConventional(string PK_UserId)
        {
            DataSet DSGetddlLst = new DataSet();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("SP_TrainingSchedule", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@SP_Type", 105);
                CMDGetDdlLst.Parameters.AddWithValue("@TraineeId", PK_UserId);
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                SDAGetDdlLst.Fill(DSGetddlLst);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGetddlLst.Dispose();
            }
            return DSGetddlLst;
        }

        public DataSet checkdropdown() //User Role check
        {

            DataSet DTDashBoard = new DataSet();
            try
            {
                SqlCommand CMCheck = new SqlCommand("Sp_Mentor", con);
                CMCheck.CommandType = CommandType.StoredProcedure;
                CMCheck.CommandTimeout = 100000;
                CMCheck.Parameters.AddWithValue("@SP_Type", 7);
                CMCheck.Parameters.AddWithValue("@UserId", System.Web.HttpContext.Current.Session["UserIDs"]);
                SqlDataAdapter SDADashBoardData = new SqlDataAdapter(CMCheck);
                SDADashBoardData.Fill(DTDashBoard);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTDashBoard.Dispose();
            }

            return DTDashBoard;
        }

        public List<NonInspectionActivity> GetActivityList()// Binding Sales Masters DashBoard of Master Page 
        {
            DataTable DTEMDashBoard = new DataTable();
            List<NonInspectionActivity> lstEnquiryDashB = new List<NonInspectionActivity>();
            try
            {
                SqlCommand CMDGetEnquriy = new SqlCommand("SP_NonInspectionActivities", con);
                CMDGetEnquriy.CommandType = CommandType.StoredProcedure;
                CMDGetEnquriy.CommandTimeout = 1000000;
                CMDGetEnquriy.Parameters.AddWithValue("@SP_Type", 24);

                SqlDataAdapter SDAGetEnquiry = new SqlDataAdapter(CMDGetEnquriy);
                SDAGetEnquiry.Fill(DTEMDashBoard);
                if (DTEMDashBoard.Rows.Count > 0)
                {
                    foreach (DataRow dr in DTEMDashBoard.Rows)
                    {
                        lstEnquiryDashB.Add(
                           new NonInspectionActivity
                           {
                               Code = Convert.ToString(dr["Code"]),
                               Name = Convert.ToString(dr["Name"]),
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
                DTEMDashBoard.Dispose();
            }
            return lstEnquiryDashB;
        }


    }
}