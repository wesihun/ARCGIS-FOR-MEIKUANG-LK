using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 隐患下发通知
    /// </summary>
    [SugarTable("hiddendangersnotice")]
    public class HiddenDangersNoticeEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReportId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string HiddenDangerLevel { get; set; }
        /// <summary>
        /// 治理人
        /// </summary>		
        public string PersonId { get; set; }
        /// <summary>
         /// 治理人名
         /// </summary>		
        public string PersonName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRRId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRRName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int DeleteMark { get; set; }
        public string States { get; set; }
        /// <summary>
        /// 是否督办   1是   0否
        /// </summary>
        public string IsSupervisor { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string RiskLevel { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string HiddenDangersDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string RiskBH { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string RiskName { get; set; }
        [SugarColumn(IsIgnore = true)]
        public DateTime ReTime { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string Result { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ReProposal { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string OrgId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string OrgName { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ZRDWName { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string TroubleshootingItems { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string RiskFactor { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ControlMeasures { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ReportImageUrl { get; set; }
        [SugarColumn(IsIgnore = true)]
        public DateTime ReportTime { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string ReportHiddenDangerLevel { get; set; }
        

    }
}
