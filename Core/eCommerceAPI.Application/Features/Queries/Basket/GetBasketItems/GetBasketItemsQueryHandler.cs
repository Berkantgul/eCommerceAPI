using eCommerceAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
    {
        readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        async Task<List<GetBasketItemsQueryResponse>> IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>.Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemAsync();
            return basketItems.Select(ba => new GetBasketItemsQueryResponse()
            {
                BasketItemId = ba.Id.ToString(),
                Name = ba.Product.Name,
                Price= ba.Product.Price,
                Quantity = ba.Quantity
            }).ToList();
        }
    }
}
