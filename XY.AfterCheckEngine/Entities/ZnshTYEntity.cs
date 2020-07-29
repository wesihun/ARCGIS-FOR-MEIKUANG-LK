using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：智能审核新开发通用实体
    /// 创 建 者：LK
    /// 创建日期：2019/8/28 14:59:06
    /// 最后修改者：LK
    /// 最后修改日期：2019/8/28 14:59:06
    /// </summary>


    /// <summary>
    /// 总体统计数
    /// </summary>
    public class TotalEntity
    {
        /// <summary>
        /// 总床位数
        /// </summary>
        public int? CWCount { get; set; }
        /// <summary>
        /// 总在院人数(人数)
        /// </summary>
        public int? ZYCount { get; set; }
        /// <summary>
        /// 床位数与在院人数不符合标识
        /// </summary>
        public bool flag { get; set; }
        /// <summary>
        /// 总门诊就诊数
        /// </summary>
        public int? MZJZCount { get; set; }
        /// <summary>
        /// 当月门诊就诊数
        /// </summary>
        public int? MZJZMothCount { get; set; }
        /// <summary>
        /// 总住院就诊数
        /// </summary>
        public int? ZYJZCount { get; set; }
        /// <summary>
        /// 当月住院就诊数
        /// </summary>
        public int? ZYJZMothCount { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RankEntity
    {
        /// <summary>
        /// 疾病排行
        /// </summary>
        public List<Disease> disease;   
        /// <summary>
        /// 西药用量排行
        /// </summary>
        public List<Drug_XY> drug_xy;
        /// <summary>
        /// 各医院自费费用排行
        /// </summary>
        public List<ZFFY> zffy;
        /// <summary>
        /// 各医院检查检验费用排行
        /// </summary>
        public List<JCJYF> jcjyf;
    }
    public class Disease
    {
        public string DiseaseName { get; set; }

        public int? SL { get; set; }

        public decimal? CJFY { get; set; }
    }
    public class Drug_XY
    {
        public string YPMC { get; set; }

        public int? SL { get; set; }
    }
    public class ZFFY
    {
        public string InstitutionCode { get; set; }
        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
    public class JCJYF
    {
        public string InstitutionCode { get; set; }
        public string Name { get; set; }

        public decimal? Price { get; set; }
    }

    public class BedInHospitalCharts
    {
        public string HospitalName { get; set; }

        public int? CWCount { get; set; }

        public int? ZYPersonCount { get; set; }

        public bool flag { get; set; }

        public int? cws { get; set; }

        public int? zys { get; set; }
        public string KSBH { get; set; }

        public string YLJGBH { get; set; }
    }
    public class PersonInfo
    {
        public string CRowId { get; set; }
        public string YLJGBH { get; set; }
        public string ZYBH { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
    }
    public class FlagEntity
    {
        public string YLJGBH { get; set; }

        public string KSBH { get; set; }
        /// <summary>
        /// 床位数
        /// </summary>
        public int? CWS { get; set; }
        /// <summary>
        /// 住院数
        /// </summary>
        public int? ZYS { get; set; }
    }
    public class MLFY
    {
        public string MLWFY { get; set; }
        public string MLWFYp { get; set; }
        public string MLNFY { get; set; }
        public string MLNFYp { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
    }
}
