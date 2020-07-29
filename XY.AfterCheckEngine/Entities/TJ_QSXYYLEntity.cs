using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：TJ_QSXYYLEntity
    /// 创 建 者：LK
    /// 创建日期：2019/9/5 8:52:48
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/5 8:52:48
    /// </summary>
    [SugarTable("TJ_QSXYYL")]
    public class TJ_QSXYYLEntity
    {
        public int CRowId { get; set; }
        public string YPMC { get; set; }
        public int? SL { get; set; }
    }
}
