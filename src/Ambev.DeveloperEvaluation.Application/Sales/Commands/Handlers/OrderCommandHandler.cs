using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Sales;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using Ambev.DeveloperEvoluation.Core.DomainObjects.DTOs;
using Ambev.DeveloperEvoluation.Core.Messages;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.IntegrationEvents;
using Ambev.DeveloperEvoluation.Core.Messages.Commons.Notifications;
using MediatR;


namespace Ambev.DeveloperEvaluation.Application.Sales.Commands.Handlers
{
    public class OrderCommandHandler :
    IRequestHandler<AddOrderItemCommand, bool>,
    IRequestHandler<UpdateOrderItemCommand, bool>,
    IRequestHandler<RemoveOrderItemCommand, bool>,
    IRequestHandler<ApplyVoucherToOrderCommand, bool>,
    IRequestHandler<StartOrderCommand, bool>,
    IRequestHandler<FinalizeOrderCommand, bool>,
    IRequestHandler<CancelOrderProcessingAndRestockCommand, bool>,
    IRequestHandler<CancelOrderProcessingCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitPrice);

            if (order == null)
            {
                order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
                order.AddItem(orderItem);

                _orderRepository.Add(order);
                order.AddEvent(new OrderDraftStartedEvent(message.CustomerId, message.ProductId));
            }
            else
            {
                var itemExists = order.HasOrderItem(orderItem);
                order.AddItem(orderItem);

                var existingOrderItem = order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
                if (itemExists && existingOrderItem != null)
                {
                    _orderRepository.UpdateItem(existingOrderItem);
                }
                else
                {
                    _orderRepository.AddItem(orderItem);
                }
            }

            order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, message.ProductId, message.Name, message.UnitPrice, message.Quantity));
            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            var item = await _orderRepository.GetItemByOrder(order.Id, message.ProductId);
            if (item == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
                return false;
            }
            if (!order.HasOrderItem(item))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
                return false;
            }

            order.UpdateUnits(item, message.Quantity);
            order.AddEvent(new OrderProductUpdatedEvent(message.CustomerId, order.Id, message.ProductId, message.Quantity));

            _orderRepository.UpdateItem(item);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(RemoveOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            var item = await _orderRepository.GetItemByOrder(order.Id, message.ProductId);
           
            if (item != null && !order.HasOrderItem(item))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
                return false;
            }
            if (item == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order item not found!"));
                return false;
            }

            order.RemoveItem(item);
            order.AddEvent(new OrderProductRemovedEvent(message.CustomerId, order.Id, message.ProductId));

            _orderRepository.RemoveItem(item);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(ApplyVoucherToOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            var voucher = await _orderRepository.GetVoucherByCode(message.VoucherCode);
            if (voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Voucher not found!"));
                return false;
            }

            var validationResult = order.ApplyVoucher(voucher);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            order.AddEvent(new VoucherAppliedToOrderEvent(message.CustomerId, order.Id, voucher.Id));
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(StartOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
            if (order == null) return false;
            order.StartOrder();

            var itemList = new List<Item>();
            order.OrderItems.ToList().ForEach(i => itemList.Add(new Item { Id = i.ProductId, Quantity = i.Quantity }));
            var productList = new OrderProductsList { OrderId = order.Id, Items = itemList };

            order.AddEvent(new OrderStartedEvent(order.Id, order.CustomerId, productList, order.TotalValue, message.CardName, message.CardNumber, message.CardExpiration, message.CardCvv));

            _orderRepository.Update(order);
            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(FinalizeOrderCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(message.OrderId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            order.CompleteOrder();
            order.AddEvent(new FinalizeOrderEvent(message.OrderId));

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(CancelOrderProcessingAndRestockCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(message.OrderId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            var itemList = new List<Item>();
            order.OrderItems.ToList().ForEach(i => itemList.Add(new Item { Id = i.ProductId, Quantity = i.Quantity }));
            var productList = new OrderProductsList { OrderId = order.Id, Items = itemList };

            order.AddEvent(new OrderProcessingCancelledEvent(order.Id, order.CustomerId, productList));
            order.SetAsDraft();

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        public async Task<bool> Handle(CancelOrderProcessingCommand message, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetById(message.OrderId);
            if (order == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                return false;
            }

            order.SetAsDraft();

            return await _orderRepository.UnitOfWork.CommitAsync();
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }

}
