using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class QueryConditionByClinic
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 就诊频次
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 就诊年
        /// </summary>
        public int? ClinicDateYear { get; set; }
        /// <summary>
        /// 就诊月
        /// </summary>
        public int? ClinicDateMonth { get; set; }
        /// <summary>
        /// 就诊机构
        /// </summary>
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal? ZFY { get; set; }
        /// <summary>
        /// 目录内费用
        /// </summary>
        public decimal? MLNFY { get; set; }
        /// <summary>
        /// 统筹支付金额
        /// </summary>
        public decimal? YBBXFY { get; set; }
       
    }
}
