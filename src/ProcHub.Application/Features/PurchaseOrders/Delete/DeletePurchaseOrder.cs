using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.PurchaseOrders.Delete;

public sealed class DeletePurchaseOrderHandler(IProcHubDbContext dbContext)
{
    public async Task HandleAsync(
        string code,
        CancellationToken cancellationToken)
    {
        // No trim on purpose

        var purchaseOrder = await dbContext.PurchaseOrders
            .FirstOrDefaultAsync(
                po => po.Code == code,
                cancellationToken)
            ?? throw new NotFoundException(
                $"PurchaseOrder with code {code} not found.");

        // InUse checks later?
            
        dbContext.PurchaseOrders.Remove(purchaseOrder);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}