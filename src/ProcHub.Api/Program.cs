using ProcHub.Infrastructure;
using ProcHub.Application;
using ProcHub.Api.Endpoints.Suppliers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();   
}

app.MapSupplierEndpoints();

app.Run();