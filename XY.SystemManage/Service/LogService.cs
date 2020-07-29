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
    /// 时间：2018/10/22 14:15:23
    /// 版本：v1.0.0
    /// 描述 日志类
    /// </summary> 
    public class LogService : ILogService
    {
        private bool result = false;

        #region 构造注入
        private readonly IXYDbContext _dbContext;
        public LogService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<LogDto> GetAll(DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<LogDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (startTime != null && endTime != null)
                {
                    DataResult = db.Queryable<LogEntity>().Where(log => log.DeleteMark == 1 && log.OperateTime >= startTime && log.OperateTime <= endTime).OrderBy(log => log.OperateTime, OrderByType.Desc).Select(log => new LogDto
                    {
                        LogId = log.LogId,
                        CategoryId = log.CategoryId,
                        SourceObjectId = log.SourceObjectId,
                        SourceContentJson = log.SourceContentJson,
                        OperateTime = log.OperateTime,
                        OperateUserId = log.OperateUserId,
                        OperateAccount = log.OperateAccount,
                        OperateTypeId=log.OperateTypeId,
                        OperateType=log.OperateType,
                        Action=log.Action,
                        ModuleId = log.ModuleId,
                        ModuleName = log.ModuleName,
                        IPAddress = log.IPAddress,
                        IPAddressName = log.IPAddressName,
                        Host = log.Host,
                        Browser = log.Browser,
                        ExecuteResult = log.ExecuteResult,
                        ExecuteResultJson = log.ExecuteResultJson,
                        Remark = log.Remark,
                        DeleteMark = log.DeleteMark
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = db.Queryable<LogEntity>().Where(log => log.DeleteMark == 1).OrderBy(log => log.OperateTime, OrderByType.Desc).Select(log => new LogDto
                    {
                        LogId = log.LogId,
                        CategoryId = log.CategoryId,
                        SourceObjectId = log.SourceObjectId,
                        SourceContentJson = log.SourceContentJson,
                        OperateTime = log.OperateTime,
                        OperateUserId = log.OperateUserId,
                        OperateAccount = log.OperateAccount,
                        OperateTypeId = log.OperateTypeId,
                        OperateType = log.OperateType,
                        Action = log.Action,
                        ModuleId = log.ModuleId,
                        ModuleName = log.ModuleName,
                        IPAddress = log.IPAddress,
                        IPAddressName = log.IPAddressName,
                        Host = log.Host,
                        Browser = log.Browser,
                        ExecuteResult = log.ExecuteResult,
                        ExecuteResultJson = log.ExecuteResultJson,
                        Remark = log.Remark,
                        DeleteMark = log.DeleteMark
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
            }

            return DataResult;
        }
        public List<LogDto> GePagetListByCondition(DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<LogDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (startTime != null && endTime != null)
                {
                    DataResult = db.Queryable<LogEntity>().Where(log => log.DeleteMark == 1 && log.OperateTime >= startTime && log.OperateTime <= endTime).OrderBy(log => log.OperateTime, OrderByType.Desc).Select(log => new LogDto
                    {
                        LogId = log.LogId,
                        CategoryId = log.CategoryId,
                        SourceObjectId = log.SourceObjectId,
                        SourceContentJson = log.SourceContentJson,
                        OperateTime = log.OperateTime,
                        OperateUserId = log.OperateUserId,
                        OperateAccount = log.OperateAccount,
                        OperateTypeId = log.OperateTypeId,
                        OperateType = log.OperateType,
                        Action = log.Action,
                        ModuleId = log.ModuleId,
                        ModuleName = log.ModuleName,
                        IPAddress = log.IPAddress,
                        IPAddressName = log.IPAddressName,
                        Host = log.Host,
                        Browser = log.Browser,
                        ExecuteResult = log.ExecuteResult,
                        ExecuteResultJson = log.ExecuteResultJson,
                        Remark = log.Remark,
                        DeleteMark = log.DeleteMark
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = db.Queryable<LogEntity>().Where(log => log.DeleteMark == 1).OrderBy(log => log.OperateTime, OrderByType.Desc).Select(log => new LogDto
                    {
                        LogId = log.LogId,
                        CategoryId = log.CategoryId,
                        SourceObjectId = log.SourceObjectId,
                        SourceContentJson = log.SourceContentJson,
                        OperateTime = log.OperateTime,
                        OperateUserId = log.OperateUserId,
                        OperateAccount = log.OperateAccount,
                        OperateTypeId = log.OperateTypeId,
                        OperateType = log.OperateType,
                        Action = log.Action,
                        ModuleId = log.ModuleId,
                        ModuleName = log.ModuleName,
                        IPAddress = log.IPAddress,
                        IPAddressName = log.IPAddressName,
                        Host = log.Host,
                        Browser = log.Browser,
                        ExecuteResult = log.ExecuteResult,
                        ExecuteResultJson = log.ExecuteResultJson,
                        Remark = log.Remark,
                        DeleteMark = log.DeleteMark
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
            }

            return DataResult;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="logEntity">日志实体</param>
        /// <returns></returns>
        public bool Insert(LogEntity logEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(logEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量杀出
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool DeleteBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var logEntity = new LogEntity();
                    logEntity.DeleteMark = 0;
                    //逻辑删除
                    var counts = db.Updateable(logEntity).UpdateColumns(it => new { it.DeleteMark })
                        .Where(it => keyValues.Contains(it.LogId)).ExecuteCommand();
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
