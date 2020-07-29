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
    /// 时间：2018/10/20 13:55:23
    /// 版本：v1.0.0
    /// 描述：字典服务实现类
    /// </summary> 6
    public class DataDictService : IDataDictService
    {
        private bool result = false;
        #region 构造注入
        private readonly IXYDbContext _dbContext;
        public DataDictService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<DataDictDto> GetAll()
        {
            var DataResult = new List<DataDictDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<DataDictEntity>().Where(de => de.DeleteMark == 1).OrderBy(de => de.SortCode)
                .Select(de => new DataDictDto
                {
                    CRowId = de.CRowId,
                    DataType = de.DataType,
                    ItemCode = de.ItemCode,
                    ItemName = de.ItemName,
                    Remark = de.Remark,
                    SortCode = de.SortCode,
                    CreateDate = de.CreateDate,
                    CreateUserId = de.CreateUserId,
                    CreateUserName = de.CreateUserName,
                    DeleteMark = de.DeleteMark,
                    ModifyDate = de.ModifyDate,
                    ModifyUserId = de.ModifyUserId,
                    ModifyUserName = de.ModifyUserName
                }).ToList();
            }
            return DataResult;
        }
        public List<DataDictDto> GetPageListByCondition(string currOrgId,string condition, string keyWord, string flid, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<DataDictDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(flid))
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyWord))
                    {
                        switch (condition)
                        {
                            case "DataType":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId),de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.DataType == flid && de.DataType != null && de.DataType.Contains(keyWord))
                                    .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId=de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate=de.CreateDate,
                                      CreateUserId=de.CreateUserId,
                                      CreateUserName=de.CreateUserName,
                                      DeleteMark=de.DeleteMark,
                                      ModifyDate=de.ModifyDate,
                                      ModifyUserId=de.ModifyUserId,
                                      ModifyUserName=de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ItemCode":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.DataType == flid && de.ItemCode != null && de.ItemCode.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ItemName":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.DataType == flid && de.ItemName != null && de.ItemName.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "Record":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.DataType == flid && de.Remark != null && de.Remark.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }
                    }
                    else
                    {
                        DataResult = db.Queryable<DataDictEntity>()
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                            .Where(de => de.DeleteMark == 1 && de.DataType == flid)
                             .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyWord))
                    {
                        switch (condition)
                        {
                            case "DataType":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.DataType != null && de.DataType.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ItemCode":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.ItemCode != null && de.ItemCode.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "ItemName":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.ItemName != null && de.ItemName.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                            case "Record":
                                DataResult = db.Queryable<DataDictEntity>()
                                    .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                                    .Where(de => de.DeleteMark == 1 && de.Remark != null && de.Remark.Contains(keyWord))
                                     .OrderBy(de => de.SortCode)
                                  .Select(de => new DataDictDto()
                                  {
                                      CRowId = de.CRowId,
                                      DataType = de.DataType,
                                      ItemCode = de.ItemCode,
                                      ItemName = de.ItemName,
                                      Remark = de.Remark,
                                      SortCode = de.SortCode,
                                      CreateDate = de.CreateDate,
                                      CreateUserId = de.CreateUserId,
                                      CreateUserName = de.CreateUserName,
                                      DeleteMark = de.DeleteMark,
                                      ModifyDate = de.ModifyDate,
                                      ModifyUserId = de.ModifyUserId,
                                      ModifyUserName = de.ModifyUserName
                                  }).ToPageList(page, limit, ref totalcount).ToList();
                                break;
                        }
                    }
                    else
                    {

                        DataResult = db.Queryable<DataDictEntity>()
                            .WhereIF(!string.IsNullOrEmpty(currOrgId), de => de.OrgId == currOrgId)
                            .Where(de => de.DeleteMark == 1)
                             .OrderBy(de => de.SortCode)
                          .Select(de => new DataDictDto()
                          {
                              CRowId = de.CRowId,
                              DataType = de.DataType,
                              ItemCode = de.ItemCode,
                              ItemName = de.ItemName,
                              Remark = de.Remark,
                              SortCode = de.SortCode,
                              CreateDate = de.CreateDate,
                              CreateUserId = de.CreateUserId,
                              CreateUserName = de.CreateUserName,
                              DeleteMark = de.DeleteMark,
                              ModifyDate = de.ModifyDate,
                              ModifyUserId = de.ModifyUserId,
                              ModifyUserName = de.ModifyUserName
                          }).ToPageList(page, limit, ref totalcount).ToList();
                    }
                }
              
            }
            return DataResult;
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="DataType">父级值</param>
        /// <param name="ItemId">主键值</param>
        /// <returns></returns>
        public DataDictDto GetEntityByDataTypeAndItemId(string DataType, string ItemId)
        {
            var DataResult = new DataDictDto();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<DataDictEntity>().Where(de => de.DeleteMark == 1 && de.DataType == DataType && de.ItemCode == ItemId).OrderBy(de => de.SortCode)
                .Select(de => new DataDictDto
                {
                    CRowId = de.CRowId,
                    DataType = de.DataType,
                    ItemCode = de.ItemCode,
                    ItemName = de.ItemName,
                    Remark = de.Remark,
                    SortCode = de.SortCode,
                    CreateDate = de.CreateDate,
                    CreateUserId = de.CreateUserId,
                    CreateUserName = de.CreateUserName,
                    DeleteMark = de.DeleteMark,
                    ModifyDate = de.ModifyDate,
                    ModifyUserId = de.ModifyUserId,
                    ModifyUserName = de.ModifyUserName
                }).ToList().SingleOrDefault();
            }
            return DataResult;
        }
        /// <summary>
        /// 字典是否重复校验
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <returns></returns>
        public bool CheckIsDataDictRepeat(string DataType, string ItemId)
        {
            using (var db = _dbContext.GetIntance())
            {
                //if(db.Queryable<DataDictEntity>().Where(it => it.DeleteMark == 1).ToList().Count() == 0)
                //{
                //    return true;
                //}
                return db.Queryable<DataDictEntity>().Any(it => it.DataType == DataType && it.ItemCode == ItemId && it.DeleteMark == 1);
            }
        }
        #endregion


        public bool ExistEnCode(string datatype,string bh, string keyValue,string currOrgId)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<DataDictEntity>().Any(it => it.DataType == datatype && it.ItemCode == bh && it.DeleteMark == 1 && it.OrgId == currOrgId);
                else
                    return db.Queryable<DataDictEntity>().Any(it => it.DataType == datatype && it.ItemCode == bh && it.CRowId != keyValue && it.DeleteMark == 1 && it.OrgId == currOrgId);
            }
        }

        public bool ExistFullName(string datatype,string name, string keyValue,string currOrgId)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(keyValue))
                    return db.Queryable<DataDictEntity>().Any(it => it.DataType == datatype && it.ItemName == name && it.DeleteMark == 1 && it.OrgId == currOrgId);
                else
                    return db.Queryable<DataDictEntity>().Any(it => it.DataType == datatype && it.ItemName == name && it.CRowId != keyValue && it.DeleteMark == 1 && it.OrgId == currOrgId);
            }
        }

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dataDictEntity">字典实体</param>
        /// <returns></returns>
        public bool Insert(DataDictEntity dataDictEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(dataDictEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataDictEntity">字典实体</param>
        /// <returns></returns>
        public bool Update(DataDictEntity dataDictEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(dataDictEntity)
                .IgnoreColumns(it => new { it.CreateDate, it.CreateUserId, it.CreateUserName, it.DeleteMark,it.OrgId })
                .Where(it => it.CRowId == dataDictEntity.CRowId)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        public bool Delete(string DataType, string ItemId, string UserId, string UserName)
        {
            var dataDictEntity = new DataDictEntity();
            dataDictEntity.DeleteMark = 0;
            dataDictEntity.ModifyUserId = UserId;
            dataDictEntity.ModifyUserName = UserName;
            dataDictEntity.ModifyDate = DateTime.Now;
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    //逻辑删除
                    db.Updateable(dataDictEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                    .Where(it => it.DataType == DataType && it.ItemCode == ItemId)
                    .ExecuteCommand();
                    if(DataType == "type")
                    {
                        db.Updateable(dataDictEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                        .Where(it => it.DataType == ItemId)
                        .ExecuteCommand();
                    }
                    db.Ado.CommitTran();
                }
                catch (Exception)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
                return true;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> DataType, List<string> ItemId, string UserId, string UserName)
        {
            bool result = false;
            if (DataType.Count() > 0 || ItemId.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var dataDictEntity = new DataDictEntity();
                    dataDictEntity.DeleteMark = 0;
                    dataDictEntity.ModifyUserId = UserId;
                    dataDictEntity.ModifyUserName = UserName;
                    dataDictEntity.ModifyDate = DateTime.Now;
                    var counts = 0;
                    for (int i = 0; i < DataType.Count(); i++)
                    {
                        //逻辑删除
                        counts += db.Updateable(dataDictEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                           .Where(it => it.DataType == DataType[i] && it.ItemCode == ItemId[i]).ExecuteCommand();
                        if(DataType[i] == "type")
                        {
                            db.Updateable(dataDictEntity).UpdateColumns(it => new { it.DeleteMark, it.ModifyDate, it.ModifyUserId, it.ModifyUserName })
                            .Where(it => it.DataType == ItemId[i])
                            .ExecuteCommand();
                        }
                        //物理删除
                        //counts += db.Deleteable<DataDictEntity>().Where(it => it.DataType == DataType[i] && it.ItemCode == ItemId[i]).ExecuteCommand();
                    }
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
