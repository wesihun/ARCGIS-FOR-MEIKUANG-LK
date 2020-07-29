using System;
using System.Collections.Generic;
using System.Text;
using XY.SystemManage.Entities;

namespace XY.SystemManage.IService
{
    /// <summary>
    /// Copyright (c) 2018 . 
    /// 作者：lk
    /// 时间：2018/10/20 10:20:23
    /// 版本：v1.0.0
    /// 描述：字典服务接口类
    /// </summary> 
    public interface IDataDictService
    {
        #region 获取数据
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        List<DataDictDto> GetAll();

        /// <summary>
        /// 根据条件获取数据字典列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyWord">搜索值</param>
        /// <param name="flid">字典分类</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <param name="totalcount">总数</param>
        /// <returns></returns>
        List<DataDictDto> GetPageListByCondition(string currOrgId,string condition, string keyWord, string flid, int page, int limit,ref int totalcount);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="DataType">父级值</param>
        /// <param name="ItemId">主键值</param>
        /// <returns></returns>
        DataDictDto GetEntityByDataTypeAndItemId(string DataType, string ItemId);

        /// <summary>
        /// 字典是否重复校验
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <returns></returns>
        bool CheckIsDataDictRepeat(string DataType, string ItemId);
        #endregion


        #region 验证数据
        /// <summary>
        /// 名称不能重复
        /// </summary>
        /// <param name="datatype">类型</param>
        /// <param name="name">字典名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string datatype,string name, string keyValue,string currOrgId);
        /// <summary>
        /// 编号不能重复
        /// </summary>
        /// <param name="datatype">类型</param>
        /// <param name="bh">字典编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string datatype, string bh, string keyValue,string currOrgId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dataDictEntity">实体</param>
        /// <returns></returns>
        bool Insert(DataDictEntity dataDictEntity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dataDictEntity"></param>
        /// <returns></returns>
        bool Update(DataDictEntity dataDictEntity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        bool Delete(string DataType, string ItemId, string UserId, string UserName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <param name="UserId">登陆用户ID</param>
        /// <param name="UserName">登陆用户名称</param>
        /// <returns></returns>
        bool DeleteBatch(List<string> DataType, List<string> ItemId, string UserId, string UserName);
        #endregion
    }
}
