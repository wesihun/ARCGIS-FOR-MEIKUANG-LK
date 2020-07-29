using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.Universal.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class HosDayErrorController : Controller
    {
        private readonly IHosDayErrorService _hosdayerrorservice;
        private readonly IMapper _mapper;
        public HosDayErrorController(IHosDayErrorService hosdayerrorservice, IMapper mapper)
        {
            _hosdayerrorservice = hosdayerrorservice;
            _mapper = mapper;
        }
        // 获取住院天数异常list
        [HttpGet]
        public IActionResult GetPageListByCondition(string condition, string keyword, string idnumber, string yljgbh, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetPageListByCondition(condition, keyword, idnumber, yljgbh, page, limit, ref count);
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
        //获取分解住院list
        [HttpGet]
        public IActionResult GetDecomposeHos(string condition, string keyword, string querystr, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetDecomposeHos(condition, keyword, querystr, page, limit, ref count);
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
        /// 根据编码获取分解住院信息
        /// </summary>
        /// <param name="institutioncode">就诊机构编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDecomposehosByCode(string idnumber, string institutioncode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _hosdayerrorservice.GetDecomposehosByCode(idnumber, institutioncode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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

        //获取入出院日期异常list
        [HttpGet]
        public IActionResult GetInOutDate(string condition, string keyword, string querystr, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetInOutDate(condition, keyword, querystr, page, limit, ref count);
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
        /// 根据编码获取入出院日期异常住院信息
        /// </summary>
        /// <param name="institutioncode">就诊机构编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInOutDateByCode(string idnumber, string institutioncode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _hosdayerrorservice.GetInOutDateByCode(idnumber, institutioncode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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

        //获取住院处方明细
        [HttpGet]
        public IActionResult GetCFDeatilListByCode(string hosregistercode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetCFDeatilList(hosregistercode, page, limit, ref count);
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
        //获取门诊处方明细
        [HttpGet]
        public IActionResult GetCFDeatilListByCodeCli(string hosregistercode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetCFDeatilListCli(hosregistercode, page, limit, ref count);
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
        //获取就诊医院list
        [HttpGet]
        public IActionResult GetYYXXList(string condition, string keyword, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetYYXXList(condition, keyword, page, limit, ref count);
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
        //获取疾病信息
        [HttpGet]
        public IActionResult GetDiseaseList(string condition, string keyword, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetDiseaseList(condition, keyword, page, limit, ref count);
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

        //获取首页显示数据
        [HttpGet]
        public IActionResult GetHomeIndexParameter()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetParameterList();
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
        //获取首页显示数据,门诊
        [HttpGet]
        public IActionResult GetHomeIndexParameterClinic()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _hosdayerrorservice.GetParameterList_Clinic();
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
        [HttpGet]
        public IActionResult AddHomeIndexParameterRedis()
        {          
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                bool flag = _hosdayerrorservice.AddHomeIndexParameterRedis();
                if (flag)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "加载缓存成功";
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "加载缓存失败";
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
