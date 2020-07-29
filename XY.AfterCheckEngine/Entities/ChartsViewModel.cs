using System;
using System.Collections.Generic;
using System.Text;

namespace XY.AfterCheckEngine.Entities
{
    /// <summary>
    /// 功能描述：图表展示用
    /// 创 建 者：LK
    /// 创建日期：2019/8/29 8:17:55
    /// 最后修改者：LK
    /// 最后修改日期：2019/8/29 8:17:55
    /// </summary>
    public class ChartsViewModel
    {
        public List<ChartsViewModel_YYZL> chartsViewModel_YYZL;        //用药诊疗
        public List<ChartsViewModel_ZY> chartsViewModel_ZY;        //住院      
    }
    public class ChartsViewModel_YYZL       //用药诊疗
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
    public class ChartsViewModel_ZY       //住院
    {
        public string YYJB { get; set; }
        public decimal? CJFY { get; set; }
        public string HosName { get; set; }
        public string YYJBBM { get; set; }
        public string InstitutionCode { get; set; }
    }
    public class ChartsViewModel_JZ       //就诊
    {
        public string Name { get; set; }
        public int? Count { get; set; }
    }

    public class ObjectT
    {
        public string NAME { get; set; }
        public decimal? CJZF { get; set; }

        public decimal? CJFY { get; set; }

        public decimal? PRICE { get; set; }
    }

