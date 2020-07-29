using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    //[Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class HealthCareCheckResultController : ControllerBase
    {
        private readonly IHealthCareCheckResultService _healthCareCheckResultService;
        private readonly IRedisDbContext _redisDbContext;
        public HealthCareCheckResultController(IHealthCareCheckResultService healthCareCheckResultService, IRedisDbContext redisDbContext)
        {
            _healthCareCheckResultService = healthCareCheckResultService;
            _redisDbContext = redisDbContext;
        }
        /// <summary>
        /// 获取审核结果
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetHealthCareCheckResult(string resgisterCode,string flag)
        {
            var resultCountModel = new RespResultCountViewModel();
            if (string.IsNullOrEmpty(resgisterCode))
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "请输入登记编码";
                return Ok(resultCountModel);
            }
            try
            {
                var data = _healthCareCheckResultService.healthCareCheckResultDtos(resgisterCode,flag);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";

                }
                String str = "callback" + "(" + JsonConvert.SerializeObject(resultCountModel).Replace("NULL","null") + ");";
                return Ok(str);
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