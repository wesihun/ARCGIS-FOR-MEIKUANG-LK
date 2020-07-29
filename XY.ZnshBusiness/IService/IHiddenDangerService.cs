using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;

namespace XY.ZnshBusiness.IService
{
    public interface IHiddenDangerService
    {
        #region 获取数据
        /// <summary>
        /// 获取隐患上报列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<HiddenDangersReportEntity> GetPageListByCondition(string states, string userid, string qdstates, string orgid,string currOrgId, string riskpointname, string hiddenlevel, int pageIndex, int pageSize, ref int totalCount);

        List<HiddenDangersNoticeEntity> GetNoticeList(string states,string userid, string orgid, string currOrgId, string zlrname, string hiddenlevel, int pageIndex, int pageSize, ref int totalCount);
        List<HiddenDangersModifyEntity> GetModifyList(string states, string userid,string orgid, string currOrgId, string qdstates, string riskpointname, string zlrname, int pageIndex, int pageSize, ref int totalCount);

        List<HiddenDangersRecheckEntity> GetRecheckList(string userid,string orgid, string states, string currOrgId, string riskpointname, string fcrname, int pageIndex, int pageSize, ref int totalCount);

        List<HiddenDangersNoticeEntity> GetHistoryList1(string userid, int pageIndex, int pageSize, ref int totalCount);

        List<HiddenDangersRecheckEntity> GetHistoryList2(string userid, int pageIndex, int pageSize, ref int totalCount);

        List<HiddenDangersModifyEntity> GetHistoryList3(string userid, int pageIndex, int pageSize, ref int totalCount);
        #endregion

        #region 提交数据
        /// <summary>IsSecondCreat
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool CreateReport(HiddenDangersReportEntity entity, string OrgId,string userid, string username);
        bool CreateNotice(HiddenDangersNoticeEntity entity);
        bool CreateModify(HiddenDangersModifyEntity entity);
        bool UpdateModify(HiddenDangersModifyEntity entity);
        bool IsSecondCreat(string noticeId);
        bool IsModifyPass(string noticeId);
        /// <summary>
        /// 是否重复上报隐患
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        bool IsSecondReport(string reportId);
        /// <summary>
        /// 是否二次复查
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        bool IsSecondRecheck(string reportId);
        bool CreateRecheck(HiddenDangersRecheckEntity entity);
        bool PushSafeDepart(string id,string zrrid);
        string GetIsSupervisor(string reportId);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool Update(HiddenDangersReportEntity entity);
        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues,string type);
        #endregion
        #region 获取数据
        List<SafeUserDto> GetSafeUserList(string currOrgId);
        #endregion 
    }
}
