using Renci.SshNet.Security;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XY.DataNS;
using XY.SystemManage.Entities;
using XY.Universal.Models;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.Service
{
    public class CheckPlanService: ICheckPlanService
    {
        private bool result = false;
        private readonly IXYDbContext _dbContext;
        public CheckPlanService(IXYDbContext dbContext)
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
        public List<CheckPlanEnity> GetPageListByCondition(string orgid,string currOrgId, string planname, string person, string executionmodel,int page, int limit, ref int totalCount)
        {
            var DataResult = new List<CheckPlanEnity>();
            using (var db = _dbContext.GetIntance())
            {
                DataResult = db.Queryable<CheckPlanEnity, DataDictEntity>((re, oe) => new object[] {
                    JoinType.Left,re.ExecutionMode == oe.ItemCode && oe.DataType == DataDictConst.RISK_ExecutionMode
                }).Where((re, oe) => re.DeleteMark == 1 && oe.DeleteMark == 1)
                .WhereIF(!string.IsNullOrEmpty(orgid), (re, oe) => re.OrgId == orgid)
                .WhereIF(!string.IsNullOrEmpty(currOrgId), (re, oe) => re.OrgId == currOrgId && oe.OrgId == currOrgId)
                .WhereIF(!string.IsNullOrEmpty(planname), (re, oe) => re.PlanName.Contains(planname))
                .WhereIF(!string.IsNullOrEmpty(person), (re, oe) => re.UserName.Contains(person))
                .WhereIF(!string.IsNullOrEmpty(executionmodel), (re, oe) => re.ExecutionMode == executionmodel)
                .Select((re, oe) => new CheckPlanEnity
                {
                    CheckDate = re.CheckDate,
                    ExecutionMode = oe.ItemName,
                    DepId = re.DepId,
                    DepName = re.DepName,
                    Id = re.Id,
                    JobId = re.JobId,
                    JobName = re.JobName,
                    LastCompleteTime = re.LastCompleteTime,
                    OrgId = re.OrgId,
                    OrgName = re.OrgName,
                    PlanName = re.PlanName,
                    RiskBH = re.RiskBH,
                    RiskName = re.RiskName,
                    RoleId = re.RoleId,
                    RoleName = re.RoleName,
                    UserId = re.UserId,
                    UserName = re.UserName                             
                }).ToPageList(page, limit, ref totalCount);
            }
            return DataResult;
        }

        public List<CheckTableEntity> GetCheckTableListByUserid(string userid)
        {
            var DataResult = new List<CheckTableEntity>();
            using (var db = _dbContext.GetIntance())
            {
                string roleid = db.Queryable<UserRoleEntity>().Where(it => it.UserId == userid).First().RoleId;
                string rolename = db.Queryable<RoleEntity>().Where(it => it.DeleteMark == 1 && it.RoleId == roleid).First().RoleName;
                DataResult = db.Queryable<CheckTableEntity>()
                    .Where(it => it.DeleteMark == 1 && it.UserId == userid).GroupBy(it => new { it.RiskPointBH, it.RiskPointName,it.RiskLevel})
                    .WhereIF(rolename == "岗位员工",it => it.RiskLevel == "1" || it.RiskLevel == "2" || it.RiskLevel == "3" || it.RiskLevel == "4")
                    .WhereIF(rolename == "班组长", it => it.RiskLevel == "1" || it.RiskLevel == "2" || it.RiskLevel == "3" )
                    .WhereIF(rolename == "部门(车间)负责人", it => it.RiskLevel == "1" || it.RiskLevel == "2" )
                    .WhereIF(rolename == "公司经理级管理人员", it => it.RiskLevel == "1")
                    .Select(it => new CheckTableEntity
                    {
                        RiskPointBH = it.RiskPointBH,
                        RiskPointName = it.RiskPointName,
                    }).ToList();
            }
            return DataResult;
        }
        public bool IsExistUserId(string userid)
        {
            using (var db = _dbContext.GetIntance())
            {
                return db.Queryable<CheckPlanEnity>().Any(it => it.UserId == userid && it.DeleteMark == 1);
            }
        }
        public bool Insert(CheckPlanEnity entity)
        {
            using (var db = _dbContext.GetIntance())
            {
                var count = db.Insertable(entity).ExecuteCommand();
                result = count > 0 ? true : false;
                
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
                    var entity = new CheckPlanEnity();
                    entity.DeleteMark = 0;
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
