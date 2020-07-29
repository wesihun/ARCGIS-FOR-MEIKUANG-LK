using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Ocelot.JwtAuthorize;
using XY.SystemManage;
using XY.DataNS;
using XY.Utilities;
using XY.SystemManage.IService;
using XY.SystemManage.Service;
using Microsoft.AspNetCore.HttpOverrides;
using AutoMapper;
using XY.SystemManage.Entities;
using Newtonsoft.Json.Serialization;
using XY.DataCache.Redis;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using XY.ZnshBusiness.IService;
using XY.ZnshBusiness.Service;
using System.Text;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using XY.Authorize.IService;
using XY.Authorize.Service;

namespace XY.ZnshBusiness.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 获取数据库链接字符串
            IConfigurationSection defaultConnection;
            //获取链接字符串
            var connectionStrings = Configuration.GetSection("ConnectionStrings");
            var dataBaseType = connectionStrings.GetSection("DataBaseType");
            switch (dataBaseType.Value.ToString())
            {
                case "SqlServer":
                    defaultConnection = connectionStrings.GetSection("SqlConnection");
                    break;
                case "MySql":
                    defaultConnection = connectionStrings.GetSection("SqlConnection");
                    break;
                default:
                    defaultConnection = connectionStrings.GetSection("OracleConnection");
                    break;
            }
            XYDbContext._dataBaseType = dataBaseType.Value.ToString();
            XYDbContext.DefaultDbConnectionString = defaultConnection.Value.ToString();
            #endregion

            #region 获取二维码配置路径
            XYDbContext.QRPath = connectionStrings.GetSection("QRPath").Value;
            #endregion

            #region 获取隐患上报配置路径 
            XYDbContext.HiddenDangerPath = connectionStrings.GetSection("HiddenDangerPath").Value;
            #endregion

            

            #region 获取Cache链接字符串
            IConfigurationSection cacheConnection;
            //获取链接字符串
            var cacheConnectionStrings = Configuration.GetSection("CacheConnectionStrings");
            var cacheType = cacheConnectionStrings.GetSection("CacheType");
            switch (cacheType.Value.ToString())
            {
                case "Redis":
                    cacheConnection = cacheConnectionStrings.GetSection("CacheConnection");
                    break;
                default:
                    cacheConnection = cacheConnectionStrings.GetSection("CacheConnection");
                    break;
            }
            RedisDbContext.RedisDbConnectionString = cacheConnection.Value.ToString();
            #endregion

            #region 配置跨域处理
            //配置跨域处理
            //var urls = "https://192.168.1.13:8000/";
            services.AddCors(options =>
                options.AddPolicy("XY",
                builder => builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                ));
            #endregion

            #region [ApiController]
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region AutoMapper
            services.AddAutoMapper();
            #endregion

            #region XYDbContext 注入
            services.AddScoped<IXYDbContext, XYDbContext>();
            #endregion

            #region RedisDbContext 注入
            services.AddScoped<IRedisDbContext, RedisDbContext>();
            #endregion

            #region 注入相关的应用服务
            //注入授权Handler
            services.AddScoped<IRiskPointManageService, RiskPointManageService>();
            services.AddScoped<IRiskUnitManageService, RiskUnitManageService>();
            services.AddScoped<IRiskClassIficationService, RiskClassIficationService>();
            services.AddScoped<IJobRiskManageService, JobRiskManageService>();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddScoped<ICheckTableService, CheckTableService>();
            services.AddScoped<ICheckPlanService, CheckPlanService>();
            services.AddScoped<IAppService, AppService>(); 
            services.AddScoped<IHiddenDangerService, HiddenDangerService>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IStatisticalService, StatisticalService>();
            #endregion

            services.AddApiJwtAuthorize((context) =>
            {
                return ValidatePermission(context);
            });

            #region swagger配置
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info
                //{
                //    Version = "v1",
                //    Title = "yilezhu's API",
                //    Description = "A simple example ASP.NET Core Web API",
                //    TermsOfService = "None",
                //    Contact = new Contact
                //    {
                //        Name = "依乐祝",
                //        Email = string.Empty,
                //        Url = "http://www.cnblogs.com/yilezhu/"
                //    },
                //    License = new License
                //    {
                //        Name = "许可证名字",
                //        Url = "http://www.cnblogs.com/yilezhu/"
                //    }
                //});
                //注册Swagger生成器，定义一个和多个Swagger 文档
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });

                // 为 Swagger JSON and UI设置xml文档注释路径
                //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var xmlPath = Path.Combine(basePath, "Arcgis.WebApi.xml");
                //c.IncludeXmlComments(xmlPath);


                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ZnshBusiness.WebApi.xml");
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<HttpHeaderOperation>(); // 添加httpHeader参数
            });
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            #region Nlog记日志
            //将日志记录到数据库                 config/NLog.config
            NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            NLog.LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码           
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 默认的HSTS值是30天。
                app.UseHsts();
            }
            app.UseCors("XY");
            //app.UseHttpsRedirection();//重定向https
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "XY API V1");
            });
            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("nlog.config");
            //app.AddNLogWeb();
            app.UseMvc();
            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
        }
        /// <summary>
        /// action请求权限验证
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool ValidatePermission(HttpContext httpContext)
        {
            var isAny = false;
            var userName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Name).Value;//登录名
            var userId = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.NameIdentifier).Value;//用户ID
            var questUrl = httpContext.Request.Path.Value.ToLower();//当前请求Action
            try
            {
                using (var db = new XYDbContext().GetIntance())
                {
                    isAny = db.Queryable<UserModuleButtonEntity>().Any(it => it.ActionUrl == questUrl && it.UserId == userId);
                }
            }
            catch (Exception)
            {
                isAny = false;
            }
            // return isAny;//正式时打开
            return true;
        }
    }
}
