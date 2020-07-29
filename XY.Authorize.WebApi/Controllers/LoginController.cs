using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.JwtAuthorize;
using XY.Authorize.IService;
using XY.Authorize.ViewModel;
using XY.Universal.Models;
using XY.Utilities;
namespace YXFR.Authorize.WebApi.Controllers
{
    [Route("auth/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {

        readonly ILogger<LoginController> _logger;
        readonly ITokenBuilder _tokenBuilder;
        private readonly IAuthorizeService _authorizeService;
        private readonly AudienceViewModel _audienceModel;
        public LoginController(ITokenBuilder tokenBuilder, ILogger<LoginController> logger, IAuthorizeService authorizeService, IOptionsMonitor<AudienceViewModel> audienceModel)
        {
            _logger = logger;
            _tokenBuilder = tokenBuilder;
            _authorizeService = authorizeService;
            _audienceModel = audienceModel.CurrentValue;
        }
        /// <summary>
        /// 登录授权
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginCheck(LoginViewModel loginViewModel)
        {
            var result = new RespLoginViewModel();

            #region 非空验证
            if (string.IsNullOrEmpty(loginViewModel.username))
            {
                result.code = -1;
                result.msg = "用户名不能为空！";
                return new JsonResult(result);
            }
            if (string.IsNullOrEmpty(loginViewModel.password))
            {
                result.code = -1;
                result.msg = "密码不能为空！";
                return new JsonResult(result);
            }
            #endregion

            try
            {
                #region 用户名和密码验证
                var user = _authorizeService.CheckLogin(loginViewModel.username);
                if (user != null)
                {
                    if (AccountAuthHelper.VerifyPassword(loginViewModel.password, user.SecretKey, user.Password))
                    {
                        if (user.UserName == DataDictConst.USER_SUPERADMIN)
                        {
                            user.IsAdmin = 1;
                        }
                        result.code = 0;
                        result.msg = "登录验证成功";
                    }
                    else
                    {
                        result.code = -1;
                        result.msg = "密码和用户名不匹配！";
                        return new JsonResult(result);
                    }
                }
                else
                {
                    result.code = -1;
                    result.msg = "用户名不存在！";
                    return new JsonResult(result);
                }
                #endregion

                #region 策略授权
                var ip = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();//获取IP地址
                var claims = new Claim[] {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId),
                        new Claim("OrganizeId", user.OrganizeId),
                        new Claim("OrganizeName", user.OrganizeName),
                        new Claim("DepId", user.DepId),
                        new Claim("DepName", user.DepName),
                        new Claim("RealName", user.RealName),
                        new Claim("RoleName",_authorizeService.GetRoleName(user.UserId)),
                        new Claim("IsAdmin", user.IsAdmin.ToString())
                    };

                //生成token
                //var token = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, DateTime.Now.AddSeconds(Convert.ToInt32(_audienceModel.expiration)));
                var token = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, DateTime.Now.AddSeconds(1008000));
                if (token != null)
                {

                    result.data = token;
                    result.RoleName = _authorizeService.GetRoleName(user.UserId);
                }
                else
                {
                    result.code = -1;
                    result.msg = "生成token出错！";
                }
                #endregion
            }
            catch (Exception ex)
            {
                result.code = -1;
                result.msg = "异常错误！" + ex.Message;
            }
            return new JsonResult(result);
        }

        
    }
}
