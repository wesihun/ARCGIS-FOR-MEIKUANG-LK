using System;
using System.Collections.Generic;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.Service
{
    /// <summary>
    /// 功能描述：BeforeCheckEngineService
    /// 创 建 者：LK
    /// 创建日期：2020/3/13 13:26:14
    /// 最后修改者：LK
    /// 最后修改日期：2020/3/13 13:26:14
    /// </summary>
    public class BeforeCheckEngineService: IBeforeCheckEngineService
    {
        private readonly IXYDbContext _dbContext;

        public BeforeCheckEngineService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Check_BeForeResultInfo> GetBeforeResultList(string states, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            List<Check_BeForeResultInfo> datalist = new List<Check_BeForeResultInfo>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (isadmin)
                {
                    datalist = db.Queryable<Check_BeForeResultInfo>()
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.RuleLevel == states)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode.Contains(queryCoditionByCheckResult.RegisterCode))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionGradeCode == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber).ToPageList(page, limit, ref totalcount);
                }
                else if (curryydm.Substring(0, 2) == "15")   //是医院  
                {
                    datalist = db.Queryable<Check_BeForeResultInfo>()
                        .Where(it => it.InstitutionCode == curryydm)
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.RuleLevel == states)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode.Contains(queryCoditionByCheckResult.RegisterCode))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), it => it.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionGradeCode == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber).ToPageList(page, limit, ref totalcount) ;
                }
                else   //旗县医保局
                {
                    var XAreaCode = returnXAreaCode(curryydm);
                    datalist = db.Queryable<Check_BeForeResultInfo>()
                        .Where(it => it.InstitutionCode.Substring(0, 6) == XAreaCode.Substring(0, 6))
                        .WhereIF(!string.IsNullOrEmpty(states), it => it.RuleLevel == states)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode == queryCoditionByCheckResult.RegisterCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.RegisterCode), it => it.RegisterCode.Contains(queryCoditionByCheckResult.RegisterCode))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), it => it.Name.Contains(queryCoditionByCheckResult.Name))
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionLevel), it => it.InstitutionGradeCode == queryCoditionByCheckResult.InstitutionLevel)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), it => it.ICDCode == queryCoditionByCheckResult.ICDCode)
                        .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), it => it.IdNumber == queryCoditionByCheckResult.IdNumber).ToPageList(page, limit, ref totalcount);
                }
                return datalist;
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
        public List<Check_BeForeResultPreInfo> GetBeforeResultDetailList(string registerCode, int page, int limit, ref int totalcount)
        {
            List<Check_BeForeResultPreInfo> datalist = new List<Check_BeForeResultPreInfo>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (!string.IsNullOrEmpty(registerCode))
                {
                    datalist = db.Queryable<Check_BeForeResultPreInfo>()
                        .Where(it => it.RegisterCode == registerCode)
                        .ToPageList(page, limit, ref totalcount);
                }
                else
                {
                    datalist = null;
                }
            }
                
            return datalist;
        }
    }
}
