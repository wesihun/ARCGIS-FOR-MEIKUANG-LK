using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Entities.Dto;
using XY.AfterCheckEngine.IService;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.Service
{
    /// <summary>
    /// 功能描述：ComplaintStatisticalService
    /// 创 建 者：LK
    /// 创建日期：2019/12/11 13:42:39
    /// 最后修改者：LK
    /// 最后修改日期：2019/12/11 13:42:39
    /// </summary>
    public class ComplaintStatisticalService: IComplaintStatisticalService
    {
        private readonly IXYDbContext _dbContext;

        public ComplaintStatisticalService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<YYKKGL> GetYYKKList(YYKKGL queryCodition, bool isAdmin, string curryydm, int page, int limit, ref int count)
        {
            var list = new List<YYKKGL>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            StringBuilder query = new StringBuilder();
            if (string.IsNullOrEmpty(queryCodition.StartTime))
            {
                queryCodition.StartTime = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(queryCodition.EndTime))
            {
                queryCodition.EndTime = new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(queryCodition.StartConclusionTime))
            {
                queryCodition.StartConclusionTime = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(queryCodition.EndConclusionTime))
            {
                queryCodition.EndConclusionTime = new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            }
            if (!string.IsNullOrEmpty(queryCodition.YYMC))
            {
                query.Append(" AND ccm.InstitutionName like '%" + queryCodition.YYMC + "%'");
            }
            if (!string.IsNullOrEmpty(queryCodition.JGDJ))
            {
                query.Append(" AND yhi.InstitutiongGradeCode='" + queryCodition.JGDJ + "' ");
            }
            if (!string.IsNullOrEmpty(queryCodition.ICDCode))
            {
                query.Append(" AND ccm.ICDCode='" + queryCodition.ICDCode + "'");
            }
            if (!string.IsNullOrEmpty(queryCodition.WGDJ))
            {
                query.Append(" AND ccm.YDLevel='" + queryCodition.WGDJ + "'");
            }
            if (!string.IsNullOrEmpty(queryCodition.StartConclusionTime))
            {
                query.Append(" AND ccm.DoubtfulConclusionTime >= '" + queryCodition.StartConclusionTime + "' ");
            }
            if (!string.IsNullOrEmpty(queryCodition.EndConclusionTime))
            {
                query.Append(" AND ccm.DoubtfulConclusionTime <= '" + queryCodition.EndConclusionTime + "' ");
            }
            using (var db = _dbContext.GetIntance())
            {
                if (isAdmin)
                {
                    if (!string.IsNullOrEmpty(queryCodition.YYBM))
                    {
                        query.Append(" AND ccm.InstitutionCode = '" + queryCodition.YYBM + "'");
                    }
                    list = db.Ado.SqlQuery<YYKKGL>("SELECT * FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                            "ccm.InstitutionCode AS YYBM,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'"+ queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                            "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                            "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                            "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                        "ccm.InstitutionCode,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                        "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                        "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                        "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj");
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    query.Append(" AND ccm.InstitutionCode = '" + curryydm + "'");
                    list = db.Ado.SqlQuery<YYKKGL>("SELECT * FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                            "ccm.InstitutionCode AS YYBM,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                            "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                            "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                            "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                        "ccm.InstitutionCode,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                        "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                        "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                        "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj");
                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0,6);
                    query.Append(" AND SUBSTRING(ccm.FamilyCode,0,7) = '" + XAreaCode + "'");
                    if (!string.IsNullOrEmpty(queryCodition.YYBM))
                    {
                        query.Append(" AND ccm.InstitutionCode = '" + queryCodition.YYBM + "'");
                    }
                    list = db.Ado.SqlQuery<YYKKGL>("SELECT * FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                            "ccm.InstitutionCode AS YYBM,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                            "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                            "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                            "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                            "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj WHERE tj.RowId BETWEEN " + bt1 + " AND " + bt2);
                    count = db.Ado.GetInt("SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.InstitutionCode) AS RowId," +
                        "ccm.InstitutionCode,ccm.InstitutionName AS YYMC,'居民基本医疗保险' AS XZLB,'住院' AS FYLY,SUM(ALLMoney) AS SJKKJE," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndTime + "',20), 23) AS SettlementTime," +
                        "CONVERT(varchar(100), convert(datetime,'" + queryCodition.StartConclusionTime + "', 20), 23) + ' 至 ' + CONVERT(varchar(100), convert(datetime,'" + queryCodition.EndConclusionTime + "',20), 23) AS ConclusionTime, SUM(yhi.zfy) AS JSJE,SUM(yhi.YBBXFY) AS TCJE,SUM(yhi.DBBXBXFY) AS DBJE," +
                        "COUNT(yhi.IdNumber) AS CKRC FROM Check_Complain_MZL AS ccm LEFT JOIN YB_HosInfo AS yhi ON ccm.RegisterCode=yhi.HosRegisterCode " +
                        "WHERE ccm.ComplaintStatus='10' AND yhi.SettlementDate>='" + queryCodition.StartTime + "' AND yhi.SettlementDate<='" + queryCodition.EndTime + "' " + query +
                        "GROUP BY ccm.InstitutionCode,ccm.InstitutionName) AS tj");
                }

            }
            return list;
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
        public List<CheckUserList> GetCheckUserListsByYLJG(YYKKGL queryCodition, bool isAdmin, string curryydm, int page, int limit, ref int count)
        {
            var list = new List<CheckUserList>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            StringBuilder query = new StringBuilder();
            if (!string.IsNullOrEmpty(queryCodition.StartTime))
            {
                query.Append(" AND ccm.SettlementDate>= '" + queryCodition.StartTime + "'");
                
            }
            if (!string.IsNullOrEmpty(queryCodition.EndTime))
            {
                query.Append(" AND ccm.SettlementDate<= '" + queryCodition.EndTime + "'");
            }
            //if (!string.IsNullOrEmpty(queryCodition.YYBM))
            //{
            //    query.Append(" AND ccm.InstitutionCode = '" + queryCodition.YYBM + "'");
            //}
            if (!string.IsNullOrEmpty(queryCodition.YYMC))
            {
                query.Append(" AND ccm.InstitutionName like '%" + queryCodition.YYMC + "%'");
            }
            if (!string.IsNullOrEmpty(queryCodition.JGDJ))
            {
                query.Append(" and a.InstitutionGradeCode='" + queryCodition.JGDJ+"'");
            }
            if (!string.IsNullOrEmpty(queryCodition.WGDJ))
            {
                query.Append(" and ccm.YDLevel='" + queryCodition.WGDJ + "'");
            }
            if (!string.IsNullOrEmpty(queryCodition.ICDCode))
            {
                query.Append(" and ccm.ICDCode='" + queryCodition.ICDCode+ "'");
            }
            if (!string.IsNullOrEmpty(queryCodition.StartConclusionTime))
            {
                query.Append(" AND ccm.DoubtfulConclusionTime >= '" + queryCodition.StartConclusionTime + "' ");
            }
            if (!string.IsNullOrEmpty(queryCodition.EndConclusionTime))
            {
                query.Append(" AND ccm.DoubtfulConclusionTime <= '" + queryCodition.EndConclusionTime + "' ");
            }
            using (var db = _dbContext.GetIntance())
            {
                if (isAdmin)
                {
                    if (!string.IsNullOrEmpty(queryCodition.YYBM))
                    {
                        query.Append(" AND ccm.InstitutionCode = '" + queryCodition.YYBM + "'");
                    }
                    list = db.Ado.SqlQuery<CheckUserList>(@"
                        SELECT * FROM(
                        SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + @"
                        ) tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + ") AS tj");
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    query.Append(" AND ccm.InstitutionCode = '" + curryydm + "'");
                    list = db.Ado.SqlQuery<CheckUserList>(@"
                        SELECT * FROM(
                        SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + @"
                        ) tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + ") AS tj");
                }
                else //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0, 6);
                    query.Append(" AND SUBSTRING(ccm.FamilyCode,0,7) = '" + XAreaCode + "'");
                    if (!string.IsNullOrEmpty(queryCodition.YYBM))
                    {
                        query.Append(" AND ccm.InstitutionCode = '" + queryCodition.YYBM + "'");
                    }
                    list = db.Ado.SqlQuery<CheckUserList>(@"
                        SELECT * FROM(
                        SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + @"
                        ) tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + ") AS tj");
                }
                 
            }
            return list;
        }
        public List<CheckUserList> GetListByRulesCode(ListByRulesCodeQuery queryCodition, int page, int limit, ref int count)
        {
            var list = new List<CheckUserList>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            StringBuilder query = new StringBuilder();
            if (string.IsNullOrEmpty(queryCodition.StartTime))
            {
                query.Append(" AND ccm.SettlementDate>= '" + queryCodition.StartTime + "'");

            }
            if (string.IsNullOrEmpty(queryCodition.EndTime))
            {
                query.Append(" AND ccm.SettlementDate<= '" + queryCodition.EndTime + " 23:59:59'");
            }
            if (!string.IsNullOrEmpty(queryCodition.InstitutionLevel))
            {
                query.Append(" AND ccm.InstitutionLevel = '" + queryCodition.InstitutionLevel + "'");
            }
            if (!string.IsNullOrEmpty(queryCodition.RulesLevel))
            {
                query.Append(" AND ccm.RulesLevel='" + queryCodition.RulesLevel + "'");
            }
            using (var db = _dbContext.GetIntance())
            {
                list = db.Ado.SqlQuery<CheckUserList>(@"
                        SELECT * FROM(
                        SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + @"
                        ) tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                count = db.Ado.GetInt(@"SELECT COUNT(1) FROM (SELECT  ROW_NUMBER() OVER(ORDER BY ccm.SettlementDate DESC) AS RowId, ccm.RegisterCode, PersonalCode, IdNumber, Personname NAME,
                        a.Gender AS Gender, a.Age AS Age,
                        ccm.InstitutionCode, ccm.InstitutionName, ICDCode, DiseaseName, ALLMoney KKJE, SettlementDate, InHosDate, OutHosDate,
                        a.InstitutionGradeCode as InstitutionGradeCode, a.InstitutionGradeName as InstitutionGradeName
                         FROM Check_Complain_MZL AS ccm LEFT JOIN(SELECT RegisterCode, MAX(cri.InstitutionGradeCode) InstitutionGradeCode,
                         MAX(cri.InstitutionGradeName) InstitutionGradeName, MAX(Gender) Gender, MAX(Age) Age
                          FROM Check_ResultInfo AS cri GROUP BY cri.RegisterCode) a ON ccm.RegisterCode = a.RegisterCode WHERE ComplaintStatus = '10'" + query + ") AS tj");
            }
            return list;
        }
        public List<Check_ComplaintMain_MZLEntity> GetYDInfoList(string registerCode)
        {
            var dataResult = new List<Check_ComplaintMain_MZLEntity>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Ado.SqlQuery<Check_ComplaintMain_MZLEntity>(@"SELECT * FROM dbo.Check_ComplaintMain_MZL WHERE RegisterCode = @registerCode AND IsPre = '0' AND ZZSHStates = '智审完成'
                            UNION SELECT * FROM dbo.Check_ComplaintMain_MZL WHERE RegisterCode = @registerCode AND IsPre = '0' AND ZZSHStates = '违规'",new { registerCode = registerCode});
            }
            return dataResult;
        }
        public Dictionary<int, string> GetQueryStates(bool isadmin, string curryydm)
        {
            if (isadmin)
            {
                Dictionary<int, string> myDictionary = new Dictionary<int, string>();
                myDictionary.Add(0, "全部");
                myDictionary.Add(1, "医院疑似病例反馈");
                myDictionary.Add(2, "人工复审");
                myDictionary.Add(22, "医院疑似病例二次反馈");
                myDictionary.Add(3, "专家审核");
                myDictionary.Add(5, "人工初审");
                myDictionary.Add(10, "疑点结论");
                return myDictionary;
            }
            
            if (curryydm.Substring(0, 2) == "15")
            {
                Dictionary<int, string> myDictionary = new Dictionary<int, string>();
                myDictionary.Add(1, "医院疑似病例反馈");
                myDictionary.Add(22, "医院疑似病例二次反馈");
                return myDictionary;
            }
            else
            {
                Dictionary<int, string> myDictionary = new Dictionary<int, string>();
                myDictionary.Add(0, "全部");
                myDictionary.Add(5, "人工初审");
                myDictionary.Add(2, "人工复审");
                myDictionary.Add(3, "专家审核");
                myDictionary.Add(10, "疑点结论");
                return myDictionary;
            }
        }
        public List<Check_Complain_MZLEntity> GetListByStates(string step, string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isAdmin, string curryydm, int page, int limit, ref int count)
        {
            var list = new List<Check_Complain_MZLEntity>();
            string bt1 = ((page - 1) * limit + 1).ToString();
            string bt2 = (page * limit).ToString();
            string querystr = string.Empty;
            if (!string.IsNullOrEmpty(step) && step != "0")
            {
                if(step == "22")
                {
                    querystr += " AND uni.StatesBS = '1' ";
                    querystr += " AND uni.IsSceondFK = '1' ";
                }
                else
                {
                    querystr += " AND uni.StatesBS = '" + step + "' ";
                }
            }
            if (!string.IsNullOrEmpty(states))
            {
                querystr += " AND uni.YDLevel = '" + states + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode))
            {
                querystr += " AND uni.RegisterCode = '" + queryCoditionByCheckResult.RegisterCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.Name))
            {
                querystr += " AND uni.PersonName = '" + queryCoditionByCheckResult.Name + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode))
            {
                querystr += " AND uni.InstitutionCode = '" + queryCoditionByCheckResult.InstitutionCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel))
            {
                querystr += " AND uni.InstitutionLevel = '" + queryCoditionByCheckResult.InstitutionLevel + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode))
            {
                querystr += " AND uni.ICDCode = '" + queryCoditionByCheckResult.ICDCode + "' ";
            }
            if (!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber))
            {
                querystr += " AND uni.IdNumber = '" + queryCoditionByCheckResult.IdNumber + "' ";
            }
            if (queryCoditionByCheckResult.StartSettleTime != null)
            {
                querystr += " AND uni.SettlementDate >= '" + queryCoditionByCheckResult.StartSettleTime + "' ";
            }
            else
            {
                querystr += " AND uni.SettlementDate >= '" + new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd") + "' ";
            }
            if (queryCoditionByCheckResult.EndSettleTime != null)
            {
                querystr += " AND uni.SettlementDate <= '" + queryCoditionByCheckResult.EndSettleTime + "' ";
            }
            else
            {
                querystr += " AND uni.SettlementDate <= '" + new DateTime(DateTime.Now.Year, 12, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd") + "' ";
            }
            using (var db = _dbContext.GetIntance())
            {
                if (isAdmin)
                {
                    list = db.Ado.SqlQuery<Check_Complain_MZLEntity>(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@" SELECT COUNT(1) FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj");
                }
                else if (curryydm.Substring(0, 2) == "15")  //是医院  
                {
                    querystr += " AND uni.InstitutionCode = '" + curryydm + "' ";
                    list = db.Ado.SqlQuery<Check_Complain_MZLEntity>(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@" SELECT COUNT(1) FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj");
                }
                else
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0, 6);
                    querystr += " AND SUBSTRING(uni.FamilyCode,0,7) = '" + XAreaCode + "' ";
                    if (!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode))
                    {
                        querystr += " AND uni.InstitutionCode = '" + queryCoditionByCheckResult.InstitutionCode + "' ";
                    }
                    list = db.Ado.SqlQuery<Check_Complain_MZLEntity>(@"
                        SELECT * FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,FamilyCode,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,FamilyCode,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj where tj.RowId BETWEEN " + bt1 + " AND " + bt2);

                    count = db.Ado.GetInt(@" SELECT COUNT(1) FROM (
                            SELECT ROW_NUMBER() OVER(ORDER BY uni.SettlementDate DESC) AS RowId,uni.PersonalCode,
                            uni.RegisterCode,uni.PersonName,uni.InstitutionCode,uni.InstitutionName,uni.DiseaseName,
                            uni.YDDescription,uni.IdNumber,uni.SettlementDate,uni.DSHWGJ,uni.DSHWGS,uni.YSHWGJ,uni.YSHWGS,
                            uni.ALLMoney,uni.ALLCount,uni.YDLevel,uni.SHLCStates,uni.StatesBS,uni.IsSceondFK,uni.InstitutionLevel
                            FROM (
                            SELECT PersonalCode,RegisterCode,Name AS PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            ResultDescription AS YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ZWGJE AS ALLMoney,ZWGSL AS ALLCount,YDDJ AS YDLevel,SHLCZT AS SHLCStates,'5' AS StatesBS,'0' AS IsSceondFK,FamilyCode,InstitutionLevel,ICDCode
                            FROM dbo.Check_ResultInfoMain WHERE SHStates is NULL AND DSHWGS <> 0 AND DataType = '2'
                            UNION ALL
                            SELECT PersonalCode,RegisterCode,PersonName,InstitutionCode,InstitutionName,DiseaseName,
                            YDDescription,IdNumber,SettlementDate,DSHWGJ,DSHWGS,YSHWGJ,YSHWGS,
                            ALLMoney,ALLCount,YDLevel,SHLCStates,ComplaintStatus AS StatesBS,IsSceondFK,FamilyCode,InstitutionLevel,ICDCode
                         FROM dbo.Check_Complain_MZL ) uni WHERE  1=1 " + querystr + " ) AS tj");
                }

           }
            return list;
        }
    }
}
