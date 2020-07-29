using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：智能审核明细信息通用实体类
    /// 创 建 者：LK
    /// 创建日期：2019/9/10 10:19:48
    /// 最后修改者：LK
    /// 最后修改日期：2019/9/10 10:19:48
    /// </summary>
    public class ZnshTYDetailEntity
    {
        /// <summary>
        /// 住院登记编码
        /// </summary>
        public string HosRegisterCode { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 入院时间
        /// </summary>
        public DateTime? InHosDate { get; set; }
        /// <summary>
        /// 出院时间
        /// </summary>
        public DateTime? OutHosDate { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public int? InHosDay { get; set; }
        /// <summary>
        /// 主要诊断
        /// </summary>
        public string DiseaseName { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>
        public decimal? ZFY { get; set; }
        /// <summary>
        /// 药品费用
        /// </summary>
        public decimal? YPFY { get; set; }
        public decimal? YPFYp { get; set; }
        /// <summary>
        /// 中药费
        /// </summary>
        public decimal? ZYF { get; set; }
        public decimal? ZYFp { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal? XYF { get; set; }
        public decimal? XYFp { get; set; }
        /// <summary>
        /// 草药费
        /// </summary>
        public decimal? CYF { get; set; }
        public decimal? CYFp { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>
        public decimal? MYF { get; set; }
        public decimal? MYFp { get; set; }
        /// <summary>
        /// 诊疗费用
        /// </summary>
        public decimal? ZLFY { get; set; }
        public decimal? ZLFYp { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>
        public decimal? JCF { get; set; }
        public decimal? JCFp { get; set; }
        /// <summary>
        /// 化验费
        /// </summary>
        public decimal? HYF { get; set; }
        public decimal? HYFp { get; set; }
        /// <summary>
        /// 特检费
        /// </summary>
        public decimal? TJF { get; set; }
        public decimal? TJFp { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public decimal? CLF { get; set; }
        public decimal? CLFp { get; set; }
        /// <summary>
        /// 治疗费
        /// </summary>
        public decimal? ZLF { get; set; }
        public decimal? ZLFp { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>
        public decimal? OtherFY { get; set; }
        public decimal? OtherFYp { get; set; }
        public decimal? TCF { get; set; }
        public decimal? SSF { get; set; }
        public decimal? XUEYF { get; set; }
        public decimal? TZF { get; set; }
        public decimal? QTF { get; set; }
        /// <summary>
        /// 自费费用
        /// </summary>
        public decimal? ZFFY { get; set; }
        /// <summary>
        /// 检查检验费用
        /// </summary>
        public decimal? JCJYFY { get; set; }
        /// <summary>
        /// 支付费用
        /// </summary>
        public decimal? PAYFY { get; set; }
        /// <summary>
        /// 医保报销费用
        /// </summary>
        public decimal? YBBXFY { get; set; }
        /// <summary>
        /// 大病报销费用
        /// </summary>
        public decimal? DBBXBXFY { get; set; }
        /// <summary>
        /// 丙类费用
        /// </summary>
        public decimal? BLFY { get; set; }
        /// <summary>
        /// 目录外费用
        /// </summary>
        public decimal? MLWFY { get; set; }
        /// <summary>
        /// 目录内费用
        /// </summary>
        public decimal? MLNFY { get; set; }
        /// <summary>
        /// 人次数
        /// </summary>
        public string PersonCount { get; set; }
    }
}
