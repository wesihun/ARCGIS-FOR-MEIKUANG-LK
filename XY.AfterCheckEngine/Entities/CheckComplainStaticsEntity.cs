using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    public class CheckComplainStaticsEntity
    {       
        /// <summary>
        /// 就诊机构编码
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 就诊机构名称
        /// </summary>
        public string InstitutionName { get; set; }
        /// <summary>
        /// 费用来源 1门诊 2住院
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 实际扣款金额
        /// </summary>
        public decimal? KKJE { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal? JSJE { get; set; }
        /// <summary>
        /// 实际补偿金额
        /// </summary>
        public decimal? SJBCJE { get; set; }
        /// <summary>
        /// 大病金额
        /// </summary>
        public decimal? DBJE { get; set; }
        /// <summary>
        /// 医疗救助金额
        /// </summary>
        public decimal? YLJZJE { get; set; }
        /// <summary>
        /// 商业保险报销金额
        /// </summary>
        public decimal? SYBXJE { get; set; }
        /// <summary>
        /// 商业保险补充金额
        /// </summary>
        public decimal? SYBXBCJE { get; set; }
        /// <summary>
        /// 扣款人次
        /// </summary>
        public int? KKRC { get; set; }

    }
}
