using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repo;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
builder.Services.AddScoped<ILeopardAccountRepo, LeopardAccountRepo>();
builder.Services.AddScoped<ILeopardProfile, LeopardProfileRepo>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


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
        "DevOrMem",
        policyBuilder => policyBuilder.RequireAssertion(
                       context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "7"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "4")));
    options.AddPolicy(
        "AdminOrMorderator",
        policyBuilder => policyBuilder.RequireAssertion(
                       context => context.User.HasClaim(claim => claim.Type == "Role")
                                  && (context.User.FindFirst(claim => claim.Type == "Role").Value == "5"
                                             || context.User.FindFirst(claim => claim.Type == "Role").Value == "6")));
});


var app = builder.Build();
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
