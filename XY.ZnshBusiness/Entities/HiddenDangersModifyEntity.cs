using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 隐患整改
    /// </summary>
    [SugarTable("hiddendangersmodify")]
    public class HiddenDangersModifyEntity
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
        
        public string NoticeId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ModifyUserId { get; set; }

        public string ModifyUserName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>		
        public string ModifyStates { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ModifySituation { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ModifyResult { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ImageUrl { get; set; }
        /// <summary>
        /// 整改时间
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int DeleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRRId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRRName { get; set; }
        /// <summary>
        /// 2已整改 待复查  3已复查 通过   4已复查未通过
        /// </summary>		
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
        public string OrgId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string OrgName { get; set; }
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
    }
}
