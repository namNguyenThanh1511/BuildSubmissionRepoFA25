using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRN231_SU25_SE173175.Repository.Basic;
using PRN231_SU25_SE173175.Repository.DBContext;
using PRN231_SU25_SE173175.Service.DTOs;
using PRN231_SU25_SE173175.Service.Interfaces;
using PRN231_SU25_SE173175.Service.Mapping;
using PRN231_SU25_SE173175.Service.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add OData support for ASP.NET Core
builder.Services.AddControllers().AddOData(options =>
	options.Select().Filter().OrderBy().Count().SetMaxTop(100));
//config general
builder.Services.AddDbContext<Su25leopardDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(), typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();

// Configure JSON serialization
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});


// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};

		// Custom authentication error handling
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				context.Response.StatusCode = 401;
				context.Response.ContentType = "application/json";
				var result = JsonSerializer.Serialize(new ErrorResponse
				{
					ErrorCode = "HB40101",
					Message = "Token missing/invalid"
				});
				return context.Response.WriteAsync(result);
			},
			OnForbidden = context =>
			{
				context.Response.StatusCode = 403;
				context.Response.ContentType = "application/json";
				var result = JsonSerializer.Serialize(new ErrorResponse
				{
					ErrorCode = "HB40301",
					Message = "Permission denied"
				});
				return context.Response.WriteAsync(result);
			}
		};
	});

// Configure model validation error format for badrequest
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.InvalidModelStateResponseFactory = context =>
	{
		var errorMessage = string.Join(" ", context.ModelState.Values
			.SelectMany(x => x.Errors)
			.Select(x => x.ErrorMessage));

		return new BadRequestObjectResult(new ErrorResponse
		{
			ErrorCode = "HB40001",
			Message = errorMessage
		});
	};
});


// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Handbag Store API", Version = "v1" });

	// Add JWT Authentication to Swagger
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter JWT token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT"
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
			Array.Empty<string>()
		}
	});
});

var app = builder.Build();

// Configure global exception handling
app.UseExceptionHandler(errorApp =>
{
	errorApp.Run(async context =>
	{
		context.Response.StatusCode = 500;
		context.Response.ContentType = "application/json";

		var errorResponse = new ErrorResponse
		{
			ErrorCode = "HB50001",
			Message = "Internal server error"
		};

		await context.Response.WriteAsJsonAsync(errorResponse);
	});
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

//config CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Add authentication middleware
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
