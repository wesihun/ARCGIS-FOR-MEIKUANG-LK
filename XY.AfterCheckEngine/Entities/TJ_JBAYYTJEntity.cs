using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：TJ_JBAYYTJEntity
    /// 创 建 者：LK
    /// 创建日期：2019/9/20 11:34:20
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/20 11:34:20
    /// </summary>
    [SugarTable("TJ_JBAYYTJ")]
    public class TJ_JBAYYTJEntity
    {
        public string CRowId { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string DiseaseName { get; set; }
        public string CJFY { get; set; }
    }
}
