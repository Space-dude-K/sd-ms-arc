using Api_pdc.Settings;
using Api_pdc_Interfaces;
using Microsoft.Extensions.Options;

namespace Api_pdc.Extensions
{
    public static class ServiceExtensions
    {
        public static void InitMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);
        }
    }
}
