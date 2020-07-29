using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    public interface IUserRoleService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        List<UserRoleDto> GetUserByRoleId(string roleId);
        /// <summary>
        /// 根据角色ID获取用户list
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="depId">机构ID</param>
        /// <returns></returns>
        List<UserRoleDto> GetUserList(string roleId, string depId);

        /// <summary>
        /// 根据角色ID获取用户list
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        List<UserRoleDto> GetUserIdListByRoleId(string roleId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="userRoleEntity"></param>
        /// <returns></returns>
        bool InsertBatch(List<UserRoleEntity> userRoleEntity);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        bool DeleteBatch(string roleId);
        #endregion
    }
}
