using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：申诉管理实体  主要针对100%违规 和 疑似违规合并用
    /// 创 建 者：LK
    /// 创建日期：2019/9/23 17:01:55
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/23 17:01:55
    /// </summary>
    public class ComplaintMainEntity
    {
        public string ComplaintCode { get; set; }
        public string CheckResultInfoCode { get; set; }
        public string RulesName { get; set; }
        public string RulesCode { get; set; }
        public string RegisterCode { get; set; }
        public string PersonalCode { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string ICDCode { get; set; }
        public string DiseaseName { get; set; }
        public string FirstTrialDescribe { get; set; }
        public string IsPre { get; set; }
        public string ComplaintResultStatus { get; set; }

        public string ComplaintDescribe { get; set; }
    }
}
