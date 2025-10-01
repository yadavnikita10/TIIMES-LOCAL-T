using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using TuvVision.Models;
using System.Configuration;
using System.Web.Mvc;

namespace TuvVision.DataAccessLayer
{
    public class DALUserRole
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TuvConnection"].ConnectionString);
        public DataTable GetRoleDashBoard() //User Role DashBoard
        {

            DataTable DTGetRoleDashBoard = new DataTable();
            try
            {
                SqlCommand CMDRoleDashBoard = new SqlCommand("SP_Role", con);
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
        public DataSet GetList()
        {
            DataSet DSGropusAndMenu = new DataSet();
            try
            {
                SqlCommand CMDGroups = new SqlCommand("SP_Role", con);
                CMDGroups.CommandType = CommandType.StoredProcedure;
                CMDGroups.CommandTimeout = 100000;
                CMDGroups.Parameters.AddWithValue("@SP_Type", 4);
                SqlDataAdapter SDAGroups = new SqlDataAdapter(CMDGroups);
                SDAGroups.Fill(DSGropusAndMenu);

            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DSGropusAndMenu.Dispose();
            }
            return DSGropusAndMenu;
        }
        //public int RoleCreatedUpdate(UserRoles UR, string MenuID, string SubMenuID)
        //{
        //    int Result = 0;
        //    con.Open();
        //    try
        //    {
        //        if (UR.UserRoleID != null)
        //        {
        //            if (MenuID != null || MenuID != "")
        //            {
        //                SqlCommand CMDRoleUpdate = new SqlCommand("SP_Role", con);
        //                CMDRoleUpdate.CommandType = CommandType.StoredProcedure;
        //                CMDRoleUpdate.CommandTimeout = 100000;
        //                CMDRoleUpdate.Parameters.AddWithValue("@SP_Type", 2);
        //                CMDRoleUpdate.Parameters.AddWithValue("@UserRoleID", UR.UserRoleID);
        //                CMDRoleUpdate.Parameters.AddWithValue("@FK_MenuName", MenuID);
        //                CMDRoleUpdate.Parameters.AddWithValue("@FK_SUB_MenuNames", SubMenuID);
        //                CMDRoleUpdate.Parameters.AddWithValue("@RoleName", UR.RoleName);
        //                CMDRoleUpdate.Parameters.AddWithValue("@Description", UR.Description);
        //                CMDRoleUpdate.Parameters.AddWithValue("@ModifiedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
        //                Result = CMDRoleUpdate.ExecuteNonQuery();
        //                if (Result != 0)
        //                {
        //                    return Result;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (MenuID != null || MenuID != "")
        //            {
        //                SqlCommand CMDRoleSave = new SqlCommand("SP_Role", con);
        //                CMDRoleSave.CommandType = CommandType.StoredProcedure;
        //                CMDRoleSave.CommandTimeout = 100000;
        //                CMDRoleSave.Parameters.AddWithValue("@SP_Type", 2);
        //                CMDRoleSave.Parameters.AddWithValue("@RoleName", UR.RoleName);
        //                CMDRoleSave.Parameters.AddWithValue("@Description", UR.Description);
        //                CMDRoleSave.Parameters.AddWithValue("@FK_MenuName", MenuID);
        //                CMDRoleSave.Parameters.AddWithValue("@FK_SUB_MenuNames", SubMenuID);
        //                CMDRoleSave.Parameters.AddWithValue("@CreatedBy", System.Web.HttpContext.Current.Session["UserIDs"]);
        //                Result = CMDRoleSave.ExecuteNonQuery();
        //            }
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

        //**********************************************************************************************************************************************
        public int RoleCreatedUpdate(UserRoles UR)
        {
            int Result = 0;
            con.Open();
            try
            {
                if (UR.UserRoleID != 0 && UR.UserRoleID != null)
                {
                    if (UR.MainMenuIds != null || UR.MainMenuIds != "")
                    {
                        SqlCommand CMDRoleUpdate = new SqlCommand("SP_Role", con);
                        CMDRoleUpdate.CommandType = CommandType.StoredProcedure;
                        CMDRoleUpdate.CommandTimeout = 100000;
                        CMDRoleUpdate.Parameters.AddWithValue("@SP_Type", 2);
                        CMDRoleUpdate.Parameters.AddWithValue("@UserRoleID", UR.UserRoleID);
                        CMDRoleUpdate.Parameters.AddWithValue("@FK_MenuName", UR.MainMenuIds);
                        CMDRoleUpdate.Parameters.AddWithValue("@FK_SUB_MenuNames", UR.SubMenuIds);
                        CMDRoleUpdate.Parameters.AddWithValue("@FK_Child_MenuNames", UR.SubMenuChildIds);
                        CMDRoleUpdate.Parameters.AddWithValue("@RoleName", UR.RoleName.Trim());
                        CMDRoleUpdate.Parameters.AddWithValue("@Description", UR.Description.Trim());
                        CMDRoleUpdate.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                        Result = CMDRoleUpdate.ExecuteNonQuery();
                        if (Result != 0)
                        {
                            return Result;
                        }
                    }
                }
                else
                {
                    if (UR.MainMenuIds != null || UR.MainMenuIds != "")
                    {
                        SqlCommand CMDRoleSave = new SqlCommand("SP_Role", con);
                        CMDRoleSave.CommandType = CommandType.StoredProcedure;
                        CMDRoleSave.CommandTimeout = 100000;
                        CMDRoleSave.Parameters.AddWithValue("@SP_Type", 2);
                        CMDRoleSave.Parameters.AddWithValue("@RoleName", UR.RoleName);
                        CMDRoleSave.Parameters.AddWithValue("@Description", UR.Description);
                        CMDRoleSave.Parameters.AddWithValue("@FK_MenuName", UR.MainMenuIds);
                        CMDRoleSave.Parameters.AddWithValue("@FK_SUB_MenuNames", UR.SubMenuIds);
                        CMDRoleSave.Parameters.AddWithValue("@FK_Child_MenuNames", UR.SubMenuChildIds);
                        CMDRoleSave.Parameters.AddWithValue("@CreatedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                        Result = CMDRoleSave.ExecuteNonQuery();
                    }
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
        //************************************************************************************************************************************************
        public DataTable GetRoleDtls(int? RoleID)
        {
            //DataTable DTEditRoleDtls = new DataTable();
            //try
            //{
                DataTable DTEditRoleDtls = new DataTable();
                try
                {
                    SqlCommand CMDEditRole = new SqlCommand("SP_Role", con);
                    CMDEditRole.CommandType = CommandType.StoredProcedure;
                    CMDEditRole.CommandTimeout = 100000;
                    CMDEditRole.Parameters.AddWithValue("@SP_Type", 5);
                    CMDEditRole.Parameters.AddWithValue("@UserRoleID", RoleID);
                    CMDEditRole.Parameters.AddWithValue("@LoginID", System.Web.HttpContext.Current.Session["UserIDs"]);
                    SqlDataAdapter SDAEditRole = new SqlDataAdapter(CMDEditRole);
                    SDAEditRole.Fill(DTEditRoleDtls);
                }
                //SqlCommand CMDEditRole = new SqlCommand("SP_Role", con);
                //CMDEditRole.CommandType = CommandType.StoredProcedure;
                //CMDEditRole.CommandTimeout = 100000;
                //CMDEditRole.Parameters.AddWithValue("@SP_Type", 5);
                //CMDEditRole.Parameters.AddWithValue("@UserRoleID", RoleID);
                //var Session = Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]);
                ////CMDEditRole.Parameters.AddWithValue("@LoginID", System.Web.HttpContext.Current.Session["UserIDs"]);
                //CMDEditRole.Parameters.AddWithValue("@LoginID", Session);
                //SqlDataAdapter SDAEditRole = new SqlDataAdapter(CMDEditRole);
                //SDAEditRole.Fill(DTEditRoleDtls);
            //}
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                DTEditRoleDtls.Dispose();
            }
            return DTEditRoleDtls;
        }
        public int DeleteUser(int RoleID)
        {
            int Result = 0;
            con.Open();
            try
            {
                SqlCommand CMDRoleDelete = new SqlCommand("SP_Role", con);
                CMDRoleDelete.CommandType = CommandType.StoredProcedure;
                CMDRoleDelete.CommandTimeout = 100000;
                CMDRoleDelete.Parameters.AddWithValue("@SP_Type", 3);
                CMDRoleDelete.Parameters.AddWithValue("@UserRoleID", RoleID);
                CMDRoleDelete.Parameters.AddWithValue("@ModifiedBy", Convert.ToString(System.Web.HttpContext.Current.Session["UserIDs"]));
                Result = CMDRoleDelete.ExecuteNonQuery();
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
        public List<UserRoles> GetMenuRights()//Checking User MenuRights
        {
            DataTable DTMenuRghtDtl = new DataTable();
            List<UserRoles> lstMenuLst = new List<UserRoles>();
            try
            {
                SqlCommand CMDMenuRghtDtl = new SqlCommand("SP_UserLogin", con);
                CMDMenuRghtDtl.CommandType = CommandType.StoredProcedure;
                CMDMenuRghtDtl.Parameters.AddWithValue("@SP_Type", 6);
                //CMDMenuRghtDtl.Parameters.AddWithValue("@LoginID", Convert.ToString(System.Web.HttpContext.Current.Session["UserLoginID"])); // Commented By Sagar panigrahi
                //CMDMenuRghtDtl.Parameters.AddWithValue("@RoleName", Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"])); // Commented By Sagar panigrahi
                SqlDataAdapter SDAMenuRgtDtl = new SqlDataAdapter(CMDMenuRghtDtl);
                SDAMenuRgtDtl.Fill(DTMenuRghtDtl);
                if (DTMenuRghtDtl.Rows.Count > 0)
                {
                    lstMenuLst = (from n in DTMenuRghtDtl.AsEnumerable()
                                  select new UserRoles
                                  {
                                      MenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_MainMenuID"])),
                                      MenuNames = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["MenuName"])),
                                      SubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_SubID"])),
                                      SubMenuName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubMenuName"])),
                                      FK_MainMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["FK_MainMenuID"])),
                                      SubSubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["PK_SubSubMenuID"])),
                                      SubSubMenuName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["SubSubMenuName"])),
                                      FK_SubMenuID = Convert.ToInt32(n.Field<Int32>(DTMenuRghtDtl.Columns["FK_SubMenuID"])),
                                      //UrlName = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["UrlName"])), // Commented By Sagar panigrahi
                                      //Action = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["Action"])), // Commented By Sagar panigrahi
                                      //Controller = Convert.ToString(n.Field<string>(DTMenuRghtDtl.Columns["Controller"])), // Commented By Sagar panigrahi
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
        public List<UserRoles> MainMenuList()
        {
            DataTable DTMainMenu = new DataTable();
            List<UserRoles> lstMainMenu = new List<UserRoles>();

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
                                   select new UserRoles
                                   {
                                       MenuID = Convert.ToInt32(n.Field<Int32>(DTMainMenu.Columns["PK_MainMenuID"])),
                                       MenuNames = Convert.ToString(n.Field<string>(DTMainMenu.Columns["MenuName"])),
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

        public DataTable InsertDataToUsercreation()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_InsertBulkDataUsercreation", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);
                CMDGetDdlLst.Parameters.AddWithValue("@Sp_Type", "1");
                SDAGetDdlLst.Fill(dt);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetInsertedData()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand CMDGetDdlLst = new SqlCommand("Sp_InsertBulkDataUsercreation", con);
                CMDGetDdlLst.CommandType = CommandType.StoredProcedure;
                CMDGetDdlLst.Parameters.AddWithValue("@Sp_Type", "2");
                SqlDataAdapter SDAGetDdlLst = new SqlDataAdapter(CMDGetDdlLst);

                SDAGetDdlLst.Fill(dt);
            }
            catch (Exception ex)
            {
                string Error = ex.Message.ToString();
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
        public DataTable GetPKUserID(string Id)
        {
            DataTable DTEditUsers = new DataTable();
            try
            {
                SqlCommand CMDEditUsers = new SqlCommand("Sp_InsertBulkDataUsercreation", con);
                CMDEditUsers.CommandType = CommandType.StoredProcedure;
                CMDEditUsers.Parameters.AddWithValue("@SP_Type", "3");
                CMDEditUsers.Parameters.AddWithValue("@pk_userid", Id);
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
    }
}