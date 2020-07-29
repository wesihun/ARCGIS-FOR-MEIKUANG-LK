using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;
using XY.SystemManage.Entities.Dtos;
using XY.ZnshBusiness.Entities;

namespace XY.ZnshBusiness.IService
{
    public interface IJobRiskManageService
    {
        #region 获取数据
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<JobRiskEntity> GetPageListByCondition(string orgid,string currOrgId, string role, string job, string riskpointname, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 根据传入的jobid  获取该岗位下的所有人员信息
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        List<UserJobEntity> GetUserListByJobId(string jobid);
        /// <summary>
        /// 根据传入的roleid  获取该角色下的所有人员信息
        /// </summary>
        /// <param name="jobid"></param>
        /// <returns></returns>
        List<UserRoleEntity> GetUserListByRoleId(string roleid);
        #endregion

        #region 验证是否重复添加
        bool IsExist(string userid, string bh, string id);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="jobriskEntity">实体</param>
        /// <returns></returns>
        bool Insert(JobRiskEntity jobriskEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="jobriskEntity">实体</param>
        /// <returns></returns>
        bool Update(JobRiskEntity jobriskEntity);
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool Delete(string keyValue);
        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues);
        #endregion
    }
}
