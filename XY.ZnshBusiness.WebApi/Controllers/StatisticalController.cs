using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XY.Universal.Models;
using XY.ZnshBusiness.IService;
using Ocelot.JwtAuthorize;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using XY.DataNS;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly IStatisticalService _statisticalService;
        private IConfiguration _configuration;
        public StatisticalController(IStatisticalService statisticalService, IConfiguration configuration)
        {
            _statisticalService = statisticalService;
            _configuration = configuration; 
        }
        /// <summary>
        /// 后台主页风险信息
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskInfoTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskInfoTJ(User.GetCurrentUserOrganizeId());
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
        /// 后台主页风险信息 按部门分
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskInfoTJByDep()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskInfoTJByDep(User.GetCurrentUserOrganizeId());
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
        /// 大数据分析
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetDataAnalysis()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                DateTime date = Convert.ToDateTime(_configuration.GetSection("ConnectionStrings").GetSection("DateTime").Value);
                var data = _statisticalService.GetDataAnalysis(User.GetCurrentUserOrganizeId(), date);
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
        /// 风险等级数量
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskLevelCount(int year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskLevelCount(User.GetCurrentUserOrganizeId(), year);
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
        /// 风险辨识数量
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskIdentifyCount(int year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskIdentifyCount(User.GetCurrentUserOrganizeId(), year);
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
        /// 风险因素类别统计
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskFactorTypeTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskFactorTypeTJ(User.GetCurrentUserOrganizeId());
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
        /// 可能导致事故类型统计
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskAccidentTypeTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskAccidentTypeTJ(User.GetCurrentUserOrganizeId());
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
        /// 作业安全风险比较图
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskByLevelTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskByLevelTJ(User.GetCurrentUserOrganizeId());
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
        /// 隐患趋势统计
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetHiddenTrendTJ(int year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetHiddenTrendTJ(User.GetCurrentUserOrganizeId(),year);
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
        /// 后台主页统计数
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetCountData()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetCountData();
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
        /// 后台主页统计柱状图  各级别风险点数量
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetRiskUnitTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetRiskUnitTJ();
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
        /// 后台主页统计饼状图  隐患类型统计
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetHiddenTJ()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _statisticalService.GetHiddenTJ();
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
        /// 组织各类别检查计划执行情况统计表
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
                var data = _statisticalService.PlanStatistical();
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
        /// 检查结果统计表
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetCheckResultList(string orgid, string states, string planname, string riskpointname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _statisticalService.CheckResultStatistical(orgid, currOrgId, states, planname, riskpointname, page, limit, ref count);
                if (data.Count != 0)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.count = count;
                    resultCountModel.data = data;
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
        /// 获取操作说明列表
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetOIList(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _statisticalService.GetOIList(page, limit, ref count);
                if (data.Count != 0)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.count = count;
                    resultCountModel.data = data;
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
        

    }
}
