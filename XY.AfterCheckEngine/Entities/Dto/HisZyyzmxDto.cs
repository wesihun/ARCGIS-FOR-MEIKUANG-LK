using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities.Dto
{
    /// <summary>
    /// 功能描述：HisZyyzmxDto
    /// 创 建 者：LK
    /// 创建日期：2019/9/6 14:20:09
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/6 14:20:09
    /// </summary>
    public class HisZyyzmxDto
    {
        public string XMMC { get; set; }
        /// <summary>
        /// 1.药物医嘱 2.诊疗医嘱
        /// </summary>
        public string YZXMLX { get; set; }
        public string YZQSSJ { get; set; }
        public string YZTZSJ { get; set; }
        public string DCYL { get; set; }
        public string JLDW { get; set; }
        public string PLCS { get; set; }
    }
}
