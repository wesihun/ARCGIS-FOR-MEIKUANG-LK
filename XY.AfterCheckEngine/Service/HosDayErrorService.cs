using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;
using XY.ZnshBusiness.Entities;

namespace XY.AfterCheckEngine.Service
{
    public class HosDayErrorService : IHosDayErrorService
    {
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;

        public HosDayErrorService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
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
                    DataResult = db.Queryable<YBHosPreInfoEntity>().Where(it => it.HosRegisterCode == hosregistercode).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = null;
                }
               
            }
            return DataResult;
        }
        /// <summary>
        /// 获取处方明细list门诊 
        /// </summary>
        /// <returns></returns>
        public List<YBClinicPreInfoEntity> GetCFDeatilListCli(string hosregistercode, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<YBClinicPreInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(hosregistercode))
                {
                    DataResult = db.Queryable<YBClinicPreInfoEntity>().Where(it => it.ClinicRegisterCode == hosregistercode).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = null;
                }

            }
            return DataResult;
        }
        /// <summary>
        /// 获取医院机构信息列表
        /// </summary>
        /// <returns></returns>

        public List<YYXXDto> GetYYXXList(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<YYXXDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (String.IsNullOrEmpty(condition))
                {
                    DataResult = db.Queryable<YYXXEntity>().Where(oe => oe.DeleteMark == 1).OrderBy((oe) => oe.CreateTime, OrderByType.Desc)
                           .Select(it => new YYXXDto
                           {
                               YYDMYYDM = it.YYDMYYDM,
                               YLJGMC = it.YLJGMC,
                               YLJGDJMC = it.YLJGDJMC
                           }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = db.Queryable<YYXXEntity>().Where(oe => oe.DeleteMark == 1 && (oe.YYDMYYDM.Contains(condition) || oe.YLJGMC.Contains(condition))).OrderBy((oe) => oe.CreateTime, OrderByType.Desc)
                           .Select(it => new YYXXDto
                           {
                               YYDMYYDM = it.YYDMYYDM,
                               YLJGMC = it.YLJGMC,
                               YLJGDJMC = it.YLJGDJMC
                           }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();

                }
            }
            return DataResult;
        }

        public List<DiseaseDirectoryDto> GetDiseaseList(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<DiseaseDirectoryDto>();
            using (var db = _dbContext.GetIntance())
            {
                if (String.IsNullOrEmpty(condition))
                {
                    DataResult = db.Queryable<DiseaseDirectoryEntity>().Where(oe => oe.DeleteMark == 1).OrderBy((oe) => oe.CreateTime, OrderByType.Desc)
                           .Select(it => new DiseaseDirectoryDto
                           {
                                DiseaseName=it.DiseaseName,
                                ICDCode=it.ICDCode
                           }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
                }
                else
                {
                    DataResult = db.Queryable<DiseaseDirectoryEntity>().Where(oe => oe.DeleteMark == 1 && (oe.DiseaseName.Contains(condition) || oe.ICDCode.Contains(condition))).OrderBy((oe) => oe.CreateTime, OrderByType.Desc)
                           .Select(it => new DiseaseDirectoryDto
                           {
                               DiseaseName = it.DiseaseName,
                               ICDCode = it.ICDCode
                           }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();

                }
            }
            return DataResult;
        }

        /// <summary>
        /// 根据条件获取住院天数异常信息列表并分页
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBHosInfoEntity> GetPageListByCondition(string condition, string keyword, string idnumber, string yljgbh, int pageIndex, int pageSize, ref int totalCount)
        {
            var DataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                JoinType.Left,et1.RegisterCode == et2.HosRegisterCode,
                }).Where((et1, et2) => et1.RulesCode == "A003")
                .WhereIF(!string.IsNullOrEmpty(condition)&& condition == "ZFY" && !string.IsNullOrEmpty(keyword),(et1, et2) => et2.ZFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "MLNFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.MLNFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "YBBXFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.YBBXFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(idnumber), (et1, et2) => et2.IdNumber.Contains(idnumber))
                .WhereIF(!string.IsNullOrEmpty(yljgbh), (et1, et2) => et2.InstitutionCode.Contains(yljgbh))
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                .Select((et1, et2) => new YBHosInfoEntity
                {
                    HosRegisterCode = et2.HosRegisterCode,
                    Name = et2.Name,
                    IdNumber = et2.IdNumber,
                    Gender = et2.Gender,
                    Age = et2.Age,
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
            return DataResult;
        }
        /// <summary>
        /// 根据编码获取分解住院信息
        /// </summary>
        /// <param name="institutioncode">就诊机构编码</param>
        public List<YBHosInfoEntity> GetDecomposehosByCode(string idnumber, string institutioncode)
        {
            var DataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                JoinType.Left,et1.PersonalCode == et2.PersonalCode,
                }).Where((et1, et2) => et1.RulesCode == "A004" && et1.IdNumber == idnumber && et2.InstitutionCode == institutioncode)               
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                .Select((et1, et2) => new YBHosInfoEntity
                {
                    HosRegisterCode = et2.HosRegisterCode,
                    PersonalCode = et1.PersonalCode,
                    InstitutionCode = et1.InstitutionCode,
                    Name = et2.Name,
                    IdNumber = et2.IdNumber,
                    Gender = et2.Gender,
                    Age = et2.Age,
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
                }).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据条件获取分解住院信息列表并分页
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBHosInfoEntity> GetDecomposeHos(string condition, string keyword, string querystr, int pageIndex, int pageSize, ref int totalCount)
        {
            DecomposeHosQuery result = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<DecomposeHosQuery>(querystr);
            }
            var DataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                JoinType.Left,et1.RegisterCode == et2.HosRegisterCode,
                }).Where((et1, et2) => et1.RulesCode == "A004")
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "ZFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.ZFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "MLNFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.MLNFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "YBBXFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.YBBXFY >= keyword.ObjToDecimal())
                .WhereIF(result !=null && !string.IsNullOrEmpty(result.IdNumber), (et1, et2) => et2.IdNumber.Contains(result.IdNumber))
                .WhereIF(result != null && !string.IsNullOrEmpty(result.InstitutionCode), (et1, et2) => et2.InstitutionCode.Equals(result.InstitutionCode))
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                .Select((et1, et2) => new YBHosInfoEntity
                {  
                    PersonalCode = et1.PersonalCode,
                    InstitutionCode = et1.InstitutionCode,
                    Name = et2.Name,
                    IdNumber = et2.IdNumber,
                    Gender = et2.Gender,
                    Age = et2.Age,
                    PersonalTypeName = et2.PersonalTypeName,                
                }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据编码获取入出院日期异常信息
        /// </summary>
        /// <param name="institutioncode">就诊机构编码</param>
        public List<YBHosInfoEntity> GetInOutDateByCode(string idnumber, string institutioncode)
        {
            var DataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                JoinType.Left,et1.PersonalCode == et2.PersonalCode,
                }).Where((et1, et2) => et1.RulesCode == "A006" && et1.IdNumber == idnumber )
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                .Select((et1, et2) => new YBHosInfoEntity
                {
                    HosRegisterCode = et2.HosRegisterCode,
                    PersonalCode = et1.PersonalCode,
                    InstitutionCode = et1.InstitutionCode,
                    Name = et2.Name,
                    IdNumber = et2.IdNumber,
                    Gender = et2.Gender,
                    Age = et2.Age,
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
                }).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 根据条件获取入出院日期异常信息列表并分页
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<YBHosInfoEntity> GetInOutDate(string condition, string keyword, string querystr, int pageIndex, int pageSize, ref int totalCount)
        {
            DecomposeHosQuery result = null;
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<DecomposeHosQuery>(querystr);
            }
            var DataResult = new List<YBHosInfoEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckResultInfoEntity, YBHosInfoEntity>((et1, et2) => new object[] {
                JoinType.Left,et1.RegisterCode == et2.HosRegisterCode,
                }).Where((et1, et2) => et1.RulesCode == "A006")
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "ZFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.ZFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "MLNFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.MLNFY >= keyword.ObjToDecimal())
                .WhereIF(!string.IsNullOrEmpty(condition) && condition == "YBBXFY" && !string.IsNullOrEmpty(keyword), (et1, et2) => et2.YBBXFY >= keyword.ObjToDecimal())
                .WhereIF(result != null && !string.IsNullOrEmpty(result.IdNumber), (et1, et2) => et2.IdNumber.Contains(result.IdNumber))
                .WhereIF(result != null && !string.IsNullOrEmpty(result.InstitutionCode), (et1, et2) => et2.InstitutionCode.Equals(result.InstitutionCode))
                .OrderBy((et1) => et1.CheckDate, OrderByType.Desc)
                .Select((et1, et2) => new YBHosInfoEntity
                {
                    PersonalCode = et1.PersonalCode,
                    InstitutionCode = et1.InstitutionCode,
                    Name = et2.Name,
                    IdNumber = et2.IdNumber,
                    Gender = et2.Gender,
                    Age = et2.Age,
                    PersonalTypeName = et2.PersonalTypeName,
                }).ToPageList(pageIndex, pageSize, ref totalCount).ToList();
            }
            return DataResult;
        }

        public List<ParameterInfoEntity> GetParameterList()
        {
            string homeindex_redisKey = SystemManageConst.HomeIndex_KEY;
            var dataResult = new List<ParameterInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {              
                dataResult = redisdb.Get<List<ParameterInfoEntity>>(homeindex_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        ParameterInfoEntity parameterInfoEntity = new ParameterInfoEntity();
                        parameterInfoEntity.HosInfoCount = db.Ado.GetInt("SELECT COUNT(1) FROM YB_HosInfo");
                        parameterInfoEntity.HosInfoCheckCount = db.Ado.GetInt("SELECT COUNT(1) FROM YB_HosInfo WHERE States != '1'");
                        parameterInfoEntity.HosInfoCheckErrorCount = db.Ado.GetInt("SELECT COUNT(1) FROM Check_ResultInfo WHERE DataType = '2'"); 
                        //审核列表
                        List<CheckResultInfoDto> checklist = db.Ado.SqlQuery<CheckResultInfoDto>(@"SELECT a1.CheckResultInfoCode,a1.RegisterCode,a1.PersonalCode,a1.InstitutionCode,a1.InstitutionName,a2.YLJGDJBM,a2.YLJGDJMC 
                                                             FROM dbo.Check_ResultInfo a1 LEFT JOIN dbo.HIS_YYXX a2 ON a1.InstitutionCode = a2.YYDMYYDM");
                        int checkcount = checklist.Count();
                        int onecount = 0;
                        int twocount = 0;
                        int threecount = 0;
                        if (checklist != null)
                        {
                            onecount = checklist.Where(it => it.YLJGDJBM == SystemManageConst.AONE).Count();  //一级
                            twocount = checklist.Where(it => it.YLJGDJBM == SystemManageConst.ATWO).Count();//二级
                            threecount = checklist.Where(it => it.YLJGDJBM == SystemManageConst.ATHREE).Count();//三级
                        }
                        parameterInfoEntity.OneProportion = Math.Floor(Math.Round(decimal.Parse(((decimal)onecount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        parameterInfoEntity.TwoProportion = Math.Floor(Math.Round(decimal.Parse(((decimal)twocount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        parameterInfoEntity.ThreeProportion = Math.Floor(Math.Round(decimal.Parse(((decimal)threecount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        dataResult = new List<ParameterInfoEntity>();
                        dataResult.Add(parameterInfoEntity);
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(homeindex_redisKey, dataResult);
                        redisdb.Expire(homeindex_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;

        }


        public List<ParameterInfoEntity> GetParameterList_Clinic()
        {
            string homeindex_redisKey = SystemManageConst.HOMEINDEXCLINIC_KEY;
            var dataResult = new List<ParameterInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                dataResult = redisdb.Get<List<ParameterInfoEntity>>(homeindex_redisKey);//从缓存里取
                if (dataResult == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        ParameterInfoEntity parameterInfoEntity = new ParameterInfoEntity();
                        parameterInfoEntity.ClinicInfoCount = db.Ado.GetInt("SELECT COUNT(1) FROM YB_ClinicInfo");
                        parameterInfoEntity.ClinicInfoCheckCount = db.Ado.GetInt("SELECT COUNT(1) FROM YB_ClinicInfo WHERE States != '1'");
                        parameterInfoEntity.ClinicInfoCheckErrorCount = db.Ado.GetInt("SELECT COUNT(1) FROM Check_ResultInfo WHERE DataType = '1'");
                        dataResult = new List<ParameterInfoEntity>();
                        dataResult.Add(parameterInfoEntity);
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(homeindex_redisKey, dataResult);
                        redisdb.Expire(homeindex_redisKey, 86400);//设置缓存时间
                    }
                }
            }
            return dataResult;
        }
        public bool AddHomeIndexParameterRedis()
        {
            string homeindex_redisKey = SystemManageConst.HomeIndex_KEY;
            var dataResult = new List<ParameterInfoEntity>();
            using (var redisdb = _redisDbContext.GetRedisIntance())
            {
                var list = redisdb.Get<List<ParameterInfoEntity>>(homeindex_redisKey);//从缓存里取
                if (list == null)
                {
                    using (var db = _dbContext.GetIntance()) //从数据库中
                    {
                        ParameterInfoEntity parameterInfoEntity = new ParameterInfoEntity();
                        var YBHosInfoEntityList = db.Queryable<YBHosInfoEntity>().ToList();
                        parameterInfoEntity.HosInfoCount = YBHosInfoEntityList.Count();
                        parameterInfoEntity.HosInfoCheckCount = YBHosInfoEntityList.Where(it => it.States != "1").Count();
                        parameterInfoEntity.HosInfoCheckErrorCount = db.Queryable<CheckResultInfoEntity>().ToList().Where(it => it.DataType == "2").Count();
                        var YBClinicInfoEntityList = db.Queryable<YBClinicInfoEntity>().ToList();
                        parameterInfoEntity.ClinicInfoCount = YBClinicInfoEntityList.Count();
                        parameterInfoEntity.ClinicInfoCheckCount = YBClinicInfoEntityList.Where(it => it.States != "1").Count();
                        parameterInfoEntity.ClinicInfoCheckErrorCount = db.Queryable<CheckResultInfoEntity>().ToList().Where(it => it.DataType == "1").Count();
                        int checkcount = db.Queryable<CheckResultInfoEntity>().ToList().Count();

                        var group = db.Queryable<CheckResultInfoEntity>().GroupBy(it => new { it.InstitutionCode, it.RegisterCode })
                                    .Select(it => new CheckResultInfoEntity
                                    {
                                        InstitutionCode = it.InstitutionCode,
                                        RegisterCode = it.RegisterCode
                                    });
                        var yyxxList1 = db.Queryable<YYXXEntity>().Where(it => it.YLJGDJBM == "8");  //一级
                        var yyxxList2 = db.Queryable<YYXXEntity>().Where(it => it.YLJGDJBM == "5");  //二级
                        var yyxxList3 = db.Queryable<YYXXEntity>().Where(it => it.YLJGDJBM == "2");  //三级
                        decimal onecount = db.Queryable(group, yyxxList1, (j1, j2) => j1.InstitutionCode == j2.YYDMYYDM).Select((j1, j2) => j1.InstitutionCode).ToList().Count().ObjToDecimal();
                        decimal twocount = db.Queryable(group, yyxxList2, (j1, j2) => j1.InstitutionCode == j2.YYDMYYDM).Select((j1, j2) => j1.InstitutionCode).ToList().Count().ObjToDecimal();
                        decimal threecount = db.Queryable(group, yyxxList3, (j1, j2) => j1.InstitutionCode == j2.YYDMYYDM).Select((j1, j2) => j1.InstitutionCode).ToList().Count().ObjToDecimal();
                        parameterInfoEntity.OneProportion = Math.Floor(Math.Round(decimal.Parse((onecount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        parameterInfoEntity.TwoProportion = Math.Floor(Math.Round(decimal.Parse((twocount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        parameterInfoEntity.ThreeProportion = Math.Floor(Math.Round(decimal.Parse((threecount / checkcount).ToString("0.000")), 2) * 100).ToString() + '%';
                        dataResult.Add(parameterInfoEntity);
                    }
                    if (dataResult != null)//加入缓存
                    {
                        redisdb.Set(homeindex_redisKey, dataResult);
                        redisdb.Expire(homeindex_redisKey, 86400);//设置缓存时间
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
