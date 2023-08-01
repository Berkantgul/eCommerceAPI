using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class ProductImageFile : eCommerceAPI.Domain.Entities.File
    {
        public ICollection<Product> Products { get; set; }
    }
}
