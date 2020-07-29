using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：HisZyyzmxEntity
    /// 创 建 者：LK
    /// 创建日期：2019/9/6 11:10:50
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/6 11:10:50
    /// </summary>
    [SugarTable("HIS_ZYYZMX")]
    public class HisZyyzmxEntity
    {
        public string CRowId { get; set; }
        public string YLJGBH { get; set; }
        public string ZYBH { get; set; }
        public string XMMC { get; set; }
        /// <summary>
        /// 1.药物医嘱 2.诊疗医嘱
        /// </summary>
        public string YZXMLX { get; set; }
        public DateTime? YZQSSJ { get; set; }
        public DateTime? YZTZSJ { get; set; }
        public string DCYL { get; set; }
        public string JLDW { get; set; }
        public string PLCS { get; set; }
        public int? DeleteMark { get; set; }
    }
}
