using ProcHub.Infrastructure;
using ProcHub.Application;
using ProcHub.Api;
using ProcHub.Api.Endpoints.Suppliers;
using ProcHub.Api.Endpoints.Users;
using ProcHub.Api.Endpoints.Authentication;
using ProcHub.Api.Endpoints.PaymentTerms;
using ProcHub.Api.Endpoints.ShippingTerms;
using ProcHub.Api.Endpoints.PurchaseOrders;
using ProcHub.Api.Endpoints.Countries;
using ProcHub.Api.Endpoints.Articles;
using ProcHub.Api.Endpoints.Articles.ArticleGroups;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddApi();

var app = builder.Build();

app.UseExceptionHandler();


await app.Services.InitializeDatabaseAsync();   

await app.Services.SeedDatabaseAsync();

await app.Services.InitializeIdentityAsync();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedDevelopmentAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthenticationEndpoints();
app.MapUserEndpoints();
app.MapSupplierEndpoints();
app.MapPaymentTermEndpoints();
app.MapShippingTermEndpoints();
app.MapPurchaseOrderEndpoints();
app.MapArticleGroupEndpoints();
app.MapArticleEndpoints();
app.MapCountryEndpoints();

app.Run();

public partial class Program { }