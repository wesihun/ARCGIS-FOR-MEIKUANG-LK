using System;
using System.Collections.Generic;
using System.Text;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.EchartsModel;

namespace XY.ZnshBusiness.IService
{
    public interface IStatisticalService
    {
        /// <summary>
        /// 风险信息统计
        /// </summary>
        /// <returns></returns>
        List<CommonModel> GetRiskInfoTJ(string orgid);
        /// <summary>
        /// 风险信息统计按部门分
        /// </summary>
        /// <returns></returns>
        List<RiskInfoByDep> GetRiskInfoTJByDep(string orgid);
        /// <summary>
        /// 大数据分析
        /// </summary>
        /// <returns></returns>
        List<CommonModel> GetDataAnalysis(string orgid, DateTime date);
        /// <summary>
        /// 风险等级数量
        /// </summary>
        /// <returns></returns>
        List<RiskLevelCount> GetRiskLevelCount(string orgid, int year);
        /// <summary>
        /// 风险辨识数量
        /// </summary>
        /// <returns></returns>
        List<RiskLevelCount> GetRiskIdentifyCount(string orgid, int year);
        /// <summary>
        /// 风险因素类别统计
        /// </summary>
        /// <returns></returns>
        List<CommonModelNameCount> GetRiskFactorTypeTJ(string orgid);
        /// <summary>
        /// 可能导致风险事故类型统计
        /// </summary>GetRiskAccidentTypeTJ
        /// <returns></returns>
        List<CommonModelNameCount> GetRiskAccidentTypeTJ(string orgid);
        /// <summary>
        /// 风险等级数量
        /// </summary>
        /// <returns></returns>
        List<RiskLevelCount> GetRiskByLevelTJ(string orgid);
        /// <summary>
        /// 隐患趋势统计
        /// </summary>
        /// <returns></returns>
        List<CommonModelNameCount> GetHiddenTrendTJ(string orgid, int year);
        /// <summary>
        /// 获取后台数量统计
        /// </summary>
        /// <returns></returns>
        CountDataModel GetCountData();
        /// <summary>
        /// 后台主页统计柱状图  各级别风险点数量
        /// </summary>
        /// <returns></returns>
        List<CommonModel> GetRiskUnitTJ();
        /// <summary>
        /// 后台主页统计饼状图  隐患类型统计
        /// </summary>
        /// <returns></returns>
        List<HiddenTJ> GetHiddenTJ();
        /// <summary>
        /// 风险点 风险因素统计
        /// </summary>
        /// <returns></returns>
        List<CommonModel> RiskFactorStatistical();
        /// <summary>
        /// 组织各类别检查计划执行情况统计表
        /// </summary>
        /// <returns></returns>
        List<CheckPlanEnity> PlanStatistical();
        /// <summary>
        /// 检查表结果统计GetOIList
        /// </summary>
        /// <returns></returns>
        List<CheckResultRecordEntity> CheckResultStatistical(string orgid,string currOrgId, string states, string planname, string riskpointname, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 下载管理
        /// </summary>
        /// <returns></returns>
        List<DownLoadEntity> GetOIList(int pageIndex, int pageSize, ref int totalCount);
    }
}
