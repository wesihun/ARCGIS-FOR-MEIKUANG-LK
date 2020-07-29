/*****************************************************************************
** Copyright (c) 2018 许洪义. All rights reserved. 
** 作者：Julia
** 时间：2018/8/8 16:26:45 
** 版本：V1.0.0 
** 描述：
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XY.SystemManage.Entities;


namespace XY.SystemManage.IService
{
    /// <summary>
    /// IBaseAreaService
    /// </summary>
    public interface IAreaService
    {
        #region 获取数据
        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns></returns>
        List<AreaDto> GetAll();
        /// <summary>
        /// 获取单个区域实体
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        AreaDto GetEntity(string KeyValue);
        /// <summary>
        /// 根据areaid模糊查询list
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<AreaDto> GetListByAreaId(string areaid);
        /// <summary>
        /// 获取区域列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyword">搜索值</param>
        /// <param name="parentId">父节点ID</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <param name="totalcount">总数</param>
        /// <returns></returns>
        List<AreaDto> GetPageListByCondition(string condition, string keyword, string parentId, int page, int limit,ref int totalcount);
        /// <summary>
        /// 获取AreaName
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        string GetAreaName(string areaid);
        /// <summary>
        /// 根据父ID查询全部子列表
        /// </summary>
        /// <param name="ParentId">父ID</param>
        /// <returns>区划列表</returns>
        List<AreaDto> GetListByParentId(string ParentId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        bool Insert(AreaEntity areaEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="action"></param>
        /// <param name="roleEntity"></param>
        /// <returns></returns>
        bool Update(AreaEntity areaEntity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        bool Delete(string keyValue, string UserId, string UserName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        bool DeleteBatch(List<string> keyValues, string UserId, string UserName);
        #endregion
    }

}
