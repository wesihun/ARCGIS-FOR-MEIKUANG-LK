using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("YB_HosInfo")]
    public class YBHosInfoEntity
    {
        /// <summary>
        /// 行政区划编码（到县级）
        /// </summary>		
        public string CityAreaCode { get; set; }
        /// <summary>
        /// 例如：呼和浩特市-武川县
        /// </summary>		
        public string CityAreaName { get; set; }
        /// <summary>
        /// 年度
        /// </summary>		
        public string Year { get; set; }
        /// <summary>
        /// 住院登记编码
        /// </summary>		
        public string HosRegisterCode { get; set; }
        /// <summary>
        /// 个人编码
        /// </summary>		
        public string PersonalCode { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>		
        public string IdNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>		
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>		
        public string Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>		
        public int Age { get; set; }
        /// <summary>
        /// 人员类型编码
        /// </summary>		
        public string PersonalTypeCode { get; set; }
        /// <summary>
        /// 人员类型名称
        /// </summary>		
        public string PersonalTypeName { get; set; }
        /// <summary>
        /// 民族编码
        /// </summary>		
        public string FolkCode { get; set; }
        /// <summary>
        /// 民族名称
        /// </summary>		
        public string FolkName { get; set; }
        /// <summary>
        /// 个人所属行政区划编码（到村级）
        /// </summary>		
        public string VillageAreaCode { get; set; }
        /// <summary>
        /// 个人所属行政区划名称（到村级）
        /// </summary>		
        public string VillageAreaName { get; set; }
        /// <summary>
        /// 入院时间
        /// </summary>		
        public DateTime InHosDate { get; set; }
        /// <summary>
        /// 出院时间
        /// </summary>		
        public DateTime OutHosDate { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>		
        public int InHosDay { get; set; }
        /// <summary>
        /// 补偿年度
        /// </summary>		
        public string CompYear { get; set; }
        /// <summary>
        /// 疾病ICD编码
        /// </summary>		
        public string ICDCode { get; set; }
        /// <summary>
        /// 疾病名称
        /// </summary>		
        public string DiseaseName { get; set; }
        /// <summary>
        /// 就诊机构编码
        /// </summary>		
        public string InstitutionCode { get; set; }
        /// <summary>
        /// 就诊机构名称
        /// </summary>		
        public string InstitutionName { get; set; }
        /// <summary>
        /// 就诊医院级别编码
        /// </summary>		
        public string InstitutionLevelCode { get; set; }
        /// <summary>
        /// 就诊医院级别名称
        /// </summary>		
        public string InstitutionLevelName { get; set; }
        /// <summary>
        /// 就诊医院等级编码
        /// </summary>		
        public string InstitutiongGradeCode { get; set; }
        /// <summary>
        /// 就诊医院等级名称
        /// </summary>		
        public string InstitutiongGradeName { get; set; }
        /// <summary>
        /// 总费用
        /// </summary>		
        public decimal ZFY { get; set; }
        /// <summary>
        /// 医保报销费用
        /// </summary>		
        public decimal YBBXFY { get; set; }
        /// <summary>
        /// 大病保险报销费用
        /// </summary>		
        public decimal DBBXBXFY { get; set; }
        /// <summary>
        /// 医疗救助费用
        /// </summary>		
        public decimal YLJZFY { get; set; }
        /// <summary>
        /// 商业保险报销费用
        /// </summary>		
        public decimal SYBXBXFY { get; set; }
        /// <summary>
        /// 政府兜底金额
        /// </summary>		
        public decimal ZFDDJE { get; set; }
        /// <summary>
        /// 其他补充金额
        /// </summary>		
        public decimal QTBCJE { get; set; }
        /// <summary>
        /// 个人自付费用
        /// </summary>		
        public decimal GRZFFY { get; set; }
        /// <summary>
        /// 目录内费用
        /// </summary>		
        public decimal MLNFY { get; set; }
        /// <summary>
        /// 目录外费用
        /// </summary>		
        public decimal? MLWFY { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>		
        public decimal? XYF { get; set; }
        /// <summary>
        /// 中药费
        /// </summary>		
        public decimal ZYF { get; set; }
        /// <summary>
        /// 草药费
        /// </summary>		
        public decimal CYF { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>		
        public decimal MYF { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>		
        public decimal JCF { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>		
        public decimal CLF { get; set; }
        /// <summary>
        /// 特材费
        /// </summary>		
        public decimal TCF { get; set; }
        /// <summary>
        /// 治疗费
        /// </summary>		
        public decimal ZLF { get; set; }
        /// <summary>
        /// 化验费
        /// </summary>		
        public decimal HYF { get; set; }
        /// <summary>
        /// 手术费
        /// </summary>		
        public decimal SSF { get; set; }
        /// <summary>
        /// 血液费
        /// </summary>		
        public decimal XUEYF { get; set; }
        /// <summary>
        /// 特检费
        /// </summary>		
        public decimal TJF { get; set; }
        /// <summary>
        /// 特治费
        /// </summary>		
        public decimal TZF { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>		
        public decimal QTF { get; set; }
        /// <summary>
        /// 靶向药费
        /// </summary>		
        public decimal BXYF { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string States { get; set; }
        /// <summary>
        /// 入院年度
        /// </summary>
        public string InHosYear { get; set; }
        /// <summary>
        /// 入院月份
        /// </summary>
        public string InHosMonth { get; set; }
        /// <summary>
        /// 出院年度
        /// </summary>
        public string OutHosYear { get; set; }
        /// <summary>
        /// 出院月份
        /// </summary>
        public string OutHosMonth { get; set; }
        /// <summary>
        /// 补偿类型
        /// </summary>
        public string CompType { get; set; }
    }
}
