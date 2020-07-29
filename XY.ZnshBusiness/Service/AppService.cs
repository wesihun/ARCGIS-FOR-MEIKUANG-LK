using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class AppService: IAppService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public AppService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CheckPlanEnity GetPlanList(string userid)
        {
            var list = new CheckPlanEnity();
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<CheckPlanEnity>()
                    .Where(it => it.DeleteMark == 1 && it.UserId == userid).First();    
            }
            return list;
        }

        public List<CheckPlanEnity> GetAllPlanList(string orgid,string currOrgId, string planName, string riskpointName, string states, int page, int limit, ref int totalCount)
        {
            var dataResult = new List<CheckPlanEnity>();
            using (var db = _dbContext.GetIntance())
            {
                dataResult = db.Queryable<CheckPlanEnity>()
                    .Where(it => it.DeleteMark == 1)
                    .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                    .WhereIF(!string.IsNullOrEmpty(planName), it => it.PlanName.Contains(planName))
                    .WhereIF(!string.IsNullOrEmpty(riskpointName), it => it.RiskName.Contains(riskpointName))                  
                    .ToList();
               

                if (dataResult.Count == 0)
                {
                    return dataResult;
                }
                else
                {
                    foreach(var list in dataResult)
                    {
                        string rolename = "";
                        string roleid = "";
                        if (db.Queryable<UserRoleEntity>().Any(it => it.UserId == list.UserId))
                        {
                            roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == list.UserId).First().RoleId;
                            if (db.Queryable<RoleEntity>().Any(it => it.DeleteMark == 1 && it.RoleId == roleid))
                            {
                                rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                            }
                        }
                        int days = 0;
                        if (list.LastCompleteTime == null)
                        {
                            list.states = "0";
                        }
                        else
                        {

                            days = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(list.LastCompleteTime.ToString()), DateTime.Now);
                            string days1 = string.Empty;   //重大风险天数
                            string days2 = string.Empty;   //较大风险天数
                            string days3 = string.Empty;   //一般风险天数
                            if (rolename == "班组长")
                            {
                                days1 = db.Queryable<DataDictEntity>().Where(it => it.DataType == "008" && it.ItemName == "重大风险").First().ItemCode;
                                days2 = db.Queryable<DataDictEntity>().Where(it => it.DataType == "008" && it.ItemName == "较大风险").First().ItemCode;
                                days3 = db.Queryable<DataDictEntity>().Where(it => it.DataType == "008" && it.ItemName == "一般风险").First().ItemCode;
                                if (list.RiskBH.Contains(","))
                                {
                                    string[] names = list.RiskName.Split(',');
                                    int i = 0;
                                    //获取重大风险总数
                                    int Count1 = db.Queryable<CheckTableEntity>().Where(it => it.DeleteMark == 1 && it.UserId == list.UserId && list.RiskBH.Split(',').Contains(it.RiskPointBH) && it.RiskLevel == "1").Count();
                                    //获取当前已完成重大风险总数
                                    int Count11 = db.Queryable<CheckResultRecordEntity>().Where(it => it.CheckPlanId == list.Id && it.UserId == list.UserId && it.CreateTime >= DateTime.Now.AddDays(Convert.ToDouble(days1))).Count();
                                    if (Count1 == Count11)
                                    {
                                        list.states = "1";
                                    }
                                    //foreach (var obj in list.RiskBH.Split(','))
                                    //{

                                    //}
                                }
                                else
                                {

                                }
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
                                if (days != 0 && days % Convert.ToInt32(list.ExecutionMode) == 0)
                                {
                                    list.states = "0";
                                }
                                else if (days == 0)
                                {
                                    list.states = "1";
                                }
                                else
                                {
                                    list.states = "2";
                                }
                            }
                        }
                        
                    }
                }
                if (!string.IsNullOrEmpty(states))
                {
                    dataResult = dataResult.Where(it => it.states == states).ToList();
                }
            }
            totalCount = dataResult.Count();
            return dataResult.Skip((page - 1) * limit).Take(limit).ToList();
        }
        public List<RiskPointDto> GetRiskPointList(string userid)
        {
            var DataResult = new List<RiskPointDto>();
            using (var db = _dbContext.GetIntance())
            {
                var planEntity = db.Queryable<CheckPlanEnity>()
                    .Where(it => it.DeleteMark == 1 && it.UserId == userid).First(); 
                if(planEntity == null)
                {
                    return null;
                }
                else
                {
                    if (planEntity.RiskBH.Contains(","))
                    {
                        string ExecutionMode = planEntity.ExecutionMode;
                        string[] names = planEntity.RiskName.Split(',');
                        int i = 0;
                        foreach (var obj in planEntity.RiskBH.Split(','))
                        {   
                            RiskPointDto riskPointDto = new RiskPointDto();
                            riskPointDto.RiskBH = obj;
                            riskPointDto.RiskName = names[i];
                            int count1 = db.Queryable<CheckTableEntity>()
                            .Where(it => it.DeleteMark == 1 && it.UserId == userid && it.RiskPointBH == obj).Count();
                            if(ExecutionMode == "1")
                            {
                                int count2 = db.Queryable<CheckResultRecordEntity>()
                                .Where(it => it.RiskPointBH == obj && it.UserId == userid && it.CreateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")).Count();
                                if (count1 == count2)
                                {
                                    riskPointDto.states = "1";
                                }
                                else
                                {
                                    riskPointDto.states = "0";
                                }
                            }
                            else
                            {
                                var checkDate = db.Queryable<CheckPlanEnity>().Where(it => it.DeleteMark == 1 && it.UserId == userid).First().CheckDate;
                                int leadtime = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(checkDate.ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                                if(leadtime % Convert.ToInt32(ExecutionMode) != 0)
                                {
                                    riskPointDto.states = "2";
                                }
                                else
                                {
                                    if (db.Queryable<CheckResultRecordEntity>().Any(it => it.RiskPointBH == obj && it.UserId == userid))
                                    {
                                        DateTime completeTime = db.Queryable<CheckResultRecordEntity>()
                                                .Where(it => it.RiskPointBH == obj && it.UserId == userid).OrderBy(it => it.CreateTime, OrderByType.Desc).First().CreateTime;
                                        int days = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(completeTime.ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                                        int count2 = db.Queryable<CheckResultRecordEntity>()
                                        .Where(it => it.RiskPointBH == obj && it.UserId == userid && it.CreateTime == completeTime).Count();
                                        if (days % Convert.ToInt32(ExecutionMode) == 0 && count1 == count2)
                                        {
                                            riskPointDto.states = "1";
                                        }
                                        if (days % Convert.ToInt32(ExecutionMode) == 0 && count1 != count2)
                                        {
                                            riskPointDto.states = "0";
                                        }
                                        else
                                        {
                                            riskPointDto.states = "2";
                                        }
                                    }
                                    else
                                    {
                                        var checkdate = planEntity.CheckDate;
                                        int days = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(checkdate.ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                                        if (days % Convert.ToInt32(ExecutionMode) == 0)
                                        {
                                            riskPointDto.states = "0";
                                        }
                                        else
                                        {
                                            riskPointDto.states = "2";
                                        }
                                    }
                                }
                                
                            }                           
                            i++;
                            DataResult.Add(riskPointDto);
                        }                
                    }
                    else
                    {
                        string ExecutionMode = planEntity.ExecutionMode;
                        RiskPointDto riskPointDto = new RiskPointDto();
                        riskPointDto.RiskBH = planEntity.RiskBH;
                        riskPointDto.RiskName = planEntity.RiskName;
                        int count1 = db.Queryable<CheckTableEntity>()
                        .Where(it => it.DeleteMark == 1 && it.UserId == userid && it.RiskPointBH == planEntity.RiskBH).Count();
                        if (ExecutionMode == "1")
                        {
                            int count2 = db.Queryable<CheckResultRecordEntity>()
                            .Where(it => it.RiskPointBH == planEntity.RiskBH && it.UserId == userid && it.CreateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")).Count();
                            if (count1 == count2)
                            {
                                riskPointDto.states = "1";
                            }
                            else
                            {
                                riskPointDto.states = "0";
                            }
                        }
                        else
                        {
                            if (db.Queryable<CheckResultRecordEntity>().Any(it => it.RiskPointBH == planEntity.RiskBH && it.UserId == userid))
                            {
                                DateTime completeTime2 = db.Queryable<CheckResultRecordEntity>()
                                        .Where(it => it.RiskPointBH == planEntity.RiskBH && it.UserId == userid).OrderBy(it => it.CreateTime, OrderByType.Desc).First().CreateTime;
                                int days2 = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(completeTime2.ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                                int count22 = db.Queryable<CheckResultRecordEntity>()
                                .Where(it => it.RiskPointBH == planEntity.RiskBH && it.UserId == userid && it.CreateTime == completeTime2).Count();
                                if (days2 % Convert.ToInt32(ExecutionMode) == 0 && count1 == count22)
                                {
                                    riskPointDto.states = "1";
                                }
                                if (days2 % Convert.ToInt32(ExecutionMode) == 0 && count1 != count22)
                                {
                                    riskPointDto.states = "0";
                                }
                                else
                                {
                                    riskPointDto.states = "2";
                                }
                            }
                            else
                            {
                                var checkdate = planEntity.CheckDate;
                                int days2 = CommonHelper.GetDateTimeSubtract(Convert.ToDateTime(checkdate.ToString("yyyy-MM-dd")), Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")));
                                if (days2 % Convert.ToInt32(ExecutionMode) == 0)
                                {
                                    riskPointDto.states = "0";
                                }
                                else
                                {
                                    riskPointDto.states = "2";
                                }
                            }
                        }                      
                        DataResult.Add(riskPointDto);
                    }
                } 
                
            }
            return DataResult;
        }
        public List<CheckTableEntity> GetQRCodeList(string riskBH, string userId)
        {
            var DataResult = new List<CheckTableEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckTableEntity>()
                    .Where(it => it.DeleteMark == 1 && it.RiskPointBH == riskBH && it.UserId == userId)
                    .ToList();
                string ExecutionMode = db.Queryable<CheckPlanEnity>()
                   .Where(it => it.DeleteMark == 1 && it.UserId == userId).First().ExecutionMode;
                foreach (var obj in DataResult)
                {
                    if (db.Queryable<CheckResultRecordEntity>().Any(it => it.CheckTableId == obj.Id && it.CreateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        obj.states1 = "1";
                    }
                    else
                    {
                        obj.states1 = "0";
                    }
                    if (db.Queryable<HiddenDangersReportEntity>().Any(it => it.CheckTableId == obj.Id && it.CreateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        obj.states2 = "1";
                        obj.states1 = "0";
                    }
                    else
                    {
                        obj.states2 = "0";
                    }
                    //////上报状态判断     
                }
                
            }
            return DataResult;
        }
        //public bool IsExitCurrUserId(string riskBH, string userId)
        //{

        //}
        public List<CheckPlanEnity> GetPlanCheckList()
        {
            var DataResult = new List<CheckPlanEnity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckPlanEnity>()
                    .Where(it => it.DeleteMark == 1)
                    .ToList();
                if(DataResult.Count() == 0)
                {
                    return DataResult;
                }
                else
                {
                    foreach (var list in DataResult)
                    {
                        int days = 0;
                        if (list.LastCompleteTime == null)
                        {
                            days = CommonHelper.GetDateTimeSubtract(list.CheckDate, DateTime.Now);
                            list.states = "0";
                        }
                        else
                        {
                            days = CommonHelper.GetDateTimeSubtract(list.CheckDate, DateTime.Now);
                        }

                        if(list.ExecutionMode == "1")
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
                            if (days != 0 && days % Convert.ToInt32(list.ExecutionMode) == 0)
                            {
                                list.states = "0";
                            }
                            else if (days == 0)
                            {
                                list.states = "1";
                            }
                            else
                            {
                                list.states = "2";
                            }
                        }
                    }
                }
            }
            return DataResult;
        }
        public List<RiskNoticeDto> GetRiskNoticeList(string userid)
        {
            var DataResult = new List<RiskNoticeDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<JobRiskEntity>().Where(it => it.DeleteMark == 1 && it.UserId == userid)
                    .OrderBy(it => it.CreateTime, OrderByType.Desc)
                    .Select(it => new RiskNoticeDto
                    {                        
                        RiskBH = it.RiskPointBH,
                        RiskName = it.RiskPointName
                    }).ToList();
                //DataResult = db.Queryable<JobRiskEntity, RiskClassIficationEntity>((de1, de2) => new object[] {
                //JoinType.Left,de1.RiskPointBH == de2.RiskPointBH
                //}).Where((de1, de2) => de1.DeleteMark == 1 && de2.DeleteMark == 1 && de1.OrgId == de2.OrgId && de1.UserId == userid)
                //    .OrderBy((de1, de2) => de1.CreateTime, OrderByType.Desc)
                //    .Select((de1, de2) => new RiskNoticeDto
                //    {
                //        AccidentType = de2.AccidentType,
                //        RiskBH = de2.RiskPointBH,
                //        RiskFactor = de2.RiskFactor,
                //        RiskLevel = de2.RiskLevel,
                //        RiskName = de2.RiskPointName,
                //        TroubleshootingItems = de2.TroubleshootingItems,
                //        ControlMeasures = de2.ControlMeasures
                //    }).ToList();
            }
            return DataResult;
        }
        public bool GetQRCodeCreat(string id,string userid,string username)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var entity = db.Queryable<CheckTableEntity>()
                    .Where(it => it.Id == id)
                    .First();
                    var palanEntity = db.Queryable<CheckPlanEnity>().Where(it => it.UserId == userid && it.RiskBH.Contains(entity.RiskPointBH)).First();
                    CheckResultRecordEntity result = new CheckResultRecordEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CheckPlanName = palanEntity.PlanName,
                        CheckPlanId = palanEntity.Id,
                        CheckTableId = entity.Id,
                        RiskPointBH = entity.RiskPointBH,
                        RiskPointName = entity.RiskPointName,
                        CreateTime = DateTime.Now,
                        UserId = userid,
                        UserName = username,
                        States = 0,
                        OrgId = palanEntity.OrgId,
                        OrgName = palanEntity.OrgName
                    };
                    db.Insertable(result).ExecuteCommand();
                    var checkPlanEntity = db.Queryable<CheckPlanEnity>().Where(it => it.DeleteMark == 1 && it.UserId == userid).First();
                    var count1 = db.Queryable<CheckTableEntity>().Where(it => it.DeleteMark == 1 && it.UserId == userid && checkPlanEntity.RiskBH.Contains(it.RiskPointBH)).Count();
                    var count2 = db.Queryable<CheckResultRecordEntity>().Where(it => it.CreateTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") && it.UserId == userid && checkPlanEntity.RiskBH.Contains(it.RiskPointBH)).Count();
                    if (count1 == count2)
                    {
                        CheckPlanEnity updateEntity = new CheckPlanEnity()
                        {
                            LastCompleteTime = DateTime.Today
                        };
                        db.Updateable(updateEntity)
                        .UpdateColumns(it => new { it.LastCompleteTime })
                        .Where(it => it.Id == checkPlanEntity.Id)
                        .ExecuteCommand();
                        PlanLogEntity planLogEntity = new PlanLogEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = userid,
                            CompleteTime = DateTime.Now,
                            PlanId = palanEntity.Id
                        };
                        db.Insertable(planLogEntity).ExecuteCommand();
                    }
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    return false;
                }
                return true;
            }
        }

        public bool IsExitCurrUserId(string riskBH, string userId)
        {
            using (var db = _dbContext.GetIntance())
            {
                return db.Queryable<CheckTableEntity>().Any(it => it.DeleteMark == 1 && it.UserId == userId && it.RiskPointBH == riskBH);
            }
        }
    }
}
