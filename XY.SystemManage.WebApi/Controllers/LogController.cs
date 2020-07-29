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
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper; 
        public LogController(ILogService logService, IMapper mapper)
        {
            _logService = logService;
            _mapper = mapper;
        }

        #region 获取数据
        /// <summary>
        /// 获取数据 
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">每页显示数量</param>
        [HttpGet]
        public IActionResult GetListByCondition(DateTime? startTime, DateTime? endTime, string condition, string keyWord, int pageIndex, int pageSize)
        {
            try
            {
                if (endTime != null)
                {
                    endTime = Convert.ToDateTime(Convert.ToDateTime(endTime).ToString("yyyy-MM-dd 23:59:59"));
                }
                RespResultCountViewModel resultCountModel = new RespResultCountViewModel();
                if (startTime > endTime)
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "起止时间不能大于结束时间";
                    return Ok(resultCountModel);
                }
                int totalCount = 0; //出参返回总条数
                var resultData = _logService.GetAll(startTime, endTime, pageIndex, pageSize, ref totalCount);
                if (resultData != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
                    resultCountModel.count = totalCount;
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

                RespResultViewModel resultModel = new RespResultCountViewModel();
                resultModel.code = -1;
                resultModel.msg = "操作失败！原因：" + ex.ToString();
                return Ok(resultModel);
            }
        }
        #endregion

        [HttpGet]
        public IActionResult GePagetListByCondition(DateTime? startTime, DateTime? endTime,  int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var data = _logService.GePagetListByCondition(startTime, endTime, page, limit, ref totalcount);
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



        #region 批量删除
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败！原因：缺少主键集合";
                    return Ok(resultModel);
                }
                bool result = _logService.DeleteBatch(keyValues);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除日志成功";
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
                resultModel.msg = "删除失败" + ex.ToString();
                return Ok(resultModel);
            }
        }
        #endregion
    }
}
