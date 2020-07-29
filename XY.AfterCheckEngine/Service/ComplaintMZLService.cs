using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.Service
{
    /// <summary>
    /// 功能描述：ComplaintMZLService
    /// 创 建 者：LK
    /// 创建日期：2019/11/19 15:39:53
    /// 最后修改者：LK
    /// 最后修改日期：2019/11/19 15:39:53
    /// </summary>
    public class ComplaintMZLService : IComplaintMZLService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;

        public ComplaintMZLService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        public DataListByCS GetFisrtInfos(string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            DataListByCS dataListByCs = new DataListByCS();
            List<CheckResultInfoEntity> dataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                //List<CheckResultPreInfoEntity> CheckResultPreInfoEntityList = db.Queryable<CheckResultPreInfoEntity>().ToList();
                //List<CheckResultInfoEntity> CheckResultInfoEntityList = db.Queryable<CheckResultInfoEntity>().Where(it => it.DataType == "2").ToList();
                var RegisterCodelist = db.Queryable<Check_Complain_MZLEntity>().Select(it => it.RegisterCode).ToList();
                //ISugarQueryable<CheckResultInfoEntity> datalist = null;
                if (isadmin)
                {
                    if (!string.IsNullOrEmpty(states))
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                        })
                       .Where((a, b) => a.DataType == "2")     //住院
                       .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                       .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                       .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                       .GroupBy((a, b) => a.RegisterCode)
                       .Having((a, b) => SqlFunc.AggregateMax(a.RuleLevel) == states)
                       .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                               .Select((a, b) => new CheckResultInfoEntity
                               {
                                   RulesName = SqlFunc.AggregateMax(a.RulesName),
                                   RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                   InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                   InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                   Name = SqlFunc.AggregateMax(a.Name),
                                   IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                   Gender = SqlFunc.AggregateMax(a.Gender),
                                   Age = SqlFunc.AggregateMax(a.Age),
                                   InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                   OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                   DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                   PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                   InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                   SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                   ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                   ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                   SHLCZT = "智审完成"
                               }).ToList();
                    }
                    else
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                        })
                        .Where((a, b) => a.DataType == "2")     //住院
                        .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                        .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                        .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                        .GroupBy((a, b) => a.RegisterCode)
                        .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                                .Select((a, b) => new CheckResultInfoEntity
                                {
                                    RulesName = SqlFunc.AggregateMax(a.RulesName),
                                    RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                    InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                    InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                    Name = SqlFunc.AggregateMax(a.Name),
                                    IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                    Gender = SqlFunc.AggregateMax(a.Gender),
                                    Age = SqlFunc.AggregateMax(a.Age),
                                    InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                    OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                    DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                    PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                    InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                    SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                    ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                    ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                    SHLCZT = "智审完成"
                                }).ToList();
                    }

                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    if (!string.IsNullOrEmpty(states))
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                        })
                       .Where((a, b) => a.DataType == "2")     //住院
                       .Where((a, b) => a.InstitutionCode == curryydm)
                       .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                       .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                       .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                       .GroupBy((a, b) => a.RegisterCode)
                       .Having((a, b) => SqlFunc.AggregateMax(a.RuleLevel) == states)
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "2", (a, b) => SqlFunc.AggregateMax(a.RuleLevel) == "2") //查询条件 包含违规
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "1", (a, b) => SqlFunc.AggregateMin(a.RuleLevel) == "1") //查询条件 疑似
                       .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                                .Select((a, b) => new CheckResultInfoEntity
                                {
                                    RulesName = SqlFunc.AggregateMax(a.RulesName),
                                    RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                    InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                    InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                    Name = SqlFunc.AggregateMax(a.Name),
                                    IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                    Gender = SqlFunc.AggregateMax(a.Gender),
                                    Age = SqlFunc.AggregateMax(a.Age),
                                    InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                    OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                    DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                    PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                    InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                    SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                    ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                    ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                    SHLCZT = "智审完成"
                                }).ToList();
                    }
                    else
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                        })
                       .Where((a, b) => a.DataType == "2")     //住院
                       .Where((a, b) => a.InstitutionCode == curryydm)
                       .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                       .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                       .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                       .GroupBy((a, b) => a.RegisterCode)
                       .Having((a, b) => SqlFunc.AggregateMax(a.RuleLevel) == states)
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "2", (a, b) => SqlFunc.AggregateMax(a.RuleLevel) == "2") //查询条件 包含违规
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "1", (a, b) => SqlFunc.AggregateMin(a.RuleLevel) == "1") //查询条件 疑似
                       .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                                .Select((a, b) => new CheckResultInfoEntity
                                {
                                    RulesName = SqlFunc.AggregateMax(a.RulesName),
                                    RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                    InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                    InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                    Name = SqlFunc.AggregateMax(a.Name),
                                    IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                    Gender = SqlFunc.AggregateMax(a.Gender),
                                    Age = SqlFunc.AggregateMax(a.Age),
                                    InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                    OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                    DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                    PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                    InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                    SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                    ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                    ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                    SHLCZT = "智审完成"
                                }).ToList();
                    }

                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    if (!string.IsNullOrEmpty(states))
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                    JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    })
                    .Where((a, b) => a.DataType == "2")     //住院
                    .Where((a, b) => a.InstitutionCode.Substring(0,6) == XAreaCode.Substring(0,6))
                    .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                    .GroupBy((a, b) => a.RegisterCode)
                    .WhereIF(!string.IsNullOrEmpty(states) && states == "2", (a, b) => SqlFunc.AggregateMax(a.RuleLevel) == "2") //查询条件 包含违规
                    .WhereIF(!string.IsNullOrEmpty(states) && states == "1", (a, b) => SqlFunc.AggregateMin(a.RuleLevel) == "1") //查询条件 疑似
                    .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                            .Select((a, b) => new CheckResultInfoEntity
                            {
                                RulesName = SqlFunc.AggregateMax(a.RulesName),
                                RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                Name = SqlFunc.AggregateMax(a.Name),
                                IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                Gender = SqlFunc.AggregateMax(a.Gender),
                                Age = SqlFunc.AggregateMax(a.Age),
                                InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                SHLCZT = "智审完成"
                            }).ToList();
                    }
                    else
                    {
                        dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                    JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    })
                    .Where((a, b) => a.DataType == "2")     //住院
                    .Where((a, b) => a.InstitutionCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                    .WhereIF(RegisterCodelist.Count > 0, (a, b) => !RegisterCodelist.Contains(a.RegisterCode))   //过滤已初审数据  包括全是刚性违规的
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), (a, b) => a.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => a.Name == queryCoditionByCheckResult.Name)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => a.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), (a, b) => b.YLJGDJBM == queryCoditionByCheckResult.InstitutionLevel)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => a.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => a.IdNumber == queryCoditionByCheckResult.IdNumber)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => a.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => a.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                    .GroupBy((a, b) => a.RegisterCode)
                    .WhereIF(!string.IsNullOrEmpty(states) && states == "2", (a, b) => SqlFunc.AggregateMax(a.RuleLevel) == "2") //查询条件 包含违规
                    .WhereIF(!string.IsNullOrEmpty(states) && states == "1", (a, b) => SqlFunc.AggregateMin(a.RuleLevel) == "1") //查询条件 疑似
                    .OrderBy((a, b) => SqlFunc.AggregateMax(a.SettlementDate), OrderByType.Desc)
                            .Select((a, b) => new CheckResultInfoEntity
                            {
                                RulesName = SqlFunc.AggregateMax(a.RulesName),
                                RegisterCode = SqlFunc.AggregateMax(a.RegisterCode),
                                InstitutionName = SqlFunc.AggregateMax(a.InstitutionName),
                                InstitutionLevel = SqlFunc.AggregateMax(b.YLJGDJBM),
                                Name = SqlFunc.AggregateMax(a.Name),
                                IdNumber = SqlFunc.AggregateMax(a.IdNumber),
                                Gender = SqlFunc.AggregateMax(a.Gender),
                                Age = SqlFunc.AggregateMax(a.Age),
                                InHosDate = SqlFunc.AggregateMax(a.InHosDate),
                                OutHosDate = SqlFunc.AggregateMax(a.OutHosDate),
                                DiseaseName = SqlFunc.AggregateMax(a.DiseaseName),
                                PersonalCode = SqlFunc.AggregateMax(a.PersonalCode),
                                InstitutionCode = SqlFunc.AggregateMax(a.InstitutionCode),
                                SettlementDate = SqlFunc.AggregateMax(a.SettlementDate),
                                ZWGSL = SqlFunc.AggregateCount(a.RegisterCode),
                                ZWGJE = SqlFunc.AggregateSum(a.MONEY),
                                SHLCZT = "智审完成"
                            }).ToList();
                    }
                }
                //添加疑点描述和疑点等级
                foreach (var obj in dataResult)
                {
                    //查询处方明细code
                    var checkResultInfoCodes = db.Queryable<CheckResultInfoEntity>().Where(it => it.RegisterCode == obj.RegisterCode && it.DataType == "2").Select(it => it.CheckResultInfoCode).ToList();
                    foreach (var code in checkResultInfoCodes)
                    {
                        //添加疑点描述
                        if (db.Queryable<CheckResultPreInfoEntity>().Any(it => it.CheckResultInfoCode == code))  //如果有处方
                        {

                            var PreInfolist = db.Queryable<CheckResultPreInfoEntity>().Where(it => it.CheckResultInfoCode == code).ToList();
                            foreach (var perInfo in PreInfolist)
                            {
                                obj.ResultDescription += "<b>" + perInfo.RulesName + "</b>" + perInfo.ResultDescription + "<br />";
                            }
                        }
                        else          //没有处方
                        {
                            obj.ResultDescription += "<b>" + db.Queryable<CheckResultInfoEntity>().Where(it => it.CheckResultInfoCode == code && it.DataType == "2").First().RulesName + "</b>" + db.Queryable<CheckResultInfoEntity>().Where(it => it.CheckResultInfoCode == code && it.DataType == "2").First().ResultDescription + "\n";
                        }
                    }
                    var list = db.Queryable<CheckResultInfoEntity>().Where(it => it.RegisterCode == obj.RegisterCode && it.DataType == "2").ToList();
                    if (list.Where(it => it.RuleLevel == "2").ToList().Count() == list.Count)   //都是刚性违规
                    {
                        obj.DSHWGJ = 0;
                        obj.DSHWGS = 0;
                        obj.YSHWGJ = obj.ZWGJE;
                        obj.YSHWGS = obj.ZWGSL;
                        obj.YDDJ = "2";
                    }
                    else if (list.Any(it => it.RuleLevel == "2"))    //其中包含刚性违规的
                    {
                        obj.DSHWGJ = list.Where(it => it.RuleLevel == "1").Sum(it => it.MONEY);
                        obj.DSHWGS = list.Where(it => it.RuleLevel == "1").Count();
                        obj.YSHWGJ = list.Where(it => it.RuleLevel == "2").Sum(it => it.MONEY);
                        obj.YSHWGS = list.Where(it => it.RuleLevel == "2").Count();
                        obj.YDDJ = "2";
                    }
                    else //所有都是疑似的
                    {
                        obj.DSHWGJ = list.Sum(it => it.MONEY);
                        obj.DSHWGS = list.Count();
                        obj.YSHWGJ = 0;
                        obj.YSHWGS = 0;
                        obj.YDDJ = "1";
                    }
                }
                dataListByCs.Record = new Record();
                dataListByCs.Record.BLSL = dataResult.Count;
                dataListByCs.Record.FY = dataResult.ToList().Sum(it => it.ZWGJE);
                dataListByCs.Record.DSHFY = dataResult.ToList().Sum(it => it.DSHWGJ);
                dataListByCs.Record.DSHTS = dataResult.ToList().Sum(it => it.DSHWGS);
                totalcount = dataResult.Count;
                dataResult = dataResult.Skip<CheckResultInfoEntity>((page - 1) * limit).Take<CheckResultInfoEntity>(limit).ToList();
                dataListByCs.checkResultInfoEntities = dataResult;

            }
            return dataListByCs;
        }
        public DataListByCS GetFisrtInfos2(string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            DataListByCS dataListByCs = new DataListByCS();
            string querystr = string.Empty;
            

            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode))
            {
                querystr += " AND RegisterCode = '"+ queryCoditionByCheckResult.RegisterCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.Name))
            {
                querystr += " AND Name = '" + queryCoditionByCheckResult.Name + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode))
            {
                querystr += " AND InstitutionCode = '" + queryCoditionByCheckResult.InstitutionCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel))
            {
                querystr += " AND InstitutionLevel = '" + queryCoditionByCheckResult.InstitutionLevel + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode))
            {
                querystr += " AND ICDCode = '" + queryCoditionByCheckResult.ICDCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber))
            {
                querystr += " AND IdNumber = '" + queryCoditionByCheckResult.IdNumber + "' ";
            }
            if (queryCoditionByCheckResult.StartSettleTime != null)
            {
                querystr += " AND SettlementDate >= '" + queryCoditionByCheckResult.StartSettleTime + "' ";
            }
            if (queryCoditionByCheckResult.EndSettleTime != null)
            {
                querystr += " AND SettlementDate <= '" + queryCoditionByCheckResult.EndSettleTime + "' ";
            }
            if (!string.IsNullOrEmpty(states))
            {
                if(states == "1")
                {
                    querystr += " AND DSHWGS <> 0 AND SHStates is NULL AND RuleLevel = '1' AND YSHWGS = 0";
                }
                else if(states == "2")
                {
                    querystr += " AND DSHWGS <> 0 AND SHStates is NULL AND RuleLevel = '1' AND YSHWGS <> 0";
                }
            }
            List<Check_ResultInfoMainEntity> dataResult = new List<Check_ResultInfoMainEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (isadmin)
                {
                    var sql = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.DataType == "2")
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "1",it => it.RuleLevel == "1" && it.YSHWGS == 0)
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "2", it => it.RuleLevel == "1" && it.YSHWGS != 0)
                        .Where(it => it.SHStates == null)   //过滤已初审数据  包括全是刚性违规的
                        .Where(it => it.DSHWGS != 0)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name == queryCoditionByCheckResult.Name)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                        .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                        .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime);
                    dataListByCs.Record = new Record();
                    dataListByCs.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ZWGJE) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_ResultInfoMain] WHERE DataType = '2'" + querystr);
                    dataListByCs.checkResultInfoEntities2 = sql.ToPageList(page, limit, ref totalcount);
                    dataListByCs.Record.BLSL = totalcount;
                    

                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    var sql = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.DataType == "2")
                        .Where(it => it.InstitutionCode == curryydm)
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "1", it => it.RuleLevel == "1" && it.YSHWGS == 0)
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "2", it => it.RuleLevel == "1" && it.YSHWGS != 0)
                        .Where(it => it.SHStates == null)   //过滤已初审数据  包括全是刚性违规的
                        .Where(it => it.DSHWGS != 0)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name == queryCoditionByCheckResult.Name)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                        .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                        .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime);
                    dataListByCs.Record = new Record();
                    dataListByCs.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ZWGJE) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_ResultInfoMain] WHERE DataType = '2'" + querystr);
                    dataListByCs.checkResultInfoEntities2 = sql.ToPageList(page, limit, ref totalcount);
                    dataListByCs.Record.BLSL = totalcount;
                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    var sql = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.DataType == "2")
                        .Where(it => it.FamilyCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "1", it => it.RuleLevel == "1" && it.YSHWGS == 0)
                        .WhereIF(!string.IsNullOrEmpty(states) && states == "2", it => it.RuleLevel == "1" && it.YSHWGS != 0)
                        .Where(it => it.SHStates == null)   //过滤已初审数据  包括全是刚性违规的
                        .Where(it => it.DSHWGS != 0)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name == queryCoditionByCheckResult.Name)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                        .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                        .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime);
                    dataListByCs.Record = new Record();
                    dataListByCs.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ZWGJE) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_ResultInfoMain] WHERE DataType = '2'" + querystr);
                    dataListByCs.checkResultInfoEntities2 = sql.ToPageList(page, limit, ref totalcount);
                    dataListByCs.Record.BLSL = totalcount;
                }
            }
            return dataListByCs;
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

        public List<CheckResultInfoEntity> GetFisrtYDDescribe(string registerCode)
        {
            var dataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<CheckResultInfoEntity>().Where(it => it.RegisterCode == registerCode && it.DataType == "2").ToList();
                //var queryList = db.Queryable<CheckResultPreInfoEntity>().ToList();
                //赋值违规费用   疑似违规费用等
                foreach (var obj in dataResult)
                {
                    obj.ZZSHZT = "未完成";
                    if (obj.RuleLevel == "1")    //疑似
                    {
                        obj.YSWGFY = obj.MONEY;
                        obj.WGFY = 0;
                        obj.WGLX = "1";
                    }
                    else if (obj.RuleLevel == "2")   //违规
                    {
                        obj.YSWGFY = 0;
                        obj.WGFY = obj.MONEY;
                        obj.WGLX = "2";
                    }
                }
                return dataResult;
            }
        }
        public List<WGConfirmEntity> GetFisrtWGConfirmList(string checkResultInfoCode)
        {
            var dataResult = new List<WGConfirmEntity>();
            using (var db = _dbContext.GetIntance())
            {
                var list = db.Queryable<CheckResultPreInfoEntity>().Where(it => it.CheckResultInfoCode == checkResultInfoCode).ToList();
                if (list.Count() > 0)   //如果有处方
                {
                    //dataResult = db.Queryable<CheckResultPreInfoEntity, YBHosPreInfoEntity>((a, b) => new object[] {
                    //JoinType.Left,a.PreCode == b.PreCode,
                    //})
                    //.Where((a, b) => a.ItemIndex == b.ItemIndex)
                    //.Where((a, b) => a.CheckResultInfoCode == checkResultInfoCode)
                    //.Select((a, b) => new WGConfirmEntity
                    //{
                    //    CheckResultPreInfoCode = a.CheckResultPreInfoCode,
                    //    RulesName = a.RulesName,
                    //    DetailName = b.ItemName,
                    //    Price = a.Price * a.Count,
                    //    BKBXJE = b.BKBXJE,
                    //    CompRatio = b.CompRatio,
                    //    Count = a.Count,
                    //    ItemCode = b.ItemCode,
                    //    YXJE = b.YXJE,
                    //    DJ = b.PRICE
                    //}).ToList();

                    dataResult = db.Ado.SqlQuery<WGConfirmEntity>(@"SELECT crpi.CheckResultPreInfoCode,crpi.RulesName,crpi.ResultDescription DetailName,(crpi.Price - crpi.LimitPrice)*crpi.[Count] AS Price,
                    yhpi.BKBXJE, yhpi.CompRatio,crpi.[Count] Count,yhpi.ItemCode,yhpi.YXJE,crpi.Price DJ,crpi.LimitPrice AS LimitPrice,(crpi.Price - crpi.LimitPrice) AS CLimitPrice 
                    FROM Check_ResultPreInfo AS crpi INNER JOIN YB_HosPreInfo AS yhpi ON crpi.PreCode=yhpi.PreCode
                    WHERE yhpi.ItemIndex=crpi.ItemIndex AND crpi.CheckResultInfoCode='" + checkResultInfoCode + "'");

                }
                else                 //否则取审核结果表
                {
                    dataResult = db.Queryable<CheckResultInfoEntity>().Where(it => it.CheckResultInfoCode == checkResultInfoCode)
                                  .Select(it => new WGConfirmEntity
                                  {
                                      CheckResultInfoCode = it.CheckResultInfoCode,
                                      RulesName = it.RulesName,
                                      YDDescription = it.ResultDescription,
                                      Price = it.MONEY,
                                      DetailName = ""
                                  }).ToList();
                }
                return dataResult;

            }
        }
        public bool FirstCommint(JsonArray jsonObject, string registerCode, CheckResultInfoEntity model, UserInfo userInfo)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    Check_Complain_MZLEntity check_Complain_MZLEntity = new Check_Complain_MZLEntity
                    {
                        CheckComplainId = ConstDefine.CreateGuid(),
                        ComplaintStatus = jsonObject.states,
                        ComplaintStatusStep = jsonObject.states,
                        PersonalCode = model.PersonalCode,
                        RegisterCode = registerCode,
                        InstitutionName = model.InstitutionName,
                        InstitutionCode = model.InstitutionCode,
                        InstitutionLevel = model.InstitutionLevel,
                        PersonName = model.Name,
                        ICDCode = model.ICDCode,
                        IdNumber = model.IdNumber,
                        DiseaseName = model.DiseaseName,
                        YDDescription = model.ResultDescription,
                        InHosDate = model.InHosDate,
                        OutHosDate = model.OutHosDate,
                        SettlementDate = model.SettlementDate,
                        DSHWGJ = model.DSHWGJ,
                        DSHWGS = model.DSHWGS,
                        YSHWGJ = model.YSHWGJ,
                        YSHWGS = model.YSHWGS,
                        ALLMoney = model.ZWGJE,
                        ALLCount = model.ZWGSL,
                        YDLevel = model.YDDJ,
                        FamilyCode = model.FamilyCode
                    };

                    check_Complain_MZLEntity.FirstTrialTime = DateTime.Now;
                    check_Complain_MZLEntity.FirstTrialUserId = userInfo.UserId;
                    check_Complain_MZLEntity.FirstTrialUserName = userInfo.UserName;
                    check_Complain_MZLEntity.FirstTrialInstitutionCode = userInfo.InstitutionCode;
                    check_Complain_MZLEntity.FirstTrialInstitutionName = userInfo.InstitutionName;
                    check_Complain_MZLEntity.FirstTrialDescribe = jsonObject.describe;
                    decimal? fy = 0;
                    int count = 0;
                    Dictionary<string, string> ruleslevel = new Dictionary<string, string>();
                    //插入从表
                    foreach (var obj in jsonObject.WGInfo)
                    {
                        CheckResultInfoEntity checkResultInfomodel = db.Queryable<CheckResultInfoEntity>().Where(it => it.CheckResultInfoCode == obj.Code).First();
                        if (obj.Value == "0")  //如果不违规  记录费用和数量  插入主表用
                        {
                            fy = checkResultInfomodel.MONEY;
                            count++;
                        }
                        Check_ComplaintMain_MZLEntity check_ComplaintMain_MZL = new Check_ComplaintMain_MZLEntity()
                        {
                            ComplaintCode = ConstDefine.CreateGuid(),
                            RegisterCode = registerCode,
                            CheckResultInfoCode = obj.Code,
                            States1 = obj.Value,
                            ComplaintStatus = jsonObject.states,
                            Price = checkResultInfomodel.MONEY,
                            RulesName = checkResultInfomodel.RulesName,
                            RulesCode = checkResultInfomodel.RulesCode,
                            RulesLevel = checkResultInfomodel.RuleLevel,
                            YDDescription = checkResultInfomodel.ResultDescription,
                            IsPre = "0",
                            FamilyCode = model.FamilyCode
                        };
                        ruleslevel.Add(obj.Code, checkResultInfomodel.RuleLevel);
                        if (obj.Value == "-1")
                        {
                            check_ComplaintMain_MZL.States1 = "1";
                        }
                        if (jsonObject.states == "10")
                        {
                            if (checkResultInfomodel.RuleLevel == "2")   //重点:初审直接结案  为都不违规状态   除刚性违规外
                            {
                                check_ComplaintMain_MZL.WGFY = checkResultInfomodel.MONEY;
                                check_ComplaintMain_MZL.YSWGFY = 0;
                                check_ComplaintMain_MZL.ZZSHStates = "违规";
                            }
                            else
                            {
                                check_ComplaintMain_MZL.YSWGFY = 0;
                                check_ComplaintMain_MZL.WGFY = 0;
                                check_ComplaintMain_MZL.ZZSHStates = "未违规";
                                check_ComplaintMain_MZL.YDDescription = "";
                            }
                        }
                        else
                        {
                            check_ComplaintMain_MZL.ZZSHStates = "未完成";
                            if (checkResultInfomodel.RuleLevel == "1")
                            {
                                if (obj.Value == "1" || obj.Value == "-1")   //默认操作为违规
                                {
                                    check_ComplaintMain_MZL.YSWGFY = checkResultInfomodel.MONEY;
                                    check_ComplaintMain_MZL.WGFY = 0;
                                }
                                else
                                {
                                    check_ComplaintMain_MZL.YSWGFY = 0;
                                    check_ComplaintMain_MZL.WGFY = 0;
                                    check_ComplaintMain_MZL.YDDescription = "";
                                }

                            }
                            else
                            {
                                check_ComplaintMain_MZL.YSWGFY = 0;
                                check_ComplaintMain_MZL.WGFY = checkResultInfomodel.MONEY;
                            }
                        }
                        db.Insertable(check_ComplaintMain_MZL).ExecuteCommand();
                    }
                    if (jsonObject.states == "10")    //直接结案  除刚性违规外其它都不违规
                    {
                        check_Complain_MZLEntity.SHLCStates = "审核结束";
                        if (model.YSHWGS > 0)    //如果有已审核违规数  则审核结果为 违规
                        {
                            check_Complain_MZLEntity.SHJG = "违规";
                            check_Complain_MZLEntity.ALLMoney = check_Complain_MZLEntity.ALLMoney - check_Complain_MZLEntity.DSHWGJ;
                            check_Complain_MZLEntity.ALLCount = check_Complain_MZLEntity.ALLCount - check_Complain_MZLEntity.DSHWGS;
                            check_Complain_MZLEntity.DSHWGJ = 0;
                            check_Complain_MZLEntity.DSHWGS = 0;
                        }
                        else
                        {
                            check_Complain_MZLEntity.SHJG = "未违规";
                            check_Complain_MZLEntity.YSHWGS = 0;
                            check_Complain_MZLEntity.YSHWGJ = 0;
                            check_Complain_MZLEntity.DSHWGJ = 0;
                            check_Complain_MZLEntity.DSHWGS = 0;
                            check_Complain_MZLEntity.ALLMoney = 0;
                            check_Complain_MZLEntity.ALLCount = 0;
                        }
                        check_Complain_MZLEntity.DoubtfulConclusionTime = DateTime.Now;
                        check_Complain_MZLEntity.FamilyCode = model.FamilyCode;
                        db.Insertable(check_Complain_MZLEntity).ExecuteCommand();
                    }
                    else
                    {
                        check_Complain_MZLEntity.SHLCStates = "人工初审疑似违规";
                        //插入主表
                        check_Complain_MZLEntity.DSHWGJ = check_Complain_MZLEntity.DSHWGJ - fy;
                        check_Complain_MZLEntity.DSHWGS = check_Complain_MZLEntity.DSHWGS - count;
                        check_Complain_MZLEntity.ALLCount = check_Complain_MZLEntity.ALLCount - count;
                        check_Complain_MZLEntity.ALLMoney = check_Complain_MZLEntity.ALLMoney - fy;
                        db.Insertable(check_Complain_MZLEntity).ExecuteCommand();
                    }
                    //插入从表(处方相关)
                    foreach (var objPre in jsonObject.WGQR)
                    {
                        CheckResultPreInfoEntity checkResultPreInfomodel = db.Queryable<CheckResultPreInfoEntity>().Where(it => it.CheckResultPreInfoCode == objPre.Code).First();
                        //YBHosPreInfoEntity hostprelist = db.Queryable<YBHosPreInfoEntity>()
                        //    .Where(it => it.PreCode == checkResultPreInfomodel.PreCode && it.ItemIndex == checkResultPreInfomodel.ItemIndex)
                        //    .First();
                        //var rulesLevel = db.Queryable<RulesMainEntity>().Where(it => it.RulesCode == checkResultPreInfomodel.RulesCode).First().CheckLevelCode;
                        var rulesLevel = "";
                        foreach (KeyValuePair<string, string> kvp in ruleslevel)
                        {
                            if(kvp.Key == checkResultPreInfomodel.CheckResultInfoCode)
                            {
                                rulesLevel = kvp.Value;
                            }
                        } 
                       
                        Check_ComplaintMain_MZLEntity check_ComplaintMain_MZLPre = new Check_ComplaintMain_MZLEntity()
                        {
                            ComplaintCode = ConstDefine.CreateGuid(),
                            RegisterCode = registerCode,
                            ComplaintStatus = objPre.Value,
                            CheckResultInfoCode = checkResultPreInfomodel.CheckResultInfoCode,
                            CheckResultPreInfoCode = objPre.Code,
                            States1 = objPre.Value,
                            RulesName = checkResultPreInfomodel.RulesName,
                            RulesCode = checkResultPreInfomodel.RulesCode,
                            RulesLevel = rulesLevel,
                            YDDescription = checkResultPreInfomodel.ResultDescription,
                            IsPre = "1",
                            PreName = checkResultPreInfomodel.ItemName,
                            Price = checkResultPreInfomodel.Price,
                            Count = checkResultPreInfomodel.Count,
                            ItemCode = checkResultPreInfomodel.ItemCode,
                            ItemName = checkResultPreInfomodel.ItemName,
                            CompRatio = checkResultPreInfomodel.CompRatio,
                            FamilyCode = model.FamilyCode
                        };
                        db.Insertable(check_ComplaintMain_MZLPre).ExecuteCommand();
                    }
                    //更新审核主表  标识已审核过
                    db.Updateable<Check_ResultInfoMainEntity>().UpdateColumns(it => new Check_ResultInfoMainEntity()
                    {
                        SHStates = "1"
                    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                    db.Ado.CommitTran();
                    return true;
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
            }
        }
        /// <summary>
        /// 根据住院登记编码获取审核状态信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        public Check_Complain_MZLEntity Get_Complain_MZLEntity(string registerCode)
        {
            var dataResult = new Check_Complain_MZLEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First();
            }
            return dataResult;
        }
        /// <summary>
        /// 根据住院登记编码获取审核状态信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <returns></returns>
        public Check_Complain_MZLEntity Get_Complain_MZLEntityALL(string registerCode)
        {
            var dataResult = new Check_Complain_MZLEntity();
            using (var db = _dbContext.GetIntance())
            {
                string shstates = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.RegisterCode == registerCode).First().SHStates;
                if (shstates == null)
                {
                    dataResult = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.RegisterCode == registerCode)
                                  .Select(a => new Check_Complain_MZLEntity
                                  {
                                      PersonName = a.Name,
                                      RegisterCode = a.RegisterCode,
                                      PersonalCode = a.PersonalCode,
                                      InstitutionName = a.InstitutionName,
                                      DiseaseName = a.DiseaseName,
                                      YDDescription = a.ResultDescription,
                                      IdNumber = a.IdNumber,
                                      SettlementDate = a.SettlementDate,
                                      DSHWGJ = a.DSHWGJ,
                                      DSHWGS = a.DSHWGS,
                                      YSHWGJ = a.YSHWGJ,
                                      YSHWGS = a.YSHWGS,
                                      ALLMoney = a.ZWGJE,
                                      ALLCount = a.ZWGSL,
                                      YDLevel = a.YDDJ
                                  }).First();
                }
                else
                {
                    dataResult = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First();
                }
                
            }
            return dataResult;
        }
        public DataListByOther GetInfosList(string step, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            DataListByOther dataListByOther = new DataListByOther();
            List<Check_Complain_MZLEntity> dataResult = new List<Check_Complain_MZLEntity>();
            string querystr = string.Empty;
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode))
            {
                querystr += " AND RegisterCode = '" + queryCoditionByCheckResult.RegisterCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.Name))
            {
                querystr += " AND PersonName = '" + queryCoditionByCheckResult.Name + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode))
            {
                querystr += " AND InstitutionCode = '" + queryCoditionByCheckResult.InstitutionCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel))
            {
                querystr += " AND InstitutionLevel = '" + queryCoditionByCheckResult.InstitutionLevel + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode))
            {
                querystr += " AND ICDCode = '" + queryCoditionByCheckResult.ICDCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber))
            {
                querystr += " AND IdNumber = '" + queryCoditionByCheckResult.IdNumber + "' ";
            }
            if (queryCoditionByCheckResult.StartSettleTime != null)
            {
                querystr += " AND SettlementDate >= '" + queryCoditionByCheckResult.StartSettleTime + "' ";
            }
            else
            {
                querystr += " AND SettlementDate >= '" + new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd") + "' ";
            }
            if (queryCoditionByCheckResult.EndSettleTime != null)
            {
                querystr += " AND SettlementDate <= '" + queryCoditionByCheckResult.EndSettleTime + "' ";
            }
            else
            {
                querystr += " AND SettlementDate <= '" + new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + "' ";
            }
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (isadmin)
                {
                    switch (step)
                    {                      
                        case "2":     //申诉
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                               .Where(it => it.ComplaintStatus == "1")
                               .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND ComplaintStatus = '1'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "22":     //二次反馈
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                               .Where(it => it.ComplaintStatus == "1" && it.IsSceondFK == "1")
                               .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND ComplaintStatus = '1' AND IsSceondFK = '1'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;           
                            break;
                        case "3":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "2")
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND ComplaintStatus = '2'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "4":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "3")
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND ComplaintStatus = '3'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "5":
                            if (queryCoditionByCheckResult.StartConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime >= '" + queryCoditionByCheckResult.StartConclusionTime + "' ";
                            }
                            if (queryCoditionByCheckResult.EndConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime <= '" + queryCoditionByCheckResult.EndConclusionTime + "' ";
                            }
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "10")
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                               .WhereIF(queryCoditionByCheckResult.StartConclusionTime != null, it => it.DoubtfulConclusionTime >= queryCoditionByCheckResult.StartConclusionTime)
                               .WhereIF(queryCoditionByCheckResult.EndConclusionTime != null, it => it.DoubtfulConclusionTime <= queryCoditionByCheckResult.EndConclusionTime)
                               .ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>(@"SELECT SUM(ALLMoney) AS FY
                                                    ,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND ComplaintStatus = '10'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        default:
                            break;
                    }
                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    switch (step)
                    {
                        case "2":     //申诉
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                               .Where(it => it.ComplaintStatus == "1")
                               .Where(it => it.InstitutionCode == curryydm)
                               .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND InstitutionCode = " + curryydm + "  AND ComplaintStatus = '1'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "3":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "2")
                                .Where(it => it.InstitutionCode == curryydm)
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND InstitutionCode = " + curryydm + "  AND ComplaintStatus = '2'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "4":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.InstitutionCode == curryydm)
                                .Where(it => it.ComplaintStatus == "3")
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND InstitutionCode = " + curryydm + "  AND ComplaintStatus = '3'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "5":
                            if (queryCoditionByCheckResult.StartConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime >= '" + queryCoditionByCheckResult.StartConclusionTime + "' ";
                            }
                            if (queryCoditionByCheckResult.EndConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime <= '" + queryCoditionByCheckResult.EndConclusionTime + "' ";
                            }
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "10")
                                .Where(it => it.InstitutionCode == curryydm)
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                                .WhereIF(queryCoditionByCheckResult.StartConclusionTime != null, it => it.DoubtfulConclusionTime >= queryCoditionByCheckResult.StartConclusionTime)
                               .WhereIF(queryCoditionByCheckResult.EndConclusionTime != null, it => it.DoubtfulConclusionTime <= queryCoditionByCheckResult.EndConclusionTime)
                               .ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>(@"SELECT SUM(ALLMoney) AS FY
                                                    ,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1  AND InstitutionCode = " + curryydm + " AND ComplaintStatus = '10'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        default:
                            break;
                    }

                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    switch (step)
                    {
                        case "2":     //申诉
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                               .Where(it => it.ComplaintStatus == "1")
                               .Where(it => it.FamilyCode.Substring(0,6) == XAreaCode.Substring(0,6))
                               .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND SUBSTRING(FamilyCode,0,7) = '" + XAreaCode.Substring(0, 6) + "' AND ComplaintStatus = '1'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "3":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                               .Where(it => it.ComplaintStatus == "2")
                               .Where(it => it.FamilyCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                               .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND SUBSTRING(FamilyCode,0,7) = '" + XAreaCode.Substring(0, 6) + "' AND ComplaintStatus = '2'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "4":
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.FamilyCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                                .Where(it => it.ComplaintStatus == "3")
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime).ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>("SELECT SUM(ALLMoney) AS FY,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND SUBSTRING(FamilyCode,0,7) = '" + XAreaCode.Substring(0, 6) + "' AND ComplaintStatus = '3'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        case "5":
                            if (queryCoditionByCheckResult.StartConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime >= '" + queryCoditionByCheckResult.StartConclusionTime + "' ";
                            }
                            if (queryCoditionByCheckResult.EndConclusionTime != null)
                            {
                                querystr += " AND DoubtfulConclusionTime <= '" + queryCoditionByCheckResult.EndConclusionTime + "' ";
                            }
                            dataResult = db.Queryable<Check_Complain_MZLEntity>()
                                .Where(it => it.ComplaintStatus == "10")
                                .Where(it => it.FamilyCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                                .WhereIF(!string.IsNullOrEmpty(states), it => it.YDLevel == states)
                                .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                               .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                               .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                               .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                               .WhereIF(queryCoditionByCheckResult.StartConclusionTime != null, it => it.DoubtfulConclusionTime >= queryCoditionByCheckResult.StartConclusionTime)
                               .WhereIF(queryCoditionByCheckResult.EndConclusionTime != null, it => it.DoubtfulConclusionTime <= queryCoditionByCheckResult.EndConclusionTime)
                               .ToPageList(page, limit, ref totalcount);
                            dataListByOther.Record = new Record();
                            dataListByOther.Record = db.Ado.SqlQuerySingle<Record>(@"SELECT SUM(ALLMoney) AS FY
                                                    ,SUM(DSHWGJ) AS DSHFY,SUM(DSHWGS) AS DSHTS FROM [Check_Complain_MZL] WHERE 1 = 1 AND SUBSTRING(FamilyCode,0,7) = '" + XAreaCode.Substring(0, 6) + "' AND ComplaintStatus = '10'" + querystr);
                            dataListByOther.check_Complain_MZLEntities = dataResult;
                            dataListByOther.Record.BLSL = totalcount;
                            break;
                        default:
                            break;
                    }
                }
            }
            return dataListByOther;
        }
        public List<Check_ComplaintMain_MZLEntity> GetYDInfoList(string registerCode)
        {
            var dataResult = new List<Check_ComplaintMain_MZLEntity>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.RegisterCode == registerCode && it.IsPre == "0").ToList();
            }
            return dataResult;
        }
        public List<Check_ComplaintMain_MZLEntity> GetWGQRInfoList(string complaintCode)
        {
            var dataResult = new List<Check_ComplaintMain_MZLEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string checkResultInfoCode = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == complaintCode).First().CheckResultInfoCode;
                dataResult = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == checkResultInfoCode && it.IsPre == "1").ToList();
                if (dataResult.Count == 0)   //如何没有处方则取主规则
                {
                    dataResult = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == checkResultInfoCode && it.IsPre == "0").ToList();
                    //price  count 有可能为null
                    foreach (var obj in dataResult)
                    {
                        if (obj.Count == null)
                        {
                            obj.Count = 1;
                        }
                        obj.ZJ = obj.Price;
                    }
                }
                else
                {
                    //price  count 有可能为null
                    foreach (var obj in dataResult)
                    {
                        //if (obj.Price == null)
                        //{
                        //    obj.Price = 0;
                        //}
                        //if (obj.Count == null)
                        //{
                        //    obj.Count = 0;
                        //}
                        //obj.ZJ = obj.Price * obj.Count;
                        obj.ZJ = (obj.Price == null ? 0 : obj.Price) * (obj.Count == null ? 0 : obj.Count);
                    }
                }
                
            }
            return dataResult;
        }

        public List<Check_ComplaintMain_MZLDto> GetWGQRInfoListByZJ(string complaintCode)
        {
            var dataResult = new List<Check_ComplaintMain_MZLDto>();
            using (var db = _dbContext.GetIntance())
            {
                string checkResultInfoCode = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == complaintCode).First().CheckResultInfoCode;
                if (db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == checkResultInfoCode && it.IsPre == "1").Count() > 0)
                {
                    dataResult = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(a => a.CheckResultInfoCode == checkResultInfoCode && a.IsPre == "1")
                  .Select(a => new Check_ComplaintMain_MZLDto
                  {
                      ComplaintCode = a.ComplaintCode,
                      RulesName = a.RulesName,
                      PreName = a.PreName,
                      Price = a.Price * a.Count,
                      States1 = a.States1,
                      States2 = a.States2,
                      States3 = a.States3,
                      States4 = a.States4,
                      StatesSecondFK = a.StatesSecondFK,
                      Count = a.Count,
                      DJ = a.Price,
                      ItemCode = a.ItemCode,
                      ItemName = a.ItemName,
                      CompRatio = a.CompRatio
                  }).ToList();
                }
                else
                {
                    dataResult = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(a => a.CheckResultInfoCode == checkResultInfoCode)
                   .Select(a => new Check_ComplaintMain_MZLDto
                   {
                       ComplaintCode = a.ComplaintCode,
                       RulesName = a.RulesName,
                       PreName = a.PreName,
                       Price = a.Price,   //总金额
                       States1 = a.States1,
                       States2 = a.States2,
                       States3 = a.States3,
                       States4 = a.States4,
                       StatesSecondFK = a.StatesSecondFK,
                       Count = 1                     
                   }).ToList();
                }
            }
            return dataResult;
        }
        public decimal? GetPrice(List<CheckResultPreInfoEntity> list, string code, string rulescode)
        {
            if (list.Count > 0)
            {
                var entity = list.Where(it => it.CheckResultInfoCode == code && it.RulesCode == rulescode).First();
                return entity.Price * entity.Count;
            }
            return 0;

        }
        public string Commint(string step, JsonArray jsonObject, string registerCode, UserInfo userInfo)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    //这个人的所有主违规list
                    var mainlistByRegister = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.RegisterCode == registerCode && it.IsPre == "0").ToList();
                    //这个人的所有处方关联list
                    //      var mainPrelistByRegister = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.RegisterCode == registerCode && it.IsPre == "1").ToList();
                    var updatelist = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First();
                    //更新主表
                    if (jsonObject.states == "10")    //直接结案
                    {
                        string shjg = "未违规";   //审核结果
                        if (mainlistByRegister.Where(it => it.RulesLevel == "2").Count() > 0)
                        {
                            shjg = "违规";
                        }
                        if (step == "2")   //第二步  反馈提交 
                        {
                            decimal? money = 0;
                            int? count = 0;
                            string shjg2= "未违规";
                            //更新从表(处方相关)
                            foreach (var objPre in jsonObject.WGQR)
                            {
                                if (objPre.Value == "1" || objPre.Value == "-1")
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "违规",
                                        States2 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                    var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == objPre.Code).First();
                                    money += list.Price * list.Count;
                                    count++;
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "未违规",
                                        States2 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }

                            }
                            //更新从表主违规
                            foreach (var obj in jsonObject.WGInfo)
                            {
                                var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code && it.IsPre == "0").First();
                                var model = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == mainModel.ComplaintCode).First();
                                if (obj.Value == "1" || obj.Value == "-1")
                                {
                                    shjg2 = "违规";
                                    if (obj.Value == "1")
                                    {
                                        var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == mainModel.CheckResultInfoCode && it.IsPre == "1").ToList();
                                        if(list.Count == 0)
                                        {
                                            money += mainModel.Price;
                                        }
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = "违规",
                                            States2 = obj.Value,
                                            WGFY = obj.Price,
                                            YSWGFY = 0
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                    else
                                    {
                                        money += mainModel.Price;
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = "违规",
                                            States2 = obj.Value
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        ZZSHStates = "未违规",
                                        YDDescription = "",
                                        States2 = obj.Value,
                                        YSWGFY = 0,
                                        WGFY = 0
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                            }
                            db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                            {
                                ComplaintStatus = jsonObject.states,
                                ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                IsSceondFK = "0",
                                SHLCStates = "审核结束",
                                SHJG = shjg2,
                                YSHWGJ = money,
                                YSHWGS = count,
                                DSHWGJ = 0,
                                DSHWGS = 0,
                                ALLMoney = money,
                                ALLCount = count,
                                DoubtfulConclusionTime = DateTime.Now,
                                ComplaintTime = DateTime.Now,
                                ComplaintUserId = userInfo.UserId,
                                ComplaintUserName = userInfo.UserName,
                                ComplaintInstitutionCode = userInfo.InstitutionCode,
                                ComplaintInstitutionName = userInfo.InstitutionName,
                                ComplaintDescribe = jsonObject.describe
                            }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();                            
                        }

                        #region 初次反馈之前流程 都违规才能提交
                        //if (step == "2")   //第二步  反馈提交  都违规才能提交
                        //{

                        //    db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                        //    {
                        //        ComplaintStatus = jsonObject.states,
                        //        ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                        //        IsSceondFK = "0",
                        //        SHLCStates = "审核结束",
                        //        SHJG = shjg,
                        //        YSHWGJ = updatelist.YSHWGJ + updatelist.DSHWGJ,
                        //        YSHWGS = updatelist.YSHWGS + updatelist.DSHWGS,
                        //        DSHWGJ = 0,
                        //        DSHWGS = 0,
                        //        DoubtfulConclusionTime = DateTime.Now,
                        //        ComplaintTime = DateTime.Now,
                        //        ComplaintUserId = userInfo.UserId,
                        //        ComplaintUserName = userInfo.UserName,
                        //        ComplaintInstitutionCode = userInfo.InstitutionCode,
                        //        ComplaintInstitutionName = userInfo.InstitutionName,
                        //        ComplaintDescribe = jsonObject.describe
                        //    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                        //    //更新从表主违规
                        //    foreach (var obj in jsonObject.WGInfo)
                        //    {
                        //        var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code && it.IsPre == "0").First();
                        //        var model = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == mainModel.ComplaintCode).First();
                        //        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //        {
                        //            ComplaintStatus = obj.Value,
                        //            ZZSHStates = "违规",
                        //            States2 = obj.Value,
                        //            YSWGFY = 0,
                        //            WGFY = model.WGFY + model.YSWGFY
                        //        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                        //    }
                        //    //更新从表(处方相关)
                        //    foreach (var objPre in jsonObject.WGQR)
                        //    {
                        //        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //        {
                        //            ComplaintStatus = objPre.Value,
                        //            ZZSHStates = "违规",
                        //            States2 = objPre.Value
                        //        }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                        //    }
                        //}
                        #endregion

                        if (step == "3") //第三步复审提交    直接结案的话  都不违规才能结案或者与医院初次反馈结果相同
                        {
                            decimal? money = 0;
                            int? count = 0;
                            string shjg2 = "未违规";
                            //更新从表(处方相关)
                            foreach (var objPre in jsonObject.WGQR)
                            {
                                if (objPre.Value == "1" || objPre.Value == "-1")
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "违规",
                                        States3 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                    var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == objPre.Code).First();
                                    money += list.Price * list.Count;
                                    count++;
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "未违规",
                                        States3 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }
                            }
                            //更新从表主违规
                            foreach (var obj in jsonObject.WGInfo)
                            {
                                var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code && it.IsPre == "0").First();
                                if (obj.Value == "1" || obj.Value == "-1")
                                {
                                    shjg2 = "违规";
                                    if (obj.Value == "1")
                                    {
                                        var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == mainModel.CheckResultInfoCode && it.IsPre == "1").ToList();
                                        if (list.Count == 0)
                                        {
                                            money += mainModel.Price;
                                        }
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = "违规",
                                            States3 = obj.Value,
                                            WGFY = obj.Price,
                                            YSWGFY = 0
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                    else
                                    {
                                        if (mainModel.WGFY == Convert.ToDecimal("0.00") && mainModel.YSWGFY == Convert.ToDecimal("0.00"))
                                        {
                                            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                            {
                                                ComplaintStatus = obj.Value,
                                                ZZSHStates = "未违规",
                                                YDDescription = "",
                                                States3 = obj.Value,
                                                YSWGFY = 0,
                                                WGFY = 0
                                            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                        }
                                        else
                                        {
                                            money += mainModel.Price;
                                            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                            {
                                                ComplaintStatus = obj.Value,
                                                ZZSHStates = "违规",
                                                States3 = obj.Value
                                            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                        }
                                       
                                       
                                    }
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        ZZSHStates = "未违规",
                                        YDDescription = "",
                                        States3 = obj.Value,
                                        YSWGFY = 0,
                                        WGFY = 0
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                            }
                            db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                            {
                                ComplaintStatus = jsonObject.states,
                                ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                IsSceondFK = "0",
                                SHLCStates = "审核结束",
                                SHJG = shjg2,
                                YSHWGJ = money,
                                YSHWGS = count,
                                DSHWGJ = 0,
                                DSHWGS = 0,
                                ALLMoney = money,
                                ALLCount = count,
                                DoubtfulConclusionTime = DateTime.Now,
                                SecondTrialTime = DateTime.Now,
                                SecondTrialUserId = userInfo.UserId,
                                SecondTrialUserName = userInfo.UserName,
                                SecondTrialInstitutionCode = userInfo.InstitutionCode,
                                SecondTrialInstitutionName = userInfo.InstitutionName,
                                SecondTrialDescribe = jsonObject.describe
                            }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();                            
                        }

                        #region
                        //if (step == "3") //第三步复审提交    直接结案的话  都不违规才能结案
                        //{
                        //    if (shjg == "违规")
                        //    {
                        //        db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                        //        {
                        //            ComplaintStatus = jsonObject.states,
                        //            ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                        //            IsSceondFK = "0",
                        //            SHLCStates = "审核结束",
                        //            SHJG = shjg,
                        //            DSHWGJ = 0,
                        //            DSHWGS = 0,
                        //            ALLMoney = updatelist.YSHWGJ,
                        //            ALLCount = updatelist.YSHWGS,
                        //            DoubtfulConclusionTime = DateTime.Now,
                        //            SecondTrialTime = DateTime.Now,
                        //            SecondTrialUserId = userInfo.UserId,
                        //            SecondTrialUserName = userInfo.UserName,
                        //            SecondTrialInstitutionCode = userInfo.InstitutionCode,
                        //            SecondTrialInstitutionName = userInfo.InstitutionName,
                        //            SecondTrialDescribe = jsonObject.describe
                        //        }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                        //    }
                        //    else
                        //    {
                        //        db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                        //        {
                        //            ComplaintStatus = jsonObject.states,
                        //            ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                        //            IsSceondFK = "0",
                        //            SHLCStates = "审核结束",
                        //            SHJG = shjg,
                        //            YSHWGJ = 0,
                        //            YSHWGS = 0,
                        //            DSHWGJ = 0,
                        //            DSHWGS = 0,
                        //            ALLCount = 0,
                        //            ALLMoney = 0,
                        //            DoubtfulConclusionTime = DateTime.Now,
                        //            SecondTrialTime = DateTime.Now,
                        //            SecondTrialUserId = userInfo.UserId,
                        //            SecondTrialUserName = userInfo.UserName,
                        //            SecondTrialInstitutionCode = userInfo.InstitutionCode,
                        //            SecondTrialInstitutionName = userInfo.InstitutionName,
                        //            SecondTrialDescribe = jsonObject.describe
                        //        }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                        //    }
                        //    //更新从表主违规
                        //    foreach (var obj in jsonObject.WGInfo)
                        //    {
                        //        var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                        //        if (mainModel.RulesLevel == "1")
                        //        {
                        //            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //            {
                        //                ComplaintStatus = obj.Value,
                        //                ZZSHStates = "未违规",
                        //                YDDescription = "",
                        //                YSWGFY = 0,
                        //                States3 = obj.Value
                        //            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                        //        }
                        //        else
                        //        {
                        //            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //            {
                        //                ComplaintStatus = obj.Value,
                        //                ZZSHStates = "违规",
                        //                States3 = obj.Value
                        //            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                        //        }

                        //    }
                        //    //更新从表(处方相关)
                        //    foreach (var objPre in jsonObject.WGQR)
                        //    {
                        //        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //        {
                        //            ComplaintStatus = objPre.Value,
                        //            ZZSHStates = "未违规",
                        //            States3 = objPre.Value
                        //        }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                        //    }
                        //}
                        #endregion

                        if (step == "22") //二次反馈提交  都违规
                        {
                            decimal? money = 0;
                            int? count = 0;
                            string shjg2 = "未违规";
                            //更新从表(处方相关)
                            foreach (var objPre in jsonObject.WGQR)
                            {
                                if (objPre.Value == "1" || objPre.Value == "-1")
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "违规",
                                        StatesSecondFK = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                    var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == objPre.Code).First();
                                    money += list.Price * list.Count;
                                    count++;
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = "未违规",
                                        StatesSecondFK = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }

                            }
                            //更新从表主违规
                            foreach (var obj in jsonObject.WGInfo)
                            {
                                var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code && it.IsPre == "0").First();
                                var model = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == mainModel.ComplaintCode).First();
                                if (obj.Value == "1" || obj.Value == "-1")
                                {
                                    shjg2 = "违规";
                                    if (obj.Value == "1")
                                    {
                                        var list = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.CheckResultInfoCode == mainModel.CheckResultInfoCode && it.IsPre == "1").ToList();
                                        if (list.Count == 0)
                                        {
                                            money += mainModel.Price;
                                        }
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = "违规",
                                            States2 = obj.Value,
                                            WGFY = obj.Price,
                                            YSWGFY = 0
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                    else
                                    {
                                        money += mainModel.Price;
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = "违规",
                                            States2 = obj.Value
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                }
                                else
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        ZZSHStates = "未违规",
                                        YDDescription = "",
                                        States2 = obj.Value,
                                        YSWGFY = 0,
                                        WGFY = 0
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                            }
                            db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                            {
                                ComplaintStatus = jsonObject.states,
                                ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                IsSceondFK = "1",
                                SHLCStates = "审核结束",
                                SHJG = shjg2,
                                YSHWGJ = money,
                                YSHWGS = count,
                                DSHWGJ = 0,
                                DSHWGS = 0,
                                ALLMoney = money,
                                ALLCount = count,
                                DoubtfulConclusionTime = DateTime.Now,
                                ComplaintSecondTime = DateTime.Now,
                                ComplaintSecondUserId = userInfo.UserId,
                                ComplaintSecondUserName = userInfo.UserName,
                                ComplaintInstitutionCode = userInfo.InstitutionCode,
                                ComplaintSecondInstitutionName = userInfo.InstitutionName,
                                ComplaintSecondDescribe = jsonObject.describe
                            }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();                           
                        }
                        #region 二次反馈  之前都违规才能提交到疑点结论
                        //if (step == "22") //二次反馈提交  都违规
                        //{
                        //    db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                        //    {
                        //        ComplaintStatus = jsonObject.states,
                        //        ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                        //        SHLCStates = "审核结束",
                        //        YSHWGJ = updatelist.YSHWGJ + updatelist.DSHWGJ,
                        //        YSHWGS = updatelist.YSHWGS + updatelist.DSHWGS,
                        //        SHJG = shjg,
                        //        DSHWGJ = 0,
                        //        DSHWGS = 0,
                        //        DoubtfulConclusionTime = DateTime.Now,
                        //        ComplaintSecondTime = DateTime.Now,
                        //        ComplaintSecondUserId = userInfo.UserId,
                        //        ComplaintSecondUserName = userInfo.UserName,
                        //        ComplaintInstitutionCode = userInfo.InstitutionCode,
                        //        ComplaintSecondInstitutionName = userInfo.InstitutionName,
                        //        ComplaintSecondDescribe = jsonObject.describe
                        //    }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                        //    //更新从表主违规
                        //    foreach (var obj in jsonObject.WGInfo)
                        //    {
                        //        var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                        //        var update = db.Queryable<Check_ComplaintMain_MZLEntity>().Where(it => it.ComplaintCode == mainModel.ComplaintCode).First();
                        //        if (update.YSWGFY != 0)
                        //        {
                        //            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //            {
                        //                ComplaintStatus = obj.Value,
                        //                ZZSHStates = "违规",
                        //                WGFY = update.YSWGFY,
                        //                YSWGFY = 0,
                        //                StatesSecondFK = obj.Value
                        //            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                        //        }
                        //        else
                        //        {
                        //            db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //            {
                        //                ComplaintStatus = obj.Value,
                        //                ZZSHStates = "违规",
                        //                StatesSecondFK = obj.Value
                        //            }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                        //        }

                        //    }
                        //    //更新从表(处方相关)
                        //    foreach (var objPre in jsonObject.WGQR)
                        //    {
                        //        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                        //        {
                        //            ComplaintStatus = objPre.Value,
                        //            ZZSHStates = "违规",
                        //            StatesSecondFK = objPre.Value
                        //        }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                        //    }
                        //}
                        #endregion
                        if (step == "4") //专家审核提交   状态为10  
                        {
                            decimal? wgfy = 0;
                            int count = 0;
                            bool flag = false;   //判断主表SHJG是否违规
                            //更新从表主违规
                            foreach (var obj in jsonObject.WGInfo)
                            {
                                string zzshStates = string.Empty;
                                var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                                if (obj.Value == "1" || obj.Value == "-1")
                                {
                                    wgfy += obj.Price;
                                    count++;
                                    zzshStates = "违规";
                                    flag = true;
                                    if (obj.Value == "1")
                                    {
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = zzshStates,
                                            States4 = obj.Value,
                                            WGFY = obj.Price,
                                            YSWGFY = 0
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                    else
                                    {
                                        db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                        {
                                            ComplaintStatus = obj.Value,
                                            ZZSHStates = zzshStates,
                                            States4 = obj.Value
                                        }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                    }
                                }
                                else
                                {
                                    zzshStates = "未违规";
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        ZZSHStates = zzshStates,
                                        YDDescription = "",
                                        States4 = obj.Value,
                                        WGFY = 0,
                                        YSWGFY = 0
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }

                            }
                            if (flag)
                            {
                                shjg = "违规";
                            }
                            else
                            {
                                shjg = "未违规";
                            }
                            //更新主表
                            db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                            {
                                ComplaintStatus = jsonObject.states,
                                ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                SHLCStates = "审核结束",
                                YSHWGJ = wgfy,
                                YSHWGS = count,
                                ALLCount = count,
                                ALLMoney = wgfy,
                                SHJG = shjg,
                                DSHWGJ = 0,
                                DSHWGS = 0,
                                DoubtfulConclusionTime = DateTime.Now,
                                ExpertTrialTime = DateTime.Now,
                                ExpertTrialUserId = userInfo.UserId,
                                ExpertTrialInstitutionName = userInfo.UserName,
                                ExpertTrialInstitutionCode = userInfo.InstitutionCode,
                                ExpertTrialUserName = userInfo.InstitutionName,
                                ExpertTrialDescribe = jsonObject.describe
                            }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                            //更新从表(处方相关)
                            foreach (var objPre in jsonObject.WGQR)
                            {
                                string zzshStates = string.Empty;
                                if (objPre.Value == "1" || objPre.Value == "-1")
                                {
                                    zzshStates = "违规";
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = zzshStates,
                                        States4 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();

                                }
                                else
                                {
                                    zzshStates = "未违规";
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        ZZSHStates = zzshStates,
                                        States4 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }
                               
                            }
                        }

                    }
                    else
                    {
                        if (step == "2")   //第二步反馈提交
                        {
                            var model = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First();
                            if (model.ComplaintStatusStep.Contains("2"))
                            {
                                //避免重复提交
                            }
                            else
                            {
                                //更新主表
                                db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                                {
                                    ComplaintStatus = jsonObject.states,
                                    ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                    IsSceondFK = "0",
                                    SHLCStates = "医院不认可疑似违规病例",
                                    ComplaintTime = DateTime.Now,
                                    ComplaintUserId = userInfo.UserId,
                                    ComplaintUserName = userInfo.UserName,
                                    ComplaintInstitutionCode = userInfo.InstitutionCode,
                                    ComplaintInstitutionName = userInfo.InstitutionName,
                                    ComplaintDescribe = jsonObject.describe
                                }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                                //更新从表主违规
                                foreach (var obj in jsonObject.WGInfo)
                                {
                                    var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        States2 = obj.Value
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                                //更新从表(处方相关)
                                foreach (var objPre in jsonObject.WGQR)
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        States2 = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }
                            }
                            
                        }
                        if (step == "3")  //第三步复审提交
                        {
                            //更新主表
                            db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                            {
                                ComplaintStatus = jsonObject.states,
                                ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                IsSceondFK = "1",
                                SHLCStates = "人工复审疑似违规",
                                SecondTrialTime = DateTime.Now,
                                SecondTrialUserId = userInfo.UserId,
                                SecondTrialUserName = userInfo.UserName,
                                SecondTrialInstitutionCode = userInfo.InstitutionCode,
                                SecondTrialInstitutionName = userInfo.InstitutionName,
                                SecondTrialDescribe = jsonObject.describe
                            }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                            //更新从表主违规
                            foreach (var obj in jsonObject.WGInfo)
                            {
                                var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                                if (obj.Value == "1")   //违规
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        States3 = obj.Value
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                                else if (obj.Value == "0")
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        States3 = obj.Value,
                                        YSWGFY = 0,
                                        WGFY = 0
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }

                            }
                            //更新从表(处方相关)
                            foreach (var objPre in jsonObject.WGQR)
                            {
                                db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                {
                                    ComplaintStatus = objPre.Value,
                                    States3 = objPre.Value
                                }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                            }
                        }
                        if (step == "22") //二次反馈提交
                        {
                            var model = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First();
                            if (model.ComplaintStatusStep.Contains("2,1,3"))
                            {
                                //避免重复提交
                            }
                            else
                            {
                                //更新主表
                                db.Updateable<Check_Complain_MZLEntity>().UpdateColumns(it => new Check_Complain_MZLEntity()
                                {
                                    ComplaintStatus = jsonObject.states,
                                    ComplaintStatusStep = updatelist.ComplaintStatusStep + "," + jsonObject.states,
                                    SHLCStates = "医院二次申诉待专家审核",
                                    ComplaintSecondTime = DateTime.Now,
                                    ComplaintSecondUserId = userInfo.UserId,
                                    ComplaintSecondUserName = userInfo.UserName,
                                    ComplaintInstitutionCode = userInfo.InstitutionCode,
                                    ComplaintSecondInstitutionName = userInfo.InstitutionName,
                                    ComplaintSecondDescribe = jsonObject.describe
                                }).Where(it => it.RegisterCode == registerCode).ExecuteCommand();
                                //更新从表主违规
                                foreach (var obj in jsonObject.WGInfo)
                                {
                                    var mainModel = mainlistByRegister.Where(it => it.CheckResultInfoCode == obj.Code).First();
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = obj.Value,
                                        StatesSecondFK = obj.Value
                                    }).Where(it => it.ComplaintCode == mainModel.ComplaintCode).ExecuteCommand();
                                }
                                //更新从表(处方相关)
                                foreach (var objPre in jsonObject.WGQR)
                                {
                                    db.Updateable<Check_ComplaintMain_MZLEntity>().UpdateColumns(it => new Check_ComplaintMain_MZLEntity()
                                    {
                                        ComplaintStatus = objPre.Value,
                                        StatesSecondFK = objPre.Value
                                    }).Where(it => it.ComplaintCode == objPre.Code).ExecuteCommand();
                                }
                            }
                            
                        }

                    }
                    string UploadId = db.Queryable<Check_Complain_MZLEntity>().Where(it => it.RegisterCode == registerCode).First().CheckComplainId;
                    db.Ado.CommitTran();
                    return UploadId;
                }
                catch (Exception ex)
                {
                    var error = ex.Message.ToString();
                    db.Ado.RollbackTran();
                    return "";
                }
            }
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
        public List<Check_ComplaintDetail_MZLEntity> GetImageInfo(string CheckComplainId, string datatype)
        {
            var DataResult = new List<Check_ComplaintDetail_MZLEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(CheckComplainId))
                {
                    try
                    {
                        DataResult = db.Queryable<Check_ComplaintDetail_MZLEntity>()
                       .WhereIF(!string.IsNullOrEmpty(datatype), it => it.Datatype == datatype)
                       .Where(it => it.CheckComplainId == CheckComplainId).ToList();
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                   
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }
        public List<Check_Complain_MZLEntity> GetComplaintStates(QueryCoditionByCheckResult queryCoditionByCheckResult, string states, bool isadmin, string curryydm,string ydlevel, int page, int limit, ref int totalcount)
        {
            List<Check_Complain_MZLEntity> data = new List<Check_Complain_MZLEntity>();
            ISugarQueryable<Check_Complain_MZLEntity> dataResult = null;
            ISugarQueryable<Check_Complain_MZLEntity> dataResult1 = null;
            ISugarQueryable<Check_Complain_MZLEntity> dataResult2 = null;
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult1 = db.Queryable<Check_ResultInfoMainEntity>().Where(it => it.DataType == "2")
                                        .Where(it => it.SHStates == null)   //过滤已初审数据  包括全是刚性违规的
                                        //.Where(it => it.DSHWGS != 0)
                                        .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "1", it => it.RuleLevel == "1" && it.YSHWGS == 0 && it.DSHWGS !=0)
                                        .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "2", it => it.RuleLevel == "1" && it.YSHWGS != 0 && it.DSHWGS ==0)
                                        .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "3", it => it.RuleLevel == "1" && it.YSHWGS != 0 && it.DSHWGS != 0)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name == queryCoditionByCheckResult.Name)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                                        .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                                        .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                                        .Select(a => new Check_Complain_MZLEntity
                                        {                                          
                                            PersonName = a.Name,
                                            CheckComplainId = "",
                                            RegisterCode = a.RegisterCode,
                                            PersonalCode = a.PersonalCode,
                                            InstitutionName = a.InstitutionName,
                                            DiseaseName = a.DiseaseName,
                                            YDDescription = a.ResultDescription,
                                            IdNumber = a.IdNumber,
                                            SettlementDate = a.SettlementDate,
                                            DSHWGJ = a.DSHWGJ,
                                            DSHWGS = a.DSHWGS,
                                            YSHWGJ = a.YSHWGJ,
                                            YSHWGS = a.YSHWGS,
                                            ALLMoney = a.ZWGJE,
                                            ALLCount = a.ZWGSL,
                                            YDLevel = a.YDDJ,
                                            SHLCStates = "0",
                                            InstitutionCode = a.InstitutionCode

                                        });
                dataResult2 = db.Queryable<Check_Complain_MZLEntity>()
                       .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "1", it => it.YSHWGS == 0 && it.DSHWGS !=0)
                       .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "2", it => it.YSHWGS != 0 && it.DSHWGS == 0)
                       .WhereIF(!string.IsNullOrEmpty(ydlevel) && ydlevel == "3", it => it.YSHWGS != 0 && it.DSHWGS != 0)
                       .WhereIF(!string.IsNullOrEmpty(states) && states != "22" && states != "2", it => it.ComplaintStatus == states)
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "2", it => it.ComplaintStatus == "2" && it.IsSceondFK == "0")
                       .WhereIF(!string.IsNullOrEmpty(states) && states == "22", it => it.ComplaintStatus == "2" && it.IsSceondFK == "1")
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.PersonName == queryCoditionByCheckResult.Name)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionLevel == queryCoditionByCheckResult.InstitutionLevel)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber)
                       .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, it => it.SettlementDate > queryCoditionByCheckResult.StartSettleTime)
                       .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, it => it.SettlementDate < queryCoditionByCheckResult.EndSettleTime)
                       .Select(a => new Check_Complain_MZLEntity
                       {
                           PersonName = a.PersonName,
                           CheckComplainId = a.CheckComplainId,
                           RegisterCode = a.RegisterCode,
                           PersonalCode = a.PersonalCode,
                           InstitutionName = a.InstitutionName,
                           DiseaseName = a.DiseaseName,
                           YDDescription = a.YDDescription,
                           IdNumber = a.IdNumber,
                           SettlementDate = a.SettlementDate,
                           DSHWGJ = a.DSHWGJ,
                           DSHWGS = a.DSHWGS,
                           YSHWGJ = a.YSHWGJ,
                           YSHWGS = a.YSHWGS,
                           ALLMoney = a.ALLMoney,
                           ALLCount = a.ALLCount,
                           YDLevel = a.YDLevel,
                           SHLCStates = a.ComplaintStatus,
                           InstitutionCode = a.InstitutionCode

                       });
                if (string.IsNullOrEmpty(states))
                {
                    dataResult = db.UnionAll<Check_Complain_MZLEntity>(dataResult1, dataResult2);
                }
                else if (states == "0")
                {
                    dataResult = dataResult1;
                }
                else
                {
                    dataResult = dataResult2;
                }
                if (isadmin)
                {
                    data = dataResult.ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    data = dataResult.Where(it => it.InstitutionCode == curryydm).ToPageList(page, limit, ref totalcount);
                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0, 6);
                    data = dataResult.Where(it => it.InstitutionCode.Contains(XAreaCode)).ToPageList(page, limit, ref totalcount);
                }
            }
            return data;
        }
    }
}
