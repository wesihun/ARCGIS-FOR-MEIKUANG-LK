using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;
using Ocelot.JwtAuthorize;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class CheckPlanController : Controller
    {
        private readonly ICheckPlanService _checkplanService;
        private readonly IMapper _mapper;
        public CheckPlanController(ICheckPlanService checkplanService, IMapper mapper)
        {
            _checkplanService = checkplanService;
            _mapper = mapper;
        }
        /// <summary>
        /// 根据条件获取检查计划列表并分页
        /// </summary>
        /// <remarks>
        /// 说明:
        /// condition  keyword联合使用
        /// </remarks>
        /// <param name="orgid">机构id</param>
        /// <param name="planname">计划名</param>
        /// <param name="person">人员</param>
        /// <param name="executionmodel">执行周期</param>
        /// <param name="page">当前页</param>
        /// <param name="limit">每页行数</param>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid,string planname, string person,string executionmodel, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _checkplanService.GetPageListByCondition(orgid, currOrgId, planname, person, executionmodel, page, limit, ref count);
                if (data.Count != 0)
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
        /// 获取该人员下的检查点信息
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="userid">id主键</param>
        /// <returns></returns>  
        [HttpGet]
        public IActionResult GetCheckTableListByUserid(string userid)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _checkplanService.GetCheckTableListByUserid(userid);
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
        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <remarks>
        /// 说明:
        /// model里不传入的为Id  CreateTime LastCompleteTime 
        /// RiskBH RiskName 多条用英文逗号分隔
        /// 其中DeleteMark 1为推送  0为不推送
        /// </remarks>
        /// <param name="model">单条</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(CheckPlanEnity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (_checkplanService.IsExistUserId(model.UserId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "该人员已有计划 请勿再次添加！";
                    return Ok(resultModel);
                }
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                bool result = _checkplanService.Insert(model);
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
                resultModel.msg = "操作失败:" + ex.Message;
                return Ok(resultModel);
            }
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (keyValues.Count() <= 0)
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除信息失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _checkplanService.DeleteBatch(keyValues);

                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除信息成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除信息失败";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.ToString();
                resultModel.data = null;
                return Ok(resultModel);
            }
        }
        #endregion
    }
}
