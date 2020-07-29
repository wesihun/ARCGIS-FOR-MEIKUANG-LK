using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_Complain_MZLEntity
    /// 创 建 者：LK
    /// 创建日期：2019/11/19 14:52:17
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/19 14:52:17
    /// </summary>
    [SugarTable("Check_Complain_MZL")]
    public class Check_Complain_MZLEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string CheckComplainId { get; set; }
        /// <summary>
        /// 申诉状态
        /// </summary>
        public string ComplaintStatus { get; set; }
        /// <summary>
        /// 申诉状态分步
        /// </summary>
        public string ComplaintStatusStep { get; set; }
        /// <summary>
        /// 登记编码
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 个人编码
        /// </summary>
        public string PersonalCode { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string InstitutionName { get; set; }
        /// <summary>
        /// 机构等级
        /// </summary>
        public string InstitutionLevel { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PersonName { get; set; }
        /// <summary>
        /// ICDCode
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>
        public string DiseaseName { get; set; }
        /// <summary>
        /// 疑点描述
        /// </summary>
        public string YDDescription { get; set; }
        /// <summary>
        /// 入院日期
        /// </summary>
        public DateTime InHosDate { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public DateTime OutHosDate { get; set; }
        /// <summary>
        /// 结算日期
        /// </summary>
        public DateTime? SettlementDate { get; set; }
        /// <summary>
        /// 待审核违规金
        /// </summary>
        public decimal? DSHWGJ { get; set; }
        /// <summary>
        /// 待审核违规数
        /// </summary>
        public int? DSHWGS { get; set; }
        /// <summary>
        /// 已审核违规金
        /// </summary>
        public decimal? YSHWGJ { get; set; }
        /// <summary>
        /// 已审核违规数
        /// </summary>
        public int? YSHWGS { get; set; }
        /// <summary>
        /// 总违规金额
        /// </summary>
        public decimal? ALLMoney { get; set; }
        /// <summary>
        /// 总违规数量
        /// </summary>
        public int? ALLCount { get; set; }
        /// <summary>
        /// 疑点等级
        /// </summary>
        public string YDLevel { get; set; }
        /// <summary>
        /// 审核流程状态
        /// </summary>
        public string SHLCStates { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public string SHJG { get; set; }
        /// <summary>
        /// 初审时间
        /// </summary>		
        public DateTime? FirstTrialTime { get; set; }
        /// <summary>
        /// 初审人用户ID
        /// </summary>		
        public string FirstTrialUserId { get; set; }
        /// <summary>
        /// 初审人用户姓名
        /// </summary>		
        public string FirstTrialUserName { get; set; }
        /// <summary>
        /// 初审人机构编码
        /// </summary>		
        public string FirstTrialInstitutionCode { get; set; }
        /// <summary>
        /// 初审人机构名称
        /// </summary>		
        public string FirstTrialInstitutionName { get; set; }
        /// <summary>
        /// 初审人描述
        /// </summary>		
        public string FirstTrialDescribe { get; set; }
        /// <summary>
        /// 申诉时间
        /// </summary>		
        public DateTime? ComplaintTime { get; set; }
        /// <summary>
        /// 申诉人用户ID
        /// </summary>		
        public string ComplaintUserId { get; set; }
        /// <summary>
        /// 申诉人姓名
        /// </summary>		
        public string ComplaintUserName { get; set; }
        /// <summary>
		/// 申诉机构编码
        /// </summary>		
        public string ComplaintInstitutionCode { get; set; }
        /// <summary>
        /// 申诉机构名称
        /// </summary>		
        public string ComplaintInstitutionName { get; set; }
        /// <summary>
        /// 申诉描述
        /// </summary>		
        public string ComplaintDescribe { get; set; }
        /// <summary>
        /// 二次申诉时间
        /// </summary>		
        public DateTime? ComplaintSecondTime { get; set; }
        /// <summary>
        /// 二次申诉人用户ID
        /// </summary>		
        public string ComplaintSecondUserId { get; set; }
        /// <summary>
        /// 二次申诉人姓名
        /// </summary>		
        public string ComplaintSecondUserName { get; set; }
        /// <summary>
		/// 二次申诉机构编码
        /// </summary>		
        public string ComplaintSecondInstitutionCode { get; set; }
        /// <summary>
        /// 二次申诉机构名称
        /// </summary>		
        public string ComplaintSecondInstitutionName { get; set; }
        /// <summary>
        /// 申诉描述
        /// </summary>		
        public string ComplaintSecondDescribe { get; set; }
        /// <summary>
        /// 复审人时间
        /// </summary>		
        public DateTime? SecondTrialTime { get; set; }
        /// <summary>
        /// 复审人用户ID
        /// </summary>		
        public string SecondTrialUserId { get; set; }
        /// <summary>
        /// 复审人用户姓名
        /// </summary>		
        public string SecondTrialUserName { get; set; }
        /// <summary>
        /// 复审人机构编码
        /// </summary>		
        public string SecondTrialInstitutionCode { get; set; }
        /// <summary>
        /// 复审人机构名称
        /// </summary>		
        public string SecondTrialInstitutionName { get; set; }
        /// <summary>
        /// 复审人描述
        /// </summary>		
        public string SecondTrialDescribe { get; set; }
        /// <summary>
        /// 专家人时间
        /// </summary>		
        public DateTime? ExpertTrialTime { get; set; }
        /// <summary>
        /// 专家人用户ID
        /// </summary>		
        public string ExpertTrialUserId { get; set; }
        /// <summary>
        /// 专家人用户姓名
        /// </summary>		
        public string ExpertTrialUserName { get; set; }
        /// <summary>
        /// 专家人机构编码
        /// </summary>		
        public string ExpertTrialInstitutionCode { get; set; }
        /// <summary>
        /// 专家人机构名称
        /// </summary>		
        public string ExpertTrialInstitutionName { get; set; }
        /// <summary>
        /// 专家人描述
        /// </summary>		
        public string ExpertTrialDescribe { get; set; }
        /// <summary>
        /// 疑点结论操作时间
        /// </summary>		
        public DateTime? DoubtfulConclusionTime { get; set; }
        /// <summary>
        /// 疑点结论操作人
        /// </summary>		
        public string DoubtfulConclusionUserId { get; set; }
        /// <summary>
        /// 疑点结论操作人
        /// </summary>		
        public string DoubtfulConclusionUserName { get; set; }
        /// <summary>
        /// 疑点结论机构编码
        /// </summary>		
        public string DoubtfulConclusionInstitutionCode { get; set; }
        /// <summary>
        /// 疑点结论机构名
        /// </summary>		
        public string DoubtfulConclusionInstitutionName { get; set; }
        /// <summary>
        /// 疑点结论描述
        /// </summary>		
        public string DoubtfulConclusionDescribe { get; set; }
        /// <summary>
        /// 是否二次反馈   1 是   0否
        /// </summary>
        public string IsSceondFK { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FamilyCode { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 状态标识
        /// </summary>
        public string StatesBS { get; set; }
    }
}
