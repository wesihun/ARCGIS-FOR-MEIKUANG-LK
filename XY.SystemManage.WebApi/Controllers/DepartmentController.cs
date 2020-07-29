using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.JwtAuthorize;
using XY.SystemManage.Entities;
using XY.SystemManage.IService;
using XY.Universal.Models;
using XY.Utilities;

namespace XY.SystemManage.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        public DepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }
        /// <summary>
        /// 获取所有部门列表并分页
        /// </summary>
        /// <param name="orgid">机构编码</param>
        /// <param name="depname">部门名</param>
        /// <param name="page">页码</param>
        /// <param name="limit">每页显示的条数</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPageListByCondition(string orgid, string depname, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            try
            {
                var resultData = _departmentService.GetPageListByCondition(orgid, depname, page, limit, ref totalcount);
                if (resultData.Count != 0)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "查询成功";
                    resultCountModel.data = resultData;
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

        /// <summary>
        /// 部门列表 
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="keyword">关键字</param>
        /// <param name="parentId">父ID</param>
        [HttpGet]
        public IActionResult GetListByCondition(string condition, string keyword)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _departmentService.GetListByCondition(condition, keyword);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
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
        #region 提交数据

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">部门实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(DepartmentEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            try
            {
                #region 验证
                if (string.IsNullOrEmpty(model.Name))
                {
                    resultModel.code = -1;
                    resultModel.msg = "部门名称不允许为空！";
                    return Ok(resultModel);
                }
                if (string.IsNullOrEmpty(model.OrgId))
                {
                    resultModel.code = -1;
                    resultModel.msg = "机构代码不允许为空！";
                    return Ok(resultModel);
                }
                if (_departmentService.ExistFullName(model.Name, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同部门名称！";
                    return Ok(resultModel);
                }
                if (_departmentService.ExistEnCode(model.BH, model.Id))
                {
                    resultModel.code = -1;
                    resultModel.msg = "已存在相同部门编号！";
                    return Ok(resultModel);
                }
                #endregion
                model.Id = ConstDefine.CreateGuid();
                model.CreateTime = DateTime.Now;
                model.DeleteMark = 1;//删除标记，原来为0
                bool result = _departmentService.Insert(model);
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
        /// <param name="model">部门实体</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(DepartmentEntity model)
        {
            var resultModel = new RespResultCountViewModel();
            #region 验证
            if (string.IsNullOrEmpty(model.OrgName))
            {
                resultModel.code = -1;
                resultModel.msg = "部门名称不允许为空！";
                return Ok(resultModel);
            }
            if (string.IsNullOrEmpty(model.BH))
            {
                resultModel.code = -1;
                resultModel.msg = "部门编号不允许为空！";
                return Ok(resultModel);
            }
            if (_departmentService.ExistFullName(model.Name, model.Id))
            {
                resultModel.code = -1;
                resultModel.msg = "已存在相同部门名称！";
                return Ok(resultModel);
            }
            if (_departmentService.ExistEnCode(model.BH, model.Id))
            {
                resultModel.code = -1;
                resultModel.msg = "已存在相同部门编号！";
                return Ok(resultModel);
            }
            #endregion
            try
            {
                bool result = _departmentService.Update(model);
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
                    resultModel.msg = "删除部门失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _departmentService.Delete(keyValue);
                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "删除部门成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "删除部门失败";
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
                    resultModel.msg = "批量删除部门失败,缺少主键";
                    return Ok(resultModel);
                }
                bool result = _departmentService.DeleteBatch(keyValues);

                if (result)
                {
                    resultModel.code = 0;
                    resultModel.msg = "批量删除部门成功";
                }
                else
                {
                    resultModel.code = -1;
                    resultModel.msg = "批量删除部门失败";
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
