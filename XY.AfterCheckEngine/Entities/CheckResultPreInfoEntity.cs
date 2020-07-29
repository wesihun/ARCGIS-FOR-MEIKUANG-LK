using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultPreInfo")]
    public class CheckResultPreInfoEntity
    {
        /// <summary>
		/// 审核结果处方信息编码
        /// </summary>		
        public string CheckResultPreInfoCode { get; set; }
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
        /// 处方编号
        /// </summary>		
        public string PreCode { get; set; }
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

        public int ItemIndex { get; set; }

        public decimal? Price { get; set; }

        public int? Count { get; set; }
        /// <summary>
        /// 项目编码  
        /// </summary>		
        public string ItemCode { get; set; }
        /// <summary>
        /// 项目名字  
        /// </summary>		
        public string ItemName { get; set; }
        /// <summary>
        /// 报销比例  
        /// </summary>		
        public decimal? CompRatio { get; set; }

    }
}
