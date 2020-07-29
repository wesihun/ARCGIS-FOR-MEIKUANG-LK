using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("AllPowerfulDrug")]
    public class AllPowerfulDrugEntity
    {
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public string CommonName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CrowId { get; set; }
    }
}
