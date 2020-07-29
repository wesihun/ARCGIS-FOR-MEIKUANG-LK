using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.JwtAuthorize;
using XY.Universal.Models;

namespace XY.Bussiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class RiskUnitManageController : ControllerBase
    {
        private readonly IRiskUnitManageService _riskunitmanageService;
        private readonly IMapper _mapper;
        public RiskUnitManageController(IRiskUnitManageService jobService,IMapper mapper)
        {
            _riskunitmanageService = jobService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetListByParentId(string test)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                //var data = _areaService.GetListByParentId(ParentId);
                //if (data != null)
                //{
                //    resultModel.code = 0;
                //    resultModel.msg = "获取区域列表数据成功";
                //    resultModel.data = data;
                //    resultModel.count = data.Count();
                //}
                //else
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "没有检索到数据";
                //    resultModel.count = 0;
                //}

                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                resultModel.count = 0;
                return Ok(resultModel);
            }
        }
    }
}
