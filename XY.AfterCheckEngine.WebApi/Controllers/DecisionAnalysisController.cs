using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.Universal.Models;
using XY.Utilities;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class DecisionAnalysisController : ControllerBase
    {
        private readonly IDecisionAnalysisService _decisionAnalysisService;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        public DecisionAnalysisController(IDecisionAnalysisService decisionAnalysisService, IMapper mapper, IRedisDbContext redisDbContext)
        {
            _decisionAnalysisService = decisionAnalysisService;
            _mapper = mapper;
            _redisDbContext = redisDbContext;
        }
        #region 获取数据
        [HttpGet]
        public IActionResult GetAll()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetAll();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        /// 获取左侧医院菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInstitutionList(string level,string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetInstitutionList(level,year);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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
        [HttpGet]
        public IActionResult GetDrugName()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetDrugName();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        /// 获取查询结果
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckResultStatusNew(string flag)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetCheckResultStatusNew(flag);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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



        [HttpGet]
        public IActionResult GetStaticsViews(string flag,string drugnames)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViews(flag,drugnames);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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

        [HttpGet]
        public IActionResult GetStaticsViews_JGJB(string flag, string drugname)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViews_JGJB(flag, drugname);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetStaticsViews_JGMC(string flag, string drugname, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViews_JGMC(flag, drugname,page,limit,ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = total;
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
        public IActionResult GetStaticsViewsByTable(string flag, string drugnames)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsByTable(flag, drugnames);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetStaticsViewsByTable_JGJB(string flag, string drugname)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsByTable_JGJB(flag, drugname);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetStaticsViewsByTable_JGMC(string flag, string drugname, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsByTable_JGMC(flag, drugname,page,limit,ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = total;
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
        public IActionResult GetStatisticsDrugInfos()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStatisticsDrugInfos();
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetStaticsViewsByRule(string flag,string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsByRule(flag,year);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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

        [HttpGet]
        public IActionResult GetStaticsViewsByJGJB(string flag,string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsByJGJB(flag,year);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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

        [HttpGet]
        public IActionResult GetStaticsViewsJGMCByRule(string rulename, string flag,string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsJGMCByRule(rulename,flag,year);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetStaticsViewsJGMCByDJ(string djname, string flag,string jgbm,string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetStaticsViewsJGMCByDJ(djname, flag,jgbm,year);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
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
        [HttpGet]
        public IActionResult GetYBHosInfoList(string year,string jgmc, string rulename,string djname, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetYBHosInfoList(year,jgmc, rulename, djname ,page, limit, ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = total;
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
        public IActionResult GetCheckResultInfosByRules(string year,string yljgdjName, string institutionName, string ruleCode, string dataType,int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _decisionAnalysisService.GetCheckResultInfosByRules(year,yljgdjName, institutionName, ruleCode,dataType, page, limit, ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = total;
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
        #endregion


        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AllPowerfulDrugEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {               
                AllPowerfulDrugEntity allpowerfulEntity = _mapper.Map<AllPowerfulDrugEntity>(model);
                allpowerfulEntity.CrowId = ConstDefine.CreateGuid();
                allpowerfulEntity.DrugCode = model.DrugCode;
                allpowerfulEntity.DrugName = model.DrugName;
                allpowerfulEntity.CommonName = model.CommonName;
                bool result = _decisionAnalysisService.Insert(allpowerfulEntity);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "新增失败";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "新增失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        [HttpPost]
        public ActionResult InsertOrUpdate(string drugcode, string drugname, string commoname)
        {
            string crowid = ConstDefine.CreateGuid(); 
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(drugcode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "药品编码不能为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(drugname))
                {
                    resultModel.code = -1;
                    resultModel.msg = "药品名称不能为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(commoname))
                {
                    resultModel.code = -1;
                    resultModel.msg = "通用名称不能为空！";
                    return Ok(resultModel);
                }
                if (_decisionAnalysisService.IsExsitsDrugCode(drugcode))
                {
                    resultModel.code = -1;
                    resultModel.msg = "药品编码为“"+drugcode+"”且名称为“"+drugname+"”的药品已存在，不能录入到系统！";
                    return Ok(resultModel);
                }
                bool result = _decisionAnalysisService.InsertOrUpdate(crowid,drugcode,drugname,commoname);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "新增失败";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "新增失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        [HttpPost]
        public ActionResult CreateDrug(string commonname)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(commonname))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请输入药品名称！";
                    return Ok(resultModel);
                }
                
                string result = _decisionAnalysisService.CreateDrug(commonname);
                if (result == "1")
                {
                    resultModel.code = 0;
                    resultModel.msg = "新增成功";
                }
                else if (result == "-2")
                {
                    resultModel.code = -2;
                    resultModel.msg = "没有满足条件的药品！";
                }
                else if (result == "-3")
                {
                    resultModel.code = 1;
                    resultModel.msg = "该药品未使用,无统计数据！";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "操作失败，请联系管理员！";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "新增失败" + ex.ToString();
                return Ok(resultModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteDrug(string commonname)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(commonname))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请选择药品名称！";
                    return Ok(resultModel);
                }

                bool result = _decisionAnalysisService.Delete(commonname);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除成功";
                }               
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "操作失败，请联系管理员！";
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
