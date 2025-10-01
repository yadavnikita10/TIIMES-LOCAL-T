using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using System;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuvVision.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
       
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public Nullable<int> U_Role_ID { get; set; }

        public string Created_By { get; set; }
        public Nullable<int> Status { get; set; }
        public string Modified_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Qualification { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public Nullable<System.DateTime> DateOf_Joining { get; set; }
        public Nullable<System.DateTime> RelievingDate { get; set; }
        public string Password { get; set; }
        public string Branch { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
       // public string Signature { get; set; }
        public string EmployeeCode { get; set; }
        public string Designation { get; set; }
        public string EmployeeGrade { get; set; }
        public string OfficeNo { get; set; }
        public string ResiNo { get; set; }
        public string MobileNo { get; set; }
        public string SAP_Vandor_code { get; set; }
        public string Active { get; set; }
        public string Language_Spoken { get; set; }
        public string Employment_Type { get; set; }
        public string Reporting_To { get; set; }
        public string DeleteStatus { get; set; }
        public string Service_Type { get; set; }
        public string Signature { get; internal set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}