using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XY.Universal.Models;
using XY.ZnshBusiness.IService;
using Ocelot.JwtAuthorize;
using XY.ZnshBusiness.Entities;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class AppController : Controller
    {
        private readonly IAppService _appService;
        private readonly IMapper _mapper;
        public AppController(IAppService appService, IMapper mapper)
        {
            _appService = appService;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取该人员下的检查计划信息
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetPlanList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {           
                var data = _appService.GetPlanList(User.GetCurrentUserId());
                if (data == null)
                    resultCountModel.data = new { };
                else
                    resultCountModel.data = data;
                resultCountModel.code = 0;
                resultCountModel.msg = "成功";            
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
        /// 获取检查计划信息带分页
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetPlanPageList(string orgid, string planName, string riskPointName,string states,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _appService.GetAllPlanList(orgid, currOrgId, planName, riskPointName, states, page, limit, ref count);
                if (data.Count == 0)
                    resultCountModel.data = new { };
                else
                    resultCountModel.data = data;
                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                resultCountModel.count = count;
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
        /// 获取该登录人员计划里的检查点信息
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskPointList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _appService.GetRiskPointList(User.GetCurrentUserId());
                if (data == null)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;
                resultCountModel.msg = "成功";
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
        /// 根据二维码获取该风险点的排查事项
        /// </summary>
        /// <remarks>
        /// states1  1已完成  0未完成     风险点排查事项完成情况
        /// states2  1已上报  0未上报     风险点上报情况
        /// </remarks>
        /// <param name="riskBH"></param>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetQRCodeList(string riskBH)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                if(!_appService.IsExitCurrUserId(riskBH, User.GetCurrentUserId()))
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "未分配该检查点!";
                    return Ok(resultCountModel);
                }
                var data = _appService.GetQRCodeList(riskBH, User.GetCurrentUserId());
                if (data == null)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;
                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
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
        /// 获取该检查计划执行情况
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetPlanCheckList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _appService.GetPlanCheckList();
                if (data == null)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;
                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
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
        /// 对单个排查事项进行检查提交
        /// </summary>
        /// <remarks>
        ///  id 为该排查事项的主键id
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetQRCodeCreat(string id)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var flag = _appService.GetQRCodeCreat(id, User.GetCurrentUserId(),User.GetCurrentUserRealName());
                if (flag)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "提交失败";
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
        /// 获取风险告知卡情况
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskNoticeList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _appService.GetRiskNoticeList(User.GetCurrentUserId());
                if (data == null)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;
                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
        }

    }
}
