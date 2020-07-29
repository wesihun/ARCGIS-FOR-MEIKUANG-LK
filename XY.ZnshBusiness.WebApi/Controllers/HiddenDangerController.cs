using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using Ocelot.JwtAuthorize;
using XY.Authorize.IService;
using XY.DataNS;
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
    public class HiddenDangerController : Controller
    {
        private readonly IHiddenDangerService _hiddenService;
        private readonly IAuthorizeService _authorizeService;
        private readonly IMapper _mapper;
        public HiddenDangerController(IAuthorizeService authorizeService, IHiddenDangerService hiddenService, IMapper mapper)
        {
            _hiddenService = hiddenService;
            _authorizeService = authorizeService;
            _mapper = mapper;
        }
        #region 获取数据
        /// <summary>
        /// 根据条件获取隐患上报管理列表并分页
        /// </summary>
        /// <remarks>
        /// riskpointname/hiddenlevel 
        /// states(无) 0 表示管理员查看   1表示当前用户对自己的隐患上报查看(岗位员工 班组长)
        ///        2 表示责任单位的责任人对已上报的隐患进行查看(部门(车间)责任人)
        ///        3 表示安全管理部门责任人查看的隐患上报情况
        /// </remarks>
        /// <param name="qdstates">当前隐患状态</param>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetReportListByCondition(string qdstates,string orgid,string riskpointname,string hiddenlevel,int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                string states = "";
                if (User.GetCurrentUserIsAdmin() == "1")
                {
                    states = "0";
                }
                if (User.GetCurrentUserRoleName() == "岗位员工" )
                {
                    states = "1";
                }
                if (User.GetCurrentUserRoleName() == "部门(车间)负责人" || User.GetCurrentUserRoleName() == "公司经理级管理人员" || User.GetCurrentUserRoleName() == "班组长")
                {
                    states = "2";
                }
                if (User.GetCurrentUserDepName() == DataDictConst.SAFE_DEPARTMENT)
                {
                    states = "3";
                }
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _hiddenService.GetPageListByCondition(states, User.GetCurrentUserId(), qdstates, orgid, currOrgId, riskpointname, hiddenlevel, page, limit, ref count);
                if (data.Count == 0)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;

                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.count = count;
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
        /// 获取待整改通知列表
        /// </summary>
        /// <remarks>
        /// states 1 整改人查看   2责任人查看
        /// </remarks>
        /// <param name="states">状态</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNoticeList(string states, string orgid, string zlrname,string hiddenlevel, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _hiddenService.GetNoticeList(states,User.GetCurrentUserId(), orgid,currOrgId, zlrname, hiddenlevel, page, limit, ref count);
                if (data.Count == 0)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;

                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                resultCountModel.count = count;
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
        /// 获取待复查通知列表
        /// </summary>
        /// states 1 整改人查看   2责任人查看
        /// </remarks>
        /// <param name="states">状态</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetModifyList(string states,string orgid,string qdstates,string riskpointname,string zlrname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _hiddenService.GetModifyList(states,User.GetCurrentUserId(),orgid, currOrgId,qdstates, riskpointname,zlrname, page, limit, ref count);
                if (data.Count == 0)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;

                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                resultCountModel.count = count;
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
        /// 获取复查通知列表
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetRecheckList(string orgid,string states, string riskpointname, string fcrname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                string currOrgId = User.GetCurrentUserOrganizeId();
                var data = _hiddenService.GetRecheckList(User.GetCurrentUserId(), orgid,currOrgId,states, riskpointname,fcrname ,page, limit, ref count);
                if (data.Count == 0)
                    resultCountModel.data = new Array[0];
                else
                    resultCountModel.data = data;

                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                resultCountModel.count = count;
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
        /// 隐患上报新增
        /// </summary>
        /// <remarks>
        /// 说明:
        /// model里除了Id  ZRDW ZRDWName ZRR ZRRName TBUserId TBUserName CreateTime DeleteMark PushMark 其它都要添加
        /// 并与files上传文件一起提交
        /// </remarks>
        /// <param name="files">文件</param>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateReport(List<IFormFile> files,HiddenDangersReportEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            if (string.IsNullOrEmpty(model.CheckTableId))
            {
                resultModel.code = -1;
                resultModel.msg = "缺少CheckTableId!";
                return Ok(resultModel);
            }        
            try
            {
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                model.PushMark = 0;
                model.States = "1";
                model.TBUserId = User.GetCurrentUserId();
                model.TBUserName = User.GetCurrentUserRealName();
                model.OrgId = User.GetCurrentUserOrganizeId();
                model.OrgName = User.GetCurrentUserOrganizeName();
                if(User.GetCurrentUserIsAdmin() == "1")   //管理员的话  提交的是挂牌督办的上报
                {
                    model.IsSupervisor = "1";
                }
                else
                {
                    model.IsSupervisor = "0";
                }
                //if (_hiddenService.IsSecondReport(model.CheckTableId))
                //{
                //    resultModel.code = -1;
                //    resultModel.msg = "已上报 请勿重复提交";
                //    return Ok(resultModel);
                //}
                var filesPath = XYDbContext.HiddenDangerPath + "/" + User.GetCurrentUserOrganizeId() +"/Report";
                if (!Directory.Exists(filesPath))
                    Directory.CreateDirectory(filesPath);
                int i = 0;
                foreach (var formFile in files)
                {
                   
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(filesPath, formFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                            stream.Flush();
                        }
                        model.ImageUrl += "/HiddenDangers/" + User.GetCurrentUserOrganizeId() + "/Report/" + formFile.FileName + ",";
                    }
                }
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = model.ImageUrl.Substring(0, model.ImageUrl.Length - 1);
                }
                bool result = _hiddenService.CreateReport(model, User.GetCurrentUserOrganizeId(),User.GetCurrentUserId(),User.GetCurrentUserRealName());
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
        /// 下发通知新增
        /// </summary>
        /// <remarks>
        /// 说明:
        /// model里除了Id CreateTime DeleteMark 其它都要添加 
        /// 其中ZRRId ZRRName 对应 TBUserId TBUserName
        /// </remarks>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateNotice(HiddenDangersNoticeEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;
                model.States = "1";
                model.IsSupervisor = _hiddenService.GetIsSupervisor(model.ReportId);
                bool result = _hiddenService.CreateNotice(model);              
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
        /// 推送到安全管理部门  责任单位处理不了
        /// </summary>
        /// <remarks>
        /// 说明:
        /// </remarks>
        /// <param name="id">该条记录的主键</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PushSafeDepart(string id,string zrrid)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "缺少主键";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(zrrid))
                {
                    resultModel.code = -1;
                    resultModel.msg = "缺少责任人id";
                    return Ok(resultModel);
                }
                bool result = _hiddenService.PushSafeDepart(id,zrrid);
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
        /// 获取安全管理部门 人员
        /// </summary>
        /// <remarks>
        /// 说明:
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafeUserList()
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                var data = _hiddenService.GetSafeUserList(User.GetCurrentUserOrganizeId());
                if (data == null)
                    resultModel.data = new Array[0];
                else
                    resultModel.data = data;
                resultModel.code = 0;
                resultModel.msg = "获取成功";
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
        /// 隐患整改新增
        /// </summary>
        /// <remarks>
        /// 说明:
        /// model里除了Id CreateTime DeleteMark ModifyUserId 其它都要添加
        /// 并与files上传文件一起提交
        /// </remarks>
        /// <param name="files">文件</param>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateModify(List<IFormFile> files, HiddenDangersModifyEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(model.NoticeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "NoticeId为必填项!";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.ReportId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "ReportId为必填项!";
                    return Ok(resultModel);
                }
                if (_hiddenService.IsSecondCreat(model.NoticeId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "请勿重复整改!";
                    return Ok(resultModel);
                }
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.ModifyUserId = User.GetCurrentUserId();
                model.DeleteMark = 1;
                model.States = "2";
                model.IsSupervisor = _hiddenService.GetIsSupervisor(model.ReportId);
                var filesPath = XYDbContext.HiddenDangerPath + "/" + User.GetCurrentUserOrganizeId() + "/Modify";
                if (!Directory.Exists(filesPath))
                    Directory.CreateDirectory(filesPath);
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(filesPath, formFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                            stream.Flush();
                        }
                        model.ImageUrl += "/HiddenDangers/" + User.GetCurrentUserOrganizeId() + "/Modify/" + formFile.FileName + ",";
                    }
                }
                bool result = false;
                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    model.ImageUrl = model.ImageUrl.Substring(0, model.ImageUrl.Length - 1);
                }
                result = _hiddenService.CreateModify(model);      
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
        /// 隐患复查新增
        /// </summary>
        /// <remarks>
        /// 说明:
        /// States 需传入3已复查 通过   4已复查未通过
        /// model里除了Id  ReUserId ReUserName ReTime DeleteMark  ReImageUrl其它都要添加
        /// 并与files上传文件一起提交
        /// </remarks>
        /// <param name="files">文件</param>
        /// <param name="model">实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateRecheck(List<IFormFile> files, HiddenDangersRecheckEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                if (string.IsNullOrEmpty(model.States) || string.IsNullOrEmpty(model.ReportId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "存在必填项！";
                    return Ok(resultModel);
                }
                if (_hiddenService.IsSecondRecheck(model.ReportId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已复查 请勿重复提交";
                    return Ok(resultModel);
                }

                model.Id = ConstDefine.CreateGuid();
                model.ReTime = DateTime.Now;
                model.DeleteMark = 1;
                model.ReUserId = User.GetCurrentUserId();
                model.ReUserName = User.GetCurrentUserRealName();
                model.IsSupervisor = _hiddenService.GetIsSupervisor(model.ReportId);
                var filesPath = XYDbContext.HiddenDangerPath + "/" + User.GetCurrentUserOrganizeId() + "/Recheck";
                if (!Directory.Exists(filesPath))
                    Directory.CreateDirectory(filesPath);
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.Combine(filesPath, formFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                            stream.Flush();
                        }
                        model.ReImageUrl += "/HiddenDangers/" + User.GetCurrentUserOrganizeId() + "/Recheck/" + formFile.FileName + ",";
                    }
                }
                if (!string.IsNullOrEmpty(model.ReImageUrl))
                {
                    model.ReImageUrl = model.ReImageUrl.Substring(0, model.ReImageUrl.Length - 1);
                }
                bool result = _hiddenService.CreateRecheck(model);
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
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="model">实体</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Edit(HiddenDangersReportEntity model)
        //{
        //    var resultModel = new RespResultCountViewModel();
        //    try
        //    {
        //        #region 验证
        //        if (string.IsNullOrEmpty(model.OrgId))
        //        {
        //            resultModel.code = -1;
        //            resultModel.msg = "机构Id不能为空！";
        //            return Ok(resultModel);
        //        }
        //        if (string.IsNullOrEmpty(model.BH))
        //        {
        //            resultModel.code = -1;
        //            resultModel.msg = "风险点编号不能为空！";
        //            return Ok(resultModel);
        //        }
        //        if (_hiddenService.IsExist(model.OrgId, model.BH, model.Id))
        //        {
        //            resultModel.code = -1;
        //            resultModel.msg = "请勿重复添加风险点编号！";
        //            return Ok(resultModel);
        //        }
        //        #endregion

        //        string activeDir = XYDbContext.QRPath;
        //        string newPath = activeDir + "/" + model.OrgName;

        //        #region 生成二维码 先删除原来的二维码
        //        RiskPointDto riskPointDto = new RiskPointDto();
        //        riskPointDto.RiskBH = model.BH;
        //        riskPointDto.RiskName = model.Name;
        //        riskPointDto.OrgId = model.OrgId;
        //        riskPointDto.OrgName = model.OrgName;
        //        string jsonStr = JsonConvert.SerializeObject(riskPointDto);
        //        var bitmap = _qrcodeService.GetQRCode(jsonStr, 10);
        //        var QRUrl = "/" + model.BH + ".bmp";
        //        var path = newPath + QRUrl;
        //        #endregion
        //        model.QRCodeUrl = path;
        //        var bh = _hiddenService.GetRiskBH(model.Id);   //获取修改前的编号  记录并删除二维码
        //        bool result = _hiddenService.Update(model);
        //        if (result)
        //        {
        //            var delePath = newPath + "/" + bh + ".bmp";
        //            if (System.IO.File.Exists(delePath))
        //            {
        //                System.IO.File.Delete(delePath);
        //            }
        //            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Bmp);
        //            resultModel.code = 0;
        //            resultModel.msg = "修改成功";
        //            resultModel.data = null;
        //        }
        //        else
        //        {
        //            resultModel.code = -1;
        //            resultModel.msg = "修改失败";
        //            resultModel.data = null;
        //        }
        //        return Ok(resultModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        resultModel.code = -1;
        //        resultModel.msg = "操作失败:" + ex.Message;
        //        resultModel.data = null;
        //        return Ok(resultModel);
        //    }
        //}
        #endregion

        #region 删除数据
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <remarks>
        /// 说明:
        /// type = 1 隐患上报列表删除
        /// 2 下发通知列表删除
        /// 3 隐患整改列表删除
        /// 4 隐患复查列表删除 
        /// </remarks>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveBatch(List<string> keyValues,string type)
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
                bool result = _hiddenService.DeleteBatch(keyValues,type);
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


        #region 隐患历史记录
        /// <summary>
        /// 获取待整改通知列表
        /// </summary>
        /// <remarks>
        /// states 1 已下发通知的列表  2  已复查的列表 3 已整改的列表
        /// </remarks>
        /// <param name="states">状态</param>
        /// <param name="page">页码</param>
        /// <param name="limit">页尺寸</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetHistoryList(string states, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int count = 0;
            try
            {
                switch (states)
                {
                    case "1":
                        if (_hiddenService.GetHistoryList1(User.GetCurrentUserId(), page, limit, ref count).Count == 0)
                            resultCountModel.data = new Array[0];
                        else
                            resultCountModel.data = _hiddenService.GetHistoryList1(User.GetCurrentUserId(), page, limit, ref count);
                        break;
                    case "2":
                        if (_hiddenService.GetHistoryList2(User.GetCurrentUserId(), page, limit, ref count).Count == 0)
                            resultCountModel.data = new Array[0];
                        else
                            resultCountModel.data = _hiddenService.GetHistoryList2(User.GetCurrentUserId(), page, limit, ref count);
                        break;
                    case "3":
                        if (_hiddenService.GetHistoryList3(User.GetCurrentUserId(), page, limit, ref count).Count == 0)
                            resultCountModel.data = new Array[0];
                        else
                            resultCountModel.data = _hiddenService.GetHistoryList3(User.GetCurrentUserId(), page, limit, ref count);
                        break;
                }

                resultCountModel.code = 0;
                resultCountModel.msg = "成功";
                resultCountModel.count = count;
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
    }
}
