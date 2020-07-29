using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_DrugStatus")]
    public class CheckDrugStatusEntity
    {
        public int? CRowId { get; set; }
        public string CheckResultStatus { get; set; }
        public string FunctionDesc { get; set; }
        public string Flag { get; set; }
    }
}
