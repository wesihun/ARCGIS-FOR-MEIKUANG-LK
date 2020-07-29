using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.JwtAuthorize;
using System;
using System.Collections.Generic;
using System.Linq;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;
using XY.Utilities;

namespace XY.SystemManage.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DataDictController : ControllerBase
    {
        private readonly IDataDictService _dataDictService;
        private readonly IMapper _mapper;
        public DataDictController(IDataDictService dataDictService, IMapper mapper)
        {
            _dataDictService = dataDictService;
            _mapper = mapper;
        }

        #region 获取数据

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="condition">查询条件(DataType-字典分类，ItemCode-字典Id，ItemName-字典名称)</param>
        /// <param name="keyWord">值</param>
        /// 
        [HttpGet]
        public IActionResult GetListByCondition(string condition, string keyWord, string flid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var resultData = _dataDictService.GetAll();
                if (!string.IsNullOrEmpty(flid))   //查询父节点下的所有子节点
                {
                    resultData = resultData.Where(t => t.DataType != null && t.DataType == flid).ToList();
                }
                if (resultData != null)
                {
                    if (!string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyWord))
                    {
                        #region 多条件查询
                        switch (condition)
                        {
                            case "DataType": //字典分类
                                resultData = resultData.Where(t => t.DataType.Contains(keyWord)).ToList();
                                break;
                            case "ItemCode": //字典Id
                                resultData = resultData.Where(t => t.ItemCode != null && t.ItemCode.Contains(keyWord)).ToList();
                                break;
                            case "ItemName": //字典名称
                                resultData = resultData.Where(t => t.ItemName != null && t.ItemName.Contains(keyWord)).ToList();
                                break;
                            case "Record": //新建档案获取档案目录
                                resultData = resultData.Where(t => t.ItemCode != null && t.ItemCode.Contains(keyWord)).ToList();
                                break;
                            default:
                                resultData = null;
                                break;
                        }
                        #endregion
                    }
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = resultData.Count;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败！原因：" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
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
        [HttpGet]
        public IActionResult GetPageListByCondition(string condition, string keyWord, string flid, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _dataDictService.GetPageListByCondition(currOrgId,condition, keyWord, flid, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = totalcount;
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "没有检索到数据";
                }

                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 根据字典分类获取数据字典
        /// </summary>
        /// <param name="DataType">字典分类编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListByDataType(string dataTypeCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var resultData = _dataDictService.GetAll();

                if (resultData != null)
                {
                    resultData = resultData.Where(t => t.DataType == dataTypeCode).ToList();
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = resultData.Count;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败！原因：" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 获取单个实体 
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        [HttpGet]
        public IActionResult GetModelById(string DataType, string ItemId)
        {
            var resultModel = new RespResultCountViewModel();
            if (string.IsNullOrEmpty(DataType) || string.IsNullOrEmpty(ItemId))
            {
                resultModel.code = -1;
                resultModel.msg = "查询失败！原因：缺少主键";
                resultModel.data = null;
                return Ok(resultModel);
            }
            try
            {
                var data = _dataDictService.GetEntityByDataTypeAndItemId(DataType, ItemId);
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "查询成功";
                    resultModel.data = data;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败！原因：" + ex.ToString();
                return Ok(resultModel);
            }
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(DataDictDto model)
        {
            RespResultViewModel resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.ItemName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.ItemCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编码不允许为空";
                    return Ok(resultModel);
                }

                if (_dataDictService.ExistFullName(model.DataType,model.ItemName, model.CRowId,User.GetCurrentUserOrganizeId()))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同名称！";
                    return Ok(resultModel);
                }
                if (_dataDictService.ExistEnCode(model.DataType, model.ItemCode, model.CRowId,User.GetCurrentUserOrganizeId()))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同编号！";
                    return Ok(resultModel);
                }
                #endregion

                DataDictEntity dataDictEntity = _mapper.Map<DataDictEntity>(model);
                dataDictEntity.ItemName = model.ItemName;
                dataDictEntity.CRowId = ConstDefine.CreateGuid();
                dataDictEntity.CreateUserId = User.GetCurrentUserId();
                dataDictEntity.CreateUserName = User.GetCurrentUserName();
                dataDictEntity.CreateDate = DateTime.Now;
                dataDictEntity.DeleteMark = 1;
                dataDictEntity.ModifyDate = System.DateTime.Now;
                dataDictEntity.ModifyUserId = "";
                dataDictEntity.ModifyUserName = "";
                dataDictEntity.OrgId = User.GetCurrentUserOrganizeId();

                bool result = _dataDictService.Insert(dataDictEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "新增失败";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(DataDictDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.DataType) && string.IsNullOrEmpty(model.ItemCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败！原因：字典分类ID或字典ID是空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.ItemName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.ItemCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编码不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.CRowId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "主键不允许为空";
                    return Ok(resultModel);
                }
                if (_dataDictService.ExistFullName(model.DataType,model.ItemName, model.CRowId,User.GetCurrentUserOrganizeId()))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同名称！";
                    return Ok(resultModel);
                }
                if (_dataDictService.ExistEnCode(model.DataType, model.ItemCode, model.CRowId,User.GetCurrentUserOrganizeId()))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同编号！";
                    return Ok(resultModel);
                }
                #endregion

                DataDictEntity dataDictEntity = _mapper.Map<DataDictEntity>(model);
                dataDictEntity.ItemName = model.ItemName;
                dataDictEntity.ModifyDate = DateTime.Now;
                dataDictEntity.ModifyUserId = User.GetCurrentUserId();
                dataDictEntity.ModifyUserName = User.GetCurrentUserName();

                bool result = _dataDictService.Update(dataDictEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "修改成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败！原因：根据主键没有找到要处理的数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultModel);
            }
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(string DataType, string ItemId)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(DataType) || string.IsNullOrEmpty(ItemId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键";
                    return Ok(resultModel);
                }
                bool result = _dataDictService.Delete(DataType, ItemId, User.GetCurrentUserId(), User.GetCurrentUserName());
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：根据主键没有找到要处理的数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败" + ex.Message;
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="DataType">字典分类</param>
        /// <param name="ItemId">字典Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> DataType, List<string> ItemId)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (DataType.Count() <= 0 || ItemId.Count() <= 0)
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键集合";
                    return Ok(resultModel);
                }
                bool result = _dataDictService.DeleteBatch(DataType, ItemId, User.GetCurrentUserId(), User.GetCurrentUserName());
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：根据主键没有找到要处理的数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败" + ex.Message;
                return Ok(resultModel);
            }
        }

        #endregion

    }
}
