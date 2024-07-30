
using CalisanYonetimSistemi.DataAccessLayer.DBContexts;
using CalisanYonetimSistemi.WebApiLayer.MyExtensions.AutoMapper;
using CalisanYonetimSistemi.WebApiLayer.MyExtensions.Services;
using Microsoft.EntityFrameworkCore;

namespace CalisanYonetimSistemi.WebApiLayer
{

    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddManager(); //MyExtensions class
            builder.Services.AddTokenSetting(builder.Configuration); //MyExtensions class (Burada configuration nesnesi builderin içinde hazirda var. Ama baska yerde IConfiguration olarak olusturaman gerekli.)
            builder.Services.AddCorsSetting(); //MyExtensions class (Farkli originlerden(platformlardan) gelen tum istekleri kabul et)
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            builder.Services.AddHttpContextAccessor(); //Token claimlerimi analiz edebilmek icin.
            builder.Services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

            builder.Services.AddControllers();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

