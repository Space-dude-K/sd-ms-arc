using Forum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Net.Http.Headers;

namespace ForumTest
{
    public class TestWithEfInMemoryDb<TContext> : WebApplicationFactory<Startup> where TContext : DbContext
    {
        public HttpClient HttpClient { get; set; }
        public TContext Context { get; set; }
        public IModel Model { get; set; }
        public TestWithEfInMemoryDb()
        {
            HttpClient = this.CreateClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                var dbName = "InMemoryTestDb_" + Guid.NewGuid().ToString();
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName));

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    Context = scopedServices.GetRequiredService<TContext>();

                    Model = Context.GetService<IDesignTimeModel>().Model;

                    // Ensure the database is created.
                    Context.Database.EnsureDeleted();
                    Context.Database.EnsureCreated();
                }
            });
        }
    }
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();
            principal.AddIdentity(new ClaimsIdentity(new[] {
            new Claim("Permission", "CanViewPage"),
            new Claim("Manager", "yes"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.NameIdentifier, "John")
        }, "FakeScheme"));
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
                new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
            AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
    public static class MockJwtToken
    {
        public static string Issuer { get; } = "https://localhost:5001";
        public static string Audience { get; } = "https://localhost:5001/resources";
        public static SecurityKey SecurityKey { get; }
        public static SigningCredentials SigningCredentials { get; }

        private static readonly JwtSecurityTokenHandler STokenHandler = new JwtSecurityTokenHandler();
        private static readonly RandomNumberGenerator SRng = RandomNumberGenerator.Create();
        private static readonly byte[] SKey = new byte[32];

        static MockJwtToken()
        {
            SRng.GetBytes(SKey);
            SecurityKey = new SymmetricSecurityKey(SKey) { KeyId = Guid.NewGuid().ToString() };
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public static string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            return STokenHandler.WriteToken(
                new JwtSecurityToken(Issuer, Audience, claims, null, DateTime.UtcNow.AddMinutes(20), SigningCredentials));
        }

        public static string GenerateJwtTokenAsUser()
        {
            return GenerateJwtToken(UserClaims);
        }
        public static List<Claim> UserClaims { get; set; } = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser") ,
            new Claim(ClaimTypes.Role, "Administrator")
            /*new Claim(JwtClaimTypes.PreferredUserName, "test"),
            new Claim(JwtClaimTypes.Email, "test@test.com"),
            new Claim(JwtClaimTypes.Subject, "10000000-0000-0000-0000-000000000000"),
            new Claim(JwtClaimTypes.Scope, "openid"),
            new Claim(JwtClaimTypes.Scope, "api.com:read"),
            new Claim(JwtClaimTypes.Scope, "api.com:write"),*/
        };
    }
    class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "Administrator")
        }));

            await next();
        }
    }
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "TestUser") ,
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public MockAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
        )
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] 
            { 
                new Claim(ClaimTypes.Name, "TestUser") ,
                new Claim(ClaimTypes.Role, "Administrator")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
    public class MockSchemeProvider : AuthenticationSchemeProvider
    {
        public MockSchemeProvider(IOptions<AuthenticationOptions> options)
            : base(options)
        {
        }

        protected MockSchemeProvider(
            IOptions<AuthenticationOptions> options,
            IDictionary<string, AuthenticationScheme> schemes
        )
            : base(options, schemes)
        {
        }

        public override Task<AuthenticationScheme> GetSchemeAsync(string name)
        {
            if (name == "Test")
            {
                var scheme = new AuthenticationScheme(
                    "Test",
                    "Test",
                    typeof(MockAuthenticationHandler)
                );
                return Task.FromResult(scheme);
            }

            return base.GetSchemeAsync(name);
        }
    }
}