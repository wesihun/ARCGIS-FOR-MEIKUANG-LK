using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 检查表管理
    /// </summary>
    [SugarTable("checktable")]
    public class CheckTableEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 风险点分级主键
        /// </summary>		
        public string ClassificationId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string RoleId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string RoleName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string JobId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string JobName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string UserName { get; set; }
        /// <summary>
        /// 风险点编号
        /// </summary>		
        public string RiskPointBH { get; set; }
        /// <summary>
        /// 风险点名
        /// </summary>		
        public string RiskPointName { get; set; }
        /// <summary>
        /// 风险因素
        /// </summary>		
        public string RiskFactor { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>		
        public string RiskLevel { get; set; }       
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 排查事项
        /// </summary>		
        public string TroubleshootingItems { get; set; }
        /// <summary>
        /// 管控措施
        /// </summary>		
        public string ControlMeasures { get; set; }
        /// <summary>
        /// 措施类型
        /// </summary>		
        public string MeasuresType { get; set; }
        /// <summary>
        /// 应急措施
        /// </summary>		
        public string EmergencyMeasures { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名
        /// </summary>		
        public string OrgName { get; set; }
        /// <summary>
        /// 完成状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string states1 { get; set; }
        /// <summary>
        /// 上报状态
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string states2 { get; set; }
    }
}
