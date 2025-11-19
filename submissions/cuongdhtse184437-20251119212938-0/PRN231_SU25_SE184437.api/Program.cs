
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PRN231_SU25_SE184437.API.Extensions;
using PRN231_SU25_SE184437.API.Middlewares;
using PRN231_SU25_SE184437.BLL.Services;
using PRN231_SU25_SE184437.DAL.Models;
using PRN231_SU25_SE184437.DAL.Repositories;

namespace PRN231_SU25_SE184437.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Services.AddDbContext<SU25LeopardDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

            _ = builder.Services.AddScoped(typeof(GenericRepository<>));

            _ = builder.Services.AddConfigureJwt(builder.Configuration);
            _ = builder.Services.AddCustomSwagger();

            _ = builder.Services.AddScoped<LeopardAccountService>();
            _ = builder.Services.AddScoped<LeopardProfileService>();

            _ = builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
                })
                .AddOData(options =>
                {
                    _ = options.Select().Filter().Expand();
                    _ = options.AddRouteComponents("odata", GetEdmModel());
                });

            _ = builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }

            _ = app.UseHttpsRedirection();

            // Add use middleware
            _ = app.UseCustomAuthMiddleware();

            _ = app.UseAuthentication();

            _ = app.UseAuthorization();

            _ = app.MapControllers();


            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            _ = builder.EntitySet<LeopardProfile>("LeopardProfiles");
            _ = builder.EntitySet<LeopardType>("LeopardTypes");

            return builder.GetEdmModel();
        }
    }
}
