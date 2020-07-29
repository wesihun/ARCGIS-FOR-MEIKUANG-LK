using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    [SugarTable("YB_ClinicInfo")]
    public class YBClinicInfoEntity
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
        /// 门诊登记编码
        /// </summary>		
        public string ClinicRegisterCode { get; set; }
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
        public int? Age { get; set; }
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
        /// 就诊时间
        /// </summary>		
       
        public DateTime ClinicDate { get; set; }
        /// <summary>
        /// 就诊年度
        /// </summary>		
        public int? ClinicDateYear { get; set; }
        /// <summary>
        /// 就诊月份
        /// </summary>		
        public int? ClinicDateMonth { get; set; }
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
        public decimal? ZFY { get; set; }
        /// <summary>
        /// 医保报销费用
        /// </summary>		
        public decimal? YBBXFY { get; set; }
        /// <summary>
        /// 个人自付费用
        /// </summary>		
        public decimal? GRZFFY { get; set; }
        /// <summary>
        /// 目录内费用
        /// </summary>		
        public decimal? MLNFY { get; set; }
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
        public decimal? ZYF { get; set; }
        /// <summary>
        /// 草药费
        /// </summary>		
        public decimal? CYF { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>		
        public decimal? MYF { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>		
        public decimal? JCF { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>		
        public decimal? CLF { get; set; }
        /// <summary>
        /// 特材费
        /// </summary>		
        public decimal? TCF { get; set; }
        /// <summary>
        /// 治疗费
        /// </summary>		
        public decimal? ZLF { get; set; }
        /// <summary>
        /// 化验费
        /// </summary>		
        public decimal? HYF { get; set; }
        /// <summary>
        /// 手术费
        /// </summary>		
        public decimal? SSF { get; set; }
        /// <summary>
        /// 血液费
        /// </summary>		
        public decimal? XUEYF { get; set; }
        /// <summary>
        /// 特检费
        /// </summary>		
        public decimal? TJF { get; set; }
        /// <summary>
        /// 特治费
        /// </summary>		
        public decimal? TZF { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>		
        public decimal? QTF { get; set; }
        /// <summary>
        /// 靶向药费
        /// </summary>		
        public decimal? BXYF { get; set; }
        /// <summary>
        /// 11门诊统筹，12门诊慢性病，13门诊特殊病
        /// </summary>		
        public string ClinicType { get; set; }

        public DateTime? ClinicDates { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string States { get; set; }
    }
}
