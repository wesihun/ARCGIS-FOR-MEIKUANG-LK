using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities
{
    /// <summary>
    /// 隐患上报
    /// </summary>
    [SugarTable("hiddendangersreport")]
    public class HiddenDangersReportEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }
        /// <summary>
        /// 位置
        /// </summary>		
        public string Position { get; set; }
        /// <summary>
        /// 
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
        public string RiskPointLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string HiddenDangersLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string HiddenDangersType { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string HiddenDangersDescribe { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRDW { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRDWName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRR { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ZRRName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string TBUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string TBUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public string ImageUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>		
        public int DeleteMark { get; set; }
        /// <summary>
        /// 是否推送安全部门   1推送  0不推送
        /// </summary>		
        public int PushMark { get; set; }
        /// <summary>
        /// 推送给安全管理部门的用户名
        /// </summary>
        public string PushZRR { get; set; }
        /// <summary>
        /// 推送给安全管理部门的用户id
        /// </summary>
        public string PushUserId { get; set; }
        /// <summary>
        /// 状态   1未整改  2已整改 待复查  3已复查 通过   4已复查未通过 5已下发通知
        /// </summary>		
        public string States { get; set; }
        /// <summary>
        /// 是否督办   1是   0否
        /// </summary>
        public string IsSupervisor { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string TroubleshootingItems { get; set; }
        public string OrgId { get; set; }
        public string OrgName { get; set; }

    }

}
