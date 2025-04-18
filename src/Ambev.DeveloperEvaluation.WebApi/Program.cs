using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Ambev.DeveloperEvaluation.WebApi.Configurations;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using Ambev.DeveloperEvoluation.Security.Configurations;
using Ambev.DeveloperEvoluation.Security.Data;
using Asp.Versioning.ApiExplorer;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtSigningCredentials.AspNetCore;
using Serilog;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.AddDefaultLogging();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddApiVersioning(opts =>
            {
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                opts.ReportApiVersions = true;
            });

            builder.Services.AddApiVersioning().AddApiExplorer(opts =>
            {
                opts.GroupNameFormat = "'v'VVV";
                opts.SubstituteApiVersionInUrl = true;
            });

            builder.Services.Configure<ApiBehaviorOptions>(opts =>
            {
                opts.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("Development",
                    builder =>
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin()
                );
            });

            builder.AddBasicHealthChecks();

            builder.Services.AddSwaggerConfig();

            builder.Services.AddDbContextWithNpgsql<DefaultContext>(
                builder.Configuration,
                connectionName: "DefaultConnection",
                migrationsAssembly: "Ambev.DeveloperEvaluation.ORM",
                migrationsHistoryTable: "__EFMigrationsHistory"
            );

            builder.Services.AddDbContextWithNpgsql<AuthDbContext>(
              builder.Configuration,
              connectionName: "AuthConnection",
              migrationsAssembly: "Ambev.DeveloperEvoluation.Security",
              migrationsHistoryTable: "__EFMigrationsHistory"
          );

            builder.Services.AddMongoDb(builder.Configuration);

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:Configuration"];
                options.InstanceName = builder.Configuration["Redis:InstanceName"];
            });


            builder.Services.AddIdentityConfiguration(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();

            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseSwaggerConfig(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
                app.SeedData<DefaultContext>(context =>
                {
                    context.Database.Migrate();
                    // Seed your data here
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthConfiguration();
            app.UseJwksDiscovery();

            app.UseBasicHealthChecks();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
