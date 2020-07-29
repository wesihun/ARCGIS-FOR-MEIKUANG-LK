using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class SexItemViewModel
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
    public class SexItemComparer : IEqualityComparer<SexItemViewModel>
    {
        public bool Equals(SexItemViewModel x, SexItemViewModel y)
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
            return x.ItemCode == y.ItemCode && x.Sex != y.Sex;
        }

        public int GetHashCode(SexItemViewModel obj)
        {
            return obj.ItemCode.GetHashCode();
        }
    }
}
