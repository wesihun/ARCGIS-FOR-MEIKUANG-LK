using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_BeForeResultInfo
    /// 创 建 者：LK
    /// 创建日期：2020/3/13 10:19:34
    /// 最后修改者：LK
    /// 最后修改日期：2020/3/13 10:19:34
    /// </summary>
    [SugarTable("Check_BeForeResultInfo")]
    public class Check_BeForeResultInfo
    {
        /// <summary>
		/// 审核结果信息编码
		/// </summary>
		public string CheckResultInfoCode { get; set; }
        /// <summary>
        /// 审核规则名称
        /// </summary>
        public string RulesName { get; set; }
        /// <summary>
        /// 审核规则编号
        /// </summary>
        public string RulesCode { get; set; }
        /// <summary>
        /// 就诊登记编码
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 个人编码
        /// </summary>
        public string PersonalCode { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 就诊机构编码
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 就诊机构名称
        /// </summary>
        public string InstitutionName { get; set; }
        /// <summary>
        /// 疾病ICD编码
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>
        public string DiseaseName { get; set; }
        /// <summary>
        /// 数据分类（1代表门诊数据，2代表住院数据）
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDescription { get; set; }
        /// <summary>
        /// 是否涉及处方违规（0不涉及，1涉及）
        /// </summary>
        public string IsPre { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RuleLevel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionGradeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string InstitutionGradeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? COUNT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price { get; set; }
    }
}
