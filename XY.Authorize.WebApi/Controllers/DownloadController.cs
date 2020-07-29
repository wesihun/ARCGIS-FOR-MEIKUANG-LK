using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Ocelot.JwtAuthorize;
using XY.Authorize.IService;
using XY.DataNS;
using XY.Universal.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace XY.Authorize.WebApi.Controllers
{
    [Route("auth/[controller]/[action]")]
    [ApiController]
    public class DownloadController : Controller
    {
        readonly ITokenBuilder _tokenBuilder;
        private readonly IAuthorizeService _authorizeService;
        private readonly AudienceViewModel _audienceModel;
        public DownloadController(ITokenBuilder tokenBuilder, IAuthorizeService authorizeService, IOptionsMonitor<AudienceViewModel> audienceModel)
        {
            _tokenBuilder = tokenBuilder;
            _authorizeService = authorizeService;
            _audienceModel = audienceModel.CurrentValue;
        }
        protected static int Counter = 1;//1:空闲 0:非空闲
        protected static ManualResetEventSlim Mres = new ManualResetEventSlim(false);
        [HttpGet]
        public IActionResult Test()
        {
            string rc = "";
            try
            {
                //如果其他线程正在操作，则等待，5秒后超时
                if (Interlocked.CompareExchange(ref Counter, 0, 1) == 0)
                    Mres.Wait(5000);

                int count = Convert.ToInt32(RedisHelper.Get("GoodsNumberKey"));
                if (count > 0)
                {
                    RedisHelper.Set("GoodsNumberKey", "-1");
                    //rc.SetMessage("重置成功！");
                    rc = "重置成功！";
                }
                else
                    //rc.SetMessage("已被重置，本次重置无效");
                    rc = "已被重置，本次重置无效！";
            }
            catch (Exception ex)
            {
                //_log.Error(ex);
            }
            finally
            {
                //转为空闲状态
                Interlocked.Exchange(ref Counter, 1);
                //设置信号量，让上面的 Mres.Wait(5000);取消等待，继续执行代码
                Mres.Set();
            }

            return Json(rc);


            //return Json(SynchronizationTest());
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <remarks>
        /// 说明:
        /// 用户下载时调用
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Download(string filename, string url)
        {
            var resultCountModel = new RespResultCountViewModel();
            try
            {
                string resultPath = XYDbContext.DownLoadPath + "/" + url;
                if (!string.IsNullOrEmpty(resultPath))
                {
                    var provider = new FileExtensionContentTypeProvider();
                    FileInfo fileInfo = new FileInfo(resultPath);
                    new FileExtensionContentTypeProvider().Mappings.TryGetValue(fileInfo.Extension, out var contenttype);
                    //return File(System.IO.File.ReadAllBytes(resultPath), contenttype ?? "application/octet-stream", _personalcenterService.Download(applyid, "2") + fileInfo.Extension);
                    return File(System.IO.File.ReadAllBytes(resultPath), contenttype ?? "application/octet-stream", filename);
                }
                else
                {
                    resultCountModel.code = -1;
                    resultCountModel.msg = "失败";
                    return Ok(resultCountModel);
                }
            }
            catch (Exception ex)
            {
                resultCountModel.code = -1;
                resultCountModel.msg = "操作失败！原因：" + ex.Message;
                return Ok(resultCountModel);
            }
        }
    }
}
