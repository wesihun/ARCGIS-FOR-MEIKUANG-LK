using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 计划完成日志记录
    /// </summary>
    [SugarTable("plan_log")]
    public class PlanLogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 计划id
        /// </summary>		
        public string PlanId { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>		
        public string UserId { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>		
        public DateTime CompleteTime { get; set; }
    }
}
