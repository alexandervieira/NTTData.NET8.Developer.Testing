using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
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
    IRequestHandler<AddOrderItemsCommand, bool>,
    IRequestHandler<UpdateOrderItemsCommand, bool>,
    IRequestHandler<UpdateOrderItemCommand, bool>,
    IRequestHandler<DeleteOrderItemCommand, bool>,
    IRequestHandler<DeleteOrderItemUnitsCommand, bool>,
    IRequestHandler<ApplyVoucherToOrderCommand, bool>,
    IRequestHandler<GenereateVoucherOrderCommand,  bool>,
    IRequestHandler<StartOrderCommand, bool>,
    IRequestHandler<FinalizeOrderCommand, bool>,
    IRequestHandler<CancelOrderProcessingAndRestockCommand, bool>,
    IRequestHandler<CancelOrderProcessingCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public OrderCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository, IMediatorHandler mediatorHandler)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AddOrderItemsCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            foreach(var messageItem in message.Items)
            {
                var product = await _productRepository.GetById(messageItem.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("order", "Product not found!"));
                    return false;
                }

                if (product.QuantityStock < messageItem.Quantity)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("order", "Insufficient product quantity!"));
                    return false;
                }

                var orderItem = new OrderItem(product.Id, product.Title, messageItem.Quantity, product.Price);
                var order = _orderRepository.GetDraftOrderByCustomerId(message.CustomerId).Result;
                if (order == null)
                {
                    order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
                    order.AddItem(orderItem);
                    _orderRepository.Add(order);
                    order.AddEvent(new OrderDraftStartedEvent(message.CustomerId, product.Id));
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

                order.AddEvent(new OrderItemAddedEvent(order.CustomerId, order.Id, product.Id, product.Title, product.Price, messageItem.Quantity));
            }
            
            return await _orderRepository.UnitOfWork.CommitAsync();
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

        public async Task<bool> Handle(UpdateOrderItemsCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            foreach (var messageItem in message.Items)
            {
                var product = await _productRepository.GetById(messageItem.ProductId);

                if (product == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("order", "Product not found!"));
                    return false;
                }

                if (product.QuantityStock < messageItem.Quantity)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("order", "Insufficient product quantity!"));
                    return false;
                }

                var order = await _orderRepository.GetDraftOrderByCustomerId(message.CustomerId);
                if (order == null)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification("order", "Order not found!"));
                    return false;
                }

                var item = await _orderRepository.GetItemByOrder(order.Id, messageItem.ProductId);
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

                order.UpdateUnits(item, messageItem.Quantity);
                order.AddEvent(new OrderProductUpdatedEvent(message.CustomerId, order.Id, messageItem.ProductId, messageItem.Quantity));

                _orderRepository.UpdateItem(item);
                _orderRepository.Update(order);
            }
            
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

        public async Task<bool> Handle(DeleteOrderItemCommand message, CancellationToken cancellationToken)
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

        public async Task<bool> Handle(DeleteOrderItemUnitsCommand message, CancellationToken cancellationToken)
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

            item.DebitUnits(message.Quantity);
            if (item.Quantity <= 0)
            {
                order.RemoveItem(item);
                _orderRepository.RemoveItem(item);
                order.AddEvent(new OrderProductRemovedEvent(message.CustomerId, order.Id, message.ProductId));
            }
            else
            {
                order.UpdateItem(item);
                _orderRepository.Update(order);
                order.AddEvent(new OrderProductUpdatedEvent(message.CustomerId, order.Id, message.ProductId, item.Quantity));
            }
            
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
            _orderRepository.UpdateDetach(order);

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

            _orderRepository.UpdateDetach(order);
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

        public async Task<bool> Handle(GenereateVoucherOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;
            
            var voucher = await _orderRepository.GetVoucherByCode(message.Code);
            if (voucher != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("voucher", "Voucher code already exists!"));
                return false;
            }

            var result = await _orderRepository.AddVoucher(new Voucher
            {
                Code = message.Code,
                DiscountValue = message.DiscountValue,
                Active = true,
                Used = false,
                Quantity = message.Quantity,
                DiscountVoucherType = message.DiscountVoucherType,
                Percentage = message.Percentage,
                UsageDate = message.UsageDate,
                ExpirationDate = message.ExpirationDate,

            });

            if (result == false) {
                await _mediatorHandler.PublishNotification(new DomainNotification("voucher", "Error creating voucher!"));
                return false;
            }

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
