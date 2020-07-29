using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models
{
    /// <summary>
    /// 查询条件通用类
    /// </summary>
    public class QueryCoditionByCheckResult
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 就诊编号
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 医疗机构登记    8 一级甲等    5二级甲等   2三级甲等
        /// </summary>
        public string InstitutionLevel { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 个人编码
        /// </summary>
        public string PersonalCode { get; set; }
        /// <summary>
        /// 疾病编码
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 开始结算时间
        /// </summary>
        public DateTime? StartSettleTime { get; set; }
        /// <summary>
        /// 结束结算时间
        /// </summary>
        public DateTime? EndSettleTime { get; set; }
        /// <summary>
        /// 结论开始时间
        /// </summary>
        public DateTime? StartConclusionTime { get; set; }
        /// <summary>
        /// 结束结算时间
        /// </summary>
        public DateTime? EndConclusionTime { get; set; }
    }
    /// <summary>
    /// 初审传参
    /// </summary>
    public class ConditionStringCS
    {
        /// <summary>
        /// 是否违规
        /// </summary>
        public string states { get; set; }
        /// <summary>
        /// 初审违规描述
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 违规金额
        /// </summary>
        public decimal wgMoney { get; set; }
        /// <summary>
        /// 实际违规金额
        /// </summary>
        public decimal modifyMoney { get; set; }
        /// <summary>
        /// 金额描述
        /// </summary>
        public string moneyDescription { get; set; }
        /// <summary>
        /// 初审违规编码数组
        /// </summary>
        public string[] rulesCode { get; set; }
    }
    public class UserInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
    }

    /// <summary>
    /// 审核统计页条件
    /// </summary>
    public class QueryConditionByCheckComplain
    {
        /// <summary>
        /// 就诊机构
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 审核规则
        /// </summary>
        public string RulesCode { get; set; }
        /// <summary>
        /// 违规状态
        /// </summary>
        public string States { get; set; }
        /// <summary>
        /// 审核开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 审核结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }

}
