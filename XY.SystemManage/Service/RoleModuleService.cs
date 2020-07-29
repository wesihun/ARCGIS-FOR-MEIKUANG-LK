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
    /// 时间：2018/10/22 13:17:23
    /// 版本：v1.0.0
    /// 描述：角色权限关系服务实现类
    /// </summary> 
    public class RoleModuleService : IRoleModuleService
    {
        #region 构造注入
        private readonly IXYDbContext _dbContext;
        public RoleModuleService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<RoleModuleDto> GetModuleByRoleId(string RoleId)
        {
            var DataResult = new List<RoleModuleDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RoleModuleEntity>().Where(rm => rm.RoleId == RoleId)
                .OrderBy(rm => rm.RoleId)
                .Select(rm => new RoleModuleDto
                {
                    ModuleId = rm.ModuleId,
                    CRowId=rm.CRowId,
                    CreateDate=rm.CreateDate,
                    CreateUserId=rm.CreateUserId,
                    CreateUserName=rm.CreateUserName,
                    RoleId=rm.RoleId
                }).ToList();
            }
            return DataResult;
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="roleModuleEntity">角色用户实体</param>
        /// <returns></returns>
        public bool InsertBatch(List<RoleModuleEntity> roleModuleEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    if (roleModuleEntity[0].ModuleId == null)
                    {
                        db.Deleteable<RoleModuleEntity>().Where(it => it.RoleId == roleModuleEntity[0].RoleId).ExecuteCommand();
                    }
                    else
                    {
                        db.Deleteable<RoleModuleEntity>().Where(it => it.RoleId == roleModuleEntity[0].RoleId).ExecuteCommand();
                        foreach (var entity in roleModuleEntity)
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
        /// <param name="RodeId">角色ID</param>
        /// <returns></returns>
        public bool DeleteBatch(string RodeId)
        {
            if (string.IsNullOrEmpty(RodeId))
            {
                return false;
            }
            using (var db = _dbContext.GetIntance())
            {
                //物理删除
                int counts = db.Deleteable<RoleModuleEntity>().Where(it => it.RoleId == RodeId).ExecuteCommand();
                return counts > 0 ? true : false;
            }
        }
        #endregion
    }
}
