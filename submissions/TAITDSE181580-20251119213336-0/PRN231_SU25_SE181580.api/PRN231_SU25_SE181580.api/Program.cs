using PRN231_SU25_SE181580.api.Extensions;
using Microsoft.EntityFrameworkCore;
using PRN231_SU25_SE181580.BLL.Implementations;
using PRN231_SU25_SE181580.BLL.Interfaces;
using PRN231_SU25_SE181580.DAL.Entities;
using Microsoft.AspNetCore.OData;

namespace PRN231_SU25_SE181580.api {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services
                .AddConfiguration(builder.Configuration)
                .AddService()
                .AddAuthenticationConfig(builder.Configuration)
                .AddRepository()
                .AddSwaggerDocumentation();

            // Enable controller-based routing
            builder.Services.AddControllers();

            // Enable CORS
            builder.Services.AddCors(options => {
                options.AddPolicy("AllowFrontend", policy => {
                    policy.WithOrigins("*") // Có thể thay * bằng domain thật nếu cần
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
                .AddOData(opt => opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100));


            // Add Authorization
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction()) {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
