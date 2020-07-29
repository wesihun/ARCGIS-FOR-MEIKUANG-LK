using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    public interface IOrganizeService
    {
        #region 获取数据
        /// <summary>
        /// 机构列表
        /// </summary>
        /// <returns></returns>
        List<OrganizeDto> GetAll();
        /// <summary>
        /// 根据父Id获取子机构信息
        /// </summary>
        /// <returns></returns>
        List<OrganizeDto> GetList(string ParentId);
        /// <summary>
        /// 获取机构列表并分页
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">查询值</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<OrganizeDto> GetPageListToTreeTable(string condition, string keyword, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 获取机构列表并分页
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">查询值</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<OrganizeDto> GetPageListByCondition(string orgname, int pageIndex, int pageSize, ref int totalCount);

        List<OrganizeDto> GetTableSelect(string orgName,string orgid, int pageIndex, int pageSize, ref int totalCount);
        /// <summary>
        /// 机构实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OrganizeDto GetEntityById(string keyValue);        

        #endregion

        #region 验证数据
        /// <summary>
        /// 机构名称不能重复
        /// </summary>
        /// <param name="organizeName">机构名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string orgName, string keyValue);
        /// <summary>
        /// 机构代码不能重复
        /// </summary>
        /// <param name="orgCode">机构代码</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string orgCode, string keyValue);
        /// <summary>
        /// 是否存在下级机构
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool ExistLower(string keyValue);

        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool Insert(OrganizeEntity OrganizeEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        bool Update(OrganizeEntity OrganizeEntity);
        /// <summary>
        /// 删除机构
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool Delete(string keyValue, string UserId, string UserName);
        /// <summary>
        /// 批量删除机构
        /// </summary>
        /// <param name="keyValues">主键List</param>
        bool DeleteBatch(List<string> keyValues, string UserId, string UserName);
        #endregion
    }
}
