using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcase
{
    public class ChangeShowcaseCommandRequest : IRequest<ChangeShowcaseCommandResponse>
    {
        public string ProductId { get; set; }
        public string ImageId { get; set; }
    }
}
