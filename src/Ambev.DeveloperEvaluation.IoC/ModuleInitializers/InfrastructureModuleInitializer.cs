using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvaluation.Domain.Events.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Catalog;
using Ambev.DeveloperEvaluation.ORM.Repositories.Sales;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());        
        builder.Services.AddScoped<IUserRepository, UserRepository>();        
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IStockService, StockService>();
        builder.Services.AddScoped<INotificationHandler<ProductLowStockEvent>,ProdutoEventHandler>();
        
    }
}