﻿using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Application.ViewModels.BasketItem;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Persistence.Services
{
    public class BasketService : IBasketService
    {
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly UserManager<AppUser> _userManager;
        readonly IOrderReadRepository _orderReadRepository;
        readonly IBasketWriteRepository _basketWriteRepository;
        readonly IBasketItemWriteRepository _basketItemWriteRepository;
        readonly IBasketItemReadRepository _basketItemReadRepository;
        readonly IBasketReadRepository _basketReadRepository;


        public BasketService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IOrderReadRepository orderReadRepository, IBasketWriteRepository basketWriteRepository, IBasketItemWriteRepository basketItemWriteRepository, IBasketItemReadRepository basketItemReadRepository, IBasketReadRepository basketReadRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _orderReadRepository = orderReadRepository;
            _basketWriteRepository = basketWriteRepository;
            _basketItemWriteRepository = basketItemWriteRepository;
            _basketItemReadRepository = basketItemReadRepository;
            _basketReadRepository = basketReadRepository;
        }

        private async Task<Basket> ContextUser()
        {
            var userName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(userName))
            {
                AppUser? user = await _userManager.Users
                    .Include(p => p.Baskets)
                    .FirstOrDefaultAsync(p => p.UserName == userName);

                var _basket = from basket in user.Baskets
                              join order in _orderReadRepository.Table
                              on basket.Id equals order.Id into BasketOrders
                              from order in BasketOrders.DefaultIfEmpty()
                              select new
                              {
                                  Basket = basket,
                                  Order = order
                              };
                Basket? targetbasket = null;
                if (_basket.Any(p => p.Order == null))
                    targetbasket = _basket.FirstOrDefault(p => p.Order is null).Basket;
                else
                {
                    targetbasket = new();
                    user.Baskets.Add(targetbasket);
                }
                await _basketWriteRepository.SaveAsync();
                return targetbasket;
            }

            throw new Exception("Beklenmeyen bir hata ile karşılaşıldı");
        }

        public async Task AddItemTobasket(VM_Create_BasketItem basketItem)
        {
            Basket? basket = await ContextUser();

            if (basket != null)
            {
                BasketItem _basketItem = await _basketItemReadRepository.GetSingleAsync(bi => bi.BasketId == basket.Id && bi.ProductId == Guid.Parse(basketItem.ProductId));
                if (_basketItem != null)
                    _basketItem.Quantity++;
                else
                {
                    await _basketItemWriteRepository.AddAsync(new()
                    {
                        BasketId = basket.Id,
                        ProductId = Guid.Parse(basketItem.ProductId),
                        Quantity = basketItem.Quantity
                    });
                }
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task<List<BasketItem>> GetBasketItemAsync()
        {
            Basket? basket = await ContextUser();

            Basket? result = await _basketReadRepository.Table
                .Include(bi => bi.BasketItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(x => x.Id == basket.Id);
            return result.BasketItems.ToList();
        }

        public async Task RemoveBasketItemAsync(string basketItemId)
        {
            BasketItem? basketItem = await _basketItemReadRepository.GetByIdAsync(basketItemId);
            if (basketItem != null)
            {
                _basketItemWriteRepository.Remove(basketItem);
                await _basketItemWriteRepository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(VM_Update_BasketItem basketItem)
        {
            BasketItem? _basketItem = await _basketItemReadRepository.GetByIdAsync(basketItem.BasketItemId);

            if (_basketItem != null)
            {
                _basketItem.Quantity = basketItem.Quantity;
                await _basketItemWriteRepository.SaveAsync();
            }
        }
        public Basket? GetUserActiveBasket
        {
            get
            {
                Basket? basket = ContextUser().Result;
                return basket;
            }
        }
    }
}
