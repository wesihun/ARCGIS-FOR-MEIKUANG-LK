using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 隐患复查
    /// </summary>
    [SugarTable("hiddendangersrecheck")]
    public class HiddenDangersRecheckEntity
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
        public string ReUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public DateTime ReTime { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string Result { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReProposal { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReImageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReportUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ReportUserName { get; set; }
        public int DeleteMark { get; set; }
        /// <summary>
        /// 3已复查 通过   4已复查未通过
        /// </summary>		
        public string States { get; set; }
        /// <summary>
        /// 是否督办   1是   0否
        /// </summary>
        public string IsSupervisor { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string OrgId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string OrgName { get; set; }
        [SugarColumn(IsIgnore = true)] 
        public string RiskPointName { get; set; }
        [SugarColumn(IsIgnore = true)] 
        public string HiddenDangerLevel { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string RiskPointLevel { get; set; }
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
        public string HiddenDangersDescribe { get; set; }
    }
}
