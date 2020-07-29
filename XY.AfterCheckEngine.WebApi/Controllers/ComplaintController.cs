using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XY.Utilities;
using XY.AfterCheckEngine.Entities;
using XY.AfterCheckEngine.Service;
using XY.AfterCheckEngine.IService;
using XY.ZnshBusiness.Entities;
using XY.Universal.Models;
using AutoMapper;
using System.Threading;
using XY.DataCache.Redis;
using XY.Universal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Transactions;
using XY.AfterCheckEngine.Entities.Dto;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly IAfterCheckService _afterCheckService;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        private readonly IComplaintMZLService _complaintMZLService;
        
        public ComplaintController(IAfterCheckService afterCheckService, IComplaintMZLService complaintMZLService, IMapper mapper, IRedisDbContext redisDbContext)
        {
            _afterCheckService = afterCheckService;
            _complaintMZLService=complaintMZLService;
            _mapper = mapper;
            _redisDbContext = redisDbContext;
        }

        /// <summary>
        /// 获取住院详细信息
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYBHosInfo(string registerCode,string personalCode)
        {           
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetYBHosInfoEntity(registerCode, personalCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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
        /// 获取费用信息
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="personalCode"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFeeDetail(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetYBHosPreInfosByHosRegisterCode(registerCode);
                List<DataListPreList> resultdata = new List<DataListPreList>();
                DataListPreList dp_ypf = new DataListPreList();
                DataListPreList dp_jcf = new DataListPreList();
                DataListPreList dp_zlf = new DataListPreList();
                DataListPreList dp_clf = new DataListPreList();
                DataListPreList dp_qtf = new DataListPreList();

                dp_ypf.type = "ypf";
                dp_ypf.yBHosPreInfoEntities = data.Where(it => it.CollectFeesCategoryCode == SystemManageConst.XYF_CODE || it.CollectFeesCategoryCode == SystemManageConst.ZCY_CODE
                 || it.CollectFeesCategoryCode == SystemManageConst.ZCHENGY_CODE || it.CollectFeesCategoryCode == SystemManageConst.MYF_CODE).ToList();

                dp_jcf.type = "jcf";
                dp_jcf.yBHosPreInfoEntities = data.Where(it => it.CollectFeesCategoryCode == "5").ToList();//检查费

                dp_zlf.type = "zlf";
                dp_zlf.yBHosPreInfoEntities = data.Where(it => it.CollectFeesCategoryCode == "9").ToList();//治疗费

                dp_clf.type = "clf";
                dp_clf.yBHosPreInfoEntities = data.Where(it => it.CollectFeesCategoryCode == "19").ToList();//材料费

                dp_qtf.type = "qtf";
                dp_qtf.yBHosPreInfoEntities = data.Where(it => it.CollectFeesCategoryCode != SystemManageConst.XYF_CODE && it.CollectFeesCategoryCode != SystemManageConst.ZCY_CODE
           && it.CollectFeesCategoryCode != SystemManageConst.ZCHENGY_CODE && it.CollectFeesCategoryCode != SystemManageConst.MYF_CODE
           && it.CollectFeesCategoryCode != "5" && it.CollectFeesCategoryCode != "9" && it.CollectFeesCategoryCode != "19").ToList();//其他费

                resultdata.Add(dp_ypf);
                resultdata.Add(dp_jcf);
                resultdata.Add(dp_zlf);
                resultdata.Add(dp_clf);
                resultdata.Add(dp_qtf);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = resultdata;
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

        [HttpGet]
        public IActionResult GetCheckComplainInfo(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintMZLService.Get_Complain_MZLEntity(registerCode);      
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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
        [HttpGet]
        public IActionResult GetCheckComplainInfoALL(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintMZLService.Get_Complain_MZLEntityALL(registerCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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
    }
}
