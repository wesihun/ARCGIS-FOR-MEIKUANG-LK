using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;
namespace XY.SystemManage.IService
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/10/22 14:13:23
    /// 版本：v1.0.0
    /// 描述：日志类
    /// </summary> 
    public interface ILogService
    {
        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页显示数量</param>
        /// <returns></returns>
        List<LogDto> GetAll(DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据条件获取日志列表并分页
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<LogDto> GePagetListByCondition(DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize, ref int totalCount);

        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="logEntity">日志实体</param>
        /// <returns></returns>
        bool Insert(LogEntity logEntity);
        #endregion

        #region 批量删除
        /// <summary>
        /// 批量杀出
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        bool DeleteBatch(List<string> keyValues);
        #endregion
    }
}
