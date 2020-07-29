using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 检查计划管理
    /// </summary>
    [SugarTable("checkplan")]
    public class CheckPlanEnity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 计划名
        /// </summary>		
        public string PlanName { get; set; }
        /// <summary>
        /// 设置检查时间
        /// </summary>		
        public DateTime CheckDate { get; set; }
        /// <summary>
        /// 执行方式
        /// </summary>		
        public string ExecutionMode { get; set; }
        /// <summary>
        /// 检查点编号  多条用,分割
        /// </summary>		
        public string RiskBH { get; set; }
        /// <summary>
        /// 检查点编号名称  多条用,分割
        /// </summary>		
        public string RiskName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string OrgName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string DepId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string DepName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string RoleName { get; set; }
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string UserName { get; set; }
        public string JobId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string JobName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 上次完成时间
        /// </summary>		
        public DateTime? LastCompleteTime { get; set; }

        /// <summary>
        /// 完成状态   1已完成   0未完成   2待完成
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string states { get; set; }
    }
}
