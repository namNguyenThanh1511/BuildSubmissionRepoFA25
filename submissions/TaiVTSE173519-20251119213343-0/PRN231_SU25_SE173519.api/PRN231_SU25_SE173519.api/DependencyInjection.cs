using Services.Interfaces;
using Services.Services;

namespace PRN231_SU25_SE173519.api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ILeoProfileService, LeoProfileService>();
            services.AddScoped<TokenService>();
            return services;
        }
    }
}
