using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XY.AfterCheckEngine.IService;
using XY.DataCache.Redis;
using XY.Universal.Models;
using XY.Universal.Models.ViewModels;
using Ocelot.JwtAuthorize;
using Newtonsoft.Json;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class SupervisionInfoController : ControllerBase
    {
        private readonly ISupervisionInfoService _supervisionInfoService;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        public SupervisionInfoController(ISupervisionInfoService supervisionInfoService, IMapper mapper, IRedisDbContext redisDbContext)
        {
            _supervisionInfoService = supervisionInfoService;
            _mapper = mapper;
            _redisDbContext = redisDbContext;
        }
        #region 获取数据
        [HttpGet]
        public IActionResult GetYBClinicInfoByCondition(QueryConditionByClinic queryConditionByClinic, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (queryConditionByClinic.Count.ToString() == "")
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBClinicInfoByCondition(queryConditionByClinic, page, limit, ref total);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取门诊信息列表数据成功";
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
        }

        [HttpGet]
        public IActionResult GetYBClinicInfo(string queryCondition, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (string.IsNullOrEmpty(queryCondition))
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBClinicInfo(queryCondition, page, limit, ref total);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取门诊信息列表数据成功";
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
        }

        [HttpGet]
        public IActionResult GetYBClinicInfoList(string clinicDate, string idNumber, string personCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (string.IsNullOrEmpty(clinicDate) || string.IsNullOrEmpty(idNumber) || string.IsNullOrEmpty(personCode))
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBClinicInfoList(clinicDate, idNumber, personCode, page, limit, ref total);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取门诊信息列表数据成功";
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
        }

        [HttpGet]
        public IActionResult GetYBClinicInfoEntity(string clinicRegisterCode, string cityAreaCode, string year)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (string.IsNullOrEmpty(clinicRegisterCode) || string.IsNullOrEmpty(cityAreaCode) || string.IsNullOrEmpty(year))
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBClinicInfoEntity(clinicRegisterCode, cityAreaCode, year);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取门诊信息数据成功";
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
        }
        [HttpGet]
        public IActionResult GetYBHosInfoEntityByCondition(string hosRegisterCode, string personalCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (string.IsNullOrEmpty(hosRegisterCode) || string.IsNullOrEmpty(personalCode) )
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBHosInfoEntityByCondition(hosRegisterCode, personalCode);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取住院信息数据成功";
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
        }
        [HttpGet]
        public IActionResult GetYBClinicPreInfoList(string clinicRegisterCode, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            if (string.IsNullOrEmpty(clinicRegisterCode) )
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "没有检索到数据";
                return Ok(resultCountModel);
            }
            else
            {
                try
                {
                    var data = _supervisionInfoService.GetYBClinicPreInfoList(clinicRegisterCode,page,limit,ref total);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取门诊信息数据成功";
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
        }
        [HttpGet]
        public IActionResult GetYBHosInfoList(string condition, string keyValue, string idNumber, string institutionCode, string inHosDate, string outHosDate, string rulescode,string datatype, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int total = 0;
            try
            {
                var data = _supervisionInfoService.GetYBHosInfoList(condition, keyValue, idNumber, institutionCode, inHosDate, outHosDate, rulescode, datatype, page, limit, ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取住院信息数据成功";
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
        /// <summary>
        /// 申诉复审
        /// </summary>
        /// <param name="rulescode"></param>
        /// <param name="registercode"></param>
        /// <param name="describe"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RepeatCheckComplaint(string[] rulescode, string registercode, string describe,decimal money)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                bool flag = _supervisionInfoService.RepeatCheckComplaint(rulescode, registercode, describe, userInfo, money);
                if (flag)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "复审成功";
                    resultCountModel.data = null;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "插入医保库数据失败";
                    resultCountModel.data = null;

                }
                return Ok(resultCountModel);
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                resultCountModel.data = null;
                return Ok(resultCountModel);
            }

        }
        [HttpGet]
        public IActionResult GetCLInfo(string checkComplainId, string type)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetCLInfo(checkComplainId, type);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取信息数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = data.Count();
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
        public IActionResult GetComplaintInfoList(string rulescode, string querystr, int page, int limit)
        {
            QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
            }
            int totalcount = 0;
            var resultCountModel = new RespResultCountViewModel();
            bool isadmin = false;
            string curryydm = User.GetCurrentUserOrganizeId();
            if (User.GetCurrentUserName() == "admin" || _supervisionInfoService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
            {
                isadmin = true;
            }
            try
            {
                var data = _supervisionInfoService.GetComplaintInfoList(rulescode, result,isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取申诉信息数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = totalcount;
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
        public IActionResult GetCheckResultInfoList(string key)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetCheckResultInfoList(key);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规信息数据成功";
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
        public IActionResult GetCheckResultPreInfoList(string key)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetCheckResultPreInfoList(key);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规处方数据成功";
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
        public IActionResult GetWGDescribe(string key)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetWGDescribe(key);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取审核结果处方信息数据成功";
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
        public IActionResult GetCheckComplaintInfoList(string registercode, string personalcode, string rulescode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetCheckComplaintInfoList(registercode, personalcode, rulescode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取住院信息数据成功";
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
        public IActionResult GetWGCFDeatilListByKey(string key,int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetWGCFDeatilListByKey(key, page, limit, ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规处方信息数据成功";
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
        public IActionResult GetWGCFDeatilList(string hosregistercode, string rulecode, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetWGCFDeatilList(hosregistercode,rulecode, page, limit,ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规处方信息数据成功";
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
        public IActionResult GetWGCFDeatilListCli(string hosregistercode, string rulecode, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetWGCFDeatilListCli(hosregistercode, rulecode, page, limit, ref total);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规处方信息数据成功";
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
        public IActionResult GetMonthCountList(string year, string flag, string status)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _supervisionInfoService.GetMonthCountList(year,flag,status);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取信息数据成功";
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
    }
}
