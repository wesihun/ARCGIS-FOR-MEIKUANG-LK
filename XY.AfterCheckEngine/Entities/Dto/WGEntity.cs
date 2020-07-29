using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities.Dto
{
    /// <summary>
    /// 功能描述：WGEntity
    /// 创 建 者：LK
    /// 创建日期：2019/11/6 14:47:50
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/6 14:47:50
    /// </summary>
    public class WGEntity
    {
        /// <summary>
        /// 违规金额
        /// </summary>
        public decimal? Money { get; set; }
        /// <summary>
        /// 违规描述
        /// </summary>
        public string WGDescription { get; set; }

    }
}
