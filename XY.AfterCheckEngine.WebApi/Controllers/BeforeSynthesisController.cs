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
using Ocelot.JwtAuthorize;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class BeforeSynthesisController : ControllerBase
    {
        private readonly IBeforeSynthesisService _beforesynthesisService;
        private readonly IMapper _mapper;
        private readonly IAfterCheckService _afterCheckService;
        public BeforeSynthesisController(IBeforeSynthesisService beforesysthesisService, IMapper mapper, IAfterCheckService afterCheckService)
        {
            _beforesynthesisService = beforesysthesisService;
            _afterCheckService = afterCheckService;
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
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                //bool isadmin = false;
                //if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
                //{
                //    isadmin = true;
                //}
                var data = _beforesynthesisService.GetTotal(yljgbh,curryydm);
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
        /// 获取费用类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFYLX()
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                var data = _beforesynthesisService.GetFYLX(yljgbh);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取费用类型数据成功";
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
        /// 获取费用类型钻取1   按科室分
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFYLX_ZQ1(int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetFYLX_ZQ1(yljgbh,page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室各费用信息成功";
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
        /// 获取费用类型钻取2   根据科室获取患者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFYLX_ZQ2(string RYKSBM, int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetFYLX_ZQ2(RYKSBM,yljgbh, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室患者信息成功";
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
        /// 获取当日就诊人次
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetJZRC(int second, int count)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                var data = _beforesynthesisService.GetJZRC(yljgbh, second, count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取当日就诊人次成功";
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
        /// 获取费科室占比信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetKSZB(int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetKSZB(yljgbh, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室患者信息成功";
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
        /// 获取规则违规人次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetGZWG(int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetGZWG(yljgbh, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室患者信息成功";
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
        /// 获取规则违规人次数钻取1  根据违规规则获取患者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetGZWG_ZQ1(string rulescode,int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetGZWG_ZQ1(yljgbh, rulescode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取患者信息成功";
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
        /// 获取患者违规描述信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGDescribe(string registercode,int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetWGDescribe(yljgbh, registercode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规描述信息成功";
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
        /// 获取患者处方信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCF(string registercode, int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetCF(yljgbh, registercode, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取患者处方信息成功";
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
        /// 获取违规科室占比
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGKSZB(int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetWGKSZB(yljgbh, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取获取违规科室占比信息成功";
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
        /// 获取违规科室占比钻取1 根据科室获取人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGKSZB_ZQ1(string ksbm, int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetWGKSZB_ZQ1(yljgbh, ksbm, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取获取违规科室占比信息成功";
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
        /// 获取违规医生人次数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGYS(int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetWGYS(yljgbh, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规医生人次数信息成功";
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
        /// 获取违规医生人次数钻取1  根据医生获取违规患者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGYS_ZQ1(string ksbm,string doctorname,int page, int limit)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                int count = 0;
                var data = _beforesynthesisService.GetWGYS_ZQ1(yljgbh,ksbm,doctorname,page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规医生人次数信息成功";
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
        #region 门诊 住院监控
        /// <summary>
        /// 获取科室列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetKSList(string datatype)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                var data = _beforesynthesisService.GetKSList(yljgbh, datatype);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取科室列表成功";
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
        /// 点击科室获取下面搜索患者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonList1(string name,string ksbm,string datatype)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                var data = _beforesynthesisService.GetPersonList1(name,ksbm, yljgbh, datatype);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取患者信息成功";
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
        /// 右侧患者信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPersonList2(string ksbm,string registercode,string datatype)
        {
            //ZnshTYDetailEntity
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string curryydm = User.GetCurrentUserOrganizeId();
                string yljgbh = _beforesynthesisService.GetYLJGBH(curryydm);
                var data = _beforesynthesisService.GetPersonList2(registercode, ksbm, yljgbh, datatype);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取患者信息成功";
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
        #endregion

    }
}