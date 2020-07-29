using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    public interface IUserService
    {
        #region 获取数据
        /// <summary>
        /// 获取用户列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">也尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<UserDto> GePagetListByUser(string orgid,string depid, string realname, string isdadmin, int page,int limit,ref int totalCount);

        List<UserDto> GetUserSelect(string depid,string roleid);
        /// <summary>
        /// 根据用户ID获取用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        UserDto GetEntityById(string keyValue);
        /// <summary>
        /// 根据用户名判断是否已存在
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        bool IsExistByUserName(string userName, string keyValue);
        /// <summary>
        ///根据用户名判断用户是否存在（）返回用户对象
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        UserEntity IsExistByUserName(string userName);
        
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        bool Insert(UserEntity userEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        bool Update(UserEntity userEntity);
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
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        bool RevisePassword(string keyValue, string Password);
        #endregion
    }
}
