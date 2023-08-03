﻿using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IStorageService _storageService;
        readonly IProductReadRepository _productReadRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UploadProductImageCommandHandler(IStorageService storageService, IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _storageService = storageService;
            _productReadRepository = productReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images",request.Files);

            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);
            
            await _productImageFileWriteRepository.AddRangeAsync(result.Select(d => new Domain.Entities.ProductImageFile() { FileName = d.fileName, Path = d.pathOrContainerName, Storage = _storageService.StorageName, Products = new List<Domain.Entities.Product>() { product } }).ToList());
            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
