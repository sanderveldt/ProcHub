using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProcHub.Application.Features.PaymentTerms.Create;
using ProcHub.Application.Features.PaymentTerms.Delete;
using ProcHub.Application.Features.PaymentTerms.Get;
using ProcHub.Application.Features.PaymentTerms.Update;
using ProcHub.Application.Features.PurchaseOrders.Create;
using ProcHub.Application.Features.PurchaseOrders.Delete;
using ProcHub.Application.Features.PurchaseOrders.Get;
using ProcHub.Application.Features.PurchaseOrders.Update;
using ProcHub.Application.Features.ShippingTerms.Create;
using ProcHub.Application.Features.ShippingTerms.Delete;
using ProcHub.Application.Features.ShippingTerms.Get;
using ProcHub.Application.Features.Suppliers.ChangeStatus;
using ProcHub.Application.Features.Suppliers.Create;
using ProcHub.Application.Features.Suppliers.Delete;
using ProcHub.Application.Features.Suppliers.Get;
using ProcHub.Application.Features.Suppliers.Update;
using ProcHub.Application.Features.Users.Create;
using ProcHub.Application.Features.Users.Delete;
using ProcHub.Application.Features.Users.Get;
using ProcHub.Application.Features.Users.Update;

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
        services.AddScoped<DeletePaymentTermHandler>();

        services.AddScoped<CreateShippingTermHandler>();
        services.AddScoped<GetShippingTermHandler>();
        services.AddScoped<ListShippingTermsHandler>();
        services.AddScoped<DeleteShippingTermHandler>();

        services.AddScoped<CreateSupplierHandler>();
        services.AddScoped<GetSupplierHandler>();
        services.AddScoped<ListSuppliersHandler>();
        services.AddScoped<UpdateSupplierHandler>();
        services.AddScoped<ChangeSupplierStatusHandler>();
        services.AddScoped<DeleteSupplierHandler>();

        services.AddScoped<CreatePurchaseOrderHandler>();
        services.AddScoped<GetPurchaseOrderHandler>();
        services.AddScoped<ListPurchaseOrdersHandler>();
        services.AddScoped<ListPurchaseOrdersBySupplierHandler>();
        services.AddScoped<DashboardListHandler>();
        services.AddScoped<UpdatePurchaseOrderDatesHandler>();
        services.AddScoped<UpdatePurchaseOrderReferencesHandler>();
        services.AddScoped<UpdatePurchaseOrderPaymentHandler>();
        services.AddScoped<DeletePurchaseOrderHandler>();

        services.AddScoped<CreateUserHandler>();
        services.AddScoped<GetUserHandler>();
        services.AddScoped<ListUsersHandler>();
        services.AddScoped<UpdateUserHandler>();
        services.AddScoped<DeleteUserHandler>();

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly);
        
        return services;
    }
}
