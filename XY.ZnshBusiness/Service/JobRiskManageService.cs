using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class JobRiskManageService: IJobRiskManageService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public JobRiskManageService(IXYDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region 获取数据
        /// <summary>
        /// 根据条件列表并分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        public List<JobRiskEntity> GetPageListByCondition(string orgid,string currOrgId, string role, string job, string riskpointname, int page, int limit, ref int totalCount)
        {
            var DataResult = new List<JobRiskEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<JobRiskEntity>()
                    .OrderBy(it => it.SortCode)
                    .Where(it => it.DeleteMark == 1)
                    .WhereIF(!string.IsNullOrEmpty(orgid),it => it.OrgId == orgid)
                    .WhereIF(!string.IsNullOrEmpty(currOrgId), it => it.OrgId == currOrgId)
                    .WhereIF(!string.IsNullOrEmpty(role), it => it.RoleName.Contains(role))
                    .WhereIF(!string.IsNullOrEmpty(job), it => it.JobName.Contains(job))
                    .WhereIF(!string.IsNullOrEmpty(riskpointname), it => it.RiskPointName.Contains(riskpointname))
                    .ToPageList(page, limit, ref totalCount);
                
            }
            return DataResult;
        }
        public List<UserJobEntity> GetUserListByJobId(string jobid)
        {
            var DataResult = new List<UserJobEntity>();
            using (var db = _dbContext.GetIntance())
            {
                    DataResult = db.Queryable<UserJobEntity>()
                        .Where(it => it.DeleteMark == 1 && it.JobId == jobid)
                        .ToList();
            }
            return DataResult;
        }
        public List<UserRoleEntity> GetUserListByRoleId(string roleid)
        {
            var DataResult = new List<UserRoleEntity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<UserRoleEntity>()
                    .Where(it => it.RoleId == roleid)
                    .ToList();
            }
            return DataResult;
        }
        #endregion


        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Insert(JobRiskEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(entity).ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool Update(JobRiskEntity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Updateable(entity)
                .IgnoreColumns(it => new { it.DeleteMark, it.CreateTime })
                .Where(it => it.Id == entity.Id)
                .ExecuteCommand();
                result = count > 0 ? true : false;
            }
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool Delete(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                using (var db = _dbContext.GetIntance())
                {

                    var entity = new JobRiskEntity();
                    entity.DeleteMark = 0;
                    //逻辑删除
                    var count = db.Updateable(entity).UpdateColumns(it => new { it.DeleteMark })
                    .Where(it => it.Id == keyValue)
                    .ExecuteCommand();
                    return count > 0 ? true : false;

                }
            }
            else
            {
                result = false;
            }
            return result;
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
                    var entity = new JobRiskEntity();
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

        public bool IsExist(string userid, string bh, string id)
        {
            bool reslut;
            using (var db = _dbContext.GetIntance())
            {
                if (string.IsNullOrEmpty(id))
                {
                    reslut = db.Queryable<JobRiskEntity>().Any(it => it.UserId == userid && it.RiskPointBH == bh && it.DeleteMark == 1);
                }
                else
                {
                    reslut = db.Queryable<JobRiskEntity>().Any(it => it.UserId == userid && it.RiskPointBH == bh && it.Id != id && it.DeleteMark == 1);
                }
            }
            return reslut;
        }
        #endregion
    }
}
