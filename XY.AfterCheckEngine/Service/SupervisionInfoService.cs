using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;
using SqlSugar;
using Newtonsoft.Json;
using XY.AfterCheckEngine.Entities.Dto;
using XY.SystemManage.Entities;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.Service
{
    public class SupervisionInfoService: ISupervisionInfoService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public SupervisionInfoService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }

        #region 获取数据
        /// <summary>
        /// 获取医保门诊信息
        /// </summary>
        /// <param name="queryConditionByClinic"></param>
        /// <returns></returns>
        public List<YBClinicInfoEntity> GetYBClinicInfoByCondition(QueryConditionByClinic queryConditionByClinic, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBClinicInfoEntity>();           
            if (queryConditionByClinic.InstitutionCode != "")//就诊机构编码不为空
            {
                //判断缓存
                string ybClinicInfo_redisKey = SystemManageConst.YBINFO_CLINIC + queryConditionByClinic.InstitutionCode + queryConditionByClinic.Count;//医保门诊缓存Key值 （由常量Key和机构编码组成）
                using (var redisdb = _redisDbContext.GetRedisIntance())
                {
                    dataResult = redisdb.Get<List<YBClinicInfoEntity>>(ybClinicInfo_redisKey);//从缓存里取
                    if (dataResult == null)
                    {
                        using (var db = _dbContext.GetIntance()) //从数据库中
                        {
                            if (!string.IsNullOrEmpty(queryConditionByClinic.Count) && queryConditionByClinic.Count.Contains(">"))
                            {
                                dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                                 it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                                .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY >= queryConditionByClinic.YBBXFY)//统筹支付金额
                                .Having(it => SqlFunc.AggregateCount(it.IdNumber) >= Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                                .OrderBy(it => it.ClinicDates, OrderByType.Desc)
                                .ToPageList(pageIndex, pageSize, ref totalCount);
                            }
                            else
                            {
                                dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                                it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY >= queryConditionByClinic.YBBXFY)//统筹支付金额
                               .Having(it => SqlFunc.AggregateCount(it.IdNumber) == Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                               .OrderBy(it => it.ClinicDates, OrderByType.Desc)
                               .ToPageList(pageIndex, pageSize, ref totalCount);
                            }
                        }
                        if (dataResult != null)//加入缓存
                        {
                            redisdb.Set(ybClinicInfo_redisKey, dataResult);
                            redisdb.Expire(ybClinicInfo_redisKey, 86400);//设置缓存时间
                        }
                    }
                }
            }
            else
            {//就诊机构代码为空，从数据库中获取全部数据
                using (var db = _dbContext.GetIntance())
                {
                    if (!string.IsNullOrEmpty(queryConditionByClinic.Count) && queryConditionByClinic.Count.Contains(">"))
                    {
                        dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                       .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                                it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY >= queryConditionByClinic.YBBXFY)//统筹支付金额
                               .Having(it => SqlFunc.AggregateCount(it.IdNumber) >= Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                               .OrderBy(it => it.ClinicDates, OrderByType.Desc)
                               .ToPageList(pageIndex, pageSize, ref totalCount);
                    }
                    else
                    {
                        dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                       .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                                it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                               .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY >= queryConditionByClinic.YBBXFY)//统筹支付金额
                               .Having(it => SqlFunc.AggregateCount(it.IdNumber) == Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                               .OrderBy(it => it.ClinicDates, OrderByType.Desc)
                               .ToPageList(pageIndex, pageSize, ref totalCount);
                    }
                }
            }

            return dataResult;
        }

        /// <summary>
        /// 获取医保门诊信息
        /// </summary>
        /// <param name="queryCondition">json字符串</param>
        /// <returns></returns>
        public List<YBClinicInfoDto> GetYBClinicInfo(string queryCondition,int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBClinicInfoDto>();
            QueryConditionByClinic queryConditionByClinic = JsonConvert.DeserializeObject<QueryConditionByClinic>(queryCondition);

            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (!string.IsNullOrEmpty(queryConditionByClinic.Count) && queryConditionByClinic.Count.Contains(">"))
                {
                    dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                     it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                    .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY >= queryConditionByClinic.YBBXFY)//统筹支付金额
                    .Having(it => SqlFunc.AggregateCount(it.IdNumber) >= Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                    .OrderBy(it => it.ClinicDates,OrderByType.Desc)
                    .Select(oe => new YBClinicInfoDto
                     {
                         Name = oe.Name,
                         IdNumber = oe.IdNumber,
                         Gender = oe.Gender,
                         PersonalTypeName = oe.PersonalTypeName,
                         Counts = SqlFunc.AggregateCount(oe.ClinicDates).ToString(),
                         PersonalCode = oe.PersonalCode,
                         ClinicDates =Convert.ToDateTime(oe.ClinicDates.ToString().Substring(0,10)),
                     }).ToPageList(pageIndex, pageSize, ref totalCount);
                }
                else
                {
                    dataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDates, it.IdNumber, it.Name, it.PersonalCode, it.Gender, it.PersonalTypeName })
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.IdNumber), it => it.IdNumber.Contains(queryConditionByClinic.IdNumber))//身份证号码
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.InstitutionCode), it => it.InstitutionCode == queryConditionByClinic.InstitutionCode)//就诊机构代码
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ClinicDateYear.ToString()) && !string.IsNullOrEmpty(queryConditionByClinic.ClinicDateMonth.ToString()),
                    it => it.ClinicDateYear == queryConditionByClinic.ClinicDateYear && it.ClinicDateMonth == queryConditionByClinic.ClinicDateMonth)//就诊时间
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.ZFY.ToString()), it => it.ZFY >= queryConditionByClinic.ZFY)//总费用
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.MLNFY.ToString()), it => it.MLNFY >= queryConditionByClinic.MLNFY)//目录内费用
                   .WhereIF(!string.IsNullOrEmpty(queryConditionByClinic.YBBXFY.ToString()), it => it.YBBXFY>= queryConditionByClinic.YBBXFY)//统筹支付金额
                   .OrderBy(it => it.ClinicDates, OrderByType.Desc)
                   .Having(it => SqlFunc.AggregateCount(it.IdNumber) == Convert.ToInt32(queryConditionByClinic.Count.Replace(">", "")))
                    .Select(oe => new YBClinicInfoDto
                    {
                        Name = oe.Name,
                        IdNumber = oe.IdNumber,
                        Gender = oe.Gender,
                        PersonalTypeName = oe.PersonalTypeName,
                        Counts = SqlFunc.AggregateCount(oe.ClinicDates).ToString(),
                        PersonalCode = oe.PersonalCode,
                        ClinicDates = Convert.ToDateTime(oe.ClinicDates.ToString().Substring(0, 10)),
                    }).ToPageList(pageIndex, pageSize, ref totalCount);
                }
                return dataResult;
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
        public bool IsCityYBJ(string OrganizeId)
        {
            if (OrganizeId.Substring(0, 2) == "15")   //为医院
            {
                return false;
            }
            else
            {
                using (var db = _dbContext.GetIntance())
                {
                    var entity = db.Queryable<OrganizeEntity>().Where(it => it.DeleteMark == 1 && it.OrganizeId == OrganizeId).First();
                    if (entity.OrgLevel == "4")   //市级
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
        /// 根据条件查询门诊信息列表
        /// </summary>
        /// <param name="clinicDate">就诊时间</param>
        /// <param name="idNumber">身份证号码</param>
        /// <param name="personCode">人员编码</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBClinicInfoEntity> GetYBClinicInfoList(string clinicDate, string idNumber, string personCode, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBClinicInfoEntity>();
            using (var db = _dbContext.GetIntance()) //从数据库中
            {
                if (!string.IsNullOrEmpty(clinicDate) && !string.IsNullOrEmpty(idNumber)&& !string.IsNullOrEmpty(personCode))
                {
                    dataResult = db.Queryable<YBClinicInfoEntity>().Where(it=>it.ClinicDates==Convert.ToDateTime(clinicDate)&&it.IdNumber==idNumber&&it.PersonalCode==it.PersonalCode)
                    .OrderBy(it=>it.ClinicDate, OrderByType.Desc).ToPageList(pageIndex, pageSize, ref totalCount);
                }
                else
                {
                    dataResult = null; 
                }
            }
            return dataResult;
        }


        /// <summary>
        /// 根据住院编码，行政区划，年度获取门诊登记信息
        /// </summary>
        /// <param name="clinicRegisterCode"></param>
        /// <returns></returns>
        public YBClinicInfoEntity GetYBClinicInfoEntity(string clinicRegisterCode,string cityAreaCode,string year)
        {
            var dataResult = new YBClinicInfoEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<YBClinicInfoEntity>().Where(it => it.ClinicRegisterCode == clinicRegisterCode&&it.Year==year&&it.CityAreaCode==cityAreaCode).ToList().SingleOrDefault();
            }
            return dataResult;
        }
        /// <summary>
        /// 根据住院登记编码、个人编码获取住院信息
        /// </summary>
        /// <param name="hosRegisterCode"></param>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        public YBHosInfoEntity GetYBHosInfoEntityByCondition(string hosRegisterCode, string personalCode)
        {
            var dataResult = new YBHosInfoEntity();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<YBHosInfoEntity>().Where(it => it.HosRegisterCode == hosRegisterCode && it.PersonalCode == personalCode).ToList().SingleOrDefault();
            }
            return dataResult;
        }
        /// <summary>
        /// 获取处方列表
        /// </summary>
        /// <param name="clinicRegisterCode">门诊登记编码</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBClinicPreInfoEntity> GetYBClinicPreInfoList(string clinicRegisterCode, int pageIndex, int pageSize, ref int totalCount)
        {
            var dataResult = new List<YBClinicPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<YBClinicPreInfoEntity>().Where(it => it.ClinicRegisterCode == clinicRegisterCode).OrderBy(it=>it.ItemIndex).ToPageList(pageIndex,pageSize,ref totalCount);
            }
            return dataResult;
        }
        /// <summary>
        /// 获取住院列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyValue"></param>
        /// <param name="idNumber"></param>
        /// <param name="institutionCode"></param>
        /// <param name="inHosDate"></param>
        /// <param name="outHosDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CheckComplaintMainDto> GetYBHosInfoList(string condition, string keyValue, string idNumber, string institutionCode, string inHosDate, string outHosDate, string rulescode,string datatype, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<CheckComplaintMainDto>();
            using (var db = _dbContext.GetIntance())
            {
                if(!string.IsNullOrEmpty(datatype) && datatype == "2")    //住院
                {
                    DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                    JoinType.Left,et1.RegisterCode == et2.HosRegisterCode,
                    }).Where((et1, et2) => et1.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(datatype), (et1, et2) => et1.DataType == datatype)
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "ZFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.ZFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "MLNFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.MLNFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "YBBXFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.YBBXFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(idNumber), (et1, et2) => et2.IdNumber.Contains(idNumber))
                    .WhereIF(!string.IsNullOrEmpty(institutionCode), (et1, et2) => et2.InstitutionCode.Contains(institutionCode))
                    .WhereIF(!string.IsNullOrEmpty(inHosDate), (et1, et2) => et2.InHosDate == inHosDate.ObjToDate())
                    .WhereIF(!string.IsNullOrEmpty(outHosDate), (et1, et2) => et2.OutHosDate == outHosDate.ObjToDate())
                    .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                    .Select((et1, et2) => new CheckComplaintMainDto
                    {
                        HosRegisterCode = et2.HosRegisterCode,
                        Name = et2.Name,
                        IdNumber = et2.IdNumber,
                        Gender = et2.Gender,
                        Age = et2.Age,
                        PersonalCode = et2.PersonalCode,
                        PersonalTypeName = et2.PersonalTypeName,
                        FolkName = et2.FolkName,
                        InstitutionName = et2.InstitutionName,
                        InstitutiongGradeName = et2.InstitutiongGradeName,
                        InstitutionLevelName = et2.InstitutionLevelName,
                        InHosDate = et2.InHosDate,
                        OutHosDate = et2.OutHosDate,
                        InHosDay = et2.InHosDay,
                        DiseaseName = et2.DiseaseName,
                        CompYear = et2.CompYear,
                        CityAreaName = et2.CityAreaName,
                        ZFY = et2.ZFY,
                        YBBXFY = et2.YBBXFY,
                        GRZFFY = et2.GRZFFY,
                        MLNFY = et2.MLNFY,
                        MLWFY = et2.MLWFY,
                        XYF = et2.Age,
                        ZYF = et2.ZYF,
                        CYF = et2.CYF,
                        MYF = et2.MYF,
                        JCF = et2.JCF,
                        CLF = et2.CLF,
                        TCF = et2.TCF,
                        ZLF = et2.ZLF,
                        HYF = et2.HYF,
                        SSF = et2.SSF,
                        XUEYF = et2.XUEYF,
                        TJF = et2.TJF,
                        TZF = et2.TZF,
                        QTF = et2.QTF,
                        BXYF = et2.BXYF
                       }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else if(!string.IsNullOrEmpty(datatype) && datatype == "1")
                {
                    DataResult = db.Queryable<CheckResultInfoEntity, YBClinicInfoEntity>((et1, et2) => new object[] {
                    JoinType.Left,et1.RegisterCode == et2.ClinicRegisterCode,
                    }).Where((et1, et2) => et1.RulesCode == rulescode)
                    .WhereIF(!string.IsNullOrEmpty(datatype), (et1, et2) => et1.DataType == datatype)
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "ZFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.ZFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "MLNFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.MLNFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(condition) && condition == "YBBXFY" && !string.IsNullOrEmpty(keyValue), (et1, et2) => et2.YBBXFY >= keyValue.ObjToDecimal())
                    .WhereIF(!string.IsNullOrEmpty(idNumber), (et1, et2) => et2.IdNumber.Contains(idNumber))
                    .WhereIF(!string.IsNullOrEmpty(institutionCode), (et1, et2) => et2.InstitutionCode.Contains(institutionCode))
                    .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                    .Select((et1, et2) => new CheckComplaintMainDto
                    {
                        HosRegisterCode = et2.ClinicRegisterCode,
                        Name = et2.Name,
                        IdNumber = et2.IdNumber,
                        Gender = et2.Gender,
                        Age = Convert.ToInt32(et2.Age),
                        PersonalCode = et2.PersonalCode,
                        PersonalTypeName = et2.PersonalTypeName,
                        FolkName = et2.FolkName,
                        InstitutionName = et2.InstitutionName,
                        InstitutiongGradeName = et2.InstitutiongGradeName,
                        InstitutionLevelName = et2.InstitutionLevelName,
                        DiseaseName = et2.DiseaseName,
                        CompYear = et2.CompYear,
                        CityAreaName = et2.CityAreaName,
                        ZFY = Convert.ToDecimal(et2.ZFY),
                        YBBXFY = Convert.ToDecimal(et2.YBBXFY),
                        GRZFFY = Convert.ToDecimal(et2.GRZFFY),
                        MLNFY = Convert.ToDecimal(et2.MLNFY),
                        MLWFY = Convert.ToDecimal(et2.MLWFY),
                        XYF = Convert.ToDecimal(et2.Age),
                        ZYF = Convert.ToDecimal(et2.ZYF),
                        CYF = Convert.ToDecimal(et2.CYF),
                        MYF = Convert.ToDecimal(et2.MYF),
                        JCF = Convert.ToDecimal(et2.JCF),
                        CLF = Convert.ToDecimal(et2.CLF),
                        TCF = Convert.ToDecimal(et2.TCF),
                        ZLF = Convert.ToDecimal(et2.ZLF),
                        HYF = Convert.ToDecimal(et2.HYF),
                        SSF = Convert.ToDecimal(et2.SSF),
                        XUEYF = Convert.ToDecimal(et2.XUEYF),
                        TJF = Convert.ToDecimal(et2.TJF),
                        TZF = Convert.ToDecimal(et2.TZF),
                        QTF = Convert.ToDecimal(et2.QTF),
                        BXYF = Convert.ToDecimal(et2.BXYF)
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = null;
                }
                
            }
            return DataResult;
        }

        public bool RepeatCheckComplaint(string[] rulescode, string registercode,string describe,UserInfo userInfo,decimal money)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var updatelist = db.Queryable<CheckComplaintEntity>().Where(it => it.RegisterCode == registercode && it.ComplaintResultStatus == ((int)CheckStates.G).ToString()).ToList();
                    foreach (var obj in updatelist)
                    {
                        if (rulescode.Contains(obj.RulesCode))
                        {
                            obj.ComplaintResultStatus = ((int)CheckStates.E).ToString();
                        }
                        else
                        {
                            obj.ComplaintResultStatus = ((int)CheckStates.D).ToString();
                        }
                        db.Updateable(obj).Where(it => it.ComplaintCode == obj.ComplaintCode).ExecuteCommand();
                    }
                    
                    if (rulescode.Length > 0)
                    {
                        db.Updateable<Check_ComplainEntity>().UpdateColumns(it => new Check_ComplainEntity()
                        {
                            RealMoneySecond = money,
                            SecondTrialTime = DateTime.Now,
                            SecondTrialUserId = userInfo.UserId,
                            SecondTrialUserName = userInfo.UserName,
                            SecondTrialInstitutionCode = userInfo.InstitutionCode,
                            SecondTrialInstitutionName = userInfo.InstitutionName,
                            ComplaintStatus = ((int)CheckStates.E).ToString(),
                            SecondTrialDescribe = describe
                        }).Where(it => it.RegisterCode == registercode).ExecuteCommand();
                        db.Updateable<YBHosInfoEntity>().UpdateColumns(it => new YBHosInfoEntity() { States = ((int)CheckStates.E).ToString() })
                             .Where(it => it.HosRegisterCode == registercode).ExecuteCommand();
                    }
                    else
                    {
                        db.Updateable<Check_ComplainEntity>().UpdateColumns(it => new Check_ComplainEntity()
                        {
                            SecondTrialTime = DateTime.Now,
                            SecondTrialUserId = userInfo.UserId,
                            SecondTrialUserName = userInfo.UserName,
                            SecondTrialInstitutionCode = userInfo.InstitutionCode,
                            SecondTrialInstitutionName = userInfo.InstitutionName,
                            ComplaintStatus = ((int)CheckStates.D).ToString(),
                            SecondTrialDescribe = describe
                        }).Where(it => it.RegisterCode == registercode).ExecuteCommand();
                        db.Updateable<YBHosInfoEntity>().UpdateColumns(it => new YBHosInfoEntity() { States = ((int)CheckStates.D).ToString() })
                             .Where(it => it.HosRegisterCode == registercode).ExecuteCommand();
                    }                                     
                    db.Ado.CommitTran();                
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
            }
            int count = 0;
            using (var db = _dbContext.GetIntanceForYb())
            {
                if (rulescode.Length > 0)
                {
                    count = db.Updateable<CompHosCostMainEntity>().UpdateColumns(it => new CompHosCostMainEntity() { CheckStates = CheckStates.E.ToString() })
                                .Where(it => it.HCM_HosRegisterCode_Vc == registercode).ExecuteCommand();
                }
                else
                {
                    count = db.Updateable<CompHosCostMainEntity>().UpdateColumns(it => new CompHosCostMainEntity() { CheckStates = CheckStates.D.ToString() })
                                .Where(it => it.HCM_HosRegisterCode_Vc == registercode).ExecuteCommand();
                }
            }
            if(count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// 获取申诉列表
        /// <returns></returns>
        public List<CheckResultInfoEntity> GetComplaintInfoList(string rulesCode, QueryCoditionByCheckResult queryCoditionByCheckResult, bool isadmin, string curryydm, int page, int limit, ref int totalcount)
        {
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (isadmin)
                {
                    DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                    JoinType.Left,a.RegisterCode == b.RegisterCode,
                    })
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(rulesCode), (a, b) => b.RulesCode == rulesCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.PersonalCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Where((a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .GroupBy((a, b) => new { b.Name, b.IdNumber, b.RegisterCode, b.Gender, b.Age, b.InstitutionName })
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
                    })
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                    .WhereIF(!string.IsNullOrEmpty(rulesCode), (a, b) => b.RulesCode == rulesCode)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                    .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                    .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.PersonalCode)
                    .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                    .Where((a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                    .Where((a, b) => b.InstitutionCode == curryydm)
                    .GroupBy((a, b) => new { b.Name, b.IdNumber, b.RegisterCode, b.Gender, b.Age, b.InstitutionName })
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
                else
                {
                    var XAreaCode = returnXAreaCode(curryydm).Substring(0,6);
                        DataResult = db.Queryable<Check_ComplainEntity, CheckComplaintEntity>((a, b) => new object[] {
                        JoinType.Left,a.RegisterCode == b.RegisterCode,
                        })
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.InstitutionCode), (a, b) => b.InstitutionCode == queryCoditionByCheckResult.InstitutionCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.Name), (a, b) => b.Name.Contains(queryCoditionByCheckResult.Name))
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.IdNumber), (a, b) => b.IdNumber.Contains(queryCoditionByCheckResult.IdNumber))
                       .WhereIF(!string.IsNullOrEmpty(rulesCode), (a, b) => b.RulesCode == rulesCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.ICDCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.ICDCode)
                       .WhereIF(!string.IsNullOrEmpty(queryCoditionByCheckResult.PersonalCode), (a, b) => b.ICDCode == queryCoditionByCheckResult.PersonalCode)
                       .WhereIF(queryCoditionByCheckResult.StartSettleTime != null, (a, b) => b.SettlementDate >= queryCoditionByCheckResult.StartSettleTime)
                       .WhereIF(queryCoditionByCheckResult.EndSettleTime != null, (a, b) => b.SettlementDate <= queryCoditionByCheckResult.EndSettleTime)
                       .Where((a, b) => a.ComplaintStatus == ((int)CheckStates.G).ToString())
                       .Where((a, b) => b.InstitutionCode.StartsWith(XAreaCode))
                       .GroupBy((a, b) => new { b.Name, b.IdNumber, b.RegisterCode, b.Gender, b.Age, b.InstitutionName })
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
        /// <summary>
        /// 获取审核结果列表
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="personalcode"></param>
        /// <param name="rulescode"></param>
        /// <returns></returns>
        public List<CheckResultInfoEntity> GetCheckResultInfoList(string key)
        {
            var DataResult = new List<CheckResultInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity>().Where(et1 => et1.CheckResultInfoCode == key)
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取审核结果处方列表
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="personalcode"></param>
        /// <param name="rulescode"></param>
        /// <returns></returns>
        public List<CheckResultPreInfoEntity> GetCheckResultPreInfoList(string key)
        {
            var DataResult = new List<CheckResultPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultPreInfoEntity>().Where(et1 => et1.CheckResultInfoCode == key)
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取审核结果处方列表
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="personalcode"></param>
        /// <param name="rulescode"></param>
        /// <returns></returns>
        public List<CheckResultPreInfoEntity> GetWGDescribe(string key)
        {
            var DataResult = new List<CheckResultPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultPreInfoEntity>().Where(et1 => et1.CheckResultInfoCode == key)
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取申诉列表
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="personalcode"></param>
        /// <param name="rulescode"></param>
        /// <returns></returns>
        public List<CheckComplaintEntity> GetCheckComplaintInfoList(string registercode, string personalcode, string rulescode)
        {
            var DataResult = new List<CheckComplaintEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckComplaintEntity>().Where(et1 => et1.RulesCode == rulescode && et1.RegisterCode == registercode && et1.PersonalCode == personalcode)
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取违规处方列表(住院)
        /// </summary>
        /// <returns></returns>
        public List<HosPreInfo_WGDto> GetWGCFDeatilListByKey(string key,int pageIndex, int pageSize, ref int totalCount)
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
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
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
        public List<HosPreInfo_WGDto> GetWGCFDeatilList(string hosregistercode,string rulecode, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<HosPreInfo_WGDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(hosregistercode))
                {
                    DataResult = db.Queryable<YBHosPreInfoEntity, CheckResultPreInfoEntity>((a, b) => new object[] {
                    JoinType.Left,a.PreCode == b.PreCode&&a.ItemIndex==b.ItemIndex&&a.HosRegisterCode==b.RegisterCode,
                    }).Where((a, b) => b.RulesCode == rulecode && a.HosRegisterCode == hosregistercode)
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
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }      
        /// <summary>
        /// 获取违规处方列表(门诊)
        /// </summary>
        /// <returns></returns>
        public List<CliPreInfo_WGDto> GetWGCFDeatilListCli(string hosregistercode, string rulecode, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<CliPreInfo_WGDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(hosregistercode))
                {
                    DataResult = db.Queryable<YBClinicPreInfoEntity, CheckResultPreInfoEntity>((a, b) => new object[] {
                    JoinType.Left,a.PreCode == b.PreCode&&a.ItemIndex==b.ItemIndex&&a.ClinicRegisterCode==b.RegisterCode,
                    }).Where((a, b) => b.RulesCode == rulecode && a.ClinicRegisterCode == hosregistercode)
                    .Select((a, b) => new CliPreInfo_WGDto
                    {
                        ClinicRegisterCode = a.ClinicRegisterCode,
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
                    }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }
        /// <summary>
        /// 根据月份汇总就诊或违规人次
        /// </summary>
        /// <param name="year">汇总年度</param>
        /// <param name="flag">1代表住院2 代表门诊</param>
        /// <param name="status">1代表就诊2代表违规就诊</param>
        /// <returns></returns>
        public List<MonthCountEntity> GetMonthCountList(string year, string flag, string status)
        {
            var DataResult = new List<MonthCountEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(year))
                {
                    if (flag == "1")//住院
                    {
                        DataResult = db.Queryable<YBHosInfoEntity>().GroupBy(it => new { it.InHosMonth})
                            .WhereIF(!string.IsNullOrEmpty(status)&&status=="2", it => it.States!="1")
                            .Where(it => it.InHosDate>Convert.ToDateTime(year+"-01-01"))
                            .OrderBy(it => it.InHosMonth)
                            .Select(it => new MonthCountEntity { Month=it.InHosMonth,Count = SqlFunc.AggregateCount(it.InHosMonth).ToString() })
                            .ToList();
                    }
                    else//门诊
                    {
                        DataResult = db.Queryable<YBClinicInfoEntity>().GroupBy(it => new { it.ClinicDateMonth })
                            .WhereIF(!string.IsNullOrEmpty(status) && status == "2", it => it.States != "1")
                            .Where(it => it.ClinicDate > Convert.ToDateTime(year + "-01-01"))
                            .OrderBy(it => it.ClinicDateMonth)
                            .Select(it => new MonthCountEntity { Month = it.ClinicDateMonth.ToString(), Count = SqlFunc.AggregateCount(it.ClinicDateMonth).ToString() })
                            .ToList();
                    }
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }

        public List<CheckComplaintDetailEntity> GetCLInfo(string CheckComplainId, string datatype)
        {
            var DataResult = new List<CheckComplaintDetailEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(CheckComplainId))
                {
                    DataResult = db.Queryable<CheckComplaintDetailEntity>()
                        .WhereIF(!string.IsNullOrEmpty(datatype),it => it.Datatype == datatype)
                        .Where(it => it.CheckComplainId == CheckComplainId).ToList();             
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }
       
        #endregion
    }
}
