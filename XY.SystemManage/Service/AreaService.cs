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
    public class AreaService : IAreaService
    {
        private bool result = false;

        private readonly IXYDbContext _dbContext;
        public AreaService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region 获取数据
        public List<AreaDto> GetAll()
        {
            using (var db = _dbContext.GetIntance())
            {
                var DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1).Select(it => new AreaDto
                {
                    AreaId = it.AreaId,
                    ParentId = it.ParentId,
                    AreaCode = it.AreaCode,
                    AreaName = it.AreaName,
                    SimpleSpelling = it.SimpleSpelling,
                    Layer = it.Layer,
                    Remark = it.Remark,
                    SortCode=it.SortCode,
                    CreateDate=it.CreateDate,
                    CreateUserId=it.CreateUserId,
                    CreateUserName=it.CreateUserName,
                    ModifyDate=it.ModifyDate,
                    ModifyUserId=it.ModifyUserId,
                    ModifyUserName=it.ModifyUserName,
                    DeleteMark=it.DeleteMark
                }).ToList();
                if (DataResult.Count() > 0)
                {
                    return DataResult;
                }
            }
            return null;
        }
        public AreaDto GetEntity(string keyValue)
        {
            var DataResult = new AreaDto();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<AreaEntity>().Where(it => it.AreaId == keyValue && it.DeleteMark == 1).Select(it => new AreaDto
                {
                    AreaId = it.AreaId,
                    ParentId = it.ParentId,
                    AreaCode = it.AreaCode,
                    AreaName = it.AreaName,
                    SimpleSpelling = it.SimpleSpelling,
                    Layer = it.Layer,
                    Remark = it.Remark,
                    SortCode = it.SortCode,
                    CreateDate = it.CreateDate,
                    CreateUserId = it.CreateUserId,
                    CreateUserName = it.CreateUserName,
                    ModifyDate = it.ModifyDate,
                    ModifyUserId = it.ModifyUserId,
                    ModifyUserName = it.ModifyUserName,
                    DeleteMark = it.DeleteMark
                }).ToList().SingleOrDefault();
            }
            return DataResult;
        }
        public List<AreaDto> GetListByParentId(string ParentId)
        {
            using (var db = _dbContext.GetIntance())
            {
                //模糊查询
                var DataResult = db.Queryable<AreaEntity>().Where(it => it.ParentId.Equals(ParentId) && it.DeleteMark == 1).Select(it => new AreaDto
                {
                    AreaId = it.AreaId,
                    ParentId = it.ParentId,
                    AreaCode = it.AreaCode,
                    AreaName = it.AreaName,
                    SimpleSpelling = it.SimpleSpelling,
                    Layer = it.Layer,
                    Remark = it.Remark
                }).ToList();
                return DataResult;
            }
          
        }
        public List<AreaDto> GetListByAreaId(string areaid)
        {
            using (var db = _dbContext.GetIntance())
            {
                //模糊查询
                var DataResult = db.Queryable<AreaEntity>().Where(it => it.AreaId.StartsWith(areaid) && it.DeleteMark == 1).Select(it => new AreaDto
                {
                    AreaId = it.AreaId,
                    ParentId = it.ParentId,
                    AreaCode = it.AreaCode,
                    AreaName = it.AreaName,
                    SimpleSpelling = it.SimpleSpelling,
                    Layer = it.Layer,
                    Remark = it.Remark,
                    SortCode = it.SortCode,
                    CreateDate = it.CreateDate,
                    CreateUserId = it.CreateUserId,
                    CreateUserName = it.CreateUserName,
                    ModifyDate = it.ModifyDate,
                    ModifyUserId = it.ModifyUserId,
                    ModifyUserName = it.ModifyUserName,
                    DeleteMark = it.DeleteMark
                }).ToList();
                if (DataResult.Count() > 0)
                {
                    return DataResult;
                }
            }
            return null;
        }

        public string GetAreaName(string areaid)
        {
            using (var db = _dbContext.GetIntance())
            {
                var entity = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 && it.AreaId == areaid).ToList().FirstOrDefault();
                if (entity != null)
                {
                    return entity.AreaName;
                }
                else
                {
                    return "";
                }
            }
        }
        public List<AreaDto> GetPageListByCondition(string condition, string keyword, string parentId, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<AreaDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(parentId))
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                    {
                        switch (condition)
                        {
                            case "AreaCode":
                                DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 && it.ParentId == parentId && it.AreaCode!=null && it.AreaCode.Contains(keyword))
                                    .Select(it => new AreaDto()
                                    {
                                        AreaId = it.AreaId,
                                        ParentId = it.ParentId,
                                        AreaCode = it.AreaCode,
                                        AreaName = it.AreaName,
                                        SimpleSpelling = it.SimpleSpelling,
                                        Layer = it.Layer,
                                        Remark = it.Remark,
                                        SortCode = it.SortCode,
                                        CreateDate = it.CreateDate,
                                        CreateUserId = it.CreateUserId,
                                        CreateUserName = it.CreateUserName,
                                        ModifyDate = it.ModifyDate,
                                        ModifyUserId = it.ModifyUserId,
                                        ModifyUserName = it.ModifyUserName,
                                        DeleteMark = it.DeleteMark
                                    }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "AreaName":
                                DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 && it.ParentId == parentId && it.AreaName.Contains(keyword))
                                   .Select(it => new AreaDto()
                                   {
                                       AreaId = it.AreaId,
                                       ParentId = it.ParentId,
                                       AreaCode = it.AreaCode,
                                       AreaName = it.AreaName,
                                       SimpleSpelling = it.SimpleSpelling,
                                       Layer = it.Layer,
                                       Remark = it.Remark,
                                       SortCode = it.SortCode,
                                       CreateDate = it.CreateDate,
                                       CreateUserId = it.CreateUserId,
                                       CreateUserName = it.CreateUserName,
                                       ModifyDate = it.ModifyDate,
                                       ModifyUserId = it.ModifyUserId,
                                       ModifyUserName = it.ModifyUserName,
                                       DeleteMark = it.DeleteMark
                                   }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }
                    }
                    else
                    {
                        DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 && it.ParentId == parentId)
                                   .Select(it => new AreaDto()
                                   {
                                       AreaId = it.AreaId,
                                       ParentId = it.ParentId,
                                       AreaCode = it.AreaCode,
                                       AreaName = it.AreaName,
                                       SimpleSpelling = it.SimpleSpelling,
                                       Layer = it.Layer,
                                       Remark = it.Remark,
                                       SortCode = it.SortCode,
                                       CreateDate = it.CreateDate,
                                       CreateUserId = it.CreateUserId,
                                       CreateUserName = it.CreateUserName,
                                       ModifyDate = it.ModifyDate,
                                       ModifyUserId = it.ModifyUserId,
                                       ModifyUserName = it.ModifyUserName,
                                       DeleteMark = it.DeleteMark
                                   }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                    {
                        switch (condition)
                        {
                            case "AreaCode":
                                DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 &&it.AreaCode.Contains(keyword))
                                    .Select(it => new AreaDto()
                                    {
                                        AreaId = it.AreaId,
                                        ParentId = it.ParentId,
                                        AreaCode = it.AreaCode,
                                        AreaName = it.AreaName,
                                        SimpleSpelling = it.SimpleSpelling,
                                        Layer = it.Layer,
                                        Remark = it.Remark,
                                        SortCode = it.SortCode,
                                        CreateDate = it.CreateDate,
                                        CreateUserId = it.CreateUserId,
                                        CreateUserName = it.CreateUserName,
                                        ModifyDate = it.ModifyDate,
                                        ModifyUserId = it.ModifyUserId,
                                        ModifyUserName = it.ModifyUserName,
                                        DeleteMark = it.DeleteMark
                                    }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "AreaName":
                                DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 &&  it.AreaName.Contains(keyword))
                                   .Select(it => new AreaDto()
                                   {
                                       AreaId = it.AreaId,
                                       ParentId = it.ParentId,
                                       AreaCode = it.AreaCode,
                                       AreaName = it.AreaName,
                                       SimpleSpelling = it.SimpleSpelling,
                                       Layer = it.Layer,
                                       Remark = it.Remark,
                                       SortCode = it.SortCode,
                                       CreateDate = it.CreateDate,
                                       CreateUserId = it.CreateUserId,
                                       CreateUserName = it.CreateUserName,
                                       ModifyDate = it.ModifyDate,
                                       ModifyUserId = it.ModifyUserId,
                                       ModifyUserName = it.ModifyUserName,
                                       DeleteMark = it.DeleteMark
                                   }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }
                    }
                    else
                    {
                        DataResult = db.Queryable<AreaEntity>().Where(it => it.DeleteMark == 1 )
                                   .Select(it => new AreaDto()
                                   {
                                       AreaId = it.AreaId,
                                       ParentId = it.ParentId,
                                       AreaCode = it.AreaCode,
                                       AreaName = it.AreaName,
                                       SimpleSpelling = it.SimpleSpelling,
                                       Layer = it.Layer,
                                       Remark = it.Remark,
                                       SortCode = it.SortCode,
                                       CreateDate = it.CreateDate,
                                       CreateUserId = it.CreateUserId,
                                       CreateUserName = it.CreateUserName,
                                       ModifyDate = it.ModifyDate,
                                       ModifyUserId = it.ModifyUserId,
                                       ModifyUserName = it.ModifyUserName,
                                       DeleteMark = it.DeleteMark
                                   }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }

            }               
            return DataResult;
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="areaEntity">区域实体</param>
        /// <returns></returns>
        public bool Insert(AreaEntity areaEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(areaEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="areaEntity">区域实体</param>
        /// <returns></returns>
        public bool Update(AreaEntity areaEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(areaEntity)
                         .IgnoreColumns(it => new { it.CreateDate, it.CreateUserId, it.CreateUserName, it.DeleteMark })
                         .Where(it => it.AreaId == areaEntity.AreaId).ExecuteCommand();
                if (count > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool Delete(string keyValue, string UserId, string UserName)
        {
            using (var db = _dbContext.GetIntance())
            {
                //判断是否存在子节点
                var getByWhere = db.Queryable<AreaEntity>().Where(it => it.ParentId == keyValue).ToList();
                if (getByWhere.Count() > 0)
                {
                    return false;
                }
                var areaEntity = new AreaEntity();
                areaEntity.DeleteMark = 0;
                areaEntity.ModifyUserId = UserId;
                areaEntity.ModifyUserName = UserName;
                areaEntity.ModifyDate = DateTime.Now;
                //逻辑删除
                var count = db.Updateable(areaEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                .Where(it => it.AreaId == keyValue)
                .ExecuteCommand();
                return count > 0 ? true : false;
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> keyValues, string UserId, string UserName)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    //判断是否存在子节点
                    var getByWhere = db.Queryable<AreaEntity>().Where(it => keyValues.Contains(it.ParentId)).ToList();
                    if (getByWhere.Count() > 0)
                    {
                        return false;
                    }
                    var areaEntity = new AreaEntity();
                    areaEntity.DeleteMark = 0;
                    areaEntity.ModifyUserId = UserId;
                    areaEntity.ModifyUserName = UserName;
                    areaEntity.ModifyDate = DateTime.Now;

                    //逻辑删除
                    var counts = db.Updateable(areaEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                        .Where(it => keyValues.Contains(it.AreaId)).ExecuteCommand();
                    result = counts > 0 ? true : false;
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
