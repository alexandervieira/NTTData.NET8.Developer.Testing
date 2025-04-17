using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.ORM.Extensions
{
    public static class MigrationsDbContextExtensions
    {
        public static IServiceCollection AddDbContextWithNpgsql<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionName,
            string? migrationsAssembly = null,
            string? migrationsHistoryTable = null
        ) where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString(connectionName)
                                    ?? throw new InvalidOperationException($"Connection string '{connectionName}' not found.");

            services.AddDbContext<TContext>(options =>
            {
                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    if (!string.IsNullOrWhiteSpace(migrationsHistoryTable))
                    {
                        sqlOptions.MigrationsHistoryTable(migrationsHistoryTable);
                    }

                    sqlOptions.MigrationsAssembly(
                        migrationsAssembly ?? typeof(TContext).Assembly.GetName().Name
                    );
                });
            });

            return services;
        }

        public static void SeedData<TContext>(this IApplicationBuilder app, Action<TContext> seeder)
            where TContext : DbContext
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

            ArgumentNullException.ThrowIfNull(context, nameof(context));
            context.Database.Migrate();

            seeder(context);

            if (!context.Set<Product>().Any())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var categories = new List<Category>();

                        var shirtCategory = new Category("Camisas", 100);
                        var mugCategory = new Category("Canecas", 101);                        
                        var smartphoneCategory = new Category("Smartphone", 102);
                        var iphoneCategory = new Category("Iphone", 103);

                        categories.Add(shirtCategory);
                        categories.Add(mugCategory);                        
                        categories.Add(smartphoneCategory);
                        categories.Add(iphoneCategory);
                        categories.ForEach(c => c.CreatedAt = DateTime.UtcNow);

                        context.Set<Category>().AddRange(categories);
                        context.SaveChanges();
                        var productsEspeciais = new List<Product>
                        {
                            new Product(shirtCategory.Id, "Camiseta 100% algodão", 99.00M, true, "Camiseta Developer", "Camiseta1.jpg", new Rating(2.9,10), new Dimensions(5,5,5)),
                            new Product(shirtCategory.Id, "Camiseta 100% algodão", 89.00M, true, "Camiseta Code", "camiseta2.jpg",  new Rating(2.9,10), new Dimensions(5,5,5)),
                            new Product(mugCategory.Id, "Aliquam erat volutpat", 49.00M, true, "Caneca StarBugs", "caneca1.jpg",  new Rating(2.9,10), new Dimensions(5,5,5)),
                            new Product(mugCategory.Id, "Aliquam erat volutpat", 45.00M, true, "Caneca Code", "caneca2.jpg",  new Rating(2.9,10), new Dimensions(5,5,5))

                        };

                        productsEspeciais.ForEach(p => p.CreatedAt = DateTime.UtcNow);
                        context.Set<Product>().AddRange(productsEspeciais);
                        context.SaveChanges();
                        var featuredProducts = new List<Product>
                        {
                            new Product(iphoneCategory.Id, "Aliquam erat volutpat *", 1998.00M, true, "IPhone*", "iphone.png",  new Rating(2.9,10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat *", 1199.00M, true, "Samsung Galaxy S4*", "galaxy-s4.jpg", new Rating(2.9, 10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat *", 1289.00M, true, "Samsung Galaxy Note*", "galaxy-note.jpg", new Rating(2.9, 10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat *", 1389.00M, true, "Z1*", "Z1.png",  new Rating(2.9,10), new Dimensions(5,5,5))

                        };

                        featuredProducts.ForEach(p => p.CreatedAt = DateTime.UtcNow);
                        context.Set<Product>().AddRange(featuredProducts);
                        context.SaveChanges();
                        var products = new List<Product>
                        {
                            new Product(iphoneCategory.Id, "Aliquam erat volutpat", 2998.00M, true, "IPhone", "iphone.png", new Rating(2.9, 10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat", 989.00M, true, "Samsung Galaxy S4", "galaxy-s4.jpg", new Rating(2.9, 10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat", 1179.00M, true, "Samsung Galaxy Note", "galaxy-note.jpg",  new Rating(2.9,10), new Dimensions(5,5,5)),
                            new Product(smartphoneCategory.Id, "Aliquam erat volutpat", 1089.00M, true, "Z1", "Z1.png", new Rating(2.9, 10), new Dimensions(5,5,5))

                        };

                        products.ForEach(p => p.CreatedAt = DateTime.UtcNow);
                        context.Set<Product>().AddRange(products);
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        //throw;
                    }
                }
            }
        }

        public static void SeedDataFromScripts<TContext>(
            this IApplicationBuilder app,
            IConfiguration configuration,
            Assembly? assemblyOverride = null,
            string? embeddedScriptsNamespace = null
        ) where TContext : DbContext
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

            ArgumentNullException.ThrowIfNull(context, nameof(context));
            context.Database.Migrate();

            var assembly = assemblyOverride ?? typeof(MigrationsDbContextExtensions).Assembly;
            var filePrefix = embeddedScriptsNamespace ?? $"{assembly.GetName().Name}.Seedings.";

            var files = assembly.GetManifestResourceNames();

            var seedingProperty = typeof(TContext).GetProperty("SeedingEntries");
            var seedingEntries = seedingProperty?.GetValue(context) as IEnumerable<dynamic>;

            foreach (var file in files.Where(f => f.StartsWith(filePrefix) && f.EndsWith(".sql"))
                                      .Select(f => new
                                      {
                                          PhysicalFile = f,
                                          LogicalFile = f.Replace(filePrefix, string.Empty)
                                      })
                                      .OrderBy(f => f.LogicalFile))
            {
                if (seedingEntries != null && seedingEntries.Any(e => e.Name == file.LogicalFile))
                    continue;

                string command;
                using (var stream = assembly.GetManifestResourceStream(file.PhysicalFile))
                using (var reader = new StreamReader(stream!))
                {
                    command = reader.ReadToEnd();
                }

                if (string.IsNullOrWhiteSpace(command)) continue;

                using var transaction = context.Database.BeginTransaction();
                try
                {
                    context.Database.ExecuteSqlRaw(command);

                    if (seedingProperty != null)
                    {
                        var newEntry = Activator.CreateInstance(seedingProperty.PropertyType.GenericTypeArguments[0]);
                        newEntry!.GetType().GetProperty("Name")?.SetValue(newEntry, file.LogicalFile);
                        var addMethod = seedingProperty.PropertyType.GetMethod("Add");
                        addMethod?.Invoke(seedingProperty.GetValue(context), new[] { newEntry });
                    }

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }

}
