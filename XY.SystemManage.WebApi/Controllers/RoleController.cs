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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        #region 获取数据
        [HttpGet]
        public IActionResult GetPageListByCondition(string rolename, int page,int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data =_roleService.GetPageListByCondition(rolename, page, limit, ref totalcount);
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
        /// 获取单个实体 
        /// </summary>
        /// <param name="keyValue">主键值</param> 
        [HttpGet]
        public IActionResult GetModelById(string keyValue)
        {
            var resultModel = new RespResultCountViewModel();
            if (string.IsNullOrEmpty(keyValue))
            {
                resultModel.code = -1;
                resultModel.msg = "查询失败！原因：缺少主键";
                return Ok(resultModel);
            }
            try
            {
                var data = _roleService.GetEntityById(keyValue);
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
                resultModel.msg = "操作失败！原因：" + ex.Message;
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
        public ActionResult Create(RoleDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.RoleName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "角色名称不允许为空";
                    return Ok(resultModel);
                }
                if (_roleService.CheckIsRoleNameRepeat(model.RoleName, model.RoleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同角色名称";
                    return Ok(resultModel);
                }
                #endregion
                RoleEntity roleEntity = _mapper.Map<RoleEntity>(model);
                roleEntity.RoleId = Guid.NewGuid().ToString();
                roleEntity.CreateUserId = User.GetCurrentUserId();
                roleEntity.CreateUserName = User.GetCurrentUserName();
                roleEntity.CreateDate = DateTime.Now;
                roleEntity.DeleteMark = 1;
                roleEntity.RoleCode = model.RoleCode;


                bool result = _roleService.Insert(roleEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "新增失败！";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败！" + ex.ToString();
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RoleDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {

                #region 验证
                if (string.IsNullOrEmpty(model.RoleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败！,主键值为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.RoleName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "角色名称不允许为空";
                    return Ok(resultModel);
                }
                if (_roleService.CheckIsRoleNameRepeat(model.RoleName, model.RoleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同角色名称";
                    return Ok(resultModel);
                }
                #endregion

                RoleEntity roleEntity = _mapper.Map<RoleEntity>(model);
                roleEntity.ModifyDate = DateTime.Now;
                roleEntity.ModifyUserId = User.GetCurrentUserId();
                roleEntity.ModifyUserName = User.GetCurrentUserName();

                bool result = _roleService.Update(roleEntity);
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
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(string keyValue)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {

                #region 验证
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键";
                    return Ok(resultModel);
                }
                if (!_roleService.CheckIsAllocateUser(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：该角色下存在用户";
                    return Ok(resultModel);
                }
                #endregion

                bool result = _roleService.Delete(keyValue, User.GetCurrentUserId(), User.GetCurrentUserName());
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
                resultModel.msg = "操作失败！" + ex.Message;
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键集合";
                    return Ok(resultModel);
                }
                if (!_roleService.CheckIsAllocateUserBatch(keyValues))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：该角色下存在用户";
                    return Ok(resultModel);
                }
                #endregion

                bool result = _roleService.DeleteBatch(keyValues, User.GetCurrentUserId(), User.GetCurrentUserName());
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
                resultModel.msg = "操作失败！" + ex.ToString();
                return Ok(resultModel);
            }
        }
        #endregion
    }
}
