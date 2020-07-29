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
    public class ModuleService : IModuleService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public ModuleService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region 获取数据
        /// <summary>
        /// 功能实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        public ModuleDto GetEntityById(string keyValue)
        {
            var DataResult = new ModuleDto();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<ModuleEntity>().Where(it => it.ModuleId == keyValue && it.DeleteMark == 1).OrderBy(it => it.SortCode)
                             .Select(it => new ModuleDto
                             {
                                 ModuleId = it.ModuleId,
                                 ParentId = it.ParentId,
                                 ModuleCode = it.ModuleCode,
                                 ModuleName = it.ModuleName,
                                 Icon = it.Icon,
                                 Url = it.Url,
                                 Remark = it.Remark,
                                 SortCode = it.SortCode,
                                 DeleteMark = it.DeleteMark,
                                 CreateDate=it.CreateDate,
                                 CreateUserId=it.CreateUserId,
                                 CreateUserName=it.CreateUserName,
                                 ModifyDate=it.ModifyDate,
                                 ModifyUserId=it.ModifyUserId,
                                 ModifyUserName=it.ModifyUserName
                             }).ToList().SingleOrDefault();
            }
            return DataResult;
        }
        /// <summary>
        /// 功能列表
        /// </summary>
        /// <returns></returns>
        public List<ModuleDto> GetList()
        {
            using (var db = _dbContext.GetIntance())
            {
                var DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                {
                    ModuleId = it.ModuleId,
                    ParentId = it.ParentId,
                    ModuleCode = it.ModuleCode,
                    ModuleName = it.ModuleName,
                    Icon = it.Icon,
                    Url = it.Url,
                    Remark = it.Remark,
                    SortCode = it.SortCode,
                    DeleteMark = it.DeleteMark,
                    CreateDate = it.CreateDate,
                    CreateUserId = it.CreateUserId,
                    CreateUserName = it.CreateUserName,
                    ModifyDate = it.ModifyDate,
                    ModifyUserId = it.ModifyUserId,
                    ModifyUserName = it.ModifyUserName
                }).ToList();
                if (DataResult.Count() > 0)
                {
                    return DataResult;
                }
            }
            return null;
        }

        public List<ModuleDto> GetPageListByCondition(string condition, string keyword, string parentId, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<ModuleDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(parentId))
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                    {
                        switch (condition)
                        {
                            case "ModuleCode":
                                DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE && it.ParentId == parentId
                                && it.ModuleCode.Contains(keyword)
                                ).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                                {
                                    ModuleId = it.ModuleId,
                                    ParentId = it.ParentId,
                                    ModuleCode = it.ModuleCode,
                                    ModuleName = it.ModuleName,
                                    Icon = it.Icon,
                                    Url = it.Url,
                                    Remark = it.Remark,
                                    SortCode = it.SortCode,
                                    DeleteMark = it.DeleteMark,
                                    CreateDate = it.CreateDate,
                                    CreateUserId = it.CreateUserId,
                                    CreateUserName = it.CreateUserName,
                                    ModifyDate = it.ModifyDate,
                                    ModifyUserId = it.ModifyUserId,
                                    ModifyUserName = it.ModifyUserName
                                }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ModuleName":
                                DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE && it.ParentId == parentId
                               && it.ModuleName.Contains(keyword)
                               ).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                               {
                                   ModuleId = it.ModuleId,
                                   ParentId = it.ParentId,
                                   ModuleCode = it.ModuleCode,
                                   ModuleName = it.ModuleName,
                                   Icon = it.Icon,
                                   Url = it.Url,
                                   Remark = it.Remark,
                                   SortCode = it.SortCode,
                                   DeleteMark = it.DeleteMark,
                                   CreateDate = it.CreateDate,
                                   CreateUserId = it.CreateUserId,
                                   CreateUserName = it.CreateUserName,
                                   ModifyDate = it.ModifyDate,
                                   ModifyUserId = it.ModifyUserId,
                                   ModifyUserName = it.ModifyUserName
                               }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }

                    }
                    else
                    {
                        DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE && it.ParentId == parentId).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                        {
                            ModuleId = it.ModuleId,
                            ParentId = it.ParentId,
                            ModuleCode = it.ModuleCode,
                            ModuleName = it.ModuleName,
                            Icon = it.Icon,
                            Url = it.Url,
                            Remark = it.Remark,
                            SortCode = it.SortCode,
                            DeleteMark = it.DeleteMark,
                            CreateDate = it.CreateDate,
                            CreateUserId = it.CreateUserId,
                            CreateUserName = it.CreateUserName,
                            ModifyDate = it.ModifyDate,
                            ModifyUserId = it.ModifyUserId,
                            ModifyUserName = it.ModifyUserName
                        }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                    {
                        switch (condition)
                        {
                            case "ModuleCode":
                                DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE
                                && it.ModuleCode.Contains(keyword)
                                ).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                                {
                                    ModuleId = it.ModuleId,
                                    ParentId = it.ParentId,
                                    ModuleCode = it.ModuleCode,
                                    ModuleName = it.ModuleName,
                                    Icon = it.Icon,
                                    Url = it.Url,
                                    Remark = it.Remark,
                                    SortCode = it.SortCode,
                                    DeleteMark = it.DeleteMark,
                                    CreateDate = it.CreateDate,
                                    CreateUserId = it.CreateUserId,
                                    CreateUserName = it.CreateUserName,
                                    ModifyDate = it.ModifyDate,
                                    ModifyUserId = it.ModifyUserId,
                                    ModifyUserName = it.ModifyUserName
                                }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ModuleName":
                                DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE 
                               && it.ModuleName.Contains(keyword)
                               ).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                               {
                                   ModuleId = it.ModuleId,
                                   ParentId = it.ParentId,
                                   ModuleCode = it.ModuleCode,
                                   ModuleName = it.ModuleName,
                                   Icon = it.Icon,
                                   Url = it.Url,
                                   Remark = it.Remark,
                                   SortCode = it.SortCode,
                                   DeleteMark = it.DeleteMark,
                                   CreateDate = it.CreateDate,
                                   CreateUserId = it.CreateUserId,
                                   CreateUserName = it.CreateUserName,
                                   ModifyDate = it.ModifyDate,
                                   ModifyUserId = it.ModifyUserId,
                                   ModifyUserName = it.ModifyUserName
                               }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }

                    }
                    else
                    {
                        DataResult = db.Queryable<ModuleEntity>().Where(it => it.DeleteMark == 1 && it.ModuleId != DataDictConst.SYSTEM_MODULE ).OrderBy(it => it.SortCode).Select(it => new ModuleDto
                        {
                            ModuleId = it.ModuleId,
                            ParentId = it.ParentId,
                            ModuleCode = it.ModuleCode,
                            ModuleName = it.ModuleName,
                            Icon = it.Icon,
                            Url = it.Url,
                            Remark = it.Remark,
                            SortCode = it.SortCode,
                            DeleteMark = it.DeleteMark,
                            CreateDate = it.CreateDate,
                            CreateUserId = it.CreateUserId,
                            CreateUserName = it.CreateUserName,
                            ModifyDate = it.ModifyDate,
                            ModifyUserId = it.ModifyUserId,
                            ModifyUserName = it.ModifyUserName
                        }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }
            }
            return DataResult;
        }    

        public List<ModuleDto> GetUserModuleList(string userId)
        {
            var DataResult = new List<ModuleDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserRoleEntity, RoleModuleEntity, ModuleEntity>((ur, rm, me) => new object[] {
                    JoinType.Left,ur.RoleId == rm.RoleId,
                    JoinType.Left,rm.ModuleId == me.ModuleId,
                }).Where((ur, rm, me) => me.DeleteMark == 1 && me.ModuleId != DataDictConst.SYSTEM_MODULE && ur.UserId == userId && me.ModuleName != "系统功能" && me.ModuleName != "机构管理" && me.ModuleName != "区划管理").OrderBy((ur, rm, me) => me.SortCode)
                .Select((ur, rm, me) => new ModuleDto
                {
                    ModuleId = me.ModuleId,
                    ParentId = me.ParentId,
                    ModuleCode = me.ModuleCode,
                    ModuleName = me.ModuleName,
                    Icon = me.Icon,
                    Url = me.Url,
                    Remark = me.Remark,
                    SortCode = me.SortCode,
                    DeleteMark = me.DeleteMark,
                    CreateDate = me.CreateDate,
                    CreateUserId = me.CreateUserId,
                    CreateUserName = me.CreateUserName,
                    ModifyDate = me.ModifyDate,
                    ModifyUserId = me.ModifyUserId,
                    ModifyUserName = me.ModifyUserName
                }).ToList();
            }
            return DataResult;
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 功能编号不能重复
        /// </summary>
        /// <param name="modelCode">编号</param>
        /// <param name="keyValue">主键</param>
        public bool ExistEnCode(string modelCode, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                var list = db.Queryable<ModuleEntity>()
                    .Where(it => it.ModuleCode == modelCode).ToList();
                if (!string.IsNullOrEmpty(keyValue))
                {
                    list = list.Where(it => it.ModuleId != keyValue).ToList();

                }
                return list.Count() == 0 ? true : false;
            }
        }
        /// <summary>
        /// 功能名称不能重复
        /// </summary>
        /// <param name="modelName">名称</param>
        /// <param name="keyValue">主键</param>
        public bool ExistFullName(string modelName, string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                var list = db.Queryable<ModuleEntity>()
                    .Where(it => it.ModuleName == modelName).ToList();
                if (!string.IsNullOrEmpty(keyValue))
                {
                    list = list.Where(it => it.ModuleId != keyValue).ToList();

                }
                return list.Count() == 0 ? true : false;
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool Delete(string keyValue,string UserId, string UserName)
        {
            using (var db = _dbContext.GetIntance())
            {
                var moduleEntity = new ModuleEntity();
                moduleEntity.DeleteMark = 0;
                moduleEntity.ModifyDate = System.DateTime.Now;
                moduleEntity.ModifyUserId = UserId;
                moduleEntity.ModifyUserName = UserName;
                //逻辑删除
                var count = db.Updateable(moduleEntity).UpdateColumns(it => new { it.DeleteMark,it.ModifyUserName,it.ModifyUserId,it.ModifyDate })
                .Where(it => it.ModuleId == keyValue)
                .ExecuteCommand();
                return count > 0 ? true : false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">菜单主键</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> keyValues, string UserId, string UserName)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var moduleEntity = new ModuleEntity();
                    moduleEntity.DeleteMark = 0;
                    moduleEntity.ModifyDate = System.DateTime.Now;
                    moduleEntity.ModifyUserId = UserId;
                    moduleEntity.ModifyUserName = UserName;
                    //逻辑删除
                    var counts = db.Updateable(moduleEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyUserName, it.ModifyUserId, it.ModifyDate })
                        .Where(it => keyValues.Contains(it.ModuleId)).ExecuteCommand();
                    result = counts > 0 ? true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="moduleEntity">功能实体</param>
        public bool Insert(ModuleEntity moduleEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(moduleEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="moduleEntity">功能实体</param>
        /// <returns></returns>
        public bool Update(ModuleEntity moduleEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(moduleEntity)
                    .IgnoreColumns(it => new { it.DeleteMark })
                    .Where(it => it.ModuleId == moduleEntity.ModuleId).ExecuteCommand();
                if (count > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        #endregion
    }
}
