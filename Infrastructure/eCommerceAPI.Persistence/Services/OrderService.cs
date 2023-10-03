using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.DTOs.Order;
using eCommerceAPI.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderWriteRepository _orderWriteRepository;

        public OrderService(IOrderWriteRepository orderWriteRepository)
        {
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task CreateOrderAsync(CreateOrder_DTO createOrder_DTO)
        {
            await _orderWriteRepository.AddAsync(new()
            {
                Address = createOrder_DTO.Address,
                Description = createOrder_DTO.Description,
                Id = Guid.Parse(createOrder_DTO.BasketId)
            });

            await _orderWriteRepository.SaveAsync();
        }
    }
}
