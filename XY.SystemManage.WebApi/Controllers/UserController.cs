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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
      
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        #region 获取数据
        /// <summary>
        /// 根据条件查找用户列表并分页
        /// </summary>
        /// <param name="condition">搜索条件</param>
        /// <param name="keyword">搜索值</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPagetListByUser(string orgid,string depid,string realname,string isadmin,int page,int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _userService.GePagetListByUser(orgid, depid,realname, isadmin, page,limit,ref totalcount);
                if (data.Count != 0)
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
        /// 获取用户下拉
        /// </summary>
        /// <param name="depid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserSelect(string depid, string roleid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _userService.GetUserSelect(depid, roleid);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
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
        /// 用户实体 
        /// </summary>
        /// <param name="keyValue">主键值</param> 
        [HttpGet]
        public IActionResult GetUserInfoById(string userid)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(userid))
                {
                    resultModel.code = -1;
                    resultModel.msg = "没有检索到用户数据,缺少主键值";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                var data = _userService.GetEntityById(userid);
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
                resultModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultModel);
            }

        }

        #region 获取当前登录用户信息
        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            var respResult = new RespResultViewModel();
            if (!string.IsNullOrEmpty(User.GetCurrentUserId()))
            {
                RespLoginUserInfo info = new RespLoginUserInfo();
                info.user_id = User.GetCurrentUserId();
                info.user_name = User.GetCurrentUserRealName();
                respResult.code = 0;
                respResult.data = info;
                respResult.msg = "获取当前用户信息成功";
            }
            else
            {
                respResult.code = -1;
                respResult.msg = "获取当前用户信息失败";
            }
            return new JsonResult(respResult);
        }
        #endregion
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(UserDto model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            { 
                #region 验证
                if (string.IsNullOrEmpty(model.UserName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "用户名不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    resultModel.code = -1;
                    resultModel.msg = "密码不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrganizeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "所属机构不允许为空";
                    return Ok(resultModel);
                }
                if (_userService.IsExistByUserName(model.UserName, model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "该用户已存在！";
                    return Ok(resultModel);
                }
                #endregion

                UserEntity userEntity = _mapper.Map<UserEntity>(model);
                userEntity.UserId = ConstDefine.CreateGuid();
                userEntity.CreateDate = DateTime.Now;
                userEntity.CreateUserId = User.GetCurrentUserId();
                userEntity.CreateUserName = User.GetCurrentUserName();
                userEntity.DeleteMark = 1;
                bool result = _userService.Insert(userEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                    resultModel.data = null;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "新增失败";
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UserDto model)
        {
            RespResultViewModel resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,无主键值";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.UserName))
                {
                    resultModel.code = -1;
                    resultModel.msg = "用户名不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    resultModel.code = -1;
                    resultModel.msg = "密码不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrganizeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "所属机构不允许为空";
                    return Ok(resultModel);
                }
                if (_userService.IsExistByUserName(model.UserName, model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "该用户已存在！";
                    return Ok(resultModel);
                }
                #endregion

                UserEntity userEntity = _mapper.Map<UserEntity>(model);
                userEntity.ModifyDate = DateTime.Now;
                userEntity.ModifyUserId = User.GetCurrentUserId();
                userEntity.ModifyUserName = User.GetCurrentUserName();
                bool result = _userService.Update(userEntity);
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
                return Ok(resultModel);
            }
        }

        /// <summary>
        /// 修改当前用户密码
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCurrentUserPassword(string newPassword, string Password)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(Password))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,缺少密码";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                var user = _userService.IsExistByUserName(User.GetCurrentUserName());
                if (user == null)
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,用户不存在";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                if (!AccountAuthHelper.VerifyPassword(Password, user.SecretKey, user.Password))
                {
                    resultModel.code = -1;
                    resultModel.msg = "当前密码输入错误！";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                UserEntity userEntity = new UserEntity();
                userEntity.ModifyDate = DateTime.Now;
                userEntity.ModifyUserId = User.GetCurrentUserId();
                userEntity.ModifyUserName = User.GetCurrentUserName();
                bool result = _userService.RevisePassword(User.GetCurrentUserId(), newPassword);
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
                resultModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultModel);
            }
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="newPassword">新密码</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditUserPassword(string userid,string username,string newPassword)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(newPassword))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,缺少密码";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                var user = _userService.IsExistByUserName(username);
                if (user == null)
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败,用户不存在";
                    resultModel.data = null;
                    return Ok(resultModel);
                }
                UserEntity userEntity = new UserEntity();
                userEntity.ModifyDate = DateTime.Now;
                userEntity.ModifyUserId = User.GetCurrentUserId();
                userEntity.ModifyUserName = User.GetCurrentUserName();
                bool result = _userService.RevisePassword(userid, newPassword);
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
                resultModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultModel);
            }
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除用户
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
                    resultModel.msg = "删除用户失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _userService.Delete(keyValue, User.GetCurrentUserId(), User.GetCurrentUserName());
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除用户成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除用户失败";
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

        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除用户失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _userService.DeleteBatch(keyValues, User.GetCurrentUserId(), User.GetCurrentUserName());
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除用户成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除用户失败";
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
