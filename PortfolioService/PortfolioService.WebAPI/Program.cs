using PortfolioService.Domain.Extensions;
using PortfolioService.Infrastructure.Extensions;
using PortfolioService.WebAPI.Extensions;
using PortfolioService.WebAPI.Middlewares;
using ApplicationExtensions = PortfolioService.Application.Extentions.ApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
PresentationExtensions.AddPresentationServices(builder.Services, builder.Configuration);
ApplicationExtensions.AddApplicationServices(builder.Services);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Configuration.AddEnvironmentVariables();


var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();
app.Use(async (context, next) =>
{
    Console.WriteLine("Przychodz¹ce ¿¹danie:");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"{header.Key}: {header.Value}");
    }
    await next();
});

// Configure the HTTP request pipeline.ocelo
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