using Api_pdc.Settings;
using Api_pdc_Entities;
using Api_pdc_Interfaces;
using Mongo.Migration.Startup;
using Mongo.Migration.Startup.DotNetCore;
using MongoDB.Driver;

namespace Api_pdc.Extensions
{
    public static class ServiceExtensions
    {
        public static void InitMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddScoped<IPdcContext, PrintDeviceContext>();
            services.AddSingleton<IMongoClient>(
                new MongoClient(configuration.GetSection("MongoDbSettings:ConnectionString").Value));
            services.AddMigration(new MongoMigrationSettings
            {
                ConnectionString = configuration.GetSection("MongoDbSettings:ConnectionString").Value,
                Database = configuration.GetSection("MongoDbSettings:DatabaseName").Value
                //VersionFieldName = "TestVersionName" // Optional
            });
        }
    }
}