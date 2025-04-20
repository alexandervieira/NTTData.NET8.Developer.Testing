using Ambev.DeveloperEvoluation.Security.Data;
using Ambev.DeveloperEvoluation.Security.Extensions;
using Ambev.DeveloperEvoluation.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.Security.JwtSigningCredentials;


namespace Ambev.DeveloperEvoluation.Security.Configurations;
public static class IdentityConfig
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettingsSection = configuration.GetSection("AppTokenSettings");
        services.Configure<AppTokenSettings>(appSettingsSection);

        services.AddMemoryCache()
                .AddDataProtection();

        services.AddJwksManager(options => options.Algorithm = Algorithm.ES256)
            .PersistKeysToDatabaseStore<AuthDbContext>();                                

        services.AddJwtConfiguration(configuration);

        services.AddIdentity<IdentityUser, IdentityRole>(o =>
        {
            o.Password.RequireDigit = false;
            o.Password.RequireLowercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequireUppercase = false;
            o.Password.RequiredUniqueChars = 0;
            o.Password.RequiredLength = 3;
        })
          .AddErrorDescriber<IdentityMessagesPtBr>()
          .AddEntityFrameworkStores<AuthDbContext>()
          .AddDefaultTokenProviders();

        return services;
    }

}
