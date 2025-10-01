using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace TuvVision.Models
{
    public class UserRoles
    {
        public UserRoles()
        {
            this.MenusList = new List<MainMenuList>();
            this.lstMenuList = new List<MainMenuList>();
        }
        public int UserRoleID { get; set; }
        public string FK_MenuName { get; set; }
        public string[] MenuName { get; set; }
        public string[] SubMenuNames { get; set; }
        public string FK_SUB_MenuNames { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed.")]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public static List<UserRoles> lstUserRole { get; set; }
        public List<MainMenuList> lstMenuList { get; set; } // Added By Sagar
        List<NameCode> lstMenu = new List<NameCode>();
        List<NameCode> lstSubMenu = new List<NameCode>();
        public List<SubMenuGroup> SubMenuGrp { get; set; }
        public List<MenuGroup> MenusGrp { get; set; }
        public List<UserRoles> lstMenuRoles { get; set; }
        //**********************************************************************
        public List<MainMenuList> MenusList { get; set; }

        public string MainMenuIds { get; set; }
        public string SubMenuIds { get; set; }
        public string SubMenuChildIds { get; set; }
        public int Pk_RightId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        //******************************************************************

        public int? MenuID { get; set; }
        public string MenuNames { get; set; }
        public int? SubMenuID { get; set; }
        public string SubMenuName { get; set; }
        public int? SubSubMenuID { get; set; }
        public string SubSubMenuName { get; set; }
        public string UrlName { get; set; }
        public int? ChildMenuID { get; set; }
        public int? PK_Menu_ID { get; set; }
        public int? FK_Menu_ID { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string ChiledMenu { get; set; }
        //public List<SubMenuListRole> SubMenuList { get; set; }
        public int FK_MainMenuID { get; set; }
        public int FK_SubMenuID { get; set; }

    }
    public class MenuGroup
    {
        public string Text { get; set; }
        public Int32 Value { get; set; }
        public bool Selected { get; set; }
        public List<MenuGroup> MenuGrp { get; set; }
    }
    public class SubMenuGroup
    {
        public string Text { get; set; }
        public Int32 Value { get; set; }
        public bool Selected { get; set; }
        public List<SubMenuGroup> SubMenuGrp { get; set; }
    }

    //public class MenuRightsRoles
    //{
    //    public int? MenuID { get; set; }
    //    public string MenuName { get; set; }
    //    public int? SubMenuID { get; set; }
    //    public string SubMenuName { get; set; }
    //    public int? SubSubMenuID { get; set; }
    //    public string SubSubMenuName { get; set; }
    //    public string UrlName { get; set; }
    //    public int? ChildMenuID { get; set; }
    //    public int? PK_Menu_ID { get; set; }
    //    public int? FK_Menu_ID { get; set; }
    //    public string Action { get; set; }
    //    public string Controller { get; set; }
    //    public string ChiledMenu { get; set; }
    //    public List<SubMenuListRole> SubMenuList { get; set; }
    //    public int FK_MainMenuID { get; set; }
    //    public int FK_SubMenuID { get; set; }
    //}
    //public class MenuListDTORole
    //{
    //    public MenuListDTORole()
    //    {
    //        this.SubMenuList = new List<Models.SubMenuListRole>();
    //    }
    //    public string MainMenuName { get; set; }
    //    public int MainMenuId { get; set; }
    //    public List<SubMenuListRole> SubMenuList { get; set; }

    //}
    //public class KeyValuePair
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Action { get; set; }
    //    public string Controller { get; set; }
    //}
    //public class SubMenuListRole
    //{
    //    public SubMenuListRole()
    //    {
    //        this.SubSubmenuList = new List<SubSubMenuListRole>();
    //    }
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Action { get; set; }
    //    public string Controller { get; set; }
    //    public List<SubSubMenuListRole> SubSubmenuList { get; set; }
    //}
    //public class SubSubMenuListRole
    //{
    //    public SubSubMenuListRole()
    //    {
    //        this.ChildMenu = new List<ChildMenuListRole>();
    //    }
    //    public List<ChildMenuListRole> ChildMenu { get; set; }
    //    public int SubSubMenuId { get; set; }
    //    public string SubSubMenuName { get; set; }
    //    public string Action { get; set; }
    //    public string Controller { get; set; }
    //}
    //public class ChildMenuListRole
    //{
    //    public int ChildId { get; set; }
    //    public string ChildName { get; set; }
    //    public string Action { get; set; }
    //    public string Controller { get; set; }
    //}
    //Sagar Model Code************************************************************************
    //public class DepartmentDetailsVM
    //{
    //    public DepartmentDetailsVM()
    //    {
    //        this.MenusList = new List<MainMenuList>();
    //    }
    //    public int Pk_Id { get; set; }
    //    [Required]
    //    public string Name { get; set; }
    //    [Required]
    //    public string Menu_Accessible { get; set; }
    //    public List<MainMenuList> MenusList { get; set; }
    //    public DateTime Created_On { get; set; }
    //    public int Created_By { get; set; }
    //    public string MainMenuIds { get; set; }
    //    public string SubMenuIds { get; set; }
    //    public string SubMenuChildIds { get; set; }
    //    public int Pk_RightId { get; set; }
    //}

    public class MainMenuList
    {
        public MainMenuList()
        {
            this.SubMenuList = new List<SubMenuLists>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubMenuLists> SubMenuList { get; set; }
        public bool IsChecked { get; set; }
    }
    public class SubMenuLists
    {
        public SubMenuLists()
        {
            this.ChildMenuList = new List<SubMenuChildList>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool MakeAppear { get; set; }
        public List<SubMenuChildList> ChildMenuList { get; set; }
        public bool IsChecked { get; set; }
    }
    public class SubMenuChildList
    {
        public SubMenuChildList()
        {
            this.SubChildMenuList = new List<SubSubMenuChildList>();
        }
        public int Id { get; set; }
        public string Child { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<SubSubMenuChildList> SubChildMenuList { get; set; }
        public bool IsChecked { get; set; }
    }
    public class SubSubMenuChildList
    {
        public int Id { get; set; }
        public string SubChild { get; set; }
    }
}