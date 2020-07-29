using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;
using NLog;
using Ocelot.JwtAuthorize;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class JobRiskManageController : Controller
    {
        private readonly Logger nlog = LogManager.GetCurrentClassLogger(); //获得日志实体;
        private readonly IJobRiskManageService _jobriskService;
        private readonly IMapper _mapper;
        public JobRiskManageController(IJobRiskManageService jobriskService, IMapper mapper)
        {
            _jobriskService = jobriskService;
            _mapper = mapper;
        }
        #region 获取数据
        /// <summary>
        /// 根据传入的jobid  获取该岗位下的所有人员信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserListByJobId(string jobid)
        {
            //LogEventInfo log = new LogEventInfo(LogLevel.Info, User.GetCurrentUserName(), "测试日志Info");
            //nlog.Log(log);
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _jobriskService.GetUserListByJobId(jobid);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
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
        /// 根据传入的roleid  获取该岗位下的所有人员信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserListByRoleId(string roleid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _jobriskService.GetUserListByRoleId(roleid);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
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
        /// 根据条件获取岗位风险点管理列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid,string role, string job,string riskpointname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _jobriskService.GetPageListByCondition(orgid,currOrgId, role,job,riskpointname, page, limit, ref count);
                if (data.Count != 0)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
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
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(JobRiskEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgId) || string.IsNullOrEmpty(model.DepId) || string.IsNullOrEmpty(model.RoleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.RiskPointBH) || string.IsNullOrEmpty(model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                if (_jobriskService.IsExist(model.UserId, model.RiskPointBH, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请勿为同一个分配相同的风险点！";
                    return Ok(resultModel);
                }
                #endregion
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                bool result = _jobriskService.Insert(model);
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
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(JobRiskEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.Id) )
                {
                    resultModel.code = -1;
                    resultModel.msg = "主键不能为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrgId) || string.IsNullOrEmpty(model.DepId) || string.IsNullOrEmpty(model.RoleId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.RiskPointBH) || string.IsNullOrEmpty(model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                if (_jobriskService.IsExist(model.UserId, model.RiskPointBH, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请勿为同一个分配相同的风险点！";
                    return Ok(resultModel);
                }
                #endregion              
                bool result = _jobriskService.Update(model);
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
                    resultModel.msg = "删除失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _jobriskService.Delete(keyValue);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败";
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
                    resultModel.msg = "批量删除信息失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _jobriskService.DeleteBatch(keyValues);

                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除信息成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除信息失败";
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
