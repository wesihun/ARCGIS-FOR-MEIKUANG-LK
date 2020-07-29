using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    public interface IJobService
    {
        #region 获取数据
        /// <summary>
        /// 根据条件获取岗位列表并分页
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="depname"></param>
        /// <param name="jobname"></param>
        /// <param name="page">页码</param>
        /// <param name="limit">也尺寸</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<JobEntity> GetPageListByCondition(string depid,string orgid, string depname, string jobname, int page, int limit, ref int totalCount);
        List<UserDto> GetUserSelect(string depid, string jobid);
        /// <summary>
        /// 获取该机构下的所有用户列表
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="orgId">机构Id</param>
        /// <returns></returns>
        List<JobEntity> GetAllList(string id,string orgId);
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="jobId">岗位ID</param>
        /// <returns></returns>
        List<UserJobEntity> GetUserByJobId(string jobId);

        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="jobEntity">岗位实体</param>
        /// <returns></returns>
        bool Insert(JobEntity jobEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="jobEntity">岗位实体</param>
        /// <returns></returns>
        bool Update(JobEntity jobEntity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool Delete(string keyValue);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool DeleteBatch(List<string> keyValues);
        /// <summary>
        /// 删除校验
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        bool CheckIsAllocateUser(string keyValue);
        /// <summary>
        /// 批量删除校验
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        bool CheckIsAllocateUserBatch(List<string> keyValues);
        /// <summary>
        /// 校验岗位名称是否重复
        /// </summary>
        /// <param name="JobName">岗位名称</param>
        /// <param name="JobId">岗位Id主键</param>
        /// <param name="DepId">部门Id</param>
        /// <param name="OrgId">机构Id</param>
        /// <returns></returns>
        bool CheckIsJobNameRepeat(string JobName, string JobId,string DepId,string OrgId);
        /// <summary>
        /// 校验岗位编号是否重复
        /// </summary>
        /// <param name="JobBH">岗位编号</param>
        /// <param name="JobId">岗位Id主键</param>
        /// <param name="DepId">部门Id</param>
        /// <param name="OrgId">机构Id</param>
        /// <returns></returns>
        bool CheckIsJobBHRepeat(string JobBH, string JobId, string DepId, string OrgId);
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="userJobEntity"></param>
        /// <returns></returns>
        bool InsertBatch(List<UserJobEntity> userJobEntity);
        #endregion
    }
}
