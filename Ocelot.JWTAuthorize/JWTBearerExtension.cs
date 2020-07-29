using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using XY.Universal.Models;

namespace Ocelot.JwtAuthorize
{
    /// <summary>
    /// Ocelot.JwtAuthorize extension
    /// </summary>
    public static class JwtBearerExtension
    {
        /// <summary>
        /// 在Ocelot项目中，启动。调用Cs类ConfigureServices方法
        /// </summary>
        /// <param name="services">Service Collection</param>  
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotJwtAuthorize(this IServiceCollection services)
        {
            var configuration = services.SingleOrDefault(s => s.ServiceType.Name == typeof(IConfiguration).Name)?.ImplementationInstance as IConfiguration;
            if (configuration == null)
            {
                throw new OcelotJwtAuthoizeException("在appset .json中找不到JwtAuthorize部分吗");
            }
            var config = configuration.GetSection("JwtAuthorize");
            var keyByteArray = Encoding.ASCII.GetBytes(config["Secret"]);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = config["Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = bool.Parse(config["RequireExpirationTime"])
            };
            return services.AddAuthentication(options =>
            {
                options.DefaultScheme = config["DefaultScheme"];
            })
             .AddJwtBearer(config["DefaultScheme"], opt =>
             {
                 opt.RequireHttpsMetadata = bool.Parse(config["IsHttps"]);
                 opt.TokenValidationParameters = tokenValidationParameters;
             });
        }

        /// <summary>
        /// 在API项目中，启动。调用Cs类ConfigureServices方法
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="validatePermission">validate permission action</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddApiJwtAuthorize(this IServiceCollection services, Func<HttpContext, bool> validatePermission)
        {
            var configuration = services.SingleOrDefault(s => s.ServiceType.Name == typeof(IConfiguration).Name)?.ImplementationInstance as IConfiguration;
            if (configuration == null)
            {
                throw new OcelotJwtAuthoizeException("在appset .json中找不到JwtAuthorize部分吗");
            }
            var config = configuration.GetSection("JwtAuthorize");

            var keyByteArray = Encoding.ASCII.GetBytes(config["Secret"]);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = config["Issuer"],
                ValidateAudience = true,
                ValidAudience = config["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = bool.Parse(config["RequireExpirationTime"])
            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var permissionRequirement = new JwtAuthorizationRequirement(
                config["Issuer"],
                config["Audience"],
                signingCredentials
                );

            permissionRequirement.ValidatePermission = validatePermission;

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(config["PolicyName"],
                          policy => policy.Requirements.Add(permissionRequirement));

            })
         .AddAuthentication(options =>
         {
             options.DefaultScheme = config["DefaultScheme"];
         })
         .AddJwtBearer(config["DefaultScheme"], o =>
         {
             o.RequireHttpsMetadata = bool.Parse(config["IsHttps"]);
             o.TokenValidationParameters = tokenValidationParameters;
         });
        }
        /// <summary>
        /// 在Authorize项目中，启动。调用Cs类ConfigureServices方法
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <returns></returns>
        public static IServiceCollection AddTokenJwtAuthorize(this IServiceCollection services)
        {
            var configuration = services.SingleOrDefault(s => s.ServiceType.Name == typeof(IConfiguration).Name)?.ImplementationInstance as IConfiguration;
            if (configuration == null)
            {
                throw new OcelotJwtAuthoizeException("在appset .json中找不到JwtAuthorize部分吗");
            }
            var config = configuration.GetSection("JwtAuthorize");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Secret"])), SecurityAlgorithms.HmacSha256);
            var permissionRequirement = new JwtAuthorizationRequirement(
               config["Issuer"],
               config["Audience"],
               signingCredentials
                );
            services.AddSingleton<ITokenBuilder, TokenBuilder>();
            return services.AddSingleton(permissionRequirement);
        }
    }
}
