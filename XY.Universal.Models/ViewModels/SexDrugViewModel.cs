using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class SexDrugViewModel
    {
        /// <summary>
        /// 药品编码
        /// </summary>
        public string DrugCode { get; set; }
        /// <summary>
        /// 药品名称
        /// </summary>
        public string DrugName { get; set; }
        /// <summary>
        /// 开始年龄
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 处方编码
        /// </summary>
        public string PreCode { get; set; }
        /// <summary>
        /// 项目序号
        /// </summary>
        public int ItemIndex { get; set; }
        /// <summary>
        /// 住院登记编码
        /// </summary>
        public string RegisterCode { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
    public class SexDrugComparer : IEqualityComparer<SexDrugViewModel>
    {
        public bool Equals(SexDrugViewModel x, SexDrugViewModel y)
        {
            y.Describe = x.Describe;
            if (y.Sex == "男性")
                y.Sex = "1";
            else if (y.Sex == "女性")
                y.Sex = "2";
            if (x.Sex == "男性")
                x.Sex = "1";
            else if (x.Sex == "女性")
                x.Sex = "2";
            return x.DrugCode == y.DrugCode && x.Sex == y.Sex;
        }

        public int GetHashCode(SexDrugViewModel obj)
        {
            return obj.DrugCode.GetHashCode();
        }
    }
}
