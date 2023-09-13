using Interfaces;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Microsoft.AspNetCore.Mvc.Versioning;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;

using System.Reflection;

using Entities.DTO.ForumDto;
using Entities.DTO.UserDto;
using Repository.DataShaping;
using Forum.Utility.ForumLinks;
using Forum.Utility.UserLinks;
using api_forum.ActionsFilters.File;
using api_forum.ActionsFilters.Forum;
using api_forum.ActionsFilters.User;
using api_forum.ActionsFilters;
using Microsoft.OpenApi.Models;

namespace Forum.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => {});
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            //services.AddScoped<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ForumContext>(opts => 
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                b => b.MigrationsAssembly("Forum")
            ));
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigureApiRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void ConfigureValidationsFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidateRoleExistsAttribute>();
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateCategoryExistsAttribute>();
            services.AddScoped<ValidateForumForCategoryExistsAttribute>();
            services.AddScoped<ValidateTopicForForumExistsAttribute>();
            services.AddScoped<ValidatePostForTopicExistsAttribute>();
            services.AddScoped<ValidateTopicCounter>();
            services.AddScoped<ValidateForumUserExistsAttribute>();
            services.AddScoped<ValidateFileExistsAttribute>();
            services.AddScoped<ValidateAppUserExistsAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/sd.k.hateoas+json");
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/sd.k.apiroot+json");
                }
                var xmlOutputFormatter = config.OutputFormatters
               .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/sd.k.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/sd.k.apiroot+xml");
                }
            });
        }
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(
            (expirationOpt) =>
            {
                expirationOpt.MaxAge = 1;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });
        }
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit= 1000,
                    Period = "5m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
        public static void ConfigureIdentityCookieAndJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            });

            services.AddAuthentication(options =>
            {
                // custom scheme defined in .AddPolicyScheme() below
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            })
            .AddCookie("Cookies", options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("key").Value))
                };
            })
            // this is the key piece!
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                // runs on each request
                options.ForwardDefaultSelector = context =>
                {
                    // filter by auth type
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return "Bearer";

                    // otherwise always check for cookie auth
                    return "Cookies";
                };
            });
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                
                // Lockout settings.
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                o.Lockout.MaxFailedAccessAttempts = 10;
                o.Lockout.AllowedForNewUsers = true;

                // User settings.
                o.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                o.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(AppRole), builder.Services);
            builder.AddRoles<AppRole>();
            builder.AddEntityFrameworkStores<ForumContext>()
            .AddDefaultTokenProviders();
            builder.AddSignInManager<SignInManager<AppUser>>();
        }
        public static void ConfigureCookie(this IServiceCollection services)
        {
            services.AddAuthentication(opts => opts.DefaultScheme = IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                })
                .AddCookie(IdentityConstants.ExternalScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.ExternalScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("key").Value;
            services.AddAuthentication(opt => 
            {
                /*opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;*/
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
        public static void ConfigureTestJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetSection("key").Value;
            services.AddAuthentication(opt =>
            {
                //opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Site API",
                    Version = "v1",
                    Description = "Site API by Space-dude-K",
                    TermsOfService = new Uri("https://github.com/Space-dude-K/forum-g"),
                    Contact = new OpenApiContact
                    {
                        Name = "Space-Dude",
                        Email = "space.dude.work@gmail.com",
                        Url = new Uri("https://github.com/Space-dude-K")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Site API license",
                        Url = new Uri("https://github.com/Space-dude-K/forum-g")
                    }
                });
                ;
                s.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Site API",
                    Version = "v2"
                });

                /*var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement() 
                {
                    { 
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },

                    },
                    new List<string>()
                    }
                });*/
            });
        }
        public static void ConfigureDataShapers(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<ForumCategoryDto>, DataShaper<ForumCategoryDto>>();
            services.AddScoped<IDataShaper<ForumBaseDto>, DataShaper<ForumBaseDto>>();
            services.AddScoped<IDataShaper<ForumTopicDto>, DataShaper<ForumTopicDto>>();
            services.AddScoped<IDataShaper<ForumPostDto>, DataShaper<ForumPostDto>>();
            services.AddScoped<IDataShaper<ForumUserDto>, DataShaper<ForumUserDto>>();
        }
        public static void ConfigureHATEOAS(this IServiceCollection services)
        {
            services.AddScoped<CategoryLinks>();
            services.AddScoped<ForumBaseLinks>();
            services.AddScoped<TopicLinks>();
            services.AddScoped<PostLinks>();

            services.AddScoped<UserDataLinks>();
        }
    }
}
