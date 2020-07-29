using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class OldDrugViewModel
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
        public int StartAge { get; set; }
        /// <summary>
        /// 结束年龄
        /// </summary>
        public int EndAge { get; set; }
        /// <summary>
        /// 当前年龄
        /// </summary>
        public int CurrentAge { get; set; }
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
    public class OldDrugComparer : IEqualityComparer<OldDrugViewModel>
    {
        public bool Equals(OldDrugViewModel x, OldDrugViewModel y)
        {
            y.Describe = x.Describe;
            return x.DrugCode == y.DrugCode && x.StartAge <= y.CurrentAge;
        }

        public int GetHashCode(OldDrugViewModel obj)
        {
            return obj.DrugCode.GetHashCode();
        }
    }
}
