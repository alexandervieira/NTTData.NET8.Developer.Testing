using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Entities.Payments;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Data;
using Ambev.DeveloperEvoluation.Core.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Product = Ambev.DeveloperEvaluation.Domain.Entities.Catalog.Product;
using Order = Ambev.DeveloperEvaluation.Domain.Entities.Sales.Order;
using Ambev.DeveloperEvaluation.ORM.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ambev.DeveloperEvaluation.ORM;

public class DefaultContext : DbContext, IUnitOfWork
{
    private readonly IMediatorHandler _mediatorHandler;
    private IDbContextTransaction _transaction;

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Voucher> Vouchers { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<SeedingEntry> SeedingEntries { get; set; }

    public DefaultContext(DbContextOptions<DefaultContext> options, 
                          IMediatorHandler mediatorHandler) : base(options)
    {
        _mediatorHandler = mediatorHandler;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
               e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            property.SetColumnType("varchar(100)");

        modelBuilder.Ignore<Event>();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DefaultContext).Assembly);
        //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        modelBuilder.HasSequence<int>("MySequence").StartsAt(1000).IncrementsBy(1);

        modelBuilder.Entity<Payment>()
        .Property(p => p.Status)
        .HasDefaultValue("Pending");        

        base.OnModelCreating(modelBuilder);
    }

    public async Task<bool> CommitAsync()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("CreatedAt").IsModified = false;
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        var success = await base.SaveChangesAsync() > 0;
        if (success) await _mediatorHandler.PublishEvents(this);

        return success;
    }       

    public async Task BeginTransactionAsync()
    {
        _transaction = await base.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public override void Dispose()
    {
        _transaction?.Dispose();
        base.Dispose();
    }
}
public class DefaultDbContextFactory : IDesignTimeDbContextFactory<DefaultContext>
{    
    public DefaultContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Ambev.DeveloperEvaluation.WebApi");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<DefaultContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(
               connectionString,
               b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
        );

        IMediatorHandler _mediatorHandler = new MediatorHandler();
        return new DefaultContext(builder.Options, _mediatorHandler);
    }
}