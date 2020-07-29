using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultMain")]
    public class CheckResultMainEntity
    {
        /// <summary>
		/// 审核结果编码
        /// </summary>		
        public string CheckResultCode { get; set; }
        /// <summary>
        /// 审核规则编号
        /// </summary>		
        public string RulesCode { get; set; }
        /// <summary>
        /// 审核规则名称
        /// </summary>		
        public string RulesName { get; set; }
        /// <summary>
        /// 涉及人次
        /// </summary>		
        public int InvolveCounts { get; set; }
        /// <summary>
        /// 涉及人数
        /// </summary>		
        public int InvolveCount { get; set; }
        /// <summary>
        /// 涉及金额
        /// </summary>		
        public decimal InvolveFee { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime CheckDate { get; set; }
    }
}
