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
namespace XY.SystemManage.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {

        private readonly IModuleService _moduleService;
        private readonly IMapper _mapper;
        public ModuleController(IModuleService moduleService, IMapper mapper)
        {
            _moduleService = moduleService;
            _mapper = mapper;
        }
        #region 获取数据

        /// <summary>
        /// 菜单列表 
        /// </summary>
        /// <param name="keyword">查询关键字</param>
        [HttpGet]
        public IActionResult GetListByCondition(string condition, string keyword, string parentId)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _moduleService.GetList();
                if (data != null && !string.IsNullOrEmpty(parentId))
                {
                    data = data.Where(it => it.ParentId == parentId).ToList();
                }

                if (data != null && !string.IsNullOrEmpty(keyword))
                {
                    #region 多条件查询
                    switch (condition)
                    {
                        case "ModuleCode":    //菜单代码
                            data = data.Where(t => t.ModuleCode != null && t.ModuleCode.Contains(keyword)).ToList();
                            break;
                        case "ModuleName":   //菜单名称                      
                            data = data.Where(t => t.ModuleName != null && t.ModuleName.Contains(keyword)).ToList();
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
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string condition, string keyword, string parentId,int page,int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _moduleService.GetPageListByCondition(condition,keyword,parentId,page,limit,ref totalcount);
               
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
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
        /// 菜单实体 
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
                    resultModel.msg = "没有检索到菜单数据,缺少主键值";
                    return Ok(resultModel);
                }
                var data = _moduleService.GetEntityById(keyValue);
                if (data != null)
                {
                    resultModel.code = 0;
                    resultModel.msg = "查询菜单成功";
                    resultModel.data = data;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到菜单数据";
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
        /// 获取当前用户菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserModule()
        {
            //Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = new List<ModuleDto>();
                if (User.GetCurrentUserName() == DataDictConst.USER_SUPERADMIN)
                {
                    data = _moduleService.GetList();
                }
                else
                {
                    data = _moduleService.GetUserModuleList(User.GetCurrentUserId());
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
        #endregion

        #region 提交数据

        /// <summary>
        /// /新增
        /// </summary>
        /// <param name="moduleModel">菜单实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ModuleDto moduleModel)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(moduleModel.ModuleName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(moduleModel.ModuleCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编号不允许为空";
                    return Ok(resultModel);
                }
                if (!_moduleService.ExistEnCode(moduleModel.ModuleCode, moduleModel.ModuleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同功能编号";
                    return Ok(resultModel);
                }
                if (!_moduleService.ExistFullName(moduleModel.ModuleName, moduleModel.ModuleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同功能名称";
                    return Ok(resultModel);
                }
                #endregion

                ModuleEntity moduleEntity = _mapper.Map<ModuleEntity>(moduleModel);
                moduleEntity.ModuleName = moduleModel.ModuleName;
                moduleEntity.ModuleId = ConstDefine.CreateGuid();
                moduleEntity.DeleteMark = 1;
                moduleEntity.CreateUserId = User.GetCurrentUserId();
                moduleEntity.CreateUserName = User.GetCurrentUserName();
                moduleEntity.CreateDate = DateTime.Now;
                bool result = _moduleService.Insert(moduleEntity);
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
                resultModel.data = null;
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改菜单表单
        /// </summary>
        /// <param name="moduleModel">菜单实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ModuleDto moduleModel)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(moduleModel.ModuleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,无主键值";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(moduleModel.ModuleName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(moduleModel.ModuleCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编号不允许为空";
                    return Ok(resultModel);
                }
                if (!_moduleService.ExistEnCode(moduleModel.ModuleCode, moduleModel.ModuleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同功能编号";
                    return Ok(resultModel);
                }
                if (!_moduleService.ExistFullName(moduleModel.ModuleName, moduleModel.ModuleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同功能名称";
                    return Ok(resultModel);
                }
                #endregion

                ModuleEntity moduleEntity = _mapper.Map<ModuleEntity>(moduleModel);
                moduleEntity.ModuleName = moduleModel.ModuleName;
                moduleEntity.ModifyUserId = User.GetCurrentUserId();
                moduleEntity.ModifyUserName = User.GetCurrentUserName();
                moduleEntity.ModifyDate = DateTime.Now;
                bool result = _moduleService.Update(moduleEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "修改成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败";
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
        public ActionResult Remove(string keyValue,string userid,string username)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除菜单失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _moduleService.Delete(keyValue,userid,username);

                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除菜单成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除菜单失败";
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
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues, string userid, string username)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除菜单失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _moduleService.DeleteBatch(keyValues, userid, username);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除菜单成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除菜单失败";
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
        #endregion
    }
}
