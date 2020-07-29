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
using XY.Authorize.IService;
using XY.Authorize.Service;
using XY.Universal.Models;
using Newtonsoft.Json.Serialization;
using XY.DataCache.Redis;

namespace XY.Authorize.WebApi
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
            #region 获取链接字符串
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

            #region 配置跨域处理
            //配置跨域处理
            //var urls = "http://127.0.0.1:8020";
            services.AddCors(options =>
                options.AddPolicy("XY",
                builder => builder.WithOrigins().AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()
                ));
            #endregion

            #region AutoMapper
            services.AddAutoMapper();
            #endregion

            #region XYDbContext 注入
            services.AddScoped<IXYDbContext, XYDbContext>();
            #endregion

            #region [ApiController]
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region RedisDbContext 注入
            services.AddScoped<IRedisDbContext, RedisDbContext>();
            #endregion

            #region 注入相关的应用服务
            //Authorize
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            #endregion

            #region 读取配置文件
            var audienceConfig = Configuration.GetSection("JwtAuthorize");
            services.Configure<AudienceViewModel>(audienceConfig);
            #endregion

            #region 获取下载路径 
            XYDbContext.DownLoadPath = connectionStrings.GetSection("DownLoadPath").Value;
            #endregion

            services.AddTokenJwtAuthorize();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "XY API", Version = "v1.0" });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 默认的HSTS值是30天.
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
        }
    }
}
