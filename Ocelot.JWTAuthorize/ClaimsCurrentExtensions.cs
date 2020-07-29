using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Ocelot.JwtAuthorize
{
    public static class ClaimsCurrentExtensions
    {
        /// <summary>
        /// 当前用户ID
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        /// <summary>
        /// 当前用户名
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst(ClaimTypes.Name)?.Value;
        }
        /// <summary>
        /// 当前登录用户真实姓名
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserRealName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("RealName")?.Value;
        }
        /// <summary>
        /// 当前用户机构ID
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserOrganizeId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("OrganizeId")?.Value;
        }
        /// <summary>
        /// 当前用户机构名称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserOrganizeName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("OrganizeName")?.Value;
        }
        /// <summary>
        /// 当前用户部门ID
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserDepId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("DepId")?.Value;
        }
        /// <summary>
        /// 当前用户部门名称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserDepName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("DepName")?.Value;
        }
        /// <summary>
        /// 当前用户部门名称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserRoleName(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("RoleName")?.Value;
        }
        /// <summary>
        /// 当前用户数据权限
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserDataPower(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("DataPower")?.Value;
        }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentUserIsAdmin(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return principal.FindFirst("IsAdmin")?.Value;
        }
    }
}
