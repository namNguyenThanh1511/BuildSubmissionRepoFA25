using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddScoped<IMainEntityService, MainEntityService>();
            return services;
        }
    }

}
