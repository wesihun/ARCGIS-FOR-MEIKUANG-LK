using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities.Dtos
{
    public class RiskPointDto
    {
        public string RiskBH { get; set; }
        public string RiskName { get; set; }
        public string OrgId { get; set; }

        public string OrgName { get; set; }
        /// <summary>
        /// 手机端获取检查点的检查状态 是否完成
        /// </summary>
        public string states { get; set; }
    }
}
