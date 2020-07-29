using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：Check_ComplaintMain_MZLEntity
    /// 创 建 者：LK
    /// 创建日期：2019/11/19 15:33:06
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/19 15:33:06
    /// </summary>
    [SugarTable("Check_ComplaintMain_MZL")]
    public class Check_ComplaintMain_MZLEntity
    {
        /// <summary>
		/// 申诉编码
        /// </summary>		
        public string ComplaintCode { get; set; }
        /// <summary>
        /// 审核结果主键
        /// </summary>		
        public string CheckResultInfoCode { get; set; }
        
        public string CheckResultPreInfoCode { get; set; }
        /// <summary>
        /// 登记编码
        /// </summary>		
        public string RegisterCode { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>		
        public string ComplaintStatus { get; set; }
        /// <summary>
        /// 审核规则名称
        /// </summary>		
        public string RulesName { get; set; }
        /// <summary>
        /// 审核规则编号
        /// </summary>		
        public string RulesCode { get; set; }
        /// <summary>
        /// 规则等级
        /// </summary>		
        public string RulesLevel { get; set; }
        /// <summary>
        /// 最终审核状态
        /// </summary>		
        public string ZZSHStates { get; set; }
        /// <summary>
        /// 违规费用
        /// </summary>		
        public decimal? WGFY { get; set; }
        /// <summary>
        /// 疑似费用
        /// </summary>		
        public decimal? YSWGFY { get; set; }
        /// <summary>
        /// 疑点描述
        /// </summary>		
        public string YDDescription { get; set; }
        /// <summary>
        /// 是否处方   1 是  0 否
        /// </summary>		
        public string IsPre { get; set; }
        /// <summary>
        /// 明细名字  
        /// </summary>		
        public string PreName { get; set; }
        /// <summary>
        /// 单价  
        /// </summary>		
        public decimal? Price { get; set; }
        /// <summary>
        /// 数量  
        /// </summary>		
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
        /// <summary>
        /// 初审状态  
        /// </summary>		
        public string States1 { get; set; }
        /// <summary>
        /// 反馈状态  
        /// </summary>		
        public string States2 { get; set; }
        /// <summary>
        /// 复审状态  
        /// </summary>		
        public string States3 { get; set; }
        /// <summary>
        /// 专家审核状态  
        /// </summary>		
        public string States4 { get; set; }
        /// <summary>
        /// 疑点结论状态  
        /// </summary>		
        public string States5 { get; set; }
        /// <summary>
        /// 二次反馈提交状态  
        /// </summary>		
        public string StatesSecondFK { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 有效金额
        /// </summary>
        public decimal? YXJE { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 不可报销金额
        /// </summary>
        public decimal? BKBXJE { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 单价
        /// </summary>
        public decimal? DJ { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 项目编号
        /// </summary>
        public int? ItemIndex { get; set; }
        [SugarColumn(IsIgnore = true)]
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? ZJ { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FamilyCode { get; set; }
    }
}
