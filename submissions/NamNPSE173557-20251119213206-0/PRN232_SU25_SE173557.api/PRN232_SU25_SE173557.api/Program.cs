using BLL;
using DAL;
using DAL.Models;
using DAL.UserRole;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using PRN232_SU25_SE173557;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ================== ĐĂNG KÝ DỊCH VỤ ==================
        // Đăng ký controller, Swagger, DI cho DAO, BL, DbContext
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Đăng ký DbContext sử dụng SQL Server
        builder.Services.AddDbContext<SU25LeopardDBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
        });

        // Đăng ký DI cho DAO
        builder.Services.AddScoped<LeopardDAO>();
        builder.Services.AddScoped<SystemAccountDAO>();

        // Đăng ký DI cho BL
        builder.Services.AddScoped<ISystemAccountBL, SystemAccountBL>();
        builder.Services.AddScoped<ILeopardBL, LeopardBL>();

        // ================== CẤU HÌNH JSON & ODATA ==================
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            })
            .AddOData(opt =>
            {
                var odataBuilder = new ODataConventionModelBuilder();
                odataBuilder.EntitySet<LeopardProfile>("LeopardProfiles");
                odataBuilder.EntitySet<LeopardType>("LeopardTypes");
                opt.AddRouteComponents("odata", odataBuilder.GetEdmModel())
                    .Select().Filter().OrderBy().Count().Expand();
            });

        // ================== ĐỌC CẤU HÌNH ==================
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();

        // ================== CẤU HÌNH AUTHENTICATION (JWT) ==================
        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]))
                };
                // Xử lý lỗi xác thực JWT
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new
                        {
                            errorCode = "HB40101",
                            message = "Token missing or invalid"
                        });
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(new
                        {
                            errorCode = "HB40301",
                            message = "Permission denied"
                        });
                        return context.Response.WriteAsync(result);
                    }
                };
            });

        // ================== CẤU HÌNH XỬ LÝ LỖI MODEL ==================
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                var errorResponse = new ErrorResponse
                {
                    ErrorCode = "HB40001",
                    Message = string.Join(" | ", errors)
                };
                var result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    DeclaredType = typeof(ErrorResponse)
                };
                return result;
            };
        });

        // ================== CẤU HÌNH SWAGGER (JWT) ==================
        builder.Services.AddSwaggerGen(c =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Authentication",
                Description = "JWT Authentication for Cosmetics Management",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };
            c.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                };
            c.AddSecurityRequirement(securityRequirement);
        });


        builder.Services.AddAuthorization(options =>
        {
            // FullAccess: Administrator & Moderator
            options.AddPolicy("FullAccess", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Role") &&
                    (

                        context.User.FindFirst("Role")!.Value == ((int)UserRoleEnum.administrator).ToString() ||
                        context.User.FindFirst("Role")!.Value == ((int)UserRoleEnum.moderator).ToString()
                  
                    )
                ));
            // ReadAccessOnly: Administrator, Moderator, Developer, Member
            options.AddPolicy("ReadAccessOnly", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "Role") &&
                    new[] {
                            ((int)UserRoleEnum.administrator).ToString(),
                            ((int)UserRoleEnum.moderator).ToString(),
                            ((int)UserRoleEnum.developer).ToString(),
                            ((int)UserRoleEnum.member).ToString()
                    }.Contains(context.User.FindFirst("Role")!.Value)
                ));
        });


        var app = builder.Build();


        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseMiddleware<ErrorHandlingMiddleware>();


        app.UseHttpsRedirection();


        app.UseAuthorization();


        app.MapControllers();


        app.Run();
    }
}

