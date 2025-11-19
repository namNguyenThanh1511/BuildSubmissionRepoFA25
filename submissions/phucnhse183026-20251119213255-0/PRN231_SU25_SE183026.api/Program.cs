using DAO.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Repository;
using Service;
using System.Text;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Mvc;
using Repository.Implements;
using Service.Implements;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<LeopardAccount>("LeopardAccounts");
modelBuilder.EntitySet<LeopardProfile>("LeopardProfiles");
modelBuilder.EntitySet<LeopardType>("LeopardTypes");

builder.Services.AddDbContext<SU25LeopardDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

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

builder.Services.AddScoped<ILeopardAccountRepository, LeopardAccountRepository>();
builder.Services.AddScoped<ILeopardProfileRepository, LeopardProfileRepository>();
builder.Services.AddScoped<ILeopardTypeRepository, LeopardTypeRepository>();
builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();
builder.Services.AddScoped<ILeopardTypeService, LeopardTypeService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

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

// Add Swagger JWT configuration
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

    options.AddPolicy("FullAccess",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role") &&
            context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
            || context.User.FindFirst(claim => claim.Type == "Role").Value == "6"));

    options.AddPolicy("Member",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "Role")
            && (context.User.FindFirst(claim => claim.Type == "Role").Value == "7"
            || context.User.FindFirst(claim => claim.Type == "Role").Value == "4")));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var message = string.Join(" | ", context.ModelState
            .Where(ms => ms.Value.Errors.Count > 0)
            .Select(ms =>
                $"{ms.Key}: {string.Join(", ", ms.Value.Errors.Select(e => e.ErrorMessage))}"
            ));

        var result = new
        {
            errorCode = "HB40001",
            message
        };

        return new BadRequestObjectResult(result);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

static async Task WriteCustomError(HttpContext context, int statusCode)
{
    var code = statusCode switch
    {
        400 or 401 or 403 or 404 => statusCode,
        _ => 500
    };

    context.Response.StatusCode = code;
    context.Response.ContentType = "application/json";

    var (errorCode, message) = code switch
    {
        400 => ("HB40001", "Missing/invalid input"),
        401 => ("HB40101", "Token missing/invalid"),
        403 => ("HB40301", "Permission denied"),
        404 => ("HB40401", "Resource not found"),
        500 => ("HB50001", "Internal server error"),
        // no need for _ here since we normalized above
    };

    await context.Response.WriteAsJsonAsync(new { errorCode, message });
}

// Add this before UseRouting / UseEndpoints / MapControllers
app.UseStatusCodePages(async context =>
{
    if (!context.HttpContext.Response.HasStarted)
    {
        await WriteCustomError(context.HttpContext, context.HttpContext.Response.StatusCode);
    }
});

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception)
    {
        await WriteCustomError(context, 500);
    }

    if (!context.Response.HasStarted && context.Response.StatusCode >= 400)
    {
        await WriteCustomError(context, context.Response.StatusCode);
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
