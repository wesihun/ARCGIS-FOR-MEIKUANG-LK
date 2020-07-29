using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.IService
{
    public interface IBeforeCheckEngineService
    {
        /// <summary>
        ///事前审核结果数据
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<Check_BeForeResultInfo> GetBeforeResultList(string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        /// <summary>
        ///事前审核结果数据详情
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<Check_BeForeResultPreInfo> GetBeforeResultDetailList(string registerCode, int page, int limit, ref int totalcount);
    }
}
