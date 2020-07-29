using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ComplaintMain")]
    public class CheckComplaintEntity
    {
        /// <summary>
		/// 申诉编码
        /// </summary>		
        public string ComplaintCode { get; set; }
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
		/// 数据分类
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
        /// 是否涉及处方违规
        /// </summary>		
        public string IsPre { get; set; }
        /// <summary>
        /// 结果状态
        /// </summary>		
        public string ComplaintResultStatus { get; set; }      
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? SettlementDate { get; set; }
    }
}
