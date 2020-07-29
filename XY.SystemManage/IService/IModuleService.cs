/*****************************************************************************
** Copyright (c) 2018 许洪义. All rights reserved. 
** 作者：lk
** 时间：2018/8/23 9:38:11 
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
    /// IModuleService
    /// </summary>
    public interface IModuleService
    {


        #region 获取数据
        /// <summary>
        /// 功能列表
        /// </summary>
        /// <returns></returns>
        List<ModuleDto> GetList();
        /// <summary>
        /// 根据条件查询功能列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyword">搜素值</param>
        /// <param name="parentId">父类Id</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <param name="totalcount">总数</param>
        /// <returns></returns>
        List<ModuleDto> GetPageListByCondition(string condition, string keyword, string parentId,int page,int limit,ref int totalcount);
        /// <summary>
        /// 功能实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ModuleDto GetEntityById(string keyValue);
        /// <summary>
        /// 获取当前用户菜单列表
        /// </summary>
        /// <returns></returns>
        List<ModuleDto> GetUserModuleList(string userId);
        #endregion

        #region 验证数据
        /// <summary>
        /// 功能编号不能重复
        /// </summary>
        /// <param name="modelCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string modelCode, string keyValue);
        /// <summary>
        /// 功能名称不能重复
        /// </summary>
        /// <param name="modelName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string modelName, string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool Delete(string keyValue,string UserId, string UserName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">菜单主键</param>
        /// <returns></returns>
        bool DeleteBatch(List<string> keyValues, string UserId, string UserName);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="moduleEntity">功能实体</param>
        /// <returns></returns>
        bool Insert(ModuleEntity moduleEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="moduleEntity">功能实体</param>
        /// <returns></returns>
        bool Update(ModuleEntity moduleEntity);
        #endregion



    }
}
