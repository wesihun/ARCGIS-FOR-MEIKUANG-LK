using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultStatus_New")]
    public class CheckResultStatusNewEntity
    {
        public int CRowId { get; set; }
        public string CheckResultStatus { get; set; }
        public string FunctionDesc { get; set; }
        public string Flag { get; set; }
    }
}
