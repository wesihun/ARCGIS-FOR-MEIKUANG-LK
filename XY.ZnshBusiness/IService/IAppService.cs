using System;
using System.Collections.Generic;
using System.Text;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;

namespace XY.ZnshBusiness.IService
{
    public interface IAppService
    {
        /// <summary>
        /// 获取登录人的执行计划列表
        /// </summary>
        /// <returns></returns>
        CheckPlanEnity GetPlanList(string userid);
        /// <summary>
        /// 获取所有执行计划列表
        /// </summary>
        /// <returns></returns>
        List<CheckPlanEnity> GetAllPlanList(string orgid,string currOrgId, string planName, string riskpointName, string states, int page, int limit, ref int totalCount);
        /// <summary>
        /// 根据计划名获取计划下的所有风险点
        /// </summary>
        /// <param name="planName">计划名</param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        List<RiskPointDto> GetRiskPointList(string userid);

        /// <summary>
        /// 根据二维码获取该风险点的排查事项
        /// </summary>
        /// <param name="riskBH"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CheckTableEntity> GetQRCodeList(string riskBH, string userId);
        /// <summary>
        /// 判断二维码是否是当前操作人
        /// </summary>
        /// <param name="riskBH"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsExitCurrUserId(string riskBH, string userId);
        /// <summary>
        /// 获取该检查计划执行情况
        /// </summary>
        /// <returns></returns>
        List<CheckPlanEnity> GetPlanCheckList();
        /// <summary>
        /// 获取风险告知卡
        /// </summary>
        /// <returns></returns>
        List<RiskNoticeDto> GetRiskNoticeList(string userid);
        /// <summary>
        /// 对单个排查事项进行检查提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        bool GetQRCodeCreat(string id, string userid, string username);
    }
}
