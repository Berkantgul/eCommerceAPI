using eCommerceAPI.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.SignalR
{
    public static class HubRegistration
    {
        public static void MapHub(this WebApplication application)
        {
            application.MapHub<ProductHub>("/product-hub");
        }
    }
}
