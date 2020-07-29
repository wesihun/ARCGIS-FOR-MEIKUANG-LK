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
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserRoleController(IUserRoleService userRoleService, IUserService userService, IMapper mapper)
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _mapper = mapper;
        }
        #region 获取数据
        /// <summary>
        /// 获取数据 
        /// </summary>
        /// <param name="roleId">查询条件(roleId-角色ID)</param>
        [HttpGet]
        public IActionResult GetListByCondition(string roleId,int page,int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                if (string.IsNullOrEmpty(roleId))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "查询失败！原因：缺少角色主键";
                    return Ok(resultCountModel);
                }
                var resultUser = _userService.GePagetListByUser("","","","",page,limit,ref totalcount); //所有用户
                var resultData = _userRoleService.GetUserByRoleId(roleId); //获取用户角色关系
                if (resultUser != null && resultData != null)
                {
                    var resultDataIdList = resultData.Select(md => md.UserId).ToList();
                    foreach (var i in resultUser)
                    {
                        if (resultDataIdList.Contains(i.UserId))
                        {
                            i.LAY_CHECKED = true;
                        }
                    }
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultUser;
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
                resultCountModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultCountModel);
            }
        }
        #endregion

        [HttpGet]
        public IActionResult GetUserIdListByRoleId(string roleId)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var resultUser = _userRoleService.GetUserIdListByRoleId(roleId); //所有用户
                if (resultUser != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultUser;
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
                resultCountModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultCountModel);
            }

        }
        [HttpGet]
        public IActionResult GetUserList(string roleId,string depId)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var resultUser = new List<UserRoleDto>();
                resultUser = _userRoleService.GetUserList(roleId,depId); 
                if (resultUser != null)
                {                   
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultUser;
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
                resultCountModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultCountModel);
            }

        }



        #region 提交数据
            /// <summary>
            /// 批量授权
            /// </summary>
            /// <param name="model">实体</param>
            /// <returns></returns>
        [HttpPost]
        public ActionResult Authorized(List<UserRoleDto> model)
        {
            var resultModel = new RespResultCountViewModel();
            if (model.Count() <= 0)
            {
                resultModel.code = 0;
                resultModel.msg = "授权失败！原因：缺少实体集合";
                return Ok(resultModel);
            }
            try
            {
                List<UserRoleEntity> userRoleEntity = _mapper.Map<List<UserRoleEntity>>(model);
                for (int i = 0; i < userRoleEntity.Count(); i++)
                {
                    userRoleEntity[i].CRowId = Guid.NewGuid().ToString();
                    userRoleEntity[i].CreateUserId = User.GetCurrentUserId();
                    userRoleEntity[i].CreateUserName = User.GetCurrentUserName();
                    userRoleEntity[i].CreateDate = DateTime.Now;
                }
                bool result = _userRoleService.InsertBatch(userRoleEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "授权成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "授权失败！";
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
        #endregion
    }
}
