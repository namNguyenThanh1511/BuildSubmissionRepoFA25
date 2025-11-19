
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repository.Interface;
using Repository.Models;
using Repository.Repositories;
using Service.Interface;
using Service.Services;
using System.Text;
using System.Text.Json.Serialization;

namespace PRN231_SU25_SE173566.api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//DI
			builder.Services.AddScoped<IAccountRepository, AccountRepository>();
			builder.Services.AddScoped<IAccountService, AccountService>();
			builder.Services.AddScoped<ILeopardService, LeopardService>();
			builder.Services.AddScoped<ILeopardProfileRepo, LeopardProfileRepo>();

	   builder.Services.AddDbContext<SU25LeopardDBContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
			// Add OData
			var modelBuilder = new ODataConventionModelBuilder();
			modelBuilder.EntitySet<LeopardProfile>("LeopardProfiles");
			modelBuilder.EntitySet<LeopardType>("LeopardTypes");

			// Add services to the container.
			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
				})
				.AddOData(
				options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
					"odata",
					modelBuilder.GetEdmModel()));

			IConfiguration configuration = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", true, true).Build();
			// Setup JWT
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
				});

			// Add SWAGGER JWT
			builder.Services.AddSwaggerGen(c =>
			{
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter your JWT token in this field",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
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
				//options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "0"));
				//options.AddPolicy("StaffOnly", policy => policy.RequireClaim("Role", "1"));

				options.AddPolicy(
					"AdminOnly",
					policyBuilder => policyBuilder.RequireAssertion(
						context => context.User.HasClaim(claim => claim.Type == "Role")
						&& context.User.FindFirst(claim => claim.Type == "Role").Value == "1"));

				options.AddPolicy(
					"StaffOnly",
						   policyBuilder => policyBuilder.RequireAssertion(
										  context => context.User.HasClaim(claim => claim.Type == "Role")
													 && context.User.FindFirst(claim => claim.Type == "Role").Value == "2"));
				options.AddPolicy(
					"AdminOrStaff",
					policyBuilder => policyBuilder.RequireAssertion(
								   context => context.User.HasClaim(claim => claim.Type == "Role")
											  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
														 || context.User.FindFirst(claim => claim.Type == "Role").Value == "6")));
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			//===================================
			var app = builder.Build();

			app.UseRouting();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			//Use Authentication
			app.UseAuthentication(); //Xac thuc - Ban la ai?
			app.UseAuthorization(); //Phan quyen - B?n có quy?n k?

			app.UseMiddleware<ErrorHandlingMiddleware>();

			//app.MapControllers();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.Run();
		}
	}
}
