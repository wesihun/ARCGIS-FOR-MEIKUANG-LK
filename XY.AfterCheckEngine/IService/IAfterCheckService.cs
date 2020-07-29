using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.IService
{
    public interface IAfterCheckService
    {
        /// <summary>
        /// 获取医保住院信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetYBHosInfos(QueryConditionViewModel queryConditionViewModel);

        /// <summary>
        /// 获取一个人在同一家医院所有的医保住院信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetYBHosInfosByPersonalByIns(QueryConditionViewModel queryConditionViewModel);


        /// <summary>
        /// 获取参保人所有住院信息
        /// </summary>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        List<YBHosInfoEntity> GetYBHosInfosByPersonalCode(string personalCode);
        /// <summary>
        /// 根据住院登记编码获取处方信息
        /// </summary>
        /// <param name="hosRegisterCode"></param>
        /// <returns></returns>
        List<YBHosPreInfoEntity> GetYBHosPreInfosByHosRegisterCode(string hosRegisterCode);
        /// <summary>
        /// 从redis里获取审核结果
        /// </summary>
        /// <param name="checkNumber"></param>
        /// <returns></returns>
        List<CheckResultInfoEntity> GetCheckResultInfoListFromRedis(string checkNumber);
        /// <summary>
        /// 从redis里获取审核结果处方信息
        /// </summary>
        /// <param name="checkPreNumber"></param>
        /// <returns></returns>
        List<CheckResultPreInfoEntity> GetCheckResultPreInfoListFromRedis(string checkPreNumber);

        /// <summary>
        /// 获取医院信息
        /// </summary>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        List<YyxxInfoViewModel> GetYyxxInfo();
        /// <summary>
        /// 获取医院信息去除一级医院
        /// </summary>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        List<YyxxInfoViewModel> GetYyxxInfoByHos();

        /// <summary>
        /// 获取就诊疾病异常审核知识库
        /// </summary>
        /// <returns></returns>
        List<DiseaseNoNormalEntity> GetDiseaseNoNormals_Knowledge();

        /// <summary>
        /// 获取住院天数异常知识库
        /// </summary>
        /// <returns></returns>
        List<RulesMainEntity> GetInHosDayUnusual_Knowledge();
        /// <summary>
        /// 获取分解住院知识库
        /// </summary>
        /// <returns></returns>
        RulesMainEntity GetDisintegrateInHos_Knowledge();
        /// <summary>
        /// 获取限定诊疗价格知识库
        /// </summary>
        /// <returns></returns>
        List<ItemLimitPriceEntity> GetItemLimitedPrice_Knowledge();

        /// <summary>
        /// 获取限儿童用药知识库
        /// </summary>
        /// <returns></returns>
        List<ChildrenDrugEntity> GetChildrenDrugEntity_Knowledge();

        /// <summary>
        /// 获取医保目录限制用药范围知识库
        /// </summary>
        /// <returns></returns>
        List<MedicalLimitDrugEntity> GetMedicalLimitedDrug_Knowledge();

        /// <summary>
        /// 获取限定性别用药知识库
        /// </summary>
        /// <returns></returns>
        List<SexDrugEntity> GetSexDrug_Knowledge();

        /// <summary>
        /// 获取老年人合理用药知识库
        /// </summary>
        /// <returns></returns>
        List<OldDrugEntity> GetOldDrug_Knowledge();

        /// <summary>
        /// 获取限定性别诊疗服务知识库
        /// </summary>
        /// <returns></returns>
        List<SexItemLimitEntity> GetSexItem_Knowledge();

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        bool UpdateResultStatus(CheckResultStatusEntity checkResultStatus);
        /// <summary>
        /// 获取审核结果状态
        /// </summary>
        /// <returns></returns>
        CheckResultStatusEntity GetCheckResultStatus();

        /// <summary>
        /// 保存审核结果时更新医保数据状态
        /// </summary>
        /// <param name="hosRegisterCodes"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateYbHosInfo(List<string> hosRegisterCodes,string status);
        /// <summary>
        /// 插入审核结果信息
        /// </summary>
        /// <param name="checkResultInfoEntities"></param>
        /// <returns></returns>
        bool InsertResultInfo(List<CheckResultInfoEntity> checkResultInfoEntities);

        /// <summary>
        /// 插入审核结果处方信息
        /// </summary>
        /// <param name="checkResultPreInfoEntities"></param>
        /// <returns></returns>
        bool InsertResultPreInfo(List<CheckResultPreInfoEntity> checkResultPreInfoEntities);

        /// <summary>
        /// 更新状态(保存审核结果)
        /// </summary>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        bool UpdateSaveResultStatus(CheckResultStatusSaveEntity checkResultStatusSaveEntity);
        /// <summary>
        /// 获取审核结果状态(保存审核结果)
        /// </summary>
        /// <returns></returns>
        CheckResultStatusSaveEntity GetSaveCheckResultStatus();
        /// <summary>
        /// 获取医保门诊信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        List<YBClinicInfoEntity> GetYBClinicInfos(QueryConditionByClinic queryConditionByClinic);
        /// <summary>
        /// 获取审核状态--门诊
        /// </summary>
        /// <returns></returns>
        CheckResultStatusEntity GetCheckResultStatusByClinic();

        /// <summary>
        /// 获取审核结果状态(保存审核结果)--门诊
        /// </summary>
        /// <returns></returns>
        CheckResultStatusSaveEntity GetSaveCheckResultStatusByClinic();

        /// <summary>
        /// 根据门诊登记编码获取处方信息
        /// </summary>
        /// <param name="clinicRegisterCode"></param>
        /// <returns></returns>
        List<YBClinicPreInfoEntity> GetYBClinicPreInfosByClinicRegisterCode(string clinicRegisterCode);
        /// <summary>
        /// 更新医保审核状态
        /// </summary>
        /// <param name="flag">1门诊2住院</param>
        /// <param name="institutioncode">机构代码</param>
        /// <returns></returns>
        bool UpdateYBData(string flag, List<string> registerCodes, string status);

        #region 保存审核结果时更新医保数据状态--门诊
        /// <summary>
        /// 保存审核结果时更新医保数据状态
        /// </summary>
        /// <param name="clinicRegisterCodes"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        bool UpdateYbClinicInfo(List<string> clinicRegisterCodes, string status);
        #endregion
        List<TreeModule> GetWGTree(string registerCode, int treeType);
        /// <summary>
        /// 获取左侧树查询页面使用
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        List<TreeModule> GetWGTreeBySearch(string registerCode);

        WGEntity GetWGMoney(string registerCode);
        /// <summary>
        /// 根绝条件获取审核结果列表并分页
        /// </summary>
        /// <param name="flag">1门诊2住院</param>
        /// <param name="queryCoditionByCheckResult"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        List<CheckResultInfoEntity> GetCheckResultInfos(string flag,string rulescode,QueryCoditionByCheckResult queryCoditionByCheckResult,bool isadmin,string curryydm, int page, int limit, ref int totalcount);
        List<string> GetComplainRulesName(string registerCode,string type);
        List<CheckResultInfoEntity> GetListByFK(string states, string rulescode, QueryCoditionByCheckResult result,bool isadmin,string curryydm,int page, int limit, ref int totalcount);
        //获取申诉中 列表
        List<CheckComplaintEntity> GetListBySS(string flag, string rulescode, QueryCoditionByCheckResult result,bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        //获取还未申诉 列表
        List<ComplaintMainEntity> GetListByWSS(string flag, string rulescode, QueryCoditionByCheckResult result, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        List<CheckResultInfoEntity> GetListByConclusion(string rulescode, QueryCoditionByCheckResult queryCoditionByCheckResult,bool isadmin,string curryydm, string states, int page, int limit, ref int totalcount);

        YBClinicInfoEntity GetYBClinicInfoEntity(string RegisterCode, string personalCode);

        YBHosInfoEntity GetYBHosInfoEntity(string RegisterCode, string personalCode);

        CheckComplaintEntity GetCheckComplaintEntity(string CheckResultInfoCode);

        List<CheckComplaintEntity> GetComplaintStatesList(string rulescode,string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin,string curryydm, int page, int limit, ref int totalcount);

        string ReturnCheckComplainId(string registerCode);

        List<CheckResultInfoEntity> GetComplaintStatesListTJ(string rulescode, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount);
        /// <summary>
        /// 反馈提交
        /// </summary>
        /// <param name="code">主键</param>
        /// <param name="describe">反馈描述</param>
        /// <param name="checkComplaint">传当前用户信息</param>
        /// <returns></returns>
        string UpdateFK(string registerCode, string describe, UserInfo userInfo);
        /// <summary>
        /// 反馈提交  更新反馈次数
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <param name="describe">反馈描述</param>
        /// <param name="checkComplaint">传当前用户信息</param>
        /// <returns></returns>
        string UpdateFeedbackCount(string registerCode, string describe, UserInfo userInfo);
        /// <summary>
        /// 是否重复添加
        /// </summary>
        /// <param name="checkResultInfoCode"></param>
        /// <returns></returns>
        bool IsExistAdd(string checkResultInfoCode);
        /// <summary>
        /// 删除已上传附件
        /// </summary>
        /// <param name="ComplaintCode"></param>
        /// <returns></returns>
        bool DeleteFiles(string ComplaintCode);
        /// <summary>
        /// 获取重复反馈次数
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        int? GetFeedbackCount(string registerCode);
        /// <summary>
        /// 判断是否是市医保局 
        /// </summary>
        /// <param name="OrganizeId"></param>
        /// <returns></returns>
        bool IsCityYBJ(string OrganizeId);
        /// <summary>
        /// 获取审核规则
        /// </summary>
        /// <returns></returns>
        
        List<RulesMainEntity> GetRulesMainEntities();
        /// <summary>
        /// 获取审核进度列表
        /// </summary>
        /// <param name="queryCoditionByCheckResult"></param>
        /// <param name="rulecode">规则编码</param>
        /// <param name="states">审核状态</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<CheckResultInfoEntity> GetComplaintStateList(QueryCoditionByCheckResult queryCoditionByCheckResult, string rulecode, string states, bool isAdmin, string curryydm, int page, int limit, ref int count);
        /// <summary>
        /// 根据条件获取统计列表
        /// </summary>
        /// <param name="queryConditionByCheckComplain"></param>
        /// <param name="isAdmin"></param>
        /// <param name="curryydm"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<CheckComplainStaticsEntity> GetCheckComplainStatics(QueryConditionByCheckComplain queryConditionByCheckComplain, bool isAdmin, string curryydm, int page, int limit, ref int count);

        #region 提交数据
        /// <summary>
        /// 新增初审
        /// </summary>
        /// <returns></returns>
        bool Insert(string registerCode, ConditionStringCS condition, UserInfo userInfo);
        bool Add(CheckComplaintDetailEntity checkComplaintDetailEntity);

        bool Add2(Check_ComplaintDetail_MZLEntity checkComplaintDetailMZLEntity);

        bool AddDescribe(string registerCode,CheckComplaintDetailEntity checkComplaintDetailEntity,string describe,UserInfo userInfo);
        #endregion
    }
}
