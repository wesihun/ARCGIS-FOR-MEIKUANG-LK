using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultInfo")]
    public class CheckResultInfoEntity
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
        public DateTime CheckDate { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>		
        public string ResultDescription { get; set; }
        /// <summary>
        /// 是否涉及处方违规（0不涉及，1涉及）
        /// </summary>
        public string IsPre { get; set; }
        /// <summary>
        /// 违规金额
        /// </summary>
        public decimal? MONEY { get; set;}
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime SettlementDate { get; set; }
        /// <summary>
        /// 入院日期
        /// </summary>
        public DateTime InHosDate { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public DateTime OutHosDate { get; set; }
        /// <summary>
        /// 规则等级   1.疑似   2. 刚性违规    
        /// </summary>
        public string RuleLevel { get; set; }
        /// <summary>
        /// 医疗机构等级编码
        /// </summary>
        public string InstitutionGradeCode { get; set; }

        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 违规次数
        /// </summary>
        public int? Count { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 申诉状态
        /// </summary>
        public string States { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 申诉表主键
        /// </summary>
        public string CheckComplainId { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 初审描述
        /// </summary>
        public string FirstTrialDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 申诉描述
        /// </summary>
        public string ComplaintDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 复审描述
        /// </summary>
        public string SecondTrialDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 疑点结论描述
        /// </summary>
        public string DoubtfulConclusionDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 建议价格
        /// </summary>
        public decimal? ProposalMoney { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 初审实际价格
        /// </summary>
        public decimal? RealMoneyFirst { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 复审实际价格
        /// </summary>
        public decimal? RealMoneySecond { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 建议描述
        /// </summary>
        public string ProposalDescribe { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 待审核违规金
        /// </summary>
        public decimal? DSHWGJ { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 待审核违规数
        /// </summary>
        public int? DSHWGS { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 已审核违规金
        /// </summary>
        public decimal? YSHWGJ { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 已审核违规数
        /// </summary>
        public int? YSHWGS { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 总违规金额
        /// </summary>
        public decimal? ZWGJE { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 总违规数量
        /// </summary>
        public int? ZWGSL { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 疑点等级
        /// </summary>
        public string YDDJ { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 审核流程状态  初审为智审完成
        /// </summary>
        public string SHLCZT { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 违规类型
        /// </summary>
        public string WGLX { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 最终审核状态
        /// </summary>
        public string ZZSHZT { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 违规费用
        /// </summary>
        public decimal? WGFY { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 疑似违规费用
        /// </summary>
        public decimal? YSWGFY { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 明细
        /// </summary>
        public string Detail { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 机构等级
        /// </summary>
        public string InstitutionLevel { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string PersonName { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 疑点描述
        /// </summary>
        public string YDDescription { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? ALLMoney { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 总数量
        /// </summary>
        public int? ALLCount { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 疑点等级
        /// </summary>
        public string YDLevel { get; set; }
        /// <summary>
        /// 家庭编码
        /// </summary>
        public string FamilyCode { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        public string Year { get; set; }
    }
}
