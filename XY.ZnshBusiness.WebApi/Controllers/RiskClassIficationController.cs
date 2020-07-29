using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Ocelot.JwtAuthorize;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.IService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class RiskClassIficationController : Controller
    {
        private readonly IRiskClassIficationService _riskclassificationService;
        private readonly IMapper _mapper;
        public RiskClassIficationController(IRiskClassIficationService riskclassificationService, IMapper mapper)
        {
            _riskclassificationService = riskclassificationService;
            _mapper = mapper;
        }
        #region 获取数据
        /// <summary>
        /// 根据条件获取风险点分级管理列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid,string riskpointbh, string riskpointname,string risklevel, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _riskclassificationService.GetPageListByCondition(orgid, currOrgId, riskpointbh, riskpointname, risklevel,page, limit, ref count);
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
        /// 根据条件获取该风险点的风险清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRiskPointList(string orgid,string riskpointbh)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _riskclassificationService.GetRiskPointList(orgid, currOrgId, riskpointbh);
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
        #endregion

        #region 提交数据
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(RiskClassIficationEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            var testid = User.GetCurrentUserOrganizeId().ToString();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgId) || string.IsNullOrEmpty(model.RiskPointBH))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                //if (_riskclassificationService.IsExist(model.OrgId, model.RiskPointBH, model.Id))
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "请勿重复添加风险点编号！";
                //    return Ok(resultModel);
                //}
                #endregion
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                model.RiskR = model.SeverityS * model.PossibleL;
                if (model.RiskR >= 12 && model.RiskR <= 20)
                {
                    model.RiskLevel = "1";
                }
                if (model.RiskR >= 8 && model.RiskR <= 10)
                {
                    model.RiskLevel = "2";
                }
                if (model.RiskR >= 4 && model.RiskR <= 6)
                {
                    model.RiskLevel = "3";
                }
                if (model.RiskR <4)
                {
                    model.RiskLevel = "4";
                }
                bool result = _riskclassificationService.Insert(model);
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

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(RiskClassIficationEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgId) || string.IsNullOrEmpty(model.RiskPointBH))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                //if (_riskclassificationService.IsExist(model.OrgId, model.RiskPointBH, model.Id))
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "请勿重复添加风险点编号！";
                //    return Ok(resultModel);
                //}
                #endregion     
                model.RiskR = model.SeverityS * model.PossibleL;
                if (model.RiskR >= 12 && model.RiskR <= 20)
                {
                    model.RiskLevel = "1";
                }
                if (model.RiskR >= 8 && model.RiskR <= 10)
                {
                    model.RiskLevel = "2";
                }
                if (model.RiskR >= 4 && model.RiskR <= 6)
                {
                    model.RiskLevel = "3";
                }
                if (model.RiskR < 4)
                {
                    model.RiskLevel = "4";
                }
                bool result = _riskclassificationService.Update(model);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "修改成功";
                    resultModel.data = null;
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "修改失败";
                    resultModel.data = null;
                }
                return Ok(resultModel);
            }
            catch (Exception ex)
            {
                resultModel.code = -1;
                resultModel.msg = "操作失败:" + ex.Message;
                resultModel.data = null;
                return Ok(resultModel);
            }
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Remove(string keyValue)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _riskclassificationService.Delete(keyValue);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除失败";
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
                bool result = _riskclassificationService.DeleteBatch(keyValues);

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
