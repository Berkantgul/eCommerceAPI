using eCommerceAPI.Application.Abstractions.Hubs;
using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IOrderHubService _orderHubService;

        public CreateOrderCommandHandler(IBasketService basketService, IOrderService orderService, IOrderHubService orderHubService)
        {
            _basketService = basketService;
            _orderService = orderService;
            _orderHubService = orderHubService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.CreateOrderAsync(new()
            {
                Address = request.Address,
                BasketId = _basketService.GetUserActiveBasket?.Id.ToString(),
                Description = request.Description
            });

            await _orderHubService.OrderAddedAsync("Heyyyy sipariş geldii !!!!!");

            return new();
        }
    }
}
