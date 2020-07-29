using System;
using System.Collections.Generic;
using System.Text;
using XY.ZnshBusiness.Entities;

namespace XY.ZnshBusiness.IService
{
    public interface ICheckPlanService
    {
        #region 获取数据
        /// <summary>
        /// 获取检查计划列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="totalCount">返回数目</param>
        /// <returns></returns>
        List<CheckPlanEnity> GetPageListByCondition(string orgid,string currOrgId, string planname, string person, string executionmodel, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取该人员下的检查点信息
        /// </summary>
        /// <returns></returns>
        List<CheckTableEntity> GetCheckTableListByUserid(string userid);
        /// <summary>
        /// 是否再次添加计划
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        bool IsExistUserId(string userid);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        bool Insert(CheckPlanEnity entity);
        /// <summary>
        /// 批量删除信息
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues);
        #endregion
    }
}
