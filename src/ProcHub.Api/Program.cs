using ProcHub.Infrastructure;
using ProcHub.Application;
using ProcHub.Api.Endpoints.Suppliers;
using ProcHub.Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();   
}

app.MapSupplierEndpoints();

app.Run();