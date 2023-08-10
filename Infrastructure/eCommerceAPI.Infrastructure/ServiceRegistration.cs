using eCommerceAPI.Application.Abstractions.Storage;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Infrastructure.Services;
using eCommerceAPI.Infrastructure.Services.Storage;
using eCommerceAPI.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
    }
}
