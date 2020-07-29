using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.IService
{
    public interface IComplaintMZLService
    {
        /// <summary>
        ///从审核结果表里获取审核申诉信息   初审信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        DataListByCS GetFisrtInfos(string states,QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        /// <summary>
        ///从审核结果表里获取审核申诉信息   初审信息2
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        DataListByCS GetFisrtInfos2(string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        /// <summary>
        ///获取初审疑点信息
        /// </summary>
        /// <returns></returns>
        List<CheckResultInfoEntity> GetFisrtYDDescribe(string registerCode);
        /// <summary>
        /// 获取初审违规确认列表
        /// </summary>Commint
        /// <returns></returns>
        List<WGConfirmEntity> GetFisrtWGConfirmList(string checkResultInfoCode);
        /// <summary>
        /// 初审提交
        /// </summary>Commint
        /// <returns></returns>
        bool FirstCommint(JsonArray jsonObject, string registerCode, CheckResultInfoEntity model, UserInfo userInfo);
        /// <summary>
        ///获取列表信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        DataListByOther GetInfosList(string step, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        /// <summary>
        /// 根据住院登记编码获取审核状态信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        Check_Complain_MZLEntity Get_Complain_MZLEntity(string registerCode);
        /// <summary>
        /// 根据住院登记编码获取审核状态信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        Check_Complain_MZLEntity Get_Complain_MZLEntityALL(string registerCode);
        /// <summary>
        /// 获取通用疑点信息列表
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        List<Check_ComplaintMain_MZLEntity> GetYDInfoList(string registerCode);
        /// <summary>
        /// 获取通用违规确认弹出列表
        /// </summary>
        /// <param name="complaintCode">主键</param>
        /// <returns></returns>
        List<Check_ComplaintMain_MZLEntity> GetWGQRInfoList(string complaintCode);
        /// <summary>
        /// 专家违规确认弹出   附件处方金额用
        /// </summary>
        /// <param name="complaintCode"></param>
        /// <returns></returns>
        List<Check_ComplaintMain_MZLDto> GetWGQRInfoListByZJ(string complaintCode);
        /// <summary>
        /// 通用提交
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <param name="registerCode"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string Commint(string step, JsonArray jsonObject, string registerCode,UserInfo userInfo);
        /// <summary>
        /// 删除已上传附件
        /// </summary>
        /// <param name="ComplaintCode"></param>
        /// <returns></returns>
        bool DeleteFiles(string ComplaintCode);
        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="CheckComplainId"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        List<Check_ComplaintDetail_MZLEntity> GetImageInfo(string CheckComplainId, string datatype);
        /// <summary>
        ///获取申述状态查询
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<Check_Complain_MZLEntity> GetComplaintStates(QueryCoditionByCheckResult queryCoditionByCheckResult, string states, bool isadmin, string curryydm,string ydlevel, int page, int limit, ref int totalcount);
    }
}
