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
using Ocelot.JwtAuthorize;
using Newtonsoft.Json;
using XY.AfterCheckEngine.Entities;
using XY.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.IO;
using XY.DataNS;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class CheckComplaintController : ControllerBase
    {
        private readonly IAfterCheckService _afterCheckService;
        private readonly ISupervisionInfoService _supervisionInfoService;
        private readonly IHosDayErrorService _hosdayerrorservice;
        private readonly IMapper _mapper;
        private readonly IRedisDbContext _redisDbContext;
        public CheckComplaintController(IAfterCheckService afterCheckService, ISupervisionInfoService supervisionInfoService, IHosDayErrorService hosdayerrorservice, IMapper mapper, IRedisDbContext redisDbContext)
        {
            _afterCheckService = afterCheckService;
            _mapper = mapper;
            _redisDbContext = redisDbContext;
            _supervisionInfoService = supervisionInfoService;
            _hosdayerrorservice = hosdayerrorservice;
        }

        /// <summary>
        /// 左侧树状菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGTree(string registerCode, int treeType)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetWGTree(registerCode,treeType);
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
        /// 查询结果页面获取左侧树
        /// </summary>
        /// <param name="registerCode"></param>
        /// <param name="treeType"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGTreeBySearch(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetWGTreeBySearch(registerCode);
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
        /// 违规金额 及描述
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGMoney(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetWGMoney(registerCode);
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
        /// 获取初审描述中的违规规则名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetComplainRulesName(string registerCode,string type)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetComplainRulesName(registerCode,type);
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
        public IActionResult GetCheckResultInfos(string flag,string rulescode, string querystr, int page, int limit)
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
            if(User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
            {
                isadmin = true;
            }
            try
            {
                var data = _afterCheckService.GetCheckResultInfos(flag, rulescode,result, isadmin,curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取人工初审列表数据成功";
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
        public IActionResult GetListByFK(string rulescode, string querystr,string states, int page, int limit)
        {
            QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            bool isadmin = false;
            string curryydm = User.GetCurrentUserOrganizeId();
            if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
            {
                isadmin = true;
            }
            try
            {
                    var data = _afterCheckService.GetListByFK(states, rulescode, result,isadmin,curryydm, page, limit, ref totalcount);
                    if (data != null)
                    {
                        resultCountModel.code = 0;
                        resultCountModel.msg = "获取数据成功";
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
        public IActionResult GetListByConclusion(string rulescode, string querystr,string states, int page, int limit)
        {
            QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
            if (!string.IsNullOrEmpty(querystr))
            {
                result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
            }
            var resultCountModel = new RespResultCountViewModel();
            int totalcount = 0;
            bool isadmin = false;
            string curryydm = User.GetCurrentUserOrganizeId();
            if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
            {
                isadmin = true;
            }
            try
            {
                var data = _afterCheckService.GetListByConclusion(rulescode, result, isadmin, curryydm,states, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疑点结论数据成功";
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
        /// <summary>
        /// 获取违规处方列表
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="rulecode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGCFDeatilList(string registercode, string rulecode, string flag, int page, int limit)
        {
            int total = 0;
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                
                if (flag == "1")//门诊
                {
                    var data= _supervisionInfoService.GetWGCFDeatilListCli(registercode, rulecode, page, limit, ref total);
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
                }
                else
                {
                    var data = _supervisionInfoService.GetWGCFDeatilList(registercode, rulecode, page, limit, ref total);
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
        /// 获取处方明细
        /// </summary>
        /// <param name="registercode"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCFDeatilListByCode(string registercode, string flag, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int count = 0;
                if (flag == "1")
                {
                    var data = _hosdayerrorservice.GetCFDeatilListCli(registercode, page, limit, ref count);
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
                }
                else
                {
                    var data = _hosdayerrorservice.GetCFDeatilList(registercode, page, limit, ref count);
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
        //获取住院或门诊的费用详细信息
        [HttpGet]
        public IActionResult GetFeeDetail(string flag, string registerCode, string personalCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {                
                if (flag == "1")
                {
                    var data =_afterCheckService.GetYBClinicInfoEntity(registerCode, personalCode);
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
                }
                else
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
        public IActionResult GetCheckComplaintEntity(string CheckResultInfoCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _afterCheckService.GetCheckComplaintEntity(CheckResultInfoCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.count = 1;
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.count = 0;
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
        public IActionResult GetComplaintStatesListTJ(string rulescode, string states, string querystr, string flag, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                QueryCoditionByCheckResult result = new QueryCoditionByCheckResult();
                if (!string.IsNullOrEmpty(querystr))
                {
                    result = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
                }
                bool isadmin = false;
                string curryydm = User.GetCurrentUserOrganizeId();
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
                {
                    isadmin = true;
                }
                int count = 0;
                var data = _afterCheckService.GetComplaintStatesListTJ(rulescode, states, result,isadmin, curryydm, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.count = count;
                    resultCountModel.data = data;
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
        public IActionResult GetComplaintStatesList(string rulescode, string states, string querystr, string flag, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                QueryCoditionByCheckResult queryCoditionByCheckResult = new QueryCoditionByCheckResult();
                if (!string.IsNullOrEmpty(querystr))
                {
                    queryCoditionByCheckResult = JsonConvert.DeserializeObject<QueryCoditionByCheckResult>(querystr);
                }
                bool isadmin = false;
                string curryydm = User.GetCurrentUserOrganizeId();
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
                {
                    isadmin = true;
                }
                int count = 0;
                var data = _afterCheckService.GetComplaintStateList(queryCoditionByCheckResult, rulescode, states, isadmin, curryydm, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.count = count;
                    resultCountModel.data = data;
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
        /// 获取统计列表
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="flag"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetComplaintStaticsList(string querystr, string flag, int page, int limit)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                QueryConditionByCheckComplain queryConditionByCheckComplain = new QueryConditionByCheckComplain();
                if (!string.IsNullOrEmpty(querystr))
                {
                    queryConditionByCheckComplain = JsonConvert.DeserializeObject<QueryConditionByCheckComplain>(querystr);
                }
                bool isadmin = false;
                string curryydm = User.GetCurrentUserOrganizeId();
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或者市医保局 可查看所有
                {
                    isadmin = true;
                }
                int count = 0;
                var data = _afterCheckService.GetCheckComplainStatics(queryConditionByCheckComplain,isadmin, curryydm, page, limit, ref count);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "成功";
                    resultCountModel.data = data;
                    resultCountModel.count = count;
                }
                else
                {
                    resultCountModel.code = 0;
                    resultCountModel.count = count;
                    resultCountModel.data = data;
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

        [HttpPost]
        public IActionResult IsRepeatCommit(string registerCode)
        {
            if (_afterCheckService.GetFeedbackCount(registerCode) >= 3)
            {
                return Ok(new { code = 1, msg = "重复反馈次数不能大于2次!" });
            }
            else
            {
                return Ok(new { code = 0, msg = "可重复反馈" });
            }
        }
        [HttpPost]
        public IActionResult DeleteFiles(string CheckComplainId)
        {
           
            if (_afterCheckService.DeleteFiles(CheckComplainId))
            {
                if (CommonHelper.DeleteDir(XYDbContext.UPLOADPATH + "医院反馈上传/" + CheckComplainId))
                {
                    return Ok(new { code = 0, msg = "删除原有附件成功!" });
                }
                else
                {
                    return Ok(new { code = -1, msg = "删除附件失败 请联系管理员!" });
                }
            }
            else
            {
                return Ok(new { code = 1, msg = "原有附件已删除!" });
            }
        }


        [HttpPost]
        public IActionResult UpdateFK(IFormCollection Files, string registerCode, string describe,bool repeat)
        {
            List<CheckComplaintDetailEntity> checkComplaintDetails = new List<CheckComplaintDetailEntity>();
            try
            {              
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                string dd = Files["File"];
                var form = Files;//定义接收类型的参数
                Hashtable hash = new Hashtable();
                string fileCode = "";//申诉主表编码
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                if (repeat)
                {
                    string CheckComplainId = _afterCheckService.UpdateFeedbackCount(registerCode, describe, userInfo);
                    if(!string.IsNullOrEmpty(CheckComplainId))
                    {
                        fileCode = CheckComplainId;
                    }
                    else
                    {
                        return Ok(new { code = -5, msg = "反馈失败,请联系管理员" });
                    }
                    
                }
                else
                {
                    string CheckComplainId = _afterCheckService.UpdateFK(registerCode, describe, userInfo);
                    if (!string.IsNullOrEmpty(CheckComplainId))
                    {
                        fileCode = CheckComplainId;
                    }
                }
               
                IFormFileCollection cols = Request.Form.Files;
                if (cols == null || cols.Count == 0)
                {
                    return Ok(new { code = -1, msg = "没有上传文件", data = hash });
                }
                if (string.IsNullOrEmpty(fileCode))
                {
                    return Ok(new { code = -4, msg = "请选择档案目录" });
                }
                int i = 0;
                foreach (IFormFile file in cols)
                {
                    i++;
                    //定义文件数组后缀格式
                    string[] LimitFileType = { ".JPG", ".JPEG", ".PNG", ".GIF", ".PNG", ".BMP", ".DOC", ".DOCX", ".XLS", ".XLSX", ".TXT", ".PDF" };
                    string[] LimitFileType1 = { ".JPG", ".JPEG", ".PNG", ".GIF", ".PNG", ".BMP" };
                    string[] LimitFileType2 = { ".DOC", ".DOCX", ".XLS", ".XLSX", ".TXT", ".PDF" };
                    //获取文件后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (LimitFileType.Contains(currentPictureExtension))
                    {
                        string filetype = string.Empty;    //1图片、2文本
                        if (LimitFileType1.Contains(currentPictureExtension))
                            filetype = "1";
                        if (LimitFileType2.Contains(currentPictureExtension))
                            filetype = "2";
                        //  var uppath = "E:/znsh/Platform_ZNSH/uploads/" + fileCode + "/" ; 
                        var uppath = XYDbContext.UPLOADPATH + "医院反馈上传/" + fileCode + "/";
                        var lookpath = "/" + fileCode + "/";
                        var new_path = Path.Combine(uppath, file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), new_path);
                        if (!Directory.Exists(uppath))
                        {
                            Directory.CreateDirectory(uppath);
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            byte[] fileByte = new byte[file.Length];//用文件的长度来初始化一个字节数组存储临时的文件
                            Stream fileStream = file.OpenReadStream();//建立文件流对象
                            string fileNameNew = Path.GetFileNameWithoutExtension(file.FileName);
                            //int a = int.Parse(fileNameNew.Substring(fileNameNew.Length - 4, 4));
                            CheckComplaintDetailEntity checkComplaintDetailEntity = new CheckComplaintDetailEntity()
                            {
                                ComplaintDetailCode = ConstDefine.CreateGuid(),
                                CheckComplainId = fileCode,
                                ImageName = file.FileName,
                                ImageSize = (int)(file.Length) / 1024,
                                ImageUrl = "/医院反馈上传" + lookpath + file.FileName,//上线时需更改
                                CreateUserId = User.GetCurrentUserId(),
                                CreateUserName = User.GetCurrentUserName(),
                                CreateTime = DateTime.Now,
                                Datatype = "1",       //医院反馈上传标识
                                FilesType = filetype
                            };
                            checkComplaintDetails.Add(checkComplaintDetailEntity);
                            //文件保存到数据库里面去
                            bool flage = _afterCheckService.Add(checkComplaintDetailEntity);
                            if (flage)
                            {
                                //再把文件保存的文件夹中
                                file.CopyTo(stream);
                                hash.Add("file", "/" + new_path);
                            }

                        }
                    }
                    else
                    {
                        return Ok(new { status = -2, message = "请上传指定格式的图片", data = hash });
                    }
                }
                using (var redisdb = _redisDbContext.GetRedisIntance())
                {
                    redisdb.SAdd(fileCode, checkComplaintDetails);
                    redisdb.Expire(fileCode, 86400);//设置缓存时间
                }
                return Ok(new { status = 0, message = "上传成功", data = hash });
            }
            catch (Exception ex)
            {

                return Ok(new { status = -3, message = "上传失败", data = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Create(string registerCode,string condition)
        {
            var resultModel = new RespResultCountViewModel();
            var conditionstr = new ConditionStringCS();
            if (!string.IsNullOrEmpty(condition))
            {
                conditionstr = JsonConvert.DeserializeObject<ConditionStringCS>(condition);
            }
            try
            {
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                bool result = _afterCheckService.Insert(registerCode, conditionstr, userInfo);
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
                resultModel.data = null;
                return Ok(resultModel);
            }
        }

        #region 文件上传
        [HttpPost]
        public ActionResult Upload(IFormCollection Files, string registerCode, string describe)
        {
            List<CheckComplaintDetailEntity> checkComplaintDetails = new List<CheckComplaintDetailEntity>();
            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                string dd = Files["File"];
                var form = Files;//定义接收类型的参数
                Hashtable hash = new Hashtable();
                string fileCode = fileCode = registerCode;
                IFormFileCollection cols = Request.Form.Files;
                if (cols == null || cols.Count == 0)
                {
                    return Ok(new { code = -1, msg = "没有上传文件", data = hash });
                }
                if (string.IsNullOrEmpty(fileCode) )
                {
                    return Ok(new { code = -4, msg = "请选择档案目录" });
                }
                int i = 0;
                foreach (IFormFile file in cols)
                {
                    i++;
                    //定义文件数组后缀格式
                    string[] LimitFileType = { ".JPG", ".JPEG", ".PNG", ".GIF", ".PNG", ".BMP",".DOC",".DOCX",".XLS",".XLSX",".TXT",".PDF" };
                    string[] LimitFileType1 = { ".JPG", ".JPEG", ".PNG", ".GIF", ".PNG", ".BMP"};
                    string[] LimitFileType2 = { ".DOC", ".DOCX", ".XLS", ".XLSX", ".TXT", ".PDF" };
                    //获取文件后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (LimitFileType.Contains(currentPictureExtension))
                    {
                        string filetype = string.Empty;    //1图片、2文本
                        if (LimitFileType1.Contains(currentPictureExtension))                        
                            filetype = "1";                       
                        if (LimitFileType2.Contains(currentPictureExtension))                       
                            filetype = "2";                        
                      //  var uppath = "E:/znsh/Platform_ZNSH/uploads/" + fileCode + "/" ; 
                        var uppath = XYDbContext.UPLOADPATH +"疑点结论上传/" + fileCode + "/";
                        var lookpath = "/" + fileCode + "/";
                        var new_path = Path.Combine(uppath, file.FileName);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), new_path);
                        if (!Directory.Exists(uppath))
                        {
                            Directory.CreateDirectory(uppath);
                        }
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            byte[] fileByte = new byte[file.Length];//用文件的长度来初始化一个字节数组存储临时的文件
                            Stream fileStream = file.OpenReadStream();//建立文件流对象
                            string fileNameNew = Path.GetFileNameWithoutExtension(file.FileName);
                            //int a = int.Parse(fileNameNew.Substring(fileNameNew.Length - 4, 4));
                            string CheckComplainId = _afterCheckService.ReturnCheckComplainId(registerCode);
                            if (string.IsNullOrEmpty(CheckComplainId))
                            {
                                return Ok(new { status = -6, message = "上传出错！"});
                            }
                            CheckComplaintDetailEntity checkComplaintDetailEntity = new CheckComplaintDetailEntity()
                            {
                                ComplaintDetailCode= ConstDefine.CreateGuid(),
                                CheckComplainId= CheckComplainId,
                                ImageName = file.FileName,
                                ImageSize=(int)(file.Length) / 1024,
                                ImageUrl= "/疑点结论上传" + lookpath + file.FileName,//上线时需更改
                                CreateUserId= User.GetCurrentUserId(),
                                CreateUserName = User.GetCurrentUserName(),
                                CreateTime= DateTime.Now,
                                Datatype = "2",     //疑点结论上传标识
                                FilesType = filetype
                            };
                            checkComplaintDetails.Add(checkComplaintDetailEntity);
                            UserInfo userInfo = new UserInfo
                            {
                                UserId = User.GetCurrentUserId(),
                                UserName = User.GetCurrentUserName(),
                                InstitutionCode = User.GetCurrentUserOrganizeId(),
                                InstitutionName = User.GetCurrentUserOrganizeName()
                            };
                            //文件保存到数据库里面去
                            bool flage =_afterCheckService.AddDescribe(registerCode,checkComplaintDetailEntity, describe, userInfo);
                            if (flage)
                            {
                                //再把文件保存的文件夹中
                                file.CopyTo(stream);
                                hash.Add("file", "/" + new_path);
                            }

                        }
                    }
                    else
                    {
                        return Ok(new { status = -2, message = "请上传指定格式的图片", data = hash });
                    }
                }
                using (var redisdb = _redisDbContext.GetRedisIntance())
                {
                    redisdb.SAdd(fileCode, checkComplaintDetails);
                    redisdb.Expire(fileCode, 86400);//设置缓存时间
                }
                return Ok(new { status = 0, message = "上传成功", data = hash });
            }
            catch (Exception ex)
            {

                return Ok(new { status = -3, message = "上传失败", data = ex.Message });
            }

        }
        #endregion
    }
}
