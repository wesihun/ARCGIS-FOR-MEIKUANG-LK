using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models
{
    public class QueryConditionViewModel
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 人员编码
        /// </summary>
        public string PersonalCode { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 机构级别
        /// </summary>
        public string InstitutionLevelCode { get; set; }
        /// <summary>
        /// 机构等级
        /// </summary>
        public string InstitutiongGradeCode { get; set; }
        /// <summary>
        /// 住院登记编码
        /// </summary>
        public string HosRegisterCode { get; set; }
        /// <summary>
        /// 行政区划代码（支持到县级）
        /// </summary>
        public string CityAreaCode { get; set; }
        /// <summary>
        /// 人员类型编码
        /// </summary>
        public string PersonalTypeCode { get; set; }
        /// <summary>
        /// 入院时间
        /// </summary>
        public DateTime? InHosDate { get; set; }
        /// <summary>
        /// 出院时间
        /// </summary>
        public DateTime? OutHosDate { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal? ZFY { get; set; }
        /// <summary>
        /// 开始年龄
        /// </summary>
        public int? StartAge { get; set; }
        /// <summary>
        /// 开始年龄
        /// </summary>
        public int? EndAge { get; set; }
    }
}
