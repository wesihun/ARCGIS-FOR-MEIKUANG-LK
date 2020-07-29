using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.Universal.Models;
using Ocelot.JwtAuthorize;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class ComplaintStatisticalController : ControllerBase
    {
        private readonly IAfterCheckService _afterCheckService;
        private readonly IComplaintStatisticalService _complaintStatisticalService;
        public ComplaintStatisticalController(IAfterCheckService afterCheckService, IComplaintStatisticalService complaintStatisticalService)
        {
            _afterCheckService = afterCheckService;
            _complaintStatisticalService = complaintStatisticalService;
        }
        /// <summary>
        /// 获取医院扣款列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYYKKList(string querystr,int page, int limit)
        {
            YYKKGL result = new YYKKGL();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<YYKKGL>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                bool isadmin = false;
                string curryydm = User.GetCurrentUserOrganizeId();
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
                {
                    isadmin = true;
                }
                var data = _complaintStatisticalService.GetYYKKList(result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        /// <summary>
        /// 根据医疗机构获取审核用户列表
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IActionResult GetCheckUserListByYLJG(string querystr, int page, int limit)
        {
            YYKKGL result = new YYKKGL();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<YYKKGL>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                bool isadmin = false;
                string curryydm = User.GetCurrentUserOrganizeId();
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
                {
                    isadmin = true;
                }
                var data = _complaintStatisticalService.GetCheckUserListsByYLJG(result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        /// <summary>
        /// 按规则分
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public IActionResult GetListByRulesCode(string querystr, int page, int limit)
        {
            ListByRulesCodeQuery result = new ListByRulesCodeQuery();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<ListByRulesCodeQuery>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _complaintStatisticalService.GetListByRulesCode(result, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        /// 获取审核结果统计疑点信息
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYDInfoList(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintStatisticalService.GetYDInfoList(registerCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疑点信息数据成功";
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
        /// 获取审核状态查询中的状态查询条件  根据权限设置
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetQueryStates()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                bool isadmin = false;
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
                {
                    isadmin = true;
                }
                var data = _complaintStatisticalService.GetQueryStates(isadmin,curryydm);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疑点信息数据成功";
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
        /// 获取审核状态查询
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetListByStates(string step, string querystr, string states, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
            }
            int totalcount = 0;
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                bool isadmin = false;
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
                {
                    isadmin = true;
                }
                var data = _complaintStatisticalService.GetListByStates(step, states, result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
    }
}