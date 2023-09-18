using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using api_gw_ocelot.Extensions;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using Api_auth_JWT;

namespace api_gw_ocelot
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());
            services.DecorateClaimAuthoriser();

            IdentityModelEventSource.ShowPII = true; //Add this line
            //services.ConfigureSqlContext(Configuration);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.ConfigureJWTExt(Configuration);
            
            //services.ConfigureIdentity();
            //services.ConfigureCookie();


            // Add services to the container.
            /*services.AddCors(options => {
                options.AddPolicy("CORSPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });*/
            //Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });

            await app.UseOcelot();

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}