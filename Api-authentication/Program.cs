using Auth_Interfaces;
using Auth_Jwt;
using Microsoft.Extensions.Configuration;

namespace Api_authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<JwtTokenService>();
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureJWT(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureCookie();
            builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}