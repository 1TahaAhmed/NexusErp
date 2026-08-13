using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusErp.Application.Common.Interfaces;
using NexusErp.Infrastructure.Data;
using NexusErp.Infrastructure.Repositories;

namespace NexusErp.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MyInventoryDatabase")));
     
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
     
        services.AddScoped<IUnitOfWork, UnitOfWork>();
     
        return services;
    }
}    