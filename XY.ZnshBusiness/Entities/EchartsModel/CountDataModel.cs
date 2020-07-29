using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities.EchartsModel
{
    public class CountDataModel
    {
        /// <summary>
        /// 重大隐患数
        /// </summary>
        public int Count1 { get; set; }
        /// <summary>
        /// 已处理重大隐患数
        /// </summary>
        public int Count11 { get; set; }
        /// <summary>
        /// 一般隐患数
        /// </summary>
        public int Count2 { get; set; }
        /// <summary>
        /// 已处理般隐患数
        /// </summary>
        public int Count22 { get; set; }
        /// <summary>
        /// 已整改数
        /// </summary>
        public int Count3 { get; set; }
        /// <summary>
        /// 待整改数
        /// </summary>
        public int Count4 { get; set; }
    }
}
