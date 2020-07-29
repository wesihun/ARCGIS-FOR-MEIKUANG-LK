using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XY.Utilities;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Service;
using XY.AfterCheckEngine.IService;
using XY.ZnshBusiness.Entities;
using XY.Universal.Models;
using AutoMapper;
using System.Threading;
using XY.DataCache.Redis;
using XY.Universal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Transactions;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class AfterCheckEngineClinicController:ControllerBase
    {
        private readonly IAfterCheckService _afterCheckService;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        public static List<CheckResultPreInfoEntity> checkResultPreInfoEntities = new List<CheckResultPreInfoEntity>();//审核结果处方信息
        public static List<CheckResultInfoEntity> checkResultInfoEntities = new List<CheckResultInfoEntity>();//审核结果信息
        private static List<YBClinicInfoEntity> ybClinicInfo { get; set; }//根据条件获取的门诊患者信息
        private static List<YBClinicInfoEntity> PersonInhosInfo { get; set; }//存储个人的门诊信息（一个人挂几次号）

        public static List<YBClinicInfoEntity> ybclinicinfoByYes = new List<YBClinicInfoEntity>(); //审核完成有问题的患者
        public static List<YBClinicInfoEntity> ybclinicinfoByNo = new List<YBClinicInfoEntity>(); //审核完成无问题的患者

        public AfterCheckEngineClinicController(IAfterCheckService afterCheckService, IMapper mapper, IRedisDbContext redisDbContext)
        {
            _afterCheckService = afterCheckService;
            _mapper = mapper;
            _redisDbContext = redisDbContext;
        }

        /// <summary>
        /// 获取的住院患者信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYBHosInfos()
        {
            QueryConditionViewModel queryConditionViewModel = new QueryConditionViewModel();
            queryConditionViewModel.InstitutionCode = "";
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetYBHosInfos(queryConditionViewModel);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取区域列表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 获取的门诊患者信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYBClinicInfos()
        {
            QueryConditionByClinic queryConditionByClinic = new QueryConditionByClinic();
            queryConditionByClinic.InstitutionCode = "";
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetYBClinicInfos(queryConditionByClinic);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取门诊列表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }



        /// <summary>
        /// 获取医院信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYyxxInfo()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetYyxxInfoByHos();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }


        /// <summary>
        /// 获取审核结果状态
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckResultStatusByClinic()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckResultStatusByClinic();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 获取保存审核结果状态
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSaveCheckResultStatusByClinic()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetSaveCheckResultStatusByClinic();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "审核结果保存成功！";
                    resultCountModel.data = data;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "审核结果保存失败！";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 从redis里获取审核结果（没有存储数据库时）
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckResultInfoListFromRedis(string checkNumber)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckResultInfoListFromRedis(checkNumber);
                if (data != null)
                {
                    List<CheckResultCountViewModel> checkResultCountViewModels = data.GroupBy(it => new { it.RulesCode, it.RulesName })
                        .Select(m => new CheckResultCountViewModel
                        {
                            Count = m.Count(),
                            RulesCode = m.Key.RulesCode,
                            RulesName = m.Key.RulesName
                        }).ToList();
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = checkResultCountViewModels;
                    resultCountModel.count = data.Select(r => r.RegisterCode).ToList().Distinct().ToList().Count();
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }


        /// <summary>
        /// 从redis里获取审核结果明细（没有存储数据库时）
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckResultInfoFromRedis(string checkNumber, string InstitutionCode, string rulesCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckResultInfoListFromRedis(checkNumber);
                if (data != null)
                {
                    List<CheckResultInfoEntity> checkResultCountViewModels = data.Where(it => it.InstitutionCode == InstitutionCode && it.RulesCode == rulesCode).ToList();
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = checkResultCountViewModels;
                    resultCountModel.count = data.Count;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }


        /// <summary>
        /// 从redis里获取审核结果单独取出医疗机构（没有存储数据库时）
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckResultToInsFromRedis(string checkNumber)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckResultInfoListFromRedis(checkNumber);
                if (data != null)
                {
                    List<CheckResultCountViewModel> checkResultCountViewModels = data.GroupBy(it => new { it.InstitutionCode, it.InstitutionName })
                        .Select(m => new CheckResultCountViewModel
                        {
                            InstitutionCode = m.Key.InstitutionCode,
                            InstitutionName = m.Key.InstitutionName
                        }).ToList();
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = checkResultCountViewModels;
                    resultCountModel.count = data.Count;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 提交审核结果
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult AfterCheckResultSubmit(string checkNumber, string checkPreNumber, List<string> institutionCodes,string syncYb)
        {
            var resultCountModel = new RespResultCountViewModel();
            int Error = 0;
            int tranError = 0;
            QueryConditionByClinic queryConditionByClinic = new QueryConditionByClinic();
            List<CheckResultInfoEntity> checkResultInfoList = _afterCheckService.GetCheckResultInfoListFromRedis(checkNumber);
            List<CheckResultPreInfoEntity> checkResultPreList = _afterCheckService.GetCheckResultPreInfoListFromRedis(checkPreNumber);
            if (checkResultInfoList.Count <= 0)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有审核违规结果";
                return Ok(resultCountModel);
            }
            List<CheckResultInfoEntity> checkResultInfoEntitiesInsert = new List<CheckResultInfoEntity>();//本次要插入表的审核信息结果
            List<CheckResultPreInfoEntity> checkResultPreInfoEntitiesInsert = new List<CheckResultPreInfoEntity>();//本次要插入表的审核处方信息结果
            List<string> ybClinicRegisterCodesYes = new List<string>();//有问题的门诊登记编码
            List<string> ybClinicRegisterCodesNo = new List<string>();//无问题的门诊登记编码
            
            ybclinicinfoByYes.Clear();//有问题的患者对象（暂时好像多余）
            ybclinicinfoByNo.Clear();//无问题的患者对象（暂时好像多余）
            List<string> clinicinfoRedisKeys = new List<string>();//存储医保门诊信息缓存Key值
            CheckResultStatusSaveEntity checkResultStatusSaveEntityStart = new CheckResultStatusSaveEntity() { CRowId = 2, SaveCheckResultStatus = "N" };
            bool checkStatusResultStart = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityStart);
            if (!checkStatusResultStart)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "审核结果保存失败!请联系管理员";
                return Ok(resultCountModel);
            }
            foreach (string institutionCode in institutionCodes)
            {
                if (string.IsNullOrEmpty(institutionCode))
                    continue;
                queryConditionByClinic.InstitutionCode = institutionCode;
                ybClinicInfo = _afterCheckService.GetYBClinicInfos(queryConditionByClinic);//按条件获取门诊信息
                List<CheckResultInfoEntity> checkResultInfoEntitiesByIns = checkResultInfoList.Where(it => it.InstitutionCode == institutionCode&&it.DataType=="1").ToList();
                foreach (YBClinicInfoEntity item in ybClinicInfo)
                {
                    if (checkResultInfoEntitiesByIns.Where(it => it.RegisterCode == item.ClinicRegisterCode).FirstOrDefault() == null)
                        ybClinicRegisterCodesNo.Add(item.ClinicRegisterCode);
                }

                foreach (CheckResultInfoEntity checkResultInfoitem in checkResultInfoEntitiesByIns)
                {
                    ybClinicRegisterCodesYes.Add(checkResultInfoitem.RegisterCode);
                    if (ybClinicInfo.Where(it => it.ClinicRegisterCode == checkResultInfoitem.RegisterCode && it.States == ((int)CheckStates.A).ToString()).FirstOrDefault() != null)
                    {
                        List<CheckResultPreInfoEntity> checkResultPreInfoEntities_temp = new List<CheckResultPreInfoEntity>();
                        checkResultInfoEntitiesInsert.Add(checkResultInfoitem);
                        checkResultPreInfoEntities_temp = checkResultPreList.Where(it => it.RegisterCode == checkResultInfoitem.RegisterCode && it.RulesCode == checkResultInfoitem.RulesCode&&it.DataType=="1").ToList();
                        if (checkResultPreInfoEntities_temp.Count > 0)
                        {
                            checkResultPreInfoEntitiesInsert.AddRange(checkResultPreInfoEntities_temp);
                        }


                    }
                }
                clinicinfoRedisKeys.Add(SystemManageConst.YBINFO_CLINIC + queryConditionByClinic.InstitutionCode);
            }
                        
            TransactionOptions transactionOption = new TransactionOptions();
            //设置事务隔离级别
            transactionOption.IsolationLevel = IsolationLevel.ReadCommitted;
            // 设置事务超时时间为60秒
            transactionOption.Timeout = new TimeSpan(0, 0, 60000);
            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
            {
                try
                {
                    if (ybClinicRegisterCodesYes.Count > 0)
                    {
                        //更新医保库数据                        
                        if (!_afterCheckService.UpdateYbClinicInfo(ybClinicRegisterCodesYes, ((int)CheckStates.B).ToString()))
                            Error++; 
                    }
                    if (ybClinicRegisterCodesNo.Count > 0)
                    {
                        if (!_afterCheckService.UpdateYbClinicInfo(ybClinicRegisterCodesNo, ((int)CheckStates.C).ToString()))
                            Error++;
                    }
                    if (checkResultInfoEntitiesInsert.Count > 0)
                    {
                        if (!_afterCheckService.InsertResultInfo(checkResultInfoEntitiesInsert))
                            Error++;
                    }
                    if (checkResultPreInfoEntitiesInsert.Count > 0)
                    {
                        if (!_afterCheckService.InsertResultPreInfo(checkResultPreInfoEntitiesInsert))
                            Error++;
                    }
                    if (clinicinfoRedisKeys.Count > 0)
                    {
                        using (var db = _redisDbContext.GetRedisIntance())
                        {
                            db.Del(clinicinfoRedisKeys.ToArray());
                            db.Del(checkNumber);
                            db.Del(checkPreNumber);
                            db.Del(SystemManageConst.HomeIndex_KEY);
                        }
                    }
                    if (Error == 0)
                    {
                        CheckResultStatusSaveEntity checkResultStatusSaveEntityEndOne = new CheckResultStatusSaveEntity() { CRowId = 2, SaveCheckResultStatus = "Y" };
                        bool checkStatusResultEndOne = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityEndOne);
                        if (!checkStatusResultEndOne)
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存成功!但状态更新失败，请联系管理员";
                            return Ok(resultCountModel);
                        }
                        else
                        {
                            tranError++;
                            resultCountModel.code = 0;
                            resultCountModel.msg = "审核结果保存成功";
                        }
                        ts.Complete();
                    }
                    else
                    {
                        CheckResultStatusSaveEntity checkResultStatusSaveEntityEndTwo = new CheckResultStatusSaveEntity() { CRowId = 2, SaveCheckResultStatus = "E" };
                        bool checkStatusResultEndTwo = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityEndTwo);
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存失败";

                    }
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    CheckResultStatusSaveEntity checkResultStatusSaveEntityEndTThree = new CheckResultStatusSaveEntity() { CRowId = 2, SaveCheckResultStatus = "E" };
                    bool checkStatusResultEndTThree = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityEndTThree);
                    resultCountModel.code = -1;
                    resultCountModel.msg = "审核结果保存失败!" + ex.Message;

                }
            }
            if (tranError > 0&&syncYb=="1")//事务成功提交且同步数据
            {
                if (ybClinicRegisterCodesYes.Count > 0)
                {
                    try
                    {
                        _afterCheckService.UpdateYBData("1", ybClinicRegisterCodesYes, ((int)CheckStates.B).ToString());
                    }
                    catch
                    {
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                        return Ok(resultCountModel);
                    }
                }
                if (ybClinicRegisterCodesNo.Count > 0)//审核无问题数据
                {
                    try
                    {
                        _afterCheckService.UpdateYBData("1", ybClinicRegisterCodesNo, ((int)CheckStates.C).ToString());
                    }
                    catch
                    {
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                        return Ok(resultCountModel);
                    }
                }
            }
            return Ok(resultCountModel);
        }

        /// <summary>
        /// 事后审核入口
        /// </summary>
        /// <param name="institutionCodes"></param>
        /// <param name="rulesCheck"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AfterCheckActionAsync(List<string> institutionCodes, List<string> rulesCheck)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                QueryConditionByClinic queryConditionByClinic = new QueryConditionByClinic();
                CheckResultStatusEntity checkResultStatus = new CheckResultStatusEntity() { CRowId = 2, CheckNumber = "N", CheckResultStatus = "N", CheckPreNumber = "N", CheckCount = 0 };
                bool checkStatusResultStart = _afterCheckService.UpdateResultStatus(checkResultStatus);
                if (!checkStatusResultStart)
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "审核引擎出错！请联系管理员";
                    return Ok(resultCountModel);
                }
                if (institutionCodes.Count <= 0)
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "请选择机构后进行审核！";
                    return Ok(resultCountModel);
                }
                int count = 0;//本次共计审核人次
                checkResultInfoEntities.Clear();//清空审核结果集合
                checkResultPreInfoEntities.Clear();//清空审核处方结果集合
                string checkNumber = "ResultInfoClinic" + CommonHelper.CreateNo();
                string checkPreNumber = "ResultPreInfoClinic" + CommonHelper.CreateNo();
                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionByClinic.InstitutionCode = institutionCode;
                    List<YBClinicPreInfoEntity> yBClinicPreInfoEntitys = new List<YBClinicPreInfoEntity>();
                    
                    ybClinicInfo = _afterCheckService.GetYBClinicInfos(queryConditionByClinic);//按条件获取门诊信息
                    count += ybClinicInfo.Count;
                    var groupPersonalList = ybClinicInfo.GroupBy(it => new { it.PersonalCode })//根据人员编码分组
                       .Select(m => new
                       {
                           m.Key.PersonalCode
                       }).ToList();

                    #region 获取知识库数据(从数据库或redis中取)
                    List<DiseaseNoNormalEntity> diseaseNoNormalinfo = _afterCheckService.GetDiseaseNoNormals_Knowledge();//就诊疾病异常审核A007
                    List<ItemLimitPriceEntity> itemLimitPricesKnowledge = _afterCheckService.GetItemLimitedPrice_Knowledge();//获取限定诊疗价格知识库B001
                    List<ChildrenDrugEntity> childrenDrugKnowledge = _afterCheckService.GetChildrenDrugEntity_Knowledge();//获取限儿童用药知识库C001
                    List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge = _afterCheckService.GetMedicalLimitedDrug_Knowledge();//获取医保目录限制用药范围知识库B002
                    List<SexDrugEntity> sexDrugKnowledge = _afterCheckService.GetSexDrug_Knowledge();//获取限定性别用药C002
                    List<OldDrugEntity> oldDrugKnowledge = _afterCheckService.GetOldDrug_Knowledge();//获取限老年人用药C003
                    List<SexItemLimitEntity> sexItemLimitKnowledge = _afterCheckService.GetSexItem_Knowledge();//获取限定性别诊疗服务D001
                    #endregion


                    foreach (YBClinicInfoEntity item in ybClinicInfo)
                    {
                        yBClinicPreInfoEntitys.Clear();
                        yBClinicPreInfoEntitys = _afterCheckService.GetYBClinicPreInfosByClinicRegisterCode(item.ClinicRegisterCode);//获取这个人的处方信息
                        AfterCheckAlgorithm(item, diseaseNoNormalinfo,yBClinicPreInfoEntitys,itemLimitPricesKnowledge, childrenDrugKnowledge,
                            medicalLimitDrugKnowledge, sexDrugKnowledge, oldDrugKnowledge, sexItemLimitKnowledge, rulesCheck);
                    }
                }

                CheckResultStatusEntity checkResultStatusEnd = new CheckResultStatusEntity() { CRowId = 2, CheckNumber = checkNumber, CheckResultStatus = "Y", CheckPreNumber = checkPreNumber, CheckCount = count };
                bool checkStatusResultEnd = _afterCheckService.UpdateResultStatus(checkResultStatusEnd);
                if (!checkStatusResultEnd)
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "审核完成，但审核引擎出错！请联系管理员";

                }
                else
                {
                    using (var db = _redisDbContext.GetRedisIntance())
                    {
                        db.Set(checkNumber, checkResultInfoEntities);
                        db.Set(checkPreNumber, checkResultPreInfoEntities);
                    }
                    resultCountModel.code = 0;
                    resultCountModel.msg = "审核完成!";

                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "出现异常!" + ex.Message;

            }
            return Ok(resultCountModel);
        }

        /// <summary>
        /// 按人次按规则审核算法
        /// </summary>
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <param name="inHosDayRulesKnowledge"></param>
        /// <param name="yBHosPreInfoEntity"></param>
        /// <returns></returns>
        private void AfterCheckAlgorithm(YBClinicInfoEntity yBclinicInfoEntity,
            List<DiseaseNoNormalEntity> diseaseNoNormalinfo,
            List<YBClinicPreInfoEntity> yBclinicPreInfoEntity,
            List<ItemLimitPriceEntity> itemLimitPriceEntitie,
            List<ChildrenDrugEntity> childrenDrugKnowledges,
            List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge,
            List<SexDrugEntity> sexDrugKnowledges,
            List<OldDrugEntity> oldDrugKnowledges, List<SexItemLimitEntity> sexItemLimitKnowledges, List<string> rulesChecks)
        {
            // 就诊疾病异常审核
            Task diseaseNoNormalCheck = DiseaseNoNormalCheck(yBclinicInfoEntity, diseaseNoNormalinfo, rulesChecks);
            //限定诊疗价格审核
            Task limitItemPriceCheck = LimitItemPriceCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, itemLimitPriceEntitie, rulesChecks);
            //限儿童用药审核
            Task childrenDrugCheck = ChildrenDrugCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, childrenDrugKnowledges, rulesChecks);
            //医保目录限制用药范围
            Task medicalLimitDrugCheck = MedicalLimitDrugCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, medicalLimitDrugKnowledge, rulesChecks);
            //限定性别用药
            Task sexDrugCheck = SexDrugCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, sexDrugKnowledges, rulesChecks);
            //限老年人用药审核
            Task oldDrugCheck = OldDrugCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, oldDrugKnowledges, rulesChecks);
            //限定性别诊疗服务
            Task sexItemCheck = SexItemCheck(yBclinicInfoEntity, yBclinicPreInfoEntity, sexItemLimitKnowledges, rulesChecks);

            //等待任务结束
            Task.WaitAll(diseaseNoNormalCheck,limitItemPriceCheck, childrenDrugCheck, medicalLimitDrugCheck,
                sexDrugCheck, oldDrugCheck);
        }

        /// <summary>
        /// 就诊疾病异常审核
        /// </summary>
        /// <param name="yBClinicInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task DiseaseNoNormalCheck(YBClinicInfoEntity yBClinicInfoEntity, List<DiseaseNoNormalEntity> diseaseNoNormalinfo, List<string> rulesChecks)
        {
            if (rulesChecks.Exists(it => it == CheckRules.A007.ToString()))
            {
                return Task.Run(() =>
                {
                    List<DiseaseNoNormalEntity> isResult = diseaseNoNormalinfo.Where(it => it.StartAge <= yBClinicInfoEntity.Age && it.EndAge >= yBClinicInfoEntity.Age && it.DiseaseCode == yBClinicInfoEntity.ICDCode).ToList();
                    if (isResult != null && isResult.Count > 0)
                    {
                        CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                        {
                            CheckResultInfoCode = CommonHelper.CreateNo(),
                            RulesCode = CheckRules.A007.ToString(),
                            RulesName = CheckRules.A007.GetDescription(),
                            RegisterCode = yBClinicInfoEntity.ClinicRegisterCode,
                            PersonalCode = yBClinicInfoEntity.PersonalCode,
                            IdNumber = yBClinicInfoEntity.IdNumber,
                            Name = yBClinicInfoEntity.Name,
                            Gender = yBClinicInfoEntity.Gender,
                            Age = (int)yBClinicInfoEntity.Age,
                            InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                            InstitutionName = yBClinicInfoEntity.InstitutionName,
                            ICDCode = yBClinicInfoEntity.ICDCode,
                            DiseaseName = yBClinicInfoEntity.DiseaseName,
                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                            CheckDate = DateTime.Now,
                            ResultDescription = SystemManageConst.DiseaseNoNormalCheckResultDescription(yBClinicInfoEntity.Name, (int)yBClinicInfoEntity.Age, yBClinicInfoEntity.DiseaseName),
                            IsPre = ((int)CheckIsPreEnum.No).ToString()

                        };
                        checkResultInfoEntities.Add(checkResultInfoEntity);
                    }
                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }


        /// <summary>
        /// 限定诊疗价格审核
        /// </summary>
        /// <param name="yBclinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="limitPriceEntitieKnowledge"></param>
        /// <param name="rulesChecks"></param>
        /// <returns></returns>
        private static Task LimitItemPriceCheck(YBClinicInfoEntity yBclinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<ItemLimitPriceEntity> limitPriceEntitieKnowledge, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.B001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把药品过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity.Count > 0 && yBClinicPreInfoEntity != null)
                    {
                        List<ItemLimitPriceViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => !array.Contains(it.CollectFeesCategoryCode)).Select(m => new ItemLimitPriceViewModel
                        {
                            ItemCode = m.ItemCode,
                            ItemName = m.ItemName,
                            ItemPrice = m.PRICE,
                            ItemLimitPrice = 0,
                            ItemIndex = m.ItemIndex,
                            PreCode = m.PreCode,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<ItemLimitPriceViewModel> itemLimitPriceModels = new List<ItemLimitPriceViewModel>();
                            switch (yBclinicInfoEntity.InstitutiongGradeCode)
                            {
                                case SystemManageConst.AONE:
                                    itemLimitPriceModels = limitPriceEntitieKnowledge.Select(m => new ItemLimitPriceViewModel
                                    {
                                        ItemCode = m.ItemCode,
                                        ItemName = m.ItemName,
                                        ItemPrice = 0,
                                        ItemLimitPrice = m.OneLimitedPrice,
                                        PreCode = "",
                                        RegisterCode = ""
                                    }).ToList();
                                    break;
                                case SystemManageConst.ATWO:
                                    itemLimitPriceModels = limitPriceEntitieKnowledge.Select(m => new ItemLimitPriceViewModel
                                    {
                                        ItemCode = m.ItemCode,
                                        ItemName = m.ItemName,
                                        ItemPrice = 0,
                                        ItemLimitPrice = m.TwoLimitedPrice,
                                        PreCode = "",
                                        RegisterCode = ""
                                    }).ToList();
                                    break;
                                case SystemManageConst.ATHREE:
                                    itemLimitPriceModels = limitPriceEntitieKnowledge.Select(m => new ItemLimitPriceViewModel
                                    {
                                        ItemCode = m.ItemCode,
                                        ItemName = m.ItemName,
                                        ItemPrice = 0,
                                        ItemLimitPrice = m.AThreeLevelPrice,
                                        PreCode = "",
                                        RegisterCode = ""
                                    }).ToList();
                                    break;
                                default:
                                    break;
                            }
                            if (itemLimitPriceModels.Count > 0)
                            {
                                //比较
                                List<ItemLimitPriceViewModel> resultItemLimitModels = clinicPreModels.Intersect(itemLimitPriceModels, new ItemLimitPriceComparer()).ToList();
                                if (resultItemLimitModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.B001.ToString(),
                                        RulesName = CheckRules.B001.GetDescription(),
                                        RegisterCode = yBclinicInfoEntity.ClinicRegisterCode,
                                        PersonalCode = yBclinicInfoEntity.PersonalCode,
                                        IdNumber = yBclinicInfoEntity.IdNumber,
                                        Name = yBclinicInfoEntity.Name,
                                        Gender = yBclinicInfoEntity.Gender,
                                        Age = (int)yBclinicInfoEntity.Age,
                                        InstitutionCode = yBclinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBclinicInfoEntity.InstitutionName,
                                        ICDCode = yBclinicInfoEntity.ICDCode,
                                        DiseaseName = yBclinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.LimitItemPriceCheckDescription(yBclinicInfoEntity.Name, yBclinicInfoEntity.IdNumber, yBclinicInfoEntity.InstitutionName),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (ItemLimitPriceViewModel item in resultItemLimitModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.B001.ToString(),
                                            RulesName = CheckRules.B001.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "项目编码：" + item.ItemCode + ";项目名称：" + item.ItemName + ";单价是：" + item.ItemPrice.ToString() + "元;限价是：" + item.ItemLimitPrice.ToString() + "元;"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }

                    }

                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }
        /// <summary>
        /// 限儿童用药审核
        /// </summary>
        /// <param name="yBclinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="childrenDrugKnowledges"></param>
        /// <param name="rulesChecks"></param>
        /// <returns></returns>
        private static Task ChildrenDrugCheck(YBClinicInfoEntity yBClinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<ChildrenDrugEntity> childrenDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity != null && yBClinicPreInfoEntity.Count > 0)
                    {
                        List<ChildrenDrugViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new ChildrenDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            StartAge = 0,
                            EndAge = 0,
                            CurrentAge = (int)yBClinicInfoEntity.Age,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<ChildrenDrugViewModel> childrenDrugViewModels = childrenDrugKnowledges.Select(m => new ChildrenDrugViewModel
                            {
                                DrugCode = m.DrugCode,
                                DrugName = m.DrugName,
                                StartAge = m.StartAge,
                                EndAge = m.EndAge,
                                CurrentAge = 0,
                                Describe = m.Describe
                            }).ToList();
                            if (childrenDrugViewModels.Count > 0)
                            {
                                //比较
                                List<ChildrenDrugViewModel> resultChildrenDrugModels = clinicPreModels.Intersect(childrenDrugViewModels, new ChildrenDrugComparer()).ToList();
                                if (resultChildrenDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C001.ToString(),
                                        RulesName = CheckRules.C001.GetDescription(),
                                        RegisterCode = yBClinicInfoEntity.ClinicRegisterCode,
                                        PersonalCode = yBClinicInfoEntity.PersonalCode,
                                        IdNumber = yBClinicInfoEntity.IdNumber,
                                        Name = yBClinicInfoEntity.Name,
                                        Gender = yBClinicInfoEntity.Gender,
                                        Age = (int)yBClinicInfoEntity.Age,
                                        InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBClinicInfoEntity.InstitutionName,
                                        ICDCode = yBClinicInfoEntity.ICDCode,
                                        DiseaseName = yBClinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.ChildrenDrugCheckDescription(yBClinicInfoEntity.Name, yBClinicInfoEntity.IdNumber, yBClinicInfoEntity.InstitutionName),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (ChildrenDrugViewModel item in resultChildrenDrugModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.C001.ToString(),
                                            RulesName = CheckRules.C001.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "药品编码：" + item.DrugCode + ";药品名称：<a href='drugdescription.html?drugcode=" + item.DrugCode + "' target=\"_blank\" style=\"color:#FF0000;\">" + item.DrugName + "</a>;'" + item.Describe + "'"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }

                    }

                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }

        /// <summary>
        /// 医保目录限制用药范围
        /// </summary>
        /// <param name="yBClinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="childrenDrugKnowledges"></param>
        /// <param name="rulesChecks"></param>
        /// <returns></returns>
        private static Task MedicalLimitDrugCheck(YBClinicInfoEntity yBClinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<MedicalLimitDrugEntity> medicalLimitDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.B002.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity != null && yBClinicPreInfoEntity.Count > 0)
                    {
                        List<MedicalLimitDrugViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new MedicalLimitDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            ICDCode = yBClinicInfoEntity.ICDCode,
                            DiseaseName = yBClinicInfoEntity.DiseaseName,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<MedicalLimitDrugViewModel> medicalLimitDrugViewModels = medicalLimitDrugKnowledges.Select(m => new MedicalLimitDrugViewModel
                            {
                                DrugCode = m.DrugCode,
                                DrugName = m.DrugName,
                                ICDCode = m.DiseaseCode,
                                DiseaseName = m.DiseaseName,
                                Describe = m.Describe
                            }).ToList();
                            if (medicalLimitDrugViewModels.Count > 0)
                            {
                                //比较
                                List<MedicalLimitDrugViewModel> resultMedicalLimitDrugModels = clinicPreModels.Intersect(medicalLimitDrugViewModels, new MedicalLimitDrugComparer()).ToList();
                                if (resultMedicalLimitDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.B002.ToString(),
                                        RulesName = CheckRules.B002.GetDescription(),
                                        RegisterCode = yBClinicInfoEntity.ClinicRegisterCode,
                                        PersonalCode = yBClinicInfoEntity.PersonalCode,
                                        IdNumber = yBClinicInfoEntity.IdNumber,
                                        Name = yBClinicInfoEntity.Name,
                                        Gender = yBClinicInfoEntity.Gender,
                                        Age = (int)yBClinicInfoEntity.Age,
                                        InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBClinicInfoEntity.InstitutionName,
                                        ICDCode = yBClinicInfoEntity.ICDCode,
                                        DiseaseName = yBClinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.MedicalLimitDrugCheckDescription(yBClinicInfoEntity.Name, yBClinicInfoEntity.IdNumber, yBClinicInfoEntity.InstitutionName, yBClinicInfoEntity.DiseaseName),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (MedicalLimitDrugViewModel item in resultMedicalLimitDrugModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.B002.ToString(),
                                            RulesName = CheckRules.B002.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "药品编码：" + item.DrugCode + ";药品名称：<a href='drugdescription.html?drugcode=" + item.DrugCode + "' target=\"_blank\" style=\"color:#FF0000;\">" + item.DrugName + "</a>;'" + item.Describe + "'"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }

                    }

                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }
        /// <summary>
        /// 限定性别用药审核
        /// </summary>
        /// <param name="yBClinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="sexDrugKnowledges"></param>
        /// <param name="rulesChecks"></param>
        /// <returns></returns>
        private static Task SexDrugCheck(YBClinicInfoEntity yBClinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<SexDrugEntity> sexDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C002.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity != null && yBClinicPreInfoEntity.Count > 0)
                    {
                        List<SexDrugViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new SexDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            Sex = yBClinicInfoEntity.Gender == "男性" ? "1" : "2",
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<SexDrugViewModel> sexDrugViewModels = sexDrugKnowledges.Select(m => new SexDrugViewModel
                            {
                                DrugCode = m.DrugCode,
                                DrugName = m.DrugName,
                                Sex = m.Sex,
                                Describe = m.Describe
                            }).ToList();
                            if (sexDrugViewModels.Count > 0)
                            {
                                //比较
                                List<SexDrugViewModel> resultSexDrugModels = clinicPreModels.Intersect(sexDrugViewModels, new SexDrugComparer()).ToList();
                                if (resultSexDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C002.ToString(),
                                        RulesName = CheckRules.C002.GetDescription(),
                                        RegisterCode = yBClinicInfoEntity.ClinicRegisterCode,
                                        PersonalCode = yBClinicInfoEntity.PersonalCode,
                                        IdNumber = yBClinicInfoEntity.IdNumber,
                                        Name = yBClinicInfoEntity.Name,
                                        Gender = yBClinicInfoEntity.Gender,
                                        Age = (int)yBClinicInfoEntity.Age,
                                        InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBClinicInfoEntity.InstitutionName,
                                        ICDCode = yBClinicInfoEntity.ICDCode,
                                        DiseaseName = yBClinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.SexDrugCheckDescription(yBClinicInfoEntity.Name, yBClinicInfoEntity.IdNumber, yBClinicInfoEntity.InstitutionName, yBClinicInfoEntity.Gender),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (SexDrugViewModel item in resultSexDrugModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.C002.ToString(),
                                            RulesName = CheckRules.C002.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "药品编码：" + item.DrugCode + ";药品名称：<a href='drugdescription.html?drugcode=" + item.DrugCode + "' target=\"_blank\" style=\"color:#FF0000;\">" + item.DrugName + "</a>;'" + item.Describe + "'"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }

                    }

                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }
        /// <summary>
        /// 限老年人用药审核
        /// </summary>
        /// <param name="yBClinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="oldDrugKnowledges"></param>
        /// <param name="rulesChecks"></param>       
        /// <returns></returns>
        private static Task OldDrugCheck(YBClinicInfoEntity yBClinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<OldDrugEntity> oldDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C003.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity != null && yBClinicPreInfoEntity.Count > 0)
                    {
                        List<OldDrugViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new OldDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            StartAge = 0,
                            EndAge = 0,
                            CurrentAge = (int)yBClinicInfoEntity.Age,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<OldDrugViewModel> oldDrugViewModels = oldDrugKnowledges.Select(m => new OldDrugViewModel
                            {
                                DrugCode = m.DrugCode,
                                DrugName = m.DrugName,
                                StartAge = m.StartAge,
                                EndAge = m.EndAge,
                                CurrentAge = 0,
                                Describe = m.Describe
                            }).ToList();
                            if (oldDrugViewModels.Count > 0)
                            {
                                //比较
                                List<OldDrugViewModel> resultOldDrugModels = clinicPreModels.Intersect(oldDrugViewModels, new OldDrugComparer()).ToList();
                                if (resultOldDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C003.ToString(),
                                        RulesName = CheckRules.C003.GetDescription(),
                                        RegisterCode = yBClinicInfoEntity.ClinicRegisterCode,
                                        PersonalCode = yBClinicInfoEntity.PersonalCode,
                                        IdNumber = yBClinicInfoEntity.IdNumber,
                                        Name = yBClinicInfoEntity.Name,
                                        Gender = yBClinicInfoEntity.Gender,
                                        Age = (int)yBClinicInfoEntity.Age,
                                        InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBClinicInfoEntity.InstitutionName,
                                        ICDCode = yBClinicInfoEntity.ICDCode,
                                        DiseaseName = yBClinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.OldDrugCheckDescription(yBClinicInfoEntity.Name, yBClinicInfoEntity.IdNumber, yBClinicInfoEntity.InstitutionName),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (OldDrugViewModel item in resultOldDrugModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.C003.ToString(),
                                            RulesName = CheckRules.C003.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "药品编码：" + item.DrugCode + ";药品名称：<a href='drugdescription.html?drugcode=" + item.DrugCode + "' target=\"_blank\" style=\"color:#FF0000;\">" + item.DrugName + "</a>;'" + item.Describe + "'"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }

                    }

                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }
        /// <summary>
        /// 限定性别诊疗服务审核
        /// </summary>
        /// <param name="yBClinicInfoEntity"></param>
        /// <param name="yBClinicPreInfoEntity"></param>
        /// <param name="sexItemLimitKnowledge"></param>
        /// <param name="rulesChecks"></param>       
        /// <returns></returns>
        private static Task SexItemCheck(YBClinicInfoEntity yBClinicInfoEntity, List<YBClinicPreInfoEntity> yBClinicPreInfoEntity, List<SexItemLimitEntity> sexItemLimitKnowledge, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.D001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把药品过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBClinicPreInfoEntity != null && yBClinicPreInfoEntity.Count > 0)
                    {
                        List<SexItemViewModel> clinicPreModels = yBClinicPreInfoEntity.Where(it => !array.Contains(it.CollectFeesCategoryCode)).Select(m => new SexItemViewModel
                        {
                            ItemCode = m.ItemCode,
                            ItemName = m.ItemName,
                            Sex = yBClinicInfoEntity.Gender == "男性" ? "1" : "2",
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.ClinicRegisterCode

                        }).ToList();
                        if (clinicPreModels != null && clinicPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<SexItemViewModel> sexItemViewModels = sexItemLimitKnowledge.Select(m => new SexItemViewModel
                            {
                                ItemCode = m.ItemCode,
                                ItemName = m.ItemName,
                                Sex = m.Sex,
                                Describe = m.Describe
                            }).ToList();
                            if (sexItemViewModels.Count > 0)
                            {
                                //比较
                                List<SexItemViewModel> resultSexItemModels = clinicPreModels.Intersect(sexItemViewModels, new SexItemComparer()).ToList();
                                if (resultSexItemModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.D001.ToString(),
                                        RulesName = CheckRules.D001.GetDescription(),
                                        RegisterCode = yBClinicInfoEntity.ClinicRegisterCode, 
                                        PersonalCode = yBClinicInfoEntity.PersonalCode,
                                        IdNumber = yBClinicInfoEntity.IdNumber,
                                        Name = yBClinicInfoEntity.Name,
                                        Gender = yBClinicInfoEntity.Gender,
                                        Age = (int)yBClinicInfoEntity.Age,
                                        InstitutionCode = yBClinicInfoEntity.InstitutionCode,
                                        InstitutionName = yBClinicInfoEntity.InstitutionName,
                                        ICDCode = yBClinicInfoEntity.ICDCode,
                                        DiseaseName = yBClinicInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.SexItemCheckDescription(yBClinicInfoEntity.Name, yBClinicInfoEntity.IdNumber, yBClinicInfoEntity.InstitutionName, yBClinicInfoEntity.Gender),
                                        IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                                    };
                                    checkResultInfoEntities.Add(checkResultInfoEntity);
                                    foreach (SexItemViewModel item in resultSexItemModels)
                                    {
                                        CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                        {
                                            RegisterCode = item.RegisterCode,
                                            CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                            RulesCode = CheckRules.D001.ToString(),
                                            RulesName = CheckRules.D001.GetDescription(),
                                            DataType = ((int)CheckDataTypeEnum.Clinic).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "诊疗编码：" + item.ItemCode + ";诊疗名称：" + item.ItemName + ";'" + item.Describe + "'"

                                        };
                                        checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
                                    }
                                }
                            }

                        }
                    }


                });
            }
            else
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("没有启用该规则");
                });
            }

        }

        /// <summary>
        /// 抽取数据后调用的审核方法
        /// </summary>
        /// <param name="institutionCodes"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AfterCheckActionAsyncByExtract(string codes)
        {
            List<string> institutionCodes = null;
            if (codes.Length > 0)
            {
                string[] str = codes.Split('|');
                institutionCodes = str.ToList();
            }
            
            List<RulesMainEntity> rulesMainEntities = _afterCheckService.GetRulesMainEntities();
            List<string> rulesCheck = new List<string>();
            foreach (RulesMainEntity item in rulesMainEntities)
            {
                rulesCheck.Add(item.RulesCode);
            }
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                QueryConditionByClinic queryConditionByClinic = new QueryConditionByClinic();

                int count = 0;//本次共计审核人次
                checkResultInfoEntities.Clear();//清空审核结果集合
                checkResultPreInfoEntities.Clear();//清空审核处方结果集合               
                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionByClinic.InstitutionCode = institutionCode;
                    List<YBClinicPreInfoEntity> yBClinicPreInfoEntitys = new List<YBClinicPreInfoEntity>();

                    ybClinicInfo = _afterCheckService.GetYBClinicInfos(queryConditionByClinic);//按条件获取门诊信息
                    count += ybClinicInfo.Count;
                    var groupPersonalList = ybClinicInfo.GroupBy(it => new { it.PersonalCode })//根据人员编码分组
                       .Select(m => new
                       {
                           m.Key.PersonalCode
                       }).ToList();

                    #region 获取知识库数据(从数据库或redis中取)
                    List<DiseaseNoNormalEntity> diseaseNoNormalinfo = _afterCheckService.GetDiseaseNoNormals_Knowledge();//就诊疾病异常审核A007
                    List<ItemLimitPriceEntity> itemLimitPricesKnowledge = _afterCheckService.GetItemLimitedPrice_Knowledge();//获取限定诊疗价格知识库B001
                    List<ChildrenDrugEntity> childrenDrugKnowledge = _afterCheckService.GetChildrenDrugEntity_Knowledge();//获取限儿童用药知识库C001
                    List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge = _afterCheckService.GetMedicalLimitedDrug_Knowledge();//获取医保目录限制用药范围知识库B002
                    List<SexDrugEntity> sexDrugKnowledge = _afterCheckService.GetSexDrug_Knowledge();//获取限定性别用药C002
                    List<OldDrugEntity> oldDrugKnowledge = _afterCheckService.GetOldDrug_Knowledge();//获取限老年人用药C003
                    List<SexItemLimitEntity> sexItemLimitKnowledge = _afterCheckService.GetSexItem_Knowledge();//获取限定性别诊疗服务D001
                    #endregion


                    foreach (YBClinicInfoEntity item in ybClinicInfo)
                    {
                        yBClinicPreInfoEntitys.Clear();
                        yBClinicPreInfoEntitys = _afterCheckService.GetYBClinicPreInfosByClinicRegisterCode(item.ClinicRegisterCode);//获取这个人的处方信息
                        AfterCheckAlgorithm(item, diseaseNoNormalinfo, yBClinicPreInfoEntitys, itemLimitPricesKnowledge, childrenDrugKnowledge,
                            medicalLimitDrugKnowledge, sexDrugKnowledge, oldDrugKnowledge, sexItemLimitKnowledge, rulesCheck);
                    }
                }
                #region 插入数据库
                int Error = 0;
                int tranError = 0;
                if (checkResultInfoEntities.Count <= 0)
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有审核违规结果";
                    return Ok(resultCountModel);
                }
                List<CheckResultInfoEntity> checkResultInfoEntitiesInsert = new List<CheckResultInfoEntity>();//本次要插入表的审核信息结果
                List<CheckResultPreInfoEntity> checkResultPreInfoEntitiesInsert = new List<CheckResultPreInfoEntity>();//本次要插入表的审核处方信息结果
                List<string> ybClinicRegisterCodesYes = new List<string>();//有问题的门诊登记编码
                List<string> ybClinicRegisterCodesNo = new List<string>();//无问题的门诊登记编码

                ybclinicinfoByYes.Clear();//有问题的患者对象（暂时好像多余）
                ybclinicinfoByNo.Clear();//无问题的患者对象（暂时好像多余）
                List<string> clinicinfoRedisKeys = new List<string>();//存储医保门诊信息缓存Key值

                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionByClinic.InstitutionCode = institutionCode;
                    ybClinicInfo = _afterCheckService.GetYBClinicInfos(queryConditionByClinic);//按条件获取门诊信息
                    List<CheckResultInfoEntity> checkResultInfoEntitiesByIns = checkResultInfoEntities.Where(it => it.InstitutionCode == institutionCode && it.DataType == "1").ToList();
                    foreach (YBClinicInfoEntity item in ybClinicInfo)
                    {
                        if (checkResultInfoEntitiesByIns.Where(it => it.RegisterCode == item.ClinicRegisterCode).FirstOrDefault() == null)
                            ybClinicRegisterCodesNo.Add(item.ClinicRegisterCode);
                    }

                    foreach (CheckResultInfoEntity checkResultInfoitem in checkResultInfoEntitiesByIns)
                    {
                        ybClinicRegisterCodesYes.Add(checkResultInfoitem.RegisterCode);
                        if (ybClinicInfo.Where(it => it.ClinicRegisterCode == checkResultInfoitem.RegisterCode && it.States == ((int)CheckStates.A).ToString()).FirstOrDefault() != null)
                        {
                            List<CheckResultPreInfoEntity> checkResultPreInfoEntities_temp = new List<CheckResultPreInfoEntity>();
                            checkResultInfoEntitiesInsert.Add(checkResultInfoitem);
                            checkResultPreInfoEntities_temp = checkResultPreInfoEntities_temp.Where(it => it.RegisterCode == checkResultInfoitem.RegisterCode && it.RulesCode == checkResultInfoitem.RulesCode && it.DataType == "1").ToList();
                            if (checkResultPreInfoEntities_temp.Count > 0)
                            {
                                checkResultPreInfoEntitiesInsert.AddRange(checkResultPreInfoEntities);
                            }


                        }
                    }
                }

                TransactionOptions transactionOption = new TransactionOptions();
                //设置事务隔离级别
                transactionOption.IsolationLevel = IsolationLevel.ReadCommitted;
                // 设置事务超时时间为60秒
                transactionOption.Timeout = new TimeSpan(0, 0, 60000);
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
                {
                    try
                    {
                        if (ybClinicRegisterCodesYes.Count > 0)
                        {
                            //更新医保库数据                        
                            if (!_afterCheckService.UpdateYbClinicInfo(ybClinicRegisterCodesYes, ((int)CheckStates.B).ToString()))
                                Error++;
                        }
                        if (ybClinicRegisterCodesNo.Count > 0)
                        {
                            if (!_afterCheckService.UpdateYbClinicInfo(ybClinicRegisterCodesNo, ((int)CheckStates.C).ToString()))
                                Error++;
                        }
                        if (checkResultInfoEntitiesInsert.Count > 0)
                        {
                            if (!_afterCheckService.InsertResultInfo(checkResultInfoEntitiesInsert))
                                Error++;
                        }
                        if (checkResultPreInfoEntitiesInsert.Count > 0)
                        {
                            if (!_afterCheckService.InsertResultPreInfo(checkResultPreInfoEntitiesInsert))
                                Error++;
                        }
                        if (Error == 0)
                        {
                            tranError++;
                            resultCountModel.code = 0;
                            resultCountModel.msg = "审核结果保存成功";
                            ts.Complete();
                        }
                        else
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存失败";

                        }
                    }
                    catch (Exception ex)
                    {
                        ts.Dispose();
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存失败!" + ex.Message;

                    }
                }
                if (tranError > 0)//事务成功提交且同步数据
                {
                    if (ybClinicRegisterCodesYes.Count > 0)
                    {
                        try
                        {
                            _afterCheckService.UpdateYBData("1", ybClinicRegisterCodesYes, ((int)CheckStates.B).ToString());
                        }
                        catch
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                            return Ok(resultCountModel);
                        }
                    }
                    if (ybClinicRegisterCodesNo.Count > 0)//审核无问题数据
                    {
                        try
                        {
                            _afterCheckService.UpdateYBData("1", ybClinicRegisterCodesNo, ((int)CheckStates.C).ToString());
                        }
                        catch
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                            return Ok(resultCountModel);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "出现异常!" + ex.Message;

            }
            return Ok(resultCountModel);
        }

    }
}
