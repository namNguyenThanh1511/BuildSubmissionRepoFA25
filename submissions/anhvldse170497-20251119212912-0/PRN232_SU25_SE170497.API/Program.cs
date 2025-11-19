using BLL.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PRN232_SU25_SE170497.API.Extensions;
using PRN232_SU25_SE170497.API.Middlewares;
using PRN232_SU25_SE170497.DAL.Models;
using PRN232_SU25_SE170497.DAL.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
    _ = builder.EntitySet<LeopardProfile>("LeopardProfile");
    _ = builder.EntitySet<LeopardType>("LeopardType");

    return builder.GetEdmModel();
}

builder.Services.AddConfigureJwt(builder.Configuration);
builder.Services.AddCustomSwagger();

builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped<ILeopardAccountService, LeopardAccountService>();
builder.Services.AddScoped<ILeopardProfileService, LeopardProfileService>();


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
