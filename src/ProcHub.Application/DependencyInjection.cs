using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProcHub.Application.Features.PaymentTerms.Create;
using ProcHub.Application.Features.PaymentTerms.Get;
using ProcHub.Application.Features.Suppliers.Create;
using ProcHub.Application.Features.Suppliers.Get;

namespace ProcHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<CreatePaymentTermHandler>();
        services.AddScoped<GetPaymentTermHandler>();
        services.AddScoped<ListPaymentTermsHandler>();
        services.AddScoped<CreateSupplierHandler>();
        services.AddScoped<GetSupplierHandler>();
        services.AddScoped<ListSuppliersHandler>();

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);
        
        return services;
    }
}