    public class Map_YYXXInfo
    {
        public int? CWCount { get; set; }
        public int? ZYPersonCount { get; set; }
    }
    public class Map_XY
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string X { get; set; }
        public string Y { get; set; }  
        public bool Flag { get; set; }
        public int? CWS { get; set; }
        public int? ZYS { get; set; }
        public string YLJGBH { get; set; }
    }
    public class DrugZLCharts   //用药诊疗图表
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        /// <summary>
        /// 中药费
        /// </summary>
        public decimal? Price1 { get; set; }
        /// <summary>
        /// 中药费百分比
        /// </summary>
        public decimal? Price1p { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal? Price2 { get; set; }
        public decimal? Price2p { get; set; }
        /// <summary>
        /// 草药费
        /// </summary>
        public decimal? Price3 { get; set; }
        public decimal? Price3p { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>
        public decimal? Price4 { get; set; }
        public decimal? Price4p { get; set; }
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
        public decimal? QTF { get; set; }
        public decimal? QTFp { get; set; }
    }
    public class BeforeDrugZLCharts   //事前用药诊疗图表
    {
        public string KSBM { get; set; }
        public string KSMC { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal? XYF { get; set; }
        /// <summary>
        ///  中成药费
        /// </summary>
        public decimal? ZCYF1 { get; set; }
        /// <summary>
        ///  中草药费
        /// </summary>
        public decimal? ZCYF2 { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>
        public decimal? MYF { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>
        public decimal? JCF { get; set; }
        /// <summary>
        /// 检验费
        /// </summary>
        public decimal? JYF { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public decimal? CLF { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>
        public decimal? QTF { get; set; }
    }
    public class DrugCharts      //用药图表
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        /// <summary>
        /// 中药费
        /// </summary>
        public decimal? Price1 { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal? Price2 { get; set; }
        /// <summary>
        /// 草药费
        /// </summary>
        public decimal? Price3 { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>
        public decimal? Price4 { get; set; }
    }
    public class ZLCharts
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>
        public decimal? JCF { get; set; }
        /// <summary>
        /// 化验费
        /// </summary>
        public decimal? HYF { get; set; }
        /// <summary>
        /// 特检费
        /// </summary>
        public decimal? TJF { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public decimal? CLF { get; set; }
        /// <summary>
        /// 治疗费
        /// </summary>
        public decimal? ZLF { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>
        public decimal? QTF { get; set; }
    }
    public class JZCharts
    {
        public string HospitalName { get; set; }
        public string TimeName { get; set; }
        public int? Count { get; set; }
    }
    /// <summary>
    /// 住院人次分流情况
    /// </summary>
    public class FLCountCharts
    {
        public string InstitutionCode { get; set; }
        public string LevelName { get; set; }
        public int? PersonCount { get; set; }
        public string HospitalName { get; set; }
    }
    /// <summary>
    /// 住院基金分流情况
    /// </summary>
    public class FundCharts
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        public string LevelName { get; set; }

        public decimal? FundPrice { get; set; }
    }

    public class ChartsByDrugName
    {
        public List<ChartsByDrugName_YYMC> ListByYYMC;

        public List<ChartsByDrugName_Age> ListByAge;
    }
    public class ChartsByDrugName_YYMC
    {
        public string YYMC { get; set; }

        public string YPMC { get; set; }

        public int? SL { get; set; }
    }
    public class ChartsByDrugName_Age
    {
        public string PersonalAge { get; set; }

        public string YPMC { get; set; }

        public int? SL { get; set; }
    }
    public class ChartsByDiseaseName
    {
        public List<ChartsByDiseaseName_YYMC> ListByYYMC;

        public List<ChartsByDiseaseName_Age> ListByAge;
    }
    public class ChartsByDiseaseName_YYMC
    {
        public string YYMC { get; set; }

        public string DiseaseName { get; set; }

        public int? SL { get; set; }
    }
    public class ChartsByDiseaseName_Age
    {
        public string PersonalAge { get; set; }

        public string DiseaseName { get; set; }

        public int? SL { get; set; }
    }
    /// <summary>
    /// 丙类目录外费用占比分析 次均费用和次均支付对比分析 各个医院药占比分析
    /// </summary>
    public class ThreeCharts
    {
        public List<Charts1> ChartsOne;
        public List<Charts2> ChartsTwo;
        public List<Charts3> ChartsThree;
    }
    /// <summary>
    /// 丙类目录外费用占比分析
    /// </summary>
    public class Charts1
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        public string YYJB { get; set; }
        public decimal? MLWFY { get; set; }
    }
    /// <summary>
    /// 次均费用和次均支付对比分析
    /// </summary>
    public class Charts2
    {
        public string InstitutionCode { get; set; }
        public string HospitalName { get; set; }
        public string YYJB { get; set; }
        public decimal CJFY { get; set; }
        public decimal CJZF { get; set; }
        public decimal ZFY { get; set; }
        public decimal YBBXFY { get; set; }
        public decimal DBBXBXFY { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 各个医院药占比分析
    /// </summary>
    public class Charts3
    {
        public string InstitutionName { get; set; }
        public decimal? YZB { get; set; }
    }
    /// <summary>
    /// 各个科室药占比分析
    /// </summary>
    public class KSZB
    {
        public string KSMC { get; set; }
        public string KSBM { get; set; }
        public decimal? YZB { get; set; }
    }
    public class HZJK
    {
        public List<BeforeHZXX> BeforeHzxxList;
        public List<BeforeCF> BeforeCFList;
    }
    /// <summary>
    /// 事前患者信息
    /// </summary>
    public class BeforeHZXX
    {
        public string RegisterCode { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int? Age { get; set; }
        public string IdNumber { get; set; }
        public string CWH { get; set; }
        public DateTime? RYRQ { get; set; }
        public DateTime? CYRQ { get; set; }
        public int? ZYTS { get; set; }
        public string ZYZD { get; set; }
        public string ZYZDDM { get; set; }
        public string MZZD { get; set; }
        public string MZZDDM { get; set; }
        public decimal? ZFY { get; set; }
        public decimal? WGFY { get; set; }
        /// <summary>
        /// 西药费
        /// </summary>
        public decimal? XYF { get; set; }
        /// <summary>
        ///  中成药费
        /// </summary>
        public decimal? ZCYF1 { get; set; }
        /// <summary>
        ///  中草药费
        /// </summary>
        public decimal? ZCYF2 { get; set; }
        /// <summary>
        /// 蒙药费
        /// </summary>
        public decimal? MYF { get; set; }
        /// <summary>
        /// 检查费
        /// </summary>
        public decimal? JCF { get; set; }
        /// <summary>
        /// 检验费
        /// </summary>
        public decimal? JYF { get; set; }
        /// <summary>
        /// 材料费
        /// </summary>
        public decimal? CLF { get; set; }
        /// <summary>
        /// 其他费
        /// </summary>
        public decimal? QTF { get; set; }

    }
    /// <summary>
    /// 事前违规描述
    /// </summary>
    public class BeforeWGDescribe
    {
        public string Describe { get; set; }
    }
    /// <summary>
    /// 事前处方
    /// </summary>
    public class BeforeCF
    {
        public string XMBM { get; set; }
        public string XMMC { get; set; }
        public decimal? DJ { get; set; }
        public int? Count { get; set; }
        public decimal? Price { get; set; }
        public decimal? SJJE { get; set; }
    }
    /// <summary>
    /// 事前科室违规人数占比
    /// </summary>
    public class BesfroeKSZB
    {
        public string KSBM { get; set; }
        public string KSMC { get; set; }

        public string ZB { get; set; }
    }
    /// <summary>
    /// 事前违规医生人次数  (处方表里的开单科室)
    /// </summary>
    public class BesfroeWGYS
    {
        public string KSBM { get; set; }
        public string KSMC { get; set; }
        public string KDYSXM { get; set; }

        public int? SL { get; set; }
    }
    /// <summary>
    /// 药占比 次均费用和次均支付 丙类目录外费用占比
    /// </summary>
    public class MapCharts
    {
       public YZBCharts yzbcharts { get; set; }
       public CJFYCharts cjfycharts { get; set; }
       public BLMLWFYCharts blmlwfycharts { get; set; }
    }
    /// <summary>
    /// 药占比 
    /// </summary>
    public class YZBCharts
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public decimal? YZB { get; set; }
    }
    /// <summary>
    /// 次均费用和次均支付
    /// </summary>
    public class CJFYCharts
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public decimal? CJFY { get; set; }
        public decimal? CJZF { get; set; }
    }
    /// <summary>
    /// 丙类目录外费用占比
    /// </summary>
    public class BLMLWFYCharts
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public decimal? MLWFY { get; set; }
    }
    /// <summary>
    /// 各项费用金额 按入院月份统计单个医院就诊人数 丙类目录外费用和总费用
    /// </summary>
    public class MapList
    {
        public List<YZBList> yzblist { get; set; }
        public List<CJFYList> cjfylist { get; set; }
        public List<BLMLWFYList> blmlwfylist { get; set; }
    }
    /// <summary>
    /// 各项费用金额
    /// </summary>
    public class YZBList
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string FEETYPE { get; set; }
        public decimal? FEE { get; set; }
    }
    /// <summary>
    /// 按入院月份统计单个医院就诊人数
    /// </summary>
    public class CJFYList
    {
        public string InHosMonth { get; set; }
        public int? JZRCS { get; set; }
    }
    /// <summary>
    /// 丙类目录外费用和总费用
    /// </summary>
    public class BLMLWFYList
    {
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string FEETYPE { get; set; }
        public decimal? FEE { get; set; }
    }

}
