using eCommerceAPI.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcase
{
    public class ChangeShowcaseCommandHandler : IRequestHandler<ChangeShowcaseCommandRequest, ChangeShowcaseCommandResponse>
    {
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseCommandResponse> Handle(ChangeShowcaseCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                .Include(p => p.Products)
                .SelectMany(p => p.Products, (pif, p) => new { pif, p }).AsQueryable();

            var data = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase);

            if (data != null)
                data.pif.Showcase = false;

            var image = await query.FirstOrDefaultAsync(p => p.p.Id == Guid.Parse(request.ProductId) && p.pif.Showcase == false);

            if (image != null)
                image.pif.Showcase = true;

            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
