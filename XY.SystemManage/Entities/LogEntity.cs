
using SqlSugar;
using System;

namespace XY.SystemManage.Entities
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/9/16 0:32:34 
    /// 版本：v1.0.0
    /// 描述：系统日志
    /// </summary>
    [SugarTable("Base_Log")]
    public class LogEntity 
    {
        #region 实体成员

        /// <summary>
        /// 日志主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string LogId { get; set; }
        /// <summary>
        /// 分类Id 1-登陆2-访问3-操作4-异常
        /// </summary>		
        public int? CategoryId { get; set; }
        /// <summary>
        /// 来源对象主键
        /// </summary>		
        public string SourceObjectId { get; set; }
        /// <summary>
        /// 来源日志内容
        /// </summary>		
        public string SourceContentJson { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>		
        public DateTime? OperateTime { get; set; }
        /// <summary>
        /// 操作用户Id
        /// </summary>		
        public string OperateUserId { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>		
        public string OperateAccount { get; set; }
       /// <summary>
       /// 操作类型ID
       /// </summary>
       public string OperateTypeId { get; set; }
        /// <summary>
       /// 操作类型
       /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 系统功能主键
        /// </summary>		
        public string ModuleId { get; set; }
        /// <summary>
        /// 系统功能名称
        /// </summary>		
        public string ModuleName { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>		
        public string IPAddress { get; set; }
        /// <summary>
        /// IP地址所在城市
        /// </summary>		
        public string IPAddressName { get; set; }
        /// <summary>
        /// 主机
        /// </summary>		
        public string Host { get; set; }
        /// <summary>
        /// 浏览器
        /// </summary>		
        public string Browser { get; set; }
        /// <summary>
        /// 执行结果状态
        /// </summary>		
        public int? ExecuteResult { get; set; }
        /// <summary>
        /// 执行结果信息
        /// </summary>		
        public string ExecuteResultJson { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        #endregion
    }
}
