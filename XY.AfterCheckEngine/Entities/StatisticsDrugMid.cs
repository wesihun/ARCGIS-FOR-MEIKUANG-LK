using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Statistics_DrugMid")]
    public class StatisticsDrugMid
    {
        public string CrowID { get; set; }
        public string CommonName { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
