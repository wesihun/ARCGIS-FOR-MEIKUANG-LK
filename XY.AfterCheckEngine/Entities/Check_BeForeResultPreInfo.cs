using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_BeForeResultPreInfo
    /// 创 建 者：LK
    /// 创建日期：2020/3/13 10:24:55
    /// 最后修改者：LK
    /// 最后修改日期：2020/3/13 10:24:55
    /// </summary>
    [SugarTable("Check_BeForeResultPreInfo")]
    public class Check_BeForeResultPreInfo
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
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RulesName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RulesCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ItemIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? COUNT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? LimitPrice { get; set; }
    }
}
