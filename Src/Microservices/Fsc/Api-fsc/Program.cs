using Api_fsc_Entities;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

namespace Api_fsc
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<FscContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("dbConnection");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly("Api-fsc"));
            });

            builder.Services.AddControllers();

            // NLog: Setup NLog for Dependency injection
            builder.Logging.ClearProviders();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            builder.Host.UseNLog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}