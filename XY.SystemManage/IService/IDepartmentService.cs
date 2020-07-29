using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    public interface IDepartmentService
    {
        #region 获取数据
        /// <summary>
        /// 获取部门列表并分页
        /// </summary>
        /// <param name="orgid">查询条件</param>
        /// <param name="depname">查询值</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<DepartmentEntity> GetPageListByCondition(string orgid,string depname, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">查询值</param>
        /// <returns></returns>
        List<DepartmentEntity> GetListByCondition(string condition, string keyword);
        #endregion

        #region 验证数据
        /// <summary>
        /// 部门名称不能重复
        /// </summary>
        /// <param name="departmentName">机构名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string departmentName, string keyValue);
        /// <summary>
        /// 部门编号不能重复
        /// </summary>
        /// <param name="BH">部门编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string BH, string keyValue);
        #endregion
        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool Insert(DepartmentEntity DepartmentEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        bool Update(DepartmentEntity DepartmentEntity);
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool Delete(string keyValue);
        /// <summary>
        /// 批量删除机构
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues);
        #endregion

    }
}
