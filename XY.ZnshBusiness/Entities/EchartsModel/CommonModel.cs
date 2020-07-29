using System;
using System.Collections.Generic;
using System.Text;

namespace XY.ZnshBusiness.Entities.EchartsModel
{
    public class CommonModel
    {
        public string Name { get; set;}
        public string Value { get; set;}
        public int Count { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        public decimal Percentum { get; set; }
    }

    public class RiskLevelCount
    {
        public string Name { get; set; }

        public List<int> Counts;
    }

    public class CommonModelNameCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
    public class Hidden
    {
        public int Count { get; set; }
    }

    public class HiddenTJ
    {
        public string Name { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
    }
}
