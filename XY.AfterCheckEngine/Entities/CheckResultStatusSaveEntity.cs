using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("Check_ResultStatusSave")]
    public class CheckResultStatusSaveEntity
    {
        public int CRowId { get; set; }
        public string SaveCheckResultStatus { get; set; }
    }
}
