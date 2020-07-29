using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.JwtAuthorize;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;
using XY.Utilities;

namespace XY.SystemManage.WebApi.Controllers
{
    [Authorize("permission")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public JobController(IJobService jobService, IUserService userService, IMapper mapper)
        {
            _jobService = jobService;
            _userService = userService;
            _mapper = mapper;
        }
        #region 获取数据
        /// <summary>
        /// 获取数据 
        /// </summary>
        /// <param name="id">主键(id-岗位ID)</param>
        [HttpGet]
        public IActionResult GetJobUserList(string jobid,string orgId,string depid, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                if (string.IsNullOrEmpty(jobid))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "查询失败！原因：缺少岗位主键";
                    return Ok(resultCountModel);
                }
                if (string.IsNullOrEmpty(orgId))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "查询失败！原因：缺少机构Id";
                    return Ok(resultCountModel);
                }
                var resultUser = _userService.GePagetListByUser(orgId, depid, "","", page, limit, ref totalcount); //改机构下的所有用户
                var resultData = _jobService.GetUserByJobId(jobid); //获取已分配岗位用户关系
                if (resultUser.Count != 0 )
                {
                    if(resultData.Count != 0)
                    {
                        var resultDataIdList = resultData.Select(md => md.UserId).ToList();
                        foreach (var i in resultUser)
                        {
                            if (resultDataIdList.Contains(i.UserId))
                            {
                                i.LAY_CHECKED = true;
                            }
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
        /// <summary>
        /// 获取数据 
        /// </summary>
        /// <param name="jobid">主键(id-岗位ID)</param>
        [HttpGet]
        public IActionResult GetJobListById(string jobid)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                if (string.IsNullOrEmpty(jobid))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "查询失败！原因：缺少主键";
                    return Ok(resultCountModel);
                } //改机构下的所有用户
                var resultData = _jobService.GetUserByJobId(jobid); //获取已分配岗位用户关系
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
                resultCountModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultCountModel);
            }
        }
        /// <summary>
        /// 批量授权
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authorized(List<UserJobEntity> model)
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
                for (int i = 0; i < model.Count(); i++)
                {
                    model[i].Id = Guid.NewGuid().ToString();
                    model[i].DeleteMark = 1;
                    model[i].CreateTime = DateTime.Now;
                }
                bool result = _jobService.InsertBatch(model);
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
        [HttpGet]
        public IActionResult GetPageListByCondition(string depid,string orgid,string depname, string jobname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _jobService.GetPageListByCondition(depid,orgid,depname,jobname, page, limit, ref totalcount);
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
        /// 获取用户下拉
        /// </summary>
        /// <param name="depid"></param>
        /// <param name="jobid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserSelect(string depid, string jobid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _jobService.GetUserSelect(depid, jobid);
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
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(JobEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.Name))
                {
                    resultModel.code = -1;
                    resultModel.msg = "岗位名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.DepId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "部门Id不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构Id不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.JobCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "岗位编号不允许为空";
                    return Ok(resultModel);
                }
                if (_jobService.CheckIsJobBHRepeat(model.JobCode, model.Id, model.DepId, model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同岗位编号";
                    return Ok(resultModel);
                }
                if (_jobService.CheckIsJobNameRepeat(model.Name, model.Id,model.DepId,model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同岗位名称";
                    return Ok(resultModel);
                }
                #endregion
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                bool result = _jobService.Insert(model);
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
        public ActionResult Edit(JobEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {

                #region 验证
                if (string.IsNullOrEmpty(model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败！,主键值为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.Name))
                {
                    resultModel.code = -1;
                    resultModel.msg = "岗位名称不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.DepId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "部门Id不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构Id不允许为空";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.JobCode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "岗位编号不允许为空";
                    return Ok(resultModel);
                }
                if (_jobService.CheckIsJobNameRepeat(model.Name, model.Id, model.DepId, model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同岗位名称";
                    return Ok(resultModel);
                }
                if (_jobService.CheckIsJobBHRepeat(model.JobCode, model.Id, model.DepId, model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同岗位编号";
                    return Ok(resultModel);
                }
                #endregion

                bool result = _jobService.Update(model);
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
                //if (!_jobService.CheckIsAllocateUser(keyValue))
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "删除失败！原因：该角色下存在用户";
                //    return Ok(resultModel);
                //}
                #endregion

                bool result = _jobService.Delete(keyValue);
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
        /// <param name="keyValues">主键值</param>
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
                //if (!_jobService.CheckIsAllocateUserBatch(keyValues))
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "删除失败！原因：该角色下存在用户";
                //    return Ok(resultModel);
                //}
                #endregion

                bool result = _jobService.DeleteBatch(keyValues);
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
