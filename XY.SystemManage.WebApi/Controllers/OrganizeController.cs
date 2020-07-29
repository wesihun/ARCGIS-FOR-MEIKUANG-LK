using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.SystemManage.Service;
using XY.Utilities;
using XY.Universal.Models;
using Ocelot.JwtAuthorize;
using System.Web;
using NLog;

namespace XY.SystemManage.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizeController : ControllerBase
    {
        private readonly Logger nlog = LogManager.GetCurrentClassLogger(); //获得日志实体;
        private readonly IOrganizeService _organizeService;
        private readonly IMapper _mapper;
        public OrganizeController(IOrganizeService organizeService, IMapper mapper)
        {
            _organizeService = organizeService;
            _mapper = mapper;
        }

        #region 获取数据
        /// <summary>
        /// 机构列表 
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="parentId">父ID</param>
        [HttpGet]
        public IActionResult GetListByCondition(string condition, string keyword)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                LogEventInfo log = new LogEventInfo(LogLevel.Info, User.GetCurrentUserName(), "测试日志Info");
                nlog.Log(log);

                var data = _organizeService.GetAll();
                if (data != null && !string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                {
                    #region 多条件查询
                    switch (condition)
                    {
                        case "OrgName":    //机构名称
                            data = data.Where(t => t.OrgName != null && t.OrgName.Contains(keyword)).ToList();
                            break;
                        case "OrgCode":   //机构代码                      
                            data = data.Where(t => t.OrgCode != null && t.OrgCode.Contains(keyword)).ToList();
                            break;
                        case "Manager":    //机构管理人
                            data = data.Where(t => t.Manager != null && t.Manager.Contains(keyword)).ToList();
                            break;
                        default:
                            break;
                    }
                    #endregion
                }

                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
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
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

        /// <summary>
        /// 获取机构树结构 
        /// </summary>
     
        [HttpGet]
        public IActionResult GetListToTree(string OrganizeId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = OrganizeId != null ? _organizeService.GetAll().Where(it => it.OrganizeId != OrganizeId) : _organizeService.GetAll();
                var resData = LayuixTree.CreateTree(
                    data.Where(it => it.ParentId == "0" || it.ParentId == null).Select(x => new TreeObject { id = x.OrganizeId, name = x.OrgName, @checked = true, open = true }).ToList(),
                    data.Where(it => it.ParentId != "0" || it.ParentId != null).Select(x => new TreeObject { id = x.OrganizeId, name = x.OrgName, pId = x.ParentId, @checked = true, open = true }).ToList()
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
        /// 获取所有机构树结构 
        /// </summary>
      
        [HttpGet]
        public IActionResult GetListToTreeTable(string condition, string keyword)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var resData = _organizeService.GetAll().Select(x => new TreeTable
                {
                    id = x.OrganizeId,
                    pid = x.ParentId,
                    title = x.OrgName,
                    OrgName = x.OrgName,
                    OrgCode = x.OrgCode,
                    Address = x.Address,
                    Leader = x.Manager,
                    SortCode = x.SortCode

                }).ToList();
                if (resData != null && !string.IsNullOrEmpty(condition) && !string.IsNullOrEmpty(keyword))
                {
                    #region 多条件查询
                    switch (condition)
                    {
                        case "OrgName":    //机构名称
                            resData = resData.Where(t => t.OrgName != null && t.OrgName.Contains(keyword)).ToList();
                            break;
                        case "OrgCode":   //机构代码                      
                            resData = resData.Where(t => t.OrgCode != null && t.OrgCode.Contains(keyword)).ToList();
                            break;
                        case "Leader":     //机构管理人
                            resData = resData.Where(t => t.Leader != null && t.Leader.Contains(keyword)).ToList();
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                resultCountModel.data = resData;
                resultCountModel.code = 0;
                if (resData != null)
                {
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
        /// 获取所有机构列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示的条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var resultData = _organizeService.GetPageListByCondition(orgname, page, limit, ref totalcount);
                if (resultData.Count != 0)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = totalcount;
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
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }


        /// <summary>
        /// 获取所有机构下拉列表 带分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTableSelect(string orgName, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                string currOrgId = User.GetCurrentUserOrganizeId();
                var resultData = _organizeService.GetTableSelect(orgName, currOrgId, page, limit, ref totalcount);
                if (resultData != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = totalcount;
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
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }


        /// <summary>
        /// 获取所有机构列表并分页
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="keyword">值</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示的条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListToTreeTable(string condition, string keyword,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var resultData = _organizeService.GetPageListToTreeTable(condition, keyword, page, limit, ref totalcount);
                if (resultData != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = totalcount;
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
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 机构单个实体 
        /// </summary>
        /// <param name="keyValue">主键值</param> 
        [HttpGet]
        public IActionResult GetModelById(string keyValue)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到机构数据,缺少主键值";
                    return Ok(resultModel);
                }
                var data = _organizeService.GetEntityById(keyValue);
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "查询机构成功";
                    resultModel.data = data;
                    resultModel.count = 1;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到机构数据";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.Message;
                resultModel.data = null;
                return Ok(resultModel);
            }

        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">机构实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(OrganizeDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构名称不允许为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrgCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构代码不允许为空！";
                    return Ok(resultModel);
                }
                if (_organizeService.ExistFullName(model.OrgName, model.OrganizeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同机构名称！";
                    return Ok(resultModel);
                }
                if (_organizeService.ExistEnCode(model.OrgCode, model.OrganizeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同机构代码！";
                    return Ok(resultModel);
                }
                #endregion

                if (model.ParentId == null)
                {
                    model.ParentId = "0";
                }
                OrganizeEntity organizeEntity = _mapper.Map<OrganizeEntity>(model);
                organizeEntity.OrgBrevityCode = CommonHelper.GetPinyinCode(organizeEntity.OrgName);
                organizeEntity.OrganizeId = ConstDefine.CreateGuid();
                organizeEntity.CreateDate = DateTime.Now;
                organizeEntity.CreateUserId = User.GetCurrentUserId();
                organizeEntity.CreateUserName = User.GetCurrentUserName();
                organizeEntity.DeleteMark = 1;//删除标记，原来为0
                bool result = _organizeService.Insert(organizeEntity);
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
                resultModel.msg = "操作失败:" + ex.Message;
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">机构实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(OrganizeDto model)
        {
            var resultModel = new RespResultCountViewModel();
            #region 验证
            if (string.IsNullOrEmpty(model.OrgName))
            {
                resultModel.code = -1;
                resultModel.msg = "机构名称不允许为空！";
                return Ok(resultModel);
            }
            if (string.IsNullOrEmpty(model.OrgCode))
            {
                resultModel.code = -1;
                resultModel.msg = "机构代码不允许为空！";
                return Ok(resultModel);
            }
            if (_organizeService.ExistFullName(model.OrgName, model.OrganizeId))
            {
                resultModel.code = -1;
                resultModel.msg = "已存在相同机构名称！";
                return Ok(resultModel);
            }
            if (_organizeService.ExistEnCode(model.OrgCode, model.OrganizeId))
            {
                resultModel.code = -1;
                resultModel.msg = "已存在相同机构代码！";
                return Ok(resultModel);
            }
            //if (string.IsNullOrEmpty(model.XAreaCode))
            //{
            //    resultModel.code = -1;
            //    resultModel.msg = "区划代码不能为空！";
            //    return Ok(resultModel);
            //}
            #endregion
            try
            {

                if (model.ParentId == null)
                {
                    model.ParentId = "0";
                }
                OrganizeEntity organizeEntity = _mapper.Map<OrganizeEntity>(model);
                organizeEntity.OrgBrevityCode = CommonHelper.GetPinyinCode(organizeEntity.OrgName);
                organizeEntity.ModifyDate = DateTime.Now;
                organizeEntity.ModifyUserId = User.GetCurrentUserId();
                organizeEntity.ModifyUserName = User.GetCurrentUserName();
                organizeEntity.ModifyDate = DateTime.Now;
                bool result = _organizeService.Update(organizeEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "修改成功";
                    resultModel.data = null;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败";
                    resultModel.data = null;
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.Message;
                resultModel.data = null;
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
                    resultModel.msg = "删除机构失败,缺少主键";
                    return Ok(resultModel);
                }
                bool existLower = _organizeService.ExistLower(keyValue);
                if (!existLower)
                {
                    bool result = _organizeService.Delete(keyValue, User.GetCurrentUserId(), User.GetCurrentUserName());

                    if (result)
                    {
                        resultModel.code = 0;
                        resultModel.msg = "删除机构成功";
                    }
                    else
                    {
                        resultModel.code = -1;
                        resultModel.msg = "删除机构失败";
                    }
                    return Ok(resultModel);
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除机构失败,存在子机构";
                    return Ok(resultModel);
                }


            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                return Ok(resultModel);
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues"></param>
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
                    resultModel.msg = "批量删除机构失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _organizeService.DeleteBatch(keyValues, User.GetCurrentUserId(), User.GetCurrentUserName());

                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除机构成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除机构失败";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                return Ok(resultModel);
            }
        }
        #endregion
    }
}
