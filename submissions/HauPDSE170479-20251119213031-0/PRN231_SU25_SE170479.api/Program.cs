using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PRN231_SU25_SE170479.api.Extensions;
using PRN231_SU25_SE170479.api.Middlewares;
using PRN231_SU25_SE170479.BLL.Services;
using PRN231_SU25_SE170479.DAL.Models;
using PRN231_SU25_SE170479.DAL.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    })
    .AddOData(options =>
    {
        _ = options.Select().Filter().OrderBy().Expand().SetMaxTop(null).Count();
        _ = options.AddRouteComponents("odata", GetEdmModel());
    });

static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    _ = builder.EntitySet<LeopardProfile>("LeopardProfiles");
    _ = builder.EntitySet<LeopardType>("LeopardTypes");

    return builder.GetEdmModel();
}

builder.Services.AddConfigureJwt(builder.Configuration);
builder.Services.AddCustomSwagger();

builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
builder.Services.AddScoped<ILeopardService, LeopardService>();


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

app.UseCustomAuthMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
