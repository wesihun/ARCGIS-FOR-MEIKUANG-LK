using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    /// <summary>
    /// 功能描述：反序列化JsonObject用
    /// 创 建 者：林肯
    /// 创建日期：2019/6/5 10:23:01
    /// 最后修改者：林肯
    /// 最后修改日期：2019/6/5 10:23:01
    /// </summary>
    public class DecomposeHosQuery
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 就诊医院编码
        /// </summary>
        public string InstitutionCode { get; set; }
    }
}
