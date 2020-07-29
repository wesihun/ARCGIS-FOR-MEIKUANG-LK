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
    public class RoleModuleController : Controller
    {
    
        private readonly IRoleModuleService _roleModuleService;
        private readonly IModuleService _moduleService;
        private readonly IMapper _mapper;
        public RoleModuleController(IRoleModuleService roleModuleService, IModuleService moduleService, IMapper mapper)
        {
            _roleModuleService = roleModuleService;
            _moduleService = moduleService;
            _mapper = mapper;
        }

        #region 获取数据
        /// <summary>
        /// 获取数据 
        /// </summary>
        /// <param name="roleId">查询条件(RoleId-角色ID)</param>
        [HttpGet]
        public IActionResult GetListByCondition(string roleId)
        {
            RespResultCountViewModel resultCountModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "查询失败！原因：缺少角色主键";
                    resultCountModel.data = null;
                    resultCountModel.count = 0;
                    return Ok(resultCountModel);
                }
                var resultModule = _moduleService.GetList(); //所有菜单
                var resultData = _roleModuleService.GetModuleByRoleId(roleId); //所授权的角色菜单
                if (resultModule != null && resultData != null)
                {
                    var resultDataIdList = resultData.Select(md => md.ModuleId).ToList();
                    foreach (var i in resultModule)
                    {
                        if (resultDataIdList.Contains(i.ModuleId))
                        {
                            i.LAY_CHECKED = true;
                        }
                    }

                    var dataXtree = LayuixTree.CreateXTree(resultModule.Where(it => it.ParentId == "0").Select(x => new XTreeObject { title = x.ModuleName, value = x.ModuleId, disabled = false, @checked = x.LAY_CHECKED }).ToList(),
                        resultModule.Where(it => it.ParentId != "0").Select(x => new XTreeObject { title = x.ModuleName, value = x.ModuleId, pId = x.ParentId, @checked = x.LAY_CHECKED, disabled = false }).ToList());
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = dataXtree;
                    resultCountModel.count = resultModule.Count;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                    resultCountModel.data = null;
                    resultCountModel.count = 0;
                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败！原因：" + ex.ToString();
                resultCountModel.data = null;
                resultCountModel.count = 0;
                return Ok(resultCountModel);
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authorized(List<RoleModuleDto> model)
        {
            RespResultViewModel resultModel = new RespResultCountViewModel();
            if (model.Count() <= 0)
            {
                resultModel.code = 0;
                resultModel.msg = "授权失败！原因：缺少实体集合";
                resultModel.data = null;
                return Ok(resultModel);
            }
            try
            {
                List<RoleModuleEntity> roleModuleEntity = _mapper.Map<List<RoleModuleEntity>>(model);
                for (int i = 0; i < roleModuleEntity.Count(); i++)
                {
                    roleModuleEntity[i].CRowId = Guid.NewGuid().ToString();
                    roleModuleEntity[i].CreateUserId = User.GetCurrentUserId();
                    roleModuleEntity[i].CreateUserName = User.GetCurrentUserName();
                    roleModuleEntity[i].CreateDate = DateTime.Now;
                }
                bool result = _roleModuleService.InsertBatch(roleModuleEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "授权成功";
                    resultModel.data = null;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "授权失败！";
                    resultModel.data = null;
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败！" + ex.ToString();
                resultModel.data = null;
                return Ok(resultModel);
            }
        }
        #endregion
    }
}
