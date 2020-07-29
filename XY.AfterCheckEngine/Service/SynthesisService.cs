using System;
using System.Collections.Generic;
using System.Text;
using XY.DataNS;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.ZnshBusiness.Entities;
using XY.Universal.Models.ViewModels;
using SqlSugar;
using XY.DataCache.Redis;
using XY.Universal.Models;
using XY.AfterCheckEngine.Entities.Dto;

namespace XY.AfterCheckEngine.Service
{
    /// <summary>
    /// 功能描述：SynthesisService
    /// 创 建 者：LK
    /// 创建日期：2019/8/28 15:08:05
    /// 最后修改者：LK
    /// 最后修改日期：2019/8/28 15:08:05
    /// </summary>
    public class SynthesisService : ISynthesisService
    {
        //private int CurrYear = DateTime.Now.Year;   //当前年份
        private int CurrYear = 2019;
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public SynthesisService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        public List<BedInHospitalCharts> GetKSBedInfoList(string code,int page, int limit, ref int count)
        {
            var list = new List<BedInHospitalCharts>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<BedInHospitalCharts>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by a1.KSBH ASC) AS RowId,a1.KSBH,a1.KSMC AS HospitalName," +
                    "a1.YLJGBH,a1.cws,CASE WHEN a2.YLJGBH IS NULL THEN 0 ELSE COUNT(1) END AS zys FROM(SELECT KSMC,KSBH,YLJGBH,COUNT(1) AS cws " +
                    "FROM dbo.HIS_CWXX WHERE YLJGBH = '"+ code + "' AND DeleteMark = 1 GROUP BY KSBH,KSMC,YLJGBH) a1 LEFT " +
                    "JOIN (SELECT * FROM HIS_ZYDJ WHERE DeleteMark = 1 AND IsOutHos = 0 AND YEAR(SCSJ) = '"+ CurrYear+"') a2 ON a1.KSBH = a2.RYKSBM " +
                    "AND a2.YLJGBH = '"+ code + "' GROUP BY a1.KSBH,a1.KSMC,a1.YLJGBH,a1.cws,a2.YLJGBH) AS tt WHERE tt.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT a1.KSBH,a1.KSMC AS HospitalName,a1.YLJGBH,a1.cws,COUNT(1) AS zys FROM (SELECT KSMC,KSBH,YLJGBH,COUNT(1) AS cws FROM dbo.HIS_CWXX WHERE " +
                    "YLJGBH = '"+code+"' AND DeleteMark = 1 GROUP BY KSBH,KSMC,YLJGBH) a1 LEFT JOIN dbo.HIS_ZYDJ a2 ON a1.KSBH = a2.RYKSBM WHERE a2.DeleteMark = 1 AND a2.IsOutHos = 0 AND YEAR(SCSJ) = '"+CurrYear+"' GROUP BY a1.KSBH,a1.KSMC,a1.YLJGBH,a1.cws) AS tt ");
            }
            foreach(var it in list)
            {
                if (it.cws >= it.zys)
                    it.flag = true;
                else
                    it.flag = false;
            }
            return list;
        }
        public List<BedInHospitalCharts> GetBedInHospitalCharts(int hoscount)
        {
            var list = new List<BedInHospitalCharts>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<BedInHospitalCharts>("SELECT TOP " + hoscount + "tt.HospitalName,SUM(tt.ZYPersonCount) AS ZYPersonCount,SUM(tt.CWCount) AS CWCount FROM (" +
                    "SELECT a2.YLJGMC AS HospitalName,COUNT(a1.YLJGBH) AS ZYPersonCount,0 AS  CWCount FROM HIS_ZYDJ a1 LEFT JOIN HIS_YYXX a2 " +
                    "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1  AND YEAR(a1.SCSJ) = '" + CurrYear + "' AND a2.DeleteMark = 1 AND a1.IsOutHos = 0 AND a1.YLJGBH IN (" +
                    "SELECT YLJGBH FROM HIS_CWXX) GROUP BY a1.SFZH,a1.YLJGBH,a2.YLJGMC " +
                    "UNION ALL SELECT a2.YLJGMC AS HospitalName,0 AS ZYPersonCount,COUNT(*) AS  CWCount FROM  HIS_CWXX a1 LEFT JOIN HIS_YYXX a2 " +
                    "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND a2.DeleteMark = 1 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_ZYDJ where IsOutHos = 0 AND YEAR(SCSJ) = '" + CurrYear + "') GROUP BY a1.YLJGBH,a2.YLJGMC" +
                    ")tt GROUP BY tt.HospitalName ORDER BY SUM(tt.ZYPersonCount) DESC");                            
            }
            return list;
        }
        
        public List<BedInHospitalCharts> GetBedInHospitalList(string name, int page, int limit, ref int count)
        {
            var list = new List<BedInHospitalCharts>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();          
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(name))
                {
                    list = db.Ado.SqlQuery<BedInHospitalCharts>("SELECT * FROM (" +
                   "SELECT tt.YLJGBH,tt.HospitalName,SUM(tt.ZYPersonCount) AS ZYPersonCount,SUM(tt.CWCount) AS CWCount,ROW_NUMBER() OVER(Order by SUM(tt.ZYPersonCount) DESC) AS RowId  FROM (" +
                   "SELECT a1.YLJGBH,a2.YLJGMC AS HospitalName,COUNT(a1.YLJGBH) AS ZYPersonCount,0 AS  CWCount FROM HIS_ZYDJ a1 LEFT JOIN HIS_YYXX a2 " +
                   "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND YEAR(a1.SCSJ) = '"+ CurrYear + "' AND a2.DeleteMark = 1 AND a1.IsOutHos = 0 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_CWXX) GROUP BY a1.SFZH,a1.YLJGBH,a2.YLJGMC " +
                   "UNION ALL SELECT a1.YLJGBH,a2.YLJGMC AS HospitalName,0 AS ZYPersonCount,COUNT(*) AS  CWCount FROM  HIS_CWXX a1 LEFT JOIN HIS_YYXX a2 ON " +
                   "a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND a2.DeleteMark = 1 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_ZYDJ where IsOutHos = 0 AND YEAR(SCSJ) = '" + CurrYear + "') GROUP BY a1.YLJGBH,a2.YLJGMC)tt " +
                   "GROUP BY tt.HospitalName,tt.YLJGBH) AS b WHERE b.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt("SELECT COUNT(1) FROM(SELECT tt.HospitalName,SUM(tt.ZYPersonCount) AS ZYPersonCount,SUM(tt.CWCount) AS CWCount FROM (" +
                       "SELECT a2.YLJGMC AS HospitalName,COUNT(a1.YLJGBH) AS ZYPersonCount,0 AS  CWCount FROM HIS_ZYDJ a1 LEFT JOIN HIS_YYXX a2 " +
                       "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND YEAR(a1.SCSJ) = '" + CurrYear + "' AND a2.DeleteMark = 1 AND a1.IsOutHos = 0 AND a1.YLJGBH IN (" +
                       "SELECT YLJGBH FROM HIS_CWXX) GROUP BY a1.SFZH,a1.YLJGBH,a2.YLJGMC " +
                       "UNION ALL SELECT a2.YLJGMC AS HospitalName,0 AS ZYPersonCount,COUNT(*) AS  CWCount FROM  HIS_CWXX a1 LEFT JOIN HIS_YYXX a2 " +
                       "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND a2.DeleteMark = 1 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_ZYDJ where IsOutHos = 0 AND YEAR(SCSJ) = '" + CurrYear + "') GROUP BY a1.YLJGBH,a2.YLJGMC" +
                       ")tt GROUP BY tt.HospitalName) xx");
                }
                else
                {
                    string query = " AND b.HospitalName LIKE '%" + name + "%'";
                    list = db.Ado.SqlQuery<BedInHospitalCharts>("SELECT * FROM (" +
                      "SELECT tt.HospitalName,SUM(tt.ZYPersonCount) AS ZYPersonCount,SUM(tt.CWCount) AS CWCount,ROW_NUMBER() OVER(Order by SUM(tt.ZYPersonCount) DESC) AS RowId  FROM (" +
                      "SELECT a2.YLJGMC AS HospitalName,COUNT(a1.YLJGBH) AS ZYPersonCount,0 AS  CWCount FROM HIS_ZYDJ a1 LEFT JOIN HIS_YYXX a2 " +
                      "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND YEAR(a1.SCSJ) = '" + CurrYear + "' AND a2.DeleteMark = 1 AND a1.IsOutHos = 0 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_CWXX) GROUP BY a1.SFZH,a1.YLJGBH,a2.YLJGMC " +
                      "UNION ALL SELECT a2.YLJGMC AS HospitalName,0 AS ZYPersonCount,COUNT(*) AS  CWCount FROM  HIS_CWXX a1 LEFT JOIN HIS_YYXX a2 ON " +
                      "a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND a2.DeleteMark = 1 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_ZYDJ where IsOutHos = 0 AND YEAR(SCSJ) = '" + CurrYear + "') GROUP BY a1.YLJGBH,a2.YLJGMC)tt " +
                      "GROUP BY tt.HospitalName) AS b WHERE b.RowId BETWEEN " + bt1 + " AND " + bt2 + query);

                    count = db.Ado.GetInt("SELECT COUNT(1) FROM(SELECT tt.HospitalName,SUM(tt.ZYPersonCount) AS ZYPersonCount,SUM(tt.CWCount) AS CWCount FROM (" +
                       "SELECT a2.YLJGMC AS HospitalName,COUNT(a1.YLJGBH) AS ZYPersonCount,0 AS  CWCount FROM HIS_ZYDJ a1 LEFT JOIN HIS_YYXX a2 " +
                       "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND YEAR(a1.SCSJ) = '" + CurrYear + "' AND a2.DeleteMark = 1 AND a1.IsOutHos = 0 AND a1.YLJGBH IN (" +
                       "SELECT YLJGBH FROM HIS_CWXX) GROUP BY a1.SFZH,a1.YLJGBH,a2.YLJGMC " +
                       "UNION ALL SELECT a2.YLJGMC AS HospitalName,0 AS ZYPersonCount,COUNT(*) AS  CWCount FROM  HIS_CWXX a1 LEFT JOIN HIS_YYXX a2 " +
                       "ON a1.YLJGBH = a2.YLJGBH WHERE a1.DeleteMark = 1 AND a2.DeleteMark = 1 AND a1.YLJGBH IN (SELECT YLJGBH FROM HIS_ZYDJ where IsOutHos = 0 AND YEAR(SCSJ) = '" + CurrYear + "') GROUP BY a1.YLJGBH,a2.YLJGMC" +
                       ")tt where tt.HospitalName like '%"+ name+"%' GROUP BY tt.HospitalName) xx");
                }
                foreach(var it in list)
                {
                    if (it.ZYPersonCount > it.CWCount)
                        it.flag = false;
                    else
                        it.flag = true;
                }
            }
            return list;
        }
        public List<DrugZLCharts> GetDrugZLCharts(int hoscount)
        {
            var list = new List<DrugZLCharts>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<DrugZLCharts>("SELECT TOP "+ hoscount + " InstitutionName AS HospitalName,InstitutionCode,cast(SUM(ZYF)/10000 as decimal(18,2)) AS Price1," +
                    "CAST(SUM(ZFY)/10000 as decimal(18,2)) AS ZFY,cast(SUM(XYF)/10000 as decimal(18,2)) AS Price2,cast(SUM(CYF)/10000 as decimal(18,2)) AS Price3,cast(SUM(MYF)/10000 as decimal(18,2)) " +
                    "AS Price4,CAST(SUM(JCF)/10000 as decimal(18,2)) AS JCF,CAST(SUM(HYF)/10000 as decimal(18,2)) AS HYF," +
                    "CAST(SUM(TJF)/10000 as decimal(18,2)) AS TJF,CAST(SUM(CLF)/10000 as decimal(18,2)) AS CLF,CAST(SUM(ZLF)/10000 as decimal(18,2)) AS ZLF,CAST((SUM(TCF) + SUM(SSF)+" +
                    " SUM(XUEYF)+ SUM(TZF)+ SUM(QTF))/10000 as decimal(18,2)) AS QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND InstitutionCode <> '' AND CompYear = '" + CurrYear +"' AND InstitutionName <> '' " +
                    "GROUP BY InstitutionCode,InstitutionName ORDER BY ZFY DESC");              
            }
            return list;
        }
        public List<DrugZLCharts> GetDrugZLList(int page, int limit, ref int count)
        {
            var list = new List<DrugZLCharts>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<DrugZLCharts>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by InstitutionCode ASC) AS RowId,InstitutionName AS HospitalName," +
                    "InstitutionCode,CAST(SUM(ZFY)/10000 as decimal(18,2)) AS ZFY,cast(SUM(ZYF)/10000 as decimal(18,2)) AS Price1,CONVERT(decimal(18,2),((sum(ZYF)/sum(ZFY))*100)) " +
                    "AS Price1p,cast(SUM(XYF)/10000 as decimal(18,2)) AS Price2,CONVERT(decimal(18,2),((sum(XYF)/sum(ZFY))*100)) AS Price2p,cast(SUM(CYF)/10000 as decimal(18,2)) AS Price3," +
                    "CONVERT(decimal(18,2),((sum(CYF)/sum(ZFY))*100)) AS Price3p,cast(SUM(MYF)/10000 as decimal(18,2)) AS Price4,CONVERT(decimal(18,2),((sum(MYF)/sum(ZFY))*100)) AS Price4p,CAST(SUM(JCF)/10000 as decimal(18,2)) AS JCF," +
                    "CONVERT(decimal(18,2),((sum(JCF)/sum(ZFY))*100)) AS JCFp,CAST(SUM(HYF)/10000 as decimal(18,2)) AS HYF,CONVERT(decimal(18,2),((sum(HYF)/sum(ZFY))*100)) AS HYFp,CAST(SUM(TJF)/10000 as decimal(18,2)) AS TJF," +
                    "CONVERT(decimal(18,2),((sum(TJF)/sum(ZFY))*100)) AS TJFp,CAST(SUM(CLF)/10000 as decimal(18,2)) AS CLF,CONVERT(decimal(18,2),((sum(CLF)/sum(ZFY))*100)) AS CLFp,CAST(SUM(ZLF)/10000 as decimal(18,2)) AS ZLF,CONVERT(decimal(18,2),((sum(ZLF)/sum(ZFY))*100)) AS ZLFp," +
                    "CAST((SUM(TCF) + SUM(SSF)+ SUM(XUEYF)+ SUM(TZF)+ SUM(QTF))/10000 as decimal(18,2)) AS QTF,CONVERT(decimal(18,2),((SUM(TCF) + SUM(SSF)+ SUM(XUEYF)+ SUM(TZF)+ SUM(QTF))/SUM(ZFY)*100)) AS QTFp FROM YB_HosInfo " +
                    "WHERE CompYear = '" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName <> '' AND InstitutionCode <> '' GROUP BY InstitutionCode,InstitutionName) AS tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionCode FROM YB_HosInfo WHERE CompYear ='" + CurrYear + "' and  InstitutionLevelCode < 5  AND InstitutionName <> '' GROUP BY InstitutionCode,InstitutionName) AS tj");
            }
            return list;
        }
        public List<ZnshTYDetailEntity> GetZQDrugZLList(string InstitutionCode, int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        ZYF = it.ZYF,
                        XYF = it.XYF,
                        CYF = it.CYF,
                        MYF = it.MYF,
                        JCF = it.JCF,
                        HYF = it.HYF,
                        TJF = it.TJF,
                        CLF = it.CLF,
                        ZLF = it.ZLF,
                        TCF = it.TCF,
                        SSF = it.SSF,
                        XUEYF = it.XUEYF,
                        TZF = it.TZF,
                        QTF = it.QTF,
                        MLNFY = it.MLNFY,
                        MLWFY = it.MLWFY
                    }).ToPageList(page, limit, ref count);
            }
            foreach(var it in list)
            {
                it.YPFY = it.ZYF + it.XYF + it.CYF + it.MYF;
                it.ZLFY = it.JCF + it.HYF + it.TJF + it.CLF + it.ZLF + it.TCF + it.SSF + it.XUEYF + it.TZF + it.QTF;
                it.OtherFY = it.TCF + it.SSF + it.XUEYF + it.TZF + it.QTF;
                it.YPFYp = Math.Round(Convert.ToDecimal((it.YPFY / it.ZFY).ToString()),4) * 100; 
                it.ZYFp = Math.Round(Convert.ToDecimal((it.ZYF / it.ZFY).ToString()), 4) * 100;
                it.XYFp = Math.Round(Convert.ToDecimal((it.XYF / it.ZFY).ToString()), 4) * 100;
                it.CYFp = Math.Round(Convert.ToDecimal((it.CYF / it.ZFY).ToString()), 4) * 100;
                it.MYFp = Math.Round(Convert.ToDecimal((it.MYF / it.ZFY).ToString()), 4) * 100;
                it.ZLFYp = Math.Round(Convert.ToDecimal((it.ZLF / it.ZFY).ToString()),4) * 100;
                it.JCFp = Math.Round(Convert.ToDecimal((it.JCF / it.ZFY).ToString()), 4) * 100;
                it.HYFp = Math.Round(Convert.ToDecimal((it.HYF / it.ZFY).ToString()), 4) * 100;
                it.TJFp = Math.Round(Convert.ToDecimal((it.TJF / it.ZFY).ToString()), 4) * 100;
                it.CLFp = Math.Round(Convert.ToDecimal((it.CLF / it.ZFY).ToString()), 4) * 100;
                it.ZLFp = Math.Round(Convert.ToDecimal((it.ZLF / it.ZFY).ToString()), 4) * 100;
                it.OtherFYp = Math.Round(Convert.ToDecimal((it.OtherFY / it.ZFY).ToString()), 4) * 100;
            }
            return list;
        }
        public List<ZnshTYDetailEntity> GetZQDiseaseListByName(string InstitutionCode,string diseasename, int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                var icdcodelist = db.Queryable<DiseaseDirectoryEntity>().Where(it => it.UserDisName == diseasename && it.ICDCode != "").Select(it => it.ICDCode).ToList();
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Where(it => icdcodelist.Contains(it.ICDCode))
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        ZYF = it.ZYF,
                        XYF = it.XYF,
                        CYF = it.CYF,
                        MYF = it.MYF,
                        JCF = it.JCF,
                        HYF = it.HYF,
                        TJF = it.TJF,
                        CLF = it.CLF,
                        ZLF = it.ZLF,
                        TCF = it.TCF,
                        SSF = it.SSF,
                        XUEYF = it.XUEYF,
                        TZF = it.TZF,
                        QTF = it.QTF,
                        MLNFY = it.MLNFY,
                        MLWFY = it.MLWFY
                    }).ToPageList(page, limit, ref count);
            }
            foreach (var it in list)
            {
                it.YPFY = it.ZYF + it.XYF + it.CYF + it.MYF;
                it.ZLFY = it.JCF + it.HYF + it.TJF + it.CLF + it.ZLF + it.TCF + it.SSF + it.XUEYF + it.TZF + it.QTF;
                it.OtherFY = it.TCF + it.SSF + it.XUEYF + it.TZF + it.QTF;
                it.YPFYp = Math.Round(Convert.ToDecimal((it.YPFY / it.ZFY).ToString()), 4) * 100;
                it.ZYFp = Math.Round(Convert.ToDecimal((it.ZYF / it.ZFY).ToString()), 4) * 100;
                it.XYFp = Math.Round(Convert.ToDecimal((it.XYF / it.ZFY).ToString()), 4) * 100;
                it.CYFp = Math.Round(Convert.ToDecimal((it.CYF / it.ZFY).ToString()), 4) * 100;
                it.MYFp = Math.Round(Convert.ToDecimal((it.MYF / it.ZFY).ToString()), 4) * 100;
                it.ZLFYp = Math.Round(Convert.ToDecimal((it.ZLF / it.ZFY).ToString()), 4) * 100;
                it.JCFp = Math.Round(Convert.ToDecimal((it.JCF / it.ZFY).ToString()), 4) * 100;
                it.HYFp = Math.Round(Convert.ToDecimal((it.HYF / it.ZFY).ToString()), 4) * 100;
                it.TJFp = Math.Round(Convert.ToDecimal((it.TJF / it.ZFY).ToString()), 4) * 100;
                it.CLFp = Math.Round(Convert.ToDecimal((it.CLF / it.ZFY).ToString()), 4) * 100;
                it.ZLFp = Math.Round(Convert.ToDecimal((it.ZLF / it.ZFY).ToString()), 4) * 100;
                it.OtherFYp = Math.Round(Convert.ToDecimal((it.OtherFY / it.ZFY).ToString()), 4) * 100;
            }
            return list;
        }
        public List<ChartsViewModel_ZY> GetTCZYCJList(string levelcode, string hosname, decimal cjfy, int page, int limit, ref int count)
        {
            var list = new List<ChartsViewModel_ZY>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(levelcode))
                {
                    if (!string.IsNullOrEmpty(hosname))
                    {
                        list = db.Ado.SqlQuery<ChartsViewModel_ZY>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) DESC) AS RowId,InstitutionName AS HosName,InstitutionCode," +
                            "CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) AS CJFY FROM dbo.YB_HosInfo WHERE InstitutiongGradeCode = '" + levelcode + "' AND InstitutionLevelCode < 5 and InstitutionCode <> '' AND CompYear = '" + CurrYear + "' GROUP BY InstitutionCode,InstitutionName " +
                            "HAVING CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) > '" + cjfy + "' AND InstitutionName like '%"+ hosname+"%') AS tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                        count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS HosName," +
                            "CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) AS CJFY FROM dbo.YB_HosInfo WHERE InstitutiongGradeCode = '" + levelcode + "' AND InstitutionLevelCode < 5 and InstitutionCode <> '' AND CompYear = '" + CurrYear + "' GROUP BY InstitutionCode,InstitutionName " +
                            "HAVING CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) > '" + cjfy + "' AND InstitutionName like '%" + hosname + "%') AS tj");
                    }
                    else
                    {
                        list = db.Ado.SqlQuery<ChartsViewModel_ZY>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) DESC) AS RowId,InstitutionName AS HosName,InstitutionCode," +
                            "CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) AS CJFY FROM dbo.YB_HosInfo WHERE InstitutiongGradeCode = '" + levelcode + "' AND InstitutionLevelCode < 5 and InstitutionCode <> '' AND CompYear = '" + CurrYear +"' GROUP BY InstitutionCode,InstitutionName " +
                            "HAVING CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) > '"+ cjfy + "') AS tj WHERE tj.RowId BETWEEN "+ bt1 +" AND " + bt2);
                        count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS HosName," +
                            "CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) AS CJFY FROM dbo.YB_HosInfo WHERE InstitutiongGradeCode = '" + levelcode + "' AND InstitutionLevelCode < 5 and InstitutionCode <> '' AND CompYear = '" + CurrYear + "' GROUP BY InstitutionCode,InstitutionName " +
                            "HAVING CONVERT(DECIMAL(18,2),((SUM(ZFY)/COUNT(InstitutionCode)))) > '" + cjfy + "') AS tj");
                    }
                }
            }
            return list;
         }
        public ChartsViewModel GetManayCharts()
        {
            var chartsData = new ChartsViewModel();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                chartsData = redisdb.Get<ChartsViewModel>(SystemManageConst.MANAY_CHARTS);//从缓存里取
                if (chartsData == null)
                {
                    chartsData = new ChartsViewModel();
                    ChartsViewModel_ZY chartsViewModel_ZY = new ChartsViewModel_ZY();
                    chartsData.chartsViewModel_YYZL = new List<ChartsViewModel_YYZL>();
                    using (var db = _dbContext.GetIntance())
                    {
                        chartsData.chartsViewModel_YYZL = db.Ado.SqlQuery<ChartsViewModel_YYZL>("SELECT '西药费' AS Name, cast(SUM(XYF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS xy " +
                            "WHERE xy.InstitutionLevelCode < 5 AND xy.CompYear = '" + CurrYear + "'UNION ALL SELECT '中药费' AS Name, cast(SUM(ZYF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS zy WHERE zy.InstitutionLevelCode < 5 AND zy.CompYear = '" + CurrYear + "'UNION ALL SELECT '草药费' AS Name, " +
                            "cast(SUM(CYF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS cy WHERE cy.CompYear = '" + CurrYear + "'UNION ALL SELECT '蒙药费' AS Name, cast(SUM(MYF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo " +
                            "AS my WHERE my.InstitutionLevelCode < 5 AND my.CompYear = '" + CurrYear + "' UNION ALL SELECT '检查费' AS Name, cast(SUM(JCF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS jc" +
                            " WHERE jc.InstitutionLevelCode < 5 AND jc.CompYear = '" + CurrYear + "' UNION ALL SELECT '化验费' AS Name, cast(SUM(HYF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS hy WHERE hy.InstitutionLevelCode < 5 AND hy.CompYear = '" + CurrYear + "' UNION ALL " +
                            "SELECT '特检费' AS Name, cast(SUM(TJF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS tj WHERE tj.InstitutionLevelCode < 5 AND tj.CompYear = '" + CurrYear + "'UNION ALL SELECT '材料费' AS Name, " +
                            "cast(SUM(CLF)/10000 as decimal(18,2)) AS Price FROM YB_HosInfo AS cl WHERE cl.InstitutionLevelCode < 5 AND cl.CompYear = '" + CurrYear + "'UNION ALL SELECT '治疗费' AS Name, cast(SUM(ZLF)/10000 as decimal(18,2)) " +
                            "AS Price FROM YB_HosInfo AS zl WHERE zl.InstitutionLevelCode < 5 AND zl.CompYear = '" + CurrYear + "'UNION ALL SELECT '其他费' AS Name, cast(SUM(QTF)/10000 as decimal(18,2)) AS Price FROM(SELECT SUM(TCF) QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND" +
                            " CompYear = '" + CurrYear + "' UNION ALL SELECT SUM(SSF) QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND CompYear = '" + CurrYear + "'UNION ALL SELECT SUM(XUEYF) QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND CompYear = '" + CurrYear + "'UNION ALL SELECT SUM(TZF) " +
                            "QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND CompYear = '" + CurrYear + "'UNION ALL SELECT SUM(QTF) QTF FROM YB_HosInfo WHERE InstitutionLevelCode < 5 AND CompYear = '" + CurrYear + "') AS qt ");                       
                        chartsData.chartsViewModel_ZY = db.Ado.SqlQuery<ChartsViewModel_ZY>("SELECT * FROM TJ_YYJBCJFY");
                        if (chartsData != null)
                        {
                            redisdb.Set(SystemManageConst.MANAY_CHARTS, chartsData);
                            redisdb.Expire(SystemManageConst.MANAY_CHARTS, 86400);//设置缓存时间
                        }
                        return chartsData;
                    }
                }
                else
                {
                    return chartsData;
                }
            }
        }
        public List<Map_XY> GetMapInfo()
        {
            var Map_XY = new List<Map_XY>();
            using (var db = _dbContext.GetIntance())
            {
                Map_XY = db.Ado.SqlQuery<Map_XY>("SELECT t1.Code,t1.Name,t1.X,t1.Y,ISNULL(t2.CWS,0) AS CWS,ISNULL(t2.ZYS,0) AS ZYS,ISNULL(t2.YLJGBH,0) AS YLJGBH " +
                   "FROM (SELECT YLJGBH AS Code,YLJGMC AS Name,X,Y from HIS_YYXX WHERE YYDMYYDM IN (select distinct InstitutionCode from YB_HosInfo " +
                   "WHERE InstitutionLevelCode < 5 AND InHosYear = '" + CurrYear + "' AND x <> '' AND y <> '')) AS t1 LEFT JOIN (SELECT a1.YLJGBH,a1.KSBH,a1.CWS,COUNT(1) AS ZYS FROM " +
                   "(SELECT YLJGBH,KSBH,COUNT(1) AS CWS FROM dbo.HIS_CWXX GROUP BY YLJGBH,KSBH) a1 LEFT JOIN dbo.HIS_ZYDJ a2 ON a1.YLJGBH = a2.YLJGBH " +
                   "WHERE a1.KSBH = a2.RYKSBM AND a2.IsOutHos = 0 AND a2.DeleteMark = 1 AND YEAR(a2.SCSJ) = '" + CurrYear + "' GROUP BY a1.YLJGBH,a1.KSBH,a1.CWS) t2 ON t1.Code = t2.YLJGBH");
                foreach (var it in Map_XY)
                {
                    if (it.ZYS > it.CWS)
                        it.Flag = false;
                    else
                        it.Flag = true;
                }
            }
            return Map_XY;
        }
        public List<Map_XY> GetMapXY()
        {
            var Map_XY = new List<Map_XY>();
            using (var db = _dbContext.GetIntance())
            {
                Map_XY = db.Ado.SqlQuery<Map_XY>("select  YYDMYYDM AS Code,YLJGMC AS Name,X,Y from HIS_YYXX WHERE YYDMYYDM IN " +
                   "(select distinct InstitutionCode from YB_HosInfo WHERE  InstitutionLevelCode < 5 AND InHosYear = '" + CurrYear + "') AND X <> '' and Y <> ''");              
            }
            return Map_XY;
        }
        public Map_YYXXInfo GetMapYYXXInfo(string code)
        {
            Map_YYXXInfo Map_YYXXInfo = new Map_YYXXInfo();
            using (var db = _dbContext.GetIntance())
            {
                Map_YYXXInfo.ZYPersonCount = db.Queryable<HisZydjEntity>().Where(it => it.YLJGBH == code && it.DeleteMark == 1 && it.IsOutHos == 0 && it.SCSJ.Year == CurrYear).GroupBy(it => it.SFZH).Select(it => it.SFZH).Count();
                Map_YYXXInfo.CWCount = db.Queryable<BedsInformationEntity>().Where(it => it.YLJGBH == code && it.DeleteMark == 1).Count();
            }
            return Map_YYXXInfo;
        }

        public List<TJ_JBPMTJEntity> GetDiseaseList(string name, int page, int limit, ref int count)
        {
            var data = new List<TJ_JBPMTJEntity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<TJ_JBPMTJEntity>()
                    .WhereIF(!string.IsNullOrEmpty(name), it => it.DiseaseName.Contains(name))
                    .ToPageList(page, limit, ref count);
            }
            return data;
         }
        public List<TJ_QSXYYLEntity> GetXYList(string name, int page, int limit, ref int count)
        {
            var data = new List<TJ_QSXYYLEntity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<TJ_QSXYYLEntity>()
                    .WhereIF(!string.IsNullOrEmpty(name), it => it.YPMC.Contains(name))
                    .ToPageList(page, limit, ref count);                                                     
            }
            return data;
        }
        public List<ZFFY> GetZFFYList(string name, int page, int limit, ref int count)
        {
            var data = new List<ZFFY>();
            string bt1 = ((page - 1) * limit +1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    data = db.Ado.SqlQuery<ZFFY>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by SUM(MLWFY) desc) AS RowId,InstitutionName AS Name,InstitutionCode,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price " +
                    "FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName like '%" + name + "%'  GROUP BY InstitutionCode,InstitutionName) AS tj " +
                    "WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS Name,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "' and  InstitutionLevelCode < 5  AND InstitutionName like '%" + name + "%' GROUP BY InstitutionCode,InstitutionName) AS tj");
                }
                else
                {
                    data = db.Ado.SqlQuery<ZFFY>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by SUM(MLWFY) desc) AS RowId,InstitutionName AS Name,InstitutionCode,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price " +
                    "FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName <> ''  GROUP BY InstitutionCode,InstitutionName) AS tj " +
                    "WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS Name,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "' and  InstitutionLevelCode < 5  AND InstitutionName <> ''  GROUP BY InstitutionCode,InstitutionName) AS tj");
                }             
            }
            return data;
        }
        public List<JCJYF> GetJCJYList(string name, int page, int limit, ref int count)
        {
            var data = new List<JCJYF>();
            string bt1 = ((page - 1) * limit +1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(name))
                {
                    data = db.Ado.SqlQuery<JCJYF>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by SUM(JCF)+SUM(HYF)+SUM(TJF) DESC) AS RowId,InstitutionName AS Name,InstitutionCode,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS Price " +
                    "FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName like '%" + name + "%'  GROUP BY InstitutionCode,InstitutionName) AS tj " +
                    "WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS Name,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName like '%" + name + "%'  GROUP BY InstitutionCode,InstitutionName) AS tj");
                }
                else
                {
                    data = db.Ado.SqlQuery<JCJYF>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by SUM(JCF)+SUM(HYF)+SUM(TJF) DESC) AS RowId,InstitutionName AS Name,InstitutionCode,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS Price " +
                    "FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName <> ''  GROUP BY InstitutionCode,InstitutionName) AS tj " +
                    "WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionName AS Name,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "' and  InstitutionLevelCode < 5 AND InstitutionName <> ''  GROUP BY InstitutionCode,InstitutionName) AS tj");
                }             
            }
            return data;
        }

        public RankEntity GetRank()
        {
            var rankEntity = new RankEntity();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                rankEntity = redisdb.Get<RankEntity>(SystemManageConst.RANK);//从缓存里取
                if (rankEntity == null)
                {
                    rankEntity = new RankEntity();
                    rankEntity.disease = new List<Disease>();
                    rankEntity.drug_xy = new List<Drug_XY>();
                    rankEntity.zffy = new List<ZFFY>();
                    rankEntity.jcjyf = new List<JCJYF>();
                    using (var db = _dbContext.GetIntance())
                    {
                        rankEntity.disease = db.Ado.SqlQuery<Disease>("SELECT TOP 5 * FROM TJ_JBPMTJ ORDER BY CJFY DESC");
                        rankEntity.drug_xy = db.Ado.SqlQuery<Drug_XY>("SELECT TOP 5 * from TJ_QSXYYL order by SL desc");
                        rankEntity.zffy = db.Ado.SqlQuery<ZFFY>("SELECT TOP 5 InstitutionName AS Name,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='"+ CurrYear + "' AND  InstitutionLevelCode < 5 and InstitutionCode <> ''  GROUP BY InstitutionCode,InstitutionName ORDER BY Price DESC");
                        rankEntity.jcjyf = db.Ado.SqlQuery<JCJYF>("SELECT TOP 5 InstitutionName AS NAME,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS PRICE FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "' AND  InstitutionLevelCode < 5 and InstitutionCode <> '' GROUP BY InstitutionCode,InstitutionName ORDER BY PRICE DESC");
                        //赤峰
                        //rankEntity.zffy = db.Ado.SqlQuery<ZFFY>("SELECT TOP 5 InstitutionName AS Name,CAST(SUM(MLWFY)/10000 AS DECIMAL(18,2)) AS Price FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "'  and InstitutionCode <> ''  GROUP BY InstitutionCode,InstitutionName ORDER BY Price DESC");
                       // rankEntity.jcjyf = db.Ado.SqlQuery<JCJYF>("SELECT TOP 5 InstitutionName AS NAME,CAST((SUM(JCF)+SUM(HYF)+SUM(TJF))/10000 AS DECIMAL(18,2)) AS PRICE FROM dbo.YB_HosInfo WHERE CompYear ='" + CurrYear + "'  and InstitutionCode <> '' GROUP BY InstitutionCode,InstitutionName ORDER BY PRICE DESC");

                    }
                    if (rankEntity != null)
                    {
                        redisdb.Set(SystemManageConst.RANK, rankEntity);
                        redisdb.Expire(SystemManageConst.RANK, 86400);//设置缓存时间
                    }
                    return rankEntity;
                }
                else
                {
                    return rankEntity;
                }
            }                     
        }

        public List<ChartsViewModel_JZ> GetSingleCharts(string yljgbh, int second,int count)
        {
            var charts = new List<ChartsViewModel_JZ>();
            var CurrDateTimeDate = DateTime.Now.Date;
            int CurrHour = DateTime.Now.Hour;   //当前小时
            float timeInterval = second * 1.0f / 3600;
            using (var db = _dbContext.GetIntance())
            {
                if (db.Queryable<HisZydjEntity>().Any() && count != 0)
                {
                    for(int i = 1;i<= count; i++)
                    {
                        if(CurrHour - i * timeInterval >= 0)
                        {
                            ChartsViewModel_JZ chartsViewModel_JZ = new ChartsViewModel_JZ();
                            chartsViewModel_JZ.Name = (CurrHour - i * timeInterval).ToString() + "-" + (CurrHour - (i-1) * timeInterval).ToString();
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
        public bool GetFlag()
        {
            var list = new List<FlagEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<FlagEntity>("SELECT a1.YLJGBH,a1.KSBH,a1.CWS,COUNT(1) AS ZYS FROM (SELECT YLJGBH,KSBH,COUNT(1) AS CWS FROM " +
                    "dbo.HIS_CWXX GROUP BY YLJGBH,KSBH) a1 LEFT JOIN dbo.HIS_ZYDJ a2 ON a1.YLJGBH = a2.YLJGBH WHERE a1.KSBH = a2.RYKSBM " +
                    "AND a2.IsOutHos = 0 AND a2.DeleteMark = 1 AND YEAR(a2.SCSJ) = '" + CurrYear +"' GROUP BY a1.YLJGBH,a1.KSBH,a1.CWS");
            }
            foreach(var it in list)
            {
                if (it.ZYS > it.CWS)
                    return false;
            }
            return true;
        }
        public List<string> GetYYFlag()
        {
            var list = new List<FlagEntity>();
            var strlist = new List<string>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<FlagEntity>("SELECT a1.YLJGBH,a1.KSBH,a1.CWS,COUNT(1) AS ZYS FROM (SELECT YLJGBH,KSBH,COUNT(1) AS CWS FROM " +
                    "dbo.HIS_CWXX GROUP BY YLJGBH,KSBH) a1 LEFT JOIN dbo.HIS_ZYDJ a2 ON a1.YLJGBH = a2.YLJGBH WHERE a1.KSBH = a2.RYKSBM " +
                    "AND a2.IsOutHos = 0 AND a2.DeleteMark = 1 AND YEAR(a2.SCSJ) = '" + CurrYear + "' GROUP BY a1.YLJGBH,a1.KSBH,a1.CWS");
            }
            foreach (var it in list)
            {
                if (it.ZYS > it.CWS)
                    strlist.Add(it.YLJGBH);
            }
            return strlist;
        }
        public TotalEntity GetTotal()
        {
            int CurrMoth = DateTime.Now.Month;   //当前月份
            var entity = new TotalEntity();           
            using (var db = _dbContext.GetIntance())
            {
                if (db.Queryable<BedsInformationEntity>().Where(it => it.DeleteMark == 1).Any())
                {
                    var yljgbhlist = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0 && it.SCSJ.Year == CurrYear).Select(it => it.YLJGBH).ToList();
                    entity.CWCount = db.Queryable<BedsInformationEntity>()
                        .Where(it => it.DeleteMark == 1 && yljgbhlist.Contains(it.YLJGBH)).Count();
                }                  
                else
                    entity.CWCount = 0;
                if (db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0 && it.SCSJ.Year == CurrYear).GroupBy(it => it.SFZH).Select(it => it.SFZH).Any())
                    entity.ZYCount = db.Queryable<HisZydjEntity>().Where(it => it.DeleteMark == 1 && it.IsOutHos == 0 && it.SCSJ.Year == CurrYear).GroupBy(it => it.SFZH).GroupBy(it => it.YLJGBH).Select(it => it.SFZH).Count();
                else
                    entity.ZYCount = 0;

                if (db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicDateYear == CurrYear).Any())
                    entity.MZJZCount = db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicDateYear == CurrYear).Count();
                else
                    entity.MZJZCount = 0;
                if (db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicDateYear == CurrYear && it.ClinicDateMonth == CurrMoth).Any())
                    entity.MZJZMothCount = db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicDateYear == CurrYear && it.ClinicDateMonth == CurrMoth).Count();
                else
                    entity.MZJZMothCount = 0;
                if (db.Queryable<YBHosInfoEntity>().Where(it => it.CompYear == CurrYear.ToString()).Any())
                    entity.ZYJZCount = db.Queryable<YBHosInfoEntity>().Where(it => it.CompYear == CurrYear.ToString()).Count();
                else
                    entity.ZYJZCount = 0;
                if (db.Queryable<YBHosInfoEntity>().Where(it => it.CompYear == CurrYear.ToString() && it.InHosMonth == CurrMoth.ToString()).Any())
                    entity.ZYJZMothCount = db.Queryable<YBHosInfoEntity>().Where(it => it.CompYear == CurrYear.ToString() && it.InHosMonth == CurrMoth.ToString()).Count();
                else
                    entity.ZYJZMothCount = 0;
            }
            return entity;          
        }

        public List<TreeModule> GetYYXXTreeList()
        {
            var data = new List<TreeModule>();
            using (var db = _dbContext.GetIntance())
            {
                var yljgbhlist = db.Queryable<HisZydjEntity>().Where(it => it.IsOutHos == 0 && it.SCSJ.Year == CurrYear && it.DeleteMark == 1).Select(it => it.YLJGBH).ToList();
                data = db.Queryable<YYXXEntity>().Where(it => yljgbhlist.Contains(it.YLJGBH) && it.DeleteMark == 1)
                    .Select(it => new TreeModule
                    {
                        ID = it.YLJGBH,
                        PID = "0",
                        NAME = it.YLJGMC
                    }).ToList();               
            }
            return data;
        }
        public List<PersonInfo> GetPersonInfo(string code)
        {
            var data = new List<PersonInfo>();
            using (var db = _dbContext.GetIntance())
            {
                var yljgbhlist = db.Queryable<HisZydjEntity>().Where(it => it.IsOutHos == 0 && it.SCSJ.Year == CurrYear && it.DeleteMark == 1).Select(it => it.YLJGBH).ToList();
                data = db.Queryable<HisZydjEntity>()
                    .WhereIF(!string.IsNullOrEmpty(code),it => it.YLJGBH == code)
                    .Where(it => it.IsOutHos == 0 && it.SCSJ.Year == CurrYear && it.DeleteMark == 1)
                    .Select(it => new PersonInfo
                    {
                        CRowId = it.CRowId,
                        YLJGBH = it.YLJGBH,
                        ZYBH = it.ZYBH,
                        Name = it.HZXM,
                        Age = it.NL,
                        Sex = it.XBMC
                    }).ToList();
            }
            return data;
        }
        public List<FLCountCharts> GetInHospitalFLList()
        {
            var data = new List<FLCountCharts>();          
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<FLCountCharts>("SELECT InstitutiongGradeName AS LevelName,COUNT(InstitutiongGradeName) AS PersonCount  FROM dbo.YB_HosInfo " +
                    "WHERE InstitutionLevelCode < 5 AND CompYear = '" + CurrYear + "' GROUP BY InstitutiongGradeCode,InstitutiongGradeName HAVING InstitutiongGradeCode IN ('8','5','2')");
            }
            return data;
        }
        public List<FLCountCharts> GetTCInHospitalFLList(string levelcode, int page, int limit, ref int count)
        {
            var data = new List<FLCountCharts>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(levelcode), it => it.InstitutiongGradeCode == levelcode)
                    .Where(it => it.CompYear == CurrYear.ToString() && SqlFunc.ToInt32(it.InstitutionLevelCode) < 5 && it.InstitutionCode != "")
                    .GroupBy(it => it.InstitutionCode)
                    .GroupBy(it => it.InstitutionName)
                    .OrderBy(it => SqlFunc.AggregateCount(it.InstitutionLevelCode),OrderByType.Desc)
                    .Select(it => new FLCountCharts
                    {
                        InstitutionCode = it.InstitutionCode,
                        HospitalName = it.InstitutionName,
                        PersonCount = SqlFunc.AggregateCount(it.InstitutionLevelCode)
                    })                   
                    .ToPageList(page, limit, ref count);
            }
            return data;
        }
        public List<FundCharts> GetInHospitalFundList()
        {
            var data = new List<FundCharts>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<FundCharts>("SELECT InstitutiongGradeName AS LevelName,CAST(SUM(YBBXFY)/10000 AS DECIMAL(18,2)) AS FundPrice  FROM dbo.YB_HosInfo " +
                    "WHERE CompYear = '" + CurrYear + "' AND InstitutionLevelCode < 5 AND InstitutionCode <> '' GROUP BY InstitutiongGradeName HAVING InstitutiongGradeName IN ('一级','二级','三级')");
            }
            return data;
        }
        public List<FundCharts> GetTCInHospitalFundList(string levelcode, int page, int limit, ref int count)
        {
            var data = new List<FundCharts>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(levelcode), it => it.InstitutiongGradeCode == levelcode)
                    .Where(it => it.CompYear == CurrYear.ToString() && SqlFunc.ToInt32(it.InstitutionLevelCode) < 5 && it.InstitutionCode != "")
                    .GroupBy(it => it.InstitutionCode)
                    .GroupBy(it => it.InstitutionName)
                    .OrderBy(it => SqlFunc.AggregateCount(it.InstitutionLevelCode), OrderByType.Desc)
                    .Select(it => new FundCharts
                    {
                        InstitutionCode = it.InstitutionCode,
                        HospitalName = it.InstitutionName,
                        FundPrice = SqlFunc.AggregateSum(it.YBBXFY/10000)
                    })
                    .ToPageList(page, limit, ref count);
            }
            return data;
        }

        public List<ZnshTYDetailEntity> GetZQChartsFourList(string InstitutionCode, int page, int limit, ref int count)
        {
            var data = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString() && SqlFunc.ToInt32(it.InstitutionLevelCode) < 5 && it.InstitutionCode != "")
                    .Where(it => it.YBBXFY != 0 || it.DBBXBXFY != 0)
                    .Where(it => it.MLWFY != 0 || it.MLWFY != null)
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        MLWFY = it.MLWFY,
                        YBBXFY = it.YBBXFY,
                        DBBXBXFY = it.DBBXBXFY
                    })
                    .ToPageList(page, limit, ref count);
                foreach(var it in data)
                {
                    it.PAYFY = it.YBBXFY + it.DBBXBXFY;
                }
            }
            return data;
        }
        public ChartsByDrugName GetChartsByDrugNameCharts(string ypmc,int count)
        {
            var data = new ChartsByDrugName();
            data.ListByYYMC = new List<ChartsByDrugName_YYMC>();
            data.ListByAge = new List<ChartsByDrugName_Age>();            
            using (var db = _dbContext.GetIntance())
            {
                data.ListByYYMC = db.Ado.SqlQuery<ChartsByDrugName_YYMC>("select TOP "+ count + " * from TJ_XYYLAJG where ypmc = '"+ ypmc + "' order by SL desc");
                data.ListByAge = db.Ado.SqlQuery<ChartsByDrugName_Age>("select * from TJ_XYANLTJ where ypmc = '"+ ypmc + "'");
            }
            return data;
         }

        public List<ChartsByDrugName_YYMC> GetChartsByDrugNameList(string ypmc, int page, int limit, ref int count)
        {
            var data = new List<ChartsByDrugName_YYMC>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<ChartsByDrugName_YYMC>("SELECT * FROM (select ROW_NUMBER() OVER(Order by SL DESC) AS RowId,* from TJ_XYYLAJG where ypmc = '" + ypmc + "') AS b WHERE b.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM(select * from TJ_XYYLAJG where ypmc = '" + ypmc + "') AS b");
            }
            return data;
        }
        public ChartsByDiseaseName GetChartsByDiseaseNameCharts(string jbmc, int count)
        {
            var data = new ChartsByDiseaseName();
            data.ListByYYMC = new List<ChartsByDiseaseName_YYMC>();
            data.ListByAge = new List<ChartsByDiseaseName_Age>();
            using (var db = _dbContext.GetIntance())
            {
                data.ListByYYMC = db.Ado.SqlQuery<ChartsByDiseaseName_YYMC>("select TOP " + count + " * from TJ_JBAYYTJ where DiseaseName = '" + jbmc + "' order by SL desc");
                data.ListByAge = db.Ado.SqlQuery<ChartsByDiseaseName_Age>("select * from TJ_JBANLTJ where DiseaseName = '" + jbmc + "'");
            }
            return data;
        }

        public List<ChartsByDiseaseName_YYMC> GetChartsByDiseaseNameList(string jbmc, int page, int limit, ref int count)
        {
            var data = new List<ChartsByDiseaseName_YYMC>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<ChartsByDiseaseName_YYMC>("SELECT * FROM (select ROW_NUMBER() OVER(Order by SL DESC) AS RowId,* from TJ_JBAYYTJ where DiseaseName = '" + jbmc + "') AS b WHERE b.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM(select * from TJ_JBAYYTJ where DiseaseName = '" + jbmc + "') AS b");
            }
            return data;
        }
        public List<Charts1> GetTCCharts1List(string levelcode, int page, int limit, ref int count)
        {
            var data = new List<Charts1>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(levelcode), it => it.InstitutiongGradeCode == levelcode)
                    .Where(it => it.CompYear == CurrYear.ToString() && SqlFunc.ToInt32(it.InstitutionLevelCode) < 5 && it.InstitutionCode != "")
                    .GroupBy(it => it.InstitutionCode)
                    .GroupBy(it => it.InstitutionName)
                    .OrderBy(it => SqlFunc.AggregateCount(it.InstitutionLevelCode), OrderByType.Desc)
                    .Select(it => new Charts1
                    {
                        InstitutionCode = it.InstitutionCode,
                        HospitalName = it.InstitutionName,
                        MLWFY = SqlFunc.AggregateSum(it.MLWFY)
                    })
                    .ToPageList(page, limit, ref count);
            }
            return data;
        }
        public List<Charts2> GetTCCharts2List(string levelcode, int page, int limit, ref int count)
        {
            var data = new List<Charts2>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(levelcode), it => it.InstitutiongGradeCode == levelcode)
                    .Where(it => it.CompYear == CurrYear.ToString() && SqlFunc.ToInt32(it.InstitutionLevelCode) < 5 && it.InstitutionCode != "")
                    .GroupBy(it => it.InstitutionCode)
                    .GroupBy(it => it.InstitutionName)
                    .OrderBy(it => SqlFunc.AggregateCount(it.InstitutionLevelCode), OrderByType.Desc)
                    .Select(it => new Charts2
                    {
                        InstitutionCode = it.InstitutionCode,
                        HospitalName = it.InstitutionName,
                        ZFY = SqlFunc.AggregateSum(it.ZFY),
                        YBBXFY = SqlFunc.AggregateSum(it.YBBXFY),
                        DBBXBXFY = SqlFunc.AggregateSum(it.DBBXBXFY),
                        Count  = SqlFunc.AggregateCount(it.InstitutionCode)
                    })
                    .ToPageList(page, limit, ref count);
                foreach(var it in data)
                {
                    it.CJFY = Convert.ToDecimal(Math.Round((it.ZFY / it.Count), 2).ToString());
                    it.CJZF = Convert.ToDecimal(Math.Round((it.YBBXFY + it.DBBXBXFY) / it.Count, 2).ToString());
                }
            }
            return data;
        }
        public ThreeCharts GetThreeChartsList(int count)
        {
            var data = new ThreeCharts();
            data.ChartsOne = new List<Charts1>();
            data.ChartsTwo = new List<Charts2>();
            data.ChartsThree = new List<Charts3>();
            using (var db = _dbContext.GetIntance())
            {
                data.ChartsOne = db.Ado.SqlQuery<Charts1>("SELECT InstitutiongGradeName + '医院' as YYJB,Convert(decimal(18,2),Convert(decimal(18,2),sum(MLWFY))/ Convert(decimal(18,2),sum(ZFY)) * 100) as MLWFY " +
                    "from YB_HosInfo WHERE InstitutionLevelCode < 5 and InstitutionCode <> '' GROUP BY InstitutiongGradeName");
                data.ChartsTwo = db.Ado.SqlQuery<Charts2>("select InstitutiongGradeName + '医院' as YYJB ,Convert(decimal(18,2),sum(ZFY)/count(*)) as CJFY,Convert(decimal(18,2),sum(isnull(YBBXFY,0)+isnull(DBBXBXFY,0))/count(*)) as CJZF " +
                    "from YB_HosInfo WHERE InstitutionLevelCode < 5 and InstitutionCode <> '' GROUP by InstitutiongGradeName");
                data.ChartsThree = db.Ado.SqlQuery<Charts3>("select TOP "+ count + " InstitutionName, Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) as YZB from YB_HosInfo " +
                    "WHERE InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 and InstitutionCode <> '' GROUP by InstitutionName ORDER by Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) desc");
            }
            return data;           
        }
        public List<Charts3> GetCharts3List(int page, int limit, ref int count)
        {
            var data = new List<Charts3>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<Charts3>("SELECT * FROM (SELECT ROW_NUMBER() OVER(Order by Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) desc) AS RowId, " +
                    "InstitutionName, Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) as YZB from YB_HosInfo WHERE InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 " +
                    "and InstitutionCode <> '' GROUP by InstitutionName) AS b WHERE b.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM(select InstitutionName, Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) as YZB from YB_HosInfo " +
                    "WHERE InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 and InstitutionCode <> '' GROUP by InstitutionName) AS b");
            }
            return data;
        }
        public MapCharts GetMapCharts(string code)
        {
            var data = new MapCharts();
            data.yzbcharts = new YZBCharts();
            data.cjfycharts = new CJFYCharts();
            data.blmlwfycharts = new BLMLWFYCharts();
            using (var db = _dbContext.GetIntance())
            {
                data.yzbcharts = db.Ado.SqlQuerySingle<YZBCharts>("select InstitutionCode,InstitutionName, Convert(decimal(18,2),((sum(XYF) + sum(ZYF) + sum(CYF) + sum(MYF))/sum(ZFY))*100) as YZB from YB_HosInfo " +
                    "WHERE InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 and InstitutionCode <> '' AND InHosYear = '"+ CurrYear +"' " +
                    "AND InstitutionCode = '"+ code + "' GROUP BY InstitutionName,InstitutionCode");
                data.cjfycharts = db.Ado.SqlQuerySingle<CJFYCharts>("select InstitutionCode,InstitutionName ,Convert(decimal(18,2),sum(ZFY)/count(*)) as CJFY,Convert(decimal(18,2),sum(isnull(YBBXFY,0)+isnull(DBBXBXFY,0))/count(*)) as CJZF " +
                    "from YB_HosInfo WHERE  InstitutionCode  = '" + code + "' AND InHosYear = '"+ CurrYear +"' GROUP by InstitutionCode,InstitutionName");
                data.blmlwfycharts = db.Ado.SqlQuerySingle<BLMLWFYCharts>("select InstitutionCode,InstitutionName,Convert(decimal(18,2),Convert(decimal(18,2),sum(MLWFY))/ Convert(decimal(18,2),sum(ZFY)) * 100) as MLWFY " +
                    "from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND InstitutionCode  = '" + code + "' GROUP by InstitutionCode,InstitutionName");
            }
            return data;
        }
        public MapList GetMapList(string code)
        {
            var data = new MapList();
            data.yzblist = new List<YZBList>();
            data.cjfylist = new List<CJFYList>();
            data.blmlwfylist = new List<BLMLWFYList>();                    
            using (var db = _dbContext.GetIntance())
            {
                data.yzblist = db.Ado.SqlQuery<YZBList>("select InstitutionCode,InstitutionName, '西药费' as FEETYPE ,Convert(decimal(18,2),sum(XYF)/10000) as FEE from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 and InstitutionCode <> '' " +
                    "AND InstitutionCode = '"+ code+ "' GROUP by InstitutionName,InstitutionCode UNION SELECT InstitutionCode,InstitutionName,  '中药费' as FEETYPE ,Convert(decimal(18,2),sum(ZYF)/10000) as FEE from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND InstitutionLevelCode < 5 and InstitutionLevelCode <> 2" +
                    " and InstitutionCode <> '' AND InstitutionCode = '"+ code+ "' GROUP by InstitutionName,InstitutionCode UNION SELECT InstitutionCode,InstitutionName, '草药费' as FEETYPE ,Convert(decimal(18,2),sum(CYF)/10000) as FEE from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND InstitutionLevelCode < 5 " +
                    "and InstitutionLevelCode <> 2 and InstitutionCode <> '' AND InstitutionCode = '"+ code+"' GROUP by InstitutionName,InstitutionCode UNION SELECT InstitutionCode,InstitutionName, '蒙药费' as FEETYPE ,Convert(decimal(18,2),sum(MYF)/10000)as FEE from YB_HosInfo " +
                    "WHERE InHosYear = '"+ CurrYear +"' AND InstitutionLevelCode < 5 and InstitutionLevelCode <> 2 and InstitutionCode <> '' AND InstitutionCode = '" + code+"'group by InstitutionName,InstitutionCode");
                data.cjfylist = db.Ado.SqlQuery<CJFYList>("select InHosMonth,count(*) as JZRCS from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND InstitutionCode  = '"+ code+"' GROUP by InHosMonth ORDER by Convert(decimal(18,2),InHosMonth)");
                data.blmlwfylist = db.Ado.SqlQuery<BLMLWFYList>("select InstitutionCode,InstitutionName,'丙类费用' as FEETYPE,Convert(decimal(18,2),sum(MLWFY)/10000) as FEE from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND  InstitutionCode  = '" + code+"' GROUP by InstitutionCode,InstitutionName " +
                    "UNION SELECT InstitutionCode,InstitutionName,'总费用' as FEETYPE,CONVERT(decimal(18,2),sum(ZFY)/10000) as FEE from YB_HosInfo WHERE InHosYear = '"+ CurrYear +"' AND  InstitutionCode  = '" + code+"' GROUP by InstitutionCode,InstitutionName");
            }
            return data;
        }
        public List<MLFY> GetMLFYpList()
        {
            var list = new List<MLFY>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<MLFY>("SELECT CONVERT(DECIMAL(18,2),SUM(MLWFY)/10000) AS MLWFY, CONVERT(DECIMAL(18,2),SUM(MLNFY)/10000) AS MLNFY, CONVERT(DECIMAL(18,2),SUM(MLWFY)/SUM(ZFY)*100)  AS MLWFYp,CONVERT(DECIMAL(18,2),SUM(MLNFY)/SUM(ZFY)*100) AS MLNFYp FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear+"' AND InstitutionLevelCode < 5	");
            }
            return list;
        }
        public List<MLFY> GetHosMLFYList(int page, int limit, ref int count)
        {
            var list = new List<MLFY>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<MLFY>("SELECT * FROM (SELECT  ROW_NUMBER() OVER(Order by SUM(ZFY) desc) AS RowId,InstitutionCode,InstitutionName,CONVERT(DECIMAL(18,2),ISNULL(SUM(MLWFY),0)/10000) AS MLWFY," +
                    "CONVERT(DECIMAL(18,2),ISNULL(SUM(MLNFY),0)/10000) AS MLNFY FROM dbo.YB_HosInfo WHERE CompYear = '"+ CurrYear+"' AND InstitutionLevelCode < 5 GROUP BY InstitutionCode,InstitutionName) tt " +
                    "WHERE tt.RowId BETWEEN " + bt1 + " AND " + bt2);
                count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT InstitutionCode,InstitutionName,CONVERT(DECIMAL(18,2),ISNULL(SUM(MLWFY),0)) AS MLWFY," +
                    "CONVERT(DECIMAL(18,2),ISNULL(SUM(MLNFY),0)) AS MLNFY FROM dbo.YB_HosInfo WHERE CompYear = '" + CurrYear + "' AND InstitutionLevelCode < 5 GROUP BY InstitutionCode,InstitutionName) tt ");
            }
            return list;
        }
        public List<HisZyyzmxDto> GetYZList(string yljgbh, string zybh)
        {
            var data = new List<HisZyyzmxDto>();
            using (var db = _dbContext.GetIntance())
            {
                if(!string.IsNullOrEmpty(yljgbh) && !string.IsNullOrEmpty(zybh))
                {
                    data = db.Ado.SqlQuery<HisZyyzmxDto>("SELECT XMMC,CASE YZXMLX WHEN '1' THEN '药物医嘱 'WHEN '2' THEN '诊疗医嘱' ELSE '' END AS YZXMLX,CONVERT(varchar(16),YZQSSJ,20) AS YZQSSJ,CONVERT(varchar(16),YZTZSJ,20) AS YZTZSJ," +
                        "DCYL,JLDW,PLCS FROM dbo.HIS_ZYYZMX WHERE YLJGBH = '"+ yljgbh + "' AND ZYBH = '" + zybh + "' AND DeleteMark = 1");
                }             
            }
            return data;
        }
        public HisZydjEntity GetGCPersonList(string crowid)
        {
            var data = new HisZydjEntity();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(crowid))
                {
                    data = db.Queryable<HisZydjEntity>()
                           .Where(it => it.CRowId == crowid && it.DeleteMark == 1)
                           .First();
                }
            }
            return data;
        }
        public List<TJ_JBAYYTJEntity> GetZQDiseaseList(string keyword, string diseaseName, int page, int limit, ref int count)
        {
            var list = new List<TJ_JBAYYTJEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<TJ_JBAYYTJEntity>()
                     .WhereIF(!string.IsNullOrEmpty(diseaseName),it=>it.DiseaseName == diseaseName)
                    .WhereIF(!string.IsNullOrEmpty(keyword),it=>it.InstitutionName.Contains(keyword))
                       .ToPageList(page, limit, ref count);
            }
            return list;
        }
        public List<ZnshTYDetailEntity> GetZQZFFYList(string InstitutionCode, int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        ZFFY = it.MLWFY
                    }).ToPageList(page, limit, ref count);
            }
            return list;
        }

        public List<ZnshTYDetailEntity> GetZQJCJYList(string InstitutionCode, int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,          
                        JCF = it.JCF,
                        HYF = it.HYF,
                        TJF = it.TJF
                    }).ToPageList(page, limit, ref count);
                foreach(var it in list)
                {
                    it.JCJYFY = it.JCF + it.HYF + it.TJF;
                }
            }
            return list;
        }

        public List<ZnshTYDetailEntity> GetZQMapYZBList(string InstitutionCode,int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        XYF = it.XYF,
                        ZYF = it.ZYF,
                        CYF = it.CYF,
                        MYF = it.MYF                        
                    }).ToPageList(page, limit, ref count);
            }
            return list;
        }
        public List<ZnshTYDetailEntity> GetZQMapCJFYList(string InstitutionCode,int moth,int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .WhereIF(moth !=0,it => SqlFunc.ToInt32(it.InHosMonth) == moth)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        DBBXBXFY = it.DBBXBXFY,
                        YBBXFY = it.YBBXFY                       
                    }).ToPageList(page, limit, ref count);
                foreach(var it in list)
                {
                    it.PAYFY = it.YBBXFY + it.DBBXBXFY;
                }
            }
            return list;
        }
        public List<ZnshTYDetailEntity> GetZQMapMLWZBList(string InstitutionCode, int page, int limit, ref int count)
        {
            var list = new List<ZnshTYDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosInfoEntity>()
                    .WhereIF(!string.IsNullOrEmpty(InstitutionCode), it => it.InstitutionCode == InstitutionCode)
                    .Where(it => it.CompYear == CurrYear.ToString())
                    .Select(it => new ZnshTYDetailEntity
                    {
                        HosRegisterCode = it.HosRegisterCode,
                        Name = it.Name,
                        Age = it.Age,
                        Gender = it.Gender,
                        IdNumber = it.IdNumber,
                        InHosDate = it.InHosDate,
                        OutHosDate = it.OutHosDate,
                        InHosDay = it.InHosDay,
                        DiseaseName = it.DiseaseName,
                        ZFY = it.ZFY,
                        BLFY = it.MLWFY
                    }).ToPageList(page, limit, ref count);
            }
            return list;
        }
        public List<YBHosPreInfoEntity> GetPreList(string HosRegisterCode, int page, int limit, ref int count)
        {
            var list = new List<YBHosPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<YBHosPreInfoEntity>()
                        .WhereIF(!string.IsNullOrEmpty(HosRegisterCode), it => it.HosRegisterCode == HosRegisterCode)
                        .ToPageList(page, limit, ref count);
            }
            return list;
        }

        /// <summary>
        /// 获取审核结果列表按照规则分组
        /// </summary>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByRule(string rulesName,int page, int limit, ref int count)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity>().GroupBy(a => new { a.RulesCode, a.RulesName })
                    .WhereIF(!string.IsNullOrEmpty(rulesName),a => a.RulesName == rulesName)
                   .Where(a => a.DataType == "2")
                   .OrderBy(a => SqlFunc.AggregateCount(a.RulesCode), OrderByType.Desc)
                   .Select(a => new StaticsViewModel() { commonname = a.RulesName,rulecode = a.RulesCode, count = SqlFunc.AggregateCount(a.RulesCode).ToString() })
                   .ToPageList(page, limit, ref count);
            }
            return DataResult;
        }
        /// <summary>
        /// 根据规则名称获取各个医院违规人数（住院）
        /// </summary>
        /// <param name="rulecode"></param>
        /// <param name="flag"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsJGMCByRule(string rulecode, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity>().GroupBy(a => new { a.InstitutionCode, a.InstitutionName, a.RulesCode })
                    .Where(a => a.DataType == "2" && a.RulesCode == rulecode)
                    .OrderBy(a => SqlFunc.AggregateCount(a.InstitutionCode), OrderByType.Desc)
                    .Select(a => new StaticsViewModel() { commonname = a.InstitutionName + "|" + a.RulesCode,InstitutionCode = a.InstitutionCode, count = SqlFunc.AggregateCount(a.InstitutionCode).ToString() })
                    .ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        /// <summary>
        /// 根据条件获取住院列表
        /// </summary>
        /// <param name="jgcode"></param>
        /// <param name="rulecode"></param>
        /// <param name="djcode"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBHosInfoEntityDto> GetYBHosInfoList(string personName,string jgcode, string rulecode, string djcode, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBHosInfoEntityDto>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                dataResult = db.Queryable<YBHosInfoEntity, CheckResultInfoEntity, YYXXEntity>((b, a, c) => new object[] {
                    JoinType.Inner,a.RegisterCode == b.HosRegisterCode &&a.PersonalCode==b.PersonalCode,
                    JoinType.Left,a.InstitutionCode == c.YYDMYYDM
                })
                .Where((b, a, c) => a.DataType == "2")
                .WhereIF(!string.IsNullOrEmpty(djcode), (b, a, c) => c.YLJGDJBM == djcode)
                .WhereIF(!string.IsNullOrEmpty(jgcode), (b, a, c) => a.InstitutionCode == jgcode)
                .WhereIF(!string.IsNullOrEmpty(rulecode), (b, a, c) => a.RulesCode == rulecode)
                .WhereIF(!string.IsNullOrEmpty(personName), (b, a, c) => b.Name.Contains(personName))
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
        /// 获取处方明细list 住院
        /// </summary>
        /// <returns></returns>
        public List<YBHosPreInfoEntity> GetCFDeatilList(string hosregistercode, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<YBHosPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(hosregistercode))
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity>().Where(it => it.HosRegisterCode == hosregistercode).ToPageList(pageIndex, pageSize, ref totalCount);
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }

        /// <summary>
        /// 获取违规处方列表(住院)
        /// </summary>
        /// <returns></returns>
        public List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string key, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<HosPreInfo_WGDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(key))
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity, CheckResultPreInfoEntity>((a, b) => new object[] {
                    JoinType.Left,a.PreCode == b.PreCode&&a.ItemIndex==b.ItemIndex&&a.HosRegisterCode==b.RegisterCode,
                    }).Where((a, b) => b.CheckResultInfoCode == key && b.DataType == "2")
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
                    }).ToPageList(pageIndex, pageSize, ref totalCount);
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }

        /// <summary>
        /// 获取审核结果列表按照医院等级分组
        /// </summary>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsByJGJB()
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    }).GroupBy((a, b) => new { b.YLJGDJBM, b.YLJGDJMC })
                   .Where((a, b) => a.DataType == "2" && b.YLJGDJMC != "")
                   .OrderBy((a, b) => SqlFunc.AggregateCount(b.YLJGDJBM), OrderByType.Desc)
                   .Select((a, b) => new StaticsViewModel() { commonname = b.YLJGDJMC, count = SqlFunc.AggregateCount(b.YLJGDJBM).ToString() })
                   .ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据机构等级获取机构各个规则违规人数
        /// </summary>
        /// <param name="djcode"></param>
        /// <param name="jgbm"></param>
        /// <returns></returns>
        public List<StaticsViewModel> GetStaticsViewsJGMCByDJ(string djcode,string jgbm, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    })
                    .WhereIF(!string.IsNullOrEmpty(jgbm), (a, b) => a.InstitutionCode == jgbm)
                    .WhereIF(!string.IsNullOrEmpty(djcode), (a, b) => b.YLJGDJBM == djcode)
                    .Where((a, b) => a.DataType == "2")
                    .GroupBy((a, b) => new { a.RulesName, a.RulesCode })
                    .OrderBy(a => SqlFunc.AggregateCount(a.RulesCode), OrderByType.Desc)
                    .Select(a => new StaticsViewModel() { rulename = a.RulesName, rulecode = a.RulesCode, count = SqlFunc.AggregateCount(a.RulesCode).ToString() })
                    .ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }
        /// <summary>
        ///  获取医院菜单
        /// </summary>
        /// <param name="level">医院等级</param>
        /// <returns></returns>
        public List<StaticsViewModel> GetHosListByLevel(string level, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<StaticsViewModel>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YYXXEntity>((a, b) => new object[] {
                        JoinType.Left,a.InstitutionCode == b.YYDMYYDM,
                    }).WhereIF(!string.IsNullOrEmpty(level),(a,b) => b.YLJGDJBM == level)
                    .Where((a, b) => a.DataType == "2")
                    .GroupBy((a, b) => new { a.InstitutionCode, a.InstitutionName })
                    .OrderBy((a, b) => SqlFunc.AggregateCount(a.InstitutionCode), OrderByType.Desc)
                    .Select((a, b) => new StaticsViewModel()
                    {
                        commonname = a.InstitutionName,
                        count = SqlFunc.AggregateCount(a.InstitutionCode).ToString(),
                        InstitutionCode = a.InstitutionCode
                    }).ToPageList(pageIndex, pageSize, ref totalCount);
            }
            return DataResult;
        }

    }
}
