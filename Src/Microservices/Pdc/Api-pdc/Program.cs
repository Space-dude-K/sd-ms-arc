using Api_pdc.Extensions;
using Api_pdc_Interfaces.MongoDbRepository;
using Api_pdc_Repository;
using NLog.Web;

namespace Api_pdc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            // MongoDb
            builder.Services.InitMongoDb(builder.Configuration);

            // Repository
            builder.Services.AddScoped<IPrintDeviceRepository, PrintDeviceRepository>();

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}