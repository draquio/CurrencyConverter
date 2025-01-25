using CurrencyConverter.Context;
using CurrencyConverter.Mappers;
using CurrencyConverter.Repositories;
using CurrencyConverter.Repositories.Interfaces;
using CurrencyConverter.Services;
using CurrencyConverter.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CurrencyConverter.IOC
{
    public static class Dependencies 
    {
        public static void InjectDependencies(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });




            service.AddScoped<ICurrencyService, CurrencyService>();

            service.AddScoped<ILogRepository, LogRepository>();
            service.AddScoped<ICurrencyCacheRepository, CurrencyCacheRepository>();

            service.AddAutoMapper(typeof(AutoMapperProfile));

            service.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "CurrencyConverter_";
            });

            service.AddHttpClient();
        }
    }
}
