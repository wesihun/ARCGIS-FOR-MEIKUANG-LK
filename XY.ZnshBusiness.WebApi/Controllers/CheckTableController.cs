using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.JwtAuthorize;
using XY.Universal.Models;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;
using XY.ZnshBusiness.IService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class CheckTableController : Controller
    {
        private readonly ICheckTableService _checktableService;
        private readonly IMapper _mapper;
        public CheckTableController(ICheckTableService checktalbleService, IMapper mapper)
        {
            _checktableService = checktalbleService;
            _mapper = mapper;
        }
        /// <summary>
        /// 根据条件获取检查表管理列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid,string condition, string keyword, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _checktableService.GetPageListByCondition(condition, keyword,orgid, currOrgId, page, limit, ref count);
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
        public IActionResult GetCheckPointSelect(string orgid, string userid, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _checktableService.GetCheckPointSelect(orgid,userid,page, limit, ref count);
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
        /// 批量授权
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Authorized(List<AuthorizedDto> model)
        {
            var resultModel = new RespResultCountViewModel();
            if (model.Count() <= 0)
            {
                resultModel.code = 0;
                resultModel.msg = "授权失败！原因：缺少实体集合";
                return Ok(resultModel);
            }
            try
            {
                bool result = _checktableService.InsertBatch(model);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "分配成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "分配失败！";
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败！" + ex.Message;
                return Ok(resultModel);
            }
        }
        /// <summary>
        /// 获取风险点分级列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetClassClassIficationList(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                var data = _checktableService.GetClassClassIficationList(page, limit, ref count);
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
        /// 获取已选中的排查事项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCheckItems(string userid,string riskpointbh)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _checktableService.GetCheckItems(userid, riskpointbh);
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
        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="classificationId">分级id集合</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(List<string> classificationId)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                bool result = _checktableService.Insert(classificationId);
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
                bool result = _checktableService.DeleteBatch(keyValues);

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
