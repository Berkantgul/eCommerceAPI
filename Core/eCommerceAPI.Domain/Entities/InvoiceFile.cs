using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Entities
{
    public class InvoiceFile : eCommerceAPI.Domain.Entities.File
    {
        public decimal Price { get; set; }
    }
}
