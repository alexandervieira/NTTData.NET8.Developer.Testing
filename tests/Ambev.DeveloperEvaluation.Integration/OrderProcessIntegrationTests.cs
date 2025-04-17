using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Entities.Catalog;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.Messages;
using Ambev.DeveloperEvaluation.Domain.Enums.Sales;
using NSubstitute;
using Xunit;
using Ambev.DeveloperEvaluation.Application.Sales.Commands.Handlers;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Integration
{
    public class OrderProcessIntegrationTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly List<Event> _publishedEvents;
        private readonly OrderCommandHandler _orderCommandHandler;

        public OrderProcessIntegrationTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _mediatorHandler = Substitute.For<IMediatorHandler>();
            _publishedEvents = new List<Event>();

            // Captura de eventos publicados
            _mediatorHandler
                .PublishEvent(Arg.Any<Event>())
                .Returns(ci =>
                {
                    _publishedEvents.Add(ci.Arg<Event>());
                    return Task.CompletedTask;
                });

            // Unit of Work padrão para commit
            _orderRepository
                .UnitOfWork
                .CommitAsync()
                .Returns(true);

            // Instância real do handler com mocks injetados
            _orderCommandHandler = new OrderCommandHandler(
                _orderRepository,
                _productRepository,
                _mediatorHandler
            );
        }        

        [Fact]
        public async Task CompleteOrderProcess_WithValidData_ShouldSucceed()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var categoryId = Guid.NewGuid();

            var product = new  Product(categoryId, "Test Product", 100.00m, true, "Test Product", "test.png", new Rating(2.9, 10), new Dimensions(5, 5, 5));
            var order = Order.OrderFactory.NewDraftOrder(customerId);

            _productRepository.GetById(productId).Returns(product);
            _orderRepository.GetDraftOrderByCustomerId(customerId).Returns(order);
            _orderRepository.GetById(orderId).Returns(order);
            _orderRepository.UnitOfWork.CommitAsync().Returns(true);

            // Captura de eventos publicados
            _mediatorHandler
                .PublishEvent(Arg.Any<Event>())
                .Returns(ci =>
                {
                    _publishedEvents.Add(ci.Arg<Event>());
                    return Task.CompletedTask;
                });

            // Act - executa comandos diretamente
            var addItemResult = await _orderCommandHandler.Handle(
                new AddOrderItemCommand(customerId, productId, "Test Product", 5, 100.00m),
                CancellationToken.None
            );

            var startOrderResult = await _orderCommandHandler.Handle(
                new StartOrderCommand(orderId, customerId, 199.90m, "Alexander Vieira", "4111111111111111", "12/2026", "123"),
                CancellationToken.None
            );

            var finalizeOrderResult = await _orderCommandHandler.Handle(
                new FinalizeOrderCommand(orderId, customerId),
                CancellationToken.None
            );

            // Assert
            Assert.True(addItemResult);
            Assert.True(startOrderResult);
            Assert.True(finalizeOrderResult);

            _orderRepository.Received(0).Add(Arg.Any<Order>());
            _orderRepository.Received(1).Update(Arg.Any<Order>());

            //Assert.Contains(_publishedEvents, e => e.GetType().Name == "OrderDraftStartedEvent");
            //Assert.Contains(_publishedEvents, e => e.GetType().Name == "OrderItemAddedEvent");
        }


        [Fact]
        public async Task CancelOrder_WithValidOrder_ShouldSucceed()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewDraftOrder(customerId);

            _orderRepository
                .GetById(orderId)
                .Returns(order);

            // Act
            var result = await _orderCommandHandler.Handle(
                new CancelOrderProcessingCommand(orderId, customerId),
                CancellationToken.None
            );

            // Assert
            Assert.True(result);

            _orderRepository.Received(0)
                .Update(Arg.Is<Order>(o => o.Status == OrderStatus.Canceled));

            //Assert.Contains(_publishedEvents, e => e.GetType().Name == "OrderCanceledEvent");
        }
    }
}
