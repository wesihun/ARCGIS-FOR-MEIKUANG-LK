using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.Universal.Models.ViewModels;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.Service
{
    public class BeforeSynthesisService: IBeforeSynthesisService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public BeforeSynthesisService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        #region  数据概览
        public TotalEntity GetTotal(string yljgbh, string curryydm)
        {
            int CurrMoth = DateTime.Now.Month;   //当前月份
            var entity = new TotalEntity();
            using (var db = _dbContext.GetIntance())
            {
                //总床位数
                if (db.Queryable<BedsInformationEntity>().Where(it => it.DeleteMark == 1).Any())
                {
                    var yljgbhlist = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0).Select(it => it.YLJGBH).ToList();
                    entity.CWCount = db.Queryable<BedsInformationEntity>()
                        .Where(it => it.DeleteMark == 1 && yljgbhlist.Contains(it.YLJGBH) && it.YLJGBH == yljgbh).Count();
                }
                else
                    entity.CWCount = 0;
                //总在院人数
                if (db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Any())
                    entity.ZYCount = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Count();
                else
                    entity.ZYCount = 0;
                //门诊就诊数
                if (db.Queryable<HIS_MZDJEntity>().Where(it => it.DeleteMark == 1 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Any())
                    entity.MZJZCount = db.Queryable<HIS_MZDJEntity>().Where(it => it.DeleteMark == 1 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).GroupBy(it => it.YLJGBH).Select(it => it.SFZH).Count();
                else
                    entity.MZJZCount = 0;
                //当月门诊就诊数
                if (db.Queryable<HIS_MZDJEntity>().Where(it => it.GHRQ.Month == CurrMoth && it.YLJGBH == yljgbh).Any())
                    entity.MZJZMothCount = db.Queryable<HIS_MZDJEntity>().Where(it => it.GHRQ.Month == CurrMoth && it.YLJGBH == yljgbh).Count();
                else
                    entity.MZJZMothCount = 0;
                //总住院就诊数
                if (db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Any())
                    entity.ZYCount = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Count();
                else
                    entity.ZYCount = 0;
                //当月住院就诊数
                if (db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.SCSJ.Month == CurrMoth && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Any())
                    entity.ZYCount = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.SCSJ.Month == CurrMoth && it.YLJGBH == yljgbh).GroupBy(it => it.SFZH).Select(it => it.SFZH).Count();
                else
                    entity.ZYCount = 0;
            }
            return entity;
        }
        public List<ChartsViewModel_YYZL> GetFYLX(string yljgbh)
        {
            List<ChartsViewModel_YYZL> ruesltData = new List<ChartsViewModel_YYZL>();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<ChartsViewModel_YYZL>("SELECT XMLBMC AS Name,SUM(SJJE) AS Price FROM dbo.HIS_ZYCF WHERE YLJGBH = '" + yljgbh + "' GROUP BY XMLBBM,XMLBMC");
            }
            return ruesltData;
        }
        public List<BeforeDrugZLCharts> GetFYLX_ZQ1(string yljgbh, int page, int limit, ref int count)
        {
            List<BeforeDrugZLCharts> ruesltData = new List<BeforeDrugZLCharts>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BeforeDrugZLCharts>(@"SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by hz.KDKSBM ASC) AS RowId,kdksbm AS KSBM,hz.KDKSMC AS KSMC
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='19' AND hz2.KDKSBM=hz.KDKSBM),0)) XYF
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='20' AND hz2.KDKSBM=hz.KDKSBM),0)) ZCYF1
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='21' AND hz2.KDKSBM=hz.KDKSBM),0)) ZCYF2
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='32' AND hz2.KDKSBM=hz.KDKSBM),0)) MYF
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='28' AND hz2.KDKSBM=hz.KDKSBM),0)) JCF
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='43' AND hz2.KDKSBM=hz.KDKSBM),0)) JYF
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM='30' AND hz2.KDKSBM=hz.KDKSBM),0)) CLF
                                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(sjje) FROM HIS_ZYCF AS hz2 WHERE hz2.XMLBBM NOT IN ('19','20','21','32','28','43','30') AND hz2.KDKSBM=hz.KDKSBM),0)) QTF
                                                                    FROM HIS_ZYCF AS hz WHERE hz.YLJGBH = '" + yljgbh + "' GROUP BY kdksbm,hz.KDKSMC) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (SELECT kdksbm AS KSBM,hz.KDKSMC AS KSMC FROM HIS_ZYCF AS hz WHERE hz.YLJGBH = '" + yljgbh + "' GROUP BY kdksbm,hz.KDKSMC) tj");
            }
            return ruesltData;
        }
        public List<ZnshTYDetailEntity> GetFYLX_ZQ2(string ksbm, string yljgbh, int page, int limit, ref int count)
        {
            List<ZnshTYDetailEntity> ruesltData = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(ksbm))
                {
                    ruesltData = db.Queryable<HisZydjEntity>().Where(it => it.RYKSBM == ksbm && it.YLJGBH == yljgbh && it.DeleteMark == 1)
                    .Select(it => new ZnshTYDetailEntity
                    {
                        Name = it.HZXM,
                        Gender = it.XBMC,
                        Age = Convert.ToInt32(it.NL),
                        IdNumber = it.SFZH,
                        InHosDate = Convert.ToDateTime(it.RYRQ),
                        DiseaseName = it.ZYJBZD
                    }).ToPageList(page, limit, ref count);
                }
                else
                {
                    ruesltData = null;
                }
            }
            return ruesltData;
        }
        public List<ChartsViewModel_JZ> GetJZRC(string yljgbh, int second, int count)
        {
            var charts = new List<ChartsViewModel_JZ>();
            var CurrDateTimeDate = DateTime.Now.Date;
            int CurrHour = DateTime.Now.Hour;   //当前小时
            float timeInterval = second * 1.0f / 3600;
            using (var db = _dbContext.GetIntance())
            {
                if (db.Queryable<HisZydjEntity>().Any() && count != 0)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        if (CurrHour - i * timeInterval >= 0)
                        {
                            ChartsViewModel_JZ chartsViewModel_JZ = new ChartsViewModel_JZ();
                            chartsViewModel_JZ.Name = (CurrHour - i * timeInterval).ToString() + "-" + (CurrHour - (i - 1) * timeInterval).ToString();
                            var time1 = CurrHour - i * timeInterval;
                            var time2 = CurrHour - (i - 1) * timeInterval;
                            chartsViewModel_JZ.Count = db.Queryable<HisZydjEntity>()
                                                        .WhereIF(!string.IsNullOrEmpty(yljgbh), it => it.YLJGBH == yljgbh)
                                                        .Where(it => it.DeleteMark == 1 && it.SCSJ.Date == CurrDateTimeDate && it.SCSJ.Hour >= time1 && it.SCSJ.Hour < time2 && it.IsOutHos == 0).Count();
                            charts.Add(chartsViewModel_JZ);
                        }
                    }
                    return charts;
                }
                else
                {
                    return null;
                }
            }
        }
        public List<KSZB> GetKSZB(string yljgbh, int page, int limit, ref int count)
        {
            List<KSZB> ruesltData = new List<KSZB>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<KSZB>(@"SELECT * FROM (
                                                    SELECT ROW_NUMBER() OVER(Order by CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM AND a2.XMLBBM IN ('19','20','21','32','28','43','30'))/(SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM)*100,0)) DESC) AS RowId,a1.KDKSBM AS KSBM,a1.KDKSMC AS KSMC
                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM AND a2.XMLBBM IN ('19','20','21','32','28','43','30'))/(SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM)*100,0)) AS YZB
                                                    FROM dbo.HIS_ZYCF a1 WHERE a1.YLJGBH = '" + yljgbh + "' GROUP BY KDKSBM,KDKSMC ) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (
                                                    SELECT ROW_NUMBER() OVER(Order by CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM AND a2.XMLBBM IN ('19','20','21','32','28','43','30'))/(SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM)*100,0)) DESC) AS RowId,a1.KDKSBM AS KSBM,a1.KDKSMC AS KSMC
                                                    ,CONVERT(DECIMAL(18,2),ISNULL((SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM AND a2.XMLBBM IN ('19','20','21','32','28','43','30'))/(SELECT SUM(a2.SJJE) FROM dbo.HIS_ZYCF a2 WHERE a2.KDKSBM = a1.KDKSBM)*100,0)) AS YZB
                                                    FROM dbo.HIS_ZYCF a1 WHERE a1.YLJGBH = '" + yljgbh + "' GROUP BY KDKSBM,KDKSMC ) tj");
            }
            return ruesltData;
        }
        #endregion

        public string GetYLJGBH(string curryydm)
        {
            using (var db = _dbContext.GetIntance())
            {
                return db.Queryable<YYXXEntity>().Where(it => it.YYDMYYDM == curryydm && it.DeleteMark == 1).First().YLJGBH;
            }
        }
        #region 审核分析
        /// <summary>
        /// 获取规则违规人次数
        /// </summary>
        /// <returns></returns>
        public List<StaticsViewModel> GetGZWG(string yljgbh, int page, int limit, ref int count)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<Check_BeForeResultInfo>()
                   .Where(a => a.DataType == "2" && a.InstitutionCode == yljgbh)
                   .GroupBy(a => new { a.RulesCode, a.RulesName })
                   .OrderBy(a => SqlFunc.AggregateCount(a.RulesCode), OrderByType.Desc)
                   .Select(a => new StaticsViewModel() { commonname = a.RulesName, rulecode = a.RulesCode, count = SqlFunc.AggregateCount(a.RulesCode).ToString() })
                   .ToPageList(page, limit, ref count);
            }
            return DataResult;
        }
        /// <summary>
        /// 获取规则违规人次数钻取1  根据违规规则获取患者信息
        /// </summary>
        /// <returns></returns>
        public List<BeforeHZXX> GetGZWG_ZQ1(string yljgbh,string rulescode, int page, int limit, ref int count)
        {
            List<BeforeHZXX> ruesltData = new List<BeforeHZXX>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT * FROM (
                                                        SELECT ROW_NUMBER() OVER(Order by cbfri.RegisterCode ASC) AS RowId,cbfri.RegisterCode,MAX(hz.HZXM) Name,MAX(hz.XBMC) Sex,MAX(hz.NL) Age,MAX(hz.SFZH) IdNumber
                                                        ,MAX(hz.RYRQ) RYRQ,MAX(hz.CYRQ) CYRQ,datediff(day, MAX(hz.RYRQ),MAX(hz.CYRQ)) ZYTS,MAX(hz.ZYJBZD) ZYZD
                                                        ,(SELECT  CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SL*hz2.DJ),0)) FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode) ZFY,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='19') XYF,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='20') ZCYF1,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='21') ZCYF2,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='32') MYF,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='28') JCF,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='43') JYF,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM='30') CLF,
                                                        (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=cbfri.RegisterCode AND hz2.XMLBBM NOT IN ('19','20','21','32','28','43','30')) QTF,
                                                        CONVERT(DECIMAL(16,2),ISNULL(SUM(cbfri.[COUNT]*cbfri.Price),0))  WGFY
                                                        FROM Check_BeForeResultInfo AS cbfri left JOIN HIS_ZYDJ AS hz 
                                                        ON cbfri.RegisterCode=hz.ZYBH
                                                        WHERE cbfri.InstitutionCode = '"+ yljgbh + "' AND cbfri.RulesCode = '"+ rulescode +"' GROUP BY cbfri.RegisterCode) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (
                                        SELECT  RegisterCode
                                        FROM Check_BeForeResultInfo AS cbfri left JOIN HIS_ZYDJ AS hz 
                                        ON cbfri.RegisterCode=hz.ZYBH WHERE cbfri.InstitutionCode = '" + yljgbh + "' AND cbfri.RulesCode = '"+ rulescode + "'  GROUP BY cbfri.RegisterCode) tj");
            }
            return ruesltData;
        }
        /// <summary>
        /// 获取患者违规描述信息
        /// </summary>
        /// <returns></returns>
        public List<BeforeWGDescribe> GetWGDescribe(string yljgbh,string registercode,int page, int limit, ref int count)
        {
            var DataResult = new List<BeforeWGDescribe>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<Check_BeForeResultInfo>()
                   .Where(a => a.DataType == "2" && a.InstitutionCode == yljgbh && a.RegisterCode == registercode)                  
                   .Select(a => new BeforeWGDescribe() { Describe = a.ResultDescription})
                   .ToPageList(page, limit, ref count);
            }
            return DataResult;
        }
        /// <summary>
        /// 获取患者处方信息
        /// </summary>
        /// <returns></returns>
        public List<BeforeCF> GetCF(string yljgbh, string registercode, int page, int limit, ref int count)
        {
            var DataResult = new List<BeforeCF>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<HIS_ZYCF>()
                   .Where(a => a.YLJGBH == yljgbh && a.ZYBH == registercode)
                   .Select(a => new BeforeCF()
                   {
                       XMBM = a.YBDZXMBM,
                       XMMC = a.XMMC,
                       DJ = a.DJ,
                       Count = a.SL,
                       Price = a.DJ * a.SL,
                       SJJE = a.SJJE
                   })
                   .ToPageList(page, limit, ref count);
            }
            return DataResult;
        }
        /// <summary>
        /// 获取科室违规人数占比
        /// </summary>
        /// <returns></returns>
        public List<BesfroeKSZB> GetWGKSZB(string yljgbh, int page, int limit, ref int count)
        {
            List<BesfroeKSZB> ruesltData = new List<BesfroeKSZB>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BesfroeKSZB>(@"SELECT * FROM (
                                                            SELECT  ROW_NUMBER() OVER(ORDER BY CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),COUNT(distinct cbfri.RegisterCode))/CONVERT(DECIMAL(18,2),(SELECT COUNT(DISTINCT zybh) FROM HIS_ZYDJ AS hz2 WHERE hz2.RYKSBM=hz.RYKSBM))) DESC) AS RowId,hz.RYKSBM KSBM ,hz.RYKSMC KSMC,CONVERT(DECIMAL(18,2),CONVERT(DECIMAL(18,2),COUNT(distinct cbfri.RegisterCode))/CONVERT(DECIMAL(18,2),(SELECT COUNT(DISTINCT zybh) FROM HIS_ZYDJ AS hz2 WHERE hz2.RYKSBM=hz.RYKSBM)))*100  ZB
                                                             FROM Check_BeForeResultInfo AS cbfri LEFT JOIN HIS_ZYDJ AS hz ON cbfri.RegisterCode=hz.ZYBH WHERE cbfri.InstitutionCode = '"+ yljgbh + "' GROUP BY hz.RYKSBM,hz.RYKSMC) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(*) FROM (
                                        SELECT hz.RYKSBM KSBM
                                         FROM Check_BeForeResultInfo AS cbfri LEFT JOIN HIS_ZYDJ AS hz ON cbfri.RegisterCode=hz.ZYBH WHERE cbfri.InstitutionCode = '" + yljgbh + "' GROUP BY hz.RYKSBM,hz.RYKSMC) tj");
            }
            return ruesltData;
        }
        public List<BeforeHZXX> GetWGKSZB_ZQ1(string yljgbh,string ksbm, int page, int limit, ref int count)
        {
            List<BeforeHZXX> ruesltData = new List<BeforeHZXX>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT * FROM (
                                                            SELECT ROW_NUMBER() OVER(Order by MAX(zydj.ZYBH) ASC) AS RowId,
                                                            MAX(zydj.ZYBH) AS RegisterCode
                                                            ,MAX(zydj.HZXM) AS Name
                                                            ,MAX(zydj.XBMC) Sex
                                                            ,MAX(zydj.NL) Age
                                                            ,MAX(zydj.SFZH) IdNumber
                                                            ,MAX(zydj.RYRQ) RYRQ
                                                            ,MAX(zydj.CYRQ) CYRQ
                                                            ,datediff(day,MAX(zydj.RYRQ),MAX(zydj.CYRQ)) ZYTS
                                                            ,MAX(zydj.ZYJBZD) ZYZD
                                                            ,(SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SL*hz2.DJ),0)) FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH) ZFY,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='19') XYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='20') ZCYF1,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='21') ZCYF2,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='32') MYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='28') JCF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='43') JYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='30') CLF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM NOT IN ('19','20','21','32','28','43','30')) QTF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(result.[COUNT]*result.Price),0)) FROM dbo.Check_BeForeResultInfo AS result WHERE result.RegisterCode=zydj.ZYBH) WGFY
                                                            FROM dbo.HIS_ZYDJ zydj LEFT JOIN dbo.HIS_ZYCF cf ON zydj.ZYBH = cf.ZYBH 
                                                            WHERE zydj.RYKSBM = '" + ksbm + "' AND zydj.YLJGBH = '"+ yljgbh +"'  GROUP BY zydj.ZYBH) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (
                                        SELECT 
                                        MAX(zydj.ZYBH) AS RegisterCode
                                        FROM dbo.HIS_ZYDJ zydj LEFT JOIN dbo.HIS_ZYCF cf ON zydj.ZYBH = cf.ZYBH 
                                        WHERE zydj.RYKSBM = '" + ksbm + "' AND zydj.YLJGBH = '" + yljgbh + "' GROUP BY zydj.ZYBH) tj");
            }
            return ruesltData;
        }
        /// <summary>
        /// 获取医生违规人次数
        /// </summary>
        /// <returns></returns>
        public List<BesfroeWGYS> GetWGYS(string yljgbh, int page, int limit, ref int count)
        {
            List<BesfroeWGYS> ruesltData = new List<BesfroeWGYS>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BesfroeWGYS>(@"SELECT * FROM (
                                                             SELECT ROW_NUMBER() OVER(Order by MAX(zycf.KDKSBM) ASC) AS RowId,zycf.KDYS KDYSXM,zycf.KDKSBM KSBM,zycf.KDKSMC KSMC,COUNT(zycf.KDYS) SL
                                                             FROM dbo.Check_BeForeResultInfo result INNER JOIN dbo.HIS_ZYCF zycf ON result.RegisterCode = zycf.ZYBH WHERE result.DataType = '2' AND zycf.YLJGBH = '"+ yljgbh +"' GROUP BY zycf.KDYS,zycf.KDKSBM,zycf.KDKSMC ) tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (
                                        SELECT zycf.KDYS KDYSXM,zycf.KDKSBM KSBM,zycf.KDKSMC KSMC,COUNT(zycf.KDYS) SL
                                        FROM dbo.Check_BeForeResultInfo result INNER JOIN dbo.HIS_ZYCF zycf ON result.RegisterCode = zycf.ZYBH WHERE result.DataType = '2' AND zycf.YLJGBH = '"+ yljgbh +"' GROUP BY zycf.KDYS,zycf.KDKSBM,zycf.KDKSMC ) tj");
            }
            return ruesltData;
        }
        /// <summary>
        ///  获取违规医生人次数钻取1  根据医生获取违规患者信息
        /// </summary>
        /// <returns></returns>
        public List<BeforeHZXX> GetWGYS_ZQ1(string yljgbh,string ksbm, string doctorname, int page, int limit, ref int count)
        {
            List<BeforeHZXX> ruesltData = new List<BeforeHZXX>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT * FROM (
                                                            SELECT ROW_NUMBER() OVER(Order by MAX(zydj.ZYBH) ASC) AS RowId,
                                                            zydj.ZYBH AS RegisterCode
                                                            ,MAX(zydj.HZXM) AS Name
                                                            ,MAX(zydj.XBMC) Sex
                                                            ,MAX(zydj.NL) Age
                                                            ,MAX(zydj.SFZH) IdNumber
                                                            ,MAX(zydj.RYRQ) RYRQ
                                                            ,MAX(zydj.CYRQ) CYRQ
                                                            ,datediff(day,MAX(zydj.RYRQ),MAX(zydj.CYRQ)) ZYTS
                                                            ,MAX(zydj.ZYJBZD) ZYZD
                                                            ,(SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SL*hz2.DJ),0)) FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH) ZFY,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='19') XYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='20') ZCYF1,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='21') ZCYF2,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='32') MYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='28') JCF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='43') JYF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM='30') CLF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(hz2.SJJE),0))  FROM HIS_ZYCF AS hz2 WHERE hz2.ZYBH=zydj.ZYBH AND hz2.XMLBBM NOT IN ('19','20','21','32','28','43','30')) QTF,
                                                            (SELECT CONVERT(DECIMAL(16,2),ISNULL(SUM(result.[COUNT]*result.Price),0))  FROM dbo.Check_BeForeResultInfo AS result WHERE result.RegisterCode=zydj.ZYBH) WGFY
                                                            FROM dbo.HIS_ZYCF zycf RIGHT JOIN dbo.HIS_ZYDJ zydj ON zydj.ZYBH = zycf.ZYBH  
                                                            WHERE zycf.KDKSBM = '" + ksbm + "' AND zycf.YLJGBH = '" + yljgbh + "' AND zycf.KDYS = '" + doctorname + "' GROUP BY zydj.ZYBH )tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (
                                        SELECT 
                                        zydj.ZYBH AS RegisterCode
                                        FROM dbo.HIS_ZYCF zycf RIGHT JOIN dbo.HIS_ZYDJ zydj ON zydj.ZYBH = zycf.ZYBH  
                                        WHERE zycf.KDKSBM = '"+ ksbm +"' AND zycf.YLJGBH = '"+ yljgbh +"' AND zycf.KDYS = '"+ doctorname +"' GROUP BY zydj.ZYBH) tj");
            }
            return ruesltData;
        }
        #endregion

        #region 门诊 住院监控
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public List<KSZB> GetKSList(string yljgbh,string datatype)
        {
            List<KSZB> ruesltData = new List<KSZB>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(datatype) && datatype == "2")
                {
                    ruesltData = db.Ado.SqlQuery<KSZB>(@"SELECT RYKSBM KSMC,RYKSMC KSBM FROM dbo.HIS_ZYDJ WHERE YLJGBH = '" + yljgbh + "' GROUP BY RYKSBM,RYKSMC");
                }else if (!string.IsNullOrEmpty(datatype) && datatype == "1")
                {
                    ruesltData = db.Ado.SqlQuery<KSZB>(@"SELECT JZKSBM KSMC,JZKSMC KSBM FROM dbo.HIS_MZDJ WHERE YLJGBH = '" + yljgbh + "' GROUP BY JZKSBM,JZKSMC");
                }
                else
                {
                    ruesltData = null;
                }
            }
            return ruesltData;
        }
        /// <summary>
        /// 点击科室获取下面搜索患者信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public List<BeforeHZXX> GetPersonList1(string name,string ksbm,string yljgbh, string datatype)
        {
            List<BeforeHZXX> ruesltData = new List<BeforeHZXX>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(datatype) && datatype == "1")
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT MZBH RegisterCode,MAX(HZXM) Name,MAX(GENDER) Sex,MAX(AGE) Age FROM dbo.HIS_MZDJ WHERE 
                                                                YLJGBH = '" + yljgbh + "' AND JZKSBM = '" + ksbm + "' AND HZXM = '"+ name + "' GROUP BY MZBH");
                    }
                    else
                    {
                        ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT MZBH RegisterCode,MAX(HZXM) Name,MAX(GENDER) Sex,MAX(AGE) Age FROM dbo.HIS_MZDJ WHERE 
                                                                YLJGBH = '" + yljgbh + "' AND JZKSBM = '" + ksbm + "' GROUP BY MZBH");
                    }
                }
                else if (!string.IsNullOrEmpty(datatype) && datatype == "2")
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT ZYBH RegisterCode,MAX(HZXM) Name,MAX(XBMC) Sex,MAX(NL) Age FROM dbo.HIS_ZYDJ WHERE 
                                                               YLJGBH = '" + yljgbh + "' AND  RYKSBM = '" + ksbm + "' AND HZXM = '"+ name +"' GROUP BY ZYBH");
                    }
                    else
                    {
                        ruesltData = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT ZYBH RegisterCode,MAX(HZXM) Name,MAX(XBMC) Sex,MAX(NL) Age FROM dbo.HIS_ZYDJ WHERE 
                                                               YLJGBH = '" + yljgbh + "' AND  RYKSBM = '" + ksbm + "' GROUP BY ZYBH");
                    }
                }
                else
                {
                    ruesltData = null;
                }
            }
            return ruesltData;
        }
        /// <summary>
        /// 右侧患者信息
        /// </summary>
        /// <param name="yljgbh"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public HZJK GetPersonList2(string registercode,string ksbm, string yljgbh, string datatype)
        {
            HZJK ruesltData = new HZJK();
            ruesltData.BeforeHzxxList = new List<BeforeHZXX>();
            ruesltData.BeforeCFList = new List<BeforeCF>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(datatype) && datatype == "1")
                {
                    ruesltData.BeforeHzxxList = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT MZBH RegisterCode,MAX(HZXM) Name,MAX(GENDER) Sex,MAX(AGE) Age,MAX(MZZD) MZZD,MAX(MZZDDM) MZZDDM  FROM dbo.HIS_MZDJ WHERE 
                                                                                YLJGBH = '"+ yljgbh +"' AND MZBH = '"+ registercode +"' GROUP BY MZBH");
                    ruesltData.BeforeCFList = db.Ado.SqlQuery<BeforeCF>(@"SELECT YBDZXMBM XMBM,XMMC,DJ,SL Count,(DJ*SL) Price, SJJE FROM dbo.HIS_MZCF 
                                                                            WHERE YLJGBH = '"+ yljgbh +"' AND MZBH = '"+ registercode +"'");
                }
                else if (!string.IsNullOrEmpty(datatype) && datatype == "2")
                {
                    ruesltData.BeforeHzxxList = db.Ado.SqlQuery<BeforeHZXX>(@"SELECT ZYBH RegisterCode,MAX(HZXM) Name,MAX(XBMC) Sex,MAX(NL) Age,MAX(RYRQ) RYRQ,MAX(CWH) CWH,MAX(ZYJBZD) ZYZD,MAX(ZYJBZDDM) ZYZDDM  FROM dbo.HIS_ZYDJ WHERE 
                                                                YLJGBH = '"+ yljgbh +"' AND  RYKSBM = '"+ ksbm +"' AND ZYBH = '"+ registercode +"' GROUP BY ZYBH ");
                    ruesltData.BeforeCFList = db.Ado.SqlQuery<BeforeCF>(@"SELECT YBDZXMBM XMBM,XMMC,DJ,SL Count,(DJ*SL) Price, SJJE FROM dbo.HIS_ZYCF WHERE YLJGBH = '"+ yljgbh +"' AND ZYBH = '"+ registercode +"'");
                }
                else
                {
                    ruesltData = null;
                }
            }
            return ruesltData;
        }
        #endregion
    }
}
