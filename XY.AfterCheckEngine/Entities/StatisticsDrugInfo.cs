using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Statistics_DrugInfo")]
    public class StatisticsDrugInfo
    {
        public string CrowID { get; set; }
        public string DrugName { get; set; }
        public int? DrugCount { get; set; }
        public decimal? Price { get; set; }
        public int? flag { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
