using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;
using XY.Utilities;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.Service
{
    public class DecisionAnalysisService : IDecisionAnalysisService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public DecisionAnalysisService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }

        #region 获取数据
        /// <summary>
        /// 获取AllPowerfulDrug所有数据
        /// </summary>
        /// <returns></returns>
        public List<AllPowerfulDrugEntity> GetAll()
        {
            var DataResult = new List<AllPowerfulDrugEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<AllPowerfulDrugEntity>().OrderBy(it=>it.CreateDate).ToList();
            }
            return DataResult;
        }
        /// <summary>
        ///  获取左侧医院菜单
        /// </summary>
        /// <param name="level">医院等级</param>
        /// <returns></returns>
        public List<TreeModule> GetInstitutionList(string level,string year)
        {
            var DataResult = new List<TreeModule>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    }).Where((a, b) => a.DataType == "2" && b.YLJGDJMC == level)
                    .WhereIF(!string.IsNullOrEmpty(year), (a, b) => a.Year == year)
                    .GroupBy((a, b) => new { a.InstitutionCode, a.InstitutionName })
                    .OrderBy((a, b) => SqlFunc.AggregateCount(a.InstitutionCode), OrderByType.Desc)
                    .Select((a, b) => new TreeModule()
                    {
                        ID = a.InstitutionCode,
                        PID = "100",
                        NAME = a.InstitutionName,
                        RuleCode = "123",
                        Url = "213"
                    }).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取通用药名
        /// </summary>
        /// <returns></returns>
        public List<string> GetDrugName()
        {
            var DataResult=new List<string>();
            using (var db = _dbContext.GetIntance())
            {
                //DataResult = db.Queryable<AllPowerfulDrugEntity>().GroupBy(it=>it.CommonName).Select(it=>it.CommonName).ToList();
                DataResult = db.Queryable<StatisticsDrugMid>().Select(it => it.CommonName).ToList();

            }
            return DataResult;
        }
        /// <summary>
        /// 获取住院或门诊的统计
        /// </summary>
        /// <param name="flag">1住院2门诊</param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViews(string flag,string drugname)
        {           
            var dataResult = new List<StaticsViewModel>();
            //判断缓存
            string rediskey = "";
            if (flag == "1")//住院
            {
                rediskey = SystemManageConst.DRUGHOSKEY;
            }
            else
            {
                rediskey = SystemManageConst.DRUGCLINICKEY;
            }
           
          
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                //删除缓存测试用
                //redisdb.Del(rediskey);
                dataResult = redisdb.Get<List<StaticsViewModel>>(rediskey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance())
                    {
                        CheckDrugStatusEntity checkResultStatus = new CheckDrugStatusEntity() { CRowId = (flag=="1"?1:2),CheckResultStatus = "N",Flag=(flag=="1"?"HOS":"CLINIC"),FunctionDesc=(flag == "1" ? "'万能神药'住院信息获取" : "'万能神药'门诊信息获取") };
                        bool checkStatusResultStart = UpdateResultStatusNew(checkResultStatus);
                        if (!checkStatusResultStart)
                        {
                            return null;
                        }
                        if (flag == "1")//住院
                        {
                            dataResult = db.Queryable<AllPowerfulDrugEntity, YBHosPreInfoEntity>((a,  c) => new object[] {
                        JoinType.Left,a.DrugCode==c.ItemCode
                    }).GroupBy(a => a.CommonName)
                            .Where(a => drugname.Contains(a.CommonName))
                            .OrderBy((a,  c) => SqlFunc.AggregateSum(c.COUNT), OrderByType.Desc)
                            .Select((a, c) => new StaticsViewModel() { commonname = a.CommonName, count = SqlFunc.AggregateSum(c.COUNT).ToString(), price = SqlFunc.AggregateSum(c.COUNT * c.PRICE) }).ToList();
                        }
                        else
                        {
                            dataResult = db.Queryable<AllPowerfulDrugEntity,  YBClinicPreInfoEntity>((a,  c) => new object[] {
                        JoinType.Left,a.DrugCode==c.ItemCode
                    }).GroupBy(a => a.CommonName)
                            .Where(a => drugname.Contains(a.CommonName))
                            .OrderBy((a, c) => SqlFunc.AggregateSum(c.COUNT), OrderByType.Desc)
                            .Select((a,  c) => new StaticsViewModel() { commonname = a.CommonName, count = SqlFunc.AggregateSum(c.COUNT).ToString(), price = SqlFunc.AggregateSum(c.COUNT * c.PRICE) }).ToList();
                        }
                        checkResultStatus = new CheckDrugStatusEntity() { CRowId = (flag == "1" ? 1 : 2), CheckResultStatus = "Y", Flag = (flag == "1" ? "HOS" : "CLINIC"),FunctionDesc = (flag == "1" ? "'万能神药'住院信息获取" : "'万能神药'门诊信息获取") };
                        bool checkStatusResultEnd = UpdateResultStatusNew(checkResultStatus);
                        if (!checkStatusResultEnd)
                        {
                            return null;
                        }
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(rediskey, dataResult);
                        redisdb.Expire(rediskey, 86400);//设置缓存时间1天
                    }
                }
                else
                {
                    var dataResultNew = new List<StaticsViewModel>();
                    string drugnamenew = drugname;
                    //遍历缓存
                    foreach (StaticsViewModel item in dataResult)
                    {
                        if (item != null && drugname.Contains(item.commonname))
                        {
                            dataResultNew.Add(item);
                            drugnamenew=drugnamenew.Replace(item.commonname+',',"");
                        }                        
                    }
                    if (drugnamenew.Length > 0)//从数据库中取
                    {
                        using (var db = _dbContext.GetIntance())
                        {
                            CheckDrugStatusEntity checkResultStatus = new CheckDrugStatusEntity() { CRowId = (flag == "1" ? 1 : 2), CheckResultStatus = "N", Flag = (flag == "1" ? "HOS" : "CLINIC"),FunctionDesc = (flag == "1" ? "'万能神药'住院信息获取" : "'万能神药'门诊信息获取") };
                            bool checkStatusResultStart = UpdateResultStatusNew(checkResultStatus);
                            if (!checkStatusResultStart)
                            {
                                return null;
                            }
                            if (flag == "1")//住院
                            {
                                dataResult = db.Queryable<AllPowerfulDrugEntity,  YBHosPreInfoEntity>((a, c) => new object[] {
                        JoinType.Left,a.DrugCode==c.ItemCode
                    }).GroupBy(a => a.CommonName)
                                .Where(a => drugnamenew.Contains(a.CommonName))
                                .OrderBy((a,  c) => SqlFunc.AggregateSum(c.COUNT), OrderByType.Desc)
                                .Select((a, c) => new StaticsViewModel() { commonname = a.CommonName, count = SqlFunc.AggregateSum(c.COUNT).ToString(), price = SqlFunc.AggregateSum(c.COUNT * c.PRICE) }).ToList();
                            }
                            else
                            {
                                dataResult = db.Queryable<AllPowerfulDrugEntity,  YBClinicPreInfoEntity>((a,c) => new object[] {
                        JoinType.Left,a.DrugCode==c.ItemCode
                    }).GroupBy(a => a.CommonName)
                                .Where(a => drugnamenew.Contains(a.CommonName))
                                .OrderBy((a, c) => SqlFunc.AggregateSum(c.COUNT), OrderByType.Desc)
                                .Select((a,  c) => new StaticsViewModel() { commonname = a.CommonName, count = SqlFunc.AggregateSum(c.COUNT).ToString(), price = SqlFunc.AggregateSum(c.COUNT * c.PRICE) }).ToList();
                            }
                            checkResultStatus = new CheckDrugStatusEntity() { CRowId = (flag == "1" ? 1 : 2), CheckResultStatus = "Y", Flag = (flag == "1" ? "HOS" : "CLINIC"),FunctionDesc = (flag == "1" ? "'万能神药'住院信息获取" : "'万能神药'门诊信息获取") };
                            bool checkStatusResultEnd = UpdateResultStatusNew(checkResultStatus);
                            if (!checkStatusResultEnd)
                            {
                                return null;
                            }
                        }
                        dataResult.AddRange(dataResultNew);
                        if (dataResult != null)//加入缓存
                        {
                            redisdb.Del(rediskey);//先删除缓存
                            redisdb.Set(rediskey, dataResult);
                            redisdb.Expire(rediskey, 86400);//设置缓存时间1天
                        }
                    }
                    else
                    {
                        dataResult = dataResultNew;
                    }
                }
            }
            return dataResult;
        }
        /// <summary>
        /// 从中间表里获取统计数据
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="drugname"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByTable(string flag, string drugname)
        {
            string[] arr = drugname.Substring(0, drugname.Length - 1).Split(',');
            var dataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<StatisticsDrugInfo, StatisticsDrugMid>((a, b) => new object[]{
                            JoinType.Inner,a.DrugName==b.CommonName
                        }).Where((a, b) => a.flag == Convert.ToInt32(flag) && drugname.Contains(b.CommonName)).OrderBy(a => a.DrugCount, OrderByType.Desc)
                        .Select(a => new StaticsViewModel() { commonname = a.DrugName, count = a.DrugCount.ToString(), price = (decimal)a.Price }).ToList();

            }


            //判断缓存
            //string rediskey = "";
            //if (flag == "1")//门诊
            //{
            //    rediskey = SystemManageConst.DRUGCLINICKEY;
            //}
            //else
            //{
            //    rediskey = SystemManageConst.DRUGHOSKEY;
            //}
            //using (var redisdb = _redisDbContext.GetRedisIntance())
            //{
            //    //删除缓存测试用
            //   // redisdb.Del(rediskey);
            //    dataResult = redisdb.Get<List<StaticsViewModel>>(rediskey);//从缓存里取
            //    if (dataResult == null||dataResult.Count==0)
            //    {
            //        using (var db = _dbContext.GetIntance())
            //        {
            //            dataResult = db.Queryable<StatisticsDrugInfo,StatisticsDrugMid>((a,b)=>new object[]{
            //                JoinType.Inner,a.DrugName==b.CommonName
            //            }).Where((a,b) => a.flag == Convert.ToInt32(flag) && drugname.Contains(b.CommonName)).OrderBy(a => a.DrugCount, OrderByType.Desc)
            //                    .Select(a => new StaticsViewModel() { commonname = a.DrugName, count = a.DrugCount.ToString(), price = (decimal)a.Price }).ToList();

            //        }
            //        if (dataResult != null)//加入缓存
            //        {
            //            redisdb.Set(rediskey, dataResult);
            //            redisdb.Expire(rediskey, 86400);//设置缓存时间1天
            //        }
            //    }
            //    else
            //    {
            //        var dataResultNew = new List<StaticsViewModel>();
            //        string drugnamenew = drugname;
            //        //遍历缓存
            //        foreach (StaticsViewModel item in dataResult)
            //        {
            //            if (item != null && drugname.Contains(item.commonname))
            //            {
            //                dataResultNew.Add(item);
            //                drugnamenew = drugnamenew.Replace(item.commonname + ',', "");
            //            }
            //        }
            //        if (drugnamenew.Length > 0)//从数据库中取
            //        {
            //            using (var db = _dbContext.GetIntance())
            //            {
            //                dataResult = db.Queryable<StatisticsDrugInfo, StatisticsDrugMid>((a, b) => new object[]{
            //                JoinType.Inner,a.DrugName==b.CommonName
            //            }).Where((a, b) => a.flag == Convert.ToInt32(flag) && drugnamenew.Contains(b.CommonName)).OrderBy(a => a.DrugCount, OrderByType.Desc)
            //                     .Select(a => new StaticsViewModel() { commonname = a.DrugName, count = a.DrugCount.ToString(), price = (decimal)a.Price }).ToList();

            //            }
            //            dataResult.AddRange(dataResultNew);
            //            if (dataResult != null)//加入缓存
            //            {
            //                redisdb.Del(rediskey);//先删除缓存
            //                redisdb.Set(rediskey, dataResult);
            //                redisdb.Expire(rediskey, 86400);//设置缓存时间1天
            //            }
            //        }
            //        else
            //        {
            //            dataResult = dataResultNew;
            //        }
            //    }
            //}
            return dataResult;
        }


       
        /// <summary>
        /// 获取统计数据按照机构级别分组
        /// </summary>
        /// <param name="flag">1住院2门诊</param>
        /// <param name="drugname">药品名称</param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViews_JGJB(string flag, string drugname)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                if (flag == "1")//住院
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity, AllPowerfulDrugEntity, YBHosInfoEntity, YYXXEntity>((a, b, c, d) => new object[] {
                        JoinType.Inner,a.ItemCode == b.DrugCode,
                        JoinType.Left,a.HosRegisterCode==c.HosRegisterCode,
                        JoinType.Left,c.InstitutionCode==d.YYDMYYDM
                    }).GroupBy((a, b, c, d) => new { d.YLJGDJBM, d.YLJGDJMC })
                        .Where((a, b,c,d) => b.CommonName == drugname&&c.InstitutionName!=null&&c.InstitutionName.Trim()!="")
                        .OrderBy(a => SqlFunc.AggregateSum(a.COUNT), OrderByType.Desc)
                         .Select((a, b, c, d) => new StaticsViewModel() { commonname = d.YLJGDJMC, count = SqlFunc.AggregateSum(a.COUNT).ToString(), price = SqlFunc.AggregateSum(a.COUNT * a.PRICE) }).ToList();

                }
                else
                {
                    DataResult = db.Queryable<YBClinicPreInfoEntity, AllPowerfulDrugEntity, YBClinicInfoEntity, YYXXEntity>((a, b, c, d) => new object[] {
                        JoinType.Left,a.ItemCode == b.DrugCode,
                        JoinType.Left,a.ClinicRegisterCode==c.ClinicRegisterCode,
                        JoinType.Left,c.InstitutionCode==d.YYDMYYDM
                    }).GroupBy((a, b, c, d) => new { d.YLJGDJBM, d.YLJGDJMC })
                       .Where((a, b,c,d) => b.CommonName == drugname && c.InstitutionName != null && c.InstitutionName.Trim() != "")
                       .OrderBy(a => SqlFunc.AggregateSum(a.COUNT), OrderByType.Desc)
                       .Select((a, b, c, d) => new StaticsViewModel() { commonname = d.YLJGDJMC, count = SqlFunc.AggregateSum(a.COUNT).ToString(), price = SqlFunc.AggregateSum(a.COUNT *a.PRICE) }).ToList();

                }
            }
            return DataResult;
        }
        /// <summary>
        /// 从中间表里获取统计数据并按照机构级别统计
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="drugname"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByTable_JGJB(string flag, string drugname)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<StatisticsDrugList>().Where(a => a.DrugName == drugname && a.flag == Convert.ToInt32(flag))
                  .GroupBy(a => new { a.InstitutionLevelName })
                  .OrderBy(a => SqlFunc.AggregateSum(a.DrugCount), OrderByType.Desc)
                  .Select(a => new StaticsViewModel() { commonname = a.InstitutionLevelName, count = SqlFunc.AggregateSum(a.DrugCount).ToString(), price = (decimal)SqlFunc.AggregateSum(a.Price) }).ToList();
            }
            return DataResult;
        }

        /// <summary>
        /// 获取统计数据按照机构名称分组
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="drugname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViews_JGMC(string flag, string drugname,int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                
                if (flag == "1")//住院
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity, AllPowerfulDrugEntity, YBHosInfoEntity>((a, b, c) => new object[] {
                        JoinType.Left,a.ItemCode == b.DrugCode,
                        JoinType.Left,a.HosRegisterCode==c.HosRegisterCode
                    }).GroupBy((a, b, c) => new { c.InstitutionCode,c.InstitutionName })
                       .Where((a, b,c) => b.CommonName == drugname && c.InstitutionName != null && c.InstitutionName.Trim() != "")
                       .OrderBy(a => SqlFunc.AggregateSum(a.COUNT), OrderByType.Desc)
                       .Select((a, b, c) => new StaticsViewModel() { commonname =c.InstitutionName, count = SqlFunc.AggregateSum(a.COUNT).ToString(), price = SqlFunc.AggregateSum(a.COUNT * a.PRICE) })
                       .ToPageList(pageIndex, pageSize, ref totalCount);
                }
                else
                {

                    DataResult = db.Queryable<YBClinicPreInfoEntity, AllPowerfulDrugEntity, YBClinicInfoEntity>((a, b, c) => new object[] {
                        JoinType.Left,a.ItemCode == b.DrugCode,
                        JoinType.Left,a.ClinicRegisterCode==c.ClinicRegisterCode
                    }).GroupBy((a, b, c) => new { c.InstitutionCode, c.InstitutionName })
                   .Where((a, b,c) => b.CommonName == drugname && c.InstitutionName != null && c.InstitutionName.Trim() != "")
                   .OrderBy(a => SqlFunc.AggregateSum(a.COUNT), OrderByType.Desc)
                   .Select((a, b, c) => new StaticsViewModel() { commonname = c.InstitutionName, count = SqlFunc.AggregateSum(a.COUNT).ToString(), price = SqlFunc.AggregateSum(a.COUNT * a.PRICE) })
                   .ToPageList(pageIndex, pageSize, ref totalCount);
                }
            }
            return DataResult;
        }
        /// <summary>
        /// 从中间表里获取统计数据并按照机构名称统计
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="drugname"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByTable_JGMC(string flag, string drugname, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<StatisticsDrugList>().Where(a=> a.DrugName == drugname && a.flag == Convert.ToInt32(flag))
                 .OrderBy(a => a.DrugCount, OrderByType.Desc)
                 .Select(a => new StaticsViewModel() { commonname = a.InstitutionName, count = a.DrugCount.ToString(), price = (decimal)a.Price })
                 .ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        /// <summary>
        /// 获取全部药品
        /// </summary>
        /// <returns></returns>
        public List<StaticsViewModel> GetStatisticsDrugInfos()
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                    //DataResult = db.Queryable<StatisticsDrugInfo>().Where(a=>a.DrugName!=null&&a.DrugName!="")
                     //   .GroupBy(a=>a.DrugName).Select(a=>new StaticsViewModel { commonname=a.DrugName }).ToList();
                DataResult = db.Queryable<DrugDirectoryEntity>().Where(a => a.NpsDrugName != null && a.NpsDrugName != "")
                    .GroupBy(a=>a.NpsDrugName).Select(a=>new StaticsViewModel { commonname=a.NpsDrugName }).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取列表是否成功状态
        /// </summary>
        /// <param name="checkResultStatus"></param>
        /// <returns></returns>
        public bool UpdateResultStatusNew(CheckDrugStatusEntity checkResultStatusNew)
        {
            bool result = false;
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(checkResultStatusNew).Where(it => it.CRowId == checkResultStatusNew.CRowId).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }

        /// <summary>
        /// 获取列表是否成功
        /// </summary>
        /// <param name="flag">标记</param>
        /// <returns></returns>
        public CheckDrugStatusEntity GetCheckResultStatusNew(string flag)
        {
            var dataResult = new CheckDrugStatusEntity();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckDrugStatusEntity>().Where(it=>it.Flag==flag).First();
            }
            return dataResult;
        }
        /// <summary>
        /// 获取审核结果列表按照规则分组
        /// </summary>
        /// <param name="flag">标记 2住院</param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByRule(string flag,string year)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity>().GroupBy(a => new { a.RulesCode, a.RulesName })
                   .Where(a => a.DataType == flag)
                   .WhereIF(!string.IsNullOrEmpty(year),a => a.Year == year)
                   .OrderBy(a => SqlFunc.AggregateCount(a.RulesCode), OrderByType.Desc)
                   .Select(a => new StaticsViewModel() { commonname = a.RulesName, count = SqlFunc.AggregateCount(a.RulesCode).ToString() })
                   .ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取审核结果列表按照医院等级分组
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByJGJB(string flag,string year)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    }).GroupBy((a,b) => new { b.YLJGDJBM,b.YLJGDJMC })
                   .Where((a, b) => a.DataType == flag)
                   .WhereIF(!string.IsNullOrEmpty(year),(a,b) => a.Year == year)
                   .OrderBy((a, b) => SqlFunc.AggregateCount(b.YLJGDJBM), OrderByType.Desc)
                   .Select((a, b) => new StaticsViewModel() { commonname = b.YLJGDJMC, count = SqlFunc.AggregateCount(b.YLJGDJBM).ToString() })
                   .ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据规则名称获取各个医院违规人数
        /// </summary>
        /// <param name="rulename"></param>
        /// <param name="flag"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsJGMCByRule(string rulename, string flag,string year)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
               DataResult = db.Queryable<CheckResultInfoEntity>().GroupBy(a => new {a.InstitutionCode,a.InstitutionName, a.RulesCode })
                   .Where(a => a.DataType == flag &&a.RulesName==rulename)
                   .WhereIF(!string.IsNullOrEmpty(year), a => a.Year == year)
                   .OrderBy(a => SqlFunc.AggregateCount(a.InstitutionCode), OrderByType.Desc)
                   .Select(a => new StaticsViewModel() { commonname = a.InstitutionName+"|"+a.RulesCode, count = SqlFunc.AggregateCount(a.InstitutionCode).ToString() })
                   .ToList();
            }
            return DataResult;
        }

        /// <summary>
        /// 根据机构等级获取机构各个规则违规人数
        /// </summary>
        /// <param name="djname"></param>
        /// <param name="flag"></param>
        /// <param name="jgbm"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsJGMCByDJ(string djname, string flag,string jgbm,string year)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity,YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    })
                    .WhereIF(!string.IsNullOrEmpty(jgbm) && jgbm != "100", (a, b) => a.InstitutionCode == jgbm)
                    .WhereIF(!string.IsNullOrEmpty(year),(a,b) => a.Year == year)
                    .Where((a, b) => a.DataType == flag && b.YLJGDJMC == djname)
                    .GroupBy((a, b) => new { a.RulesName,a.RulesCode })
                    .OrderBy(a => SqlFunc.AggregateCount(a.RulesCode), OrderByType.Desc)
                    .Select(a => new StaticsViewModel() { rulename = a.RulesName,rulecode = a.RulesCode, count = SqlFunc.AggregateCount(a.RulesCode).ToString() })
                    .ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据机构名称和规则名称获取住院列表
        /// </summary>
        /// <param name="jgmc"></param>
        /// <param name="rulename"></param>
        /// <param name="djmc">机构等级</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBHosInfoEntityDto> GetYBHosInfoList(string year,string jgmc, string rulename,string djmc, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBHosInfoEntityDto>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<YBHosInfoEntity,CheckResultInfoEntity,YYXXEntity>((b, a,c) => new object[] {
                    JoinType.Inner,a.RegisterCode == b.HosRegisterCode &&a.PersonalCode==b.PersonalCode,
                    JoinType.Left,a.InstitutionCode == c.YYDMYYDM
                })
                .Where((b, a, c) => a.DataType == "2")
                .WhereIF(!string.IsNullOrEmpty(year), (b, a, c) => a.Year == year)
                .WhereIF(!string.IsNullOrEmpty(djmc), (b, a, c) => c.YLJGDJMC == djmc)
                .WhereIF(!string.IsNullOrEmpty(jgmc) && jgmc != "全部", (b, a, c) => a.InstitutionName == jgmc)
                .WhereIF(!string.IsNullOrEmpty(rulename), (b, a, c) => a.RulesName == rulename)
                .OrderBy((b, a, c) => b.InHosDate, OrderByType.Desc)
                .Select(
                    (b, a, c) => new YBHosInfoEntityDto()
                    {
                        CheckResultInfoCode = a.CheckResultInfoCode,
                        CityAreaCode = b.CityAreaCode,
                        CityAreaName = b.CityAreaName,
                        Year = b.Year,
                        HosRegisterCode = b.HosRegisterCode,
                        PersonalCode = b.PersonalCode,
                        IdNumber = b.IdNumber,
                        Name = b.Name,
                        Gender = b.Gender,
                        Age = b.Age,
                        PersonalTypeCode = b.PersonalTypeCode,
                        PersonalTypeName = b.PersonalTypeName,
                        FolkCode = b.FolkCode,
                        FolkName = b.FolkName,
                        VillageAreaCode = b.VillageAreaCode,
                        VillageAreaName = b.VillageAreaName,
                        InHosDate = b.InHosDate,
                        OutHosDate = b.OutHosDate,
                        InHosDay = b.InHosDay,
                        CompYear = b.CompYear,
                        ICDCode = b.ICDCode,
                        DiseaseName = b.DiseaseName,
                        InstitutionCode = b.InstitutionCode,
                        InstitutionName = b.InstitutionName,
                        InstitutionLevelCode = b.InstitutionLevelCode,
                        InstitutionLevelName = b.InstitutionLevelName,
                        InstitutiongGradeCode = b.InstitutiongGradeCode,
                        InstitutiongGradeName = b.InstitutiongGradeName,
                        ZFY = b.ZFY,
                        YBBXFY = b.YBBXFY,
                        DBBXBXFY = b.DBBXBXFY,
                        YLJZFY = b.YLJZFY,
                        SYBXBXFY = b.SYBXBXFY,
                        ZFDDJE = b.ZFDDJE,
                        QTBCJE = b.QTBCJE,
                        GRZFFY = b.GRZFFY,
                        MLNFY = b.MLNFY,
                        MLWFY = b.MLWFY,
                        XYF = b.XYF,
                        ZYF = b.ZYF,
                        CYF = b.CYF,
                        MYF = b.MYF,
                        JCF = b.JCF,
                        CLF = b.CLF,
                        TCF = b.TCF,
                        ZLF = b.ZLF,
                        HYF = b.HYF,
                        SSF = b.SSF,
                        XUEYF = b.XUEYF,
                        TJF = b.TJF,
                        TZF = b.TZF,
                        QTF = b.QTF,
                        BXYF = b.BXYF,
                        States = b.States,
                        InHosYear = b.InHosYear,
                        InHosMonth = b.InHosMonth,
                        OutHosYear = b.OutHosYear,
                        OutHosMonth = b.OutHosMonth
                    }
                 )
                .ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return dataResult;
        }
        /// <summary>
        /// 根据医疗机构等级、机构编码、规则编码获取审核结果数据并以头像列表展示
        /// </summary>
        /// <param name="yljgdj">医疗机构等级</param>
        /// <param name="institutionCode">机构编码</param>
        /// <param name="ruleCode">规则编码</param>
        /// <param name="dataType">数据类型 1门诊2住院</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CheckResultInfoDto> GetCheckResultInfosByRules(string year,string yljgdjName, string institutionName, string ruleCode,string dataType, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<CheckResultInfoDto>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, c) => new object[] {
                    JoinType.Left,a.InstitutionCode == c.YYDMYYDM
                })
                .Where((a, c) => a.DataType == dataType)
                .WhereIF(!string.IsNullOrEmpty(year), (a, c) => a.Year == year)
                .WhereIF(!string.IsNullOrEmpty(yljgdjName) && (institutionName == "全部" || string.IsNullOrEmpty(institutionName)), (a, c) => c.YLJGDJMC == yljgdjName)
                .WhereIF(!string.IsNullOrEmpty(yljgdjName) && !string.IsNullOrEmpty(institutionName) && institutionName != "全部", (a, c) => a.InstitutionName == institutionName && c.YLJGDJMC == yljgdjName)
                .WhereIF(string.IsNullOrEmpty(yljgdjName) && !string.IsNullOrEmpty(institutionName) && institutionName != "全部", (a, c) => a.InstitutionName == institutionName)
                .WhereIF(!string.IsNullOrEmpty(ruleCode), (a) => a.RulesCode == ruleCode)
                .OrderBy((a, c) => a.CheckDate, OrderByType.Desc).Select((a, c) => new CheckResultInfoDto()
                {
                    Age = a.Age,
                    CheckDate = a.CheckDate,
                    CheckResultInfoCode = a.CheckResultInfoCode,
                    DataType = a.DataType,
                    DiseaseName = a.DiseaseName,
                    Gender = a.Gender,
                    ICDCode = a.ICDCode,
                    IdNumber = a.IdNumber,
                    InstitutionCode = a.InstitutionCode,
                    InstitutionName = a.InstitutionName,
                    IsPre = a.IsPre,
                    Name = a.Name,
                    PersonalCode = a.PersonalCode,
                    RegisterCode = a.RegisterCode,
                    ResultDescription = a.ResultDescription,
                    RulesCode = a.RulesCode,
                    RulesName = a.RulesName,
                    YLJGDJBM = c.YLJGDJBM,
                    YLJGDJMC = c.YLJGDJMC
                }).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return dataResult;
        }

        #endregion

        #region 验证数据
        /// <summary>
        /// 判断药品编码是否已存在
        /// </summary>
        /// <param name="drugcode"></param>
        /// <returns></returns>
        public bool IsExsitsDrugCode(string drugcode)
        {
            int ss = 0;
            using (var db = _dbContext.GetIntance())
            {
                ss = db.Queryable<AllPowerfulDrugEntity>().Where(it=>it.DrugCode==drugcode).ToList().Count;
            }
            return ss > 0 ? true:false ;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="allpowerfuldrugEntity">实体</param>
        /// <returns></returns>
        public bool Insert(AllPowerfulDrugEntity allpowerfuldrugEntity)
        {
            using (var db = _dbContext.GetIntance())
            {                
                var count = db.Insertable(allpowerfuldrugEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 根据药品编码、药品名称和通用名称录入数据
        /// </summary>
        /// <param name="drugcode"></param>
        /// <param name="drugname"></param>
        /// <param name="commonname"></param>
        /// <returns></returns>
        public bool InsertOrUpdate(string crowid, string drugcode, string drugname, string commoname)
        {
            AllPowerfulDrugEntity allpowerfuldrugEntity = new AllPowerfulDrugEntity();
            allpowerfuldrugEntity.DrugCode = drugcode;
            allpowerfuldrugEntity.DrugName = drugname;
            allpowerfuldrugEntity.CommonName = commoname;
            allpowerfuldrugEntity.CrowId = crowid;
            allpowerfuldrugEntity.CreateDate = DateTime.Now;
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(allpowerfuldrugEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 根据通用名称录入中间表数据
        /// </summary>
        /// <param name="commonname">通用名称</param>
        /// <returns></returns>
        public string CreateDrug(string commonname)
        {
            //获取包含该名字的所有药品信息
            List<DrugDirectoryEntity> drugDirectoryEntity = new List<DrugDirectoryEntity>();
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    bool result=db.Queryable<StatisticsDrugInfo>().Any(it => it.DrugName == commonname);
                    if (!result)
                    {
                        return "-3";//选择的药品不存在与库中
                    }
                    StatisticsDrugMid statisticsDrugMid= db.Queryable<StatisticsDrugMid>().Where(a => a.CommonName == commonname).First();
                    if (statisticsDrugMid == null)//新增
                    {
                        statisticsDrugMid = new StatisticsDrugMid();
                        statisticsDrugMid.CommonName = commonname.Trim();
                        statisticsDrugMid.CrowID = Guid.NewGuid().ToString();
                        statisticsDrugMid.CreateDate = System.DateTime.Now;
                        var count = db.Insertable(statisticsDrugMid).ExecuteCommand();
                        if (count > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            return "-1";
                        }
                    }
                    else
                    {
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    return "-1";
                }


            }         

        }
        /// <summary>
        /// 根据通用名称录入中间表数据
        /// </summary>
        /// <param name="commonname"></param>
        /// <returns></returns>
        public string CreateNewDrug(string commonname)
        {
            //获取包含该名字的所有药品信息
            List<DrugDirectoryEntity> drugDirectoryEntity = new List<DrugDirectoryEntity>();
            using (var db = _dbContext.GetIntance())
            {
                try
                {                   
                    StatisticsDrugMid statisticsDrugMid = db.Queryable<StatisticsDrugMid>().Where(a => a.CommonName == commonname).First();
                    if (statisticsDrugMid == null)//新增
                    {
                        statisticsDrugMid = new StatisticsDrugMid();
                        statisticsDrugMid.CommonName = commonname;
                        statisticsDrugMid.CrowID = Guid.NewGuid().ToString();
                        statisticsDrugMid.CreateDate = System.DateTime.Now;
                        var count = db.Insertable(statisticsDrugMid).ExecuteCommand();
                        if (count > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            return "-1";
                        }
                    }
                    else
                    {
                        return "1";
                    }
                }
                catch (Exception ex)
                {
                    return "-1";
                }
            }

        }
        /// <summary>
        /// 删除药品
        /// </summary>
        /// <param name="commonname"></param>
        /// <returns></returns>
        public bool Delete(string commonname)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    var result = db.Deleteable<StatisticsDrugMid>().Where(it => it.CommonName==commonname).ExecuteCommand();
                    return result > 0 ? true:false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
