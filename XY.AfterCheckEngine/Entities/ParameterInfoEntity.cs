using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{    
    public class ParameterInfoEntity
    {
        /// <summary>
        /// 住院就诊人次
        /// </summary>
        public int? HosInfoCount { get; set; }
        /// <summary>
        /// 住院审核人次数
        /// </summary>
        public int? HosInfoCheckCount { get; set; }
        /// <summary>
        /// 住院审核违规人次数
        /// </summary>
        public int? HosInfoCheckErrorCount { get; set; }
        /// <summary>
        /// 门诊就诊人次
        /// </summary>
        public int? ClinicInfoCount { get; set; }
        /// <summary>
        /// 门诊审核人次数
        /// </summary>
        public int? ClinicInfoCheckCount { get; set; }
        /// <summary>
        /// 门诊审核违规人次数
        /// </summary>
        public int? ClinicInfoCheckErrorCount { get; set; }
        /// <summary>
        /// 一级医院违规占比
        /// </summary>
        public string OneProportion { get; set; }
        /// <summary>
        /// 二级医院违规占比
        /// </summary>
        public string TwoProportion { get; set; }
        /// <summary>
        /// 三级医院违规占比
        /// </summary>
        public string ThreeProportion { get; set; }

    }
}
