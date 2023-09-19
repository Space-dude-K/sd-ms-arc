using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ocelot.Authorization;
using Ocelot.Middleware;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace api_gw_ocelot.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection DecorateClaimAuthoriser(this IServiceCollection services)
        {
            var serviceDescriptor = services.First(x => x.ServiceType == typeof(IClaimsAuthorizer));
            services.Remove(serviceDescriptor);

            var newServiceDescriptor = new ServiceDescriptor(serviceDescriptor.ImplementationType, 
                serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime);
            services.Add(newServiceDescriptor);

            services.AddTransient<IClaimsAuthorizer, ClaimAuthorizerDecorator>();

            return services;
        }
        // TODO
        public static bool AuthorizeMultipleRoles(this HttpContext ctx)
        {
            if (ctx.Items.DownstreamRoute().AuthenticationOptions.AuthenticationProviderKey == null) return true;
            else
            {
                // http://schemas.microsoft.com/ws/2008/06/identity/claims/role
                bool auth = false;

                Claim[] claims = ctx.User.Claims.ToArray<Claim>();

                Dictionary<string, string> required = ctx.Items.DownstreamRoute().RouteClaimsRequirement;

                Regex reor = new Regex(@"[^,\s+$ ][^\,]*[^,\s+$ ]");
                MatchCollection matches;

                Regex reand = new Regex(@"[^&\s+$ ][^\&]*[^&\s+$ ]");
                MatchCollection matchesand;
                int cont = 0;
                foreach (KeyValuePair<string, string> claim in required)
                {
                    matches = reor.Matches(claim.Value);
                    foreach (Match match in matches)
                    {
                        matchesand = reand.Matches(match.Value);
                        cont = 0;
                        foreach (Match m in matchesand)
                        {
                            foreach (Claim cl in claims)
                            {
                                if (cl.Type == claim.Key)
                                {
                                    if (cl.Value == m.Value)
                                    {
                                        cont++;
                                    }
                                }
                            }
                        }
                        if (cont == matchesand.Count)
                        {
                            auth = true;
                            break;
                        }
                    }
                }
                return auth;
            }
        }
    }
}