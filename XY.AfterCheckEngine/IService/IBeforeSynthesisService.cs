using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.Universal.Models.ViewModels;

namespace XY.AfterCheckEngine.IService
{
    /// <summary>
    /// 功能描述：事前审核结果展示
    /// 创 建 者：LK
    /// 创建日期：2020/4/17 
    /// 最后修改者：LK
    /// 最后修改日期：2020/4/17 
    /// </summary>
    public interface IBeforeSynthesisService
    {
        /// <summary>
        /// 获取总体概况总数
        /// </summary>
        /// <returns></returns>
        TotalEntity GetTotal(string yljgbh,string curryydm);
        /// <summary>
        /// 获取费用类型
        /// </summary>
        /// <returns></returns>
        List<ChartsViewModel_YYZL> GetFYLX(string yljgbh);
        /// <summary>
        /// 获取费用类型钻取1
        /// </summary>
        /// <returns></returns>
        List<BeforeDrugZLCharts> GetFYLX_ZQ1(string yljgbh,int page, int limit, ref int count);
        /// <summary>
        /// 获取费用类型钻取2
        /// </summary>
        /// <returns></returns>
        List<ZnshTYDetailEntity> GetFYLX_ZQ2(string ksbm,string yljgbh, int page, int limit, ref int count);
        /// <summary>
        /// 获取就诊人次当日
        /// </summary>
        /// <returns></returns>
        List<ChartsViewModel_JZ> GetJZRC(string yljgbh, int second, int count);
        /// <summary>
        /// 获取科室占比
        /// </summary>
        /// <returns></returns>
        List<KSZB> GetKSZB(string yljgbh, int page, int limit, ref int count);
        /// <summary>
        /// 获取规则违规人次数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StaticsViewModel> GetGZWG(string yljgbh, int page, int limit, ref int count);
        /// <summary>
        /// 获取规则违规人次数钻取1  根据违规规则获取患者信息
        /// </summary>
        /// <returns></returns>
        List<BeforeHZXX> GetGZWG_ZQ1(string yljgbh, string rulescode, int page, int limit, ref int count);
        /// <summary>
        /// 获取患者违规描述信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="registercode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BeforeWGDescribe> GetWGDescribe(string yljgbh, string registercode, int page, int limit, ref int count);
        /// <summary>
        /// 获取患者处方信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="registercode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BeforeCF> GetCF(string yljgbh, string registercode, int page, int limit, ref int count);
        /// <summary>
        /// 获取违规科室占比
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BesfroeKSZB> GetWGKSZB(string yljgbh, int page, int limit, ref int count);
        /// <summary>
        /// 获取违规科室占比钻取1 根据科室获取人员信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BeforeHZXX> GetWGKSZB_ZQ1(string yljgbh, string ksbm, int page, int limit, ref int count);
        /// <summary>
        /// 获取医生违规人次数
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BesfroeWGYS> GetWGYS(string yljgbh, int page, int limit, ref int count);
        /// <summary>
        /// 获取违规医生人次数钻取1  根据医生获取违规患者信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="ksbm"></param>
        /// <param name="doctorname"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<BeforeHZXX> GetWGYS_ZQ1(string yljgbh, string ksbm, string doctorname, int page, int limit, ref int count);
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        List<KSZB> GetKSList(string yljgbh, string datatype);
        /// <summary>
        /// 点击科室获取下面搜索患者信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        List<BeforeHZXX> GetPersonList1(string name,string ksbm, string yljgbh, string datatype);
        /// <summary>
        /// 右侧患者信息
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="ksbm"></param>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        HZJK GetPersonList2(string registercode, string ksbm, string yljgbh, string datatype);
        /// <summary>
        /// 获取医疗机构编号
        /// </summary>
        /// <returns></returns>
        string GetYLJGBH(string curryydm);
    }
}
