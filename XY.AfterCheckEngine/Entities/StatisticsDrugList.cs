using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Statistics_DrugList")]
    public class StatisticsDrugList
    {
        public string CrowID { get; set; }
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionLevelCode { get; set; }
        public string InstitutionLevelName { get; set; }
        public int? DrugCount { get; set; }
        public decimal? Price { get; set; }
        public int? flag { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
