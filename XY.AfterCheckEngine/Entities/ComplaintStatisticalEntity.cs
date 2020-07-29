using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：YYKKGL   医院扣款管理
    /// 创 建 者：LK
    /// 创建日期：2019/12/11 11:19:52
    /// 最后修改者：LK
    /// 最后修改日期：2019/12/11 11:19:52
    /// </summary>
    public class YYKKGL        
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string RowId { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string YYMC { get; set; }
        /// <summary>
        /// 险种类别
        /// </summary>
        public string XZLB { get; set; }
        /// <summary>
        /// 费用来源
        /// </summary>
        public string FYLY { get; set; }
        /// <summary>
        /// 实际扣款金额
        /// </summary>
        public decimal? SJKKJE { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? SettlementDate { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal? JSJE { get; set; }
        /// <summary>
        /// 统筹金额
        /// </summary>
        public decimal? TCJE { get; set; }
        /// <summary>
        /// 大病金额
        /// </summary>
        public decimal? DBJE { get; set; }
        /// <summary>
        /// 公务员金额
        /// </summary>
        public decimal? GWYJE { get; set; }
        /// <summary>
        /// 扣款人次
        /// </summary>
        public int? CKRC { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 结算时间（string）
        /// </summary>
        public string SettlementTime { get; set; }
        /// <summary>
        /// 结论时间
        /// </summary>
        public string ConclusionTime { get; set; }
        /// <summary>
        /// 医院编码
        /// </summary>
        public string YYBM { get; set; }
        /// <summary>
        /// 机构等级
        /// </summary>
        public string JGDJ { get; set; }
        /// <summary>
        /// 违规等级
        /// </summary>
        public string WGDJ { get; set; }
        /// <summary>
        /// ICDCode
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 结论开始时间
        /// </summary>
        public string StartConclusionTime { get; set; }
        /// <summary>
        /// 结束结算时间
        /// </summary>
        public string EndConclusionTime { get; set; }
    }

    public class CheckUserList
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string RowId { get; set; }
        /// <summary>
        /// 住院登记编码
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
        /// 审核时间
        /// </summary>		
        public DateTime CheckDate { get; set; }       
        /// <summary>
        /// 违规金额
        /// </summary>
        public decimal? KKJE { get; set; }
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
        /// 医疗机构等级编码
        /// </summary>
        public string InstitutionGradeCode { get; set; }
        /// <summary>
        /// 医疗机构等级名称
        /// </summary>
        public string InstitutionGradeName { get; set; }      
    }
    public class ListByRulesCode
    {
        public string XH { get; set; }
        public string RulesCode { get; set; }
        public string RulesLevel { get; set; }
        public int? WGBLS { get; set; }
        public decimal? WGJE { get; set; }
        public int? WGS { get; set; }
    }
    public class ListByRulesCodeQuery
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        public string InstitutionLevel { get; set; }
        public string RulesLevel { get; set; }
    }
}
