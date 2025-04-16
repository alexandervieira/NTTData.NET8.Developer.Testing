using Ambev.DeveloperEvaluation.Application.Catalog.Services;
using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Handlers;
using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Application.Sales.Events.Handlers;
using Ambev.DeveloperEvaluation.Application.Sales.Queries;
using Ambev.DeveloperEvaluation.Domain.Events.Catalog;
using Ambev.DeveloperEvaluation.Domain.Events.Payments;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Payments;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Payments;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Context;
using Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption;
using Ambev.DeveloperEvaluation.ORM.ExternalServices.AntiCorruption.Interfaces;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories.Catalog;
using Ambev.DeveloperEvaluation.ORM.Repositories.Payments;
using Ambev.DeveloperEvaluation.ORM.Repositories.Sales;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
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

        builder.Services.AddScoped<IProductContext, ProductContext>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        #region Catalog
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IStockService, StockService>();

        builder.Services.AddScoped<INotificationHandler<ProductLowStockEvent>, ProductEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderStartedEvent>, ProductEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderProcessingCancelledEvent>, ProductEventHandler>();
        #endregion

        #region Sales
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderQueries, OrderQueries>();

        builder.Services.AddScoped<IRequestHandler<AddOrderItemCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<AddOrderItemsCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<UpdateOrderItemCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<UpdateOrderItemsCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<DeleteOrderItemCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<DeleteOrderItemUnitsCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<ApplyVoucherToOrderCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<StartOrderCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<FinalizeOrderCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<CancelOrderProcessingCommand, bool>, OrderCommandHandler>();
        builder.Services.AddScoped<IRequestHandler<CancelOrderProcessingAndRestockCommand, bool>, OrderCommandHandler>();
        
        builder.Services.AddScoped<INotificationHandler<OrderItemAddedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderDraftStartedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderProductAddedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderProductRemovedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderProductUpdatedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<VoucherAppliedToOrderEvent>, OrderEventHandler>();

        builder.Services.AddScoped<INotificationHandler<OrderStockRejectedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderPaymentRejectedEvent>, OrderEventHandler>();
        builder.Services.AddScoped<INotificationHandler<OrderPaymentCompletedEvent>, OrderEventHandler>();
        #endregion

        #region Payments
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<ICreditCardPaymentFacade, CreditCardPaymentFacade>();
        builder.Services.AddScoped<IPayPalGateway, PayPalGateway>();
        builder.Services.AddScoped<IPaymentConfigurationManager, PagamentoConfigurationManager>();

        builder.Services.AddScoped<INotificationHandler<OrderStockConfirmedEvent>, PaymentEventHandler>();
        #endregion



    }
}