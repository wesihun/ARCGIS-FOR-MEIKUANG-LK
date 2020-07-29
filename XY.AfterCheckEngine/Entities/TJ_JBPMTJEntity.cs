using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：TJ_JBPMTJEntity
    /// 创 建 者：LK
    /// 创建日期：2019/9/5 11:31:48
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/5 11:31:48
    /// </summary>
    [SugarTable("TJ_JBPMTJ")]
    public class TJ_JBPMTJEntity
    {
        public int CRowId { get; set; }
        public string DiseaseName { get; set; }
        public decimal? CJFY { get; set; }
    }
}
