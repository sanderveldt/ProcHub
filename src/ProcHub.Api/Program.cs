using ProcHub.Infrastructure;
using ProcHub.Application;
using ProcHub.Api;
using ProcHub.Api.Endpoints.Suppliers;
using ProcHub.Api.Endpoints.Users;
using ProcHub.Api.Endpoints.Authentication;
using ProcHub.Api.Endpoints.PaymentTerms;
using ProcHub.Api.Endpoints.ShippingTerms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.Services.InitializeDatabaseAsync();   
}

await app.Services.InitializeIdentityAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoints();
app.MapUserEndpoints();
app.MapSupplierEndpoints();
app.MapPaymentTermEndpoints();
app.MapShippingTermEndpoints();

app.Run();