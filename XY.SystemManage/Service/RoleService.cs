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
using XY.DataCache.Redis;
namespace XY.SystemManage.Service
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/10/17 16:45:23
    /// 版本：v1.0.0
    /// 描述：角色服务实现类
    /// </summary> 
    public class RoleService : IRoleService
    {

        private bool result = false;
        #region 构造注入
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public RoleService(IXYDbContext dbContext,IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<RoleDto> GetAll()
        {
            var DataResult = new List<RoleDto>();
            //using (var db = _dbContext.GetIntance())
            //{
            //    DataResult = db.Queryable<RoleEntity, OrganizeEntity>((re, oe) => new object[] {
            //        JoinType.Left,re.OrganizeId == oe.OrganizeId
            //    }).Where((re, oe) => re.DeleteMark == 1 && oe.DeleteMark == 1)
            //    .OrderBy((re) => re.SortCode)
            //    .Select((re, oe) => new RoleDto
            //    {
            //        RoleName = re.RoleName,
            //        RoleId = re.RoleId,
            //        Category = re.Category,
            //        Remark = re.Remark,
            //        SortCode = re.SortCode
            //    }).ToList();
            //}
            return DataResult;
        }
        /// <summary>
        /// 根据条件获取角色列表
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyWord">搜索值</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public List<RoleDto> GetPageListByCondition(string rolename, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<RoleDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<RoleEntity>().Where(re => re.DeleteMark == 1)
                    .WhereIF(!string.IsNullOrEmpty(rolename),re => re.RoleName.Contains(rolename))
                        .OrderBy((re) => re.SortCode)
                        .Select(re => new RoleDto
                        {
                            RoleName = re.RoleName,
                            RoleId = re.RoleId,
                            Category = re.Category,
                            Remark = re.Remark,
                            SortCode = re.SortCode,
                            RoleCode = re.RoleCode,
                            DeleteMark = re.DeleteMark,
                            CreateDate = re.CreateDate,
                            CreateUserId = re.CreateUserId,
                            CreateUserName = re.CreateUserName,
                            ModifyDate = re.ModifyDate,
                            ModifyUserId = re.ModifyUserId,
                            ModifyUserName = re.ModifyUserName
                        }).ToPageList(page, limit, ref totalCount).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleDto GetEntityById(string keyValue)
        {
            var DataResult = new RoleDto();
            //using (var db = _dbContext.GetIntance())
            //{
            //    DataResult = db.Queryable<RoleEntity, OrganizeEntity>((re, oe) => new object[] {
            //        JoinType.Left,re.OrganizeId == oe.OrganizeId
            //    }).Where((re, oe) => re.DeleteMark == 1 && oe.DeleteMark == 1 && re.RoleId == keyValue)
            //   .OrderBy((re) => re.SortCode)
            //   .Select((re, oe) => new RoleDto
            //   {
            //       RoleName = re.RoleName,
            //       RoleId = re.RoleId,
            //       Category = re.Category,
            //       Remark = re.Remark,
            //       SortCode = re.SortCode
            //   }).ToList().SingleOrDefault();
            //}
            return DataResult;
        }
        /// <summary>
        /// 删除校验
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool CheckIsAllocateUser(string keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    //获取角色下使用的用户
                    var resultUserRole = db.Queryable<UserRoleEntity>().Where(it => it.RoleId == keyValues).ToList();
                    return resultUserRole.Count() == 0 ? true : false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除校验
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool CheckIsAllocateUserBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    //获取角色下使用的用户
                    var resultUserRole = db.Queryable<UserRoleEntity>().Where(it => keyValues.Contains(it.RoleId)).ToList();
                    return resultUserRole.Count() == 0 ? true : false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 校验角色名称是否重复
        /// </summary>
        /// <param name="keyValue">角色名称</param>
        /// /// <param name="keyValue">角色Id</param>
        /// <returns></returns>
        public bool CheckIsRoleNameRepeat(string roleName, string roleId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    reslut = db.Queryable<RoleEntity>().Any(it => it.RoleName == roleName && it.DeleteMark == 1);
                }
                else
                {
                    reslut = db.Queryable<RoleEntity>().Any(it => it.RoleName == roleName && it.RoleId != roleId && it.DeleteMark == 1);
                }
            }
            return reslut;
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public bool Insert(RoleEntity roleEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(roleEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public bool Update(RoleEntity roleEntity)
        {

            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(roleEntity)
                .IgnoreColumns(it => new { it.CreateDate, it.CreateUserId, it.CreateUserName, it.DeleteMark })
                .Where(it => it.RoleId == roleEntity.RoleId)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        public bool Delete(string keyValue, string UserId, string UserName)
        {
            using (var db = _dbContext.GetIntance())
            {
                var roleEntity = new RoleEntity();                
                roleEntity.DeleteMark = 0;
                roleEntity.ModifyUserId = UserId;
                roleEntity.ModifyUserName = UserName;
                roleEntity.ModifyDate = DateTime.Now;
                //逻辑删除
                var count = db.Updateable(roleEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                .Where(it => it.RoleId == keyValue)
                .ExecuteCommand();
                //物理删除
                //var count = db.Deleteable<RoleEntity>().Where(it => it.RoleId == keyValue).ExecuteCommand();
                return count == 1 ? true : false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> keyValues, string UserId, string UserName)
        {
            bool result = false;
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var roleEntity = new RoleEntity();
                    roleEntity.DeleteMark = 0;
                    roleEntity.ModifyUserId = UserId;
                    roleEntity.ModifyUserName = UserName;
                    roleEntity.ModifyDate = DateTime.Now;
                    //逻辑删除
                    var counts = db.Updateable(roleEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                        .Where(it => keyValues.Contains(it.RoleId)).ExecuteCommand();
                    //物理删除
                    //var counts = db.Deleteable<RoleEntity>().Where(it => keyValues.Contains(it.RoleId)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        #endregion


    }
}