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
    public class AfterCheckEngineController : ControllerBase
    {
        private readonly IAfterCheckService _afterCheckService;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        public static List<CheckResultPreInfoEntity> checkResultPreInfoEntities = new List<CheckResultPreInfoEntity>();//审核结果处方信息
        public static List<CheckResultInfoEntity> checkResultInfoEntities = new List<CheckResultInfoEntity>();//审核结果信息
        private static List<YBHosInfoEntity> Ybhosinfo { get; set; } //根据条件获取的住院患者信息
        private static List<YBHosInfoEntity> PersonInhosInfo { get; set; }//存储个人的住院信息（一个人住过几次院）

        public static List<YBHosInfoEntity> ybhosinfoByYes = new List<YBHosInfoEntity>(); //审核完成有问题的患者
        public static List<YBHosInfoEntity> ybhosinfoByNo = new List<YBHosInfoEntity>(); //审核完成无问题的患者

        public AfterCheckEngineController(IAfterCheckService afterCheckService, IMapper mapper, IRedisDbContext redisDbContext)
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
        public IActionResult GetCheckResultStatus()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckResultStatus();
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
        public IActionResult GetSaveCheckResultStatus()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetSaveCheckResultStatus();
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

        public IActionResult AfterCheckResultSubmit(string checkNumber, string checkPreNumber, List<string> institutionCodes, string syncYb)
        {
            var resultCountModel = new RespResultCountViewModel();
            int Error = 0;
            int tranError = 0;
            QueryConditionViewModel queryConditionViewModel = new QueryConditionViewModel();
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
            List<string> ybHosRegisterCodesYes = new List<string>();//有问题的住院登记编码
            List<string> ybHosRegisterCodesNo = new List<string>();//无问题的住院登记编码
            ybhosinfoByYes.Clear();//有问题的患者对象（暂时好像多余）
            ybhosinfoByNo.Clear();//无问题的患者对象（暂时好像多余）
            List<string> hosinfoRedisKeys = new List<string>();//存储医保住院信息缓存Key值
            CheckResultStatusSaveEntity checkResultStatusSaveEntityStart = new CheckResultStatusSaveEntity() { CRowId = 1, SaveCheckResultStatus = "N" };
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
                queryConditionViewModel.InstitutionCode = institutionCode;
                Ybhosinfo = _afterCheckService.GetYBHosInfos(queryConditionViewModel);//按条件获取住院信息
                List<CheckResultInfoEntity> checkResultInfoEntitiesByIns = checkResultInfoList.Where(it => it.InstitutionCode == institutionCode && it.DataType == "2").ToList();
                foreach (YBHosInfoEntity item in Ybhosinfo)
                {
                    if (checkResultInfoEntitiesByIns.Where(it => it.RegisterCode == item.HosRegisterCode).FirstOrDefault() == null)
                        ybHosRegisterCodesNo.Add(item.HosRegisterCode);
                }

                foreach (CheckResultInfoEntity checkResultInfoitem in checkResultInfoEntitiesByIns)
                {
                    ybHosRegisterCodesYes.Add(checkResultInfoitem.RegisterCode);
                    if (Ybhosinfo.Where(it => it.HosRegisterCode == checkResultInfoitem.RegisterCode && it.States == ((int)CheckStates.A).ToString()).FirstOrDefault() != null)
                    {
                        List<CheckResultPreInfoEntity> checkResultPreInfoEntities_temp = new List<CheckResultPreInfoEntity>();
                        checkResultInfoEntitiesInsert.Add(checkResultInfoitem);
                        checkResultPreInfoEntities_temp = checkResultPreList.Where(it => it.RegisterCode == checkResultInfoitem.RegisterCode && it.RulesCode == checkResultInfoitem.RulesCode && it.DataType == "2").ToList();
                        if (checkResultPreInfoEntities_temp.Count > 0)
                        {
                            checkResultPreInfoEntitiesInsert.AddRange(checkResultPreInfoEntities_temp);
                        }


                    }
                }
                hosinfoRedisKeys.Add(SystemManageConst.YBINFO_HOS + queryConditionViewModel.InstitutionCode);
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
                    if (ybHosRegisterCodesYes.Count > 0)
                    {
                        if (!_afterCheckService.UpdateYbHosInfo(ybHosRegisterCodesYes, ((int)CheckStates.B).ToString()))
                            Error++;
                    }
                    if (ybHosRegisterCodesNo.Count > 0)
                    {
                        if (!_afterCheckService.UpdateYbHosInfo(ybHosRegisterCodesNo, ((int)CheckStates.C).ToString()))
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
                    if (hosinfoRedisKeys.Count > 0)
                    {
                        using (var db = _redisDbContext.GetRedisIntance())
                        {
                            db.Del(hosinfoRedisKeys.ToArray());
                            db.Del(checkNumber);
                            db.Del(checkPreNumber);
                            db.Del(SystemManageConst.HomeIndex_KEY);
                        }
                    }
                    if (Error == 0)
                    {
                        CheckResultStatusSaveEntity checkResultStatusSaveEntityEndOne = new CheckResultStatusSaveEntity() { CRowId = 1, SaveCheckResultStatus = "Y" };
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
                        CheckResultStatusSaveEntity checkResultStatusSaveEntityEndTwo = new CheckResultStatusSaveEntity() { CRowId = 1, SaveCheckResultStatus = "E" };
                        bool checkStatusResultEndTwo = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityEndTwo);
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存失败";

                    }
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    CheckResultStatusSaveEntity checkResultStatusSaveEntityEndTThree = new CheckResultStatusSaveEntity() { CRowId = 1, SaveCheckResultStatus = "E" };
                    bool checkStatusResultEndTThree = _afterCheckService.UpdateSaveResultStatus(checkResultStatusSaveEntityEndTThree);
                    resultCountModel.code = -1;
                    resultCountModel.msg = "审核结果保存失败!" + ex.Message;

                }
            }
            if (tranError > 0 && syncYb == "1")//事务成功提交且同步数据
            {
                if (ybHosRegisterCodesYes.Count > 0)//审核有问题数据
                {
                    try
                    {
                        _afterCheckService.UpdateYBData("2", ybHosRegisterCodesYes, ((int)CheckStates.B).ToString());
                    }
                    catch
                    {
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                        return Ok(resultCountModel);
                    }
                }
                if (ybHosRegisterCodesNo.Count > 0)//审核无问题数据
                {
                    try
                    {
                        _afterCheckService.UpdateYBData("2", ybHosRegisterCodesNo, ((int)CheckStates.C).ToString());
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
                QueryConditionViewModel queryConditionViewModel = new QueryConditionViewModel();
                CheckResultStatusEntity checkResultStatus = new CheckResultStatusEntity() { CRowId = 1, CheckNumber = "N", CheckResultStatus = "N", CheckPreNumber = "N", CheckCount = 0 };
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
                string checkNumber = "ResultInfo" + CommonHelper.CreateNo();
                string checkPreNumber = "ResultPreInfo" + CommonHelper.CreateNo();
                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionViewModel.InstitutionCode = institutionCode;
                    List<YBHosPreInfoEntity> yBHosPreInfoEntitys = new List<YBHosPreInfoEntity>();
                    Ybhosinfo = _afterCheckService.GetYBHosInfos(queryConditionViewModel);//按条件获取住院信息
                    count += Ybhosinfo.Count;
                    var groupPersonalList = Ybhosinfo.GroupBy(it => new { it.PersonalCode })//根据人员编码分组
                       .Select(m => new
                       {
                           m.Key.PersonalCode
                       }).ToList();

                    #region 获取知识库数据(从数据库或redis中取)
                    List<DiseaseNoNormalEntity> diseaseNoNormalinfo = _afterCheckService.GetDiseaseNoNormals_Knowledge();//就诊疾病异常审核A007
                    List<RulesMainEntity> inHosDayRulesKnowledge = _afterCheckService.GetInHosDayUnusual_Knowledge();//住院天数异常知识库A003
                    RulesMainEntity disintegrateInHosKnowledge = _afterCheckService.GetDisintegrateInHos_Knowledge();//分解住院A004
                    int inHosDayValue = 7;//分解住院默认七天
                    if (disintegrateInHosKnowledge != null)
                        inHosDayValue = int.Parse(disintegrateInHosKnowledge.RulesValue);
                    List<ItemLimitPriceEntity> itemLimitPricesKnowledge = _afterCheckService.GetItemLimitedPrice_Knowledge();//获取限定诊疗价格知识库B001
                    List<ChildrenDrugEntity> childrenDrugKnowledge = _afterCheckService.GetChildrenDrugEntity_Knowledge();//获取限儿童用药知识库C001
                    List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge = _afterCheckService.GetMedicalLimitedDrug_Knowledge();//获取医保目录限制用药范围知识库B002
                    List<SexDrugEntity> sexDrugKnowledge = _afterCheckService.GetSexDrug_Knowledge();//获取限定性别用药C002
                    List<OldDrugEntity> oldDrugKnowledge = _afterCheckService.GetOldDrug_Knowledge();//获取限老年人用药C003
                    List<SexItemLimitEntity> sexItemLimitKnowledge = _afterCheckService.GetSexItem_Knowledge();//获取限定性别诊疗服务D001
                    #endregion


                    foreach (YBHosInfoEntity item in Ybhosinfo)
                    {
                        yBHosPreInfoEntitys.Clear();
                        yBHosPreInfoEntitys = _afterCheckService.GetYBHosPreInfosByHosRegisterCode(item.HosRegisterCode);//获取这个人的处方信息
                        AfterCheckAlgorithm(item, diseaseNoNormalinfo, inHosDayRulesKnowledge,
                            yBHosPreInfoEntitys, itemLimitPricesKnowledge, childrenDrugKnowledge,
                            medicalLimitDrugKnowledge, sexDrugKnowledge, oldDrugKnowledge, sexItemLimitKnowledge, rulesCheck);
                    }

                    #region 分解住院审核（无法同步，单独审核）
                    if (rulesCheck.Exists(it => it == CheckRules.A004.ToString()))
                    {
                        foreach (var item in groupPersonalList)
                        {
                            List<YBHosInfoEntity> disintegrateInHos = new List<YBHosInfoEntity>();
                            //获取一个人在同一家医院所有的医保住院信息
                            QueryConditionViewModel queryConditionView = new QueryConditionViewModel() { InstitutionCode = queryConditionViewModel.InstitutionCode, PersonalCode = item.PersonalCode };
                            PersonInhosInfo = _afterCheckService.GetYBHosInfosByPersonalByIns(queryConditionView).OrderBy(it => it.InHosDate).ThenBy(it => it.OutHosDate).ToList();

                            if (PersonInhosInfo.Count > 1)
                            {
                                for (int i = PersonInhosInfo.Count - 1; i >= 0; i--)
                                {
                                    DateTime inHosdate = PersonInhosInfo[i].InHosDate;
                                    PersonInhosInfo.Remove(PersonInhosInfo[i]);
                                    for (int j = PersonInhosInfo.Count - 1; j >= 0; j--)
                                    {
                                        if (CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) >= 0 && CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) <= inHosDayValue)
                                        {
                                            if (disintegrateInHos.Where(it => it.PersonalCode == PersonInhosInfo[j].PersonalCode).ToList().Count <= 0)
                                                disintegrateInHos.Add(PersonInhosInfo[j]);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (disintegrateInHos.Count > 0)
                            {
                                CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                {
                                    CheckResultInfoCode = CommonHelper.CreateNo(),
                                    RulesCode = CheckRules.A004.ToString(),
                                    RulesName = CheckRules.A004.GetDescription(),
                                    RegisterCode = disintegrateInHos[0].HosRegisterCode,
                                    PersonalCode = disintegrateInHos[0].PersonalCode,
                                    IdNumber = disintegrateInHos[0].IdNumber,
                                    Name = disintegrateInHos[0].Name,
                                    Gender = disintegrateInHos[0].Gender,
                                    Age = disintegrateInHos[0].Age,
                                    InstitutionCode = disintegrateInHos[0].InstitutionCode,
                                    InstitutionName = disintegrateInHos[0].InstitutionName,
                                    ICDCode = disintegrateInHos[0].ICDCode,
                                    DiseaseName = disintegrateInHos[0].DiseaseName,
                                    DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                    CheckDate = DateTime.Now,
                                    ResultDescription = SystemManageConst.ResolveInHosCheckDescription(disintegrateInHos[0].Name, disintegrateInHos[0].Age, inHosDayValue, disintegrateInHos[0].InstitutionName),
                                    IsPre = ((int)CheckIsPreEnum.No).ToString()

                                };
                                checkResultInfoEntities.Add(checkResultInfoEntity);
                            }

                        }
                    }
                    #endregion

                    #region 入出院日期异常（无法同步，单独审核）
                    if (rulesCheck.Exists(it => it == CheckRules.A006.ToString()))
                    {
                        foreach (var item in groupPersonalList)
                        {
                            List<YBHosInfoEntity> inOutHos = new List<YBHosInfoEntity>();
                            PersonInhosInfo = _afterCheckService.GetYBHosInfosByPersonalCode(item.PersonalCode).OrderBy(it => it.InHosDate).ThenBy(it => it.OutHosDate).ToList();
                            if (PersonInhosInfo.Count > 1)
                            {
                                for (int i = PersonInhosInfo.Count - 1; i >= 0; i--)
                                {
                                    DateTime inHosdate = PersonInhosInfo[i].InHosDate;
                                    PersonInhosInfo.Remove(PersonInhosInfo[i]);
                                    for (int j = PersonInhosInfo.Count - 1; j >= 0; j--)
                                    {
                                        if (CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) < 0)
                                        {
                                            if (inOutHos.Where(it => it.PersonalCode == PersonInhosInfo[j].PersonalCode).ToList().Count <= 0)
                                                inOutHos.Add(PersonInhosInfo[j]);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (inOutHos.Count > 0)
                            {
                                CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                {
                                    CheckResultInfoCode = CommonHelper.CreateNo(),
                                    RulesCode = CheckRules.A006.ToString(),
                                    RulesName = CheckRules.A006.GetDescription(),
                                    RegisterCode = inOutHos[0].HosRegisterCode,
                                    PersonalCode = inOutHos[0].PersonalCode,
                                    IdNumber = inOutHos[0].IdNumber,
                                    Name = inOutHos[0].Name,
                                    Gender = inOutHos[0].Gender,
                                    Age = inOutHos[0].Age,
                                    InstitutionCode = inOutHos[0].InstitutionCode,
                                    InstitutionName = inOutHos[0].InstitutionName,
                                    ICDCode = inOutHos[0].ICDCode,
                                    DiseaseName = inOutHos[0].DiseaseName,
                                    DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                    CheckDate = DateTime.Now,
                                    ResultDescription = SystemManageConst.InOutHosCheckDescription(inOutHos[0].Name, inOutHos[0].IdNumber),
                                    IsPre = ((int)CheckIsPreEnum.No).ToString()

                                };
                                checkResultInfoEntities.Add(checkResultInfoEntity);
                            }
                        }
                    }

                    #endregion
                }

                CheckResultStatusEntity checkResultStatusEnd = new CheckResultStatusEntity() { CRowId = 1, CheckNumber = checkNumber, CheckResultStatus = "Y", CheckPreNumber = checkPreNumber, CheckCount = count };
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
        private void AfterCheckAlgorithm(YBHosInfoEntity yBHosInfoEntity,
            List<DiseaseNoNormalEntity> diseaseNoNormalinfo,
            List<RulesMainEntity> inHosDayRulesKnowledge,
            List<YBHosPreInfoEntity> yBHosPreInfoEntity,
            List<ItemLimitPriceEntity> itemLimitPriceEntitie,
            List<ChildrenDrugEntity> childrenDrugKnowledges,
            List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge,
            List<SexDrugEntity> sexDrugKnowledges,
            List<OldDrugEntity> oldDrugKnowledges, List<SexItemLimitEntity> sexItemLimitKnowledges, List<string> rulesChecks)
        {
            // 就诊疾病异常审核
            Task diseaseNoNormalCheck = DiseaseNoNormalCheck(yBHosInfoEntity, diseaseNoNormalinfo, rulesChecks);
            //住院天数异常审核
            Task inHosDayUnusualCheck = InHosDayUnusualCheck(yBHosInfoEntity, inHosDayRulesKnowledge, rulesChecks);
            //床位费与住院天数不符
            Task berthHosDayNotCheck = BerthHosDayNotCheck(yBHosInfoEntity, yBHosPreInfoEntity, rulesChecks);
            //限定诊疗价格审核
            Task limitItemPriceCheck = LimitItemPriceCheck(yBHosInfoEntity, yBHosPreInfoEntity, itemLimitPriceEntitie, rulesChecks);
            //限儿童用药审核
            Task childrenDrugCheck = ChildrenDrugCheck(yBHosInfoEntity, yBHosPreInfoEntity, childrenDrugKnowledges, rulesChecks);
            //医保目录限制用药范围
            Task medicalLimitDrugCheck = MedicalLimitDrugCheck(yBHosInfoEntity, yBHosPreInfoEntity, medicalLimitDrugKnowledge, rulesChecks);
            //限定性别用药
            Task sexDrugCheck = SexDrugCheck(yBHosInfoEntity, yBHosPreInfoEntity, sexDrugKnowledges, rulesChecks);
            //限老年人用药审核
            Task oldDrugCheck = OldDrugCheck(yBHosInfoEntity, yBHosPreInfoEntity, oldDrugKnowledges, rulesChecks);
            //限定性别诊疗服务
            Task sexItemCheck = SexItemCheck(yBHosInfoEntity, yBHosPreInfoEntity, sexItemLimitKnowledges, rulesChecks);

            //等待任务结束
            Task.WaitAll(diseaseNoNormalCheck, inHosDayUnusualCheck, berthHosDayNotCheck,
                limitItemPriceCheck, childrenDrugCheck, medicalLimitDrugCheck,
                sexDrugCheck, oldDrugCheck);
        }

        /// <summary>
        /// 就诊疾病异常审核
        /// </summary>
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task DiseaseNoNormalCheck(YBHosInfoEntity yBHosInfoEntity, List<DiseaseNoNormalEntity> diseaseNoNormalinfo, List<string> rulesChecks)
        {
            if (rulesChecks.Exists(it => it == CheckRules.A007.ToString()))
            {
                return Task.Run(() =>
                {
                    List<DiseaseNoNormalEntity> isResult = diseaseNoNormalinfo.Where(it => it.StartAge <= yBHosInfoEntity.Age && it.EndAge >= yBHosInfoEntity.Age && it.DiseaseCode == yBHosInfoEntity.ICDCode).ToList();
                    if (isResult != null && isResult.Count > 0)
                    {
                        CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                        {
                            CheckResultInfoCode = CommonHelper.CreateNo(),
                            RulesCode = CheckRules.A007.ToString(),
                            RulesName = CheckRules.A007.GetDescription(),
                            RegisterCode = yBHosInfoEntity.HosRegisterCode,
                            PersonalCode = yBHosInfoEntity.PersonalCode,
                            IdNumber = yBHosInfoEntity.IdNumber,
                            Name = yBHosInfoEntity.Name,
                            Gender = yBHosInfoEntity.Gender,
                            Age = yBHosInfoEntity.Age,
                            InstitutionCode = yBHosInfoEntity.InstitutionCode,
                            InstitutionName = yBHosInfoEntity.InstitutionName,
                            ICDCode = yBHosInfoEntity.ICDCode,
                            DiseaseName = yBHosInfoEntity.DiseaseName,
                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                            CheckDate = DateTime.Now,
                            ResultDescription = SystemManageConst.DiseaseNoNormalCheckResultDescription(yBHosInfoEntity.Name, yBHosInfoEntity.Age, yBHosInfoEntity.DiseaseName),
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
        /// 住院天数异常审核
        /// </summary>
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task InHosDayUnusualCheck(YBHosInfoEntity yBHosInfoEntity, List<RulesMainEntity> inHosDayRulesMainEntities, List<string> rulesChecks)
        {
            if (rulesChecks.Exists(it => it == CheckRules.A003.ToString()))
            {
                return Task.Run(() =>
                {
                    List<RulesMainEntity> isResult = inHosDayRulesMainEntities.Where(it => int.Parse(it.RulesValue) >= yBHosInfoEntity.InHosDay).ToList();
                    if (isResult != null && isResult.Count > 0)
                    {
                        CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                        {
                            CheckResultInfoCode = CommonHelper.CreateNo(),
                            RulesCode = CheckRules.A003.ToString(),
                            RulesName = CheckRules.A003.GetDescription(),
                            RegisterCode = yBHosInfoEntity.HosRegisterCode,
                            PersonalCode = yBHosInfoEntity.PersonalCode,
                            IdNumber = yBHosInfoEntity.IdNumber,
                            Name = yBHosInfoEntity.Name,
                            Gender = yBHosInfoEntity.Gender,
                            Age = yBHosInfoEntity.Age,
                            InstitutionCode = yBHosInfoEntity.InstitutionCode,
                            InstitutionName = yBHosInfoEntity.InstitutionName,
                            ICDCode = yBHosInfoEntity.ICDCode,
                            DiseaseName = yBHosInfoEntity.DiseaseName,
                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                            CheckDate = DateTime.Now,
                            ResultDescription = SystemManageConst.InHosDayUnusualCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.Age, yBHosInfoEntity.DiseaseName, yBHosInfoEntity.InstitutionName, yBHosInfoEntity.InHosDate.ToString(), yBHosInfoEntity.OutHosDate.ToString(), yBHosInfoEntity.InHosDay),
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
        /// 床位费与住院天数不符
        /// </summary>
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task BerthHosDayNotCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<string> rulesChecks)
        {
            if (rulesChecks.Exists(it => it == CheckRules.A005.ToString()))
            {
                return Task.Run(() =>
                {
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        var groupBerthHosBySum = yBHosPreInfoEntity.Where(it => it.CollectFeesCategoryCode == SystemManageConst.BERTH_CODE).GroupBy(m => new { m.HosRegisterCode }).
                   Select(it => new
                   {
                       reshoscode = it.Key.HosRegisterCode,
                       BerthHosBySum = it.Sum(s => s.COUNT)
                   }).ToList();
                        if (groupBerthHosBySum.Count > 0 && yBHosInfoEntity.InHosDay != groupBerthHosBySum[0].BerthHosBySum && groupBerthHosBySum[0].BerthHosBySum - yBHosInfoEntity.InHosDay > 1)
                        {
                            CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                            {
                                CheckResultInfoCode = CommonHelper.CreateNo(),
                                RulesCode = CheckRules.A005.ToString(),
                                RulesName = CheckRules.A005.GetDescription(),
                                RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                PersonalCode = yBHosInfoEntity.PersonalCode,
                                IdNumber = yBHosInfoEntity.IdNumber,
                                Name = yBHosInfoEntity.Name,
                                Gender = yBHosInfoEntity.Gender,
                                Age = yBHosInfoEntity.Age,
                                InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                InstitutionName = yBHosInfoEntity.InstitutionName,
                                ICDCode = yBHosInfoEntity.ICDCode,
                                DiseaseName = yBHosInfoEntity.DiseaseName,
                                DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                CheckDate = DateTime.Now,
                                ResultDescription = SystemManageConst.BerthHosDayNotCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.DiseaseName, yBHosInfoEntity.InstitutionName, yBHosInfoEntity.InHosDate.ToString(), yBHosInfoEntity.OutHosDate.ToString(), yBHosInfoEntity.InHosDay, groupBerthHosBySum[0].BerthHosBySum),
                                IsPre = ((int)CheckIsPreEnum.Yes).ToString()
                            };
                            checkResultInfoEntities.Add(checkResultInfoEntity);
                            foreach (YBHosPreInfoEntity item in yBHosPreInfoEntity.Where(it => it.CollectFeesCategoryCode == SystemManageConst.BERTH_CODE))
                            {
                                CheckResultPreInfoEntity checkResultPreInfoEntity = new CheckResultPreInfoEntity()
                                {
                                    RegisterCode = item.HosRegisterCode,
                                    CheckResultInfoCode = checkResultInfoEntity.CheckResultInfoCode,
                                    ItemIndex = item.ItemIndex,
                                    RulesCode = CheckRules.A005.ToString(),
                                    RulesName = CheckRules.A005.GetDescription(),
                                    DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                    CheckDate = DateTime.Now,
                                    PreCode = item.PreCode,
                                    CheckResultPreInfoCode = CommonHelper.CreateNo()
                                };
                                checkResultPreInfoEntities.Add(checkResultPreInfoEntity);
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
        /// 限定诊疗价格审核
        /// </summary>
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task LimitItemPriceCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<ItemLimitPriceEntity> limitPriceEntitieKnowledge, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.B001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把药品过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity.Count > 0 && yBHosPreInfoEntity != null)
                    {
                        List<ItemLimitPriceViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => !array.Contains(it.CollectFeesCategoryCode)).Select(m => new ItemLimitPriceViewModel
                        {
                            ItemCode = m.ItemCode,
                            ItemName = m.ItemName,
                            ItemPrice = m.PRICE,
                            ItemLimitPrice = 0,
                            ItemIndex = m.ItemIndex,
                            PreCode = m.PreCode,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
                        {
                            //处理知识库信息
                            List<ItemLimitPriceViewModel> itemLimitPriceModels = new List<ItemLimitPriceViewModel>();
                            switch (yBHosInfoEntity.InstitutiongGradeCode)
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
                                List<ItemLimitPriceViewModel> resultItemLimitModels = hosPreModels.Intersect(itemLimitPriceModels, new ItemLimitPriceComparer()).ToList();
                                if (resultItemLimitModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.B001.ToString(),
                                        RulesName = CheckRules.B001.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.LimitItemPriceCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
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
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task ChildrenDrugCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<ChildrenDrugEntity> childrenDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        List<ChildrenDrugViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new ChildrenDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            StartAge = 0,
                            EndAge = 0,
                            CurrentAge = yBHosInfoEntity.Age,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
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
                                List<ChildrenDrugViewModel> resultChildrenDrugModels = hosPreModels.Intersect(childrenDrugViewModels, new ChildrenDrugComparer()).ToList();
                                if (resultChildrenDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C001.ToString(),
                                        RulesName = CheckRules.C001.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.ChildrenDrugCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                            CheckDate = DateTime.Now,
                                            PreCode = item.PreCode,
                                            ItemIndex = item.ItemIndex,
                                            CheckResultPreInfoCode = CommonHelper.CreateNo(),
                                            ResultDescription = "药品编码：" + item.DrugCode + ";药品名称：<a href='drugdescription.html?drugcode="+ item.DrugCode + "' target=\"_blank\" style=\"color:#FF0000;\">" + item.DrugName + "</a>;'" + item.Describe + "'"

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
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task MedicalLimitDrugCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<MedicalLimitDrugEntity> medicalLimitDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.B002.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        List<MedicalLimitDrugViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new MedicalLimitDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            ICDCode = yBHosInfoEntity.ICDCode,
                            DiseaseName = yBHosInfoEntity.DiseaseName,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
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
                                List<MedicalLimitDrugViewModel> resultMedicalLimitDrugModels = hosPreModels.Intersect(medicalLimitDrugViewModels, new MedicalLimitDrugComparer()).ToList();
                                if (resultMedicalLimitDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.B002.ToString(),
                                        RulesName = CheckRules.B002.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.MedicalLimitDrugCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName, yBHosInfoEntity.DiseaseName),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
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
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task SexDrugCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<SexDrugEntity> sexDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C002.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        List<SexDrugViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new SexDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            Sex = yBHosInfoEntity.Gender == "男性" ? "1" : "2",
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
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
                                List<SexDrugViewModel> resultSexDrugModels = hosPreModels.Intersect(sexDrugViewModels, new SexDrugComparer()).ToList();
                                if (resultSexDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C002.ToString(),
                                        RulesName = CheckRules.C002.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.SexDrugCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName, yBHosInfoEntity.Gender),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
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
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task OldDrugCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<OldDrugEntity> oldDrugKnowledges, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.C003.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把诊疗项目过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        List<OldDrugViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => array.Contains(it.CollectFeesCategoryCode)).Select(m => new OldDrugViewModel
                        {
                            DrugCode = m.ItemCode,
                            DrugName = m.ItemName,
                            StartAge = 0,
                            EndAge = 0,
                            CurrentAge = yBHosInfoEntity.Age,
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
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
                                List<OldDrugViewModel> resultOldDrugModels = hosPreModels.Intersect(oldDrugViewModels, new OldDrugComparer()).ToList();
                                if (resultOldDrugModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.C003.ToString(),
                                        RulesName = CheckRules.C003.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.OldDrugCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
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
        /// <param name="yBHosInfoEntity"></param>
        /// <param name="diseaseNoNormalinfo"></param>
        /// <returns></returns>
        private static Task SexItemCheck(YBHosInfoEntity yBHosInfoEntity, List<YBHosPreInfoEntity> yBHosPreInfoEntity, List<SexItemLimitEntity> sexItemLimitKnowledge, List<string> rulesChecks)
        {

            if (rulesChecks.Exists(it => it == CheckRules.D001.ToString()))
            {
                return Task.Run(() =>
                {
                    //处理处方信息把药品过滤掉
                    string[] array = new string[4] { SystemManageConst.XYF_CODE, SystemManageConst.ZCHENGY_CODE, SystemManageConst.ZCY_CODE, SystemManageConst.MYF_CODE };
                    if (yBHosPreInfoEntity != null && yBHosPreInfoEntity.Count > 0)
                    {
                        List<SexItemViewModel> hosPreModels = yBHosPreInfoEntity.Where(it => !array.Contains(it.CollectFeesCategoryCode)).Select(m => new SexItemViewModel
                        {
                            ItemCode = m.ItemCode,
                            ItemName = m.ItemName,
                            Sex = yBHosInfoEntity.Gender == "男性" ? "1" : "2",
                            PreCode = m.PreCode,
                            ItemIndex = m.ItemIndex,
                            RegisterCode = m.HosRegisterCode

                        }).ToList();
                        if (hosPreModels != null && hosPreModels.Count > 0)
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
                                List<SexItemViewModel> resultSexItemModels = hosPreModels.Intersect(sexItemViewModels, new SexItemComparer()).ToList();
                                if (resultSexItemModels.Count > 0)
                                {
                                    CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                    {
                                        CheckResultInfoCode = CommonHelper.CreateNo(),
                                        RulesCode = CheckRules.D001.ToString(),
                                        RulesName = CheckRules.D001.GetDescription(),
                                        RegisterCode = yBHosInfoEntity.HosRegisterCode,
                                        PersonalCode = yBHosInfoEntity.PersonalCode,
                                        IdNumber = yBHosInfoEntity.IdNumber,
                                        Name = yBHosInfoEntity.Name,
                                        Gender = yBHosInfoEntity.Gender,
                                        Age = yBHosInfoEntity.Age,
                                        InstitutionCode = yBHosInfoEntity.InstitutionCode,
                                        InstitutionName = yBHosInfoEntity.InstitutionName,
                                        ICDCode = yBHosInfoEntity.ICDCode,
                                        DiseaseName = yBHosInfoEntity.DiseaseName,
                                        DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                        CheckDate = DateTime.Now,
                                        ResultDescription = SystemManageConst.SexItemCheckDescription(yBHosInfoEntity.Name, yBHosInfoEntity.IdNumber, yBHosInfoEntity.InstitutionName, yBHosInfoEntity.Gender),
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
                                            DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
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
                QueryConditionViewModel queryConditionViewModel = new QueryConditionViewModel();
                int count = 0;//本次共计审核人次
                checkResultInfoEntities.Clear();//清空审核结果集合
                checkResultPreInfoEntities.Clear();//清空审核处方结果集合              
                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionViewModel.InstitutionCode = institutionCode;
                    List<YBHosPreInfoEntity> yBHosPreInfoEntitys = new List<YBHosPreInfoEntity>();
                    Ybhosinfo = _afterCheckService.GetYBHosInfos(queryConditionViewModel);//按条件获取住院信息
                    count += Ybhosinfo.Count;
                    var groupPersonalList = Ybhosinfo.GroupBy(it => new { it.PersonalCode })//根据人员编码分组
                       .Select(m => new
                       {
                           m.Key.PersonalCode
                       }).ToList();

                    #region 获取知识库数据(从数据库或redis中取)
                    List<DiseaseNoNormalEntity> diseaseNoNormalinfo = _afterCheckService.GetDiseaseNoNormals_Knowledge();//就诊疾病异常审核A007
                    List<RulesMainEntity> inHosDayRulesKnowledge = _afterCheckService.GetInHosDayUnusual_Knowledge();//住院天数异常知识库A003
                    RulesMainEntity disintegrateInHosKnowledge = _afterCheckService.GetDisintegrateInHos_Knowledge();//分解住院A004
                    int inHosDayValue = 7;//分解住院默认七天
                    if (disintegrateInHosKnowledge != null)
                        inHosDayValue = int.Parse(disintegrateInHosKnowledge.RulesValue);
                    List<ItemLimitPriceEntity> itemLimitPricesKnowledge = _afterCheckService.GetItemLimitedPrice_Knowledge();//获取限定诊疗价格知识库B001
                    List<ChildrenDrugEntity> childrenDrugKnowledge = _afterCheckService.GetChildrenDrugEntity_Knowledge();//获取限儿童用药知识库C001
                    List<MedicalLimitDrugEntity> medicalLimitDrugKnowledge = _afterCheckService.GetMedicalLimitedDrug_Knowledge();//获取医保目录限制用药范围知识库B002
                    List<SexDrugEntity> sexDrugKnowledge = _afterCheckService.GetSexDrug_Knowledge();//获取限定性别用药C002
                    List<OldDrugEntity> oldDrugKnowledge = _afterCheckService.GetOldDrug_Knowledge();//获取限老年人用药C003
                    List<SexItemLimitEntity> sexItemLimitKnowledge = _afterCheckService.GetSexItem_Knowledge();//获取限定性别诊疗服务D001
                    #endregion


                    foreach (YBHosInfoEntity item in Ybhosinfo)
                    {
                        yBHosPreInfoEntitys.Clear();
                        yBHosPreInfoEntitys = _afterCheckService.GetYBHosPreInfosByHosRegisterCode(item.HosRegisterCode);//获取这个人的处方信息
                        AfterCheckAlgorithm(item, diseaseNoNormalinfo, inHosDayRulesKnowledge,
                            yBHosPreInfoEntitys, itemLimitPricesKnowledge, childrenDrugKnowledge,
                            medicalLimitDrugKnowledge, sexDrugKnowledge, oldDrugKnowledge, sexItemLimitKnowledge, rulesCheck);
                    }

                    #region 分解住院审核（无法同步，单独审核）
                    if (rulesCheck.Exists(it => it == CheckRules.A004.ToString()))
                    {
                        foreach (var item in groupPersonalList)
                        {
                            List<YBHosInfoEntity> disintegrateInHos = new List<YBHosInfoEntity>();
                            //获取一个人在同一家医院所有的医保住院信息
                            QueryConditionViewModel queryConditionView = new QueryConditionViewModel() { InstitutionCode = queryConditionViewModel.InstitutionCode, PersonalCode = item.PersonalCode };
                            PersonInhosInfo = _afterCheckService.GetYBHosInfosByPersonalByIns(queryConditionView).OrderBy(it => it.InHosDate).ThenBy(it => it.OutHosDate).ToList();

                            if (PersonInhosInfo.Count > 1)
                            {
                                for (int i = PersonInhosInfo.Count - 1; i >= 0; i--)
                                {
                                    DateTime inHosdate = PersonInhosInfo[i].InHosDate;
                                    PersonInhosInfo.Remove(PersonInhosInfo[i]);
                                    for (int j = PersonInhosInfo.Count - 1; j >= 0; j--)
                                    {
                                        if (CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) >= 0 && CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) <= inHosDayValue)
                                        {
                                            if (disintegrateInHos.Where(it => it.PersonalCode == PersonInhosInfo[j].PersonalCode).ToList().Count <= 0)
                                                disintegrateInHos.Add(PersonInhosInfo[j]);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (disintegrateInHos.Count > 0)
                            {
                                CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                {
                                    CheckResultInfoCode = CommonHelper.CreateNo(),
                                    RulesCode = CheckRules.A004.ToString(),
                                    RulesName = CheckRules.A004.GetDescription(),
                                    RegisterCode = disintegrateInHos[0].HosRegisterCode,
                                    PersonalCode = disintegrateInHos[0].PersonalCode,
                                    IdNumber = disintegrateInHos[0].IdNumber,
                                    Name = disintegrateInHos[0].Name,
                                    Gender = disintegrateInHos[0].Gender,
                                    Age = disintegrateInHos[0].Age,
                                    InstitutionCode = disintegrateInHos[0].InstitutionCode,
                                    InstitutionName = disintegrateInHos[0].InstitutionName,
                                    ICDCode = disintegrateInHos[0].ICDCode,
                                    DiseaseName = disintegrateInHos[0].DiseaseName,
                                    DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                    CheckDate = DateTime.Now,
                                    ResultDescription = SystemManageConst.ResolveInHosCheckDescription(disintegrateInHos[0].Name, disintegrateInHos[0].Age, inHosDayValue, disintegrateInHos[0].InstitutionName),
                                    IsPre = ((int)CheckIsPreEnum.No).ToString()

                                };
                                checkResultInfoEntities.Add(checkResultInfoEntity);
                            }

                        }
                    }
                    #endregion

                    #region 入出院日期异常（无法同步，单独审核）
                    if (rulesCheck.Exists(it => it == CheckRules.A006.ToString()))
                    {
                        foreach (var item in groupPersonalList)
                        {
                            List<YBHosInfoEntity> inOutHos = new List<YBHosInfoEntity>();
                            PersonInhosInfo = _afterCheckService.GetYBHosInfosByPersonalCode(item.PersonalCode).OrderBy(it => it.InHosDate).ThenBy(it => it.OutHosDate).ToList();
                            if (PersonInhosInfo.Count > 1)
                            {
                                for (int i = PersonInhosInfo.Count - 1; i >= 0; i--)
                                {
                                    DateTime inHosdate = PersonInhosInfo[i].InHosDate;
                                    PersonInhosInfo.Remove(PersonInhosInfo[i]);
                                    for (int j = PersonInhosInfo.Count - 1; j >= 0; j--)
                                    {
                                        if (CommonHelper.DateDiff(PersonInhosInfo[j].OutHosDate, inHosdate) < 0)
                                        {
                                            if (inOutHos.Where(it => it.PersonalCode == PersonInhosInfo[j].PersonalCode).ToList().Count <= 0)
                                                inOutHos.Add(PersonInhosInfo[j]);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (inOutHos.Count > 0)
                            {
                                CheckResultInfoEntity checkResultInfoEntity = new CheckResultInfoEntity()
                                {
                                    CheckResultInfoCode = CommonHelper.CreateNo(),
                                    RulesCode = CheckRules.A006.ToString(),
                                    RulesName = CheckRules.A006.GetDescription(),
                                    RegisterCode = inOutHos[0].HosRegisterCode,
                                    PersonalCode = inOutHos[0].PersonalCode,
                                    IdNumber = inOutHos[0].IdNumber,
                                    Name = inOutHos[0].Name,
                                    Gender = inOutHos[0].Gender,
                                    Age = inOutHos[0].Age,
                                    InstitutionCode = inOutHos[0].InstitutionCode,
                                    InstitutionName = inOutHos[0].InstitutionName,
                                    ICDCode = inOutHos[0].ICDCode,
                                    DiseaseName = inOutHos[0].DiseaseName,
                                    DataType = ((int)CheckDataTypeEnum.Hos).ToString(),
                                    CheckDate = DateTime.Now,
                                    ResultDescription = SystemManageConst.InOutHosCheckDescription(inOutHos[0].Name, inOutHos[0].IdNumber),
                                    IsPre = ((int)CheckIsPreEnum.No).ToString()

                                };
                                checkResultInfoEntities.Add(checkResultInfoEntity);
                            }
                        }
                    }

                    #endregion
                }
                #region  将结果插入数据库
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
                List<string> ybHosRegisterCodesYes = new List<string>();//有问题的住院登记编码
                List<string> ybHosRegisterCodesNo = new List<string>();//无问题的住院登记编码
                ybhosinfoByYes.Clear();//有问题的患者对象（暂时好像多余）
                ybhosinfoByNo.Clear();//无问题的患者对象（暂时好像多余）                
                foreach (string institutionCode in institutionCodes)
                {
                    if (string.IsNullOrEmpty(institutionCode))
                        continue;
                    queryConditionViewModel.InstitutionCode = institutionCode;
                    Ybhosinfo = _afterCheckService.GetYBHosInfos(queryConditionViewModel);//按条件获取住院信息
                    List<CheckResultInfoEntity> checkResultInfoEntitiesByIns = checkResultInfoEntities.Where(it => it.InstitutionCode == institutionCode && it.DataType == "2").ToList();
                    foreach (YBHosInfoEntity item in Ybhosinfo)
                    {
                        if (checkResultInfoEntitiesByIns.Where(it => it.RegisterCode == item.HosRegisterCode).FirstOrDefault() == null)
                            ybHosRegisterCodesNo.Add(item.HosRegisterCode);
                    }

                    foreach (CheckResultInfoEntity checkResultInfoitem in checkResultInfoEntitiesByIns)
                    {
                        ybHosRegisterCodesYes.Add(checkResultInfoitem.RegisterCode);
                        if (Ybhosinfo.Where(it => it.HosRegisterCode == checkResultInfoitem.RegisterCode && it.States == ((int)CheckStates.A).ToString()).FirstOrDefault() != null)
                        {
                            List<CheckResultPreInfoEntity> checkResultPreInfoEntities_temp = new List<CheckResultPreInfoEntity>();
                            checkResultInfoEntitiesInsert.Add(checkResultInfoitem);
                            checkResultPreInfoEntities_temp = checkResultPreInfoEntities.Where(it => it.RegisterCode == checkResultInfoitem.RegisterCode && it.RulesCode == checkResultInfoitem.RulesCode && it.DataType == "2").ToList();
                            if (checkResultPreInfoEntities_temp.Count > 0)
                            {
                                checkResultPreInfoEntitiesInsert.AddRange(checkResultPreInfoEntities_temp);
                            }


                        }
                    }
                }
                //TransactionOptions transactionOption = new TransactionOptions();
                ////设置事务隔离级别
                //transactionOption.IsolationLevel = IsolationLevel.ReadCommitted;
                //// 设置事务超时时间为60秒
                //transactionOption.Timeout = new TimeSpan(0, 0, 60000);
                //using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required, transactionOption))
                //{
                    try
                    {
                        if (ybHosRegisterCodesYes.Count > 0)
                        {
                            if (!_afterCheckService.UpdateYbHosInfo(ybHosRegisterCodesYes, ((int)CheckStates.B).ToString()))
                                Error++;
                        }
                        if (ybHosRegisterCodesNo.Count > 0)
                        {
                            if (!_afterCheckService.UpdateYbHosInfo(ybHosRegisterCodesNo, ((int)CheckStates.C).ToString()))
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
                        }
                        else
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存失败";

                        }
                    }
                    catch (Exception ex)
                    {
                        //ts.Dispose();
                        resultCountModel.code = -1;
                        resultCountModel.msg = "审核结果保存失败!" + ex.Message;

                    }
                //}
                //if (tranError > 0)//事务成功提交且同步数据
                //{
                    if (ybHosRegisterCodesYes.Count > 0)//审核有问题数据
                    {
                        try
                        {
                            _afterCheckService.UpdateYBData("2", ybHosRegisterCodesYes, ((int)CheckStates.B).ToString());
                        }
                        catch
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                            return Ok(resultCountModel);
                        }
                    }
                    if (ybHosRegisterCodesNo.Count > 0)//审核无问题数据
                    {
                        try
                        {
                            _afterCheckService.UpdateYBData("2", ybHosRegisterCodesNo, ((int)CheckStates.C).ToString());
                        }
                        catch
                        {
                            resultCountModel.code = -1;
                            resultCountModel.msg = "审核结果保存成功!但医保数据状态更新失败，请联系管理员";
                            return Ok(resultCountModel);
                        }
                    }

               // }

                #endregion
                return Ok(resultCountModel);

            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "出现异常!" + ex.Message;
                return Ok(resultCountModel);

            }

        }

    }
}