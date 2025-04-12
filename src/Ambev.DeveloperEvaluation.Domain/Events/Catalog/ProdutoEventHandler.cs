using Ambev.DeveloperEvaluation.Domain.Repositories.Catalog;
using Ambev.DeveloperEvaluation.Domain.Services.Catalog;
using Ambev.DeveloperEvoluation.Core.Communication.Mediator;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events.Catalog
{
    public class ProdutoEventHandler : INotificationHandler<ProductLowStockEvent>
    {
        private readonly IStockService _stockService;
        private readonly IProductRepository _productRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ProdutoEventHandler(IStockService stockService, 
            IProductRepository productRepository, IMediatorHandler mediatorHandler)
        {
            _stockService = stockService;
            _productRepository = productRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task Handle(ProductLowStockEvent notification, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(notification.AggregateId);
            // Enviar e-mail ou notificação para o responsável
        }

    }
   
}
