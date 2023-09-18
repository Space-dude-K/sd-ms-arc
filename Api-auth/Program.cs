using Api_auth.Extensions;
using Api_auth_Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Api_auth_JWT;

namespace Api_auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureIdentity();

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            builder.Services.ConfigureJWTExt(builder.Configuration);

            builder.Services.AddScoped<IAuthManager, AuthManager>();

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