using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;

namespace XY.AfterCheckEngine.IService
{
    public interface ISupervisionInfoService
    {
        /// <summary>
        /// 根据条件获取医保门诊信息
        /// </summary>
        /// <param name="queryConditionByClinic"></param>
        /// <returns></returns>
        List<YBClinicInfoEntity> GetYBClinicInfoByCondition(QueryConditionByClinic queryConditionByClinic, int pageIndex, int pageSize, ref int totalCount);


        /// <summary>
        /// 根据条件获取医保门诊信息
        /// </summary>
        /// <param name="queryCondition">json字符串</param>
        /// <returns></returns>
        List<YBClinicInfoDto> GetYBClinicInfo(string queryCondition, int pageIndex, int pageSize, ref int totalCount);

        List<YBClinicInfoEntity> GetYBClinicInfoList(string clinicDate,string idNumber,string personCode, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 判断是否是市医保局 
        /// </summary>
        /// <param name="OrganizeId"></param>
        /// <returns></returns>
        bool IsCityYBJ(string OrganizeId);

        YBClinicInfoEntity GetYBClinicInfoEntity(string clinicRegisterCode, string cityAreaCode, string year);

        YBHosInfoEntity GetYBHosInfoEntityByCondition(string hosRegisterCode, string personalCode);

        List<YBClinicPreInfoEntity> GetYBClinicPreInfoList(string clinicRegisterCode, int pageIndex, int pageSize, ref int totalCount);

        List<CheckComplaintDetailEntity> GetCLInfo(string CheckComplainId, string datatype);
        //复审
        bool RepeatCheckComplaint(string[] rulescode, string registercode,string describe,UserInfo userInfo,decimal money);
        List<CheckComplaintMainDto> GetYBHosInfoList(string condition, string keyValue, string idNumber, string institutionCode, string inHosDate, string outHosDate,string rulescode,string datatype, int pageIndex, int pageSize, ref int totalCount);
        List<CheckResultInfoEntity> GetComplaintInfoList(string rulesCode, QueryCoditionByCheckResult queryCoditionByCheckResult,bool isadmin,string curryydm, int page, int limit, ref int totalcount);


        List<CheckResultInfoEntity> GetCheckResultInfoList(string key);

        List<CheckResultPreInfoEntity> GetCheckResultPreInfoList(string key);

        List<CheckResultPreInfoEntity> GetWGDescribe(string key);

        List<CheckComplaintEntity> GetCheckComplaintInfoList(string registercode, string personalcode, string rulescode);
        /// <summary>
        /// 获取违规处方明细并分页
        /// </summary>
        /// <param name="hosregistercode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<HosPreInfo_WGDto> GetWGCFDeatilList(string hosregistercode, string rulecode, int pageIndex, int pageSize, ref int totalCount);

        List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string key, int pageIndex, int pageSize, ref int totalCount);
        List<CliPreInfo_WGDto> GetWGCFDeatilListCli(string hosregistercode, string rulecode, int pageIndex, int pageSize, ref int totalCount);
        List<MonthCountEntity> GetMonthCountList(string year, string flag, string status);
        
    }
}
