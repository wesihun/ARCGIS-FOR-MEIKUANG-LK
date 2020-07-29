using System;
using System.Collections.Generic;
using System.Text;

namespace XY.Universal.Models.ViewModels
{
    public class MedicalLimitDrugViewModel
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
        /// ICD编码
        /// </summary>
        public string ICDCode { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>
        public string DiseaseName { get; set; }
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
    public class MedicalLimitDrugComparer : IEqualityComparer<MedicalLimitDrugViewModel>
    {
        public bool Equals(MedicalLimitDrugViewModel x, MedicalLimitDrugViewModel y)
        {
            y.Describe = x.Describe;
            return x.DrugCode.Trim() == y.DrugCode.Trim() && x.ICDCode.Trim() != y.ICDCode.Trim();
        }

        public int GetHashCode(MedicalLimitDrugViewModel obj)
        {
            return obj.DrugCode.GetHashCode();
        }
    }
}
