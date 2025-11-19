using PRN231_SU25_SE181580.DAL;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE181580.DAL.Entities;

namespace PRN231_SU25_SE181580.api.Extensions {
    public static class ConfigurationExtension
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<SU25LeopardDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            
            return service;
        }
    }
}
