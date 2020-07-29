using ICSharpCode.SharpZipLib.Zip;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class CheckTableService: ICheckTableService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public CheckTableService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 根据条件列表并分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        public List<CheckTableEntity> GetPageListByCondition(string condition, string keyword,string orgid,string currOrgId, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<CheckTableEntity>();
            using (var db = _dbContext.GetIntance())
            {
                if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                {
                    DataResult = db.Queryable<CheckTableEntity>()
                        .WhereIF(condition == "RiskPointName", it => it.RiskPointName == keyword)
                        .WhereIF(condition == "RiskLevel", it => it.RiskLevel == keyword)
                        .WhereIF(condition == "RiskPointBH", it => it.RiskPointBH == keyword)
                        .Where(it => it.DeleteMark == 1)
                        .WhereIF(!string.IsNullOrEmpty(orgid),it => it.OrgId == orgid)
                        .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                        .ToPageList(page, limit, ref totalCount);
                }
                else
                {
                    DataResult = db.Queryable<CheckTableEntity>()
                       .Where(it => it.DeleteMark == 1)
                       .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrgId == orgid)
                       .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                       .ToPageList(page, limit, ref totalCount);
                }
            }
            return DataResult;
        }
        public List<CheckTableEntity> GetCheckPointSelect(string orgid,string userid, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<CheckTableEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckTableEntity>()
                    .Where(it => it.DeleteMark == 1)
                    .WhereIF(!string.IsNullOrEmpty(orgid), it => it.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(userid), it => it.UserId == userid)
                    .GroupBy(it => new { it.RiskPointBH,it.RiskPointName})
                    .Select( it => new CheckTableEntity
                    {
                        RiskPointBH = it.RiskPointBH,
                        RiskPointName = it.RiskPointName
                    })
                    .ToPageList(page, limit, ref totalCount);
            }
            return DataResult;
        }
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="userRoleEntity">角色用户实体</param>
        /// <returns></returns>
        public bool InsertBatch(List<AuthorizedDto> models)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    var entity = new CheckTableEntity();
                    entity.DeleteMark = 0;
                    for (int i = 0; i < models.Count(); i++)
                    {

                        var riskClassIficationEntity = db.Queryable<RiskClassIficationEntity>().Where(it => it.Id == models[i].classificationid).First();
                        var jobRiskEntity = db.Queryable<JobRiskEntity>().Where(it => it.Id == models[i].jobriskid).First();
                        if(riskClassIficationEntity == null)
                        {
                            db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark }).Where(it => it.UserId == jobRiskEntity.UserId).ExecuteCommand();
                            return true;
                            //db.Deleteable<CheckTableEntity>().Where(it => it.UserId == jobRiskEntity.UserId).ExecuteCommand();
                        }
                        else
                        {
                            db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark }).Where(it => it.UserId == jobRiskEntity.UserId && it.RiskPointBH == riskClassIficationEntity.RiskPointBH).ExecuteCommand();
                        }
                                             
                    }
                    for (int i = 0; i < models.Count(); i++)
                    {
                        var riskClassIficationEntity = db.Queryable<RiskClassIficationEntity>().Where(it => it.Id == models[i].classificationid).First();
                        var jobRiskEntity = db.Queryable<JobRiskEntity>().Where(it => it.Id == models[i].jobriskid).First();
                        CheckTableEntity checkTableEntitie = new CheckTableEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ClassificationId = models[i].classificationid,
                            CreateTime = DateTime.Now,
                            DeleteMark = 1,
                            JobId = jobRiskEntity.JobId,
                            JobName = jobRiskEntity.JobName,
                            OrgId = jobRiskEntity.OrgId,
                            OrgName = jobRiskEntity.OrgName,
                            RoleId = jobRiskEntity.RoleId,
                            RoleName = jobRiskEntity.RoleName,
                            UserId = jobRiskEntity.UserId,
                            UserName = jobRiskEntity.UserName,
                            RiskFactor = riskClassIficationEntity.RiskFactor,
                            RiskLevel = riskClassIficationEntity.RiskLevel,
                            RiskPointBH = riskClassIficationEntity.RiskPointBH,
                            RiskPointName = riskClassIficationEntity.RiskPointName,
                            TroubleshootingItems = riskClassIficationEntity.TroubleshootingItems,
                            ControlMeasures = riskClassIficationEntity.ControlMeasures,
                            MeasuresType = riskClassIficationEntity.MeasuresType,
                            EmergencyMeasures = riskClassIficationEntity.EmergencyMeasures
                        };
                        db.Insertable(checkTableEntitie).ExecuteCommand();
                    }
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw ex;
                }
                return true;
            }


        }
        /// <summary>
        /// 获取过滤的风险点分级列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        public List<RiskClassIficationEntity> GetClassClassIficationList(int page, int limit, ref int totalCount)
        {
            var DataResult = new List<RiskClassIficationEntity>();
            using (var db = _dbContext.GetIntance())
            {
                var classificationIdlist = db.Queryable<CheckTableEntity>().Where(it => it.DeleteMark == 1)
                    .Select(it => it.ClassificationId).ToList();
                if(classificationIdlist.Count() == 0)
                {
                    DataResult = db.Queryable<RiskClassIficationEntity>()
                   .Where(it => it.DeleteMark == 1)
                   .ToPageList(page, limit, ref totalCount);
                }
                else
                {
                    DataResult = db.Queryable<RiskClassIficationEntity>()
                   .Where(it => it.DeleteMark == 1 && !classificationIdlist.Contains(it.Id))
                   .ToPageList(page, limit, ref totalCount);
                }
               
            }
            return DataResult;
        }

        public List<CheckTableEntity> GetCheckItems(string userid, string riskpointbh)
        {
            var DataResult = new List<CheckTableEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckTableEntity>()
                    .Where(it => it.DeleteMark == 1)
                    .WhereIF(!string.IsNullOrEmpty(userid),it => it.UserId == userid)
                    .WhereIF(!string.IsNullOrEmpty(riskpointbh), it => it.RiskPointBH == riskpointbh)
                    .ToList();
            }
            return DataResult;
        }

        public bool Insert(List<string> classificationId)
        {
            var list = new List<CheckTableEntity>();
           
            using (var db = _dbContext.GetIntance())
            {
                list = db.Queryable<RiskClassIficationEntity, JobRiskEntity>((de1, de2) => new object[] {
                    JoinType.Left,de1.RiskPointBH == de2.RiskPointBH
                    }).Where((de1, de2) => classificationId.Contains(de1.Id) && de1.DeleteMark == 1 && de2.DeleteMark == 1 && de1.OrgId == de2.OrgId)
                    .Select((de1, de2) => new CheckTableEntity
                    {
                      ClassificationId = de1.Id,
                      DeleteMark = 1,
                      RiskFactor = de1.RiskFactor,
                      RiskLevel = de1.RiskLevel,
                      RiskPointBH = de1.RiskPointBH,
                      RiskPointName = de1.RiskPointName,
                      OrgId = de1.OrgId,
                      OrgName = de1.OrgName,
                      RoleId = de2.RoleId,
                      RoleName = de2.RoleName,
                      JobId = de2.JobId,
                      JobName = de2.JobName,
                      UserId = de2.UserId,
                      UserName = de2.UserName,
                      TroubleshootingItems = de1.TroubleshootingItems,

                    }).ToList();
                if(list.Count() == 0)
                {
                    return false;
                }
                else
                {
                    foreach(var obj in list)
                    {
                        obj.Id = Guid.NewGuid().ToString();
                        obj.CreateTime = DateTime.Now;
                    }
                    return db.Insertable(list.ToArray()).ExecuteCommand() > 0 ? true : false;
                }
                
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键List</param>
        public bool DeleteBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var entity = new CheckTableEntity();
                    entity.DeleteMark = 0;
                    //逻辑删除
                    var counts = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                        .Where(it => keyValues.Contains(it.Id)).ExecuteCommand();
                    result = counts > 0 ? result = true : false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
