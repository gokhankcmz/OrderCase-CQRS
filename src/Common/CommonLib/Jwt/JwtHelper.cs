using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using CommonLib.Models.ErrorModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CommonLib.Jwt
{
    public static class JwtHelper
    {
        public static IConfiguration Configuration;
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration
            configuration)
        {
            Configuration = configuration;
            var provider = "Bearer";
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("secret").Value;
            services.AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(provider, options =>
                {
                    options.TokenValidationParameters = ValidationParameters();
                });
        }
        
        public static string GetClaimOrThrow(this IHeaderDictionary headerDictionary, string claimType, Exception exception=null)
        {
            var token = headerDictionary["Authorization"].ToString();
            token = token.Replace("Bearer ", "");
            exception ??= new UnAuthorized();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, ValidationParameters(), out var validatedToken);
                var validJwtToken = (JwtSecurityToken) validatedToken;
                if (validatedToken.ValidTo < DateTime.UtcNow)
                {
                    exception = new UnAuthorized("Token outdated.");
                    throw exception;
                }
                var claim = validJwtToken.Claims.FirstOrDefault(x => x.Type.Equals(claimType));
                return claim.Value;

            }
            catch (Exception e)
            {
                throw exception;
            }

        }
        
        public static TokenValidationParameters ValidationParameters() =>
            new() 
            {
                ValidateLifetime = false, 
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A.VeryLongAndSecretKey"))
            
            };
    }
}