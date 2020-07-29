using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.SystemManage.Service;
using XY.Utilities;
using XY.Universal.Models;
using Ocelot.JwtAuthorize;
namespace XY.SystemManage.WebApi.Controllers
{

    [Authorize("permission")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IMapper _mapper;
        public AreaController(IAreaService areaService,IMapper mapper)
        {
            _areaService = areaService;
            _mapper = mapper;
        }

        #region 获取数据

        /// <summary>
        /// 获取全部区划列表并绑定成树结构
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListToTree(string parentId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = parentId != null ? _areaService.GetAll().Where(it => it.ParentId != parentId) : _areaService.GetAll();
                var resData = LayuixTree.CreateTree(
                    data.Where(it => it.ParentId == "0" || it.ParentId == null).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, @checked = true, open = true }).ToList(),
                    data.Where(it => it.ParentId != "0" || it.ParentId != null).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, pId = x.ParentId, @checked = true, open = true }).ToList()
                    );
                if (resData != null)
                {
                    return Ok(resData);
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                    return Ok(resultCountModel);
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        [HttpGet]
        public IActionResult GetListByParentId(string ParentId)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                var data = _areaService.GetListByParentId(ParentId);
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "获取区域列表数据成功";
                    resultModel.data = data;
                    resultModel.count = data.Count();
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到数据";
                    resultModel.count = 0;
                }

                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                resultModel.count = 0;
                return Ok(resultModel);
            }
        }


        /// <summary>
        /// 获取全部区划列表并绑定成树结构
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListToTreeToXian(string parentId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = parentId != null ? _areaService.GetAll().Where(it => it.ParentId != parentId && it.Layer<4) : _areaService.GetAll().Where(it => it.Layer < 4);
                var resData = LayuixTree.CreateTree(
                    data.Where(it => it.ParentId == "0" || it.ParentId == null).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, @checked = true, open = true }).ToList(),
                    data.Where(it => it.ParentId != "0" || it.ParentId != null).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, pId = x.ParentId, @checked = true, open = true }).ToList()
                    );
                if (resData != null)
                {
                    return Ok(resData);
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                    return Ok(resultCountModel);
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 获取区域列表（不输入条件获取全部数据） 
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">查询关键字</param>
        /// <param name="parentId">父节点ID</param>
        [HttpGet]
        public IActionResult GetListByCondition(string condition, string keyword, string parentId)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                var data = _areaService.GetAll();
                if (data != null && !string.IsNullOrEmpty(parentId))
                {
                    data = data.Where(t => t.ParentId != null && t.ParentId == parentId).ToList();
                }
                if (data != null && !string.IsNullOrEmpty(keyword))
                {
                    #region 多条件查询
                    switch (condition)
                    {
                        case "AreaCode":    //区域代码
                            data = data.Where(t => t.AreaCode != null && t.AreaCode.Contains(keyword)).ToList();
                            break;
                        case "AreaName":   //区域名称                      
                            data = data.Where(t => t.AreaName != null && t.AreaName.Contains(keyword)).ToList();
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "获取区域列表数据成功";
                    resultModel.data = data;
                    resultModel.count = data.Count();
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到数据";
                    resultModel.count = 0;
                }

                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                resultModel.count = 0;
                return Ok(resultModel);
            }
        }
        /// <summary>
        /// 获取区域列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyword">搜索值</param>
        /// <param name="parentId">父节点ID</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string condition, string keyword, string parentId, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _areaService.GetPageListByCondition(condition, keyword,parentId, page, limit, ref totalcount);
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
        /// 获取区域单个实体 
        /// </summary>
        /// <param name="keyValue">主键值</param> 
        [HttpGet]
        public IActionResult GetModelById(string keyValue)
        {
            var resultModel = new RespResultViewModel();
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到区域数据,缺少主键值";
                    return Ok(resultModel);
                }
                var data = _areaService.GetEntity(keyValue);
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "获取区域单个实体成功";
                    resultModel.data = data;
                }
                else
                {
                    resultModel.code = 0;
                    resultModel.msg = "没有检索到区域数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultModel);
            }

        }

        /// <summary>
        /// 获取Ztree地区树
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZTreeByAreaIdList(string AreaId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(AreaId))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有地区ID";
                    return Ok(resultCountModel);
                }
                var data = _areaService.GetAll().Where(it => it.AreaId.Contains(AreaId)).ToList();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = data;
                    return Ok(resultCountModel);
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                    return Ok(resultCountModel);
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 获取TreeSelect地区树
        /// </summary>
        /// <param name="AreaId">根节点地区ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetTreeSelectAreaList(string AreaId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(AreaId))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有地区ID";
                    return Ok(resultCountModel);
                }
                var data = _areaService.GetAll().Where(it => it.AreaId.Contains(AreaId)).ToList();
                var resData = LayuixTree.CreateTree(
                    data.Where(it => it.AreaId == AreaId).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, @checked = true, open = true }).ToList(),
                    data.Where(it => it.AreaId != AreaId && it.AreaId.Contains(AreaId)).Select(x => new TreeObject { id = x.AreaId, name = x.AreaName, pId = x.ParentId, @checked = true, open = true }).ToList()
                    );
                if (resData != null)
                {
                    return Ok(resData);
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                    return Ok(resultCountModel);
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 根据区域代码获取区域名称
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAreaNameByAreaId(string areaid)
        {
            var resultModel = new RespResultViewModel();
            var xjqydata = new AreaDto();
            var xjdata = new AreaDto();
            var cjdata = new AreaDto();
            try
            {
                if (string.IsNullOrEmpty(areaid))
                {
                    resultModel.code = -1;
                    resultModel.msg = "条件不允许为空！";
                    return Ok(resultModel);
                }

                if (areaid.Length == 12)
                {
                    cjdata = _areaService.GetEntity(areaid);
                    xjdata = _areaService.GetEntity(areaid.Substring(0, 9));
                    xjqydata = _areaService.GetEntity(areaid.Substring(0, 6));
                }
                else if (areaid.Length == 9)
                {
                    xjdata = _areaService.GetEntity(areaid);
                    xjqydata = _areaService.GetEntity(areaid.Substring(0, 6));
                }
                else
                {
                    xjqydata = _areaService.GetEntity(areaid);
                }
                var obj = new
                {
                    xjqyname = xjqydata.AreaName,
                    xjname = xjdata.AreaName,
                    cjdata = cjdata.AreaName
                };
                resultModel.code = 0;
                resultModel.msg = "获取区域名称成功";
                resultModel.data = obj;
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
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
        public ActionResult Create(AreaDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (model.ParentId == null)
                {
                    model.ParentId = "0";
                }
                AreaEntity areaEntity = _mapper.Map<AreaEntity>(model);
                areaEntity.AreaId = model.AreaCode;
                areaEntity.CreateUserId = User.GetCurrentUserId();
                areaEntity.CreateUserName = User.GetCurrentUserName();
                areaEntity.CreateDate = DateTime.Now;
                areaEntity.DeleteMark = 1;
                bool result = _areaService.Insert(areaEntity);
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
                resultModel.msg = "新增失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        [HttpPost]
        public ActionResult Edit(AreaDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {

                if (string.IsNullOrEmpty(model.AreaId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败！,原因：AreaId是空";
                    return Ok(resultModel);
                }
                if (model.ParentId == null)
                {
                    model.ParentId = "0";
                }
                AreaEntity areaEntity = _mapper.Map<AreaEntity>(model);
                areaEntity.ModifyDate = DateTime.Now;
                areaEntity.ModifyUserId = User.GetCurrentUserId();
                areaEntity.ModifyUserName = User.GetCurrentUserName();
                bool result = _areaService.Update(areaEntity);
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
                resultModel.msg = "修改失败！原因：" + ex.ToString();
                return Ok(resultModel);
            }
        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(string keyValue)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键";
                    return Ok(resultModel);
                }

                bool result = _areaService.Delete(keyValue, User.GetCurrentUserId(), User.GetCurrentUserName());
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
                resultModel.msg = "删除失败" + ex.Message;
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键集合";
                    return Ok(resultModel);
                }
                bool result = _areaService.DeleteBatch(keyValues, User.GetCurrentUserId(), User.GetCurrentUserName());
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除区域成功";
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
                resultModel.msg = "删除失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        #endregion
    }
}
