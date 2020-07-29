using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities.Dtos
{
    public class RiskNoticeDto
    {
        public string RiskBH { get; set; }
        public string RiskName { get; set; }
        public string RiskLevel { get; set; }
        public string RiskFactor { get; set; }
        public string TroubleshootingItems { get; set; }
        public string AccidentType { get; set; }
        public string ControlMeasures { get; set; }
    }
}
