using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.IService
{
    public interface IComplaintStatisticalService
    {
        /// <summary>
        /// 获取医院扣款列表
        /// </summary>
        /// <returns></returns>
        List<YYKKGL> GetYYKKList(YYKKGL queryCodition, bool isAdmin, string curryydm, int page, int limit, ref int count);
        /// <summary>
        /// 获取某一医疗机构的所有患者住院信息
        /// </summary>
        /// <param name="queryCodition"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<CheckUserList> GetCheckUserListsByYLJG(YYKKGL queryCodition, bool isAdmin, string curryydm, int page, int limit, ref int count);
        /// <summary>
        /// 按审核规则获取审核结果信息
        /// </summary>
        /// <param name="queryCodition"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<CheckUserList> GetListByRulesCode(ListByRulesCodeQuery queryCodition, int page, int limit, ref int count);
        /// <summary>
        /// 获取审核结果疑点信息列表
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        List<Check_ComplaintMain_MZLEntity> GetYDInfoList(string registerCode);
        /// <summary>
        /// 获取审核状态查询中的状态查询条件  根据权限设置
        /// </summary>
        /// <returns></returns>
        Dictionary<int, string> GetQueryStates(bool isadmin,string curryydm);
        /// <summary>
        /// 获取审核状态查询
        /// </summary>
        /// <returns></returns>
        List<Check_Complain_MZLEntity> GetListByStates(string step, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int count);
    }
}
