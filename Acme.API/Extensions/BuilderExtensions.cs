using Acme.Core.Interfaces;
using Acme.Repository;
using Acme.Services;

namespace Acme.API.Extensions
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IOcrService, OcrTestService>();
            services.AddTransient<IProductCodeService, ProductCodeService>();
            services.AddTransient<IProductCodeRepository, ProductCodeRepository>();
            services.AddTransient<IReceiptProcessor, ReceiptProcessor>();
            
            return services;
        }
    }
}
