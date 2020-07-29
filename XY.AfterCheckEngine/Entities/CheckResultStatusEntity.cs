using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultStatus")]
    public class CheckResultStatusEntity
    {
        public int CRowId { get; set; }
        public string CheckResultStatus { get; set; }
        public string CheckNumber { get; set; }
        public string CheckPreNumber { get; set; }
        public int CheckCount { get; set; }
    }
}
