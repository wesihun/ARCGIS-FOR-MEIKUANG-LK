using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 风险分级
    /// </summary>
    [SugarTable("riskclassification")]
    public class RiskClassIficationEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 风险点编号
        /// </summary>		
        public string RiskPointBH { get; set; }
        /// <summary>
        /// 风险点名称
        /// </summary>		
        public string RiskPointName { get; set; }
        /// <summary>
        /// R值
        /// </summary>		
        public int RiskR { get; set; }
        /// <summary>
        /// S值
        /// </summary>		
        public int SeverityS { get; set; }
        /// <summary>
        /// L值
        /// </summary>		
        public int PossibleL { get; set; }
        /// <summary>
        /// 风险因素
        /// </summary>		
        public string RiskFactor { get; set; }
        /// <summary>
        /// 风险因素类型
        /// </summary>		
        public string RiskFactorType { get; set; }
        /// <summary>
        /// 排查事项
        /// </summary>		
        public string TroubleshootingItems { get; set; }
        /// <summary>
        /// 管控措施
        /// </summary>		
        public string ControlMeasures { get; set; }
        /// <summary>
        /// 风险等级
        /// </summary>		
        public string RiskLevel { get; set; }
        /// <summary>
        /// 事故类型
        /// </summary>		
        public string AccidentType { get; set; }
        /// <summary>
        /// 措施类型
        /// </summary>		
        public string MeasuresType { get; set; }
        /// <summary>
        /// 应急措施
        /// </summary>		
        public string EmergencyMeasures { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        public int? DeleteMark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>		
        public string OrgId { get; set; }
        /// <summary>
        /// 机构名
        /// </summary>		
        public string OrgName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string ExecutionMode { get; set; }

    }
}
