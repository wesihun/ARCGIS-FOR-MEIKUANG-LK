using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;
namespace XY.SystemManage.IService
{
   public interface IRoleModuleService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        List<RoleModuleDto> GetModuleByRoleId(string RoleId);

        #endregion

        #region 提交数据
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="roleModuleEntity">实体</param>
        /// <returns></returns>
        bool InsertBatch(List<RoleModuleEntity> roleModuleEntity);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="RodeId">角色ID</param>
        /// <returns></returns>
        bool DeleteBatch(string RodeId);
        #endregion
    }
}