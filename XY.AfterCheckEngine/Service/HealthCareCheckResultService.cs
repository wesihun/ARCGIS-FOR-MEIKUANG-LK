using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;

namespace XY.AfterCheckEngine.Service
{
    public class HealthCareCheckResultService : IHealthCareCheckResultService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public HealthCareCheckResultService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        /// <summary>
        /// 获取审核数据
        /// </summary>
        /// <param name="resgisterCode"></param>
        /// <param name="flag">1门诊2住院</param>
        /// <returns></returns>
        public List<HealthCareCheckResultDto> healthCareCheckResultDtos(string resgisterCode,string flag)
        {
            string rediskey = resgisterCode + "CheckResult";
            var result = new List<HealthCareCheckResultDto>();
            //using (var redisdb = _redisDbContext.GetRedisIntance())
            //{
               // result = redisdb.Get<List<HealthCareCheckResultDto>>(rediskey);//从缓存里取
               // if (result == null)
               // {
                    using (var db = _dbContext.GetIntance())
                    {
                        result = db.Queryable<CheckResultInfoEntity, CheckResultPreInfoEntity>((a, b) => new object[] {
                        JoinType.Left,a.CheckResultInfoCode == b.CheckResultInfoCode
                        })
                        .Where((a, b) => a.RegisterCode == resgisterCode&&a.DataType==flag)
                        .Select((a, b) => new HealthCareCheckResultDto() {
                            CheckResultInfoCode = a.CheckResultInfoCode,
                            RulesName = a.RulesName,
                            RulesCode = a.RulesCode,
                            RegisterCode = a.RegisterCode,
                            PersonalCode = a.PersonalCode,
                            IdNumber = a.IdNumber,
                            Name = a.Name,
                            Gender = a.Gender,
                            Age = a.Age,
                            InstitutionCode =a.InstitutionCode,
                            InstitutionName = a.InstitutionName,
                            ICDCode = a.ICDCode,
                            DiseaseName = a.DiseaseName,
                            DataType =a.DataType,
                            CheckDate =  a.CheckDate,
                            ResultDescription =a.ResultDescription,
                            IsPre = a.IsPre,
                            PreCode = b.PreCode,
                            ItemIndex = b.ItemIndex,
                            PreResultDescription = b.ResultDescription,
                        }).ToList();
                        //if (result.Count > 0)
                        //{
                           // redisdb.Set(rediskey, result);
                           // redisdb.Expire(rediskey, 86400);//设置缓存时间1天
                       // }
                    }
               // }
           // }
            return result;
        }
        /// <summary>
        /// 获取违规处方列表
        /// </summary>
        /// <returns></returns>
        public List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string code,string flag)
        {
            var DataResult = new List<HosPreInfo_WGDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(code))
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity, CheckResultPreInfoEntity>((a, b) => new object[] {
                    JoinType.Left,a.PreCode == b.PreCode&&a.ItemIndex==b.ItemIndex&&a.HosRegisterCode==b.RegisterCode,
                    }).Where((a, b) => b.CheckResultInfoCode == code && b.DataType == flag)
                    .Select((a, b) => new HosPreInfo_WGDto
                    {
                        HosRegisterCode = a.HosRegisterCode,
                        PreCode = a.PreCode,
                        ItemIndex = a.ItemIndex,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        CollectFeesCategoryCode = a.CollectFeesCategoryCode,
                        CollectFeesCategoryName = a.CollectFeesCategoryName,
                        CollectFeesProjectGradeCode = a.CollectFeesProjectGradeCode,
                        CollectFeesProjectGradeName = a.CollectFeesProjectGradeName,
                        ZFY = a.ZFY,
                        YXJE = a.YXJE,
                        BKBXJE = a.BKBXJE,
                        COUNT = a.COUNT,
                        PRICE = a.PRICE,
                        CompRatio = a.CompRatio,
                        HisItemCode = a.HisItemCode,
                        HisItemName = a.HisItemName,
                        ResultDescription = b.ResultDescription
                    }).ToList();
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }
    }
}
