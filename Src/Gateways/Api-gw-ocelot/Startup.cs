using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using api_gw_ocelot.Extensions;
using Api_auth_JWT;
using Ocelot.Authorization;
using System.IdentityModel.Tokens.Jwt;

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
            services.ConfigureJWTExt(Configuration);
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

            var ocelotConfig = new OcelotPipelineConfiguration
            {
                AuthorizationMiddleware = async (ctx, next) =>
                {
                    if (ctx.AuthorizeMultipleRoles())
                    {
                        await next.Invoke();
                    }
                    else
                    {
                        ctx.Items.SetError(new UnauthorizedError($"Fail to authorize"));
                    }
                }
            };

            await app.UseOcelot();

            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}