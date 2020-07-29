using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ocelot.JwtAuthorize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XY.AfterCheckEngine.IService;
using XY.Universal.Models;
using XY.AfterCheckEngine.Entities.Dto;
using XY.AfterCheckEngine.Entities;
using System.Collections;
using System.IO;
using XY.DataNS;
using XY.Utilities;

namespace XY.AfterCheckEngine.WebApi.Controllers
{
    [Authorize("permission")]
    [Produces("application/json")]
    [Route("afterengine/[controller]/[action]")]
    [ApiController]
    public class ComplaintMZLController : ControllerBase
    {
        private readonly IComplaintMZLService _complaintMZService;
        private readonly IAfterCheckService _afterCheckService;
        public ComplaintMZLController(IComplaintMZLService complaintMZLService, IAfterCheckService afterCheckService)
        {
            _afterCheckService = afterCheckService;
            _complaintMZService = complaintMZLService;
        }
        /// <summary>
        /// 获取初审列表
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="states">状态   1疑似   2刚性违规</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFisrtInfos(string querystr, string states, int page, int limit)
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
                var data = _complaintMZService.GetFisrtInfos(states, result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data.checkResultInfoEntities;
                    resultCountModel.complaindata = data.Record;
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
        /// 获取初审列表2
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="states">状态   1疑似   2刚性违规</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFisrtInfos2(string querystr, string states, int page, int limit)
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
                var data = _complaintMZService.GetFisrtInfos2(states, result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data.checkResultInfoEntities2;
                    resultCountModel.complaindata = data.Record;
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
        /// 获取初审疑点信息
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFisrtYDDescribe(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintMZService.GetFisrtYDDescribe(registerCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取初审疑点信息数据成功";
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
        /// <summary>
        /// 获取疑点信息通用
        /// </summary>
        /// <param name="registerCode">登记编码</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetYDInfoList(string registerCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintMZService.GetYDInfoList(registerCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取疑点信息数据成功";
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

        /// <summary>
        /// 获取初审违规确认列表
        /// </summary>
        /// <param name="checkResultInfoCode">审核结果主键</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFisrtWGConfirmList(string checkResultInfoCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int totalcount = 0;
                var data = _complaintMZService.GetFisrtWGConfirmList(checkResultInfoCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取初审违规确认列表成功";
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
        /// 获取通用违规确认列表
        /// </summary>
        /// <param name="complaintCode">从表主键</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGQRInfoList(string complaintCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int totalcount = 0;
                var data = _complaintMZService.GetWGQRInfoList(complaintCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取违规确认列表成功";
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
        /// 获取专家违规确认列表
        /// </summary>
        /// <param name="complaintCode">从表主键</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWGQRInfoListByZJ(string complaintCode)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int totalcount = 0;
                var data = _complaintMZService.GetWGQRInfoListByZJ(complaintCode);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取专家违规确认列表成功";
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
        /// 初审提交
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <param name="registerCode">登记编码</param>
        /// <param name="jsonmodel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult FirstCommint(string jsonArray,string registerCode,string jsonmodel)
        {
            JsonArray jsonObject =  JsonConvert.DeserializeObject<JsonArray>(jsonArray);
            CheckResultInfoEntity model = JsonConvert.DeserializeObject<CheckResultInfoEntity>(jsonmodel);
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int totalcount = 0;
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                bool flag = _complaintMZService.FirstCommint(jsonObject,registerCode,model,userInfo);
                if (flag)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "提交成功";
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
        /// 测试上传
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <param name="registerCode">登记编码</param>
        /// <param name="jsonmodel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CommintFK111(IFormCollection Files, string jsonArray, string registerCode, string fkcount)
        {
          //  JsonArray jsonObject = JsonConvert.DeserializeObject<JsonArray>(jsonArray);
            var resultCountModel = new RespResultCountViewModel();
            //string fileCode = "";//申诉主表编码
            try
            {
                //UserInfo userInfo = new UserInfo
                //{
                //    UserId = User.GetCurrentUserId(),
                //    UserName = User.GetCurrentUserName(),
                //    InstitutionCode = User.GetCurrentUserOrganizeId(),
                //    InstitutionName = User.GetCurrentUserOrganizeName()
                //};
                ////改ID为申诉主表的主键CheckComplainId
                //if (!string.IsNullOrEmpty(fkcount) && fkcount == "1")
                //{
                //    fileCode = _complaintMZService.Commint("2", jsonObject, registerCode, userInfo);
                //}
                //else if (!string.IsNullOrEmpty(fkcount) && fkcount == "2")
                //{
                //    fileCode = _complaintMZService.Commint("22", jsonObject, registerCode, userInfo);
                //}

                IFormFileCollection cols = Request.Form.Files;
         
           
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
                        var uppath = XYDbContext.UPLOADPATH + "医院初次反馈上传/" + 222222 + "/";
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
                            //再把文件保存的文件夹中
                            file.CopyTo(stream);
                        }
                    }
                    else
                    {
                        return Ok(new { status = -2, message = "请上传指定格式的图片", data = "" });
                    }
                }
                return Ok(new { status = 0, message = "上传成功", data = "" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = -3, message = "上传失败", data = ex.Message });
            }
        }




        /// <summary>
        /// 反馈提交
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <param name="registerCode">登记编码</param>
        /// <param name="jsonmodel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CommintFK(IFormCollection Files, string jsonArray, string registerCode,string fkcount)
        {
            JsonArray jsonObject = JsonConvert.DeserializeObject<JsonArray>(jsonArray);
            var resultCountModel = new RespResultCountViewModel();
            Hashtable hash = new Hashtable();
            string fileCode = "";//申诉主表编码
            try
            {
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                //改ID为申诉主表的主键CheckComplainId
                if(!string.IsNullOrEmpty(fkcount) && fkcount == "1")
                {
                    fileCode = _complaintMZService.Commint("2", jsonObject, registerCode, userInfo);
                }
                else if (!string.IsNullOrEmpty(fkcount) && fkcount == "2")
                {
                    fileCode = _complaintMZService.Commint("22", jsonObject, registerCode, userInfo);
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
                       

                        if (!string.IsNullOrEmpty(fkcount) && fkcount == "2")
                        {
                            var uppath = XYDbContext.UPLOADPATH + "医院二次反馈上传/" + fileCode + "/";
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
                                Check_ComplaintDetail_MZLEntity checkComplaintDetailEntity = new Check_ComplaintDetail_MZLEntity()
                                {
                                    ComplaintDetailCode = ConstDefine.CreateGuid(),
                                    CheckComplainId = fileCode,
                                    ImageName = file.FileName,
                                    ImageSize = (int)(file.Length) / 1024,
                                    ImageUrl = "/医院二次反馈上传" + lookpath + file.FileName,//上线时需更改
                                    CreateUserId = User.GetCurrentUserId(),
                                    CreateUserName = User.GetCurrentUserName(),
                                    CreateTime = DateTime.Now,
                                    Datatype = "2",       //二次反馈
                                    FilesType = filetype
                                };
                                //文件保存到数据库里面去
                                bool flag = _afterCheckService.Add2(checkComplaintDetailEntity);
                                if (flag)
                                {
                                    //再把文件保存的文件夹中
                                    file.CopyTo(stream);
                                    hash.Add("file", "/" + new_path);
                                }
                            }

                            #region 第二次反馈时  删除第一次反馈上传文件 再重新创建  待用
                            //if (_afterCheckService.DeleteFiles(fileCode))
                            //{
                            //    if (CommonHelper.DeleteDir(XYDbContext.UPLOADPATH + "医院反馈上传/" + fileCode))
                            //    {
                            //        if (!Directory.Exists(uppath))
                            //        {
                            //            Directory.CreateDirectory(uppath);
                            //        }
                            //        using (var stream = new FileStream(path, FileMode.Create))
                            //        {
                            //            byte[] fileByte = new byte[file.Length];//用文件的长度来初始化一个字节数组存储临时的文件
                            //            Stream fileStream = file.OpenReadStream();//建立文件流对象
                            //            string fileNameNew = Path.GetFileNameWithoutExtension(file.FileName);
                            //            //int a = int.Parse(fileNameNew.Substring(fileNameNew.Length - 4, 4));
                            //            CheckComplaintDetailEntity checkComplaintDetailEntity = new CheckComplaintDetailEntity()
                            //            {
                            //                ComplaintDetailCode = ConstDefine.CreateGuid(),
                            //                CheckComplainId = fileCode,
                            //                ImageName = file.FileName,
                            //                ImageSize = (int)(file.Length) / 1024,
                            //                ImageUrl = "/医院反馈上传" + lookpath + file.FileName,//上线时需更改
                            //                CreateUserId = User.GetCurrentUserId(),
                            //                CreateUserName = User.GetCurrentUserName(),
                            //                CreateTime = DateTime.Now,
                            //                Datatype = "2",       //二次反馈
                            //                FilesType = filetype
                            //            };
                            //            //文件保存到数据库里面去
                            //            bool flag = _afterCheckService.Add(checkComplaintDetailEntity);
                            //            if (flag)
                            //            {
                            //                //再把文件保存的文件夹中
                            //                file.CopyTo(stream);
                            //                hash.Add("file", "/" + new_path);
                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        return Ok(new { code = -1, msg = "删除附件失败 请联系管理员!" });
                            //    }
                            //}
                            //else
                            //{
                            //    return Ok(new { code = 1, msg = "原有附件已删除!" });
                            //}
                            #endregion
                        }
                        else
                        {
                            var uppath = XYDbContext.UPLOADPATH + "医院初次反馈上传/" + fileCode + "/";
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
                                Check_ComplaintDetail_MZLEntity checkComplaintDetailEntity = new Check_ComplaintDetail_MZLEntity()
                                {
                                    ComplaintDetailCode = ConstDefine.CreateGuid(),
                                    CheckComplainId = fileCode,
                                    ImageName = file.FileName,
                                    ImageSize = (int)(file.Length) / 1024,
                                    ImageUrl = "/医院初次反馈上传" + lookpath + file.FileName,//上线时需更改
                                    CreateUserId = User.GetCurrentUserId(),
                                    CreateUserName = User.GetCurrentUserName(),
                                    CreateTime = DateTime.Now,
                                    Datatype = "1",       //医院初次反馈
                                    FilesType = filetype
                                };
                                //文件保存到数据库里面去
                                bool flag = _afterCheckService.Add2(checkComplaintDetailEntity);
                                if (flag)
                                {
                                    //再把文件保存的文件夹中
                                    file.CopyTo(stream);
                                    hash.Add("file", "/" + new_path);
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        return Ok(new { status = -2, message = "请上传指定格式的图片", data = hash });
                    }
                }
                return Ok(new { status = 0, message = "上传成功", data = hash });
            }
            catch (Exception ex)
            {
                return Ok(new { status = -3, message = "上传失败", data = ex.Message });
            }
        }

        /// <summary>
        /// 通用提交
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <param name="registerCode">登记编码</param>
        /// <param name="jsonmodel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Commint(string step,string jsonArray, string registerCode)
        {
            JsonArray jsonObject = JsonConvert.DeserializeObject<JsonArray>(jsonArray);
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                int totalcount = 0;
                UserInfo userInfo = new UserInfo
                {
                    UserId = User.GetCurrentUserId(),
                    UserName = User.GetCurrentUserName(),
                    InstitutionCode = User.GetCurrentUserOrganizeId(),
                    InstitutionName = User.GetCurrentUserOrganizeName()
                };
                string id = _complaintMZService.Commint(step,jsonObject, registerCode, userInfo);
                if (!string.IsNullOrEmpty(id))
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "提交成功";
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
        /// 获取列表
        /// </summary>
        /// <param name="querystr"></param>
        /// <param name="states">状态   1疑似   2刚性违规</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetInfosList(string step,string querystr, string states, int page, int limit)
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
                var data = _complaintMZService.GetInfosList(step,states, result, isadmin, curryydm, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取数据成功";
                    resultCountModel.data = data.check_Complain_MZLEntities;
                    resultCountModel.complaindata = data.Record;
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
        public IActionResult GetImageInfo(string checkComplainId, string type)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                var data = _complaintMZService.GetImageInfo(checkComplainId, type);
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
        public IActionResult GetComplaintStates(string querystr, string states,string ydlevel, int page, int limit)
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
                if (User.GetCurrentUserName() == "admin" || _afterCheckService.IsCityYBJ(curryydm))  //如果为管理员或市医保 可查看所有
                {
                    isadmin = true;
                }
                int totalcount = 0;
                var data = _complaintMZService.GetComplaintStates(result, states, isadmin, curryydm, ydlevel, page, limit, ref totalcount);
                if (data != null)
                {
                    resultCountModel.code = 0;
                    resultCountModel.msg = "获取信息数据成功";
                    resultCountModel.data = data;
                    resultCountModel.count = totalcount;
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "没有检索到数据";
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败:" + ex.ToString();
                return Ok(resultCountModel);
            }
            return Ok(resultCountModel);

        }

    }
}