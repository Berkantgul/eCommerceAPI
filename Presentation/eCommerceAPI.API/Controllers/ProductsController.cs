﻿using eCommerceAPI.Application.Repositories;
using eCommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new(){Id = Guid.NewGuid() ,Name="Product 1",Price=1000,Stock=10},
            //    new(){Id=Guid.NewGuid(),Name="Product 2",Price=2000,Stock=10},
            //    new(){Id=Guid.NewGuid(),Name="Product 3",Price=3000,Stock=10}
            //});
            //await _productWriteRepository.SaveAsync();

            Product product = await _productReadRepository.GetByIdAsync(id);
            product.Stock = 50;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }
    }
}
