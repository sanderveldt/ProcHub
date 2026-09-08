using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProcHub.Application.Features.PaymentTerms.Create;
using ProcHub.Application.Features.PaymentTerms.Get;
using ProcHub.Application.Features.PaymentTerms.Update;
using ProcHub.Application.Features.ShippingTerms.Create;
using ProcHub.Application.Features.ShippingTerms.Get;
using ProcHub.Application.Features.Suppliers.Create;
using ProcHub.Application.Features.Suppliers.Get;
using ProcHub.Application.Features.Suppliers.Update;

namespace ProcHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreatePaymentTermHandler>();
        services.AddScoped<GetPaymentTermHandler>();
        services.AddScoped<ListPaymentTermsHandler>();
        services.AddScoped<UpdatePaymentTermHandler>();

        services.AddScoped<CreateShippingTermHandler>();
        services.AddScoped<ListShippingTermsHandler>();

        services.AddScoped<CreateSupplierHandler>();
        services.AddScoped<GetSupplierHandler>();
        services.AddScoped<ListSuppliersHandler>();
        services.AddScoped<UpdateSupplierHandler>();

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);
        
        return services;
    }
}
