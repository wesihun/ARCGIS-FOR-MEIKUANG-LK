using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    /// 描述：角色服务接口类
    public interface IRoleService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        List<RoleDto> GetAll();
        /// <summary>
        /// 根据条件获取角色列表并分页
        /// </summary>
        /// <param name="rolename"></param>
        /// <param name="page">页码</param>
        /// <param name="limit">也尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<RoleDto> GetPageListByCondition(string rolename, int page, int limit,ref int totalCount);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RoleDto GetEntityById(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool Insert(RoleEntity roleEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool Update(RoleEntity roleEntity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        bool Delete(string keyValue, string UserId, string UserName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        bool DeleteBatch(List<string> keyValues, string UserId, string UserName);

        /// <summary>
        /// 删除校验
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        bool CheckIsAllocateUser(string keyValue);
        /// <summary>
        /// 批量删除校验
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        bool CheckIsAllocateUserBatch(List<string> keyValues);
        /// <summary>
        /// 校验角色名称是否重复
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        bool CheckIsRoleNameRepeat(string roleName, string roleId);
        #endregion
    }
}
