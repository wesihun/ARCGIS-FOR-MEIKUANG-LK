using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：CheckComplainEntity
    /// 创 建 者：LK
    /// 创建日期：2019/11/6 17:44:49
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/6 17:44:49
    /// </summary>
    [SugarTable("Check_Complain")]
    public class Check_ComplainEntity
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
        /// 系统违规次数
        /// </summary>
        public int? Count { get; set; }
        /// <summary>
        /// 登记编码
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 建议价格
        /// </summary>
        public decimal? ProposalMoney { get; set; }
        /// <summary>
        /// 初审实际价格
        /// </summary>
        public decimal? RealMoneyFirst { get; set; }
        /// <summary>
        /// 复审实际价格
        /// </summary>
        public decimal? RealMoneySecond { get; set; }
        /// <summary>
        /// 建议描述
        /// </summary>
        public string ProposalDescribe { get; set; }
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
        /// 反馈次数
        /// </summary>
        public int? FeedbackCount { get; set; }
    }
}
