
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repository;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text;
using Repository.Entities;
using Service.Impl;
using WebAPI.Model;

namespace PRN231_SU25_SE183954
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<LeopardType>("LeopardType");
			modelBuilder.EntitySet<LeopardProfile>("LeopardProfile");

			// Add services to the container.
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<Dbcontext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddControllers()
				.AddOData(options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
					"odata",
					modelBuilder.GetEdmModel())
				)
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.WriteIndented = true;
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				});
			builder.Services.AddScoped<AuthService>();
			builder.Services.AddScoped<AuthRepository>();
			builder.Services.AddScoped<LeopardProfileService>();
			builder.Services.AddScoped<LeopardProfileRepository>();
			builder.Services.AddScoped(typeof(GenericRepository<>));

			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var firstError = context.ModelState
						.Where(e => e.Value.Errors.Count > 0)
						.Select(e => new
						{
							Field = e.Key,
							Error = e.Value.Errors.First().ErrorMessage
						})
						.FirstOrDefault();
					var errorResponse = new ErrorRes(
						"HB40001",
						$"{firstError?.Error}"
					);
					return new BadRequestObjectResult(errorResponse);
				};
			});


			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						RoleClaimType = ClaimTypes.Role,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = builder.Configuration["Jwt:Issuer"],
						ValidAudience = builder.Configuration["Jwt:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
					};
					options.Events = new JwtBearerEvents
					{
						OnChallenge = context =>
						{
							context.HandleResponse();
							context.Response.StatusCode = StatusCodes.Status401Unauthorized;
							context.Response.ContentType = "application/json";
							var error = ErrorRes.Unauthorized("Token missing/invalid");
							return context.Response.WriteAsJsonAsync(error);
						},
						OnForbidden = context =>
						{
							context.Response.StatusCode = StatusCodes.Status403Forbidden;
							context.Response.ContentType = "application/json";
							var error = ErrorRes.Forbidden("Permission denied");
							return context.Response.WriteAsJsonAsync(error);
						}
					};
				}
				);

			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "Just need to put the token only"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
			new List<string> { }
		}
	});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.UseHttpsRedirection();
			app.Run();
		}
	}
}
