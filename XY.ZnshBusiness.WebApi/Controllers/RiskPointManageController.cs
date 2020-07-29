using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using Ocelot.JwtAuthorize;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XY.DataNS;
using XY.Universal.Models;
using XY.Utilities;
using XY.ZnshBusiness.Entities;
using XY.ZnshBusiness.Entities.Dtos;
using XY.ZnshBusiness.IService;

namespace XY.ZnshBusiness.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("bussiness/[controller]/[action]")]
    [ApiController]
    public class RiskPointManageController : ControllerBase
    {
        private readonly IRiskPointManageService _riskmanageService;
        private readonly IQRCodeService _qrcodeService;
        private readonly IMapper _mapper;
        public RiskPointManageController(IRiskPointManageService riskmanageService, IQRCodeService qrcodeService, IMapper mapper)
        {
            _riskmanageService = riskmanageService;
            _qrcodeService = qrcodeService;
            _mapper = mapper;
        }

        #region 获取数据
        /// <summary>
        /// 根据条件获取风险点管理列表并分页
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid,string riskpointbh, string riskpointname,string riskunitbh,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _riskmanageService.GetPageListByCondition(orgid, currOrgId, riskpointbh, riskpointname, riskunitbh, page, limit, ref count);
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
        public ActionResult Create(RiskPointEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构Id不能为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.BH))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编号不能为空！";
                    return Ok(resultModel);
                }
                model.RiskPointBH = model.RiskUnitBH + "-" + model.BH;
                if (_riskmanageService.IsExist(model.OrgId, model.RiskPointBH, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请勿重复添加风险点编号！";
                    return Ok(resultModel);
                }
                #endregion
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;

                //创建目录
                string activeDir = XYDbContext.QRPath;
                string newPath = activeDir + "/" + model.OrgName;
                if (!Directory.Exists(newPath))//如果不存在就创建 dir 文件夹  
                    Directory.CreateDirectory(newPath);

                #region 生成二维码
                RiskPointDto riskPointDto = new RiskPointDto();
                riskPointDto.RiskBH = model.RiskPointBH;
                riskPointDto.RiskName = model.Name;
                riskPointDto.OrgId = model.OrgId;
                riskPointDto.OrgName = model.OrgName;
                string jsonStr = JsonConvert.SerializeObject(riskPointDto);
                var bitmap = _qrcodeService.GetQRCode(jsonStr, 10);
                var QRUrl = "/" + Guid.NewGuid().ToString() + ".bmp";
                var path = newPath + QRUrl;
                #endregion

                model.QRCodeUrl = path;
                bool result = _riskmanageService.Insert(model);
                if (result)
                { 
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
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
        public ActionResult Edit(RiskPointEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构Id不能为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.BH))
                {
                    resultModel.code = -1;
                    resultModel.msg = "编号不能为空！";
                    return Ok(resultModel);
                }
                model.RiskPointBH = model.RiskUnitBH + "-" + model.BH;
                if (_riskmanageService.IsExist(model.OrgId, model.RiskPointBH, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请勿重复添加风险点编号！";
                    return Ok(resultModel);
                }
                #endregion

                string activeDir = XYDbContext.QRPath;
                string newPath = activeDir + "/" + model.OrgName;

                #region 生成二维码 先删除原来的二维码
                RiskPointDto riskPointDto = new RiskPointDto();
                riskPointDto.RiskBH = model.RiskPointBH;
                riskPointDto.RiskName = model.Name;
                riskPointDto.OrgId = model.OrgId;
                riskPointDto.OrgName = model.OrgName;
                string jsonStr = JsonConvert.SerializeObject(riskPointDto);
                var bitmap = _qrcodeService.GetQRCode(jsonStr, 10);
                var QRUrl = "/" + Guid.NewGuid().ToString() + ".bmp";
                var path = newPath + QRUrl;
                #endregion

                model.QRCodeUrl = path;
                var delePath = _riskmanageService.GetQRCodeUrl(model.Id);   //获取修改前的编号  记录并删除二维码
                bool result = _riskmanageService.Update(model);
                if (result)
                {     
                    if (System.IO.File.Exists(delePath))
                    {
                        System.IO.File.Delete(delePath);
                    }
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
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
                bool result = _riskmanageService.Delete(keyValue);
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
                bool result = _riskmanageService.DeleteBatch(keyValues);

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