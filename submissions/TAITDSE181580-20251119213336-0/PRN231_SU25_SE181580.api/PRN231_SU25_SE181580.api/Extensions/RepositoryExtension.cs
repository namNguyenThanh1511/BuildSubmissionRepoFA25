using PRN231_SU25_SE181580.DAL.Implementations;
using PRN231_SU25_SE181580.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRN231_SU25_SE181580.DAL.Entities;
using PRN231_SU25_SE181580.api.Extensions;

namespace PRN231_SU25_SE181580.api.Extensions {
    public static class RepositoryExtension {
        public static IServiceCollection AddRepository(this IServiceCollection services) {

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepoBase<>), typeof(RepoBase<>));

            return services;
        }
    }
}
