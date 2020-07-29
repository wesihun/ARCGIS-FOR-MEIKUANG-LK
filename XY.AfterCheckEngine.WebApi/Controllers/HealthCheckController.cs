using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XY.Universal.Models;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// 服务检查
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHealthCheck()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                resultCountModel.code = 0;
                resultCountModel.msg = "正常";
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