using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class CheckResultCountViewModel
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string RulesCode { get; set; }
        public string RulesName { get; set; }
        public int Count { get; set; }
    }
}
