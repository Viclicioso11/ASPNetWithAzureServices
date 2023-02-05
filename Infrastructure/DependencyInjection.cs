using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencyInjection(this IServiceCollection services, IConfiguration config)
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(config["BlobStorageConfig:Connection"]);
            });

            services.AddTransient<IFileDBService, FileDBService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}
