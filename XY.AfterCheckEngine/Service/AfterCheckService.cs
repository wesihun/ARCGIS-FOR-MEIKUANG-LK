using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.Universal.Models;
using XY.ZnshBusiness.Entities;
using SqlSugar;
using XY.DataNS;
using XY.DataCache.Redis;
using System.Linq;
using XY.Universal.Models.ViewModels;
using XY.SystemManage.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.Utilities;

namespace XY.AfterCheckEngine.Service
{
    public class AfterCheckService : IAfterCheckService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
       
        public AfterCheckService(IXYDbContext dbContext,IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
       
        #region 获取医保住院信息
        /// <summary>
        /// 获取医保住院信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        public List<YBHosInfoEntity> GetYBHosInfos(QueryConditionViewModel queryConditionViewModel)
        {
            string[] array = new string[] { ((int)CheckStates.A).ToString(), ((int)CheckStates.B).ToString(), ((int)CheckStates.E).ToString() };
            string ybHosInfo_redisKey = SystemManageConst.YBINFO_HOS + queryConditionViewModel.InstitutionCode;//医保住院缓存Key值 （由常量Key和机构编码组成）
            var dataResult = new List<YBHosInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<YBHosInfoEntity>>(ybHosInfo_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YBHosInfoEntity>().Where(it => it.InstitutionCode == queryConditionViewModel.InstitutionCode && array.Contains(it.States)).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(ybHosInfo_redisKey, dataResult);
                        redisdb.Expire(ybHosInfo_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            #region 过滤条件
            if (dataResult != null)//条件过滤
            {
                if (!string.IsNullOrEmpty(queryConditionViewModel.CityAreaCode))//行政区划编码
                {
                    dataResult = dataResult.Where(it => it.CityAreaCode == queryConditionViewModel.CityAreaCode).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionViewModel.IdNumber))//身份证号
                {
                    dataResult = dataResult.Where(it => it.IdNumber == queryConditionViewModel.IdNumber).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionViewModel.HosRegisterCode))//住院登记编码
                {
                    dataResult = dataResult.Where(it => it.HosRegisterCode == queryConditionViewModel.HosRegisterCode).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionViewModel.PersonalTypeCode))//人员类型
                {
                    dataResult = dataResult.Where(it => it.PersonalTypeCode == queryConditionViewModel.PersonalTypeCode).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionViewModel.InstitutiongGradeCode))//机构等级
                {
                    dataResult = dataResult.Where(it => it.InstitutiongGradeCode == queryConditionViewModel.InstitutiongGradeCode).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionViewModel.InstitutionLevelCode))//机构级别
                {
                    dataResult = dataResult.Where(it => it.InstitutionLevelCode == queryConditionViewModel.InstitutionLevelCode).ToList();
                }
                if (queryConditionViewModel.InHosDate != null && queryConditionViewModel.OutHosDate != null)//入出院时间
                {
                    dataResult = dataResult.Where(it => it.InHosDate >= queryConditionViewModel.InHosDate && it.OutHosDate <= queryConditionViewModel.OutHosDate).ToList();
                }
                if (queryConditionViewModel.StartAge != null && queryConditionViewModel.EndAge != null)//入出院时间
                {
                    dataResult = dataResult.Where(it => it.Age >= queryConditionViewModel.StartAge && it.Age <= queryConditionViewModel.EndAge).ToList();
                }
                if (queryConditionViewModel.ZFY != null)//总费用
                {
                    dataResult = dataResult.Where(it => it.ZFY >= queryConditionViewModel.ZFY).ToList();
                }
            }
            #endregion
            return dataResult;
        }
        #endregion

        #region 获取一个人在同一家医院所有的医保住院信息
        /// <summary>
        /// 获取一个人在同一家医院所有的医保住院信息
        /// </summary>
        /// <param name="queryConditionViewModel"></param>
        /// <returns></returns>
        public List<YBHosInfoEntity> GetYBHosInfosByPersonalByIns(QueryConditionViewModel queryConditionViewModel)
        {
            var dataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<YBHosInfoEntity>().Where(it => it.InstitutionCode == queryConditionViewModel.InstitutionCode && it.PersonalCode == queryConditionViewModel.PersonalCode).ToList();
            }
            return dataResult;
        }
        #endregion

        #region 获取参保人所有住院信息
        public List<YBHosInfoEntity> GetYBHosInfosByPersonalCode(string personalCode)
        {
            var dataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<YBHosInfoEntity>().Where(it => it.PersonalCode == personalCode).ToList();
            }
            return dataResult;
        }
        #endregion

        #region 根据住院登记编码获取处方信息
        /// <summary>
        /// 根据住院登记编码获取处方信息
        /// </summary>
        /// <param name="hosRegisterCode"></param>
        /// <returns></returns>
        public List<YBHosPreInfoEntity> GetYBHosPreInfosByHosRegisterCode(string hosRegisterCode)
        {
            string hosRegisterCode_redisKey = hosRegisterCode;
            var dataResult = new List<YBHosPreInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<YBHosPreInfoEntity>>(hosRegisterCode_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YBHosPreInfoEntity>().Where(it => it.HosRegisterCode == hosRegisterCode).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(hosRegisterCode_redisKey, dataResult);
                        redisdb.Expire(hosRegisterCode_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        #endregion

        #region 获取医院信息
        /// <summary>
        /// 获取医院信息
        /// </summary>
        /// <returns></returns>
        public List<YyxxInfoViewModel> GetYyxxInfo()
        {
            string yyxxCode_redisKey = SystemManageConst.YYXX_KEY;
            var dataResult = new List<YyxxInfoViewModel>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<YyxxInfoViewModel>>(yyxxCode_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YYXXEntity>().Where(it => it.DeleteMark == 1).OrderBy(it=>it.YLJGBH,OrderByType.Desc)
                            .Select(xx => new YyxxInfoViewModel
                            {
                                code = xx.YYDMYYDM,
                                value = xx.YLJGMC
                            }).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(yyxxCode_redisKey, dataResult);
                        redisdb.Expire(yyxxCode_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }

        public List<YyxxInfoViewModel> GetYyxxInfoByHos()
        {
            string yyxxCode_redisKey = SystemManageConst.YYXX_KEY;
            var dataResult = new List<YyxxInfoViewModel>();
            string[] array = new string[7] { SystemManageConst.AONE, SystemManageConst.ATWO, SystemManageConst.ATHREE, SystemManageConst.BTWO, SystemManageConst.BTHREE,SystemManageConst.CTWO,SystemManageConst.CTHREE };
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                //redisdb.Del(yyxxCode_redisKey);//测试用
                dataResult = redisdb.Get<List<YyxxInfoViewModel>>(yyxxCode_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YYXXEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.YLJGBH, OrderByType.Desc)
                            .Select(xx => new YyxxInfoViewModel
                            {
                                code = xx.YYDMYYDM,
                                value = xx.YLJGMC,
                                lever = xx.YLJGDJBM
                            }).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(yyxxCode_redisKey, dataResult);
                        redisdb.Expire(yyxxCode_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            dataResult = dataResult.Where(it => array.Contains(it.lever)).ToList();
            return dataResult;
        }
        #endregion

        #region 保存审核结果时更新医保数据状态
        /// <summary>
        /// 保存审核结果时更新医保数据状态
        /// </summary>
        /// <param name="hosRegisterCodes"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateYbHosInfo(List<string> hosRegisterCodes, string status)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                YBHosInfoEntity yBHosInfoEntity = new YBHosInfoEntity();
                if (db.Updateable<YBHosInfoEntity>().UpdateColumns(it=> new YBHosInfoEntity() { States = status }).Where(it => hosRegisterCodes.Contains(it.HosRegisterCode) && it.States != ((int)CheckStates.B).ToString()).ExecuteCommand() >= 0)
                    result = true;
                else
                    result = false;

            }
            return result;
        }
        #endregion


        #region 获取知识库信息

        /// <summary>
        /// 获取就诊疾病异常审核知识库
        /// </summary>
        /// <returns></returns>
        public List<DiseaseNoNormalEntity> GetDiseaseNoNormals_Knowledge()
        {
            var dataResult = new List<DiseaseNoNormalEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<DiseaseNoNormalEntity>>(SystemManageConst.KNOWLEDGE_AGEANDDIS);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<DiseaseNoNormalEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_AGEANDDIS, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_AGEANDDIS, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
      
        /// <summary>
        /// 获取住院天数异常知识库
        /// </summary>
        /// <returns></returns>
        public List<RulesMainEntity> GetInHosDayUnusual_Knowledge()
        {
            var dataResult = new List<RulesMainEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<RulesMainEntity>().Where(it => it.DeleteMark == 1 && it.RulesCode == CheckRules.A003.ToString()).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
            }
            return dataResult;
        }
        /// <summary>
        /// 获取分解住院知识库
        /// </summary>
        /// <returns></returns>
        public RulesMainEntity GetDisintegrateInHos_Knowledge()
        {
            var dataResult = new RulesMainEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<RulesMainEntity>().Where(it => it.DeleteMark == 1 && it.RulesCode == CheckRules.A004.ToString()).OrderBy(it => it.SortCode, OrderByType.Asc).First();
            }
            return dataResult;
        }
        /// <summary>
        /// 获取限定诊疗价格知识库
        /// </summary>
        /// <returns></returns>
        public List<ItemLimitPriceEntity> GetItemLimitedPrice_Knowledge()
        {
            var dataResult = new List<ItemLimitPriceEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<ItemLimitPriceEntity>>(SystemManageConst.KNOWLEDGE_ITEMLIMITEDPRICE);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<ItemLimitPriceEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_ITEMLIMITEDPRICE, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_ITEMLIMITEDPRICE, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 获取限儿童用药知识库
        /// </summary>
        /// <returns></returns>
        public List<ChildrenDrugEntity> GetChildrenDrugEntity_Knowledge()
        {
            var dataResult = new List<ChildrenDrugEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<ChildrenDrugEntity>>(SystemManageConst.KNOWLEDGE_CHILDRENDRUG);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<ChildrenDrugEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_CHILDRENDRUG, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_CHILDRENDRUG, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 获取医保目录限制用药范围知识库
        /// </summary>
        /// <returns></returns>
        public List<MedicalLimitDrugEntity> GetMedicalLimitedDrug_Knowledge()
        {
            var dataResult = new List<MedicalLimitDrugEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<MedicalLimitDrugEntity>>(SystemManageConst.KNOWLEDGE_MEDICALLIMITEDRUG);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<MedicalLimitDrugEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_MEDICALLIMITEDRUG, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_MEDICALLIMITEDRUG, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 获取限定性别用药知识库
        /// </summary>
        /// <returns></returns>
        public List<SexDrugEntity> GetSexDrug_Knowledge()
        {
            var dataResult = new List<SexDrugEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<SexDrugEntity>>(SystemManageConst.KNOWLEDGE_SEXDRUG);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<SexDrugEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_SEXDRUG, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_SEXDRUG, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 获取老年人合理用药知识库
        /// </summary>
        /// <returns></returns>
        public List<OldDrugEntity> GetOldDrug_Knowledge()
        {
            var dataResult = new List<OldDrugEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<OldDrugEntity>>(SystemManageConst.KNOWLEDGE_OLDDRUG);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<OldDrugEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_OLDDRUG, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_OLDDRUG, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 获取限定性别诊疗服务知识库
        /// </summary>
        /// <returns></returns>
        public List<SexItemLimitEntity> GetSexItem_Knowledge()
        {
            var dataResult = new List<SexItemLimitEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<SexItemLimitEntity>>(SystemManageConst.KNOWLEDGE_SEXITEM);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<SexItemLimitEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode, OrderByType.Asc).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(SystemManageConst.KNOWLEDGE_SEXITEM, dataResult);
                        redisdb.Expire(SystemManageConst.KNOWLEDGE_SEXITEM, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }




        #endregion

        #region 审核时更新是否审核完成状态
        /// <summary>
        /// 审核时更新是否审核完成状态
        /// </summary>
        /// <param name="checkResultStatus"></param>
        /// <returns></returns>
        public bool UpdateResultStatus(CheckResultStatusEntity checkResultStatus)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(checkResultStatus).Where(it=> it.CRowId == checkResultStatus.CRowId).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        #endregion

        #region 获取是否审核完成状态
        /// <summary>
        /// 获取是否审核完成状态
        /// </summary>
        /// <returns></returns>
        public CheckResultStatusEntity GetCheckResultStatus()
        {
            var dataResult = new CheckResultStatusEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckResultStatusEntity>().First();
            }
            return dataResult;
        }
        #endregion

        #region 从redis里获取审核结果
        /// <summary>
        /// 从redis里获取审核结果
        /// </summary>
        /// <param name="checkNumber"></param>
        /// <returns></returns>
        public List<CheckResultInfoEntity> GetCheckResultInfoListFromRedis(string checkNumber)
        {
            var dataResult = new List<CheckResultInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<CheckResultInfoEntity>>(checkNumber);//从缓存里取
            }
            return dataResult;
        }
        #endregion

        #region 从redis里获取审核结果处方信息
        /// <summary>
        /// 从redis里获取审核结果处方信息
        /// </summary>
        /// <param name="checkPreNumber"></param>
        /// <returns></returns>
        public List<CheckResultPreInfoEntity> GetCheckResultPreInfoListFromRedis(string checkPreNumber)
        {
            var dataResult = new List<CheckResultPreInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<CheckResultPreInfoEntity>>(checkPreNumber);//从缓存里取
            }
            return dataResult;
        }
        #endregion

        #region 插入审核结果信息
        /// <summary>
        /// 插入审核结果信息
        /// </summary>
        /// <param name="checkResultInfoEntities"></param>
        /// <returns></returns>
        public bool InsertResultInfo(List<CheckResultInfoEntity> checkResultInfoEntities)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                if (db.Insertable(checkResultInfoEntities.ToArray()).ExecuteCommand() > 0)
                    result = true;
                else
                    result = false;
            }
            return result;
        }
        #endregion

        #region 插入审核结果处方信息
        /// <summary>
        /// 插入审核结果处方信息
        /// </summary>
        /// <param name="checkResultPreInfoEntities"></param>
        /// <returns></returns>
        public bool InsertResultPreInfo(List<CheckResultPreInfoEntity> checkResultPreInfoEntities)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                if (db.Insertable(checkResultPreInfoEntities.ToArray()).ExecuteCommand() > 0)
                    result = true;
                else
                    result = false;
            }
            return result;
        }
        #endregion

        #region 更新是否保存完审核结果状态
        /// <summary>
        /// 更新是否保存完审核结果状态
        /// </summary>
        /// <param name="checkResultStatusSaveEntity"></param>
        /// <returns></returns>
        public bool UpdateSaveResultStatus(CheckResultStatusSaveEntity checkResultStatusSaveEntity)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(checkResultStatusSaveEntity).Where(it => it.CRowId == checkResultStatusSaveEntity.CRowId).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        #endregion

        #region 获取是否保存完审核结果状态
        /// <summary>
        /// 获取是否保存完审核结果状态
        /// </summary>
        /// <returns></returns>
        public CheckResultStatusSaveEntity GetSaveCheckResultStatus()
        {
            var dataResult = new CheckResultStatusSaveEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckResultStatusSaveEntity>().First();
            }
            return dataResult;
        }





        #endregion


        #region 获取医保门诊信息
        public List<YBClinicInfoEntity> GetYBClinicInfos(QueryConditionByClinic queryConditionByClinic)
        {
            string[] array = new string[] { ((int)CheckStates.A).ToString(), ((int)CheckStates.B).ToString(), ((int)CheckStates.E).ToString() };
            string ybClinicInfo_redisKey = SystemManageConst.YBINFO_CLINIC + queryConditionByClinic.InstitutionCode;//医保门诊缓存Key值 （由常量Key和机构编码组成）
            var dataResult = new List<YBClinicInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<YBClinicInfoEntity>>(ybClinicInfo_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YBClinicInfoEntity>().Where(it => it.InstitutionCode == queryConditionByClinic.InstitutionCode && array.Contains(it.States)).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(ybClinicInfo_redisKey, dataResult);
                        redisdb.Expire(ybClinicInfo_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            #region 过滤条件
            if (dataResult != null)//条件过滤
            {                
                if (!string.IsNullOrEmpty(queryConditionByClinic.IdNumber))//身份证号
                {
                    dataResult = dataResult.Where(it => it.IdNumber == queryConditionByClinic.IdNumber).ToList();
                }
                if (!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode))//就诊机构
                {
                    dataResult = dataResult.Where(it => it.InstitutionCode== queryConditionByClinic.InstitutionCode).ToList();
                }               
            }
            #endregion
            return dataResult;
        }
        #endregion

        #region 获取是否审核完成状态
        /// <summary>
        /// 获取是否审核完成状态--门诊
        /// </summary>
        /// <returns></returns>
        public CheckResultStatusEntity GetCheckResultStatusByClinic()
        {
            var dataResult = new CheckResultStatusEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckResultStatusEntity>().Where(it=>it.CRowId==2).First();
            }
            return dataResult;
        }
        #endregion

        #region 获取是否保存完审核结果状态--门诊
        /// <summary>
        /// 获取是否保存完审核结果状态--门诊
        /// </summary>
        /// <returns></returns>
        public CheckResultStatusSaveEntity GetSaveCheckResultStatusByClinic()
        {
            var dataResult = new CheckResultStatusSaveEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckResultStatusSaveEntity>().Where(it=>it.CRowId==2).First();
            }
            return dataResult;
        }
        #endregion

        #region 根据门诊登记编码获取门诊处方
        /// <summary>
        /// 根据门诊登记编码获取门诊处方
        /// </summary>
        /// <param name="clinicRegisterCode"></param>
        /// <returns></returns>
        public List<YBClinicPreInfoEntity> GetYBClinicPreInfosByClinicRegisterCode(string clinicRegisterCode)
        {
            string clinicRegisterCode_redisKey = clinicRegisterCode;
            var dataResult = new List<YBClinicPreInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<YBClinicPreInfoEntity>>(clinicRegisterCode_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        dataResult = db.Queryable<YBClinicPreInfoEntity>().Where(it => it.ClinicRegisterCode == clinicRegisterCode).ToList();
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(clinicRegisterCode_redisKey, dataResult);
                        redisdb.Expire(clinicRegisterCode_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        #endregion
        #region 更新医保审核结果状态
        public bool UpdateYBData(string flag, List<string> registerCodes, string status)
        {
            bool result = false;
            using (var db = _dbContext.GetIntanceForYb())
            {
                if (flag == "1")//门诊
                {
                    if (db.Updateable<CompClinicCostMainEntity>().UpdateColumns(it => new CompClinicCostMainEntity() { CheckStates = status }).Where(it => registerCodes.Contains(it.CCM_ClinicRegisterCode_Vc) && (it.CheckStates==null || it.CheckStates != ((int)CheckStates.B).ToString())).ExecuteCommand() >= 0)
                        result = true;
                    else
                        result = false;
                }
                else//住院
                {
                    if (db.Updateable<CompHosCostMainEntity>().UpdateColumns(it => new CompHosCostMainEntity() { CheckStates = status }).Where(it => registerCodes.Contains(it.HCM_HosRegisterCode_Vc) && (it.CheckStates == null || it.CheckStates != ((int)CheckStates.B).ToString())).ExecuteCommand() >= 0)
                        result = true;
                    else
                        result = false;
                }
            }
            return result;
        }
        #endregion

        #region 保存审核结果时更新医保数据状态--门诊
        /// <summary>
        /// 保存审核结果时更新医保数据状态
        /// </summary>
        /// <param name="clinicRegisterCodes"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateYbClinicInfo(List<string> clinicRegisterCodes, string status)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                YBClinicInfoEntity yBClinicInfoEntity = new YBClinicInfoEntity();
                if (db.Updateable<YBClinicInfoEntity>().UpdateColumns(it => new YBClinicInfoEntity() { States = status }).Where(it => clinicRegisterCodes.Contains(it.ClinicRegisterCode) && it.States != ((int) CheckStates.B).ToString()).ExecuteCommand() >= 0)
                    result = true;
                else
                    result = false;

            }
            return result;
        }
        #endregion

        public List<TreeModule> GetWGTree(string registerCode,int treeType)
        {
            var data = new List<TreeModule>();
            using (var db = _dbContext.GetIntance())
            {
                switch (treeType)
                {
                    case (int)ComplaintTreeType.Type1:
                        data = db.Queryable<CheckResultInfoEntity>()
                       .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                       .GroupBy(it => it.RulesCode)
                        .Select(it => new TreeModule
                        {
                            ID = SqlFunc.AggregateMax(it.CheckResultInfoCode),
                            PID = "100",
                            NAME = SqlFunc.AggregateMax(it.RulesName),
                            RuleCode = SqlFunc.AggregateMax(it.RulesCode),
                            States = "",
                            CheckResultInfoCode = SqlFunc.AggregateMax(it.CheckResultInfoCode)
                        }).ToList();
                        break;
                    case (int)ComplaintTreeType.Type2:  //States为8 变灰色  
                        data = db.Queryable<CheckComplaintEntity>()
                       .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                       .GroupBy(it => it.RulesCode)
                        .Select(it => new TreeModule
                        {
                            ID = SqlFunc.AggregateMax(it.CheckResultInfoCode),
                            PID = "100",
                            NAME = SqlFunc.AggregateMax(it.RulesName),
                            RuleCode = SqlFunc.AggregateMax(it.RulesCode),
                            States = SqlFunc.AggregateMax(it.ComplaintResultStatus),
                            CheckResultInfoCode = SqlFunc.AggregateMax(it.CheckResultInfoCode)
                        }).ToList();
                        break;
                    case (int)ComplaintTreeType.Type3:  //States为8 9 变灰色 
                        data = db.Queryable<CheckComplaintEntity>()
                        .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                        .GroupBy(it => it.RulesCode)
                        .Select(it => new TreeModule
                        {
                            ID = SqlFunc.AggregateMax(it.CheckResultInfoCode),
                            PID = "100",
                            NAME = SqlFunc.AggregateMax(it.RulesName),
                            RuleCode = SqlFunc.AggregateMax(it.RulesCode),
                            States = SqlFunc.AggregateMax(it.ComplaintResultStatus),
                            CheckResultInfoCode = SqlFunc.AggregateMax(it.CheckResultInfoCode)
                        }).ToList();
                        break;
                    case (int)ComplaintTreeType.Type4:   //States为8 9 4变灰色  
                        data = db.Queryable<CheckComplaintEntity>()
                         .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                        .GroupBy(it => it.RulesCode)
                        .Select(it => new TreeModule
                        {
                            ID = SqlFunc.AggregateMax(it.CheckResultInfoCode),
                            PID = "100",
                            NAME = SqlFunc.AggregateMax(it.RulesName),
                            RuleCode = SqlFunc.AggregateMax(it.RulesCode),
                            States = SqlFunc.AggregateMax(it.ComplaintResultStatus),
                            CheckResultInfoCode = SqlFunc.AggregateMax(it.CheckResultInfoCode)
                        }).ToList();
                        break;
                    default:
                        break;
                }
               
            }
            return data;
        }

        /// <summary>
        /// 获取左侧树查询页面使用
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        public List<TreeModule> GetWGTreeBySearch(string registerCode)
        {
            var data = new List<TreeModule>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<CheckResultInfoEntity>()
                       .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                       .GroupBy(it => it.RulesCode)
                        .Select(it => new TreeModule
                        {
                            ID = SqlFunc.AggregateMax(it.CheckResultInfoCode),
                            PID = "100",
                            NAME = SqlFunc.AggregateMax(it.RulesName),
                            RuleCode = SqlFunc.AggregateMax(it.RulesCode),
                            States = "",
                            CheckResultInfoCode = SqlFunc.AggregateMax(it.CheckResultInfoCode)
                        }).ToList();
            }
            return data;
        }

        public WGEntity GetWGMoney(string registerCode)
        {
            var data = new WGEntity();
            using (var db = _dbContext.GetIntance())
            {
                var list = db.Queryable<CheckResultInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode).ToList();
                var grouplist = db.Queryable<CheckResultInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                    .GroupBy(it => it.RegisterCode)
                    .GroupBy(it => it.RulesCode)
                    .Select(it => new CheckResultInfoEntity
                    {
                        IsPre = SqlFunc.AggregateMax(it.IsPre),
                        MONEY = SqlFunc.AggregateMax(it.MONEY),
                        RulesName = SqlFunc.AggregateMax(it.RulesName),
                        RulesCode = SqlFunc.AggregateMax(it.RulesCode)
                    }).ToList();
                data.WGDescription = "经系统审核，该条住院信息违反";
                int count = 1;
                if(list.Any(it => it.IsPre == "0"))
                {
                    data.Money = list.Where(it => it.IsPre == "0").First().MONEY;
                }
                else
                {
                    data.Money = list.Sum(it => it.MONEY);
                }
                foreach (var obj in grouplist)
                {
                    if (obj.IsPre == "1")
                        obj.MONEY = list.Where(it => it.RulesCode == obj.RulesCode).Sum(it => it.MONEY);
                    data.WGDescription += count + ". \"" + obj.RulesName + "\" 规则,涉及违规金额" + obj.MONEY + "元;";
                    count++;
                }
           
            }
            return data;
        }
        /// <summary>
        /// 根据条件获取审核结果列表
        /// </summary>
        /// <param name="queryCoditionByCheckResult"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="totalcount"></param>
        /// <returns></returns>
        public List<CheckResultInfoEntity> GetCheckResultInfos(string flag,string rulescode,QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {               
                var Check_ComplaintMainlist = db.Queryable<CheckComplaintEntity>().Where(it => it.ComplaintResultStatus != ((int)CheckStates.D).ToString()).Select(it => it.CheckResultInfoCode).ToList();
                if (isadmin)
                {
                    DataResult = db.Queryable<CheckResultInfoEntity>()
                            .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                            .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                            .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                            .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                            .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), it => it.PersonalCode == queryCoditionByCheckResult.PersonalCode)
                            .WhereIF(!string.IsNullOrEmpty(rulescode),it => it.RulesCode == rulescode)
                            .WhereIF(queryCoditionByCheckResult.StartSettleTime != null,it=>it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                            .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                            .Where(it => !Check_ComplaintMainlist.Contains(it.CheckResultInfoCode))
                            .Where(it => it.DataType == flag)                         
                            .GroupBy(it => it.RegisterCode)
                            .OrderBy(it => SqlFunc.AggregateMax(it.SettlementDate), OrderByType.Desc)
                            .Select(it => new CheckResultInfoEntity
                            {
                                Name = SqlFunc.AggregateMax(it.Name), 
                                IdNumber = SqlFunc.AggregateMax(it.IdNumber),
                                Gender = SqlFunc.AggregateMax(it.Gender),
                                Age = SqlFunc.AggregateMax(it.Age),
                                DiseaseName = SqlFunc.AggregateMax(it.DiseaseName),
                                InstitutionName = SqlFunc.AggregateMax(it.InstitutionName),
                                PersonalCode = SqlFunc.AggregateMax(it.PersonalCode),
                                RegisterCode = SqlFunc.AggregateMax(it.RegisterCode),
                                InstitutionCode = SqlFunc.AggregateMax(it.InstitutionCode),
                                SettlementDate = SqlFunc.AggregateMax(it.SettlementDate),
                                Count = SqlFunc.AggregateCount(it.RegisterCode)
                            })
                           .ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    DataResult = db.Queryable<CheckResultInfoEntity>().WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                          .Where(it => !Check_ComplaintMainlist.Contains(it.CheckResultInfoCode))
                           .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), it => it.PersonalCode == queryCoditionByCheckResult.PersonalCode)
                           .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                            .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                          .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                          .Where(it => it.DataType == flag)
                          .Where(it => it.InstitutionCode == curryydm)
                          .GroupBy(it => it.RegisterCode)
                          .OrderBy(it => SqlFunc.AggregateMax(it.SettlementDate), OrderByType.Desc)
                            .Select(it => new CheckResultInfoEntity
                            {
                                Name = SqlFunc.AggregateMax(it.Name),
                                IdNumber = SqlFunc.AggregateMax(it.IdNumber),
                                Gender = SqlFunc.AggregateMax(it.Gender),
                                Age = SqlFunc.AggregateMax(it.Age),
                                DiseaseName = SqlFunc.AggregateMax(it.DiseaseName),
                                InstitutionName = SqlFunc.AggregateMax(it.InstitutionName),
                                PersonalCode = SqlFunc.AggregateMax(it.PersonalCode),
                                RegisterCode = SqlFunc.AggregateMax(it.RegisterCode),
                                SettlementDate = SqlFunc.AggregateMax(it.SettlementDate),
                                InstitutionCode = SqlFunc.AggregateMax(it.InstitutionCode),
                                Count = SqlFunc.AggregateCount(it.RegisterCode)
                            })
                          .ToPageList(page, limit, ref totalcount);
                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    DataResult = db.Queryable<CheckResultInfoEntity>().WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.PersonalCode == queryCoditionByCheckResult.ICDCode)
                          .Where(it => !Check_ComplaintMainlist.Contains(it.CheckResultInfoCode))
                          .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                          .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                          .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), it => it.ICDCode == queryCoditionByCheckResult.PersonalCode)
                          .Where(it => it.DataType == flag)
                          .WhereIF(!string.IsNullOrEmpty(XAreaCode), it => it.InstitutionCode.Contains(XAreaCode.Substring(0, 6)))
                          .GroupBy(it => it.RegisterCode)
                          .OrderBy(it => SqlFunc.AggregateMax(it.SettlementDate), OrderByType.Desc)
                          .Select(it => new CheckResultInfoEntity
                            {
                              Name = SqlFunc.AggregateMax(it.Name),
                              IdNumber = SqlFunc.AggregateMax(it.IdNumber),
                              Gender = SqlFunc.AggregateMax(it.Gender),
                              Age = SqlFunc.AggregateMax(it.Age),
                              DiseaseName = SqlFunc.AggregateMax(it.DiseaseName),
                              InstitutionName = SqlFunc.AggregateMax(it.InstitutionName),
                              PersonalCode = SqlFunc.AggregateMax(it.PersonalCode),
                              RegisterCode = SqlFunc.AggregateMax(it.RegisterCode),
                              SettlementDate = SqlFunc.AggregateMax(it.SettlementDate),
                              InstitutionCode = SqlFunc.AggregateMax(it.InstitutionCode),
                              Count = SqlFunc.AggregateCount(it.RegisterCode)
                          })
                          .ToPageList(page, limit, ref totalcount);
                }                            
            }
            return DataResult;
        }

        public List<string> GetComplainRulesName(string registerCode, string type)
        {
            List<string> rulesList = new List<string>();
            using (var db = _dbContext.GetIntance())
            {
                if(type == "1")
                   rulesList = db.Queryable<CheckComplaintEntity>().Where(it => it.RegisterCode == registerCode && it.ComplaintResultStatus != CheckStates.H.ToString()).GroupBy(it => it.RulesName).Select(it => it.RulesName).ToList();
                if(type == "2")
                    rulesList = db.Queryable<CheckComplaintEntity>().Where(it => it.RegisterCode == registerCode).Where(it => it.ComplaintResultStatus == CheckStates.E.ToString() || it.ComplaintResultStatus == CheckStates.K.ToString()).GroupBy(it => it.RulesName).Select(it => it.RulesName).ToList();
            }
            return rulesList;
         }

        public List<CheckResultInfoEntity> GetListByFK(string states,string rulescode, QueryCoditionByCheckResult result, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {                 
                if (isadmin)    //为管理员
                {
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0",(a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString() || a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString())
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(result.IdNumber), (a, b) => b.IdNumber == result.IdNumber)
                    .WhereIF(!string.IsNullOrEmpty(result.ICDCode), (a, b) => b.ICDCode == result.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(result.InstitutionCode), (a, b) => b.InstitutionCode == result.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(result.Name), (a, b) => b.Name == result.Name)
                    .WhereIF(!string.IsNullOrEmpty(result.PersonalCode), (a, b) => b.PersonalCode == result.PersonalCode)
                    .WhereIF(result.StartSettleTime != null, (a, b) => b.SettlementDate >= result.StartSettleTime)
                    .WhereIF(result.EndSettleTime != null, (a, b) => b.SettlementDate <= result.EndSettleTime)
                    .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b)  => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    }).ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0", (a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString() || a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString())
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(result.IdNumber), (a, b) => b.IdNumber == result.IdNumber)
                    .WhereIF(!string.IsNullOrEmpty(result.ICDCode), (a, b) => b.ICDCode == result.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(result.InstitutionCode), (a, b) => b.InstitutionCode == result.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(result.Name), (a, b) => b.Name == result.Name)
                    .WhereIF(!string.IsNullOrEmpty(result.PersonalCode), (a, b) => b.PersonalCode == result.PersonalCode)
                    .WhereIF(result.StartSettleTime != null, (a, b) => b.SettlementDate >= result.StartSettleTime)
                    .WhereIF(result.EndSettleTime != null, (a, b) => b.SettlementDate <= result.EndSettleTime)
                    .Where((a, b) => b.InstitutionCode == curryydm)
                    .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    }).ToPageList(page, limit, ref totalcount);
                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0, 6);
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0", (a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString() || a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.J).ToString())
                   .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                   .WhereIF(!string.IsNullOrEmpty(result.IdNumber), (a, b) => b.IdNumber == result.IdNumber)
                   .WhereIF(!string.IsNullOrEmpty(result.ICDCode), (a, b) => b.ICDCode == result.ICDCode)
                   .WhereIF(!string.IsNullOrEmpty(result.InstitutionCode), (a, b) => b.InstitutionCode == result.InstitutionCode)
                   .WhereIF(!string.IsNullOrEmpty(result.Name), (a, b) => b.Name == result.Name)
                   .WhereIF(!string.IsNullOrEmpty(result.PersonalCode), (a, b) => b.PersonalCode == result.PersonalCode)
                   .WhereIF(result.StartSettleTime != null, (a, b) => b.SettlementDate >= result.StartSettleTime)
                    .WhereIF(result.EndSettleTime != null, (a, b) => b.SettlementDate <= result.EndSettleTime)
                   .Where((a, b) => b.InstitutionCode.StartsWith(XAreaCode))
                   .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    }).ToPageList(page, limit, ref totalcount);
                }
            }
            return DataResult;
        }
        public bool IsCityYBJ(string OrganizeId)
        {
            if(OrganizeId.Substring(0,2) == "15")   //为医院
            {
                return false;
            }
            else
            {
                using (var db = _dbContext.GetIntance())
                {
                    var entity = db.Queryable<OrganizeEntity>().Where(it => it.DeleteMark == 1 && it.OrganizeId == OrganizeId).First();
                    if(entity.OrgLevel == "4")   //市级
                    {
                        return true;
                    }
                    else                     //县级
                    {
                        return false;
                    }
                }
            }          
        }
        /// <summary>
        /// 返回旗县医保局所属区划代码
        /// </summary>
        /// <param name="OrganizeId"></param>
        /// <returns></returns>
        public string returnXAreaCode(string OrganizeId)
        {
            using (var db = _dbContext.GetIntance())
            {
                var entity = db.Queryable<OrganizeEntity>().Where(it => it.DeleteMark == 1 && it.OrganizeId == OrganizeId).First();
                return entity.XAreaCode;
            }
        }
        public List<CheckResultInfoEntity> GetListByConclusion(string rulescode, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm,string states, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (isadmin)
                {
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString() || a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString())
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.PersonalCode == queryCoditionByCheckResult.PersonalCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    })
                    .ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString() || a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString())
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.PersonalCode == queryCoditionByCheckResult.PersonalCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Where((a, b) => b.InstitutionCode == curryydm)
                    .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    })
                    .ToPageList(page, limit, ref totalcount);
                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0,6);
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(states == "0", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString() || a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "1", (a, b) => a.ComplaintStatus == ((int)CheckStates.K).ToString())
                    .WhereIF(states == "2", (a, b) => a.ComplaintStatus == ((int)CheckStates.E).ToString())
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => b.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.PersonalCode == queryCoditionByCheckResult.PersonalCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Where((a, b) => b.InstitutionCode.StartsWith(XAreaCode))
                    .GroupBy((a, b) => b.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(b.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        ProposalMoney = SqlFunc.AggregateMax(a.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(a.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(a.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(a.ProposalDescribe),
                        States = SqlFunc.AggregateMax(a.ComplaintStatus),
                        FirstTrialDescribe = SqlFunc.AggregateMax(a.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(a.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(a.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(a.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(a.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(b.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(b.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(b.Name),
                        IdNumber = SqlFunc.AggregateMax(b.IdNumber),
                        Gender = SqlFunc.AggregateMax(b.Gender),
                        Age = SqlFunc.AggregateMax(b.Age),
                        InstitutionName = SqlFunc.AggregateMax(b.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(b.DiseaseName),
                        Count = SqlFunc.AggregateCount(b.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(b.SettlementDate)
                    })
                    .ToPageList(page, limit, ref totalcount);
                }

            }
            return DataResult;
        }
        /// <summary>
        /// 根据住院编码，个人编码获取门诊登记信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        public YBClinicInfoEntity GetYBClinicInfoEntity(string registerCode, string personalCode)
        {
            var dataResult = new YBClinicInfoEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicRegisterCode == registerCode && it.PersonalCode == personalCode).ToList().SingleOrDefault();
            }
            return dataResult;
        }
        /// <summary>
        /// 根据住院登记编码、个人编码获取住院信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        public YBHosInfoEntity GetYBHosInfoEntity(string registerCode, string personalCode)
        {
            var dataResult = new YBHosInfoEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<YBHosInfoEntity>().Where(it => it.HosRegisterCode == registerCode && it.PersonalCode == personalCode).ToList().SingleOrDefault();
            }
            return dataResult;
        }

        public List<CheckComplaintEntity> GetComplaintStatesList(string rulescode, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var list = new List<CheckComplaintEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (isadmin)
                {
                    list = db.Queryable<CheckComplaintEntity>()
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(rulescode),it => it.RulesCode == rulescode)
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.ComplaintResultStatus == states)
                        .ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    list = db.Queryable<CheckComplaintEntity>()
                         .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                        .Where(it => it.InstitutionCode == curryydm)
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.ComplaintResultStatus == states)
                        .ToPageList(page, limit, ref totalcount);
                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    list = db.Queryable<CheckComplaintEntity>()
                         .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                        .Where(it => it.InstitutionCode.Contains(XAreaCode.Substring(0, 6)))
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.ComplaintResultStatus == states)
                        .ToPageList(page, limit, ref totalcount);
                }
                
            }
            return list;
        }
        public List<CheckResultInfoEntity> GetComplaintStatesListTJ(string rulescode, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {            
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (states=="0")
                {
                    states = null;
                }
                if (isadmin)
                {
                    DataResult = db.Queryable<CheckResultInfoEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    })
                    .WhereIF(states == "-1", (a, b) => b.ComplaintStatus == null)
                    .WhereIF(!string.IsNullOrEmpty(states) && states != "-1", (a, b) => b.ComplaintStatus == states)
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => a.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .GroupBy((a, b) => a.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {                        
                        States = SqlFunc.AggregateMax(b.ComplaintStatus),                        
                        InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                        Name = SqlFunc.AggregateMax(a.Name),
                        IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                        Gender = SqlFunc.AggregateMax(a.Gender),
                        Age = SqlFunc.AggregateMax(a.Age),
                        InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                        Count = SqlFunc.AggregateCount(a.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate)
                    })
                    .ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    DataResult = db.Queryable<CheckResultInfoEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(!string.IsNullOrEmpty(states) && states != "-1", (a, b) => b.ComplaintStatus == states)
                    .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => a.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Where(a=>a.InstitutionCode==curryydm)
                    .GroupBy((a, b) => a.RegisterCode)
                    .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        States = SqlFunc.AggregateMax(b.ComplaintStatus),
                        InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                        PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                        RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                        Name = SqlFunc.AggregateMax(a.Name),
                        IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                        Gender = SqlFunc.AggregateMax(a.Gender),
                        Age = SqlFunc.AggregateMax(a.Age),
                        InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                        Count = SqlFunc.AggregateCount(a.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate)
                    })
                    .ToPageList(page, limit, ref totalcount);
                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0, 6);
                    DataResult = db.Queryable<CheckResultInfoEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).WhereIF(!string.IsNullOrEmpty(states) && states != "-1", (a, b) => b.ComplaintStatus == states)
                     .WhereIF(!string.IsNullOrEmpty(rulescode), (a, b) => a.RulesCode == rulescode)
                     .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                     .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name.Contains(queryCoditionByCheckResult.Name))
                     .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                     .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                     .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                     .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                     .Where(a => a.InstitutionCode.StartsWith(XAreaCode))
                     .GroupBy((a, b) => a.RegisterCode)
                     .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                     .Select((a, b) => new CheckResultInfoEntity
                     {
                         States = SqlFunc.AggregateMax(b.ComplaintStatus),
                         InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                         PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                         RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                         Name = SqlFunc.AggregateMax(a.Name),
                         IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                         Gender = SqlFunc.AggregateMax(a.Gender),
                         Age = SqlFunc.AggregateMax(a.Age),
                         InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                         DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                         Count = SqlFunc.AggregateCount(a.RegisterCode),
                         SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate)
                     })
                     .ToPageList(page, limit, ref totalcount);
                }
            }
            return DataResult;
        }
        /// <summary>
        /// 根据审核编码获取申诉主表的记录
        /// </summary>
        /// <param name="CheckResultInfoCode"></param>
        /// <returns></returns>
        public CheckComplaintEntity GetCheckComplaintEntity(string CheckResultInfoCode)
        {
            var dataResult = new CheckComplaintEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<CheckComplaintEntity>().Where(it => it.ComplaintCode == CheckResultInfoCode).ToList().SingleOrDefault();
            }
           return dataResult;
        }       
        public bool IsExistAdd(string checkResultInfoCode)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(checkResultInfoCode))
            {
                return flag;
            }
            using (var db = _dbContext.GetIntance())
            {
                flag = db.Queryable<CheckComplaintEntity>().Any(it => it.CheckResultInfoCode == checkResultInfoCode);
               
            }
            return flag;
        }

        public string ReturnCheckComplainId(string registerCode)
        {
            string CheckComplainId = string.Empty;
            using (var db = _dbContext.GetIntance())
            {
                CheckComplainId = db.Queryable<Check_ComplainEntity>().Where(it => it.RegisterCode == registerCode).First().CheckComplainId;
                return CheckComplainId;
            }
        }

        public string UpdateFK(string registerCode, string describe,UserInfo userInfo)
        {
            string CheckComplainId = string.Empty;
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var updatelist = db.Queryable<CheckComplaintEntity>().Where(it => it.RegisterCode == registerCode && it.ComplaintResultStatus != ((int)CheckStates.H).ToString()).ToList();
                    foreach(var obj in updatelist)
                    {
                       obj.ComplaintResultStatus = ((int)CheckStates.G).ToString();
                       db.Updateable(obj).Where(it => it.ComplaintCode == obj.ComplaintCode).ExecuteCommand();
                    }
                    if (db.Updateable<Check_ComplainEntity>().UpdateColumns(it => new Check_ComplainEntity()
                    {
                        FeedbackCount = 1,
                        ComplaintTime = DateTime.Now,
                        ComplaintUserId = userInfo.UserId,
                        ComplaintUserName = userInfo.UserName,
                        ComplaintInstitutionCode = userInfo.InstitutionCode,
                        ComplaintInstitutionName = userInfo.InstitutionName,
                        ComplaintStatus = ((int)CheckStates.G).ToString(),
                        ComplaintDescribe = describe
                    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand() > 0)
                    {
                        CheckComplainId = db.Queryable<Check_ComplainEntity>().Where(it => it.RegisterCode == registerCode).First().CheckComplainId;
                        return CheckComplainId;
                    }                 
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return CheckComplainId;
                }
                return CheckComplainId;
            }
        }
        public string UpdateFeedbackCount(string registerCode, string describe,UserInfo userInfo)
        {
            string states = string.Empty;
            string CheckComplainId = string.Empty;
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var getByPrimaryKey = db.Queryable<Check_ComplainEntity>().Where(it => it.RegisterCode == registerCode).First();
                    if (db.Updateable<Check_ComplainEntity>().UpdateColumns(it => new Check_ComplainEntity()
                    {
                        FeedbackCount = getByPrimaryKey.FeedbackCount + 1,
                        ComplaintTime = DateTime.Now,
                        ComplaintUserId = userInfo.UserId,
                        ComplaintUserName = userInfo.UserName,
                        ComplaintInstitutionCode = userInfo.InstitutionCode,
                        ComplaintInstitutionName = userInfo.InstitutionName,
                        ComplaintStatus = ((int)CheckStates.G).ToString(),
                        ComplaintDescribe = describe
                    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand() > 0)
                    {
                        CheckComplainId = db.Queryable<Check_ComplainEntity>().Where(it => it.RegisterCode == registerCode).First().CheckComplainId;
                        return CheckComplainId;
                    }
                    db.Ado.CommitTran();
                }
                 catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return CheckComplainId;
                }
            }              
            return CheckComplainId;
        }
        public bool DeleteFiles(string ComplaintCode)
        {
            using (var db = _dbContext.GetIntance())
            {
                if (db.Deleteable<CheckComplaintDetailEntity>().Where(it => it.CheckComplainId == ComplaintCode).ExecuteCommand() > 0)
                    return true;
                else
                    return false;
            }
                
        }
        public int? GetFeedbackCount(string registerCode)
        {
            using (var db = _dbContext.GetIntance())
            {
                var list = db.Queryable<Check_ComplainEntity>().Where(it => it.RegisterCode == registerCode).First();
                if(list != null)
                {
                    return list.FeedbackCount;
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<RulesMainEntity> GetRulesMainEntities()
        {           
            using (var db = _dbContext.GetIntance())
            {
                return db.Queryable<RulesMainEntity>().ToList();

            }
         
        }


        /// <summary>
        /// 获取审核进度列表
        /// </summary>
        /// <param name="queryCoditionByCheckResult"></param>
        /// <param name="rulecode"></param>
        /// <param name="states"></param>
        /// <param name="isAdmin"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CheckResultInfoEntity> GetComplaintStateList(QueryCoditionByCheckResult queryCoditionByCheckResult, string rulecode, string states, bool isAdmin, string curryydm, int page, int limit, ref int count)
        {
            List<CheckResultInfoEntity> resultList = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (isAdmin)
                {
                    resultList = db.Queryable<CheckComplaintEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).GroupBy(a => a.RegisterCode)
                    .Where(a => a.DataType == "2")//住院
                    .WhereIF(!string.IsNullOrEmpty(rulecode), (a, b) => a.RulesCode == rulecode)
                    .WhereIF(!string.IsNullOrEmpty(states), (a, b) => b.ComplaintStatus == states)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), a => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), a => a.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), a => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), a => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), a => a.PersonalCode.Contains(queryCoditionByCheckResult.PersonalCode))
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, a => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, a => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .OrderBy(a => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(a.Name),
                        IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                        Gender = SqlFunc.AggregateMax(a.Gender),
                        Age = SqlFunc.AggregateMax(a.Age),
                        InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                        Count = SqlFunc.AggregateCount(a.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate),
                        States = SqlFunc.AggregateMax(b.ComplaintStatus),
                        PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                        ProposalMoney = SqlFunc.AggregateMax(b.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(b.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(b.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(b.ProposalDescribe),
                        FirstTrialDescribe = SqlFunc.AggregateMax(b.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(b.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(b.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(b.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(b.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode)
                    }).ToPageList(page, limit, ref count);

                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    resultList = db.Queryable<CheckComplaintEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).GroupBy(a => a.RegisterCode)
                    .Where(a => a.DataType == "2")//住院
                    .Where(a => a.InstitutionCode == curryydm)
                    .WhereIF(!string.IsNullOrEmpty(rulecode), (a, b) => a.RulesCode == rulecode)
                    .WhereIF(!string.IsNullOrEmpty(states), (a, b) => b.ComplaintStatus == states)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), a => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), a => a.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), a => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), a => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), a => a.PersonalCode.Contains(queryCoditionByCheckResult.PersonalCode))
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, a => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, a => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .OrderBy(a => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(a.Name),
                        IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                        Gender = SqlFunc.AggregateMax(a.Gender),
                        Age = SqlFunc.AggregateMax(a.Age),
                        InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                        Count = SqlFunc.AggregateCount(a.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate),
                        States = SqlFunc.AggregateMax(b.ComplaintStatus),
                        PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                        ProposalMoney = SqlFunc.AggregateMax(b.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(b.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(b.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(b.ProposalDescribe),
                        FirstTrialDescribe = SqlFunc.AggregateMax(b.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(b.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(b.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(b.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(b.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode)
                    }).ToPageList(page, limit, ref count);

                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    resultList = db.Queryable<CheckComplaintEntity, Check_ComplainEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    }).GroupBy(a => a.RegisterCode)
                    .Where(a => a.DataType == "2")//住院
                    .Where(a => a.InstitutionCode.Contains(XAreaCode.Substring(0, 6)))
                    .WhereIF(!string.IsNullOrEmpty(rulecode), (a, b) => a.RulesCode == rulecode)
                    .WhereIF(!string.IsNullOrEmpty(states), (a, b) => b.ComplaintStatus == states)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), a => a.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), a => a.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), a => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), a => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), a => a.PersonalCode.Contains(queryCoditionByCheckResult.PersonalCode))
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, a => a.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, a => a.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Select((a, b) => new CheckResultInfoEntity
                    {
                        RegisterCode = SqlFunc.AggregateMax(b.RegisterCode),
                        Name = SqlFunc.AggregateMax(a.Name),
                        IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                        Gender = SqlFunc.AggregateMax(a.Gender),
                        Age = SqlFunc.AggregateMax(a.Age),
                        InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                        DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                        Count = SqlFunc.AggregateCount(a.RegisterCode),
                        SettlementDate = (DateTime)SqlFunc.AggregateMax(a.SettlementDate),
                        States = SqlFunc.AggregateMax(b.ComplaintStatus),
                        PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                        ProposalMoney = SqlFunc.AggregateMax(b.ProposalMoney),
                        RealMoneyFirst = SqlFunc.AggregateMax(b.RealMoneyFirst),
                        RealMoneySecond = SqlFunc.AggregateMax(b.RealMoneySecond),
                        ProposalDescribe = SqlFunc.AggregateMax(b.ProposalDescribe),
                        FirstTrialDescribe = SqlFunc.AggregateMax(b.FirstTrialDescribe),
                        ComplaintDescribe = SqlFunc.AggregateMax(b.ComplaintDescribe),
                        SecondTrialDescribe = SqlFunc.AggregateMax(b.SecondTrialDescribe),
                        DoubtfulConclusionDescribe = SqlFunc.AggregateMax(b.DoubtfulConclusionDescribe),
                        CheckComplainId = SqlFunc.AggregateMax(b.CheckComplainId),
                        InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode)
                    }).ToPageList(page, limit, ref count);

                }


            }
            return resultList;
        }

        public List<CheckComplainStaticsEntity> GetCheckComplainStatics(QueryConditionByCheckComplain queryConditionByCheckComplain, bool isAdmin, string curryydm, int page, int limit, ref int count)
        {
            List<CheckComplainStaticsEntity> resultList = new List<CheckComplainStaticsEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string sql = @"SELECT InstitutionCode,InstitutionName,Type,KKJE,JSJE,SJBCJE,DBJE,KKRC,'"+queryConditionByCheckComplain.StartDate.ToString() + @"' StartTime,'"+queryConditionByCheckComplain.EndDate.ToString()+@"' EndTime  FROM (SELECT ROW_NUMBER() OVER(Order by a.ComplaintTime desc) AS RowId,* FROM 
                                   (SELECT yhi.InstitutionCode,yhi.InstitutionName,'医院' Type,sum(distinct cc.RealMoneySecond) KKJE,
                                    SUM(distinct yhi.ZFY) JSJE,SUM(distinct yhi.YBBXFY) SJBCJE,SUM(distinct yhi.DBBXBXFY) DBJE,
                                    SUM(distinct yhi.YLJZFY) YLJZJE,SUM(distinct yhi.SYBXBXFY) SYBXJE,SUM(distinct yhi.QTBCJE) SYBXBCJE,
                                    (SELECT COUNT(1) FROM Check_ComplaintMain WHERE InstitutionCode = yhi.InstitutionCode AND ComplaintResultStatus = '10') KKRC,MAX(cc.ComplaintTime) ComplaintTime
                                         FROM Check_Complain AS cc LEFT JOIN Check_ComplaintMain AS ccm ON cc.RegisterCode=ccm.RegisterCode
                                    left JOIN YB_HosInfo AS yhi ON cc.RegisterCode = yhi.HosRegisterCode
                                    WHERE cc.ComplaintStatus = '10' ";                
                if (!string.IsNullOrEmpty(queryConditionByCheckComplain.InstitutionCode))
                {
                    sql += " and yhi.InstitutionCode ='" + queryConditionByCheckComplain.InstitutionCode + "'";
                }
                if (!string.IsNullOrEmpty(queryConditionByCheckComplain.RulesCode))
                {
                    sql += " and  ccm.RulesCode='" + queryConditionByCheckComplain.RulesCode + "'";
                }
                if (queryConditionByCheckComplain.StartDate != null)
                {
                    sql += " and cc.ComplaintTime>='" + queryConditionByCheckComplain.StartDate + "'";
                }
                if (queryConditionByCheckComplain.EndDate != null)
                {
                    sql += " and cc.ComplaintTime<='" + queryConditionByCheckComplain.EndDate + "'";
                }
                if (isAdmin)
                {
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    sql += " and yhi.InstitutionCode ='" + curryydm + "'";
                }
                else
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    sql += " and yhi.InstitutionCode like '" + XAreaCode.Substring(0, 6) + "%'";
                }
                sql += " GROUP BY yhi.InstitutionCode,yhi.InstitutionName ) a)b ";
                string sql1 = " WHERE b.RowId BETWEEN " + (limit * (page - 1) + 1) + " AND " + (page * limit);
                resultList = db.Ado.SqlQuery<CheckComplainStaticsEntity>(sql);
                count = resultList.Count;
                resultList = db.Ado.SqlQuery<CheckComplainStaticsEntity>(sql+sql1);

            }
            return resultList;
        }


        #region 提交数据
        public bool Insert(string registerCode, ConditionStringCS condition, UserInfo userInfo)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var checkComplaintList = db.Queryable<CheckResultInfoEntity>()
                        .WhereIF(!string.IsNullOrEmpty(registerCode), it => it.RegisterCode == registerCode)
                        .Select(it => new CheckComplaintEntity
                        {
                     //       ComplaintCode = ConstDefine.CreateGuid(),
                            CheckResultInfoCode = it.CheckResultInfoCode,
                            RulesName = it.RulesName,
                            RulesCode = it.RulesCode,
                            RegisterCode = it.RegisterCode,
                            PersonalCode = it.PersonalCode,
                            IdNumber = it.IdNumber,
                            Name = it.Name,
                            Gender = it.Gender,
                            Age = it.Age,
                            InstitutionCode = it.InstitutionCode,
                            InstitutionName = it.InstitutionName,
                            ICDCode = it.ICDCode,
                            DiseaseName = it.DiseaseName,
                            DataType = it.DataType,
                            CheckDate = it.CheckDate,
                            ResultDescription = it.ResultDescription,
                            IsPre = it.IsPre,
                            SettlementDate = it.SettlementDate
                        })
                        .ToList();
                    //加入审核状态
                    foreach(var obj in checkComplaintList)
                    {
                        obj.ComplaintCode = ConstDefine.CreateGuid();
                        if (condition.rulesCode.Contains(obj.RulesCode))
                        {
                            obj.ComplaintResultStatus = ((int)CheckStates.J).ToString();
                        }
                        else
                        {
                            obj.ComplaintResultStatus = ((int)CheckStates.H).ToString();
                        }
                    }
                    db.Insertable(checkComplaintList.ToArray()).ExecuteCommand();
                    Check_ComplainEntity check_ComplainEntity = new Check_ComplainEntity
                    {
                        CheckComplainId = ConstDefine.CreateGuid(),
                        Count = checkComplaintList.Count(),
                        RegisterCode = registerCode,
                        ProposalMoney = condition.wgMoney,
                        RealMoneyFirst = condition.modifyMoney,
                        ProposalDescribe  = condition.moneyDescription,
                        FirstTrialTime = DateTime.Now,
                        FirstTrialUserId = userInfo.UserId,
                        FirstTrialUserName = userInfo.UserName,
                        FirstTrialInstitutionCode = userInfo.InstitutionCode,
                        FirstTrialInstitutionName = userInfo.InstitutionName,
                        FirstTrialDescribe = condition.describe
                    };
                    ////加入审核状态
                    if(condition.rulesCode.Length > 0)
                    {
                        check_ComplainEntity.ComplaintStatus = ((int)CheckStates.J).ToString();
                    }
                    else
                    {
                        check_ComplainEntity.ComplaintStatus = ((int)CheckStates.H).ToString();
                    }
                    db.Insertable(check_ComplainEntity).ExecuteCommand();
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    string str = ex.Message.ToString();
                    db.Ado.RollbackTran();
                    return false;
                }
            }
            return true;
        }

        public bool AddDescribe(string registerCode,CheckComplaintDetailEntity checkComplaintDetailEntity,string describe, UserInfo userInfo)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    db.Insertable(checkComplaintDetailEntity).ExecuteCommand();
                    var updatelist = db.Queryable<CheckComplaintEntity>().Where(it => it.RegisterCode == registerCode && it.ComplaintResultStatus == ((int)CheckStates.E).ToString()).ToList();
                    foreach (var obj in updatelist)
                    {
                        obj.ComplaintResultStatus = ((int)CheckStates.K).ToString();
                        db.Updateable(obj).Where(it => it.ComplaintCode == obj.ComplaintCode).ExecuteCommand();
                    }
                    db.Updateable<Check_ComplainEntity>().UpdateColumns(it => new Check_ComplainEntity()
                    {
                        DoubtfulConclusionTime = DateTime.Now,
                        DoubtfulConclusionUserId = userInfo.UserId,
                        DoubtfulConclusionUserName = userInfo.UserName,
                        DoubtfulConclusionInstitutionCode = userInfo.InstitutionCode,
                        DoubtfulConclusionInstitutionName = userInfo.InstitutionName,
                        ComplaintStatus = ((int)CheckStates.K).ToString(),
                        DoubtfulConclusionDescribe = describe
                    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();                
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }        
            }
            return true;
        }
        public bool Add(CheckComplaintDetailEntity checkComplaintDetailEntity)
        {
            int count = 0;
            using (var db = _dbContext.GetIntance())
            {            
                 count = db.Insertable(checkComplaintDetailEntity).ExecuteCommand();             
            }
            return count > 0 ? true : false;
        }
        public bool Add2(Check_ComplaintDetail_MZLEntity checkComplaintDetailMZLEntity)
        {
            int count = 0;
            using (var db = _dbContext.GetIntance())
            {
                count = db.Insertable(checkComplaintDetailMZLEntity).ExecuteCommand();
            }
            return count > 0 ? true : false;
        }
        public List<CheckComplaintEntity> GetListBySS(string flag,string rulescode, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<CheckComplaintEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if(isadmin)
                {
                    DataResult = db.Queryable<CheckComplaintEntity>().WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                          .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                          .Where(it => it.DataType == flag)
                          .Where(it => it.ComplaintResultStatus == ((int)CheckStates.G).ToString())
                          .ToPageList(page, limit, ref totalcount);
                }
                else
                {
                    DataResult = db.Queryable<CheckComplaintEntity>().WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                          .WhereIF(!string.IsNullOrEmpty(rulescode), it => it.RulesCode == rulescode)
                          .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                          .Where(it => it.DataType == flag)
                          .Where(it => it.InstitutionCode == curryydm)
                          .Where(it => it.ComplaintResultStatus == ((int)CheckStates.G).ToString())
                          .ToPageList(page, limit, ref totalcount);
                }
                
            }
            return DataResult;
        }

        public List<ComplaintMainEntity> GetListByWSS(string flag,string rulescode, QueryCoditionByCheckResult result, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var list = new List<ComplaintMainEntity>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                var querystr = string.Empty;
                if (!string.IsNullOrEmpty(rulescode))
                    querystr = " AND hb.RulesCode = '" + rulescode + "'";
                if (!string.IsNullOrEmpty(result.IdNumber))
                    querystr += " AND hb.IdNumber = '" + result.IdNumber + "'";
                if (!string.IsNullOrEmpty(result.ICDCode))
                    querystr += " AND hb.ICDCode = '" + result.ICDCode + "'";
                if (!string.IsNullOrEmpty(result.InstitutionCode))
                    querystr += " AND hb.InstitutionCode = '" + result.InstitutionCode + "'";
                if (!string.IsNullOrEmpty(result.Name))
                    querystr += " AND hb.Name like '%" + result.Name + "%'";
                if (isadmin)    //为管理员
                {
                    list = db.Ado.SqlQuery<ComplaintMainEntity>("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY RulesCode ASC) AS RowId,* FROM(SELECT * FROM(SELECT '' AS ComplaintDescribe,'' AS ComplaintResultStatus,'' AS ComplaintCode,CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode," +
                            "IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName,'' AS  FirstTrialDescribe,IsPre FROM dbo.Check_ResultInfo WHERE DataType = " + flag + " AND CheckResultInfoCode NOT IN (SELECT CheckResultInfoCode FROM Check_ComplaintMain) AND RulesCode IN (SELECT RulesCode FROM dbo.Check_RulesMain " +
                            "WHERE DeleteMark = 1 AND CheckLevelCode = " + ((int)RulesType.R002).ToString() + ")UNION ALL SELECT ComplaintDescribe,'' AS ComplaintResultStatus,ComplaintCode,CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName," +
                            "FirstTrialDescribe,IsPre FROM dbo.Check_ComplaintMain WHERE DataType = " + flag + " AND ComplaintResultStatus = " + ((int)CheckStates.J).ToString() + ") AS tt ) hb WHERE 1=1 " + querystr + ") SS WHERE SS.RowId BETWEEN " + bt1 + " AND " + bt2);
                    totalcount = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName,'' AS  FirstTrialDescribe,IsPre FROM dbo.Check_ResultInfo" +
                       " WHERE DataType = " + flag + " AND CheckResultInfoCode NOT IN (SELECT CheckResultInfoCode FROM Check_ComplaintMain) AND RulesCode IN (SELECT RulesCode FROM dbo.Check_RulesMain WHERE DeleteMark = 1 AND CheckLevelCode = " + ((int)RulesType.R002).ToString() + ")UNION ALL SELECT CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName," +
                       " ICDCode,DiseaseName,FirstTrialDescribe,IsPre FROM dbo.Check_ComplaintMain WHERE DataType = " + flag + " AND ComplaintResultStatus = " + ((int)CheckStates.J).ToString() + ") AS hb WHERE 1=1 " + querystr);

                }
                else
                {
                    list = db.Ado.SqlQuery<ComplaintMainEntity>("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY RulesCode ASC) AS RowId,* FROM(SELECT * FROM(SELECT '' AS ComplaintDescribe,'' AS ComplaintResultStatus,'' AS ComplaintCode,CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode," +
                           "IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName,'' AS  FirstTrialDescribe,IsPre FROM dbo.Check_ResultInfo WHERE DataType = 2 AND CheckResultInfoCode NOT IN (SELECT CheckResultInfoCode FROM Check_ComplaintMain) AND RulesCode IN (SELECT RulesCode FROM dbo.Check_RulesMain " +
                           "WHERE  DeleteMark = 1 AND CheckLevelCode = " + ((int)RulesType.R002).ToString() + ")UNION ALL SELECT ComplaintDescribe,'' AS ComplaintResultStatus,ComplaintCode,CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName," +
                           "FirstTrialDescribe,IsPre FROM dbo.Check_ComplaintMain WHERE DataType = " + flag + " AND ComplaintResultStatus = " + ((int)CheckStates.J).ToString() + ") AS tt ) hb WHERE 1=1 " + querystr + " AND hb.InstitutionCode = '" + curryydm + "') SS WHERE SS.RowId BETWEEN " + bt1 + " AND " + bt2);
                    totalcount = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName,ICDCode,DiseaseName,'' AS  FirstTrialDescribe,IsPre FROM dbo.Check_ResultInfo" +
                         " WHERE DataType = " + flag + " AND CheckResultInfoCode NOT IN (SELECT CheckResultInfoCode FROM Check_ComplaintMain) AND  RulesCode IN (SELECT RulesCode FROM dbo.Check_RulesMain WHERE DeleteMark = 1 AND CheckLevelCode = " + ((int)RulesType.R002).ToString() + ")UNION ALL SELECT CheckResultInfoCode,RulesName,RulesCode,RegisterCode,PersonalCode,IdNumber,Name,Gender,Age,InstitutionCode,InstitutionName," +
                         "ICDCode,DiseaseName,FirstTrialDescribe,IsPre FROM dbo.Check_ComplaintMain WHERE DataType = " + flag + " AND  ComplaintResultStatus = " + ((int)CheckStates.J).ToString() + ") AS hb WHERE 1=1 " + querystr + " AND hb.InstitutionCode = '" + curryydm + "'");
                }

            }
            return list;
        }

        #endregion
    }
}
