using Ambev.DeveloperEvoluation.Security.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using NetDevPack.Security.JwtSigningCredentials;
using NetDevPack.Security.JwtSigningCredentials.Store.EntityFrameworkCore;
using System;

namespace Ambev.DeveloperEvoluation.Security.Data;

public class AuthDbContext : IdentityDbContext, ISecurityKeyContext
{        
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }        

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDateTimePropertiesToUtc();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ConvertDateTimePropertiesToUtc();
        return base.SaveChanges();
    }

    public void ConvertDateTimePropertiesToUtc()
    {
        foreach (var entry in ChangeTracker.Entries()) 
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue is DateTime dateTime)
                    {
                        property.CurrentValue = dateTime.Kind == DateTimeKind.Utc
                            ? dateTime
                            : dateTime.ToUniversalTime();
                    }
                }
            }
        }
    }

}

public class DefaultDbContextFactory : IDesignTimeDbContextFactory<AuthDbContext>
{
    public AuthDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Ambev.DeveloperEvaluation.WebApi");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<AuthDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvoluation.Security")
        );
        
        return new AuthDbContext(builder.Options);
    }
}