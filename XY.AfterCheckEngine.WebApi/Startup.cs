using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using XY.DataCache.Redis;
using XY.DataNS;
using Ocelot.JwtAuthorize;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using XY.SystemManage.Entities;
using Microsoft.AspNetCore.HttpOverrides;
using XY.AfterCheckEngine.Service;
using XY.AfterCheckEngine.IService;
namespace XY.AfterCheckEngine.WebApi
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
            IConfigurationSection defaultConnectionForYb;
            //获取链接字符串
            var connectionStrings = Configuration.GetSection("ConnectionStrings");
            var uploadtPath = connectionStrings.GetSection("UploadPath");
            var dataBaseType = connectionStrings.GetSection("DataBaseType");
            switch (dataBaseType.Value.ToString())
            {
                case "SqlServer":
                    defaultConnection = connectionStrings.GetSection("SqlConnection");
                    break;
                default:
                    defaultConnection = connectionStrings.GetSection("OracleConnection");
                    break;
            }
            defaultConnectionForYb = connectionStrings.GetSection("SqlConnectionForYb");
            XYDbContext._dataBaseType = dataBaseType.Value.ToString();
            XYDbContext.DefaultDbConnectionString = defaultConnection.Value.ToString();
            XYDbContext.DefaultDbConnectionStringForYb = defaultConnectionForYb.Value.ToString();
            XYDbContext.UPLOADPATH = uploadtPath.Value.ToString();
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
            services.AddScoped<IAfterCheckService, AfterCheckService>();
            services.AddScoped<IHosDayErrorService, HosDayErrorService>();
            services.AddScoped<ISupervisionInfoService, SupervisionInfoService>();
            services.AddScoped<IDecisionAnalysisService, DecisionAnalysisService>();
            services.AddScoped<IHealthCareCheckResultService, HealthCareCheckResultService>();
            services.AddScoped<ISynthesisService, SynthesisService>(); 
            services.AddScoped<IComplaintMZLService, ComplaintMZLService>();
            services.AddScoped<IComplaintStatisticalService, ComplaintStatisticalService>();
            services.AddScoped<IBeforeCheckEngineService, BeforeCheckEngineService>();
            services.AddScoped<IBeforeSynthesisService, BeforeSynthesisService>();
            #endregion

            services.AddApiJwtAuthorize((context) =>
            {
                return ValidatePermission(context);
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "XY API", Version = "v1.0" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
