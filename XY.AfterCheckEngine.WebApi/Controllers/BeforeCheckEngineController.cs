using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ocelot.JwtAuthorize;
using XY.AfterCheckEngine.IService;
using XY.Universal.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class BeforeCheckEngineController : Controller
    {
        private readonly IBeforeCheckEngineService _ibeforeCheckEngineService;
        private readonly IAfterCheckService _afterCheckService;
        public BeforeCheckEngineController(IBeforeCheckEngineService beforeCheckEngineService, IAfterCheckService afterCheckService)
        {
            _ibeforeCheckEngineService = beforeCheckEngineService;
            _afterCheckService = afterCheckService;
        }
        /// <summary>
        /// 获取事前审核结果数据
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="states">状态   1疑似   2刚性违规</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetBeforeResultList(string querystr, string states, int page, int limit)
        {
            QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            bool isadmin = false;
            string curryydm = User.GetCurrentUserOrganizeId();
            if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
            {
                isadmin = true;
            }
            try
            {
                var data = _ibeforeCheckEngineService.GetBeforeResultList(states, result, isadmin, curryydm, page, limit, ref totalcount);
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
        /// 获取事前审核结果数据详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetBeforeResultDetailList(string registerCode,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _ibeforeCheckEngineService.GetBeforeResultDetailList(registerCode, page, limit, ref totalcount);
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
