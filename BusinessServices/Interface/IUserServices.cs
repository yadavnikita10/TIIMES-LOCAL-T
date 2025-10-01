using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessServices
{
    /// <summary>
    /// IUserServices Service
    /// </summary>
    public interface IUserServices
    {
        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        UserEntity GetUserById(int id);

        /// <summary>
        /// Gets all users by status.
        /// </summary>
        /// <param name="statusIds">The status ids.</param>
        /// <param name="isClient">if set to <c>true</c> [is client].</param>
        /// <returns></returns>
        IEnumerable<UserEntity> GetAllUsersByStatus(string statusIds, bool isClient);

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        int CreateUser(UserEntity model);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        bool UpdateUser(int id, UserEntity model);

        /// <summary>
        /// Updates the user status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="modifiedDate">The modified date.</param>
        /// <returns></returns>
        bool UpdateUserStatus(int id, int status, DateTime modifiedDate);

        /// <summary>
        /// Gets the users by roles.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <param name="statusIds">The status ids.</param>
        /// <returns></returns>
        IEnumerable<UserEntity> GetUsersByRoles(string roleIds, string statusIds);

        /// <summary>
        /// Gets the name of the user profile by user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        UserEntity GetUserProfileByUserName(string userName);

        /// <summary>
        /// Gets the menus by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="statusIds">The status ids.</param>
        /// <returns></returns>
        IEnumerable<SubMenuEntity> GetMenusByRoleId(string roleId, string statusIds);
    }
}