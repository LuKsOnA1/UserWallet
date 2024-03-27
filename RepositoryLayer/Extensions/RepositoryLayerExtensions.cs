using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.Repository.Concrete;
using RepositoryLayer.UnitOfWorks.Abstract;
using RepositoryLayer.UnitOfWorks.Concrete;

namespace RepositoryLayer.Extensions
{
    public static class RepositoryLayerExtensions
    {
        public static IServiceCollection LoadRepositoryLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
