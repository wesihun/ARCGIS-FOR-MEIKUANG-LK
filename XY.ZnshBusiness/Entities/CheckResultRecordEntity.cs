using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 检查结果记录
    /// </summary>
    [SugarTable("checkresultrecord")]
    public class CheckResultRecordEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 计划id
        /// </summary>		
        public string CheckPlanId { get; set; }
        /// <summary>
        /// 计划名
        /// </summary>		
        public string CheckPlanName { get; set; }
        /// <summary>
        /// 检查表主键
        /// </summary>		
        public string CheckTableId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string RiskPointBH { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string RiskPointName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string OrgName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 是否存在隐患  1是  0否
        /// </summary>
        public int States { get; set; }
        /// <summary>
        /// 排查事项
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TroubleshootingItems { get; set; }
    }
}
