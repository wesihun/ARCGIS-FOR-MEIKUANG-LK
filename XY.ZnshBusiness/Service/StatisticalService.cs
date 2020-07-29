using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.EchartsModel;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class StatisticalService: IStatisticalService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public StatisticalService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CommonModel> GetRiskInfoTJ(string orgid)
        {
            var data = new List<CommonModel>();
            using (var db = _dbContext.GetIntance())
            {
                int TotalCount = db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid).Count();
                CommonModel model1 = new CommonModel();
                model1.Name = "重大风险";
                model1.Count = db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.RiskLevel == "1" && it.OrgId == orgid).Count();
                model1.Percentum = Math.Round(Convert.ToDecimal(model1.Count)/Convert.ToDecimal(TotalCount), 4) * 100;
                data.Add(model1);
                CommonModel model2 = new CommonModel();
                model2.Name = "较大风险";
                model2.Count = db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.RiskLevel == "2" && it.OrgId == orgid).Count();
                model2.Percentum = Math.Round(Convert.ToDecimal(model2.Count) / Convert.ToDecimal(TotalCount), 4) * 100;
                data.Add(model2);
                CommonModel model3 = new CommonModel();
                model3.Name = "一般风险";
                model3.Count = db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.RiskLevel == "3" && it.OrgId == orgid).Count();
                model3.Percentum = Math.Round(Convert.ToDecimal(model3.Count) / Convert.ToDecimal(TotalCount), 4) * 100;
                data.Add(model3);
                CommonModel model4 = new CommonModel();
                model4.Name = "低风险";
                model4.Count = db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.RiskLevel == "4" && it.OrgId == orgid).Count();
                model4.Percentum = Math.Round(Convert.ToDecimal(model4.Count) / Convert.ToDecimal(TotalCount), 4) * 100;
                data.Add(model4);
            }
            return data;
        }
        public List<RiskInfoByDep> GetRiskInfoTJByDep(string orgid)
        {
            var data = new List<RiskInfoByDep>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<RiskInfoByDep>(@"select xx.a1DepName as Name,count(xx.RiskLevel) as Count, CASE xx.RiskLevel WHEN '1' THEN '重大风险' WHEN '2' THEN  
                                                        '较大风险' WHEN '3' THEN '重大风险' WHEN '4' THEN '一般风险' ELSE '' END RiskName,xx.RiskLevel from (select * from                     
                                                        (SELECT DepName as a1DepName,RiskPointBH as a1RiskPointBH,RiskPointName as a1RiskPointName FROM jobrisk where OrgId = '"+ orgid +"' "+
                                                        "and DeleteMark = 1 group by DepName,RiskPointBH,RiskPointName) a1 "+
                                                        "left join riskclassification a2 on a1.a1RiskPointBH = a2.RiskPointBH and a2.OrgId = '"+ orgid +"' and a2.DeleteMark = 1) xx "+
                                                         "group by xx.a1DepName,xx.RiskLevel");
            }
            return data;
        }
        public List<CommonModel> GetDataAnalysis(string orgid, DateTime date)
        {
            var data = new List<CommonModel>();
            using (var db = _dbContext.GetIntance())
            {
                CommonModel model1 = new CommonModel();
                model1.Name = "运行天数";
                model1.Count = CommonHelper.GetDateTimeSubtract(date, DateTime.Now); ;
                data.Add(model1);
                CommonModel model2 = new CommonModel();
                model2.Name = "使用人数";
                model2.Count = db.Queryable<UserEntity>().Where(it => it.DeleteMark == 1 && it.OrganizeId == orgid).Count();
                data.Add(model2);
                CommonModel model3 = new CommonModel();
                model3.Name = "排查次数";
                model3.Count = db.Queryable<CheckResultRecordEntity>().Where(it => it.OrgId == orgid).Count();
                data.Add(model3);
            }
            return data;
        }

        public List<RiskLevelCount> GetRiskLevelCount(string orgid, int year)
        {
            var data = new List<RiskLevelCount>();
            using (var db = _dbContext.GetIntance())
            {
                RiskLevelCount model1 = new RiskLevelCount();
                model1.Name = "第一季度";
                model1.Counts = new List<int>();   
                model1.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "1" && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                model1.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "2" && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                model1.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "3" && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                model1.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "4" && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                data.Add(model1);
                RiskLevelCount model2 = new RiskLevelCount();
                model2.Counts = new List<int>();
                model2.Name = "第二季度";
                model2.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "1" && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).Count());
                model2.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "2" && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).Count());
                model2.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "3" && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).Count());
                model2.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "4" && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).Count());
                data.Add(model2);
                RiskLevelCount model3 = new RiskLevelCount();
                model3.Counts = new List<int>();
                model3.Name = "第三季度";
                model3.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "1" && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).Count());
                model3.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "2" && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).Count());
                model3.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "3" && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).Count());
                model3.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "4" && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).Count());
                data.Add(model3);
                RiskLevelCount model4 = new RiskLevelCount();
                model4.Counts = new List<int>();
                model4.Name = "第四季度";
                model4.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "1" && it.CreateTime.Month >= 10 && it.CreateTime.Month <= 12).Count());
                model4.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "2" && it.CreateTime.Month >= 10 && it.CreateTime.Month <= 12).Count());
                model4.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "3" && it.CreateTime.Month >= 10 && it.CreateTime.Month <= 12).Count());
                model4.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year &&it.RiskLevel == "4" && it.CreateTime.Month >= 10 && it.CreateTime.Month <= 12).Count());
                data.Add(model4);
            }
            return data;
        }

        public List<RiskLevelCount> GetRiskIdentifyCount(string orgid, int year)
        {
            var data = new List<RiskLevelCount>();
            using (var db = _dbContext.GetIntance())
            {
                RiskLevelCount model1 = new RiskLevelCount();
                model1.Name = "第一季度";
                model1.Counts = new List<int>();
                model1.Counts.Add(db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                model1.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).GroupBy(it => it.RiskFactor).Count());
                data.Add(model1);
                RiskLevelCount model2 = new RiskLevelCount();
                model2.Counts = new List<int>();
                model2.Name = "第二季度";
                model2.Counts.Add(db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).Count());
                model2.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 4 && it.CreateTime.Month <= 6).GroupBy(it => it.RiskFactor).Count());
                data.Add(model2);
                RiskLevelCount model3 = new RiskLevelCount();
                model3.Counts = new List<int>();
                model3.Name = "第三季度";
                model3.Counts.Add(db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).Count());
                model3.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 7 && it.CreateTime.Month <= 9).GroupBy(it => it.RiskFactor).Count());
                data.Add(model3);
                RiskLevelCount model4 = new RiskLevelCount();
                model4.Counts = new List<int>();
                model4.Name = "第四季度";
                model4.Counts.Add(db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 1 && it.CreateTime.Month <= 3).Count());
                model4.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month >= 10 && it.CreateTime.Month <= 12).GroupBy(it => it.RiskFactor).Count());
                data.Add(model4);
            }
            return data;
        }
        
        public List<CommonModelNameCount> GetRiskFactorTypeTJ(string orgid)
        {
            var data = new List<CommonModelNameCount>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<RiskClassIficationEntity, DataDictEntity>((rcie, dde) => new object[] {
                    JoinType.Left,rcie.RiskFactorType == dde.ItemCode && dde.DataType == DataDictConst.RISK_FACTOR,
                }).Where((rcie, dde) => rcie.DeleteMark == 1 && dde.DeleteMark == 1).WhereIF(!string.IsNullOrEmpty(orgid), (rcie, dde) => rcie.OrgId == orgid && dde.OrgId == orgid).GroupBy((rcie, dde) => dde.ItemName)
               .Select((rcie, dde) => new CommonModelNameCount
               {
                   Name = dde.ItemName,
                   Count = SqlFunc.AggregateCount(dde.ItemName)
               }).ToList();

            }
            return data;
        }
        public List<CommonModelNameCount> GetRiskAccidentTypeTJ(string orgid)
        {
            var data = new List<CommonModelNameCount>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<RiskClassIficationEntity, DataDictEntity>((rcie, dde) => new object[] {
                    JoinType.Left,rcie.RiskFactorType == dde.ItemCode && dde.DataType == DataDictConst.ACCIDENT_TYPE,
                }).Where((rcie, dde) => rcie.DeleteMark == 1 && dde.DeleteMark == 1).WhereIF(!string.IsNullOrEmpty(orgid), (rcie, dde) => rcie.OrgId == orgid && dde.OrgId == orgid).GroupBy((rcie, dde) => dde.ItemName)
               .Select((rcie, dde) => new CommonModelNameCount
               {
                   Name = dde.ItemName,
                   Count = SqlFunc.AggregateCount(dde.ItemName)
               }).ToList();
            }
            return data;
        }
        public List<RiskLevelCount> GetRiskByLevelTJ(string orgid)
        {
            var data = new List<RiskLevelCount>();
            using (var db = _dbContext.GetIntance())
            {
                var riskPointNameList = db.Queryable<RiskPointEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid).GroupBy(it => it.Name).Select(it => it.Name).ToList();
                foreach(var obj in riskPointNameList)
                {
                    RiskLevelCount model = new RiskLevelCount();
                    model.Name = obj;
                    model.Counts = new List<int>();
                    model.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.RiskPointName == obj && it.RiskLevel == "1").Count());
                    model.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.RiskPointName == obj && it.RiskLevel == "2").Count());
                    model.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.RiskPointName == obj && it.RiskLevel == "3").Count());
                    model.Counts.Add(db.Queryable<RiskClassIficationEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.RiskPointName == obj && it.RiskLevel == "4").Count());
                    data.Add(model);
                }
            }
            return data;
        }

        public List<CommonModelNameCount> GetHiddenTrendTJ(string orgid,int year)
        {
            var data = new List<CommonModelNameCount>();
            using (var db = _dbContext.GetIntance())
            {
                for(int i = 1; i < 13; i++)
                {
                    CommonModelNameCount model1 = new CommonModelNameCount();
                    model1.Name = i.ToString() + "月";
                    model1.Count = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.OrgId == orgid && it.CreateTime.Year == year && it.CreateTime.Month == i).Count();
                    data.Add(model1);
                }
            }
            return data;
        }

        public List<CommonModel> GetRiskUnitTJ()
        {
            var data = new List<CommonModel>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Ado.SqlQuery<CommonModel>(@"SELECT a1.UnitName as Name,count(a1.OrgId) as Value FROM riskunit a1 join riskpoint a2                     
                                                    on a1.OrgId = a2.OrgId and a1.UnitBH = a2.RiskUnitBH where a1.DeleteMark = 1 and a2.DeleteMark = 1
                                                    group by a1.OrgId, a1.UnitBH, a1.UnitName");
            }  
            return data;
        }
        public List<HiddenTJ> GetHiddenTJ()
        {
            var data = new List<HiddenTJ>();
            using (var db = _dbContext.GetIntance())
            {
                HiddenTJ model1 = new HiddenTJ();
                model1.Name = "重大隐患";
                model1.Count1 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.HiddenDangersLevel == "1").Count();
                model1.Count2 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.HiddenDangersLevel == "1" && it.States == "3").Count();
                data.Add(model1);
                HiddenTJ model2 = new HiddenTJ();
                model2.Name = "一般隐患"; 
                model2.Count1 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.HiddenDangersLevel == "2").Count();
                model2.Count2 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.DeleteMark == 1 && it.HiddenDangersLevel == "2" && it.States == "3").Count();
                data.Add(model2);
                //HiddenTJ model3 = new HiddenTJ();
                //model3.Name = "未上报";
                //model3.Count1 = db.Queryable<CheckTableEntity>().Where(it => it.DeleteMark == 1).Count() - model1.Count - model2.Count;
                //data.Add(model3);
            }
            return data;
        }
        public CountDataModel GetCountData()
        {
            var data = new CountDataModel();
            using (var db = _dbContext.GetIntance())
            {
                data.Count1 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.HiddenDangersLevel == "1" && it.DeleteMark == 1).ToList().Count;
                data.Count11 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.HiddenDangersLevel == "1" && it.DeleteMark == 1 && it.States == "3").ToList().Count;
                data.Count2 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.HiddenDangersLevel == "2" && it.DeleteMark == 1).ToList().Count;
                data.Count22 = db.Queryable<HiddenDangersReportEntity>().Where(it => it.HiddenDangersLevel == "2" && it.DeleteMark == 1 && it.States == "3").ToList().Count;
                data.Count3 = db.Queryable<HiddenDangersNoticeEntity>().Where(it => it.States == "2").ToList().Count;
                data.Count4 = db.Queryable<HiddenDangersNoticeEntity>().Where(it => it.States == "1" && it.DeleteMark == 1).ToList().Count;
            }
            return data;
        }

        public List<CommonModel> RiskFactorStatistical()
        {
            var data = new List<CommonModel>();
            using (var db = _dbContext.GetIntance())
            {
                //data = db.Queryable<CommonModel>().Any(it => it.OrgId == orgid && it.UnitBH == bh && it.DeleteMark == 1);
            }
            return data;
        }
        public List<CheckPlanEnity> PlanStatistical()
        {
            var data = new List<CheckPlanEnity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<CheckPlanEnity>()
                    .Where(it => it.DeleteMark == 1).ToList();
                if (data.Count == 0)
                {
                    return data;
                }
                else
                {
                    int days = 0;
                    foreach(var list in data)
                    {
                        if (list.LastCompleteTime == null)
                        {
                            days = CommonHelper.GetDateTimeSubtract(list.CheckDate, DateTime.Now);
                        }
                        else
                        {
                            days = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(list.LastCompleteTime.ToString()), DateTime.Now);
                        }
                        if (list.ExecutionMode == "1")
                        {
                            if (days >= 1)
                            {
                                list.states = "0";
                            }
                            else
                            {
                                list.states = "1";
                            }
                        }
                        else
                        {
                            if (days % Convert.ToInt32(list.ExecutionMode) == 0)
                            {
                                list.states = "1";
                            }
                            else
                            {
                                list.states = "0";
                            }
                        }
                    }
                }
            }
            return data;
        }

        public List<CheckResultRecordEntity> CheckResultStatistical(string orgid,string currOrgId,string states,string planname, string riskpointname, int page, int limit, ref int totalCount)
        {
            var data = new List<CheckResultRecordEntity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<CheckResultRecordEntity, CheckTableEntity>((re, oe) => new object[] {
                    JoinType.Left,re.CheckTableId == oe.Id
                }).WhereIF(!string.IsNullOrEmpty(orgid), (re, oe) => re.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(states), (re, oe) => re.States == Convert.ToInt32(states))
                    .WhereIF(!string.IsNullOrEmpty(planname), (re, oe) => re.CheckPlanName.Contains(planname))
                    .WhereIF(!string.IsNullOrEmpty(riskpointname), (re, oe) => re.RiskPointName.Contains(riskpointname)) 
                    .WhereIF(!string.IsNullOrEmpty(currOrgId),(re,oe) => re.OrgId == currOrgId)
                .Select((re, oe) => new CheckResultRecordEntity
                {
                    CheckPlanId = re.CheckPlanId,
                    CheckTableId = re.CheckTableId,
                    Id = re.Id,
                    CheckPlanName = re.CheckPlanName,
                    CreateTime = re.CreateTime,
                    OrgId = re.OrgId,
                    OrgName = re.OrgName,
                    RiskPointBH = re.RiskPointBH,
                    RiskPointName = re.RiskPointName,
                    States = re.States,
                    UserId = re.UserId,
                    UserName = re.UserName,
                    TroubleshootingItems = oe.TroubleshootingItems
                }).ToPageList(page, limit, ref totalCount);
            }
            return data;
        }
        public List<DownLoadEntity> GetOIList(int page, int limit, ref int totalCount)
        {
            var data = new List<DownLoadEntity>();
            using (var db = _dbContext.GetIntance())
            {
                data = db.Queryable<DownLoadEntity>().Where(it => it.DeleteMark == 1).OrderBy(it => it.SortCode).ToPageList(page, limit, ref totalCount);
            }
            return data;
        }
    }
}
