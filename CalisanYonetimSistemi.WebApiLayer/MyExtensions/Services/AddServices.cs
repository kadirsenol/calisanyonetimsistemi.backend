using CalisanYonetimSistemi.BussinesLayer.Abstract;
using CalisanYonetimSistemi.BussinesLayer.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CalisanYonetimSistemi.WebApiLayer.MyExtensions.Services
{
    public static class AddServices
    {
        public static IServiceCollection AddManager(this IServiceCollection services)
        {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IPerformansDegerlendirmeManager, PerformansDegerlendirmeManager>();
            services.AddScoped<IIzinManager, IzinManager>();
            services.AddScoped<IRaporManager, RaporManager>();



            return services;

        }

        public static IServiceCollection AddTokenSetting(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://localhost:5237",
                        ValidAudience = "http://localhost:5237",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qwertyuioplkjhgfdsazxcvbnmqwertlkjfdslkjflksjfklsjfklsjdflskjflyuioplkjhgfdsazxcvbnmmnbv")), /// Burayı secret dosyasına gönder
                        ClockSkew = TimeSpan.Zero
                    });
            return services;
        }

        public static IServiceCollection AddCorsSetting(this IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(builder =>
                                                                        builder
                                                                        .AllowAnyOrigin()
                                                                        .AllowAnyHeader()
                                                                        .AllowAnyMethod()));

            return services;

        }


    }
}
