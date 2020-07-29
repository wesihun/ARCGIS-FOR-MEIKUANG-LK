using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Ocelot.JwtAuthorize;
using Swashbuckle.AspNetCore.Swagger;
using XY.DataCache.Redis;
using XY.DataNS;
using XY.SystemManage.Entities;

namespace XY.Bussiness.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用。使用此方法将服务添加到容器中。
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


            //SystemManage
            //services.AddScoped<IOrganizeService, OrganizeService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IOrganizeService, OrganizeService>();
            //services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IAreaService, AreaService>();
            //services.AddScoped<IModuleService, ModuleService>();
            //services.AddScoped<IRoleService, RoleService>();
            //services.AddScoped<IDataDictService, DataDictService>();
            //services.AddScoped<IModuleButtonService, ModuleButtonService>();
            //services.AddScoped<IUserRoleService, UserRoleService>();
            //services.AddScoped<IRoleModuleService, RoleModuleService>();
            //services.AddScoped<ILogService, LogService>();
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

        // 此方法由运行时调用。使用此方法配置HTTP请求管道
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
