using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    /// <summary>
    /// 诊疗项目超限价使用
    /// </summary>
    public class ItemLimitPriceViewModel
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 项目单价
        /// </summary>
        public decimal ItemPrice { get; set; }
        /// <summary>
        /// 项目限价
        /// </summary>
        public decimal ItemLimitPrice { get; set; }
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
    }



    public class ItemLimitPriceComparer : IEqualityComparer<ItemLimitPriceViewModel>
    {
        public bool Equals(ItemLimitPriceViewModel x, ItemLimitPriceViewModel y)
        {
            y.ItemLimitPrice = x.ItemLimitPrice;
            return x.ItemCode == y.ItemCode && x.ItemLimitPrice < y.ItemPrice;
        }

        public int GetHashCode(ItemLimitPriceViewModel obj)
        {
            return obj.ItemCode.GetHashCode();
        }
    }
}
