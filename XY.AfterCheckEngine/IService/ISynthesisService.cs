using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Universal.Models.ViewModels;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.IService
{
    /// <summary>
    /// 功能描述：智能审核改主题
    /// 创 建 者：LK
    /// 创建日期：2019/8/28 14:51:13
    /// 最后修改者：LK
    /// 最后修改日期：2019/8/28 14:51:13
    /// </summary>
    public interface ISynthesisService
    {
        #region 获取数据
        /// <summary>
        /// 获取总体概况总数
        /// </summary>
        /// <returns></returns>
        TotalEntity GetTotal();
        /// <summary>
        /// 获取总床位数与总在院人数匹配标识
        /// </summary>
        /// <returns></returns>
        bool GetFlag();
        /// <summary>
        /// 获取医院科室床位数与在院人数匹配标识
        /// </summary>
        /// <returns></returns>
        List<string> GetYYFlag();
        /// <summary>
        /// 床位在院人数弹出图表
        /// </summary>
        /// <returns></returns>
        List<BedInHospitalCharts> GetBedInHospitalCharts(int hoscount);
        /// <summary>
        /// 床位在院人数弹出列表
        /// </summary>
        /// <returns></returns>
        List<BedInHospitalCharts> GetBedInHospitalList(string name, int page, int limit, ref int count);
        /// <summary>
        /// 获取科室床位信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BedInHospitalCharts> GetKSBedInfoList(string code, int page, int limit, ref int count);
        /// <summary>
        /// 获取多个图表数据(全盟用药、诊疗服务情况 住院次均费用对比分析)
        /// </summary>
        /// <returns></returns>
        ChartsViewModel GetManayCharts();
        /// <summary>
        /// 获取全盟用药、诊疗服务情况弹出图表
        /// </summary>
        /// <returns></returns>
        List<DrugZLCharts> GetDrugZLCharts(int hoscount);
        /// <summary>
        /// 获取全盟用药、诊疗服务情况弹出列表
        /// </summary>
        /// <returns></returns>
        List<DrugZLCharts> GetDrugZLList(int page, int limit, ref int count);
        /// <summary>
        /// 全盟用药、诊疗服务钻取列表
        /// </summary>
        /// <param name="InstitutionCode"></param>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQDrugZLList(string InstitutionCode, int page, int limit, ref int count);
        /// <summary>
        /// 钻取疾病list
        /// </summary>
        /// <param name="InstitutionCode"></param>
        /// <param name="diseasename">疾病名称</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQDiseaseListByName(string InstitutionCode, string diseasename, int page, int limit, ref int count);
        /// 获取目录费用占比
        List<MLFY> GetMLFYpList();
        /// 获取各个医院目录费用
        List<MLFY> GetHosMLFYList(int page, int limit, ref int count);
        /// <summary>
        /// 住院次均费用对比分析弹出
        /// </summary>
        /// <param name="levelcode">医院等级编码</param>
        /// <param name="hosname">医院名称 查询用</param>
        /// <param name="cjfy">次均费用</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ChartsViewModel_ZY> GetTCZYCJList(string levelcode,string hosname, decimal cjfy, int page, int limit, ref int count);
        /// <summary>
        /// 获取单个图表数据(就诊人次当日)
        /// </summary>
        /// <returns></returns>
        List<ChartsViewModel_JZ> GetSingleCharts(string yljgbh, int second,int count);
        /// <summary>
        /// 获取排名
        /// </summary>
        /// <returns></returns>
        RankEntity GetRank();
        /// <summary>
        /// 主要疾病排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TJ_JBPMTJEntity> GetDiseaseList(string name,int page, int limit, ref int count);
        /// <summary>
        /// 钻取主要疾病排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TJ_JBAYYTJEntity> GetZQDiseaseList(string keyword,string diseaseName,int page, int limit, ref int count);
        /// <summary>
        /// 主要西药排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TJ_QSXYYLEntity> GetXYList(string name, int page, int limit, ref int count);
        /// <summary>
        /// 医院自费费用排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ZFFY> GetZFFYList(string name, int page, int limit, ref int count);
        /// <summary>
        /// 医院钻取自费费用排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQZFFYList(string InstitutionCode, int page, int limit, ref int count);
        /// <summary>
        /// 主要检查检验费排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<JCJYF> GetJCJYList(string name, int page, int limit, ref int count);
        /// <summary>
        /// 钻取主要检查检验费排名list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQJCJYList(string InstitutionCode, int page, int limit, ref int count);
        /// <summary>
        /// 根据药品名称弹出图表
        /// </summary>
        /// <param name="ypmc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ChartsByDrugName GetChartsByDrugNameCharts(string ypmc, int count);
        /// <summary>
        /// 根据药品名称弹出list
        /// </summary>
        /// <param name="ypmc"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ChartsByDrugName_YYMC> GetChartsByDrugNameList(string ypmc, int page, int limit, ref int count);
        /// <summary>
        /// 根据疾病名称弹出图表
        /// </summary>
        /// <param name="jbmc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ChartsByDiseaseName GetChartsByDiseaseNameCharts(string jbmc, int count);
        /// <summary>
        /// 根据疾病名称弹出list
        /// </summary>
        /// <param name="jbmc"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ChartsByDiseaseName_YYMC> GetChartsByDiseaseNameList(string jbmc, int page, int limit, ref int count);
        /// <summary>
        /// 获取医院信息事前  主要是坐标
        /// </summary>
        /// <returns></returns>
        List<Map_XY> GetMapInfo();
        /// <summary>
        /// 获取医院信息  主要是坐标
        /// </summary>
        /// <returns></returns>
        List<Map_XY> GetMapXY();
        /// <summary>
        /// 获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Map_YYXXInfo GetMapYYXXInfo(string code);
        /// <summary>
        /// 获取医院机构树
        /// </summary>
        /// <returns></returns>
        List<TreeModule> GetYYXXTreeList();
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="yljgbh">医疗机构编号</param>
        /// <returns></returns>
        List<PersonInfo> GetPersonInfo(string code);
        // <summary>
        /// 获取住院人次分流情况
        /// </summary>
        /// <returns></returns>
        List<FLCountCharts> GetInHospitalFLList();
        /// <summary>
        /// 根据医院级别获取医院人次分流数据
        /// </summary>
        /// <param name="levelcode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<FLCountCharts> GetTCInHospitalFLList(string levelcode, int page, int limit, ref int count);
        // <summary>
        /// 获取住院基金分流情况
        /// </summary>
        /// <returns></returns>
        List<FundCharts> GetInHospitalFundList();
        /// <summary>
        /// 根据医院级别获取医院基金分流数据
        /// </summary>
        /// <param name="levelcode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<FundCharts> GetTCInHospitalFundList(string levelcode, int page, int limit, ref int count);
        /// <summary>
        /// 钻取统计概况前4个图表list
        /// </summary>
        /// <param name="InstitutionCode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQChartsFourList(string InstitutionCode, int page, int limit, ref int count);
        // <summary>
        /// 丙类目录外费用占比分析 次均费用和次均支付对比分析 各个医院药占比分析
        /// </summary>
        /// <returns></returns>
        ThreeCharts GetThreeChartsList(int count);
        /// <summary>
        /// 各个医院药占比分析list
        /// </summary>
        /// <returns></returns>
        List<Charts3> GetCharts3List(int page, int limit, ref int count);
        /// <summary>
        /// 丙类目录外费用占比分析弹出
        /// </summary>
        /// <param name="levelcode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Charts1> GetTCCharts1List(string levelcode, int page, int limit, ref int count);
        /// <summary>
        /// 次均费用和次均支付对比分析弹出
        /// </summary>
        /// <param name="levelcode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Charts2> GetTCCharts2List(string levelcode, int page, int limit, ref int count);
        /// <summary>
        /// 获取地图界面图表
        /// </summary>
        /// <returns></returns>
        MapCharts GetMapCharts(string code);
        /// <summary>
        /// 获取地图界面list
        /// </summary>
        /// <returns></returns>
        MapList GetMapList(string code);
        /// <summary>
        /// 钻取获取地图界面药占比list
        /// </summary>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQMapYZBList(string InstitutionCode,int page, int limit, ref int count);
        /// <summary>
        /// 钻取获取地图界面次均费用list
        /// </summary>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQMapCJFYList(string InstitutionCode, int moth, int page, int limit, ref int count);
        /// <summary>
        /// 钻取获取地图界面目录外占比list
        /// </summary>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetZQMapMLWZBList(string InstitutionCode, int page, int limit, ref int count);
        /// <summary>
        /// 获取医嘱list
        /// </summary>
        /// <returns></returns>
        List<HisZyyzmxDto> GetYZList(string yljgbh,string zybh);
        /// <summary>
        /// 获取挂床人详细信息
        /// </summary>
        /// <returns></returns>
        HisZydjEntity GetGCPersonList(string crowid);
        /// <summary>
        /// 获取处方明细
        /// </summary>
        /// <param name="HosRegisterCode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<YBHosPreInfoEntity> GetPreList(string HosRegisterCode, int page, int limit, ref int count);
        /// <summary>
        /// 获取审核结果列表按照规则分组 (住院)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StaticsViewModel> GetStaticsViewsByRule(string rulesName, int page, int limit, ref int count);
        /// <summary>
        /// 根据规则编码获取各个医院违规人数
        /// </summary>
        /// <param name="rulecode"></param>
        /// <returns></returns>
        List<StaticsViewModel> GetStaticsViewsJGMCByRule(string rulecode, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据条件获取住院列表
        /// </summary>
        /// <param name="jgcode"></param>
        /// <param name="rulecode"></param>
        /// <param name="djcode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<YBHosInfoEntityDto> GetYBHosInfoList(string personName, string jgcode, string rulecode, string djcode, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取处方明细list 住院
        /// </summary>
        /// <param name="hosregistercode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<YBHosPreInfoEntity> GetCFDeatilList(string hosregistercode, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取违规处方列表(住院)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string key, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取审核结果列表按照医院等级分组
        /// </summary>
        /// <returns></returns>
        List<StaticsViewModel> GetStaticsViewsByJGJB();
        /// <summary>
        /// 根据机构等级获取机构各个规则违规人数
        /// </summary>
        /// <param name="djcode"></param>
        /// <param name="jgbm"></param>
        /// <returns></returns>
        List<StaticsViewModel> GetStaticsViewsJGMCByDJ(string djcode, string jgbm, int pageIndex, int pageSize, ref int totalCount);
        /// 获取医院列表
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        List<StaticsViewModel> GetHosListByLevel(string level, int pageIndex, int pageSize, ref int totalCount);
        #endregion
    }
}
