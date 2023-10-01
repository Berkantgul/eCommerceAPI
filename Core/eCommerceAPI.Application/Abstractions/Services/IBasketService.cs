using eCommerceAPI.Application.ViewModels.BasketItem;
using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<List<BasketItem>> GetBasketItemAsync();
        public Task AddItemTobasket(VM_Create_BasketItem basketItem);
        public Task UpdateQuantityAsync(VM_Update_BasketItem basketItem);
        public Task RemoveBasketItemAsync(string basketItemId);
    }
}
