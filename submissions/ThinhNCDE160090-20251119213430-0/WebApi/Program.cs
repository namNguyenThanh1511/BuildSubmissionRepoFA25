using BOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repository;
using Service;
using System.Text;
using System.Text.Json.Serialization;
using UserService.Services.UnitOfWorks;

var builder = WebApplication.CreateBuilder(args);

// Add OData
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<LeopardAccount>("SystemAccount");

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



builder.Services.AddDbContext<Su25leopardDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILeopardAccountRepository, LeppardAccountRepository>();
builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
// Setup JWT
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "role",
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
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
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "1"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "2")));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
