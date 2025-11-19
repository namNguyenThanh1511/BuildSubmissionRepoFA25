using PRN231_SU25_SE181580.BLL.Implementations;
using PRN231_SU25_SE181580.BLL.Interfaces;
using PRN231_SU25_SE181580.DAL.Implementations;
using PRN231_SU25_SE181580.DAL.Interfaces;
using PRN231_SU25_SE181580.DAL.Entities;

namespace PRN231_SU25_SE181580.api.Extensions {
    public static class ServiceExtension
    {
        public static IServiceCollection AddService(this IServiceCollection service)
        {           
            service.AddTransient<IAuthenticateService, AuthenticateService>();
            service.AddScoped<ILeoPardProfileService, LeopardProfileService>();

            return service;
        }
    }
}
