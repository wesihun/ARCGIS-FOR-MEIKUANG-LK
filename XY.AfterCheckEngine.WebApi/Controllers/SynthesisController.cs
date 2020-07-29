using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XY.AfterCheckEngine.IService;
using XY.Universal.Models;
using XY.ZnshBusiness.IService;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class SynthesisController : ControllerBase
    {
        private readonly ISynthesisService _synthesisService;
        private readonly IMapper _mapper;
        public SynthesisController(ISynthesisService systhesisService, IMapper mapper)
        {
            _synthesisService = systhesisService;
            _mapper = mapper;
        }
        /// <summary>
        /// 统计总数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTotal()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
               var data = _synthesisService.GetTotal();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取总数成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取总床位数与总在院人数匹配标识
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFlag()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                bool flag = _synthesisService.GetFlag();
                resultCountModel.code = 0;
                resultCountModel.data = flag;
                resultCountModel.msg = "获取标识成功";
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
        /// 获取医院科室床位数与在院人数匹配标识
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYYFlag()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetYYFlag();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取标识成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 床位在院人数弹出图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetBedInHospitalCharts(int hoscount)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetBedInHospitalCharts(hoscount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取床位在院人数图表成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 床位在院人数弹出列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetBedInHospitalList(string name, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetBedInHospitalList(name,page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取床位在院人数图表成功";
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
        /// 获取科室床位信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetKSBedInfoList(string code, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetKSBedInfoList(code, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室床位信息成功";
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
        /// 获取全盟用药诊疗情况弹出图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDrugZLCharts(int hoscount)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetDrugZLCharts(hoscount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取全盟用药诊疗情况弹出图表成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取全盟用药诊疗情况弹出列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDrugZLList(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetDrugZLList(page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取全盟用药诊疗情况弹出列表成功";
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
        /// 获取全盟用药诊疗钻取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQDrugZLList(string InstitutionCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetZQDrugZLList(InstitutionCode,page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取全盟用药诊疗钻取列表成功";
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
        /// 获取疾病钻取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQDiseaseListByName(string InstitutionCode, string diseasename, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetZQDiseaseListByName(InstitutionCode, diseasename, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疾病钻取列表成功";
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
        /// 住院次均费用对比分析弹出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTCZYCJList(string levelcode, string hosname, decimal cjfy, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetTCZYCJList(levelcode, hosname,cjfy, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取住院次均费用对比分析弹出列表成功";
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
        /// 多个图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetManayCharts()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetManayCharts();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取多个图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 单个图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSingleCharts(string yljgbh,int second,int count)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetSingleCharts(yljgbh,second,count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取单个图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 统计排名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRank()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetRank();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取排名数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 主要疾病排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDiseaseList(string name,int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetDiseaseList(name,page, limit,ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疾病排名数据成功";
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
        /// 西药用量排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetXYList(string name, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetXYList(name, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取西药用量排名数据成功";
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
        /// 各医院自费费用排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZFFYList(string name, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetZFFYList(name, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取各医院自费费用排名数据成功";
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
        /// 钻取各医院疾病次均费用排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQDiseaseList(string keyword,string diseaseName, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetZQDiseaseList(keyword, diseaseName, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取钻取各医院次均费用排名list数据成功";
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
        /// 钻取各医院自费费用排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQZFFYList(string InstitutionCode, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetZQZFFYList(InstitutionCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取 钻取各医院自费费用数据成功";
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
        /// 钻取各医院检查检验费用排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQJCJYList(string InstitutionCode, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetZQJCJYList(InstitutionCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取钻取各医院检查检验费用数据成功";
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
        /// 各医院检查检验费用排名list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetJCJYList(string name, int page, int limit)
        {
            int count = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetJCJYList(name, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取各医院检查检验费用排名数据成功";
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
        /// 获取地图数据事前
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMapInfo()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMapInfo();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取地图数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取目录费用占比
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMLFYpList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMLFYpList();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取目录费用占比成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取各个医院目录费用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHosMLFYList(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetHosMLFYList(page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取各个医院目录费用成功";
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
        /// 获取地图数据智能审核
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMapXY()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMapXY();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取地图数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取医院信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMapYYXXInfo(string code)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMapYYXXInfo(code);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取地图数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonInfo(string code)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetPersonInfo(code);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取人员信息数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取住院人次分流情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInHospitalFLList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetInHospitalFLList();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取住院人次分流成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取医院人次分流情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTCInHospitalFLList(string levelcode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetTCInHospitalFLList(levelcode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取医院人次分流情况成功";
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
        /// 获取住院基金分流情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInHospitalFundList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetInHospitalFundList();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取住院基金分流成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取医院基金分流数据情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTCInHospitalFundList(string levelcode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetTCInHospitalFundList(levelcode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取医院基金分流数据情况成功";
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
        /// 钻取统计概况前4个图表list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQChartsFourList(string InstitutionCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetZQChartsFourList(InstitutionCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "钻取统计概况前4个图表list成功";
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
        /// 获取丙类目录外费用占比分析弹出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTCCharts1List(string levelcode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetTCCharts1List(levelcode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取丙类目录外费用占比分析弹出";
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
        /// 获取次均费用和次均支付对比分析弹出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetTCCharts2List(string levelcode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetTCCharts2List(levelcode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取次均费用和次均支付对比分析弹出成功";
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
        /// 获取医院机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYYXXTreeList()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetYYXXTreeList();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取医院机构树成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 根据药品名称弹出图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetChartsByDrugNameCharts(string ypmc,int count)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetChartsByDrugNameCharts(ypmc,count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "根据药品名称弹出图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 根据药品名称弹出list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetChartsByDrugNameList(string ypmc, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetChartsByDrugNameList(ypmc, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "根据药品名称弹出list数据成功";
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
        /// 根据疾病名称弹出图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetChartsByDiseaseNameCharts(string jbmc, int count)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetChartsByDiseaseNameCharts(jbmc, count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "根据疾病名称弹出图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 根据疾病名称弹出list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetChartsByDiseaseNameList(string jbmc, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetChartsByDiseaseNameList(jbmc, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "根据疾病名称弹出list数据成功";
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
        /// 丙类目录外费用占比分析 次均费用和次均支付对比分析 各个医院药占比分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetThreeChartsList(int count)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetThreeChartsList(count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "根据丙类目录外费用占比分析 次均费用和次均支付对比分析 各个医院药占比分析数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 各个医院药占比分析List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCharts3List(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                var data = _synthesisService.GetCharts3List(page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取各个医院药占比分析List数据成功";
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
        /// 获取地图界面图表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMapCharts(string code)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMapCharts(code);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取地图界面图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取地图界面list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMapList(string code)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetMapList(code);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取地图界面图表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取医嘱列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYZList(string yljgbh, string zybh)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetYZList(yljgbh, zybh);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取医嘱列表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count;
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
        /// 钻取获取地图界面药占比list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQMapYZBList(string InstitutionCode,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetZQMapYZBList(InstitutionCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取钻取获取地图界面药占比list成功";
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
        /// 钻取获取地图界面次均费用list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQMapCJFYList(string InstitutionCode,int month, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetZQMapCJFYList(InstitutionCode, month, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取钻取获取地图界面次均费用list成功";
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
        /// 钻取获取地图界面目录外占比list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetZQMapMLWZBList(string InstitutionCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetZQMapMLWZBList(InstitutionCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取钻取获取地图界面目录外占比list成功";
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
        /// 获取挂床人员详细信息列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetGCPersonList(string crowid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _synthesisService.GetGCPersonList(crowid);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取挂床人员详细信息列表数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = 1;
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
        /// 获取处方明细
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPreList(string HosRegisterCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _synthesisService.GetPreList(HosRegisterCode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取处方明细成功";
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
        /// 统计违规人次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetViolationPersonCount(string personName,string datatype,string rulecode,string jgcode,string hosregistercode,string checkResultInfoCode,string rulesName,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;               
                switch (datatype)
                {
                    case "1":
                        var data1 = _synthesisService.GetStaticsViewsByRule(rulesName,page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data1;
                        resultCountModel.count = count;
                        break;
                    case "2":
                        var data2 = _synthesisService.GetStaticsViewsJGMCByRule(rulecode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data2;
                        resultCountModel.count = count;
                        break;
                    case "3":
                        var data3 = _synthesisService.GetYBHosInfoList(personName,jgcode, rulecode,null,page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data3;
                        resultCountModel.count = count;
                        break;
                    case "4":
                        var data4 = _synthesisService.GetCFDeatilList(hosregistercode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data4;
                        resultCountModel.count = count;
                        break;
                    case "5":
                        var data5 = _synthesisService.GetWGCFDeatilListByKey(checkResultInfoCode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data5;
                        resultCountModel.count = count;
                        break;
                    default:
                        resultCountModel.code = -1;
                        resultCountModel.msg = "没有检索到数据";
                        break;
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
        /// 医院等级违规人次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHosLevelCount(string personName,string datatype, string djcode, string jgcode, string hosregistercode, string checkResultInfoCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                switch (datatype)
                {
                    case "1":
                        var data1 = _synthesisService.GetStaticsViewsByJGJB();
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data1;
                        break;
                    case "2":
                        var data2 = _synthesisService.GetHosListByLevel(djcode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data2;
                        resultCountModel.count = count;
                        break;
                    case "3":
                        var data3 = _synthesisService.GetStaticsViewsJGMCByDJ(djcode, jgcode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data3;
                        resultCountModel.count = count;
                        break;
                    case "4":
                        var data4 = _synthesisService.GetYBHosInfoList(personName,jgcode, null, djcode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data4;
                        resultCountModel.count = count;
                        break;
                    case "5":
                        var data5 = _synthesisService.GetCFDeatilList(hosregistercode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data5;
                        resultCountModel.count = count;
                        break;
                    case "6":
                        var data6 = _synthesisService.GetWGCFDeatilListByKey(checkResultInfoCode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data6;
                        resultCountModel.count = count;
                        break;
                    default:
                        resultCountModel.code = -1;
                        resultCountModel.msg = "没有检索到数据";
                        break;
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
        /// 按医疗机构统计违规人数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHosPersonCount(string personName,string datatype, string jgcode, string hosregistercode, string checkResultInfoCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                switch (datatype)
                {
                    case "1":
                        var data1 = _synthesisService.GetHosListByLevel(null,page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data1;
                        resultCountModel.count = count;
                        break;
                    case "2":
                        var data2 = _synthesisService.GetStaticsViewsJGMCByDJ(null, jgcode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data2;
                        resultCountModel.count = count;
                        break;
                    case "3":
                        var data3 = _synthesisService.GetYBHosInfoList(personName,jgcode, null, null, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data3;
                        resultCountModel.count = count;
                        break;
                    case "4":
                        var data4 = _synthesisService.GetCFDeatilList(hosregistercode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data4;
                        resultCountModel.count = count;
                        break;
                    case "5":
                        var data5 = _synthesisService.GetWGCFDeatilListByKey(checkResultInfoCode, page, limit, ref count);
                        resultCountModel.code = 0;
                        resultCountModel.msg = "成功";
                        resultCountModel.data = data5;
                        resultCountModel.count = count;
                        break;
                    default:
                        resultCountModel.code = -1;
                        resultCountModel.msg = "没有检索到数据";
                        break;
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