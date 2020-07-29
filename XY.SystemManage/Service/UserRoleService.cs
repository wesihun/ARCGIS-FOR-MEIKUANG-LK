using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.Utilities;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;
namespace XY.SystemManage.Service
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/10/22 11:21:23
    /// 版本：v1.0.0
    /// 描述：角色用户关系服务实现类
    /// </summary> 
    public class UserRoleService : IUserRoleService
    {
        #region 构造注入
        private readonly IXYDbContext _dbContext;
        public UserRoleService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public List<UserRoleDto> GetUserByRoleId(string roleId)
        {
            var DataResult = new List<UserRoleDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserRoleEntity>().Where(ur => ur.RoleId == roleId)
                .OrderBy((ur) => ur.RoleId)
                .Select(ur => new UserRoleDto
                {
                    UserId = ur.UserId,
                    CreateUserId=ur.CreateUserId,
                    CreateUserName=ur.CreateUserName,
                    RoleId=ur.RoleId,
                    CRowId=ur.CRowId,
                    CreateDate=ur.CreateDate
                }).ToList();
            }
            return DataResult;
        }
        #endregion

        public List<UserRoleDto> GetUserList(string roleId,string depId)
        {
            var DataResult = new List<UserRoleDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserRoleEntity, UserEntity>((de1, de2) => new object[] {
                JoinType.Left,de1.UserId == de2.UserId
                }).WhereIF(!string.IsNullOrEmpty(roleId),(de1, de2) =>de1.RoleId == roleId)
                .WhereIF(!string.IsNullOrEmpty(depId), (de1, de2) => de2.DepId == depId)
                .Where((de1, de2) => de2.DeleteMark == 1)
                .Select((de1, de2) => new UserRoleDto
                {
                    UserId = de2.UserId,
                    UserName = de2.RealName
                }).ToList();
            }
            return DataResult;
        }
        public List<UserRoleDto> GetUserIdListByRoleId(string roleId)
        {
            var DataResult = new List<UserRoleDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    DataResult = db.Queryable<UserRoleEntity>().Where(ur => ur.RoleId == roleId)
                           .OrderBy((ur) => ur.RoleId)
                           .Select(ur => new UserRoleDto
                           {
                               UserId = ur.UserId
                           }).ToList();
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }

        #region 提交数据
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="userRoleEntity">角色用户实体</param>
        /// <returns></returns>
        public bool InsertBatch(List<UserRoleEntity> userRoleEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Deleteable<UserRoleEntity>().Where(it => it.RoleId == userRoleEntity[0].RoleId).ExecuteCommand();
                    if(userRoleEntity[0].UserId != null)
                    {
                        foreach (var entity in userRoleEntity)
                        {
                            db.Insertable(entity).ExecuteCommand();
                        }
                    }                
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw ex;
                }
                return true;
            }


        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public bool DeleteBatch(string roleId)
        {
            if (roleId.Count() <= 0)
            {
                return false;
            }
            using (var db = _dbContext.GetIntance())
            {
                //物理删除
                var counts = db.Deleteable<UserRoleEntity>().Where(it => it.RoleId == roleId).ExecuteCommand();
                return counts > 0 ? true : false;
            }
        }
        #endregion
    }
}
