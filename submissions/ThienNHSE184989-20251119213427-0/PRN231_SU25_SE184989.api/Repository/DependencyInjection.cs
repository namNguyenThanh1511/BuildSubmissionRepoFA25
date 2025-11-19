using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Base;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddDbContext<SU25LeopardDBContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IMainEntityRepo, MainEntityRepo>();
            services.AddScoped<IAccountRepo, AccountRepo>();
            return services;
        }
    }

}
