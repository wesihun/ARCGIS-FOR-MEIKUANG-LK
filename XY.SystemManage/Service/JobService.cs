using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;

namespace XY.SystemManage.Service
{
    public class JobService : IJobService
    {
        private bool result = false;
        #region 构造注入
        private readonly IXYDbContext _dbContext;
        private readonly IRedisDbContext _redisDbContext;
        public JobService(IXYDbContext dbContext, IRedisDbContext redisDbContext)
        {
            _dbContext = dbContext;
            _redisDbContext = redisDbContext;
        }
        #endregion

        public List<JobEntity> GetAllList(string id, string orgId)
        {
            var DataResult = new List<JobEntity>();

            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<JobEntity>()
                            .Where(it => it.DeleteMark == 1 && it.OrgId == orgId)
                            .OrderBy(it => it.SortCode).ToList();
            }
            return DataResult;
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="id">岗位ID</param>
        /// <returns></returns>
        public List<UserJobEntity> GetUserByJobId(string id)
        {
            var DataResult = new List<UserJobEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserJobEntity>().Where(it => it.JobId == id && it.DeleteMark == 1).ToList();
            }
            return DataResult;
        }
        public List<UserDto> GetUserSelect(string depid, string jobid)
        {
            var DataResult = new List<UserDto>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserEntity, UserJobEntity>((ue, ur) => new object[] {
                    JoinType.Left,ue.UserId == ur.UserId,
                }).Where((ue, ur) => ue.DeleteMark == 1).OrderBy((ue) => ue.SortCode)
                .WhereIF(!string.IsNullOrEmpty(depid), (ue, ur) => ue.DepId == depid)
                .WhereIF(!string.IsNullOrEmpty(jobid), (ue, ur) => ur.JobId == jobid)
                .Select((ue, ur) => new UserDto
                {
                    UserId = ue.UserId,
                    UserName = ue.UserName,
                    RealName = ue.RealName
                }).ToList();
            }
            return DataResult;
        }
        public bool Delete(string keyValue)
        {
            using (var db = _dbContext.GetIntance())
            {
                var jobEntity = new JobEntity();
                jobEntity.DeleteMark = 0;
                //逻辑删除
                var count = db.Updateable(jobEntity).UpdateColumns(it => new { it.DeleteMark })
                .Where(it => it.Id == keyValue)
                .ExecuteCommand();
                return count == 1 ? true : false;
            }
        }

        public bool DeleteBatch(List<string> keyValues)
        {
            bool result = false;
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    var jobEntity = new JobEntity();
                    jobEntity.DeleteMark = 0;
                    //逻辑删除
                    var counts = db.Updateable(jobEntity).UpdateColumns(it => new { it.DeleteMark })
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
        public List<JobEntity> GetPageListByCondition(string depid,string orgid, string depname, string jobname,int page, int limit, ref int totalCount)
        {
            var DataResult = new List<JobEntity>();

            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<JobEntity>()
                            .WhereIF(!string.IsNullOrEmpty(jobname),it=> it.Name.Contains(jobname))
                            .WhereIF(!string.IsNullOrEmpty(orgid),it => it.OrgId == orgid)
                            .WhereIF(!string.IsNullOrEmpty(depid), it => it.DepId == depid)
                            .WhereIF(!string.IsNullOrEmpty(depname), it => it.DepName.Contains(depname))
                            .Where(it => it.DeleteMark == 1)
                            .OrderBy(it => it.SortCode).ToPageList(page, limit, ref totalCount);
            }
            return DataResult;
        }

        public bool Insert(JobEntity jobEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(jobEntity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }

        public bool Update(JobEntity jobEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(jobEntity)
                .IgnoreColumns(it => new { it.CreateTime,it.DeleteMark })
                .Where(it => it.Id == jobEntity.Id)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        public bool CheckIsJobNameRepeat(string JobName, string JobId, string DepId, string OrgId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(JobId))
                {
                    reslut = db.Queryable<JobEntity>().Any(it => it.Name == JobName && it.DeleteMark == 1 && it.DepId == DepId && it.OrgId == OrgId);
                }
                else
                {
                    reslut = db.Queryable<JobEntity>().Any(it => it.Name == JobName && it.Id != JobId && it.DeleteMark == 1 && it.DepId == DepId && it.OrgId == OrgId);
                }
            }
            return reslut;
        }
        public bool CheckIsJobBHRepeat(string JobBH, string JobId, string DepId, string OrgId)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(JobId))
                {
                    reslut = db.Queryable<JobEntity>().Any(it => it.JobCode == JobBH && it.DeleteMark == 1 && it.DepId == DepId && it.OrgId == OrgId);
                }
                else
                {
                    reslut = db.Queryable<JobEntity>().Any(it => it.JobCode == JobBH && it.Id != JobId && it.DeleteMark == 1 && it.DepId == DepId && it.OrgId == OrgId);
                }
            }
            return reslut;
        }
        /// <summary>
        /// 删除校验
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool CheckIsAllocateUser(string keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    //获取角色下使用的用户
                    var resultUserRole = db.Queryable<JobEntity>().Where(it => it.Id == keyValues).ToList();
                    return resultUserRole.Count() == 0 ? true : false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除校验
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        public bool CheckIsAllocateUserBatch(List<string> keyValues)
        {
            if (keyValues.Count() > 0)
            {
                using (var db = _dbContext.GetIntance())
                {
                    //获取角色下使用的用户
                    var resultUserRole = db.Queryable<JobEntity>().Where(it => keyValues.Contains(it.Id)).ToList();
                    return resultUserRole.Count() == 0 ? true : false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="userJobEntity">实体</param>
        /// <returns></returns>
        public bool InsertBatch(List<UserJobEntity> userJobEntity)
        {
            using (var db = _dbContext.GetIntance())
            {
                try
                {
                    db.Ado.BeginTran();
                    if (userJobEntity[0].UserId != null)
                    {
                        db.Deleteable<UserJobEntity>().Where(it => it.JobId == userJobEntity[0].JobId).ExecuteCommand();                       
                        foreach (var entity in userJobEntity)
                        {
                            entity.UserName = db.Queryable<UserEntity>().Where(it => it.DeleteMark == 1 && it.UserId == entity.UserId).First().RealName;
                            db.Insertable(entity).ExecuteCommand();
                        }
                    }
                    else
                    {
                        db.Deleteable<UserJobEntity>().Where(it => it.JobId == userJobEntity[0].JobId).ExecuteCommand();
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
    }
}
