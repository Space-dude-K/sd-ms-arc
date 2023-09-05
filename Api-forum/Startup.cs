using AspNetCoreRateLimit;
using Interfaces;
using Interfaces.Forum;
using Forum.Extensions;
using Forum.Utility.ForumLinks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Services;
using Forum.Utility.UserLinks;
using Services.Forum;
using Microsoft.AspNetCore.Http.Features;

namespace Forum
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly string appUrl;

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;

            appUrl = configuration?["ASPNETCORE_URLS"]?.Split(";").First() ?? string.Empty;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();

            services.ConfigureIISIntegration();
            /*services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
                o.BufferBodyLengthLimit = int.MaxValue;
                o.BufferBody = true;
                o.ValueCountLimit = int.MaxValue;
            });*/

            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.ConfigureRepositoryManager();
            services.ConfigureApiRepositoryManager();

            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.CacheProfiles.Add("60SecondsDuration", new CacheProfile
                {
                    Duration = 60
                });
            })
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            // To return 422 instead of 400, the first thing we have to do is to suppress
            // the BadRequest error when the ModelState is invalid
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.ConfigureValidationsFilters();

            services.ConfigureDataShapers();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>(c =>
                c.BaseAddress = new Uri(appUrl));
            services.AddHttpClient<IHttpForumService, HttpForumService>(c =>
                c.BaseAddress = new Uri(appUrl));


            services.AddScoped<IForumModelService, ForumModelService>();
            services.AddCustomMediaTypes();

            // HATEOAS
            services.ConfigureHATEOAS();

            // Versioning service
            services.ConfigureVersioning();

            // Caching
            // TODO. Validation
            services.ConfigureResponseCaching();
            services.ConfigureHttpCacheHeaders();
            services.AddHttpContextAccessor();

            // Memory cache for memory cache library
            services.AddMemoryCache();

            // Rate limiting, throttling
            services.ConfigureRateLimitingOptions();
            services.AddHttpContextAccessor();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            // Authentication and autorization
            services.ConfigureIdentity();
            services.ConfigureCookie();
            services.ConfigureJWT(Configuration);
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.AddControllersWithViews();

            services.ConfigureSwagger();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // will add middleware for using HSTS, which adds the 
                // Strict - Transport - Security header
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
            // enables using static files for the request. If we don’t set a path to the static files directory, it will use a wwwroot
            // folder in our project by default.
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            // will forward proxy headers to the current request. This will help us during application deployment
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "Forum API v1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "Forum API v2");
            });

        }
    }
}