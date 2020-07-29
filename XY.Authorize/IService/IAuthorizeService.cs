using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.Authorize.IService
{
    public interface IAuthorizeService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        UserEntity CheckLogin(string userName);
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="userid">用户名</param>
        /// <returns></returns>
        string GetRoleName(string userid);
    }
}
