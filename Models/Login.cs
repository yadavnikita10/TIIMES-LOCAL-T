using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TuvVision.Models
{
    public class Login
    {
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Invalid User Name...")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        ///changes by Savio S to capture ipaddr of loggedin user.
        public string ipAddr { get; set; }
        ///changes by Savio S to capture ipaddr of loggedin user
        [Required(ErrorMessage = "Invalid Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
    public class MenuRights
    {
        public int? MenuID { get; set; }
        public string MenuName { get; set; }
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
        public List<SubMenuList> SubMenuList { get; set; }
        public int FK_MainMenuID { get; set; }
        public int FK_SubMenuID { get; set; }
        //Added by Sagar Panigrahi 06-Aug-2019
        public string MainMenuController { get; set; }
        public string MainMenuAction { get; set; }
        public string SubMenuController { get; set; }
        public string SubMenuAction { get; set; }
        public string SubSubMenuController { get; set; }
        public string SubSubMenuAction { get; set; }
        public int OrderNo { get; set; }
    }
    public class MenuListDTO
    {
        public MenuListDTO()
        {
            this.SubMenuList = new List<Models.SubMenuList>();
        }
        public string MainMenuName { get; set; }
        public int MainMenuId { get; set; }
        public List<SubMenuList> SubMenuList { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

    }
    public class KeyValuePair
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
    public class SubMenuList
    {
        public SubMenuList()
        {
            this.SubSubmenuList = new List<SubSubMenuList>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public List<SubSubMenuList> SubSubmenuList { get; set; }
    }
    public class SubSubMenuList
    {
        public SubSubMenuList()
        {
            this.ChildMenu = new List<ChildMenuList>();
        }
        public List<ChildMenuList> ChildMenu { get; set; }
        public int SubSubMenuId { get; set; }
        public string SubSubMenuName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
    public class ChildMenuList
    {
        public int ChildId { get; set; }
        public string ChildName { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }

    //public class RoleM_LeftSubCategory
    //{
    //    public int LeftSubCat_Id { get; set; }
    //    public Nullable<int> LeftMenucat_Id { get; set; }
    //    public string SubCat_Name { get; set; }
    //    public string R_Controller { get; set; }
    //    public string R_Action { get; set; }
    //    public string Optional_ID { get; set; }
    //    public Nullable<int> OrderNo { get; set; }
    //    public string Status { get; set; }
    //    public string Menu_Name { get; set; }

    //    public int SSC_Id { get; set; }
    //    public string SSC_Name { get; set; }
    //}
    public class AccessRightsIds
    {
        public string FkMenuId { get; set; }
        public string FkSubMenuId { get; set; }
        public string FkSubSubMenuIds { get; set; }
        
    }
}