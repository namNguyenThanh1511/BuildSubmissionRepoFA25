using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.OData;
using DAL.Repo;
using BLL.Service;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<LeopardType>("LeopardType");
modelBuilder.EntitySet<LeopardProfile>("LeopardProfile");

IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

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

var connectionString = configuration["ConnectionStrings:DefaultConnectionString"];
builder.Services.AddDbContext<Su25leopardDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.



builder.Services.AddControllers();
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
// Add services to the container.
builder.Services.AddScoped<IGenericRepo<LeopardAccount>, GenericRepo<LeopardAccount>>();
builder.Services.AddScoped<IGenericRepo<LeopardProfile>, GenericRepo<LeopardProfile>>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<LeopardService>();


builder.Services.AddAuthorization(options =>
{

    options.AddPolicy("AdminAndMod",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "RoleId") &&
            (context.User.FindFirst(claim => claim.Type == "RoleId").Value == "5"
            || context.User.FindFirst(claim => claim.Type == "RoleId").Value == "6")));

    options.AddPolicy("DevAndMem",
        policyBuilder => policyBuilder.RequireAssertion(
            context => context.User.HasClaim(claim => claim.Type == "RoleId")
            && (context.User.FindFirst(claim => claim.Type == "RoleId").Value == "7"
            || context.User.FindFirst(claim => claim.Type == "RoleId").Value == "4" 
            || context.User.FindFirst(claim => claim.Type == "RoleId").Value == "5"
            || context.User.FindFirst(claim => claim.Type == "RoleId").Value == "6")));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
